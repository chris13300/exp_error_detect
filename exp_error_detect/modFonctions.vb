Module modFonctions
    Public Function expV2(cheminEXP As String) As Boolean
        Dim lecture As IO.FileStream, buffer As Long, tabEXP() As Byte

        lecture = New IO.FileStream(cheminEXP, IO.FileMode.Open)

        'SugaR Experience version 2
        '0123456789abcdef0123456789
        buffer = 26
        ReDim tabEXP(buffer - 1)
        lecture.Read(tabEXP, 0, buffer)
        lecture.Close()

        If System.Text.Encoding.UTF8.GetString(tabEXP) <> "SugaR Experience version 2" Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function nomFichier(chemin As String) As String
        Return My.Computer.FileSystem.GetName(chemin)
    End Function

End Module
