Public Class Form1
    Public Shared progdir As String = My.Computer.FileSystem.CurrentDirectory
    Public Shared hpsvars As String
    Public Shared soundtype As String = "stereo"
    Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Dim strm As System.IO.Stream
        Dim Originalfile As String
        strm = OpenFileDialog1.OpenFile()
        TextBox3.Text = OpenFileDialog1.FileName.ToString()
        Originalfile = OpenFileDialog1.FileName.ToString()
        If Not (strm Is Nothing) Then
            ' would do something here but it's not necessary.
        End If
    End Sub

    Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Originalfile As String
        Originalfile = OpenFileDialog1.FileName.ToString()
        If soundtype = "stereo" Then
            hpsvars = " -y -i """ + Originalfile + """ -ar 32000 -acodec pcm_s16le -map_channel 0.0.0 LEFT.wav -ar 32000 -acodec pcm_s16le -map_channel 0.0.1 RIGHT.wav"
        End If
        If soundtype = "mono" Then
            hpsvars = " -y -i """ + Originalfile + """ -ar 32000 -acodec pcm_s16le -map_channel 0.0.0 LEFT.wav -ar 32000 -acodec pcm_s16le -map_channel 0.0.0 RIGHT.wav"
        End If
        Using runffmpeg As New Process
            runffmpeg.StartInfo.Arguments = hpsvars
            runffmpeg.StartInfo.FileName = progdir + "\tool\ffmpeg.exe"
            runffmpeg.Start()
            runffmpeg.WaitForExit()
        End Using
        Using dspleft As New Process
            dspleft.StartInfo.Arguments = "-e left.wav left.dsp"
            dspleft.StartInfo.FileName = progdir + "\tool\dspadpcm.exe"
            dspleft.Start()
            dspleft.WaitForExit()
        End Using
        Using dspright As New Process
            dspright.StartInfo.Arguments = "-e right.wav right.dsp"
            dspright.StartInfo.FileName = progdir + "\tool\dspadpcm.exe"
            dspright.Start()
            dspright.WaitForExit()
        End Using
        Using d2hmake As New Process
                d2hmake.StartInfo.Arguments = "--left_dsp LEFT.dsp --right_dsp RIGHT.dsp -o """ + Originalfile + ".hps"" --loop_point" + TextBox2.Text
                d2hmake.StartInfo.FileName = progdir + "\tool\dsp2hps.exe"
                d2hmake.Start()
                d2hmake.WaitForExit()
            End Using
            IO.File.Delete(progdir + "\tool\left.wav")
        IO.File.Delete(progdir + "\tool\right.wav")
        IO.File.Delete(progdir + "\tool\left.dsp")
        IO.File.Delete(progdir + "\tool\right.dsp")
        IO.File.Delete(progdir + "\tool\left.txt")
        IO.File.Delete(progdir + "\tool\right.txt")
    End Sub

    Public Sub WaitForExit()

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        soundtype = "stereo"
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        soundtype = "mono"
    End Sub
End Class
