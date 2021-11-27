using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System;

namespace UT61E_Multimeter
{
    //Multimeter display

    public partial class display : UserControl
    {
        private PrivateFontCollection private_fonts;
        private Font digiFont;
        private Font boldFont;
        private Font bigFont;

        private enum ModeType { n = 0, v = 1, o = 2, f = 3, h = 4, u = 5, m = 6, a = 7, d = 8, c = 9, t = 10 };

        private ModeType[] modeTable = { ModeType.a, ModeType.d, ModeType.h, ModeType.o, ModeType.t, ModeType.c, ModeType.f, ModeType.n, ModeType.n, ModeType.n, ModeType.n, ModeType.v, ModeType.n, ModeType.u, ModeType.n, ModeType.m };
        private int[,] dpTable = {
            { -1, 0, 2, 1, 2, 2, 1, 1, 2, 0, 2 },
            { -1, 1, 0, 2, 3, 3, 2, 0, 2, 0, 2 },
            { -1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 2 },
            { -1, 3, 2, 1, 1, 0, 0, 0, 2, 0, 2 },
            { -1, 2, 0, 2, 2, 0, 0, 0, 2, 0, 2 },
            { -1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 2 },
            { -1, 0, 2, 1, 1, 0, 0, 0, 2, 0, 2 },
            { -1, 0, 0, 2, 2, 0, 0, 0, 2, 0, 2 }
        };

        enum MulType { n, v, mv, r, kr, mr, f, nf, uf, mf, h, kh, mh, a, ua, ma, p };

        private MulType[,] mulTable = {
            { MulType.n, MulType.v, MulType.r, MulType.nf, MulType.h, MulType.ua, MulType.ma, MulType.a, MulType.p, MulType.v, MulType.r },
            { MulType.n, MulType.v, MulType.kr, MulType.nf, MulType.h, MulType.ua, MulType.ma, MulType.n, MulType.p, MulType.v, MulType.r  },
            { MulType.n, MulType.v, MulType.kr, MulType.uf, MulType.n, MulType.n, MulType.n, MulType.n, MulType.n, MulType.v, MulType.r  },
            { MulType.n, MulType.v, MulType.kr, MulType.uf, MulType.kh, MulType.n, MulType.n, MulType.n, MulType.p, MulType.v, MulType.r  },
            { MulType.n, MulType.mv, MulType.mr, MulType.uf, MulType.kh, MulType.n, MulType.n, MulType.n, MulType.p, MulType.v, MulType.r  },
            { MulType.n, MulType.n, MulType.mr, MulType.mf, MulType.mh, MulType.n, MulType.n, MulType.n, MulType.p, MulType.v, MulType.r  },
            { MulType.n, MulType.n, MulType.mr, MulType.mf, MulType.mh, MulType.n, MulType.n, MulType.n, MulType.p, MulType.v, MulType.r  },
            { MulType.n, MulType.n, MulType.n, MulType.mf, MulType.mh, MulType.n, MulType.n, MulType.n, MulType.p, MulType.v, MulType.r  }
        };

        public int Range { get; set; }
        public string Digits { get; set; }
        public int Mode { get; set; }
        public bool Pct { get; set; }
        public bool Minus { get; set; }
        public bool Lowbatt { get; set; }
        public bool Ol { get; set; }
        public bool Max { get; set; }
        public bool Min { get; set; }
        public bool Delta { get; set; }
        public bool Ul { get; set; }
        public bool Pmax { get; set; }
        public bool Pmin { get; set; }
        public bool Dc { get; set; }
        public bool Ac { get; set; }
        public bool Auto { get; set; }
        public bool Freq { get; set; }
        public bool Hold { get; set; }

        public display()
        {
            Reset();
            private_fonts = new PrivateFontCollection();
            LoadFont();
            InitializeComponent();
            BackColor = Color.FromArgb(0xFB, 0x7C, 0x00);
            digiFont = new Font(private_fonts.Families[0], 44);
            boldFont = new Font(this.Font, FontStyle.Bold);
            bigFont = new Font(boldFont.FontFamily, 14, FontStyle.Bold);
            this.Paint += Display_Paint;
        }

        ~display()
        {
            private_fonts.Dispose();
        }

        public void Reset()
        {
            Range = 0;
            Digits = "-----";
            Mode = 7;
            Pct = false;
            Minus = false;
            Lowbatt = false;
            Ol = false;
            Max = false;
            Min = false;
            Delta = false;
            Ul = false;
            Pmax = false;
            Pmin = false;
            Dc = false;
            Ac = false;
            Auto = true;
            Freq = false;
            Hold = false;
            this.Invalidate();
        }

        void LoadFont()
        {

            // receive resource stream
            byte[] fontdata = Properties.Resources.seg7;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem((int)fontdata.Length);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, (int)fontdata.Length);

            // pass the font to the font collection
            private_fonts.AddMemoryFont(data, (int)fontdata.Length);

            // free up the unsafe memory
            Marshal.FreeCoTaskMem(data);
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
            Brush bg = new SolidBrush(Color.FromArgb(0xE9, 0x73, 0x00));
            Brush fg = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22));

            //Sort out the mode and range
            ModeType modeIndex = modeTable[Mode];
            if (Freq) modeIndex = ModeType.h;               //Frequency mode select
            int dp = dpTable[Range, (int)modeIndex] + (Pct ? 1 : 0);
            int barVal = 0;             //Bargraph value
            int leadingZeros = 0;       //Number of leading zeros to strip
            int digitStart = 45;        //X-Coord of digits where to start drawing

            //Read the actual measurement value
            int value;
            bool gotValue = int.TryParse(Digits, out value);
            if (gotValue)
            {
                //Scale to unit value
                value /= (int)Math.Pow(10.0,  (double)(4 - dp));
                barVal = value / 5;  //Keep the value for the bargraph
                leadingZeros = dp;  //Starting with all decimal places,
                if (value >= 10) leadingZeros--;    //remove
                if (value >= 100) leadingZeros--;   //decimals
                if (value >= 1000) leadingZeros--;  //as
                if (value >= 10000) leadingZeros--; //required
            }
            //Strip leading zeros
            string dig = Digits.Substring(leadingZeros);
            //Move start position past blanked zeros
            digitStart += leadingZeros * 60;
            
            //Draw digit background
            e.Graphics.DrawString("-88888", digiFont, bg, new Point(-15, 40));
            //Draw background decimal points as well as current decimal point
            e.Graphics.DrawString("        .", digiFont, dp == 0 ? fg : bg, new Point(-16, 40));
            e.Graphics.DrawString("            .", digiFont, dp == 1 ? fg : bg, new Point(-16, 40));
            e.Graphics.DrawString("                .", digiFont, dp == 2 ? fg : bg, new Point(-16, 40));
            e.Graphics.DrawString("                    .", digiFont, dp == 3 ? fg : bg, new Point(-16, 40));
            //Draw minus sign
            if (Minus) e.Graphics.DrawString("-", digiFont, fg, new Point(-16, 40));
            //Overload indication
            if (Ol)
                e.Graphics.DrawString("0L", digiFont, fg, new Point(105, 40));
            //Underload indication
            else if (Ul)
                e.Graphics.DrawString("VL", digiFont, fg, new Point(105, 40));
            //Draw normal value
            else
                e.Graphics.DrawString(dig, digiFont, fg, new Point(digitStart, 40));

            //Draw bargraph
            for (int n = 0; n < 45; n++)
            {
                Brush col = n <= barVal || Ol ? fg : bg;
                int x = 25 + (n * 8);
                if (n % 10 != 0)
                {
                    if (n != 44) e.Graphics.FillRectangle(col, new Rectangle(x, 150 - ((n - 1) % 2) * 2, 4, 10 + ((n - 1) % 2) * 2));
                    else e.Graphics.FillPolygon(col, new Point[] { new Point(x, 144), new Point(x, 160), new Point(x + 15, 152) });
                }
                else
                {
                    e.Graphics.FillRectangle(col, new Rectangle(x, 145, 4, 15));
                    e.Graphics.DrawString((n / 2).ToString(), boldFont, fg, x - 5 - (n / 2 > 9 ? 5 : 0), 165);
                }
            }

            //Draw the unit and multipliers, icons and images.
            MulType mul = mulTable[Range, (int)modeIndex];
            e.Graphics.FillRectangle(Minus ? fg : bg, new Rectangle(10, 151, 10, 4));
            e.Graphics.DrawString("OL", boldFont, Ol ? fg : bg, 394, 144);
            e.Graphics.DrawString("μ", bigFont, mul == MulType.ua ? fg : bg, 346, 4);
            e.Graphics.DrawString("m", bigFont, mul == MulType.ma || mul == MulType.mv ? fg : bg, 360, 4);
            e.Graphics.DrawString("V", bigFont, modeIndex == ModeType.v ? fg : bg, 380, 4);
            e.Graphics.DrawString("A", bigFont, modeIndex == ModeType.a || modeIndex == ModeType.u || modeIndex == ModeType.m? fg : bg, 396, 4);
            e.Graphics.DrawString("C", bigFont, bg, 360, 24);
            e.Graphics.DrawString("F", bigFont, bg, 378, 24);
            e.Graphics.DrawString("%", bigFont, modeIndex == ModeType.h && Pct ? fg : bg, 393, 24);
            e.Graphics.DrawString("M", bigFont, mul == MulType.mr || mul == MulType.mh ? fg : bg, 360, 44);
            e.Graphics.DrawString("k", bigFont, mul == MulType.kr || mul == MulType.kh ? fg : bg, 380, 44);
            e.Graphics.DrawString("Ω", bigFont, modeIndex == ModeType.o ? fg : bg, 393, 44);
            e.Graphics.DrawString("Hz", bigFont, (modeIndex == ModeType.h || Freq) && !Pct ? fg : bg, 382, 64);
            e.Graphics.DrawString("m", bigFont, mul == MulType.mf ? fg : bg, 350, 84);
            e.Graphics.DrawString("μ", bigFont, mul == MulType.uf ? fg : bg, 370, 84);
            e.Graphics.DrawString("n", bigFont, mul == MulType.nf ? fg : bg, 384, 84);
            e.Graphics.DrawString("F", bigFont, modeIndex == ModeType.f ? fg : bg, 398, 84);
            e.Graphics.DrawString("MIN", boldFont, Min ? fg : bg, 356, 112);
            e.Graphics.DrawString("MAX", boldFont, Max ? fg : bg, 383, 112);
            e.Graphics.DrawString("Pmin", boldFont, Pmin ? fg : bg, 344, 126);
            e.Graphics.DrawString("Pmax", boldFont, Pmax ? fg : bg, 378, 126);
            e.Graphics.DrawString("DC", bigFont, Dc ? fg : bg, 60, 5);
            e.Graphics.DrawString("AC", bigFont, Ac ? fg : bg, 96, 5);
            e.Graphics.DrawString("AUTO", bigFont, Auto ? fg : bg, 120, 115);
            e.Graphics.DrawString("MANU", bigFont, Auto ? bg : fg, 200, 115);
            picBatt.Image = Lowbatt ? Properties.Resources.batt : Properties.Resources.lbatt;
            picHold.Image = Hold ? Properties.Resources.hold : Properties.Resources.lhold;
            //picEF.Image = ef ? Properties.Resources.lhold : Properties.Resources.hold;
            picDiode.Image = modeIndex == ModeType.d ? Properties.Resources.diode : Properties.Resources.ldiode;
            picBeep.Image = modeIndex == ModeType.c ? Properties.Resources.beeper : Properties.Resources.lbeeper;
            picDelta.Image = Delta ? Properties.Resources.delta : Properties.Resources.ldelta;
            bg.Dispose();
            fg.Dispose();
        }


    }
}
