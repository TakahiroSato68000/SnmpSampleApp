using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SnmpSampleApp
{
    public class ConfigData
    {
        private static readonly Lazy<ConfigData> _instance = new Lazy<ConfigData>(() => new ConfigData());
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SnmpSampleApp");
        private static readonly string SettingsFilePath = Path.Combine(AppDataFolder, "data.xml");
        private object _lock = new object();

        public static ConfigData Instance
        {
            get
            {
                var instance = _instance.Value;
                instance.LoadData();
                return instance;
            }
        } 

        public List<string> IpAddressList { get; set; } = new List<string>();
        public List<string> CommandList { get; set; } = new List<string>();

        private ConfigData()
        {
        }

        private void SetDefaultData()
        {
            IpAddressList = new List<string> { "127.0.0.1" };
            CommandList = new List<string> { "1.3" };
        }

        public void LoadData()
        {
            lock (_lock)
            {
                if (File.Exists(SettingsFilePath))
                {
                    try
                    {
                        using (var stream = new FileStream(SettingsFilePath, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof(ConfigData));
                            var data = (ConfigData)serializer.Deserialize(stream);
                            IpAddressList = data.IpAddressList;
                            CommandList = data.CommandList;
                        }
                    }
                    catch
                    {
                        // 読み込みに失敗した場合、デフォルト値を使用
                        SetDefaultData();
                    }
                }
                else
                {
                    // 設定ファイルが存在しない場合、デフォルト値を使用
                    SetDefaultData();
                }
            }
        }

        public void SaveData()
        {
            lock (_lock)
            {
                if (!Directory.Exists(AppDataFolder))
                {
                    Directory.CreateDirectory(AppDataFolder);
                }

                using (var stream = new FileStream(SettingsFilePath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(ConfigData));
                    serializer.Serialize(stream, this);
                }
            }
        }

        public void AddToListIfNotExists<T>(List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
            }
            list.Insert(0, item);
        }
    }
}