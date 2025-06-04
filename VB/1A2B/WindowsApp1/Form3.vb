Public Class Form3
    Private times_count As Integer = 0

    Public Property FormTitle As String
    Private result As String
    Private Function GenerateRandomNumber() As String
        Dim digits As List(Of Char) = New List(Of Char) From {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        Dim random As New Random()
        Dim result As String = ""

        For i As Integer = 0 To 3
            Dim index As Integer = random.Next(0, digits.Count)
            result += digits(index)
            digits.RemoveAt(index)
        Next

        Return result
    End Function

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = FormTitle
        result = GenerateRandomNumber()
        Me.AcceptButton = Button1
        Me.CancelButton = Button2
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim text As String
        Dim Response
        text = "確定要放棄看答案嗎??"
        Response = MsgBox(text, vbYesNo)
        If Response = vbYes Then
            TextBox2.Text = result
            times_count = 0
            text = "要重新產生數字嗎??"
            Response = MsgBox(text, vbYesNo)
            If Response = vbYes Then
                result = GenerateRandomNumber()
                TextBox1.Text = ""
                TextBox2.Text = "****"
                Label1.Text = "0: 0A0B (0000)"
            Else
                Me.Close()
                Dim newForm As New Form1()
                newForm.Show()
            End If
        Else
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Dim newForm As New Form1()
        newForm.Show()
    End Sub

    Private Function repeatnum(input As String) As Boolean
        For i As Integer = 0 To input.Length - 1
            For j As Integer = i + 1 To input.Length - 1
                If input(i) = input(j) Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim A_Count As Integer = 0
        Dim B_Count As Integer = 0
        Dim number As String = TextBox1.Text
        Dim text As String
        Dim Response
        If number.Length <> 4 Then
            MsgBox("輸入數字位數錯誤")
        ElseIf repeatnum(number) = True Then
            MsgBox("輸入數字有重複數字")
        Else
            times_count += 1
            For i As Integer = 0 To 3
                If number(i) = result(i) Then
                    A_Count += 1
                ElseIf result.Contains(number(i)) Then
                    B_Count += 1
                End If
            Next
            If A_Count = 4 Then
                text = "恭喜您猜了" & times_count & "次猜中了!!要繼續下一題??"
                Response = MsgBox(text, vbYesNo)
                If Response = vbYes Then
                    result = GenerateRandomNumber()
                    times_count = 0
                    TextBox1.Text = ""
                    TextBox2.Text = "****"
                    Label1.Text = "0: 0A0B (0000)"
                Else
                    Me.Close()
                End If
            Else
                Label1.Text = $"{times_count}: {A_Count}A{B_Count}B ({number}) "
            End If

        End If
    End Sub
End Class
