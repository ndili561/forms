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
            label1.Visible = false;
            button1.Text = "Click to copy the files";
        }

 

        public void Copy1(string sourceDirectory, string targetDirectory)
        {

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
       
            GetSize(diSource, diTarget);
            maxbytes = maxbytes / 1024;

           

            this.Invoke(new MethodInvoker(delegate() { progressBar1.Maximum = maxbytes; }));
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

                this.Invoke(new MethodInvoker(delegate () { progressBar1.Step = copied; }));
                ;
                this.Invoke(new MethodInvoker(delegate () { progressBar1.PerformStep(); }));
                this.Invoke(new MethodInvoker(delegate () { label1.Visible = true; }));

                this.Invoke(new MethodInvoker(delegate () { label1.Text = (total / 1048576).ToString() + "MB of " + (maxbytes / 1024).ToString() + "MB copied"; }));
                this.Invoke(new MethodInvoker(delegate () { label1.Refresh(); }));

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

        private void  button1_Click(object sender, EventArgs e)
        {
            CopyFiles(@"C:\Users\Dilillon\Documents", @"C:\Users\Dilillon\Desktop\copy");
           
        }


        public async Task CopyFiles(string source, string destination)
        {
            await Task.Run(()=> Copy1(source, destination)) ;
            MessageBox.Show("Done");
        }
    }
}

