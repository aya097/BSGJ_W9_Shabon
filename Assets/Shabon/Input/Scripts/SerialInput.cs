#nullable enable
using System.IO.Ports;
using UnityEngine;

namespace Shabon.Input
{
    public class SerialInput
    {
        public SerialInput()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Debug.Log("Found port: " + port);
            }
        }
    }
}
