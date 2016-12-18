﻿#define HAS_VIDEO_PROPERTIES

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
using SerialPortSupport;

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
        private IPictureUIUpdateZoneData uiUpdateZoneData_;
        private IPictureUISelMirrorBoundData uiSelMirrorBoundData_;
        private CalcBrightnessModeE calcBrightnessMode_ = CalcBrightnessModeE.Median;
        private ICalcBrightness calcBrightness_;
        private Queue<float> brightnessDiffQueue_ = new Queue<float>();
        private float brightnessDiffSum_ = 0;
        private bool stopUpdateVideoFrames_ = false;

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

        // connection to Dial Indicator
        private string portNameDI_;
        private int baudRateDI_ = 115200;
        private SerialConnection connectionDI_;
        private DateTime lastDIRequest_;

        private enum DIUnit { Inch, Mm }
        private bool valDIValid_ = false;
        private double valDI_;
        private DIUnit valDIUnit_;
        private event EventHandler ValDIChanged;
        private ZoneReading[] zoneReadings_;

        private delegate void TimeoutDelegate(SerialConnection connection);
        private delegate void ReceiveDelegate(byte[] data);
        private class BaseConnectionHandler : SerialConnection.IReceiveHandler
        {
            public BaseConnectionHandler(MainForm parent, ReceiveDelegate receiveDelegate, SerialConnection connection)
            {
                parent_ = parent;
                receiveDelegate_ = receiveDelegate;
                connection_ = connection;
            }

            public void Error()
            {
                TimeoutDelegate d = new TimeoutDelegate(parent_.SerialError);
                parent_.BeginInvoke(d, new object[] { connection_ });
            }

            public void Received(byte[] data)
            {
                parent_.BeginInvoke(receiveDelegate_, new object[] { data });
            }

            private MainForm parent_;
            private ReceiveDelegate receiveDelegate_;
            private SerialConnection connection_;
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

        private void StartVideoSource()
        {
            if (videoSource_ != null)
            {
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

                        if (videoSourceName_ != null && videoSourceName_ == settings_.Camera)
                        {
                            int i = comboBoxResolution.Items.IndexOf(settings_.Resolution);
                            if (i >= 0)
                                idx = i;
                        } // else set the highest resolution as active

                        videoSource_.VideoResolution = videoSource_.VideoCapabilities[idx];
                        comboBoxResolution.SelectedIndex = idx;
                    }

#if HAS_VIDEO_PROPERTIES
                    SetVideoProperties(videoSource_);
#endif

                }
                catch { }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured
                videoSource_.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                //Start recording
                videoSource_.Start();
            }
        }

        private void StopVideoSource(bool saveProperties)
        {
            if (videoSource_ != null)
            {
#if HAS_VIDEO_PROPERTIES
                if(saveProperties)
                    SaveVideoProperties(videoSource_);
#endif
                if (videoSource_.IsRunning)
                {
                    videoSource_.SignalToStop();
                    videoSource_.WaitForStop();
                }
                videoSource_ = null;
                videoSourceName_ = null;
            }
            comboBoxResolution.Items.Clear();
        }

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
            if (videosources != null && videosources.Count > 0)
            {
                int idx = -1;
                string camera = settings_.Camera;
                for (int i = 0; i < videosources.Count; ++i)
                {
                    comboBoxCamera.Items.Add(videosources[i].Name);
                    if (idx < 0 && camera == videosources[i].Name)
                        idx = i;
                }
                if (idx < 0)
                {
                    if (camera.Length != 0)
                    {
                        string s = String.Format("The camera \"{1}\" used last time is not found. Do you want to continue?{0}{0}"+
                                                 "\"Yes\" to continue (old camera settings will be lost!){0}" +
                                                 "\"No\" to exit apptication.", Environment.NewLine, camera);
                        if (MessageBox.Show(s, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    idx = 0;
                }
                
                videoSource_ = new VideoCaptureDevice(videosources[idx].MonikerString);
                videoSourceName_ = videosources[idx].Name;
                comboBoxCamera.SelectedIndex = idx;
                StartVideoSource();
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

            ValDIChanged += new EventHandler(OnValDIChanged);

            portNameDI_ = settings_.PortNameDI;
            baudRateDI_ = settings_.BaudRateDI;
            lastDIRequest_ = DateTime.Now;

            init_ = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopVideoSource(true);
            settings_.Save();
        }

        private DialogResult ShowDialog(Form form)
        {
            stopUpdateVideoFrames_ = true;
            try
            {
                DialogResult res = form.ShowDialog();
                stopUpdateVideoFrames_ = false;
                return res;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                stopUpdateVideoFrames_ = false;
                return DialogResult.Cancel;
            }
        }

        private delegate void SetNewFrameDelegate(Bitmap bitmap);
        private void SetNewFrame(Bitmap bitmap)
        {
            if (!stopUpdateVideoFrames_)
            {
                pictureBox.Image = bitmap;
                UpdateBrightness((Bitmap)bitmap.Clone());
            }
            SendDIRequest();

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

                string fmt = calcBrightness_.FloatFormat(calcBrightnessMode_);
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

        private void OnValDIChanged(object sender, EventArgs e)
        {
            DIReadingsChanged();
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
                if (pictureBox.Image != null)
                {
                    PointF ptOld = GetPanelCenter();
                    SizeF szOld = pictureBox.Size;
                    if (videoSource_ != null)
                        pictureBox.Size = new Size((int)(videoSource_.VideoResolution.FrameSize.Width * scale),
                                                   (int)(videoSource_.VideoResolution.FrameSize.Height * scale));
                    ScrollPictureBoxPointToCenter(new PointF(ptOld.X * pictureBox.Size.Width / szOld.Width, ptOld.Y * pictureBox.Size.Height / szOld.Height));
                }
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

            IPictureUIHandler uiHandler;
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
        private void CloseConnection(SerialConnection connection)
        {
            if (connection != null)
            {
                if (connection == connectionDI_)
                {
                    connectionDI_.Close();
                    connectionDI_ = null;
                    valDIValid_ = false;
                    UpdateDIControls();
                }
            }
        }
        private void SerialError(SerialConnection connection)
        {
            CloseConnection(connection);
        }
        private void SendCommand(SerialConnection connection_, char cmd, int receiveCnt, ReceiveDelegate receiveDelegate)
        {
            if (connection_ != null)
                connection_.SendReceiveRequest(new byte[] { (byte)cmd }, receiveCnt, new BaseConnectionHandler(this, receiveDelegate, connection_));
        }

        private void UpdateDIControls()
        {
            if (connectionDI_ == null)
            {
                textBoxDIStatus.Text = "Disconnected";
                textBoxDIValue.Text = "";
                labelDIUnit.Text = "";
            }
            else
            {
                textBoxDIStatus.Text = "Connected to " + portNameDI_;
                if (valDIValid_)
                {
                    textBoxDIValue.Text = checkBoxHideDI.Checked ? "Hidden" : valDI_.ToString();
                    labelDIUnit.Text = valDIUnit_ == DIUnit.Inch ? "inch" : "mm";
                }
                else
                {
                    textBoxDIValue.Text = "------";
                    labelDIUnit.Text = "";
                }
            }
        }

        private void SendDIRequest()
        {
            if (connectionDI_ != null)
            {
                DateTime now = DateTime.Now;
                TimeSpan timeout = TimeSpan.FromMilliseconds(1000);
                if (now - lastDIRequest_ > timeout)
                {
                    lastDIRequest_ = now;
                    SendCommand(connectionDI_, 'm', 10, this.ReceiveDIPosition);
                }
            }
        }
        
        private void ReceiveDIPosition(byte[] data)
        {
            if (data.Length >= 10 && data[0] == 0)
            {
                bool prevValid = valDIValid_;
                double prevVal = valDI_;
                DIUnit prevUnit = valDIUnit_;
                
                int iv = ((int)data[2]) * 100000 + ((int)data[3]) * 10000 + ((int)data[4]) * 1000 + ((int)data[5]) * 100 + ((int)data[6]) * 10 + ((int)data[7]);
                valDI_ = iv;
                switch (data[8])
                {
                    case 2: valDI_ /= 100; break;
                    case 3: valDI_ /= 1000; break;
                    case 4: valDI_ /= 10000; break;
                    case 5:
                    default: valDI_ /= 100000; break;
                }
                if(data[1] != 0)
                    valDI_ = -valDI_;
                valDIUnit_ = data[9] != 0 ? DIUnit.Inch : DIUnit.Mm;
                valDIValid_ = true;

                if (prevValid && (prevVal != valDI_ || prevUnit != valDIUnit_) && ValDIChanged != null)
                    ValDIChanged(this, new EventArgs());
            }
            else
                valDIValid_ = false;
            UpdateDIControls();
        }

        private string MakeFileName()
        {
            DateTime dt = DateTime.Now;
            return String.Format("ZoneDIReadings{0}-{1}-{2}_{3}-{4}-{5}.txt",
                dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"), dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"));
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
        private void DIReadingsChanged()
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
                videoSource_.SignalToStop();
                videoSource_.WaitForStop();
                videoSource_.VideoResolution = videoSource_.VideoCapabilities[comboBoxResolution.SelectedIndex];
                CorrectPictureSize();
                videoSource_.Start();
                settings_.Resolution = (string)comboBoxResolution.SelectedItem;
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

        private void comboBoxZoneNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            activeZone_ = comboBoxZoneNum.SelectedIndex;
            if (uiUpdateZoneData_ != null)
                uiUpdateZoneData_.ActiveZone = activeZone_;
            ActiveZoneChanged();
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
            if (ShowDialog(form) != DialogResult.OK)
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

        private void buttonConnectDI_Click(object sender, EventArgs e)
        {
            ConnectionForm form = new ConnectionForm(portNameDI_, baudRateDI_);
            if (ShowDialog(form) != DialogResult.OK)
                return;

            if (form.DisconnectAll)
            {
                CloseConnection(connectionDI_);
                return;
            }

            CloseConnection(connectionDI_);

            settings_.PortNameDI = portNameDI_ = form.PortName;
            settings_.BaudRateDI = baudRateDI_ = form.BaudRate;

            if (portNameDI_ != null)
            {
                SerialConnection connection = null;
                try
                {
                    connection = new SerialConnection(portNameDI_, baudRateDI_);
                }
                catch (Exception)
                {
                }

                if (connection != null)
                {
                    connectionDI_ = connection;
                    valDIValid_ = false;
                }
            }
            UpdateDIControls();
        }

        private void timerPoll_Tick(object sender, EventArgs e)
        {
            SendDIRequest();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection(connectionDI_);
        }

        private bool SaveZone(int zone)
        {
            if (valDIValid_)
            {
                if (zoneReadings_ == null)
                    zoneReadings_ = new ZoneReading[zoneBounds_.Length - 1];
                if (zone >= 0 && zone < zoneReadings_.Length)
                {
                    if (zoneReadings_[zone].seq_ == null)
                        zoneReadings_[zone].seq_ = new List<double>();
                    zoneReadings_[zone].seq_.Add(valDI_);
                }
                return true;
            }
            return false;
        }

        private void buttonSaveZoneRedingsToFile_Click(object sender, EventArgs e)
        {
            if (zoneReadings_ == null)
                return;

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = MakeFileName();
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            stopUpdateVideoFrames_ = true;
            try
            {
                DialogResult res = savefile.ShowDialog();
                stopUpdateVideoFrames_ = false;
                if (res != DialogResult.OK)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                stopUpdateVideoFrames_ = false;
                return;
            }

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(savefile.FileName))
                {
                    for (int i = 0; i < zoneReadings_.Length; ++i)
                    {
                        if (zoneReadings_[i].seq_ == null || zoneReadings_[i].seq_.Count <= 0)
                            sw.WriteLine("N/A");
                        else if (zoneReadings_[i].seq_.Count == 1)
                            sw.WriteLine(zoneReadings_[i].seq_[0].ToString());
                        else
                        {
                            string s = "";
                            for (int j = 0; j < zoneReadings_[i].seq_.Count; ++j)
                            {
                                if (j > 0)
                                    s += ",";
                                s += zoneReadings_[i].seq_[j].ToString();
                            }
                            sw.WriteLine(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBoxHideDI_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDIControls();
        }

        private void buttonStoreDI_Click(object sender, EventArgs e)
        {
            if (SaveZone(comboBoxZoneNum.SelectedIndex))
            {
                if (checkBoxAdvanceFwd.Checked && comboBoxZoneNum.SelectedIndex < comboBoxZoneNum.Items.Count - 1)
                    comboBoxZoneNum.SelectedIndex = comboBoxZoneNum.SelectedIndex + 1;
                else if (checkBoxAdvanceBack.Checked && comboBoxZoneNum.SelectedIndex > 0)
                    comboBoxZoneNum.SelectedIndex = comboBoxZoneNum.SelectedIndex - 1;
            }
            else
                Console.Beep();
        }

        private void checkBoxAdvanceFwd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAdvanceFwd.Checked && checkBoxAdvanceBack.Checked)
                checkBoxAdvanceBack.Checked = false;
        }

        private void checkBoxAdvanceBack_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAdvanceFwd.Checked && checkBoxAdvanceBack.Checked)
                checkBoxAdvanceFwd.Checked = false;
        }

        private void buttonEditZoneReadings_Click(object sender, EventArgs e)
        {
            if (zoneBounds_ != null)
            {
                EditDIReadings form = new EditDIReadings(zoneReadings_, zoneBounds_.Length - 1);
                if (ShowDialog(form) == DialogResult.OK)
                    zoneReadings_ = form.ZoneReadings;
            }
        }

        private void buttonClearDIs_Click(object sender, EventArgs e)
        {
            stopUpdateVideoFrames_ = true;
            DialogResult res = MessageBox.Show("Clear all? you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            stopUpdateVideoFrames_ = false;
            if (res == DialogResult.Yes)
                zoneReadings_ = null;
        }

        private int RebuildCameraArrayAndGetIndex(FilterInfoCollection videosources)
        {
            comboBoxCamera.Items.Clear();
            int idx = -1;
            for (int i = 0; i < videosources.Count; ++i)
            {
                comboBoxCamera.Items.Add(videosources[i].Name);
                if (idx < 0 && (string)comboBoxCamera.SelectedItem == videosources[i].Name)
                    idx = i;
            }
            if (idx < 0)
                idx = 0;

            init_ = false;
            comboBoxCamera.SelectedIndex = idx;
            init_ = true;

            return idx;
        }
        
        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;

            int idx = comboBoxCamera.SelectedIndex;

            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videosources != null && videosources.Count > 0)
            {
                if (videosources.Count != comboBoxCamera.Items.Count ||
                    comboBoxCamera.SelectedIndex >= videosources.Count ||
                    videosources[comboBoxCamera.SelectedIndex].Name != (string)comboBoxCamera.SelectedItem)
                {
                    // rebuild camera array
                    idx = RebuildCameraArrayAndGetIndex(videosources);
                }

                StopVideoSource(true);
                videoSource_ = new VideoCaptureDevice(videosources[idx].MonikerString);
                videoSourceName_ = videosources[idx].Name;
                StartVideoSource();
            }
        }

        private void comboBoxCamera_DropDown(object sender, EventArgs e)
        {
            if (!init_)
                return;

            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videosources != null && videosources.Count > 0)
            {
                if (videosources.Count != comboBoxCamera.Items.Count ||
                    comboBoxCamera.SelectedIndex >= videosources.Count ||
                    videosources[comboBoxCamera.SelectedIndex].Name != (string)comboBoxCamera.SelectedItem)
                {
                    // rebuild camera array
                    RebuildCameraArrayAndGetIndex(videosources);
                }
            }
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
        [DefaultSettingValueAttribute("30")]
        public float ZoneAngle
        {
            get { return (float)this["ZoneAngle"]; }
            set { this["ZoneAngle"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("60")]
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
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string Camera
        {
            get { return (string)this["Camera"]; }
            set { this["Camera"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string Resolution
        {
            get { return (string)this["Resolution"]; }
            set { this["Resolution"] = value; }
        }

#if HAS_VIDEO_PROPERTIES
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

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string PortNameDI
        {
            get { return (string)this["PortNameDI"]; }
            set { this["PortNameDI"] = value; }
        }
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("115200")]
        public int BaudRateDI
        {
            get { return (int)this["BaudRateDI"]; }
            set { this["BaudRateDI"] = value; }
        }
    }
}
