using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component
{
    public delegate void EventHandler1(String host, String msg);
    public class NSBoardCast
    {

        static public Form form = null;
        static public event EventHandler1 DidResponedChanged = null;
        //public event EventHandler1 DidResponedChanged;


        static public void Send(String host, int port, String msg, long timeoutMS)
        {
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse("192.168.0.255"), port); 
            Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); 
            Server.EnableBroadcast = true;
            Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 100); //設定的時間內解除阻塞模式

            byte[] pushdata = new byte[1024]; //定義要送出的封包大小

            Thread t = new Thread(()=> {

                pushdata = Encoding.ASCII.GetBytes(msg); //把要送出的資料轉成byte型態
                Server.SendTo(pushdata, remoteIP); //送出的資料跟目的


                IPEndPoint IPEnd = new IPEndPoint(IPAddress.Any, 8888);
                EndPoint IP = (EndPoint)IPEnd;

                long old = NSTime.CurrentMillisecond();
                while (old+ timeoutMS > NSTime.CurrentMillisecond())
                {
                    try {
                        byte[] getdata = new byte[1024]; //要接收的封包大小

                        int recv = Server.ReceiveFrom(getdata, ref IP);
                        string input = Encoding.ASCII.GetString(getdata, 0, recv); //把接收的byte資料轉回string型態
                        Console.WriteLine("received: {0} from: {1}", input, IP.ToString());
                        if (DidResponedChanged != null)
                        {
                            if (form == null)
                            {
                                DidResponedChanged(IP.ToString(), input);
                            }else
                            {
                                form.Invoke(new Action(() => DidResponedChanged(IP.ToString(), input)));
                            }
                            
                        }

                    }
                    catch (SocketException e)
                    {

                    }
                    
                    Thread.Sleep(200); //每秒發送一次
                }
                Server.Close();
                Console.WriteLine("Send did terminal");
            });
            t.IsBackground = true;
            t.Start();
        }

        static public void Recv(int port, String msg, long timeoutMS)
        {
            IPEndPoint IPEnd = new IPEndPoint(IPAddress.Any, port);
            Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 100); //設定的時間內解除阻塞模式
            Client.Bind(IPEnd);
            EndPoint IP = (EndPoint)IPEnd; //我真的不知道為何一定要這行才能成功= =，誰能解釋一下

            Thread t = new Thread(()=> {
                long old = NSTime.CurrentMillisecond();
                while (old + timeoutMS > NSTime.CurrentMillisecond())
                {
                    try
                    {
                        byte[] getdata = new byte[1024]; //要接收的封包大小
                        int recv = Client.ReceiveFrom(getdata, ref IP); //把接收的封包放進getdata且傳回大小存入recv
                        string input = Encoding.ASCII.GetString(getdata, 0, recv); //把接收的byte資料轉回string型態
                        Console.WriteLine("received: {0} from: {1}", input, IP.ToString());
                        Client.SendTo(Encoding.ASCII.GetBytes(msg), recv, SocketFlags.None, IP); //將原資料送回去

                        if (form == null)
                        {
                            DidResponedChanged(IP.ToString(), input);
                        }
                        else
                        {
                            form.Invoke(new Action(() => DidResponedChanged(IP.ToString(), input)));
                        }
                    }
                    catch (SocketException e)
                    {

                    }
                   
                }
                Client.Close();
                Console.WriteLine("Recv did terminal");
            });
            t.IsBackground = true;
            t.Start();            
        }
    }
}
