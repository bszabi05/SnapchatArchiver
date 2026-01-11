using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SnapchatArchiver
{
    public partial class MainForm : Form
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private void EnableDarkMode(IntPtr handle)
        {
            try
            {
                int darkMode = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DWM hiba: " + ex.Message);
            }
        }

        private string selectedZipPath = "";
        private string historyFile = "download_history.txt";
        private string outputPath = "Snapchat_Archivum";
        private HashSet<string> history = new HashSet<string>();
        private Dictionary<MsgKey, string> _lang = Localization.GetDictionary(Localization.Language.EN);
        public MainForm()
        {
            InitializeComponent();
            EnableDarkMode(this.Handle);
            UpdateLanguage(); 
            if (File.Exists(historyFile)){
                history = new HashSet<string>(File.ReadAllLines(historyFile));

            }
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.InitialDirectory = "c:\\";
                ofd.Filter = _lang[MsgKey.FileFilter];

                if (ofd.ShowDialog() == DialogResult.OK) { 
                    selectedZipPath = ofd.FileName;
                    txbPath.Text = selectedZipPath;
                    Log(string.Format(_lang[MsgKey.SelectedFileLog], Path.GetFileName(selectedZipPath)));
                }

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedZipPath) || !File.Exists(selectedZipPath)) {
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

                await Task.Run(() => ZipFile.ExtractToDirectory(selectedZipPath,extractPath));

                string htmlPath = Path.Combine(extractPath, "html","memories_history.html");
                if (!File.Exists(htmlPath)) {
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

                foreach (var row in rows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols == null || cols.Count < 4) continue;

                    string rawDate = cols[0].InnerText.Trim();
                    string mediaType = cols[1].InnerText.Trim().ToLower();

                    var linkNode = cols[3].SelectSingleNode(".//a[contains(@onclick, 'downloadMemories')]");

                    if (linkNode == null)
                    {
                        linkNode = row.SelectSingleNode(".//a[contains(@onclick, 'downloadMemories')]");
                    }

                    if (linkNode != null)
                    {
                        string onclick = linkNode.GetAttributeValue("onclick", "");
                        var match = Regex.Match(onclick, @"downloadMemories\('([^']+)'");

                        if (match.Success)
                        {
                            string url = match.Groups[1].Value;

                            if (history.Contains(url)) continue;

                            string dateSafe = Sanitize(rawDate);
                            string baseName = $"{dateSafe}_{mediaType}";

                            try
                            {
                                Log(string.Format(_lang[MsgKey.Downloading], baseName));

                                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                                if (!response.IsSuccessStatusCode)
                                {
                                    Log(string.Format(_lang[MsgKey.ServerError], response.StatusCode, baseName));
                                    continue;
                                }

                                var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                                byte[] data = await response.Content.ReadAsByteArrayAsync();

                                string tempFile = Path.Combine(outputPath, "download.tmp");
                                File.WriteAllBytes(tempFile, data);

                                if (contentType.Contains("zip") || url.Contains(".zip"))
                                {
                                    HandleZipMedia(tempFile, baseName);
                                }
                                else
                                {
                                    string ext = (mediaType.Contains("video") || contentType.Contains("video")) ? ".mp4" : ".jpg";
                                    string finalPath = Path.Combine(outputPath, baseName + ext);

                                    if (File.Exists(finalPath)) File.Delete(finalPath);
                                    File.Move(tempFile, finalPath);
                                }

                                File.AppendAllLines(historyFile, new[] { url });
                                history.Add(url);
                                Log(string.Format(_lang[MsgKey.SuccessMemory], baseName));
                            }
                            catch (Exception ex)
                            {
                                Log(string.Format(_lang[MsgKey.DownloadError], baseName, ex.Message));
                            }
                        }
                    }
                    
                }

                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                Log(_lang[MsgKey.ProcessFinished]);
                MessageBox.Show(_lang[MsgKey.ProcessFinished], _lang[MsgKey.MessageBoxSuccessTitle], MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                Log(string.Format(_lang[MsgKey.CriticalError], ex.Message));
            }
            finally
            {
                btnBrowse.Enabled = true;
                btnStart.Enabled = true;
            }
        }

        private void HandleZipMedia(string zipFile, string baseName)
        {
            string subExtract = Path.Combine(outputPath, "temp_sub");
            if (Directory.Exists(subExtract))
            {
                Directory.Delete(subExtract, true);
            }
            ZipFile.ExtractToDirectory(zipFile, subExtract);

            foreach (var file in Directory.GetFiles(subExtract, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(file).ToLower();
                string finalName = (ext == ".png") ? $"{baseName}_overlay.png" : $"{baseName}{ext}";

                string targetPath = Path.Combine(outputPath, finalName);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
                File.Move(file, targetPath);
            }
            Directory.Delete(subExtract, true);
            File.Delete(zipFile);
        }

        private void Log(string text)
        {
            if (lsbLog.InvokeRequired)
            {
                lsbLog.Invoke(new Action(() => Log(text)));
            }
            else
            {
                lsbLog.Items.Insert(0,$"{DateTime.Now:HH:mm:ss} - {text}");
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
