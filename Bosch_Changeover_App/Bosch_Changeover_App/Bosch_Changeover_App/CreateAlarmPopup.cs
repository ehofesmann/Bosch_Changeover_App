﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace Bosch_Changeover_App

{
    public partial class CreateAlarmPopup : Form
    {
        Form1 parentForm;

        bool editing = false;
        string selectedTime;
        string selectedMessage;

        bool alarmSet = false;

        string wavPath = @"C:\Windows\Media\Alarm01.wav";
        SoundPlayer soundPlayer;
        //alarmNotification alarmNotification;

        private string partType;
        private string line;
        private string alarmTime;
        private string station;


        public CreateAlarmPopup(Form1 pf)
        {
            InitializeComponent();
            this.FormClosed += SaveEditing;
            parentForm = pf;
            update_currentTime();
            soundPlayer = new SoundPlayer();
            //alarmNotification = new alarmNotification(this);

            partType = "";
            line = "";
            alarmTime = "";
            station = "";
        }

        public void createAlarmFromLineButton(Card partType)
        {

            partTypeTextBox.Text = partType.getPartType().ToString();
            Debug.WriteLine(partType.getLine().ToString());
            lineComboBox.SelectedIndex = lineComboBox.FindStringExact(partType.getLine().ToString());
            //      stationComboBox.SelectedIndex = stationComboBox.FindstringExact( partType.getStartStation().ToString());
        }


        public void editAlarm(string partType, string lineNum, string station, string alarmTime, bool desktopNotification, bool emailNotification)
        {
            editing = true;
            partTypeTextBox.Text = partType;
            lineComboBox.SelectedIndex = lineComboBox.FindStringExact(lineNum);
            stationComboBox.SelectedIndex = stationComboBox.FindStringExact(station);
            if (!alarmTimeComboBox.Items.Contains(alarmTime))
            {
                alarmTimeComboBox.SelectedItem = "Custom";
            }
            desktopCheckBox.Checked = desktopNotification;
            emailCheckBox.Checked = emailNotification;
        }


        private void CreateAlarmPopup_Load(object sender, EventArgs e)
        {

        }

        public void update_currentTime()
        {

            currentTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
        }

        public void updateData()
        {
            string wavFile = "";
            string wavName = wavFile.Replace(wavPath, string.Empty);
            wavName = wavName.Replace(".wav", string.Empty);
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (editing)
            {
                parentForm.saveButtonClicked();
                editing = false;
            }
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int i;
            int s;
            int l = -1;


            if (!partTypeTextBox.Text.All(char.IsDigit) || partTypeTextBox.Text.Length != 10)
            {
                MessageBox.Show("Part Type must be a 10 digit number.");
            }

            else if (!int.TryParse(stationComboBox.Text, out s))
            {
                MessageBox.Show("Station must be an integer.");
            }
            else if (!int.TryParse(lineComboBox.Text, out l) || !(l == 1 || l == 2 || l == 3))
            {
                MessageBox.Show("Line must be either 1, 2, or 3.");
            }
            else if (!int.TryParse(alarmTimeComboBox.Text, out i))
            {
                MessageBox.Show("Alarm Time must be an integer.");
            }
            else
            {

                int N = i;//Int32.Parse(this.alarmTimeComboBox.SelectedItem.ToString());
                selectedTime = currentTimeLabel + alarmTimeLabel.Text;
                selectedMessage = "Alarm";

                //gotta set the variables that get sent back to the parent form
                partType = partTypeTextBox.Text;
                line = lineComboBox.Text;
                alarmTime = alarmTimeComboBox.Text;
                station = stationComboBox.Text;

                soundPlayer.SoundLocation = wavPath;

                //alarmNotification.Message(selectedMessage);
                alarmSet = true;


                parentForm.saveButtonClicked();
                editing = false;
                this.Close();
            }


        }

        public void Resume()
        {
            soundPlayer.Stop();
        }

        public string getPartType()
        {
            return partType;
        }

        public string getLine()
        {
            return line;
        }

        public string getStation()
        {
            return station;
        }

        public string getAlarmTime()
        {
            return alarmTime;
        }

        public bool getDesktopNotification()
        {
            return desktopCheckBox.Checked;
        }

        public bool getEmailNotification()
        {
            return emailCheckBox.Checked;
        }

        public int getN()
        {
            int N;
            int.TryParse(alarmTimeComboBox.Text, out N);//Int32.Parse(this.alarmTimeComboBox.SelectedItem.ToString());
            return N;
        }

        public Timer getTimer()
        {
            return timer1;
        }

        private void SaveEditing(object sender, FormClosedEventArgs e)
        {

            if (editing)
            {
                parentForm.saveButtonClicked();
                editing = false;
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {

            if (alarmSet)
            {
                if (currentTimeLabel.Text == selectedTime)
                {
                    alarmSet = false;

                    soundPlayer.Play();
                    //alarmNotification.ShowDialog();
                }
            }

        }

        private void alarmTimeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void partTypeTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
