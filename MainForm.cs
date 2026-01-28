using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapchatArchiver
{
    public partial class MainForm : Form
    {
      

        private string selectedZipPath = "";
        private string historyFile = "download_history.txt";
        private string outputPath = "Snapchat_Archivum";
        private HashSet<string> history = new HashSet<string>();
        private Dictionary<MsgKey, string> _lang = Localization.GetDictionary(Localization.Language.EN);
        public MainForm()
        {
            InitializeComponent();
            ThemeManager.ApplyDarkMode(this.Handle);
            UpdateLanguage();
            if (File.Exists(historyFile))
            {
                history = new HashSet<string>(File.ReadAllLines(historyFile));

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "c:\\";
                ofd.Filter = _lang[MsgKey.FileFilter];

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedZipPath = ofd.FileName;
                    txbPath.Text = selectedZipPath;
                    Log(string.Format(_lang[MsgKey.SelectedFileLog], Path.GetFileName(selectedZipPath)));
                }

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedZipPath) || !File.Exists(selectedZipPath))
            {
                MessageBox.Show(_lang[MsgKey.SelectZipError]);
                return;
            }

            btnBrowse.Enabled = false;
            btnStart.Enabled = false;

            try
            {
                string extractPath = Path.Combine(Path.GetTempPath(), "SnapchatArchiver_Temp");
                Directory.CreateDirectory(outputPath);
                Log(_lang[MsgKey.Extracting]);
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                await Task.Run(() => ZipHandler.ExtractMainArchive(selectedZipPath, extractPath));
                
                string htmlPath = Path.Combine(extractPath, "html", "memories_history.html");
                if (!File.Exists(htmlPath))
                {
                    Log(_lang[MsgKey.InvalidZipError]);
                    return;
                }

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(htmlPath);

                var rows = doc.DocumentNode.SelectNodes("//tr")?.Skip(1) ?? Enumerable.Empty<HtmlNode>();
                int totalCount = rows.Count();
                Log(string.Format(_lang[MsgKey.FoundCount], totalCount));

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                client.Timeout = TimeSpan.FromMinutes(5);

                var parser = new SnapchatArchiver.SnapData();

                foreach (var row in rows)
                {
                    var metadata = SnapData.ParseSnapRow(row);

                    if (metadata == null || string.IsNullOrEmpty(metadata.DownloadUrl))
                    {
                        continue;
                    }


                    if (history.Contains(metadata.DownloadUrl))
                    {
                        continue;
                    }

                    string mediaType = metadata.Type.ToLower();
                    string dateSafe = Sanitize(metadata.Date.ToString("yyyy-MM-dd HH_mm_ss"));
                    string baseName = $"{dateSafe}_{mediaType}";

                    try
                    {
                        Log(string.Format(_lang[MsgKey.Downloading], baseName));

                        var response = await client.GetAsync(metadata.DownloadUrl, HttpCompletionOption.ResponseHeadersRead); 
                        if (!response.IsSuccessStatusCode)
                        {
                            Log(string.Format(_lang[MsgKey.ServerError], response.StatusCode, baseName));
                            continue;
                        }

                        var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                        byte[] data = await response.Content.ReadAsByteArrayAsync();

                        string tempFile = Path.Combine(outputPath, "download.tmp");
                        Downloader.SaveToFile(tempFile, data);

                        string finalImagePath = "";
                        string overlayPath = "";

                        if (contentType.Contains("zip") || metadata.DownloadUrl.Contains(".zip"))
                        {
                            ZipHandler.HandleZipMedia(tempFile, baseName, outputPath);
                            finalImagePath = Path.Combine(outputPath, baseName + ".jpg");
                            overlayPath = Path.Combine(outputPath, baseName + "_overlay.png");
                        }
                        else
                        {
                            string ext = (mediaType.Contains("video") || contentType.Contains("video")) ? ".mp4" : ".jpg";
                            finalImagePath = Path.Combine(outputPath, baseName + ext);

                            if (File.Exists(finalImagePath))
                            {
                                File.Delete(finalImagePath);
                            }
                            File.Move(tempFile, finalImagePath);
                            if (ext == ".mp4")
                            {
                                SnapData.ProcessVideoFinal(finalImagePath, metadata);
                            }
                        }

                        if (finalImagePath.ToLower().EndsWith(".jpg") && File.Exists(finalImagePath))
                        {
                            SnapData.ProcessImageFinal(finalImagePath, overlayPath, metadata);

                            if (!string.IsNullOrEmpty(overlayPath) && File.Exists(overlayPath))
                            {
                                File.Delete(overlayPath);
                            }
                        }
                        if (File.Exists(tempFile)) {
                            File.Delete(tempFile);
                        }
                        File.AppendAllLines(historyFile, new[] { metadata.DownloadUrl });
                        history.Add(metadata.DownloadUrl);
                        Log(string.Format(_lang[MsgKey.SuccessMemory], baseName));
                    }
                    catch (Exception ex)
                    {
                        Log(string.Format(_lang[MsgKey.DownloadError], baseName, ex.Message));
                        continue;
                    }

                }

                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                Log(_lang[MsgKey.ProcessFinished]);
                MessageBox.Show(_lang[MsgKey.ProcessFinished], _lang[MsgKey.MessageBoxSuccessTitle], MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Log(string.Format(_lang[MsgKey.CriticalError], ex.Message));
            }
            finally
            {
                btnBrowse.Enabled = true;
                btnStart.Enabled = true;
            }
        }

       

        private void Log(string text)
        {
            if (lsbLog.InvokeRequired)
            {
                lsbLog.Invoke(new Action(() => Log(text)));
            }
            else
            {
                lsbLog.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} - {text}");
            }

        }

        private string Sanitize(string text)
        {
            string safe = Regex.Replace(text, @"[\\/*?:""<>|]", "_");
            return safe.Replace("__", "_").Trim('_');
        }
        private void UpdateLanguage()
        {
            btnBrowse.Text = _lang[MsgKey.BrowseBtn];
            btnStart.Text = _lang[MsgKey.StartBtn];
            txbPath.Text = _lang[MsgKey.ZipPathLabel];
        }

        private void btnHU_Click(object sender, EventArgs e)
        {
            _lang = Localization.GetDictionary(Localization.Language.HU);
            UpdateLanguage();

            btnHU.BackColor = Color.FromArgb(255, 252, 0);
            btnHU.ForeColor = Color.Black;
            btnEN.BackColor = Color.FromArgb(45, 45, 45);
            btnEN.ForeColor = Color.White;
        }

        private void btnEN_Click(object sender, EventArgs e)
        {
            _lang = Localization.GetDictionary(Localization.Language.EN);
            UpdateLanguage();

            btnEN.BackColor = Color.FromArgb(255, 252, 0);
            btnEN.ForeColor = Color.Black;
            btnHU.BackColor = Color.FromArgb(45, 45, 45);
            btnHU.ForeColor = Color.White;
        }


        

    }
}