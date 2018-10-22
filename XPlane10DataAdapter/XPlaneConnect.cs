using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace XPlane10DataAdapter
{
    public class XPlaneConnect : IXPlaneConnect, IDisposable
    {
        int xplanePort = 49009;
        string xplaneIp = "255.255.255.0";
        IPEndPoint xplaneAddr;
        UdpClient client;

        public XPlaneConnect()
        {
            xplaneIp = IPAddress.Loopback.ToString();
            xplanePort = 49009;
            xplaneAddr = new IPEndPoint(IPAddress.Parse(xplaneIp), xplanePort);
            client = new UdpClient(0);
        }

        public XPlaneConnect(string xpHost, int xpPort, int port)
        {
            xplaneIp = xpHost;
            xplanePort = xpPort;
            xplaneAddr = new IPEndPoint(IPAddress.Parse(xplaneIp), xplanePort);
            client = new UdpClient(port);
        }

        public byte[] ReadUdp()
        {
            return client.Receive(ref xplaneAddr);
        }

        public void SendUdp(byte[] buffer)
        {
            client.Send(buffer, buffer.Length, xplaneAddr);
        }

        public void PauseSim(bool pause)
        { 
             //                  S     I     M     U     LEN   VAL
            byte[] msg = { 0x53, 0x49, 0x4D, 0x55, 0x00, 0x00 };
            msg[5] = (byte)(pause == true ? 0x01 : 0x00);
            SendUdp(msg);
        }

        public void PauseSim(int pause) 
        {
            if(pause < 0 || pause> 2)
            {
                throw new ArgumentException("pause must be a value in the range [0, 2].");
            }   
            //            S     I     M     U     LEN   VAL
            byte[] msg = { 0x53, 0x49, 0x4D, 0x55, 0x00, 0x00 };
            msg[5] = (byte) pause;
            SendUdp(msg);
        }

        public float[] GetDREF(string dref) 
        {
            return GetDREFs(new string[] {dref
            })[0];
        }

        public List<float[]> GetDREFs(string[] drefs)
        {
            //Preconditions
            if(drefs == null || drefs.Length == 0)
            {
                throw new ArgumentException("drefs must be a valid array with at least one dref.");
            }
            if(drefs.Length > 255)
            {
                throw new ArgumentException("Can not request more than 255 DREFs at once.");
            }
            var drefBytes = new List<byte[]>();
            for(int i = 0; i<drefs.Length; ++i)
            {
                drefBytes.Add(Encoding.UTF8.GetBytes(drefs[i]));
                if(drefBytes[i].Length == 0)
                {
                    throw new ArgumentException("DREF " + i + " is an empty string!");
                }
                if(drefBytes[i].Length > 255)
                {
                    throw new ArgumentException("DREF " + i + " is too long (must be less than 255 bytes in UTF-8). Are you sure this is a valid DREF?");
                }
            }
            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("GETD"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.Add((byte)drefs.Length);
            foreach(var dref in drefBytes)
            {
                sendBytesVBuilder.Add((byte)dref.Length);
                sendBytesVBuilder.AddRange(dref);
            }
            SendUdp(sendBytesVBuilder.ToArray());
            Thread.Sleep(100);
            var data = ReadUdp();
            if(data.Length == 0)
            {
                throw new Exception("No response received.");
            }
            if(data.Length< 6)
            {
                throw new Exception("Response too short");
            }
            var result = new List<float[]>();
            int cur = 6;
            for(int j = 0; j < drefs.Length; ++j)
            {
                var nums = new float[data[cur++]];
                result.Add(nums);
                for(int k = 0; k < result[j].Length; ++k) //TODO: There must be a better way to do this
                {
                    result[j][k] = BitConverter.ToSingle(data, cur);
                    cur += 4;
                }
            }
            return result;
        }

        public void SendDREF(string dref, float value) 
        {
            SendDREF(dref, new float[] {value});
        }

        public void SendDREF(string dref, float[] value) 
        {
            SendDREFs(new string[] {dref
            }, new List<float[]> { value } );
        }

        public void SendDREFs(string[] drefs, List<float[]> values) 
        {
            //Preconditions
            if(drefs == null || drefs.Length == 0)
            {
                throw new ArgumentException(("drefs must be non-empty."));
            }
            if (values == null || values.Count != drefs.Length)
            {
                throw new ArgumentException("values must be of the same size as drefs.");
            }
      
            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("DREF"));
            sendBytesVBuilder.Add(0xFF);
            for(int i = 0; i<drefs.Length; ++i)
            {
                string dref = drefs[i];
                float[] value = values[i];
                if (dref == null)
                {
                    throw new ArgumentException("dref must be a valid string.");
                }
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentException("value must be non-null and should contain at least one value.");
                }
                //Convert drefs to bytes.
                byte[] drefBytes = Encoding.UTF8.GetBytes(dref); 
                if (drefBytes.Length == 0)
                {
                    throw new ArgumentException("DREF is an empty string!");
                }
                if (drefBytes.Length > 255)
                {
                    throw new ArgumentException("dref must be less than 255 bytes in UTF-8. Are you sure this is a valid dref?");
                }

                var byteBuffer = new List<byte>();

                for (int j = 0; j < value.Length; ++j)
                {
                    byteBuffer.AddRange(BitConverter.GetBytes(value[j]));
                }

                sendBytesVBuilder.Add((byte)drefBytes.Length);
                sendBytesVBuilder.AddRange(drefBytes);
                sendBytesVBuilder.Add((byte)value.Length);
                sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            }
            SendUdp(sendBytesVBuilder.ToArray());
        }

        public float[] GetCTRL(int ac = 0)
        {
            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("GETC"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.Add((byte)ac);
            SendUdp(sendBytesVBuilder.ToArray());
            Thread.Sleep(100);
            var data = ReadUdp();
            if(data.Length == 0)
            {
                throw new Exception("No response received.");
            }
            if(data.Length< 31)
            {
                throw new Exception("Response too short");
            }

            // Parse response
            float[] result = new float[7];
            result[0] = BitConverter.ToSingle(data, 5); 
            result[1] = BitConverter.ToSingle(data, 9);
            result[2] = BitConverter.ToSingle(data, 13);
            result[3] = BitConverter.ToSingle(data, 17);
            result[4] = BitConverter.ToSingle(data, 21);
            result[5] = BitConverter.ToSingle(data, 22);
            result[6] = BitConverter.ToSingle(data, 27);
            return result;
        }

        public void SendCTRL(float[] values)
        {
            SendCTRL(values, 0);
        }

        public void SendCTRL(float[] values, int ac) 
        {
            //Preconditions
            if(values == null)
            {
                throw new ArgumentException("ctrl must no be null.");
            }
            if(values.Length > 7)
            {
                throw new ArgumentException("ctrl must have 7 or fewer elements.");
            }
            if(ac < 0 || ac> 9)
            {
                throw new ArgumentException("ac must be non-negative and less than 9.");
            }
            //Pad command values and convert to bytes
            int i;

            var byteBuffer = new List<byte>();

            for(i = 0; i< 6; ++i)
            {
                if(i == 4)
                {
                    if(i >= values.Length)
                    {
                        byteBuffer.Add(0xFF);
                    }
                    else
                    {
                        byteBuffer.Add((byte)values[i]);
                    }
                }
                else if (i >= values.Length)
                {
                    byteBuffer.AddRange(BitConverter.GetBytes(-998.0f));
                }
                else
                {
                    byteBuffer.AddRange(BitConverter.GetBytes(values[i]));
                }
            }
            byteBuffer.Add((byte)ac);
            byteBuffer.AddRange(BitConverter.GetBytes(values.Length == 7 ? values[6] : -998.0f));

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("CTRL"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());
        }

        public float[] GetPOSI(int ac = 0) 
        {
            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("GETP"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.Add((byte)ac);
            SendUdp(sendBytesVBuilder.ToArray());
            Thread.Sleep(100);
            // Read response
            byte[] data = ReadUdp();
            if(data.Length == 0)
            {
                throw new Exception("No response received.");
            }
            if(data.Length < 34)
            {
                throw new Exception("Response too short");
            }
            // Parse response
            float[] result = new float[7];
            for(int i = 0; i< 7; ++i)
            {
                result[i] = BitConverter.ToSingle(data, 6 + 4 * i);
            }
            return result;
        }

        public void SendPOSI(float[] values) 
        {
            SendPOSI(values, 0);
        }

        public void SendPOSI(float[] values, int ac)
        {
            //Preconditions
            if(values == null)
            {
                throw new ArgumentException("posi must no be null.");
            }
            if(values.Length > 7)
            {
                throw new ArgumentException("posi must have 7 or fewer elements.");
            }
            if(ac< 0 || ac> 255)
            {
                throw new ArgumentException("ac must be between 0 and 255.");
            }

            //Pad command values and convert to bytes
            int i;
            var byteBuffer = new List<byte>();
            for(i = 0; i < values.Length; ++i)
            {
                byteBuffer.AddRange(BitConverter.GetBytes(values[i]));
            }
            for(; i< 7; ++i)
            {
                byteBuffer.AddRange(BitConverter.GetBytes(-998.0f));
            }

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("POSI"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());
        }

        public List<float[]> ReadData()
        {
            byte[] buffer = ReadUdp();
            int cur = 5;
            int len = buffer[cur++];
            var result = new List<float[]>();
            for (int i = 0; i<len; ++i)
            {
                result.Add(new float[9]);
                for(int j = 0; j < 9; ++j)
                {
                    result[i][j] = BitConverter.ToSingle(buffer, cur);
                    cur += 4;
                }
            }
            return result;
        }

        public void SendData(List<float[]> data)
        {
            //Preconditions
            if(data == null || data.Count == 0)
            {
                throw new ArgumentException("data must be a non-null, non-empty array.");
            }

            var byteBuffer = new List<byte>();

            for(int i = 0; i < data.Count; ++i)
            {
                int rowStart = 9 * 4 * i;
                float[] row = data[i];
                if(row.Length != 9)
                {
                    throw new ArgumentException("Rows must contain exactly 9 items. (Row " + i + ")");
                }
                byteBuffer.AddRange(BitConverter.GetBytes(0));
                for(int j = 1; j < row.Length; ++j)
                {
                    byteBuffer.AddRange(BitConverter.GetBytes(row[j]));
                }
            }

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("DATA"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());
        }

        public void SelectData(int[] rows)
        {
            //Preconditions
            if(rows == null || rows.Length == 0)
            {
                throw new ArgumentException("rows must be a non-null, non-empty array.");
            }

            var byteBuffer = new List<byte>();

            for(int i = 0; i < rows.Length; ++i)
            {
                byteBuffer.AddRange(BitConverter.GetBytes(rows[i]));
            }

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("DSEL"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());
        }

        public void SendText(string msg) 
        {
            SendText(msg, -1, -1);
        }

        public void SendText(string msg, int x, int y)
        {
            //Preconditions
            if(msg == null)
            {
                msg = "";
            }
            //Convert drefs to bytes.
            byte[] msgBytes = Encoding.UTF8.GetBytes("msg");
            if (msgBytes.Length > 255)
            {
                throw new ArgumentException("msg must be less than 255 bytes in UTF-8.");
            }

            var byteBuffer = new List<byte>();

            byteBuffer.AddRange(BitConverter.GetBytes(x));
            byteBuffer.AddRange(BitConverter.GetBytes(y));

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("TEXT"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            sendBytesVBuilder.Add((byte)msg.Length);
            sendBytesVBuilder.AddRange(msgBytes);
            SendUdp(sendBytesVBuilder.ToArray());

        }

        public void SendView(ViewType view)
        {

            var byteBuffer = new List<byte>();

            byteBuffer.AddRange(BitConverter.GetBytes((int)view));

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("VIEW"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());

        }

        public void SendWYPT(WayPointOp op, float[] points)
        {
            //Preconditions
            if(points.Length % 3 != 0)
            {
                throw new ArgumentException("points.length should be divisible by 3.");
            }
            if(points.Length / 3 > 255)
            {
                throw new ArgumentException("Too many points. Must be less than 256.");
            }

            var byteBuffer = new List<byte>();

            foreach(var f in points)
            {
                byteBuffer.AddRange(BitConverter.GetBytes(f));
            }

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("WYPT"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.Add((byte)op);
            sendBytesVBuilder.Add((byte)(points.Length / 3));
            sendBytesVBuilder.AddRange(byteBuffer.ToArray());
            SendUdp(sendBytesVBuilder.ToArray());

        }

        public void SetCONN(int port)
        {
            if(port< 0 || port >= 0xFFFF)
            {   
                throw new ArgumentException("Invalid port (must be non-negative and less than 65536).");
            }

            var sendBytesVBuilder = new List<byte>();
            sendBytesVBuilder.AddRange(Encoding.UTF8.GetBytes("CONN"));
            sendBytesVBuilder.Add(0xFF);
            sendBytesVBuilder.Add((byte)port);
            sendBytesVBuilder.Add((byte)(port >> 8));
            SendUdp(sendBytesVBuilder.ToArray());

            xplaneAddr = new IPEndPoint(IPAddress.Parse(xplaneIp), port);
            ReadUdp(); // Try to read response
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~XPlaneConnect() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
