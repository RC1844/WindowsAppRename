using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AppName
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                Title = "选择文件夹",
                InitialDirectory = @"",   //@是取消转义字符的意思
                RestoreDirectory = false,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private bool NotNull(ref string Target, string str)
        {
            bool b = true;
            if (str != null && !str.Equals(""))
            {
                str=System.Text.RegularExpressions.Regex.Replace(str, @"\s{1,}", "");
                if (str != null && !str.Equals(""))
                {
                    Target = str;
                    b = false;
                }
            }
            return b;
        }
        private void RenameFile(string DirPath)
        {
            if (Directory.Exists(DirPath))
            {
                foreach (string FilePath in Directory.GetFiles(DirPath, "*.exe"))
                {
                    if (File.Exists(FilePath))
                    {
                        Console.WriteLine(FilePath);
                        FileVersionInfo FileInfo = FileVersionInfo.GetVersionInfo(FilePath);
                        string Version = string.Format("{0}.{1}.{2}.{3}", FileInfo.FileMajorPart, FileInfo.FileMinorPart, FileInfo.FileBuildPart, FileInfo.FilePrivatePart);
                        string FileName = Path.GetFileNameWithoutExtension(FilePath);
                        if(NotNull(ref FileName, FileInfo.ProductName))
                        {
                            _ = NotNull(ref FileName, FileInfo.InternalName);
                        }

                        if (Version.Equals("0.0.0.0"))
                        {
                            Version = "";
                            if(NotNull(ref Version, FileInfo.FileVersion))
                            {
                                _ = NotNull(ref Version, FileInfo.ProductVersion);
                            }
                        }
                        if (!Version.Equals(""))
                        {
                            FileName += "_" + Version;
                        }
                        FileName += ".exe";
                        Console.WriteLine(FileName);
                        FileName = textBox1.Text + "\\" + FileName;
                        File.Move(FilePath, FileName);
                    }
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            RenameFile(textBox1.Text);
        }
    }
}
