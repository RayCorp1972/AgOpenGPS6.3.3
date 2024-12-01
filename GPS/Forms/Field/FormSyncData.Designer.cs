namespace AgOpenGPS
{
    partial class FormSyncData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtb_Username = new System.Windows.Forms.TextBox();
            this.btnSaveSync = new System.Windows.Forms.Button();
            this.btnSyncCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtb_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbSync = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtb_ServerIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtb_Username
            // 
            this.txtb_Username.BackColor = System.Drawing.Color.AliceBlue;
            this.txtb_Username.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtb_Username.Location = new System.Drawing.Point(17, 135);
            this.txtb_Username.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtb_Username.Name = "txtb_Username";
            this.txtb_Username.Size = new System.Drawing.Size(634, 36);
            this.txtb_Username.TabIndex = 1;
            // 
            // btnSaveSync
            // 
            this.btnSaveSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSync.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveSync.FlatAppearance.BorderSize = 0;
            this.btnSaveSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSync.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSaveSync.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnSaveSync.Location = new System.Drawing.Point(588, 417);
            this.btnSaveSync.Name = "btnSaveSync";
            this.btnSaveSync.Size = new System.Drawing.Size(83, 79);
            this.btnSaveSync.TabIndex = 4;
            this.btnSaveSync.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveSync.UseVisualStyleBackColor = false;
            this.btnSaveSync.Click += new System.EventHandler(this.btnSaveSync_Click);
            // 
            // btnSyncCancel
            // 
            this.btnSyncCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnSyncCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSyncCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSyncCancel.FlatAppearance.BorderSize = 0;
            this.btnSyncCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSyncCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSyncCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnSyncCancel.Location = new System.Drawing.Point(464, 416);
            this.btnSyncCancel.Name = "btnSyncCancel";
            this.btnSyncCancel.Size = new System.Drawing.Size(77, 79);
            this.btnSyncCancel.TabIndex = 5;
            this.btnSyncCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSyncCancel.UseVisualStyleBackColor = false;
            this.btnSyncCancel.Click += new System.EventHandler(this.btnSyncCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(13, 107);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 23);
            this.label4.TabIndex = 154;
            this.label4.Text = "Enter Username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(13, 191);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 23);
            this.label1.TabIndex = 155;
            this.label1.Text = "Enter Password";
            // 
            // txtb_Password
            // 
            this.txtb_Password.BackColor = System.Drawing.Color.AliceBlue;
            this.txtb_Password.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtb_Password.Location = new System.Drawing.Point(17, 219);
            this.txtb_Password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtb_Password.Name = "txtb_Password";
            this.txtb_Password.Size = new System.Drawing.Size(634, 36);
            this.txtb_Password.TabIndex = 156;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(13, 427);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 23);
            this.label2.TabIndex = 157;
            this.label2.Text = "Turn on Sync?";
            // 
            // ckbSync
            // 
            this.ckbSync.AutoSize = true;
            this.ckbSync.Location = new System.Drawing.Point(17, 467);
            this.ckbSync.Name = "ckbSync";
            this.ckbSync.Size = new System.Drawing.Size(131, 27);
            this.ckbSync.TabIndex = 158;
            this.ckbSync.Text = "Sync On/Off";
            this.ckbSync.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(13, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 159;
            this.label3.Text = "Server IP";
            // 
            // txtb_ServerIp
            // 
            this.txtb_ServerIp.BackColor = System.Drawing.Color.AliceBlue;
            this.txtb_ServerIp.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtb_ServerIp.Location = new System.Drawing.Point(17, 51);
            this.txtb_ServerIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtb_ServerIp.Name = "txtb_ServerIp";
            this.txtb_ServerIp.Size = new System.Drawing.Size(634, 36);
            this.txtb_ServerIp.TabIndex = 160;
            // 
            // FormSyncData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(676, 501);
            this.ControlBox = false;
            this.Controls.Add(this.txtb_ServerIp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ckbSync);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtb_Password);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSyncCancel);
            this.Controls.Add(this.btnSaveSync);
            this.Controls.Add(this.txtb_Username);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormSyncData";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create New Field ";
            this.Load += new System.EventHandler(this.FormSyncData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtb_Username;
        private System.Windows.Forms.Button btnSaveSync;
        private System.Windows.Forms.Button btnSyncCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtb_Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckbSync;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtb_ServerIp;
    }
}