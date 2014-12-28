' author: Chi-Hsu Chen
' datetime: 20140224

' 簡易版logging util
Public Class Log
    Dim oStreamWriter As System.IO.StreamWriter
    Dim _FILENAME As String = Format(Now.Year, "00") & Format(Now.Month, "00") & Format(Now.Day, "00") & "-" & Format(Now.Day, "00") & Format(Now.Hour, "00") & Format(Now.Minute, "00") & Format(Now.Second, "00")
    Dim _FOLDER As String = Application.StartupPath & "\Data\"

    Const _TXT_EXT As String = ".txt"
    Const _XML_EXT As String = ".xml"
    Const _EMPTY_STRING As String = ""

    Public Sub checkFolder()
        If Not IO.Directory.Exists(_FOLDER) Then
            IO.Directory.CreateDirectory(_FOLDER)
        End If
    End Sub

    Public Function WriteArrayToFile(ByVal btnArray() As Button, ByVal lsRowSize As Integer, ByVal lsColSize As Integer, ByVal FilePath As String) As String
        Dim lsContent As String = ""
        Dim xmlDoc As New Xml.XmlDocument
        Dim xmlPattern As Xml.XmlElement
        Dim xmlContent As Xml.XmlElement
        Dim lsFilePath As String = _FOLDER & _FILENAME

        Try
            ' 手動指定filename
            If FilePath <> _EMPTY_STRING Then
                lsFilePath = FilePath.Substring(0, FilePath.IndexOf("."))
            End If

            oStreamWriter = New IO.StreamWriter(lsFilePath & _TXT_EXT, True, System.Text.Encoding.Default)

            For i As Integer = 0 To btnArray.GetLength(0) - 1
                If btnArray(i).BackColor = Color.Black Then
                    lsContent = lsContent & "1,"
                Else
                    lsContent = lsContent & "0,"
                End If
            Next

            ' format of dump file
            'line 1: row size
            'line 2: col size
            'line 3: array content

            With oStreamWriter
                .WriteLine(lsRowSize.ToString)
                .WriteLine(lsColSize.ToString)
                .WriteLine(lsContent)
                .Flush()
            End With

            ' 存成xml format
            ' 建立節點
            xmlPattern = xmlDoc.CreateElement("Pattern")
            xmlDoc.AppendChild(xmlPattern)

            xmlContent = xmlDoc.CreateElement("Content")
            xmlPattern.AppendChild(xmlContent)

            With xmlContent
                .SetAttribute("RowSize", lsRowSize.ToString)
                .SetAttribute("ColSize", lsColSize.ToString)
                .SetAttribute("Array", lsContent)
            End With

            xmlDoc.Save(lsFilePath & _XML_EXT)

            Return lsFilePath
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return _EMPTY_STRING
        End Try
    End Function

    Public Sub Dispose()
        Me.Finalize()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()
        checkFolder()
    End Sub
End Class
