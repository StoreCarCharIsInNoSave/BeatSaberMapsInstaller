using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BeatSaberMapsInstaller
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string[] archives = System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.zip");
            foreach (string archivePath in archives)
            {
              
                using (ZipArchive zip = ZipFile.OpenRead(archivePath))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (entry.FullName == "info.dat")
                        {

                            entry.ExtractToFile(Path.Combine(Environment.CurrentDirectory, entry.FullName));
                            string infoText = File.ReadAllText(Environment.CurrentDirectory + "\\info.dat");
                            File.Delete(Environment.CurrentDirectory + "\\info.dat");
                            JObject data = JObject.Parse(infoText);
                            Console.WriteLine("----------Запущена установка " + data["_songName"] + "----------");
                            try
                            {
                                Directory.CreateDirectory(@"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels\" + data["_songName"]);
                                ZipFile.ExtractToDirectory(archivePath, @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels\" + data["_songName"]);

                                Console.WriteLine("----------Установка завершена----------");
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine("Ошибка при установке " + data["_songName"]);
                                Console.WriteLine(e.Message);
                            }


                        }
                    }
                }
                File.Delete(archivePath);
                
            }

            Console.ReadKey();
        }
    }
}
