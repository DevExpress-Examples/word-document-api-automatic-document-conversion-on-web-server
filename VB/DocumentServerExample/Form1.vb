Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraRichEdit
Imports System.IO
Imports System.Diagnostics

Namespace DocumentServerExample
    Partial Public Class Form1
        Inherits Form

        Private fileList As New List(Of String)()
        Private dictF As New Dictionary(Of DocumentFormat, String)()
        Private server As RichEditDocumentServer
        Private counter As PerformanceCounter

        Public Sub New()
            InitializeComponent()
            InitializeFormatDictionary()
            server = New RichEditDocumentServer()

            textBox1.Text = Application.StartupPath
            AddHandler timer1.Tick, AddressOf timer1_Tick

            Dim procName As String = Process.GetCurrentProcess().ProcessName
            counter = New PerformanceCounter("Process", "Working Set - Private", procName)
            ShowMemoryUsage()
        End Sub

        Private Sub InitializeFormatDictionary()
            dictF.Add(DocumentFormat.OpenXml, "docx")
            dictF.Add(DocumentFormat.Rtf, "rtf")
            dictF.Add(DocumentFormat.PlainText, "txt")
            dictF.Add(DocumentFormat.Doc, "doc")
            dictF.Add(DocumentFormat.ePub, "epub")
            dictF.Add(DocumentFormat.OpenDocument, "odt")
            dictF.Add(DocumentFormat.WordML, "xml")
            dictF.Add(DocumentFormat.Html, "htm")
            dictF.Add(DocumentFormat.Mht, "mht")
        End Sub

        Private Sub PerformConversion(ByVal filePath As String, ByVal sourceFormat As DocumentFormat, ByVal destFormat As DocumentFormat)
            Dim fsIn As FileStream = File.OpenRead(filePath)
            Dim outFileName As String = Path.ChangeExtension(filePath,dictF(destFormat))
            server.LoadDocument(fsIn,sourceFormat)
            Dim fsOut As FileStream = File.Open(outFileName, FileMode.Create)

            If destFormat = DocumentFormat.Rtf Then
                server.Options.Export.Rtf.Compatibility.DuplicateObjectAsMetafile = False
            End If

            server.SaveDocument(fsOut,destFormat)
            fsIn.Close()
            fsOut.Close()
        End Sub

        Private Sub UpdateFileList(ByVal path As String)
            If Directory.Exists(path) Then
                Dim files() As String = System.IO.Directory.GetFiles(path, "*.docx", System.IO.SearchOption.AllDirectories)

                For Each file As String In files
                    If Not fileList.Contains(file) Then
                        fileList.Add(file)
                        PerformConversion(file, DocumentFormat.OpenXml, DocumentFormat.Rtf)
                    End If
                Next file
            End If
        End Sub

        Private Sub btnConvert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConvert.Click
            If timer1.Enabled Then
                timer1.Stop()
                btnConvert.Text = "Start!"
            Else
                timer1.Start()
                btnConvert.Text = "Stop!"
            End If
        End Sub

        Private Sub timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
            UpdateFileList(textBox1.Text)
            ShowMemoryUsage()
        End Sub
        Private Sub ShowMemoryUsage()
            label1.Text = String.Format("Memory usage: {0:N0} K", counter.RawValue\1024)
        End Sub
    End Class
End Namespace