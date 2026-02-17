using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NationalInstruments.TestStand.Interop.API;
using NationalInstruments.TestStand.Utility;

namespace TestExec
{
	/// <summary>
	/// Summary description for AboutBox.
	/// </summary>
	public class AboutBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel yourLogoLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel evironmentLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel versionLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel engineVersionLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel licenseLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel companyLabel;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel copyrightLabel;
        private Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutBox(Localizer localizer)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
	
			// localize the strings on the about box
	
			// for strings that are different when we are an editor
			if (localizer.ApplicationMgr.IsEditor) 
				localizer.LocalizeForm(this, "TSUI_OI_EDITOR_ABOUT_BOX", false); 	

			// for strings that are the same regardless of editor mode
			localizer.LocalizeForm(this, "TSUI_OI_ABOUT_BOX", false); 	
				
			// add the version strings
			this.versionLabel.Caption += " ";
			this.versionLabel.Caption += "<YEAR> (<MAJOR>.<MINOR>.<PATCH>.<BUILD>)"; // <--- YOUR VERSION HERE.  (This is the version displayed in the about box.)
			this.engineVersionLabel.Caption += " " + localizer.Engine.VersionString;	
			
			// add license description
			this.licenseLabel.Caption += localizer.Engine.GetLicenseDescription(0);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.yourLogoLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.evironmentLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.versionLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.engineVersionLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.licenseLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.companyLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.copyrightLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.okButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.yourLogoLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.evironmentLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.versionLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.engineVersionLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.companyLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(10, 17);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(578, 59);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.White;
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox2.Location = new System.Drawing.Point(0, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(600, 288);
			this.pictureBox2.TabIndex = 9;
			this.pictureBox2.TabStop = false;
			// 
			// yourLogoLabel
			// 
			this.yourLogoLabel.Location = new System.Drawing.Point(428, 2);
			this.yourLogoLabel.Name = "yourLogoLabel";
			this.yourLogoLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("yourLogoLabel.OcxState")));
			this.yourLogoLabel.Size = new System.Drawing.Size(103, 13);
			this.yourLogoLabel.TabIndex = 1;
			this.yourLogoLabel.TabStop = false;
			// 
			// evironmentLabel
			// 
			this.evironmentLabel.Location = new System.Drawing.Point(233, 96);
			this.evironmentLabel.Name = "evironmentLabel";
			this.evironmentLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("evironmentLabel.OcxState")));
			this.evironmentLabel.Size = new System.Drawing.Size(134, 13);
			this.evironmentLabel.TabIndex = 2;
			this.evironmentLabel.TabStop = false;
			// 
			// versionLabel
			// 
			this.versionLabel.Location = new System.Drawing.Point(261, 121);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("versionLabel.OcxState")));
			this.versionLabel.Size = new System.Drawing.Size(78, 13);
			this.versionLabel.TabIndex = 3;
			this.versionLabel.TabStop = false;
			// 
			// engineVersionLabel
			// 
			this.engineVersionLabel.Location = new System.Drawing.Point(238, 146);
			this.engineVersionLabel.Name = "engineVersionLabel";
			this.engineVersionLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("engineVersionLabel.OcxState")));
			this.engineVersionLabel.Size = new System.Drawing.Size(125, 13);
			this.engineVersionLabel.TabIndex = 4;
			this.engineVersionLabel.TabStop = false;
			// 
			// licenseLabel
			// 
			this.licenseLabel.Location = new System.Drawing.Point(263, 171);
			this.licenseLabel.Name = "licenseLabel";
			this.licenseLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("licenseLabel.OcxState")));
			this.licenseLabel.Size = new System.Drawing.Size(75, 13);
			this.licenseLabel.TabIndex = 5;
			this.licenseLabel.TabStop = false;
			// 
			// companyLabel
			// 
			this.companyLabel.Location = new System.Drawing.Point(259, 196);
			this.companyLabel.Name = "companyLabel";
			this.companyLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("companyLabel.OcxState")));
			this.companyLabel.Size = new System.Drawing.Size(83, 13);
			this.companyLabel.TabIndex = 6;
			this.companyLabel.TabStop = false;
			// 
			// copyrightLabel
			// 
			this.copyrightLabel.Location = new System.Drawing.Point(254, 221);
			this.copyrightLabel.Name = "copyrightLabel";
			this.copyrightLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("copyrightLabel.OcxState")));
			this.copyrightLabel.Size = new System.Drawing.Size(93, 13);
			this.copyrightLabel.TabIndex = 7;
			this.copyrightLabel.TabStop = false;
			// 
			// okButton
			// 
			this.okButton.AutoSize = true;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(262, 252);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(66, 23);
			this.okButton.TabIndex = 10;
			this.okButton.Text = "OK_BTN";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// AboutBox
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(600, 288);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.copyrightLabel);
			this.Controls.Add(this.companyLabel);
			this.Controls.Add(this.licenseLabel);
			this.Controls.Add(this.engineVersionLabel);
			this.Controls.Add(this.evironmentLabel);
			this.Controls.Add(this.yourLogoLabel);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.pictureBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AboutBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.yourLogoLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.evironmentLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.versionLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.engineVersionLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.companyLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
