Imports System.Net

Public Class Form3
    Public player_name() As Label
    Public player_hit_num() As Integer
    Public player_shooting_num() As Integer
    Public player_img(7) As Bitmap
    Public player_num As Integer
    Public scene_num As Integer = 0
    Public scenes_break As Boolean = False

    Dim player() As PictureBox
    Dim player_health_graph() As Label
    Dim player_health() As Integer
    Dim face_direaction() As Boolean
    Dim alive() As Boolean
    Dim scenes() As PictureBox
    Dim airplane_times() As Integer

    Dim player_deathimg(3) As Bitmap


    Dim zero As Integer = 0
    Dim count As Integer = 0
    Dim check_count As Integer = -1
    Dim power As Integer = 0 '0~25
    Dim real_round As Integer = -1
    Dim round As Integer
    Dim pressing_time As Integer
    Dim pressing_counter As Integer
    Dim temp As Integer
    Dim jump_counter As Integer = 0
    Dim jumping_player As Integer
    Dim shoot_x As Integer
    Dim shoot_y As Integer
    Dim y As Integer
    Dim x As Integer
    Dim scene As Integer
    Dim shoot_player As Integer

    Dim Pressing_left As Boolean = False
    Dim Pressing_right As Boolean = False
    Dim Pressing_space As Boolean = False
    Dim move_direaction As Boolean
    Dim airplane As Boolean
    Dim moving As Boolean = False
    Dim gravity_enabled As Boolean = True
    Dim movable As Boolean
    Dim jumping As Boolean
    Dim counting As Boolean
    Dim is_falling As Boolean
    Dim PictureBox_temp As PictureBox

    Dim maxX1 As Integer
    Dim minX1 As Integer
    Dim maxY1 As Integer
    Dim minY1 As Integer
    Dim maxX2 As Integer
    Dim minX2 As Integer
    Dim maxY2 As Integer
    Dim minY2 As Integer


    Public Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReDim player(player_num - 1)  '定義區
        ReDim player_name(player_num - 1)
        ReDim player_health(player_num - 1)
        ReDim player_health_graph(player_num - 1)
        ReDim player_hit_num(player_num - 1)
        ReDim player_shooting_num(player_num - 1)
        ReDim face_direaction(player_num - 1)
        ReDim airplane_times(player_num - 1)
        ReDim alive(player_num - 1)

        player_img(0) = My.Resources.藍1
        player_img(1) = My.Resources.紅1
        player_img(2) = My.Resources.藍2
        player_img(3) = My.Resources.紅2
        player_img(4) = My.Resources.藍1
        player_img(5) = My.Resources.紅1
        player_img(6) = My.Resources.藍2
        player_img(7) = My.Resources.紅2
        player_img(4).RotateFlip(RotateFlipType.RotateNoneFlipX)
        player_img(5).RotateFlip(RotateFlipType.RotateNoneFlipX)
        player_img(6).RotateFlip(RotateFlipType.RotateNoneFlipX)
        player_img(7).RotateFlip(RotateFlipType.RotateNoneFlipX)
        player_deathimg(0) = My.Resources.藍1dead
        player_deathimg(1) = My.Resources.紅1dead
        player_deathimg(2) = My.Resources.藍2dead
        player_deathimg(3) = My.Resources.藍2dead

        For i As Integer = 0 To player_num - 1 '創建角色
            player(i) = New PictureBox
            player_name(i) = New Label
            player_health_graph(i) = New Label
            Me.Controls.Add(player(i))
            Me.Controls.Add(player_name(i))
            Me.Controls.Add(player_health_graph(i))
            If i Mod 2 = 0 Then
                player(i).Tag = 1
                player(i).Image = player_img(i + 4)
            Else
                player(i).Tag = 0
                player(i).Image = player_img(i)
            End If

            player(i).Size = New Size(50, 50)
            player(i).BackColor = Color.Transparent
            player(i).SizeMode = PictureBoxSizeMode.StretchImage
            player_name(i).AutoSize = True
            player_name(i).BackColor = Color.Transparent
            player_health_graph(i).Size = New Size(55, 13)

            player_health_graph(i).Text = 100
            player_health_graph(i).BackColor = Color.Orange
            player_health(i) = 100
            player_hit_num(i) = 0
            player_shooting_num(i) = 0
            face_direaction(i) = player(i).Tag
            airplane_times(i) = 3
            alive(i) = True
        Next i

        Initialization()
        now_player.Enabled = True
        global_timer.Enabled = True
        If Form1.isMuted Then
            My.Computer.Audio.Stop()
            PictureBox5.Image = My.Resources.mute
        Else
            My.Computer.Audio.Play(My.Resources.GameMusic, AudioPlayMode.BackgroundLoop)
            PictureBox5.Image = My.Resources.music
        End If
        PictureBox5.BackColor = Color.Transparent
    End Sub


    Sub Scenes1_create() '生成場景，由前一個表單呼叫
        ReDim scenes(28)
        scene_num = 28
        scene = 1
        While temp < scene_num
            scenes(temp) = New PictureBox
            If temp < 4 Then '第一層空島
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 30, 60)
            ElseIf temp < 8 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 230, 60)
            ElseIf temp < 12 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 430, 60)
            ElseIf temp < 16 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 630, 60)
            ElseIf temp < 20 Then '第二層空島
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 80, 200)
            ElseIf temp < 24 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 330, 200)
            ElseIf temp < 28 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 580, 200)
            End If
            scenes(temp).Size = New Size(35, 60)
            scenes(temp).SizeMode = PictureBoxSizeMode.StretchImage
            Select Case temp Mod 4
                Case 0
                    scenes(temp).Image = My.Resources.空島1
                Case 1
                    scenes(temp).Image = My.Resources.空島2
                Case 2
                    scenes(temp).Image = My.Resources.空島3
                Case 3
                    scenes(temp).Image = My.Resources.空島4
            End Select
            scenes(temp).BackColor = Color.Transparent
            Me.Controls.Add(scenes(temp))
            temp += 1
        End While
        scenes(scene_num) = New PictureBox '建立地面
        scenes(scene_num).Image = My.Resources.土地
        scenes(scene_num).SizeMode = PictureBoxSizeMode.StretchImage
        scenes(scene_num).Location = New Point(0, 370)
        scenes(scene_num).Width = 800
        scenes(scene_num).Height = 80
        Me.Controls.Add(scenes(scene_num))

        For i As Integer = 0 To player_num - 1  '將角色放置在起始位置
            If i Mod 2 = 0 Then '0 2
                player(i).Location = New Point(i * 25 + 50, 320 - i * 85)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            Else '1 3
                player(i).Location = New Point(ClientSize.Width - 100 - (i - 1) * 25, 320 - (i - 1) * 85)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            End If
        Next i
        now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
    End Sub
    Sub Scenes2_create() '生成場景，由前一個表單呼叫
        Me.BackgroundImage = My.Resources.地圖2背景
        Me.BackgroundImageLayout = ImageLayout.Stretch
        scene_num = 24
        ReDim scenes(scene_num)
        scene = 2
        While temp < scene_num
            scenes(temp) = New PictureBox
            If temp < 4 Then '第一層空島
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 0, 310)
            ElseIf temp < 8 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 660, 310)
            ElseIf temp < 12 Then '第一層空島
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 100, 250)
            ElseIf temp < 16 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 560, 250)
            ElseIf temp < 20 Then '第二層空島
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 200, 190)
            ElseIf temp < 24 Then
                scenes(temp).Location = New Point(35 * (temp Mod 4) + 460, 190)
            End If
            scenes(temp).Size = New Size(35, 60)
            scenes(temp).SizeMode = PictureBoxSizeMode.StretchImage
            Select Case temp Mod 4
                Case 0
                    scenes(temp).Image = My.Resources.空島1
                Case 1
                    scenes(temp).Image = My.Resources.空島2
                Case 2
                    scenes(temp).Image = My.Resources.空島3
                Case 3
                    scenes(temp).Image = My.Resources.空島4
            End Select
            scenes(temp).BackColor = Color.Transparent
            Me.Controls.Add(scenes(temp))
            temp += 1
        End While
        scenes(scene_num) = New PictureBox '建立地面
        scenes(scene_num).Image = My.Resources.土地
        scenes(scene_num).SizeMode = PictureBoxSizeMode.StretchImage
        scenes(scene_num).Location = New Point(0, 370)
        scenes(scene_num).Width = 800
        scenes(scene_num).Height = 80
        Me.Controls.Add(scenes(scene_num))

        For i As Integer = 0 To player_num - 1  '將角色放置在起始位置
            If i Mod 2 = 0 Then
                player(i).Location = New Point(i * 50 + 50, 260 - i * 30)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            Else
                player(i).Location = New Point(ClientSize.Width - 100 - (i - 1) * 50, 260 - (i - 1) * 30)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            End If
        Next i
        now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
    End Sub
    Sub Scenes3_create() '生成場景，由前一個表單呼叫
        Me.BackgroundImage = My.Resources.地圖3背景
        Me.BackgroundImageLayout = ImageLayout.Stretch
        scene_num = 0
        ReDim scenes(scene_num)
        scenes(scene_num) = New PictureBox
        scenes(scene_num).Image = My.Resources.地圖3前景
        scenes(scene_num).SizeMode = PictureBoxSizeMode.StretchImage
        scenes(scene_num).Location = New Point(0, 370)
        scenes(scene_num).Width = 800
        scenes(scene_num).Height = 80
        Me.Controls.Add(scenes(scene_num))

        For i As Integer = 0 To player_num - 1  '將角色放置在起始位置
            If i Mod 2 = 0 Then
                player(i).Location = New Point(i * 100 + 50, 320)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            Else
                player(i).Location = New Point(ClientSize.Width - (i) * 100, 320)
                player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
            End If
        Next i
        now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
    End Sub
    Sub GameBody() '初始化，因為直接在timer裡初始化的話，可能會一次初始化好幾次導致回合數亂掉，
        '           所以用count讓初始化可以只運行一次，並做遊戲是否結束的檢測
        If count = 0 Then
            Initialization()
            If Winning_check() Then
                Form7.Show()
                Me.Close()
            End If
        End If
    End Sub


    Function Initialization() '每回合的初始化
        count = 0
        check_count = 0
        pressing_counter = 0
        fire_timer.Enabled = False
        show_power.Size = New Size(0, 10)
        show_power.BackColor = Color.Transparent
        show_power.Location = New Point(85.2)
        bullet.Location = New Point(-1, -1)
        jumping = False
        airplane = False
        Pressing_left = False
        Pressing_right = False
        Pressing_space = False
        movable = True
        real_round += 1
        real_round = real_round Mod player_num

        If player_health(real_round) = 0 Then
            round = (real_round + 2) Mod player_num
        Else
            round = real_round
        End If
        For i As Integer = 0 To player_num - 1
            If alive(i) AndAlso player_health(i) <= zero Then
                If Not Form1.isMuted Then
                    My.Computer.Audio.Play(My.Resources.die, AudioPlayMode.Background)
                End If
                player(i).Image = player_deathimg(i)
                player(i).Location = New Point(player(i).Location.X, player(i).Location.Y)
                player_health_graph(i).Visible = False
                alive(i) = False
            End If
        Next
        airplane_label.Text = "紙飛機 次數:" + airplane_times(round).ToString
        x = 0
        pressing_time = 26
        Label1.Text = "0"
        now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
        Return Nothing
    End Function


    Function Winning_check() As Boolean '檢測遊戲是否結束
        If player_num = 2 Then
            If player_health(0) = 0 Then
                If Not Form1.isMuted Then
                    My.Computer.Audio.Play(My.Resources.die, AudioPlayMode.Background)
                End If
                MsgBox("red win",, "Game")
                Return True
            ElseIf player_health(1) = 0 Then
                If Not Form1.isMuted Then
                    My.Computer.Audio.Play(My.Resources.die, AudioPlayMode.Background)
                End If
                MsgBox("blue win",, "Game")
                Return True
            End If
        ElseIf player_health(0) = 0 And player_health(2) = 0 Then
            If Not Form1.isMuted Then
                My.Computer.Audio.Play(My.Resources.die, AudioPlayMode.Background)
            End If
            MsgBox("red win",, "Game")
            Return True

        ElseIf player_health(1) = 0 And player_health(3) = 0 Then
            If Not Form1.isMuted Then
                My.Computer.Audio.Play(My.Resources.die, AudioPlayMode.Background)
            End If
            MsgBox("blue win",, "Game")
            Return True
        End If
        Return False
    End Function
    Function Route(route_x As Integer, power As Integer) As Integer '計算彈道的二次函數 輸入power以及x值 return y
        temp = 0
        While temp ^ 2 < (375 - power * 15) / 0.002
            temp += 1
        End While
        Dim route_y As Integer = (((route_x - temp) ^ 2) / 500) + (power * 15)
        Return route_y
    End Function

    Sub Bullet_check() '子彈的碰撞處理

        If round = shoot_player Then
            For i As Integer = 0 To player_num - 1 '對象為玩家的碰撞檢測
                If i <> shoot_player AndAlso airplane = False AndAlso player_health(i) > 0 AndAlso hit_detect(bullet, player(i)) AndAlso player(shoot_player).Tag <> player(i).Tag Then
                    player_hit_num(shoot_player) += 1
                    player_health(i) -= 25
                    player_health_graph(i).Text = player_health(i).ToString
                    player_health_graph(i).Size = New Size(player_health_graph(i).Size.Width - 12, player_health_graph(i).Size.Height)
                    If Not Form1.isMuted Then
                        My.Computer.Audio.Play(My.Resources.Bullet_hit, AudioPlayMode.Background)
                    End If
                    fire_timer.Enabled = False
                    GameBody()
                    check_count += 1
                    count += 1
                End If
            Next i

            For i As Integer = 0 To scene_num  '對象為場景的碰撞檢測
                If hit_detect(bullet, scenes(i), True) And y < bullet.Location.Y Then
                    If airplane Then
                        player(shoot_player).Location = New Point(bullet.Location.X, scenes(i).Location.Y - 50)
                        player_name(shoot_player).Location = New Point(player(shoot_player).Location.X, player(shoot_player).Location.Y - 30)
                        player_health_graph(shoot_player).Location = New Point(player(shoot_player).Location.X, player(shoot_player).Location.Y - 15)
                        now_player.Location = New Point(player(shoot_player).Location.X + 10, player(shoot_player).Location.Y - 60)
                    End If
                    If Not Form1.isMuted Then
                        My.Computer.Audio.Play(My.Resources.Bullet_hit, AudioPlayMode.Background)
                    End If
                    If scenes_break AndAlso i < scene_num Then
                        scenes(i).Location = New Point(800, 400)
                        scenes(i).Enabled = False
                    End If
                    fire_timer.Enabled = False
                    GameBody()
                    check_count += 1
                    count += 1
                End If
            Next i

            If bullet.Location.X > 800 OrElse bullet.Location.X < 0 OrElse bullet.Location.Y > 400 OrElse bullet.Location.Y < -200 Then
                '子彈飛出範圍的檢測
                bullet.Location = New Point(-1, -1)
                fire_timer.Enabled = False
                GameBody()
                count += 1
            End If
        End If

    End Sub

    Function hit_detect(bullet As PictureBox, PictureBox_temp As PictureBox, Optional scene As Boolean = False) As Boolean '檢測兩個picturebox之間有沒有重疊(碰撞)
        maxX1 = bullet.Location.X + bullet.Width
        minX1 = bullet.Location.X
        maxY1 = bullet.Location.Y + bullet.Height
        minY1 = bullet.Location.Y

        maxX2 = PictureBox_temp.Location.X + PictureBox_temp.Width
        minX2 = PictureBox_temp.Location.X
        If scene Then
            maxY2 = PictureBox_temp.Location.Y + 15
        Else
            maxY2 = PictureBox_temp.Location.Y + PictureBox_temp.Height
        End If
        minY2 = PictureBox_temp.Location.Y

        Return maxX1 > minX2 And maxX2 > minX1 And maxY1 > minY2 And maxY2 > minY1
    End Function
    Function fall_detect(player As Point) As Boolean '檢查玩家腳下是否有東西可以站

        Dim falling As Boolean = True
        For i As Integer = 0 To scene_num - 1
            If scenes(i).Location.X < player.X + 25 AndAlso scenes(i).Location.X + 35 > player.X + 25 AndAlso scenes(i).Location.Y < player.Y - 5 AndAlso scenes(i).Location.Y + 20 > player.Y - 5 And scenes(i).Enabled = True Then
                falling = False
            End If
        Next i
        If player.Y > 370 Then
            falling = False
        End If
        Return falling
    End Function


    Private Sub airplane_Button_Click(sender As Object, e As EventArgs) Handles airplane_label.Click, airplane_picturebox.Click '紙飛機道具
        If movable And airplane = False And airplane_times(round) > 0 Then
            airplane = True
            airplane_times(round) -= 1
        End If
    End Sub

    Private Sub global_timer_Tick(sender As Object, e As EventArgs) Handles global_timer.Tick '全域的timer 控制移動、重力、跳躍、發射的力道檢測
        If moving Then
            If face_direaction(round) AndAlso player(round).Location.X < 750 Then
                player(round).Location = New Point(player(round).Location.X + 5, player(round).Location.Y)
                player_name(round).Location = New Point(player(round).Location.X, player(round).Location.Y - 30)
                player_health_graph(round).Location = New Point(player(round).Location.X, player(round).Location.Y - 15)
                now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
            ElseIf player(round).Location.X > 0 Then
                player(round).Location = New Point(player(round).Location.X - 5, player(round).Location.Y)
                player_name(round).Location = New Point(player(round).Location.X, player(round).Location.Y - 30)
                player_health_graph(round).Location = New Point(player(round).Location.X, player(round).Location.Y - 15)
                now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
            End If
        End If

        If gravity_enabled Then
            For i As Integer = 0 To player_num - 1
                If alive(i) AndAlso fall_detect(New Point(player(i).Location.X - 15, player(i).Location.Y + 52)) And fall_detect(New Point(player(i).Location.X + 15, player(i).Location.Y + 52)) Then
                    player(i).Location = New Point(player(i).Location.X, player(i).Location.Y + 5)
                    player_name(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 30)
                    player_health_graph(i).Location = New Point(player(i).Location.X, player(i).Location.Y - 15)
                    If i = round Then
                        now_player.Location = New Point(player(i).Location.X + 10, player(i).Location.Y - 60)
                    End If
                End If
            Next i
        End If

        If jumping Then
            If jump_counter < 8 Then
                player(jumping_player).Location = New Point(player(jumping_player).Location.X, player(jumping_player).Location.Y - 8)
                player_name(jumping_player).Location = New Point(player_name(jumping_player).Location.X, player_name(jumping_player).Location.Y - 8)
                player_health_graph(jumping_player).Location = New Point(player_health_graph(jumping_player).Location.X, player_health_graph(jumping_player).Location.Y - 8)
                now_player.Location = New Point(player(round).Location.X + 10, player(round).Location.Y - 60)
            Else
                jump_counter = 0
                jumping = False
                gravity_enabled = True
            End If
            jump_counter += 1
        End If

        If counting Then
            If pressing_time > 0 And movable Then
                If pressing_time >= 20 Then
                    If pressing_counter Mod 3 = 0 Then
                        pressing_time -= 1
                    End If
                ElseIf pressing_time >= 15 Then
                    If pressing_counter Mod 2 = 0 Then
                        pressing_time -= 1
                    End If
                Else
                    pressing_time -= 1
                End If
            End If
            If pressing_counter < 100 Then
                pressing_counter += 1
            End If
            show_power.Size = New Size((26 - pressing_time) * 25, 10)
            Label1.Text = 26 - pressing_time
        End If
    End Sub

    Private Sub fire_timer_Tick(sender As Object, e As EventArgs) Handles fire_timer.Tick '控制子彈的彈道
        If face_direaction(round) Then
            bullet.Location = New Point(shoot_x + x, Route(x, pressing_time) - player(round).Height - (320 - shoot_y))
            x += 5
        Else
            bullet.Location = New Point(shoot_x - x, Route(x, pressing_time) - player(round).Height - (320 - shoot_y))
            x += 5
        End If
        'If check_count Mod 5 = 0 Then
        Bullet_check()
        'End If
        check_count += 1
        y = bullet.Location.Y
    End Sub
    Private Sub Form3_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown '鍵盤控制事件
        If e.KeyCode = 37 Or e.KeyCode = 65 Then '左
            move_direaction = False
            Pressing_left = True
            If movable Then
                If Not Form1.isMuted Then
                    My.Computer.Audio.Play(My.Resources.moving, AudioPlayMode.BackgroundLoop)
                End If
                face_direaction(round) = False
                player(round).Image = player_img(round)
                moving = True
            End If

        ElseIf e.KeyCode = 39 Or e.KeyCode = 68 Then '右
            move_direaction = True
            Pressing_right = True
            If movable Then
                If Not Form1.isMuted Then
                    My.Computer.Audio.Play(My.Resources.moving, AudioPlayMode.BackgroundLoop)
                End If
                face_direaction(round) = True
                player(round).Image = player_img(round + 4)
                moving = True
            End If
        End If

        If (e.KeyCode = 38 Or e.KeyCode = 87) And jumping = False And Not fall_detect(New Point(player(round).Location.X, player(round).Location.Y + 52)) And movable And jumping = False Then '上 跳
            jumping_player = round
            jump_counter = 0
            gravity_enabled = False
            jumping = True
        End If

        If e.KeyCode = 32 Then '空白鍵
            counting = True
            show_power.BackColor = Color.OrangeRed
            Pressing_space = True
        End If
        If e.KeyCode = 16 Then 'shift 紙飛機
            airplane_Button_Click(airplane_label, New EventArgs)
        End If
    End Sub

    Private Sub Form3_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp '鍵盤控制事件
        If e.KeyCode = 37 Or e.KeyCode = 65 Then '左
            If Pressing_left Then
                My.Computer.Audio.Stop()
                Pressing_left = False
                moving = False
            End If

            My.Computer.Audio.Stop()
        ElseIf e.KeyCode = 39 Or e.KeyCode = 68 Then '右
            If Pressing_right Then
                My.Computer.Audio.Stop()
                Pressing_right = False
                moving = False
            End If

        End If

        If e.KeyCode = 32 Or e.KeyCode = 65 And Pressing_space Then '空白鍵
            Pressing_space = False
            shoot_x = player(round).Location.X + 25
            shoot_y = player(round).Location.Y
            shoot_player = round
            power = pressing_time
            counting = False
            moving = False
            count = 0
            check_count = 0
            fire_timer.Enabled = True
            player_shooting_num(round) += 1
            movable = False
            If Not Form1.isMuted Then
                My.Computer.Audio.Play(My.Resources.shooting, AudioPlayMode.Background)
            End If
        End If
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
End Class