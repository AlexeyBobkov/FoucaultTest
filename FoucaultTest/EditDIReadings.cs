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
    public partial class EditDIReadings : Form
    {
        public EditDIReadings(ZoneReading[] zoneReadings, int maxZoneNumber)
        {
            zoneReadings_ = zoneReadings;
            maxZoneNumber_ = maxZoneNumber;
            InitializeComponent();
        }

        public ZoneReading[] ZoneReadings { get { return zoneReadings_; } }

        // implementation
        private ZoneReading[] zoneReadings_;
        private int maxZoneNumber_;

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(ParseEditReadings())
                DialogResult = DialogResult.OK;
        }
        
        private void InitEditReadings()
        {
            if (ZoneReadings == null)
                return;

            string s = "";
            for (int i = 0; i < ZoneReadings.Length; ++i) 
            {
                if(i > 0)
                    s += Environment.NewLine;

                if (ZoneReadings[i].seq_ != null)
                {
                    for (int j = 0; j < ZoneReadings[i].seq_.Count; ++j)
                    {
                        if (j > 0)
                            s += ",";
                        s += ZoneReadings[i].seq_[j].ToString();
                    }
                }
            }

            textBoxReadings.Text = s;
        }

        private bool ParseEditReadings()
        {
            if(textBoxReadings.Text.Length <= 0)
            {
                if (zoneReadings_ == null)
                    return true;

                switch(MessageBox.Show("Clear all? you sure?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.OK:
                        zoneReadings_ = null;
                        return true;
                    case DialogResult.Cancel:
                        InitEditReadings();
                        return false;
                }
            }

            try
            {
                string[] lines = textBoxReadings.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                ZoneReading[] newZoneReadings = new ZoneReading[lines.Length < maxZoneNumber_ ? lines.Length : maxZoneNumber_];
                for (int i = newZoneReadings.Length; --i >= 0; )
                    newZoneReadings[i].seq_ = lines[i].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select<string, double>(double.Parse).ToList();

                zoneReadings_ = newZoneReadings;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void EditDIReadings_Load(object sender, EventArgs e)
        {
            InitEditReadings();
        }
    }
}
