// Note:	This application has a manifest file in the project. This manifest file includes the Microsoft.Windows.Common-Controls which 
//			enables the application to display controls using the XP theme that the operating system selects.
//			A post build event embeds this manifest file into the executable.
//			In order for the manifest file to enable the executable to display with the XP theme:
//			1. The manifest file must have the same name as the executable. For example, if your executable is named MyExecutable.exe, your manifest file is required to have the name MyExecutable.exe.manifest.
//			2. The manifest file must include the Microsoft.Windows.Common-Controls.
//			3. The manifest file must reside in the same directory as the executable.
//			Also note that if you enable the Project Properties>>Debug>>Enable Visual Studio Hosting Process option, the XP theme adaption does not occur when debugging the executable
//			because the Visual Studio environment creates the process and does not allow the manifest file to be embedded into the executable.

// Note:	This example can function as an editor or an operator interface. The user can change the edit mode with a keystroke (ctrl-alt-shift-insert) or with a command line 
//			argument. For more information and for instructions on how prevent the user from changing the edit mode, refer to the TestStand Reference Manual>>Creating Custom 
//			User Interfaces>>Editor versus Operator Interface Applications>>Creating Editor Applications.

// Note:	TestStand installs the source code files for the default user interfaces in the <TestStand>\UserInterfaces and <TestStand Public>\UserInterfaces directories. 
//			To modify the installed user interfaces or to create new user interfaces, modify the files in the <TestStand Public>\UserInterfaces directory. 
//			You can use the read-only source files for the default user interfaces in the <TestStand>\UserInterfaces directory as a reference. 
//			National Instruments recommends that you track the changes you make to the user interface source code files so you can integrate the changes with any enhancements in future versions of the TestStand User Interfaces.

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

// TestStand Core API 
using NationalInstruments.TestStand.Interop.API; 

// TestStand User Interface Controls
using NationalInstruments.TestStand.Interop.UI;
using NationalInstruments.TestStand.Interop.UI.Support;

// .net specific functions for use with TestStand APIs (TSUtil)
using NationalInstruments.TestStand.Utility;

namespace TestExec
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr axApplicationMgr;
		private System.Windows.Forms.Timer GCTimer;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr axExecutionViewMgr;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr axSequenceFileViewMgr;
		private System.Windows.Forms.MainMenu mainMenu1;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxListBar axListBar;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxStatusBar axStatusBar;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage reportTab;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxReportView axReportView;
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem executeMenu;
		private System.Windows.Forms.MenuItem debugMenu;
		private System.Windows.Forms.MenuItem configureMenu;
		private System.Windows.Forms.MenuItem toolsMenu;
		private System.Windows.Forms.MenuItem helpMenu;
		private System.Windows.Forms.MenuItem aboutBoxItem;

		// Values that identifies a tag page in this application. Because these values can be converted to numbers, we can store them
		// in a TestStand property. We use this to attach a new numeric property to each TestStand execution object to store which tab page 
		// the application displays for that execution.
		enum TabPageIdentifier 
		{
			NotATab,
			SequenceFile,
			Execution,
			Report
		};

		// list bar page indices
		private const int	SEQUENCE_FILES_PAGE_INDEX = 0;	// first page in list bar
		private const int	EXECUTIONS_PAGE_INDEX = 1;		// second page in list bar

		// global flag that indicates if we are programmatically changing the active tab page
		private bool		programmaticallyUpdatingTabPages = false;

		private const int	WM_QUERYENDSESSION = 0x11;

		// flag that will be set to true if the user tries to shut down windows
		private bool		sessionEnding = false;

		// a localizer converts text in .NET menus and Forms to the current language by obtaining the translations from the TestStand string resource files
		private Localizer	localizer;
		private System.Windows.Forms.MenuItem editMenu;
		private System.Windows.Forms.TabPage fileTab;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView axFileSteps;
        private System.Windows.Forms.TabPage executionTab;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxButton axEntryPoint2Button;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxButton axEntryPoint1Button;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxListBox axThreads;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxListBox axCallStack;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxButton axTerminateRestartBtn;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxButton axBreakResumeBtn;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView axExecutionSteps;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxButton axRunSequenceButton;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel axSequenceFileLabel;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxLabel axExecutionLabel;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxListBox axSequencesList;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView axFileVariables;
        private NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView axExecutionVariables;
		private NationalInstruments.TestStand.Interop.UI.Ax.AxInsertionPalette axInsertionPalette;

		private string errorDlgTitle = "Error"; // localized in MainForm_Load

		public MainForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			this.Icon = Properties.Resources.testexec;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.GCTimer = new System.Windows.Forms.Timer(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.editMenu = new System.Windows.Forms.MenuItem();
			this.executeMenu = new System.Windows.Forms.MenuItem();
			this.debugMenu = new System.Windows.Forms.MenuItem();
			this.configureMenu = new System.Windows.Forms.MenuItem();
			this.toolsMenu = new System.Windows.Forms.MenuItem();
			this.helpMenu = new System.Windows.Forms.MenuItem();
			this.aboutBoxItem = new System.Windows.Forms.MenuItem();
			this.axListBar = new NationalInstruments.TestStand.Interop.UI.Ax.AxListBar();
			this.axStatusBar = new NationalInstruments.TestStand.Interop.UI.Ax.AxStatusBar();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.fileTab = new System.Windows.Forms.TabPage();
			this.axInsertionPalette = new NationalInstruments.TestStand.Interop.UI.Ax.AxInsertionPalette();
			this.axFileVariables = new NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView();
			this.axSequenceFileLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.axRunSequenceButton = new NationalInstruments.TestStand.Interop.UI.Ax.AxButton();
			this.axSequencesList = new NationalInstruments.TestStand.Interop.UI.Ax.AxListBox();
			this.axFileSteps = new NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView();
			this.axEntryPoint2Button = new NationalInstruments.TestStand.Interop.UI.Ax.AxButton();
			this.axEntryPoint1Button = new NationalInstruments.TestStand.Interop.UI.Ax.AxButton();
			this.executionTab = new System.Windows.Forms.TabPage();
			this.axExecutionVariables = new NationalInstruments.TestStand.Interop.UI.Ax.AxVariablesView();
			this.axExecutionLabel = new NationalInstruments.TestStand.Interop.UI.Ax.AxLabel();
			this.axCallStack = new NationalInstruments.TestStand.Interop.UI.Ax.AxListBox();
			this.axExecutionSteps = new NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceView();
			this.axTerminateRestartBtn = new NationalInstruments.TestStand.Interop.UI.Ax.AxButton();
			this.axBreakResumeBtn = new NationalInstruments.TestStand.Interop.UI.Ax.AxButton();
			this.axThreads = new NationalInstruments.TestStand.Interop.UI.Ax.AxListBox();
			this.reportTab = new System.Windows.Forms.TabPage();
			this.axReportView = new NationalInstruments.TestStand.Interop.UI.Ax.AxReportView();
			this.axApplicationMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr();
			this.axExecutionViewMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr();
			this.axSequenceFileViewMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr();
			((System.ComponentModel.ISupportInitialize)(this.axListBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axStatusBar)).BeginInit();
			this.tabControl.SuspendLayout();
			this.fileTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axInsertionPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axFileVariables)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequenceFileLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axRunSequenceButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequencesList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axFileSteps)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axEntryPoint2Button)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axEntryPoint1Button)).BeginInit();
			this.executionTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionVariables)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axCallStack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionSteps)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axTerminateRestartBtn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axBreakResumeBtn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axThreads)).BeginInit();
			this.reportTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axReportView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axApplicationMgr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionViewMgr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequenceFileViewMgr)).BeginInit();
			this.SuspendLayout();
			// 
			// GCTimer
			// 
			this.GCTimer.Interval = 3000;
			this.GCTimer.Tick += new System.EventHandler(this.GCTimerTick);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenu,
            this.editMenu,
            this.executeMenu,
            this.debugMenu,
            this.configureMenu,
            this.toolsMenu,
            this.helpMenu});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.Text = "FILE";
			// 
			// editMenu
			// 
			this.editMenu.Index = 1;
			this.editMenu.Text = "EDIT";
			// 
			// executeMenu
			// 
			this.executeMenu.Index = 2;
			this.executeMenu.Text = "EXECUTE";
			// 
			// debugMenu
			// 
			this.debugMenu.Index = 3;
			this.debugMenu.Text = "DEBUG";
			// 
			// configureMenu
			// 
			this.configureMenu.Index = 4;
			this.configureMenu.Text = "CONFIGURE";
			// 
			// toolsMenu
			// 
			this.toolsMenu.Index = 5;
			this.toolsMenu.Text = "TOOLS";
			// 
			// helpMenu
			// 
			this.helpMenu.Index = 6;
			this.helpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutBoxItem});
			this.helpMenu.Text = "HELP";
			// 
			// aboutBoxItem
			// 
			this.aboutBoxItem.Index = 0;
			this.aboutBoxItem.Text = "ABOUT";
			this.aboutBoxItem.Click += new System.EventHandler(this.aboutBoxItem_Click);
			// 
			// axListBar
			// 
			this.axListBar.Enabled = true;
			this.axListBar.Location = new System.Drawing.Point(0, 0);
			this.axListBar.Name = "axListBar";
			this.axListBar.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axListBar.OcxState")));
			this.axListBar.Size = new System.Drawing.Size(160, 478);
			this.axListBar.TabIndex = 0;
			this.axListBar.CurPageChanged += new NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CurPageChangedEventHandler(this.axListBar_CurPageChanged);
			this.axListBar.CreateContextMenu += new NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CreateContextMenuEventHandler(this.axListBar_CreateContextMenu);
			this.axListBar.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_BorderDraggedEventHandler(this.axListBar_BorderDragged);
			// 
			// axStatusBar
			// 
			this.axStatusBar.Enabled = true;
			this.axStatusBar.Location = new System.Drawing.Point(0, 477);
			this.axStatusBar.Name = "axStatusBar";
			this.axStatusBar.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axStatusBar.OcxState")));
			this.axStatusBar.Size = new System.Drawing.Size(958, 22);
			this.axStatusBar.TabIndex = 19;
			this.axStatusBar.TabStop = false;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.fileTab);
			this.tabControl.Controls.Add(this.executionTab);
			this.tabControl.Controls.Add(this.reportTab);
			this.tabControl.Location = new System.Drawing.Point(160, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(768, 478);
			this.tabControl.TabIndex = 1;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// fileTab
			// 
			this.fileTab.Controls.Add(this.axInsertionPalette);
			this.fileTab.Controls.Add(this.axFileVariables);
			this.fileTab.Controls.Add(this.axSequenceFileLabel);
			this.fileTab.Controls.Add(this.axRunSequenceButton);
			this.fileTab.Controls.Add(this.axSequencesList);
			this.fileTab.Controls.Add(this.axFileSteps);
			this.fileTab.Controls.Add(this.axEntryPoint2Button);
			this.fileTab.Controls.Add(this.axEntryPoint1Button);
			this.fileTab.Location = new System.Drawing.Point(4, 22);
			this.fileTab.Name = "fileTab";
			this.fileTab.Size = new System.Drawing.Size(760, 452);
			this.fileTab.TabIndex = 0;
			this.fileTab.Text = "FILE_TAB";
			// 
			// axInsertionPalette
			// 
			this.axInsertionPalette.Location = new System.Drawing.Point(540, 0);
			this.axInsertionPalette.Name = "axInsertionPalette";
			this.axInsertionPalette.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axInsertionPalette.OcxState")));
			this.axInsertionPalette.Size = new System.Drawing.Size(224, 397);
			this.axInsertionPalette.TabIndex = 3;
			this.axInsertionPalette.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._InsertionPaletteEvents_BorderDraggedEventHandler(this.axInsertionPalette_BorderDragged);
			// 
			// axFileVariables
			// 
			this.axFileVariables.Location = new System.Drawing.Point(0, 277);
			this.axFileVariables.Name = "axFileVariables";
			this.axFileVariables.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFileVariables.OcxState")));
			this.axFileVariables.Size = new System.Drawing.Size(283, 123);
			this.axFileVariables.TabIndex = 1;
			this.axFileVariables.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEventHandler(this.axFileVariables_BorderDragged);
			// 
			// axSequenceFileLabel
			// 
			this.axSequenceFileLabel.Location = new System.Drawing.Point(7, 402);
			this.axSequenceFileLabel.Name = "axSequenceFileLabel";
			this.axSequenceFileLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSequenceFileLabel.OcxState")));
			this.axSequenceFileLabel.Size = new System.Drawing.Size(352, 13);
			this.axSequenceFileLabel.TabIndex = 9;
			this.axSequenceFileLabel.TabStop = false;
			this.axSequenceFileLabel.Visible = false;
			this.axSequenceFileLabel.ConnectionActivity += new NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEventHandler(this.axSequenceFileLabel_ConnectionActivity);
			// 
			// axRunSequenceButton
			// 
			this.axRunSequenceButton.Location = new System.Drawing.Point(334, 416);
			this.axRunSequenceButton.Name = "axRunSequenceButton";
			this.axRunSequenceButton.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axRunSequenceButton.OcxState")));
			this.axRunSequenceButton.Size = new System.Drawing.Size(152, 24);
			this.axRunSequenceButton.TabIndex = 6;
			// 
			// axSequencesList
			// 
			this.axSequencesList.Location = new System.Drawing.Point(280, 277);
			this.axSequencesList.Name = "axSequencesList";
			this.axSequencesList.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSequencesList.OcxState")));
			this.axSequencesList.Size = new System.Drawing.Size(264, 123);
			this.axSequencesList.TabIndex = 2;
			this.axSequencesList.CreateContextMenu += new NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_CreateContextMenuEventHandler(this.axSequencesList_CreateContextMenu);
			// 
			// axFileSteps
			// 
			this.axFileSteps.Enabled = true;
			this.axFileSteps.Location = new System.Drawing.Point(0, 0);
			this.axFileSteps.Name = "axFileSteps";
			this.axFileSteps.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFileSteps.OcxState")));
			this.axFileSteps.Size = new System.Drawing.Size(544, 280);
			this.axFileSteps.TabIndex = 0;
			this.axFileSteps.CreateContextMenu += new NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEventHandler(this.axSequenceView_CreateContextMenu);
			this.axFileSteps.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEventHandler(this.axFileSteps_BorderDragged);
			// 
			// axEntryPoint2Button
			// 
			this.axEntryPoint2Button.Location = new System.Drawing.Point(166, 416);
			this.axEntryPoint2Button.Name = "axEntryPoint2Button";
			this.axEntryPoint2Button.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEntryPoint2Button.OcxState")));
			this.axEntryPoint2Button.Size = new System.Drawing.Size(152, 24);
			this.axEntryPoint2Button.TabIndex = 5;
			// 
			// axEntryPoint1Button
			// 
			this.axEntryPoint1Button.Location = new System.Drawing.Point(0, 416);
			this.axEntryPoint1Button.Name = "axEntryPoint1Button";
			this.axEntryPoint1Button.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEntryPoint1Button.OcxState")));
			this.axEntryPoint1Button.Size = new System.Drawing.Size(152, 24);
			this.axEntryPoint1Button.TabIndex = 4;
			// 
			// executionTab
			// 
			this.executionTab.Controls.Add(this.axExecutionVariables);
			this.executionTab.Controls.Add(this.axExecutionLabel);
			this.executionTab.Controls.Add(this.axCallStack);
			this.executionTab.Controls.Add(this.axExecutionSteps);
			this.executionTab.Controls.Add(this.axTerminateRestartBtn);
			this.executionTab.Controls.Add(this.axBreakResumeBtn);
			this.executionTab.Controls.Add(this.axThreads);
			this.executionTab.Location = new System.Drawing.Point(4, 22);
			this.executionTab.Name = "executionTab";
			this.executionTab.Size = new System.Drawing.Size(760, 452);
			this.executionTab.TabIndex = 1;
			this.executionTab.Text = "EXECUTION_TAB";
			// 
			// axExecutionVariables
			// 
			this.axExecutionVariables.Location = new System.Drawing.Point(0, 296);
			this.axExecutionVariables.Name = "axExecutionVariables";
			this.axExecutionVariables.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axExecutionVariables.OcxState")));
			this.axExecutionVariables.Size = new System.Drawing.Size(367, 104);
			this.axExecutionVariables.TabIndex = 1;
			this.axExecutionVariables.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEventHandler(this.axExecutionVariables_BorderDragged);
			// 
			// axExecutionLabel
			// 
			this.axExecutionLabel.Location = new System.Drawing.Point(9, 401);
			this.axExecutionLabel.Name = "axExecutionLabel";
			this.axExecutionLabel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axExecutionLabel.OcxState")));
			this.axExecutionLabel.Size = new System.Drawing.Size(331, 13);
			this.axExecutionLabel.TabIndex = 6;
			this.axExecutionLabel.TabStop = false;
			this.axExecutionLabel.Visible = false;
			this.axExecutionLabel.ConnectionActivity += new NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEventHandler(this.axExecutionLabel_ConnectionActivity);
			// 
			// axCallStack
			// 
			this.axCallStack.Location = new System.Drawing.Point(364, 296);
			this.axCallStack.Name = "axCallStack";
			this.axCallStack.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCallStack.OcxState")));
			this.axCallStack.Size = new System.Drawing.Size(208, 104);
			this.axCallStack.TabIndex = 2;
			this.axCallStack.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_BorderDraggedEventHandler(this.axCallStack_BorderDragged);
			// 
			// axExecutionSteps
			// 
			this.axExecutionSteps.Enabled = true;
			this.axExecutionSteps.Location = new System.Drawing.Point(0, 0);
			this.axExecutionSteps.Name = "axExecutionSteps";
			this.axExecutionSteps.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axExecutionSteps.OcxState")));
			this.axExecutionSteps.Size = new System.Drawing.Size(762, 296);
			this.axExecutionSteps.TabIndex = 0;
			this.axExecutionSteps.CreateContextMenu += new NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEventHandler(this.axExecutionView_CreateContextMenu);
			this.axExecutionSteps.BorderDragged += new NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEventHandler(this.axExecutionSteps_BorderDragged);
			// 
			// axTerminateRestartBtn
			// 
			this.axTerminateRestartBtn.Location = new System.Drawing.Point(145, 416);
			this.axTerminateRestartBtn.Name = "axTerminateRestartBtn";
			this.axTerminateRestartBtn.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTerminateRestartBtn.OcxState")));
			this.axTerminateRestartBtn.Size = new System.Drawing.Size(127, 24);
			this.axTerminateRestartBtn.TabIndex = 5;
			// 
			// axBreakResumeBtn
			// 
			this.axBreakResumeBtn.Location = new System.Drawing.Point(2, 416);
			this.axBreakResumeBtn.Name = "axBreakResumeBtn";
			this.axBreakResumeBtn.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBreakResumeBtn.OcxState")));
			this.axBreakResumeBtn.Size = new System.Drawing.Size(127, 24);
			this.axBreakResumeBtn.TabIndex = 4;
			// 
			// axThreads
			// 
			this.axThreads.Location = new System.Drawing.Point(575, 296);
			this.axThreads.Name = "axThreads";
			this.axThreads.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axThreads.OcxState")));
			this.axThreads.Size = new System.Drawing.Size(185, 104);
			this.axThreads.TabIndex = 3;
			// 
			// reportTab
			// 
			this.reportTab.Controls.Add(this.axReportView);
			this.reportTab.Location = new System.Drawing.Point(4, 22);
			this.reportTab.Name = "reportTab";
			this.reportTab.Size = new System.Drawing.Size(760, 452);
			this.reportTab.TabIndex = 2;
			this.reportTab.Text = "REPORT_TAB";
			// 
			// axReportView
			// 
			this.axReportView.Enabled = true;
			this.axReportView.Location = new System.Drawing.Point(0, 0);
			this.axReportView.Name = "axReportView";
			this.axReportView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axReportView.OcxState")));
			this.axReportView.Size = new System.Drawing.Size(635, 451);
			this.axReportView.TabIndex = 0;
			// 
			// axApplicationMgr
			// 
			this.axApplicationMgr.Enabled = true;
			this.axApplicationMgr.Location = new System.Drawing.Point(656, 0);
			this.axApplicationMgr.Name = "axApplicationMgr";
			this.axApplicationMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axApplicationMgr.OcxState")));
			this.axApplicationMgr.Size = new System.Drawing.Size(32, 32);
			this.axApplicationMgr.TabIndex = 20;
			this.axApplicationMgr.ExitApplication += new System.EventHandler(this.axApplicationMgr_ExitApplication);
			this.axApplicationMgr.Wait += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_WaitEventHandler(this.axApplicationMgr_Wait);
			this.axApplicationMgr.ReportError += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_ReportErrorEventHandler(this.axApplicationMgr_ReportError);
			this.axApplicationMgr.PostCommandExecute += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_PostCommandExecuteEventHandler(this.axApplicationMgr_PostCommandExecute);
			this.axApplicationMgr.QueryShutdown += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_QueryShutdownEventHandler(this.axApplicationMgr_QueryShutdown);
			this.axApplicationMgr.DisplaySequenceFile += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplaySequenceFileEventHandler(this.axApplicationMgr_DisplaySequenceFile);
			this.axApplicationMgr.DisplayExecution += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayExecutionEventHandler(this.axApplicationMgr_DisplayExecution);
			this.axApplicationMgr.DisplayReport += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayReportEventHandler(this.axApplicationMgr_DisplayReport);
			this.axApplicationMgr.StartExecution += new NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_StartExecutionEventHandler(this.axApplicationMgr_StartExecution);
			this.axApplicationMgr.EditModeChanged += new System.EventHandler(this.axApplicationMgr_EditModeChanged);
			// 
			// axExecutionViewMgr
			// 
			this.axExecutionViewMgr.Enabled = true;
			this.axExecutionViewMgr.Location = new System.Drawing.Point(752, 0);
			this.axExecutionViewMgr.Name = "axExecutionViewMgr";
			this.axExecutionViewMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axExecutionViewMgr.OcxState")));
			this.axExecutionViewMgr.Size = new System.Drawing.Size(32, 32);
			this.axExecutionViewMgr.TabIndex = 21;
			this.axExecutionViewMgr.ExecutionChanged += new NationalInstruments.TestStand.Interop.UI.Ax._ExecutionViewMgrEvents_ExecutionChangedEventHandler(this.axExecutionViewMgr_ExecutionChanged);
			// 
			// axSequenceFileViewMgr
			// 
			this.axSequenceFileViewMgr.Enabled = true;
			this.axSequenceFileViewMgr.Location = new System.Drawing.Point(704, 0);
			this.axSequenceFileViewMgr.Name = "axSequenceFileViewMgr";
			this.axSequenceFileViewMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSequenceFileViewMgr.OcxState")));
			this.axSequenceFileViewMgr.Size = new System.Drawing.Size(32, 32);
			this.axSequenceFileViewMgr.TabIndex = 22;
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(928, 494);
			this.Controls.Add(this.axStatusBar);
			this.Controls.Add(this.axListBar);
			this.Controls.Add(this.axSequenceFileViewMgr);
			this.Controls.Add(this.axExecutionViewMgr);
			this.Controls.Add(this.axApplicationMgr);
			this.Controls.Add(this.tabControl);
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TESTSTAND_USER_INTERFACE";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MenuStart += new System.EventHandler(this.MainForm_MenuStart);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			((System.ComponentModel.ISupportInitialize)(this.axListBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axStatusBar)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.fileTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axInsertionPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axFileVariables)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequenceFileLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axRunSequenceButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequencesList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axFileSteps)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axEntryPoint2Button)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axEntryPoint1Button)).EndInit();
			this.executionTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axExecutionVariables)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axCallStack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionSteps)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axTerminateRestartBtn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axBreakResumeBtn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axThreads)).EndInit();
			this.reportTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axReportView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axApplicationMgr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axExecutionViewMgr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axSequenceFileViewMgr)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			LaunchTestStandApplicationInNewDomain.LaunchProtected(
				new LaunchTestStandApplicationInNewDomain.MainEntryPointDelegateWithArgs(MainEntryPoint), 
				args,
				"TestStand C# UI",
				new LaunchTestStandApplicationInNewDomain.DisplayErrorMessageDelegate(DisplayError), 
                true);
		}

		static private void DisplayError(string caption, string message)
		{
			MessageBox.Show(message, caption);
		}

		static private void MainEntryPoint(string[] args)
		{
			NationalInstruments.TestStand.Utility.ApplicationWrapper.Run(new MainForm());
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			SplashScreen	splashScreen = null;

			try
			{
				if (!this.axApplicationMgr.ApplicationWillExitOnStart)
					splashScreen = new SplashScreen();  // show splash screen

				// If this UI is running in a CLR other than the one TestStand uses,
				// then it needs its own GCTimer for that version of the CLR. If it's running in the
				// same CLR as TestStand then the engine's gctimer enabled by the ApplicationMgr
				// is sufficient. See the API help for Engine.DotNetGarbageCollectionInterval for more details.
				if (System.Environment.Version.ToString() != this.axApplicationMgr.GetEngine().DotNetCLRVersion)
					this.GCTimer.Enabled = true;

				// localize error dialog title bar
				object found;
				errorDlgTitle = this.axApplicationMgr.GetEngine().GetResourceString("TSUI_OI_MAIN_PANEL", "ERR_BOX_TITLE", errorDlgTitle, out found);

				// this application allows setting of breakpoints on sequences files, so let them persist
				this.axApplicationMgr.GetEngine().PersistBreakpoints = true;

				// connect controls that are always visible
                this.ConnectListBarPages();
                this.ConnectStatusBarPanes();

				// connect controls on the Sequence File tab
				this.axSequenceFileViewMgr.ConnectSequenceView(this.axFileSteps);
				this.axSequenceFileViewMgr.ConnectCommand(this.axEntryPoint1Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 0, CommandConnectionOptions.CommandConnection_NoOptions);
				this.axSequenceFileViewMgr.ConnectCommand(this.axEntryPoint2Button, CommandKinds.CommandKind_ExecutionEntryPoints_Set, 1, CommandConnectionOptions.CommandConnection_NoOptions);
				this.axSequenceFileViewMgr.ConnectCommand(this.axRunSequenceButton, CommandKinds.CommandKind_RunCurrentSequence, 0, CommandConnectionOptions.CommandConnection_NoOptions);
                this.axSequenceFileViewMgr.ConnectCaption(this.axSequenceFileLabel, CaptionSources.CaptionSource_CurrentSequenceFile, false);
				this.axSequenceFileViewMgr.ConnectVariables(this.axFileVariables);
				this.axSequenceFileViewMgr.ConnectInsertionPalette(this.axInsertionPalette);
				this.axSequenceFileViewMgr.ConnectSequenceList(this.axSequencesList).SetColumnVisible(SeqListConnectionColumns.SeqListConnectionColumn_Comments, true);
				
				// connect controls on the Execution tab
				this.axExecutionViewMgr.ConnectExecutionView(this.axExecutionSteps, ExecutionViewOptions.ExecutionViewConnection_NoOptions);
				this.axExecutionViewMgr.ConnectVariables(this.axExecutionVariables);
				this.axExecutionViewMgr.ConnectCallStack(this.axCallStack);
				this.axExecutionViewMgr.ConnectThreadList(this.axThreads);
				this.axExecutionViewMgr.ConnectCommand(this.axBreakResumeBtn, CommandKinds.CommandKind_BreakResume, 0, CommandConnectionOptions.CommandConnection_NoOptions);
				this.axExecutionViewMgr.ConnectCommand(this.axTerminateRestartBtn, CommandKinds.CommandKind_TerminateRestart, 0, CommandConnectionOptions.CommandConnection_NoOptions);
                this.axExecutionViewMgr.ConnectCaption(this.axExecutionLabel, CaptionSources.CaptionSource_CurrentExecution, false);

				// connect controls on the Report tab
				this.axExecutionViewMgr.ConnectReportView(this.axReportView);
				
				// start up the TestStand User Interface Components. this also logs in the user
				this.axApplicationMgr.Start();

				// build the menubar for the first time (must call initially to ensure MenuStart event is sent for shortcut keys to TestStand command items)
                this.RebuildMenuBar();	
				
				// localize strings in top level menu items and controls
				this.localizer = new Localizer(this.axApplicationMgr.GetEngine(), this.axApplicationMgr);
				this.localizer.LocalizeForm(this, "TSUI_OI_MAIN_PANEL", true);		// localize .net controls and TestStand UI Controls

				// remember window and control positions from last time
				LayoutPersister.LoadSizes(this.axApplicationMgr, this.axListBar, this.tabControl, this.axFileSteps, this.axSequencesList, this.axFileVariables, this.axInsertionPalette, this.axExecutionSteps, this.axCallStack, this.axThreads, this.axExecutionVariables);
				LayoutPersister.LoadBounds(this.axApplicationMgr, this);

				// decide which tab pages to initially show
                this.ShowAppropriateTabs();

				// give initial control focus to the step list control
				this.axFileSteps.Select();
			}
			catch (Exception exception) 
			{
				MessageBox.Show(this, exception.Message);

				if (splashScreen != null)
					splashScreen.Hide();

				Application.Exit();
			}

			if (splashScreen != null)
				splashScreen.Hide();
		}

		// if the execution has a report, switch to the report tab either immediately if the execution is visible, or, whenever the execution is viewed next
		private void ShowReport(Execution execution)
		{
			// switch to report view when this execution is next viewed
			execution.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, (double)TabPageIdentifier.Report); // activate the reportTab when the user views this execution

			if (execution.Id == this.axExecutionViewMgr.Execution.Id) // is this execution the currently displayed execution?			
                this.ShowAppropriateTabs();  // switch to report view tab now
		}

		// show the sequence file list and execution list in list bar pages
		private void ConnectListBarPages()
		{
			// connect listbar page 0 to SequenceFileList
			this.axSequenceFileViewMgr.ConnectSequenceFileList(this.axListBar.Pages[0], false);

			// connect listbar page 1 to ExecutionList
			ExecutionListConnection connection = this.axExecutionViewMgr.ConnectExecutionList(this.axListBar.Pages[1]);
			// display the execution name on the first line, the serial number (if any) on the next line, the socket index (if any) on the next line, and the model execution state on the last line (the expression string looks complicated here because we have to escape the quotes the C# compiler.)
			connection.DisplayExpression = @"""%CurrentExecution%\n"" + (""%UUTSerialNumber%"" == """" ? """" : (ResStr(""TSUI_OI_MAIN_PANEL"", ""SERIAL_NUMBER"") + "" %UUTSerialNumber%\n"")) + (""%TestSocketIndex%"" == """" ? """" : ResStr(""TSUI_OI_MAIN_PANEL"", ""SOCKET_NUMBER"") + "" %TestSocketIndex%\n"") + ""%ModelState%""";
		}

		private void ConnectStatusBarPanes()
		{
			StatusBarPanes	panes = this.axStatusBar.Panes;

			// User
			this.axApplicationMgr.ConnectCaption(panes["User"], CaptionSources.CaptionSource_UserName, false);
			// Engine Environment
			this.axExecutionViewMgr.ConnectCaption(panes["EngineEnvironment"], CaptionSources.CaptionSource_EngineEnvironment, false).LongName = false;
			// File Process Model
			this.axSequenceFileViewMgr.ConnectCaption(panes["FileModel"], CaptionSources.CaptionSource_CurrentProcessModelFile, false).LongName = false;
			// Execution Process Model
			this.axExecutionViewMgr.ConnectCaption(panes["ExecutionModel"], CaptionSources.CaptionSource_CurrentProcessModelFile, false).LongName = false;
			// File Selected Steps
			this.axSequenceFileViewMgr.ConnectCaption(panes["FileSelectedSteps"], CaptionSources.CaptionSource_SelectedSteps_ZeroBased, false);
			// File Number of Steps
			this.axSequenceFileViewMgr.ConnectCaption(panes["FileNumberOfSteps"], CaptionSources.CaptionSource_NumberOfSteps, false);
			// Execution Selected Steps
			this.axExecutionViewMgr.ConnectCaption(panes["ExecutionSelectedSteps"], CaptionSources.CaptionSource_SelectedSteps_ZeroBased, false);
			// Execution Number of Steps
			this.axExecutionViewMgr.ConnectCaption(panes["ExecutionNumberOfSteps"], CaptionSources.CaptionSource_NumberOfSteps, false);
			// Report Location
			this.axExecutionViewMgr.ConnectCaption(panes["ReportLocation"], CaptionSources.CaptionSource_ReportLocation, true);
			// Progress Text
			this.axExecutionViewMgr.ConnectCaption(panes["ProgressText"], CaptionSources.CaptionSource_ProgressText, false);
			// Progress Percent Text
			this.axExecutionViewMgr.ConnectCaption(panes["ProgressPercent"], CaptionSources.CaptionSource_ProgressPercent, false);
			// Progress Percent Bar
			this.axExecutionViewMgr.ConnectNumeric(panes["ProgressPercent"], NumericSources.NumericSource_ProgressPercent);
		}

		// handle request to close form (via Windows close box, for example)
		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Don't set e.Cancel to true if windows is shutting down.
			// Doing so would prevent windows from shutting down or logging out.
			if (!sessionEnding)
			{
				// initiate shutdown and cancel close if shutdown is not complete.  The applicationMgr will
				// send the ExitApplication event when shutdown is complete and we can close then
				if (axApplicationMgr.Shutdown() == false)
					e.Cancel = true;
				else
				{
					this.localizer = null;

					// closing now, add all tabs so that they are disposed
					this.tabControl.TabPages.Clear();
                    this.tabControl.TabPages.Add(this.reportTab);
                    this.tabControl.TabPages.Add(this.executionTab);
                    this.tabControl.TabPages.Add(this.fileTab);
				}
			}
		}

		protected override void WndProc(ref Message msg)
		{
			// set the sessionEnding flag so I will know the form is closing because the user
			// is trying to shutdown, restart, or logoff windows
			if (msg.Msg == WM_QUERYENDSESSION)
			{
				sessionEnding = true;
				Application.Exit();
			}

			base.WndProc(ref msg);
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			if (!programmaticallyUpdatingTabPages)	// filter out programmatically triggered activation events that might be due only to hidden tabs being made visible again
			{
				// remember which tab is active so when execution is revisited in the future, we can activate the same tab
				// is the new tab an execution tab and there is a current execution?
				if (this.axListBar.CurrentPage == EXECUTIONS_PAGE_INDEX && this.axExecutionViewMgr.Execution != null) 
				{
					TabPageIdentifier	thisTab = this.tabControl.SelectedIndex == 0 ? TabPageIdentifier.Execution : TabPageIdentifier.Report;
					
					// store the activated tab index in a custom property added to the execution
					this.axExecutionViewMgr.Execution.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, (double)thisTab);
				}
			}

            this.ShowAppropriateStatusBarPanes();		
		}

		// It is now ok to exit, close the form
		private void axApplicationMgr_ExitApplication(object sender, System.EventArgs e)
		{
			// discard any current menu items that were inserted by Menus.InsertCommandsInMenu. These menu items might refer to TestStand objects, so delete them before the engine is destroyed. Note that it is too early to do this in QueryShutdown because a menu might be used after QueryShutdown returns, particularly if an unload callback runs.
			Menus.RemoveMenuCommands(this.mainMenu1, false, false);
			Environment.ExitCode = this.axApplicationMgr.ExitCode;
			this.Close();

			TSHelper.DoSynchronousGCForCOMObjectDestruction();
		}

		// release any TestStand objects and save any settings here
		private void axApplicationMgr_QueryShutdown(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_QueryShutdownEvent e)
		{
			LayoutPersister.SaveSizes(this.axApplicationMgr, this.axListBar, this.tabControl, this.axFileSteps, this.axSequencesList, this.axFileVariables, this.axInsertionPalette, this.axExecutionSteps, this.axCallStack, this.axThreads, this.axExecutionVariables);
			LayoutPersister.SaveBounds(this.axApplicationMgr, this);
		}

		// ApplicationMgr sends this message when it's busy doing something so we know to display a hourglass cursor or an equivalent
		private void axApplicationMgr_Wait(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_WaitEvent e)
		{
			if (e.showWait)
				this.Cursor = Cursors.WaitCursor;
			else
				this.Cursor = Cursors.Default;
		}
		
		// the ApplicationMgr sends this event to request that the UI display the report for a particular execution
		private void axApplicationMgr_DisplayReport(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayReportEvent e)
		{
            this.ShowReport(e.exec);
		}	

		// the ApplicationMgr sends this event to request that the UI display a particular execution
		private void axApplicationMgr_DisplayExecution(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplayExecutionEvent e)
		{
			// bring application to front if we hit a breakpoint
			if (e.reason == ExecutionDisplayReasons.ExecutionDisplayReason_Breakpoint || e.reason == ExecutionDisplayReasons.ExecutionDisplayReason_BreakOnRunTimeError)
				this.Activate();

			// show this execution
			this.axExecutionViewMgr.Execution = e.exec;
			// show the executions page in the list bar
			this.axListBar.CurrentPage = EXECUTIONS_PAGE_INDEX;
			// in case we are already showing the executions page, ensure we switch to steps or report tab as appropriate
            this.ShowAppropriateTabs(); 
		}

		// the ApplicationMgr sends this event to request that the UI display a particular sequence file
		private void axApplicationMgr_DisplaySequenceFile(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_DisplaySequenceFileEvent e)
		{
			// show this sequence file
			this.axSequenceFileViewMgr.SequenceFile = e.file;
			// show the sequence files page in the list bar
			this.axListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX;
		}

		// the ApplicationMgr sends this event to report any error that occurs when the TestStand UI controls respond to user input (ie, an error 
		// other than one returned by a method you call directly). For example, if a TestStand menu command generates an error, this handler displays it
		private void axApplicationMgr_ReportError(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_ReportErrorEvent e)
		{
			Engine engine = axApplicationMgr.GetEngine();
			engine.DisplayErrorDialog(errorDlgTitle, e.errorMessage, e.errorCode, 0);
		}

		// the ApplicationMgr sends this event whenever an execution starts
		private void axApplicationMgr_StartExecution(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_StartExecutionEvent e)
		{
			// add a custom property to the execution to store which tab we are displaying for this execution. Initially show the execution tab
			e.exec.AsPropertyObject().SetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing, (double)TabPageIdentifier.Execution);		
		}

		// the ExecutionViewMgr sends this event whenever a new execution is selected
		private void axExecutionViewMgr_ExecutionChanged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ExecutionViewMgrEvents_ExecutionChangedEvent e)
		{
			// switch to report or steps tab depending on what the execution displayed last
            this.ShowAppropriateTabs();
		}

		// build a context menu for a control that has been right-clicked
		private void BuildCommandSetMenu(CommandKinds commandSet, int menuHandle)
		{
			Commands	cmds = this.axApplicationMgr.NewCommands();
			int			unused;

			// insert items for the specified command or command set in the context menu
			cmds.InsertKind(commandSet, GetActiveViewManager(), -1, "", "", out unused);
			Menus.RemoveInvalidShortcutKeys(cmds);	// remove any shortcuts that .NET does not support
			cmds.InsertIntoWin32Menu(menuHandle, -1, true, true);   // we are using the context menu that the control provides because it requires fewer lines of code. We could have built a .net context menu instead. If you build a .net context menu, you should note that they do not display for ActiveX controls. However, you can display a context menu for the parent form in response to a ActiveX control right-mouse-click event and achieve the same result. To display the context menu at the correct location, convert the click coordinates from control coordinates to form coordinates
		}

		// create a right-click menu for the SequenceView control that displays execution steps
		private void axExecutionView_CreateContextMenu(object sender, NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEvent e)
		{
            this.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceViewContextMenu_Set, e.menuHandle);
		}

		// create a right-click menu for the SequenceView control that displays sequence file steps
		private void axSequenceView_CreateContextMenu(object sender, NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_CreateContextMenuEvent e)
		{
            this.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceViewContextMenu_Set, e.menuHandle);
		}

		// create a right-click menu for the ListBar control
		private void axListBar_CreateContextMenu(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CreateContextMenuEvent e)
		{
            this.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultListBarContextMenu_Set, e.menuHandle);
		}

		// create a right-click menu for the sequence list box control
		private void axSequencesList_CreateContextMenu(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_CreateContextMenuEvent e)
		{
            this.BuildCommandSetMenu(CommandKinds.CommandKind_DefaultSequenceListContextMenu_Set, e.menuHandle);
		}

		// the ListBar sends this event when the listbar switches to a new page
		private void axListBar_CurPageChanged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_CurPageChangedEvent e)
		{
            this.ShowAppropriateTabs();
            this.UpdateWindowTitle();    // even if the current file and execution haven't changed, what we show in the title bar changes depending on which listbar page is active
		}

		// append the caption for the selected file or execution to the application window title
		private void UpdateWindowTitle()
		{
			object unused;
			string title = this.axApplicationMgr.GetEngine().GetResourceString("TSUI_OI_MAIN_PANEL", "TESTSTAND_USER_INTERFACE", "", out unused);
			string documentDescription = null;

			if (this.axListBar.CurrentPage == SEQUENCE_FILES_PAGE_INDEX)	// sequence files are visible
				documentDescription += this.axSequenceFileViewMgr.GetCaptionText(CaptionSources.CaptionSource_CurrentSequenceFile, false, "");
			else	// executions are visible
				documentDescription += this.axExecutionViewMgr.GetCaptionText(CaptionSources.CaptionSource_CurrentExecution, false, "");
			
			if (documentDescription != null && documentDescription != "")
				title += " - " + documentDescription;

			this.Text = title;
		}

        // a hidden label control is connected to CaptionSource_CurrentSequenceFile so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current file
        private void axSequenceFileLabel_ConnectionActivity(object sender, NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEvent e)
        {
            this.UpdateWindowTitle();
        }

        // a hidden label control is connected to CaptionSource_CurrentExecution so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current execution
        private void axExecutionLabel_ConnectionActivity(object sender, NationalInstruments.TestStand.Interop.UI.Ax._LabelEvents_ConnectionActivityEvent e)
        {
            this.UpdateWindowTitle();
        }

		// based on the current listbar page, show and hide the tabs that appear in the space to the right of the listbar
		private void ShowAppropriateTabs()
		{
			programmaticallyUpdatingTabPages = true;	

			TabPage	pageToDisplay = null;

			if (this.axListBar.CurrentPage == SEQUENCE_FILES_PAGE_INDEX)
				pageToDisplay = this.fileTab;
			else			
			{	// we are viewing an execution...
				pageToDisplay = this.executionTab; // default tab

				// if the report tab page was the last tab displayed for this execution, re-activate it instead
				if (this.axExecutionViewMgr.Execution != null)
					if (TabPageIdentifier.Report == (TabPageIdentifier)this.axExecutionViewMgr.Execution.AsPropertyObject().GetValNumber("NIUI.LastActiveTab", PropertyOptions.PropOption_InsertIfMissing))
						pageToDisplay = this.reportTab;					
			}

			if ((this.tabControl.SelectedTab != pageToDisplay) ||	// prevent flashing if tab page has not changed
				(this.tabControl.TabCount == 3))					// if all three tabs are visible at once, then we have never called this function before and we need to display the correct tabs
			{
				// store the current Active control as tabControl.SelectedTab changes focus to the first control on the tab
				Control curActiveCtl = this.ActiveControl;

				// show and hide tabs as appropriate (.NET doesn't let you hide a tab page, so we remove all pages and add back the ones we want to show)
				this.tabControl.TabPages.Clear();


				if (pageToDisplay == this.fileTab)
					this.tabControl.TabPages.Add(this.fileTab);
				else
				{
					this.tabControl.TabPages.Add(this.executionTab);
					this.tabControl.TabPages.Add(this.reportTab);
				}

				this.tabControl.SelectedTab = pageToDisplay;

				// set the current Active control as tabControl.SelectedTab changes focus to the first control on the tab
				if (curActiveCtl != null && curActiveCtl.CanFocus)
					this.ActiveControl = curActiveCtl;
			}

			programmaticallyUpdatingTabPages = false;

            this.ShowAppropriateStatusBarPanes(); // needed because .net doesn't send the tabControl_SelectedIndexChanged when adding the first tab to a tab control
		}

		private void ShowAppropriateStatusBarPanes()
		{
			if (this.tabControl.SelectedIndex >= 0)
			{
				if (this.axListBar.CurrentPage == SEQUENCE_FILES_PAGE_INDEX) // if only the files tab is visible
					axStatusBar.ShowPanes("User, EngineEnvironment, FileModel, FileSelectedSteps, FileNumberOfSteps");
				else		
				if (this.tabControl.SelectedIndex == 0)	// execution tab is selected
					axStatusBar.ShowPanes("User, EngineEnvironment, ExecutionModel, ExecutionSelectedSteps, ExecutionNumberOfSteps, ProgressText, ProgressPercent");
				else									// report tab is selected
					axStatusBar.ShowPanes("User, EngineEnvironment, ExecutionModel, ReportLocation, ProgressText, ProgressPercent");
			}
		}

		// The menu has been accessed, rebuild it with currently applicable items
		private void MainForm_MenuStart(object sender, System.EventArgs e)
		{
            this.RebuildMenuBar();
		}

		// return the SequenceFileViewMgr or the ExecutionViewMgr depending on whether we are displaying sequence files or executions
		private object GetActiveViewManager()
		{
			if (this.axListBar.CurrentPage == SEQUENCE_FILES_PAGE_INDEX)	// sequence files are visible, sequence file menu commands apply
				return this.axSequenceFileViewMgr;
			else if (this.axListBar.CurrentPage == EXECUTIONS_PAGE_INDEX)	// executions are visible, execution menu commands apply
				return this.axExecutionViewMgr;
			else
				return null;
		}

		// make sure all menus have appropriate items with the correct enabled states
		private void RebuildMenuBar()
		{
			try
			{
                // avoid flicker that was introduced with .NET 2.0
                Menus.BeginUpdate(this);

				// discard any current menu items that were inserted by Menus.InsertCommandsInMenu
				Menus.RemoveMenuCommands(this.mainMenu1, false, false);

				// determine which view manager menu commands apply to
				object	viewMgr = GetActiveViewManager();

				// rebuild File menu
				ArrayList	fileMenuCommands = new ArrayList();
				fileMenuCommands.Add(CommandKinds.CommandKind_DefaultFileMenu_Set);				// add all the usual commands in a File menu
				Menus.InsertCommandsInMenu(fileMenuCommands, this.fileMenu, null, viewMgr, true);

				// rebuild Edit menu
				ArrayList	editMenuCommands = new ArrayList();
				editMenuCommands.Add(CommandKinds.CommandKind_DefaultEditMenu_Set);				// add all the usual commands in an Edit menu
				Menus.InsertCommandsInMenu(editMenuCommands, this.editMenu, null, viewMgr, true);

				// rebuild Execute menu
				ArrayList	executeMenuCommands = new ArrayList();
				executeMenuCommands.Add(CommandKinds.CommandKind_DefaultExecuteMenu_Set);		// add all the usual commands in a Execute menu				
				Menus.InsertCommandsInMenu(executeMenuCommands, this.executeMenu, null, viewMgr, true);

				// rebuild Debug menu
				ArrayList	debugMenuCommands = new ArrayList();
				debugMenuCommands.Add(CommandKinds.CommandKind_DefaultDebugMenu_Set);			// add all the usual commands in a Debug menu
				Menus.InsertCommandsInMenu(debugMenuCommands, this.debugMenu, null, viewMgr, true);

				// rebuild Configure menu
				ArrayList	configureMenuCommands = new ArrayList();
				configureMenuCommands.Add(CommandKinds.CommandKind_DefaultConfigureMenu_Set);	// add all the usual commands in a Configure menu
                if (axApplicationMgr.IsEditor)
                    configureMenuCommands.Add(CommandKinds.CommandKind_ConfigureEngineEnvironment);		
				Menus.InsertCommandsInMenu(configureMenuCommands, this.configureMenu, null, viewMgr, true);

				// rebuild Tools menu
				ArrayList	toolsMenuCommands = new ArrayList();
				toolsMenuCommands.Add(CommandKinds.CommandKind_DefaultToolsMenu_Set);			// add all the usual commands in a Tools menu

				Menus.InsertCommandsInMenu(toolsMenuCommands, this.toolsMenu, null, viewMgr, true);

				// rebuild the Help menu. Note that the help menu already contains an "About..." item, which is not a TestStand command item
				ArrayList	helpMenuCommands = new ArrayList();
				helpMenuCommands.Add(CommandKinds.CommandKind_Separator);				// separates the existing About... item
				helpMenuCommands.Add(CommandKinds.CommandKind_DefaultHelpMenu_Set);		// add all the usual commands in a Help menu. Note that most help items appear only when in Edit mode.
				Menus.InsertCommandsInMenu(helpMenuCommands, this.helpMenu, null, viewMgr, true);

				// remove duplicate separators and shortcut keys
				Menus.CleanupMenu(this.mainMenu1);

                Menus.EndUpdate();
            }
			catch (Exception theException)
			{
				MessageBox.Show(this, theException.Message, errorDlgTitle);
			}	
		}				

		// Release all objects periodically.  .NET lets COM objects pile up on the managed heap, seemingly even objects you don't know about such
		// as parameters to unhandled ActiveX events.  This timer ensures that all COM objects are released in a timely manner,
		// thus preventing the performance hiccup that could occur when .NET finally decides to collect garbage. Also, this timer
		// ensures that actions triggered by object destruction run in a timely manner. For example: sequence file unload callbacks.
		private void GCTimerTick(object sender, System.EventArgs e)
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, false); // force .net garbage collection
		}				

		// displays the about box when the user selects Help>>About...
		private void aboutBoxItem_Click(object sender, System.EventArgs e)
		{
			using (AboutBox aboutBox = new AboutBox(this.localizer))
			{
				aboutBox.ShowDialog(this);
			}
		}

		// adjust controls to fit within current window size
		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (this.Visible)	// filter out the resize events that occur before initialization is complete
				this.ArrangeControls();				
		}

		// arrange the controls for the first time
		private void MainForm_Shown(object sender, EventArgs e)
		{
			this.ArrangeControls();
		}		

		// adjust controls to fit within current window size
		private void ArrangeControls()
        {
			if (this.WindowState == FormWindowState.Minimized)	// don't shrink controls and inadvertently close subpanes when the window is minimized
				return; 

            const int buttonVerticalMargin = 6;
            const int buttonHorizontalSpacing = 12;

			// resize controls to fit current window height
			this.tabControl.Height = this.ClientRectangle.Height - this.axStatusBar.Height;
			this.axListBar.Height = this.tabControl.Height;			

            // there is a vertical drag bar between listbar and tab control
            Splitters.DivideSpaceBetweenPanes(true, 0, this.ClientRectangle.Width, this.axListBar, this.tabControl);

            // obtain new size of tab area interior
            Rectangle tabInteriorRect = this.tabControl.DisplayRectangle;

            // File and Execution Tabs have buttons at the bottom. figure out how much space is left after the buttons are placed
            int buttonHeight = this.axBreakResumeBtn.Height;
            int buttonTop = Math.Max(tabInteriorRect.Height - buttonHeight - buttonVerticalMargin, 40 + buttonVerticalMargin);

			// compute the size of the tab control interior area minus space for the buttons. this area is shared by controls that be resized
            // don't shrink control area to less than 40 by 40 pixels so that if the window is resized to have no visible area, the controls aren't made so small that we forget their relative sizes
            int resizableTabControlAreaHeight = Math.Max(buttonTop - buttonVerticalMargin, 40);
            int resizableTabControlAreaWidth = Math.Max(tabInteriorRect.Width, 40);

            // FILE TAB:

            // place the file tab buttons
            this.axEntryPoint1Button.Top = buttonTop;
            this.axEntryPoint2Button.Top = buttonTop;
            this.axRunSequenceButton.Top = buttonTop;
            this.axEntryPoint2Button.Left = axEntryPoint1Button.Right + buttonHorizontalSpacing;
            this.axRunSequenceButton.Left = axEntryPoint2Button.Right + buttonHorizontalSpacing;

            // hide editor-only controls if not an editor
            bool isEditor = this.axApplicationMgr.IsEditor;
            this.axInsertionPalette.Visible = isEditor;

            // there is a horizontal drag bar between FileSteps and SequenceList/FileVariables
			Splitters.DivideSpaceBetweenPanes(false, 0, resizableTabControlAreaHeight, this.axFileSteps, this.axSequencesList);

			// file variables has same height and top as the sequences list
			this.axFileVariables.Top = this.axSequencesList.Top;
            this.axFileVariables.Height = this.axSequencesList.Height;

            // there is a vertical drag bar between the FileSteps and the Insertion Palette
            if (!isEditor)
                this.axFileSteps.Width = resizableTabControlAreaWidth; // Insertion Palette is not visible, nothing to split
            else
				Splitters.DivideSpaceBetweenPanes(true, 0, resizableTabControlAreaWidth, this.axFileSteps, this.axInsertionPalette);

			this.axInsertionPalette.Height = resizableTabControlAreaHeight;

			// there is a vertical drag bar between the FileVariables and the SequencesList 
			Splitters.DivideSpaceBetweenPanes(true, 0, this.axFileSteps.Width, this.axFileVariables, this.axSequencesList);

            // EXECUTION TAB:

            // place the execution tab buttons
            this.axBreakResumeBtn.Top = buttonTop;
            this.axTerminateRestartBtn.Top = buttonTop;
            this.axTerminateRestartBtn.Left = axBreakResumeBtn.Right + buttonHorizontalSpacing;

            // size ExecutionSteps to tab control interior width
            this.axExecutionSteps.Width = resizableTabControlAreaWidth;

            // there is a horizontal drag bar between ExecutionSteps and CallStack/Threads/ExecutionVariablesView
			Splitters.DivideSpaceBetweenPanes(false, 0, resizableTabControlAreaHeight, this.axExecutionSteps, this.axCallStack);

			// execution variables and threads have the same height and top as the callstack
            this.axExecutionVariables.Height = this.axThreads.Height = this.axCallStack.Height;
            this.axExecutionVariables.Top = this.axThreads.Top = this.axCallStack.Top;

			// there are vertical drag bars between the ExecutionVariables, CallStack, and Threads
			Splitters.DivideSpaceBetweenPanes(true, 0, this.axExecutionSteps.Width, axExecutionVariables, this.axCallStack, this.axThreads);

            // REPORT TAB:
            this.axReportView.Height = tabInteriorRect.Height;
            this.axReportView.Width = tabInteriorRect.Width;

            this.Update();  // needed so that drawing keeps pace with drag bar movement		
        }

        // user dragged the vertical bar on the listbar that separates it from the tab control
        private void axListBar_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ListBarEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axListBar, this.tabControl, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
	        this.ArrangeControls();		
		}
        
        // user dragged the horizontal bar on the file steps that separates it from the sequence list and file variables
		private void axFileSteps_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axFileSteps, this.axSequencesList, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();					
		}

		// user dragged vertical bar on the file variables that separates it from the sequences list 
		private void axFileVariables_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axFileVariables, this.axSequencesList, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();
		}

		// user dragged the vertical bar on the insertion palette that separates it from the file step list
		private void axInsertionPalette_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._InsertionPaletteEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axInsertionPalette, this.axFileSteps, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();
		}
		
		// user dragged the horizontal bar on the execution steps lists that separates it from the call stack, thread list, and execution variables
        private void axExecutionSteps_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._SequenceViewEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axExecutionSteps, this.axCallStack, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();
		}

		// user dragged the vertical bar on the execution variables that separates it from the callstack
		private void axExecutionVariables_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._VariablesViewEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axExecutionVariables, this.axCallStack, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();
		}

        // user dragged the vertical bar on the callstack that separates it from the thread list
		private void axCallStack_BorderDragged(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ListBoxEvents_BorderDraggedEvent e)
		{
			Splitters.DragSplitter(this.axCallStack, this.axThreads, e.newX, e.newY, e.newWidth, e.newHeight, e.bordersChanged);
			this.ArrangeControls();
		}

		// user toggled edit mode. the only way to do that in this application is to type ctrl-shift-alt-insert, which is the edit mode toggle key this application specifies in designer for the ApplicationMgr control. 
		// to prevent edit mode from being toggled with a hotkey, set ApplicationMgr.EditModeShortcutKey to ShortcutKey_VK_NOT_A_KEY
		private void axApplicationMgr_EditModeChanged(object sender, EventArgs e)
		{
			this.axInsertionPalette.Width = Math.Max(this.axInsertionPalette.Width, 260);	// make sure the palette is wide enough in case it is going to be shown
			this.ArrangeControls();	// relayout the controls to reflect the change in edit mode
		}

		private void axApplicationMgr_PostCommandExecute(object sender, NationalInstruments.TestStand.Interop.UI.Ax._ApplicationMgrEvents_PostCommandExecuteEvent e)
		{
			if (e.command.Kind == CommandKinds.CommandKind_CloseCompletedExecutions)
			{
				if (this.axApplicationMgr.Executions.Count == 0)
				{
					// if we closed all the executions, switch to the files page instead of showing an empty executions page
					this.axListBar.CurrentPage = SEQUENCE_FILES_PAGE_INDEX;
				}
			}
		}
 	}
}


