using System.Security.Cryptography;
using System.Text.Json;

namespace ReNameTool
{
    internal class Program
    {
        static readonly string ExeInDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static readonly string ResultPath = Path.Combine(ExeInDirectory, "Result");
        static readonly List<string> Extensions = new List<string>() { "JPG", "JPEG", "PNG", "GIF", "SVG" };

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"讀取輸出結果資料夾");
                if (Directory.Exists(ResultPath))
                {
                    
                }
                else
                {
                    Directory.CreateDirectory(ResultPath);
                }

                GetRecordedData();

                if (Directory.Exists(ExeInDirectory))
                {
                    Console.WriteLine($"{ExeInDirectory}中的圖檔列表：");

                    var filePaths = Directory.GetFiles(ExeInDirectory).ToList();

                    using (SHA1 sha1 = SHA1.Create())
                    {
                        foreach (string filePath in filePaths)
                        {
                            foreach (var fileSuffix in Extensions)
                            {
                                if (filePath.EndsWith($".{fileSuffix}") || filePath.EndsWith($".{fileSuffix.ToLower()}"))
                                {
                                    Console.WriteLine(Path.GetFileName(filePath));

                                    FileInfo fileInfo = new FileInfo(filePath);

                                    if (fileInfo != null)
                                    {
                                        string hashString = GetHashString(sha1, filePath);

                                        if (CheckRecordedData(hashString)) break;

                                        fileInfo.CopyTo(Path.Combine(ResultPath, $"{(RecordedData.MaxFileNunber++).ToString().PadLeft(4, '0')}{fileInfo.Extension}"));

                                        AddRecordedData(hashString);
                                    }

                                    break;
                                }
                            }


                        }
                    }

                    SetRecordedData();

                }
                else
                {
                    Console.WriteLine($"資料夾 {ExeInDirectory} 不存在。");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
                Console.ReadKey();
            }
        }

        private static string GetHashString(SHA1 sha1, string filePath)
        {
            var hashBytes = sha1.ComputeHash(File.ReadAllBytes(filePath));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

            return hashString;
        }


        static readonly string RecordedPath = Path.Combine(ResultPath, "ReNameRecordedData");
        static RecordedDataModel RecordedData { get; set; }

        [Serializable]
        class RecordedDataModel
        {
            public int MaxFileNunber {  get; set; }
            public HashSet<string>? HashData { get; set; }
        }

        private static void GetRecordedData()
        {
            if (File.Exists(RecordedPath))
            {
                string jsonString = File.ReadAllText(RecordedPath);
                if (jsonString != null)
                {
                    RecordedData = JsonSerializer.Deserialize<RecordedDataModel>(jsonString);
                }
            }
            else
            {
                if (RecordedData == null)
                {
                    RecordedData = new RecordedDataModel()
                    {
                        MaxFileNunber = 1,
                        HashData = new HashSet<string>()
                    };
                }
            }
        }

        private static void SetRecordedData()
        {
            string json = JsonSerializer.Serialize(RecordedData);
            File.WriteAllText(RecordedPath, json);
        }

        private static void AddRecordedData(string data)
        {
            if (RecordedData == null) GetRecordedData();
            RecordedData.HashData.Add(data);
        }

        private static bool CheckRecordedData(string data)
        {
            if (RecordedData == null) GetRecordedData();
            return RecordedData.HashData.Contains(data);
        }
    }
}