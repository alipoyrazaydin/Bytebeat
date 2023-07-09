Imports System.Runtime.InteropServices
Imports BytebeatTesting.WaveDirectInterop
' I ported this code from my KSL library so
' don't expect anything to work properly.
' WARNING: UNOPTIMIZED CODE!
Public Class WaveDirect
    Dim mInit As Boolean = False
    Dim mReady As Boolean = False
    Dim mSampleRate As Integer = 44100
    Dim mChannels As Integer = 1
    Dim mBlockCount As Integer = 8
    Dim mBlockSample As Integer = 512
    Dim mBlockFree As Integer = mBlockCount
    Dim mBlockMemory1 As IntPtr = Nothing
    Dim mBlockMemory2 As IntPtr = Nothing
    Dim mBlockMemoryS As Boolean = False
    Dim mBufferLock As Boolean = False

    Dim wavFormat As WAVEFORMATEX
    Dim wavHead1 As WAVEHDR
    Dim wavHead2 As WAVEHDR
    Dim hWaveOut As Int32
    Dim Counter As Long = 1
    Public Sub New()
    End Sub
    Public Sub SetAudioFormat(SampleRate As Integer, Channels As Integer, Optional BlockCount As Integer = 8, Optional BlockSample As Integer = 512)
        If mInit Then Throw New Exception("Audio Manager instance is initialized, Cannot perform action!")
        mSampleRate = SampleRate
        mChannels = Channels
        mBlockCount = BlockCount
        mBlockSample = BlockSample
    End Sub
    Dim BufferSamples As Integer
    Dim BufferBytes As Integer
    Dim WDelegate As WaveDelegate = New WaveDelegate(AddressOf waveOutProcWRP)
    Function WrapModulo(r As Decimal, k As Decimal)
        Dim remainder = r Mod k
        Return If(remainder < 0, remainder + k, remainder)
    End Function
    Public Function Init() As Integer
        With wavFormat
            .wFormatTag = WAVE_FORMAT_PCM
            .nChannels = mChannels
            .wBitsPerSample = mBlockCount
            .nSamplesPerSec = mSampleRate
            .nBlockAlign = .nChannels * .wBitsPerSample / 8
            .nAvgBytesPerSec = .nBlockAlign * .nSamplesPerSec
            .cbSize = 0
        End With
        BufferSamples = wavFormat.nSamplesPerSec
        BufferBytes = BufferSamples * wavFormat.nBlockAlign
        mBlockMemory1 = Marshal.AllocHGlobal(BufferBytes)
        mBlockMemory2 = Marshal.AllocHGlobal(BufferBytes)
        waveOutOpen(hWaveOut, WAVE_MAPPER, wavFormat, WDelegate, 0, CALLBACK_FUNCTION)
        For i = 0 To BufferBytes - 1
            Dim j As Single = Algorithm.Invoke(Counter)
            Marshal.WriteByte(mBlockMemory1, i * wavFormat.nBlockAlign, CByte(If(Single.IsNaN(j), 0, Convert.ToByte(WrapModulo(j, 256)))))
            Counter += 1
        Next
        With wavHead1
            .lpData = mBlockMemory1
            .dwBufferLength = BufferBytes
            .lpNext = 1
        End With
        With wavHead2
            .lpData = mBlockMemory2
            .dwBufferLength = BufferBytes
            .lpNext = 1
        End With
        waveOutPrepareHeader(hWaveOut, wavHead1, Len(wavHead1))
        waveOutPrepareHeader(hWaveOut, wavHead2, Len(wavHead2))
        waveOutWrite(hWaveOut, wavHead1, Len(wavHead1))
        mReady = True
        Dim lpThread As New Threading.Thread(New Threading.ThreadStart(AddressOf MainThread))
        lpThread.IsBackground = True
        lpThread.Start()
    End Function
    Private Function waveOutProcWRP(ByVal hwo As IntPtr, ByVal uMsg As Integer, ByVal dwInstance As Integer, ByRef wavhdr As WAVEHDR, ByVal dwParam2 As Integer)
        Return waveOutProc(hwo, uMsg, dwInstance, wavhdr, dwParam2)
    End Function
    Private Function waveOutProc(ByVal hwo As IntPtr, ByVal uMsg As Integer, ByVal dwInstance As Integer, ByRef wavhdr As WAVEHDR, ByVal dwParam2 As Integer)
        If uMsg = WOM_DONE Then
            mBufferLock = False
        End If
    End Function
    Public Property Algorithm As Func(Of Long, Single)
    Public Function MainThread() As Integer
        While mReady
            While mBufferLock = True
            End While
            Dim wv As WAVEHDR = If(mBlockMemoryS = True, wavHead1, wavHead2)
            Dim va As IntPtr = If(mBlockMemoryS = True, mBlockMemory1, mBlockMemory2)
            mBufferLock = True

            For i = 0 To BufferBytes - 1
                Dim j As Double = Algorithm.Invoke(Counter)
                Marshal.WriteByte(va, i * wavFormat.nBlockAlign, CByte(If(Single.IsNaN(j), 0, Convert.ToByte(WrapModulo(j, 256)))))
                Counter += 1
            Next
            waveOutWrite(hWaveOut, wv, Len(wv))
            mBlockMemoryS = Not mBlockMemoryS
        End While
    End Function
End Class
Public Class WaveDirectInterop
    <StructLayout(LayoutKind.Sequential)>
    Public Structure WAVEHDR
        Public lpData As Integer
        Public dwBufferLength As Integer
        Public dwBytesRecorded As Integer
        Public dwUser As Integer
        Public dwFlags As Integer
        Public dwLoops As Integer
        Public lpNext As Integer
        Public Reserved As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure WAVEFORMATEX
        Public wFormatTag As Int16
        Public nChannels As Int16
        Public nSamplesPerSec As Int32
        Public nAvgBytesPerSec As Int32
        Public nBlockAlign As Int16
        Public wBitsPerSample As Int16
        Public cbSize As Int16
    End Structure

    Public Declare Function waveOutOpen Lib "winmm.dll" (ByRef lphWaveOut As Int32, ByVal uDeviceID As Int32, ByRef lpFormat As WAVEFORMATEX, ByVal dwCallback As WaveDelegate, ByVal dwInstance As Int32, ByVal dwFlags As Int32) As Int32
    Public Declare Function waveOutClose Lib "winmm.dll" (ByVal hWaveOut As Int32) As Int32
    Public Declare Function waveOutPrepareHeader Lib "winmm.dll" (ByVal hWaveOut As Int32, ByRef lpWaveOutHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    Public Declare Function waveOutUnprepareHeader Lib "winmm.dll" (ByVal hWaveOut As Int32, ByRef lpWaveOutHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    Public Declare Function waveOutWrite Lib "winmm.dll" (ByVal hWaveOut As Int32, ByRef lpWaveOutHdr As WAVEHDR, ByVal uSize As Int32) As Int32
    Public Delegate Sub WaveDelegate(ByVal hwo As IntPtr, ByVal uMsg As Integer, ByVal dwInstance As Integer, ByRef wavhdr As WAVEHDR, ByVal dwParam2 As Integer)

    Public Const WAVE_MAPPER = -1&
    Public Const WAVE_FORMAT_PCM = 1
    Public Const CALLBACK_FUNCTION = &H30000                   ' to set up a callback to a function
    Public Const WHDR_DONE = &H1                               ' done bit
    Public Const WHDR_PREPARED = &H2                           ' set if this header has been prepared
    Public Const WHDR_BEGINLOOP = &H4                          ' loop start block
    Public Const WHDR_ENDLOOP = &H8                            ' loop end block
    Public Const WHDR_INQUEUE = &H10                           ' reserved for driver
    Public Const MM_WOM_OPEN = &H3BB                           ' waveform output
    Public Const MM_WOM_CLOSE = &H3BC
    Public Const MM_WOM_DONE = &H3BD
    Public Const WOM_OPEN = MM_WOM_OPEN
    Public Const WOM_CLOSE = MM_WOM_CLOSE
    Public Const WOM_DONE = MM_WOM_DONE
End Class