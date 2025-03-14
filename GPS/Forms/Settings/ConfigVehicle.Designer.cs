﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        #region Vehicle Save---------------------------------------------
        private void btnVehicleSave_Click(object sender, EventArgs e)
        {
            if (tboxVehicleNameSave.Text.Trim().Length > 0)
            {
                SettingsIO.ExportAll(mf.vehiclesDirectory + tboxVehicleNameSave.Text.Trim() + ".XML");

                mf.vehicleFileName = tboxVehicleNameSave.Text.Trim();
                Properties.Settings.Default.setVehicle_vehicleName = mf.vehicleFileName;
                Properties.Settings.Default.Save();

                tboxVehicleNameSave.Text = "";
                btnVehicleSave.Enabled = false;

                LoadBrandImage();

                mf.vehicle = new CVehicle(mf);
                mf.tool = new CTool(mf);

                //reset AOG
                mf.LoadSettings();

                SectionFeetInchesTotalWidthLabelUpdate();
            }

            UpdateVehicleListView();
            UpdateSummary();
        }

        private void btnVehicleLoad_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                //save current vehicle
                SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");


                if (lvVehicles.SelectedItems.Count > 0)
                {
                    DialogResult result3 = MessageBox.Show(
                        "Load: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML",
                        gStr.gsSaveAndReturn,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    
                    if (result3 == DialogResult.Yes)
                    {
                        bool success = SettingsIO.ImportAll(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                        if (!success) return;

                        mf.vehicleFileName = lvVehicles.SelectedItems[0].SubItems[0].Text;
                        Properties.Settings.Default.setVehicle_vehicleName = mf.vehicleFileName;
                        Properties.Settings.Default.Save();

                        LoadBrandImage();

                        mf.vehicle = new CVehicle(mf);
                        mf.tool = new CTool(mf);

                        //reset AOG
                        mf.LoadSettings();

                        SectionFeetInchesTotalWidthLabelUpdate();

                        //Form Steer Settings
                        mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
                        mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

                        mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
                        mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

                        mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
                        mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
                        mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
                        mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

                        mf.SendPgnToLoop(mf.p_252.pgn);

                        //machine module settings
                        mf.p_238.pgn[mf.p_238.set0] = Properties.Settings.Default.setArdMac_setting0;
                        mf.p_238.pgn[mf.p_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                        mf.p_238.pgn[mf.p_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

                        mf.SendPgnToLoop(mf.p_238.pgn);

                        //steer config
                        mf.p_251.pgn[mf.p_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
                        mf.p_251.pgn[mf.p_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
                        mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
                        mf.p_251.pgn[mf.p_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10));

                        if (Properties.Settings.Default.setAS_isConstantContourOn)
                            mf.p_251.pgn[mf.p_251.angVel] = 1;
                        else mf.p_251.pgn[mf.p_251.angVel] = 0;

                        mf.SendPgnToLoop(mf.p_251.pgn);

                        //machine settings    
                        mf.p_238.pgn[mf.p_238.set0] = Properties.Settings.Default.setArdMac_setting0;
                        mf.p_238.pgn[mf.p_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                        mf.p_238.pgn[mf.p_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

                        //mf.p_238.pgn[mf.p_238.user1] = Properties.Settings.Default.setArdMac_user1;
                        //mf.p_238.pgn[mf.p_238.user2] = Properties.Settings.Default.setArdMac_user2;
                        //mf.p_238.pgn[mf.p_238.user3] = Properties.Settings.Default.setArdMac_user3;
                        //mf.p_238.pgn[mf.p_238.user4] = Properties.Settings.Default.setArdMac_user4;

                        mf.SendPgnToLoop(mf.p_238.pgn);

                        //Send Pin configuration
                        SendRelaySettingsToMachineModule();

                        ///Remind the user
                        mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");
                    }

                    UpdateVehicleListView();
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                UpdateVehicleListView();
            }

            UpdateSummary();
        }

        private void btnVehicleDelete_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (lvVehicles.SelectedItems.Count > 0)
                {
                    if (lvVehicles.SelectedItems[0].SubItems[0].Text != mf.vehicleFileName)
                    {
                        DialogResult result3 = MessageBox.Show(
                        "Delete: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML",
                        gStr.gsSaveAndReturn,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button2);
                        if (result3 == DialogResult.Yes)
                        {
                            File.Delete(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                        }
                    }
                    else
                    {
                        var form = new FormTimedMessage(2000, "Vehicle In Use", "Select Different Vehicle");
                        form.Show(this);
                    }
                }
                else
                {
                    var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                    form.Show(this);
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
            }
            UpdateVehicleListView();
        }

        private void tboxVehicleNameSave_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            //btnVehicleSaveAs.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();

            if (String.IsNullOrEmpty(tboxVehicleNameSave.Text.Trim()))
            {
                btnVehicleSave.Enabled = false;
            }
            else
            {
                btnVehicleSave.Enabled = true;
            }
        }
        private void tboxVehicleNameSave_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {

                if (mf.isKeyboardOn)
                {
                    mf.KeyboardToText((TextBox)sender, this);
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                tboxVehicleNameSave.Enabled = false;
            }
        }
        //private void tboxCreateNewVehicle_Click(object sender, EventArgs e)
        //{
        //    if (!mf.isJobStarted)
        //    {

        //        if (mf.isKeyboardOn)
        //        {
        //            mf.KeyboardToText((TextBox)sender, this);
        //        }
        //    }
        //    else
        //    {
        //        var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
        //        form.Show(this);
        //        tboxCreateNewVehicle.Enabled = false;
        //    }
        //}

        //private void tboxCreateNewVehicle_TextChanged(object sender, EventArgs e)
        //{
        //    var textboxSender = (TextBox)sender;
        //    var cursorPosition = textboxSender.SelectionStart;
        //    textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
        //    textboxSender.SelectionStart = cursorPosition;

        //    btnVehicleSaveAs.Enabled = false;
        //    btnVehicleLoad.Enabled = false;
        //    btnVehicleDelete.Enabled = false;

        //    lvVehicles.SelectedItems.Clear();

        //    if (String.IsNullOrEmpty(tboxCreateNewVehicle.Text.Trim()))
        //    {
        //        btnVehicleNewSave.Enabled = false;
        //    }
        //    else
        //    {
        //        btnVehicleNewSave.Enabled = true;
        //    }
        //}
        //private void btnVehicleNewSave_Click(object sender, EventArgs e)
        //{
        //    if (tboxCreateNewVehicle.Text.Trim().Length > 0)
        //    {
        //        //SettingsIO.ExportAll(mf.vehiclesDirectory + tboxCreateNewVehicle.Text.Trim() + ".XML");

        //        Settings.Default.Reset();
        //        Settings.Default.Save();

        //        Properties.Settings.Default.setVehicle_vehicleName = tboxCreateNewVehicle.Text.Trim();
        //        Properties.Settings.Default.setDisplay_isTermsAccepted = true;

        //        Properties.Settings.Default.Save();
        //        tboxCreateNewVehicle.Text = "";
        //        btnVehicleNewSave.Enabled = false;

        //        lblCurrentVehicle.Text = mf.vehicleFileName = Properties.Settings.Default.setVehicle_vehicleName;

        //        SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");
        //        LoadBrandImage();

        //        mf.vehicle = new CVehicle(mf);
        //        mf.tool = new CTool(mf);

        //        //reset AOG
        //        mf.LoadSettings();

        //        chkDisplaySky.Checked = mf.isSkyOn;
        //        chkDisplayBrightness.Checked = mf.isBrightnessOn;
        //        chkDisplayFloor.Checked = mf.isTextureOn;
        //        chkDisplayGrid.Checked = mf.isGridOn;
        //        chkDisplaySpeedo.Checked = mf.isSpeedoOn;
        //        chkDisplayDayNight.Checked = mf.isAutoDayNight;
        //        chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
        //        chkSvennArrow.Checked = mf.isSvennArrowOn;
        //        chkDisplayLogNMEA.Checked = mf.isLogNMEA;
        //        chkDisplayPolygons.Checked = mf.isDrawPolygons;
        //        chkDisplayLightbar.Checked = mf.isLightbarOn;
        //        chkDisplayKeyboard.Checked = mf.isKeyboardOn;
        //        chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;

        //        if (mf.isMetric) rbtnDisplayMetric.Checked = true;
        //        else rbtnDisplayImperial.Checked = true;

        //        SaveDisplaySettings();

        //        lblCurrentVehicle.Text = Properties.Settings.Default.setVehicle_vehicleName;

        //        if (mf.isMetric)
        //        {
        //            lblInchesCm.Text = gStr.gsCentimeters;
        //            lblFeetMeters.Text = gStr.gsMeters;
        //            lblSecTotalWidthFeet.Visible = false;
        //            lblSecTotalWidthInches.Visible = false;
        //            lblSecTotalWidthMeters.Visible = true;
        //        }
        //        else
        //        {
        //            lblInchesCm.Text = gStr.gsInches;
        //            lblFeetMeters.Text = "Feet";
        //            lblSecTotalWidthFeet.Visible = true;
        //            lblSecTotalWidthInches.Visible = true;
        //            lblSecTotalWidthMeters.Visible = false;
        //        }

        //        if (mf.isMetric)
        //        {
        //            lblSecTotalWidthMeters.Text = (mf.tool.width * 100).ToString() + " cm";
        //        }
        //        else
        //        {
        //            double toFeet = mf.tool.width * 3.2808;
        //            lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
        //            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
        //            lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
        //        }


        //        //Form Steer Settings
        //        mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
        //        mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

        //        mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
        //        mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

        //        mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
        //        mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
        //        mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
        //        mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

        //        mf.SendPgnToLoop(mf.p_252.pgn);

        //        //machine module settings
        //        mf.p_238.pgn[mf.p_238.set0] = Properties.Settings.Default.setArdMac_setting0;
        //        mf.p_238.pgn[mf.p_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
        //        mf.p_238.pgn[mf.p_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

        //        mf.SendPgnToLoop(mf.p_238.pgn);

        //        //steer config
        //        mf.p_251.pgn[mf.p_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
        //        mf.p_251.pgn[mf.p_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
        //        mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
        //        mf.p_251.pgn[mf.p_251.minSpeed] = 5; //0.5 kmh

        //        if (Properties.Settings.Default.setAS_isConstantContourOn)
        //            mf.p_251.pgn[mf.p_251.angVel] = 1;
        //        else mf.p_251.pgn[mf.p_251.angVel] = 0;

        //        mf.SendPgnToLoop(mf.p_251.pgn);

        //        //Send Pin configuration
        //        SendRelaySettingsToMachineModule();

        //        ///Remind the user
        //        mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");

        //        UpdateVehicleListView();
        //    }
        //}

        private void btnVehicleSaveAs_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (lvVehicles.SelectedItems.Count > 0)
                {
                    DialogResult result3 = MessageBox.Show(
                        "Overwrite: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML",
                        gStr.gsSaveAndReturn,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        SettingsIO.ExportAll(mf.vehiclesDirectory + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML");
                    }
                    UpdateVehicleListView();
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                UpdateVehicleListView();
            }
        }

        private void UpdateVehicleListView()
        {
            DirectoryInfo dinfo = new DirectoryInfo(mf.vehiclesDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.XML");

            //load the listbox
            lvVehicles.Items.Clear();
            foreach (FileInfo file in Files)
            {
                lvVehicles.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            //deselect everything
            lvVehicles.SelectedItems.Clear();
            lblSummaryVehicleName.Text = Properties.Settings.Default.setVehicle_vehicleName;

            //tboxCreateNewVehicle.Text = "";
            //tboxVehicleNameSave.Text = "";
        }

        private void tboxVehicleNameSave_Enter(object sender, EventArgs e)
        {
            //btnVehicleSaveAs.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();
        }

        private void SaveDisplaySettings()
        {
            mf.isTextureOn = chkDisplayFloor.Checked;
            mf.isGridOn = chkDisplayGrid.Checked;
            mf.isSpeedoOn = chkDisplaySpeedo.Checked;
            mf.isSideGuideLines = chkDisplayExtraGuides.Checked;

            mf.isLogNMEA = chkDisplayLogNMEA.Checked;
            mf.isDrawPolygons = chkDisplayPolygons.Checked;
            mf.isLightbarOn = chkDisplayLightbar.Checked;
            mf.isKeyboardOn = chkDisplayKeyboard.Checked;

            mf.isBrightnessOn = chkDisplayBrightness.Checked;
            mf.isSvennArrowOn = chkSvennArrow.Checked;
            mf.isLogElevation = chkDisplayLogElevation.Checked;

            //mf.timeToShowMenus = (int)nudMenusOnTime.Value;

            Properties.Settings.Default.setDisplay_isBrightnessOn = mf.isBrightnessOn;
            Properties.Settings.Default.setDisplay_isTextureOn = mf.isTextureOn;
            Properties.Settings.Default.setMenu_isGridOn = mf.isGridOn;
            Properties.Settings.Default.setMenu_isCompassOn = mf.isCompassOn;

            Properties.Settings.Default.setDisplay_isSvennArrowOn = mf.isSvennArrowOn;
            Properties.Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn;
            Properties.Settings.Default.setDisplay_isStartFullScreen = chkDisplayStartFullScreen.Checked;
            Properties.Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines;

            Properties.Settings.Default.setMenu_isPureOn = mf.isPureDisplayOn;
            Properties.Settings.Default.setMenu_isLightbarOn = mf.isLightbarOn;
            Properties.Settings.Default.setDisplay_isKeyboardOn = mf.isKeyboardOn;
            Properties.Settings.Default.setDisplay_isLogElevation = mf.isLogElevation;

            if (rbtnDisplayMetric.Checked) { Properties.Settings.Default.setMenu_isMetric = true; mf.isMetric = true; }
            else { Properties.Settings.Default.setMenu_isMetric = false; mf.isMetric = false; }

            Properties.Settings.Default.Save();
        }

        #endregion

        #region Antenna Enter/Leave
        private void tabVAntenna_Enter(object sender, EventArgs e)
        {
            nudAntennaHeight.Value = (int)(Properties.Settings.Default.setVehicle_antennaHeight * mf.m2InchOrCm);

            nudAntennaPivot.Value = (int)((Properties.Settings.Default.setVehicle_antennaPivot) * mf.m2InchOrCm);

            //negative is to the right
            nudAntennaOffset.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_antennaOffset) * mf.m2InchOrCm);

            rbtnAntennaLeft.Checked = false;
            rbtnAntennaRight.Checked = false;
            rbtnAntennaCenter.Checked = false;
            rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
            rbtnAntennaCenter.Checked = Properties.Settings.Default.setVehicle_antennaOffset == 0;

            if (Properties.Settings.Default.setVehicle_vehicleType == 0)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaTractor;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 1)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaHarvester;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 2)
                pboxAntenna.BackgroundImage = Properties.Resources.Antenna4WD;

            label98.Text = mf.unitsInCm;
            label99.Text = mf.unitsInCm;
            label100.Text = mf.unitsInCm;
        }

        private void tabVAntenna_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void rbtnAntennaLeft_Click(object sender, EventArgs e)
        {
            if (rbtnAntennaRight.Checked)
                mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
            else if (rbtnAntennaLeft.Checked)
                mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
            else
            {
                mf.vehicle.antennaOffset = 0;
                nudAntennaOffset.Value = 0;
            }

            Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset;
        }

        private void nudAntennaOffset_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if ((double)nudAntennaOffset.Value == 0)
                {
                    rbtnAntennaLeft.Checked = false;
                    rbtnAntennaRight.Checked = false;
                    rbtnAntennaCenter.Checked = true;
                    mf.vehicle.antennaOffset = 0;
                }
                else
                {
                    if (!rbtnAntennaLeft.Checked && !rbtnAntennaRight.Checked)
                        rbtnAntennaRight.Checked = true;

                    if (rbtnAntennaRight.Checked)
                        mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
                    else
                        mf.vehicle.antennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
                }

                Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset;
            }

            //rbtnAntennaLeft.Checked = false;
            //rbtnAntennaRight.Checked = false;
            //rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            //rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
        }

        private void nudAntennaPivot_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_antennaPivot = (double)nudAntennaPivot.Value * mf.inchOrCm2m;
                mf.vehicle.antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
            }
        }


        private void nudAntennaHeight_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_antennaHeight = (double)nudAntennaHeight.Value * mf.inchOrCm2m;
                mf.vehicle.antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
            }
        }

        #endregion

        #region Vehicle Dimensions

        private void tabVDimensions_Enter(object sender, EventArgs e)
        {
            nudWheelbase.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_wheelbase) * mf.m2InchOrCm);

            nudVehicleTrack.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_trackWidth) * mf.m2InchOrCm);

            nudTractorHitchLength.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_hitchLength) * mf.m2InchOrCm);

            if (mf.vehicle.vehicleType == 0)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBase;
                nudTractorHitchLength.Visible = true;
            }
            else if (mf.vehicle.vehicleType == 1)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBaseHarvester;
                nudTractorHitchLength.Visible = false;
            }
            else if (mf.vehicle.vehicleType == 2)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBase4WD;
                nudTractorHitchLength.Visible = true;
            }

            if (Properties.Settings.Default.setTool_isToolTrailing || Properties.Settings.Default.setTool_isToolTBT)
            {
                nudTractorHitchLength.Visible = true;
                label94.Visible = true;
                label27.Visible = true;
            }
            else
            {
                nudTractorHitchLength.Visible = true;
                label94.Visible = false;
                label27.Visible = false;
            }

            label94.Text = mf.unitsInCm;
            label95.Text = mf.unitsInCm;
            label97.Text = mf.unitsInCm;
        }

        private void nudTractorHitchLength_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.hitchLength = (double)nudTractorHitchLength.Value * mf.inchOrCm2m;
                if (!Properties.Settings.Default.setTool_isToolFront)
                {
                    mf.tool.hitchLength *= -1;
                }
                Properties.Settings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
            }
        }



        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
                mf.vehicle.wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
                Properties.Settings.Default.Save();
            }
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_trackWidth = (double)nudVehicleTrack.Value * mf.inchOrCm2m;
                mf.vehicle.trackWidth = Properties.Settings.Default.setVehicle_trackWidth;
                mf.tram.halfWheelTrack = mf.vehicle.trackWidth * 0.5;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Vehicle Guidance

        private void tabVGuidance_Enter(object sender, EventArgs e)
        {
        }

        private void tabVGuidance_Leave(object sender, EventArgs e)
        {
        }
        #endregion

        #region VConfig Enter/Leave

        private void tabVConfig_Enter(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType == 0) rbtnTractor.Checked = true;
            else if (mf.vehicle.vehicleType == 1) rbtnHarvester.Checked = true;
            else if (mf.vehicle.vehicleType == 2) rbtn4WD.Checked = true;

            original = null;
            TabImageSetup();
        }

        private void tabVConfig_Leave(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.vehicleType = 0;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.vehicleType = 1;
                Properties.Settings.Default.setVehicle_vehicleType = 1;

                if ( mf.tool.hitchLength < 0) mf.tool.hitchLength *= -1;

                Properties.Settings.Default.setTool_isToolFront = true;
                Properties.Settings.Default.setTool_isToolTBT = false;
                Properties.Settings.Default.setTool_isToolTrailing = false;
                Properties.Settings.Default.setTool_isToolRearFixed = false;
            }
            if (rbtn4WD.Checked)
            {
                mf.vehicle.vehicleType = 2;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            if (mf.vehicle.vehicleType == 0) //2WD tractor
            {
                Properties.Settings.Default.setVehicle_isPivotBehindAntenna = true;
                Properties.Settings.Default.setVehicle_isSteerAxleAhead = true;
            }
            if (mf.vehicle.vehicleType == 1) //harvestor
            {
                Properties.Settings.Default.setVehicle_isPivotBehindAntenna = true;
                Properties.Settings.Default.setVehicle_isSteerAxleAhead = false;
            }
            if (mf.vehicle.vehicleType == 2) //4WD
            {
                Properties.Settings.Default.setVehicle_isPivotBehindAntenna = false;
                Properties.Settings.Default.setVehicle_isSteerAxleAhead = true;
            }

            mf.vehicle.isPivotBehindAntenna = Properties.Settings.Default.setVehicle_isPivotBehindAntenna;
            mf.vehicle.isSteerAxleAhead = Properties.Settings.Default.setVehicle_isSteerAxleAhead;

            //the old brand code
            if (cboxIsImage.Checked)
                Properties.Settings.Default.setDisplay_isVehicleImage = false;
            else
                Properties.Settings.Default.setDisplay_isVehicleImage = true;

            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);

            Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;

            if (rbtnTractor.Checked == true)
            {
                Settings.Default.setBrand_TBrand = brand;

                Bitmap bitmap = mf.GetTractorBrand(brand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnHarvester.Checked == true)

            {
                Settings.Default.setBrand_HBrand = brandH;
                Bitmap bitmap = mf.GetHarvesterBrand(brandH);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtn4WD.Checked == true)
            {
                Settings.Default.setBrand_WDBrand = brand4WD;
                Bitmap bitmap = mf.Get4WDBrandFront(brand4WD);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

                Settings.Default.setBrand_WDBrand = brand4WD;
                bitmap = mf.Get4WDBrandRear(brand4WD);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }

            Properties.Settings.Default.Save();
        }

        //brand variables
        TBrand brand;
        HBrand brandH;
        WDBrand brand4WD;

        //Opacity Bar

        Image original = null;

        private void rbtnVehicleType_Click(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.vehicleType = 0;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.vehicleType = 1;
                Properties.Settings.Default.setVehicle_vehicleType = 1;
            }
            if (rbtn4WD.Checked)
            {
                mf.vehicle.vehicleType = 2;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            original = null;
            TabImageSetup();
        }

        private void SetOpacity()
        {
            if (original == null) original = (Bitmap)pboxAlpha.BackgroundImage.Clone();
            pboxAlpha.BackColor = Color.Transparent;
            pboxAlpha.BackgroundImage = SetAlpha((Bitmap)original, (byte)(mf.vehicleOpacityByte));
        }

        private void btnOpacityUp_Click(object sender, EventArgs e)
        {
            mf.vehicleOpacity = Math.Min(mf.vehicleOpacity + 0.2, 1);
            lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
            Properties.Settings.Default.Save();
            SetOpacity();
        }

        private void btnOpacityDn_Click(object sender, EventArgs e)
        {
            mf.vehicleOpacity = Math.Max(mf.vehicleOpacity - 0.2, 0.2);
            lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
            Properties.Settings.Default.Save();
            SetOpacity();
        }

        private void hsbarOpacity_ValueChanged(object sender, EventArgs e)
        {
            //lblOpacityPercent.Text = hsbarOpacity.Value.ToString() + "%";
            //mf.vehicleOpacityByte = (byte)(255 * (hsbarOpacity.Value * 0.01));
            //Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;

            //if (original == null) original = (Bitmap)pboxAlpha.BackgroundImage.Clone();
            //pboxAlpha.BackColor = Color.Transparent;
            //pboxAlpha.BackgroundImage = SetAlpha((Bitmap)original, (byte)(255 * (hsbarOpacity.Value * 0.01)));
        }

        private void cboxIsImage_Click(object sender, EventArgs e)
        {
            //mf.vehicleOpacity = (hsbarOpacity.Value * 0.01);
            mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);

            mf.isVehicleImage = (!cboxIsImage.Checked);
            Properties.Settings.Default.setDisplay_isVehicleImage = mf.isVehicleImage;
            Properties.Settings.Default.Save();
            //original = null;
            TabImageSetup();
        }

        private void TabImageSetup()
        {
            panel4WdBrands.Visible = false;
            panelTractorBrands.Visible = false;
            panelHarvesterBrands.Visible = false;

            if (mf.isVehicleImage)
            {
                if (mf.vehicle.vehicleType == 0)
                {
                    panelTractorBrands.Visible = true;

                    brand = Settings.Default.setBrand_TBrand;

                    if (brand == TBrand.AGOpenGPS)
                        rbtnBrandTAgOpenGPS.Checked = true;
                    else if (brand == TBrand.Case)
                        rbtnBrandTCase.Checked = true;
                    else if (brand == TBrand.Claas)
                        rbtnBrandTClaas.Checked = true;
                    else if (brand == TBrand.Deutz)
                        rbtnBrandTDeutz.Checked = true;
                    else if (brand == TBrand.Fendt)
                        rbtnBrandTFendt.Checked = true;
                    else if (brand == TBrand.JDeere)
                        rbtnBrandTJDeere.Checked = true;
                    else if (brand == TBrand.Kubota)
                        rbtnBrandTKubota.Checked = true;
                    else if (brand == TBrand.Massey)
                        rbtnBrandTMassey.Checked = true;
                    else if (brand == TBrand.NewHolland)
                        rbtnBrandTNH.Checked = true;
                    else if (brand == TBrand.Same)
                        rbtnBrandTSame.Checked = true;
                    else if (brand == TBrand.Steyr)
                        rbtnBrandTSteyr.Checked = true;
                    else if (brand == TBrand.Ursus)
                        rbtnBrandTUrsus.Checked = true;
                    else if (brand == TBrand.Valtra)
                        rbtnBrandTValtra.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);
                }
                else if (mf.vehicle.vehicleType == 1)
                {
                    panelHarvesterBrands.Visible = true;

                    brandH = Settings.Default.setBrand_HBrand;

                    if (brandH == HBrand.AgOpenGPS)
                        rbtnBrandHAgOpenGPS.Checked = true;
                    else if (brandH == HBrand.Case)
                        rbtnBrandHCase.Checked = true;
                    else if (brandH == HBrand.Claas)
                        rbtnBrandHClaas.Checked = true;
                    else if (brandH == HBrand.JDeere)
                        rbtnBrandHJDeere.Checked = true;
                    else if (brandH == HBrand.NewHolland)
                        rbtnBrandHNH.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);
                }
                else if (mf.vehicle.vehicleType == 2)
                {
                    panel4WdBrands.Visible = true;

                    brand4WD = Settings.Default.setBrand_WDBrand;

                    if (brand4WD == WDBrand.AgOpenGPS)
                        rbtnBrand4WDAgOpenGPS.Checked = true;
                    else if (brand4WD == WDBrand.Case)
                        rbtnBrand4WDCase.Checked = true;
                    else if (brand4WD == WDBrand.Challenger)
                        rbtnBrand4WDChallenger.Checked = true;
                    else if (brand4WD == WDBrand.JDeere)
                        rbtnBrand4WDJDeere.Checked = true;
                    else if (brand4WD == WDBrand.NewHolland)
                        rbtnBrand4WDNH.Checked = true;
                    else if (brand4WD == WDBrand.Holder)
                        rbtnBrand4WDHolder.Checked = true;

                    pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(Settings.Default.setBrand_WDBrand);
                }

                mf.vehicleOpacityByte = (byte)(255 * (mf.vehicleOpacity));
                Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicleOpacity * 100);
                lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
                mf.vehicleColor = Color.FromArgb(254, 254, 254);
            }
            else
            {
                pboxAlpha.BackgroundImage = Properties.Resources.TriangleVehicle;
                lblOpacityPercent.Text = ((int)(mf.vehicleOpacity * 100)).ToString() + "%";
                mf.vehicleColor = Color.FromArgb(254, 254, 254);
            }

            cboxIsImage.Checked = !mf.isVehicleImage;

            original = null;
            SetOpacity();
        }

        static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a = alpha / 255f;
            Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems = {
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, a, 0},
                            new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }

        private void LoadBrandImage()
        {
            if (rbtnTractor.Checked == true)
            {
                Bitmap bitmap = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnHarvester.Checked == true)

            {
                Bitmap bitmap = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtn4WD.Checked == true)

            {
                Bitmap bitmap = mf.Get4WDBrandFront(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
                bitmap = mf.Get4WDBrandRear(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
        }

        //Check Brand is changed

        private void rbtnBrandTAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.AGOpenGPS;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.Case;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brand = TBrand.Claas;
                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandTDeutz_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Deutz;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTFendt_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Fendt;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.JDeere;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTKubota_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Kubota;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTMassey_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Massey;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTSame_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Same;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTSteyr_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Steyr;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTUrsus_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Ursus;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandTValtra_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand = TBrand.Valtra;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(brand);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.AgOpenGPS;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.Case;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHClaas_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.Claas;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrandHJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                {
                    brandH = HBrand.JDeere;
                    pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                    original = null;
                    SetOpacity();
                }
            }
        }

        private void rbtnBrandHNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brandH = HBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(brandH);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDAgOpenGPS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.AgOpenGPS;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDCase_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Case;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDChallenger_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Challenger;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDJDeere_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.JDeere;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        private void rbtnBrand4WDNH_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.NewHolland;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }
        private void rbtnBrand4WDHolder_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                brand4WD = WDBrand.Holder;
                pboxAlpha.BackgroundImage = mf.Get4WDBrandFront(brand4WD);
                original = null;
                SetOpacity();
            }
        }

        #endregion
    }
}

