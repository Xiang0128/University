Public Class Form5
    Dim currentImage As Integer = 1
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox1.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox1.Image = My.Resources.music
        End If
        PictureBox1.BackColor = Color.Transparent
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Form3.player_name(0).Text = TextBox1.Text
        Form3.player_name(1).Text = TextBox2.Text
        Form3.player_name(2).Text = TextBox3.Text
        Form3.player_name(3).Text = TextBox4.Text

        Form8.Show()
        Me.Close()
    End Sub

    Private isMuted As Boolean = False
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form1.isMuted = Not Form1.isMuted
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox1.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox1.Image = My.Resources.music
        End If
    End Sub
End Class