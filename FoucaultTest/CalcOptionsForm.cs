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

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            /*
            options_ = new Options()
            {
                SelectPenColor = Color.Red,
                InactiveZoneColor = Color.Black,
                ActiveZoneColor = Color.Red,
                ZoneHeight = 0.16,
                SideTolerance = 10,
                ZoneAngle = 20,
                TimeAveragingCnt = 30,
            };
             **/
            // we only reset the options changed in this dialog box
            options_.ZoneAngle = 30;
            options_.TimeAveragingCnt = 60;

            init_ = false;
            textBoxAngle.Text = (options_.ZoneAngle * 2).ToString();
            textBoxTimeAveragingCount.Text = options_.TimeAveragingCnt.ToString();
            init_ = true;
        }
    }
}
