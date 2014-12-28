' author: Chi-Hsu Chen
' datetime: 20140224

Public Class frmMain
    Dim oUtil As New Util
    Dim oLog As New Log
    Dim lsCurrentOpenFile As String
    Const _DEFAULT_COL_SIZE As Integer = 15
    Const _DEFAULT_ROW_SIZE As Integer = 30

    Private Sub pf_CalSize()
        If Not (txtCol.Text = "" Or txtRow.Text = "") Then
            If (IsNumeric(txtCol.Text) And IsNumeric(txtRow.Text)) Then
                txtTotalSize.Text = (Integer.Parse(txtCol.Text) * Integer.Parse(txtRow.Text)).ToString
            End If
        End If
    End Sub

    Protected Friend Sub Btn_Click(sender As Object, e As EventArgs)
        With DirectCast(sender, Button)
            .BackColor = IIf(.BackColor = Color.White, Color.Black, Color.White)
        End With
    End Sub

    Protected Friend Sub btn_MouseMove(sender As Object, e As MouseEventArgs)
#If DEBUG Then
        Console.WriteLine(e.Button.ToString)
#End If

        With DirectCast(sender, Button)
            If chkColorWhite.Checked Then
                .BackColor = Color.White
            End If
        End With
    End Sub

    Private Sub AdjustWindowSize()
        Dim lsWindowWidth As Integer
        Dim lsWindowHeight As Integer

        With Panel1
            .Height = Integer.Parse(txtRow.Text) * oUtil.getHeight
            .Width = Integer.Parse(txtCol.Text) * oUtil.getWidth
        End With
        With GroupBox1
            .Left = Panel1.Left
            .Top = Panel1.Top + Panel1.Height + 10
        End With

        lsWindowHeight = Panel1.Height + Panel1.Top + GroupBox1.Height + StatusStrip1.Height + 50
        lsWindowWidth = Panel1.Left + Panel1.Width + 50
        If Me.Width < lsWindowWidth Then
            Me.Width = lsWindowWidth
        End If
        If Me.Height < lsWindowHeight Then
            Me.Height = lsWindowHeight
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Me
            .Text = "字元Pattern產生器"
            .Left = (Screen.PrimaryScreen.Bounds.Width - .Width) / 2
            .Top = (Screen.PrimaryScreen.Bounds.Height - .Height) / 2
        End With

        txtCol.Text = _DEFAULT_COL_SIZE.ToString()
        txtRow.Text = _DEFAULT_ROW_SIZE.ToString()

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Panel1.Controls.Count = 0 Then
            StatusStrip1.Items(0).Text = "尚未產生Pattern!!"
            Exit Sub
        End If

        lsCurrentOpenFile = oLog.WriteArrayToFile(oUtil.getbtnArray, Integer.Parse(txtRow.Text), Integer.Parse(txtCol.Text), lsCurrentOpenFile)
        StatusStrip1.Items(0).Text = "已存檔，存檔路徑於" & lsCurrentOpenFile
        oLog.Dispose()
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        If Not (txtCol.Text = "" Or txtRow.Text = "") Then
            If (IsNumeric(txtCol.Text) And IsNumeric(txtRow.Text)) Then
                txtTotalSize.Text = (Integer.Parse(txtCol.Text) * Integer.Parse(txtRow.Text)).ToString

                With oUtil
                    .pf_CreateControlArray(Integer.Parse(txtTotalSize.Text), Integer.Parse(txtCol.Text), Me.Panel1)
                    .pf_AddClickHandler(AddressOf Btn_Click)
                    .pf_AddMouseEventHandler(AddressOf btn_MouseMove)
                End With
                AdjustWindowSize()
                lsCurrentOpenFile = ""
            End If
        End If
    End Sub

    ' 載入from file
    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim lsStreamReader As IO.StreamReader
        Dim lsIndex As Integer = 0
        Dim lsRowSize As Integer = 0
        Dim lsColSize As Integer = 0
        Dim lsArrayToParse As String = ""

        Dim xmlDoc As New Xml.XmlDocument
        Dim xmlElement As Xml.XmlElement

        OpenFileDialog.DefaultExt = "*.*"
        If OpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            ' check if file exist
            If Not IO.File.Exists(OpenFileDialog.FileName) Then
                MsgBox(OpenFileDialog.FileName & " not exist, please confirm!!", vbOKOnly + vbInformation, Me.Text)
                Exit Sub
            End If

            'clear all the controls
            Panel1.Controls.Clear()

            ' 依照副檔名決定存取方式
            Select Case OpenFileDialog.FileName.Substring(OpenFileDialog.FileName.Length - 3, 3)
                Case "txt"
                    lsStreamReader = New IO.StreamReader(OpenFileDialog.FileName)
                    While lsStreamReader.Peek >= 0 Or lsIndex <= 2
                        Select Case lsIndex
                            Case 0 'row
                                lsRowSize = Integer.Parse(lsStreamReader.ReadLine())
                            Case 1 'col
                                lsColSize = Integer.Parse(lsStreamReader.ReadLine())
                            Case 2 'content
                                lsArrayToParse = lsStreamReader.ReadLine()
                        End Select

                        lsIndex += 1
                    End While

                Case "xml"
                    xmlDoc.Load(OpenFileDialog.FileName)
                    xmlElement = xmlDoc.SelectSingleNode("Pattern/Content")

                    ' get attribute value by attribute name
                    lsRowSize = Integer.Parse(xmlElement.GetAttribute("RowSize"))
                    lsColSize = Integer.Parse(xmlElement.GetAttribute("ColSize"))
                    lsArrayToParse = xmlElement.GetAttribute("Array")

            End Select

            'refresh information
            txtRow.Text = lsRowSize.ToString
            txtCol.Text = lsColSize.ToString
            txtTotalSize.Text = (lsRowSize * lsColSize).ToString

#If DEBUG Then
            Console.WriteLine("Row=" & txtRow.Text)
            Console.WriteLine("Col=" & txtCol.Text)
            Console.WriteLine("Array Cotent=" & lsArrayToParse)
#End If
            With oUtil
                .pf_CreateControlArray(Integer.Parse(txtTotalSize.Text), Integer.Parse(txtCol.Text), Me.Panel1)
                .pf_AddClickHandler(AddressOf Btn_Click)
                .pf_AddMouseEventHandler(AddressOf btn_MouseMove)
                .pf_FillContetToArray(lsArrayToParse)
            End With
            AdjustWindowSize()
            lsCurrentOpenFile = OpenFileDialog.FileName
            StatusStrip1.Items(0).Text = "從檔案" & lsCurrentOpenFile & "載入完成"
        End If
    End Sub

    Private Sub txtCol_TextChanged(sender As Object, e As EventArgs) Handles txtCol.TextChanged
        pf_CalSize()
    End Sub

    Private Sub txtRow_TextChanged(sender As Object, e As EventArgs) Handles txtRow.TextChanged
        pf_CalSize()
    End Sub
End Class
