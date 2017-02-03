namespace FoucaultTest
{
    partial class CalcOptionsForm
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
            this.textBoxAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTimeAveragingCount = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxAutoPrecision = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxStabilizationTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxAngle
            // 
            this.textBoxAngle.Location = new System.Drawing.Point(28, 39);
            this.textBoxAngle.Name = "textBoxAngle";
            this.textBoxAngle.Size = new System.Drawing.Size(97, 20);
            this.textBoxAngle.TabIndex = 1;
            this.textBoxAngle.TextChanged += new System.EventHandler(this.textBoxAngle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zone Angle (0-180 deg)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Averaging Count";
            // 
            // textBoxTimeAveragingCount
            // 
            this.textBoxTimeAveragingCount.Location = new System.Drawing.Point(145, 39);
            this.textBoxTimeAveragingCount.Name = "textBoxTimeAveragingCount";
            this.textBoxTimeAveragingCount.Size = new System.Drawing.Size(97, 20);
            this.textBoxTimeAveragingCount.TabIndex = 3;
            this.textBoxTimeAveragingCount.TextChanged += new System.EventHandler(this.textBoxTimeAveragingCount_TextChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(145, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 43);
            this.button1.TabIndex = 9;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(261, 152);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 43);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(28, 152);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(97, 43);
            this.buttonDefault.TabIndex = 8;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Auto Mode Precision";
            // 
            // textBoxAutoPrecision
            // 
            this.textBoxAutoPrecision.Location = new System.Drawing.Point(28, 91);
            this.textBoxAutoPrecision.Name = "textBoxAutoPrecision";
            this.textBoxAutoPrecision.Size = new System.Drawing.Size(97, 20);
            this.textBoxAutoPrecision.TabIndex = 5;
            this.textBoxAutoPrecision.TextChanged += new System.EventHandler(this.textBoxAutoPrecision_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Stabilization Time (sec)";
            // 
            // textBoxStabilizationTime
            // 
            this.textBoxStabilizationTime.Location = new System.Drawing.Point(145, 91);
            this.textBoxStabilizationTime.Name = "textBoxStabilizationTime";
            this.textBoxStabilizationTime.Size = new System.Drawing.Size(97, 20);
            this.textBoxStabilizationTime.TabIndex = 7;
            this.textBoxStabilizationTime.TextChanged += new System.EventHandler(this.textBoxStabilizationTime_TextChanged);
            // 
            // CalcOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 222);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxStabilizationTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxAutoPrecision);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTimeAveragingCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAngle);
            this.Name = "CalcOptionsForm";
            this.Text = "Calculation Options";
            this.Load += new System.EventHandler(this.CalcOptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTimeAveragingCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAutoPrecision;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxStabilizationTime;
    }
}