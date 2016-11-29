#define HAS_VIDEO_PROPERTIES

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
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
        private string videoSourceName_;
        private double[] zoneBounds_;   // zone descriptions
        private RectangleF mirrorBound_;
        private bool ignoreHScrollBarScaleChange_ = false;
        private bool ignoreCheckBoxFitToScreen_ = false;

        // UI options
        private MainFormSettings settings_ = new MainFormSettings();
        private Options options_ = new Options();

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

#if HAS_VIDEO_PROPERTIES
        private void SetVideoProperty(VideoCaptureDevice videoSource, VideoProcAmpProperty property, int val)
        {
            int min, max, step, def;
            VideoProcAmpFlags flags;

            try
            {
                videoSource.GetVideoPropertyRange(property, out min, out max, out step, out def, out flags);
                if (val >= min && val <= max)
                    videoSource.SetVideoProperty(property, val, flags);
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        
        private void SetVideoProperties(VideoCaptureDevice videoSource)
        {
            if (videoSourceName_ != null && videoSourceName_ == settings_.Camera)
            {
                SetVideoProperty(videoSource, VideoProcAmpProperty.Brightness, settings_.Brightness);
                SetVideoProperty(videoSource, VideoProcAmpProperty.Contrast, settings_.Contrast);
                SetVideoProperty(videoSource, VideoProcAmpProperty.Hue, settings_.Hue);
                SetVideoProperty(videoSource, VideoProcAmpProperty.Saturation, settings_.Saturation);
                SetVideoProperty(videoSource, VideoProcAmpProperty.Sharpness, settings_.Sharpness);
                SetVideoProperty(videoSource, VideoProcAmpProperty.Gamma, settings_.Gamma);
            }

            /*
            int min, max, step, def;
            VideoProcAmpFlags flags;

            videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Sharpness, out min, out max, out step, out def, out flags);
            videoSource.SetVideoProperty(VideoProcAmpProperty.Sharpness, min, flags);

            videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Saturation, out min, out max, out step, out def, out flags);
            videoSource.SetVideoProperty(VideoProcAmpProperty.Saturation, min, flags);
             * */
        }

        private int GetVideoProperty(VideoCaptureDevice videoSource, VideoProcAmpProperty property)
        {
            int val;
            VideoProcAmpFlags flags;
            try
            {
                videoSource.GetVideoProperty(property, out val, out flags);
                return val;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                return Int32.MinValue;
            }
        }

        private void SaveVideoProperties(VideoCaptureDevice videoSource)
        {
            if (videoSourceName_ != null)
            {
                settings_.Camera = videoSourceName_;
                settings_.Brightness = GetVideoProperty(videoSource, VideoProcAmpProperty.Brightness);
                settings_.Contrast = GetVideoProperty(videoSource, VideoProcAmpProperty.Contrast);
                settings_.Hue = GetVideoProperty(videoSource, VideoProcAmpProperty.Hue);
                settings_.Saturation = GetVideoProperty(videoSource, VideoProcAmpProperty.Saturation);
                settings_.Sharpness = GetVideoProperty(videoSource, VideoProcAmpProperty.Sharpness);
                settings_.Gamma = GetVideoProperty(videoSource, VideoProcAmpProperty.Gamma);
            }
        }
#endif

        private void MainForm_Load(object sender, EventArgs e)
        {
            options_.SelectPenColor = settings_.SelectPenColor;
            options_.InactiveZoneColor = settings_.InactiveZoneColor;
            options_.ActiveZoneColor = settings_.ActiveZoneColor;
            options_.ZoneHeight = settings_.ZoneHeight;
            options_.SideTolerance = settings_.SideTolerance;
            options_.ZoneAngle = settings_.ZoneAngle;
            options_.TimeAveragingCnt = settings_.TimeAveragingCnt;
            options_.CalibAveragingCnt = settings_.CalibAveragingCnt;

            //pictureBox.Image = new Bitmap(Properties.Resources.P1030892_1);

            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Check if atleast one video source is available
            if (videosources != null && videosources.Capacity > 0)
            {
                //For example use first video device. You may check if this is your webcam.
                videoSource_ = new VideoCaptureDevice(videosources[0].MonikerString);
                videoSourceName_ = videosources[0].Name;

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

#if HAS_VIDEO_PROPERTIES
                SetVideoProperties(videoSource_);
#endif

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
#if HAS_VIDEO_PROPERTIES
                SaveVideoProperties(videoSource_);
#endif
                if(videoSource_.IsRunning)
                    videoSource_.SignalToStop();
                videoSource_ = null;
            }
            settings_.Save();
        }

        private delegate void SetNewFrameDelegate(Bitmap bitmap);
        private void SetNewFrame(Bitmap bitmap)
        {
            pictureBox.Image = bitmap;
            UpdateBrightness((Bitmap)bitmap.Clone());
        }
        private void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            BeginInvoke(new SetNewFrameDelegate(SetNewFrame), (Bitmap)eventArgs.Frame.Clone());
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
            calibrationLeft_ = options_.CalibAveragingCnt;
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
                labelBrightnessCalib.Text = String.Format("Calibration: {0}% done", ((options_.CalibAveragingCnt - calibrationLeft_) * 100) / options_.CalibAveragingCnt);
                return false;
            }

            // calibratuon ended
            for (int zone = zoneBrightnessCalib_.Length; --zone >= 0; )
            {
                for (int mode = (int)CalcBrightnessModeE.Size; --mode >= 0; )
                {
                    zoneBrightnessCalib_[zone].l_[mode] /= options_.CalibAveragingCnt;
                    zoneBrightnessCalib_[zone].r_[mode] /= options_.CalibAveragingCnt;
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
            while (brightnessDiffQueue_.Count > options_.TimeAveragingCnt)
                brightnessDiffSum_ -= brightnessDiffQueue_.Dequeue();
            return brightnessDiffSum_ / brightnessDiffQueue_.Count;
        }

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
                textBoxBrightnessDiff.Text = diff.ToString(options_.TimeAveragingCnt > 1 ? "F2" : fmt);
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
            {
                try
                {
                    videoSource_.DisplayPropertyPage(IntPtr.Zero);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
            if (options_.SelectPenColor != form.Options.SelectPenColor ||
                options_.InactiveZoneColor != form.Options.InactiveZoneColor ||
                options_.ActiveZoneColor != form.Options.ActiveZoneColor ||
                options_.ZoneHeight != form.Options.ZoneHeight)
            {
                fUpdateUIHandler = true;
            }
            if (options_.ZoneAngle != form.Options.ZoneAngle)
            {
                fUpdateUIHandler = true;
                fUpdateCalcHandler = true;
            }
            if (options_.TimeAveragingCnt != form.Options.TimeAveragingCnt)
                fResetBrightnessQueue = true;
            if (options_.CalibAveragingCnt != form.Options.CalibAveragingCnt)
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

            settings_.SelectPenColor = options_.SelectPenColor;
            settings_.InactiveZoneColor = options_.InactiveZoneColor;
            settings_.ActiveZoneColor = options_.ActiveZoneColor;
            settings_.ZoneHeight = options_.ZoneHeight;
            settings_.SideTolerance = options_.SideTolerance;
            settings_.ZoneAngle = options_.ZoneAngle;
            settings_.TimeAveragingCnt = options_.TimeAveragingCnt;
            settings_.CalibAveragingCnt = options_.CalibAveragingCnt;
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

    sealed class MainFormSettings : ApplicationSettingsBase
    {
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("Red")]
        public Color SelectPenColor
        {
            get { return (Color)this["SelectPenColor"]; }
            set { this["SelectPenColor"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("Black")]
        public Color InactiveZoneColor
        {
            get { return (Color)this["InactiveZoneColor"]; }
            set { this["InactiveZoneColor"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("Red")]
        public Color ActiveZoneColor
        {
            get { return (Color)this["ActiveZoneColor"]; }
            set { this["ActiveZoneColor"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("0.16")]
        public double ZoneHeight
        {
            get { return (double)this["ZoneHeight"]; }
            set { this["ZoneHeight"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("10")]
        public int SideTolerance
        {
            get { return (int)this["SideTolerance"]; }
            set { this["SideTolerance"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("20")]
        public float ZoneAngle
        {
            get { return (float)this["ZoneAngle"]; }
            set { this["ZoneAngle"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("30")]
        public int TimeAveragingCnt
        {
            get { return (int)this["TimeAveragingCnt"]; }
            set { this["TimeAveragingCnt"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("60")]
        public int CalibAveragingCnt
        {
            get { return (int)this["CalibAveragingCnt"]; }
            set { this["CalibAveragingCnt"] = value; }
        }

        // camera attributes
#if HAS_VIDEO_PROPERTIES
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string Camera
        {
            get { return (string)this["Camera"]; }
            set { this["Camera"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Brightness
        {
            get { return (int)this["Brightness"]; }
            set { this["Brightness"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Contrast
        {
            get { return (int)this["Contrast"]; }
            set { this["Contrast"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Hue
        {
            get { return (int)this["Hue"]; }
            set { this["Hue"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Saturation
        {
            get { return (int)this["Saturation"]; }
            set { this["Saturation"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Sharpness
        {
            get { return (int)this["Sharpness"]; }
            set { this["Sharpness"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("-2147483648")]
        public int Gamma
        {
            get { return (int)this["Gamma"]; }
            set { this["Gamma"] = value; }
        }
#endif
    }
}
