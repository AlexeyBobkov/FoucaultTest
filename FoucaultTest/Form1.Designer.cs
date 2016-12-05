namespace FoucaultTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageEdge = new System.Windows.Forms.TabPage();
            this.buttonDelMirrorBound = new System.Windows.Forms.Button();
            this.buttonEdgeDetect = new System.Windows.Forms.Button();
            this.tabPageFoucault = new System.Windows.Forms.TabPage();
            this.buttonClearDIs = new System.Windows.Forms.Button();
            this.buttonSaveZoneRedingsToFile = new System.Windows.Forms.Button();
            this.buttonSaveAndPrev = new System.Windows.Forms.Button();
            this.buttonSaveAndNext = new System.Windows.Forms.Button();
            this.checkBoxUseCalibration = new System.Windows.Forms.CheckBox();
            this.labelBrightnessCalib = new System.Windows.Forms.Label();
            this.buttonBrightnessCalib = new System.Windows.Forms.Button();
            this.checkBoxMedianCalc = new System.Windows.Forms.CheckBox();
            this.buttonFoucaultOptions = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBrightnessDiff = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBrightnessRight = new System.Windows.Forms.TextBox();
            this.textBoxBrightnessLeft = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxZoneVisualization = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxZoneNum = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelDIUnit = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDIValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDIStatus = new System.Windows.Forms.TextBox();
            this.buttonConnectDI = new System.Windows.Forms.Button();
            this.buttonAutoPosition = new System.Windows.Forms.Button();
            this.checkBoxFitToScreen = new System.Windows.Forms.CheckBox();
            this.buttonCopyPicture = new System.Windows.Forms.Button();
            this.labelScale = new System.Windows.Forms.Label();
            this.hScrollBarScale = new System.Windows.Forms.HScrollBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonCameraSettings = new System.Windows.Forms.Button();
            this.comboBoxResolution = new System.Windows.Forms.ComboBox();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.pictureBox = new FoucaultTestClasses.CustomPictureBox();
            this.timerPoll = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageEdge.SuspendLayout();
            this.tabPageFoucault.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1124, 798);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageEdge);
            this.tabControl.Controls.Add(this.tabPageFoucault);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(807, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(314, 792);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageEdge
            // 
            this.tabPageEdge.Controls.Add(this.buttonDelMirrorBound);
            this.tabPageEdge.Controls.Add(this.buttonEdgeDetect);
            this.tabPageEdge.Location = new System.Drawing.Point(4, 22);
            this.tabPageEdge.Name = "tabPageEdge";
            this.tabPageEdge.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEdge.Size = new System.Drawing.Size(306, 766);
            this.tabPageEdge.TabIndex = 0;
            this.tabPageEdge.Text = "Mirror Bound";
            this.tabPageEdge.UseVisualStyleBackColor = true;
            // 
            // buttonDelMirrorBound
            // 
            this.buttonDelMirrorBound.Location = new System.Drawing.Point(74, 36);
            this.buttonDelMirrorBound.Name = "buttonDelMirrorBound";
            this.buttonDelMirrorBound.Size = new System.Drawing.Size(167, 67);
            this.buttonDelMirrorBound.TabIndex = 0;
            this.buttonDelMirrorBound.Text = "Delete Mirror Bound";
            this.buttonDelMirrorBound.UseVisualStyleBackColor = true;
            this.buttonDelMirrorBound.Click += new System.EventHandler(this.buttonDelMirrorBound_Click);
            // 
            // buttonEdgeDetect
            // 
            this.buttonEdgeDetect.Location = new System.Drawing.Point(74, 144);
            this.buttonEdgeDetect.Name = "buttonEdgeDetect";
            this.buttonEdgeDetect.Size = new System.Drawing.Size(167, 64);
            this.buttonEdgeDetect.TabIndex = 1;
            this.buttonEdgeDetect.Text = "Edge Auto Detect";
            this.buttonEdgeDetect.UseVisualStyleBackColor = true;
            // 
            // tabPageFoucault
            // 
            this.tabPageFoucault.Controls.Add(this.buttonClearDIs);
            this.tabPageFoucault.Controls.Add(this.buttonSaveZoneRedingsToFile);
            this.tabPageFoucault.Controls.Add(this.buttonSaveAndPrev);
            this.tabPageFoucault.Controls.Add(this.buttonSaveAndNext);
            this.tabPageFoucault.Controls.Add(this.checkBoxUseCalibration);
            this.tabPageFoucault.Controls.Add(this.labelBrightnessCalib);
            this.tabPageFoucault.Controls.Add(this.buttonBrightnessCalib);
            this.tabPageFoucault.Controls.Add(this.checkBoxMedianCalc);
            this.tabPageFoucault.Controls.Add(this.buttonFoucaultOptions);
            this.tabPageFoucault.Controls.Add(this.label5);
            this.tabPageFoucault.Controls.Add(this.textBoxBrightnessDiff);
            this.tabPageFoucault.Controls.Add(this.label4);
            this.tabPageFoucault.Controls.Add(this.textBoxBrightnessRight);
            this.tabPageFoucault.Controls.Add(this.textBoxBrightnessLeft);
            this.tabPageFoucault.Controls.Add(this.label3);
            this.tabPageFoucault.Controls.Add(this.comboBoxZoneVisualization);
            this.tabPageFoucault.Controls.Add(this.label2);
            this.tabPageFoucault.Controls.Add(this.label1);
            this.tabPageFoucault.Controls.Add(this.comboBoxZoneNum);
            this.tabPageFoucault.Location = new System.Drawing.Point(4, 22);
            this.tabPageFoucault.Name = "tabPageFoucault";
            this.tabPageFoucault.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFoucault.Size = new System.Drawing.Size(306, 766);
            this.tabPageFoucault.TabIndex = 1;
            this.tabPageFoucault.Text = "Foucault";
            this.tabPageFoucault.UseVisualStyleBackColor = true;
            // 
            // buttonClearDIs
            // 
            this.buttonClearDIs.Location = new System.Drawing.Point(161, 90);
            this.buttonClearDIs.Name = "buttonClearDIs";
            this.buttonClearDIs.Size = new System.Drawing.Size(124, 30);
            this.buttonClearDIs.TabIndex = 6;
            this.buttonClearDIs.Text = "Clear DIs";
            this.buttonClearDIs.UseVisualStyleBackColor = true;
            this.buttonClearDIs.Click += new System.EventHandler(this.buttonClearDIs_Click);
            // 
            // buttonSaveZoneRedingsToFile
            // 
            this.buttonSaveZoneRedingsToFile.Location = new System.Drawing.Point(20, 90);
            this.buttonSaveZoneRedingsToFile.Name = "buttonSaveZoneRedingsToFile";
            this.buttonSaveZoneRedingsToFile.Size = new System.Drawing.Size(124, 30);
            this.buttonSaveZoneRedingsToFile.TabIndex = 5;
            this.buttonSaveZoneRedingsToFile.Text = "Save DIs to file";
            this.buttonSaveZoneRedingsToFile.UseVisualStyleBackColor = true;
            this.buttonSaveZoneRedingsToFile.Click += new System.EventHandler(this.buttonSaveZoneRedingsToFile_Click);
            // 
            // buttonSaveAndPrev
            // 
            this.buttonSaveAndPrev.Location = new System.Drawing.Point(161, 54);
            this.buttonSaveAndPrev.Name = "buttonSaveAndPrev";
            this.buttonSaveAndPrev.Size = new System.Drawing.Size(124, 30);
            this.buttonSaveAndPrev.TabIndex = 4;
            this.buttonSaveAndPrev.Text = "Save DI && Prev";
            this.buttonSaveAndPrev.UseVisualStyleBackColor = true;
            this.buttonSaveAndPrev.Click += new System.EventHandler(this.buttonSaveAndPrev_Click);
            // 
            // buttonSaveAndNext
            // 
            this.buttonSaveAndNext.Location = new System.Drawing.Point(20, 54);
            this.buttonSaveAndNext.Name = "buttonSaveAndNext";
            this.buttonSaveAndNext.Size = new System.Drawing.Size(124, 30);
            this.buttonSaveAndNext.TabIndex = 3;
            this.buttonSaveAndNext.Text = "Save DI && Next";
            this.buttonSaveAndNext.UseVisualStyleBackColor = true;
            this.buttonSaveAndNext.Click += new System.EventHandler(this.buttonSaveAndNext_Click);
            // 
            // checkBoxUseCalibration
            // 
            this.checkBoxUseCalibration.AutoSize = true;
            this.checkBoxUseCalibration.Checked = true;
            this.checkBoxUseCalibration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseCalibration.Enabled = false;
            this.checkBoxUseCalibration.Location = new System.Drawing.Point(20, 201);
            this.checkBoxUseCalibration.Name = "checkBoxUseCalibration";
            this.checkBoxUseCalibration.Size = new System.Drawing.Size(97, 17);
            this.checkBoxUseCalibration.TabIndex = 11;
            this.checkBoxUseCalibration.Text = "Use Calibration";
            this.checkBoxUseCalibration.UseVisualStyleBackColor = true;
            this.checkBoxUseCalibration.CheckedChanged += new System.EventHandler(this.checkBoxUseCalibration_CheckedChanged);
            // 
            // labelBrightnessCalib
            // 
            this.labelBrightnessCalib.Location = new System.Drawing.Point(158, 162);
            this.labelBrightnessCalib.Name = "labelBrightnessCalib";
            this.labelBrightnessCalib.Size = new System.Drawing.Size(124, 33);
            this.labelBrightnessCalib.TabIndex = 10;
            this.labelBrightnessCalib.Text = "No Caliration";
            this.labelBrightnessCalib.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonBrightnessCalib
            // 
            this.buttonBrightnessCalib.Location = new System.Drawing.Point(20, 162);
            this.buttonBrightnessCalib.Name = "buttonBrightnessCalib";
            this.buttonBrightnessCalib.Size = new System.Drawing.Size(124, 33);
            this.buttonBrightnessCalib.TabIndex = 9;
            this.buttonBrightnessCalib.Text = "Calibrate";
            this.buttonBrightnessCalib.UseVisualStyleBackColor = true;
            this.buttonBrightnessCalib.Click += new System.EventHandler(this.buttonBrightnessCalib_Click);
            // 
            // checkBoxMedianCalc
            // 
            this.checkBoxMedianCalc.AutoSize = true;
            this.checkBoxMedianCalc.Checked = true;
            this.checkBoxMedianCalc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMedianCalc.Location = new System.Drawing.Point(128, 328);
            this.checkBoxMedianCalc.Name = "checkBoxMedianCalc";
            this.checkBoxMedianCalc.Size = new System.Drawing.Size(61, 17);
            this.checkBoxMedianCalc.TabIndex = 18;
            this.checkBoxMedianCalc.Text = "Median";
            this.checkBoxMedianCalc.UseVisualStyleBackColor = true;
            this.checkBoxMedianCalc.CheckedChanged += new System.EventHandler(this.checkBoxMedianCalc_CheckedChanged);
            // 
            // buttonFoucaultOptions
            // 
            this.buttonFoucaultOptions.Location = new System.Drawing.Point(94, 410);
            this.buttonFoucaultOptions.Name = "buttonFoucaultOptions";
            this.buttonFoucaultOptions.Size = new System.Drawing.Size(124, 46);
            this.buttonFoucaultOptions.TabIndex = 0;
            this.buttonFoucaultOptions.Text = "Options";
            this.buttonFoucaultOptions.UseVisualStyleBackColor = true;
            this.buttonFoucaultOptions.Click += new System.EventHandler(this.buttonFoucaultOptions_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(16, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "Difference";
            // 
            // textBoxBrightnessDiff
            // 
            this.textBoxBrightnessDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBrightnessDiff.Location = new System.Drawing.Point(20, 328);
            this.textBoxBrightnessDiff.Name = "textBoxBrightnessDiff";
            this.textBoxBrightnessDiff.ReadOnly = true;
            this.textBoxBrightnessDiff.Size = new System.Drawing.Size(92, 31);
            this.textBoxBrightnessDiff.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(125, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Brighness Right";
            // 
            // textBoxBrightnessRight
            // 
            this.textBoxBrightnessRight.Location = new System.Drawing.Point(128, 274);
            this.textBoxBrightnessRight.Name = "textBoxBrightnessRight";
            this.textBoxBrightnessRight.ReadOnly = true;
            this.textBoxBrightnessRight.Size = new System.Drawing.Size(91, 20);
            this.textBoxBrightnessRight.TabIndex = 15;
            // 
            // textBoxBrightnessLeft
            // 
            this.textBoxBrightnessLeft.Location = new System.Drawing.Point(20, 274);
            this.textBoxBrightnessLeft.Name = "textBoxBrightnessLeft";
            this.textBoxBrightnessLeft.ReadOnly = true;
            this.textBoxBrightnessLeft.Size = new System.Drawing.Size(91, 20);
            this.textBoxBrightnessLeft.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Brightness Left";
            // 
            // comboBoxZoneVisualization
            // 
            this.comboBoxZoneVisualization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneVisualization.FormattingEnabled = true;
            this.comboBoxZoneVisualization.Location = new System.Drawing.Point(161, 27);
            this.comboBoxZoneVisualization.Name = "comboBoxZoneVisualization";
            this.comboBoxZoneVisualization.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneVisualization.TabIndex = 8;
            this.comboBoxZoneVisualization.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoneVisualization_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Visualization";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zone #";
            // 
            // comboBoxZoneNum
            // 
            this.comboBoxZoneNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneNum.FormattingEnabled = true;
            this.comboBoxZoneNum.Location = new System.Drawing.Point(20, 27);
            this.comboBoxZoneNum.Name = "comboBoxZoneNum";
            this.comboBoxZoneNum.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneNum.TabIndex = 1;
            this.comboBoxZoneNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoneNum_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelPictureBox, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 168F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(798, 792);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelDIUnit);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBoxDIValue);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.textBoxDIStatus);
            this.panel1.Controls.Add(this.buttonConnectDI);
            this.panel1.Controls.Add(this.buttonAutoPosition);
            this.panel1.Controls.Add(this.checkBoxFitToScreen);
            this.panel1.Controls.Add(this.buttonCopyPicture);
            this.panel1.Controls.Add(this.labelScale);
            this.panel1.Controls.Add(this.hScrollBarScale);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.buttonCameraSettings);
            this.panel1.Controls.Add(this.comboBoxResolution);
            this.panel1.Controls.Add(this.comboBoxCamera);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 627);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 162);
            this.panel1.TabIndex = 0;
            // 
            // labelDIUnit
            // 
            this.labelDIUnit.Location = new System.Drawing.Point(504, 72);
            this.labelDIUnit.Name = "labelDIUnit";
            this.labelDIUnit.Size = new System.Drawing.Size(86, 21);
            this.labelDIUnit.TabIndex = 15;
            this.labelDIUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(423, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "DI Reading:";
            // 
            // textBoxDIValue
            // 
            this.textBoxDIValue.Location = new System.Drawing.Point(426, 72);
            this.textBoxDIValue.Name = "textBoxDIValue";
            this.textBoxDIValue.ReadOnly = true;
            this.textBoxDIValue.Size = new System.Drawing.Size(72, 20);
            this.textBoxDIValue.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(424, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Dial Indicator";
            // 
            // textBoxDIStatus
            // 
            this.textBoxDIStatus.Location = new System.Drawing.Point(427, 31);
            this.textBoxDIStatus.Name = "textBoxDIStatus";
            this.textBoxDIStatus.ReadOnly = true;
            this.textBoxDIStatus.Size = new System.Drawing.Size(216, 20);
            this.textBoxDIStatus.TabIndex = 11;
            this.textBoxDIStatus.Text = "Disconnected";
            // 
            // buttonConnectDI
            // 
            this.buttonConnectDI.Location = new System.Drawing.Point(661, 31);
            this.buttonConnectDI.Name = "buttonConnectDI";
            this.buttonConnectDI.Size = new System.Drawing.Size(112, 37);
            this.buttonConnectDI.TabIndex = 12;
            this.buttonConnectDI.Text = "Connect";
            this.buttonConnectDI.UseVisualStyleBackColor = true;
            this.buttonConnectDI.Click += new System.EventHandler(this.buttonConnectDI_Click);
            // 
            // buttonAutoPosition
            // 
            this.buttonAutoPosition.Location = new System.Drawing.Point(250, 72);
            this.buttonAutoPosition.Name = "buttonAutoPosition";
            this.buttonAutoPosition.Size = new System.Drawing.Size(132, 32);
            this.buttonAutoPosition.TabIndex = 8;
            this.buttonAutoPosition.Text = "Auto Position";
            this.buttonAutoPosition.UseVisualStyleBackColor = true;
            this.buttonAutoPosition.Click += new System.EventHandler(this.buttonAutoPosition_Click);
            // 
            // checkBoxFitToScreen
            // 
            this.checkBoxFitToScreen.AutoSize = true;
            this.checkBoxFitToScreen.Checked = true;
            this.checkBoxFitToScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFitToScreen.Location = new System.Drawing.Point(144, 72);
            this.checkBoxFitToScreen.Name = "checkBoxFitToScreen";
            this.checkBoxFitToScreen.Size = new System.Drawing.Size(86, 17);
            this.checkBoxFitToScreen.TabIndex = 7;
            this.checkBoxFitToScreen.Text = "Fit to Screen";
            this.checkBoxFitToScreen.UseVisualStyleBackColor = true;
            this.checkBoxFitToScreen.CheckedChanged += new System.EventHandler(this.checkBoxFitToScreen_CheckedChanged);
            // 
            // buttonCopyPicture
            // 
            this.buttonCopyPicture.Location = new System.Drawing.Point(250, 110);
            this.buttonCopyPicture.Name = "buttonCopyPicture";
            this.buttonCopyPicture.Size = new System.Drawing.Size(132, 32);
            this.buttonCopyPicture.TabIndex = 9;
            this.buttonCopyPicture.Text = "Copy Picture";
            this.buttonCopyPicture.UseVisualStyleBackColor = true;
            this.buttonCopyPicture.Click += new System.EventHandler(this.buttonCopyPicture_Click);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(141, 14);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(34, 13);
            this.labelScale.TabIndex = 5;
            this.labelScale.Text = "Scale";
            // 
            // hScrollBarScale
            // 
            this.hScrollBarScale.LargeChange = 1;
            this.hScrollBarScale.Location = new System.Drawing.Point(144, 31);
            this.hScrollBarScale.Minimum = -100;
            this.hScrollBarScale.Name = "hScrollBarScale";
            this.hScrollBarScale.Size = new System.Drawing.Size(238, 21);
            this.hScrollBarScale.TabIndex = 6;
            this.hScrollBarScale.TabStop = true;
            this.hScrollBarScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScale_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Resolution";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Camera";
            // 
            // buttonCameraSettings
            // 
            this.buttonCameraSettings.Location = new System.Drawing.Point(21, 110);
            this.buttonCameraSettings.Name = "buttonCameraSettings";
            this.buttonCameraSettings.Size = new System.Drawing.Size(104, 32);
            this.buttonCameraSettings.TabIndex = 4;
            this.buttonCameraSettings.Text = "Settings";
            this.buttonCameraSettings.UseVisualStyleBackColor = true;
            this.buttonCameraSettings.Click += new System.EventHandler(this.buttonCameraSettings_Click);
            // 
            // comboBoxResolution
            // 
            this.comboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxResolution.FormattingEnabled = true;
            this.comboBoxResolution.Location = new System.Drawing.Point(21, 72);
            this.comboBoxResolution.Name = "comboBoxResolution";
            this.comboBoxResolution.Size = new System.Drawing.Size(104, 21);
            this.comboBoxResolution.TabIndex = 3;
            this.comboBoxResolution.SelectedIndexChanged += new System.EventHandler(this.comboBoxResolution_SelectedIndexChanged);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(21, 31);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(104, 21);
            this.comboBoxCamera.TabIndex = 1;
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.AutoScroll = true;
            this.panelPictureBox.Controls.Add(this.pictureBox);
            this.panelPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPictureBox.Location = new System.Drawing.Point(3, 3);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(792, 618);
            this.panelPictureBox.TabIndex = 0;
            this.panelPictureBox.SizeChanged += new System.EventHandler(this.panelPictureBox_SizeChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = null;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(575, 566);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // timerPoll
            // 
            this.timerPoll.Enabled = true;
            this.timerPoll.Interval = 300;
            this.timerPoll.Tick += new System.EventHandler(this.timerPoll_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 798);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Foucault Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageEdge.ResumeLayout(false);
            this.tabPageFoucault.ResumeLayout(false);
            this.tabPageFoucault.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPictureBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FoucaultTestClasses.CustomPictureBox pictureBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageEdge;
        private System.Windows.Forms.TabPage tabPageFoucault;
        private System.Windows.Forms.ComboBox comboBoxZoneNum;
        private System.Windows.Forms.ComboBox comboBoxZoneVisualization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonEdgeDetect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBrightnessDiff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBrightnessRight;
        private System.Windows.Forms.TextBox textBoxBrightnessLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonFoucaultOptions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonCameraSettings;
        private System.Windows.Forms.ComboBox comboBoxResolution;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.Panel panelPictureBox;
        private System.Windows.Forms.HScrollBar hScrollBarScale;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Button buttonCopyPicture;
        private System.Windows.Forms.CheckBox checkBoxFitToScreen;
        private System.Windows.Forms.Button buttonDelMirrorBound;
        private System.Windows.Forms.CheckBox checkBoxMedianCalc;
        private System.Windows.Forms.Button buttonBrightnessCalib;
        private System.Windows.Forms.Label labelBrightnessCalib;
        private System.Windows.Forms.CheckBox checkBoxUseCalibration;
        private System.Windows.Forms.Button buttonAutoPosition;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDIValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDIStatus;
        private System.Windows.Forms.Button buttonConnectDI;
        private System.Windows.Forms.Label labelDIUnit;
        private System.Windows.Forms.Timer timerPoll;
        private System.Windows.Forms.Button buttonSaveAndPrev;
        private System.Windows.Forms.Button buttonSaveAndNext;
        private System.Windows.Forms.Button buttonSaveZoneRedingsToFile;
        private System.Windows.Forms.Button buttonClearDIs;
    }
}

