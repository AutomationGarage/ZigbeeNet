using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZigBeeNet.Transport;
using Serilog;
using System.Collections.Concurrent;

namespace ZigBeeNet.Tranport.SerialPort
{
    public class ZigBeeSerialPort : IZigBeePort
    {
        BlockingCollection<byte> buffer;

        string portName;
        int baudRate;
        System.IO.Ports.SerialPort port;

        CancellationTokenSource cts;

        public static string[] Ports { get => System.IO.Ports.SerialPort.GetPortNames(); }

        public ZigBeeSerialPort(string portName, int baudRate = 115200)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            buffer = new BlockingCollection<byte>(new ConcurrentQueue<byte>());
        }

        public void Close()
        {
            port?.Close();
            port?.Dispose();
        }

        public bool Open()
        {
            try
            {
                return Open(baudRate);
            }
            catch (Exception e)
            {
                Log.Warning("Unable to open serial port: " + e.Message);
                return false;
            }
        }

        public bool Open(int baudRate)
        {
            cts = new CancellationTokenSource();
            this.baudRate = baudRate;

            bool success = false;

            Log.Debug("Opening port {Port} at {Baudrate} baud.", portName, baudRate);

            port = new System.IO.Ports.SerialPort(portName, baudRate);

            try
            {
                if (Environment.OSVersion.Platform.ToString().StartsWith("Win"))
                {
                    port.Open();
                }
                else
                {
                    if (File.Exists(portName))
                    {
                        port.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("{Exception} - Error opening port {Port}\n{Port}", ex.GetType().Name, portName, ex.Message);
            }

            if (port.IsOpen)
            {
                port.DiscardInBuffer();
                new Task(ReaderTask, cts.Token).Start();
                success = true;
            }

            return success;
        }

        public byte? Read()
        {
            return Read(9999999);
        }

        public byte? Read(int timeout)
        {
            try
            {
                /* This blocks until data available (Producer Consumer pattern) */
                if (buffer.TryTake(out byte value, timeout))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while reading byte from serial port");
                return null;
            }
        }

        public void Write(byte[] value)
        {
            if (port == null) { return; }

            if (port.IsOpen)
            {
                try
                {
                    port.Write(value, 0, value.Length);

                    Log.Debug("Write data to serialport: {Data}", BitConverter.ToString(value));
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error while writing to serial port");
                }
            }
        }

        void ReaderTask()
        {
            var message = new byte[512];

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var n = port.Read(message, 0, message.Length); // read may return anything from 0 - length , 0 = end of stream
                    if (n == 0) break;

                    for (int i = 0; i < n; i++)
                    {
                        buffer.Add(message[i]);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error while reading from serial port");
                    cts.Cancel();
                }
            }

            Close();
        }

        public void PurgeRxBuffer()
        {
            port?.DiscardInBuffer();
        }
    }
}
