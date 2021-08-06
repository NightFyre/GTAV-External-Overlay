﻿using Simple_GTAV_External_Trainer.Helpers;
using System.Runtime.InteropServices;
using GTAV_External_Trainer.Helpers;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Memory;
using System;
using System.Collections.Generic;

#region LICENSE INFORMATION :: IMPORTANT
//“Commons Clause” License Condition v1.0
//
//The Software is provided to you by the Licensor under the License, as defined below, subject to the following condition.
//
//Without limiting other conditions in the License, the grant of rights under the License will not include, and the License does not grant to you, the right to Sell the Software.
//
//For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide to third parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support services related to the Software), a product or service whose value derives, entirely or substantially, from the functionality of the Software. Any license notice or attribution required by the License must also include this Commons Clause License Condition notice.
//
//Software: GTAV - External - Overlay
//License: MIT
//Licensor: xCENTx
//--------------------------------------------------------------------------------
//MIT License

//Copyright (c) 2021 xCENTx

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
//----------------------------------------------------------------------------------
#endregion

///Created by NightFyreTV
// Simple GTA V External Overlay
// Version 1.6

///Contributors
// - RasqueP

///FEATURES
// TRIGGER BOT
// GOD MODE
// INFINITE AMMO
// NEVER WANTED
// PERFECT WEAPON
// RP BOOSTER
// TUNE CAR

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

        static Mem m = new Mem();
        private const string GTAVPROCESSNAME = "GTA5.exe";
        bool gtavRunning;
        private bool bMenuControl = true;
        private bool bAutoShoot = false;
        private bool bGodMode = false;
        private bool bNeverWanted = false;
        private bool bAllOff = false;
        private bool bControllerMode = false;
        private bool bInfiniteAmmo = false;
        private bool bRPBoost = false;

        ///Weapondata
        private Weapon weapon = new Weapon(m);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int a, int b, int c, int d, int damnIwonderifpeopleactuallyreadsthis);

        int leftDown = 0x02;
        int leftUp = 0x04;
        int flag = 0;

        #region METHODS

        #region FUNCTION KEYS
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
                flag = m.ReadInt(gData.ENEMY_IN_CROSSHAIRS);
                var ISZOOMED = m.ReadInt(gData.IS_ZOOMED);
                var ENTITY = m.ReadInt(gData.ENTITY);

                if ((GetAsyncKeyState(Keys.RButton) < 0) && (!bControllerMode))
                {
                    if (flag >= 1 && bAutoShoot)
                    {
                        if (ISZOOMED == 1 && ENTITY != 0)
                        {
                            shoot(7);
                            shoot(1);
                        }
                        //Patch for First Person
                        if (ENTITY != 0)
                        {
                            shoot(7);
                            shoot(1);
                        }
                    }
                    //Patch for Scoped Weapons
                    else if (ENTITY != 0 && bAutoShoot)
                    {
                        shoot(7);
                        shoot(1);
                    }
                    
                }

                //Controller mode because why not
                else if ((bControllerMode) && (GetAsyncKeyState(Keys.RButton) == 0))
                {
                    if (flag >= 1 && bAutoShoot)
                    {
                        if (ISZOOMED == 1 && ENTITY != 0)
                        {
                            shoot(7);
                            shoot(1);
                        }
                        //Patch for First Person
                        if (ENTITY != 0)
                        {
                            shoot(7);
                            shoot(1);
                        }
                    }
                    //Patch for Scoped Weapons
                    else if ((ISZOOMED == 1 && ENTITY != 0) && (bAutoShoot))
                    {
                        shoot(7);
                        shoot(1);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void GODMODE()
        {
            if (m.ReadByte(gData.GODMODE) == 0 && bGodMode)
            {
                m.WriteMemory(gData.GODMODE, "byte", "1");
            }
            if (m.ReadByte(gData.GODMODE) == 1 && !bGodMode)
            {
                m.WriteMemory(gData.GODMODE, "byte", "0");
            }
        }

        private void NEVERWANTED()
        {
            if (m.ReadByte(gData.WANTED_LEVEL) >= 1 && bNeverWanted)
            {
                m.WriteMemory(gData.WANTED_LEVEL, "byte", "0");
            }
        }
        #endregion

        #region NUMPAD
        private void INFINITEAMMO()
        {
            if (m.ReadByte(gData.INFINITE_AMMO) != 2 && bInfiniteAmmo)
            {
                m.FreezeValue(gData.INFINITE_AMMO, "byte", "2");
            }
            else if (m.ReadByte(gData.INFINITE_AMMO) != 0 && !bInfiniteAmmo)
            {
                m.UnfreezeValue(gData.INFINITE_AMMO);
                m.WriteMemory(gData.INFINITE_AMMO, "byte", "0");
            }
        }

        private void WANTEDHACK()
        {
            while (true)
            {
                if (bRPBoost)
                {
                    RPBOOSTER();
                }
            }
        }

        private void RPBOOSTER()
        {
            m.WriteMemory(gData.WANTED_LEVEL, "byte", "5");
            Thread.Sleep(500);
            m.WriteMemory(gData.WANTED_LEVEL, "byte", "0");
            Thread.Sleep(500);
        }
        #endregion

        #region CLEAN UP
        private void ALLOFF()
        {
            if (bAutoShoot && bAllOff)
            {
                bAutoShoot = false;
                if (bControllerMode)
                    bControllerMode = false;
            }
            if (bGodMode && bAllOff)
            {
                if (m.ReadByte(gData.GODMODE) == 1)
                {
                    m.WriteMemory(gData.GODMODE, "byte", "0");
                }

                bGodMode = false;
            }
            if (bNeverWanted && bAllOff)
            {
                bNeverWanted = false;
            }
            if (bInfiniteAmmo && bAllOff)
            {
                if (m.ReadByte(gData.INFINITE_AMMO) == 2)
                {
                    m.UnfreezeValue(gData.INFINITE_AMMO);
                    m.WriteMemory(gData.INFINITE_AMMO, "byte", "0");
                }
                bInfiniteAmmo = false;
            }
            if (bRPBoost && bAllOff)
            {
                bRPBoost = false;
            }

            #region PERFECT WEAPON DISABLE
            if (weapon.bPerfectWeapon && bAllOff)
            {
                weapon.ResetStats();
                weapon.bPerfectWeapon = false;
            }
            #endregion

            bAllOff = false;
        }

        private void EJECT()
        {
            bAllOff = true;
            ALLOFF();

            //Just in case , do a final sweep
            if (m.ReadByte(gData.GODMODE) == 1)
            {
                m.WriteMemory(gData.GODMODE, "byte", "0");
            }
            if (m.ReadByte(gData.INFINITE_AMMO) == 2)
            {
                m.UnfreezeValue(gData.INFINITE_AMMO);
                m.WriteMemory(gData.INFINITE_AMMO, "byte", "0");
            }

            #region WEAPON DATA

            weapon.ResetStats();
            
            #endregion

            bAutoShoot = false;
            bGodMode = false;
            bNeverWanted = false;
            bControllerMode = false;
            bInfiniteAmmo = false;
            weapon.bPerfectWeapon = false;
            bRPBoost = false;
        }
        #endregion

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
            keyMgr.AddKey(Keys.F8);         // CONTROLLER MODE
            keyMgr.AddKey(Keys.F9);         // ALL OFF
            keyMgr.AddKey(Keys.NumPad1);    // INFINITE AMMO
            keyMgr.AddKey(Keys.NumPad2);    // RPBooster
            keyMgr.AddKey(Keys.NumPad3);    // Perfect Weapon (SOCOM INSPIRED)
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
                    this.bControllerMode = !this.bControllerMode;
                    break;
                case Keys.F9:
                    this.bAllOff = !this.bAllOff;
                    break;
                case Keys.NumPad1:
                    this.bInfiniteAmmo = !this.bInfiniteAmmo;
                    break;
                case Keys.NumPad2:
                    this.bRPBoost = !this.bRPBoost;
                    break;
                case Keys.NumPad3:
                    this.weapon.bPerfectWeapon = !this.weapon.bPerfectWeapon;
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
                Thread WANTEDXPHACK = new Thread(WANTEDHACK) { IsBackground = true };
                WANTEDXPHACK.Start();
                Thread PERFECTWEAPONHACK = new Thread(weapon.WEAPONHACK) { IsBackground = true };
                PERFECTWEAPONHACK.Start();
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
            Quit();
        }

        private void Quit()
        {
            EJECT();
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
            GODMODE();
            NEVERWANTED();
            INFINITEAMMO();
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

            ///DEBUG
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
            Rectangle TestBox = new Rectangle(1750, 881, 150, 180);     //BOX POSITIION

            PointF HeaderTextPosTest = new PointF(1747.0F, 880.0F);     //NIGHTFYRETV

            PointF MenuOption1PosTest = new PointF(1749.0f, 905.0F);    //TRIGGERBOT
            PointF MenuOption2PosTest = new PointF(1749.0f, 920.0F);    //GODMODE
            PointF MenuOption3PosTest = new PointF(1749.0f, 935.0F);    //NEVERWANTED
            PointF MenuOption4PosTest = new PointF(1749.0f, 950.0F);    //CONTROLLER
            PointF MenuOption8PosTest = new PointF(1749.0f, 970.0f);    //INFINITE AMMO
            PointF MenuOption9PosTest = new PointF(1749.0f, 985.0f);    //RP BOOSTER
            PointF MenuOption10PosTest = new PointF(1749.0f, 1000.0f);    //Perfect Weapon

            PointF MenuOption5PosTest = new PointF(1749.0f, 1015.0F);   //ALL OFF
            PointF MenuOption6PosTest = new PointF(1749.0f, 1030.0F);   //SHOW HIDE
            PointF MenuOption7PosTest = new PointF(1749.0f, 1045.0F);   //QUIT MENU

            //g.FillRectangle(blackFill, InfoBox);
            g.FillRectangle(blackFill, TestBox);
            g.DrawString("NightFyreTV", HeaderFont, redBrush, HeaderTextPosTest);

            if (!bAutoShoot)
            {
                g.DrawString("[F5] TRIGGERBOT", InfoTextFont, whiteBrush, MenuOption1PosTest);
            }
            else
            {
                g.DrawString("[F5] TRIGGERBOT", InfoTextFont, GreenBrush, MenuOption1PosTest);
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
            if (!bControllerMode)
            {
                g.DrawString("[F8] CONTROLLER", InfoTextFont, whiteBrush, MenuOption4PosTest);
            }
            else
            {
                g.DrawString("[F8] CONTROLLER", InfoTextFont, GreenBrush, MenuOption4PosTest);
            }
            if (!bInfiniteAmmo)
            {
                g.DrawString("[1] INF AMMO", InfoTextFont, whiteBrush, MenuOption8PosTest);
            }
            else
            {
                g.DrawString("[1] INF AMMO", InfoTextFont, GreenBrush, MenuOption8PosTest);
            }
            if (!bRPBoost)
            {
                g.DrawString("[2] RP BOOSTER", InfoTextFont, whiteBrush, MenuOption9PosTest);
            }
            else
            {
                g.DrawString("[2] RP BOOSTER", InfoTextFont, GreenBrush, MenuOption9PosTest);
            }
            if (!weapon.bPerfectWeapon)
            {
                g.DrawString("[3] PERFECT WEAPON", InfoTextFont, whiteBrush, MenuOption10PosTest);
            }
            else
            {
                g.DrawString("[3] PERFECT WEAPON", InfoTextFont, GreenBrush, MenuOption10PosTest);
            }
            g.DrawString("ALL OFF [F9]", InfoTextFont, redBrush, MenuOption5PosTest);
            g.DrawString("SHOW / HIDE [INSERT]", InfoTextFont, redBrush, MenuOption6PosTest);
            g.DrawString("QUIT MENU [DELETE]", InfoTextFont, redBrush, MenuOption7PosTest);
        }
        #endregion

    }
}
