using System;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //Latitude
        public class CPGN_D0
        {
            /// <summary>
            ///  Latitude Longitude 8 bytes as modified float
            ///  double lat = (encodedAngle / (0x7FFFFFFF / 90.0));
            ///  double lon = (encodedAngle / (0x7FFFFFFF / 180.0));
            /// </summary>
            public byte[] latLong = new byte[] { 0x80, 0x81, 0x7F, 0xD0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };


            public void LoadLatitudeLongitude(double lat, double lon)
            {

                int encodedAngle = (int)(lat * (0x7FFFFFFF / 90.0));
                //double angle = (encodedAngle / (0x7FFFFFFF / 90.0));

                byte[] lat6 = BitConverter.GetBytes(encodedAngle);
                Array.Copy(lat6, 0, latLong, 5, 4);

                encodedAngle = (int)(lon * (0x7FFFFFFF / 180.0));
                //double angle = (encodedAngle / (0x7FFFFFFF / 180.0));

                lat6 = BitConverter.GetBytes(encodedAngle);
                Array.Copy(lat6, 0, latLong, 9, 4);
            }
        }

        //AutoSteerData
        public class CPGN_FE
        {
            /// <summary>
            /// 8 bytes
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFE, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int speedLo = 5;
            public int speedHi = 6;
            public int status = 7;
            public int steerAngleLo = 8;
            public int steerAngleHi = 9;
            public int lineDistance = 10;
            public int sc1to8 = 11;
            public int sc9to16 = 12;

            public void Reset()
            {
            }
        }

        public class CPGN_FD
        {
            /// <summary>
            /// From steer module
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFD, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int actualLo = 5;
            public int actualHi = 6;
            public int headLo = 7;
            public int headHi = 8;
            public int rollLo = 9;
            public int rollHi = 10;
            public int switchStatus = 11;
            public int pwm = 12;

            public void Reset()
            {
            }
        }


        //AutoSteer Settings
        public class CPGN_FC
        {
            /// <summary>
            /// PGN - 252 - FC gainProportional=5 HighPWM=6  LowPWM = 7 MinPWM = 8 
            /// CountsPerDegree = 9 wasOffsetHi = 10 wasOffsetLo = 11 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFC, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int gainProportional = 5;
            public int highPWM = 6;
            public int lowPWM = 7;
            public int minPWM = 8;
            public int countsPerDegree = 9;
            public int wasOffsetLo = 10;
            public int wasOffsetHi = 11;
            public int ackerman = 12;

            public CPGN_FC()
            {
                pgn[gainProportional] = Properties.Settings.Default.setAS_Kp;
                pgn[highPWM] = Properties.Settings.Default.setAS_highSteerPWM;
                pgn[lowPWM] = Properties.Settings.Default.setAS_lowSteerPWM;
                pgn[minPWM] = Properties.Settings.Default.setAS_minSteerPWM;
                pgn[countsPerDegree] = Properties.Settings.Default.setAS_countsPerDegree;
                pgn[wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8)); ;
                pgn[wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_Kp));
                pgn[ackerman] = Properties.Settings.Default.setAS_ackerman;
            }

            public void Reset()
            {
            }
        }

        //Autosteer Board Config
        public class CPGN_FB
        {
            /// <summary>
            /// 
            /// PGN - 251 - FB 
            /// set0=5 maxPulse = 6 minSpeed = 7 ackermanFix = 8
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xFB, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int set0 = 5;
            public int maxPulse = 6;
            public int minSpeed = 7;
            public int set1 = 8;
            public int angVel = 9;
            //public int  = 10;
            //public int  = 11;
            //public int  = 12;

            public CPGN_FB()
            {
                pgn[set0] = 0;
                pgn[maxPulse] = 0;
                pgn[minSpeed] = 0;
                pgn[set1] = 0;
                pgn[angVel] = 0;
            }

            public void Reset()
            {
            }
        }

        //Machine Data
        public class CPGN_EF
        {
            /// <summary>
            /// PGN - 239 - EF 
            /// uturn=5  tree=6  hydLift = 8 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEF, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int uturn = 5;
            public int speed = 6;
            public int hydLift = 7;
            public int tram = 8;
            //public int geoStop = 9; //out of bounds etc
            public int lineDistanceL = 9;       //extra mm +/- needed on working width (mm to the line, *-1 if heading is not same way as ABline)
            public int lineDistanceH = 10;
            public int sc1to8 = 11;
            public int sc9to16 = 12;

            public CPGN_EF()
            {
            }

            public void Reset()
            {
            }
        }
        public class CPGN_E5
        {
            /// <summary>
            /// PGN - 229 - E5 
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE5, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int sc1to8 = 5;
            public int sc9to16 = 6;
            public int sc17to24 = 7;
            public int sc25to32 = 8;
            public int sc33to40 = 9;
            public int sc41to48 = 10;
            public int sc49to56 = 11;
            public int sc57to64 = 12;
            public int toolLSpeed = 13;
            public int toolRSpeed = 14;

            public CPGN_E5()
            {
            }

            public void Reset()
            {
            }
        }

        //Machine Config
        public class CPGN_EE
        {
            /// <summary>
            /// PGN - 238 - EE 
            /// raiseTime=5  lowerTime=6   enableHyd= 7 set0 = 8
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEE, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int raiseTime = 5;
            public int lowerTime = 6;
            public int enableHyd = 7;
            public int set0 = 8;  //reverse left/right
            public int user1 = 9;       //Target width (+200cm)                                            5  
            public int user2 = 10;      //Calabration command instrction                                   
            public int user3 = 11;      //Set value L
            public int user4 = 12;      //Set value H
            public int user5 = 13;      //deadBand
            public int user6 = 14;      //Pwm High
            public int user7 = 15;      //Pwm Low
            public int user8 = 16;      //Pwm / Black-White
            public int user9 = 17;      //Auto + Button wider
            public int user10 = 18;      //Auto - button smaller
            public int user12 = 19;      //Manual wider/smaller


            // PGN  - 127.239 0x7FEF
            int crc = 0;

            public CPGN_EE()
            {
                pgn[raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
                pgn[lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;
                pgn[enableHyd] = Properties.Settings.Default.setArdMac_isHydEnabled;
                pgn[set0] = Properties.Settings.Default.setArdMac_setting0;

                pgn[user1] = Properties.Settings.Default.setArdMac_user1;
                pgn[user2] = Properties.Settings.Default.setArdMac_user2;
                pgn[user3] = Properties.Settings.Default.setArdMac_user3;
                pgn[user4] = Properties.Settings.Default.setArdMac_user4;
                pgn[user5] = Properties.Settings.Default.setArdMac_user5;
                pgn[user6] = Properties.Settings.Default.setArdMac_user6;
                pgn[user7] = Properties.Settings.Default.setArdMac_user7;
                pgn[user8] = Properties.Settings.Default.setArdMac_user8;
                pgn[user9] = Properties.Settings.Default.setArdMac_user9;
                pgn[user10] = Properties.Settings.Default.setArdMac_user10;
                pgn[user12] = Properties.Settings.Default.setArdMac_user12;

            }

            public void MakeCRC()
            {
                crc = 0;
                for (int i = 2; i < pgn.Length - 1; i++)
                {
                    crc += pgn[i];
                }
                pgn[pgn.Length - 1] = (byte)crc;
            }

            public void Reset()
            {
            }
        }

        //Relay Config
        public class CPGN_EC
        {
            /// <summary>
            /// PGN - 236 - EC
            /// Pin conifg 1 to 20
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEC, 24,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0xCC };

            //where in the pgn is which pin
            public int pin0 = 5;
            public int pin1 = 6;
            public int pin2 = 7;
            public int pin3 = 8;
            public int pin4 = 9;
            public int pin5 = 10;
            public int pin6 = 11;
            public int pin7 = 12;
            public int pin8 = 13;
            public int pin9 = 14;

            public int pin10 = 15;
            public int pin11 = 16;
            public int pin12 = 17;
            public int pin13 = 18;
            public int pin14 = 19;
            public int pin15 = 20;
            public int pin16 = 21;

            public int pin17 = 22;
            public int pin18 = 23;
            public int pin19 = 24;
            public int pin20 = 25;
            public int pin21 = 26;
            public int pin22 = 27;
            public int pin23 = 28;

            // PGN  - 127.237 0x7FED
            int crc = 0;

            public CPGN_EC()
            {
                string[] words;

                words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

                pgn[pin0] = (byte)int.Parse(words[0]);
                pgn[pin1] = (byte)int.Parse(words[1]);
                pgn[pin2] = (byte)int.Parse(words[2]);
                pgn[pin3] = (byte)int.Parse(words[3]);
                pgn[pin4] = (byte)int.Parse(words[4]);
                pgn[pin5] = (byte)int.Parse(words[5]);
                pgn[pin6] = (byte)int.Parse(words[6]);
                pgn[pin7] = (byte)int.Parse(words[7]);
                pgn[pin8] = (byte)int.Parse(words[8]);
                pgn[pin9] = (byte)int.Parse(words[9]);

                pgn[pin10] = (byte)int.Parse(words[10]);
                pgn[pin11] = (byte)int.Parse(words[11]);
                pgn[pin12] = (byte)int.Parse(words[12]);
                pgn[pin13] = (byte)int.Parse(words[13]);
                pgn[pin14] = (byte)int.Parse(words[14]);
                pgn[pin15] = (byte)int.Parse(words[15]);
                pgn[pin16] = (byte)int.Parse(words[16]);
                pgn[pin17] = (byte)int.Parse(words[17]);
                pgn[pin18] = (byte)int.Parse(words[18]);
                pgn[pin19] = (byte)int.Parse(words[19]);

                pgn[pin20] = (byte)int.Parse(words[20]);
                pgn[pin21] = (byte)int.Parse(words[21]);
                pgn[pin22] = (byte)int.Parse(words[22]);
                pgn[pin23] = (byte)int.Parse(words[23]);

            }

            public void MakeCRC()
            {
                crc = 0;
                for (int i = 2; i < pgn.Length - 1; i++)
                {
                    crc += pgn[i];
                }
                pgn[pgn.Length - 1] = (byte)crc;
            }

            public void Reset()
            {
            }
        }

        public class CPGN_EB
        {
            /// <summary>
            /// PGN - 235 - EB
            /// Section dimensions
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEB, 33,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0xCC };

            //where in the pgn is which pin
            public int sec0Lo = 5;
            public int sec1Lo = 7;
            public int sec2Lo = 9;
            public int sec3Lo = 11;
            public int sec4Lo = 13;
            public int sec5Lo = 15;
            public int sec6Lo = 17;
            public int sec7Lo = 19;
            public int sec8Lo = 21;
            public int sec9Lo = 23;
            public int sec10Lo = 25;
            public int sec11Lo = 27;
            public int sec12Lo = 29;
            public int sec13Lo = 31;
            public int sec14Lo = 33;
            public int sec15Lo = 35;

            public int sec0Hi = 6;
            public int sec1Hi = 8;
            public int sec2Hi = 10;
            public int sec3Hi = 12;
            public int sec4Hi = 14;
            public int sec5Hi = 16;
            public int sec6Hi = 18;
            public int sec7Hi = 20;
            public int sec8Hi = 22;
            public int sec9Hi = 24;
            public int sec10Hi = 26;
            public int sec11Hi = 28;
            public int sec12Hi = 30;
            public int sec13Hi = 32;
            public int sec14Hi = 34;
            public int sec15Hi = 36;

            public int numSections = 37;

            public CPGN_EB()
            {
                pgn[sec0Lo] = 0;
                pgn[sec1Lo] = 0;
                pgn[sec2Lo] = 0;
                pgn[sec3Lo] = 0;
                pgn[sec4Lo] = 0;
                pgn[sec5Lo] = 0;
                pgn[sec6Lo] = 0;
                pgn[sec7Lo] = 0;
                pgn[sec8Lo] = 0;
                pgn[sec9Lo] = 0;
                pgn[sec10Lo] = 0;
                pgn[sec11Lo] = 0;
                pgn[sec12Lo] = 0;
                pgn[sec13Lo] = 0;
                pgn[sec14Lo] = 0;
                pgn[sec15Lo] = 0;

                pgn[sec0Hi] = 0;
                pgn[sec1Hi] = 0;
                pgn[sec2Hi] = 0;
                pgn[sec3Hi] = 0;
                pgn[sec4Hi] = 0;
                pgn[sec5Hi] = 0;
                pgn[sec6Hi] = 0;
                pgn[sec7Hi] = 0;
                pgn[sec8Hi] = 0;
                pgn[sec9Hi] = 0;
                pgn[sec10Hi] = 0;
                pgn[sec11Hi] = 0;
                pgn[sec12Hi] = 0;
                pgn[sec13Hi] = 0;
                pgn[sec14Hi] = 0;
                pgn[sec15Hi] = 0;

                pgn[numSections] = 0;
            }

            public void Reset()
            {
            }
        }

        public class CPGN_E4
        {
            /// <summary>
            /// 8 bytes
            /// </summary>
            public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE4, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
            public int rate0 = 5;
            public int rate1 = 6;
            public int rate2 = 7;
            //public int  = 6;
            //public int = 7;
            //public int gleLo = 8;
            //public int gleHi = 9;
            //public int tance = 10;
            //public int = 11;
            //public int  = 12;
        }

        ////ISOBUS TC-SC CONTROL
        //public class CPGN_E6
        //{
        //    /// <summary>
        //    /// PGN - 240 - EG
        //    /// numberofsections,Sectionwidth(The width of each section in mm), 
        //    /// </summary>
        //    public byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE6, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0xCC };
        //    public int numberOfSections = 5;       //Number of sections                                            
        //    public int workingWidth = 6;      //The width of each section in mm                             
        //    public int actualSectionStates = 7;      //The actual operation state each section currently has  (0=off, 1=on, 2=error, 3 =unknown / not-installed)
        //    public int SetPointSectionStates = 8;      //The desired state for each section  (0=off, 1=on, 2=reserved, 3 = not-installed), a section can for any reason not adhere to this setpoint. The reason will most of the times be communicated to the operator via a notification on the VT
        //    public int Section1 = 9;
        //    public int Section2 = 10;
        //    public int Section3 = 11;
        //    public int Section4 = 12;
        //    public int Section5 = 13;
        //    public int Section6 = 14;
        //    public int Section7 = 15;
        //    public int Section8 = 16;
        //    public int Section9 = 17;
        //    public int Section10 = 18;
        //    public int Section11 = 19;
        //    public int Section12 = 20;
        //    public int Section13 = 21;
        //    public int Section14 = 22;
        //    public int Section15 = 23;

        //    // PGN  - 127.240 0x7FEF
        //    int crc = 0;

        //    public CPGN_E6()
        //    {
        //        pgn[numberOfSections] = 0;               
        //        pgn[actualSectionStates] = 0;
        //        pgn[SetPointSectionStates] = 0;
        //        pgn[Section1] = 0;
        //        pgn[Section2] = 0;
        //        pgn[Section3] = 0;
        //        pgn[Section4] = 0;
        //        pgn[Section5] = 0;
        //        pgn[Section6] = 0;
        //        pgn[Section7] = 0;
        //        pgn[Section8] = 0;
        //        pgn[Section9] = 0;
        //        pgn[Section10] = 0;
        //        pgn[Section11] = 0;
        //        pgn[Section12] = 0;
        //        pgn[Section13] = 0;
        //        pgn[Section14] = 0;
        //        pgn[Section15] = 0;
        //    }

        //    public void MakeCRC()
        //    {
        //        crc = 0;
        //        for (int i = 2; i < pgn.Length - 1; i++)
        //        {
        //            crc += pgn[i];
        //        }
        //        pgn[pgn.Length - 1] = (byte)crc;
        //    }

        //    public void Reset()
        //    {
        //    }
        //}

        public class CPGN_E6
        {
            /// <summary>
            /// PGN - 240 - EG
            /// </summary>
            public byte[] pgn = new byte[]
            {
            0x80, 0x81, 0x7F, 0xE6, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC
            };
            public int numberOfSections = 5; // Number of sections
            public int actualSectionStates = 6;
            public int SetPointSectionStates = 7;

            // Section offsets in the PGN
            public int Section1 = 8;
            public int Section2 = 10;
            public int Section3 = 12;
            public int Section4 = 14;
            public int Section5 = 16;
            public int Section6 = 18;
            public int Section7 = 20;
            public int Section8 = 22;
            public int Section9 = 24;
            public int Section10 = 26;
            public int Section11 = 28;
            public int Section12 = 30;
            public int Section13 = 32;
            public int Section14 = 34;
            public int Section15 = 36;

            public CPGN_E6()
            {
                

                // Reset sections to default values
                ResetSections();
            }

            /// <summary>
            /// Reset all section widths to 0 within the predefined PGN structure
            /// </summary>
            public void ResetSections()
            {
                for (int i = 8; i < pgn.Length - 1; i += 2) // Exclude the CRC byte
                {
                    pgn[i] = 0;       // LSB
                    pgn[i + 1] = 0;   // MSB
                }
            }

            /// <summary>
            /// Set section widths based on incoming data
            /// </summary>
            /// <param name="incomingData">Incoming data (2 bytes per section width)</param>
            public void SetSectionsFromIncoming(byte[] incomingData)
            {
                if (incomingData.Length % 2 != 0)
                    throw new ArgumentException("Invalid incoming data length");

                int offsetBase = Section1; // Starting position for Section1 in PGN
                int sectionsToSet = incomingData.Length / 2;

                for (int i = 0; i < sectionsToSet; i++)
                {
                    int offset = offsetBase + i * 2;
                    if (offset + 1 >= pgn.Length - 1) break; // Prevent overflow, leave CRC intact

                    int sectionWidth = incomingData[i * 2] + (incomingData[i * 2 + 1] << 8);

                    // Set the section width in the PGN
                    pgn[offset] = (byte)(sectionWidth & 0xFF);        // LSB
                    pgn[offset + 1] = (byte)((sectionWidth >> 8) & 0xFF); // MSB
                }
            }

            /// <summary>
            /// Compute CRC and update the last byte in the PGN
            /// </summary>
            public void MakeCRC()
            {
                int crc = 0;
                for (int i = 2; i < pgn.Length - 1; i++) // CRC calculation excludes the last byte
                {
                    crc += pgn[i];
                }
                pgn[pgn.Length - 1] = (byte)(crc & 0xFF);
            }
        }




        //pgn instances

        /// <summary>
        /// autoSteerData - FE - 254 - 
        /// </summary>
        public CPGN_FE p_254 = new CPGN_FE();

        /// <summary>
        /// autoSteerSettings PGN - 252 - FC
        /// </summary>
        public CPGN_FC p_252 = new CPGN_FC();

        /// <summary>
        /// autoSteerConfig PGN - 251 - FB
        /// </summary>
        public CPGN_FB p_251 = new CPGN_FB();

        /// <summary>
        /// machineData PGN - 239 - EF
        /// </summary>
        public CPGN_EF p_239 = new CPGN_EF();

        /// <summary>
        /// machineConfig PGN - 238 - EE
        /// </summary>
        public CPGN_EE p_238 = new CPGN_EE();

        /// <summary>
        /// relayConfig PGN - 236 - EC
        /// </summary>
        public CPGN_EC p_236 = new CPGN_EC();

        /// <summary>
        /// Section dimensions PGN - 235 - EB
        /// </summary>
        public CPGN_EB p_235 = new CPGN_EB();

        /// <summary>
        /// Section dimensions PGN - 228 - E4
        /// </summary>
        public CPGN_E4 p_228 = new CPGN_E4();

        /// <summary>
        /// Section Symmetric PGN - 235 - EB
        /// </summary>
        public CPGN_E5 p_229 = new CPGN_E5();

        /// <summary>
        ///  TC-SC (section control)
        /// </summary>
        public CPGN_E6 p_240 = new CPGN_E6();

    }
}
    
