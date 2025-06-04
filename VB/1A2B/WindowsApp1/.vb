Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form1
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Application.Exit()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        ComboBox1.Text = ""
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text
        Dim password As String = ComboBox1.Text
        If ComboBox1.Text = "" Then
            MsgBox("請輸入使用者")
        ElseIf ComboBox1.Items.IndexOf(ComboBox1.Text) = -1 Then
            MsgBox("無此使用者")
        Else
            If username = password Then
                Dim form3 As New Form3()
                form3.FormTitle = username & " 歡迎來玩猜看看"
                form3.Show()
                Me.Hide()
            Else
                MsgBox("密碼錯誤")
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        Me.CancelButton = Button3
    End Sub
End Class
