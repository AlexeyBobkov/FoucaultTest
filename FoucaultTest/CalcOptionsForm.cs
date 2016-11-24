using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FoucaultTest
{
    public partial class CalcOptionsForm : Form
    {
        public CalcOptionsForm(FoucaultTestClasses.CalcOptions options)
        {
            options_ = options;
            InitializeComponent();
        }

        public FoucaultTestClasses.CalcOptions Options
        {
            get { return options_; }
        }

        private bool init_ = false;
        private FoucaultTestClasses.CalcOptions options_;

        private void CalcOptionsForm_Load(object sender, EventArgs e)
        {
            textBoxPixelNumber.Text = options_.calcBrightnessPixelNum_.ToString();
            textBoxAveragingCount.Text = options_.timeAveragingCnt_.ToString();
            init_ = true;
        }

        private void textBoxPixelNumber_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.calcBrightnessPixelNum_ = Convert.ToInt32(textBoxPixelNumber.Text);
                if (options_.calcBrightnessPixelNum_ < 1)
                    options_.calcBrightnessPixelNum_ = 1;
                else if (options_.calcBrightnessPixelNum_ > 100000)
                    options_.calcBrightnessPixelNum_ = 100000;
            }
            catch (System.FormatException)
            {
                options_.calcBrightnessPixelNum_ = 30000;
            }
        }

        private void textBoxAveragingCount_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.timeAveragingCnt_ = Convert.ToInt32(textBoxAveragingCount.Text);
                if (options_.timeAveragingCnt_ < 1)
                    options_.timeAveragingCnt_ = 1;
                else if (options_.timeAveragingCnt_ > 500)
                    options_.timeAveragingCnt_ = 500;
            }
            catch (System.FormatException)
            {
                options_.timeAveragingCnt_ = 30000;
            }
        }
    }
}
