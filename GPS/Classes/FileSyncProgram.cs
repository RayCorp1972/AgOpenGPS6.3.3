using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.IO;
using System.Net.Http;

namespace AgOpenGPS.Classes
{
    class FileSyncProgram
    {
      
        private readonly FormGPS mf;

        private static string localDirectory = @"C:\AgOpenGPS\Fields\";  // Local directory on tablet
        private static string serverUrl = "http://85.215.198.173/";             // XAMPP server URL
        private static string serverDirectory = "AOGTestFiles/AgOpenGPS/";                     // Server directory (relative to XAMPP)

        public FileSyncProgram(FormGPS _f)
        {
          mf= _f;
            
        }
        static async Task Main(string[] args)
        {
            // Monitor the local directory for changes
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = localDirectory,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };

            // Event handler for file changes
            watcher.Changed += (sender, e) =>
            {
                Console.WriteLine($"File {e.Name} has been changed locally. Uploading to server...");
                UploadFile(e.FullPath);
            };

            // Start monitoring the directory
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Starting file synchronization. Press any key to exit.");

            // Continuous synchronization loop (downloads changes from server)
            while (true)
            {
                await SyncWithServer();
                await Task.Delay(10000);  // Sync every 10 seconds (adjust as needed)
            }
        }

        // Upload a local file to the server
        public static void UploadFile(string filePath)
        {
           
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                        content.Add(fileContent, "file", Path.GetFileName(filePath));

                        // Send the file to the server
                        HttpResponseMessage response = client.PostAsync($"{serverUrl}upload.php", content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"File {Path.GetFileName(filePath)} uploaded successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Error uploading file to server.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
            }
        }

        // Sync with the server (download new/modified files)
        public static async Task SyncWithServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{serverUrl}list_files.php");

                    if (response.IsSuccessStatusCode)
                    {
                        // Assume server returns a list of file names and their modification times
                        string[] serverFiles = (await response.Content.ReadAsStringAsync()).Split('\n');

                        foreach (var fileInfo in serverFiles)
                        {
                            string[] fileData = fileInfo.Split('|');
                            string fileName = fileData[0];
                            DateTime serverModifiedTime = DateTime.Parse(fileData[1]);

                            string localFilePath = Path.Combine(localDirectory, fileName);

                            if (!File.Exists(localFilePath) || File.GetLastWriteTime(localFilePath) < serverModifiedTime)
                            {
                                // Download the file from the server
                                Console.WriteLine($"Downloading {fileName} from server...");
                                await DownloadFileFromServer(fileName, localFilePath);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error fetching file list from server.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing with server: {ex.Message}");
            }
        }

        // Download file from the server
        public static async Task DownloadFileFromServer(string fileName, string localFilePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{serverUrl}{serverDirectory}/{fileName}");

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] fileData = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(localFilePath, fileData);
                        Console.WriteLine($"Downloaded {fileName} successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download {fileName} from server.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }

    }
}
