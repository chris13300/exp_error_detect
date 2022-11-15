Module modMain

    Sub Main()
        Dim fileEXP As String, tabEXP() As Byte, i As Long, iMem As Long, j As Integer
        Dim myString As String, min_count_value As Integer

        Dim tabKEY(7) As Byte, tabKEYMEM(7) As Byte, sameKEY As Boolean
        Dim tabCOUNT(3) As Byte, tabCOUNTMEM(3) As Byte, sameCOUNT As Boolean

        Dim fixedPositions As Integer, minFixedCount As Integer
        Dim maxPositionErrors As Integer, positionErrors As Integer

        Console.Title = My.Computer.Name

        If My.Computer.FileSystem.GetFileInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "Documents\Visual Studio 2013\Projects\exp_error_detect\exp_error_detect\bin\Debug\exp_error_detect.exe").LastWriteTime > My.Computer.FileSystem.GetFileInfo(My.Application.Info.AssemblyName & ".exe").LastWriteTime Then
            MsgBox("Il existe une version plus récente de ce programme !", MsgBoxStyle.Information)
            End
        End If

        fileEXP = Replace(Command(), """", "")
        If Not expV2(fileEXP) Then
            MsgBox(nomFichier(fileEXP) & " <> experience format v2 !?", MsgBoxStyle.Exclamation)
            End
        End If

        Console.WriteLine("Min count value ? (20 to 120)")
        myString = Console.ReadLine
        If IsNumeric(myString) Then
            min_count_value = CInt(myString)
            If 20 <= min_count_value And min_count_value <= 120 Then
                Console.Write(vbCrLf & "Loading " & nomFichier(fileEXP) & "... ")
                tabEXP = My.Computer.FileSystem.ReadAllBytes(fileEXP)
                Console.WriteLine("OK" & vbCrLf)

                fixedPositions = 0
                minFixedCount = 1000000
                maxPositionErrors = 0
                iMem = -1
                Array.Clear(tabKEYMEM, 0, tabKEYMEM.Length)
                Array.Clear(tabCOUNTMEM, 0, tabCOUNTMEM.Length)
                For i = 26 To UBound(tabEXP) Step 24
                    'position                    move             eval              depth             count
                    'h  0  1  2  3  4  5  6  7   8  9    a   b    c   d   e   f     0   1   2   3     4   5   6   7
                    'i +0 +1 +2 +3 +4 +5 +6 +7  +8 +9  +10 +11  +12 +13 +14 +15   +16 +17 +18 +19   +20 +21 +22 +23

                    'get key position and count value
                    Array.Copy(tabEXP, i, tabKEY, 0, 8)
                    Array.Copy(tabEXP, i + 20, tabCOUNT, 0, 4)

                    sameKEY = True
                    For j = 0 To 7
                        If tabKEY(j) <> tabKEY(j) Then
                            sameKEY = False
                            Exit For
                        End If
                    Next

                    'same position ?
                    If sameKEY Then
                        'yes

                        sameCOUNT = True
                        For j = 0 To 3
                            If tabCOUNT(j) <> tabCOUNTMEM(j) Then
                                sameCOUNT = False
                                Exit For
                            End If
                        Next

                        'same count value ?
                        If sameCOUNT And min_count_value <= tabCOUNT(0) And tabCOUNT(0) < 255 And tabCOUNT(1) = 0 And tabCOUNT(2) = 0 And tabCOUNT(3) = 0 Then
                            'yes

                            positionErrors = positionErrors + 1
                            'memorize the index of experience array (=first error of this position)
                            If iMem = -1 Then
                                iMem = i - 24
                                positionErrors = positionErrors + 1
                            End If
                        Else
                            'no

                            'fix errors of previous position
                            If positionErrors >= 3 Then
                                'stats
                                If maxPositionErrors < positionErrors Then
                                    maxPositionErrors = positionErrors
                                End If

                                'bad count value => count = 1
                                For j = iMem To i - 24 Step 24
                                    'stats
                                    If tabEXP(j + 20) < minFixedCount Then
                                        minFixedCount = tabEXP(j + 20)
                                    End If

                                    tabEXP(j + 20) = 1
                                Next
                                fixedPositions = fixedPositions + 1
                            End If

                            'memorize the count value of the new position
                            Array.Copy(tabCOUNT, tabCOUNTMEM, 4)
                            iMem = -1
                            positionErrors = 0
                        End If

                    Else
                        'no

                        'memorize the key of the new position
                        Array.Copy(tabKEY, tabKEYMEM, 8)
                        iMem = -1
                        positionErrors = 0
                    End If
                Next

                Console.WriteLine("Fixed positions : " & Trim(Format(fixedPositions, "# ### ##0")))
                Console.WriteLine("Min count value : " & Trim(Format(minFixedCount, "# ### ##0")))
                Console.WriteLine("Max. errors/pos : " & Trim(Format(maxPositionErrors, "# ### ##0")) & vbCrLf)

                'save fixed experience file ?
                If fixedPositions >= 1000 Then
                    Console.WriteLine("Fix them ? ('y' or 'n')")
                    If Replace(Console.ReadLine, ",", "") = "y" Then
                        Console.Write(vbCrLf & "Saving " & nomFichier(fileEXP) & "... ")
                        My.Computer.FileSystem.WriteAllBytes(fileEXP, tabEXP, False)
                        Console.WriteLine("OK" & vbCrLf)
                    End If
                End If
            End If
        End If
        
        Console.WriteLine("Press ENTER to close this window.")
        Console.ReadLine()
    End Sub

End Module
