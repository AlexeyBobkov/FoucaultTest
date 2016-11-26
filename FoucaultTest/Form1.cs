using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using FoucaultTestClasses;

namespace FoucaultTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private bool init_ = false;
        private VideoCaptureDevice videoSource_;
        private double[] zoneBounds_;   // zone descriptions
        private RectangleF mirrorBound_;
        private bool ignoreHScrollBarScaleChange_ = false;
        private bool ignoreCheckBoxFitToScreen_ = false;
        //private bool ignoreMirrorBoundChanged_ = false;

        // UI options
        private Options options_ = new Options()
        {
            selectPenColor_ = Color.Red,
            inactiveZoneColor_ = Color.Black,
            activeZoneColor_ = Color.Red,
            zoneHeight_ = 0.16,
            sideTolerance_ = 10,
            zoneAngle_ = 20,
            timeAveragingCnt_ = 30,
            calibAveragingCnt_ = 60
        };

        // UI mode and handlers
        private UIModeE uiMode_;
        private PictureUIUpdateZoneData uiUpdateZoneData_;
        private PictureUISelMirrorBoundData uiSelMirrorBoundData_;
        private CalcBrightnessModeE calcBrightnessMode_ = CalcBrightnessModeE.Median;
        private ICalcBrightness calcBrightness_;
        private Queue<float> brightnessDiffQueue_ = new Queue<float>();
        private float brightnessDiffSum_ = 0;

        // zones
        private ZoneVisualizationE zoneVisualization_;
        private int activeZone_;

        // calibration
        private struct ZoneBrightness
        {
            public float[] l_, r_;  // data for every mode of averaging
        }
        private int calibrationLeft_ = 0;
        private ZoneBrightness[] zoneBrightnessCalib_;

        private struct Zone
        {
            public double innerBound_, outerBound_;
        }

        private int GetZoneNum() { return zoneBounds_ != null ? zoneBounds_.Length - 1 : 0; }

        private Zone GetZone(int index) { return new Zone() { innerBound_ = zoneBounds_[index], outerBound_ = zoneBounds_[index + 1] }; }

        private static void LoadZonesFromFile(string path, ref double[] zoneBounds)
        {
            List<double> zones = new List<double>();
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    string line;
                    line = sr.ReadLine();
                    if (line == null)
                        return;
                    double mirrorD = Convert.ToDouble(line);
                    if (mirrorD <= 0)
                        return;

                    line = sr.ReadLine();
                    if (line == null)
                        return;

                    string[] parts = line.Split(',');
                    double prev = 0;
                    foreach (string part in parts)
                    {
                        double curr = Convert.ToDouble(part) * 2 / mirrorD;
                        if (curr <= prev)
                            return;
                        zones.Add(prev = curr);
                    }
                }
            }
            catch (Exception)
            {
            }
            zoneBounds = zones.Count > 0 ? zones.ToArray() : null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //pictureBox.Image = new Bitmap(Properties.Resources.P1030892_1);

            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Check if atleast one video source is available
            if (videosources != null && videosources.Capacity > 0)
            {
                //For example use first video device. You may check if this is your webcam.
                videoSource_ = new VideoCaptureDevice(videosources[0].MonikerString);

                try
                {
                    //Check if the video device provides a list of supported resolutions
                    if (videoSource_.VideoCapabilities.Length > 0)
                    {
                        int maxW = 0;
                        int idx = 0;

                        //Search for the highest resolution
                        for (int i = 0; i < videoSource_.VideoCapabilities.Length; i++)
                        {
                            VideoCapabilities vc = videoSource_.VideoCapabilities[i];
                            comboBoxResolution.Items.Add(vc.FrameSize.Width.ToString() + "x" + vc.FrameSize.Height.ToString());

                            if (vc.FrameSize.Width > maxW)
                            {
                                maxW = vc.FrameSize.Width;
                                idx = i;
                            }
                        }
                        //Set the highest resolution as active
                        videoSource_.VideoResolution = videoSource_.VideoCapabilities[idx];
                        comboBoxResolution.SelectedIndex = idx;
                    }
                }
                catch { }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured
                videoSource_.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                //Start recording
                videoSource_.Start();
            }

            comboBoxResolution.Enabled =
            buttonCameraSettings.Enabled =
            buttonCopyPicture.Enabled =
            buttonEdgeDetect.Enabled = videoSource_ != null;

            CorrectPictureSize();

            // load zones
            string startupPath = Application.StartupPath + @"\";
            LoadZonesFromFile(startupPath + "Zones.csv", ref zoneBounds_);
            if (GetZoneNum() > 0)
            {
                for (int i = 1; i <= GetZoneNum(); ++i)
                    comboBoxZoneNum.Items.Add(i);
                comboBoxZoneNum.SelectedIndex = 0;
            }

            // visualization
            comboBoxZoneVisualization.Items.Add("None");
            comboBoxZoneVisualization.Items.Add("Zone Bounds");
            comboBoxZoneVisualization.Items.Add("Active Zone Only");
            //comboBoxZoneVisualization.Items.Add("Zone Bounds 2");
            //comboBoxZoneVisualization.Items.Add("Active Zone Only 2");
            comboBoxZoneVisualization.SelectedIndex = 1;
            zoneVisualization_ = ZoneVisualizationE.ZoneBoundsOnly;

            // UI mode: select mirror bound first
            uiMode_ = UIModeE.SelectMirrorBound;
            UpdateUIHandler();
            UpdateCalcHandler(true);

            pictureBox.ImageSizeChanged += new EventHandler(PictureBoxImageSizeChanged);

            init_ = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop and free the webcam object if application is closing
            if (videoSource_ != null)
            {
                if(videoSource_.IsRunning)
                    videoSource_.SignalToStop();
                videoSource_ = null;
            }
        }

        private void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            pictureBox.Image = (Bitmap)eventArgs.Frame.Clone();
            BeginInvoke(new UpdateBrightnessDelegate(UpdateBrightness), (Bitmap)eventArgs.Frame.Clone());
        }

        private void StartCalibration()
        {
            if (zoneBounds_ == null || zoneBounds_.Length <= 1 || uiMode_ != UIModeE.ShowZones || mirrorBound_.IsEmpty || pictureBox.Image == null)
            {
                Console.Beep();
                labelBrightnessCalib.Text = "Can't start calibration!";
                return;
            }
            labelBrightnessCalib.Text = "Calibration started...";
            checkBoxUseCalibration.Enabled = false;
            ResetBrightnessQueue();

            zoneBrightnessCalib_ = new ZoneBrightness[zoneBounds_.Length - 1];
            for (int zone = zoneBrightnessCalib_.Length; --zone >= 0; )
            {
                zoneBrightnessCalib_[zone].l_ = new float[(int)CalcBrightnessModeE.Size];
                zoneBrightnessCalib_[zone].r_ = new float[(int)CalcBrightnessModeE.Size];
            }
            calibrationLeft_ = options_.calibAveragingCnt_;
        }
        private bool MakeCalibrationStep(Bitmap bitmap)
        {
            if(calibrationLeft_ <= 0)
                return true;
            --calibrationLeft_;

            for (int zone = zoneBrightnessCalib_.Length; --zone >= 0; )
            {
                for (int mode = (int)CalcBrightnessModeE.Size; --mode >= 0; )
                {
                    float l = 0, r = 0;
                    calcBrightness_.GetBrightness(bitmap, zone, (CalcBrightnessModeE)mode, ref l, ref r);
                    zoneBrightnessCalib_[zone].l_[mode] += l;
                    zoneBrightnessCalib_[zone].r_[mode] += r;
                }
            }

            if (calibrationLeft_ != 0)
            {
                labelBrightnessCalib.Text = String.Format("Calibration: {0}% done", ((options_.calibAveragingCnt_ - calibrationLeft_) * 100) / options_.calibAveragingCnt_);
                return false;
            }

            // calibratuon ended
            for (int zone = zoneBrightnessCalib_.Length; --zone >= 0; )
            {
                for (int mode = (int)CalcBrightnessModeE.Size; --mode >= 0; )
                {
                    zoneBrightnessCalib_[zone].l_[mode] /= options_.calibAveragingCnt_;
                    zoneBrightnessCalib_[zone].r_[mode] /= options_.calibAveragingCnt_;
                }
            }
            labelBrightnessCalib.Text = "Calibration done!";
            checkBoxUseCalibration.Enabled = true;
            return true;
        }
        
        private float AddAndCalcDiff(float newDiff)
        {
            brightnessDiffSum_ += newDiff;
            brightnessDiffQueue_.Enqueue(newDiff);
            while (brightnessDiffQueue_.Count > options_.timeAveragingCnt_)
                brightnessDiffSum_ -= brightnessDiffQueue_.Dequeue();
            return brightnessDiffSum_ / brightnessDiffQueue_.Count;
        }

        private delegate void UpdateBrightnessDelegate(Bitmap bitmap);
        private void UpdateBrightness(Bitmap bitmap)
        {
            if (calibrationLeft_ > 0)
            {
                // calibration in progress
                if (!MakeCalibrationStep(bitmap))
                {
                    bitmap.Dispose();
                    return;
                }
            }
            if (calcBrightness_ != null)
            {
                float l = 0, r = 0;
                calcBrightness_.GetBrightness(bitmap, activeZone_, calcBrightnessMode_, ref l, ref r);
                bitmap.Dispose();

                string fmt = calcBrightness_.FloatFormat;
                textBoxBrightnessLeft.Text = l.ToString(fmt);
                textBoxBrightnessRight.Text = r.ToString(fmt);

                if (zoneBrightnessCalib_ != null && checkBoxUseCalibration.Checked)
                {
                    if (zoneBrightnessCalib_[activeZone_].l_[(int)calcBrightnessMode_] != 0)
                        l *= zoneBrightnessCalib_[activeZone_].r_[(int)calcBrightnessMode_] / zoneBrightnessCalib_[activeZone_].l_[(int)calcBrightnessMode_];
                    //l -= zoneBrightnessCalib_[activeZone_].l_[(int)calcBrightnessMode_];
                    //r -= zoneBrightnessCalib_[activeZone_].r_[(int)calcBrightnessMode_];
                }
                float diff = AddAndCalcDiff(l - r);
                textBoxBrightnessDiff.Text = diff.ToString(options_.timeAveragingCnt_ > 1 ? "F2" : fmt);
                return;
            }
            bitmap.Dispose();
            textBoxBrightnessLeft.Text =
            textBoxBrightnessRight.Text =
            textBoxBrightnessDiff.Text = "N/A";
        }

        private Size GetPictureBoxPanelSize()
        {
            Size pictureSize = pictureBox.Image.Size;
            Size panelSize = panelPictureBox.ClientSize;
            if (!panelPictureBox.VerticalScroll.Visible)
                panelSize.Width -= SystemInformation.VerticalScrollBarWidth;
            if (!panelPictureBox.HorizontalScroll.Visible)
                panelSize.Height -= SystemInformation.HorizontalScrollBarHeight;
            return panelSize;
        }

        private PointF GetPanelCenter()
        {
            // only for AutoScroll = true mode
            Size panelSize = GetPictureBoxPanelSize();
            return new PointF(panelSize.Width / 2 + panelPictureBox.HorizontalScroll.Value, panelSize.Height / 2 + panelPictureBox.VerticalScroll.Value);
        }

        private void ScrollPictureBoxPointToCenter(PointF pt)
        {
            Size panelSize = GetPictureBoxPanelSize();
            RectangleF newPanelRect = new RectangleF(pt.X - panelSize.Width / 2, pt.Y - panelSize.Height / 2, panelSize.Width, panelSize.Height);
            newPanelRect.Offset(-panelPictureBox.HorizontalScroll.Value, -panelPictureBox.VerticalScroll.Value);

            using (Control c = new Control() { Parent = panelPictureBox, Width = 1, Height = 1, Left = (int)newPanelRect.Left, Top = (int)newPanelRect.Top })
            {
                panelPictureBox.ScrollControlIntoView(c);
                c.Parent = null;
            }
            using (Control c = new Control() { Parent = panelPictureBox, Width = 1, Height = 1, Left = (int)newPanelRect.Right - 1, Top = (int)newPanelRect.Bottom - 1 })
            {
                panelPictureBox.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        private double PictureScale
        {
            get
            {
                System.Diagnostics.Debug.Assert(hScrollBarScale.Maximum == -hScrollBarScale.Minimum, "Scroll Max != -Min");
                return Math.Pow(8, (double)hScrollBarScale.Value / hScrollBarScale.Maximum);
            }
        }

        private void PictureBoxImageSizeChanged(object sender, EventArgs e)
        {
            ImageSizeChanged();
        }

        private void CorrectPictureSize()
        {
            double scale = PictureScale;

            labelScale.Text = "Scale: " + (scale * 100).ToString("F1") + "%";
            if (checkBoxFitToScreen.Checked)
            {
                panelPictureBox.AutoScroll = false;
                pictureBox.Size = panelPictureBox.ClientSize;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                panelPictureBox.AutoScroll = true;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PointF ptOld = GetPanelCenter();
                SizeF szOld = pictureBox.Size;
                if (videoSource_ != null)
                    pictureBox.Size = new Size((int)(videoSource_.VideoResolution.FrameSize.Width * scale),
                                               (int)(videoSource_.VideoResolution.FrameSize.Height * scale));
                ScrollPictureBoxPointToCenter(new PointF(ptOld.X * pictureBox.Size.Width / szOld.Width, ptOld.Y * pictureBox.Size.Height / szOld.Height));
            }
        }

        private void UpdateCalcHandler(bool force)
        {
            if (force && calcBrightness_ != null)
            {
                calcBrightness_.Dispose();
                calcBrightness_ = null;
            }
            if (uiMode_ == UIModeE.ShowZones && zoneBounds_ != null && !mirrorBound_.IsEmpty && pictureBox.Image != null)
            {
                Size imageSize = pictureBox.Image.Size;
                RectangleF mirrorBoundAbs = new RectangleF(imageSize.Width * mirrorBound_.Left, imageSize.Height * mirrorBound_.Top,
                    imageSize.Width * mirrorBound_.Width, imageSize.Height * mirrorBound_.Height);
                if (calcBrightness_ == null)
                    calcBrightness_ = new CalcBrightness(mirrorBoundAbs, zoneBounds_, options_);
                else
                {
                    calcBrightness_.MirrorBoundAbs = mirrorBoundAbs;
                    calcBrightness_.ZoneBounds = zoneBounds_;
                }
            }
            else if (calcBrightness_ != null)
            {
                calcBrightness_.Dispose();
                calcBrightness_ = null;
            }
        }

        private void UpdateUIHandler()
        {
            if (uiSelMirrorBoundData_ != null)
            {
                uiSelMirrorBoundData_.MirrorBoundChanged -= OnMirrorBoundChanged;
                uiSelMirrorBoundData_.Dispose();
                uiSelMirrorBoundData_ = null;
            }
            if (uiUpdateZoneData_ != null)
            {
                uiUpdateZoneData_.Dispose();
                uiUpdateZoneData_ = null;
            }

            PictureUIHandler uiHandler;
            switch (uiMode_)
            {
                default:
                case UIModeE.Off:
                    uiHandler = new UIOffHandler(pictureBox);
                    break;

                case UIModeE.SelectMirrorBound:
                    if (mirrorBound_.IsEmpty)
                    {
                        var h = new UISelNewMirrorBoundHandler(pictureBox, options_);
                        uiHandler = h;
                        uiSelMirrorBoundData_ = h;
                    }
                    else
                    {
                        var h = new UISelMirrorBoundHandler(pictureBox, mirrorBound_, options_);
                        uiHandler = h;
                        uiSelMirrorBoundData_ = h;
                    }
                    break;

                case UIModeE.ShowZones:
                    switch (zoneVisualization_)
                    {
                        default:
                        case ZoneVisualizationE.Off:
                            uiHandler = new UIOffHandler(pictureBox);
                            break;

                        case ZoneVisualizationE.ZoneBoundsOnly:
                            {
                                var h = new UIShowZoneBoundsHandler(pictureBox, mirrorBound_, options_);
                                uiHandler = h;
                                uiUpdateZoneData_ = h;
                            }
                            break;

                        case ZoneVisualizationE.ActiveOnly:
                            {
                                var h = new UIShowActiveZoneHandler(pictureBox, mirrorBound_, options_);
                                uiHandler = h;
                                uiUpdateZoneData_ = h;
                            }
                            break;

                        case ZoneVisualizationE.ZoneBoundsOnly2:
                            {
                                var h = new UIShowZoneBoundsHandler2(pictureBox, mirrorBound_, options_);
                                uiHandler = h;
                                uiUpdateZoneData_ = h;
                            }
                            break;

                        case ZoneVisualizationE.ActiveOnly2:
                            {
                                var h = new UIShowActiveZoneHandler2(pictureBox, mirrorBound_, options_);
                                uiHandler = h;
                                uiUpdateZoneData_ = h;
                            }
                            break;
                    }
                    break;
            }
            if (uiSelMirrorBoundData_ != null)
            {
                uiSelMirrorBoundData_.MirrorBoundChanged += OnMirrorBoundChanged;
            }
            if (uiUpdateZoneData_ != null)
            {
                uiUpdateZoneData_.ZoneBounds = zoneBounds_;
                uiUpdateZoneData_.ActiveZone = activeZone_;
            }
            pictureBox.SetUIHandler(uiHandler);
            pictureBox.Invalidate();
        }

        private void ResetCalibration()
        {
            zoneBrightnessCalib_ = null;
            calibrationLeft_ = 0;
            labelBrightnessCalib.Text = "No calibration";
            checkBoxUseCalibration.Enabled = false;
        }

        private void ResetBrightnessQueue()
        {
            brightnessDiffQueue_.Clear();
            brightnessDiffSum_ = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // change handlers
        private void MirrorBoundChanged()
        {
            UpdateUIHandler();
            UpdateCalcHandler(false);
            ResetBrightnessQueue();
            ResetCalibration();
        }
        private void ImageSizeChanged()
        {
            UpdateUIHandler();
            UpdateCalcHandler(true);
            ResetBrightnessQueue();
            ResetCalibration();
        }
        private void UIModeChanged()
        {
            UpdateUIHandler();
            UpdateCalcHandler(false);
            ResetBrightnessQueue();
        }
        private void VisualizationChanged()
        {
            UpdateUIHandler();
        }
        private void CalcTypeChanged()
        {
            calcBrightnessMode_ = checkBoxMedianCalc.Checked ? CalcBrightnessModeE.Median : CalcBrightnessModeE.Mean;
            UpdateCalcHandler(true);
            ResetBrightnessQueue();
        }
        private void ActiveZoneChanged()
        {
            ResetBrightnessQueue();
        }
        ////////////////////////////////////////////////////////////////////////////////////

        private void OnMirrorBoundChanged(object sender, EventArgs e)
        {
            if (uiSelMirrorBoundData_ != null)
            {
                mirrorBound_ = uiSelMirrorBoundData_.MirrorBound;
                MirrorBoundChanged();
            }
        }
    

        private void buttonCameraSettings_Click(object sender, EventArgs e)
        {
            if (videoSource_ != null)
                videoSource_.DisplayPropertyPage(IntPtr.Zero);
        }

        private void comboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            if (videoSource_ != null)
            {
                videoSource_.Stop();
                videoSource_.VideoResolution = videoSource_.VideoCapabilities[comboBoxResolution.SelectedIndex];
                CorrectPictureSize();
                videoSource_.Start();
            }
        }

        private void hScrollBarScale_Scroll(object sender, ScrollEventArgs e)
        {
            if (!init_ || ignoreHScrollBarScaleChange_)
                return;
            CorrectPictureSize();
        }

        private void buttonCopyPicture_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
                Clipboard.SetImage(pictureBox.Image);
        }

        private void checkBoxFitToScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (!init_ || ignoreCheckBoxFitToScreen_)
                return;
            hScrollBarScale.Enabled = !checkBoxFitToScreen.Checked;
            CorrectPictureSize();
        }

        private void panelPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (checkBoxFitToScreen.Checked)
            {
                panelPictureBox.AutoScroll = false;
                pictureBox.Size = panelPictureBox.ClientSize;
            }
            else
            {
                panelPictureBox.AutoScroll = true;
            }
        }

        private void buttonDelMirrorBound_Click(object sender, EventArgs e)
        {
            mirrorBound_ = new RectangleF();
            MirrorBoundChanged();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            if (tabControl.SelectedTab == tabPageEdge)
            {
                uiMode_ = UIModeE.SelectMirrorBound;
            }
            else
            {
                uiMode_ = UIModeE.ShowZones;
            }
            UIModeChanged();
        }

        private void buttonLoadZones_Click(object sender, EventArgs e)
        {

        }

        private void buttonManualZones_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxZoneNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            activeZone_ = comboBoxZoneNum.SelectedIndex;
            if (uiUpdateZoneData_ != null)
                uiUpdateZoneData_.ActiveZone = activeZone_;
            ActiveZoneChanged();
        }

        private void checkBoxZoneNumAuto_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxZoneVisualization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            switch (comboBoxZoneVisualization.SelectedIndex)
            {
                default:
                case 0:
                    zoneVisualization_ = ZoneVisualizationE.Off;
                    break;

                case 1:
                    zoneVisualization_ = ZoneVisualizationE.ZoneBoundsOnly;
                    break;

                case 2:
                    zoneVisualization_ = ZoneVisualizationE.ActiveOnly;
                    break;

                case 3:
                    zoneVisualization_ = ZoneVisualizationE.ZoneBoundsOnly2;
                    break;

                case 4:
                    zoneVisualization_ = ZoneVisualizationE.ActiveOnly2;
                    break;
            }
            VisualizationChanged();
        }

        private void buttonFoucaultOptions_Click(object sender, EventArgs e)
        {
            CalcOptionsForm form = new CalcOptionsForm(options_);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            bool fUpdateUIHandler = false, fUpdateCalcHandler = false, fResetBrightnessQueue = false, fResetCalibration = false;
            if (options_.selectPenColor_ != form.Options.selectPenColor_ ||
                options_.inactiveZoneColor_ != form.Options.inactiveZoneColor_ ||
                options_.activeZoneColor_ != form.Options.activeZoneColor_ ||
                options_.zoneHeight_ != form.Options.zoneHeight_)
            {
                fUpdateUIHandler = true;
            }
            if (options_.zoneAngle_ != form.Options.zoneAngle_)
            {
                fUpdateUIHandler = true;
                fUpdateCalcHandler = true;
            }
            if (options_.timeAveragingCnt_ != form.Options.timeAveragingCnt_)
                fResetBrightnessQueue = true;
            if (options_.calibAveragingCnt_ != form.Options.calibAveragingCnt_)
                fResetCalibration = true;

            options_ = form.Options;

            if(fUpdateUIHandler)
                UpdateUIHandler();
            if(fUpdateCalcHandler)
                UpdateCalcHandler(true);
            if (fUpdateCalcHandler || fResetBrightnessQueue)
                ResetBrightnessQueue();
            if (fUpdateCalcHandler || fResetCalibration)
                ResetCalibration();
        }

        private void checkBoxMedianCalc_CheckedChanged(object sender, EventArgs e)
        {
            CalcTypeChanged();
        }

        private void buttonBrightnessCalib_Click(object sender, EventArgs e)
        {
            StartCalibration();
        }

        private void checkBoxUseCalibration_CheckedChanged(object sender, EventArgs e)
        {
            ResetBrightnessQueue();
        }

        private void buttonAutoPosition_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null || mirrorBound_.Width <= 0 || mirrorBound_.Left < 0 || mirrorBound_.Right > pictureBox.Image.Size.Width)
                return;
            Size pictureSize = pictureBox.Image.Size;
            Size panelSize = GetPictureBoxPanelSize();
            float scale = panelSize.Width * 0.95F / (mirrorBound_.Width * pictureSize.Width);

            int newScrollBarScaleValue = (int)(hScrollBarScale.Maximum * Math.Log(scale, 8));

            ignoreHScrollBarScaleChange_ = true;
            hScrollBarScale.Enabled = true;
            hScrollBarScale.Value = newScrollBarScaleValue;
            ignoreHScrollBarScaleChange_ = false;

            ignoreCheckBoxFitToScreen_ = true;
            checkBoxFitToScreen.Checked = false;
            ignoreCheckBoxFitToScreen_ = false;

            CorrectPictureSize();

            PointF ptMirrorCenter = new PointF(pictureBox.Width * (mirrorBound_.Left + mirrorBound_.Right) / 2, pictureBox.Height * (mirrorBound_.Top + mirrorBound_.Bottom) / 2);
            ScrollPictureBoxPointToCenter(ptMirrorCenter);
        }
    }
}
