using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xaml;
using System.Xml;
using System.Xml.Serialization;
using Yukarinette;

namespace YukarinetTextout
{
    /// <summary>
    /// InputVoice to Text
    /// </summary>
    public class Textout:IYukarinetteInterface
    {
        private string appLocalDataPath;

        private const string settingFileName = "TextOut.config";

        private SettingData settingData;


        /// <summary>PluginName </summary>
        public override string Name { get { return "TextOut"; } }
        

        public Textout()
        {
        }

        public override void Loaded()
        {
            base.Loaded();
            try
            {
                var local = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                appLocalDataPath = Path.Combine(local, @"Yukarinette\plugins\");
                settingData = ReadSetting();
            }
            catch (Exception ex)
            {
                throw new YukarinetteException(ex);
            }
        }

        /// <summary>OnSetting </summary>
        /// <remarks>Show dialog and Setting save path</remarks>
        public override void Setting()
        {
            base.Setting();

            EditSetting();
        }




        /// <summary> </summary>
        public override void AfterSpeech(string text)
        {
            base.AfterSpeech(text);

            AppendText(text, this.settingData.Path);
        }

        private void AppendText(string text, string path)
        {
            using (var sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
        }


        private SettingData ReadSetting()
        {
            var path = Path.Combine(appLocalDataPath, settingFileName);
            if (File.Exists(path))
            {
                var serializer = new XmlSerializer(typeof (SettingData));
                using (var reader = XmlReader.Create(path))
                {
                    return (SettingData) serializer.Deserialize(reader);
                }
            }
            else
            {
                var outPath = GetDefaultOutputPath();
                return new SettingData()
                {
                    Path = outPath,
                };
            }
        }

        private void EditSetting()
        {
            var f = new Setting();
            f.Init(new Setting.InitParam() { Path = this.settingData.Path });
            bool? result = f.ShowDialog();
            if (result == true)
            {
                var outPath = f.GetOutputPath();
                if (!string.IsNullOrWhiteSpace(outPath))
                {
                    this.settingData.Path = outPath;
                    this.SaveSetting();
                }
            }
        }

        private void SaveSetting()
        {
            var path = Path.Combine(appLocalDataPath, settingFileName);
            var serializer = new XmlSerializer(typeof(SettingData));
            using (var sw = XmlWriter.Create(path))
            {
                serializer.Serialize(sw, this.settingData ?? new SettingData());
            }
        }

        private string GetDefaultOutputPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                                            "textout.txt");
        }
    }
}
