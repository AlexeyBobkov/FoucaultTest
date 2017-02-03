using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FoucaultTestClasses;

namespace FoucaultTest
{
    public partial class CalcOptionsForm : Form
    {
        public CalcOptionsForm(Options options)
        {
            options_ = options;
            InitializeComponent();
        }

        public Options Options
        {
            get { return options_; }
        }

        private bool init_ = false;
        private Options options_;

        private void CalcOptionsForm_Load(object sender, EventArgs e)
        {
            textBoxAngle.Text = (options_.ZoneAngle*2).ToString();
            textBoxTimeAveragingCount.Text = options_.TimeAveragingCnt.ToString();
            textBoxAutoPrecision.Text = options_.AutoPrecision.ToString();
            textBoxStabilizationTime.Text = options_.AutoStabilizationTime.ToString();
            init_ = true;
        }

        private void textBoxAngle_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.ZoneAngle = Convert.ToInt32(textBoxAngle.Text)/2;
                if (options_.ZoneAngle < 1)
                    options_.ZoneAngle = 1;
                else if (options_.ZoneAngle > 90)
                    options_.ZoneAngle = 90;
            }
            catch (System.FormatException)
            {
                options_.ZoneAngle = 20;
            }
        }

        private void textBoxTimeAveragingCount_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.TimeAveragingCnt = Convert.ToInt32(textBoxTimeAveragingCount.Text);
                if (options_.TimeAveragingCnt < 1)
                    options_.TimeAveragingCnt = 1;
                else if (options_.TimeAveragingCnt > 500)
                    options_.TimeAveragingCnt = 500;
            }
            catch (System.FormatException)
            {
                options_.TimeAveragingCnt = 30;
            }
        }

        private void textBoxAutoPrecision_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.AutoPrecision = Convert.ToDouble(textBoxAutoPrecision.Text);
                if (options_.AutoPrecision <= 0)
                    options_.AutoPrecision = 1;
            }
            catch (System.FormatException)
            {
                options_.AutoPrecision = 1;
            }
        }

        private void textBoxStabilizationTime_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.AutoStabilizationTime = Convert.ToDouble(textBoxStabilizationTime.Text);
                if (options_.AutoStabilizationTime <= 0)
                    options_.AutoStabilizationTime = 3.0;
            }
            catch (System.FormatException)
            {
                options_.AutoStabilizationTime = 3.0;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            // we only reset the options changed in this dialog box
            options_.ZoneAngle = 30;
            options_.TimeAveragingCnt = 60;
            options_.AutoPrecision = 1.0;
            options_.AutoStabilizationTime = 3.0;

            init_ = false;
            textBoxAngle.Text = (options_.ZoneAngle * 2).ToString();
            textBoxTimeAveragingCount.Text = options_.TimeAveragingCnt.ToString();
            textBoxAutoPrecision.Text = options_.AutoPrecision.ToString();
            textBoxStabilizationTime.Text = options_.AutoStabilizationTime.ToString();
            init_ = true;
        }
    }
}
