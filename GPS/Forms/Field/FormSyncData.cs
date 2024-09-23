using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSyncData : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormSyncData(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormSyncData_Load(object sender, EventArgs e)
        {
            txtb_Password.Text = Properties.Settings.Default.Sync_Pass;
            txtb_Username.Text = Properties.Settings.Default.Sync_User;
            ckbSync.Checked = Properties.Settings.Default.Sync_chkBox;
        }

        private void btnSaveSync_Click(object sender, EventArgs e)
        {
            if (ckbSync.Checked) // Check if the checkbox is checked
            {
                // Check if both textboxes are empty
                if (string.IsNullOrWhiteSpace(txtb_Username.Text) || string.IsNullOrWhiteSpace(txtb_Password.Text))
                {
                    // Show warning message
                    MessageBox.Show("Vul gegevens in of zet Sync uit!!");
                    Close();
                    // Optionally, you can also uncheck the checkbox
                    //ckbSync.Checked = false;
                }
                else
                {
                    // Textboxes are filled, continue processing
                    Properties.Settings.Default.Sync_Pass = txtb_Password.Text;
                    Properties.Settings.Default.Sync_User = txtb_Username.Text;
                    Properties.Settings.Default.Sync_chkBox = true;
                    Properties.Settings.Default.Save();
                    Close();
                }
            }
            else
            {
                Properties.Settings.Default.Sync_chkBox = false;
                Close();
            }
        }

        private void btnSyncCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}