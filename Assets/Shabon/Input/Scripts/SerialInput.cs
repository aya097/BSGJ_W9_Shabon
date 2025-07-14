#nullable enable
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Shabon.Input
{
    public class SerialInput : IDisposable
    {
        public float Value0 => _value0;
        public float Value1 => _value1;
        public float Value2 => _value2;
        public float Value3 => _value3;


        private const int PortNum = 2;
        private List<SerialPort?> _serialPort = new();
        private Thread _thread0, _thread1;
        private bool _isRunning;
        private int _portItr = 0;

        private float _value0;
        private float _value1;
        private float _value2;
        private float _value3;
        public SerialInput()
        {
#if UNITY_WEBGL
            return;
#endif
            // 接続可能なport名を取得
            string[] ports = SerialPort.GetPortNames();
            var availablePorts = ports.Where(s => s.Contains("COM") || s.Contains("usbmodem"));
            foreach (string port in availablePorts)
            {
                Debug.Log("Found port: " + port);
            }

            _portItr = 0;
            foreach (string port in availablePorts)
            {
                _serialPort.Add(new SerialPort(port, 115200, Parity.None, 8, StopBits.One));
                _serialPort[_portItr]!.ReadTimeout = 2000;
                _serialPort[_portItr]!.DtrEnable = true;
                _serialPort[_portItr]!.RtsEnable = true;
                try
                {
                    _serialPort[_portItr]!.Open();
                    _portItr++;
                    if (_portItr >= PortNum) break;
                }
                catch (Exception e)
                {
                    Debug.Log($"ポート{port}が接続できませんでした。{e}");
                }
            }

            _isRunning = true;
            _thread0 = new Thread(ReadData0);
            _thread1 = new Thread(ReadData1);
            _thread0.Start();
            _thread1.Start();
        }
        private void ReadData0()
        {
            ReadData(0);
        }

        private void ReadData1()
        {
            ReadData(1);
        }
        private void ReadData(int num)
        {
            while (_isRunning)
            {
                if (num >= _serialPort.Count) return;

                if (_serialPort[num] != null && _serialPort[num]!.IsOpen)
                {
                    string message = _serialPort[num]!.ReadLine();
                    // , で区切られている
                    string[] values = message.Split(',');

                    if (values[0] == "Handle")
                    {
                        if (float.TryParse(values[1], out float v0))
                        {
                            _value0 = v0;
                        }
                        if (float.TryParse(values[2], out float v1))
                        {
                            _value1 = v1;
                        }
                        if (float.TryParse(values[3], out float v3))
                        {
                            _value3 = v3;
                        }
                    }
                    else if (values[0] == "Angle")
                    {
                        if (float.TryParse(values[1], out float v2))
                        {
                            _value2 = v2;
                        }
                    }
                }
            }
        }

        void IDisposable.Dispose()
        {
#if UNITY_WEBGL
            return;
#endif
            _isRunning = false;
            _thread0.Join();
            _thread1.Join();
            for (int i = 0; i < _serialPort.Count; i++)
            {
                _serialPort[i]?.Dispose();
            }
        }
    }
}
