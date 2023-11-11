using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JsonDTO
{
    public class Network
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }
        public string ip { get; set; }
        public int port { get; set; }

        public Network(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            Connect();
        }

        public bool Connected => Client.Connected;

        public void Connect(string ip, int port)
        {
            Disconnect();
            Client = new TcpClient(ip, port);
            Stream = Client.GetStream();
            Reader = new BinaryReader(Stream);
            Writer = new BinaryWriter(Stream);
        }

        public void Connect() => Connect(ip, port);

        public void Disconnect()
        {
            Client?.Close();
            Stream?.Close();
            Reader?.Close();
            Writer?.Close();
        }

        public void Send(object MessageObject)
        {
            Writer.Write(NetworkFactory.Serialize(MessageObject));
        }

        public T Request<T>(object MessageObject)
        {
            Send(MessageObject);
            return Read<T>();

        }

        public T Read<T>()
        {
            return NetworkFactory.Deserialize<T>(Reader.ReadString());
        }
        
        public object Request(object MessageObject)
        {
            Send(MessageObject);
            return Read();
        }
        public object Read()
        {
            return NetworkFactory.DeserializeAuto(Reader.ReadString());
        }
        
    }
}
