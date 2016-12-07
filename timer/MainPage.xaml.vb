Imports Windows.UI.Popups
Imports Windows.System.Threading

Public NotInheritable Class MainPage
    Inherits Page

    Private dTime As DateTime '残り時間計算用
    Private flg_Cancel As Boolean
    Private flg_Pause  As Boolean

    Private Async Sub btn_start_Click(sender As Object, e As RoutedEventArgs) Handles btn_start.Click
        'Dim uri = New Uri("ms-appx:///Assets/alarm.wav")
        'Dim file = Await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri)
        'Dim stream = Await file.OpenAsync(Windows.Storage.FileAccessMode.Read)

        'Dim media = New MediaElement()
        'media.SetSource(stream, file.ContentType)

        'フラグに初期値Falseを設定
        flg_Cancel = False
        flg_Pause  = False
        
        '変数 dTime にTimePickerの時間を代入
        dTime = New DateTime(2000,01,01,tp_time.Time.Hours,tp_time.Time.Minutes,tp_time.Time.Seconds)

        If tp_time.Time.Hours <> 0 Then
        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Hours & ":" & _
            dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)
        Else
        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)
        End If

        '非表示設定
        Call VisibleSwitch (flg_Cancel)

        Dim dispatcherTimer = New DispatcherTimer()
        'Timer変数の定義
        AddHandler dispatcherTimer.Tick, AddressOf dispatcherTimer_Tick
        '間隔を1秒に設定
        dispatcherTimer.Interval = New TimeSpan(0,0,1)
        'Timerを起動
        dispatcherTimer.Start()

        '一時停止フラグを立てる
        flg_Pause = True

        If flg_Pause = True Then
            btn_start.Content = "一時停止"
        End If

    End Sub

    Private Async Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Dim mdAlarm As New MessageDialog ("タイマー終了","タイマー")
        
        '残り0秒になるまでカウントダウン
        If tb_countdown.Text <> "0:0" Then
            '1秒減らす
            dTime = dTime.AddSeconds(-1)
        Else
            tb_countdown.Visibility = Visibility.Collapsed
            Await mdAlarm.ShowAsync()
        End If

        '残り1時間以上の場合は時：分：秒の形式で残り時間を表示
        If dTime.TimeOfDay.Hours <> 0 Then
        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Hours & ":" & _
            dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)
        '残り1時間を切った場合は分：秒の形式で残り時間を表示
        Else
        '時間を表示
        tb_countdown.Text = (dTime.TimeOfDay.Minutes & ":" & _
            dTime.TimeOfDay.Seconds)
        End If

    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles btn_cancel.Click
        flg_Cancel = True
        Call VisibleSwitch(flg_Cancel)
'        Call btn_start_Click(sender ,e)
    End Sub

'    Private Sub VisibleSwitch (tb_countdown As TextBlock, tp_time As TimePicker, tb_hour As TextBlock, tb_minute As TextBlock)
    Private Sub VisibleSwitch (flg_Cancel As Boolean)
        If flg_Cancel = False Then
            '時間設定用TimePickerの非表示
            tp_time.Visibility = Visibility.Collapsed
            tb_hour.Visibility = Visibility.Collapsed
            tb_minute.Visibility = Visibility.Collapsed

            'カウントダウン用TextBlockの表示
            tb_countdown.Visibility = Visibility.Visible
        Else
            '時間設定用TimePickerの非表示
            tp_time.Visibility = Visibility.Visible
            tb_hour.Visibility = Visibility.Visible
            tb_minute.Visibility = Visibility.Visible

            'カウントダウン用TextBlockの表示
            tb_countdown.Visibility = Visibility.Collapsed

            btn_start.Content = "開始"
        End If
    End Sub

End Class
