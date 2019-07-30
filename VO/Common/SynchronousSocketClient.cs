using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using WxPay2017.API.VO;
using System.Configuration;

namespace WxPay2017.API.VO.Common
{
    public class SynchronousSocketClient
    {
        public bool StartClient(SocketData socketData)
        {
            bool IsDone = false;

            if (socketData.PhyAddress == null)
                socketData.PhyAddress = "";
            if (socketData.IpAddress == null)
                socketData.IpAddress = "";
            //中转协议
            XDocument xdoc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("root",
                    new XElement("common",
                    new XElement("building_id", socketData.BuildingId),
                    new XElement("gateway_id", socketData.GatewayId),
                    new XElement("type", socketData.Type)
                    ),
                    new XElement("supcon",
                    //new XAttribute("operation", socketData.Type),
                    //new XElement("port", socketData.Port),
                    new XElement("meterid", socketData.MeterId),
                    new XElement("mac", socketData.PhyAddress),
                    new XElement("ipAddress", socketData.IpAddress),
                    new XElement("gbCode",socketData.GbCode)
                    //new XElement("value", socketData.Value)
                    )
                )
            );

            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                //IPHostEntry ipHostInfo = Dns.GetHostEntry((ConfigurationManager.AppSettings["Remote:IpAddress"]));
                //IPAddress ipAddress = ipHostInfo.AddressList[2];
                //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8888);
                IPAddress ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings["Remote:IpAddress"]);//获取远程服务器Ip
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Int32.Parse(ConfigurationManager.AppSettings["Remote:Port"]));//获取远程服务器端口




                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    byte[] msg = Encoding.ASCII.GetBytes(xdoc.ToString());//+ "<EOF>"   去掉结束标识


                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    //int bytesRec = sender.Receive(bytes);
                    //Console.WriteLine("Echoed test = {0}",
                    //    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    IsDone = true;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return IsDone;
        }
    }
}
