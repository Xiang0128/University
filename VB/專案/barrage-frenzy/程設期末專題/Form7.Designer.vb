<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form7
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.new_game = New System.Windows.Forms.Button()
        Me.game_over = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'new_game
        '
        Me.new_game.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.new_game.Location = New System.Drawing.Point(58, 503)
        Me.new_game.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.new_game.Name = "new_game"
        Me.new_game.Size = New System.Drawing.Size(155, 46)
        Me.new_game.TabIndex = 4
        Me.new_game.Text = "開啟新局"
        Me.new_game.UseVisualStyleBackColor = True
        '
        'game_over
        '
        Me.game_over.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.game_over.Location = New System.Drawing.Point(834, 503)
        Me.game_over.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.game_over.Name = "game_over"
        Me.game_over.Size = New System.Drawing.Size(155, 46)
        Me.game_over.TabIndex = 5
        Me.game_over.Text = "結束遊戲"
        Me.game_over.UseVisualStyleBackColor = True
        '
        'Form7
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1067, 562)
        Me.Controls.Add(Me.game_over)
        Me.Controls.Add(Me.new_game)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form7"
        Me.Text = "結算畫面"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents new_game As Button
    Friend WithEvents game_over As Button
End Class
