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
            this.buttonLoadMirrorData = new System.Windows.Forms.Button();
            this.tabPageFoucault = new System.Windows.Forms.TabPage();
            this.textBoxDbgBrightness = new System.Windows.Forms.TextBox();
            this.textBoxDbgDI = new System.Windows.Forms.TextBox();
            this.checkBoxDbgEmulateBrightness = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgEmulateDI = new System.Windows.Forms.CheckBox();
            this.buttonAutoMeasurements = new System.Windows.Forms.Button();
            this.buttonClearDIs = new System.Windows.Forms.Button();
            this.buttonEditZoneReadings = new System.Windows.Forms.Button();
            this.checkBoxAdvanceBack = new System.Windows.Forms.CheckBox();
            this.checkBoxAdvanceFwd = new System.Windows.Forms.CheckBox();
            this.buttonStoreDI = new System.Windows.Forms.Button();
            this.buttonSaveZoneRedingsToFile = new System.Windows.Forms.Button();
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
            this.tabPageLWT = new System.Windows.Forms.TabPage();
            this.buttonClearDIsLWT = new System.Windows.Forms.Button();
            this.buttonEditZoneReadingsLWT = new System.Windows.Forms.Button();
            this.checkBoxAdvanceBackLWT = new System.Windows.Forms.CheckBox();
            this.checkBoxAdvanceFwdLWT = new System.Windows.Forms.CheckBox();
            this.buttonStoreDI_LWT = new System.Windows.Forms.Button();
            this.buttonSaveZoneRedingsToFileLWT = new System.Windows.Forms.Button();
            this.buttonLWTOptions = new System.Windows.Forms.Button();
            this.comboBoxZoneVizualizationLWT = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxZoneNumLWT = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxUseOffset = new System.Windows.Forms.CheckBox();
            this.labelOffset = new System.Windows.Forms.Label();
            this.buttonSetZero = new System.Windows.Forms.Button();
            this.checkBoxHideDI = new System.Windows.Forms.CheckBox();
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
            this.buttonSavePicture = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageEdge.SuspendLayout();
            this.tabPageFoucault.SuspendLayout();
            this.tabPageLWT.SuspendLayout();
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
            this.tabControl.Controls.Add(this.tabPageLWT);
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
            this.tabPageEdge.Controls.Add(this.buttonLoadMirrorData);
            this.tabPageEdge.Location = new System.Drawing.Point(4, 22);
            this.tabPageEdge.Name = "tabPageEdge";
            this.tabPageEdge.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEdge.Size = new System.Drawing.Size(306, 766);
            this.tabPageEdge.TabIndex = 0;
            this.tabPageEdge.Text = "Working Rectangle";
            this.tabPageEdge.UseVisualStyleBackColor = true;
            // 
            // buttonDelMirrorBound
            // 
            this.buttonDelMirrorBound.Location = new System.Drawing.Point(74, 36);
            this.buttonDelMirrorBound.Name = "buttonDelMirrorBound";
            this.buttonDelMirrorBound.Size = new System.Drawing.Size(167, 67);
            this.buttonDelMirrorBound.TabIndex = 0;
            this.buttonDelMirrorBound.Text = "Delete Working Rectangle";
            this.buttonDelMirrorBound.UseVisualStyleBackColor = true;
            this.buttonDelMirrorBound.Click += new System.EventHandler(this.buttonDelMirrorBound_Click);
            // 
            // buttonLoadMirrorData
            // 
            this.buttonLoadMirrorData.Location = new System.Drawing.Point(74, 144);
            this.buttonLoadMirrorData.Name = "buttonLoadMirrorData";
            this.buttonLoadMirrorData.Size = new System.Drawing.Size(167, 64);
            this.buttonLoadMirrorData.TabIndex = 1;
            this.buttonLoadMirrorData.Text = "Load Mirror Data";
            this.buttonLoadMirrorData.UseVisualStyleBackColor = true;
            this.buttonLoadMirrorData.Click += new System.EventHandler(this.buttonLoadMirrorData_Click);
            // 
            // tabPageFoucault
            // 
            this.tabPageFoucault.Controls.Add(this.textBoxDbgBrightness);
            this.tabPageFoucault.Controls.Add(this.textBoxDbgDI);
            this.tabPageFoucault.Controls.Add(this.checkBoxDbgEmulateBrightness);
            this.tabPageFoucault.Controls.Add(this.checkBoxDbgEmulateDI);
            this.tabPageFoucault.Controls.Add(this.buttonAutoMeasurements);
            this.tabPageFoucault.Controls.Add(this.buttonClearDIs);
            this.tabPageFoucault.Controls.Add(this.buttonEditZoneReadings);
            this.tabPageFoucault.Controls.Add(this.checkBoxAdvanceBack);
            this.tabPageFoucault.Controls.Add(this.checkBoxAdvanceFwd);
            this.tabPageFoucault.Controls.Add(this.buttonStoreDI);
            this.tabPageFoucault.Controls.Add(this.buttonSaveZoneRedingsToFile);
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
            this.tabPageFoucault.Text = "Foucault Test";
            this.tabPageFoucault.UseVisualStyleBackColor = true;
            // 
            // textBoxDbgBrightness
            // 
            this.textBoxDbgBrightness.Location = new System.Drawing.Point(161, 660);
            this.textBoxDbgBrightness.Name = "textBoxDbgBrightness";
            this.textBoxDbgBrightness.Size = new System.Drawing.Size(100, 20);
            this.textBoxDbgBrightness.TabIndex = 22;
            this.textBoxDbgBrightness.Text = "100";
            // 
            // textBoxDbgDI
            // 
            this.textBoxDbgDI.Location = new System.Drawing.Point(161, 631);
            this.textBoxDbgDI.Name = "textBoxDbgDI";
            this.textBoxDbgDI.Size = new System.Drawing.Size(100, 20);
            this.textBoxDbgDI.TabIndex = 20;
            this.textBoxDbgDI.Text = "0";
            // 
            // checkBoxDbgEmulateBrightness
            // 
            this.checkBoxDbgEmulateBrightness.AutoSize = true;
            this.checkBoxDbgEmulateBrightness.Location = new System.Drawing.Point(17, 660);
            this.checkBoxDbgEmulateBrightness.Name = "checkBoxDbgEmulateBrightness";
            this.checkBoxDbgEmulateBrightness.Size = new System.Drawing.Size(116, 17);
            this.checkBoxDbgEmulateBrightness.TabIndex = 21;
            this.checkBoxDbgEmulateBrightness.Text = "Emulate Brightness";
            this.checkBoxDbgEmulateBrightness.UseVisualStyleBackColor = true;
            this.checkBoxDbgEmulateBrightness.CheckedChanged += new System.EventHandler(this.checkBoxDbgEmulateBrightness_CheckedChanged);
            // 
            // checkBoxDbgEmulateDI
            // 
            this.checkBoxDbgEmulateDI.AutoSize = true;
            this.checkBoxDbgEmulateDI.Location = new System.Drawing.Point(17, 631);
            this.checkBoxDbgEmulateDI.Name = "checkBoxDbgEmulateDI";
            this.checkBoxDbgEmulateDI.Size = new System.Drawing.Size(78, 17);
            this.checkBoxDbgEmulateDI.TabIndex = 19;
            this.checkBoxDbgEmulateDI.Text = "Emulate DI";
            this.checkBoxDbgEmulateDI.UseVisualStyleBackColor = true;
            this.checkBoxDbgEmulateDI.CheckedChanged += new System.EventHandler(this.checkBoxDbgEmulateDI_CheckedChanged);
            // 
            // buttonAutoMeasurements
            // 
            this.buttonAutoMeasurements.Location = new System.Drawing.Point(161, 378);
            this.buttonAutoMeasurements.Name = "buttonAutoMeasurements";
            this.buttonAutoMeasurements.Size = new System.Drawing.Size(124, 31);
            this.buttonAutoMeasurements.TabIndex = 17;
            this.buttonAutoMeasurements.Text = "Start Auto";
            this.buttonAutoMeasurements.UseVisualStyleBackColor = true;
            this.buttonAutoMeasurements.Click += new System.EventHandler(this.buttonAutoMeasurements_Click);
            // 
            // buttonClearDIs
            // 
            this.buttonClearDIs.Location = new System.Drawing.Point(78, 240);
            this.buttonClearDIs.Name = "buttonClearDIs";
            this.buttonClearDIs.Size = new System.Drawing.Size(155, 39);
            this.buttonClearDIs.TabIndex = 9;
            this.buttonClearDIs.Text = "Clear Readings";
            this.buttonClearDIs.UseVisualStyleBackColor = true;
            this.buttonClearDIs.Click += new System.EventHandler(this.buttonClearDIs_Click);
            // 
            // buttonEditZoneReadings
            // 
            this.buttonEditZoneReadings.Location = new System.Drawing.Point(79, 150);
            this.buttonEditZoneReadings.Name = "buttonEditZoneReadings";
            this.buttonEditZoneReadings.Size = new System.Drawing.Size(154, 39);
            this.buttonEditZoneReadings.TabIndex = 7;
            this.buttonEditZoneReadings.Text = "Edit Readings";
            this.buttonEditZoneReadings.UseVisualStyleBackColor = true;
            this.buttonEditZoneReadings.Click += new System.EventHandler(this.buttonEditZoneReadings_Click);
            // 
            // checkBoxAdvanceBack
            // 
            this.checkBoxAdvanceBack.AutoSize = true;
            this.checkBoxAdvanceBack.Location = new System.Drawing.Point(161, 118);
            this.checkBoxAdvanceBack.Name = "checkBoxAdvanceBack";
            this.checkBoxAdvanceBack.Size = new System.Drawing.Size(97, 17);
            this.checkBoxAdvanceBack.TabIndex = 6;
            this.checkBoxAdvanceBack.Text = "Advance Back";
            this.checkBoxAdvanceBack.UseVisualStyleBackColor = true;
            this.checkBoxAdvanceBack.CheckedChanged += new System.EventHandler(this.checkBoxAdvanceBack_CheckedChanged);
            // 
            // checkBoxAdvanceFwd
            // 
            this.checkBoxAdvanceFwd.AutoSize = true;
            this.checkBoxAdvanceFwd.Checked = true;
            this.checkBoxAdvanceFwd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAdvanceFwd.Location = new System.Drawing.Point(18, 118);
            this.checkBoxAdvanceFwd.Name = "checkBoxAdvanceFwd";
            this.checkBoxAdvanceFwd.Size = new System.Drawing.Size(110, 17);
            this.checkBoxAdvanceFwd.TabIndex = 5;
            this.checkBoxAdvanceFwd.Text = "Advance Forward";
            this.checkBoxAdvanceFwd.UseVisualStyleBackColor = true;
            this.checkBoxAdvanceFwd.CheckedChanged += new System.EventHandler(this.checkBoxAdvanceFwd_CheckedChanged);
            // 
            // buttonStoreDI
            // 
            this.buttonStoreDI.Location = new System.Drawing.Point(161, 30);
            this.buttonStoreDI.Name = "buttonStoreDI";
            this.buttonStoreDI.Size = new System.Drawing.Size(124, 64);
            this.buttonStoreDI.TabIndex = 4;
            this.buttonStoreDI.Text = "Store Reading";
            this.buttonStoreDI.UseVisualStyleBackColor = true;
            this.buttonStoreDI.Click += new System.EventHandler(this.buttonStoreDI_Click);
            // 
            // buttonSaveZoneRedingsToFile
            // 
            this.buttonSaveZoneRedingsToFile.Location = new System.Drawing.Point(78, 195);
            this.buttonSaveZoneRedingsToFile.Name = "buttonSaveZoneRedingsToFile";
            this.buttonSaveZoneRedingsToFile.Size = new System.Drawing.Size(155, 39);
            this.buttonSaveZoneRedingsToFile.TabIndex = 8;
            this.buttonSaveZoneRedingsToFile.Text = "Save Readings";
            this.buttonSaveZoneRedingsToFile.UseVisualStyleBackColor = true;
            this.buttonSaveZoneRedingsToFile.Click += new System.EventHandler(this.buttonSaveZoneRedingsToFile_Click);
            // 
            // checkBoxMedianCalc
            // 
            this.checkBoxMedianCalc.AutoSize = true;
            this.checkBoxMedianCalc.Checked = true;
            this.checkBoxMedianCalc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMedianCalc.Location = new System.Drawing.Point(197, 324);
            this.checkBoxMedianCalc.Name = "checkBoxMedianCalc";
            this.checkBoxMedianCalc.Size = new System.Drawing.Size(61, 17);
            this.checkBoxMedianCalc.TabIndex = 14;
            this.checkBoxMedianCalc.Text = "Median";
            this.checkBoxMedianCalc.UseVisualStyleBackColor = true;
            this.checkBoxMedianCalc.CheckedChanged += new System.EventHandler(this.checkBoxMedianCalc_CheckedChanged);
            // 
            // buttonFoucaultOptions
            // 
            this.buttonFoucaultOptions.Location = new System.Drawing.Point(100, 489);
            this.buttonFoucaultOptions.Name = "buttonFoucaultOptions";
            this.buttonFoucaultOptions.Size = new System.Drawing.Size(124, 46);
            this.buttonFoucaultOptions.TabIndex = 18;
            this.buttonFoucaultOptions.Text = "Options";
            this.buttonFoucaultOptions.UseVisualStyleBackColor = true;
            this.buttonFoucaultOptions.Click += new System.EventHandler(this.buttonFoucaultOptions_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(14, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 24);
            this.label5.TabIndex = 15;
            this.label5.Text = "Difference";
            // 
            // textBoxBrightnessDiff
            // 
            this.textBoxBrightnessDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBrightnessDiff.Location = new System.Drawing.Point(18, 378);
            this.textBoxBrightnessDiff.Name = "textBoxBrightnessDiff";
            this.textBoxBrightnessDiff.ReadOnly = true;
            this.textBoxBrightnessDiff.Size = new System.Drawing.Size(73, 31);
            this.textBoxBrightnessDiff.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Brighness Right";
            // 
            // textBoxBrightnessRight
            // 
            this.textBoxBrightnessRight.Location = new System.Drawing.Point(100, 324);
            this.textBoxBrightnessRight.Name = "textBoxBrightnessRight";
            this.textBoxBrightnessRight.ReadOnly = true;
            this.textBoxBrightnessRight.Size = new System.Drawing.Size(73, 20);
            this.textBoxBrightnessRight.TabIndex = 13;
            // 
            // textBoxBrightnessLeft
            // 
            this.textBoxBrightnessLeft.Location = new System.Drawing.Point(18, 324);
            this.textBoxBrightnessLeft.Name = "textBoxBrightnessLeft";
            this.textBoxBrightnessLeft.ReadOnly = true;
            this.textBoxBrightnessLeft.Size = new System.Drawing.Size(73, 20);
            this.textBoxBrightnessLeft.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Brightness Left";
            // 
            // comboBoxZoneVisualization
            // 
            this.comboBoxZoneVisualization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneVisualization.FormattingEnabled = true;
            this.comboBoxZoneVisualization.Location = new System.Drawing.Point(18, 30);
            this.comboBoxZoneVisualization.Name = "comboBoxZoneVisualization";
            this.comboBoxZoneVisualization.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneVisualization.TabIndex = 1;
            this.comboBoxZoneVisualization.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoneVisualization_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Visualization";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Zone #";
            // 
            // comboBoxZoneNum
            // 
            this.comboBoxZoneNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneNum.FormattingEnabled = true;
            this.comboBoxZoneNum.Location = new System.Drawing.Point(18, 73);
            this.comboBoxZoneNum.Name = "comboBoxZoneNum";
            this.comboBoxZoneNum.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneNum.TabIndex = 3;
            this.comboBoxZoneNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoneNum_SelectedIndexChanged);
            // 
            // tabPageLWT
            // 
            this.tabPageLWT.Controls.Add(this.buttonClearDIsLWT);
            this.tabPageLWT.Controls.Add(this.buttonEditZoneReadingsLWT);
            this.tabPageLWT.Controls.Add(this.checkBoxAdvanceBackLWT);
            this.tabPageLWT.Controls.Add(this.checkBoxAdvanceFwdLWT);
            this.tabPageLWT.Controls.Add(this.buttonStoreDI_LWT);
            this.tabPageLWT.Controls.Add(this.buttonSaveZoneRedingsToFileLWT);
            this.tabPageLWT.Controls.Add(this.buttonLWTOptions);
            this.tabPageLWT.Controls.Add(this.comboBoxZoneVizualizationLWT);
            this.tabPageLWT.Controls.Add(this.label13);
            this.tabPageLWT.Controls.Add(this.label14);
            this.tabPageLWT.Controls.Add(this.comboBoxZoneNumLWT);
            this.tabPageLWT.Location = new System.Drawing.Point(4, 22);
            this.tabPageLWT.Name = "tabPageLWT";
            this.tabPageLWT.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLWT.Size = new System.Drawing.Size(306, 766);
            this.tabPageLWT.TabIndex = 2;
            this.tabPageLWT.Text = "Lateral Wire Test";
            this.tabPageLWT.UseVisualStyleBackColor = true;
            // 
            // buttonClearDIsLWT
            // 
            this.buttonClearDIsLWT.Location = new System.Drawing.Point(78, 240);
            this.buttonClearDIsLWT.Name = "buttonClearDIsLWT";
            this.buttonClearDIsLWT.Size = new System.Drawing.Size(155, 39);
            this.buttonClearDIsLWT.TabIndex = 32;
            this.buttonClearDIsLWT.Text = "Clear Readings";
            this.buttonClearDIsLWT.UseVisualStyleBackColor = true;
            // 
            // buttonEditZoneReadingsLWT
            // 
            this.buttonEditZoneReadingsLWT.Location = new System.Drawing.Point(79, 150);
            this.buttonEditZoneReadingsLWT.Name = "buttonEditZoneReadingsLWT";
            this.buttonEditZoneReadingsLWT.Size = new System.Drawing.Size(154, 39);
            this.buttonEditZoneReadingsLWT.TabIndex = 30;
            this.buttonEditZoneReadingsLWT.Text = "Edit Readings";
            this.buttonEditZoneReadingsLWT.UseVisualStyleBackColor = true;
            // 
            // checkBoxAdvanceBackLWT
            // 
            this.checkBoxAdvanceBackLWT.AutoSize = true;
            this.checkBoxAdvanceBackLWT.Location = new System.Drawing.Point(161, 118);
            this.checkBoxAdvanceBackLWT.Name = "checkBoxAdvanceBackLWT";
            this.checkBoxAdvanceBackLWT.Size = new System.Drawing.Size(97, 17);
            this.checkBoxAdvanceBackLWT.TabIndex = 29;
            this.checkBoxAdvanceBackLWT.Text = "Advance Back";
            this.checkBoxAdvanceBackLWT.UseVisualStyleBackColor = true;
            // 
            // checkBoxAdvanceFwdLWT
            // 
            this.checkBoxAdvanceFwdLWT.AutoSize = true;
            this.checkBoxAdvanceFwdLWT.Checked = true;
            this.checkBoxAdvanceFwdLWT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAdvanceFwdLWT.Location = new System.Drawing.Point(18, 118);
            this.checkBoxAdvanceFwdLWT.Name = "checkBoxAdvanceFwdLWT";
            this.checkBoxAdvanceFwdLWT.Size = new System.Drawing.Size(110, 17);
            this.checkBoxAdvanceFwdLWT.TabIndex = 28;
            this.checkBoxAdvanceFwdLWT.Text = "Advance Forward";
            this.checkBoxAdvanceFwdLWT.UseVisualStyleBackColor = true;
            // 
            // buttonStoreDI_LWT
            // 
            this.buttonStoreDI_LWT.Location = new System.Drawing.Point(161, 30);
            this.buttonStoreDI_LWT.Name = "buttonStoreDI_LWT";
            this.buttonStoreDI_LWT.Size = new System.Drawing.Size(124, 64);
            this.buttonStoreDI_LWT.TabIndex = 27;
            this.buttonStoreDI_LWT.Text = "Store Reading";
            this.buttonStoreDI_LWT.UseVisualStyleBackColor = true;
            // 
            // buttonSaveZoneRedingsToFileLWT
            // 
            this.buttonSaveZoneRedingsToFileLWT.Location = new System.Drawing.Point(78, 195);
            this.buttonSaveZoneRedingsToFileLWT.Name = "buttonSaveZoneRedingsToFileLWT";
            this.buttonSaveZoneRedingsToFileLWT.Size = new System.Drawing.Size(155, 39);
            this.buttonSaveZoneRedingsToFileLWT.TabIndex = 31;
            this.buttonSaveZoneRedingsToFileLWT.Text = "Save Readings";
            this.buttonSaveZoneRedingsToFileLWT.UseVisualStyleBackColor = true;
            // 
            // buttonLWTOptions
            // 
            this.buttonLWTOptions.Location = new System.Drawing.Point(79, 327);
            this.buttonLWTOptions.Name = "buttonLWTOptions";
            this.buttonLWTOptions.Size = new System.Drawing.Size(154, 46);
            this.buttonLWTOptions.TabIndex = 41;
            this.buttonLWTOptions.Text = "Options";
            this.buttonLWTOptions.UseVisualStyleBackColor = true;
            // 
            // comboBoxZoneVizualizationLWT
            // 
            this.comboBoxZoneVizualizationLWT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneVizualizationLWT.FormattingEnabled = true;
            this.comboBoxZoneVizualizationLWT.Location = new System.Drawing.Point(18, 30);
            this.comboBoxZoneVizualizationLWT.Name = "comboBoxZoneVizualizationLWT";
            this.comboBoxZoneVizualizationLWT.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneVizualizationLWT.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Visualization";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 57);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "Zone #";
            // 
            // comboBoxZoneNumLWT
            // 
            this.comboBoxZoneNumLWT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoneNumLWT.FormattingEnabled = true;
            this.comboBoxZoneNumLWT.Location = new System.Drawing.Point(18, 73);
            this.comboBoxZoneNumLWT.Name = "comboBoxZoneNumLWT";
            this.comboBoxZoneNumLWT.Size = new System.Drawing.Size(124, 21);
            this.comboBoxZoneNumLWT.TabIndex = 26;
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
            this.panel1.Controls.Add(this.buttonSavePicture);
            this.panel1.Controls.Add(this.checkBoxUseOffset);
            this.panel1.Controls.Add(this.labelOffset);
            this.panel1.Controls.Add(this.buttonSetZero);
            this.panel1.Controls.Add(this.checkBoxHideDI);
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
            // checkBoxUseOffset
            // 
            this.checkBoxUseOffset.AutoSize = true;
            this.checkBoxUseOffset.Checked = true;
            this.checkBoxUseOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseOffset.Location = new System.Drawing.Point(655, 76);
            this.checkBoxUseOffset.Name = "checkBoxUseOffset";
            this.checkBoxUseOffset.Size = new System.Drawing.Size(76, 17);
            this.checkBoxUseOffset.TabIndex = 16;
            this.checkBoxUseOffset.Text = "Use Offset";
            this.checkBoxUseOffset.UseVisualStyleBackColor = true;
            this.checkBoxUseOffset.CheckedChanged += new System.EventHandler(this.checkBoxUseOffset_CheckedChanged);
            // 
            // labelOffset
            // 
            this.labelOffset.Location = new System.Drawing.Point(565, 72);
            this.labelOffset.Name = "labelOffset";
            this.labelOffset.Size = new System.Drawing.Size(84, 26);
            this.labelOffset.TabIndex = 15;
            this.labelOffset.Text = "0";
            this.labelOffset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSetZero
            // 
            this.buttonSetZero.Location = new System.Drawing.Point(487, 72);
            this.buttonSetZero.Name = "buttonSetZero";
            this.buttonSetZero.Size = new System.Drawing.Size(72, 26);
            this.buttonSetZero.TabIndex = 14;
            this.buttonSetZero.Text = "Offset";
            this.buttonSetZero.UseVisualStyleBackColor = true;
            this.buttonSetZero.Click += new System.EventHandler(this.buttonSetZero_Click);
            // 
            // checkBoxHideDI
            // 
            this.checkBoxHideDI.AutoSize = true;
            this.checkBoxHideDI.Location = new System.Drawing.Point(655, 123);
            this.checkBoxHideDI.Name = "checkBoxHideDI";
            this.checkBoxHideDI.Size = new System.Drawing.Size(48, 17);
            this.checkBoxHideDI.TabIndex = 20;
            this.checkBoxHideDI.Text = "Hide";
            this.checkBoxHideDI.UseVisualStyleBackColor = true;
            this.checkBoxHideDI.CheckedChanged += new System.EventHandler(this.checkBoxHideDI_CheckedChanged);
            // 
            // labelDIUnit
            // 
            this.labelDIUnit.Location = new System.Drawing.Point(565, 122);
            this.labelDIUnit.Name = "labelDIUnit";
            this.labelDIUnit.Size = new System.Drawing.Size(45, 21);
            this.labelDIUnit.TabIndex = 19;
            this.labelDIUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(484, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Reading:";
            // 
            // textBoxDIValue
            // 
            this.textBoxDIValue.Location = new System.Drawing.Point(487, 122);
            this.textBoxDIValue.Name = "textBoxDIValue";
            this.textBoxDIValue.ReadOnly = true;
            this.textBoxDIValue.Size = new System.Drawing.Size(72, 20);
            this.textBoxDIValue.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(484, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Dial Indicator Status:";
            // 
            // textBoxDIStatus
            // 
            this.textBoxDIStatus.Location = new System.Drawing.Point(487, 31);
            this.textBoxDIStatus.Name = "textBoxDIStatus";
            this.textBoxDIStatus.ReadOnly = true;
            this.textBoxDIStatus.Size = new System.Drawing.Size(147, 20);
            this.textBoxDIStatus.TabIndex = 12;
            this.textBoxDIStatus.Text = "Disconnected";
            // 
            // buttonConnectDI
            // 
            this.buttonConnectDI.Location = new System.Drawing.Point(655, 15);
            this.buttonConnectDI.Name = "buttonConnectDI";
            this.buttonConnectDI.Size = new System.Drawing.Size(112, 37);
            this.buttonConnectDI.TabIndex = 13;
            this.buttonConnectDI.Text = "Connect";
            this.buttonConnectDI.UseVisualStyleBackColor = true;
            this.buttonConnectDI.Click += new System.EventHandler(this.buttonConnectDI_Click);
            // 
            // buttonAutoPosition
            // 
            this.buttonAutoPosition.Location = new System.Drawing.Point(315, 72);
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
            this.checkBoxFitToScreen.Location = new System.Drawing.Point(209, 72);
            this.checkBoxFitToScreen.Name = "checkBoxFitToScreen";
            this.checkBoxFitToScreen.Size = new System.Drawing.Size(86, 17);
            this.checkBoxFitToScreen.TabIndex = 7;
            this.checkBoxFitToScreen.Text = "Fit to Screen";
            this.checkBoxFitToScreen.UseVisualStyleBackColor = true;
            this.checkBoxFitToScreen.CheckedChanged += new System.EventHandler(this.checkBoxFitToScreen_CheckedChanged);
            // 
            // buttonCopyPicture
            // 
            this.buttonCopyPicture.Location = new System.Drawing.Point(315, 110);
            this.buttonCopyPicture.Name = "buttonCopyPicture";
            this.buttonCopyPicture.Size = new System.Drawing.Size(58, 32);
            this.buttonCopyPicture.TabIndex = 9;
            this.buttonCopyPicture.Text = "Copy";
            this.buttonCopyPicture.UseVisualStyleBackColor = true;
            this.buttonCopyPicture.Click += new System.EventHandler(this.buttonCopyPicture_Click);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(206, 14);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(34, 13);
            this.labelScale.TabIndex = 5;
            this.labelScale.Text = "Scale";
            // 
            // hScrollBarScale
            // 
            this.hScrollBarScale.LargeChange = 1;
            this.hScrollBarScale.Location = new System.Drawing.Point(209, 31);
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
            this.buttonCameraSettings.Size = new System.Drawing.Size(149, 32);
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
            this.comboBoxResolution.Size = new System.Drawing.Size(149, 21);
            this.comboBoxResolution.TabIndex = 3;
            this.comboBoxResolution.SelectedIndexChanged += new System.EventHandler(this.comboBoxResolution_SelectedIndexChanged);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(21, 31);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(149, 21);
            this.comboBoxCamera.TabIndex = 1;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            this.comboBoxCamera.DropDown += new System.EventHandler(this.comboBoxCamera_DropDown);
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
            this.timerPoll.Tick += new System.EventHandler(this.timerPoll_Tick);
            // 
            // buttonSavePicture
            // 
            this.buttonSavePicture.Location = new System.Drawing.Point(389, 110);
            this.buttonSavePicture.Name = "buttonSavePicture";
            this.buttonSavePicture.Size = new System.Drawing.Size(58, 32);
            this.buttonSavePicture.TabIndex = 10;
            this.buttonSavePicture.Text = "Save";
            this.buttonSavePicture.UseVisualStyleBackColor = true;
            this.buttonSavePicture.Click += new System.EventHandler(this.buttonSavePicture_Click);
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
            this.tabPageLWT.ResumeLayout(false);
            this.tabPageLWT.PerformLayout();
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
        private System.Windows.Forms.Button buttonLoadMirrorData;
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
        private System.Windows.Forms.Button buttonAutoPosition;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDIValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDIStatus;
        private System.Windows.Forms.Button buttonConnectDI;
        private System.Windows.Forms.Label labelDIUnit;
        private System.Windows.Forms.Timer timerPoll;
        private System.Windows.Forms.Button buttonSaveZoneRedingsToFile;
        private System.Windows.Forms.CheckBox checkBoxHideDI;
        private System.Windows.Forms.CheckBox checkBoxAdvanceBack;
        private System.Windows.Forms.CheckBox checkBoxAdvanceFwd;
        private System.Windows.Forms.Button buttonStoreDI;
        private System.Windows.Forms.Button buttonEditZoneReadings;
        private System.Windows.Forms.Button buttonClearDIs;
        private System.Windows.Forms.Button buttonSetZero;
        private System.Windows.Forms.Label labelOffset;
        private System.Windows.Forms.CheckBox checkBoxUseOffset;
        private System.Windows.Forms.Button buttonAutoMeasurements;
        private System.Windows.Forms.CheckBox checkBoxDbgEmulateDI;
        private System.Windows.Forms.TextBox textBoxDbgBrightness;
        private System.Windows.Forms.TextBox textBoxDbgDI;
        private System.Windows.Forms.CheckBox checkBoxDbgEmulateBrightness;
        private System.Windows.Forms.Button buttonSavePicture;
        private System.Windows.Forms.TabPage tabPageLWT;
        private System.Windows.Forms.Button buttonClearDIsLWT;
        private System.Windows.Forms.Button buttonEditZoneReadingsLWT;
        private System.Windows.Forms.CheckBox checkBoxAdvanceBackLWT;
        private System.Windows.Forms.CheckBox checkBoxAdvanceFwdLWT;
        private System.Windows.Forms.Button buttonStoreDI_LWT;
        private System.Windows.Forms.Button buttonSaveZoneRedingsToFileLWT;
        private System.Windows.Forms.Button buttonLWTOptions;
        private System.Windows.Forms.ComboBox comboBoxZoneVizualizationLWT;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxZoneNumLWT;
    }
}

