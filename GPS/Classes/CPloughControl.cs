using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using System.Drawing;

namespace AgOpenGPS
{
    public class CPloughControl
    {
        public uint[] texture;

        public bool PloughControl = false;
        public bool InvertPloughWiderSmaller = false;
        public bool InvertPloughDirection = false;
        

        // Plough variables
        public Int16 ploughWidth = 0;
        public byte ploughMode = 255;
        public byte deadBand = 0;
        public int steerModuleConnectedCounter = 0;
        public bool plusPressed = false;
        public bool minPressed = false;
       

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
               // nudInvert.Value = 0;  //van b naar a - trekker rechts wijkt af van lijn ploeg breder              
                Properties.Settings.Default.setArdMac_user14 = true;
                SaveSettingsPlough();
                Properties.Settings.Default.Save();
            }
        }

        public void Plougcontrol()
        {
            String PloughControl = (gStr.gsPloughControl);
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
            
            int center =  -685;
            
            GL.Color3(0.9652f, 0.9752f, 0.1f); 

           // mf.font.DrawText(center + 10, 150, PloughControl, .9); // 
            mf.font.DrawText(center + 10, 180, DesiredPloughWidth + ": " + (decimal)Properties.Settings.Default.setArdMac_user1 + "cm", 0.7);
            mf.font.DrawText(center + 10, 210, CurrentPloughWidth + ": " + ploughWidth.ToString() + "cm", 0.7);
            mf.font.DrawText(center + 10, 240, Deadzone + ": " + (decimal)Properties.Settings.Default.setArdMac_user5 + "mm", 0.7);

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
            mf.font.DrawText(center + 25, 310, "" + Wider + "     " + Narrow, 0.8);
            PlougPwmMinus();
            PlougPwmPlus();
            Omkeer();

        }

        private void Omkeer()
        {

            String invert = (gStr.gsInvert);
            String pwmInvert = (gStr.gsPwmInvert);

            int center = -685;

            if (Properties.Settings.Default.setPlough_AblineFlip == false)
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 410, invert + ": " + "L", 0.8);
            }
            else
            {
                GL.Color3(0.9652f, 0.9752f, 0.1f); //Yellow
                mf.font.DrawText(center + 10, 410, invert + ": " + "R", 0.8);
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

        private void PlougPwmMinus() // Druk knop - in scherm voor smaller ploeg
        {
            GL.Enable(EnableCap.Texture2D);      // Select Our Texture
            GL.Color3(0.90f, 0.90f, 0.93f);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ZoomOut48]);        // Select Our Texture
            GL.Begin(PrimitiveType.Quads);
            int hite = 395;
            int center2 = -500; // 160 = position on main
            {
                GL.TexCoord2(0, 0); GL.Vertex2(center2, hite - 55); // 45 is grote van knop
                GL.TexCoord2(1, 0); GL.Vertex2(center2 + 55, hite - 55); // 
                GL.TexCoord2(1, 1); GL.Vertex2(center2 + 55, hite); // 
                GL.TexCoord2(0, 1); GL.Vertex2(center2, hite); //
            }
            GL.End();
        }

        private void PlougPwmPlus() // Druk knop + in scherm voor breder ploeg
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ZoomIn48]);        // Select Our Texture
            GL.Color3(0.90f, 0.90f, 0.93f);
            GL.Begin(PrimitiveType.Quads);

            int hite = 395;
            int center2 = -650;
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
            mf.p_238.pgn[mf.p_238.user1] = (byte)Properties.Settings.Default.setArdMac_user1;                       //Target width cm 
            mf.p_238.pgn[mf.p_238.user5] = (byte)Properties.Settings.Default.setArdMac_user5;
            mf.p_238.pgn[mf.p_238.user6] = (byte)Properties.Settings.Default.setArdMac_user6; 
            mf.p_238.pgn[mf.p_238.user7] = (byte)Properties.Settings.Default.setArdMac_user7; 
            mf.p_238.pgn[mf.p_238.user8] = (byte)Properties.Settings.Default.setArdMac_user8;           
            mf.p_238.pgn[mf.p_238.user13] = Properties.Settings.Default.setArdMac_user13;

            mf.SendPgnToLoop(mf.p_238.pgn);
            mf.p_238.pgn[mf.p_238.user2] = 0;                                  
        }

    }
}
