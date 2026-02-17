' Note:	    This application has a manifest file in the project. This manifest file includes the Microsoft.Windows.Common-Controls which 
'			enables the application to display controls using the XP theme that the operating system selects.
'			A post build event embeds this manifest file into the executable.
'			In order for the manifest file to enable the executable to display with the XP theme:
'			1. The manifest file must have the same name as the executable. For example, if your executable is named MyExecutable.exe, your manifest file is required to have the name MyExecutable.exe.manifest.
'			2. The manifest file must include the Microsoft.Windows.Common-Controls.
'			3. The manifest file must reside in the same directory as the executable.
'			Also note that if you enable the Project Properties>>Debug>>Enable Visual Studio Hosting Process option, the XP theme adaption does not occur when debugging the executable
'			because the Visual Studio environment creates the process and does not allow the manifest file to be embedded into the executable.

' Note:	    This example can function as an editor or an operator interface. The user can change the edit mode with a keystroke (ctrl-alt-shift-insert) or with a command line 
'		    argument. For more information and for instructions on how prevent the user from changing the edit mode, refer to the TestStand Reference Manual>>Creating Custom 
'		    User Interfaces>>Editor versus Operator Interface Applications>>Creating Editor Applications.

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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        ' VB.NET Workaround: must disable the tabControl_SelectedIndexChanged during initialization because otherwise the event handler will try to access uninitialized controls.
        ' C# does not have this problem because it installs event handlers after it sets up a control, thus initialization doesn't generate events.
        programmaticallyUpdatingTabPages = True
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        programmaticallyUpdatingTabPages = False
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
    Friend WithEvents AxApplicationMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr
    Friend WithEvents AxSequenceFileViewMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr
    Friend WithEvents AxExecutionViewMgr As NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr
    Friend WithEvents configureMenu As System.Windows.Forms.MenuItem
    Friend WithEvents fileMenu As System.Windows.Forms.MenuItem
    Friend WithEvents executeMenu As System.Windows.Forms.MenuItem
    Friend WithEvents helpMenu As System.Windows.Forms.MenuItem
    Friend WithEvents aboutBoxItem As System.Windows.Forms.MenuItem
    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents fileTab As System.Windows.Forms.TabPage
    Friend WithEvents executionTab As System.Windows.Forms.TabPage
    Friend WithEvents reportTab As System.Windows.Forms.TabPage
    Friend WithEvents GCTimer As System.Windows.Forms.Timer
    Friend WithEvents debugMenu As System.Windows.Forms.MenuItem
    Friend WithEvents toolsMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents AxListBar As NationalInstruments.TestStand.Interop.UI.Ax.AxListBar
    Friend WithEvents AxStatusBar As NationalInstruments.TestStand.Interop.UI.Ax.AxStatusBar
    Friend WithEvents AxReportView As NationalInstruments.TestStand.Interop.UI.Ax.AxReportView
    Friend WithEvents AxExecutionSteps As NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView
    Friend WithEvents AxBreakResumeBtn As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents AxTerminateRestartBtn As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents AxEntryPoint1Button As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents AxEntryPoint2Button As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Friend WithEvents AxFileSteps As NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView
    Friend WithEvents AxSequencesList As NationalInstruments.TestStand.Interop.UI.Ax.AxListBox
    Private WithEvents AxSequenceFileLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Private WithEvents AxRunSequenceButton As NationalInstruments.TestStand.Interop.UI.Ax.AxButton
    Private WithEvents AxExecutionLabel As NationalInstruments.TestStand.Interop.UI.Ax.AxLabel
    Private WithEvents AxThreads As NationalInstruments.TestStand.Interop.UI.Ax.AxListBox
    Private WithEvents AxCallStack As NationalInstruments.TestStand.Interop.UI.Ax.AxListBox
    Friend WithEvents AxFileVariables As NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView
    Friend WithEvents AxExecutionVariables As NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView
    Friend WithEvents AxInsertionPalette As NationalInstruments.TestStand.Interop.UI.Ax.AxInsertionPalette
    Friend WithEvents editMenu As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.AxApplicationMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr()
        Me.AxSequenceFileViewMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr()
        Me.AxExecutionViewMgr = New NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr()
        Me.AxFileSteps = New NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView()
        Me.AxEntryPoint2Button = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.AxEntryPoint1Button = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.AxTerminateRestartBtn = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.AxBreakResumeBtn = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.AxExecutionSteps = New NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView()
        Me.AxReportView = New NationalInstruments.TestStand.Interop.UI.Ax.AxReportView()
        Me.AxListBar = New NationalInstruments.TestStand.Interop.UI.Ax.AxListBar()
        Me.AxStatusBar = New NationalInstruments.TestStand.Interop.UI.Ax.AxStatusBar()
        Me.configureMenu = New System.Windows.Forms.MenuItem()
        Me.fileMenu = New System.Windows.Forms.MenuItem()
        Me.executeMenu = New System.Windows.Forms.MenuItem()
        Me.helpMenu = New System.Windows.Forms.MenuItem()
        Me.aboutBoxItem = New System.Windows.Forms.MenuItem()
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.fileTab = New System.Windows.Forms.TabPage()
        Me.AxInsertionPalette = New NationalInstruments.TestStand.Interop.UI.Ax.AxInsertionPalette()
        Me.AxFileVariables = New NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView()
        Me.AxSequenceFileLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.AxRunSequenceButton = New NationalInstruments.TestStand.Interop.UI.Ax.AxButton()
        Me.AxSequencesList = New NationalInstruments.TestStand.Interop.UI.Ax.AxListBox()
        Me.executionTab = New System.Windows.Forms.TabPage()
        Me.AxExecutionVariables = New NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView()
        Me.AxThreads = New NationalInstruments.TestStand.Interop.UI.Ax.AxListBox()
        Me.AxCallStack = New NationalInstruments.TestStand.Interop.UI.Ax.AxListBox()
        Me.AxExecutionLabel = New NationalInstruments.TestStand.Interop.UI.Ax.AxLabel()
        Me.reportTab = New System.Windows.Forms.TabPage()
        Me.GCTimer = New System.Windows.Forms.Timer(Me.components)
        Me.debugMenu = New System.Windows.Forms.MenuItem()
        Me.toolsMenu = New System.Windows.Forms.MenuItem()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.editMenu = New System.Windows.Forms.MenuItem()
        CType(Me.AxApplicationMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxSequenceFileViewMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxExecutionViewMgr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxFileSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxEntryPoint2Button, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxEntryPoint1Button, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxTerminateRestartBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxBreakResumeBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxExecutionSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxReportView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxListBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxStatusBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabControl.SuspendLayout()
        Me.fileTab.SuspendLayout()
        CType(Me.AxInsertionPalette, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxFileVariables, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxSequenceFileLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxRunSequenceButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxSequencesList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.executionTab.SuspendLayout()
        CType(Me.AxExecutionVariables, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxThreads, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxCallStack, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxExecutionLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.reportTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'AxApplicationMgr
        '
        Me.AxApplicationMgr.Enabled = True
        Me.AxApplicationMgr.Location = New System.Drawing.Point(528, 0)
        Me.AxApplicationMgr.Name = "AxApplicationMgr"
        Me.AxApplicationMgr.OcxState = CType(resources.GetObject("AxApplicationMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxApplicationMgr.Size = New System.Drawing.Size(32, 32)
        Me.AxApplicationMgr.TabIndex = 5
        '
        'AxSequenceFileViewMgr
        '
        Me.AxSequenceFileViewMgr.Enabled = True
        Me.AxSequenceFileViewMgr.Location = New System.Drawing.Point(584, 0)
        Me.AxSequenceFileViewMgr.Name = "AxSequenceFileViewMgr"
        Me.AxSequenceFileViewMgr.OcxState = CType(resources.GetObject("AxSequenceFileViewMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxSequenceFileViewMgr.Size = New System.Drawing.Size(32, 32)
        Me.AxSequenceFileViewMgr.TabIndex = 6
        '
        'AxExecutionViewMgr
        '
        Me.AxExecutionViewMgr.Enabled = True
        Me.AxExecutionViewMgr.Location = New System.Drawing.Point(640, 0)
        Me.AxExecutionViewMgr.Name = "AxExecutionViewMgr"
        Me.AxExecutionViewMgr.OcxState = CType(resources.GetObject("AxExecutionViewMgr.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxExecutionViewMgr.Size = New System.Drawing.Size(32, 32)
        Me.AxExecutionViewMgr.TabIndex = 2
        '
        'AxFileSteps
        '
        Me.AxFileSteps.Enabled = True
        Me.AxFileSteps.Location = New System.Drawing.Point(0, 0)
        Me.AxFileSteps.Name = "AxFileSteps"
        Me.AxFileSteps.OcxState = CType(resources.GetObject("AxFileSteps.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxFileSteps.Size = New System.Drawing.Size(493, 291)
        Me.AxFileSteps.TabIndex = 0
        '
        'AxEntryPoint2Button
        '
        Me.AxEntryPoint2Button.Location = New System.Drawing.Point(152, 407)
        Me.AxEntryPoint2Button.Name = "AxEntryPoint2Button"
        Me.AxEntryPoint2Button.OcxState = CType(resources.GetObject("AxEntryPoint2Button.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxEntryPoint2Button.Size = New System.Drawing.Size(127, 24)
        Me.AxEntryPoint2Button.TabIndex = 5
        '
        'AxEntryPoint1Button
        '
        Me.AxEntryPoint1Button.Location = New System.Drawing.Point(1, 407)
        Me.AxEntryPoint1Button.Name = "AxEntryPoint1Button"
        Me.AxEntryPoint1Button.OcxState = CType(resources.GetObject("AxEntryPoint1Button.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxEntryPoint1Button.Size = New System.Drawing.Size(127, 24)
        Me.AxEntryPoint1Button.TabIndex = 4
        '
        'AxTerminateRestartBtn
        '
        Me.AxTerminateRestartBtn.Location = New System.Drawing.Point(143, 421)
        Me.AxTerminateRestartBtn.Name = "AxTerminateRestartBtn"
        Me.AxTerminateRestartBtn.OcxState = CType(resources.GetObject("AxTerminateRestartBtn.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxTerminateRestartBtn.Size = New System.Drawing.Size(127, 24)
        Me.AxTerminateRestartBtn.TabIndex = 5
        '
        'AxBreakResumeBtn
        '
        Me.AxBreakResumeBtn.Location = New System.Drawing.Point(0, 421)
        Me.AxBreakResumeBtn.Name = "AxBreakResumeBtn"
        Me.AxBreakResumeBtn.OcxState = CType(resources.GetObject("AxBreakResumeBtn.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxBreakResumeBtn.Size = New System.Drawing.Size(127, 24)
        Me.AxBreakResumeBtn.TabIndex = 4
        '
        'AxExecutionSteps
        '
        Me.AxExecutionSteps.Enabled = True
        Me.AxExecutionSteps.Location = New System.Drawing.Point(0, 0)
        Me.AxExecutionSteps.Name = "AxExecutionSteps"
        Me.AxExecutionSteps.OcxState = CType(resources.GetObject("AxExecutionSteps.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxExecutionSteps.Size = New System.Drawing.Size(635, 297)
        Me.AxExecutionSteps.TabIndex = 0
        '
        'AxReportView
        '
        Me.AxReportView.Enabled = True
        Me.AxReportView.Location = New System.Drawing.Point(0, 0)
        Me.AxReportView.Name = "AxReportView"
        Me.AxReportView.OcxState = CType(resources.GetObject("AxReportView.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxReportView.Size = New System.Drawing.Size(635, 451)
        Me.AxReportView.TabIndex = 0
        '
        'AxListBar
        '
        Me.AxListBar.Enabled = True
        Me.AxListBar.Location = New System.Drawing.Point(0, 0)
        Me.AxListBar.Name = "AxListBar"
        Me.AxListBar.OcxState = CType(resources.GetObject("AxListBar.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxListBar.Size = New System.Drawing.Size(160, 478)
        Me.AxListBar.TabIndex = 3
        '
        'AxStatusBar
        '
        Me.AxStatusBar.Enabled = True
        Me.AxStatusBar.Location = New System.Drawing.Point(0, 475)
        Me.AxStatusBar.Name = "AxStatusBar"
        Me.AxStatusBar.OcxState = CType(resources.GetObject("AxStatusBar.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxStatusBar.Size = New System.Drawing.Size(798, 19)
        Me.AxStatusBar.TabIndex = 5
        Me.AxStatusBar.TabStop = False
        '
        'configureMenu
        '
        Me.configureMenu.Index = 4
        Me.configureMenu.Text = "CONFIGURE"
        '
        'fileMenu
        '
        Me.fileMenu.Index = 0
        Me.fileMenu.Text = "FILE"
        '
        'executeMenu
        '
        Me.executeMenu.Index = 2
        Me.executeMenu.Text = "EXECUTE"
        '
        'helpMenu
        '
        Me.helpMenu.Index = 6
        Me.helpMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.aboutBoxItem})
        Me.helpMenu.Text = "HELP"
        '
        'aboutBoxItem
        '
        Me.aboutBoxItem.Index = 0
        Me.aboutBoxItem.Text = "ABOUT"
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.fileTab)
        Me.tabControl.Controls.Add(Me.executionTab)
        Me.tabControl.Controls.Add(Me.reportTab)
        Me.tabControl.Location = New System.Drawing.Point(160, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(640, 478)
        Me.tabControl.TabIndex = 4
        '
        'fileTab
        '
        Me.fileTab.Controls.Add(Me.AxInsertionPalette)
        Me.fileTab.Controls.Add(Me.AxFileVariables)
        Me.fileTab.Controls.Add(Me.AxSequenceFileLabel)
        Me.fileTab.Controls.Add(Me.AxRunSequenceButton)
        Me.fileTab.Controls.Add(Me.AxSequencesList)
        Me.fileTab.Controls.Add(Me.AxFileSteps)
        Me.fileTab.Controls.Add(Me.AxEntryPoint2Button)
        Me.fileTab.Controls.Add(Me.AxEntryPoint1Button)
        Me.fileTab.Location = New System.Drawing.Point(4, 22)
        Me.fileTab.Name = "fileTab"
        Me.fileTab.Size = New System.Drawing.Size(632, 452)
        Me.fileTab.TabIndex = 0
        Me.fileTab.Text = "SEQUENCE_FILE"
        '
        'AxInsertionPalette
        '
        Me.AxInsertionPalette.Location = New System.Drawing.Point(493, 0)
        Me.AxInsertionPalette.Name = "AxInsertionPalette"
        Me.AxInsertionPalette.OcxState = CType(resources.GetObject("AxInsertionPalette.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxInsertionPalette.Size = New System.Drawing.Size(136, 396)
        Me.AxInsertionPalette.TabIndex = 3
        '
        'AxFileVariables
        '
        Me.AxFileVariables.Location = New System.Drawing.Point(0, 292)
        Me.AxFileVariables.Name = "AxFileVariables"
        Me.AxFileVariables.OcxState = CType(resources.GetObject("AxFileVariables.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxFileVariables.Size = New System.Drawing.Size(280, 104)
        Me.AxFileVariables.TabIndex = 1
        '
        'AxSequenceFileLabel
        '
        Me.AxSequenceFileLabel.Location = New System.Drawing.Point(-4, 393)
        Me.AxSequenceFileLabel.Name = "AxSequenceFileLabel"
        Me.AxSequenceFileLabel.OcxState = CType(resources.GetObject("AxSequenceFileLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxSequenceFileLabel.Size = New System.Drawing.Size(352, 13)
        Me.AxSequenceFileLabel.TabIndex = 12
        Me.AxSequenceFileLabel.TabStop = False
        Me.AxSequenceFileLabel.Visible = False
        '
        'AxRunSequenceButton
        '
        Me.AxRunSequenceButton.Location = New System.Drawing.Point(310, 407)
        Me.AxRunSequenceButton.Name = "AxRunSequenceButton"
        Me.AxRunSequenceButton.OcxState = CType(resources.GetObject("AxRunSequenceButton.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxRunSequenceButton.Size = New System.Drawing.Size(152, 24)
        Me.AxRunSequenceButton.TabIndex = 6
        '
        'AxSequencesList
        '
        Me.AxSequencesList.Location = New System.Drawing.Point(278, 292)
        Me.AxSequencesList.Name = "AxSequencesList"
        Me.AxSequencesList.OcxState = CType(resources.GetObject("AxSequencesList.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxSequencesList.Size = New System.Drawing.Size(215, 104)
        Me.AxSequencesList.TabIndex = 2
        '
        'executionTab
        '
        Me.executionTab.Controls.Add(Me.AxExecutionSteps)
        Me.executionTab.Controls.Add(Me.AxExecutionVariables)
        Me.executionTab.Controls.Add(Me.AxThreads)
        Me.executionTab.Controls.Add(Me.AxCallStack)
        Me.executionTab.Controls.Add(Me.AxExecutionLabel)
        Me.executionTab.Controls.Add(Me.AxTerminateRestartBtn)
        Me.executionTab.Controls.Add(Me.AxBreakResumeBtn)
        Me.executionTab.Location = New System.Drawing.Point(4, 22)
        Me.executionTab.Name = "executionTab"
        Me.executionTab.Size = New System.Drawing.Size(632, 452)
        Me.executionTab.TabIndex = 1
        Me.executionTab.Text = "EXECUTION"
        Me.executionTab.Visible = False
        '
        'AxExecutionVariables
        '
        Me.AxExecutionVariables.Location = New System.Drawing.Point(0, 296)
        Me.AxExecutionVariables.Name = "AxExecutionVariables"
        Me.AxExecutionVariables.OcxState = CType(resources.GetObject("AxExecutionVariables.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxExecutionVariables.Size = New System.Drawing.Size(239, 104)
        Me.AxExecutionVariables.TabIndex = 1
        '
        'AxThreads
        '
        Me.AxThreads.Location = New System.Drawing.Point(445, 296)
        Me.AxThreads.Name = "AxThreads"
        Me.AxThreads.OcxState = CType(resources.GetObject("AxThreads.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxThreads.Size = New System.Drawing.Size(187, 104)
        Me.AxThreads.TabIndex = 3
        '
        'AxCallStack
        '
        Me.AxCallStack.Location = New System.Drawing.Point(244, 296)
        Me.AxCallStack.Name = "AxCallStack"
        Me.AxCallStack.OcxState = CType(resources.GetObject("AxCallStack.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxCallStack.Size = New System.Drawing.Size(208, 104)
        Me.AxCallStack.TabIndex = 2
        '
        'AxExecutionLabel
        '
        Me.AxExecutionLabel.Location = New System.Drawing.Point(0, 402)
        Me.AxExecutionLabel.Name = "AxExecutionLabel"
        Me.AxExecutionLabel.OcxState = CType(resources.GetObject("AxExecutionLabel.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxExecutionLabel.Size = New System.Drawing.Size(331, 13)
        Me.AxExecutionLabel.TabIndex = 11
        Me.AxExecutionLabel.TabStop = False
        Me.AxExecutionLabel.Visible = False
        '
        'reportTab
        '
        Me.reportTab.Controls.Add(Me.AxReportView)
        Me.reportTab.Location = New System.Drawing.Point(4, 22)
        Me.reportTab.Name = "reportTab"
        Me.reportTab.Size = New System.Drawing.Size(632, 452)
        Me.reportTab.TabIndex = 2
        Me.reportTab.Text = "REPORT"
        Me.reportTab.Visible = False
        '
        'GCTimer
        '
        Me.GCTimer.Interval = 3000
        '
        'debugMenu
        '
        Me.debugMenu.Index = 3
        Me.debugMenu.Text = "DEBUG"
        '
        'toolsMenu
        '
        Me.toolsMenu.Index = 5
        Me.toolsMenu.Text = "TOOLS"
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileMenu, Me.editMenu, Me.executeMenu, Me.debugMenu, Me.configureMenu, Me.toolsMenu, Me.helpMenu})
        '
        'editMenu
        '
        Me.editMenu.Index = 1
        Me.editMenu.Text = "EDIT"
        '
        'MainForm
        '
        Me.ClientSize = New System.Drawing.Size(798, 494)
        Me.Controls.Add(Me.AxListBar)
        Me.Controls.Add(Me.AxApplicationMgr)
        Me.Controls.Add(Me.AxExecutionViewMgr)
        Me.Controls.Add(Me.AxSequenceFileViewMgr)
        Me.Controls.Add(Me.AxStatusBar)
        Me.Controls.Add(Me.tabControl)
        Me.Menu = Me.mainMenu1
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TESTSTAND_USER_INTERFACE"
        CType(Me.AxApplicationMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxSequenceFileViewMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxExecutionViewMgr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxFileSteps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxEntryPoint2Button, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxEntryPoint1Button, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxTerminateRestartBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxBreakResumeBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxExecutionSteps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxReportView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxListBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxStatusBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabControl.ResumeLayout(False)
        Me.fileTab.ResumeLayout(False)
        CType(Me.AxInsertionPalette, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxFileVariables, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxSequenceFileLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxRunSequenceButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxSequencesList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.executionTab.ResumeLayout(False)
        CType(Me.AxExecutionVariables, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxThreads, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxCallStack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxExecutionLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.reportTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Values that identifies a tag page in this application. Because these values can be converted to numbers, we can store them
    ' in a TestStand property. We use this to attach a new numeric property to each TestStand execution object to store which tab page 
    ' the application displays for that execution.
    Enum TabPageIdentifier
        NotATab
        SequenceFile
        Execution
        Report
    End Enum

    ' list bar page indices
    Private Const SEQUENCE_FILES_PAGE_INDEX As Integer = 0  ' first page in list bar
    Private Const EXECUTIONS_PAGE_INDEX As Integer = 1 ' second page in list bar

    ' global flag that indicates if we are programmatically changing the active tab page
    Private programmaticallyUpdatingTabPages As Boolean = False

    Private Const WM_QUERYENDSESSION As Integer = &H11

    ' flag that will be set to true if the user tries to shut down windows
    Private sessionEnding As Boolean = False

    ' a localizer converts text in .NET menus and Forms to the current language by obtaining the translations from the TestStand string resource files
    Private localizer As Localizer

    Private errorDlgTitle As String = "Error" ' localized in MainForm_Load

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
        Dim splashScreen As SplashScreen = Nothing

        Me.Icon = My.Resources.testexec

        Try
            If Not Me.AxApplicationMgr.ApplicationWillExitOnStart Then
                splashScreen = New SplashScreen ' display splash screen
            End If

            ' If this UI is running in a CLR other than the one TestStand uses,
            ' then it needs its own GCTimer for that version of the CLR. If it's running in the
            ' same CLR as TestStand then the engine's gctimer enabled by the ApplicationMgr
            ' is sufficient. See the API help for Engine.DotNetGarbageCollectionInterval for more details.
            If (System.Environment.Version.ToString() <> Me.AxApplicationMgr.GetEngine().DotNetCLRVersion) Then
                Me.GCTimer.Enabled = True
            End If

            ' localize error dialog title bar
            errorDlgTitle = Me.AxApplicationMgr.GetEngine().GetResourceString("TSUI_OI_MAIN_PANEL", "ERR_BOX_TITLE")

            ' this application allows setting of breakpoints on sequences files, so let them persist
            Me.AxApplicationMgr.GetEngine().PersistBreakpoints = True

            ' connect controls that are always visible
            Me.ConnectListBarPages()
            Me.ConnectStatusBarPanes()

            ' connect controls on the Sequence File tab
            Me.AxSequenceFileViewMgr.ConnectSequenceView(Me.AxFileSteps)
            Me.AxSequenceFileViewMgr.ConnectCommand(Me.AxEntryPoint1Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 0, CommandConnectionOptions.CommandConnection_NoOptions)
            Me.AxSequenceFileViewMgr.ConnectCommand(Me.AxEntryPoint2Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 1, CommandConnectionOptions.CommandConnection_NoOptions)
            Me.AxSequenceFileViewMgr.ConnectCommand(Me.AxRunSequenceButton, CommandKinds.CommandKind_RunCurrentSequence, 0, CommandConnectionOptions.CommandConnection_NoOptions)
            Me.AxSequenceFileViewMgr.ConnectCaption(Me.AxSequenceFileLabel, CaptionSources.CaptionSource_CurrentSequenceFile, False)
            Me.AxSequenceFileViewMgr.ConnectVariables(Me.AxFileVariables)
            Me.AxSequenceFileViewMgr.ConnectInsertionPalette(Me.AxInsertionPalette)
            Me.AxSequenceFileViewMgr.ConnectSequenceList(Me.AxSequencesList).SetColumnVisible(SeqListConnectionColumns.SeqListConnectionColumn_Comments, True)

            ' connect controls on the Execution tab
            Me.AxExecutionViewMgr.ConnectExecutionView(Me.AxExecutionSteps, ExecutionViewOptions.ExecutionViewConnection_NoOptions)
            Me.AxExecutionViewMgr.ConnectVariables(Me.AxExecutionVariables)
            Me.AxExecutionViewMgr.ConnectCallStack(Me.AxCallStack)
            Me.AxExecutionViewMgr.ConnectThreadList(Me.AxThreads)
            Me.AxExecutionViewMgr.ConnectCommand(Me.AxBreakResumeBtn, CommandKinds.CommandKind_BreakResume, 0, CommandConnectionOptions.CommandConnection_NoOptions)
            Me.AxExecutionViewMgr.ConnectCommand(Me.AxTerminateRestartBtn, CommandKinds.CommandKind_TerminateRestart, 0, CommandConnectionOptions.CommandConnection_NoOptions)
            Me.AxExecutionViewMgr.ConnectCaption(Me.AxExecutionLabel, CaptionSources.CaptionSource_CurrentExecution, False)

            ' connect controls on the Report tab
            Me.AxExecutionViewMgr.ConnectReportView(Me.AxReportView)

            ' start up the TestStand User Interface Components. this also logs in the user
            Me.AxApplicationMgr.Start()

            ' build the menubar for the first time
            Me.RebuildMenuBar()

            ' localize strings in top level menu items and controls
            Me.localizer = New Localizer(Me.AxApplicationMgr.GetEngine(), Me.AxApplicationMgr)
            Me.localizer.LocalizeForm(Me, "TSUI_OI_MAIN_PANEL", True)       ' localize .net controls
            Me.AxApplicationMgr.LocalizeAllControls("TSUI_OI_MAIN_PANEL")   ' localize TestStand UI Controls

            ' remember window and control positions from last time
            LayoutPersister.LoadSizes(Me.AxApplicationMgr, Me.AxListBar, Me.tabControl, Me.AxFileSteps, Me.AxSequencesList, Me.AxFileVariables, Me.AxInsertionPalette, Me.AxExecutionSteps, Me.AxCallStack, Me.AxThreads, Me.AxExecutionVariables)
            LayoutPersister.LoadBounds(Me.AxApplicationMgr, Me)

            ' decide which tab pages to initially show
            ShowAppropriateTabs()

            ' give initial control focus to the step list control
            Me.AxFileSteps.Select()
        Catch exception As Exception
            MessageBox.Show(Me, exception.Message)

            If Not splashScreen Is Nothing Then
                splashScreen.Hide()
            End If

            Application.Exit()
        End Try

        If Not splashScreen Is Nothing Then
            splashScreen.Hide()
        End If
    End Sub

    ' if the execution has a report, switch to the report tab either immediately if the execution is visible, or, whenever the execution is viewed next
    Private Sub ShowReport(ByVal execution As Execution)
        ' switch to report view when this execution is next viewed 
        execution.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, CDbl(TabPageIdentifier.Report)) ' activate the reportTab when the user views this execution
        If execution.Id = Me.AxExecutionViewMgr.Execution.Id Then ' is this execution the currently displayed execution?			
            Me.ShowAppropriateTabs() ' switch to report view tab now
        End If
    End Sub

    ' show the sequence file list and execution list in list bar pages
    Private Sub ConnectListBarPages()
        ' connect listbar page 0 to SequenceFileList
        Me.AxSequenceFileViewMgr.ConnectSequenceFileList(Me.AxListBar.Pages(0), False)

        ' connect listbar page 1 to ExecutionList
        Dim connection As ExecutionListConnection = Me.AxExecutionViewMgr.ConnectExecutionList(Me.AxListBar.Pages(1))
        ' display the execution name on the first line, the serial number (if any) on the next line, the socket index (if any) on the next line, and the model execution state on the last line (the expression string looks complicated here because we have to escape the quotes for the VB.net compiler.)
        connection.DisplayExpression = """%CurrentExecution%\n"" + (""%UUTSerialNumber%"" == """" ? """" : (ResStr(""TSUI_OI_MAIN_PANEL"",""SERIAL_NUMBER"") + "" %UUTSerialNumber%\n"")) + (""%TestSocketIndex%"" == """" ? """" : ResStr(""TSUI_OI_MAIN_PANEL"",""SOCKET_NUMBER"") + "" %TestSocketIndex%\n"") + ""%ModelState%"""
    End Sub

    Private Sub ConnectStatusBarPanes()
        Dim panes As StatusBarPanes = Me.AxStatusBar.Panes

        ' User
        Me.AxApplicationMgr.ConnectCaption(panes("User"), CaptionSources.CaptionSource_UserName, False)
        ' EngineEnvironment
        Me.AxApplicationMgr.ConnectCaption(panes("EngineEnvironment"), CaptionSources.CaptionSource_EngineEnvironment, False)
        ' File Process Model
        Me.AxSequenceFileViewMgr.ConnectCaption(panes("FileModel"), CaptionSources.CaptionSource_CurrentProcessModelFile, False).LongName = False
        ' Execution Process Model
        Me.AxExecutionViewMgr.ConnectCaption(panes("ExecutionModel"), CaptionSources.CaptionSource_CurrentProcessModelFile, False).LongName = False
        ' File Selected Steps
        Me.AxSequenceFileViewMgr.ConnectCaption(panes("FileSelectedSteps"), CaptionSources.CaptionSource_SelectedSteps_ZeroBased, False)
        ' File Number of Steps
        Me.AxSequenceFileViewMgr.ConnectCaption(panes("FileNumberOfSteps"), CaptionSources.CaptionSource_NumberOfSteps, False)
        ' Execution Selected Steps
        Me.AxExecutionViewMgr.ConnectCaption(panes("ExecutionSelectedSteps"), CaptionSources.CaptionSource_SelectedSteps_ZeroBased, False)
        ' Execution Number of Steps
        Me.AxExecutionViewMgr.ConnectCaption(panes("ExecutionNumberOfSteps"), CaptionSources.CaptionSource_NumberOfSteps, False)
        ' Progress Text
        Me.AxExecutionViewMgr.ConnectCaption(panes("ProgressText"), CaptionSources.CaptionSource_ProgressText, False)
        ' Progress Percent Text
        Me.AxExecutionViewMgr.ConnectCaption(panes("ProgressPercent"), CaptionSources.CaptionSource_ProgressPercent, False)
        ' Progress Percent Bar
        Me.AxExecutionViewMgr.ConnectNumeric(panes("ProgressPercent"), NumericSources.NumericSource_ProgressPercent)
        ' Report Location
        Me.AxExecutionViewMgr.ConnectCaption(panes("ReportLocation"), CaptionSources.CaptionSource_ReportLocation, True)
    End Sub

    ' handle request to close form (via Windows close box, for example)
    Private Sub MainForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        ' Don't set e.Cancel to True if windows is shutting down.
        ' Doing so would prevent windows from shutting down or logging out.
        If Not sessionEnding Then
            ' initiate shutdown and cancel close if shutdown is not complete.  The applicationMgr will
            ' send the ExitApplication event when shutdown is complete and we can close then
            If Me.AxApplicationMgr.Shutdown() = False Then
                e.Cancel = True
            Else
                Me.localizer = Nothing

                ' closing now, add all tabs so that they are disposed
                Me.tabControl.TabPages.Clear()
                Me.tabControl.TabPages.Add(reportTab)
                Me.tabControl.TabPages.Add(executionTab)
                Me.tabControl.TabPages.Add(fileTab)
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

    Private Sub tabControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabControl.SelectedIndexChanged
        If Not programmaticallyUpdatingTabPages Then ' filter out programmatically triggered activation events that might be due only to hidden tabs being made visible again
            ' remember which tab is active so when execution is revisited in the future, we can activate the same tab
            ' is the new tab an execution tab and there is a current execution?
            If Me.AxListBar.CurrentPage = EXECUTIONS_PAGE_INDEX And Not (Me.AxExecutionViewMgr.Execution Is Nothing) Then
                Dim thisTab As TabPageIdentifier
                If Me.tabControl.SelectedIndex = 0 Then
                    thisTab = TabPageIdentifier.Execution
                Else
                    thisTab = TabPageIdentifier.Report
                End If

                ' store the activated tab index in a custom property added to the execution
                Me.AxExecutionViewMgr.Execution.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, CDbl(thisTab))
            End If

            Me.ShowAppropriateStatusBarPanes()
        End If
    End Sub

    ' release any TestStand objects and save any settings here
    Private Sub AxApplicationMgr_QueryShutdown(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_QueryShutdownEvent) Handles AxApplicationMgr.QueryShutdown
        LayoutPersister.SaveSizes(Me.AxApplicationMgr, Me.AxListBar, Me.tabControl, Me.AxFileSteps, Me.AxSequencesList, Me.AxFileVariables, Me.AxInsertionPalette, Me.AxExecutionSteps, Me.AxCallStack, Me.AxThreads, Me.AxExecutionVariables)
        LayoutPersister.SaveBounds(Me.AxApplicationMgr, Me)
    End Sub

    ' It is now ok to exit, close the form
    Private Sub axApplicationMgr_ExitApplication(ByVal sender As Object, ByVal e As System.EventArgs) Handles AxApplicationMgr.ExitApplication
        ' discard any current menu items that were inserted by Menus.InsertCommandsInMenu. These menu items might refer to TestStand objects, so delete them before the engine is destroyed. Note that it is too early to do this in QueryShutdown because a menu might be used after QueryShutdown returns, particularly if an unload callback runs.
        Menus.RemoveMenuCommands(Me.mainMenu1, False, False)
        Environment.ExitCode = Me.AxApplicationMgr.ExitCode
        Close()

        TSHelper.DoSynchronousGCForCOMObjectDestruction() ' force .net garbage collection to ensure ensure all TestStand objects are freed before the TestStand engine unloads 							
    End Sub

    ' ApplicationMgr sends this message when it's busy doing something so we know to display a hourglass cursor or an equivalent
    Private Sub axApplicationMgr_Wait(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_WaitEvent) Handles AxApplicationMgr.Wait
        If e.showWait Then
            Me.Cursor = Cursors.WaitCursor
        Else
            Me.Cursor = Cursors.Default
        End If
    End Sub

    ' the ApplicationMgr sends this event to request that the UI display the report for a particular execution
    Private Sub axApplicationMgr_DisplayReport(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayReportEvent) Handles AxApplicationMgr.DisplayReport
        Me.ShowReport(e.exec)
    End Sub

    ' the ApplicationMgr sends this event to request that the UI display a particular execution
    Private Sub axApplicationMgr_DisplayExecution(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayExecutionEvent) Handles AxApplicationMgr.DisplayExecution

        ' bring application to front if we hit a breakpoint
        If (e.reason = ExecutionDisplayReasons.ExecutionDisplayReason_Breakpoint) Or (e.reason = ExecutionDisplayReasons.ExecutionDisplayReason_BreakOnRunTimeError) Then
            Me.Activate()
        End If

        ' show this execution
        Me.AxExecutionViewMgr.Execution = e.exec
        ' show the executions page in the list bar
        Me.AxListBar.CurrentPage = EXECUTIONS_PAGE_INDEX
        ' in case we are already showing the executions page, ensure we switch to steps or report tab as appropriate
        Me.ShowAppropriateTabs()
    End Sub

    ' the ApplicationMgr sends this event to request that the UI display a particular sequence file
    Private Sub axApplicationMgr_DisplaySequenceFile(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplaySequenceFileEvent) Handles AxApplicationMgr.DisplaySequenceFile
        ' show this sequence file
        Me.AxSequenceFileViewMgr.SequenceFile = e.file
        ' show the sequence files page in the list bar
        Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX
    End Sub

    ' the ApplicationMgr sends this event to report any error that occurs when the TestStand UI controls respond to user input (ie, an error 
    ' other than one returned by a method you call directly). For example, if a TestStand menu command generates an error, this handler displays it
    Private Sub axApplicationMgr_ReportError(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_ReportErrorEvent) Handles AxApplicationMgr.ReportError

        AxApplicationMgr.GetEngine().DisplayErrorDialog(errorDlgTitle, e.errorMessage, e.errorCode, 0)

    End Sub

    ' the ApplicationMgr sends this event whenever an execution starts
    Private Sub axApplicationMgr_StartExecution(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_StartExecutionEvent) Handles AxApplicationMgr.StartExecution
        ' add a custom property to the execution to store which tab we are displaying for this execution. Initially show the execution tab
        e.exec.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, CDbl(TabPageIdentifier.Execution))
    End Sub

    ' the ExecutionViewMgr sends this event whenever a new execution is selected
    Private Sub axExecutionViewMgr_ExecutionChanged(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ExecutionViewMgrEvents_ExecutionChangedEvent) Handles AxExecutionViewMgr.ExecutionChanged
        ' switch to report or steps tab depending on what the execution displayed last
        Me.ShowAppropriateTabs()
    End Sub

    ' build a context menu for a control that has been right-clicked
    Private Sub BuildCommandSetMenu(ByVal commandSet As CommandKinds, ByVal menuHandle As Integer)
        Dim cmds As Commands
        Dim unused As Integer

        cmds = Me.AxApplicationMgr.NewCommands()
        ' insert items for the specified command or command set in the context menu
        cmds.InsertKind(commandSet, GetActiveViewManager(), -1, "", "", unused)
        Menus.RemoveInvalidShortcutKeys(cmds)   ' remove any shortcuts that .NET does not support
        cmds.InsertIntoWin32Menu(menuHandle, -1, True, True)    ' we are using the context menu that the control provides because it requires fewer lines of code. We could have built a .net context menu instead. If you build a .net context menu, you should note that they do not display for ActiveX controls. However, you can display a context menu for the parent form in response to a ActiveX control right-mouse-click event and achieve the same result. To display the context menu at the correct location, convert the click coordinates from control coordinates to form coordinates
    End Sub

    ' create a right-click menu for the SequenceView control that displays execution steps
    Private Sub AxExecutionView_CreateContextMenu(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEvent) Handles AxExecutionSteps.CreateContextMenu
        Me.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceViewContextMenu_Set, e.menuHandle)
    End Sub

    ' create a right-click menu for the SequenceView control that displays sequence file steps
    Private Sub AxSequenceView_CreateContextMenu(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEvent) Handles AxFileSteps.CreateContextMenu
        Me.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceViewContextMenu_Set, e.menuHandle)
    End Sub

    ' create a right-click menu for the ListBar control
    Private Sub AxListBar_CreateContextMenu(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CreateContextMenuEvent) Handles AxListBar.CreateContextMenu
        Me.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultListBarContextMenu_Set, e.menuHandle)
    End Sub

    ' create a right-click menu for the sequences list box control
    Private Sub AxSequencesList_CreateContextMenu(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_CreateContextMenuEvent) Handles AxSequencesList.CreateContextMenu
        Me.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceListContextMenu_Set, e.menuHandle)
    End Sub

    ' the ListBar sends this event when the listbar switches to a new page
    Private Sub axListBar_CurPageChanged(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CurPageChangedEvent) Handles AxListBar.CurPageChanged
        Me.ShowAppropriateTabs()
        Me.UpdateWindowTitle()
    End Sub

    ' append the caption for the selected file or execution to the application window title
    Private Sub UpdateWindowTitle()
        Dim unused As Object = Nothing
        Dim title As String = Me.AxApplicationMgr.GetEngine().GetResourceString("TSUI_OI_MAIN_PANEL", "TESTSTAND_USER_INTERFACE", "", unused)
        Dim documentDescription As String = Nothing

        If (Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX) Then   ' sequence files are visible
            documentDescription += Me.AxSequenceFileViewMgr.GetCaptionText(CaptionSources.CaptionSource_CurrentSequenceFile, False, "")
        Else    ' executions are visible
            documentDescription += Me.AxExecutionViewMgr.GetCaptionText(CaptionSources.CaptionSource_CurrentExecution, False, "")
        End If

        If (documentDescription <> Nothing And documentDescription <> "") Then
            title += " - " + documentDescription
        End If

        Me.Text = title
    End Sub

    ' a hidden label control is connected to CaptionSource_CurrentSequenceFile so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current file
    Private Sub AxSequenceFileLabel_ConnectionActivity(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEvent) Handles AxSequenceFileLabel.ConnectionActivity
        Me.UpdateWindowTitle()
    End Sub

    ' a hidden label control is connected to CaptionSource_CurrentExecution so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current execution
    Private Sub AxExecutionLabel_ConnectionActivity(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEvent) Handles AxExecutionLabel.ConnectionActivity
        Me.UpdateWindowTitle()
    End Sub

    ' based on the current listbar page, show and hide the tabs that appear in the space to the right of the listbar
    Private Sub ShowAppropriateTabs()
        programmaticallyUpdatingTabPages = True

        Dim curActiveCtl As Control
        Dim pageToDisplay As TabPage = Nothing


        If Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX Then
            pageToDisplay = Me.fileTab
        Else
            ' we are viewing an execution...
            pageToDisplay = Me.executionTab ' default tab

            ' if the report tab page was the last tab displayed for this execution, re-activate it instead
            If Not (Me.AxExecutionViewMgr.Execution Is Nothing) Then
                If TabPageIdentifier.Report = CType(Me.AxExecutionViewMgr.Execution.AsPropertyObject().GetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing), TabPageIdentifier) Then
                    pageToDisplay = Me.reportTab
                End If
            End If
        End If

        If Not Me.tabControl.SelectedTab Is pageToDisplay Or Me.tabControl.TabCount = 3 Then ' prevent flashing if tab page has not changed. if all three tabs are visible at once, then we have never called this function before and we need to display the correct tabs
            ' store the current Active control as Me.tabControl.SelectedTab changes focus to the first control on the tab
            curActiveCtl = Me.ActiveControl

            ' show and hide tabs as appropriate (.NET doesn't let you hide a tab page, so we remove all pages and add back the ones we want to show)
            Me.tabControl.TabPages.Clear()

            If pageToDisplay Is Me.fileTab Then
                Me.tabControl.TabPages.Add(Me.fileTab)
            Else
                Me.tabControl.TabPages.Add(Me.executionTab)
                Me.tabControl.TabPages.Add(Me.reportTab)
            End If

            Me.tabControl.SelectedTab = pageToDisplay

            ' set the current Active control as Me.tabControl.SelectedTab changes focus to the first control on the tab
            If ((Not (curActiveCtl Is Nothing)) AndAlso curActiveCtl.CanFocus) Then
                Me.ActiveControl = curActiveCtl
            End If
        End If

        programmaticallyUpdatingTabPages = False
        Me.ShowAppropriateStatusBarPanes() ' call this here because we can't call it in tabControl_SelectedIndexChanged if programmaticallyUpdatingTabPages is true because we might still be initializing the application, also needed because .net doesn't send the tabControl_SelectedIndexChanged when adding the first tab to a tab control
    End Sub

    Private Sub ShowAppropriateStatusBarPanes()
        If Me.tabControl.SelectedIndex >= 0 Then
            If Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX Then ' if only the files tab is visible
                Me.AxStatusBar.ShowPanes("User, EngineEnvironment, FileModel, FileSelectedSteps, FileNumberOfSteps")
            Else
                If Me.tabControl.SelectedIndex = 0 Then     ' execution tab is selected
                    Me.AxStatusBar.ShowPanes("User, EngineEnvironment, ExecutionModel, ExecutionSelectedSteps, ExecutionNumberOfSteps, ProgressText, ProgressPercent")
                Else                                        ' report tab is selected
                    Me.AxStatusBar.ShowPanes("User, EngineEnvironment, ExecutionModel, ReportLocation, ProgressText, ProgressPercent")
                End If
            End If
        End If
    End Sub

    ' determine which view manager menu commands apply to
    Private Function GetActiveViewManager() As Object
        If Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX Then ' sequence files are visible, sequence file menu commands apply
            Return Me.AxSequenceFileViewMgr
        ElseIf Me.AxListBar.CurrentPage = EXECUTIONS_PAGE_INDEX Then ' executions are visible, execution menu commands apply
            Return Me.AxExecutionViewMgr
        Else
            Return Nothing
        End If
    End Function


    ' The menu has been accessed, rebuild it with currently applicable items
    Private Sub MainForm_MenuStart(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MenuStart
        RebuildMenuBar()
    End Sub

    ' make sure all menus have appropriate items with the correct enabled states
    Private Sub RebuildMenuBar()
        Try
            Menus.BeginUpdate(Me)

            ' discard any current menu items that were inserted by Menus.InsertCommandsInMenu
            Menus.RemoveMenuCommands(Me.mainMenu1, False, False)

            Dim viewMgr As Object = GetActiveViewManager()

            ' rebuild File menu
            Dim fileMenuCommands As New ArrayList
            fileMenuCommands.Add(CommandKinds.CommandKind_DefaultFileMenu_Set)              ' add all the usual commands in a File menu
            Menus.InsertCommandsInMenu(fileMenuCommands, Me.fileMenu, Nothing, viewMgr, True)

            ' rebuild Edit menu
            Dim editMenuCommands As New ArrayList
            editMenuCommands.Add(CommandKinds.CommandKind_DefaultEditMenu_Set)              ' add all the usual commands in an Edit menu
            Menus.InsertCommandsInMenu(editMenuCommands, Me.editMenu, Nothing, viewMgr, True)

            ' rebuild Execute menu
            Dim executeMenuCommands As New ArrayList
            executeMenuCommands.Add(CommandKinds.CommandKind_DefaultExecuteMenu_Set)        ' add all the usual commands in a Execute menu				
            Menus.InsertCommandsInMenu(executeMenuCommands, Me.executeMenu, Nothing, viewMgr, True)

            ' rebuild Debug menu
            Dim debugMenuCommands As New ArrayList
            debugMenuCommands.Add(CommandKinds.CommandKind_DefaultDebugMenu_Set)            ' add all the usual commands in a Debug menu
            Menus.InsertCommandsInMenu(debugMenuCommands, Me.debugMenu, Nothing, viewMgr, True)

            ' rebuild Configure menu
            Dim configureMenuCommands As New ArrayList
            configureMenuCommands.Add(CommandKinds.CommandKind_DefaultConfigureMenu_Set)    ' add all the usual commands in a Configure menu
            If AxApplicationMgr.IsEditor Then
                configureMenuCommands.Add(CommandKinds.CommandKind_ConfigureEngineEnvironment)
            End If
            Menus.InsertCommandsInMenu(configureMenuCommands, Me.configureMenu, Nothing, viewMgr, True)

            ' rebuild Tools menu
            Dim toolsMenuCommands As New ArrayList
            toolsMenuCommands.Add(CommandKinds.CommandKind_DefaultToolsMenu_Set)            ' add all the usual commands in a Tools menu
            Menus.InsertCommandsInMenu(toolsMenuCommands, Me.toolsMenu, Nothing, viewMgr, True)

            ' rebuild the Help menu. Note that the help menu already contains an "About..." item, which is not a TestStand command item
            Dim helpMenuCommands As New ArrayList
            helpMenuCommands.Add(CommandKinds.CommandKind_Separator)                        ' separates the existing About... item
            helpMenuCommands.Add(CommandKinds.CommandKind_DefaultHelpMenu_Set)              ' add all the usual commands in a Help menu. Note that most help items appear only when in Edit mode.
            Menus.InsertCommandsInMenu(helpMenuCommands, Me.helpMenu, Nothing, viewMgr, True)

            ' remove duplicate separators and shortcut keys
            Menus.CleanupMenu(Me.mainMenu1)

            Menus.EndUpdate()
        Catch theException As Exception
            MessageBox.Show(Me, theException.Message, errorDlgTitle)
        End Try
    End Sub

    ' build a context menu for a sequence view control that has been right-clicked
    Private Sub BuildSequenceViewContextMenu(ByVal menuHandle As Integer)
        Dim cmds As Commands = Me.AxApplicationMgr.NewCommands()
        Dim unused As Integer = 0

        ' insert items for default sequence view context menu in the context menu
        cmds.InsertKind(CommandKinds.CommandKind_DefaultSequenceViewContextMenu_Set, GetActiveViewManager(), -1, "", "", unused)
        Menus.RemoveInvalidShortcutKeys(cmds) ' remove any shortcuts that .NET does not support
        cmds.InsertIntoWin32Menu(menuHandle, -1, True, True)  ' we are using the context menu that the control provides because it requires fewer lines of code. We could have built a .net context menu instead. If you build a .net context menu, you should note that they do not display for ActiveX controls. However, you can display a context menu for the parent form in response to a ActiveX control right-mouse-click event and achieve the same result. To display the context menu at the correct location, convert the click coordinates from control coordinates to form coordinates
    End Sub

    ' Release all objects periodically.  .NET lets COM objects pile up on the managed heap, seemingly even objects you don't know about such
    ' as parameters to unhandled ActiveX events.  This timer ensures that all COM objects are released in a timely manner,
    ' thus preventing the performance hiccup that could occur when .NET finally decides to collect garbage. Also, this timer
    ' ensures that actions triggered by object destruction run in a timely manner. For example: sequence file unload callbacks.
    Private Sub GCTimerTick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GCTimer.Tick
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, False) ' force .net garbage collection		
    End Sub 'GCTimerTick

    ' displays the about box when the user selects Help>>About...
    Private Sub aboutBoxItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles aboutBoxItem.Click
        Dim aboutBox As New AboutBox(Me.localizer)

        aboutBox.ShowDialog(Me)
    End Sub

    ' adjust controls to fit within current window size
    Private Sub MainForm_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If (Me.Visible) Then     ' filter out the resize events that occur before initialization is complete
            Me.ArrangeControls()
        End If
    End Sub

    ' arrange the controls for the first time
    Private Sub MainForm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.ArrangeControls()
    End Sub

    Private Sub ArrangeControls()
        If (Me.WindowState = FormWindowState.Minimized) Then ' don't shrink controls and inadvertently close subpanes when the window is minimized
            Return
        End If

        Const buttonVerticalMargin As Integer = 6
        Const buttonHorizontalSpacing As Integer = 12

        ' resize controls to fit current window height
        Me.tabControl.Height = Me.ClientRectangle.Height - Me.AxStatusBar.Height
        Me.AxListBar.Height = Me.tabControl.Height

        ' there is a vertical drag bar between listbar and tab control
        Splitters.DivideSpaceBetweenPanes(True, 0, Me.ClientRectangle.Width, Me.AxListBar, Me.tabControl)

        ' obtain new size of tab area interior
        Dim tabInteriorRect As Rectangle = Me.tabControl.DisplayRectangle

        ' File and Execution Tabs have buttons at the bottom. figure out how much space is left after the buttons are placed
        Dim buttonHeight As Integer = Me.AxBreakResumeBtn.Height
        Dim buttonTop As Integer = Math.Max(tabInteriorRect.Height - buttonHeight - buttonVerticalMargin, 40 + buttonVerticalMargin)

        ' compute the size of the tab control interior area minus space for the buttons. this area is shared by controls that be resized
        ' don't shrink control area to less than 40 by 40 pixels so that if the window is resized to have no visible area, the controls aren't made so small that we forget their relative sizes
        Dim resizableTabControlAreaHeight As Integer = Math.Max(buttonTop - buttonVerticalMargin, 40)
        Dim resizableTabControlAreaWidth As Integer = Math.Max(tabInteriorRect.Width, 40)

        ' FILE TAB:

        ' place the file tab buttons
        Me.AxEntryPoint1Button.Top = buttonTop
        Me.AxEntryPoint2Button.Top = buttonTop
        Me.AxRunSequenceButton.Top = buttonTop
        Me.AxEntryPoint2Button.Left = AxEntryPoint1Button.Right + buttonHorizontalSpacing
        Me.AxRunSequenceButton.Left = AxEntryPoint2Button.Right + buttonHorizontalSpacing

        ' hide editor-only controls if not an editor
        Dim isEditor As Boolean = Me.AxApplicationMgr.IsEditor
        Me.AxInsertionPalette.Visible = isEditor

        ' there is a horizontal drag bar between FileSteps and SequenceList/FileVariables
        Splitters.DivideSpaceBetweenPanes(False, 0, resizableTabControlAreaHeight, Me.AxFileSteps, Me.AxSequencesList)

        ' file variables has same height and top as the sequences list
        Me.AxFileVariables.Top = Me.AxSequencesList.Top
        Me.AxFileVariables.Height = Me.AxSequencesList.Height

        ' there is a vertical drag bar between the FileSteps and the Insertion Palette
        If (Not isEditor) Then
            Me.AxFileSteps.Width = resizableTabControlAreaWidth ' Insertion Palette is not visible, nothing to split
        Else
            Splitters.DivideSpaceBetweenPanes(True, 0, resizableTabControlAreaWidth, Me.AxFileSteps, Me.AxInsertionPalette)
        End If

        Me.AxInsertionPalette.Height = resizableTabControlAreaHeight

        ' there is a vertical drag bar between the FileVariables and the SequencesList
        Splitters.DivideSpaceBetweenPanes(True, 0, Me.AxFileSteps.Width, Me.AxFileVariables, Me.AxSequencesList)

        ' EXECUTION TAB:

        ' place the execution tab buttons
        Me.AxBreakResumeBtn.Top = buttonTop
        Me.AxTerminateRestartBtn.Top = buttonTop
        Me.AxTerminateRestartBtn.Left = AxBreakResumeBtn.Right + buttonHorizontalSpacing

        ' size ExecutionSteps to tab control interior width
        Me.AxExecutionSteps.Width = resizableTabControlAreaWidth

        ' there is a horizontal drag bar between ExecutionSteps and ExecutionVariablesView/CallStack/Threads/
        Splitters.DivideSpaceBetweenPanes(False, 0, resizableTabControlAreaHeight, Me.AxExecutionSteps, Me.AxCallStack)

        ' execution variables and threads have the same height and top as the callstack
        Me.AxExecutionVariables.Height = Me.AxCallStack.Height
        Me.AxThreads.Height = Me.AxCallStack.Height
        Me.AxExecutionVariables.Top = Me.AxCallStack.Top
        Me.AxThreads.Top = Me.AxCallStack.Top

        Me.AxThreads.Top = Me.AxCallStack.Top

        ' there are vertical drag bars between the ExecutionVariables, CallStack, and Threads
        Splitters.DivideSpaceBetweenPanes(True, 0, Me.AxExecutionSteps.Width, Me.AxExecutionVariables, Me.AxCallStack, Me.AxThreads)

        ' REPORT TAB:
        Me.AxReportView.Height = tabInteriorRect.Height
        Me.AxReportView.Width = tabInteriorRect.Width

        Me.Update()  ' needed so that drawing keeps pace with drag bar movement		
    End Sub

    ' user dragged the vertical bar that separates the listbar from rest of the application
    Private Sub AxListBar_BorderDragged(ByVal sender As Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_BorderDraggedEvent) Handles AxListBar.BorderDragged
        Splitters.DragSplitter(Me.AxListBar, Me.tabControl, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged the horizontal bar that separates the step list from the sequence list and file variables
    Private Sub AxFileSteps_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEvent) Handles AxFileSteps.BorderDragged
        Splitters.DragSplitter(Me.AxFileSteps, Me.AxSequencesList, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged vertical bar that separates the file variables from the sequences list
    Private Sub AxFileVariables_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEvent) Handles AxFileVariables.BorderDragged
        Splitters.DragSplitter(Me.AxFileVariables, Me.AxSequencesList, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged the vertical bar on the insertion palette that separates it from the file step list
    Private Sub AxInsertionPalette_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._InsertionPaletteEvents_BorderDraggedEvent) Handles AxInsertionPalette.BorderDragged
        Splitters.DragSplitter(Me.AxInsertionPalette, Me.AxFileSteps, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged the horizontal bar that separates the execution step list from the call stack, thread list, and execution variables
    Private Sub AxExecutionSteps_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEvent) Handles AxExecutionSteps.BorderDragged
        Splitters.DragSplitter(Me.AxExecutionSteps, Me.AxCallStack, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged the vertical bar that separates the execution variables from the callstack
    Private Sub AxExecutionVariables_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEvent) Handles AxExecutionVariables.BorderDragged
        Splitters.DragSplitter(Me.AxExecutionVariables, Me.AxCallStack, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user dragged the vertical bar that separates the callstack from the thread list
    Private Sub axCallStack_BorderDragged(ByVal sender As System.Object, ByVal e As NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_BorderDraggedEvent) Handles AxCallStack.BorderDragged
        Splitters.DragSplitter(Me.AxCallStack, Me.AxThreads, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged)
        Me.ArrangeControls()
    End Sub

    ' user toggled edit mode. the only way to do that in this application is to type ctrl-shift-alt-insert, which is the edit mode toggle key this application specifies in designer for the ApplicationMgr control. 
    ' to prevent edit mode from being toggled with a hotkey, set ApplicationMgr.EditModeShortcutKey to ShortcutKey_VK_NOT_A_KEY
    Private Sub AxApplicationMgr_EditModeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxApplicationMgr.EditModeChanged
        Me.AxInsertionPalette.Width = Math.Max(Me.AxInsertionPalette.Width, 260)    ' make sure the palette is wide enough in case it is going to be shown
        Me.ArrangeControls()    ' relayout the controls to reflect the change in edit mode
    End Sub

    Private Sub AxApplicationMgr_PostCommandExecute(sender As System.Object, e As NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_PostCommandExecuteEvent) Handles AxApplicationMgr.PostCommandExecute
        If e.command.Kind = CommandKinds.CommandKind_CloseCompletedExecutions Then
            If Me.AxApplicationMgr.Executions.Count = 0 Then
                ' if we closed all the executions, switch to the files page instead of showing an empty executions page
                Me.AxListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX
            End If
        End If
    End Sub
End Class
