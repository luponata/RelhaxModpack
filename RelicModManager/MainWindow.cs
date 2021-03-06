﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Drawing;
using System.Xml.XPath;
using System.Xml.Linq;
using RelhaxModpack.DatabaseComponents;
using RelhaxModpack.InstallerComponents;

namespace RelhaxModpack
{
    public partial class MainWindow : Form
    {
        //all instance variables required to be up here
        private WebClient Downloader;
        private const int MBDivisor = 1048576;
        //sample:  c:/games/World_of_Tanks
        public string tanksLocation;
        //the location to pass into the installer
        private string tanksVersionForInstaller;
        //the folder where the user's app data is stored (C:\Users\username\AppData)
        private string appDataFolder;
        //the string representation from the xml document manager_version.xml. also passed into the installer for logging the version of the database installed at that time
        private string databaseVersionString;
        //timer to measure download speed
        Stopwatch sw = new Stopwatch();
        //The list of all mods
        private List<Category> parsedCatagoryLists;
        //queue for downloading mods
        private List<IDatabasePackage> DatabasePackagesToDownload;
        //The ordered lists to install
        private List<Dependency> globalDependenciesToInstall;
        private List<Dependency> dependenciesToInstall;
        private List<LogicalDependency> logicalDependenciesToInstall;
        private List<SelectableDatabasePackage> modsConfigsToInstall;
        private List<Dependency> appendedDependenciesToInstall;
        private List<SelectableDatabasePackage> ModsWithShortcuts;
        private List<Shortcut> Shortcuts;
        private List<InstallGroup> InstallGroups;
        //list of all current dependencies
        private List<Dependency> currentDependencies;
        private List<LogicalDependency> currentLogicalDependencies;
        //DeveloperSelections namelist
        public static List<DeveloperSelections> developerSelections = new List<DeveloperSelections>();
        //list of patches
        private List<Patch> patchList;
        //list of all needed files from the current loaded modInfo file
        public static List<string> usedFilesList;
        //counter for Utils.exception calls
        public static int errorCounter = 0;
        // string tempOldDownload; => using userToken at Async download
        private List<Mod> userMods;
        string currentModDownloading;
        private Installer ins;
        private Installer unI;
        private string tanksVersion;//0.9.x.y
        List<double> timeRemainArray;
        //the ETA variable for downlading
        double actualTimeRemain = 0;
        float previousTotalBytesDownloaded = 0;
        float currentTotalBytesDownloaded = 0;
        float differenceTotalBytesDownloaded = 0;
        float sessionDownloadSpeed = 0;
        private int downloadCounter = -1;
        private object lockerMain = new object();
        private LoadingGifPreview gp;
        List<string> supportedVersions = new List<string>();
        List<SelectableDatabasePackage> modsConfigsWithData;
        private float scale = 1.0f;
        public static float originalMainWindowHeight { get; set; }
        public static float originalMainWindowWidth { get; set; }

        //  interpret the created CiInfo buildTag as an "us-US" or a "de-DE" timeformat and return it as a local time- and dateformat string
        public static string compileTime()//if getting build error, check windows date and time format settings https://puu.sh/xgCqO/e97e2e4a34.png
        {
            string date = CiInfo.BuildTag;
            if (Utils.ConvertDateToLocalCultureFormat(date, out date))
                return date;
            else
                return "Error in dateTime format: " + date;
        }

        /// <summary>
        /// gets now the "Release version" from RelhaxModpack-properties
        /// https://stackoverflow.com/questions/2959330/remove-characters-before-character
        /// https://www.mikrocontroller.net/topic/140764
        /// </summary>
        /// <returns></returns>
        public string ManagerVersion()
        {
            string managerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().IndexOf('.') + 1);
            if (Program.Version == Program.ProgramVersion.Beta)
                managerVersion = managerVersion + "_BETA";
            return managerVersion;
        }

        //The constructur for the application
        public MainWindow()
        {
            Logging.Manager("MainWindow Constructed");
            InitializeComponent();
            this.SetStyle(                                      /// add double buffering and possibly reduce flicker https://stackoverflow.com/questions/1550293/stopping-textbox-flicker-during-update
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);
            originalMainWindowHeight = this.Size.Height;
            originalMainWindowWidth = this.Size.Width;
        }

        //handler for the mod download file progress
        void downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (Settings.InstantExtraction)
                return;
            if (!DownloadTimer.Enabled)
                DownloadTimer.Enabled = true;
            string totalSpeedLabel = "";
            //get the download information into numeric classes
            float bytesIn = float.Parse(e.BytesReceived.ToString());
            float totalBytes = float.Parse(e.TotalBytesToReceive.ToString());
            float MBytesIn = (float)bytesIn / MBDivisor;
            currentTotalBytesDownloaded = bytesIn;
            //create the download progress string
            string currentModDownloadingShort = currentModDownloading;
            if (currentModDownloading.Length > 200)
                currentModDownloadingShort = currentModDownloading.Substring(0, 15) + "...";
            //set the progress bar
            childProgressBar.Value = e.ProgressPercentage;
            //set the download speed
            if (sessionDownloadSpeed < 0)
                sessionDownloadSpeed = 0;
            sessionDownloadSpeed = (float)Math.Round(sessionDownloadSpeed, 2);
            totalSpeedLabel = "" + sessionDownloadSpeed + " MB/s";
            //get the ETA for the download
            double totalTimeToDownload = totalBytes / (e.BytesReceived / sw.Elapsed.TotalSeconds);
            double timeRemain = totalTimeToDownload - sw.Elapsed.TotalSeconds;
            if (timeRemain < 0)
            {
                timeRemain = 0;
            }
            if (timeRemainArray == null)
                timeRemainArray = new List<double>();
            timeRemainArray.Add(timeRemain);
            if (timeRemainArray.Count >= 10)
            {
                double timeAverageRemain = 0;
                foreach (double d in timeRemainArray)
                    timeAverageRemain += d;
                actualTimeRemain = timeAverageRemain / 10;
                timeRemainArray.Clear();
            }
            //round to a whole number
            actualTimeRemain = Math.Round(actualTimeRemain, 0);
            //prevent the eta from becomming less than 0
            if (actualTimeRemain < 0)
                actualTimeRemain = 0;
            //convert the total seconds to mins and seconds
            int actualTimeMins = (int)actualTimeRemain / 60;
            int actualTimeSecs = (int)actualTimeRemain % 60;
            string downloadStatus = string.Format("{0} {1} ({2} MB {3} {4} MB)\n{5} {6} mins {7} secs",
                Translations.getTranslatedString("Downloading"), currentModDownloadingShort, Math.Round(MBytesIn, 1), Translations.getTranslatedString("of"), Math.Round(totalBytes / MBDivisor, 1), totalSpeedLabel, actualTimeMins, actualTimeSecs);
            downloadProgress.Text = downloadStatus;
        }

        //handler for the mod download file complete event
        void downloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadTimer.Enabled = false;
            //check to see if the user cancled the download
            if (e != null && e.Cancelled)
            {
                //update the UI and download state
                ToggleUIButtons(true);
                downloadProgress.Text = Translations.getTranslatedString("idle");
                parrentProgressBar.Value = 0;
                childProgressBar.Value = 0;
                return;
            }
            //i think a complete download means that error is null, if error is ever not null this will catch it and we can log it
            else if (e != null && e.Error != null)
            {
                AsyncDownloadArgs userToken = (AsyncDownloadArgs)e.UserState;
                if (Program.testMode)
                    Utils.ExceptionLog("downloader_DownloadFileCompleted", "downloaded file: " + userToken.url.ToString(), e.Error);
                else
                    Utils.ExceptionLog("downloader_DownloadFileCompleted", "downloaded file: " + Path.GetFileName(userToken.zipFile.ToString()), e.Error);
                DialogResult result = MessageBox.Show(string.Format("{0}\n{1}\n\n{2}", Translations.getTranslatedString("failedToDownload_1"), Path.GetFileName(userToken.zipFile.ToString()), Translations.getTranslatedString("failedToDownload_2")),"critical",MessageBoxButtons.AbortRetryIgnore,MessageBoxIcon.Question);
                if (result == DialogResult.Abort)
                {
                    Application.Exit();
                }
                else if (result == DialogResult.Retry)
                {
                    Downloader.DownloadFileAsync(userToken.url, userToken.zipFile, e.UserState);
                    return;
                }
            }
            //at this point we're here for NOT an error and NOT a cancel
            downloadCounter++;
            if (DatabasePackagesToDownload.Count != downloadCounter)
            {
                //downloader components
                AsyncDownloadArgs args = new AsyncDownloadArgs();
                args.url = new Uri(Utils.ReplaceMacro(DatabasePackagesToDownload[downloadCounter].StartAddress) + DatabasePackagesToDownload[downloadCounter].ZipFile + DatabasePackagesToDownload[downloadCounter].EndAddress);
                args.zipFile = Path.Combine(Settings.RelhaxDownloadsFolder,DatabasePackagesToDownload[downloadCounter].ZipFile);
                //for the next file in the queue, delete it.
                if (File.Exists(args.zipFile)) File.Delete(args.zipFile);
                //download new zip file
                if (Downloader != null)
                    Downloader.Dispose();
                Downloader = new WebClient();
                Downloader.DownloadProgressChanged += downloader_DownloadProgressChanged;
                Downloader.DownloadFileCompleted += downloader_DownloadFileCompleted;
                Downloader.Proxy = null;
                Downloader.DownloadFileAsync(args.url, args.zipFile, args);
                Logging.Manager("downloading " + Path.GetFileName(args.zipFile));
                //UI components
                if(!Settings.InstantExtraction)
                {
                    totalProgressBar.Maximum = (int)InstallerEventArgs.InstallProgress.Done;
                    totalProgressBar.Value = 1;//backup technically, but don't worry about it (for now)
                    cancelDownloadButton.Enabled = true;
                    cancelDownloadButton.Visible = true;
                    DownloadTimer.Enabled = true;
                    if (timeRemainArray == null)
                        timeRemainArray = new List<double>();
                    timeRemainArray.Clear();
                    actualTimeRemain = 0;
                    sw.Reset();
                    sw.Start();
                    currentModDownloading = Path.GetFileNameWithoutExtension(args.zipFile);
                    if ((parrentProgressBar.Value + 1) <= parrentProgressBar.Maximum)
                        parrentProgressBar.Value++;
                }
                //locking system for stopping data and access races
                if (downloadCounter!=0)
                {
                    lock (lockerMain)
                    {
                        DatabasePackagesToDownload[downloadCounter - 1].ReadyForInstall = true;
                    }
                }
            }
            if (DatabasePackagesToDownload.Count == downloadCounter || Settings.InstantExtraction)
            {
                cancelDownloadButton.Enabled = false;
                cancelDownloadButton.Visible = false;
                if (ins == null)
                {
                    ins = new Installer()
                    {
                        AppPath = Application.StartupPath,
                        GlobalDependencies = this.globalDependenciesToInstall,
                        Dependencies = this.dependenciesToInstall,
                        LogicalDependencies = this.logicalDependenciesToInstall,
                        AppendedDependencies = this.appendedDependenciesToInstall,
                        ModsConfigsToInstall = this.modsConfigsToInstall,
                        ModsConfigsWithData = this.modsConfigsWithData,
                        TanksLocation = this.tanksLocation,
                        TanksVersion = this.tanksVersionForInstaller,
                        UserMods = this.userMods,
                        AppDataFolder = this.appDataFolder,
                        DatabaseVersion = this.databaseVersionString,
                        Shortcuts = this.Shortcuts,
                        InstallGroups = this.InstallGroups,
                        TotalCategories = this.parsedCatagoryLists.Count
                    };
                    ins.InstallProgressChanged += I_InstallProgressChanged;
                    ins.StartInstallation();
                }
                if(DatabasePackagesToDownload.Count == downloadCounter && Settings.InstantExtraction)
                {
                    //locking system for stopping data and access races
                    if (downloadCounter != 0)
                    {
                        lock (lockerMain)
                        {
                            DatabasePackagesToDownload[downloadCounter - 1].ReadyForInstall = true;
                        }
                    }
                }
            }
        }

        //method to check for updates to the application on startup
        private void CheckmanagerUpdates()
        {
            Logging.Manager("Starting check for application updates");
            //download the updates
            WebClient updater = new WebClient();
            updater.Proxy = null;
            if (File.Exists(Settings.ManagerInfoDatFile))
                File.Delete(Settings.ManagerInfoDatFile);
            try
            {
                updater.DownloadFile("http://wotmods.relhaxmodpack.com/RelhaxModpack/managerInfo.dat", Settings.ManagerInfoDatFile);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("checkmanagerUpdates", @"Tried to access http://wotmods.relhaxmodpack.com/RelhaxModpack/managerInfo.dat", ex);
                MessageBox.Show(string.Format("{0} managerInfo.dat", Translations.getTranslatedString("failedToDownload_1")));
                Application.Exit();
            }
            if (Program.skipUpdate && !Program.testMode)
            {
                MessageBox.Show(Translations.getTranslatedString("skipUpdateWarning"));
            }
            string version = "";
            string xmlString = Utils.GetStringFromZip(Settings.ManagerInfoDatFile, "manager_version.xml");
            if (!xmlString.Equals(""))
            {
                XDocument doc = XDocument.Parse(xmlString);

                //parse the database version
                var databaseVersion = doc.XPathSelectElement("//version/database");
                DatabaseVersionLabel.Text = "Latest Database v" + databaseVersion.Value;
                Settings.DatabaseVersion = databaseVersion.Value;
                //parse the manager version
                
                var applicationVersion = Program.betaApplication ? doc.XPathSelectElement("//version/manager_beta") : doc.XPathSelectElement("//version/manager");
                version = applicationVersion.Value;
                Logging.Manager(string.Format("Local application is {0}, current online is {1}", ManagerVersion(), version));

                if (!Program.skipUpdate && !version.Equals(ManagerVersion()))
                {
                    Logging.Manager("exe is out of date. displaying user update window");
                    //out of date
                    VersionInfo vi = new VersionInfo();
                    vi.ShowDialog();
                    DialogResult result = vi.result;
                    if (result.Equals(DialogResult.Yes))
                    {
                        Logging.Manager("User accepted downloading new version");
                        //download new version
                        sw.Reset();
                        sw.Start();
                        string newExeName = Path.Combine(Application.StartupPath, "RelhaxModpack_update.exe");
                        updater.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloader_DownloadProgressChanged);
                        updater.DownloadFileCompleted += new AsyncCompletedEventHandler(updater_DownloadFileCompleted);

                        // check if manager instance is already running and ask user to close it
                        bool loop = true;
                        while (loop)
                        {
                            System.Threading.Thread.Sleep(500);
                            loop = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
                            if (!loop)
                                break;
                            result = MessageBox.Show(Translations.getTranslatedString("closeInstanceRunningForUpdate"), Translations.getTranslatedString("critical"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
                            if (result == DialogResult.Cancel)
                            {
                                Logging.Manager("User canceled update, because he does not want to end the parallel running Relhax instance.");
                                Application.Exit();
                                break;
                            }
                        }

                        if (File.Exists(newExeName)) File.Delete(newExeName);
                        string modpackExeURL = Program.betaApplication ? "http://wotmods.relhaxmodpack.com/RelhaxModpack/RelhaxModpackBeta.exe" : "http://wotmods.relhaxmodpack.com/RelhaxModpack/RelhaxModpack.exe";
                        updater.DownloadFileAsync(new Uri(modpackExeURL), newExeName);
                        Logging.Manager("New application download started");
                        currentModDownloading = "update ";
                    }
                    else
                    {
                        Logging.Manager("User declined downlading new version");
                        //close the application
                        Application.Exit();
                    }
                }
            }
            else
            {
                Logging.Manager("ERROR. Failed to get 'manager_version.xml'");
                MessageBox.Show(Translations.getTranslatedString("failedManager_version"), Translations.getTranslatedString("critical"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //close the application
                this.Close();
            }

        }
        //handler for when the update download is complete
        void updater_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadTimer.Enabled = false;
            if (e.Error != null && e.Error.Message.Equals("The remote server returned an error: (404) Not Found."))
            {
                //404
                Logging.Manager("ERROR: unable to download new application version");
                MessageBox.Show(Translations.getTranslatedString("cantDownloadNewVersion"));
                this.Close();
            }

            // part/idea of a new batch script if previous download/update failed https://stackoverflow.com/questions/4619088/windows-batch-file-file-download-from-a-url
            string newExeName = Path.Combine(Application.StartupPath, "RelicCopyUpdate.bat");
            try
            {
                //Utils.GetStringFromZip(Settings.ManagerInfoDatFile, "supported_clients.xml");
                string updateScript = "";
                updateScript = Utils.GetStringFromZip(Settings.ManagerInfoDatFile, "RelicCopyUpdate.txt");
                File.WriteAllText(newExeName, updateScript);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("updater_DownloadFileCompleted", "create RelicCopyUpdate.bat failed", ex);
                string msgTxt = string.Format(Translations.getTranslatedString("failedCreateUpdateBat"), Path.Combine(Application.StartupPath, "RelhaxModpack.exe"), "RelhaxModpack_update.exe", "RelhaxModpack.exe");
                if (DialogResult.Yes == MessageBox.Show(msgTxt, Translations.getTranslatedString("critical"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                {
                    // call the windows explorer and open at the relhax folder
                    ProcessStartInfo explorer = new ProcessStartInfo();
                    explorer.FileName = "explorer.exe";
                    explorer.Arguments = Application.StartupPath;
                    Process callExplorer = new Process();
                    callExplorer.StartInfo = explorer;
                    callExplorer.Start();
                }
                try
                {
                    // try to create a textfile at the temp folder
                    string howToPath = Path.Combine(Path.GetTempPath(), "howTo.txt");
                    File.WriteAllText(howToPath, msgTxt);
                    // call the notepad and open the howto.txt file
                    ProcessStartInfo notepad = new ProcessStartInfo();
                    notepad.FileName = "notepad.exe";
                    notepad.Arguments = howToPath;
                    Process callNotepad = new Process();
                    callNotepad.StartInfo = notepad;
                    callNotepad.Start();
                }
                catch (Exception e2)
                {
                    Utils.ExceptionLog("updater_DownloadFileCompleted", "failed to create howTo.txt", e2);
                }
                Application.Exit();
            }

            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = newExeName;
                info.Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1).ToArray());
                Process installUpdate = new Process();
                installUpdate.StartInfo = info;
                installUpdate.Start();
            }
            catch (Win32Exception e3)
            {
                Utils.ExceptionLog("updater_DownloadFileCompleted", "could not start new application version", e3);
                MessageBox.Show(Translations.getTranslatedString("cantStartNewApp") + newExeName);
            }
            Application.Exit();
        }

        //gets the version of tanks that this is, in the format
        //of the res_mods version folder i.e. 0.9.17.0.3
        private string getFolderVersion()
        {
            if (!File.Exists(Path.Combine(tanksLocation, "version.xml")))
                return null;
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(tanksLocation, "version.xml"));
            XmlNode node = doc.SelectSingleNode("//version.xml/version");
            string[] temp = node.InnerText.Split('#');
            string version = temp[0].Trim();
            version = version.Substring(2);
            return version;
        }

        //check to see if the supplied version of tanks is on the list of supported client versions
        private bool isClientVersionSupported(string detectedVersion)
        {
            supportedVersions.Clear();
            string xmlString = Utils.GetStringFromZip(Settings.ManagerInfoDatFile, "supported_clients.xml");  //xml doc name can change
            XDocument doc = XDocument.Parse(xmlString);
            bool result = doc.Descendants("version")
                   .Where(arg => arg.Value.Equals(detectedVersion))
                   .Any();
            if (result)
            {
                XElement element = doc.Descendants("version")
                   .Where(arg => arg.Value.Equals(detectedVersion))
                   .Single();
                // store the onlinefolder version to the string
                Settings.TanksOnlineFolderVersion = element.Attribute("folder").Value;
            }
            else
            {
                // store the the last onlinefolder version to the string, if no valid detectedVersion association is found
                Settings.TanksOnlineFolderVersion = doc.Descendants("version").Last().Attribute("folder").Value;
            }
            // fill the supportedVersions array to possible create messages
            StringReader rdr = new StringReader(xmlString);
            var docV = new XPathDocument(rdr);
            foreach (var version in docV.CreateNavigator().Select("//versions/version"))
            {
                supportedVersions.Add(version.ToString());
            }
            return result;
        }

        #region Tanks Install Auto/Manuel Search Code
        //checks the registry to get the location of where WoT is installed
        private string autoFindTanks()
        {
            List<string> searchPathWoT = new List<string>();
            string[] registryPathArray = new string[] { };

            // here we need the value for the searchlist
            // check replay link
            registryPathArray = new string[] { @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.wotreplay\shell\open\command", @"HKEY_CURRENT_USER\Software\Classes\.wotreplay\shell\open\command" };
            foreach (string regEntry in registryPathArray)
            {
                // get values from from registry
                object obj = Registry.GetValue(regEntry, "", -1);
                // if it is not "null", it is containing possible a string
                if (obj != null)
                {
                    try
                    {
                        // add the thing to the checklist, but remove the Quotation Marks in front of the string and the trailing -> " "%1"
                        searchPathWoT.Add(((string)obj).Substring(1).Substring(0, ((string)obj).Length - 7));
                    }
                    catch
                    { } // only exception catching
                }
            }

            // here we need the value for the searchlist
            string regPath = @"HKEY_CURRENT_USER\Software\Wargaming.net\Launcher\Apps\wot";
            RegistryKey subKeyHandle = Registry.CurrentUser.OpenSubKey(regPath.Replace(@"HKEY_CURRENT_USER\", ""));
            if (subKeyHandle != null)
            {
                // get the value names at the reg Key one by one
                foreach (string valueName in subKeyHandle.GetValueNames())
                {
                    // read the value from the regPath
                    object obj = Registry.GetValue(regPath, valueName, -1);
                    if (obj != null)
                    {
                        try
                        {
                            // we did get only a path to used WoT folders, so add the game name to the path and add it to the checklist
                            searchPathWoT.Add(Path.Combine((string)obj, "WorldOfTanks.exe"));
                        }
                        catch
                        { } // only exception catching
                    }
                }
            }

            // here we need the value name for the searchlist
            registryPathArray = new string[] { @"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache", @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store" };
            foreach (string p in registryPathArray)
            {
                // set the handle to the registry key
                subKeyHandle = Registry.CurrentUser.OpenSubKey(p);
                if (subKeyHandle == null) continue;            // subKeyHandle == null not existsting
                // parse all value names of the registry key abouve
                foreach (string valueName in subKeyHandle.GetValueNames())
                {
                    try
                    {
                        // if the lower string "worldoftanks.exe" is contained => match !!
                        if (valueName.ToLower().Contains("Worldoftanks.exe".ToLower()))
                        {
                            // remove (replace it with "") the attachment ".ApplicationCompany" or ".FriendlyAppName" in the string and add the string to the searchlist
                            searchPathWoT.Add(valueName.Replace(".ApplicationCompany", "").Replace(".FriendlyAppName", ""));
                        }
                    }
                    catch
                    { } // only exception catching
                }
            }

            // this searchlist is long, maybe 30-40 entries (system depended), but the best possibility to find a currently installed WoT game.
            foreach (string path in searchPathWoT)
            {
                if (File.Exists(path))
                {
                    Logging.Manager(string.Format("valid game path found: {0}", path));
                    // write the path to the central value holder
                    tanksLocation = path;
                    // return the path
                    return path;
                }
            }
            // send "null" back if nothing found
            return null;
        }

        //prompts the user to specify where the "WorldOfTanks.exe" file is
        //return the file path and name of "WorldOfTanks.exe"
        public string manuallyFindTanks()
        {
            // try to give an untrained user a littlebit support
            if (autoFindTanks() != null)
            {
                FindWotExe.InitialDirectory = Path.GetDirectoryName(tanksLocation);
            }
            //unable to find it in the registry (or user activated manually selection), so ask for it
            if (FindWotExe.ShowDialog().Equals(DialogResult.Cancel))
            {
                downloadProgress.Text = Translations.getTranslatedString("canceled");
                return null;
            }
            tanksLocation = FindWotExe.FileName;
            return "all good";
        }
        #endregion

        //handler for before the window is displayed
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //set window header text to current version so user knows
            //this.Text = this.Text +  " " + ManagerVersion();
            ApplicationVersionLabel.Text = "Application v" + ManagerVersion();
            if (Program.testMode) this.Text = this.Text + " TEST MODE";
            if (Program.betaDatabase) this.Text = this.Text + " (BETA DB)";
            if (Program.Version == Program.ProgramVersion.Beta) this.Text = this.Text + " (BETA APP)";
            if (Program.Version == Program.ProgramVersion.Alpha) this.Text = this.Text + " (ALPHA APP)";
            //setup the gif preview loading window
            gp = new LoadingGifPreview(this.Location.X + this.Size.Width + 5, this.Location.Y);
            //show the wait screen
            PleaseWait wait = new PleaseWait();
            wait.Show();
            WebRequest.DefaultWebProxy = null;
            Application.DoEvents();
            Logging.Manager(string.Format("|RelHax Modpack {0} ({1})", ManagerVersion(), Program.Version == Program.ProgramVersion.Beta ? "beta" : Program.Version == Program.ProgramVersion.Alpha ? "alpha" : "stable"));
            Logging.Manager(string.Format("|Built on {0}", compileTime()));
            Logging.Manager(string.Format("|Running on {0}", System.Environment.OSVersion.ToString()));
            /*
            //check for single instance
            Logging.Manager("Check for single instance");
            wait.loadingDescBox.Text = Translations.getTranslatedString("appSingleInstance");
            Application.DoEvents();
            int numberInstances = 0;
            foreach (Process p in Process.GetProcesses())
            {
                string s = p.MainWindowTitle;
                if(s.Contains("Relhax"))
                {
                    numberInstances++;
                }
            }
            if(numberInstances > 2)
            {
                MessageBox.Show(Translations.getTranslatedString("anotherInstanceRunning"));
                Application.Exit();
            }
            */
            //create directory structures
            wait.loadingDescBox.Text = Translations.getTranslatedString("verDirStructure");
            Application.DoEvents();
            Logging.Manager("Verifying Directory Structure");
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "RelHaxDownloads"))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "RelHaxDownloads"));
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "RelHaxUserMods"))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "RelHaxUserMods"));
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "RelHaxModBackup"))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "RelHaxModBackup"));
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "RelHaxUserConfigs"))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "RelHaxUserConfigs"));
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "RelHaxTemp"))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "RelHaxTemp"));
            //check if old dll files can be deleted
            try
            {
                string[] filesToDelete = { "DotNetZip.dll", "Ionic.Zip.dll", "Newtonsoft.Json.dll" };
                foreach (string s in filesToDelete)
                    if (File.Exists(Path.Combine(Application.StartupPath, s)))
                        File.Delete(Path.Combine(Application.StartupPath, s));
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex);
            }

            //add method to disable the modpack for during patch day
            //this will involve having a hard coded true or false, along with a command line arguement to over-ride
            //to disable from patch day set it to false.
            //to enable for patch day (prevent users to use it), set it to true.
            if (false && !Program.patchDayTest)
            {
                Logging.Manager("Patch day disable detected. Remember To override use /patchday");
                MessageBox.Show(Translations.getTranslatedString("patchDayMessage"));
                this.Close();
            }

            //check for updates
            wait.loadingDescBox.Text = Translations.getTranslatedString("checkForUpdates");
            Application.DoEvents();
            this.CheckmanagerUpdates();

            //load settings
            wait.loadingDescBox.Text = Translations.getTranslatedString("loadingSettings");
            Logging.Manager("Loading settings");
            Settings.LoadSettings();
            this.ApplySettings(true);
            if (Program.testMode)
            {
                Logging.Manager("Test Mode is ON, loading local modInfo.xml");
            }
            if (Program.autoInstall)
            {
                Logging.Manager("Auto Install is ON, checking for config pref xml at " + Path.Combine(Application.StartupPath, "RelHaxUserConfigs", Program.configName));
                if (!File.Exists(Path.Combine(Application.StartupPath, "RelHaxUserConfigs", Program.configName)))
                {
                    Logging.Manager(string.Format("ERROR: {0} does NOT exist, loading in fontRegular mode", Program.configName));
                    MessageBox.Show(string.Format(Translations.getTranslatedString("configNotExist"), Program.configName));
                    Program.autoInstall = false;
                }
                if (!Settings.CleanInstallation)
                {
                    Logging.Manager("ERROR: clean installation is set to false. This must be set to true for auto install to work. Loading in fontRegular mode.");
                    MessageBox.Show(Translations.getTranslatedString("autoAndClean"));
                    Program.autoInstall = false;
                }
                if (Settings.FirstLoad)
                {
                    Logging.Manager("ERROR: First time loading cannot be an auto install mode, loading in fontRegular mode");
                    MessageBox.Show(Translations.getTranslatedString("autoAndFirst"));
                    Program.autoInstall = false;
                }
            }
            //check if it can still load in autoInstall config mode
            if (Program.autoInstall)
            {
                Logging.Manager("Program.autoInstall still true, loading in auto install mode");
                wait.Close();
                this.installRelhaxMod_Click(null, null);
                return;
            }
            if (Settings.FirstLoad)
            {
                //helper = new FirstLoadHelper(this.Location.X + this.Size.Width + 10, this.Location.Y);
                //helperText = helper.helperText.Text;
                //helper.Show();
                //Settings.FirstLoad = false;
                generic_MouseLeave(null, null);
            }
            wait.Close();
            ToggleUIButtons(true);
            Application.DoEvents();
            Program.saveSettings = true;
        }

        //handler for when the install relhax modpack button is pressed
        //basicly the entire install process
        private void installRelhaxMod_Click(object sender, EventArgs e)
        {
            Utils.TotallyNotStatPaddingForumPageViewCount();
            ToggleUIButtons(false);
            //reset progress bars
            parrentProgressBar.Value = parrentProgressBar.Minimum;
            totalProgressBar.Value = totalProgressBar.Minimum;
            childProgressBar.Value = childProgressBar.Minimum;
            //get the user appData folder
            appDataFolder = "";
            appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wargaming.net", "WorldOfTanks");
            Logging.Manager("appDataFolder parsed as " + appDataFolder);
            if (appDataFolder.Equals("") || !Directory.Exists(appDataFolder))
            {
                Logging.Manager("ERROR: appDataFolder does not exist");
                appDataFolder = "-1";
                if (Settings.ClearCache)
                {
                    //can't locate folder, continue installation anyway?
                    DialogResult clearCacheFailResult = MessageBox.Show(Translations.getTranslatedString("appDataFolderNotExist"), Translations.getTranslatedString("appDataFolderNotExistHeader"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (clearCacheFailResult == DialogResult.No)
                    {
                        Logging.Manager("user stopped installation");
                        ToggleUIButtons(true);
                        return;
                    }
                }
            }
            //reset the interface
            this.downloadProgress.Text = "";
            //attempt to locate the tanks directory automatically
            //if it fails, it will prompt the user to return the world of tanks exe
            if (Settings.ForceManuel || this.autoFindTanks() == null)
            {
                if (this.manuallyFindTanks() == null)
                {
                    Logging.Manager("user stopped installation");
                    ToggleUIButtons(true);
                    return;
                }
            }
            //parse all strings for installation
            tanksLocation = tanksLocation.Substring(0, tanksLocation.Length - 17);
            Settings.TanksLocation = tanksLocation;
            Logging.Manager("tanksLocation parsed as " + tanksLocation);
            Logging.Manager("customUserMods parsed as " + Path.Combine(Application.StartupPath, "RelHaxUserMods"));
            if (tanksLocation.Equals(Application.StartupPath))
            {
                //display error and abort
                MessageBox.Show(Translations.getTranslatedString("moveOutOfTanksLocation"));
                ToggleUIButtons(true);
                return;
            }
            tanksVersion = this.getFolderVersion();
            tanksVersionForInstaller = tanksVersion;
            Settings.TanksVersion = tanksVersion;
            Logging.Manager("tanksVersion parsed as " + tanksVersion);
            //determine if the tanks client version is supported
            if (!Program.testMode && !isClientVersionSupported(tanksVersion))
            {
                //log and inform the user
                Logging.Manager("WARNING: Detected client version is " + tanksVersion + ", not supported");
                Logging.Manager("Supported versions are: " + string.Join(", ", supportedVersions));
                // parse the string that we get from the server and delete all "Testserver" entries (Testserver entries are the version number with prefix "T")
                string publicVersions = string.Join("\n", supportedVersions.Select(sValue => sValue.Trim()).ToArray().Where(s => !(s.Substring(0, 1) == "T")).ToArray());
                MessageBox.Show(string.Format("{0}: {1}\n{2}\n\n{3}:\n{4}", Translations.getTranslatedString("detectedClientVersion"), tanksVersion, Translations.getTranslatedString("supportNotGuarnteed"), Translations.getTranslatedString("supportedClientVersions"), publicVersions), Translations.getTranslatedString("critical"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // select the last public modpack version
                tanksVersion = publicVersions.Split('\n').Last();
                // go to Client check again, because the online folder must be set correct
                isClientVersionSupported(tanksVersion);
                Logging.Manager(string.Format("Version selected: {0}  OnlineFolder: {1}", tanksVersion, Settings.TanksOnlineFolderVersion));
            }
            //if the user wants to, check if the database has actually changed
            if (Settings.NotifyIfSameDatabase && SameDatabaseVersions())        // the get the string databaseVersionString filles in any case, the function must be performed first!
            {
                if (MessageBox.Show(Translations.getTranslatedString("DatabaseVersionsSameBody"), Translations.getTranslatedString("DatabaseVersionsSameHeader"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    ToggleUIButtons(true);
                    return;
                }
            }
            //reset the childProgressBar value
            childProgressBar.Maximum = 100;
            //childProgressBar.Value = 0;
            //check to make sure that the md5hashdatabase is valid before using it
            if ((File.Exists(Settings.MD5HashDatabaseXmlFile)) && (!XMLUtils.IsValidXml(Settings.MD5HashDatabaseXmlFile)))
            {
                File.Delete(Settings.MD5HashDatabaseXmlFile);
            }
            //show the mod selection window
            ModSelectionList list = new ModSelectionList()
            {
                TanksVersion = this.tanksVersion,
                TanksLocation = this.tanksLocation,
                MainWindowStartX = this.Location.X + this.Size.Width,
                MainWindowStartY = this.Location.Y
            };
            if (list.ShowDialog() != DialogResult.OK)
            {
                try
                {
                    list.Dispose();
                }
                catch
                {
                    Logging.Manager("INFO: Failed to dispose list");
                }
                list = null;
                GC.Collect();
                ToggleUIButtons(true);
                return;
            }
            //check to see if WoT is running
            bool WoTRunning = true;
            while (WoTRunning)
            {
                WoTRunning = false;
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle.Equals("WoT Client"))
                        WoTRunning = true;
                }
                if (!WoTRunning)
                    break;
                MessageBox.Show(Translations.getTranslatedString("WoTRunningMessage"), Translations.getTranslatedString("WoTRunningHeader"));
            }
            //have the application display that it is loading. it is actually doing installation calculations
            downloadProgress.Text = Translations.getTranslatedString("loading");
            Application.DoEvents();
            //run the installer calculations
            ProcessInstallCalculations(list);
        }

        #region Installer calculations
        private void ProcessInstallCalculations(ModSelectionList list)
        {

            /*
             * parses all the mods and configs into seperate lists for many types op options
             * like mods/configs to install, mods/configs with data, and others
            */
            //copies it instead
            currentDependencies = new List<Dependency>(list.Dependencies);
            currentLogicalDependencies = new List<LogicalDependency>(list.LogicalDependencies);
            parsedCatagoryLists = new List<Category>(list.ParsedCatagoryList);
            globalDependenciesToInstall = new List<Dependency>(list.GlobalDependencies);
            dependenciesToInstall = new List<Dependency>();
            logicalDependenciesToInstall = new List<LogicalDependency>();
            modsConfigsToInstall = new List<SelectableDatabasePackage>();
            appendedDependenciesToInstall = new List<Dependency>();
            modsConfigsWithData = new List<SelectableDatabasePackage>();
            patchList = new List<Patch>();
            userMods = new List<Mod>();
            ModsWithShortcuts = new List<SelectableDatabasePackage>();
            Shortcuts = new List<Shortcut>();
            DatabasePackagesToDownload = new List<IDatabasePackage>();
            InstallGroups = new List<InstallGroup>();

            //code for super extraction mode. seperates the categories into installer groups.
            int installGroupCounter = 0;
            if(Settings.SuperExtraction)
            {
                foreach(Category c in parsedCatagoryLists)
                {
                    if(c.InstallGroup == installGroupCounter)
                    {
                        if (InstallGroups.Count == installGroupCounter)
                            InstallGroups.Add(new InstallGroup());
                        InstallGroups[installGroupCounter].Categories.Add(c);
                    }
                    else
                    {
                        while (installGroupCounter != c.InstallGroup)
                            installGroupCounter++;
                        if (InstallGroups.Count == installGroupCounter)
                            InstallGroups.Add(new InstallGroup());
                        InstallGroups[installGroupCounter].Categories.Add(c);
                    }
                }
            }

            try
            {
                //if mod/config is Enabled and checked, add it to list of mods to extract/install
                foreach (Category c in parsedCatagoryLists)
                {
                    //will itterate through every catagory once
                    foreach (Mod m in c.Mods)
                    {
                        //will itterate through every mod of every catagory once
                        if (m.Enabled && m.Checked)
                        {
                            //move each mod that is enalbed and checked to a new list of mods to install
                            //also check that it actually has a zip file
                            if (!m.ZipFile.Equals(""))
                            {
                                m.ExtractPath = m.ExtractPath.Equals("") ? Utils.ReplaceMacro(@"{app}") : Utils.ReplaceMacro(m.ExtractPath);
                                modsConfigsToInstall.Add(m);
                            }

                            //since it is checked, regardless if it has a zipfile, check if it has userdata
                            if (m.UserFiles.Count > 0)
                                modsConfigsWithData.Add(m);

                            //if it has shortcuts to create, add them to the list here
                            if (m.ShortCuts.Count > 0)
                                ModsWithShortcuts.Add(m);

                            //check for configs
                            if (m.configs.Count > 0)
                                ProcessConfigs(m.configs);

                            //at least one mod of this catagory is checked, add any dependenciesToInstall required
                            if (c.Dependencies.Count > 0)
                                ProcessDependencies(c.Dependencies);

                            //check dependency is Enabled and has a zip file with it
                            if (m.Dependencies.Count > 0)
                                ProcessDependencies(m.Dependencies);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "if mod/config is Enabled and checked, add it to list of mods to extract/install", ex);
            }

            try
            {
                //build the list of mods and configs that use each logical dependency
                foreach (LogicalDependency d in currentLogicalDependencies)
                {
                    foreach (Dependency depD in currentDependencies)
                    {
                        foreach (LogicalDependency ld in depD.LogicalDependencies)
                        {
                            if (ld.PackageName.Equals(d.PackageName))
                            {
                                DatabaseLogic dbl = new DatabaseLogic()
                                {
                                    PackageName = depD.PackageName,
                                    Enabled = depD.Enabled,
                                    Checked = dependenciesToInstall.Contains(depD),
                                    NotFlag = ld.NegateFlag
                                };
                                d.DatabasePackageLogic.Add(dbl);
                            }
                        }
                    }
                    try
                    {
                        //itterate through every mod and config once for each dependency
                        //check each one's dependecy list, if PackageName's match, add it to the dependency's list of mods/configs that use it
                        foreach (Category c in parsedCatagoryLists)
                        {
                            //will itterate through every catagory once
                            foreach (Mod m in c.Mods)
                            {
                                foreach (LogicalDependency ld in m.LogicalDependencies)
                                {
                                    if (ld.PackageName.Equals(d.PackageName))
                                    {
                                        DatabaseLogic dbl = new DatabaseLogic()
                                        {
                                            PackageName = m.PackageName,
                                            Enabled = m.Enabled,
                                            Checked = m.Checked,
                                            NotFlag = ld.NegateFlag
                                        };
                                        d.DatabasePackageLogic.Add(dbl);
                                    }
                                }
                                if (m.configs.Count > 0)
                                    ProcessConfigsLogical(d, m.configs);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.ExceptionLog("installRelhaxMod_Click", "build the list of mods and configs that use each logical dependency", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "add package to dependency's list of mods/configs that use it", ex);
            }

            try
            {
                //now each logical dependency has a complete list of every dependency, mod, and config that uses it, and if it is Enabled and checked
                //indicate if the logical dependency will be installed
                foreach (LogicalDependency ld in currentLogicalDependencies)
                {
                    //idea is that if all mod/config/dependency are to be installed, then install the logical dependency
                    //and factor in the negate flag
                    bool addIt = true;
                    foreach (DatabaseLogic dl in ld.DatabasePackageLogic)
                    {
                        if (dl.NotFlag)
                        {
                            //package must NOT be checked for it to be included
                            //Enabled must = true, checked must = false
                            //otherwise break and don't add
                            if (dl.Enabled && dl.Checked)
                            {
                                addIt = false;
                                break;
                            }
                        }
                        else
                        {
                            //package MUST be checked for it to be included
                            //Enabled must = true, checked must = true
                            //otherwise break and don't add
                            if (dl.Enabled && !dl.Checked)
                            {
                                addIt = false;
                                break;
                            }
                        }
                    }
                    if (addIt && !logicalDependenciesToInstall.Contains(ld))
                        logicalDependenciesToInstall.Add(ld);
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "now each logical dependency has a complete list of every dependency ...", ex);
            }

            //create the list of shortcuts
            try
            {
                foreach (Dependency d in globalDependenciesToInstall)
                {
                    if (d.Enabled && d.shortCuts.Count > 0)
                    {
                        foreach (Shortcut sc in d.shortCuts)
                        {
                            if (sc.Enabled)
                            {
                                Shortcuts.Add(sc);
                            }
                        }
                    }
                }
                foreach (Dependency d in dependenciesToInstall)
                {
                    if (d.Enabled && d.shortCuts.Count > 0)
                    {
                        foreach (Shortcut sc in d.shortCuts)
                        {
                            if (sc.Enabled)
                            {
                                Shortcuts.Add(sc);
                            }
                        }
                    }
                }
                foreach (LogicalDependency ld in logicalDependenciesToInstall)
                {
                    if (ld.Enabled && ld.Shortcuts.Count > 0)
                    {
                        foreach (Shortcut sc in ld.Shortcuts)
                        {
                            if (sc.Enabled)
                            {
                                Shortcuts.Add(sc);
                            }
                        }
                    }
                }
                foreach (SelectableDatabasePackage dbo in modsConfigsToInstall)
                {
                    if (dbo.Enabled && dbo.ShortCuts.Count > 0)
                    {
                        foreach (Shortcut sc in dbo.ShortCuts)
                        {
                            if (sc.Enabled)
                            {
                                Shortcuts.Add(sc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("create the list of shortcuts", ex);
            }

            //verify that all global dependencies, dependencies, and logicalDependencies are actually Enabled and have valid zip files
            //if they don't, remove them. if they do, macro the ExtractPath
            try
            {
                for (int i = 0; i < globalDependenciesToInstall.Count; i++)
                {
                    if ((!globalDependenciesToInstall[i].Enabled) || globalDependenciesToInstall[i].ZipFile.Equals(""))
                    {
                        globalDependenciesToInstall.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        globalDependenciesToInstall[i].ExtractPath = globalDependenciesToInstall[i].ExtractPath.Trim().Equals("") ? Utils.ReplaceMacro(@"{app}") : Utils.ReplaceMacro(globalDependenciesToInstall[i].ExtractPath.Trim());
                    }
                }
                for (int i = 0; i < dependenciesToInstall.Count; i++)
                {
                    if ((!dependenciesToInstall[i].Enabled) || dependenciesToInstall[i].ZipFile.Equals(""))
                    {
                        dependenciesToInstall.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        dependenciesToInstall[i].ExtractPath = dependenciesToInstall[i].ExtractPath.Trim().Equals("") ? Utils.ReplaceMacro(@"{app}") : Utils.ReplaceMacro(dependenciesToInstall[i].ExtractPath.Trim());
                    }
                }
                for (int i = 0; i < logicalDependenciesToInstall.Count; i++)
                {
                    if ((!logicalDependenciesToInstall[i].Enabled) || logicalDependenciesToInstall[i].ZipFile.Equals(""))
                    {
                        logicalDependenciesToInstall.RemoveAt(i);
                        i--;
                    }
                    {
                        logicalDependenciesToInstall[i].ExtractPath = logicalDependenciesToInstall[i].ExtractPath.Trim().Equals("") ? Utils.ReplaceMacro(@"{app}") : Utils.ReplaceMacro(logicalDependenciesToInstall[i].ExtractPath.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "verify that all ... are actually Enabled", ex);
            }

            //check for dependencies that actually need to be installed at the end
            try
            {
                for (int i = 0; i < globalDependenciesToInstall.Count; i++)
                {
                    if (globalDependenciesToInstall[i].AppendExtraction)
                    {
                        appendedDependenciesToInstall.Add(globalDependenciesToInstall[i]);
                        globalDependenciesToInstall.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < dependenciesToInstall.Count; i++)
                {
                    if (dependenciesToInstall[i].AppendExtraction)
                    {
                        appendedDependenciesToInstall.Add(dependenciesToInstall[i]);
                        dependenciesToInstall.RemoveAt(i);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "check for dependencies that actually need to be installed at the end", ex);
            }

            try
            {
                //check for any user mods to install
                for (int i = 0; i < list.UserMods.Count; i++)
                {
                    if (list.UserMods[i].Enabled && list.UserMods[i].Checked)
                    {
                        this.userMods.Add(list.UserMods[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("installRelhaxMod_Click", "check for any user mods to install", ex);
            }

            //check that we will actually install something
            if (modsConfigsToInstall.Count == 0 && userMods.Count == 0)
            {
                //pull out because there are no mods to install
                downloadProgress.Text = Translations.getTranslatedString("idle");
                ToggleUIButtons(true);
                list.Dispose();
                list = null;
                GC.Collect();
                return;
            }
            //if the user did not select any relhax modpack mods to install
            if (modsConfigsToInstall.Count == 0)
            {
                //clear any dependencies and logicalDependencies since this is a user mod only installation
                dependenciesToInstall.Clear();
                logicalDependenciesToInstall.Clear();
                appendedDependenciesToInstall.Clear();
            }
            //macro replacements

            //reset the download counter
            downloadCounter = -1;
            //create the download list
            foreach (Dependency d in globalDependenciesToInstall)
            {
                if (d.DownloadFlag)
                {
                    d.ReadyForInstall = false;
                    DatabasePackagesToDownload.Add(d);
                }
                else
                    d.ReadyForInstall = true;
            }
            foreach (Dependency d in dependenciesToInstall)
            {
                if (d.DownloadFlag)
                {
                    d.ReadyForInstall = false;
                    DatabasePackagesToDownload.Add(d);
                }
                else
                    d.ReadyForInstall = true;
            }
            foreach (Dependency d in appendedDependenciesToInstall)
            {
                if (d.DownloadFlag)
                {
                    d.ReadyForInstall = false;
                    DatabasePackagesToDownload.Add(d);
                }
                else
                    d.ReadyForInstall = true;
            }
            foreach (LogicalDependency ld in logicalDependenciesToInstall)
            {
                if (ld.DownloadFlag)
                {
                    ld.ReadyForInstall = false;
                    DatabasePackagesToDownload.Add(ld);
                }
                else
                    ld.ReadyForInstall = true;
            }
            foreach (SelectableDatabasePackage dbo in modsConfigsToInstall)
            {
                if (dbo.DownloadFlag)
                {
                    //CRC's don't match, need to re-download
                    dbo.ReadyForInstall = false;
                    DatabasePackagesToDownload.Add(dbo);
                }
                else
                    dbo.ReadyForInstall = true;
            }
            //upadte the parrentProgressPar
            parrentProgressBar.Maximum = DatabasePackagesToDownload.Count;
            //at this point, there may be user mods selected,
            //and there is at least one mod to extract
            downloader_DownloadFileCompleted(null, null);
            //release no longer needed rescources and end the installation process
            list.Dispose();
            list = null;
            GC.Collect();
        }

        private void ProcessConfigs(List<Config> configList)
        {
            foreach (Config config in configList)
            {
                if (config.Enabled && config.Checked)
                {
                    if (!config.ZipFile.Equals(""))
                    {
                        config.ExtractPath = config.ExtractPath.Equals("")? Utils.ReplaceMacro(@"{app}") : Utils.ReplaceMacro(config.ExtractPath);
                        modsConfigsToInstall.Add(config);
                    }

                    //check for userdata
                    if (config.UserFiles.Count > 0)
                        modsConfigsWithData.Add(config);

                    //check for shortcuts
                    if (config.ShortCuts.Count > 0)
                        ModsWithShortcuts.Add(config);

                    //check for configs
                    if (config.configs.Count > 0)
                        ProcessConfigs(config.configs);

                    //check for dependencies
                    if (config.Dependencies.Count > 0)
                        ProcessDependencies(config.Dependencies);
                }
            }
        }

        private void ProcessConfigsLogical(LogicalDependency d, List<Config> configList)
        {
            foreach (Config config in configList)
            {
                foreach (LogicalDependency ld in config.LogicalDependencies)
                {
                    if (ld.PackageName.Equals(d.PackageName))
                    {
                        DatabaseLogic dl = new DatabaseLogic()
                        {
                            PackageName = config.PackageName,
                            Enabled = config.Enabled,
                            Checked = config.Checked,
                            NotFlag = ld.NegateFlag
                        };
                        d.DatabasePackageLogic.Add(dl);
                    }
                }
            }
        }

        //processes a list of dependencies to add them (if needed) to the list of dependencies to install
        private void ProcessDependencies(List<Dependency> dependencies)
        {
            //every dependency is only a PackageName, and each must be added if they are not there already
            //but first need to find it
            foreach (Dependency d in dependencies)
            {
                Dependency temp = null;
                //find the actual dependency object from the list of available dependencies
                bool error = true;
                foreach (Dependency dd in currentDependencies)
                {
                    if (dd.PackageName.Equals(d.PackageName))
                    {
                        //the PackageName has been linked to the dependency
                        error = false;
                        temp = dd;
                        break;
                    }
                }
                if (error)
                {
                    Logging.Manager(string.Format("ERROR: could not match PackageName '{0}' from the list of dependencies", d.PackageName));
                    break;
                }
                //dependency has been found, if it's not in the list currently to install, add it
                if (!dependenciesToInstall.Contains(temp))
                    dependenciesToInstall.Add(temp);
            }
        }
        
        //Checks if the current database version is the same as the database version last installed into the selected World_of_Tanks directory
        private bool SameDatabaseVersions()
        {
            try
            {
                string xmlString = Utils.GetStringFromZip(Settings.ManagerInfoDatFile, "manager_version.xml");  //xml doc name can change
                XDocument doc = XDocument.Parse(xmlString);

                var databaseVersion = doc.CreateNavigator().SelectSingleNode("/version/database");
                databaseVersionString = databaseVersion.InnerXml;
                Settings.DatabaseVersion = databaseVersionString;
                string installedfilesLogPath = Path.Combine(tanksLocation, "logs", "installedRelhaxFiles.log");
                if (!File.Exists(installedfilesLogPath))
                    return false;
                string[] lastInstalledDatabaseVersionString = File.ReadAllText(installedfilesLogPath).Split('\n');
                //use index 0 of array, index 18 of string array
                string theDatabaseVersion = lastInstalledDatabaseVersionString[0].Substring(18).Trim();
                if (databaseVersionString.Equals(theDatabaseVersion))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog("SameDatabaseVersions", ex);
                return false;
            }
        }
        #endregion

        #region progress reporting

        private string createExtractionMsgBoxProgressOutput(string[] s)
        {
            return string.Format("{0} {1} {2} {3}\n{4}\n{5}: {6} MB",
                Translations.getTranslatedString("extractingPackage"),
                    s[0],
                    Translations.getTranslatedString("of"),
                    s[1],
                    Utils.Truncate(string.Format("{0}: {1}", Translations.getTranslatedString("file"), s[2]), downloadProgress.Font, downloadProgress.Width, 2),
                    Translations.getTranslatedString("size"),
                    s[3].Equals("0") ? "0.01" : s[3]);
        }

        private void I_InstallProgressChanged(object sender, InstallerEventArgs e)
        {
            string message = "";
            totalProgressBar.Maximum = (int)InstallerEventArgs.InstallProgress.Done;
            switch (e.InstalProgress)
            {
                case InstallerEventArgs.InstallProgress.BackupMods:
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.BackupMods;
                    parrentProgressBar.Value = 0;
                    message = string.Format("{0} {1} {2} {3}", Translations.getTranslatedString("backupModFile"), e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.BackupUserData:
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.BackupUserData;
                    parrentProgressBar.Value = 0;
                    message = string.Format("{0} {1} {2} {3}", Translations.getTranslatedString("backupUserdatas"), e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.DeleteMods:
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.DeleteMods;
                    parrentProgressBar.Value = 0;
                    message = string.Format("{0} {1} {2} {3}\n{4}: {5}", Translations.getTranslatedString("deletingFiles"), e.ChildProcessed, Translations.getTranslatedString("of"),
                        e.ChildTotalToProcess, Translations.getTranslatedString("file"), e.currentFile);
                    break;
                case InstallerEventArgs.InstallProgress.DeleteWoTCache:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.DeleteWoTCache;
                    childProgressBar.Value = 0;
                    parrentProgressBar.Value = 0;
                    message = Translations.getTranslatedString("deletingWOTCache") + " ";
                    break;
                case InstallerEventArgs.InstallProgress.ExtractGlobalDependencies:
                case InstallerEventArgs.InstallProgress.ExtractDependencies:
                case InstallerEventArgs.InstallProgress.ExtractLogicalDependencies:
                case InstallerEventArgs.InstallProgress.ExtractMods:
                case InstallerEventArgs.InstallProgress.ExtractConfigs:
                case InstallerEventArgs.InstallProgress.ExtractAppendedDependencies:
                    totalProgressBar.Value = (int)e.InstalProgress;
                    parrentProgressBar.Maximum = e.ParrentTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ParrentProcessed) && (e.ParrentProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ParrentProcessed;
                    if (Settings.SuperExtraction && e.InstalProgress == InstallerEventArgs.InstallProgress.ExtractMods)
                    {
                        childProgressBar.Value = childProgressBar.Minimum;
                        message = string.Format("{0} {1} {2} {3}\n{4} {5}\n{6} MB", Translations.getTranslatedString("parallelExtraction"), e.ParrentProcessed, Translations.getTranslatedString("of"),
                            e.ParrentTotalToProcess, Translations.getTranslatedString("file"), e.currentFile, Math.Round(e.currentFileSizeProcessed / MBDivisor, 2).ToString());
                    }
                    else
                    {
                        childProgressBar.Maximum = e.ChildTotalToProcess;
                        if (e.ChildProcessed > 0)
                            if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                                childProgressBar.Value = e.ChildProcessed;
                        message = createExtractionMsgBoxProgressOutput(new string[] { e.ParrentProcessed.ToString(), e.ParrentTotalToProcess.ToString(),
                        e.currentFile, Math.Round(e.currentFileSizeProcessed / MBDivisor, 2).ToString() });
                    }
                    break;
                case InstallerEventArgs.InstallProgress.RestoreUserData:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.RestoreUserData;
                    parrentProgressBar.Value = 0;
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1} {2} {3}", Translations.getTranslatedString("restoringUserData"), e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.UnpackXmlFiles:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.UnpackXmlFiles;
                    childProgressBar.Minimum = 0;
                    parrentProgressBar.Minimum = 0;
                    parrentProgressBar.Value = 0;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1} {2} {3}\n{4}", Translations.getTranslatedString("unpackingXMLFiles"), e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess, e.currentFile);
                    break;
                case InstallerEventArgs.InstallProgress.PatchMods:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.PatchMods;
                    childProgressBar.Value = 0;
                    parrentProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1}, {2} {3} {4}", Translations.getTranslatedString("patchingFile"), e.currentFile, e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.InstallFonts:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.InstallFonts;
                    parrentProgressBar.Value = 0;
                    childProgressBar.Value = 0;
                    message = Translations.getTranslatedString("installingFonts") + " ";
                    break;
                case InstallerEventArgs.InstallProgress.ExtractUserMods:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.ExtractUserMods;
                    parrentProgressBar.Maximum = e.ParrentTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ParrentProcessed) && (e.ParrentProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ParrentProcessed;
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if (e.ChildProcessed > 0)
                        childProgressBar.Value = e.ChildProcessed;
                    message = createExtractionMsgBoxProgressOutput(new string[] { e.ParrentProcessed.ToString(), e.ParrentTotalToProcess.ToString(), e.currentFile,
                        Math.Round(e.currentFileSizeProcessed / MBDivisor, 2).ToString() });
                    break;
                case InstallerEventArgs.InstallProgress.PatchUserMods:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.PatchMods;
                    childProgressBar.Value = 0;
                    parrentProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1}, {2} {3} {4}", Translations.getTranslatedString("userPatchingFile"), e.currentFile, e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.ExtractAtlases:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.ExtractAtlases;
                    parrentProgressBar.Maximum = e.ParrentTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ParrentProcessed) && (e.ParrentProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ParrentProcessed;
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if (e.ChildProcessed > 0)
                        childProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0}: {1}\n{2}: {3}\n{4} {5} {6}", Translations.getTranslatedString("AtlasExtraction"), e.currentFile, Translations.getTranslatedString("AtlasTexture"),
                        e.currentSubFile, e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    break;
                case InstallerEventArgs.InstallProgress.CreateAtlases:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.CreateAtlases;
                    parrentProgressBar.Minimum = 0;
                    parrentProgressBar.Maximum = e.ParrentTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ParrentProcessed) && (e.ParrentProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ParrentProcessed;
                    childProgressBar.Minimum = 0;
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1} {2} {3}\n {4} {5} {6} {7}", Translations.getTranslatedString("AtlasCreating"), e.ParrentProcessed, Translations.getTranslatedString("of"), e.ParrentTotalToProcess, e.ChildProcessed,
                        Translations.getTranslatedString("of"), e.ChildTotalToProcess, Translations.getTranslatedString("stepsComplete"));//AtlasCreating, stepsComplete
                    /*
                    if (e.ChildTotalToProcess == -1)
                    {
                        childProgressBar.Value = childProgressBar.Minimum;
                        message = string.Format("{0}: {1}\n\n{2} {3}", Translations.getTranslatedString("AtlasCreating"), e.currentFile, e.ChildProcessed, Translations.getTranslatedString("AtlasOptimations"));
                    }
                    else
                    {
                        childProgressBar.Maximum = e.ChildTotalToProcess;
                        if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                            childProgressBar.Value = e.ChildProcessed;
                        message = string.Format("{0}: {1}\n{2}: {3}\n{4} {5} {6}", Translations.getTranslatedString("AtlasCreating"), e.currentFile, Translations.getTranslatedString("AtlasTexture"), e.currentSubFile, e.ChildProcessed, Translations.getTranslatedString("of"), e.ChildTotalToProcess);
                    }
                    */
                    break;
                case InstallerEventArgs.InstallProgress.InstallUserFonts:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.InstallFonts;
                    parrentProgressBar.Value = 0;
                    childProgressBar.Value = 0;
                    message = Translations.getTranslatedString("installingUserFonts") + " ";
                    break;
                case InstallerEventArgs.InstallProgress.CheckDatabase:
                    totalProgressBar.Value = (int)InstallerEventArgs.InstallProgress.CheckDatabase;
                    parrentProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((parrentProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= parrentProgressBar.Maximum))
                        parrentProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0}: {1}", Translations.getTranslatedString("deletingFile"), e.currentFile);
                    break;
                case InstallerEventArgs.InstallProgress.CleanUp:
                    message = Translations.getTranslatedString("done");
                    totalProgressBar.Value = totalProgressBar.Maximum;
                    parrentProgressBar.Maximum = 1;
                    parrentProgressBar.Value = parrentProgressBar.Maximum;
                    childProgressBar.Maximum = 1;
                    childProgressBar.Value = childProgressBar.Maximum;
                    downloadProgress.Text = message;
                    if (Settings.ShowInstallCompleteWindow)
                    {
                        using (InstallFinished IF = new InstallFinished(tanksLocation))
                        {
                            System.Media.SystemSounds.Beep.Play();
                            IF.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show(Translations.getTranslatedString("installationFinished"), Translations.getTranslatedString("information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case InstallerEventArgs.InstallProgress.Done:
                    //dispose of a lot of stuff
                    if (ins != null)
                    {
                        ins.Dispose();
                        ins = null;
                    }
                    if (unI != null)
                    {
                        unI.Dispose();
                        unI = null;
                    }
                    globalDependenciesToInstall = null;
                    dependenciesToInstall = null;
                    logicalDependenciesToInstall = null;
                    appendedDependenciesToInstall = null;
                    modsConfigsToInstall = null;
                    DatabasePackagesToDownload = null;
                    parsedCatagoryLists = null;
                    ModsWithShortcuts = null;
                    Shortcuts = null;
                    InstallGroups = null;
                    currentDependencies = null;
                    currentLogicalDependencies = null;
                    usedFilesList = null;
                    patchList = null;
                    userMods = null;
                    modsConfigsWithData = null;
                    if (Settings.FirstLoad)
                        Settings.FirstLoad = false;
                    if (Program.autoInstall)
                        Program.autoInstall = false;
                    ToggleUIButtons(true);
                    break;
                case InstallerEventArgs.InstallProgress.Uninstall:
                    totalProgressBar.Value = 0;
                    parrentProgressBar.Value = 0;
                    childProgressBar.Maximum = e.ChildTotalToProcess;
                    if ((childProgressBar.Minimum <= e.ChildProcessed) && (e.ChildProcessed <= childProgressBar.Maximum))
                        childProgressBar.Value = e.ChildProcessed;
                    message = string.Format("{0} {1} {2} {3}\n{4}: {5}", Translations.getTranslatedString("uninstallingFile"), e.ChildProcessed, Translations.getTranslatedString("of"),
                        e.ChildTotalToProcess, Translations.getTranslatedString("file"), e.currentFile);
                    break;
                case InstallerEventArgs.InstallProgress.UninstallDone:
                    message = Translations.getTranslatedString("done");
                    totalProgressBar.Value = totalProgressBar.Maximum;
                    parrentProgressBar.Maximum = 1;
                    parrentProgressBar.Value = parrentProgressBar.Maximum;
                    childProgressBar.Maximum = 1;
                    childProgressBar.Value = childProgressBar.Maximum;
                    break;
                default:
                    Logging.Manager("Invalid state: " + e.InstalProgress);
                    break;
            }
            if (errorCounter > 0 && Program.testMode)
            {
                this.ErrorCounterLabel.Visible = true;
                this.ErrorCounterLabel.Text = string.Format("Error counter: {0}", errorCounter);
            }
            downloadProgress.Text = message;
        }
        #endregion

        //Main method to uninstall the modpack
        private void UninstallRelhaxMod_Click(object sender, EventArgs e)
        {
            ToggleUIButtons(false);
            //reset progress bars
            parrentProgressBar.Value = parrentProgressBar.Minimum;
            totalProgressBar.Value = totalProgressBar.Minimum;
            childProgressBar.Value = childProgressBar.Minimum;
            //reset the interface
            this.downloadProgress.Text = "";
            //attempt to locate the tanks directory
            if (Settings.ForceManuel || this.autoFindTanks() == null)
            {
                if (this.manuallyFindTanks() == null)
                {
                    ToggleUIButtons(true);
                    return;
                }
            }
            //parse all strings
            tanksLocation = tanksLocation.Substring(0, tanksLocation.Length - 17);
            Logging.Manager(string.Format("tanksLocation parsed as {0}", tanksLocation));
            Logging.Manager(string.Format("customUserMods parsed as {0}", Path.Combine(Application.StartupPath, "RelHaxUserMods")));
            tanksVersion = this.getFolderVersion();
            if (MessageBox.Show(string.Format("{0}\n\n{1}", Translations.getTranslatedString("confirmUninstallMessage"), tanksLocation), Translations.getTranslatedString("confirmUninstallHeader"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                unI = new Installer()
                {
                    AppPath = Application.StartupPath,
                    TanksLocation = tanksLocation,
                    TanksVersion = tanksVersion
                };
                unI.InstallProgressChanged += I_InstallProgressChanged;
                Logging.Manager("Started Uninstallation process");
                //run the recursive complete uninstaller
                unI.StartUninstallation();
            }
            else
            {
                ToggleUIButtons(true);
            }
        }
        //applies all settings from static settings class to this form
        private void ApplySettings(bool init = false)
        {
            //set translation text
            Control[] translationSetList = new Control[] { forceManuel, cleanInstallCB, backupModsCheckBox, cancerFontCB, saveLastInstallCB, saveUserDataCB, darkUICB,
                installRelhaxMod, uninstallRelhaxMod, settingsGroupBox,loadingImageGroupBox, languageSelectionGB, findBugAddModLabel, formPageLink, selectionDefault, selectionLegacy, donateLabel,
                cancelDownloadButton, fontSizeGB, expandNodesDefault, disableBordersCB, clearCacheCB, DiscordServerLink, viewAppUpdates, viewDBUpdates, disableColorsCB, clearLogFilesCB,
                notifyIfSameDatabaseCB, ShowInstallCompleteWindowCB,  createShortcutsCB, InstantExtractionCB };
            foreach (var set in translationSetList)
            {
                set.Text = Translations.getTranslatedString(set.Name);
            }
            viewTypeGB.Text = Translations.getTranslatedString("ModSelectionListViewSelection");
            if (init)
            {
                //apply all checkmarks
                this.forceManuel.Checked = Settings.ForceManuel;
                this.cleanInstallCB.Checked = Settings.CleanInstallation;
                this.backupModsCheckBox.Checked = Settings.BackupModFolder;
                this.cancerFontCB.Checked = Settings.ComicSans;
                this.saveLastInstallCB.Checked = Settings.SaveLastConfig;
                this.saveUserDataCB.Checked = Settings.SaveUserData;
                this.darkUICB.Checked = Settings.DarkUI;
                this.expandNodesDefault.Checked = Settings.ExpandAllLegacy;
                this.disableBordersCB.Checked = Settings.DisableBorders;
                this.clearCacheCB.Checked = Settings.ClearCache;
                this.disableColorsCB.Checked = Settings.DisableColorChange;
                this.clearLogFilesCB.Checked = Settings.DeleteLogs;
                this.Font = Settings.AppFont;
                this.notifyIfSameDatabaseCB.Checked = Settings.NotifyIfSameDatabase;
                this.ShowInstallCompleteWindowCB.Checked = Settings.ShowInstallCompleteWindow;
                this.createShortcutsCB.Checked = Settings.CreateShortcuts;
                this.InstantExtractionCB.Checked = Settings.InstantExtraction;
                this.SuperExtractionCB.Checked = Settings.SuperExtraction;
                switch (Settings.UninstallMode)
                {
                    case (Settings.UninstallModes.Smart):
                        SmartUninstallModeRB.Checked = true;
                        break;
                    case (Settings.UninstallModes.Clean):
                        CleanUninstallModeRB.Checked = true;
                        break;
                }
                switch (Settings.GIF)
                {
                    case (Settings.LoadingGifs.Standard):
                        {
                            standardImageRB.Checked = true;
                            break;
                        }
                    case (Settings.LoadingGifs.ThirdGuards):
                        {
                            thirdGuardsLoadingImageRB.Checked = true;
                            break;
                        }
                }
                LanguageComboBox.SelectedIndexChanged -= LanguageComboBox_SelectedIndexChanged;
                switch (Translations.language)
                {
                    //english = 0, polish = 1, german = 2, french = 3
                    case (Translations.Languages.English):
                        LanguageComboBox.SelectedIndex = 0;
                        break;
                    case (Translations.Languages.German):
                        LanguageComboBox.SelectedIndex = 2;
                        break;
                    case (Translations.Languages.Polish):
                        LanguageComboBox.SelectedIndex = 1;
                        break;
                    case (Translations.Languages.French):
                        LanguageComboBox.SelectedIndex = 3;
                        break;
                }
                LanguageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
                switch (Settings.SView)
                {
                    case (Settings.SelectionView.Default):
                        //set default button, but disable checkedChanged handler to prevent stack overflow
                        selectionDefault.Checked = true;
                        break;
                    case (Settings.SelectionView.Legacy):
                        selectionLegacy.Checked = true;
                        break;
                }
                switch (Settings.FontSizeforum)
                {
                    case (Settings.FontSize.Font100):
                        fontSize100.Checked = true;
                        break;
                    case (Settings.FontSize.Font125):
                        fontSize125.Checked = true;
                        break;
                    case (Settings.FontSize.Font175):
                        fontSize175.Checked = true;
                        break;
                    case (Settings.FontSize.Font225):
                        fontSize225.Checked = true;
                        break;
                    case (Settings.FontSize.Font275):
                        fontSize275.Checked = true;
                        break;
                    case (Settings.FontSize.DPI100):
                        DPI100.Checked = true;
                        break;
                    case (Settings.FontSize.DPI125):
                        DPI125.Checked = true;
                        break;
                    case (Settings.FontSize.DPI175):
                        DPI175.Checked = true;
                        break;
                    case (Settings.FontSize.DPI225):
                        DPI225.Checked = true;
                        break;
                    case (Settings.FontSize.DPI275):
                        DPI275.Checked = true;
                        break;
                    case (Settings.FontSize.DPIAUTO):
                        DPIAUTO.Checked = true;
                        break;
                }
            }
        }

        //for when downloads are started, a timer to keep track of the download speed and ETA
        private void DownloadTimer_Tick(object sender, EventArgs e)
        {
            differenceTotalBytesDownloaded = currentTotalBytesDownloaded - previousTotalBytesDownloaded;
            float intervalInSeconds = (float)DownloadTimer.Interval / 1000;
            float sessionMBytesDownloaded = differenceTotalBytesDownloaded / MBDivisor;
            sessionDownloadSpeed = sessionMBytesDownloaded / intervalInSeconds;
            //set the previous for the last amount of bytes downloaded
            previousTotalBytesDownloaded = currentTotalBytesDownloaded;
        }
        //toggle UI buttons to be Enabled or disabled
        public void ToggleUIButtons(bool enableToggle)
        {
            forceManuel.Enabled = enableToggle;
            installRelhaxMod.Enabled = enableToggle;
            uninstallRelhaxMod.Enabled = enableToggle;
            cleanInstallCB.Enabled = enableToggle;
            cancerFontCB.Enabled = enableToggle;
            backupModsCheckBox.Enabled = enableToggle;
            darkUICB.Enabled = enableToggle;
            saveUserDataCB.Enabled = enableToggle;
            saveLastInstallCB.Enabled = enableToggle;
            ToggleScaleRBs(enableToggle);
            DPIAUTO.Enabled = enableToggle;
            clearCacheCB.Enabled = enableToggle;
            clearLogFilesCB.Enabled = enableToggle;
            notifyIfSameDatabaseCB.Enabled = enableToggle;
            ShowInstallCompleteWindowCB.Enabled = enableToggle;
            createShortcutsCB.Enabled = enableToggle;
            InstantExtractionCB.Enabled = enableToggle;
            SuperExtractionCB.Enabled = enableToggle;
            SmartUninstallModeRB.Enabled = enableToggle;
            CleanUninstallModeRB.Enabled = enableToggle;
        }

        public void ToggleScaleRBs(bool enableToggle)
        {
            float[] scales = new float[] { Settings.Scale100, Settings.Scale125, Settings.Scale175, Settings.Scale225, Settings.Scale275 };
            RadioButton[,] radioButtons = new RadioButton[,] { { fontSize100, DPI100 }, { fontSize125, DPI125 }, { fontSize175, DPI175 }, { fontSize225, DPI225 }, { fontSize275, DPI275 } };
            for (int i = 0; i < scales.Count(); i++)
            {
                float floatHeight = originalMainWindowHeight * scales[i];
                float floatWidth = originalMainWindowWidth * scales[i];
                radioButtons[i, 0].Enabled = CheckMainWindowSizeToMonitorSize((int)floatHeight, (int)floatWidth) && enableToggle;
                radioButtons[i, 1].Enabled = radioButtons[i, 0].Enabled;
            }
        }

        // https://stackoverflow.com/questions/254197/how-can-i-get-the-active-screen-dimensions
        private bool CheckMainWindowSizeToMonitorSize(int intHeight, int intWidth)              
        {
            var hwnd = this.Handle;
            var monitor = NativeMethods.MonitorFromWindow(hwnd, NativeMethods.MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new NativeMethods.NativeMonitorInfo();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);
                var width = (monitorInfo.Monitor.Right - monitorInfo.Monitor.Left);
                var height = (monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top);
                return (intHeight < height && intWidth < width);
            }
            else
            {
                return false;
            }
        }

        //handler for when the window is goingto be closed
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save settings
            if (Program.saveSettings) Settings.saveSettings();
            Logging.Manager("cleaning \"RelHaxTemp\" folder");
            Utils.DirectoryDelete(Path.Combine(Application.StartupPath, "RelHaxTemp"), true);
            Logging.Manager(string.Format("Exception counted: {0}", errorCounter));
            Logging.Manager("Application Closing");
            Logging.Manager("|------------------------------------------------------------------------------------------------|");
            Logging.Dispose();
        }

        #region LinkClicked Events
        private void donateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=76KNV8KXKYNG2");
        }

        private void findBugAddModLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/1LmPCMAx0RajW4lVYAnguHjjd8jArtWuZIGciFN76AI4/edit?usp=sharing");
        }

        private void DiscordServerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/58fdPvK");
        }
        //when the "visit form page" link is clicked. the link clicked handler
        private void formPageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.worldoftanks.com/index.php?/topic/535868-");
        }
        #endregion

        #region Loading animations handlers
        //handler for when the "standard" loading animation is clicked
        private void standardImageRB_CheckedChanged(object sender, EventArgs e)
        {
            if (standardImageRB.Checked)
            {
                Settings.GIF = Settings.LoadingGifs.Standard;
            }
            else if (thirdGuardsLoadingImageRB.Checked)
            {
                Settings.GIF = Settings.LoadingGifs.ThirdGuards;
            }
        }

        private void standardImageRB_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;
            RadioButton rb = (RadioButton)sender;
            Settings.LoadingGifs backup = Settings.GIF;
            if (rb.Name.Equals("standardImageRB"))
            {
                Settings.GIF = Settings.LoadingGifs.Standard;
            }
            else if (rb.Name.Equals("thirdGuardsLoadingImageRB"))
            {
                Settings.GIF = Settings.LoadingGifs.ThirdGuards;
            }
            else
                return;
            //create the preview
            gp.Hide();
            gp.SetLoadingImage();
            gp.Show();
            GC.Collect();
        }
        #endregion

        #region MouseEnter/MouseLeave events
        private void generic_MouseLeave(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled && Settings.FirstLoad)
                downloadProgress.Text = Translations.getTranslatedString("helperText");
            else
                downloadProgress.Text = "";
        }

        private void forceManuel_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("forceManuelDescription");
        }

        private void cleanInstallCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("cleanInstallDescription");
        }

        private void backupModsCheckBox_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("backupModsDescription");
        }

        private void cancerFontCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("comicSansDescription");
        }

        private void largerFontButton_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("enlargeFontDescription");
        }

        private void standardImageRB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("selectGifDesc");
        }

        private void saveLastInstallCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("saveLastConfigInstall");
        }

        private void font_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("font_MouseEnter");
        }

        private void selectionView_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("selectionView_MouseEnter");
        }

        private void expandNodesDefault_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("expandAllDesc");
        }

        private void language_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("language_MouseEnter");
        }

        private void disableBordersCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("disableBordersDesc");

        }
        private void clearCacheCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("clearCachCBExplanation");
        }

        private void saveUserDataCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("saveUserDataDesc");
        }

        private void cleanUninstallCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("cleanUninstallDescription");
        }

        private void disableColorsCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("disableColorsCBExplanation");
        }

        private void clearLogFilesCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("clearLogFilesDescription");
        }

        private void darkUICB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("darkUIDesc");
        }

        private void notifyIfSameDatabaseCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("notifyIfSameDatabaseCBExplanation");
        }

        private void ShowInstallCompleteWindowCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("ShowInstallCompleteWindowCBExplanation");
        }

        private void CreateShortcutsCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("CreateShortcutsCBExplanation");
        }

        private void InstantExtractionCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("InstantExtractionCBExplanation");
        }

        private void SuperExtractionCB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("SuperExtractionCBExplanation");
        }

        private void SmartUninstallModeRB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("SmartUninstallModeRBExplanation");
        }

        private void CleanUninstallModeRB_MouseEnter(object sender, EventArgs e)
        {
            if (installRelhaxMod.Enabled)
                downloadProgress.Text = Translations.getTranslatedString("CleanUninstallModeRBExplanation");
        }
        #endregion

        #region CheckChanged/SelectedIndexChanged events
        //handler for selection the new language from the combobox
        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LanguageComboBox.SelectedIndex)
            {
                case 0://english
                    Translations.language = Translations.Languages.English;
                    break;
                case 1://polish
                    Translations.language = Translations.Languages.Polish;
                    break;
                case 2://german
                    Translations.language = Translations.Languages.German;
                    break;
                case 3://french
                    Translations.language = Translations.Languages.French;
                    break;
            }
            this.ApplySettings();
        }
        //handler for what happends when the check box "clean install" is checked or not
        private void cleanInstallCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.CleanInstallation = cleanInstallCB.Checked;
        }
        //enalbes the user to use "comic sans" font for the 1 person that would ever want to do that
        private void cancerFontCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ComicSans = cancerFontCB.Checked;
            Settings.ApplInternalProperties();
            this.Font = Settings.AppFont;
        }
        //handler for when the "force manuel" checkbox is checked
        private void forceManuel_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ForceManuel = forceManuel.Checked;
        }
        //handler for when the "backupResMods mods" checkbox is changed
        private void backupModsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.BackupModFolder = backupModsCheckBox.Checked;
        }

        private void saveLastInstallCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SaveLastConfig = saveLastInstallCB.Checked;
        }

        private void saveUserDataCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SaveUserData = saveUserDataCB.Checked;
        }

        private void darkUICB_CheckedChanged(object sender, EventArgs e)
        {
            //set the thing
            Settings.DarkUI = darkUICB.Checked;
            Settings.setUIColor(this);
        }

        private void languageENG_CheckedChanged(object sender, EventArgs e)
        {
            Translations.language = Translations.Languages.English;
            this.ApplySettings();
        }

        private void languageGER_CheckedChanged(object sender, EventArgs e)
        {
            Translations.language = Translations.Languages.German;
            this.ApplySettings();
        }

        private void selectionDefault_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SView = Settings.SelectionView.Default;
            this.ApplySettings();
        }

        private void selectionLegacy_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SView = Settings.SelectionView.Legacy;
            this.ApplySettings();
        }

        private void languagePL_CheckedChanged(object sender, EventArgs e)
        {
            Translations.language = Translations.Languages.Polish;
            this.ApplySettings();
        }

        private void languageFR_CheckedChanged(object sender, EventArgs e)
        {
            Translations.language = Translations.Languages.French;
            this.ApplySettings();
        }

        private void expandNodesDefault_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ExpandAllLegacy = expandNodesDefault.Checked;
        }

        private void disableBordersCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.DisableBorders = disableBordersCB.Checked;
        }

        private void clearCacheCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ClearCache = clearCacheCB.Checked;
        }

        private void fontSize100_CheckedChanged(object sender, EventArgs e)
        {
            if (fontSize100.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Dpi)
                {
                    Settings.FontSizeforum = Settings.FontSize.DPI100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    float temp = 1.0f / scale;
                    this.Scale(new SizeF(temp, temp));
                    scale = 1.0f;
                    this.Font = Settings.AppFont;
                }
                //change settings enum
                Settings.FontSizeforum = Settings.FontSize.Font100;
                //apply change of settings enum
                Settings.ApplInternalProperties();
                //get new scalingMode (or no change, get it anyway)
                this.AutoScaleMode = Settings.AppScalingMode;
                //get new font
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void fontSize125_CheckedChanged(object sender, EventArgs e)
        {
            if (fontSize125.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Dpi)
                {
                    Settings.FontSizeforum = Settings.FontSize.DPI100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    float temp = 1.0f / scale;
                    this.Scale(new SizeF(temp, temp));
                    scale = 1.0f;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.Font125;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void fontSize175_CheckedChanged(object sender, EventArgs e)
        {
            if (fontSize175.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Dpi)
                {
                    Settings.FontSizeforum = Settings.FontSize.DPI100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    float temp = 1.0f / scale;
                    this.Scale(new SizeF(temp, temp));
                    scale = 1.0f;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.Font175;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void fontSize225_CheckedChanged(object sender, EventArgs e)
        {
            if (fontSize225.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Dpi)
                {
                    Settings.FontSizeforum = Settings.FontSize.DPI100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    float temp = 1.0f / scale;
                    this.Scale(new SizeF(temp, temp));
                    scale = 1.0f;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.Font225;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void fontSize275_CheckedChanged(object sender, EventArgs e)
        {
            if (fontSize275.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Dpi)
                {
                    Settings.FontSizeforum = Settings.FontSize.DPI100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    float temp = 1.0f / scale;
                    this.Scale(new SizeF(temp, temp));
                    scale = 1.0f;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.Font275;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPI100_CheckedChanged(object sender, EventArgs e)
        {
            if (DPI100.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPI100;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.Scale100 / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.Scale100;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPI125_CheckedChanged(object sender, EventArgs e)
        {
            if (DPI125.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPI125;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.Scale125 / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.Scale125;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPI175_CheckedChanged(object sender, EventArgs e)
        {
            if (DPI175.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPI175;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.Scale175 / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.Scale175;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPI225_CheckedChanged(object sender, EventArgs e)
        {
            if (DPI225.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPI225;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.Scale225 / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.Scale225;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPI275_CheckedChanged(object sender, EventArgs e)
        {
            if (DPI275.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPI275;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.Scale275 / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.Scale275;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void DPIAUTO_CheckedChanged(object sender, EventArgs e)
        {
            if (DPIAUTO.Checked)
            {
                if (this.AutoScaleMode == System.Windows.Forms.AutoScaleMode.Font)
                {
                    Settings.FontSizeforum = Settings.FontSize.Font100;
                    Settings.ApplInternalProperties();
                    this.AutoScaleMode = Settings.AppScalingMode;
                    this.Font = Settings.AppFont;
                }
                Settings.FontSizeforum = Settings.FontSize.DPIAUTO;
                Settings.ApplInternalProperties();
                this.AutoScaleMode = Settings.AppScalingMode;
                float temp = Settings.ScaleSize / scale;
                this.Scale(new SizeF(temp, temp));
                scale = Settings.ScaleSize;
                this.Font = Settings.AppFont;
                ToggleScaleRBs(true);
            }
        }

        private void clearLogFilesCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.DeleteLogs = clearLogFilesCB.Checked;
        }

        private void disableColorsCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.DisableColorChange = disableColorsCB.Checked;
        }

        private void notifyIfSameDatabaseCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.NotifyIfSameDatabase = notifyIfSameDatabaseCB.Checked;
        }

        private void ShowInstallCompleteWindow_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShowInstallCompleteWindow = ShowInstallCompleteWindowCB.Checked;
        }

        private void CreateShortcutsCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.CreateShortcuts = createShortcutsCB.Checked;
        }

        private void InstantExtractionCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.InstantExtraction = InstantExtractionCB.Checked;
        }

        private void SmartUninstallModeRB_CheckedChanged(object sender, EventArgs e)
        {
            if (SmartUninstallModeRB.Checked)
                Settings.UninstallMode = Settings.UninstallModes.Smart;
        }

        private void SuperExtractionCB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SuperExtraction = SuperExtractionCB.Checked;
        }

        private void CleanUninstallModeRB_CheckedChanged(object sender, EventArgs e)
        {
            if (CleanUninstallModeRB.Checked)
                Settings.UninstallMode = Settings.UninstallModes.Clean;
        }
        #endregion

        #region Click events

        private void cancelDownloadButton_Click(object sender, EventArgs e)
        {
            Downloader.CancelAsync();
        }

        private void viewAppUpdates_Click(object sender, EventArgs e)
        {
            int xloc = this.Location.X + this.Size.Width + 10;
            int yloc = this.Location.Y;
            using (ViewUpdates vu = new ViewUpdates(xloc, yloc, Settings.ManagerInfoDatFile, Program.betaApplication ? "releaseNotes_beta.txt" : "releaseNotes.txt"))
            {
                vu.ShowDialog();
            }
        }

        private void viewDBUpdates_Click(object sender, EventArgs e)
        {
            int xloc = this.Location.X + this.Size.Width + 10;
            int yloc = this.Location.Y;
            using (ViewUpdates vu = new ViewUpdates(xloc, yloc, Settings.ManagerInfoDatFile, "databaseUpdate.txt"))
            {
                vu.ShowDialog();
            }
        }

        private void DiagnosticUtilitiesButton_Click(object sender, EventArgs e)
        {
            ToggleUIButtons(false);
            //attempt to locate the tanks directory
            if (Settings.ForceManuel || this.autoFindTanks() == null)
            {
                if (this.manuallyFindTanks() == null)
                {
                    ToggleUIButtons(true);
                    return;
                }
            }
            //parse all strings
            tanksLocation = tanksLocation.Substring(0, tanksLocation.Length - 17);
            Logging.Manager(string.Format("tanksLocation parsed as {0}", tanksLocation));
            using (Diagnostics d = new Diagnostics()
            {
                TanksLocation = this.tanksLocation,
                AppStartupPath = Application.StartupPath,
                ParentWindow = this
            })
            {
                d.ShowDialog();
            }
            ToggleUIButtons(true);
        }
        #endregion
    }
}
