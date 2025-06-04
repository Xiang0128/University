Public Class Form8
    Dim rand As New Random()
    Dim currentImage As Integer = 1
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form3.Scenes1_create()
        Form3.Show()
        Me.Close()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Form3.Scenes2_create()
        Form3.Show()
        Me.Close()
    End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Form3.Scenes3_create()
        Form3.Show()
        Me.Close()
    End Sub



    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Dim randomNumber As Integer = rand.Next(0, 999)
        Dim remainder As Integer = randomNumber Mod 3
        ' 根據隨機數顯示對應的地圖
        Select Case remainder
            Case 0
                Form3.Scenes1_create()
                Form3.Show()
                Me.Close()
            Case 1
                Form3.Scenes2_create()
                Form3.Show()
                Me.Close()
            Case 2
                Form3.Scenes3_create()
                Form3.Show()
                Me.Close()
        End Select
    End Sub


    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Form1.isMuted = Not Form1.isMuted
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox5.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox5.Image = My.Resources.music
        End If
    End Sub

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Form1.isMuted Then
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
        End If
        PictureBox5.BackColor = Color.Transparent
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        If Form3.scenes_break = True Then
            Form3.scenes_break = False
            Label1.Text = "場景破壞:關"
        Else
            Form3.scenes_break = True
            Label1.Text = "場景破壞:開"
        End If

    End Sub
End Class