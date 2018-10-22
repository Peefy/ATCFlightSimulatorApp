using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using ATCFlightSimulatorApp.Utils;
using ATCFlightSimulatorApp.Utils.JsonModels;

namespace ATCFlightSimulatorApp.Services
{
    public class ATCFlightClient : IATCFlightClient, IDisposable
    {
        TcpClient _client;
        NetworkStream _stream;
        CommunicationConfig _config;

        string _serverIp = "192.168.0.130";
        int _serverPort = 12000;
        bool _isConnect;

        public ATCFlightClient()
        {
            _config = JsonFileConfig.Instance.CommunicationConfig;
            _serverIp = _config.HostIp;
            _serverPort = _config.HostPort;
            //_client = new TcpClient(_serverIp, _serverPort);
        }

        public void SendTo(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }

        public byte[] Recieve()
        {
            var data = new byte[65535];
            var bytes = _stream.Read(data, 0, data.Length);
            var rdata = new byte[bytes];
            Array.Copy(rdata, data, bytes);
            return rdata;
        }

        public bool Connect()
        {
            _client = new TcpClient(_serverIp, _serverPort);
            _stream = _client.GetStream();
            _isConnect = true;
            return _isConnect;
        }

        public bool DisConnect()
        {
            _stream?.Close();
            _client?.Close();
            return true;
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

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ATCFlightClient() {
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
