//Please, if you use this, share the improvements
// Added plough control


using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{


    public partial class FormConfig : Form
    { 
        public bool IsInvertChecked;
        //class variables
        private readonly FormGPS mf = null;

        private bool isClosing = false;

        //constructor
        public FormConfig(Form callingForm)
        {

              
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            tab1.Appearance = TabAppearance.FlatButtons;
            tab1.ItemSize = new Size(0, 1);
            tab1.SizeMode = TabSizeMode.Fixed;

            HideSubMenu();

            nudTrailingHitchLength.Controls[0].Enabled = false;
            nudDrawbarLength.Controls[0].Enabled = false;
            nudTankHitch.Controls[0].Enabled = false;
            nudTractorHitchLength.Controls[0].Enabled = false;

            nudLookAhead.Controls[0].Enabled = false;
            nudLookAheadOff.Controls[0].Enabled = false;
            nudTurnOffDelay.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;
            nudOverlap.Controls[0].Enabled = false;
            nudCutoffSpeed.Controls[0].Enabled = false;

            nudAntennaHeight.Controls[0].Enabled = false;
            nudAntennaOffset.Controls[0].Enabled = false;
            nudAntennaPivot.Controls[0].Enabled = false;
            nudVehicleTrack.Controls[0].Enabled = false;
            nudWheelbase.Controls[0].Enabled = false;

            nudMinCoverage.Controls[0].Enabled = false;
            nudDefaultSectionWidth.Controls[0].Enabled = false;

            nudSection01.Controls[0].Enabled = false;
            nudSection02.Controls[0].Enabled = false;
            nudSection03.Controls[0].Enabled = false;
            nudSection04.Controls[0].Enabled = false;
            nudSection05.Controls[0].Enabled = false;
            nudSection06.Controls[0].Enabled = false;
            nudSection07.Controls[0].Enabled = false;
            nudSection08.Controls[0].Enabled = false;
            nudSection09.Controls[0].Enabled = false;
            nudSection10.Controls[0].Enabled = false;
            nudSection11.Controls[0].Enabled = false;
            nudSection12.Controls[0].Enabled = false;
            nudSection13.Controls[0].Enabled = false;
            nudSection14.Controls[0].Enabled = false;
            nudSection15.Controls[0].Enabled = false;
            nudSection16.Controls[0].Enabled = false;
            nudNumberOfSections.Controls[0].Enabled = false;

            nudZone1To.Controls[0].Enabled = false;
            nudZone2To.Controls[0].Enabled = false;
            nudZone3To.Controls[0].Enabled = false;
            nudZone4To.Controls[0].Enabled = false;
            nudZone5To.Controls[0].Enabled = false;
            nudZone6To.Controls[0].Enabled = false;

            nudRaiseTime.Controls[0].Enabled = false;
            nudLowerTime.Controls[0].Enabled = false;

            nudUser1.Controls[0].Enabled = false;
            nudUser2.Controls[0].Enabled = false;
            nudUser3.Controls[0].Enabled = false;
            nudUser4.Controls[0].Enabled = false;

            nudTramWidth.Controls[0].Enabled = false;

            nudDualHeadingOffset.Controls[0].Enabled = false;
            nudDualReverseDistance.Controls[0].Enabled = false;

            nudOverlap.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;

            nudTrailingToolToPivotLength.Controls[0].Enabled = false;
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {

            String PloughControl = (gStr.gsPloughControl);
            String DesiredPloughWidth = (gStr.gsDesiredPloughWidth);
            String Deadzone = (gStr.gsDeadzoneinmm);
            String OnOff = (gStr.gsOnOff);
            String Step = (gStr.gsStepWidth);
            String Invert = (gStr.gsInvertWideNarrow);
            String Distance = (gStr.gsEntermeasureddifference);

            groupBox6.Text = PloughControl;
            label110.Text = DesiredPloughWidth;
            label108.Text = Deadzone;
            label109.Text = OnOff;
            lblStepsManual.Text = Step;
            label179.Text = Invert;
            label113.Text = Distance;




            //since we reset, save current state
            mf.SaveFormGPSWindowSettings();
            
            nudUser1.Value = Properties.Settings.Default.setArdMac_user1;
            nudPwmSet.Value = Properties.Settings.Default.setArdMac_pwmSet;

            //Plough Control
            chbPloeg.Checked = mf.isPlougOn;
            //chbPwm.Checked = mf.isPwmOn;
            //chbBlackWhite.Checked = mf.isBlackWhiteOn;
            chbRotationSensor.Checked = mf.isRotationSensorOn;
            //chbInvert.Checked = mf.isInvertOn;
            nudDeadzone.Value = Properties.Settings.Default.setArdMac_deadZone;
            cbManualPloughDir.Checked = Properties.Settings.Default.setPlough_AblineFlipManual;
            cbManualPloughDir.BackgroundImage = Properties.Resources.PlRight;

            //metric or imp on spinners min/maxes
            if (!mf.isMetric) FixMinMaxSpinners();

            //the pick a saved vehicle box
            UpdateVehicleListView();

            //tabTSections_Enter(this, e);
            lblVehicleToolWidth.Text = Convert.ToString((int)(mf.tool.width * 100 * mf.cm2CmOrIn));
            SectionFeetInchesTotalWidthLabelUpdate();

            tab1.SelectedTab = tabSummary;
            tboxVehicleNameSave.Focus();

            label29.Text = gStr.gsSaveAs;
            UpdateSummary();
            //label3.Text = gStr.gsCurrent;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {


            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }

            // Plough

            if (chbPloeg.Checked)
            {
                Properties.Settings.Default.setPlough_isPlougOn = true;
                mf.btnPloughControl.Visible = true;
            }
            else
            {
                Properties.Settings.Default.setPlough_isPlougOn = false;
                mf.btnPloughControl.Visible = false;
            }

            //if (chbPwm.Checked)
            //{
            //    Properties.Settings.Default.setPlough_isPwmOn = true;
            //}
            //else
            //{
            //    Properties.Settings.Default.setPlough_isPwmOn = false;
            //}

            //if (chbBlackWhite.Checked)
            //{
            //    Properties.Settings.Default.setPlough_IsBlackWhiteOn = true;
            //}
            //else
            //{
            //    Properties.Settings.Default.setPlough_IsBlackWhiteOn = false;
            //}

            if (chbRotationSensor.Checked)
            {
                Properties.Settings.Default.setPlough_isRotationSensorOn = true;
            }
            else
            {
                Properties.Settings.Default.setPlough_isRotationSensorOn = false;
            }

           

            if (cbManualPloughDir.Checked == true)
            {
                Properties.Settings.Default.setPlough_AblineFlipManual = true;

            }
            else
            {
                Properties.Settings.Default.setPlough_AblineFlipManual = false;
            }


            //reload all the settings from default and user.config
            mf.LoadSettings();

            //save current vehicle
            SettingsIO.ExportAll(mf.vehiclesDirectory + mf.vehicleFileName + ".XML");
        }

        private void FixMinMaxSpinners()
        {
            nudTankHitch.Maximum = (Math.Round(nudTankHitch.Maximum / 2.54M));
            nudTankHitch.Minimum = Math.Round(nudTankHitch.Minimum / 2.54M);

            nudDrawbarLength.Maximum = Math.Round(nudDrawbarLength.Maximum / 2.54M);
            nudDrawbarLength.Minimum = Math.Round(nudDrawbarLength.Minimum / 2.54M);

            nudTrailingHitchLength.Maximum = Math.Round(nudTrailingHitchLength.Maximum / 2.54M);
            nudTrailingHitchLength.Minimum = Math.Round(nudTrailingHitchLength.Minimum / 2.54M);

            nudTractorHitchLength.Maximum = Math.Round(nudTractorHitchLength.Maximum / 2.54M);
            nudTractorHitchLength.Minimum = Math.Round(nudTractorHitchLength.Minimum / 2.54M);

            nudVehicleTrack.Maximum = Math.Round(nudVehicleTrack.Maximum / 2.54M);
            nudVehicleTrack.Minimum = Math.Round(nudVehicleTrack.Minimum / 2.54M);

            nudWheelbase.Maximum = Math.Round(nudWheelbase.Maximum / 2.54M);
            nudWheelbase.Minimum = Math.Round(nudWheelbase.Minimum / 2.54M);

            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54M);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54M);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54M);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54M);

            nudDefaultSectionWidth.Maximum = Math.Round(nudDefaultSectionWidth.Maximum / 2.54M);
            nudDefaultSectionWidth.Minimum = Math.Round(nudDefaultSectionWidth.Minimum / 3.0M);

            nudSection01.Maximum = Math.Round(nudSection01.Maximum / 2.54M);
            nudSection01.Minimum = Math.Round(nudSection01.Minimum / 2.54M);
            nudSection02.Maximum = Math.Round(nudSection02.Maximum / 2.54M);
            nudSection02.Minimum = Math.Round(nudSection02.Minimum / 2.54M);
            nudSection03.Maximum = Math.Round(nudSection03.Maximum / 2.54M);
            nudSection03.Minimum = Math.Round(nudSection03.Minimum / 2.54M);
            nudSection04.Maximum = Math.Round(nudSection04.Maximum / 2.54M);
            nudSection04.Minimum = Math.Round(nudSection04.Minimum / 2.54M);
            nudSection05.Maximum = Math.Round(nudSection05.Maximum / 2.54M);
            nudSection05.Minimum = Math.Round(nudSection05.Minimum / 2.54M);
            nudSection06.Maximum = Math.Round(nudSection06.Maximum / 2.54M);
            nudSection06.Minimum = Math.Round(nudSection06.Minimum / 2.54M);
            nudSection07.Maximum = Math.Round(nudSection07.Maximum / 2.54M);
            nudSection07.Minimum = Math.Round(nudSection07.Minimum / 2.54M);
            nudSection08.Maximum = Math.Round(nudSection08.Maximum / 2.54M);
            nudSection08.Minimum = Math.Round(nudSection08.Minimum / 2.54M);
            nudSection09.Maximum = Math.Round(nudSection09.Maximum / 2.54M);
            nudSection09.Minimum = Math.Round(nudSection09.Minimum / 2.54M);
            nudSection10.Maximum = Math.Round(nudSection10.Maximum / 2.54M);
            nudSection10.Minimum = Math.Round(nudSection10.Minimum / 2.54M);
            nudSection11.Maximum = Math.Round(nudSection11.Maximum / 2.54M);
            nudSection11.Minimum = Math.Round(nudSection11.Minimum / 2.54M);
            nudSection12.Maximum = Math.Round(nudSection12.Maximum / 2.54M);
            nudSection12.Minimum = Math.Round(nudSection12.Minimum / 2.54M);
            nudSection13.Maximum = Math.Round(nudSection13.Maximum / 2.54M);
            nudSection13.Minimum = Math.Round(nudSection13.Minimum / 2.54M);
            nudSection14.Maximum = Math.Round(nudSection14.Maximum / 2.54M);
            nudSection14.Minimum = Math.Round(nudSection14.Minimum / 2.54M);
            nudSection15.Maximum = Math.Round(nudSection15.Maximum / 2.54M);
            nudSection15.Minimum = Math.Round(nudSection15.Minimum / 2.54M);
            nudSection16.Maximum = Math.Round(nudSection16.Maximum / 2.54M);
            nudSection16.Minimum = Math.Round(nudSection16.Minimum / 2.54M);

            nudTramWidth.Minimum = Math.Round(nudTramWidth.Minimum / 2.54M);
            nudTramWidth.Maximum = Math.Round(nudTramWidth.Maximum / 2.54M);

            //Meters to feet
            nudTurnDistanceFromBoundary.Minimum = Math.Round(nudTurnDistanceFromBoundary.Minimum * 3.28M);
            nudTurnDistanceFromBoundary.Maximum = Math.Round(nudTurnDistanceFromBoundary.Maximum * 3.28M);

            nudOffset.Maximum = Math.Round(nudOffset.Maximum / 2.54M);
            nudOffset.Minimum = Math.Round(nudOffset.Minimum / 2.54M);
            nudOverlap.Maximum = Math.Round(nudOverlap.Maximum / 2.54M);
            nudOverlap.Minimum = Math.Round(nudOverlap.Minimum / 2.54M);

            nudTrailingToolToPivotLength.Maximum = Math.Round(nudTrailingToolToPivotLength.Maximum / 2.54M);
            nudTrailingToolToPivotLength.Minimum = Math.Round(nudTrailingToolToPivotLength.Minimum / 2.54M);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Close();
        }

        private void tabSummary_Enter(object sender, EventArgs e)
        {
            SectionFeetInchesTotalWidthLabelUpdate();
            lblSummaryVehicleName.Text = Properties.Settings.Default.setVehicle_vehicleName;
            UpdateSummary();
        }

        private void tabSummary_Leave(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lvVehicles.SelectedItems.Count > 0)
            {
                //btnVehicleSaveAs.Enabled = true;
                btnVehicleLoad.Enabled = true;
                btnVehicleDelete.Enabled = true;
            }
            else
            {
                //btnVehicleSaveAs.Enabled = false;
                btnVehicleLoad.Enabled = false;
                btnVehicleDelete.Enabled = false;
            }
        }

        private void tabDisplay_Enter(object sender, EventArgs e)
        {
            chkDisplayBrightness.Checked = mf.isBrightnessOn;
            chkDisplayFloor.Checked = mf.isTextureOn;
            chkDisplayGrid.Checked = mf.isGridOn;
            chkDisplaySpeedo.Checked = mf.isSpeedoOn;
            chkDisplayStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkSvennArrow.Checked = mf.isSvennArrowOn;
            chkDisplayExtraGuides.Checked = mf.isSideGuideLines;
            chkDisplayLogNMEA.Checked = mf.isLogNMEA;
            chkDisplayPolygons.Checked = mf.isDrawPolygons;
            chkDisplayLightbar.Checked = mf.isLightbarOn;
            chkDisplayKeyboard.Checked = mf.isKeyboardOn;
            chkDisplayLogElevation.Checked = mf.isLogElevation;

            if (mf.isMetric) rbtnDisplayMetric.Checked = true;
            else rbtnDisplayImperial.Checked = true;
        }

        private void tabDisplay_Leave(object sender, EventArgs e)
        {
            SaveDisplaySettings();
        }

        private void rbtnDisplayImperial_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Imperial");
            mf.isMetric = false;
            Properties.Settings.Default.setMenu_isMetric = mf.isMetric;
            Properties.Settings.Default.Save();
            isClosing = true;
            Close();
        }

        private void rbtnDisplayMetric_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Units Set", "Metric");
            mf.isMetric = true;
            Properties.Settings.Default.setMenu_isMetric = mf.isMetric;
            Properties.Settings.Default.Save();
            isClosing = true;
            Close();
            //FormConfig_Load(this, e);
        }



        #region Plough Tab

        private void btnSetDeadzone_Click(object sender, EventArgs e)
        {
            SaveSettingsMachine();
           
            Properties.Settings.Default.Save();
        }

        private void btnSetPwmMax_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setArdMac_pwmMax = (byte)nudPwmMax.Value;
            SaveSettingsMachine();
            Properties.Settings.Default.Save();
        }

        private void btnSetPwmMin_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setArdMac_pwmMin = (byte)nudPwmMin.Value;
            SaveSettingsMachine();
            Properties.Settings.Default.Save();
        }

        private void btnSetPwm_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setArdMac_pwmSet = (byte)nudPwmSet.Value;
            SaveSettingsMachine();
            Properties.Settings.Default.Save();

        }

        private void btnSetWidth_Click(object sender, EventArgs e)
        {

            SaveSettingsMachine();

            Properties.Settings.Default.Save();

           // mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);


        }

        private void btn_calMin_Click(object sender, EventArgs e)
        {
            mf.p_238.pgn[mf.p_238.user2] = 111; //Cal min command
            SaveSettingsMachine();

            Properties.Settings.Default.Save();
            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);

        }

        private void btn_calMax_Click(object sender, EventArgs e)
        {
            mf.p_238.pgn[mf.p_238.user2] = 222; //Cal max command

            SaveSettingsMachine();

            Properties.Settings.Default.Save();
            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);
        }

        private void label150_Click(object sender, EventArgs e)
        {

        }

        //private void chbInvert_Click(object sender, EventArgs e)
        //{
        //    if (chbInvert.Checked)
        //    {
        //        chbInvert.BackgroundImage = Properties.Resources.PlLeft;
        //        nudInvert.Value = 0;
        //        Properties.Settings.Default.setArdMac_user12 = (byte)nudInvert.Value;
        //        SaveSettingsMachine();
        //        Properties.Settings.Default.Save();
        //    }
        //    else
        //    {
        //        chbInvert.BackgroundImage = Properties.Resources.PlRight;
        //        nudInvert.Value = 1;
        //        Properties.Settings.Default.setArdMac_user12 = (byte)nudInvert.Value;
        //        SaveSettingsMachine();
        //        Properties.Settings.Default.Save();
        //    }
        //}
      

        public void chbInvert_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        

        // Werkt
        //private void cbManualPloughDir_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cbManualPloughDir.Checked == true)
        //    {
        //        cbManualPloughDir.BackgroundImage = Properties.Resources.PlRight;
        //        Properties.Settings.Default.setPlough_AblineFlipManual = false;
        //        mf.btnPloughDir.BackgroundImage = Properties.Resources.PlRight;

        //    }
        //    else
        //    {
        //        cbManualPloughDir.BackgroundImage = Properties.Resources.PlLeft;                    ;
        //        Properties.Settings.Default.setPlough_AblineFlipManual = true;
        //        mf.btnPloughDir.BackgroundImage = Properties.Resources.PlLeft;
        //    }
        //}
        #endregion

        private void btnRemote_Click(object sender, EventArgs e)
        {

            OpenExeFile(sender, e);
           
        }

        public void OpenExeFile(object sender, EventArgs e)
        {
            // Path to the .exe file (assuming it's in the same directory as the program)
            string exePath = Application.StartupPath + @"\Anydesk.exe";

            // Start the .exe file
            Process.Start(exePath);
        }

        private void chbPloeg_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnSetPwmMin_Click_1(object sender, EventArgs e)
        {

        }

        private void EasyModeBtn_Click(object sender, EventArgs e)
        {
            


        }

        private void nudPwmSet_ValueChanged(object sender, EventArgs e)
        {

        }
    }

}