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
        //private bool ignoreMirrorBoundChanged_ = false;

        // UI options
        private UIOptions options_ = new UIOptions()
        {
            selectPenColor_ = Color.Red,
            inactiveZoneColor_ = Color.Black,
            activeZoneColor_ = Color.Red,
            zoneHeight_ = 0.16,
            sideTolerance_ = 10,
            zoneAngle_ = 20
        };
        private CalcOptions calcOptions_ = new CalcOptions()
        {
            calcBrightnessPixelNum_ = 30000,
            timeAveragingCnt_ = 30
        };

        // UI mode and handlers
        private UIModeE uiMode_;
        private PictureUIUpdateZoneData uiUpdateZoneData_;
        private PictureUISelMirrorBoundData uiSelMirrorBoundData_;
        private ICalcBrightness calcBrightness_;
        private Queue<float> brightnessDiffQueue_ = new Queue<float>();
        private float brightnessDiffSum_ = 0;

        // zones
        private ZoneVisualizationE zoneVisualization_;
        private int activeZone_;

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
                            comboBoxResolution.Items.Add(String.Format(vc.FrameSize.Width.ToString() + "x" + vc.FrameSize.Height.ToString()));

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
            comboBoxZoneVisualization.Items.Add("Zone Bounds 2");
            comboBoxZoneVisualization.Items.Add("Active Zone Only 2");
            comboBoxZoneVisualization.SelectedIndex = 2;
            zoneVisualization_ = ZoneVisualizationE.ActiveOnly;

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

        private float AddAndCalcDiff(float newDiff)
        {
            brightnessDiffSum_ += newDiff;
            brightnessDiffQueue_.Enqueue(newDiff);
            while (brightnessDiffQueue_.Count > calcOptions_.timeAveragingCnt_)
                brightnessDiffSum_ -= brightnessDiffQueue_.Dequeue();
            return brightnessDiffSum_ / brightnessDiffQueue_.Count;
        }

        private delegate void UpdateBrightnessDelegate(Bitmap bitmap);
        private void UpdateBrightness(Bitmap bitmap)
        {
            if (calcBrightness_ != null)
            {
                float l = 0, r = 0;
                if (calcBrightness_.GetBrightness(bitmap, activeZone_, ref l, ref r))
                {
                    bitmap.Dispose();

                    //float diff = checkBoxMedianCalc.Checked ? l - r : AddAndCalcDiff(l - r);
                    float diff = AddAndCalcDiff(l - r);
                    string fmt = calcBrightness_.FloatFormat;
                    textBoxBrightnessLeft.Text = l.ToString(fmt);
                    textBoxBrightnessRight.Text = r.ToString(fmt);
                    textBoxBrightnessDiff.Text = diff.ToString(calcOptions_.timeAveragingCnt_ > 1 ? "F2" : fmt);
                    return;
                }
            }
            bitmap.Dispose();
            textBoxBrightnessLeft.Text =
            textBoxBrightnessRight.Text =
            textBoxBrightnessDiff.Text = "N/A";
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
            UpdateCalcHandler(false);
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
                if (videoSource_ != null)
                    pictureBox.Size = new Size((int)(videoSource_.VideoResolution.FrameSize.Width * scale),
                                               (int)(videoSource_.VideoResolution.FrameSize.Height * scale));
            }
        }

        private void UpdateCalcHandler(bool force)
        {
            brightnessDiffQueue_.Clear();
            brightnessDiffSum_ = 0;
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
                {
                    if(checkBoxMedianCalc.Checked)
                        calcBrightness_ = new CalcMedianBrightness(mirrorBoundAbs, zoneBounds_, calcOptions_);
                    else
                        calcBrightness_ = new CalcMeanBrightness(mirrorBoundAbs, zoneBounds_, calcOptions_);
                }
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

        private void OnMirrorBoundChanged(object sender, EventArgs e)
        {
            if (uiSelMirrorBoundData_ != null)
            {
                mirrorBound_ = uiSelMirrorBoundData_.MirrorBound;
                UpdateUIHandler();
                UpdateCalcHandler(false);
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
            if (!init_)
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
            UpdateUIHandler();
            UpdateCalcHandler(false);
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
            UpdateUIHandler();
            UpdateCalcHandler(false);
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
            brightnessDiffQueue_.Clear();
            brightnessDiffSum_ = 0;
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
            UpdateUIHandler();
        }

        private void checkBoxZoneBoundsOnly_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonFoucaultOptions_Click(object sender, EventArgs e)
        {
            CalcOptionsForm form = new CalcOptionsForm(calcOptions_);
            if (form.ShowDialog() != DialogResult.OK)
                return;
            calcOptions_ = form.Options;
            UpdateCalcHandler(true);
        }

        private void checkBoxMedianCalc_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCalcHandler(true);
        }
    }
}
