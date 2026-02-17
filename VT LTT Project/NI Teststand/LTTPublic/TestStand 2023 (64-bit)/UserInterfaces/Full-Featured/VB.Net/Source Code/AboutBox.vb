Imports NationalInstruments.TestStand.Interop.API
Imports NationalInstruments.TestStand.Utility

Public Class AboutBox
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	<CLSCompliant(False)> _
	Public Sub New(ByVal localizer As Localizer)
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		' localize the strings on the about box

		' for strings that are different when we are an editor
		If localizer.ApplicationMgr.IsEditor Then
			localizer.LocalizeForm(Me, "TSUI_OI_EDITOR_ABOUT_BOX", False)
		End If

		' for strings that are the same regardless of editor mode
		localizer.LocalizeForm(Me, "TSUI_OI_ABOUT_BOX", False)

		' add the version strings
		Me.versionLabel.Caption += " "
        Me.versionLabel.Caption += "<YEAR> (<MAJOR>.<MINOR>.<PATCH>.<BUILD>)" '  <--- YOUR VERSION HERE.  (This is the version displayed in the about box.)
       	Me.engineVersionLabel.Caption += " " + localizer.Engine.VersionString

		' add license description
		Me.licenseLabel.Caption += localizer.Engine.GetLicenseDescription(0)
	End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents pictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents versionLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents copyrightLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents companyLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents licenseLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents engineVersionLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents evironmentLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents yourLogoLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutBox))
        Me.pictureBox2 = New System.Windows.Forms.PictureBox()
        Me.versionLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.copyrightLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.companyLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.licenseLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.engineVersionLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.evironmentLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.yourLogoLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        Me.okButton = New System.Windows.Forms.Button()
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.versionLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.copyrightLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.companyLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.licenseLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.engineVersionLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.evironmentLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.yourLogoLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictureBox2
        '
        Me.pictureBox2.BackColor = System.Drawing.Color.White
        Me.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pictureBox2.Location = New System.Drawing.Point(0, 0)
        Me.pictureBox2.Name = "pictureBox2"
        Me.pictureBox2.Size = New System.Drawing.Size(600, 296)
        Me.pictureBox2.TabIndex = 19
        Me.pictureBox2.TabStop = False
        '
        'versionLabel
        '
        Me.versionLabel.Location = New System.Drawing.Point(262, 130)
        Me.versionLabel.Name = "versionLabel"
        Me.versionLabel.OcxState = CType(resources.GetObject("versionLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.versionLabel.Size = New System.Drawing.Size(78, 13)
        Me.versionLabel.TabIndex = 3
        Me.versionLabel.TabStop = False
        '
        'copyrightLabel
        '
        Me.copyrightLabel.Location = New System.Drawing.Point(255, 230)
        Me.copyrightLabel.Name = "copyrightLabel"
        Me.copyrightLabel.OcxState = CType(resources.GetObject("copyrightLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.copyrightLabel.Size = New System.Drawing.Size(93, 13)
        Me.copyrightLabel.TabIndex = 7
        Me.copyrightLabel.TabStop = False
        '
        'companyLabel
        '
        Me.companyLabel.Location = New System.Drawing.Point(260, 205)
        Me.companyLabel.Name = "companyLabel"
        Me.companyLabel.OcxState = CType(resources.GetObject("companyLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.companyLabel.Size = New System.Drawing.Size(83, 13)
        Me.companyLabel.TabIndex = 6
        Me.companyLabel.TabStop = False
        '
        'licenseLabel
        '
        Me.licenseLabel.Location = New System.Drawing.Point(264, 180)
        Me.licenseLabel.Name = "licenseLabel"
        Me.licenseLabel.OcxState = CType(resources.GetObject("licenseLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.licenseLabel.Size = New System.Drawing.Size(75, 13)
        Me.licenseLabel.TabIndex = 5
        Me.licenseLabel.TabStop = False
        '
        'engineVersionLabel
        '
        Me.engineVersionLabel.Location = New System.Drawing.Point(239, 155)
        Me.engineVersionLabel.Name = "engineVersionLabel"
        Me.engineVersionLabel.OcxState = CType(resources.GetObject("engineVersionLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.engineVersionLabel.Size = New System.Drawing.Size(125, 13)
        Me.engineVersionLabel.TabIndex = 4
        Me.engineVersionLabel.TabStop = False
        '
        'evironmentLabel
        '
        Me.evironmentLabel.Location = New System.Drawing.Point(249, 105)
        Me.evironmentLabel.Name = "evironmentLabel"
        Me.evironmentLabel.OcxState = CType(resources.GetObject("evironmentLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.evironmentLabel.Size = New System.Drawing.Size(104, 13)
        Me.evironmentLabel.TabIndex = 2
        Me.evironmentLabel.TabStop = False
        '
        'yourLogoLabel
        '
        Me.yourLogoLabel.Location = New System.Drawing.Point(396, 11)
        Me.yourLogoLabel.Name = "yourLogoLabel"
        Me.yourLogoLabel.OcxState = CType(resources.GetObject("yourLogoLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.yourLogoLabel.Size = New System.Drawing.Size(103, 13)
        Me.yourLogoLabel.TabIndex = 1
        Me.yourLogoLabel.TabStop = False
        '
        'pictureBox1
        '
        Me.pictureBox1.Image = CType(resources.GetObject("pictureBox1.Image"), System.Drawing.Image)
        Me.pictureBox1.Location = New System.Drawing.Point(11, 26)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(578, 59)
        Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pictureBox1.TabIndex = 20
        Me.pictureBox1.TabStop = False
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.okButton.Location = New System.Drawing.Point(263, 261)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 21
        Me.okButton.Text = "OK_BTN"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'AboutBox
        '
        Me.AcceptButton = Me.okButton
        Me.CancelButton = Me.okButton
        Me.ClientSize = New System.Drawing.Size(600, 296)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.versionLabel)
        Me.Controls.Add(Me.copyrightLabel)
        Me.Controls.Add(Me.companyLabel)
        Me.Controls.Add(Me.licenseLabel)
        Me.Controls.Add(Me.engineVersionLabel)
        Me.Controls.Add(Me.evironmentLabel)
        Me.Controls.Add(Me.yourLogoLabel)
        Me.Controls.Add(Me.pictureBox1)
        Me.Controls.Add(Me.pictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "AboutBox"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.versionLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.copyrightLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.companyLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.licenseLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.engineVersionLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.evironmentLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.yourLogoLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
End Class
