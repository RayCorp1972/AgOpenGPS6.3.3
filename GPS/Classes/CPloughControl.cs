using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using System.Drawing;
using AgOpenGPS.Properties;
using System.IO;

namespace AgOpenGPS
{
    public class CPloughControl
    {
        public uint[] texture;

        public bool PloughControl = false;
        public bool InvertPloughWiderSmaller = false;
        public bool InvertPloughDirection = false;
        public bool isInvertOn1 = false;

        // Plough variables
        public Int16 ploughWidth = 0;
        public byte ploughMode = 255;
        public byte deadBand = 0;
        public int steerModuleConnectedCounter = 0;
        public bool plusPressed = false;
        public bool minPressed = false;

        Image plLeft = Resources.PlLeft;
        Image plRight = Resources.PlRight;
        private readonly FormGPS mf;

        public CPloughControl(FormGPS _f)
        {
            //constructor
            mf = _f;
           // Properties.Settings.Default.Save();



        }

        public void PloughDirInvert()
        {
            if (mf.isInvertOn) //Right
            {
                //nudInvert.Value = 1; // van a naar b - trekker rechts wijkt af van lijn ploeg smaller              
                Properties.Settings.Default.setArdMac_user14 = false;
                SaveSettingsPlough();
                Properties.Settings.Default.Save();
            }
            else //Left
            {
               //nudInvert.Value = 0;  //van b naar a - trekker rechts wijkt af van lijn ploeg breder              
                Properties.Settings.Default.setArdMac_user14 = true;
                SaveSettingsPlough();
                Properties.Settings.Default.Save();
            }
        }

        public void Plougcontrol()
        {
            
            String DesiredPloughWidth = (gStr.gsDesiredPloughWidth);
            String CurrentPloughWidth = (gStr.gsCurrentPloughWidth);
            String Deadzone = (gStr.gsDeadzoneinmm);
            String AutoConfig = (gStr.gsAutoConfigOff);
            String AutoSwitch = (gStr.gsAutoSwitchOff);
            String Hold = (gStr.gsHoldPlough);
            String Wider = (gStr.gsWider);
            String Max = (gStr.gsMax);
            String Min = (gStr.gsMin);
            String Narrow = (gStr.gsSmaller);
            String Abline = (gStr.gsNoline);
            String SectionOff = (gStr.gsSectionoff);
            String Both = (gStr.gsBoth);

            int center = mf.oglMain.Width / -2 + 10;
            GL.Color3(0.9652f, 0.9752f, 0.1f); 

           
            mf.font.DrawText(center + 10, 180, DesiredPloughWidth + ": " + (decimal)Properties.Settings.Default.setArdMac_user1 + "cm", 0.7);
            mf.font.DrawText(center + 10, 210, CurrentPloughWidth + ": " + ploughWidth.ToString() + "cm", 0.7);
            mf.font.DrawText(center + 10, 240, Deadzone + ": " + (decimal)Properties.Settings.Default.setArdMac_deadZone + "mm", 0.7);

            if (ploughMode == 0) mf.font.DrawText(center + 10, 270, SectionOff, 0.7);
            else if (ploughMode == 1) mf.font.DrawText(center + 10, 270, AutoConfig, 1);
            else if (ploughMode == 2) mf.font.DrawText(center + 10, 270, AutoSwitch, 1);
            else if (ploughMode == 3) mf.font.DrawText(center + 10, 270, Hold + "  (A)", 1);
            else if (ploughMode == 4) mf.font.DrawText(center + 10, 270, Wider + " (A)", 1);
            else if (ploughMode == 5) mf.font.DrawText(center + 10, 270, Max + "  (A)", 1);
            else if (ploughMode == 6) mf.font.DrawText(center + 10, 270, Narrow + "  (A)", 1);
            else if (ploughMode == 7) mf.font.DrawText(center + 10, 270, Min, 1);
            else if (ploughMode == 8) mf.font.DrawText(center + 10, 270, Abline, 1);
            else if (ploughMode == 9) mf.font.DrawText(center + 10, 270, Both, 1);
            else if (ploughMode == 10) mf.font.DrawText(center + 10, 270, "Breder (M)", 1);
            else if (ploughMode == 11) mf.font.DrawText(center + 10, 270, "Smaller (M)", 1);
            else if (ploughMode == 12) mf.font.DrawText(center + 10, 270, "Stop (M)", 1);
            else if (ploughMode == 13) mf.font.DrawText(center + 10, 270, "Geeft signaal", 1);
            mf.font.DrawText(center + 25, 310, "" + Wider + "   " + Narrow, 0.8);
            PlougPwmMinus();
            PlougPwmPlus();
            Omkeer();

        }

        private void Omkeer()
        {

            String invert = (gStr.gsInvert);
            String pwmInvert = (gStr.gsPwmInvert);

            int center = mf.oglMain.Width / -2 + 10;

            if (Properties.Settings.Default.setPlough_AblineFlip == false )
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 410, invert + ": " + "L", 1);
                mf.btnPloughDir.BackgroundImage = plRight;

            }
            else
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 410, invert + ": " + "R", 1);
                mf.btnPloughDir.BackgroundImage = plLeft;

            }
            if (Properties.Settings.Default.setArdMac_user14 == true)
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 440, pwmInvert + ": " + "Nee", 0.8);
            }
            else if (Properties.Settings.Default.setArdMac_user14 == false)
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 440, pwmInvert + ": " + "Ja", 0.8);
            }

        }

        private void PlougPwmMinus() // press on screen - button for plough smaller
        {
            GL.Enable(EnableCap.Texture2D);      // Select Our Texture
            GL.Color3(0.90f, 0.90f, 0.93f);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ZoomOut48]);        // Select Our Texture
            GL.Begin(PrimitiveType.Quads);
            int hite = 395;
            int center2 = mf.oglMain.Width / -2 + 160; // 160 = position on main
            {
                GL.TexCoord2(0, 0); GL.Vertex2(center2, hite - 55); // 45 is grote van knop
                GL.TexCoord2(1, 0); GL.Vertex2(center2 + 55, hite - 55); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center2 + 55, hite); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center2, hite); //
            }
            GL.End();
        }

        private void PlougPwmPlus() // press on screen + button for plough wider
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ZoomIn48]);        
            GL.Color3(0.90f, 0.90f, 0.93f);
            GL.Begin(PrimitiveType.Quads);

            int hite = 395;
            int center2 = mf.oglMain.Width / -2 + 48;
            {
                GL.TexCoord2(0, 0); GL.Vertex2(center2, hite - 55); // 
                GL.TexCoord2(1, 0); GL.Vertex2(center2 + 55, hite - 55); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center2 + 55, hite); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center2, hite); //
            }
            GL.End();
        }

        public void SaveSettingsPlough() //wat kunnen we hiermee?

        {  

        }

        public void PwmPloughManualSetPlus()
        {

            if (mf.PlAuto == true)
            {
                ploughWidth = Properties.Settings.Default.setArdMac_user1;
                byte incrementAmount = Properties.Settings.Default.setArdMac_pwmSet; // You can adjust this value to change the increment amount
                ploughWidth += incrementAmount;
                Properties.Settings.Default.setArdMac_user1 = (byte)ploughWidth;
                mf.p_238.pgn[mf.p_238.user1] = (byte)ploughWidth;
                mf.SendPgnToLoop(mf.p_238.pgn);
                Properties.Settings.Default.Save();
            }
            else
            {
                mf.p_238.pgn[mf.p_238.ManualWiderSmaller] = 1;
                mf.SendPgnToLoop(mf.p_238.pgn);

            }

        }

        public void PwmPloughManualSetMin()
        {

            if (mf.PlAuto == true)
            {
                ploughWidth = Properties.Settings.Default.setArdMac_user1;
                byte decrementAmount = Properties.Settings.Default.setArdMac_pwmSet;
                ploughWidth -= decrementAmount;
                Properties.Settings.Default.setArdMac_user1 = (byte)ploughWidth;
                mf.p_238.pgn[mf.p_238.ManualWiderSmaller] = (byte)ploughWidth;
                mf.SendPgnToLoop(mf.p_238.pgn);
                Properties.Settings.Default.Save();
            }
            else
            {

                mf.p_238.pgn[mf.p_238.ManualWiderSmaller] = 2;
                mf.SendPgnToLoop(mf.p_238.pgn);

            }
        }
       
        public void ChangeploughDirection()
        {

            int set = 1;
            int reset = 2046;
            int sett = 0;

            if (mf.isInvertOn) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
           
            isInvertOn1 = !isInvertOn1;


            if (mf.isInvertOn1)
            {
                mf.btnInvertDir.Text = "Uit";
                Properties.Settings.Default.setArdMac_user14 = true;
                sett |= set;
                Properties.Settings.Default.setArdMac_setting0 = (byte)sett;
                mf.p_238.pgn[mf.p_238.set0] = (byte)sett;
                mf.SendPgnToLoop(mf.p_238.pgn);

            }

            else
            {
                mf.btnInvertDir.Text = "Aan";
                Properties.Settings.Default.setArdMac_user14 = false;
                sett &= reset;
                Properties.Settings.Default.setArdMac_setting0 = (byte)sett;
                mf.p_238.pgn[mf.p_238.set0] = (byte)sett;
                mf.SendPgnToLoop(mf.p_238.pgn);
            }
            //     mf.btnInvertDir.Text = "Uit";
            //     mf.nudlessNumericUpDown1.Value = 0;
            //     Properties.Settings.Default.setArdMac_user14 = true;
            //     Properties.Settings.Default.setArdMac_PloughDirection = (byte)mf.nudlessNumericUpDown1.Value;
            //     //mf.p_238.pgn[mf.p_238.PloughDirection] = (byte)mf.nudlessNumericUpDown1.Value;
            //     //mf.SendPgnToLoop(mf.p_238.pgn);


            // }
            // else
            // {
            //     mf.btnInvertDir.Text = "Aan";
            //     mf.nudlessNumericUpDown1.Value = 1;
            //     Properties.Settings.Default.setArdMac_user14 = false;
            //     Properties.Settings.Default.setArdMac_PloughDirection = (byte)mf.nudlessNumericUpDown1.Value;
            //     //mf.p_238.pgn[mf.p_238.PloughDirection] = (byte)mf.nudlessNumericUpDown1.Value;
            //     //mf.SendPgnToLoop(mf.p_238.pgn);
            //    // mf.isInvertOn = false;
            // }


        }

        public void AutoButtonPlough()
        {
            if (mf.isPlougOn)
            {
                // Load images from resources
                Image plAuto1 = Resources.plAuto1;
                Image plMan = Resources.plMan;


                if (IsSameImage(mf.btnPloughControl.BackgroundImage, plAuto1))
                {
                    mf.btnPloughControl.BackgroundImage = plMan;
                    mf.PlAuto = false;
                    mf.p_238.pgn[mf.p_238.ManualWiderSmaller] = 1;
                    mf.SendPgnToLoop(mf.p_238.pgn);


                }

                else if (IsSameImage(mf.btnPloughControl.BackgroundImage, plMan))
                {

                    mf.btnPloughControl.BackgroundImage = plAuto1;
                    mf.PlAuto = true;
                    mf.p_238.pgn[mf.p_238.ManualWiderSmaller] = 0;
                    mf.SendPgnToLoop(mf.p_238.pgn);
                }
            }
        }

        private bool IsSameImage(Image img1, Image img2)
        {
            if (img1 == null || img2 == null)
            {
                return false;
            }

            using (MemoryStream ms1 = new MemoryStream(), ms2 = new MemoryStream())
            {
                img1.Save(ms1, img1.RawFormat);
                img2.Save(ms2, img2.RawFormat);
                return ms1.ToArray().SequenceEqual(ms2.ToArray());
            }
        }

        public void AblineFlip()
        {
            // Load images from resources
            Image plLeft = Resources.PlLeft;
            Image plRight = Resources.PlRight;


            if (IsSameImage(mf.btnPloughDir.BackgroundImage, plLeft))
            {
                mf.btnPloughDir.BackgroundImage = plRight;
                Properties.Settings.Default.setPlough_AblineFlipManual = false;



            }

            else if (IsSameImage(mf.btnPloughDir.BackgroundImage, plRight))
            {

                mf.btnPloughDir.BackgroundImage = plLeft;
                Properties.Settings.Default.setPlough_AblineFlipManual = true;
            }
        }


    }
}
