using sensor_monitoring_backend.Domain.Entity;
using System.Net.Sockets;
using System.Net;
using sensor_monitoring_backend.Data;
using Microsoft.EntityFrameworkCore;
using sensor_monitoring_backend.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Threading;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Services
{
    public class TcpConnection
    {
        public TcpConnection(DatabaseContext context)
        {
            _context = context;
            //_hub = hub;
        }


        private readonly DatabaseContext _context;
        //private readonly SensorHub _hub;



        public async void ListenForConnection()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("185.97.119.141"); //For Server
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1"); //For Local Test

                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    TcpClient client = await server.AcceptTcpClientAsync();

                    Thread threadAssignToNewClient = new Thread(new ThreadStart(async () =>
                    {
                        await ProccessNewClient(client);
                    }));
                    threadAssignToNewClient.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }



        public async Task ProccessNewClient(TcpClient client)
        {
            Console.WriteLine("Connected!");

            Byte[] bytes = new Byte[256];
            String data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;

            try
            {
                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //Console.WriteLine("Received: {0}", data);

                    //await CheckSensorAddedToDatabase(data);
                    // Process the data sent by the client.
                    data = data.ToUpper();
                    await CheckDeviceAddedToDatabase(data, client);

                    Console.WriteLine("Sent: {0}", data);
                    client.Close();
                    Console.WriteLine("Connection After Send Close At End");
                    break;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error On Stream : {0}", e);
            }

            // Shutdown and end connection
            client.Close();
            Console.WriteLine("Connection Close At End");
        }




        public async Task CheckDeviceAddedToDatabase(string rawData, TcpClient client)
        {
            string[] splitedStringWithSharp = rawData.Split("#");
            string[] splitedStringWithSharpFirstSectionSplitedWithComma = splitedStringWithSharp[0].Split(",");
            int lenghtStringPhoneNumber = splitedStringWithSharpFirstSectionSplitedWithComma[1].Length;
            string phoneNumber = "";
            if (splitedStringWithSharpFirstSectionSplitedWithComma[1].StartsWith("0"))
            {
                phoneNumber = splitedStringWithSharpFirstSectionSplitedWithComma[1];
            }
            else
            {
                phoneNumber = "0" + splitedStringWithSharpFirstSectionSplitedWithComma[1].Substring(2, lenghtStringPhoneNumber - 2);
            }
            IEnumerable<Device> phonenumberFilteredDevices = _context.Devices
                .Where(device => device.PhoneNumber == phoneNumber)
                .Include(current => current.Sensors)
                .ThenInclude(current => current.PacketDecoders);
            if (phonenumberFilteredDevices.Count() == 0)
            {
                client.Close();
                Console.WriteLine("Connection Close At End");
            }
            else
            {
                await AddDeterminedValuesForSensor(rawData, phonenumberFilteredDevices.First());
            }

        }

        public async Task AddDeterminedValuesForSensor(string rawData, Device device)
        {
            string[] splitedStringWithSharp = rawData.Split("#");
            try
            {
                for (int i = 1; i < splitedStringWithSharp.Length; i++)
                {
                    try {
                        string portSensor = splitedStringWithSharp[i].Substring(1, 1);
                        Int32 year = Int32.Parse("20" + splitedStringWithSharp[i].Substring(2, 2));
                        Int32 month = Int32.Parse(splitedStringWithSharp[i].Substring(4, 2));
                        Int32 day = Int32.Parse(splitedStringWithSharp[i].Substring(6, 2));
                        Int32 hour = Int32.Parse(splitedStringWithSharp[i].Substring(8, 2));
                        Int32 minute = Int32.Parse(splitedStringWithSharp[i].Substring(10, 2));
                        Int32 second = Int32.Parse(splitedStringWithSharp[i].Substring(12, 2));
                        DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                        Sensor? sensor = device.Sensors.Where(item => item.PortName.ToString() == portSensor).FirstOrDefault();
                        if (sensor != null)
                        {
                            foreach (PacketDecoder packetDecoder in sensor.PacketDecoders)
                            {
                                List<string> stringList = CreateListStringFromMainFrame(2, splitedStringWithSharp[i]);
                                string hexString = CreateHexString(packetDecoder.ByteNumbers, packetDecoder.StartByte, stringList);
                                double value = 0;
                                switch (packetDecoder.KindProperty)
                                {
                                    case Numerical.INT16:
                                        break;
                                    case Numerical.INT32:
                                        value = ConvertHexToInt32(hexString);
                                        break;
                                    case Numerical.INT64:
                                        break;
                                    case Numerical.DOUBLE:
                                        value = ConvertHexToDouble(hexString);
                                        break;
                                    case Numerical.FLOAT:
                                        value = ConvertHexToFloat(hexString);
                                        break;
                                    default:
                                        break;
                                }


                                DeterminedValue determineValue = new DeterminedValue()
                                {
                                    DateTimeDetermined = dateTime.ToUniversalTime(),
                                    PacketDecoderId = packetDecoder.Id,
                                    PacketDecoder = packetDecoder,
                                    Value = value
                                };
                                packetDecoder.DeterminedValues.Add(determineValue);
                                await _context.DeterminedValues.AddAsync(determineValue);
                            }
                        }

                        //await _hub.Clients.All.SendAsync("ReciveDeterminedValue", determineValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("One Packet Has Problem :{0}",e);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine("For On Packets Has Problem : {0}",e);
            }
           
        }



        public List<string> CreateListStringFromMainFrame(int lengthString, string mainFrame)
        {
            List<string> frameSplitedList = new List<string>();
            for (int i = 0; i < mainFrame.Length; i = i + lengthString)
            {
                if (i + 1 == mainFrame.Length)
                {
                    frameSplitedList.Add(mainFrame[i].ToString());
                }
                else
                {
                    frameSplitedList.Add(mainFrame[i].ToString() + mainFrame[i + 1].ToString());
                }
            }
            return frameSplitedList;
        }


        public string CreateHexString(List<Double> byteNumbers,int startNumber, List<string> frameSplitedString)
        {
            string hexString = "";
            if (byteNumbers.Contains(1001))
            {
                if (byteNumbers[0] % 1 != 0)
                {
                    hexString = hexString + frameSplitedString[Convert.ToInt32(Math.Ceiling(byteNumbers[0])) + startNumber - 1][1];
                    for (int index = Convert.ToInt32(Math.Ceiling(byteNumbers[0])) + 1; index < frameSplitedString.Count - startNumber - 1; index++)
                    {
                        hexString = hexString + frameSplitedString[index + startNumber - 1];
                    }
                }
                else
                {
                    for (int index = startNumber - 1 + Convert.ToInt32(byteNumbers[0]); index < frameSplitedString.Count; index++)
                    {
                        hexString = hexString + frameSplitedString[index];
                    }
                }
            }
            else
            {
                for (int index = 0; index < byteNumbers.Count; index++)
                {
                    if (byteNumbers[index] % 1 != 0)
                    {
                        if (index == 0)
                        {
                            hexString = hexString + frameSplitedString[Convert.ToInt32(Math.Floor(byteNumbers[index])) + startNumber - 1][1];
                        }
                        else if (index == byteNumbers.Count - 1)
                        {
                            hexString = hexString + frameSplitedString[Convert.ToInt32(Math.Ceiling(byteNumbers[index])) + startNumber - 1][0];
                        }
                    }
                    else
                    {
                        hexString = hexString + frameSplitedString[Convert.ToInt32(byteNumbers[index]) + startNumber - 1 ];
                    }
                }
            }
            return hexString;
        }

        public float ConvertHexToFloat(string hexString)
        {
            uint num = uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] floatValues = BitConverter.GetBytes(num);
            float deteminedValue = BitConverter.ToSingle(floatValues, 0);
            return deteminedValue;
        }

        public Int32 ConvertHexToInt32(string hexString)
        {
            return Convert.ToInt32(hexString, 16);
        }

        public Double ConvertHexToDouble(string hexString)
        {
            return Convert.ToDouble(hexString);
        }
    }
}
