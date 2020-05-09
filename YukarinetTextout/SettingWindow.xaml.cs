using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace YukarinetTextout
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class Setting : Window
    {
        /// <summary>
        /// Initialize parameter
        /// </summary>
        internal struct InitParam { 
            /// <summary>
            /// OutputPath
            /// </summary>
            internal string Path { get; set; }
        }

        public Setting()
        {
            InitializeComponent();
        }


        public string GetOutputPath()
        {
            return txtPath.Text;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="p"></param>
        internal void Init(InitParam p)
        {
            txtPath.Text = p.Path;
        }

        

        private void btnDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "Text Files(.txt)|*.txt|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                txtPath.Text =  saveFileDialog.FileName;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var fullPath = System.IO.Path.GetFullPath(this.GetOutputPath());
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid file path. パスが不正です。");
                return;
            }

            DialogResult = true;
        }
    }
}
