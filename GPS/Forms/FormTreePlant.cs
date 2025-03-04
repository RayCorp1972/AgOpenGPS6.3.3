using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTreePlant : Form
    {
        private readonly FormGPS mf = null;
        private double lastDist;
        private bool wasRed, isRunning, AutoAan;
        private int trees;
     
        
        public FormTreePlant(Form callingForm)
        {
            mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();
            isRunning = mf.Tree.isPlanting;
            nudRadius.Value = (decimal)mf.Tree.treeRadi;

            //this.Text = gStr.gsTreePlantControl;

            ////Label
            //label12.Text = gStr.gsSpacing;
            //label1.Text = gStr.gsStep;
            //label3.Text = gStr.gsTrees;

            ////Button
            //btnZeroDistance.Text = gStr.gsBegin;
            //btnStop.Text = gStr.gsDone;

            //nudTreeSpacing.Controls[0].Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //if (mf.manualBtnState != AgOpenGPS.FormGPS.btnStates.Off)
            //{
            //    mf.btnManualOffOn.PerformClick();
            //}
            //Properties.Settings.Default.setDistance_TreeSpacing = mf.vehicle.treeSpacing;
            //Properties.Settings.Default.Save();
            //mf.vehicle.treeSpacing = 0;
            mf.Tree.isPlanting = false;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            isRunning = mf.Tree.isPlanting;

            if (mf.AutoAanTree)
            {

                if (mf.Buiten) // Binnen perceel grens
                {


                    if (isRunning)
                    {
                        //mf.Tree.isPlanting = true;
                        lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter * 0.01).ToString();
                        if (lastDist > mf.treeSpacingCounter)
                        {

                            wasRed = !wasRed;
                            if (wasRed) btnZeroDistance.BackColor = Color.DarkSeaGreen;

                            else btnZeroDistance.BackColor = Color.LightGreen;

                        }
                        btnZeroDistance.Text = "Stop";

                    }
                    else
                    {
                        btnZeroDistance.Text = "Start";

                    }
                    if (mf.Tree.isSound)
                    {
                        button1.BackColor = Color.DarkGreen;
                        this.BackColor = Color.Black;
                    }
                    else
                    {
                        button1.BackColor = Color.Orange;

                    }
                    if (mf.treeTrigger == 1) pictureBox1.Image = Properties.Resources.SwitchOn;
                    if (mf.treeTrigger == 1) this.BackColor = Color.Red;
                    else pictureBox1.Image = Properties.Resources.SwitchOff;
                    lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
                    lblSpeed.Text = mf.pn.speed.ToString("N1");
                    lblTrees.Text = mf.Tree.ptList.Count.ToString();
                    lastDist = mf.treeSpacingCounter;

                    this.lblTrees.TextChanged += new System.EventHandler(this.lblTrees_TextChanged);
                    mf.Tree.isPlanting = true;

                }
                else
                {
                    lastDist = 0;
                    mf.treeSpacingCounter = 0;
                    lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter * 0.01).ToString();
                    btnZeroDistance.Text = "Standby";
                    mf.Tree.isPlanting = false;

                }


            }

            if (!mf.AutoAanTree)
            {
                if (isRunning)
                {
                    //mf.Tree.isPlanting = true;
                    lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter * 0.01).ToString();
                    if (lastDist > mf.treeSpacingCounter)
                    {

                        wasRed = !wasRed;
                        if (wasRed) btnZeroDistance.BackColor = Color.DarkSeaGreen;

                        else btnZeroDistance.BackColor = Color.LightGreen;

                    }
                    btnZeroDistance.Text = "Stop";

                }
                else
                {
                    btnZeroDistance.Text = "Start";

                }
                if (mf.Tree.isSound)
                {
                    button1.BackColor = Color.DarkGreen;
                    this.BackColor = Color.Black;
                }
                else
                {
                    button1.BackColor = Color.Orange;

                }
                if (mf.treeTrigger == 1) pictureBox1.Image = Properties.Resources.SwitchOn;
                if (mf.treeTrigger == 1) this.BackColor = Color.Red;
                else pictureBox1.Image = Properties.Resources.SwitchOff;
                lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
                lblSpeed.Text = mf.pn.speed.ToString("N1");
                lblTrees.Text = mf.Tree.ptList.Count.ToString();
                lastDist = mf.treeSpacingCounter;

                this.lblTrees.TextChanged += new System.EventHandler(this.lblTrees_TextChanged);
                //mf.Tree.isPlanting = true;

            }
        }
        



        //else
        //{
        //lastDist = 0;
        //mf.treeSpacingCounter = 0;
        //lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter * 0.01).ToString();
        //btnZeroDistance.Text = "Standby";
        ////mf.Tree.isPlanting = false;

        //}
        //isRunning = mf.Tree.isPlanting;
    
            //else

            //{

            //    if (isRunning)
            //    {
            //        lastDist = 0;
            //        mf.treeSpacingCounter = 0;

            //        mf.distanceCurrentStepFix = 0;
            //        lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter).ToString();
            //        lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
            //        btnZeroDistance.BackColor = Color.OrangeRed;

            //        //mf.Tree.isPlanting = false;
            //    }
            //    else
            //    {
            //        lastDist = 0;
            //        trees = 0;
            //        mf.treeSpacingCounter = 0;

            //        mf.distanceCurrentStepFix = 0;
            //        lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter).ToString();
            //        lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
            //        btnZeroDistance.BackColor = Color.LightGreen;
            //      // mf.Tree.isPlanting = true;



            //    }


            //    isRunning = mf.Tree.isPlanting;
        //    }

        //}

        private void lblTrees_TextChanged(object sender, EventArgs e)
        {
           this.BackColor = Color.Red;
        }
        private void btnZeroDistance_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                lastDist = 0;
                mf.treeSpacingCounter = 0;
                
                mf.distanceCurrentStepFix = 0;
                lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter).ToString();
                lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
                btnZeroDistance.BackColor = Color.OrangeRed;
                
                mf.Tree.isPlanting = false;
               
            }
            else
            {
                lastDist = 0;
                trees = 0;
                mf.treeSpacingCounter = 0;
              

                mf.distanceCurrentStepFix = 0;
                lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter).ToString();
                lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
                btnZeroDistance.BackColor = Color.LightGreen;
                
                mf.Tree.isPlanting = true;

               
            }


            isRunning = mf.Tree.isPlanting;
        }

        private void NudTreeSpacing_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);


            Properties.Settings.Default.SetTreeSpace_User = (double)nudTreeSpacing.Value;
            mf.vehicle.treeSpacing = (double)nudTreeSpacing.Value;
            Properties.Settings.Default.Save();
        }

        private void NudTreeSpacing_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
            btnStop.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mf.Tree.isSound = !mf.Tree.isSound;
            button1.BackColor = Color.Green;
        }

        private void nudRadius_Click(object sender, EventArgs e)
        {

            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
            Properties.Settings.Default.SetTreeRadius_User = (int)nudRadius.Value;

            mf.Tree.treeRadi = (int)nudRadius.Value;

        }

        private void button2_Click(object sender, EventArgs e)
        
            {
            DialogResult result = MessageBox.Show(
       "Weet u zeker dat u de Punten wil wissen?", // Message
       "Let Op!",             // Title
       MessageBoxButtons.OKCancel, // OK & Cancel buttons
       MessageBoxIcon.Question     // Question mark icon
         );

            if (result == DialogResult.OK)
            {
                mf.Tree.ptList?.Clear();
                lblTrees.Text = "0";
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
     }
        // hij flikkert
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.BackColor == Color.Green)
            {
                button3.BackColor = Color.WhiteSmoke;
                label6.Text = "Auto Uit";
                mf.AutoAanTree = false;


            }
            else
            {
                button3.BackColor = Color.Green;
                label6.Text = "Auto Aan";
                mf.AutoAanTree = true;
            }
        }

        private void FormTreePlant_Load(object sender, EventArgs e)
        {
            nudTreeSpacing.Value = (decimal)Properties.Settings.Default.SetTreeSpace_User;
            nudRadius.Value = Properties.Settings.Default.SetTreeRadius_User;


            mf.vehicle.treeSpacing = Properties.Settings.Default.SetTreeSpace_User;
            nudTreeSpacing.Value = (decimal)mf.vehicle.treeSpacing;
            lastDist = 0;
            mf.treeSpacingCounter = 0;
            trees = 0;
            isRunning = false;
            btnZeroDistance.Text = "Start";
            btnZeroDistance.BackColor = Color.OrangeRed;
            
        }
    }
}
