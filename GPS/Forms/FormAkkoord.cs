using System;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AgOpenGPS
{
    public partial class FormAkkoord : Form
    {
       

        private FormJob formJob;

        public FormAkkoord(FormGPS formGPS)
        {
            InitializeComponent();
            

        }

        public FormAkkoord(FormJob formJob)
        {
            this.formJob = formJob;
        }

        private void FormAkkoord_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
