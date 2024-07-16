using ArrayFunctionsNamespace;
using BitsFunctionsNamespace;
using FileFunctionsNamespace;
using MathFunctionsNamespace;
using NetworkFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;


namespace NetworkFunctionsNamespace
{
	public enum MY_TCP_CMD
	{
		DO_NOTHING,
		FILE_START,
		FILE_END,
		DATA
	}
	class CRC8Test
	{
		
		public void ChecksumToConsole(byte[] byte_arr)
		{
			byte crc8_value = CRC8Compute.ComputeChecksum(byte_arr);
			Console.WriteLine(crc8_value.ToString());
		}
	}
	/// <summary>
	/// Written. 2024.04.19 12:26. Warsaw. Workplace. <br></br>
	/// 
	/// </summary>
	public static class CRC8Compute
	{
		/// <summary>
		/// Written. 2024.04.19 12:55. Warsaw. Workplace.
		/// Tested. Works. CRC-8/DVB-S2, Poly = 0xD5. 2024.04.19 12:56. Warsaw. Workplace.
		/// </summary>
		public static byte[] CRC8Table = new byte[256]
		{
			0x00, 0xD5, 0x7F, 0xAA, 0xFE, 0x2B, 0x81, 0x54,
			0x29, 0xFC, 0x56, 0x83, 0xD7, 0x02, 0xA8, 0x7D,
			0x52, 0x87, 0x2D, 0xF8, 0xAC, 0x79, 0xD3, 0x06,
			0x7B, 0xAE, 0x04, 0xD1, 0x85, 0x50, 0xFA, 0x2F,
			0xA4, 0x71, 0xDB, 0x0E, 0x5A, 0x8F, 0x25, 0xF0,
			0x8D, 0x58, 0xF2, 0x27, 0x73, 0xA6, 0x0C, 0xD9,
			0xF6, 0x23, 0x89, 0x5C, 0x08, 0xDD, 0x77, 0xA2,
			0xDF, 0x0A, 0xA0, 0x75, 0x21, 0xF4, 0x5E, 0x8B,
			0x9D, 0x48, 0xE2, 0x37, 0x63, 0xB6, 0x1C, 0xC9,
			0xB4, 0x61, 0xCB, 0x1E, 0x4A, 0x9F, 0x35, 0xE0,
			0xCF, 0x1A, 0xB0, 0x65, 0x31, 0xE4, 0x4E, 0x9B,
			0xE6, 0x33, 0x99, 0x4C, 0x18, 0xCD, 0x67, 0xB2,
			0x39, 0xEC, 0x46, 0x93, 0xC7, 0x12, 0xB8, 0x6D,
			0x10, 0xC5, 0x6F, 0xBA, 0xEE, 0x3B, 0x91, 0x44,
			0x6B, 0xBE, 0x14, 0xC1, 0x95, 0x40, 0xEA, 0x3F,
			0x42, 0x97, 0x3D, 0xE8, 0xBC, 0x69, 0xC3, 0x16,
			0xEF, 0x3A, 0x90, 0x45, 0x11, 0xC4, 0x6E, 0xBB,
			0xC6, 0x13, 0xB9, 0x6C, 0x38, 0xED, 0x47, 0x92,
			0xBD, 0x68, 0xC2, 0x17, 0x43, 0x96, 0x3C, 0xE9,
			0x94, 0x41, 0xEB, 0x3E, 0x6A, 0xBF, 0x15, 0xC0,
			0x4B, 0x9E, 0x34, 0xE1, 0xB5, 0x60, 0xCA, 0x1F,
			0x62, 0xB7, 0x1D, 0xC8, 0x9C, 0x49, 0xE3, 0x36,
			0x19, 0xCC, 0x66, 0xB3, 0xE7, 0x32, 0x98, 0x4D,
			0x30, 0xE5, 0x4F, 0x9A, 0xCE, 0x1B, 0xB1, 0x64,
			0x72, 0xA7, 0x0D, 0xD8, 0x8C, 0x59, 0xF3, 0x26,
			0x5B, 0x8E, 0x24, 0xF1, 0xA5, 0x70, 0xDA, 0x0F,
			0x20, 0xF5, 0x5F, 0x8A, 0xDE, 0x0B, 0xA1, 0x74,
			0x09, 0xDC, 0x76, 0xA3, 0xF7, 0x22, 0x88, 0x5D,
			0xD6, 0x03, 0xA9, 0x7C, 0x28, 0xFD, 0x57, 0x82,
			0xFF, 0x2A, 0x80, 0x55, 0x01, 0xD4, 0x7E, 0xAB,
			0x84, 0x51, 0xFB, 0x2E, 0x7A, 0xAF, 0x05, 0xD0,
			0xAD, 0x78, 0xD2, 0x07, 0x53, 0x86, 0x2C, 0xF9
		};
		/// <summary>
		/// Written. 2024.04.19 13:00. Warsaw. Workplace.
		/// </summary>
		/// <param name="arr_in"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte ComputeChecksum(byte[] arr_in, int length)
		{
			byte crc_sum = 0;
			for (byte i = 0; i < length; i++)
			{
				crc_sum = CRC8Table[crc_sum ^ arr_in[i]];
			}
			return crc_sum;
		}
		/// <summary>
		/// Written. 2024.04.19 12:30. Warsaw. Workplace. <br></br>
		/// Tested. Works. CRC-8/DVB-S2, Poly = 0xD5. 2024.04.19 12:56. Warsaw. Workplace.
		/// </summary>
		/// <param name="arr_in"></param>
		/// <returns></returns>
		public static byte ComputeChecksum(byte[] arr_in)
		{
			return ComputeChecksum(arr_in, arr_in.Length);
		}
	}
	/// <summary>
	/// Calculates CRC16 of byte[].
	/// Written. 2024.03.22 15:44. Warsaw. Workplace. <br></br>
	/// Tested. Works. 2024.03.22 15:44. Warsaw. Workplace.
	/// </summary>
	class CRC16Class
	{
		public const UInt16 DefaultPolynomial = 0x8005;
		UInt16 internal_polynomial = DefaultPolynomial;
		public UInt16 Polynomial
		{
			get
			{
				return internal_polynomial;
			}
			set
			{
				internal_polynomial = value;
				CreateCRCReflectedTable(internal_polynomial);
			}
		}
		public UInt16[] CRC16_Reflected_Table = null;
		public CRC16Class(UInt16 polynomial_in)
		{
			CRC16_Reflected_Table = CreateCRCReflectedTable(polynomial_in);
		}
		/// <summary>
		/// Calculates reflect CRC16 table. <br></br>
		/// Tested. Works. 2024.03.22 15:33. Warsaw. Workplace.
		/// </summary>
		/// <param name="polynomial"></param>
		/// <returns></returns>
		private UInt16[] CreateCRCReflectedTable(UInt16 polynomial)
		{
			UInt16[] createTable = new UInt16[256];
			// 2024.03.21 16:42. Warsaw. Workplace.
			// for using shift right there is reversed polynomial - that is reversed bits in the number (1st, 2nd -> 32nd, 31st)
			UInt16 polynomial_reversed = (UInt16)BitsFunctions.BitsReversed(polynomial);
			for (UInt16 i = 0; i < 256; i++)
			{
				UInt16 entry = i;
				for (UInt16 j = 0; j < 8; j++)
				{
					if ((entry & 1) == 1)
					{
						entry = (UInt16)((entry >> 1) ^ polynomial_reversed);
					}
					else
					{
						entry = (UInt16)(entry >> 1);
					}
				}
				createTable[i] = entry;
			}
			return createTable;
		}
		/// <summary>
		/// Calculates CRC16.<br></br>
		/// Written. 2024.03.22 15:34. Warsaw. Workplace. <br></br>
		/// Tested. Works. 2024.03.22 15:45. Warsaw. Workplace.
		/// </summary>
		/// <param name="byte_arr"></param>
		/// <returns></returns>
		public UInt16 ComputeChecksum(byte[] byte_arr)
		{
			// 2024.03.22 10:31. Warsaw. Workplace.
			// Uses reflected CRC16 table. 
			ushort crc = 0xffff;
			foreach (byte t in byte_arr)
			{
				var index = (byte)((crc & 0xff) ^ t);
				crc = (ushort)((crc >> 8) ^ CRC16_Reflected_Table[index]);
			}
			return (UInt16)crc;
		}
		public byte[] ComputeChecksumBytes(byte[] byte_arr)
		{
			return BitConverter.GetBytes(ComputeChecksum(byte_arr));
		}
	}
	/// <summary>
	/// Calculates CRC32 of byte[].
	/// Written. 2024.02.18 11:44. Warsaw. Hostel. <br></br>
	/// Tested. Works. 2024.03.22 10:31. Warsaw. Workplace.
	/// </summary>
	class CRC32Class
	{
		// 2024.02.18 12:07. Warsaw. Hostel.
		// sum is can be ok checksum. crs32 is longer to compute.
		/*
         * CRC8:
public static class Crc8
{
    private static readonly byte[] Table = new byte[256];
    private const byte Poly = 0xd5;
    public static byte ComputeChecksum(params byte[] bytes)
    {
        byte crc = 0;
        if (bytes is {Length: > 0}) crc = bytes.Aggregate(crc, (current, b) => Table[current ^ b]);
        return crc;
    }
    static Crc8()
    {
        for (var i = 0; i < 256; ++i)
        {
            var temp = i;
            for (var j = 0; j < 8; ++j)
                if ((temp & 0x80) != 0)
                    temp = (temp << 1) ^ Poly;
                else
                    temp <<= 1;
            Table[i] = (byte) temp;
        }
    }
}
*/
		// 2024.02.18 12:19. Warsaw. Hostel. CRC16 needs to be found
		public const UInt32 DefaultPolynomial = 0x04C11DB7;
		UInt32 internal_polynomial = DefaultPolynomial;
		public UInt32 Polynomial
		{
			get
			{
				return internal_polynomial;
			}
			set
			{
				internal_polynomial = value;
				CreateCRCReflectedTable(internal_polynomial);
			}
		}
		public UInt32[] CRS32_Reflected_Table = null;
		public CRC32Class(UInt32 polynomial_in)
		{
			CRS32_Reflected_Table = CreateCRCReflectedTable(polynomial_in);
		}
		private UInt32[] CreateCRCReflectedTable(UInt32 polynomial)
		{
			UInt32[] createTable = new UInt32[256];
			// 2024.03.21 16:42. Warsaw. Workplace.
			// for using shift right there is reversed polynomial - that is reversed bits in the number (1st, 2nd -> 32nd, 31st)
			UInt32 polynomial_reversed = (uint)BitsFunctions.BitsReversed((int)polynomial);
			for (UInt32 i = 0; i < 256; i++)
			{
				UInt32 entry = i;
				for (UInt32 j = 0; j < 8; j++)
				{
					if ((entry & 1) == 1)
					{
						entry = (entry >> 1) ^ polynomial_reversed;
					}
					else
					{
						entry = entry >> 1;
					}
				}
				createTable[i] = entry;
			}
			return createTable;
		}
		/// <summary>
		/// Calculates CRC32.<br></br>
		/// Written. 2024.03.21. 10:00 - 15:00. Warsaw. Workplace. <br></br>
		/// Tested. Works. Comparison to 2 websites was used to check the result. 2024.03.22 10:20. Warsaw. Workplace.
		/// </summary>
		/// <param name="byte_arr"></param>
		/// <returns></returns>
		public UInt32 ComputeChecksum(byte[] byte_arr)
		{
			// 2024.03.22 10:31. Warsaw. Workplace.
			// Uses reflected CRC32 table. 
			var crc = 0xffffffff;
			foreach (var t in byte_arr)
			{
				var index = (byte)((crc & 0xff) ^ t);
				crc = (crc >> 8) ^ CRS32_Reflected_Table[index];
			}
			return ~crc;
		}
		public byte[] ComputeChecksumBytes(byte[] byte_arr)
		{
			return BitConverter.GetBytes(ComputeChecksum(byte_arr));
		}
	}
	/// <summary>
	/// Written. 2024.03.22 15:43. Warsaw. Workplace. <br></br>
	/// Tested. Works. <br></br>
	/// - Shows in console CRC32 reflected table. <br></br>
	/// - Shows in console CRC32 checksum of provided byte[]. <br></br>
	/// </summary>
	class CRC16Test
	{
		CRC16Class crc16 = new CRC16Class(CRC16Class.DefaultPolynomial);
		public void ShowTable()
		{
			Console.Write(ArrayFunctions.UInt16Array.Convert.ToFileString(crc16.CRC16_Reflected_Table, 8, 16, " "));
		}
		public void SetPolynomial(ushort num_in)
		{
			crc16.Polynomial = num_in;
		}
		public void ChecksumToConsole(byte[] byte_arr)
		{
			ushort crc16_value = crc16.ComputeChecksum(byte_arr);
			Console.WriteLine(crc16_value.ToString());
		}
	}
	/// <summary>
	/// Written. 2024.03.22 13:03. Warsaw. Workplace. <br></br>
	/// Tested. Works. 2024.03.22 13:22. Warsaw. Workplace. <br></br>
	/// - Shows in console CRC32 reflected table. <br></br>
	/// - Shows in console CRC32 checksum of provided byte[]. <br></br>
	/// </summary>
	class CRC32Test
	{
		CRC32Class crc32 = new CRC32Class(CRC32Class.DefaultPolynomial);
		public void ShowTable()
		{
			Console.Write(ArrayFunctions.UInt32Array.Convert.ToFileString(crc32.CRS32_Reflected_Table, 8, 16, " "));
		}
		/// <summary>
		/// Tested. Works. 2024.03.22 13:21. Warsaw. Workplace.
		/// </summary>
		/// <param name="num_in"></param>
		public void SetPolynomial(uint num_in)
		{
			crc32.Polynomial = num_in;
		}
		public void ChecksumToConsole(byte[] byte_arr)
		{
			uint crc32_value = crc32.ComputeChecksum(byte_arr);
			Console.WriteLine(crc32_value.ToString());
		}
	}
	/// <summary>
	/// Written. 2024.06.13 20:50. Gdansk. Home 
	/// </summary>
	public static class TCPClientFunctions
	{

		/// <summary>
		/// Written. 2024.06.13 20:50. Gdansk. Home 
		/// </summary>
		/// <param name="ip_address"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public static TcpClient Create(string ip_address, Int32 port)
		{
			IPAddress server_address = IPAddress.Parse(ip_address);
			IPEndPoint server_endpoint = new IPEndPoint(server_address, port);
			TcpClient client_tcp = new TcpClient(server_endpoint);
			return client_tcp;
		}




		/*
		TcpClient ClientTCP = null;
		IPAddress ServerAddress = null;
		public byte[] PreparePacket(byte[] data_in, MY_TCP_CMD cmd_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
		{
			byte[] arr_out = new byte[0];
			byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
			byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
			byte[] cmd_bytes = new byte[1];
			cmd_bytes[0] = (byte)cmd_in;
			header_bytes = ArrayFunctions.ByteArray.MergeArrays(header_bytes, cmd_bytes);
			arr_out = ArrayFunctions.ByteArray.MergeArrays(header_bytes, data_in);
			arr_out = ArrayFunctions.ByteArray.MergeArrays(data_in, end_of_packet_bytes);
			return arr_out;
		}
		public void ServerConnect(string ip_address, Int32 port)
		{
			ServerAddress = IPAddress.Parse(ip_address);
			ClientTCP = new TcpClient();
			ClientTCP.Connect(ServerAddress, port);
			while (ClientTCP.Connected == false)
			{
				Thread.Sleep(100);
				Console.WriteLine("Waiting server");
			}
		}
		public void SendBytes(byte[] bytes_in)
		{
			ClientTCP.Client.Send(bytes_in);
		}
		*/
	}
	public static class TCPServerFunctions
	{
		// 2024.06.13 20:36. Gdansk. Home 
		// TCPClient is ok. TCPSocket can be accessed from there.
		// TCPSocket allows to get/set more if it is needed.



		/// <summary>
		/// Checks if there is incoming connection to the server. <br></br>
		/// Written. 2024.06.13 20:36. Gdansk. Home 
		/// </summary>
		/// <param name="server"></param>
		/// <returns>
		/// true - there is connection waiting. <br></br>
		/// false - there is no incoming connection.
		/// </returns>
		public static bool IsConnection(TcpListener server)
		{
			bool check_return = false;
			if (server.Pending() == true)
			{
				check_return = true;
			}
			return check_return;
		}
		


			/// <summary>
			/// Written. 2024.06.11 14:41. Warsaw. Workplace.
			/// not tested.
			/// </summary>
			/// <param name="ip_address">Format: "192.168.1.50"</param>
			/// <param name="port"></param>
			/// <returns></returns>
			public static TcpListener Greate(string ip_address, int port)
		{
			TcpListener tcp_server;
			IPAddress address = IPAddress.Parse(ip_address);
			byte[] addr = address.GetAddressBytes();
			tcp_server = new TcpListener(address, port);
			return tcp_server;
		}
		/// <summary>
		/// Written. 2024.06.11 14:40. Warsaw. Workplace.
		/// not tested.
		/// </summary>
		/// <param name="tcp_server"></param>
		/// <returns></returns>
		public static bool Start(ref TcpListener tcp_server)
		{
			bool return_value = false;
			// 2024.06.11 14:40. Warsaw. Workplace.
			// There will be expection if the IP is wrong.
			tcp_server.Start();
			if (tcp_server.Server.Connected == true)
			{
				return_value = true;
			}
			return return_value;
		}
		public static TcpListener ServerTCP = null;
		public static IPAddress ServerAddress = null;
		public static TcpClient ServerTcpClient = null;
		public static byte[] PreparePacket(byte[] data_in, MY_TCP_CMD cmd_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
		{
			byte[] arr_out = new byte[0];
			byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
			byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
			byte[] cmd_bytes = new byte[1];
			cmd_bytes[0] = (byte)cmd_in;
			header_bytes = ArrayFunctions.ByteArray.MergeArrays(header_bytes, cmd_bytes);
			arr_out = ArrayFunctions.ByteArray.MergeArrays(header_bytes, data_in);
			arr_out = ArrayFunctions.ByteArray.MergeArrays(data_in, end_of_packet_bytes);
			return arr_out;
		}
		public static bool CheckPacket(byte[] packet_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
		{
			byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
			Int32 header_num = ArrayFunctions.ByteArray.IndexOf(packet_in, header_bytes);
			byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
			Int32 end_of_packet_num = ArrayFunctions.ByteArray.IndexOf(packet_in, end_of_packet_bytes);
			bool bool_out = false;
			if ((header_num != -1) && (end_of_packet_num != -1))
			{
				bool_out = true;
			}
			return bool_out;
		}
		public static MY_TCP_CMD CommandGet(byte[] packet_in)
		{
			return (MY_TCP_CMD)packet_in[0];
		}
		public static byte[] DataFromPacket(byte[] packet_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
		{
			byte[] arr_out = new byte[0];
			byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
			byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
			Int32 header_index = ArrayFunctions.ByteArray.LastIndexOf(packet_in, header_bytes);
			Int32 end_of_packet_index = ArrayFunctions.ByteArray.IndexOf(packet_in, end_of_packet_bytes);
			arr_out = ArrayFunctions.ByteArray.ExtractArray.BelowIndex(packet_in, header_index);
			arr_out = ArrayFunctions.ByteArray.ExtractArray.AboveIndex(arr_out, end_of_packet_index);
			return arr_out;
		}
		public static void ServerStart(string ip_address, Int32 port, Int32 buffer_size = 0x3FFFF)
		{
			ServerAddress = IPAddress.Parse(ip_address);
			ServerTCP = new TcpListener(ServerAddress, port);
			ServerTCP.Server.ReceiveBufferSize = buffer_size;
			ServerTCP.Server.SendBufferSize = buffer_size;
			Console.WriteLine("Server started");
			Console.WriteLine(ServerAddress.ToString() + " " + port.ToString());
			ServerTCP.Start();
		}
		public static void AcceptClient()
		{
			Int32 msg_cnt = 0;
			const Int32 msg_cnt_max = 10;
			//System.Windows.Input.Key key_in = Key.Space;
			bool key_pressed = false;
			//while ((ServerTCP.Pending() == false) && (key_pressed == false))
			//{
			//    Thread.Sleep(100);
			//    msg_cnt++;
			//    if (msg_cnt == msg_cnt_max)
			//    {
			//        msg_cnt = 0;
			//        Console.WriteLine("Press" + " " + key_in.ToString() + " to return to menu");
			//    }
			//    if (Keyboard.IsKeyDown(System.Windows.Input.Key.Space) == true)
			//    {
			//        Console.WriteLine(key_in.ToString() + " is pressed");
			//        key_pressed = true;
			//    }
			//}
			if (key_pressed == false)
			{
				ServerTcpClient = ServerTCP.AcceptTcpClient();
				NetworkStream stream_for_client = ServerTcpClient.GetStream();
				byte[] ClientBytes = AcceptBytes();
				Console.WriteLine("accept bytes length" + " " + ClientBytes.ToString());
				FileFunctions.TextFile.BytesToConsole(ClientBytes);
				ClientBytes = AcceptBytes();
				Console.WriteLine("accept bytes length" + " " + ClientBytes.ToString());
				FileFunctions.TextFile.BytesToConsole(ClientBytes);
				byte[] AcceptBytes()
				{
					byte[] for_return = new byte[3];
					stream_for_client.Read(for_return, 0, for_return.Length);
					//stream_for_client.Close();
					return for_return;
				}
			}
		}
	}
	static class NetworkFunctions
	{
		public class CheckSum
		{
			//public static byte[] SumBytes(byte[] bytes_in)
			//{
			//    UInt32 check_sum = SumBytes(bytes_in);
			//}
			public static class SumBytes
			{
				public static byte[] Bytes(byte[] bytes_in)
				{
					return MathFunctions.UInt32ToBytes(SumBytes.Number(bytes_in));
				}
				public static UInt32 Number(byte[] bytes_in)
				{
					UInt32 result = 0;
					for (Int32 i = 0; i < bytes_in.Length; i++)
					{
						//Console.Clear();
						//Console.WriteLine("max UInt32 " + UInt32.MaxValue.ToString());
						//Console.WriteLine(result.ToString());
						//Console.WriteLine(bytes_in[i].ToString());
						//Console.WriteLine("Difference " + (UInt32.MaxValue - result).ToString());
						//Console.ReadKey();
						// 2023-07-21 14:05 note. when value is max there is no number
						// to add to get 0. any addition will give number bigger than zero
						if ((UInt32.MaxValue - result) >= bytes_in[i])
						{
							result += bytes_in[i];
						}
						else
						{
							UInt32 difference = bytes_in[i] - (UInt32.MaxValue - result);
							result = difference;
						}
					}
					return result;
				}
			}
		}
		private static void ServerSendBytes(byte[] arr_in, TcpClient client_tcp)
		{
			try
			{
				NetworkStream stream_for_client = client_tcp.GetStream();
				stream_for_client.Write(arr_in, 0, arr_in.Length);
				stream_for_client.Close();
			}
			catch
			{
				ReportFunctions.ReportError("Something wrong in " + nameof(ServerSendBytes));
			}
		}
		private static byte[] ServerRecieveBytes(TcpClient client_tcp)
		{
			byte[] for_return = new byte[0];
			try
			{
				NetworkStream stream_for_client = client_tcp.GetStream();
				for_return = new byte[stream_for_client.Length];
				stream_for_client.Read(for_return, 0, for_return.Length);
				stream_for_client.Close();
			}
			catch
			{
				ReportFunctions.ReportError("Something wrong in " + nameof(ServerSendBytes));
			}
			return for_return;
		}



		/// <summary>
		/// Written. 2024-07-07 09-53. Warsaw. Hostel 3.
		/// </summary>
		public static class Packet
		{

			/// <summary>
			/// Written. 2024-07-07 10-01. Warsaw. Hostel 3.
			/// </summary>
			public static class Create
			{
				/// <summary>
				/// Written. 2024-07-07 10-02. Warsaw. Hostel 3. <br></br>
				/// not tested. <br></br>
				/// 1 byte - Header, 1 byte - Len, 1 byte - CRC8.
				/// <param name="data_in"></param>
				/// <param name="header_in"></param>
				/// <returns></returns>
				public static byte[] HeaderLenDataCRC8(byte[] data_in, byte header_in)
				{
					if (data_in.Length > 120)
					{
						// LEN because of byte can not be large than 255 and with byte stuffing 128 bytes
						// can be stuffed with byte giving 256 bytes no leaving place of header, crc8 and len bytes.
						
						ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
						return data_in;
					}
					
					byte[] byte_stuffing_arr = Byte_stuffing_insert(data_in, header_in);
					byte crc8 = CRC8Compute.ComputeChecksum(byte_stuffing_arr);
					byte[] bytes_of_packet = new byte[data_in.Length + 1 + 1 + 1];
					bytes_of_packet[0] = header_in;
					bytes_of_packet[1] = (byte)(byte_stuffing_arr.Length + 1); // +1 is crc8 byte.
					Array.Copy(byte_stuffing_arr, 0, bytes_of_packet, 2, byte_stuffing_arr.Length);
					bytes_of_packet[bytes_of_packet.Length - 1] = crc8;
					return bytes_of_packet;
				}
			}




            /// <summary>
            /// Written. 2024-07-07 09-20. Warsaw. Hostel 3.
            /// not tested.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="byte_to_insert"></param>
            /// <param name="skip_bytes"></param>
            /// <returns></returns>
            public static byte[] Byte_stuffing_insert(byte[] arr_in, byte byte_to_insert, int skip_bytes)
            {
                // 2024-07-07 09-58. Warsaw. Hostel 3.
				// THe function may not be needed because LEN is not defined until there is byte stuffing and creating
				// packet start with byte stuffing of data.
				
				
				// 2024-07-07 09-20. Warsaw. Hostel 3.
                // THere may be not fast performance - execution time.
                byte[] stuffing_arr = new byte[arr_in.Length - skip_bytes];
                Array.Copy(arr_in, skip_bytes - 1 + 1, stuffing_arr, 0, stuffing_arr.Length);
                stuffing_arr = Byte_stuffing_insert(arr_in, byte_to_insert);
                byte[] arr_out = new byte[skip_bytes + stuffing_arr.Length];
                Array.Copy(arr_in, skip_bytes - 1 + 1, arr_out, 0, skip_bytes);
                Array.Copy(stuffing_arr, 0, arr_out, skip_bytes - 1 + 1, stuffing_arr.Length);
                return arr_out;
            }


            public static byte[] Byte_stuffing_insert(byte[] arr_in, byte byte_to_insert)
            {
                // counting
                Int32 bytes_found = 0;
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    if (arr_in[i] == byte_to_insert)
                    {
                        bytes_found++;
                    }
                }
                byte[] arr_out = new byte[arr_in.Length + bytes_found];
                // filling 
                Int32 arr_out_index = 0;
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                    if (arr_in[i] == byte_to_insert)
                    {
                        arr_out[arr_out_index] = arr_in[i];
                        arr_out_index++;
                    }
                }
                return arr_out;
            }



            /// <summary>
            /// Written. 2024-07-07 09-51. Warsaw. Hostel 3.
            /// not tested.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="skip_byte"></param>
            /// <returns></returns>
            public static byte[] InsertChecksumCRC8(byte[] arr_in, int skip_byte)
			{
				if (arr_in.Length == 0)
				{
					ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
					return arr_in;
				}

				if (skip_byte >= arr_in.Length)
				{
					ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
					return arr_in;
				}

				byte[] arr_out = new byte[arr_in.Length + 1];
				byte[] crc8_arr = new byte[arr_in.Length - skip_byte];
				Array.Copy(arr_in, skip_byte - 1 + 1, crc8_arr, 0, crc8_arr.Length);
				byte crc8 = CRC8Compute.ComputeChecksum(crc8_arr);
				Array.Copy(arr_in, arr_out, arr_in.Length);
				arr_out[arr_out.Length - 1] = crc8;

				return arr_out;
			}
		}

        public static byte[] ByteArrayCheckSum(byte[] arr_in, Int32 num_size = 2)
		{
			byte[] num_out = new byte[0];
			if (num_size <= 0)
			{
				ReportFunctions.ReportError(nameof(num_size) + " wrong.\r\n" + nameof(num_size) + " is " + num_size.ToString());
				return num_out;
			}
			Int32 div_res = 0;
			System.Math.DivRem(num_size, 2, out div_res);
			if (div_res != 0)
			{
				ReportFunctions.ReportError(nameof(num_size) + " wrong.\r\n" + nameof(num_size) + " is " + num_size.ToString());
				return num_out;
			}
			num_out = new byte[num_size];
			UInt32 sum_calc = 0;
			// filling 
			for (Int32 i = 0; i < arr_in.Length; i++)
			{
				sum_calc += arr_in[i];
			}
			for (Int32 i = 0; i < num_size; i++)
			{
				num_out[i] = (byte)(sum_calc >> 8 * i);
			}
			return num_out;
		}
		public static byte[] Byte_stuffing_remove(byte[] arr_in, byte byte_to_remove)
		{
			// counting
			Int32 bytes_found = 0;
			for (Int32 i = 0; i < arr_in.Length; i++)
			{
				if (i == arr_in.Length - 1)
				{
					break;
				}
				if (arr_in[i] == byte_to_remove)
				{
					bytes_found++;
					i++;
				}
			}
			byte[] arr_out = new byte[arr_in.Length - bytes_found];
			// filling 
			Int32 arr_out_index = 0;
			for (Int32 i = 0; i < arr_in.Length; i++)
			{
				arr_out[arr_out_index] = arr_in[i];
				arr_out_index++;
				if (arr_in[i] == byte_to_remove)
				{
					i++;
				}
			}
			return arr_out;
		}
		public static string[] StringSplitCmdData(string string_in, char delimer_in)
		{
			return string_in.Split(new char[] { delimer_in }, 2);
		}
		class NetworkDataClass
		{
			public NetworkDataClass()
			{
				CMD_Method_Bind.Clear();
				CMD_Method_Bind.Add(commands_list_enum.cmd1, commands_list_cmd1_method);
				CMD_Method_Bind.Add(commands_list_enum.cmd2, commands_list_cmd2_method);
			}
			private void commands_list_cmd2_method()
			{
				throw new NotImplementedException();
			}
			private void commands_list_cmd1_method()
			{
				throw new NotImplementedException();
			}
			// not finished.
			// 2023-05-05 09:55 ByteArrayToVariables()        
			Dictionary<string, UInt32> data_values = new Dictionary<string, UInt32>();
			Dictionary<string, Type> data_types = new Dictionary<string, Type>();
			byte[] network_bytes_received = new byte[0];
			public void ByteArrayToVariables()
			{
				MemoryStream memory_read = new MemoryStream(network_bytes_received);
				BinaryReader binary_read = new BinaryReader(memory_read);
				var_1 = binary_read.ReadUInt32();
				var_2 = binary_read.ReadUInt32();
				binary_read.Close();
				memory_read.Close();
			}
			void VariablesToDictionaries()
			{
				data_values = new Dictionary<string, UInt32>();
				data_types = new Dictionary<string, Type>();
				data_values.Add(nameof(var_1), var_1);
				data_types.Add(nameof(var_1), var_1.GetType());
				data_values.Add(nameof(var_2), var_2);
				data_types.Add(nameof(var_2), var_2.GetType());
			}
			// template
			public UInt32 var_1 = 0;
			public UInt32 var_2 = 0;
			void DictionariesToVariables()
			{
				var_1 = data_values[nameof(var_1)];
				var_2 = data_values[nameof(var_2)];
			}
			//[StructLayout(LayoutKind.Explicit)]
			//public struct Union
			//{
			//    [FieldOffset(0)] public Int32 num_1;
			//    [FieldOffset(0)] public Int32 num_2;
			//}
			public void ToConsole()
			{
				VariablesToDictionaries();
				for (Int32 i = 0; i < data_values.Count; i++)
				{
					Console.WriteLine(data_values.ElementAt(i).Key + " " + data_values.ElementAt(i).Value);
				}
			}
			public enum commands_list_enum
			{
				DoNothing = 0,
				NotFound = 1,
				cmd1 = 10,
				cmd2 = 40
			}
			public commands_list_enum CMD_TO_ENUM(byte cmd_in)
			{
				commands_list_enum enum_out = commands_list_enum.DoNothing;
				byte[] enum_values = (byte[])Enum.GetValues(typeof(commands_list_enum));
				if (enum_values.Contains(cmd_in))
				{
					enum_out = (commands_list_enum)cmd_in;
				}
				else
				{
					enum_out = commands_list_enum.NotFound;
				}
				return enum_out;
			}
			Dictionary<commands_list_enum, Action> CMD_Method_Bind = new Dictionary<commands_list_enum, Action>();
			public void CMD_Execute(commands_list_enum cmd_in)
			{
				switch (cmd_in)
				{
					case commands_list_enum.DoNothing: return;
					case commands_list_enum.NotFound: return;
					case commands_list_enum.cmd1:
						CMD_Method_Bind[commands_list_enum.cmd1]();
						return;
					case commands_list_enum.cmd2:
						CMD_Method_Bind[commands_list_enum.cmd2]();
						return;
					default: return;
				}
			}
		}
	}
}
