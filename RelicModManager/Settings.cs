﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Reflection;
using Microsoft.Win32;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Drawing.Text;

namespace RelicModManager
{
    public static class Settings
    {
        public static string fontName { get; set; }
        public static float fontSize { get; set; }
        public static bool backupModFolder { get; set; }
        public static bool cleanInstallation { get; set; }
        public static int loadingGif { get; set; }
        public static bool forceManuel { get; set; }
        public static bool largeFont { get; set; }
        public static bool comicSans { get; set; }
        public enum LoadingGifs { standard=0, thirdGuards=1 };
        public static LoadingGifs gif;
        public const float normalSizeFont = 8.25F;
        public const float largeSizeFont = 10.0F;
        public const string defaultFontType = "Microsoft Sance Serif";
        public const string comicSansFontType = "Comic Sans MS";
        public static string settingsXmlFile = Application.StartupPath + "\\RelHaxSettings.xml";
        //loads settings from xml file
        public static void loadSettings()
        {
            Settings.appendToLog("Loading application settings");
            if (!File.Exists(settingsXmlFile))
            {
                Settings.appendToLog("WARNING:Settings xml not found, loading defaults");
                //could also use this to determine if first load or not
                Settings.comicSans = false;
                Settings.largeFont = false;
                Settings.backupModFolder = false;
                Settings.cleanInstallation = true;
                Settings.loadingGif = (int)LoadingGifs.standard;
                Settings.forceManuel = false;
                Settings.gif = Settings.LoadingGifs.standard;
                Settings.applyInternalSettings();
            }
            else
            {
                Settings.appendToLog("Loading xml file");
                XmlDocument doc = new XmlDocument();
                doc.Load(settingsXmlFile);
                XmlNodeList settingsList = doc.ChildNodes[0].ChildNodes;
                foreach (XmlNode n in settingsList)
                {
                    switch (n.Name)
                    {
                        case "comicSans":
                            Settings.comicSans = bool.Parse(n.InnerText);
                            break;
                        case "largeFont":
                            Settings.largeFont = bool.Parse(n.InnerText);
                            break;
                        case "backupModFolder":
                            Settings.backupModFolder = bool.Parse(n.InnerText);
                            break;
                        case "cleanInstallation":
                            Settings.cleanInstallation = bool.Parse(n.InnerText);
                            break;
                        case "loadingGif":
                            Settings.loadingGif = int.Parse(n.InnerText);
                            break;
                        case "forceManuel":
                            Settings.forceManuel = bool.Parse(n.InnerText);
                            break;
                    }
                }
            }
            Settings.applyInternalSettings();
            Settings.appendToLog("Settings loaded sucessfully");
        }

        public static void applyInternalSettings()
        {
            if (Settings.largeFont)
            {
                Settings.fontSize = Settings.largeSizeFont;
            }
            else
            {
                Settings.fontSize = Settings.normalSizeFont;
            }
            if (Settings.comicSans)
            {
                Settings.fontName = Settings.comicSansFontType;
            }
            else
            {
                Settings.fontName = Settings.defaultFontType;
            }
            switch (Settings.loadingGif)
            {
                case 0:
                    Settings.gif = Settings.LoadingGifs.standard;
                    break;
                case 1:
                    Settings.gif = Settings.LoadingGifs.thirdGuards;
                    break;
            }
        }

        //saves settings to xml file
        public static void saveSettings()
        {
            Settings.appendToLog("Saving application settings");
            if (File.Exists(settingsXmlFile)) File.Delete(settingsXmlFile);
            XmlDocument doc = new XmlDocument();
            XmlElement settingsHolder = doc.CreateElement("settings");
            doc.AppendChild(settingsHolder);
            /*
             * Settings.comicSans = false;
                Settings.largeFont = false;
                Settings.backupModFolder = false;
                Settings.cleanInstallation = true;
                Settings.loadingGif = (int)LoadingGifs.standard;
                Settings.forceManuel = false;
                Settings.gif = Settings.LoadingGifs.standard;
             */
            XmlElement xcomicSans = doc.CreateElement("comicSans");
            xcomicSans.InnerText = "" + comicSans;
            settingsHolder.AppendChild(xcomicSans);

            XmlElement xlargeFont = doc.CreateElement("largeFont");
            xlargeFont.InnerText = "" + largeFont;
            settingsHolder.AppendChild(xlargeFont);

            XmlElement xbackupModFolder = doc.CreateElement("backupModFolder");
            xbackupModFolder.InnerText = "" + backupModFolder;
            settingsHolder.AppendChild(xbackupModFolder);

            XmlElement xcleanInstallation = doc.CreateElement("cleanInstallation");
            xcleanInstallation.InnerText = "" + cleanInstallation;
            settingsHolder.AppendChild(xcleanInstallation);

            switch (Settings.gif)
            {
                case (Settings.LoadingGifs.standard):
                    {
                        Settings.loadingGif = 0;
                        break;
                    }
                case (Settings.LoadingGifs.thirdGuards):
                    {
                        Settings.loadingGif = 1;
                        break;
                    }
            }

            XmlElement xloadingGif = doc.CreateElement("loadingGif");
            xloadingGif.InnerText = "" + loadingGif;
            settingsHolder.AppendChild(xloadingGif);

            XmlElement xforceManuel = doc.CreateElement("forceManuel");
            xforceManuel.InnerText = "" + forceManuel;
            settingsHolder.AppendChild(xforceManuel);

            doc.Save(settingsXmlFile);
            Settings.appendToLog("Settings saved sucessfully");
        }
        //logs string info to the log output
        private static void appendToLog(string info)
        {
            //the method should automaticly make the file if it's not there
            File.AppendAllText(Application.StartupPath + "\\RelHaxLog.txt", info + "\n");
        }

        public static Image getLoadingImage(LoadingGifs gif)
        {
            switch (gif)
            {
                case (LoadingGifs.standard):
                    {
                        return RelicModManager.Properties.Resources.loading;
                    }
                case (LoadingGifs.thirdGuards):
                    {
                        return RelicModManager.Properties.Resources.loading_3rdguards;
                    }
            }
            return null;
        }

        public static Image getLoadingImage()
        {
            switch (Settings.gif)
            {
                case (LoadingGifs.standard):
                    {
                        return RelicModManager.Properties.Resources.loading;
                    }
                case (LoadingGifs.thirdGuards):
                    {
                        return RelicModManager.Properties.Resources.loading_3rdguards;
                    }
            }
            return null;
        }

        public static Font getFont(string fontName, float fontSize)
        {
            return new System.Drawing.Font(fontName, fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
    }
}