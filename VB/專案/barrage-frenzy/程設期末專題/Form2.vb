Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox4.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox4.Image = My.Resources.music
        End If
        PictureBox4.BackColor = Color.Transparent
    End Sub


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        '1 vs 1
        Form3.player_num = 2
        Form3.Show()
        Form3.Hide()
        Form4.Show()
        Me.Close()

    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        '2 vs 2
        Form3.player_num = 4
        Form3.Show()
        Form3.Hide()
        Form5.Show()
        Me.Close()

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Form1.Show()
        Me.Close()
    End Sub


    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Form1.isMuted = Not Form1.isMuted
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox4.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox4.Image = My.Resources.music
        End If
    End Sub
End Class