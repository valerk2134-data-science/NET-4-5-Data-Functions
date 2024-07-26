using ArrayFunctionsNamespace;
using BitsFunctionsNamespace;
using ReportFunctionsNamespace;
using System.IO.Ports;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace USB_as_VCP_Namespace
{
    /*
    1. 2023.09.03 11:16. 5ms send time was used and the console showed correctly so I assume
    the console wrote into buffer what to show and at screen update showed the strings correctly.
    2. 2023.09.03 11:43. performance check.
    2.1. amount of messages was 2 times less than frame rate at speed 5-10 ms. 
    rx was on average in the middle of the frame.
    2.2. with send repeat time 100ms the amount of messages was 7-8.
    75Hz is 13.3ms per frame
    60Hz is 16.7ms per frame
    the main delay is from waiting to send and not from refresh rate therefore amount of rx was close
    to 10 (1000ms/100ms)
    */
    /// <summary>
    /// 2023.09.03 11:06. written. <br></br>
    /// 2023.09.03 11:06. tested. receive to console. <br></br>
    /// 2023.09.03 11:07. tested. receive until delay.
    /// <code>
    /// 2023.09.03 11:10. <br></br>
    /// important! time less than 50ms is close to refresh rate of screen (15-20ms) <br></br>
    /// function (that shows information on the screen) will be waiting until update 
    /// </code>
    /// </summary>
    public class USB_as_VCP
    {
        SerialPort USBPort = null;
        public delegate void ReceiveCallbackFunction();
        public ReceiveCallbackFunction ReceiveCallback;
        Form Form_In_Use;
        /// <summary>
        /// Requires: <br></br>
        /// Portname. <br></br>
        /// Speed. <br></br>
        /// Form. Form is required for proper function work because parallel thread in use during work. <br></br>
        /// Parallel thread does not have permission to do modification of variables that were created by main thread <br></br>
        /// 
        /// </summary>
        /// <param name="port_name"></param>
        /// <param name="speed_in"></param>
        /// <param name="form_called_from"></param>
        public USB_as_VCP(string port_name, Int32 speed_in, Form form_called_from)
        {
            Form_In_Use = form_called_from;
            USBPort = new SerialPort();
            USBPort.BaudRate = speed_in;
            USBPort.PortName = port_name;
            USBPort.Parity = Parity.None;
            USBPort.StopBits = StopBits.One;
            USBPort.RtsEnable = false;
            USBPort.DtrEnable = false;
            USB_Timer_rx_delay.Tick += USB_Timer_rx_delay_Tick;
            USB_Timer_send_period.Tick += USB_Timer_send_period_Tick;
            // 2023.09.03 11:21. check performance. tested. works.
            /*
            rx_timer_performance.Interval = 1000;
            rx_timer_performance.Tick += Rx_timer_performance_Tick;
            */
        }
        
        /// <summary>
        /// Written. 2024.02.01 12:27. Warsaw. Workplace.
        /// </summary>
        public string PortName
        {
            get
            {
                return USBPort.PortName;
            }
        }
        
        
        /// <summary>
        /// Written. 2023.12.21 10:44. Workplace <br></br>
        /// 2023.12.21 10:54. Workplace. Did not decrease memory usage if new instance is created. <br></br>
        /// It may be because something else was connected to the instance
        /// </summary>
        public void DisposeInstance()
        {
            USBPort.Dispose();
        }
        /// <summary>
        /// Written. 2023.11.30 17:47. Warsaw. Workplace. 
        /// </summary>
        /// <param name="form_called_from"></param>
        public USB_as_VCP(Form form_called_from)
        {
            Form_In_Use = form_called_from;
            USBPort = new SerialPort();
            USBPort.Parity = Parity.None;
            USBPort.StopBits = StopBits.One;
            USBPort.RtsEnable = false;
            USBPort.DtrEnable = false;
            USB_Timer_rx_delay.Tick += USB_Timer_rx_delay_Tick;
            USB_Timer_send_period.Tick += USB_Timer_send_period_Tick;
            // 2023.09.03 11:21. check performance. tested. works.
            /*
            rx_timer_performance.Interval = 1000;
            rx_timer_performance.Tick += Rx_timer_performance_Tick;
            */
        }
        private void Rx_timer_performance_Tick(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine(rx_performance_total_count);
            rx_performance_total_count = 0;
        }
        private void USB_Timer_send_period_Tick(object sender, EventArgs e)
        {
            USB_Timer_send_period.Stop();
            Send(USB_send_period_bytes);
            USB_Timer_send_period.Start();
        }
        private void USB_Timer_rx_delay_Tick(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Stop();
            USB_Timer_rx_finished = true;
            Is_Bytes_Received = true;
            ReceiveCallback.Invoke();
        }
        void USB_Timer_Start(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Start();
        }
        void USB_Timer_Stop(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Stop();
        }
        // 2023.09.03 11:19. performance check variables
        Int32 rx_performance_total_count = 0;
        /// <summary>
        /// 2023.09.03 11:41. for performance check. tested. works. 
        /// </summary>
        public Timer rx_timer_performance = new Timer();
        private void USBPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (USB_Reception_Type == USB_Receive_Methods_List.NoReception)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte no_write_byte = 0;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    no_write_byte = (byte)USBPort.ReadByte();
                }
                return;
            }


            // Added. 2024.03.23 17:28. Warsaw. Hostel.
            // not tested.
            if (USB_Reception_Type == USB_Receive_Methods_List.Recieve_MODBUS_RTU)
            {
                int rx_bytes_number = USBPort.BytesToRead;
                for (int i = 0; i < rx_bytes_number; i++)
                {
                    MODBUS_RTU_CMD_RX.LoadByte((byte)USBPort.ReadByte());
                    if (MODBUS_RTU_CMD_RX.IsPacketReceived == true)
                    {
                        USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                        if (MODBUS_CMD_CRC16Check() == true)
                        {
                            if (MODBUS_RTU_CMD_RX.IsErrorAnswer == true)
                            {
                                MODBUS_Callback_bytes = new byte[1];
                                Array.Copy(MODBUS_RTU_CMD_RX.ReceivedBytes, 2, MODBUS_Callback_bytes, 0, 1);
                                MODBUS_RTU_Error_Callback(MODBUS_Callback_bytes);
                                break;
                            }

                            if (MODBUS_RTU_CMD_RX.SingleRegister == true)
                            {
                                MODBUS_Callback_bytes = new byte[4];
                                Array.Copy(MODBUS_RTU_CMD_RX.ReceivedBytes, 2, MODBUS_Callback_bytes, 0, MODBUS_Callback_bytes.Length);
                                MODBUS_RTU_Single_Register_Callback(MODBUS_Callback_bytes);
                                break;
                            }

                            MODBUS_Callback_bytes = new byte[MODBUS_RTU_CMD_RX.NumberOfBytes];
                            Array.Copy(MODBUS_RTU_CMD_RX.ReceivedBytes, 3, MODBUS_Callback_bytes, 0, MODBUS_Callback_bytes.Length);
                            MODBUS_RTU_Callback(MODBUS_Callback_bytes);
                            break;

                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + "Reception failed. CRC16 is not correct");
                        }
                        break;
                    }
                }
                return;
            }


                // 2023.09.28 14:25. added.
                if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveNumberOfBytes)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte[] rx_bytes = new byte[rx_bytes_number];
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _recieve_bytes_count++;
                    _usb_rx_buffer[_recieve_bytes_size - 1] = (byte)USBPort.ReadByte();
                }
                if (_recieve_bytes_count >= _recieve_bytes_size)
                {
                    Form_In_Use.Invoke(RecieveNumberOfBytesCallback);
                    _recieve_bytes_count = 0;
                }
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveToConsole)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte[] rx_bytes = new byte[rx_bytes_number];
                /*
                2023.09.03 10:38. that code was showing mixed letter in console
                Console.write I assume takes 1 frame for update and with transfer 10 letter in 100ms
                with 15-20ms update time there were in certain moment two function updating console.
                //for (Int32 i = 0; i < rx_bytes_number; i++)
                //{
                //    rx_bytes[i] = (byte)USBPort.ReadByte();
                //}
                */
                rx_bytes_number = USBPort.Read(rx_bytes, 0, rx_bytes.Length);
                if (RecieveToConsole_OutputType == ConsoleOutput.Char)
                {
                    string str_to_console = "";
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        str_to_console += ((char)rx_bytes[i]);
                    }
                    Console.Write(str_to_console);
                    // 2023.09.03 11:40. for performance check
                    /*
                    if (str_to_console.Contains("\n") == true)
                    {
                        rx_performance_total_count += 1;
                    }
                    */
                }
                if (RecieveToConsole_OutputType == ConsoleOutput.Decimal)
                {
                    string str_to_console = "";
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        str_to_console += System.Convert.ToString(rx_bytes[i], 10) + " ";
                    }
                    Console.Write(str_to_console);
                }
                
                
                if (RecieveToConsole_OutputType == ConsoleOutput.HEX)
                {
                    string str_to_console = "";
                    if (HEXReceptionData.CountPerRow == -1)
                    {
                        for (Int32 i = 0; i < rx_bytes_number; i++)
                        {
                            str_to_console += System.Convert.ToString(rx_bytes[i], 16).ToUpper().PadLeft(2, '0');
                            if (HEXReceptionData.SpaceBetween == true)
                            {
                                str_to_console += " ";
                            }
                        }
                    }
                    else
                    {                        
                        for (Int32 i = 0; i < rx_bytes_number; i++)
                        {
                            str_to_console += System.Convert.ToString(rx_bytes[i], 16).ToUpper().PadLeft(2, '0');
                            HEXReceptionData.Count += 1;
                            
                            if (HEXReceptionData.Count == HEXReceptionData.CountPerRow)
                            {
                                str_to_console += "\r\n";
                                HEXReceptionData.Count = 0;
                                continue;
                            }

                            if (HEXReceptionData.SpaceBetween == true)
                            {
                                str_to_console += " ";
                            }
                        }
                    }




                    Console.Write(str_to_console);
                }
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveUntildDelay)
            {
                Form_In_Use.Invoke(new EventHandler(USB_Timer_Stop));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                if (USB_Timer_rx_finished == true)
                {
                    byte no_write_byte = 0;
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        no_write_byte = (byte)USBPort.ReadByte();
                    }
                    return;
                }
                bool buffer_is_full = false;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    if (_usb_rx_buffer_count < _usb_rx_buffer_size)
                    {
                        _usb_rx_buffer_count += 1;
                        _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    }
                    else
                    {
                        // note. implementation.
                        buffer_is_full = true;
                    }
                }
                if (buffer_is_full == true)
                {
                    Is_Bytes_Received = true;
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayMaxLength, _usb_rx_buffer_count);
                }
                Form_In_Use.Invoke(new EventHandler(USB_Timer_Start));
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveByLengthInt8)
            {
                TimerRecieveByLength_DelayLastByte.Stop();
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _usb_rx_buffer_count += 1;
                    _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    if (RecieveByLength_LengthIsRead == false)
                    {
                        if (_usb_rx_buffer_count >= _usb_rx_buffer_size)
                        {
                            // note on >=. == is enough and it should work. 2023.12.14 10:49. Workplace.
                            Int32 NewSize = _usb_rx_buffer[_usb_rx_buffer_count - 1];
                            BufferRxIncrease = NewSize;
                            RecieveByLength_LengthIsRead = true;
                            RecieveByLength_Length = NewSize;
                        }
                    }
                    else
                    {
                        RecieveByLength_Count += 1;
                        if (RecieveByLength_Count >= RecieveByLength_Length)
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Console.WriteLine(nameof(RecieveByLengthInt16) + " done. " + RecieveByLength_Length.ToString() + " bytes were received");
                            RecieveByLengthCallback(_usb_rx_buffer);
                        }
                    }
                }
                TimerRecieveByLength_DelayLastByte.Start();
            }
            // Written. 2023.12.14 10:53. Workplace.
            // Works. 2023.12.14 14:08. Workplace.
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveByLengthInt16)
            {
                Form_In_Use.Invoke(new EventHandler(TimerStoptRecieveByLength));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _usb_rx_buffer_count += 1;
                    _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    // 2024.01.20 17:16. Warsaw. Hostel.
                    // Obtaining Length is done by setting buffer size equal 3 bytes
                    // for cmd and length and once it is filled, it means there is
                    // length bytes are received.
                    if (RecieveByLength_LengthIsRead == false)
                    {
                        if (_usb_rx_buffer_count >= _usb_rx_buffer_size)
                        {
                            // note on >=. == is enough and it should work. 2023.12.14 10:49. Workplace.
                            Int32 NewSize = (_usb_rx_buffer[_usb_rx_buffer_count - 1 - 1] << 8);
                            NewSize |= (_usb_rx_buffer[_usb_rx_buffer_count - 1] << 0);
                            BufferRxIncrease = NewSize;
                            RecieveByLength_LengthIsRead = true;
                            RecieveByLength_Length = NewSize;
                        }
                    }
                    else
                    {
                        RecieveByLength_Count += 1;
                        if (RecieveByLength_Count >= RecieveByLength_Length)
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Console.WriteLine(nameof(RecieveByLengthInt16) + " done. " + RecieveByLength_Length.ToString() + " bytes were received");
                            Form_In_Use.Invoke(RecieveByLengthCallback, _usb_rx_buffer);
                            // note that it is secondary threat and it does not have access to the main threat. 2023.12.14 14:26. Workplace.
                            TimerRecieveByLength_DelayLastByte.Tick -= TimerRecieveByLength_DelayLastByte_Tick;
                            TimerRecieveByLength_NoByteDelay.Tick -= TimerRecieveByLength_NoByteDelay_Tick;
                            // skip if there are bytes. 2023.12.14 13:54. Workplace
                            byte byte_read = 0;
                            for (Int32 j = i + 1; j < rx_bytes_number; j++)
                            {
                                byte_read = (byte)USBPort.ReadByte();
                            }
                            return;
                        }
                    }
                }
                Form_In_Use.Invoke(new EventHandler(TimerStartRecieveByLength));
            }



           
            // Written. 2024.01.20 17:09. Warsaw. Hostel.
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveStrings)
            {
                Form_In_Use.Invoke(new EventHandler(TimerReceiveStrings_LastByteDelay_Stop));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    ReceiveStringsArray[ReceiveStringsArrIndex] += ((char)((byte)USBPort.ReadByte())).ToString();
                    if (ReceiveStringsArray[ReceiveStringsArrIndex].Contains("\r\n") == true)
                    {
                        // Certain amount of string to receive. 2024.01.30 10:39. Warsaw, Workplace
                        if (ReceiveStringsArrIndex >= (ReceiveStringsCount - 1))
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Form_In_Use.Invoke(new EventHandler(ReceiveStringsCompleted));
                            return;
                        }
                        
                        
                        ReceiveStringsArrIndex += 1;
                        ReceiveStringsArray[ReceiveStringsArrIndex] = "";
                        if (ReceiveStringsArrIndex > (ReceiveStringsArray.Length - (ReceiveStringsArray.Length / 4)))
                        {
                            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArray.Length * 2);
                        }
                    }
                }


                Form_In_Use.Invoke(new EventHandler(TimerReceiveStrings_LastByteDelay_Start));

            }








        }

        

        /// <summary>
        /// Written. 2023.11.30 17:52. Warsaw. Workplace. 
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return USBPort.IsOpen;
            }
        }
        /// <summary>
        /// Tested. Works. 2023.12.12 16:52. Workplace. <br></br>
        /// <br></br>
        /// 2024.01.23 15:50. Warsaw. Workplace. <br></br>
        /// Important. Reception starts as soon as Open() is called.
        /// It was tested with 5 seconds delay between Open() with clear buffer
        /// and adding += DataReceived.
        /// </summary>
        public bool Open(bool report_error = true)
        {
            bool result_out = false;
            try
            {
                
                USBPort.Open();            
                USBPort.DiscardInBuffer();
                USBPort.DiscardOutBuffer();               
                USBPort.DataReceived += USBPort_DataReceived;
                result_out = true;
            }
            catch (Exception e)
            {
                if (report_error == true)
                {
                    ReportFunctions.ReportError("Open VCP error.\r\n" + e.Message);
                }
            }
            return result_out;
        }

        byte[] _usb_rx_buffer = new byte[128];
        Int32 _usb_rx_buffer_count = 0;
        Int32 _usb_rx_buffer_size = 128;

        public byte[] ReceivedBytes
        {
            get
            {
                byte[] bytes_out = new byte[_usb_rx_buffer_count];
                Array.Copy(_usb_rx_buffer, bytes_out, _usb_rx_buffer_count);
                return bytes_out;
            }
        }
        /// <summary>
        /// Written. 2023.12.14 10:43. Workplace
        /// </summary>
        public Int32 BufferRxIncrease
        {
            set
            {
                _usb_rx_buffer_size += value;
                Array.Resize(ref _usb_rx_buffer, _usb_rx_buffer_size);
            }
        }
        public Int32 BufferRxSize
        {
            get
            {
                return _usb_rx_buffer_size;
            }
            set
            {
                _usb_rx_buffer = new byte[value];
                _usb_rx_buffer_size = value;
            }
        }

        /// <summary>
        /// Buffer is needed to keep bytes which were declared in a function - local variable. <br></br>
        /// The bytes are lost after exit from function and not all bytes may be transfered. <br></br>
        /// <br></br>
        /// Written. 2024.02.01 12:05. Warsaw. Workplace. <br></br>
        /// </summary>
        public Int32 BufferTxSize
        {
            get
            {
                return _usb_tx_buffer_size;
            }
            set
            {
                _usb_tx_buffer = new byte[value];
                _usb_tx_buffer_size = value;
            }
        }
        
        byte[] _usb_tx_buffer = new byte[128];
        Int32 _usb_tx_buffer_count = 0;
        Int32 _usb_tx_buffer_size = 128;
       

        public bool Is_Bytes_Received = false;
        Timer USB_Timer_rx_delay = new Timer();
        bool USB_Timer_rx_finished = true;
        enum USB_Receive_Methods_List
        {
            NoReception,
            ReceiveUntildDelay,
            ReceiveToConsole,
            ReceiveNumberOfBytes,
            ReceiveByLengthInt16,
            ReceiveByLengthInt8,
            ReceiveStrings,
            Recieve_MODBUS_RTU
        }
        USB_Receive_Methods_List USB_Reception_Type = USB_Receive_Methods_List.NoReception;
        //bool USB_Reception_1_byte_rx = false;
        Int32 _recieve_bytes_count = 0;
        Int32 _recieve_bytes_size = 0;
        public delegate void RecieveNumberOfBytesDelegate(byte[] arr_in);
        public RecieveNumberOfBytesDelegate RecieveNumberOfBytesCallback;
        public void RecieveNumberOfBytes(Int32 number_in, RecieveNumberOfBytesDelegate callback_in)
        {
            _recieve_bytes_size = number_in;
            _recieve_bytes_count = 0;
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveNumberOfBytes;
            RecieveNumberOfBytesCallback += callback_in;
        }
        public void RecieveNumberOfBytes(Int32 number_in)
        {
            RecieveNumberOfBytes(number_in, null);
        }
        public void RecieveUntilDelay(Int32 delay_in = 200)
        {
            // 2023.08.23 13:45. delay 5 ms is not good. datareceive from serial port
            // may have delay greater than 5 ms. 15 ms is min. 
            // 50 ms is ok.
            if (USBPort.IsOpen == false)
            {
                Open();
            }
            USB_Timer_rx_delay.Interval = delay_in;
            USB_Timer_rx_finished = false;
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveUntildDelay;
            Is_Bytes_Received = false;
            _usb_rx_buffer_count = 0;
            //  USB_Reception_1_byte_rx = false;
        }
        Timer USB_Timer_send_period = new Timer();
        byte[] USB_send_period_bytes = new byte[0];
        /// <summary>
        /// 2023.09.02 20:22. written.
        /// 2023.09.02 20:22. tested. works.
        /// </summary>
        /// <param name="data_in"></param>
        /// <param name="period"></param>
        public void SendPeriod(byte[] data_in, Int32 period)
        {
            USB_Timer_send_period.Interval = period;
            USB_send_period_bytes = ArrayFunctions.ByteArray.Copy(data_in);
            Send(USB_send_period_bytes);
            USB_Timer_send_period.Start();
        }
        /// <summary>
        /// 2023.09.02 20:22. written.
        /// 2023.09.02 21:54. tested. works.
        /// </summary>
        /// <param name="string_in"></param>
        /// <param name="period"></param>
        public void SendPeriod(string string_in, Int32 period)
        {
            // 2023.09.09 12:07. importance 3. Encoding is selected according to UTF of the
            // string and not which encoding bytes encoding.
            byte[] utf7_bytes = Encoding.UTF8.GetBytes(string_in);
            SendPeriod(utf7_bytes, period);
        }
        /// <summary>
        /// 2023.09.02 21:54. written.
        /// 2023.09.02 21:54. tested. works.
        /// 2023.09.02 21:55. note. implementation in the function.
        /// </summary>
        /// 
        /// 
        /// <param name="data_in"></param>
        /// <returns></returns>
        public bool Send(byte[] data_in, bool msg_to_console = false)
        {
            bool for_return = false;
            try
            {
                // 2023.09.02 21:16. skips every 2nd byte. I assume writes 2 bytes chars.
                // USBPort.BaseStream.BeginWrite. skips 2nd byte.
                // USBPort.BaseStream.WriteAsync. skips every 2nd byte.
                // 2023.09.02 21:56. works. tested.
                /*
                for (Int32 i = 0; i < data_in.Length; i++)
                {
                    USBPort.BaseStream.WriteByte(data_in[i]);
                } 
                 */

                // Added. 2024.02.01 12:09. Warsaw. Workplace.
                if (BufferTxSize < data_in.Length)
                {
                    BufferTxSize = data_in.Length * 2;
                }
                Array.Copy(data_in, _usb_tx_buffer, data_in.Length);
                Int32 data_length = data_in.Length;

                for (Int32 i = 0; i < data_length; i++)
                {
                    USBPort.BaseStream.WriteByte(_usb_tx_buffer[i]);
                }
                for_return = true;
                if (msg_to_console == true)
                {
                    //Console.WriteLine();
                    Console.Write(DateTime.Now.ToString("HH:mm:ss") + " " + "Sent in " + USBPort.PortName + ":");
                    for (Int32 i = 0; i < data_length; i++)
                    {
                        Console.Write(Convert.ToString(_usb_tx_buffer[i], 16).PadLeft(2, '0') + " ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                ReportFunctions.ReportError("VCP. Send error.\r\n" + e.Message);
            }
            return for_return;
        }
        public bool Send(string string_in)
        {
            bool for_return = false;
            try
            {
                byte[] utf8_bytes = Encoding.UTF8.GetBytes(string_in);
                Send(utf8_bytes);
                for_return = true;
            }
            catch (Exception e)
            {
                ReportFunctions.ReportError("VCP. Send error.\r\n" + e.Message);
            }
            return for_return;
        }
        /// <summary>
        /// Tested. Works. 2023.12.12 16:53. Workplace.
        /// </summary>
        public bool Close(bool show_error = true)
        {
            bool result_out = false;
            try
            {
                USBPort.DataReceived -= USBPort_DataReceived;
                Thread.Sleep(50);
                USBPort.DiscardInBuffer();
                USBPort.DiscardOutBuffer();
                USBPort.Close();
                result_out = true;
            }
            catch (Exception e)
            {
                if (show_error == true)
                {
                    ReportFunctions.ReportError("Close VCP error.\r\n" + e.Message);
                }
            }
            return result_out;
        }
        enum ConsoleOutput
        {
            Char,
            HEX,
            Decimal
        }
        ConsoleOutput RecieveToConsole_OutputType = ConsoleOutput.Char;
        public void RecieveToConsoleDecimal()
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;
            RecieveToConsole_OutputType = ConsoleOutput.Decimal;
        }
        /// <summary>
        /// Tested. Works. 2024.04.16 14:34. Warsaw. Workplace.
        /// </summary>
        public void RecieveToConsoleChar()
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;
            RecieveToConsole_OutputType = ConsoleOutput.Char;
        }

        class RecieveToConsoleHEXSettings
        {
            public int Count = 0;
            public int CountPerRow = 40;
            public bool SpaceBetween = true;
            public ConsoleOutput OutputType = ConsoleOutput.HEX; // may be not required. 2024.04.16 14:39
        }

        RecieveToConsoleHEXSettings HEXReceptionData = new RecieveToConsoleHEXSettings();
        /// <summary>
        /// Tested. Works. 2024.04.16 15:05. Warsaw. Workplace.
        /// </summary>
        /// <param name="count_per_row"></param>
        /// <param name="space_between"></param>
        public void RecieveToConsoleHEX(int count_per_row = 20, bool space_between = true)
        {
            HEXReceptionData.SpaceBetween = space_between;
            HEXReceptionData.CountPerRow = count_per_row;
            HEXReceptionData.Count = 0;
            RecieveToConsole_OutputType = ConsoleOutput.HEX;
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;            
        }
        Int32 RecieveByLength_Length = 0;
        Int32 RecieveByLength_Count = 0;
        Int32 RecieveByLength_1st_byte = 0;
        bool RecieveByLength_LengthIsRead = false;
        bool RecieveByLength_ReceptionError = false;
        Timer TimerRecieveByLength_DelayLastByte = new Timer();
        Timer TimerRecieveByLength_NoByteDelay = new Timer();
        public delegate void RecieveByLengthDelegate(byte[] arr_in);
        public RecieveByLengthDelegate RecieveByLengthCallback;
        public void RecieveByLengthInt16(Int32 length_first_byte, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveByLengthInt16;
            RecieveByLength_1st_byte = length_first_byte;
            BufferRxSize = (length_first_byte + 1) + 1;
            // +1 because 1st byte of LEN already counted. 2023.12.14 10:45. Workplace. 
            TimerRecieveByLength_DelayLastByte.Interval = delay_last_byte;
            TimerRecieveByLength_DelayLastByte.Tick += TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Interval += delay_last_byte * 10;
            TimerRecieveByLength_NoByteDelay.Tick += TimerRecieveByLength_NoByteDelay_Tick;
            _usb_rx_buffer_count = 0;
            RecieveByLength_Count = 0;
            RecieveByLength_LengthIsRead = false;
            RecieveByLength_ReceptionError = false;
        }



        Timer TimerReceiveStrings_LastByteDelay = new Timer();
        string[] ReceiveStringsArray = new string[128];
        Int32 ReceiveStringsArrIndex = 0;
        Int32 ReceiveStringsCount = -1;
        // 2024.01.23 14:19. Warsaw. Workplace. 
        // The function is called from Timer.Tick and it is from main thread.
        // There is no need to use Invoke.
        public delegate void ReceiveStringDelegate(string[] arr_in);
        public ReceiveStringDelegate ReceiveStringCallback = null;
        /// <summary>
        /// Written. 2024.01.20 16:47. Warsaw. Hostel <br></br>
        /// not tested <br></br>
        /// Note. Delay from start is not implemented. 2024.01.30 11:05. Warsaw. Workplace.
        /// </summary>
        /// <param name="strings_count"></param>
        /// <param name="delay_last_byte"></param>
        public void ReceiveStringsStart(Int32 strings_count = -1, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveStrings;
            ReceiveStringsCount = strings_count;
            ReceiveStringsArray[0] = "";
            ReceiveStringsArrIndex = 0;
            TimerReceiveStrings_LastByteDelay.Interval = delay_last_byte;
            TimerReceiveStrings_LastByteDelay.Tick += TimerReceiveStrings_LastByteDelay_Tick;
        }



        /// <summary>
        /// Written. 2024.01.30 11:08. Warsaw. Workplace.
        /// not tested
        /// </summary>
        public void ReceiveStringsStop()
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;            
        }




        /// <summary>
        /// Callback for receive certain amount of strings <br></br>
        /// Written. 2024.01.30 10:44. Warsaw. Workplace.
        /// </summary>
        /// <param name="strings_in"></param>
        private void ReceiveStringsCompleted(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;


            // 2024.01.30 10:47. Warsaw. Workplace.
            // removing "\r\n"

            for (Int32 i = 0; i <= ReceiveStringsArrIndex; i++)
            {
                ReceiveStringsArray[i] = ReceiveStringsArray[i].Replace("\r\n", "");
            }

            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArrIndex + 1);

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + nameof(ReceiveStringsStart) + ". Reception is completed.\r\n" +            
            ReceiveStringsArray.Length.ToString() + " strings were received.\r\n");
            ReceiveStringCallback(ReceiveStringsArray);
        }



        /// <summary>
        /// Written. 2024.01.20 16:52. Warsaw. Hostel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Start(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Start();
        }



        /// <summary>
        /// Written. 2024.01.20 16:54. Warsaw. Hostel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Stop(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
        }




        /// <summary>
        /// Written. 2024.01.20 17:05. Warsaw. Hostel.
        /// not tested.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Tick(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;

            // 2024.01.23 15:28. Warsaw. Workplace.
            // Checking if the last string is received, not fully received, VCP was waiting for string
            if (ReceiveStringsArray[ReceiveStringsArrIndex].Contains("\r\n") == false)
            {
                ReceiveStringsArrIndex -= 1;
            }


            // 2024.01.23 14:33. Warsaw. Workplace.
            // removing "\r\n"

            for (Int32 i = 0; i <= ReceiveStringsArrIndex; i++)
            {
                ReceiveStringsArray[i] = ReceiveStringsArray[i].Replace("\r\n", "");
            }

            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArrIndex + 1);

            if (ReceiveStringsCount == -1)
            {
                Console.WriteLine(nameof(ReceiveStringsStart) + ". Reception is completed.\r\n" +
                TimerReceiveStrings_LastByteDelay.Interval.ToString() + "ms was reached.\r\n" +
                ReceiveStringsArray.Length.ToString() + " strings were received.\r\n");
            }
            else
            {
                Console.WriteLine(nameof(ReceiveStringsStart) + ". Failure in receiving all strings.\r\n" +
                     ReceiveStringsArray.Length.ToString() + "/" + ReceiveStringsCount.ToString() + " strings were received.\r\n");
            }
            ReceiveStringCallback(ReceiveStringsArray);
        }












        /// <summary>
        /// Not finished. 2024.01.20 16:43.
        /// </summary>
        /// <param name="length_first_byte"></param>
        /// <param name="delay_last_byte"></param>
        public void RecieveByLengthInt8(Int32 length_first_byte, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveByLengthInt8;
            RecieveByLength_1st_byte = length_first_byte;
            BufferRxSize = (length_first_byte + 1);
            TimerRecieveByLength_DelayLastByte.Interval = delay_last_byte;
            TimerRecieveByLength_DelayLastByte.Tick += TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Interval += delay_last_byte * 10;
            TimerRecieveByLength_NoByteDelay.Tick += TimerRecieveByLength_NoByteDelay_Tick;
            _usb_rx_buffer_count = 0;
            RecieveByLength_Count = 0;
            RecieveByLength_LengthIsRead = false;
            RecieveByLength_ReceptionError = false;
        }



        /// <summary>
        /// Receives certain amount of bytes defined by structure of command to be received. <br></br>
        /// Written. 2023.12.27 16:27. Workplace.
        /// </summary>
        /// <param name="cmd_code"></param>
        /// <param name="delay_last_byte"></param>
        public void RecieveByCMD(Int32 cmd_code, Int32 total_bytes, Int32 delay_last_byte = 500)
        {
            // Not completed. 2024.03.22 10:46. Warsaw. Workplace.
        }


        /// <summary>
        /// Written. 2024.03.25 17:00. Warsaw. Workplace.
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        public static string MODBUS_RTU_GetErrorMessage(byte error_code)
        {
            if (error_code == 0x01)
            {
                return "Illegal function";
            }

            if (error_code == 0x02)
            {
                return "Illegal address";
            }

            if (error_code == 0x03)
            {
                return "Illegal data";
            }

            if (error_code == 0x04)
            {
                return "Slave error";
            }

            return "Error code was not found";
        }

        MODBUS_RTU_TX_CMD_Class MODBUS_RTU_TX_CMD = null;

        /// <summary>
        /// Written. 2024.03.25 13:04. Warsaw. Workplace.
        /// </summary>
        public class MODBUS_RTU_TX_CMD_Class
        {
            /// <summary>
            /// Written. 2024.03.25 17:07. Warsaw. Workplace. <br></br>
            /// Error code - 0x86 according MODBUS APPLICATION PROTOCOL SPECIFICATION v1.1a, v1.1b
            /// </summary>
            public byte ErrorCode = 0x86;
            public byte ToAddress = 0;
            public byte FunctionCode = 0;
            public ushort RegisterAddress = 0;
            public ushort NumberOfRegisters = 0;
            public bool IsAnswerReceived = false;
            /// <summary>
            /// For 0x06 function.
            /// 2024.03.25 14:24. Warsaw. Workplace.
            /// </summary>
            public ushort RegisterValue = 0;
            public ushort CRC16 = 0;
            public byte[] TransmitBytes = null;
            public byte[] GetBytesForCRC16()
            {
                byte[] arr_out = new byte[6];
                int arr_index = 0;

                arr_out[arr_index] = ToAddress;
                arr_index += 1;

                arr_out[arr_index] = FunctionCode;
                arr_index += 1;

                arr_out[arr_index] = (byte)(RegisterAddress >> 8);
                arr_index += 1;

                arr_out[arr_index] = (byte)(RegisterAddress >> 0);
                arr_index += 1;

                if ((FunctionCode == 0x04) ||
                    (FunctionCode == 0x03))
                { 

                    arr_out[arr_index] = (byte)(NumberOfRegisters >> 8);
                    arr_index += 1;

                    arr_out[arr_index] = (byte)(NumberOfRegisters >> 0);
                    arr_index += 1;
                }

                if (FunctionCode == 0x06)
                {
                    arr_out[arr_index] = (byte)(RegisterValue >> 8);
                    arr_index += 1;

                    arr_out[arr_index] = (byte)(RegisterValue >> 0);
                    arr_index += 1;
                }

                return arr_out;
            }

            /// <summary>
            /// Create all required bytes for transfer <br></br> 
            /// Note. The function is called automatically after MODBUS RTU Send. <br></br>
            /// Note. Function code must be assigned for correct execution. <br></br>
            /// Written. 2024.03.25 14:26. Warsaw. Workplace.
            ///  
            /// </summary>
            public void CreatePacket()
            {
                TransmitBytes = new byte[8];
                int arr_index = 0;

                TransmitBytes[arr_index] = ToAddress;
                arr_index += 1;

                TransmitBytes[arr_index] = FunctionCode;
                arr_index += 1;

                TransmitBytes[arr_index] = (byte)(RegisterAddress >> 8);
                arr_index += 1;

                TransmitBytes[arr_index] = (byte)(RegisterAddress >> 0);
                arr_index += 1;

                if ((FunctionCode == 0x04) ||
                    (FunctionCode == 0x03))
                {
                    TransmitBytes[arr_index] = (byte)(NumberOfRegisters >> 8);
                    arr_index += 1;

                    TransmitBytes[arr_index] = (byte)(NumberOfRegisters >> 0);
                    arr_index += 1;
                }

                if (FunctionCode == 0x06)
                {
                    TransmitBytes[arr_index] = (byte)(RegisterValue >> 8);
                    arr_index += 1;

                    TransmitBytes[arr_index] = (byte)(RegisterValue >> 0);
                    arr_index += 1;
                }

                TransmitBytes[arr_index] = (byte)(CRC16 >> 0);
                arr_index += 1;

                TransmitBytes[arr_index] = (byte)(CRC16 >> 8);
                arr_index += 1;
            }

        }

        /// <summary>
        /// Written. 2024.03.22 11:06. Warsaw. Workplace.
        /// For receving 1 reply by MODBUS RTU. For another reply a new instance should be made
        /// </summary>
        class MODBUS_RTU_RX_CMD_Class
        {
            /// <summary>
            /// Written. 2024.03.25 17:06. Warsaw. Workplace. <br></br>
            /// Error code - 0x86 according MODBUS APPLICATION PROTOCOL SPECIFICATION v1.1a, v1.1b
            /// </summary>
            public byte ErrorCode = 0x86;
            public bool IsErrorAnswer = false;
            /// <summary>
            /// Slave address.
            /// </summary>
            public byte FromAddress = 0;
            public byte FunctionCode = 0;
            /// <summary>
            /// These are bytes of registers. CRC32 is not included in the bytes.<br></br>
            /// Note that register is 16 bits.
            /// </summary>
            public byte NumberOfBytes = 0;
            public bool SingleRegister = false;
            public UInt16 CRC16 = 0;
            public byte[] ReceivedBytes = null;
            public uint ReceivedCount = 0;
            public MODBUS_RTU_RX_CMD_Class()
            {
                ReceivedBytes = new byte[3];
            }
            public bool IsPacketReceived = false;
            public void CreatePacket()
            {
                               
                byte[] arr_3_bytes = new byte[3];
                Array.Copy(ReceivedBytes, arr_3_bytes, 3);
                // 2024.03.22 11:11. Warsaw. Workplace.
                // +3 - byte with address byte, byte with function code, byte with number of bytes.
                // +2 - CRC16 bytes.
                ReceivedBytes = new byte[NumberOfBytes + 3 + 2];
                Array.Copy(arr_3_bytes, ReceivedBytes, 3);
                
            }

            public void LoadByte(byte byte_in)
            {
                ReceivedCount += 1;

                // Address from which to receive bytes
                if (ReceivedCount == 1)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    if (byte_in != FromAddress)
                    {
                        ReceivedCount = 0;
                    }
                    return;
                }

                // Function code.
                if (ReceivedCount == 2)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    if ((byte_in != FunctionCode) &&
                        (byte_in != ErrorCode))
                    {
                        ReceivedCount = 0;
                    }

                    if (byte_in == ErrorCode)
                    {
                        IsErrorAnswer = true;
                    }

                    return;
                }

                // Number of bytes.
                if (ReceivedCount == 3)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    NumberOfBytes = byte_in;

                    if (IsErrorAnswer == true)
                    {
                        NumberOfBytes = 0;
                        CreatePacket();
                        return;
                    }

                    if (FunctionCode == 0x06)                        
                    {
                        SingleRegister = true;
                        NumberOfBytes = 4 - 1;
                        CreatePacket();
                        return;
                    }

                    CreatePacket();
                    return;
                }


                if (SingleRegister == true)
                {
                    // Register and value
                    if ((ReceivedCount > 3) &&
                        (ReceivedCount <= (3 + NumberOfBytes + 2)))
                    {
                        ReceivedBytes[ReceivedCount - 1] = byte_in;

                        if (ReceivedCount >= (3 + NumberOfBytes + 2))
                        {
                            // 2024.03.26 10:46. Warsaw. Workplace. 
                            // CRC16 comes with low byte 1st
                            CRC16 = ReceivedBytes[ReceivedBytes.Length - 2];
                            CRC16 |= (ushort)(ReceivedBytes[ReceivedBytes.Length - 1] << 8);
                            IsPacketReceived = true;
                        }
                    }
                    return;
                }

                if (IsErrorAnswer == false)
                {
                    // Register and value
                    if ((ReceivedCount > 3) &&
                        (ReceivedCount <= (3 + NumberOfBytes + 2)))
                    {
                        ReceivedBytes[ReceivedCount - 1] = byte_in;

                        if (ReceivedCount >= (3 + NumberOfBytes + 2))
                        {
                            // 2024.03.26 11:37. Warsaw. Workplace. 
                            // CRC16 comes with low byte 1st
                            CRC16 = ReceivedBytes[ReceivedBytes.Length - 2];
                            CRC16 |= (ushort)(ReceivedBytes[ReceivedBytes.Length - 1] << 8);
                            IsPacketReceived = true;
                        }
                    }
                    return;
                }

                if (IsErrorAnswer == true)
                {
                    // CRC16 left to receive
                    if ((ReceivedCount > 3) &&
                        (ReceivedCount <= (3 + 2)))
                    {
                        ReceivedBytes[ReceivedCount - 1] = byte_in;

                        if (ReceivedCount == (3 + 2))
                        {
                            CRC16 = ReceivedBytes[ReceivedBytes.Length - 2];
                            CRC16 |= (ushort)(ReceivedBytes[ReceivedBytes.Length - 1] << 8);
                            IsPacketReceived = true;
                        }
                        return;
                    }
                }
            }
        }



        public bool MODBUS_TX_RX_message = true;
        // Added. 2024.03.23 17:04. Warsaw. Hostel.
        public delegate void ReceiveMODBUSDelegate(byte[] arr_in);
        public ReceiveMODBUSDelegate MODBUS_RTU_Callback = null;
        public ReceiveMODBUSDelegate MODBUS_RTU_Single_Register_Callback = null;
        public ReceiveMODBUSDelegate MODBUS_RTU_Error_Callback = null;
        MODBUS_RTU_RX_CMD_Class MODBUS_RTU_CMD_RX = null;
        public CRC16Class CRC16Calculation = new CRC16Class(CRC16Class.DefaultPolynomial);
        byte[] MODBUS_Callback_bytes = null;
        private bool MODBUS_CMD_CRC16Check()
        {
            byte[] arr_with_bytes = new byte[MODBUS_RTU_CMD_RX.ReceivedBytes.Length - 2];
            Array.Copy(MODBUS_RTU_CMD_RX.ReceivedBytes, arr_with_bytes, arr_with_bytes.Length);
            ushort crc16_of_received_bytes = CRC16Calculation.ComputeChecksum(arr_with_bytes);
            bool check_result_out = false;
            if (crc16_of_received_bytes == MODBUS_RTU_CMD_RX.CRC16)
            {
                check_result_out = true;
            }
            return check_result_out;
        }


        /// <summary>
        /// It wil take the required value for proper reception of answer. <br></br>
        /// Written. 2024.03.25 13:19. Warsaw. Workplace.
        /// </summary>
        /// <param name="tx_cmd"></param>
        public void MODBUS_RTU_Receive(MODBUS_RTU_TX_CMD_Class tx_cmd)
        {
            MODBUS_RTU_Receive(tx_cmd.ToAddress, tx_cmd.FunctionCode, tx_cmd.ErrorCode);
        }


        /// <summary>
        /// Written. 2024.03.22 10:51. Warsaw. Workplace.
        /// </summary>
        /// <param name="from_address"></param>
        /// <param name="bytes_amount"></param>
        /// <param name="delay_last_byte">1st byte is the trigger to count the delay. Each byte resets the counter</param>
        public void MODBUS_RTU_Receive(byte from_address, byte function_code, byte error_code = 0x86, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.Recieve_MODBUS_RTU;
            MODBUS_RTU_CMD_RX = new MODBUS_RTU_RX_CMD_Class();
            MODBUS_RTU_CMD_RX.FromAddress = from_address;
            MODBUS_RTU_CMD_RX.FunctionCode = function_code;
            MODBUS_RTU_CMD_RX.ErrorCode = error_code;
        }


        /// <summary>
        /// Creates packet and sends the bytes. <br></br>
        /// Written. 2024.03.25 13:16. Warsaw. Workplace.
        /// </summary>
        /// <param name="modbus_rtu_cmd"></param>
        public void MODBUS_RTU_Send(MODBUS_RTU_TX_CMD_Class modbus_rtu_cmd)
        {
            MODBUS_RTU_TX_CMD = new MODBUS_RTU_TX_CMD_Class();
            MODBUS_RTU_TX_CMD.ToAddress = modbus_rtu_cmd.ToAddress;
            MODBUS_RTU_TX_CMD.FunctionCode = modbus_rtu_cmd.FunctionCode;
            MODBUS_RTU_TX_CMD.RegisterAddress = modbus_rtu_cmd.RegisterAddress;
            MODBUS_RTU_TX_CMD.NumberOfRegisters = modbus_rtu_cmd.NumberOfRegisters;
            MODBUS_RTU_TX_CMD.RegisterValue = modbus_rtu_cmd.RegisterValue;
            MODBUS_RTU_TX_CMD.ErrorCode = modbus_rtu_cmd.ErrorCode;
            ushort crc16_for_cmd = CRC16Calculation.ComputeChecksum(MODBUS_RTU_TX_CMD.GetBytesForCRC16());
            MODBUS_RTU_TX_CMD.CRC16 = crc16_for_cmd;
            MODBUS_RTU_TX_CMD.CreatePacket();
            Send(MODBUS_RTU_TX_CMD.TransmitBytes, MODBUS_TX_RX_message);
        }
        /// <summary>
        /// Written. 2024.03.25 13:12. Warsaw. Workplace.
        /// to packet -> modbus uses cmd_class
        /// </summary>
        /// <param name="to_address"></param>
        /// <param name="function_code"></param>
        /// <param name="reg_address"></param>
        /// <param name="reg_count"></param>
        public void MODBUS_RTU_Send(byte to_address, byte function_code, ushort reg_address, ushort reg_count, byte error_code = 0x86)
        {
            MODBUS_RTU_TX_CMD_Class modbus_cmd = new MODBUS_RTU_TX_CMD_Class();
            modbus_cmd.ToAddress = to_address;
            modbus_cmd.FunctionCode = function_code;
            modbus_cmd.RegisterAddress = reg_address;
            modbus_cmd.NumberOfRegisters = reg_count;
            modbus_cmd.ErrorCode = error_code;
            MODBUS_RTU_Send(modbus_cmd);
        }





        /// <summary>
        /// Written. 2024.04.19 12:26. Warsaw. Workplace. <br></br>
        /// 
        /// </summary>
        class CRC8Class
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
            /// Written. 2024.04.19 12:30. Warsaw. Workplace. <br></br>
            /// Tested. Works. CRC-8/DVB-S2, Poly = 0xD5. 2024.04.19 12:56. Warsaw. Workplace.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public byte ComputeChecksum(byte[] arr_in)
            {
                byte crc_sum = 0;
                for (byte i = 0; i < arr_in.Length; i++)
                {
                    crc_sum = CRC8Table[crc_sum ^ arr_in[i]];
                }
                return crc_sum;
            }
        }






        /// <summary>
        /// Calculates CRC16 of byte[].
        /// Written. 2024.03.22 15:44. Warsaw. Workplace. <br></br>
        /// Tested. Works. 2024.03.22 15:44. Warsaw. Workplace.
        /// </summary>
        public class CRC16Class
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






        private void TimerStartRecieveByLength(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Start();
        }
        private void TimerStoptRecieveByLength(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Stop();
        }
        private void TimerRecieveByLength_NoByteDelay_Tick(object sender, EventArgs e)
        {
        }




        private void TimerRecieveByLength_DelayLastByte_Tick(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Stop();
            TimerRecieveByLength_DelayLastByte.Tick -= TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Tick -= TimerRecieveByLength_NoByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
            Console.WriteLine(nameof(RecieveByLengthInt16) + " error. Reception ended because delay " +
                TimerRecieveByLength_DelayLastByte.Interval.ToString() + " ms was reached without receiving all bytes.\r\n" +
                "Number of bytes to receive is " + RecieveByLength_Length.ToString());
        }
    }
}
