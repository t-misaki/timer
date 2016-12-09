Imports Windows.UI.Popups
Imports Windows.System.Threading

Public NotInheritable Class MainPage
    Inherits Page

    Private dTime As DateTime             '残り時間計算用
    Private disTimer As DispatcherTimer
    Private flg_Cancel As Boolean = False 'キャンセルフラグ
    Private flg_Pause  As Boolean = False '一時停止フラグ
    Private mdAlarm As New MessageDialog ("タイマー終了","タイマー") 'お知らせダイアログ

    Private Async Sub btn_start_Click(sender As Object, e As RoutedEventArgs) Handles btn_start.Click
        'Dim uri = New Uri("ms-appx:///Assets/alarm.wav")
        'Dim file = Await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri)
        'Dim stream = Await file.OpenAsync(Windows.Storage.FileAccessMode.Read)

        'Dim media = New MediaElement()
        'media.SetSource(stream, file.ContentType)

        soundalarm.Play()


        'フラグ設定
        flg_Cancel = False

        'オブジェクトの可視性設定
        Call VisibleSwitch (flg_Cancel)

        'ボタンに表示されている文字が "開始" の場合
        If btn_start.Content = "開始" Then
            'Timerの設定をする
            Call TimerSet()

            '一時停止でない場合
            If flg_Pause = False Then
                '############################# debug用 #############################
                '変数 dTime にTimePickerの時間を代入
'                dTime = New DateTime(2000,01,01,tp_time.Time.Hours,tp_time.Time.Minutes,tp_time.Time.Seconds)
                dTime = New DateTime(2000,01,01,0,0,3)
            End If

            '残り1時間以上の場合は "時：分：秒" の形式で残り時間を表示
            If tp_time.Time.Hours <> 0 Then
                tb_countdown.FontSize = 80
                tb_countdown.Text = (dTime.ToString("HH:mm:ss"))
            '残り1時間を切った場合は分：秒の形式で残り時間を表示
            Else
                tb_countdown.FontSize = 100
                tb_countdown.Text = (dTime.ToString("mm:ss"))
            End If

            'Timerを起動
            disTimer.Start()

            'ボタンの表示文字を "一時停止" に切り替えフラグを立てる
            btn_start.Content = "一時停止"
            flg_Pause = True

        'ボタンに表示されている文字が "一時停止" の場合
        Else If btn_start.Content = "一時停止" Then
            'Timerを停止
            disTimer.Stop()

            'ボタンの表示文字を "開始" に切り替える
            btn_start.Content = "開始"
        End If
    End Sub

    Private Async Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)      
        '残り時間が0秒でない場合
        If tb_countdown.Text <> "00:00" Then
            '1秒減らす
            dTime = dTime.AddSeconds(-1)
        '残り時間が0秒の場合
        Else
            'Timerを停止
            disTimer.Stop()
            '表示していた残り時間を非表示にする
            tb_countdown.Visibility = Visibility.Collapsed
            'お知らせダイアログを表示する
            Call ShowDialog
        End If

        '残り1時間以上の場合は時：分：秒の形式で残り時間を表示
        If dTime.TimeOfDay.Hours <> 0 Then
            tb_countdown.FontSize = 80
            tb_countdown.Text = (dTime.ToString("HH:mm:ss"))
        '残り1時間を切った場合は分：秒の形式で残り時間を表示
        Else
            tb_countdown.FontSize = 100
            tb_countdown.Text = (dTime.ToString("mm:ss"))
        End If
    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles btn_cancel.Click
        '時間設定用TimePickerが表示されていない場合
        If tp_time.Visibility = Visibility.Collapsed Then
            'Timerを停止
            disTimer.Stop()
        End If

        'フラグ設定
        flg_Cancel = True
        flg_Pause = False

        'オブジェクトの可視性設定
        Call VisibleSwitch(flg_Cancel)
    End Sub

    Private Sub VisibleSwitch (flg_Cancel As Boolean)
        'キャンセルボタンが押さていない場合
        If flg_Cancel = False Then
            '時間設定用TimePickerの非表示
            tp_time.Visibility = Visibility.Collapsed
            tb_hour.Visibility = Visibility.Collapsed
            tb_minute.Visibility = Visibility.Collapsed

            'カウントダウン用TextBlockの表示
            tb_countdown.Visibility = Visibility.Visible
        'キャンセルボタンが押された場合
        Else
            '時間設定用TimePickerの表示
            tp_time.Visibility = Visibility.Visible
            tb_hour.Visibility = Visibility.Visible
            tb_minute.Visibility = Visibility.Visible

            'カウントダウン用TextBlockの非表示
            tb_countdown.Visibility = Visibility.Collapsed

            btn_start.Content = "開始"
        End If
    End Sub

    Private Sub TimerSet()
        '新しいDispatcherTimerを作成
        disTimer = New DispatcherTimer()

        '間隔を1秒に設定
        disTimer.Interval = New TimeSpan(0, 0, 1)

        'Timer変数の定義
        AddHandler disTimer.Tick, AddressOf dispatcherTimer_Tick
    End Sub

    Private Async Sub ShowDialog()

        Await mdAlarm.ShowAsync()

        'フラグ設定
        flg_Cancel = True
        flg_Pause = False

        'オブジェクトの可視性設定
        Call VisibleSwitch(flg_Cancel)

    End Sub
End Class
