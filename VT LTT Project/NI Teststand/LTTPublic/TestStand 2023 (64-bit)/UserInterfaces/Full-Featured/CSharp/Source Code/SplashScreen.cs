using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using NationalInstruments.TestStand.Utility;

namespace TestExec
{
	/// <summary>
	/// Summary description for SplashScreen.
	/// </summary>
	public class SplashScreen : Form
	{
		private PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public SplashScreen()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			pictureBox1.Image = TSHelper.GetTestStandSplashImage();

			Show();
			Update();
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
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(770, 442);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// SplashScreen
			// 
			this.AutoScaleMode = AutoScaleMode.None;
			this.ClientSize = new Size(770, 442);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterScreen;
			((ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
