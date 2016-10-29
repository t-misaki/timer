Imports Windows.UI.Popups
Imports Windows.System.Threading

Public NotInheritable Class MainPage
    Inherits Page

    Private dTime As DateTime '残り時間計算用

    Private Async Sub btn_start_Click(sender As Object, e As RoutedEventArgs) Handles btn_start.Click
        'ダイアログ出力
'        Dim dialog As New MessageDialog()
'        Await dialog.ShowAsync()

'        textBlock.Text = tp_time.Time.ToString
        
        'Dim uri = New Uri("ms-appx:///Assets/alarm.wav")
        'Dim file = Await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri)
        'Dim stream = Await file.OpenAsync(Windows.Storage.FileAccessMode.Read)

        'Dim media = New MediaElement()
        'media.SetSource(stream, file.ContentType)

        '変数 dTime にTimePickerの時間を代入
        dTime = New DateTime(2000,01,01,tp_time.Time.Hours,tp_time.Time.Minutes,tp_time.Time.Seconds)

        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Hours & ":" & _
            dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)

        '時間設定用TimePickerの非表示
        tp_time.Visibility = Visibility.Collapsed
        tb_hour.Visibility = Visibility.Collapsed
        tb_minute.Visibility = Visibility.Collapsed

        'カウントダウン用TextBlockの表示
        tb_countdown.Visibility = Visibility.Visible

        'Timer変数の定義
        Dim dispatcherTimer = New DispatcherTimer()
        AddHandler dispatcherTimer.Tick, AddressOf dispatcherTimer_Tick
        '間隔を1秒に設定
        dispatcherTimer.Interval = New TimeSpan(0,0,1)
        'Timerを起動
        dispatcherTimer.Start()

    End Sub

    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Hours & ":" & _
            dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)

        '1秒減らす
        dTime = dTime.AddSeconds(-1)

        'If textBlock.Text = 0 Then
        'End If
    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles btn_cancel.Click
        
    End Sub
End Class
