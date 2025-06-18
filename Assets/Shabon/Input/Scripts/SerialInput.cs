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

        private const int PortNum = 2;
        private List<SerialPort?> _serialPort = new();
        private Thread _thread;
        private int _portItr = 0;

        private float _value0;
        private float _value1;
        private float _value2;
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


            _thread = new Thread(ReadData);
            _thread.Start();
        }

        private void ReadData()
        {
            while (true)
            {
                for (int i = 0; i < _serialPort.Count; i++)
                {
                    if (_serialPort[i] != null && _serialPort[i]!.IsOpen)
                    {
                        string message = _serialPort[i]!.ReadLine();
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
                                if (_value1 == 1f) Debug.Log("wil clap");
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
        }

        void IDisposable.Dispose()
        {
            _thread.Join();
            _serialPort[0]?.Dispose();
            _serialPort[1]?.Dispose();
            Debug.Log("Disposed!!");
        }

        ~SerialInput()
        {
            _thread.Join();
            _serialPort[0]?.Dispose();
            _serialPort[1]?.Dispose();
            Debug.Log("Disposed!!");
        }
    }
}
