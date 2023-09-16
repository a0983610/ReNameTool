using System.Security.Cryptography;
using System.Text.Json;

namespace ReNameTool
{
    internal class Program
    {
        static readonly string ExeInDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static readonly string ResultPath = Path.Combine(ExeInDirectory, "Result");
        static readonly string RecordedPath = Path.Combine(ResultPath, "ReNameRecordedData");
        static readonly List<string> Extensions = new List<string>() { "JPG", "PNG" };
        private static Dictionary<string, string>? RecordedDatas { get; set; }

        static void Main(string[] args)
        {
            try
            {
                int i = 1;

                Console.WriteLine($"建立輸出結果資料夾");
                if (Directory.Exists(ResultPath))
                {
                    var results = Directory.GetFiles(ResultPath).ToList();
                    results = results.Select(i => Path.GetFileNameWithoutExtension(i)).ToList();

                    var result = results.Max((it) =>
                    {
                        int i = 0;
                        int.TryParse(it, out i);
                        return i;
                    });

                    i = result + 1;

                    if (File.Exists(RecordedPath))
                    {
                        string jsonString = File.ReadAllText(RecordedPath);
                        if (jsonString != null)
                        {
                            RecordedDatas = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                        }
                    }

                }
                else
                {
                    Directory.CreateDirectory(ResultPath);
                }

                if (RecordedDatas == null) RecordedDatas = new Dictionary<string, string>();

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

                                    if (fileInfo != null && fileInfo.DirectoryName != null)
                                    {
                                        string hashString = GetHashString(sha1, filePath);

                                        if (RecordedDatas.ContainsKey(fileInfo.Name))
                                        {
                                            var hashStringRecorded = RecordedDatas[fileInfo.Name];

                                            if (hashStringRecorded == hashString)
                                            {
                                                break;
                                            }
                                        }

                                        fileInfo.CopyTo(Path.Combine(ResultPath, $"{(i++).ToString().PadLeft(4, '0')}{fileInfo.Extension}"));

                                        RecordedDatas.Add(fileInfo.Name, hashString);
                                    }

                                    break;
                                }
                            }


                        }
                    }

                    string json = JsonSerializer.Serialize(RecordedDatas);
                    File.WriteAllText(RecordedPath, json);

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
    }
}