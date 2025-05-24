#nullable enable
using System;
using System.IO.Ports;
using System.Linq;
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
            // 接続可能なport名を取得
            string[] ports = SerialPort.GetPortNames();
            var availablePorts = ports.Where(s => s.Contains("COM") || s.Contains("usbmodem"));
            foreach (string port in availablePorts)
            {
                Debug.Log("Found port: " + port);
            }

            foreach (string port in availablePorts)
            {
                _serialPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
                _serialPort.ReadTimeout = 2000;
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
                try
                {
                    _serialPort.Open();
                    break;
                }
                catch (Exception e)
                {
                    Debug.Log($"ポート{port}が接続できませんでした。{e}");
                }
            }


            _thread = new Thread(ReadData);
            _thread.Start();
        }

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
