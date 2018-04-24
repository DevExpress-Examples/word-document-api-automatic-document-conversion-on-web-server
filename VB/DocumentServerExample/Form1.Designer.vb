Namespace DocumentServerExample
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.btnConvert = New System.Windows.Forms.Button()
            Me.timer1 = New System.Windows.Forms.Timer(Me.components)
            Me.textBox1 = New System.Windows.Forms.TextBox()
            Me.label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            ' 
            ' btnConvert
            ' 
            Me.btnConvert.Location = New System.Drawing.Point(12, 35)
            Me.btnConvert.Name = "btnConvert"
            Me.btnConvert.Size = New System.Drawing.Size(75, 23)
            Me.btnConvert.TabIndex = 0
            Me.btnConvert.Text = "Start!"
            Me.btnConvert.UseVisualStyleBackColor = True
            ' 
            ' timer1
            ' 
            Me.timer1.Interval = 5000
            ' 
            ' textBox1
            ' 
            Me.textBox1.Dock = System.Windows.Forms.DockStyle.Top
            Me.textBox1.Location = New System.Drawing.Point(0, 0)
            Me.textBox1.Name = "textBox1"
            Me.textBox1.Size = New System.Drawing.Size(438, 20)
            Me.textBox1.TabIndex = 1
            ' 
            ' label1
            ' 
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(9, 65)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(0, 13)
            Me.label1.TabIndex = 2
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(438, 82)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.textBox1)
            Me.Controls.Add(Me.btnConvert)
            Me.Name = "Form1"
            Me.Text = "Form1"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region

        Private WithEvents btnConvert As System.Windows.Forms.Button
        Private timer1 As System.Windows.Forms.Timer
        Private textBox1 As System.Windows.Forms.TextBox
        Private label1 As System.Windows.Forms.Label
    End Class
End Namespace

