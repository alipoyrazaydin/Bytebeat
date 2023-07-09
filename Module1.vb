Module Module1
    Sub Main()
        ActualMain()
        Console.ReadLine()
        Console.WriteLine("   -) Started playing the algorithm.")
        Console.CursorVisible = False
        Song()
        Dim kk As New Threading.Thread(Sub()
                                           Dim st As New Stopwatch()
                                           st.Start()
                                           While True
                                               Console.SetCursorPosition(18, 10)
                                               Console.BackgroundColor = ConsoleColor.Black
                                               Console.ForegroundColor = ConsoleColor.Gray
                                               Console.Write(st.Elapsed.ToString("hh\:mm\:ss") & ":" & CInt(st.Elapsed.Milliseconds))
                                               System.Threading.Thread.Sleep(10)
                                           End While
                                       End Sub) With {.IsBackground = True}
        kk.Start()
        Console.ReadLine()
    End Sub
    Sub Song()
        Dim wp As New WaveDirect
        wp.SetAudioFormat(8000, 1)
        wp.Algorithm = AddressOf Algorithm
        wp.Init()
    End Sub
    Sub ActualMain()
        Console.SetWindowPosition(0, 0)
        Console.Clear()
        Console.SetWindowPosition(0, 0)
        Console.CursorVisible = False
        Console.BackgroundColor = ConsoleColor.Gray
        Console.ForegroundColor = ConsoleColor.Black
        Console.WriteLine(" ViperEngine 4's Bytebeat Engine".PadRight(Console.WindowWidth))
        Console.SetCursorPosition(0, Console.WindowHeight - 1)
        Console.WriteLine(" Made by KIGIPUX with Love and KSL :P".PadRight(Console.WindowWidth))
        Console.SetWindowPosition(0, 0)
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.SetCursorPosition(0, 2)
        Console.Write("   Current Algorithm: ")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write("   Music Pack by KIGIPUX, Check code for more info")
        Console.WriteLine()
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("   Press ENTER to start playing.")
    End Sub
End Module