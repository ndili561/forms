using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinApp
{
    public partial class Form1 : Form


    {

        int maxbytes = 0;
        int copied = 0;
        int total = 0;
        public Form1()
        {
            InitializeComponent();
        }

 

        public void Copy1(string sourceDirectory, string targetDirectory)
        {

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            //Gets size of all files present in source folder.
            GetSize(diSource, diTarget);
            maxbytes = maxbytes / 1024;

            progressBar1.Maximum = maxbytes;
            CopyAll(diSource, diTarget);
        }
        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (FileInfo fi in source.GetFiles())
            {

                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);

                total += (int)fi.Length;

                copied += (int)fi.Length;
                copied /= 1024;
                progressBar1.Step = copied;

                progressBar1.PerformStep();
                label1.Text = (total / 1048576).ToString() + "MB of " + (maxbytes / 1024).ToString() + "MB copied";



                label1.Refresh();
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {



                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public void GetSize(DirectoryInfo source, DirectoryInfo target)
        {


            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (FileInfo fi in source.GetFiles())
            {
                maxbytes += (int)fi.Length;//Size of File


            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                GetSize(diSourceSubDir, nextTargetSubDir);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Copy1(@"C:\Users\Dilillon\source", @"C:\Users\Dilillon\Desktop\copy");

            MessageBox.Show("Done");

        }
    }
}

