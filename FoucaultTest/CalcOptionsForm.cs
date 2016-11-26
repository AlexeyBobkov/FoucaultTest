﻿using System;
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
            textBoxAngle.Text = (options_.zoneAngle_*2).ToString();
            textBoxTimeAveragingCount.Text = options_.timeAveragingCnt_.ToString();
            textBoxCalibAveragingCount.Text = options_.calibAveragingCnt_.ToString();
            init_ = true;
        }

        private void textBoxAngle_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.zoneAngle_ = Convert.ToInt32(textBoxAngle.Text)/2;
                if (options_.zoneAngle_ < 1)
                    options_.zoneAngle_ = 1;
                else if (options_.zoneAngle_ > 90)
                    options_.zoneAngle_ = 90;
            }
            catch (System.FormatException)
            {
                options_.zoneAngle_ = 20;
            }
        }

        private void textBoxTimeAveragingCount_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.timeAveragingCnt_ = Convert.ToInt32(textBoxTimeAveragingCount.Text);
                if (options_.timeAveragingCnt_ < 1)
                    options_.timeAveragingCnt_ = 1;
                else if (options_.timeAveragingCnt_ > 500)
                    options_.timeAveragingCnt_ = 500;
            }
            catch (System.FormatException)
            {
                options_.timeAveragingCnt_ = 30;
            }
        }

        private void textBoxCalibAveragingCount_TextChanged(object sender, EventArgs e)
        {
            if (!init_)
                return;
            try
            {
                options_.calibAveragingCnt_ = Convert.ToInt32(textBoxCalibAveragingCount.Text);
                if (options_.calibAveragingCnt_ < 1)
                    options_.calibAveragingCnt_ = 1;
                else if (options_.calibAveragingCnt_ > 500)
                    options_.calibAveragingCnt_ = 500;
            }
            catch (System.FormatException)
            {
                options_.calibAveragingCnt_ = 60;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            options_ = new Options()
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
            init_ = false;
            textBoxAngle.Text = (options_.zoneAngle_ * 2).ToString();
            textBoxTimeAveragingCount.Text = options_.timeAveragingCnt_.ToString();
            textBoxCalibAveragingCount.Text = options_.calibAveragingCnt_.ToString();
            init_ = true;
        }
    }
}
