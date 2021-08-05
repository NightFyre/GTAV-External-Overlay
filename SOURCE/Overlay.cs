using System.Runtime.InteropServices;
using GTAV_External_Trainer.Helpers;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Memory;
using System;

namespace Simple_GTAV_External_Trainer
{
    public partial class Overlay : Form
    {

        #region WINDOW SETUP

        public const string WINDOW_NAME = "Grand Theft Auto V";
        IntPtr handle = FindWindow(null, WINDOW_NAME);

        RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        #endregion

        #region PROCESS INFO
        Mem m = new Mem();
        private const string GTAVPROCESSNAME = "GTA5.exe";
        bool gtavRunning;
        private bool bMenuControl = true;
        private bool bAutoShoot = false;
        private bool bGodMode = false;
        private bool bNeverWanted = false;
        private bool bAllOff = false;

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int a, int b, int c, int d, int damnIwonderifpeopleactuallyreadsthis);

        int leftDown = 0x02;
        int leftUp = 0x04;
        int flag = 0;

        #region METHODS
        void shoot(int delay)
        {
            mouse_event(leftDown, 0, 0, 0, 0);
            Thread.Sleep(1);
            mouse_event(leftUp, 0, 0, 0, 0);
            Thread.Sleep(delay);
        }

        void triggerbot()
        {

            while (true)
            {
                if (GetAsyncKeyState(Keys.RButton) < 0)
                {
                    flag = m.ReadInt("GTA5.exe+1FB2380");
                    if (flag >= 1 && bAutoShoot)
                    {
                        shoot(10);
                        shoot(1);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void GodMode()
        {
            var GODMODE = "GTA5.exe+25333D8,0x8,0x189";
            var FLAG = m.ReadByte(GODMODE);
            if (FLAG == 0 && bGodMode)
            {
                m.WriteMemory(GODMODE, "byte", "1");
            }
            if (FLAG == 1 && !bGodMode)
            {
                m.WriteMemory(GODMODE, "byte", "0");
            }
        }

        private void NeverWanted()
        {
            var WANTEDLEVEL = "GTA5.exe+25333D8,0x8,0x10C8,0x888";
            var FLAG = m.ReadByte(WANTEDLEVEL);
            if (FLAG >= 1 && bNeverWanted)
            {
                m.WriteMemory(WANTEDLEVEL, "byte", "0");
            }
        }

        private void ALLOFF()
        {
            if (bAutoShoot && bAllOff)
            {
                bAutoShoot = false;
            }
            if (bGodMode && bAllOff)
            {
                var GODMODE = "GTA5.exe+25333D8,0x8,0x189";
                var FLAG = m.ReadByte(GODMODE);
                if (FLAG == 1)
                {
                    m.WriteMemory(GODMODE, "byte", "0");
                }

                bGodMode = false;
            }
            if (bNeverWanted && bAllOff)
            {
                bNeverWanted = false;
            }
            bAllOff = false;
        }

        #endregion

        #endregion

        #region KEYBINDS
        public void KeyAssign()
        {
            KeysMgr keyMgr = new KeysMgr();
            keyMgr.AddKey(Keys.Insert);     // MENU
            keyMgr.AddKey(Keys.Delete);     // QUIT
            keyMgr.AddKey(Keys.F5);         // Auto Shoot
            keyMgr.AddKey(Keys.F6);         // God Mode
            keyMgr.AddKey(Keys.F7);         // Never Wanted
            keyMgr.AddKey(Keys.F8);         // ALL OFF
            keyMgr.KeyDownEvent += new KeysMgr.KeyHandler(KeyDownEvent);
        }

        private void KeyDownEvent(int Id, string Name)
        {
            switch ((Keys)Id)
            {
                case Keys.Insert:
                    this.bMenuControl = !this.bMenuControl;
                    break;
                case Keys.Delete:
                    Quit();
                    break;
                case Keys.F5:
                    this.bAutoShoot = !this.bAutoShoot;
                    break;
                case Keys.F6:
                    this.bGodMode = !this.bGodMode;
                    break;
                case Keys.F7:
                    this.bNeverWanted = !this.bNeverWanted;
                    break;
                case Keys.F8:
                    this.bAllOff = !this.bAllOff;
                    break;
            }
        }
        #endregion

        #region MAIN FORM

        public Overlay()
        {
            InitializeComponent();
        }

        private void Initialize(object sender, EventArgs e)
        {
            int PID = m.GetProcIdFromName(GTAVPROCESSNAME);
            if (PID > 0)
            {
                m.OpenProcess(PID);
                Thread TB = new Thread(triggerbot) { IsBackground = true };
                TB.Start();
            }
            this.BackColor = Color.Orange;
            this.TransparencyKey = Color.Orange;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            int InitialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, InitialStyle | 0x800000 | 0x20);
            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;
            KeyAssign();
        }

        private void DrawPaint(object sender, PaintEventArgs e)
        {
            if (bMenuControl)
            {
                DrawMenuInfo(sender, e);
            }
        }

        private void Close(object sender, FormClosingEventArgs e)
        {
            ALLOFF();
            Quit();
        }

        private void Quit()
        {
            m.CloseProcess();
            Environment.Exit(0);
        }

        #region TIMERS

        private void ProcessTimer_Tick(object sender, EventArgs e)
        {
            int PID = m.GetProcIdFromName(GTAVPROCESSNAME);
            if (PID > 0)
            {
                m.OpenProcess(PID);
                gtavRunning = true;
                return;
            }
            gtavRunning = false;
        }

        private void MemoryTimer_Tick(object sender, EventArgs e)
        {
            if (!gtavRunning)
            {
                return;
            }
            GodMode();
            NeverWanted();
            ALLOFF();
        }
        
        private void WindowHookTimer_Tick(object sender, EventArgs e)
        {
            GetWindowRect(handle, out rect);
            Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            Top = rect.top;
            Left = rect.left;
            this.Refresh();
        }

        #endregion

        #endregion

        #region DRAW

        public void DrawMenuInfo(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black);
            Brush redBrush = new SolidBrush(Color.Red);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush GreenBrush = new SolidBrush(Color.LimeGreen);
            Brush blackFill = new SolidBrush(Color.FromArgb(0, 0, 0));
            Font HeaderFont = new Font("Arial", 16);
            Font InfoTextFont = new Font("Arial", 10);

            //Menu Position 1 || TOP LEFT
            PointF HeaderTextPos = new PointF(0.0F, 25.0F);
            PointF MenuOption1Pos = new PointF(2.0F, 50.0F);
            PointF MenuOption2Pos = new PointF(2.0F, 70.0F);
            PointF MenuOption3Pos = new PointF(2.0F, 90.0F);
            PointF MenuOption4Pos = new PointF(2.0F, 120.0F);
            PointF MenuOption5Pos = new PointF(2.0F, 135.0F);
            PointF MenuOption6Pos = new PointF(2.0F, 150.0F);
            Rectangle InfoBox = new Rectangle(3, 26, 155, 140);

            //Menu Position 2 || BOTTOM RIGHT
            Rectangle TestBox = new Rectangle(1750, 921, 150, 140);
            PointF HeaderTextPosTest = new PointF(1747.0F, 920.0F);
            PointF MenuOption1PosTest = new PointF(1749.0f, 945.0F);
            PointF MenuOption2PosTest = new PointF(1749.0f, 965.0F);
            PointF MenuOption3PosTest = new PointF(1749.0f, 985.0F);
            PointF MenuOption4PosTest = new PointF(1749.0f, 1015.0F);
            PointF MenuOption5PosTest = new PointF(1749.0f, 1030.0F);
            PointF MenuOption6PosTest = new PointF(1749.0f, 1045.0F);

            //g.FillRectangle(blackFill, InfoBox);
            g.FillRectangle(blackFill, TestBox);
            g.DrawString("NightFyreTV", HeaderFont, redBrush, HeaderTextPosTest);

            if (!bAutoShoot)
            {
                g.DrawString("[F5] AUTOSHOOT", InfoTextFont, whiteBrush, MenuOption1PosTest);
            }
            else
            {
                g.DrawString("[F5] AUTOSHOOT", InfoTextFont, GreenBrush, MenuOption1PosTest);
            }
            if (!bGodMode)
            {
                g.DrawString("[F6] GOD MODE", InfoTextFont, whiteBrush, MenuOption2PosTest);
            }
            else
            {
                g.DrawString("[F6] GOD MODE", InfoTextFont, GreenBrush, MenuOption2PosTest);
            }
            if (!bNeverWanted)
            {
                g.DrawString("[F7] NEVER WANTED", InfoTextFont, whiteBrush, MenuOption3PosTest);
            }
            else
            {
                g.DrawString("[F7] NEVER WANTED", InfoTextFont, GreenBrush, MenuOption3PosTest);
            }
            g.DrawString("ALL OFF [F8]", InfoTextFont, redBrush, MenuOption4PosTest);
            g.DrawString("SHOW / HIDE [INSERT]", InfoTextFont, redBrush, MenuOption5PosTest);
            g.DrawString("QUIT MENU [DELETE]", InfoTextFont, redBrush, MenuOption6PosTest);
        }

        #endregion

    }
}
