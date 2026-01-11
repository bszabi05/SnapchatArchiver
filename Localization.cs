using System;
using System.Collections.Generic;

namespace SnapchatArchiver
{
    public enum MsgKey
    {
        BrowseBtn,
        StartBtn,
        ZipPathLabel,
        SelectZipError,
        Extracting,
        InvalidZipError,
        FoundCount,
        Downloading,
        ServerError,
        DownloadError,
        SuccessMemory,
        ProcessFinished,
        MessageBoxSuccessTitle,
        MessageBoxErrorTitle,
        CriticalError,
        SelectedFileLog,
        FileFilter
    }

    public static class Localization
    {
        public enum Language { HU, EN }

        private static readonly Dictionary<MsgKey, string> Hungarian = new Dictionary<MsgKey, string>
        {
            { MsgKey.BrowseBtn, "TALLÓZÁS" },
            { MsgKey.StartBtn, "ARCHIVÁLÁS INDÍTÁSA" },
            { MsgKey.ZipPathLabel, "Válassz ki egy ZIP fájlt..." },
            { MsgKey.SelectZipError, "Kérlek, előbb válaszd ki a Snapchat adatexport ZIP fájlt!" },
            { MsgKey.Extracting, "Zip kicsomagolása..." },
            { MsgKey.InvalidZipError, "Hiba: Érvénytelen ZIP fájl!" },
            { MsgKey.FoundCount, "{0} elem feldolgozása..." },
            { MsgKey.Downloading, "Letöltés: {0}" },
            { MsgKey.ServerError, "Szerver hiba: {0} - {1}" },
            { MsgKey.DownloadError, "Hiba ({0}): {1}" },
            { MsgKey.SuccessMemory, "Siker: {0}" },
            { MsgKey.ProcessFinished, "KÉSZ! Minden fájl mentve!" },
            { MsgKey.MessageBoxSuccessTitle, "Siker" },
            { MsgKey.MessageBoxErrorTitle, "Hiba" },
            { MsgKey.CriticalError, "Hiba történt! {0}" },
            { MsgKey.SelectedFileLog, "Kiválasztott fájl: {0}" },
            { MsgKey.FileFilter, "ZIP fájlok (*.zip)|*.zip|Minden fájl (*.*)|*.*" }
        };

        private static readonly Dictionary<MsgKey, string> English = new Dictionary<MsgKey, string>
        {
            { MsgKey.BrowseBtn, "BROWSE" },
            { MsgKey.StartBtn, "START ARCHIVING" },
            { MsgKey.ZipPathLabel, "Select a ZIP file..." },
            { MsgKey.SelectZipError, "Please select the Snapchat export ZIP file first!" },
            { MsgKey.Extracting, "Extracting Zip..." },
            { MsgKey.InvalidZipError, "Error: Invalid ZIP file!" },
            { MsgKey.FoundCount, "Processing {0} items..." },
            { MsgKey.Downloading, "Downloading: {0}" },
            { MsgKey.ServerError, "Server error: {0} - {1}" },
            { MsgKey.DownloadError, "Error ({0}): {1}" },
            { MsgKey.SuccessMemory, "Success: {0}" },
            { MsgKey.ProcessFinished, "DONE! All files saved!" },
            { MsgKey.MessageBoxSuccessTitle, "Success" },
            { MsgKey.MessageBoxErrorTitle, "Error" },
            { MsgKey.CriticalError, "An error occurred! " },
            { MsgKey.SelectedFileLog, "Selected file: {0}" },
            { MsgKey.FileFilter, "ZIP files (*.zip)|*.zip|All files (*.*)|*.*" }
        };

        public static Dictionary<MsgKey, string> GetDictionary(Language lang)
        {
            return lang == Language.HU ? Hungarian : English;
        }
    }
}