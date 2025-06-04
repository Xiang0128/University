Imports System.Data.SqlTypes

Public Class Form7
    Dim player_name() As Label
    Dim player_dmg() As Label
    Dim player_shoot_num() As Label
    Dim player_hitting_rate() As Label
    ' Dim player_picture() As PictureBox

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReDim player_name(Form3.player_num - 1)
        ReDim player_dmg(Form3.player_num - 1)
        ReDim player_shoot_num(Form3.player_num - 1)
        ReDim player_hitting_rate(Form3.player_num - 1)
        'ReDim player_picture(Form3.player_num - 1)

        For i As Integer = 0 To Form3.player_num - 1
            player_name(i) = New Label
            player_dmg(i) = New Label
            player_shoot_num(i) = New Label
            player_hitting_rate(i) = New Label
            'player_picture(i) = New PictureBox

            player_name(i).Text = Form3.player_name(i).Text
            player_dmg(i).Text = Form3.player_hit_num(i) * 25
            player_shoot_num(i).Text = Form3.player_shooting_num(i)
            player_hitting_rate(i).Text = CType((Form3.player_hit_num(i) * 100) / (Form3.player_shooting_num(i) * 100) * 100, Integer).ToString + "%"
            'player_picture(i).Image = Form3.player_img(i)

            player_name(i).Font = New Font("微軟正黑體", 18, FontStyle.Bold)
            player_dmg(i).Font = New Font("微軟正黑體", 18, FontStyle.Bold)
            player_shoot_num(i).Font = New Font("微軟正黑體", 18, FontStyle.Bold)
            player_hitting_rate(i).Font = New Font("微軟正黑體", 18, FontStyle.Bold)

            player_name(i).AutoSize = True
            player_dmg(i).AutoSize = True
            player_shoot_num(i).AutoSize = True
            player_hitting_rate(i).AutoSize = True
            'player_picture(i).SizeMode = PictureBoxSizeMode.StretchImage
            'player_picture(i).Size = New Size(100, 100)

            If Form3.player_num = 2 Then
                Me.BackgroundImage = My.Resources.結算1v1
                Me.BackgroundImageLayout = ImageLayout.Stretch
                'player_picture(i).Location = New Point(300 * i + 250, 50)
                player_name(i).Location = New Point(300 * i + 280, 150)
                player_dmg(i).Location = New Point(300 * i + 300, 220)
                player_shoot_num(i).Location = New Point(300 * i + 305, 290)
                player_hitting_rate(i).Location = New Point(300 * i + 295, 350)
            Else
                Me.BackgroundImage = My.Resources.結算2v2
                Me.BackgroundImageLayout = ImageLayout.Stretch
                'player_picture(i).Location = New Point(125 * i + 200, 50)
                player_name(i).Location = New Point(125 * i + 200, 150)
                player_dmg(i).Location = New Point(125 * i + 200, 220)
                player_shoot_num(i).Location = New Point(125 * i + 200, 290)
                player_hitting_rate(i).Location = New Point(125 * i + 200, 350)
            End If

            player_name(i).BackColor = Color.Transparent
            player_dmg(i).BackColor = Color.Transparent
            player_shoot_num(i).BackColor = Color.Transparent
            player_hitting_rate(i).BackColor = Color.Transparent
            'player_picture(i).BackColor = Color.Transparent

            Me.Controls.Add(player_name(i))
            Me.Controls.Add(player_dmg(i))
            Me.Controls.Add(player_shoot_num(i))
            Me.Controls.Add(player_hitting_rate(i))
            'Me.Controls.Add(player_picture(i))
        Next i

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles new_game.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub game_over_Click(sender As Object, e As EventArgs) Handles game_over.Click
        Form1.Close()
    End Sub
End Class