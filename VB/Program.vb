Imports DevExpress.XtraRichEdit
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Threading
Imports System.IO

Namespace DocumentServerExample
    Class Program
        Shared Sub Main(ByVal args As String())
            Dim fileList As New List(Of String)()
            Dim server As New RichEditDocumentServer()
            Dim dictF As New Dictionary(Of DocumentFormat, String)()
            Dim counter As PerformanceCounter

            InitializeFormatDictionary(dictF)
            Console.WriteLine("This application automatically converts docx files in a specified folder to rtf. The default folder:" & vbCrLf & AppDomain.CurrentDomain.BaseDirectory & vbCrLf & " Do you want to change the folder? y\n")
            Dim answer As String = Console.ReadLine()?.ToLower()
            Dim folder As String

            If answer = "y" Then
                Console.WriteLine("Specify a folder")
                folder = Console.ReadLine()
            Else
                folder = AppDomain.CurrentDomain.BaseDirectory
            End If

            Console.WriteLine("Running conversion. Enter 'Stop' to stop the automatic conversion.")
            Dim procName As String = Process.GetCurrentProcess().ProcessName
            counter = New PerformanceCounter("Process", "Working Set - Private", procName)
            Dim timer As New Timer(Sub(state) Task(folder, server, fileList, dictF), Nothing, 1000, 1000)
            Dim timer2 As New Timer(Sub(state) Task_Memory(counter), Nothing, 1000, 7000)

            Dim answer2 As String = Console.ReadLine()?.ToLower()
            If answer2 = "stop" Then
                Console.WriteLine("Exiting the application.")
            End If
        End Sub

        Private Shared Sub InitializeFormatDictionary(ByVal dictF As Dictionary(Of DocumentFormat, String))
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

        Private Shared Sub Task(ByVal folder As String, ByVal server As RichEditDocumentServer, ByVal fileList As List(Of String), ByVal dictF As Dictionary(Of DocumentFormat, String))
            UpdateFileList(folder, server, fileList, dictF)
        End Sub

        Private Shared Sub Task_Memory(ByVal counter As PerformanceCounter)
            ShowMemoryUsage(counter)
        End Sub

        Private Shared Sub UpdateFileList(ByVal path As String, ByVal server As RichEditDocumentServer, ByVal fileList As List(Of String), ByVal dictF As Dictionary(Of DocumentFormat, String))
            If Directory.Exists(path) Then
                Dim files As String() = Directory.GetFiles(path, "*.docx", SearchOption.AllDirectories)
                For Each file As String In files
                    If Not fileList.Contains(file) Then
                        fileList.Add(file)
                        PerformConversion(file, DocumentFormat.OpenXml, DocumentFormat.Rtf, server, dictF)
                    End If
                Next
            End If
        End Sub

        Private Shared Sub ShowMemoryUsage(ByVal counter As PerformanceCounter)
            Console.WriteLine(String.Format("Memory usage: {0:N0} K", counter.RawValue / 1024))
        End Sub

        Private Shared Sub PerformConversion(ByVal filePath As String, ByVal sourceFormat As DocumentFormat, ByVal destFormat As DocumentFormat, ByVal server As RichEditDocumentServer, ByVal dictF As Dictionary(Of DocumentFormat, String))
            Dim fsIn As FileStream = File.OpenRead(filePath)
            Dim outFileName As String = Path.ChangeExtension(filePath, dictF(destFormat))
            server.LoadDocument(fsIn, sourceFormat)
            Dim fsOut As FileStream = File.Open(outFileName, FileMode.Create)

            If destFormat = DocumentFormat.Rtf Then
                server.Options.Export.Rtf.Compatibility.DuplicateObjectAsMetafile = False
            End If

            server.SaveDocument(fsOut, destFormat)
            Console.WriteLine(outFileName & " file is converted.")
            fsIn.Close()
            fsOut.Close()
        End Sub
    End Class
End Namespace
