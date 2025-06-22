from machine import ADC, Pin
import utime

#  デバッグ用
Led_Red = Pin(13, Pin.OUT)

# ADC0 に接続（GPI026）
adc_submic = ADC(0)
# ADC1 に接続（GPIO27）
adc_mic = ADC(1)
# ADC2 に接続（GPIO28）
adc_magnet = ADC(2)

#  サブマイク
send_value_submic = 0
threshold_submic = 2  # 超えたら出力

# 磁石のパラメータ
rot_num = 0  # 回転数
voltage_buffer = 0  # 前回の電圧
threshold_magnet = 2  # 回転とみなす電圧差
interval_ms_magnet = 200  # 送信する周期
send_value_magnet = 0  # 送信する値
last_sent_magnet = utime.ticks_ms()  # 最終送信時刻

# マイクのパラメータ
threshold_mic = 2  # 超えたら出力
send_value_mic = 0  # 送信する値
chattering_ms = 200  # チャタリング
last_time_mic = 0  # 最後に出力した時刻

#  送信時間
send_interval_ms = 100
last_send_time = utime.ticks_ms()


def read_voltage(adc):
    raw = adc.read_u16()  # 0〜65535（実際は12ビットの精度）
    voltage = (raw / 65535) * 3.3  # 実際の電圧に変換
    return voltage


while True:
    # サブマイクの取得
    voltage = read_voltage(adc_submic)
    if (voltage >= threshold_submic):
        voltage /= 3.3
        if (voltage > send_value_submic):
            send_value_submic = voltage

     #  磁石（風車）の取得
    voltage = read_voltage(adc_magnet)
    #  差があれば回転したとみなす
    diff = voltage - voltage_buffer
    if (abs(diff) > threshold_magnet):
        rot_num += 1  # 回転数を増やす
     # 電圧保存
    voltage_buffer = voltage

    now = utime.ticks_ms()
    # 磁石の送信データ更新
    if (utime.ticks_diff(now, last_sent_magnet) >= interval_ms_magnet):
        #  value = rot/s
        send_value_magnet = rot_num / \
            (utime.ticks_diff(now, last_sent_magnet) / 1000)
        rot_num = 0
        last_sent_magnet = now

    # チャタリング時間外なら
    if (utime.ticks_diff(now, last_time_mic) >= chattering_ms):
        # 音取得
        voltage = read_voltage(adc_mic)
        # 閾値を超えていたら
        if (voltage > threshold_mic):
            send_value_mic = 1
            last_time_mic = now
        else:
            send_value_mic = 0

    # 値を送信
    if (utime.ticks_diff(now, last_send_time) >= send_interval_ms):
        print("Handle,", send_value_magnet, ",",
              send_value_mic, ",", send_value_submic)
        send_value_submic = 0
        last_send_time = now

    # 値を送信
#    print("Handle,",send_value_magnet,",",send_value_mic)
    # デバッグ用LED
    # 回転しているとき or マイクが出力するとき
    if rot_num > 0 or send_value_mic:
        Led_Red.value(1)
    else:
        Led_Red.value(0)
