using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace DocumentServerExample
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
        List<string> fileList = new List<string>();
        RichEditDocumentServer server = new RichEditDocumentServer();
        Dictionary<DocumentFormat, string> dictF = new Dictionary<DocumentFormat, string>();
        PerformanceCounter counter;
            InitializeFormatDictionary(dictF);
            Console.WriteLine("This application automatically converts docx files in a specified folder to rtf. The default folder:\r\n" + AppDomain.CurrentDomain.BaseDirectory + "\r\n Do you want to change the folder? y\\n");
            string answer = Console.ReadLine()?.ToLower();
            if (answer == "y")
            {
                Console.WriteLine("Specify a folder");
                var folder = Console.ReadLine();
            }
            else {
                var folder = AppDomain.CurrentDomain.BaseDirectory;
            }
            Console.WriteLine("Running conversion. Enter \"stop\" to stop the automatic conversion.");
            string procName = Process.GetCurrentProcess().ProcessName;
            counter = new PerformanceCounter("Process", "Working Set - Private", procName);
            Timer timer = new Timer(new TimerCallback(_ => Task(AppDomain.CurrentDomain.BaseDirectory, server, fileList, dictF)), null, 1000, 1000);
            Timer timer2 = new Timer(new TimerCallback(_ => Task_Memory(counter)), null, 1000, 7000);
            string answer2 = Console.ReadLine()?.ToLower();
            if (answer2 == "stop")
            {
                Console.WriteLine("Exiting the application.");
            }
        }

        private static void InitializeFormatDictionary(Dictionary<DocumentFormat, string> dictF)
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
        private static void Task(string folder, RichEditDocumentServer server, List<string> fileList, Dictionary<DocumentFormat, string> dictF)
        {
            UpdateFileList(folder, server, fileList, dictF);
        }
        private static void Task_Memory(PerformanceCounter counter)
        {
            ShowMemoryUsage(counter);
        }

        private static void UpdateFileList(string path, RichEditDocumentServer server, List<string> fileList, Dictionary<DocumentFormat, string> dictF)
        {
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.docx", System.IO.SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    if (!fileList.Contains(file))
                    {
                        fileList.Add(file);
                        PerformConversion(file, DocumentFormat.OpenXml, DocumentFormat.Rtf, server, dictF);
                    }
                }
            }
        }
        private static void ShowMemoryUsage(PerformanceCounter counter)
        {
            Console.WriteLine(String.Format("Memory usage: {0:N0} K", counter.RawValue / 1024));
        }
        private static void PerformConversion(string filePath, DocumentFormat sourceFormat, DocumentFormat destFormat, RichEditDocumentServer server, Dictionary<DocumentFormat, string> dictF)
        {
            FileStream fsIn = File.OpenRead(filePath);
            string outFileName = Path.ChangeExtension(filePath, dictF[destFormat]);
            server.LoadDocument(fsIn, sourceFormat);
            FileStream fsOut = File.Open(outFileName, FileMode.Create);

            if (destFormat == DocumentFormat.Rtf)
            {
                server.Options.Export.Rtf.Compatibility.DuplicateObjectAsMetafile = false;
            }

            server.SaveDocument(fsOut, destFormat);
            Console.WriteLine(outFileName + " file is converted.");
            fsIn.Close();
            fsOut.Close();
        }
    }
}