Module Music
    Public Function Algorithm(ByVal t As Long) As Double

        '--- 8000Hz Sample Rate - Sierpinski harmony
        Return (t * 5 And t >> 7) Or (t * 3 And t >> 10)
        '---------------------------------

        '--- 8000Hz Sample Rate - goofy-ahh code
        'Return CInt(t / 10000000.0 * t * t + t) Mod 127 Or t >> 4 Or t >> 5 Or t Mod 127 + (t >> 16) Or t
        '---------------------------------

        '--- 8000Hz Sample Rate - cat girl song
        'Return 17 * t Or (t >> 2) + If(t And 32768, 13, 14) * t Or t >> 3 Or t >> 5
        '---------------------------------

        '--- 44100Hz Sample Rate - arppegiator that eats itself
        'Return (((t >> 1) * (15 And (&H234568A0 >> ((t >> 8) And 28)))) Or ((t >> 1) >> (t >> 11)) Xor (t >> 12)) + ((t >> 4) And (t And 24))
        '---------------------------------

        '--- 8000Hz Sample Rate - cool melody+drum idk
        'Return t * IIf(t And 16384, 6, 5) * (4 - (1 And t >> 8)) >> (3 And t >> 9) Or (t Or t * 3) >> 5
        '---------------------------------

        '--- 8000Hz Sample Rate - cool-ified earrape
        'Return ((Not t >> Math.Max((t >> 10) Mod 16, (t >> 12) Mod 16)) And t * Asc("H$TT`0l6"((t >> 11) Mod 8)) / 19) * (10 - (t >> 16))
        '---------------------------------

        '--- 8000Hz Sample Rate - Cat Meow ( code is buggy :< )
        'Return t * ((((t / 2 >> 10 Or t) Mod 16) * t >> 8) And (8 * t >> (12) And 18)) Or -(t / 16) + 64
        '---------------------------------

        '--- 44100Hz Sample Rate - cool music
        'Dim z As Single = 12 * (t >> 18 And 1)
        'Dim w As Single = t / 32768 Mod 1
        'Return 64 * Math.Abs(Math.Sin(t / 50 * p({0, 3, 7, 12, 12, 19, 19, 7}(t >> 11 + (t >> 13 And 3 Xor t >> 14 And 1) And 7) + z))) ^ 0.25 + 32 * Math.Sin(70 / w ^ 0.3) + 16 * (t >> 15 And 1) * (1 - w) * Math.Sin((t / 3 Mod 1024 Or 0) ^ 3) + p({0, 3, 7, 12}(t >> 11 And 3) + z) * t / 11 Mod 16 + (t >> 13 And 1 AndAlso p(&HA7305730 >> (t >> 14 And 28) And 15) * t / 11 And 32) + 48
        '---------------------------------

        '--- 32000Hz Sample Rate - cool music 2
        'Dim tune As Single = -0.0
        'Dim a As Single = t * ({1, 1, 1, 1, 1.5, 1.5, 1.33, 1.33}(t >> 15 And 7) + tune)
        'Dim b As Double = t * ({1, 1.33}(t >> 14 And 1) + tune)
        'Return 128 + (Math.Sin(t * (Int16.Parse("6868686834343434"(t >> 13 And 15)) + tune) / 41) / 8 + (((b * 4) Mod 256) / 256) / 4 + Math.Atan(Math.Tan(a / 41) + Math.Cos(a / 100)) / ({4, 8, 16, 32}(t >> 13 And 3) + tune)) * 100
        '---------------------------------

        '--- 8000Hz Sample Rate - cool drums
        'Return (2 * t * If(t And 16384, 6, 5) * (4 - (3 And t >> 8)) >> (3 And -t >> If(t And 4096, 2, 15)) Or t >> If(t And 8192, If(t And 4096, 4, 5), 3))
        '---------------------------------

        '--- 44100Hz Sample Rate - this thing is alive trust me bro
        'Return -(((30 * Math.Sin(t >> 4)) Xor t >> 9) Or (Math.Tan(t >> 13) And t >> 4) * t >> 5)
        '---------------------------------
    End Function
    Function p(v As Single)
        Return 2 ^ (v / 12)
    End Function

    Dim Ma As Single = 0
    Dim Mb As Single = 0
    Dim Mc As Single = 0
    '' This one is a surprize, go to Module1 and change the
    '' algorithm function to this function.
    Public Function WindyMetallicRaindrops(t As Long) As Single
        If t > 1 Then
            Ma = (Ma * 0.999) + (Rnd() * 0.001)
            If Mb < 0 Then
                Mb = Rnd() * 0.7
                Mc = Rnd()
            Else
                Mb = Mb - (1 / 44100)
            End If
        End If
        Return (Math.Abs((Ma * 256 * (Math.Sin(t / 50000) * 5 + 10)) Mod 256 - 128) + ((t / 300 * ((Mc * 10) + 200)) And 2) * 255 * Math.Pow(Mb, Rnd() / 5 + 4))
    End Function
End Module
