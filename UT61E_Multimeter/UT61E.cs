using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace UT61E_Multimeter
{
    public partial class UT61E : Form
    {
        static SerialPort serial;

        //Incoming data buffer
        //StringBuilder msg;
        List<byte> msg;

        //Count of serial com ports
        int portCount;

        //If we think the serial port is open
        bool portOpen = false;

        //Descriptions of serial com ports
        List<string> portDescriptions;

        //Serial data validity
        enum DataState { NONE, OK, BAD };
        DataState serialState = DataState.NONE;

        public UT61E()
        {
            InitializeComponent();
            cbxPorts.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            cbxPorts.ComboBox.DrawItem += cbxDrawItem;
            cbxPorts.ComboBox.DropDownClosed += cbxDropDownClosed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new SerialPort object with default settings.  
            serial = new SerialPort();
            msg = new List<byte>();
            portDescriptions = new List<string>();
            ScanPorts();
        }

        private void cbxPorts_DropDown(object sender, EventArgs e)
        {
            ScanPorts();
        }


        void ScanPorts()
        {
            cbxPorts.Text = "Scanning..";
            cbxPorts.Items.Clear();
            portDescriptions.Clear();
            string[] portNames = SerialPort.GetPortNames();
            portCount = portNames.Length;
            if (portCount == 0)
                cbxPorts.Text = "(No Ports)";
            else
            {
                List<string> ports = new List<string>();
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
                {
                    var portDesc = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString()); 
                    var portList = portDesc.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();
                    foreach (string s in portList)
                    {
                        int pos = s.IndexOf('(');
                        portDescriptions.Add(s.Substring(0, pos-1));
                        ports.Add(s.Substring(pos + 1, s.IndexOf(')') - pos - 1));
                    }
                }
                cbxPorts.Items.AddRange(ports.ToArray());
                if (ports.Contains<string>(Properties.Settings.Default.Port))
                    cbxPorts.Text = Properties.Settings.Default.Port;
                else
                    cbxPorts.Text = ports[0];
            }
        }

        //Show port description tooltip
        void cbxDrawItem(object sender, DrawItemEventArgs e)
        {
            string text = cbxPorts.ComboBox.GetItemText(cbxPorts.ComboBox.Items[e.Index]);
            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }

            if (cbxPorts.DroppedDown && (e.State & DrawItemState.Selected) == DrawItemState.Selected)
                toolTip1.Show(portDescriptions[e.Index], cbxPorts.ComboBox, e.Bounds.Right + 3, e.Bounds.Bottom + 6);
            else
                toolTip1.Hide(cbxPorts.ComboBox);
            e.DrawFocusRectangle();
        }

        private void cbxDropDownClosed(object sender, EventArgs e)
        {
            toolTip1.Hide(cbxPorts.ComboBox);
        }

        //Close the serial port and tidy up
        void ClosePort()
        {
            serial.Close();
            statusLbl.Text = "Not Connected";
            btnConnect.Text = "Connect";
            portOpen = false;
            serialState = DataState.NONE;
            display1.Reset();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Port already open?
            if (serial.IsOpen)
            {
                ClosePort();
                return;
            }
            //Check selected port is reasonable
            if (cbxPorts.Text.ToLower().Contains("no ports"))
            {
                statusLbl.Text = "No COM Ports!";
                return;
            }
            if (!cbxPorts.Text.ToLower().Contains("com"))
            {
                statusLbl.Text = "Please Choose Port";
                return;
            }

            // Set the appropriate port properties.  
            serial.PortName = cbxPorts.Text;
            serial.BaudRate = 19200;
            serial.Parity = Parity.Odd;
            serial.DataBits = 7;
            serial.StopBits = StopBits.One;
            serial.Handshake = Handshake.None;
            serial.DtrEnable = true;
            // Set the read/write timeouts  
            serial.ReadTimeout = 500;
            serial.WriteTimeout = 500;

            //Open the port
            try { serial.Open(); }
            catch(Exception ex) {
                if (ex.GetType() == typeof(ArgumentException))
                    statusLbl.Text = "Not a COM Port";
                else if (ex.GetType() == typeof(UnauthorizedAccessException))
                    statusLbl.Text = "Unauthorized";
                else if (ex.GetType() == typeof(ArgumentOutOfRangeException))
                    statusLbl.Text = "Out Of Range";
                else if (ex.GetType() == typeof(IOException))
                    statusLbl.Text = "IO Exception";
                else if (ex.GetType() == typeof(InvalidOperationException))
                    statusLbl.Text = "Port Already Open";
                else
                    statusLbl.Text = "Unknown Error?";
                return;
            }
            //Success - show user
            statusLbl.Text = "Connected, Waiting for Data..";
            btnConnect.Text = "Disconnect";
            //Set to go
            Properties.Settings.Default.Port = cbxPorts.Text;
            portOpen = true;
            RecieveData();
        }

        //https://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport
        private void RecieveData()
        {
            Action kickoffRead = null;
            kickoffRead = delegate {
                if (serial.IsOpen)
                {
                    var buffer = new byte[4096];
                    serial.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
                    {
                        try
                        {
                            int actualLength = serial.BaseStream.EndRead(ar);
                            byte[] received = new byte[actualLength];
                            Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                            raiseAppSerialDataEvent(received);
                        }
                        catch (IOException exc)
                        {
                            handleAppSerialError(exc);
                        }
                        kickoffRead();
                    }, null);
                }
            };
            kickoffRead();
        }

        private void handleAppSerialError(IOException ex)
        {
            if (ex.GetType() == typeof(ArgumentException))
                statusLbl.Text = "Not a COM Port";
            else if (ex.GetType() == typeof(UnauthorizedAccessException))
                statusLbl.Text = "Unauthorized";
            else if (ex.GetType() == typeof(ArgumentOutOfRangeException))
                statusLbl.Text = "Out Of Range";
            else if (ex.GetType() == typeof(IOException))
                statusLbl.Text = "IO Exception";
            else if (ex.GetType() == typeof(InvalidOperationException))
                statusLbl.Text = "Port Not Open";
            else
                statusLbl.Text = "Unknown Error?";
        }

        private void raiseAppSerialDataEvent(byte[] received)
        {
            //Add received data to buffer
            msg.AddRange(received);
            
            //Look for linefeed, '\n'
            if (msg.Contains(10))       
            {
                //Extract line
                int pos = msg.IndexOf(10);
                byte[] line = msg.GetRange(0, pos - 1).ToArray();
                //Trim buffer
                msg.RemoveRange(0, pos + 1);
                //Parse and check result
                DataState res = Parse(line);
                if (res != serialState)
                {
                    serialState = res;
                    if (serialState == DataState.OK) statusLbl.Text = "Connected OK";
                    else if (serialState == DataState.BAD) statusLbl.Text = "Connected, Bad Data!";
                }
            }
            /*
            string input = Encoding.ASCII.GetString(received);
            //Append to buffer
            msg.Append(input);
            //Got full line?
            if (msg.ToString().Contains('\n'))
            {
                int pos = msg.ToString().IndexOf('\n');
                string line = msg.ToString().Substring(0, pos).Trim();
                //Trim buffer
                msg.Remove(0, pos + 1);
                //Parse and check result
                DataState res = Parse(line);
                if (res != serialState)
                {
                    serialState = res;
                    if (serialState == DataState.OK) statusLbl.Text = "Connected OK";
                    else if (serialState == DataState.BAD) statusLbl.Text = "Connected, Bad Data!";
                }
            }
            */
        }

/*        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (InvokeRequired)
            {
                //Call back into function from UI Thread
                try { this.Invoke(call_DataReceived, new object[] { sender, e }); }
                // ..unless the UI has been closed.
                catch (InvalidOperationException) { if (this.IsHandleCreated) throw; }
            }
            else
            {
                //Read existing data
                SerialPort sp = (SerialPort)sender;
                string input;
                try { input = sp.ReadExisting(); }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ArgumentException))
                        statusLbl.Text = "Not a COM Port";
                    else if (ex.GetType() == typeof(UnauthorizedAccessException))
                        statusLbl.Text = "Unauthorized";
                    else if (ex.GetType() == typeof(ArgumentOutOfRangeException))
                        statusLbl.Text = "Out Of Range";
                    else if (ex.GetType() == typeof(IOException))
                        statusLbl.Text = "IO Exception";
                    else if (ex.GetType() == typeof(InvalidOperationException))
                        statusLbl.Text = "Port Not Open";
                    else
                        statusLbl.Text = "Unknown Error?";
                    return;
                }
                //Append to buffer
                msg.Append(input);
                //Got full line?
                if (msg.ToString().Contains('\n'))
                {
                    int pos = msg.ToString().IndexOf('\n');
                    string line = msg.ToString().Substring(0, pos).Trim();
                    //Trim buffer
                    msg.Remove(0, pos + 1);
                    //Parse and check result
                    DataState res = Parse(line);
                    if (res != serialState)
                    {
                        serialState = res;
                        if (serialState == DataState.OK) statusLbl.Text = "Connected OK";
                        else if (serialState == DataState.BAD) statusLbl.Text = "Connected, Bad Data!";
                    }
                }
            }
        }
*/
        
        /*
        Protocol:
        
        Byte	Meaning
        ====	=======
        0x00	Measurement range
                bit 6-3: always 0110
                bit 2-0: measurement range. See byte 6 for details.

        0x01	Digit 1
                bit 6-4: always 011
                bit 3-0
			        0000: 0
			        0001: 1
			        0010: 2
			        0011: 3
			        0100: 4
			        0101: 5
			        0110: 6
			        0111: 7
			        1000: 8
			        1001: 9

        0x02	Digit 2
                bit 6-4: always 011
                bit 3-0: See byte 0x01

        0x03	Digit 3
                bit 6-4: always 011
                bit 3-0: See byte 0x01

        0x04	Digit 4
                bit 6-4: always 011
                bit 3-0: See byte 0x01

        0x05	Digit 5
                bit 6-4: always 011
                bit 3-0: See byte 0x01

        0x06	DMM mode
                bit 6-4: always 011
                bit 3-0: Measurement mode
                    Byte 6:  0xB       0x3       0x6       0x2       0xD       0xF       0x0      0x2
                    Byte 0:  V, mV     Ohm       F         Hz        uA        mA        A        %
                        0    2.2000    220.00    22.000n   220.00    220.00u   22.000m   10.000   100.0
                        1    22.000    2.2000k   220.00n   2200.0    2200.0u   220.00m   -        100.0
                        2    220.00    22.000k   2.2000u   -         -         -         -        -
                        3    1000.0    220.00k   22.000u   22.000k   -         -         -        100.0
                        4    220.00m   2.200M    220.00u   220.00k   -         -         -        100.0
                        5    -         22.000M   2.2000m   2.2000M   -         -         -        100.0
                        6    -         220.00M   22.000m   22.000M   -         -         -        100.0
                        7    -         -         220.00m   220.00M   -         -         -        100.0

        0x07	Info flags
                bit 6-4: always 011
                bit 3:   percent mode
                bit 2:   minus (negative value)
                bit 1:   low battery
                bit 0:   OL (overload)

        0x08	Relative mode flags
                bit 6-4: always 011
                bit 3:   MAX (unused in this project)
                bit 2:   MIN (unused in this project)
                bit 1:   relative mode (delta)
                bit 0:   [RMR] (unused in this project)

        0x09	Limit flags
                bit 6-4: always 011
                bit 3:   UL (underload)
                bit 2:   Peak max
                bit 1:   Peak min
                bit 0:   always 0

        0x0A	Voltage and auto range flags
                bit 6-4: always 011
                bit 3:   DC measurement
                bit 2:   AC measurement
                bit 1:   auto range
                bit 0:   frequency measurement (Hz)
        
        0x0B	Hold
                bit 6-4: always 011
                bit 3:   always 0
                bit 2:   VBAR (unused in this project)
                bit 1:   data hold
                bit 0:   LPF (unused in this project)

        0x0C	Footer, always 0x0D (\r)
        0x0D	Footer, always 0x0A (\n)
        */

        //Parse a UT61E Serial packet
        private DataState Parse(byte[] bytes)
        {
            if (bytes.Length == 12)
            {
                for (int test = 0; test < bytes.Length; test++)
                    if ((bytes[test] & 0b00110000) != 0b00110000)
                        return DataState.BAD;
                display1.Range = bytes[0] & 7;
                display1.Digits = "";
                display1.Digits += Convert.ToChar((bytes[1] & 0x0F) + 0x30);
                display1.Digits += Convert.ToChar((bytes[2] & 0x0F) + 0x30);
                display1.Digits += Convert.ToChar((bytes[3] & 0x0F) + 0x30);
                display1.Digits += Convert.ToChar((bytes[4] & 0x0F) + 0x30);
                display1.Digits += Convert.ToChar((bytes[5] & 0x0F) + 0x30);
                display1.Mode = bytes[6] & 0x0F;
                display1.Pct = (bytes[7] & 0x08) > 0;
                display1.Minus = (bytes[7] & 0x04) > 0;
                display1.Lowbatt = (bytes[7] & 0x02) > 0;
                display1.Ol = (bytes[7] & 0x01) > 0;
                display1.Max = (bytes[8] & 0x08) > 0;
                display1.Min = (bytes[8] & 0x04) > 0;
                display1.Delta = (bytes[8] & 0x02) > 0;
                display1.Ul = (bytes[9] & 0x08) > 0;
                display1.Pmax = (bytes[9] & 0x04) > 0;
                display1.Pmin = (bytes[9] & 0x02) > 0;
                display1.Dc = (bytes[10] & 0x08) > 0;
                display1.Ac = (bytes[10] & 0x04) > 0;
                display1.Auto = (bytes[10] & 0x02) > 0;
                display1.Freq = (bytes[10] & 0x01) > 0;
                display1.Hold = (bytes[11] & 0x02) > 0;
                display1.Invalidate();
                return DataState.OK;
            }
            return DataState.BAD;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            //Port closed unexpectedly?
            if (portOpen && !serial.IsOpen)
            {
                ClosePort();
            }
            //ports have changed and selected port no longer exists or no port selected
            if ((ports.Length != portCount) && !ports.Contains<string>(cbxPorts.Text))
            {
                ClosePort();
                ScanPorts();
            }
        }

  
    }
}
