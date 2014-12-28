' author: Chi-Hsu Chen
' datetime: 20140224

Public Class Util
    Const _BTN_WIDTH = 20
    Const _BTN_HEIGHT = 20
    Dim btnArray() As Button

    ' 從載入檔案內容設定button array backcolor
    Public Sub pf_FillContetToArray(ByVal lsContent As String)
        Dim lsContentArray() As String

        lsContentArray = Split(lsContent, ",")

        If (lsContentArray.GetLength(0) - 1) <> btnArray.GetLength(0) Then
            MsgBox("Array size mismatch!")
            Exit Sub
        End If

        For i As Integer = 0 To lsContentArray.GetLength(0) - 1
            If lsContentArray(i) = "1" Then
                btnArray(i).BackColor = Color.Black
            ElseIf lsContentArray(i) = "0" Then
                btnArray(i).BackColor = Color.White
            End If
        Next
    End Sub

    Public Sub pf_AddClickHandler(ByVal lsFunc As EventHandler)
        For i As Integer = 0 To btnArray.GetLength(0) - 1
            AddHandler btnArray(i).Click, lsFunc
        Next
    End Sub

    Public Sub pf_AddMouseEventHandler(ByVal lsFunc As MouseEventHandler)
        For i As Integer = 0 To btnArray.GetLength(0) - 1
            AddHandler btnArray(i).MouseMove, lsFunc
        Next
    End Sub

#Region "自我建立控制項Button陣列並依據所給參數分布成NxM型態"
    Public Sub pf_CreateControlArray(ByVal lsArraySize As Integer, ByVal lsShift As Integer, ByRef lsContainer As Panel)
        ReDim btnArray(lsArraySize - 1)

        For i As Integer = 0 To lsArraySize - 1
            btnArray(i) = New Button
            With btnArray(i)
                .Width = _BTN_WIDTH
                .Height = _BTN_HEIGHT

                '對角線排列
                'If i <> 0 Then
                '    .Left = btnArray(i - 1).Left + .Width
                '    If i Mod lsShift = 0 Then
                '        .Top = btnArray(i - 1).Top + .Height
                '    Else
                '        .Top = btnArray(i - 1).Top
                '    End If
                'End If

                '行列對齊排列
                If i <> 0 Then
                    If i Mod lsShift = 0 Then
                        .Left = btnArray(0).Left
                        .Top = btnArray(i - 1).Top + .Height
                    Else
                        .Left = btnArray(i - 1).Left + .Width
                        .Top = btnArray(i - 1).Top
                    End If
                End If

                .Text = ""
                .TabIndex = i
                .BackColor = Color.White
            End With

            ''AddHandler btnArray(i).Click, lsFunctionClick
            ''AddHandler btnArray(i).MouseMove, lsFuncMouseMove
            lsContainer.Controls.Add(btnArray(i))
            Application.DoEvents()

#If DEBUG Then
            Console.WriteLine(i.ToString & ";Left=" & btnArray(i).Left & ";Top=" & btnArray(i).Top)
#End If
        Next
    End Sub
#End Region

    Property getWidth() As Integer
        Get
            Return _BTN_WIDTH
        End Get
        Set(value As Integer)

        End Set
    End Property

    Property getHeight() As Integer
        Get
            Return _BTN_HEIGHT
        End Get
        Set(value As Integer)

        End Set
    End Property

    Property getbtnArray() As Button()
        Get
            Return btnArray
        End Get
        Set(value As Button())

        End Set
    End Property

    Public Sub Dispose()
        Me.Finalize()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
