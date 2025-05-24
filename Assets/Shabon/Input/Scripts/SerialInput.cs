#nullable enable
using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

namespace Shabon.Input
{
    public class SerialInput : IDisposable
    {
        public float Value => _value;

        private SerialPort? _serialPort;
        private Thread _thread;

        private float _value;
        public SerialInput()
        {
            // 接続可能なポートを検索
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Debug.Log("Found port: " + port);
            }

            foreach (string port in ports)
            {
                _serialPort = new SerialPort(port,115200,Parity.None,8,StopBits.One);
                _serialPort.ReadTimeout = 2000;
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
                try
                {
                    _serialPort.Open();
                    break;
                }
               catch(Exception e)
                {
                    Debug.Log($"ポート{port}が開けませんでした。{e}");
                }
            }
                

            _thread = new Thread(ReadData);
            _thread.Start();
        }

        // データ受信時に呼ぶ関数
        private void ReadData()
        {
             while (true)
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    string message = _serialPort.ReadLine();
                    _value = float.Parse(message);
                }
            }
        }

        public void Dispose()
        {
            _thread.Join();
            _serialPort?.Dispose();
        }

        ~SerialInput()
        {
            Dispose();
        }
    }
}
