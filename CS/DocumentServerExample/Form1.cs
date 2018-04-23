using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraRichEdit;
using System.IO;
using System.Diagnostics;

namespace DocumentServerExample
{
    public partial class Form1 : Form
    {
        List<string> fileList = new List<string>();
        Dictionary<DocumentFormat, string> dictF = new Dictionary<DocumentFormat, string>();
        RichEditDocumentServer server;
        PerformanceCounter counter;

        public Form1()
        {
            InitializeComponent();
            InitializeFormatDictionary();
            server = new RichEditDocumentServer();
            
            textBox1.Text = Application.StartupPath;
            timer1.Tick += new EventHandler(timer1_Tick);

            string procName = Process.GetCurrentProcess().ProcessName;
            counter = new PerformanceCounter("Process", "Working Set - Private", procName);
            ShowMemoryUsage();
        }

        private void InitializeFormatDictionary()
        {
            dictF.Add(DocumentFormat.OpenXml, "docx");
            dictF.Add(DocumentFormat.Rtf, "rtf");
            dictF.Add(DocumentFormat.PlainText, "txt");
            dictF.Add(DocumentFormat.Doc, "doc");
            dictF.Add(DocumentFormat.ePub, "epub");
            dictF.Add(DocumentFormat.OpenDocument, "odt");
            dictF.Add(DocumentFormat.WordML, "xml");
            dictF.Add(DocumentFormat.Html, "htm");
            dictF.Add(DocumentFormat.Mht, "mht");
        }

        private void PerformConversion(string filePath, DocumentFormat sourceFormat, DocumentFormat destFormat)
        {
            FileStream fsIn = File.OpenRead(filePath);
            string outFileName = Path.ChangeExtension(filePath,dictF[destFormat]);
            server.LoadDocument(fsIn,sourceFormat);
            FileStream fsOut = File.Open(outFileName, FileMode.Create);

            if (destFormat == DocumentFormat.Rtf) {
                server.Options.Export.Rtf.Compatibility.DuplicateObjectAsMetafile = false;
            }
   
            server.SaveDocument(fsOut,destFormat);
            fsIn.Close();
            fsOut.Close();
        }

        private void UpdateFileList(string path)
        {
            if (Directory.Exists(path)) {
                string[] files = System.IO.Directory.GetFiles(path, "*.docx", System.IO.SearchOption.AllDirectories);

                foreach (string file in files) {
                    if (!fileList.Contains(file)) {
                        fileList.Add(file);
                        PerformConversion(file, DocumentFormat.OpenXml, DocumentFormat.Rtf);
                    }
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled) {
                timer1.Stop();
                btnConvert.Text = "Start!";
            }
            else {
                timer1.Start();
                btnConvert.Text = "Stop!";
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            UpdateFileList(textBox1.Text);
            ShowMemoryUsage();
        }
        private void ShowMemoryUsage()
        {
            label1.Text = String.Format("Memory usage: {0:N0} K", counter.RawValue/1024);
        }
    }
}