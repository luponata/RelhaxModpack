﻿namespace RelhaxModpack
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.childProgressBar = new System.Windows.Forms.ProgressBar();
            this.FindWotExe = new System.Windows.Forms.OpenFileDialog();
            this.forceManuel = new System.Windows.Forms.CheckBox();
            this.formPageLink = new System.Windows.Forms.LinkLabel();
            this.parrentProgressBar = new System.Windows.Forms.ProgressBar();
            this.installRelhaxMod = new System.Windows.Forms.Button();
            this.uninstallRelhaxMod = new System.Windows.Forms.Button();
            this.cleanInstallCB = new System.Windows.Forms.CheckBox();
            this.cancerFontCB = new System.Windows.Forms.CheckBox();
            this.backupModsCheckBox = new System.Windows.Forms.CheckBox();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.SuperExtractionCB = new System.Windows.Forms.CheckBox();
            this.InstantExtractionCB = new System.Windows.Forms.CheckBox();
            this.createShortcutsCB = new System.Windows.Forms.CheckBox();
            this.saveUserDataCB = new System.Windows.Forms.CheckBox();
            this.saveLastInstallCB = new System.Windows.Forms.CheckBox();
            this.ShowInstallCompleteWindowCB = new System.Windows.Forms.CheckBox();
            this.notifyIfSameDatabaseCB = new System.Windows.Forms.CheckBox();
            this.clearLogFilesCB = new System.Windows.Forms.CheckBox();
            this.darkUICB = new System.Windows.Forms.CheckBox();
            this.clearCacheCB = new System.Windows.Forms.CheckBox();
            this.languageSelectionGB = new System.Windows.Forms.GroupBox();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.loadingImageGroupBox = new System.Windows.Forms.GroupBox();
            this.thirdGuardsLoadingImageRB = new System.Windows.Forms.RadioButton();
            this.standardImageRB = new System.Windows.Forms.RadioButton();
            this.findBugAddModLabel = new System.Windows.Forms.LinkLabel();
            this.cancelDownloadButton = new System.Windows.Forms.Button();
            this.DownloadTimer = new System.Windows.Forms.Timer(this.components);
            this.downloadProgress = new System.Windows.Forms.RichTextBox();
            this.viewTypeGB = new System.Windows.Forms.GroupBox();
            this.disableColorsCB = new System.Windows.Forms.CheckBox();
            this.disableBordersCB = new System.Windows.Forms.CheckBox();
            this.expandNodesDefault = new System.Windows.Forms.CheckBox();
            this.selectionLegacy = new System.Windows.Forms.RadioButton();
            this.selectionDefault = new System.Windows.Forms.RadioButton();
            this.donateLabel = new System.Windows.Forms.LinkLabel();
            this.fontSizeGB = new System.Windows.Forms.GroupBox();
            this.DPI275 = new System.Windows.Forms.RadioButton();
            this.DPI225 = new System.Windows.Forms.RadioButton();
            this.fontSize275 = new System.Windows.Forms.RadioButton();
            this.fontSize225 = new System.Windows.Forms.RadioButton();
            this.DPIAUTO = new System.Windows.Forms.RadioButton();
            this.DPI125 = new System.Windows.Forms.RadioButton();
            this.DPI175 = new System.Windows.Forms.RadioButton();
            this.DPI100 = new System.Windows.Forms.RadioButton();
            this.fontSize175 = new System.Windows.Forms.RadioButton();
            this.fontSize125 = new System.Windows.Forms.RadioButton();
            this.fontSize100 = new System.Windows.Forms.RadioButton();
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.DiscordServerLink = new System.Windows.Forms.LinkLabel();
            this.viewAppUpdates = new System.Windows.Forms.Button();
            this.viewDBUpdates = new System.Windows.Forms.Button();
            this.ErrorCounterLabel = new System.Windows.Forms.Label();
            this.VersionTable = new System.Windows.Forms.TableLayoutPanel();
            this.DatabaseVersionLabel = new System.Windows.Forms.Label();
            this.ApplicationVersionLabel = new System.Windows.Forms.Label();
            this.DiagnosticUtilitiesButton = new System.Windows.Forms.Button();
            this.UninstallModeGroupBox = new System.Windows.Forms.GroupBox();
            this.CleanUninstallModeRB = new System.Windows.Forms.RadioButton();
            this.SmartUninstallModeRB = new System.Windows.Forms.RadioButton();
            this.settingsGroupBox.SuspendLayout();
            this.languageSelectionGB.SuspendLayout();
            this.loadingImageGroupBox.SuspendLayout();
            this.viewTypeGB.SuspendLayout();
            this.fontSizeGB.SuspendLayout();
            this.VersionTable.SuspendLayout();
            this.UninstallModeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // childProgressBar
            // 
            this.childProgressBar.Location = new System.Drawing.Point(13, 553);
            this.childProgressBar.Name = "childProgressBar";
            this.childProgressBar.Size = new System.Drawing.Size(465, 16);
            this.childProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.childProgressBar.TabIndex = 11;
            // 
            // FindWotExe
            // 
            this.FindWotExe.Filter = "WorldOfTanks.exe|WorldOfTanks.exe";
            this.FindWotExe.Title = "Find WorldOfTanks.exe";
            // 
            // forceManuel
            // 
            this.forceManuel.Location = new System.Drawing.Point(6, 15);
            this.forceManuel.Name = "forceManuel";
            this.forceManuel.Size = new System.Drawing.Size(220, 17);
            this.forceManuel.TabIndex = 13;
            this.forceManuel.Text = "Force manual game detection";
            this.forceManuel.UseVisualStyleBackColor = true;
            this.forceManuel.CheckedChanged += new System.EventHandler(this.forceManuel_CheckedChanged);
            this.forceManuel.MouseEnter += new System.EventHandler(this.forceManuel_MouseEnter);
            this.forceManuel.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // formPageLink
            // 
            this.formPageLink.AutoSize = true;
            this.formPageLink.Location = new System.Drawing.Point(10, 594);
            this.formPageLink.Name = "formPageLink";
            this.formPageLink.Size = new System.Drawing.Size(132, 13);
            this.formPageLink.TabIndex = 16;
            this.formPageLink.TabStop = true;
            this.formPageLink.Text = "View Modpack Form Page";
            this.formPageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.formPageLink_LinkClicked);
            // 
            // parrentProgressBar
            // 
            this.parrentProgressBar.Location = new System.Drawing.Point(13, 531);
            this.parrentProgressBar.Name = "parrentProgressBar";
            this.parrentProgressBar.Size = new System.Drawing.Size(465, 16);
            this.parrentProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.parrentProgressBar.TabIndex = 17;
            // 
            // installRelhaxMod
            // 
            this.installRelhaxMod.Location = new System.Drawing.Point(245, 12);
            this.installRelhaxMod.Name = "installRelhaxMod";
            this.installRelhaxMod.Size = new System.Drawing.Size(232, 38);
            this.installRelhaxMod.TabIndex = 19;
            this.installRelhaxMod.Text = "Install Relhax Modpack";
            this.installRelhaxMod.UseVisualStyleBackColor = true;
            this.installRelhaxMod.Click += new System.EventHandler(this.installRelhaxMod_Click);
            // 
            // uninstallRelhaxMod
            // 
            this.uninstallRelhaxMod.Location = new System.Drawing.Point(245, 56);
            this.uninstallRelhaxMod.Name = "uninstallRelhaxMod";
            this.uninstallRelhaxMod.Size = new System.Drawing.Size(232, 37);
            this.uninstallRelhaxMod.TabIndex = 20;
            this.uninstallRelhaxMod.Text = "Uninstall Relhax Modpack";
            this.uninstallRelhaxMod.UseVisualStyleBackColor = true;
            this.uninstallRelhaxMod.Click += new System.EventHandler(this.UninstallRelhaxMod_Click);
            // 
            // cleanInstallCB
            // 
            this.cleanInstallCB.Checked = true;
            this.cleanInstallCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cleanInstallCB.Location = new System.Drawing.Point(6, 32);
            this.cleanInstallCB.Name = "cleanInstallCB";
            this.cleanInstallCB.Size = new System.Drawing.Size(220, 17);
            this.cleanInstallCB.TabIndex = 21;
            this.cleanInstallCB.Text = "Clean Installation (Recommended)";
            this.cleanInstallCB.UseVisualStyleBackColor = true;
            this.cleanInstallCB.CheckedChanged += new System.EventHandler(this.cleanInstallCB_CheckedChanged);
            this.cleanInstallCB.MouseEnter += new System.EventHandler(this.cleanInstallCB_MouseEnter);
            this.cleanInstallCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // cancerFontCB
            // 
            this.cancerFontCB.Location = new System.Drawing.Point(6, 49);
            this.cancerFontCB.Name = "cancerFontCB";
            this.cancerFontCB.Size = new System.Drawing.Size(220, 17);
            this.cancerFontCB.TabIndex = 23;
            this.cancerFontCB.Text = "Cancer font";
            this.cancerFontCB.UseVisualStyleBackColor = true;
            this.cancerFontCB.CheckedChanged += new System.EventHandler(this.cancerFontCB_CheckedChanged);
            this.cancerFontCB.MouseEnter += new System.EventHandler(this.cancerFontCB_MouseEnter);
            this.cancerFontCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // backupModsCheckBox
            // 
            this.backupModsCheckBox.Location = new System.Drawing.Point(6, 100);
            this.backupModsCheckBox.Name = "backupModsCheckBox";
            this.backupModsCheckBox.Size = new System.Drawing.Size(220, 32);
            this.backupModsCheckBox.TabIndex = 24;
            this.backupModsCheckBox.Text = "Backup current mods folder";
            this.backupModsCheckBox.UseVisualStyleBackColor = true;
            this.backupModsCheckBox.CheckedChanged += new System.EventHandler(this.backupModsCheckBox_CheckedChanged);
            this.backupModsCheckBox.MouseEnter += new System.EventHandler(this.backupModsCheckBox_MouseEnter);
            this.backupModsCheckBox.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.SuperExtractionCB);
            this.settingsGroupBox.Controls.Add(this.InstantExtractionCB);
            this.settingsGroupBox.Controls.Add(this.createShortcutsCB);
            this.settingsGroupBox.Controls.Add(this.saveUserDataCB);
            this.settingsGroupBox.Controls.Add(this.saveLastInstallCB);
            this.settingsGroupBox.Controls.Add(this.ShowInstallCompleteWindowCB);
            this.settingsGroupBox.Controls.Add(this.forceManuel);
            this.settingsGroupBox.Controls.Add(this.cancerFontCB);
            this.settingsGroupBox.Controls.Add(this.notifyIfSameDatabaseCB);
            this.settingsGroupBox.Controls.Add(this.backupModsCheckBox);
            this.settingsGroupBox.Controls.Add(this.cleanInstallCB);
            this.settingsGroupBox.Controls.Add(this.clearLogFilesCB);
            this.settingsGroupBox.Controls.Add(this.darkUICB);
            this.settingsGroupBox.Controls.Add(this.clearCacheCB);
            this.settingsGroupBox.Location = new System.Drawing.Point(13, 99);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(464, 179);
            this.settingsGroupBox.TabIndex = 25;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Modpack Settings";
            // 
            // SuperExtractionCB
            // 
            this.SuperExtractionCB.Location = new System.Drawing.Point(232, 100);
            this.SuperExtractionCB.Name = "SuperExtractionCB";
            this.SuperExtractionCB.Size = new System.Drawing.Size(220, 32);
            this.SuperExtractionCB.TabIndex = 37;
            this.SuperExtractionCB.Text = "Super extraction mode (Experimental)";
            this.SuperExtractionCB.UseVisualStyleBackColor = true;
            this.SuperExtractionCB.CheckedChanged += new System.EventHandler(this.SuperExtractionCB_CheckedChanged);
            this.SuperExtractionCB.MouseEnter += new System.EventHandler(this.SuperExtractionCB_MouseEnter);
            this.SuperExtractionCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // InstantExtractionCB
            // 
            this.InstantExtractionCB.Location = new System.Drawing.Point(6, 132);
            this.InstantExtractionCB.Name = "InstantExtractionCB";
            this.InstantExtractionCB.Size = new System.Drawing.Size(220, 32);
            this.InstantExtractionCB.TabIndex = 36;
            this.InstantExtractionCB.Text = "Instant extraction mode (experimental)";
            this.InstantExtractionCB.UseVisualStyleBackColor = true;
            this.InstantExtractionCB.CheckedChanged += new System.EventHandler(this.InstantExtractionCB_CheckedChanged);
            this.InstantExtractionCB.MouseEnter += new System.EventHandler(this.InstantExtractionCB_MouseEnter);
            this.InstantExtractionCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // createShortcutsCB
            // 
            this.createShortcutsCB.Location = new System.Drawing.Point(232, 83);
            this.createShortcutsCB.Name = "createShortcutsCB";
            this.createShortcutsCB.Size = new System.Drawing.Size(220, 17);
            this.createShortcutsCB.TabIndex = 35;
            this.createShortcutsCB.Text = "Create Shortcuts";
            this.createShortcutsCB.UseVisualStyleBackColor = true;
            this.createShortcutsCB.CheckedChanged += new System.EventHandler(this.CreateShortcutsCB_CheckedChanged);
            this.createShortcutsCB.MouseEnter += new System.EventHandler(this.CreateShortcutsCB_MouseEnter);
            this.createShortcutsCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // saveUserDataCB
            // 
            this.saveUserDataCB.Location = new System.Drawing.Point(6, 83);
            this.saveUserDataCB.Name = "saveUserDataCB";
            this.saveUserDataCB.Size = new System.Drawing.Size(220, 17);
            this.saveUserDataCB.TabIndex = 27;
            this.saveUserDataCB.Text = "Save User created data";
            this.saveUserDataCB.UseVisualStyleBackColor = true;
            this.saveUserDataCB.CheckedChanged += new System.EventHandler(this.saveUserDataCB_CheckedChanged);
            this.saveUserDataCB.MouseEnter += new System.EventHandler(this.saveUserDataCB_MouseEnter);
            this.saveUserDataCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // saveLastInstallCB
            // 
            this.saveLastInstallCB.Location = new System.Drawing.Point(6, 66);
            this.saveLastInstallCB.Name = "saveLastInstallCB";
            this.saveLastInstallCB.Size = new System.Drawing.Size(220, 17);
            this.saveLastInstallCB.TabIndex = 26;
            this.saveLastInstallCB.Text = "Save last install\'s config";
            this.saveLastInstallCB.UseVisualStyleBackColor = true;
            this.saveLastInstallCB.CheckedChanged += new System.EventHandler(this.saveLastInstallCB_CheckedChanged);
            this.saveLastInstallCB.MouseEnter += new System.EventHandler(this.saveLastInstallCB_MouseEnter);
            this.saveLastInstallCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // ShowInstallCompleteWindowCB
            // 
            this.ShowInstallCompleteWindowCB.Location = new System.Drawing.Point(232, 66);
            this.ShowInstallCompleteWindowCB.Name = "ShowInstallCompleteWindowCB";
            this.ShowInstallCompleteWindowCB.Size = new System.Drawing.Size(220, 17);
            this.ShowInstallCompleteWindowCB.TabIndex = 34;
            this.ShowInstallCompleteWindowCB.Text = "Show Install complete window";
            this.ShowInstallCompleteWindowCB.UseVisualStyleBackColor = true;
            this.ShowInstallCompleteWindowCB.CheckedChanged += new System.EventHandler(this.ShowInstallCompleteWindow_CheckedChanged);
            this.ShowInstallCompleteWindowCB.MouseEnter += new System.EventHandler(this.ShowInstallCompleteWindowCB_MouseEnter);
            this.ShowInstallCompleteWindowCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // notifyIfSameDatabaseCB
            // 
            this.notifyIfSameDatabaseCB.Location = new System.Drawing.Point(232, 132);
            this.notifyIfSameDatabaseCB.Name = "notifyIfSameDatabaseCB";
            this.notifyIfSameDatabaseCB.Size = new System.Drawing.Size(220, 32);
            this.notifyIfSameDatabaseCB.TabIndex = 33;
            this.notifyIfSameDatabaseCB.Text = "Inform if no new database available";
            this.notifyIfSameDatabaseCB.UseVisualStyleBackColor = true;
            this.notifyIfSameDatabaseCB.CheckedChanged += new System.EventHandler(this.notifyIfSameDatabaseCB_CheckedChanged);
            this.notifyIfSameDatabaseCB.MouseEnter += new System.EventHandler(this.notifyIfSameDatabaseCB_MouseEnter);
            this.notifyIfSameDatabaseCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // clearLogFilesCB
            // 
            this.clearLogFilesCB.Location = new System.Drawing.Point(232, 49);
            this.clearLogFilesCB.Name = "clearLogFilesCB";
            this.clearLogFilesCB.Size = new System.Drawing.Size(220, 17);
            this.clearLogFilesCB.TabIndex = 32;
            this.clearLogFilesCB.Text = "Clear log files";
            this.clearLogFilesCB.UseVisualStyleBackColor = true;
            this.clearLogFilesCB.CheckedChanged += new System.EventHandler(this.clearLogFilesCB_CheckedChanged);
            this.clearLogFilesCB.MouseEnter += new System.EventHandler(this.clearLogFilesCB_MouseEnter);
            this.clearLogFilesCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // darkUICB
            // 
            this.darkUICB.Location = new System.Drawing.Point(232, 15);
            this.darkUICB.Name = "darkUICB";
            this.darkUICB.Size = new System.Drawing.Size(220, 17);
            this.darkUICB.TabIndex = 30;
            this.darkUICB.Text = "Dark UI";
            this.darkUICB.UseVisualStyleBackColor = true;
            this.darkUICB.CheckedChanged += new System.EventHandler(this.darkUICB_CheckedChanged);
            this.darkUICB.MouseEnter += new System.EventHandler(this.darkUICB_MouseEnter);
            this.darkUICB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // clearCacheCB
            // 
            this.clearCacheCB.Location = new System.Drawing.Point(232, 32);
            this.clearCacheCB.Name = "clearCacheCB";
            this.clearCacheCB.Size = new System.Drawing.Size(220, 17);
            this.clearCacheCB.TabIndex = 31;
            this.clearCacheCB.Text = "Clear WoT Cache Data";
            this.clearCacheCB.UseVisualStyleBackColor = true;
            this.clearCacheCB.CheckedChanged += new System.EventHandler(this.clearCacheCB_CheckedChanged);
            this.clearCacheCB.MouseEnter += new System.EventHandler(this.clearCacheCB_MouseEnter);
            this.clearCacheCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // languageSelectionGB
            // 
            this.languageSelectionGB.Controls.Add(this.LanguageComboBox);
            this.languageSelectionGB.Location = new System.Drawing.Point(351, 397);
            this.languageSelectionGB.Name = "languageSelectionGB";
            this.languageSelectionGB.Size = new System.Drawing.Size(126, 40);
            this.languageSelectionGB.TabIndex = 30;
            this.languageSelectionGB.TabStop = false;
            this.languageSelectionGB.Text = "Language";
            // 
            // LanguageComboBox
            // 
            this.LanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Items.AddRange(new object[] {
            "English",
            "Polski",
            "Deutsch",
            "Francais"});
            this.LanguageComboBox.Location = new System.Drawing.Point(6, 13);
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.Size = new System.Drawing.Size(112, 21);
            this.LanguageComboBox.TabIndex = 4;
            this.LanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // loadingImageGroupBox
            // 
            this.loadingImageGroupBox.Controls.Add(this.thirdGuardsLoadingImageRB);
            this.loadingImageGroupBox.Controls.Add(this.standardImageRB);
            this.loadingImageGroupBox.Location = new System.Drawing.Point(351, 344);
            this.loadingImageGroupBox.Name = "loadingImageGroupBox";
            this.loadingImageGroupBox.Size = new System.Drawing.Size(126, 50);
            this.loadingImageGroupBox.TabIndex = 26;
            this.loadingImageGroupBox.TabStop = false;
            this.loadingImageGroupBox.Text = "Loading Image";
            // 
            // thirdGuardsLoadingImageRB
            // 
            this.thirdGuardsLoadingImageRB.AutoSize = true;
            this.thirdGuardsLoadingImageRB.Location = new System.Drawing.Point(6, 30);
            this.thirdGuardsLoadingImageRB.Name = "thirdGuardsLoadingImageRB";
            this.thirdGuardsLoadingImageRB.Size = new System.Drawing.Size(72, 17);
            this.thirdGuardsLoadingImageRB.TabIndex = 1;
            this.thirdGuardsLoadingImageRB.TabStop = true;
            this.thirdGuardsLoadingImageRB.Text = "3rdguards";
            this.thirdGuardsLoadingImageRB.UseVisualStyleBackColor = true;
            this.thirdGuardsLoadingImageRB.CheckedChanged += new System.EventHandler(this.standardImageRB_CheckedChanged);
            this.thirdGuardsLoadingImageRB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.standardImageRB_MouseDown);
            this.thirdGuardsLoadingImageRB.MouseEnter += new System.EventHandler(this.standardImageRB_MouseEnter);
            this.thirdGuardsLoadingImageRB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // standardImageRB
            // 
            this.standardImageRB.AutoSize = true;
            this.standardImageRB.Location = new System.Drawing.Point(6, 13);
            this.standardImageRB.Name = "standardImageRB";
            this.standardImageRB.Size = new System.Drawing.Size(68, 17);
            this.standardImageRB.TabIndex = 0;
            this.standardImageRB.TabStop = true;
            this.standardImageRB.Text = "Standard";
            this.standardImageRB.UseVisualStyleBackColor = true;
            this.standardImageRB.CheckedChanged += new System.EventHandler(this.standardImageRB_CheckedChanged);
            this.standardImageRB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.standardImageRB_MouseDown);
            this.standardImageRB.MouseEnter += new System.EventHandler(this.standardImageRB_MouseEnter);
            this.standardImageRB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // findBugAddModLabel
            // 
            this.findBugAddModLabel.AutoSize = true;
            this.findBugAddModLabel.Location = new System.Drawing.Point(10, 575);
            this.findBugAddModLabel.Name = "findBugAddModLabel";
            this.findBugAddModLabel.Size = new System.Drawing.Size(163, 13);
            this.findBugAddModLabel.TabIndex = 27;
            this.findBugAddModLabel.TabStop = true;
            this.findBugAddModLabel.Text = "Find a bug? Want a mod added?";
            this.findBugAddModLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.findBugAddModLabel_LinkClicked);
            // 
            // cancelDownloadButton
            // 
            this.cancelDownloadButton.Enabled = false;
            this.cancelDownloadButton.Location = new System.Drawing.Point(375, 575);
            this.cancelDownloadButton.Name = "cancelDownloadButton";
            this.cancelDownloadButton.Size = new System.Drawing.Size(103, 60);
            this.cancelDownloadButton.TabIndex = 28;
            this.cancelDownloadButton.Text = "Cancel Download";
            this.cancelDownloadButton.UseVisualStyleBackColor = true;
            this.cancelDownloadButton.Visible = false;
            this.cancelDownloadButton.Click += new System.EventHandler(this.cancelDownloadButton_Click);
            // 
            // DownloadTimer
            // 
            this.DownloadTimer.Interval = 1000;
            this.DownloadTimer.Tick += new System.EventHandler(this.DownloadTimer_Tick);
            // 
            // downloadProgress
            // 
            this.downloadProgress.DetectUrls = false;
            this.downloadProgress.Location = new System.Drawing.Point(13, 443);
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.ReadOnly = true;
            this.downloadProgress.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.downloadProgress.Size = new System.Drawing.Size(465, 60);
            this.downloadProgress.TabIndex = 29;
            this.downloadProgress.Text = "";
            // 
            // viewTypeGB
            // 
            this.viewTypeGB.Controls.Add(this.disableColorsCB);
            this.viewTypeGB.Controls.Add(this.disableBordersCB);
            this.viewTypeGB.Controls.Add(this.expandNodesDefault);
            this.viewTypeGB.Controls.Add(this.selectionLegacy);
            this.viewTypeGB.Controls.Add(this.selectionDefault);
            this.viewTypeGB.Location = new System.Drawing.Point(176, 284);
            this.viewTypeGB.Name = "viewTypeGB";
            this.viewTypeGB.Size = new System.Drawing.Size(174, 153);
            this.viewTypeGB.TabIndex = 31;
            this.viewTypeGB.TabStop = false;
            this.viewTypeGB.Text = "Selection View";
            // 
            // disableColorsCB
            // 
            this.disableColorsCB.Location = new System.Drawing.Point(18, 48);
            this.disableColorsCB.Name = "disableColorsCB";
            this.disableColorsCB.Size = new System.Drawing.Size(151, 17);
            this.disableColorsCB.TabIndex = 4;
            this.disableColorsCB.Text = "Disable color change";
            this.disableColorsCB.UseVisualStyleBackColor = true;
            this.disableColorsCB.CheckedChanged += new System.EventHandler(this.disableColorsCB_CheckedChanged);
            this.disableColorsCB.MouseEnter += new System.EventHandler(this.disableColorsCB_MouseEnter);
            this.disableColorsCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // disableBordersCB
            // 
            this.disableBordersCB.Location = new System.Drawing.Point(18, 30);
            this.disableBordersCB.Name = "disableBordersCB";
            this.disableBordersCB.Size = new System.Drawing.Size(151, 20);
            this.disableBordersCB.TabIndex = 3;
            this.disableBordersCB.Text = "Disable borders";
            this.disableBordersCB.UseVisualStyleBackColor = true;
            this.disableBordersCB.CheckedChanged += new System.EventHandler(this.disableBordersCB_CheckedChanged);
            this.disableBordersCB.MouseEnter += new System.EventHandler(this.disableBordersCB_MouseEnter);
            this.disableBordersCB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // expandNodesDefault
            // 
            this.expandNodesDefault.AutoSize = true;
            this.expandNodesDefault.Location = new System.Drawing.Point(18, 110);
            this.expandNodesDefault.MaximumSize = new System.Drawing.Size(100, 40);
            this.expandNodesDefault.MinimumSize = new System.Drawing.Size(50, 20);
            this.expandNodesDefault.Name = "expandNodesDefault";
            this.expandNodesDefault.Size = new System.Drawing.Size(75, 20);
            this.expandNodesDefault.TabIndex = 2;
            this.expandNodesDefault.Text = "Expand all";
            this.expandNodesDefault.UseVisualStyleBackColor = true;
            this.expandNodesDefault.CheckedChanged += new System.EventHandler(this.expandNodesDefault_CheckedChanged);
            this.expandNodesDefault.MouseEnter += new System.EventHandler(this.expandNodesDefault_MouseEnter);
            this.expandNodesDefault.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // selectionLegacy
            // 
            this.selectionLegacy.AutoSize = true;
            this.selectionLegacy.Location = new System.Drawing.Point(6, 93);
            this.selectionLegacy.Name = "selectionLegacy";
            this.selectionLegacy.Size = new System.Drawing.Size(60, 17);
            this.selectionLegacy.TabIndex = 1;
            this.selectionLegacy.TabStop = true;
            this.selectionLegacy.Text = "Legacy";
            this.selectionLegacy.UseVisualStyleBackColor = true;
            this.selectionLegacy.CheckedChanged += new System.EventHandler(this.selectionLegacy_CheckedChanged);
            this.selectionLegacy.MouseEnter += new System.EventHandler(this.selectionView_MouseEnter);
            this.selectionLegacy.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // selectionDefault
            // 
            this.selectionDefault.AutoSize = true;
            this.selectionDefault.Location = new System.Drawing.Point(6, 13);
            this.selectionDefault.Name = "selectionDefault";
            this.selectionDefault.Size = new System.Drawing.Size(59, 17);
            this.selectionDefault.TabIndex = 0;
            this.selectionDefault.TabStop = true;
            this.selectionDefault.Text = "Default";
            this.selectionDefault.UseVisualStyleBackColor = true;
            this.selectionDefault.CheckedChanged += new System.EventHandler(this.selectionDefault_CheckedChanged);
            this.selectionDefault.MouseEnter += new System.EventHandler(this.selectionView_MouseEnter);
            this.selectionDefault.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // donateLabel
            // 
            this.donateLabel.AutoSize = true;
            this.donateLabel.Location = new System.Drawing.Point(10, 614);
            this.donateLabel.Name = "donateLabel";
            this.donateLabel.Size = new System.Drawing.Size(162, 13);
            this.donateLabel.TabIndex = 32;
            this.donateLabel.TabStop = true;
            this.donateLabel.Text = "Donation for further development";
            this.donateLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.donateLabel_LinkClicked);
            // 
            // fontSizeGB
            // 
            this.fontSizeGB.Controls.Add(this.DPI275);
            this.fontSizeGB.Controls.Add(this.DPI225);
            this.fontSizeGB.Controls.Add(this.fontSize275);
            this.fontSizeGB.Controls.Add(this.fontSize225);
            this.fontSizeGB.Controls.Add(this.DPIAUTO);
            this.fontSizeGB.Controls.Add(this.DPI125);
            this.fontSizeGB.Controls.Add(this.DPI175);
            this.fontSizeGB.Controls.Add(this.DPI100);
            this.fontSizeGB.Controls.Add(this.fontSize175);
            this.fontSizeGB.Controls.Add(this.fontSize125);
            this.fontSizeGB.Controls.Add(this.fontSize100);
            this.fontSizeGB.Location = new System.Drawing.Point(13, 284);
            this.fontSizeGB.Name = "fontSizeGB";
            this.fontSizeGB.Size = new System.Drawing.Size(159, 153);
            this.fontSizeGB.TabIndex = 33;
            this.fontSizeGB.TabStop = false;
            this.fontSizeGB.Text = "Scaling Mode";
            // 
            // DPI275
            // 
            this.DPI275.AutoSize = true;
            this.DPI275.Location = new System.Drawing.Point(81, 77);
            this.DPI275.Name = "DPI275";
            this.DPI275.Size = new System.Drawing.Size(72, 17);
            this.DPI275.TabIndex = 10;
            this.DPI275.TabStop = true;
            this.DPI275.Text = "DPI 2.75x";
            this.DPI275.UseVisualStyleBackColor = true;
            this.DPI275.CheckedChanged += new System.EventHandler(this.DPI275_CheckedChanged);
            this.DPI275.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPI275.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // DPI225
            // 
            this.DPI225.AutoSize = true;
            this.DPI225.Location = new System.Drawing.Point(81, 61);
            this.DPI225.Name = "DPI225";
            this.DPI225.Size = new System.Drawing.Size(72, 17);
            this.DPI225.TabIndex = 9;
            this.DPI225.TabStop = true;
            this.DPI225.Text = "DPI 2.25x";
            this.DPI225.UseVisualStyleBackColor = true;
            this.DPI225.CheckedChanged += new System.EventHandler(this.DPI225_CheckedChanged);
            this.DPI225.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPI225.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // fontSize275
            // 
            this.fontSize275.AutoSize = true;
            this.fontSize275.Location = new System.Drawing.Point(6, 77);
            this.fontSize275.Name = "fontSize275";
            this.fontSize275.Size = new System.Drawing.Size(75, 17);
            this.fontSize275.TabIndex = 8;
            this.fontSize275.TabStop = true;
            this.fontSize275.Text = "Font 2.75x";
            this.fontSize275.UseVisualStyleBackColor = true;
            this.fontSize275.CheckedChanged += new System.EventHandler(this.fontSize275_CheckedChanged);
            this.fontSize275.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.fontSize275.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // fontSize225
            // 
            this.fontSize225.AutoSize = true;
            this.fontSize225.Location = new System.Drawing.Point(6, 61);
            this.fontSize225.Name = "fontSize225";
            this.fontSize225.Size = new System.Drawing.Size(75, 17);
            this.fontSize225.TabIndex = 7;
            this.fontSize225.TabStop = true;
            this.fontSize225.Text = "Font 2.25x";
            this.fontSize225.UseVisualStyleBackColor = true;
            this.fontSize225.CheckedChanged += new System.EventHandler(this.fontSize225_CheckedChanged);
            this.fontSize225.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.fontSize225.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // DPIAUTO
            // 
            this.DPIAUTO.AutoSize = true;
            this.DPIAUTO.Location = new System.Drawing.Point(81, 93);
            this.DPIAUTO.Name = "DPIAUTO";
            this.DPIAUTO.Size = new System.Drawing.Size(76, 17);
            this.DPIAUTO.TabIndex = 6;
            this.DPIAUTO.TabStop = true;
            this.DPIAUTO.Text = "DPI AUTO";
            this.DPIAUTO.UseVisualStyleBackColor = true;
            this.DPIAUTO.CheckedChanged += new System.EventHandler(this.DPIAUTO_CheckedChanged);
            this.DPIAUTO.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPIAUTO.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // DPI125
            // 
            this.DPI125.AutoSize = true;
            this.DPI125.Location = new System.Drawing.Point(81, 29);
            this.DPI125.Name = "DPI125";
            this.DPI125.Size = new System.Drawing.Size(72, 17);
            this.DPI125.TabIndex = 5;
            this.DPI125.TabStop = true;
            this.DPI125.Text = "DPI 1.25x";
            this.DPI125.UseVisualStyleBackColor = true;
            this.DPI125.CheckedChanged += new System.EventHandler(this.DPI125_CheckedChanged);
            this.DPI125.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPI125.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // DPI175
            // 
            this.DPI175.AutoSize = true;
            this.DPI175.Location = new System.Drawing.Point(81, 45);
            this.DPI175.Name = "DPI175";
            this.DPI175.Size = new System.Drawing.Size(72, 17);
            this.DPI175.TabIndex = 4;
            this.DPI175.TabStop = true;
            this.DPI175.Text = "DPI 1.75x";
            this.DPI175.UseVisualStyleBackColor = true;
            this.DPI175.CheckedChanged += new System.EventHandler(this.DPI175_CheckedChanged);
            this.DPI175.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPI175.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // DPI100
            // 
            this.DPI100.AutoSize = true;
            this.DPI100.Location = new System.Drawing.Point(81, 13);
            this.DPI100.Name = "DPI100";
            this.DPI100.Size = new System.Drawing.Size(57, 17);
            this.DPI100.TabIndex = 3;
            this.DPI100.TabStop = true;
            this.DPI100.Text = "DPI 1x";
            this.DPI100.UseVisualStyleBackColor = true;
            this.DPI100.CheckedChanged += new System.EventHandler(this.DPI100_CheckedChanged);
            this.DPI100.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.DPI100.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // fontSize175
            // 
            this.fontSize175.AutoSize = true;
            this.fontSize175.Location = new System.Drawing.Point(6, 45);
            this.fontSize175.Name = "fontSize175";
            this.fontSize175.Size = new System.Drawing.Size(75, 17);
            this.fontSize175.TabIndex = 2;
            this.fontSize175.TabStop = true;
            this.fontSize175.Text = "Font 1.75x";
            this.fontSize175.UseVisualStyleBackColor = true;
            this.fontSize175.CheckedChanged += new System.EventHandler(this.fontSize175_CheckedChanged);
            this.fontSize175.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.fontSize175.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // fontSize125
            // 
            this.fontSize125.AutoSize = true;
            this.fontSize125.Location = new System.Drawing.Point(6, 29);
            this.fontSize125.Name = "fontSize125";
            this.fontSize125.Size = new System.Drawing.Size(75, 17);
            this.fontSize125.TabIndex = 1;
            this.fontSize125.TabStop = true;
            this.fontSize125.Text = "Font 1.25x";
            this.fontSize125.UseVisualStyleBackColor = true;
            this.fontSize125.CheckedChanged += new System.EventHandler(this.fontSize125_CheckedChanged);
            this.fontSize125.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.fontSize125.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // fontSize100
            // 
            this.fontSize100.AutoSize = true;
            this.fontSize100.Location = new System.Drawing.Point(6, 13);
            this.fontSize100.Name = "fontSize100";
            this.fontSize100.Size = new System.Drawing.Size(60, 17);
            this.fontSize100.TabIndex = 0;
            this.fontSize100.TabStop = true;
            this.fontSize100.Text = "Font 1x";
            this.fontSize100.UseVisualStyleBackColor = true;
            this.fontSize100.CheckedChanged += new System.EventHandler(this.fontSize100_CheckedChanged);
            this.fontSize100.MouseEnter += new System.EventHandler(this.font_MouseEnter);
            this.fontSize100.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Location = new System.Drawing.Point(13, 509);
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(465, 16);
            this.totalProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.totalProgressBar.TabIndex = 34;
            // 
            // DiscordServerLink
            // 
            this.DiscordServerLink.AutoSize = true;
            this.DiscordServerLink.Location = new System.Drawing.Point(10, 633);
            this.DiscordServerLink.Name = "DiscordServerLink";
            this.DiscordServerLink.Size = new System.Drawing.Size(77, 13);
            this.DiscordServerLink.TabIndex = 35;
            this.DiscordServerLink.TabStop = true;
            this.DiscordServerLink.Text = "Discord Server";
            this.DiscordServerLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DiscordServerLink_LinkClicked);
            // 
            // viewAppUpdates
            // 
            this.viewAppUpdates.Location = new System.Drawing.Point(13, 12);
            this.viewAppUpdates.Name = "viewAppUpdates";
            this.viewAppUpdates.Size = new System.Drawing.Size(226, 23);
            this.viewAppUpdates.TabIndex = 36;
            this.viewAppUpdates.Text = "View latest application updates";
            this.viewAppUpdates.UseVisualStyleBackColor = true;
            this.viewAppUpdates.Click += new System.EventHandler(this.viewAppUpdates_Click);
            // 
            // viewDBUpdates
            // 
            this.viewDBUpdates.Location = new System.Drawing.Point(13, 41);
            this.viewDBUpdates.Name = "viewDBUpdates";
            this.viewDBUpdates.Size = new System.Drawing.Size(226, 23);
            this.viewDBUpdates.TabIndex = 37;
            this.viewDBUpdates.Text = "View latest database updates";
            this.viewDBUpdates.UseVisualStyleBackColor = true;
            this.viewDBUpdates.Click += new System.EventHandler(this.viewDBUpdates_Click);
            // 
            // ErrorCounterLabel
            // 
            this.ErrorCounterLabel.AutoSize = true;
            this.ErrorCounterLabel.Location = new System.Drawing.Point(288, 610);
            this.ErrorCounterLabel.Name = "ErrorCounterLabel";
            this.ErrorCounterLabel.Size = new System.Drawing.Size(80, 13);
            this.ErrorCounterLabel.TabIndex = 38;
            this.ErrorCounterLabel.Text = "Error counter: 0";
            this.ErrorCounterLabel.Visible = false;
            // 
            // VersionTable
            // 
            this.VersionTable.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.VersionTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.VersionTable.ColumnCount = 2;
            this.VersionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.VersionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.VersionTable.Controls.Add(this.DatabaseVersionLabel, 1, 0);
            this.VersionTable.Controls.Add(this.ApplicationVersionLabel, 0, 0);
            this.VersionTable.Location = new System.Drawing.Point(13, 652);
            this.VersionTable.Name = "VersionTable";
            this.VersionTable.RowCount = 1;
            this.VersionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.VersionTable.Size = new System.Drawing.Size(465, 16);
            this.VersionTable.TabIndex = 39;
            // 
            // DatabaseVersionLabel
            // 
            this.DatabaseVersionLabel.AutoSize = true;
            this.DatabaseVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.DatabaseVersionLabel.Location = new System.Drawing.Point(325, 1);
            this.DatabaseVersionLabel.Name = "DatabaseVersionLabel";
            this.DatabaseVersionLabel.Size = new System.Drawing.Size(136, 14);
            this.DatabaseVersionLabel.TabIndex = 0;
            this.DatabaseVersionLabel.Text = "Latest Database v{version}";
            // 
            // ApplicationVersionLabel
            // 
            this.ApplicationVersionLabel.AutoSize = true;
            this.ApplicationVersionLabel.Location = new System.Drawing.Point(4, 1);
            this.ApplicationVersionLabel.Name = "ApplicationVersionLabel";
            this.ApplicationVersionLabel.Size = new System.Drawing.Size(109, 13);
            this.ApplicationVersionLabel.TabIndex = 1;
            this.ApplicationVersionLabel.Text = "Application v{version]";
            // 
            // DiagnosticUtilitiesButton
            // 
            this.DiagnosticUtilitiesButton.Location = new System.Drawing.Point(13, 70);
            this.DiagnosticUtilitiesButton.Name = "DiagnosticUtilitiesButton";
            this.DiagnosticUtilitiesButton.Size = new System.Drawing.Size(226, 23);
            this.DiagnosticUtilitiesButton.TabIndex = 40;
            this.DiagnosticUtilitiesButton.Text = "Diagnostic Utilities";
            this.DiagnosticUtilitiesButton.UseVisualStyleBackColor = true;
            this.DiagnosticUtilitiesButton.Click += new System.EventHandler(this.DiagnosticUtilitiesButton_Click);
            // 
            // UninstallModeGroupBox
            // 
            this.UninstallModeGroupBox.Controls.Add(this.CleanUninstallModeRB);
            this.UninstallModeGroupBox.Controls.Add(this.SmartUninstallModeRB);
            this.UninstallModeGroupBox.Location = new System.Drawing.Point(351, 284);
            this.UninstallModeGroupBox.Name = "UninstallModeGroupBox";
            this.UninstallModeGroupBox.Size = new System.Drawing.Size(126, 58);
            this.UninstallModeGroupBox.TabIndex = 41;
            this.UninstallModeGroupBox.TabStop = false;
            this.UninstallModeGroupBox.Text = "Uninstall Mode";
            // 
            // CleanUninstallModeRB
            // 
            this.CleanUninstallModeRB.AutoSize = true;
            this.CleanUninstallModeRB.Location = new System.Drawing.Point(6, 30);
            this.CleanUninstallModeRB.Name = "CleanUninstallModeRB";
            this.CleanUninstallModeRB.Size = new System.Drawing.Size(52, 17);
            this.CleanUninstallModeRB.TabIndex = 1;
            this.CleanUninstallModeRB.TabStop = true;
            this.CleanUninstallModeRB.Text = "Clean";
            this.CleanUninstallModeRB.UseVisualStyleBackColor = true;
            this.CleanUninstallModeRB.CheckedChanged += new System.EventHandler(this.CleanUninstallModeRB_CheckedChanged);
            this.CleanUninstallModeRB.MouseEnter += new System.EventHandler(this.CleanUninstallModeRB_MouseEnter);
            this.CleanUninstallModeRB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // SmartUninstallModeRB
            // 
            this.SmartUninstallModeRB.AutoSize = true;
            this.SmartUninstallModeRB.Location = new System.Drawing.Point(6, 13);
            this.SmartUninstallModeRB.Name = "SmartUninstallModeRB";
            this.SmartUninstallModeRB.Size = new System.Drawing.Size(52, 17);
            this.SmartUninstallModeRB.TabIndex = 0;
            this.SmartUninstallModeRB.TabStop = true;
            this.SmartUninstallModeRB.Text = "Smart";
            this.SmartUninstallModeRB.UseVisualStyleBackColor = true;
            this.SmartUninstallModeRB.CheckedChanged += new System.EventHandler(this.SmartUninstallModeRB_CheckedChanged);
            this.SmartUninstallModeRB.MouseEnter += new System.EventHandler(this.SmartUninstallModeRB_MouseEnter);
            this.SmartUninstallModeRB.MouseLeave += new System.EventHandler(this.generic_MouseLeave);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(489, 674);
            this.Controls.Add(this.UninstallModeGroupBox);
            this.Controls.Add(this.DiagnosticUtilitiesButton);
            this.Controls.Add(this.VersionTable);
            this.Controls.Add(this.ErrorCounterLabel);
            this.Controls.Add(this.viewDBUpdates);
            this.Controls.Add(this.viewAppUpdates);
            this.Controls.Add(this.DiscordServerLink);
            this.Controls.Add(this.totalProgressBar);
            this.Controls.Add(this.fontSizeGB);
            this.Controls.Add(this.donateLabel);
            this.Controls.Add(this.viewTypeGB);
            this.Controls.Add(this.languageSelectionGB);
            this.Controls.Add(this.downloadProgress);
            this.Controls.Add(this.cancelDownloadButton);
            this.Controls.Add(this.findBugAddModLabel);
            this.Controls.Add(this.loadingImageGroupBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.uninstallRelhaxMod);
            this.Controls.Add(this.installRelhaxMod);
            this.Controls.Add(this.parrentProgressBar);
            this.Controls.Add(this.formPageLink);
            this.Controls.Add(this.childProgressBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Relhax";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.languageSelectionGB.ResumeLayout(false);
            this.loadingImageGroupBox.ResumeLayout(false);
            this.loadingImageGroupBox.PerformLayout();
            this.viewTypeGB.ResumeLayout(false);
            this.viewTypeGB.PerformLayout();
            this.fontSizeGB.ResumeLayout(false);
            this.fontSizeGB.PerformLayout();
            this.VersionTable.ResumeLayout(false);
            this.VersionTable.PerformLayout();
            this.UninstallModeGroupBox.ResumeLayout(false);
            this.UninstallModeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar childProgressBar;
        private System.Windows.Forms.OpenFileDialog FindWotExe;
        private System.Windows.Forms.CheckBox forceManuel;
        private System.Windows.Forms.LinkLabel formPageLink;
        private System.Windows.Forms.ProgressBar parrentProgressBar;
        private System.Windows.Forms.Button installRelhaxMod;
        private System.Windows.Forms.Button uninstallRelhaxMod;
        private System.Windows.Forms.CheckBox cleanInstallCB;
        private System.Windows.Forms.CheckBox cancerFontCB;
        private System.Windows.Forms.CheckBox backupModsCheckBox;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.GroupBox loadingImageGroupBox;
        private System.Windows.Forms.RadioButton thirdGuardsLoadingImageRB;
        private System.Windows.Forms.RadioButton standardImageRB;
        private System.Windows.Forms.LinkLabel findBugAddModLabel;
        private System.Windows.Forms.CheckBox saveLastInstallCB;
        private System.Windows.Forms.Button cancelDownloadButton;
        private System.Windows.Forms.CheckBox saveUserDataCB;
        private System.Windows.Forms.Timer DownloadTimer;
        private System.Windows.Forms.RichTextBox downloadProgress;
        private System.Windows.Forms.CheckBox darkUICB;
        private System.Windows.Forms.GroupBox languageSelectionGB;
        private System.Windows.Forms.GroupBox viewTypeGB;
        private System.Windows.Forms.RadioButton selectionLegacy;
        private System.Windows.Forms.RadioButton selectionDefault;
        private System.Windows.Forms.LinkLabel donateLabel;
        private System.Windows.Forms.CheckBox expandNodesDefault;
        private System.Windows.Forms.GroupBox fontSizeGB;
        private System.Windows.Forms.RadioButton fontSize175;
        private System.Windows.Forms.RadioButton fontSize125;
        private System.Windows.Forms.RadioButton fontSize100;
        private System.Windows.Forms.RadioButton DPI100;
        private System.Windows.Forms.CheckBox disableBordersCB;
        private System.Windows.Forms.RadioButton DPI125;
        private System.Windows.Forms.RadioButton DPI175;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.LinkLabel DiscordServerLink;
        private System.Windows.Forms.CheckBox clearCacheCB;
        private System.Windows.Forms.Button viewAppUpdates;
        private System.Windows.Forms.Button viewDBUpdates;
        private System.Windows.Forms.CheckBox disableColorsCB;
        private System.Windows.Forms.CheckBox clearLogFilesCB;
        private System.Windows.Forms.RadioButton DPIAUTO;
        private System.Windows.Forms.RadioButton DPI275;
        private System.Windows.Forms.RadioButton DPI225;
        private System.Windows.Forms.RadioButton fontSize275;
        private System.Windows.Forms.RadioButton fontSize225;
        private System.Windows.Forms.CheckBox notifyIfSameDatabaseCB;
        private System.Windows.Forms.CheckBox ShowInstallCompleteWindowCB;
        public System.Windows.Forms.Label ErrorCounterLabel;
        private System.Windows.Forms.CheckBox createShortcutsCB;
        private System.Windows.Forms.TableLayoutPanel VersionTable;
        private System.Windows.Forms.Label DatabaseVersionLabel;
        private System.Windows.Forms.Label ApplicationVersionLabel;
        private System.Windows.Forms.ComboBox LanguageComboBox;
        private System.Windows.Forms.Button DiagnosticUtilitiesButton;
        private System.Windows.Forms.CheckBox InstantExtractionCB;
        private System.Windows.Forms.GroupBox UninstallModeGroupBox;
        private System.Windows.Forms.RadioButton SmartUninstallModeRB;
        private System.Windows.Forms.CheckBox SuperExtractionCB;
        private System.Windows.Forms.RadioButton CleanUninstallModeRB;
    }
}

