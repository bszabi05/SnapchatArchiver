# 👻 Snapchat Archiver 

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows](https://img.shields.io/badge/Platform-Windows-blue.svg)](https://www.microsoft.com/windows)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2%2B-512BD4.svg)](https://dotnet.microsoft.com/download/dotnet-framework)

**Snapchat Archiver** is a high-performance, open-source desktop utility designed to help users preserve their digital memories. It automates the process of downloading images and videos from an official Snapchat data export.

---

## ✨ Features

* **Mass Download:** Automatically scans your export and fetches all media content.
* **Smart File Organization:** Saves files with descriptive names based on timestamps (e.g., `2025-08-13_image.jpg`).
* **Snapchat Dark UI:** A sleek, user-friendly interface inspired by the Snapchat aesthetic.
* **Multilingual Support:** Fully localized in **English** and **Hungarian**.
* **Incremental Backups:** Tracks download history to skip already archived files, saving time and bandwidth.

---

## 📖 Step-by-Step Guide

### 1. Request Your Data from Snapchat
1.  Go directly to the **[Snapchat Download My Data](https://accounts.snapchat.com/v2/download-my-data)** portal.
2.  Log in with your credentials.
3.  Scroll down to the **"Export Your Memories"** section.
4.  Select a date range (or leave "All time" for a full backup).
5.  Submit the request. 
6.  Wait for the confirmation email from Snapchat. Once it arrives, download the **ZIP** file from the same portal.

### 2. Using the Archiver
1.  Go to the [Releases](https://github.com/bszabi05/SnapchatArchiver/releases) page and download the latest `SnapchatArchiver_v1.0.zip`.
2.  Extract the ZIP file to any folder on your PC.
3.  Launch `SnapchatArchiver.exe`.
4.  Click the **BROWSE** button and select the ZIP file you received from Snapchat.
5.  Click **START ARCHIVING** and watch the magic happen!

### 📂 Where are my files saved?
All your media content will be saved automatically into a folder named **`Snapchat_Archivum`**. 
* This folder is created **directly next to the application** (`SnapchatArchiver.exe`).
* Inside, files are organized and named by date (e.g., `2025-08-13_image.jpg`).
* The application also creates a `download_history.txt` file to keep track of your progress and prevent duplicates.

---

## 🛠️ Technical Details

* **Framework:** .NET Framework 4.7.2 (WinForms)
* **Dependencies:** Uses [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/) for efficient HTML parsing.
* **Custom UI:** Implements `DwmSetWindowAttribute` for a native Windows Dark Mode title bar.

---

## ⚠️ Important Disclaimer

* **Link Expiration:** Snapchat media links provided in the HTML export are temporary. If the tool reports "403 Forbidden" or "Expired" errors, you must request a **new** data export from Snapchat.
* **Terms of Service:** This tool is for personal use only. The developer is not responsible for any misuse or violation of Snapchat's Terms of Service.
* **Third-Party:** This project is **not** affiliated with, sponsored by, or endorsed by Snap Inc.

---

## ⚖️ License

Distributed under the **MIT License**. See the `LICENSE.txt` file for the full legal text.

---
**Developed with ❤️ by bszabi05 - 2026**
