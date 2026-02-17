' Note:	    This application has a manifest file in the project. This manifest file includes the Microsoft.Windows.Common-Controls which 
'			enables the application to display controls using the XP theme that the operating system selects.
'			A post build event embeds this manifest file into the executable.
'			In order for the manifest file to enable the executable to display with the XP theme:
'			1. The manifest file must have the same name as the executable. For example, if your executable is named MyExecutable.exe, your manifest file is required to have the name MyExecutable.exe.manifest.
'			2. The manifest file must include the Microsoft.Windows.Common-Controls.
'			3. The manifest file must reside in the same directory as the executable.
'			Also note that if you enable the Project Properties>>Debug>>Enable Visual Studio Hosting Process option, the XP theme adaption does not occur when debugging the executable
'			because the Visual Studio environment creates the process and does not allow the manifest file to be embedded into the executable.

' Note:	    TestStand installs the source code files for the default user interfaces in the <TestStand>\UserInterfaces and <TestStand Public>\UserInterfaces directories. 
'			To modify the installed user interfaces or to create new user interfaces, modify the files in the <TestStand Public>\UserInterfaces directory. 
'			You can use the read-only source files for the default user interfaces in the <TestStand>\UserInterfaces directory as a reference. 
'			National Instruments recommends that you track the changes you make to the user interface source code files so you can integrate the changes with any enhancements in future versions of the TestStand User Interfaces.

' TestStand Core API 
Imports NationalInstruments.TestStand.Interop.API

' TestStand User Interface Controls
Imports NationalInstruments.TestStand.Interop.UI
Imports NationalInstruments.TestStand.Interop.UI.Support

' .net specific functions for use with TestStand APIs (TSUtil)
Imports NationalInstruments.TestStand.Utility


Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private Const WM_QUERYENDSESSION As Integer = &H11

    ' flag that will be set to true if the user tries to shut down windows
    Private sessionEnding As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents axFilesComboBox As NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox
    Friend WithEvents sequenceFileLabel As System.Windows.Forms.Label
    Friend WithEvents axOpenFileButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents sequenceLabel As System.Windows.Forms.Label
    Friend WithEvents axSequencesComboBox As NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox
    Friend WithEvents axCloseFileButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axEntryPoint1Button As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axEntryPoint2Button As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axSequenceView As NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView
    Friend WithEvents axReportView As NationalInstruments.TestStand.Interop.UI.Ax.AxReportView
    Friend WithEvents executionLabel As System.Windows.Forms.Label
    Friend WithEvents axCloseExecutionButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axExecutionsComboBox As NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox
    Friend WithEvents axSequenceFileViewMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr
    Friend WithEvents axExecutionViewMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr
    Friend WithEvents axApplicationMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr
    Friend WithEvents gcTimer As System.Timers.Timer
    Friend WithEvents axRunSelectedButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axLoginLogoutButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axTerminateRestartButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axBreakResumeButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axTerminateAllButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents axExitButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.axFilesComboBox = New NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox()
        Me.sequenceFileLabel = New System.Windows.Forms.Label()
        Me.axOpenFileButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.sequenceLabel = New System.Windows.Forms.Label()
        Me.axSequencesComboBox = New NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox()
        Me.axCloseFileButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axEntryPoint1Button = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axEntryPoint2Button = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axRunSelectedButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axSequenceView = New NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView()
        Me.axLoginLogoutButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axTerminateRestartButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axBreakResumeButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axReportView = New NationalInstruments.TestStand.Interop.UI.Ax.AxReportView()
        Me.executionLabel = New System.Windows.Forms.Label()
        Me.axCloseExecutionButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axExecutionsComboBox = New NationalInstruments.TestStand.Interop.UI.Ax.AxComboBox()
        Me.axSequenceFileViewMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr()
        Me.axExecutionViewMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr()
        Me.axApplicationMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr()
        Me.gcTimer = New System.Timers.Timer()
        Me.axExitButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.axTerminateAllButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        CType(Me.axFilesComboBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axOpenFileButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axSequencesComboBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axCloseFileButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axEntryPoint1Button, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axEntryPoint2Button, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axRunSelectedButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axSequenceView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axLoginLogoutButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axTerminateRestartButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axBreakResumeButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axReportView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axCloseExecutionButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axExecutionsComboBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axSequenceFileViewMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axExecutionViewMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axApplicationMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gcTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axExitButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.axTerminateAllButton, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'axFilesComboBox
        '
        Me.axFilesComboBox.Location = New System.Drawing.Point(108, 12)
        Me.axFilesComboBox.Name = "axFilesComboBox"
        Me.axFilesComboBox.OcxState = CType(resources.GetObject("axFilesComboBox.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axFilesComboBox.Size = New System.Drawing.Size(507, 22)
        Me.axFilesComboBox.TabIndex = 1
        '
        'sequenceFileLabel
        '
        Me.sequenceFileLabel.Location = New System.Drawing.Point(6, 16)
        Me.sequenceFileLabel.Name = "sequenceFileLabel"
        Me.sequenceFileLabel.Size = New System.Drawing.Size(96, 16)
        Me.sequenceFileLabel.TabIndex = 0
        Me.sequenceFileLabel.Text = "Sequence Files:"
        '
        'axOpenFileButton
        '
        Me.axOpenFileButton.Location = New System.Drawing.Point(619, 10)
        Me.axOpenFileButton.Name = "axOpenFileButton"
        Me.axOpenFileButton.OcxState = CType(resources.GetObject("axOpenFileButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axOpenFileButton.Size = New System.Drawing.Size(167, 26)
        Me.axOpenFileButton.TabIndex = 2
        '
        'sequenceLabel
        '
        Me.sequenceLabel.Location = New System.Drawing.Point(6, 43)
        Me.sequenceLabel.Name = "sequenceLabel"
        Me.sequenceLabel.Size = New System.Drawing.Size(96, 16)
        Me.sequenceLabel.TabIndex = 3
        Me.sequenceLabel.Text = "Sequences:"
        '
        'axSequencesComboBox
        '
        Me.axSequencesComboBox.Location = New System.Drawing.Point(108, 40)
        Me.axSequencesComboBox.Name = "axSequencesComboBox"
        Me.axSequencesComboBox.OcxState = CType(resources.GetObject("axSequencesComboBox.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axSequencesComboBox.Size = New System.Drawing.Size(507, 22)
        Me.axSequencesComboBox.TabIndex = 4
        '
        'axCloseFileButton
        '
        Me.axCloseFileButton.Location = New System.Drawing.Point(619, 38)
        Me.axCloseFileButton.Name = "axCloseFileButton"
        Me.axCloseFileButton.OcxState = CType(resources.GetObject("axCloseFileButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axCloseFileButton.Size = New System.Drawing.Size(167, 26)
        Me.axCloseFileButton.TabIndex = 5
        '
        'axEntryPoint1Button
        '
        Me.axEntryPoint1Button.Location = New System.Drawing.Point(107, 67)
        Me.axEntryPoint1Button.Name = "axEntryPoint1Button"
        Me.axEntryPoint1Button.OcxState = CType(resources.GetObject("axEntryPoint1Button.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axEntryPoint1Button.Size = New System.Drawing.Size(167, 26)
        Me.axEntryPoint1Button.TabIndex = 6
        '
        'axEntryPoint2Button
        '
        Me.axEntryPoint2Button.Location = New System.Drawing.Point(278, 67)
        Me.axEntryPoint2Button.Name = "axEntryPoint2Button"
        Me.axEntryPoint2Button.OcxState = CType(resources.GetObject("axEntryPoint2Button.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axEntryPoint2Button.Size = New System.Drawing.Size(167, 26)
        Me.axEntryPoint2Button.TabIndex = 7
        '
        'axRunSelectedButton
        '
        Me.axRunSelectedButton.Location = New System.Drawing.Point(449, 67)
        Me.axRunSelectedButton.Name = "axRunSelectedButton"
        Me.axRunSelectedButton.OcxState = CType(resources.GetObject("axRunSelectedButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axRunSelectedButton.Size = New System.Drawing.Size(167, 26)
        Me.axRunSelectedButton.TabIndex = 8
        '
        'axSequenceView
        '
        Me.axSequenceView.Enabled = True
        Me.axSequenceView.Location = New System.Drawing.Point(8, 129)
        Me.axSequenceView.Name = "axSequenceView"
        Me.axSequenceView.OcxState = CType(resources.GetObject("axSequenceView.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axSequenceView.Size = New System.Drawing.Size(778, 203)
        Me.axSequenceView.TabIndex = 12
        '
        'axLoginLogoutButton
        '
        Me.axLoginLogoutButton.Location = New System.Drawing.Point(448, 571)
        Me.axLoginLogoutButton.Name = "axLoginLogoutButton"
        Me.axLoginLogoutButton.OcxState = CType(resources.GetObject("axLoginLogoutButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axLoginLogoutButton.Size = New System.Drawing.Size(167, 26)
        Me.axLoginLogoutButton.TabIndex = 20
        '
        'axTerminateRestartButton
        '
        Me.axTerminateRestartButton.Location = New System.Drawing.Point(178, 335)
        Me.axTerminateRestartButton.Name = "axTerminateRestartButton"
        Me.axTerminateRestartButton.OcxState = CType(resources.GetObject("axTerminateRestartButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axTerminateRestartButton.Size = New System.Drawing.Size(167, 26)
        Me.axTerminateRestartButton.TabIndex = 14
        '
        'axBreakResumeButton
        '
        Me.axBreakResumeButton.Location = New System.Drawing.Point(8, 335)
        Me.axBreakResumeButton.Name = "axBreakResumeButton"
        Me.axBreakResumeButton.OcxState = CType(resources.GetObject("axBreakResumeButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axBreakResumeButton.Size = New System.Drawing.Size(167, 26)
        Me.axBreakResumeButton.TabIndex = 13
        '
        'axReportView
        '
        Me.axReportView.Enabled = True
        Me.axReportView.Location = New System.Drawing.Point(8, 364)
        Me.axReportView.Name = "axReportView"
        Me.axReportView.OcxState = CType(resources.GetObject("axReportView.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axReportView.Size = New System.Drawing.Size(778, 203)
        Me.axReportView.TabIndex = 19
        '
        'executionLabel
        '
        Me.executionLabel.Location = New System.Drawing.Point(6, 103)
        Me.executionLabel.Name = "executionLabel"
        Me.executionLabel.Size = New System.Drawing.Size(96, 16)
        Me.executionLabel.TabIndex = 9
        Me.executionLabel.Text = "Executions:"
        '
        'axCloseExecutionButton
        '
        Me.axCloseExecutionButton.Location = New System.Drawing.Point(619, 97)
        Me.axCloseExecutionButton.Name = "axCloseExecutionButton"
        Me.axCloseExecutionButton.OcxState = CType(resources.GetObject("axCloseExecutionButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axCloseExecutionButton.Size = New System.Drawing.Size(167, 26)
        Me.axCloseExecutionButton.TabIndex = 11
        '
        'axExecutionsComboBox
        '
        Me.axExecutionsComboBox.Location = New System.Drawing.Point(108, 99)
        Me.axExecutionsComboBox.Name = "axExecutionsComboBox"
        Me.axExecutionsComboBox.OcxState = CType(resources.GetObject("axExecutionsComboBox.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axExecutionsComboBox.Size = New System.Drawing.Size(507, 22)
        Me.axExecutionsComboBox.TabIndex = 10
        '
        'axSequenceFileViewMgr
        '
        Me.axSequenceFileViewMgr.Enabled = True
        Me.axSequenceFileViewMgr.Location = New System.Drawing.Point(712, 328)
        Me.axSequenceFileViewMgr.Name = "axSequenceFileViewMgr"
        Me.axSequenceFileViewMgr.OcxState = CType(resources.GetObject("axSequenceFileViewMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axSequenceFileViewMgr.Size = New System.Drawing.Size(32, 32)
        Me.axSequenceFileViewMgr.TabIndex = 17
        '
        'axExecutionViewMgr
        '
        Me.axExecutionViewMgr.Enabled = True
        Me.axExecutionViewMgr.Location = New System.Drawing.Point(750, 328)
        Me.axExecutionViewMgr.Name = "axExecutionViewMgr"
        Me.axExecutionViewMgr.OcxState = CType(resources.GetObject("axExecutionViewMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axExecutionViewMgr.Size = New System.Drawing.Size(32, 32)
        Me.axExecutionViewMgr.TabIndex = 18
        '
        'axApplicationMgr
        '
        Me.axApplicationMgr.Enabled = True
        Me.axApplicationMgr.Location = New System.Drawing.Point(674, 328)
        Me.axApplicationMgr.Name = "axApplicationMgr"
        Me.axApplicationMgr.OcxState = CType(resources.GetObject("axApplicationMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axApplicationMgr.Size = New System.Drawing.Size(32, 32)
        Me.axApplicationMgr.TabIndex = 16
        '
        'gcTimer
        '
        Me.gcTimer.Interval = 3000.0R
        Me.gcTimer.SynchronizingObject = Me
        '
        'axExitButton
        '
        Me.axExitButton.Location = New System.Drawing.Point(619, 570)
        Me.axExitButton.Name = "axExitButton"
        Me.axExitButton.OcxState = CType(resources.GetObject("axExitButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axExitButton.Size = New System.Drawing.Size(167, 26)
        Me.axExitButton.TabIndex = 21
        '
        'axTerminateAllButton
        '
        Me.axTerminateAllButton.Location = New System.Drawing.Point(348, 335)
        Me.axTerminateAllButton.Name = "axTerminateAllButton"
        Me.axTerminateAllButton.OcxState = CType(resources.GetObject("axTerminateAllButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.axTerminateAllButton.Size = New System.Drawing.Size(167, 26)
        Me.axTerminateAllButton.TabIndex = 15
        '
        'MainForm
        '
        Me.ClientSize = New System.Drawing.Size(794, 604)
        Me.Controls.Add(Me.axTerminateAllButton)
        Me.Controls.Add(Me.axExitButton)
        Me.Controls.Add(Me.axSequenceView)
        Me.Controls.Add(Me.axLoginLogoutButton)
        Me.Controls.Add(Me.axTerminateRestartButton)
        Me.Controls.Add(Me.axBreakResumeButton)
        Me.Controls.Add(Me.axReportView)
        Me.Controls.Add(Me.executionLabel)
        Me.Controls.Add(Me.axExecutionsComboBox)
        Me.Controls.Add(Me.axSequenceFileViewMgr)
        Me.Controls.Add(Me.axExecutionViewMgr)
        Me.Controls.Add(Me.axApplicationMgr)
        Me.Controls.Add(Me.axRunSelectedButton)
        Me.Controls.Add(Me.axEntryPoint2Button)
        Me.Controls.Add(Me.axEntryPoint1Button)
        Me.Controls.Add(Me.axCloseFileButton)
        Me.Controls.Add(Me.sequenceLabel)
        Me.Controls.Add(Me.axSequencesComboBox)
        Me.Controls.Add(Me.axOpenFileButton)
        Me.Controls.Add(Me.sequenceFileLabel)
        Me.Controls.Add(Me.axCloseExecutionButton)
        Me.Controls.Add(Me.axFilesComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Simple Test Executive Operator Interface Example"
        CType(Me.axFilesComboBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axOpenFileButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axSequencesComboBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axCloseFileButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axEntryPoint1Button, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axEntryPoint2Button, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axRunSelectedButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axSequenceView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axLoginLogoutButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axTerminateRestartButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axBreakResumeButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axReportView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axCloseExecutionButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axExecutionsComboBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axSequenceFileViewMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axExecutionViewMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axApplicationMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gcTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axExitButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.axTerminateAllButton, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim threading As STAThreadAttribute
    Public Shared Sub Main(ByVal args As String())
        LaunchTestStandApplicationInNewDomain.LaunchProtected(New LaunchTestStandApplicationInNewDomain.MainEntryPointDelegateWithArgs(AddressOf MainEntryPoint), args, "TestStand Visual Basic UI", New LaunchTestStandApplicationInNewDomain.DisplayErrorMessageDelegate(AddressOf DisplayError), True)
    End Sub

	Public Shared Sub DisplayError(ByVal caption As String, ByVal message As String)
        MessageBox.Show(message, caption)
    End Sub
	
    Public Shared Sub MainEntryPoint(ByVal args As String())
        NationalInstruments.TestStand.Utility.ApplicationWrapper.Run(New MainForm())
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' If this UI is running in a CLR other than the one TestStand uses,
            ' then it needs its own GCTimer for that version of the CLR. If it's running in the
            ' same CLR as TestStand then the engine's gctimer enabled by the ApplicationMgr
            ' is sufficient. See the API help for Engine.DotNetGarbageCollectionInterval for more details.
            If (System.Environment.Version.ToString() <> axApplicationMgr.GetEngine().DotNetCLRVersion) Then
                Me.gcTimer.Enabled = True
            End If

            ' connect TestStand comboboxes 
            axSequenceFileViewMgr.ConnectSequenceFileList(axFilesComboBox, True)
            axSequenceFileViewMgr.ConnectSequenceList(axSequencesComboBox)

            ' specify what information to display in each execution list combobox entry (the expression string looks extra complicated here because we have to escape the quotes for the VB.net compiler.)
            axExecutionViewMgr.ConnectExecutionList(axExecutionsComboBox).DisplayExpression = """%CurrentExecution% - "" + (""%UUTSerialNumber%"" == """" ? """" : (ResStr(""TSUI_OI_MAIN_PANEL"",""SERIAL_NUMBER"") + "" %UUTSerialNumber% - "")) + (""%TestSocketIndex%"" == """" ? """" : (ResStr(""TSUI_OI_MAIN_PANEL"",""SOCKET_NUMBER"") + "" %TestSocketIndex% - "")) + ""%ModelState%"""

            ' connect sequence view to execution view manager									  
            axExecutionViewMgr.ConnectExecutionView(axSequenceView, ExecutionViewOptions.ExecutionViewConnection_NoOptions)

            ' connect report view to execution view manager									  
            axExecutionViewMgr.ConnectReportView(axReportView)

            ' connect TestStand buttons to commands
            axApplicationMgr.ConnectCommand(axTerminateAllButton, CommandKinds.CommandKind_TerminateAll, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axApplicationMgr.ConnectCommand(axLoginLogoutButton, CommandKinds.CommandKind_LoginLogout, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axApplicationMgr.ConnectCommand(axExitButton, CommandKinds.CommandKind_Exit, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axSequenceFileViewMgr.ConnectCommand(axOpenFileButton, CommandKinds.CommandKind_OpenSequenceFiles, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axSequenceFileViewMgr.ConnectCommand(axCloseFileButton, CommandKinds.CommandKind_Close, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axSequenceFileViewMgr.ConnectCommand(axEntryPoint1Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axSequenceFileViewMgr.ConnectCommand(axEntryPoint2Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 1, CommandConnectionOptions.CommandConnection_EnableImage)
            axSequenceFileViewMgr.ConnectCommand(axRunSelectedButton, CommandKinds.CommandKind_RunCurrentSequence, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axExecutionViewMgr.ConnectCommand(axCloseExecutionButton, CommandKinds.CommandKind_Close, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axExecutionViewMgr.ConnectCommand(axBreakResumeButton, CommandKinds.CommandKind_BreakResume, 0, CommandConnectionOptions.CommandConnection_EnableImage)
            axExecutionViewMgr.ConnectCommand(axTerminateRestartButton, CommandKinds.CommandKind_TerminateRestart, 0, CommandConnectionOptions.CommandConnection_EnableImage)

            ' show all step groups at once in the sequence view
            axExecutionViewMgr.StepGroupMode = StepGroupModes.StepGroupMode_AllGroups

            axApplicationMgr.Start() ' start up the TestStand User Interface Components. this also logs in the user
        Catch theException As Exception
            MessageBox.Show(Me, theException.Message, "Error")
            Application.Exit()
        End Try
    End Sub

    ' handle request to close form (via Windows close box, for example)
    Private Sub MainForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        ' Don't set e.Cancel to True if windows is shutting down.
        ' Doing so would prevent windows from shutting down or logging out.
        If Not sessionEnding Then
            ' initiate shutdown and cancel close if shutdown is not complete.  The applicationMgr will
            ' send the ExitApplication event when shutdown is complete and we can close then
            If (axApplicationMgr.Shutdown() = False) Then
                e.Cancel = True
            End If
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_QUERYENDSESSION Then
            sessionEnding = True
            Application.Exit()
        End If

        MyBase.WndProc(m)
    End Sub

    ' It is now ok to exit, close the form
    Private Sub axApplicationMgr_ExitApplication(ByVal sender As Object, ByVal e As System.EventArgs) Handles axApplicationMgr.ExitApplication
		Environment.ExitCode = Me.axApplicationMgr.ExitCode
		Close()

        TSHelper.DoSynchronousGCForCOMObjectDestruction() ' force .net garbage collection to ensure ensure all TestStand objects are freed before the TestStand engine unloads 							
    End Sub

    ' ApplicationMgr sends this event to handle any errors it detects.  For example, if a TestStand menu command
    ' generates an error, this handler displays it
    Private Sub axApplicationMgr_ReportError(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_ReportErrorEvent) Handles axApplicationMgr.ReportError

        axApplicationMgr.GetEngine().DisplayErrorDialog("Error", e.errorMessage, e.errorCode, CommonDialogOptions.CommonDlgOption_DisableGotoLocation)

    End Sub

    ' the ApplicationMgr sends this event to request that the UI display a particular execution
    Private Sub axApplicationMgr_DisplayExecution(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayExecutionEvent) Handles axApplicationMgr.DisplayExecution
        ' bring application to front if we hit a breakpoint
        If (e.reason = ExecutionDisplayReasons.ExecutionDisplayReason_Breakpoint) Or (e.reason = ExecutionDisplayReasons.ExecutionDisplayReason_BreakOnRunTimeError) Then
            Me.Activate()
        End If

        axExecutionViewMgr.Execution = e.exec
    End Sub

    ' the ApplicationMgr sends this event to request that the UI display a particular sequence file
    Private Sub axApplicationMgr_DisplaySequenceFile(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplaySequenceFileEvent) Handles axApplicationMgr.DisplaySequenceFile
        axSequenceFileViewMgr.SequenceFile = e.file
    End Sub

    ' Release all objects periodically.  .NET lets COM objects pile up on the managed heap, seemingly even objects you don't know about such
    ' as parameters to unhandled ActiveX events.  This timer ensures that all COM objects are released in a timely manner,
    ' thus preventing the performance hiccup that could occur when .NET finally decides to collect garbage. Also, this timer
    ' ensures that actions triggered by object destruction run in a timely manner. For example: sequence file unload callbacks.
    Private Sub gcTimer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles gcTimer.Elapsed
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, False) ' force .net garbage collection		
    End Sub





End Class
