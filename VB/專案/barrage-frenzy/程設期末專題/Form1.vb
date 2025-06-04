Public Class Form1
    Public isMuted As Boolean = False
    Private Sub Game_start(sender As Object, e As EventArgs) Handles start.Click
        Form2.Show()
        Me.Visible = False

    End Sub
    Private Sub Game_over(sender As Object, e As EventArgs) Handles over.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)

        My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)

            PictureBox1.BackColor = Color.Transparent
    End Sub


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        isMuted = Not isMuted
        If isMuted Then
            My.Computer.Audio.Stop()
            PictureBox1.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox1.Image = My.Resources.music
        End If
    End Sub

    Private Sub manual_Click(sender As Object, e As EventArgs) Handles manual.Click
        Me.Hide()
        Form6.Show()
    End Sub
End Class
