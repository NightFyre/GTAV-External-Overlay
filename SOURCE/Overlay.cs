using Memory;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using GTAV_External_Trainer.Helpers;
using System.Runtime.InteropServices;
using Simple_GTAV_External_Trainer.Helpers;
using Simple_GTAV_External_Trainer.Helpers.Weapon;


#region LICENSE INFORMATION :: IMPORTANT
//“Commons Clause” License Condition v1.0
//
//The Software is provided to you by the Licensor under the License,
//as defined below, subject to the following condition.
//
//Without limiting other conditions in the License,
//the grant of rights under the License will not include,
//and the License does not grant to you, the right to Sell the Software.
//
//For purposes of the foregoing,
//“Sell” means practicing any or all of the rights granted to you under the License to provide to third parties,
//for a fee or other consideration (including without limitation fees for hosting or consulting/ support services related to the Software),
//a product or service whose value derives, entirely or substantially, from the functionality of the Software.
//Any license notice or attribution required by the License must also include this Commons Clause License Condition notice.
//
//Software: GTAV - External - Overlay
//License: MIT
//Licensor: xCENTx
//--------------------------------------------------------------------------------
///MIT License
///
///Copyright (c) 2021 xCENTx
///
///Permission is hereby granted, free of charge, to any person obtaining a copy
///of this software and associated documentation files (the "Software"), to deal
///in the Software without restriction, including without limitation the rights
///to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
///copies of the Software, and to permit persons to whom the Software is
///furnished to do so, subject to the following conditions:
///
///The above copyright notice and this permission notice shall be included in all
///copies or substantial portions of the Software.
///
///THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
///IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
///FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
///AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
///LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
///OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
///SOFTWARE.
///----------------------------------------------------------------------------------
#endregion

///Created by NightFyreTV
/// Simple GTA V External Overlay
/// Version 1.6

///Contributors
/// - RasqueP

///CREDITS
/// - PLEB
/// - Violency
/// - Fold

// FEATURES
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        #endregion

        #region PROCESS INFO

        #region VARIABLES
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
        private bool bTuneCar = false;
        private bool bCarModActive = false;
        private bool bPerfectWeapon = false;
        int leftDown = 0x02;
        int leftUp = 0x04;
        int flag = 0;

        ///Weapondata
        private Weapon weapon = new Weapon(m);

        //WeaponData v2.0
        Dictionary<string, string> WeaponData = new Dictionary<string, string>();

        //Vehicle Data 
        Dictionary<string, string> data = new Dictionary<string, string>();


        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int a, int b, int c, int d, int damnIwonderifpeopleactuallyreadsthis);

        #region WEAPON BOOLS
        bool bWEAPONHACKFLAG;
        //Unarmed
        bool bHands;
        bool bCellPhone;

        //Pistols
        bool bPistol;
        bool bPistolMk2;
        bool bCombatPistol;
        bool bApPistol;
        bool bStunGun;
        bool bPistol50;
        bool bSNSPistol;
        bool bSNSPistolMk2;
        bool bHeavyPistol;
        bool bVintagePistol;
        bool bFlareGun;
        bool bRevolver;
        bool bRevolverMk2;
        bool bDoubleAction;
        bool bAtomizer;
        bool bCeramicPistol;
        bool bNavyRevolver;

        //SMG's
        bool bMicroSMG;
        bool bSMG;
        bool bSMGMk2;
        bool bAssaultSMG;
        bool bCombatPDW;
        bool bMachinePistol;
        bool bMiniSMG;
        bool bHellbringer;

        //Machine Guns
        bool bMG;
        bool bCombatMG;
        bool bCombatMGMk2;

        //Assault Rifles
        bool bAssaultRifle;
        bool bAssaultRifleMk2;
        bool bCarbineRifle;
        bool bCarbineRifleMk2;
        bool bAdvancedRifle;
        bool bSpecialCarbine;
        bool bSpecialCarbineMk2;
        bool bBullpupRifle;
        bool bBullpupRifleMk2;
        bool bMilitaryRifle;

        //Shotguns
        bool bPumpShotgun;
        bool bPumpShotgunMk2;
        bool bSawnOffSG;
        bool bAssaultShotgun;
        bool bBullpupShotgun;
        bool bMusket;
        bool bHeavyShotgun;
        bool bDoubleBarrel;
        bool bSweeperShotgun;

        //Snipers
        bool bSniperRifle;
        bool bHeavySniper;
        bool bHeavySniperMk2;
        bool bMarksmanRifle;
        bool bMarksmanRifleMk2;

        //Special
        bool bMiniGun;
        bool bRailGun;
        bool bWidowMaker;
        #endregion

        #endregion

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

        #region TUNE CAR
        private void SPEEDHACK()
        {
            bool IsDriving = false;

            //Vehicle Info
            var pDriving = m.ReadInt(gData.VehicleState);
            var VehicleID = m.ReadLong(gData.VehicleID);
            var VehicleHealth = m.ReadFloat(gData.VehicleHealth);
            var EngineHealth = m.ReadFloat(gData.EngineHealth);
            var VehicleGravity = m.ReadFloat(gData.VehicleGravity);

            //Vehicle Handling Info
            var VehicleAcceleration = m.ReadFloat(gData.VehicleAcceleration);
            var BrakeForce = m.ReadFloat(gData.VehBrakeForce);
            var HandbrakeForce = m.ReadFloat(gData.VehHandbrakeforce);
            var DamageForceMultiplier = m.ReadFloat(gData.VehDamageMultiplier);
            var CollisionForceMultiplier = m.ReadFloat(gData.VehCollisionMultiplier);

            if (bTuneCar)
            {
                if (pDriving == 16)
                {
                    IsDriving = false;
                }
                if (pDriving == 0)
                {
                    IsDriving = true;
                }

                if (IsDriving)
                {
                    if ((!bCarModActive) && (VehicleGravity != 20))
                    {
                        data.Add("dVehicleID", VehicleID.ToString("X"));
                        data.Add("VehicleAcceleration", VehicleAcceleration.ToString());
                        data.Add("VehicleGravity", VehicleGravity.ToString());
                        data.Add("BrakeForce", BrakeForce.ToString());
                        data.Add("HandbrakeForce", HandbrakeForce.ToString());
                        data.Add("DamageMultiplier", DamageForceMultiplier.ToString());
                        data.Add("CollisionMultiplier", CollisionForceMultiplier.ToString());
                        m.WriteMemory(gData.VehicleAcceleration, "float", "3.5");
                        m.WriteMemory(gData.VehicleGravity, "float", "20");
                        m.WriteMemory(gData.VehBrakeForce, "float", "5");
                        m.WriteMemory(gData.VehHandbrakeforce, "float", "3");
                        m.WriteMemory(gData.VehDamageMultiplier, "float", "0");
                        m.WriteMemory(gData.VehCollisionMultiplier, "float", "0");
                        bCarModActive = true;
                    }
                    if (VehicleHealth < 1000)
                    {
                        m.WriteMemory(gData.VehicleHealth, "float", "1000");
                    }
                    if (EngineHealth < 1000)
                    {
                        m.WriteMemory(gData.EngineHealth, "float", "1000");
                    }
                }
                else
                {
                    if (bCarModActive)
                    {
                        string vAccelerationData = data["VehicleAcceleration"];
                        string vGravityData = data["VehicleGravity"];
                        string vBrakeForceData = data["BrakeForce"];
                        string vHandbrakeForceData = data["HandbrakeForce"];
                        string vDamageMultiplierData = data["DamageMultiplier"];
                        string vCollisionMultiplierData = data["CollisionMultiplier"];
                        float vAcceleration = float.Parse(vAccelerationData);
                        float vGravity = float.Parse(vGravityData);
                        float vBrakeForce = float.Parse(vBrakeForceData);
                        float vHandbrakeForce = float.Parse(vHandbrakeForceData);
                        float vDamage = float.Parse(vDamageMultiplierData);
                        float vCollision = float.Parse(vCollisionMultiplierData);
                        m.WriteMemory(gData.VehicleAcceleration, "float", vAcceleration.ToString());
                        m.WriteMemory(gData.VehicleGravity, "float", vGravity.ToString());
                        m.WriteMemory(gData.VehBrakeForce, "float", vBrakeForce.ToString());
                        m.WriteMemory(gData.VehHandbrakeforce, "float", vHandbrakeForce.ToString());
                        m.WriteMemory(gData.VehDamageMultiplier, "float", vDamage.ToString());
                        m.WriteMemory(gData.VehCollisionMultiplier, "float", vCollision.ToString());
                        data.Remove("dVehicleID");
                        data.Remove("VehicleAcceleration");
                        data.Remove("VehicleGravity");
                        data.Remove("BrakeForce");
                        data.Remove("HandbrakeForce");
                        data.Remove("DamageMultiplier");
                        data.Remove("CollisionMultiplier");
                        bCarModActive = false;
                        bTuneCar = false;
                    }

                }
            }
            else
            {
                if (bCarModActive)
                {
                    string vAccelerationData = data["VehicleAcceleration"];
                    string vGravityData = data["VehicleGravity"];
                    string vBrakeForceData = data["BrakeForce"];
                    string vHandbrakeForceData = data["HandbrakeForce"];
                    string vDamageMultiplierData = data["DamageMultiplier"];
                    string vCollisionMultiplierData = data["CollisionMultiplier"];
                    float vAcceleration = float.Parse(vAccelerationData);
                    float vGravity = float.Parse(vGravityData);
                    float vBrakeForce = float.Parse(vBrakeForceData);
                    float vHandbrakeForce = float.Parse(vHandbrakeForceData);
                    float vDamage = float.Parse(vDamageMultiplierData);
                    float vCollision = float.Parse(vCollisionMultiplierData);
                    m.WriteMemory(gData.VehicleAcceleration, "float", vAcceleration.ToString());
                    m.WriteMemory(gData.VehicleGravity, "float", vGravity.ToString());
                    m.WriteMemory(gData.VehBrakeForce, "float", vBrakeForce.ToString());
                    m.WriteMemory(gData.VehHandbrakeforce, "float", vHandbrakeForce.ToString());
                    m.WriteMemory(gData.VehDamageMultiplier, "float", vDamage.ToString());
                    m.WriteMemory(gData.VehCollisionMultiplier, "float", vCollision.ToString());
                    data.Remove("dVehicleID");
                    data.Remove("VehicleAcceleration");
                    data.Remove("VehicleGravity");
                    data.Remove("BrakeForce");
                    data.Remove("HandbrakeForce");
                    data.Remove("DamageMultiplier");
                    data.Remove("CollisionMultiplier");
                    bCarModActive = false;
                }
            }
        }
        #endregion

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
            if (bPerfectWeapon && bAllOff)
            {
                DEACTIVATE();
                bPerfectWeapon = false;
            }
            //if (weapon.bPerfectWeapon && bAllOff)
            //{
            //    weapon.ResetStats();
            //    weapon.bPerfectWeapon = false;
            //}
            #endregion

            #region TUNE CAR DISABLE
            if (bCarModActive && bAllOff)
            {
                string vAccelerationData = data["VehicleAcceleration"];
                string vGravityData = data["VehicleGravity"];
                string vBrakeForceData = data["BrakeForce"];
                string vHandbrakeForceData = data["HandbrakeForce"];
                string vDamageMultiplierData = data["DamageMultiplier"];
                string vCollisionMultiplierData = data["CollisionMultiplier"];
                float vAcceleration = float.Parse(vAccelerationData);
                float vGravity = float.Parse(vGravityData);
                float vBrakeForce = float.Parse(vBrakeForceData);
                float vHandbrakeForce = float.Parse(vHandbrakeForceData);
                float vDamage = float.Parse(vDamageMultiplierData);
                float vCollision = float.Parse(vCollisionMultiplierData);
                m.WriteMemory(gData.VehicleAcceleration, "float", vAcceleration.ToString());
                m.WriteMemory(gData.VehicleGravity, "float", vGravity.ToString());
                m.WriteMemory(gData.VehBrakeForce, "float", vBrakeForce.ToString());
                m.WriteMemory(gData.VehHandbrakeforce, "float", vHandbrakeForce.ToString());
                m.WriteMemory(gData.VehDamageMultiplier, "float", vDamage.ToString());
                m.WriteMemory(gData.VehCollisionMultiplier, "float", vCollision.ToString());
                data.Remove("dVehicleID");
                data.Remove("VehicleAcceleration");
                data.Remove("VehicleGravity");
                data.Remove("BrakeForce");
                data.Remove("HandbrakeForce");
                data.Remove("DamageMultiplier");
                data.Remove("CollisionMultiplier");
                bCarModActive = false;
                bTuneCar = false;
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

            if (bPerfectWeapon)
            {
                DEACTIVATE();
            }
            //weapon.ResetStats();

            #endregion

            #region TUNE CAR DISABLE
            if (bCarModActive)
            {
                string vAccelerationData = data["VehicleAcceleration"];
                string vGravityData = data["VehicleGravity"];
                string vBrakeForceData = data["BrakeForce"];
                string vHandbrakeForceData = data["HandbrakeForce"];
                string vDamageMultiplierData = data["DamageMultiplier"];
                string vCollisionMultiplierData = data["CollisionMultiplier"];
                float vAcceleration = float.Parse(vAccelerationData);
                float vGravity = float.Parse(vGravityData);
                float vBrakeForce = float.Parse(vBrakeForceData);
                float vHandbrakeForce = float.Parse(vHandbrakeForceData);
                float vDamage = float.Parse(vDamageMultiplierData);
                float vCollision = float.Parse(vCollisionMultiplierData);
                m.WriteMemory(gData.VehicleAcceleration, "float", vAcceleration.ToString());
                m.WriteMemory(gData.VehicleGravity, "float", vGravity.ToString());
                m.WriteMemory(gData.VehBrakeForce, "float", vBrakeForce.ToString());
                m.WriteMemory(gData.VehHandbrakeforce, "float", vHandbrakeForce.ToString());
                m.WriteMemory(gData.VehDamageMultiplier, "float", vDamage.ToString());
                m.WriteMemory(gData.VehCollisionMultiplier, "float", vCollision.ToString());
                data.Remove("dVehicleID");
                data.Remove("VehicleAcceleration");
                data.Remove("VehicleGravity");
                data.Remove("BrakeForce");
                data.Remove("HandbrakeForce");
                data.Remove("DamageMultiplier");
                data.Remove("CollisionMultiplier");
                bCarModActive = false;
                bTuneCar = false;
            }
            #endregion

            bAutoShoot = false;
            bGodMode = false;
            bNeverWanted = false;
            bControllerMode = false;
            bInfiniteAmmo = false;
            weapon.bPerfectWeapon = false;
            bRPBoost = false;
            bPerfectWeapon = false;
        }
        #endregion

        #endregion

        #region WEAPON HACK v2.0
        private void ACTIVATE()
        {
            //Read Current Weapon Address and Addresses for Weapon Data that will be changed
            var CWeaponPTR = m.ReadLong(gData.WeaponID);
            var DamageAddr = CWeaponPTR + 0xB0;
            var SpreadAddr = CWeaponPTR + 0x74;
            var Penetration = CWeaponPTR + 0x110;
            var VelocityAddr = CWeaponPTR + 0x11C;
            var RangeAddr = CWeaponPTR + 0x28C;
            var RecoilAddr = CWeaponPTR + 0x2F4;

            //Read Current weapon data
            var WeaponDamage = m.ReadFloat(gData.WEAPON_DAMAGE);
            var WeaponSpread = m.ReadFloat(gData.WEAPON_SPREAD);
            var WeaponPenetration = m.ReadFloat(gData.WEAPON_BPENETRATION);
            var WeaponVelocity = m.ReadFloat(gData.WEAPON_MVELOCITY);
            var WeaponRange = m.ReadFloat(gData.WEAPON_RANGE);
            var WeaponRecoil = m.ReadFloat(gData.WEAPON_RECOIL);

            //Store the addresses to dictionary so that they can be called later to restore weapon data to default
            WeaponData.Add("CURRENTWEAPON", CWeaponPTR.ToString("X"));
            WeaponData.Add("WeaponDamageAddr", DamageAddr.ToString("X"));
            WeaponData.Add("WeaponSpreadAddr", SpreadAddr.ToString("X"));
            WeaponData.Add("WeaponPenetration", Penetration.ToString("X"));
            WeaponData.Add("WeaponVelocity", VelocityAddr.ToString("X"));
            WeaponData.Add("WeaponRange", RangeAddr.ToString("X"));
            WeaponData.Add("WeaponRecoil", RecoilAddr.ToString("X"));

            //Store Weapon Data
            WeaponData.Add("Damage", WeaponDamage.ToString());
            WeaponData.Add("Spread", WeaponSpread.ToString());
            WeaponData.Add("Penetration", WeaponPenetration.ToString());
            WeaponData.Add("Velocity", WeaponVelocity.ToString());
            WeaponData.Add("Range", WeaponRange.ToString());
            WeaponData.Add("Recoil", WeaponRecoil.ToString());

            //Write Patched Data
            m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
            m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
            m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
            m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
            m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
            m.WriteMemory(gData.WEAPON_RECOIL, "float", "0");
            bWEAPONHACKFLAG = true;
        }

        private void DEACTIVATE()
        {
            //This restores any patched weapon data
            var DamageAddr = WeaponData["WeaponDamageAddr"];
            var damage = WeaponData["Damage"];
            m.WriteMemory(DamageAddr, "float", damage);
            WeaponData.Remove("WeaponDamageAddr");
            WeaponData.Remove("Damage");

            var SpreadAddr = WeaponData["WeaponSpreadAddr"];
            var spread = WeaponData["Spread"];
            m.WriteMemory(SpreadAddr, "float", spread);
            WeaponData.Remove("WeaponSpreadAddr");
            WeaponData.Remove("Spread");

            var PenetrationAddr = WeaponData["WeaponPenetration"];
            var penetration = WeaponData["Penetration"];
            m.WriteMemory(PenetrationAddr, "float", penetration);
            WeaponData.Remove("WeaponPenetration");
            WeaponData.Remove("Penetration");

            var VelocityAddr = WeaponData["WeaponVelocity"];
            var velocity = WeaponData["Velocity"];
            m.WriteMemory(VelocityAddr, "float", velocity);
            WeaponData.Remove("WeaponVelocity");
            WeaponData.Remove("Velocity");

            var RangeAddr = WeaponData["WeaponRange"];
            var range = WeaponData["Range"];
            m.WriteMemory(RangeAddr, "float", range);
            WeaponData.Remove("WeaponRange");
            WeaponData.Remove("Range");

            var RecoilAddr = WeaponData["WeaponRecoil"];
            var recoil = WeaponData["Recoil"];
            m.WriteMemory(RecoilAddr, "float", recoil);
            WeaponData.Remove("WeaponRecoil");
            WeaponData.Remove("Recoil");
            WeaponData.Remove("CURRENTWEAPON");

            bPistol = false;
            bPistolMk2 = false;
            bCombatPistol = false;
            bApPistol = false;
            bStunGun = false;
            bPistol50 = false;
            bSNSPistol = false;
            bSNSPistolMk2 = false;
            bHeavyPistol = false;
            bVintagePistol = false;
            bFlareGun = false;
            bRevolver = false;
            bRevolverMk2 = false;
            bDoubleAction = false;
            bAtomizer = false;
            bCeramicPistol = false;
            bNavyRevolver = false;

            //SMG's
            bMicroSMG = false;
            bSMG = false;
            bSMGMk2 = false;
            bAssaultSMG = false;
            bCombatPDW = false;
            bMachinePistol = false;
            bMiniSMG = false;
            bHellbringer = false;

            //Machine Guns
            bMG = false;
            bCombatMG = false;
            bCombatMGMk2 = false;

            //Assault Rifles
            bAssaultRifle = false;
            bAssaultRifleMk2 = false;
            bCarbineRifle = false;
            bCarbineRifleMk2 = false;
            bAdvancedRifle = false;
            bSpecialCarbine = false;
            bSpecialCarbineMk2 = false;
            bBullpupRifle = false;
            bBullpupRifleMk2 = false;
            bMilitaryRifle = false;

            //Shotguns
            bPumpShotgun = false;
            bPumpShotgunMk2 = false;
            bSawnOffSG = false;
            bAssaultShotgun = false;
            bBullpupShotgun = false;
            bMusket = false;
            bHeavyShotgun = false;
            bDoubleBarrel = false;
            bSweeperShotgun = false;

            //Snipers
            bSniperRifle = false;
            bHeavySniper = false;
            bHeavySniperMk2 = false;
            bMarksmanRifle = false;
            bMarksmanRifleMk2 = false;

            //Special
            bMiniGun = false;
            bRailGun = false;
            bWidowMaker = false;
            bWEAPONHACKFLAG = false;
        }

        private void WEAPONHACK()
        {
            var CURRENT_WEAPON = m.ReadInt("GTA5.exe+1CDDFEC");

            //Restoration
            #region UNARMED
            if (CURRENT_WEAPON == (int)WeaponHash.HANDS)
            {
                if (bWEAPONHACKFLAG && !bHands)
                {
                    DEACTIVATE();
                }
            }

            if (CURRENT_WEAPON == (int)WeaponHash.CELL_PHONE)
            {
                if (bWEAPONHACKFLAG)
                {
                    DEACTIVATE();
                }
            }
            #endregion

            //COMPELTE
            #region PISTOLS
            if (CURRENT_WEAPON == (int)WeaponHash.PISTOL)
            {
                if (bWEAPONHACKFLAG && !bPistol)
                {
                    DEACTIVATE();
                }
                if (!bPistol)
                {
                    ACTIVATE();
                    bPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.PISTOL_MK2)
            {
                if (bWEAPONHACKFLAG && !bPistolMk2)
                {
                    DEACTIVATE();
                }
                if (!bPistolMk2)
                {
                    ACTIVATE();
                    bPistolMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.COMBAT_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bCombatPistol)
                {
                    DEACTIVATE();
                }
                if (!bCombatPistol)
                {
                    ACTIVATE();
                    bCombatPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.AP_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bApPistol)
                {
                    DEACTIVATE();
                }
                if (!bApPistol)
                {
                    ACTIVATE();
                    bApPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.STUN_GUN)
            {
                if (bWEAPONHACKFLAG && !bStunGun)
                {
                    DEACTIVATE();
                }
                if (!bStunGun)
                {
                    ACTIVATE();
                    bStunGun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.PISTOL_50)
            {
                if (bWEAPONHACKFLAG && !bPistol50)
                {
                    DEACTIVATE();
                }
                if (!bPistol50)
                {
                    ACTIVATE();
                    bPistol50 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SNS_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bSNSPistol)
                {
                    DEACTIVATE();
                }
                if (!bSNSPistol)
                {
                    ACTIVATE();
                    bSNSPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SNS_PISTOLMK2)
            {
                if (bWEAPONHACKFLAG && !bSNSPistolMk2)
                {
                    DEACTIVATE();
                }
                if (!bSNSPistolMk2)
                {
                    ACTIVATE();
                    bSNSPistolMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.HEAVY_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bHeavyPistol)
                {
                    DEACTIVATE();
                }
                if (!bHeavyPistol)
                {
                    ACTIVATE();
                    bHeavyPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.VINTAGE_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bVintagePistol)
                {
                    DEACTIVATE();
                }
                if (!bVintagePistol)
                {
                    ACTIVATE();
                    bVintagePistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.FLARE_GUN)
            {
                if (bWEAPONHACKFLAG && !bFlareGun)
                {
                    DEACTIVATE();
                }
                if (!bFlareGun)
                {
                    ACTIVATE();
                    bFlareGun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.REVOLVER)
            {
                if (bWEAPONHACKFLAG && !bRevolver)
                {
                    DEACTIVATE();
                }
                if (!bRevolver)
                {
                    ACTIVATE();
                    bRevolver = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.REVOLVERMK2)
            {
                if (bWEAPONHACKFLAG && !bRevolverMk2)
                {
                    DEACTIVATE();
                }
                if (!bRevolverMk2)
                {
                    ACTIVATE();
                    bRevolverMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.DOUBLE_ACTION)
            {
                if (bWEAPONHACKFLAG && !bDoubleAction)
                {
                    DEACTIVATE();
                }
                if (!bDoubleAction)
                {
                    ACTIVATE();
                    bDoubleAction = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.ATOMIZER)
            {
                if (bWEAPONHACKFLAG && !bAtomizer)
                {
                    DEACTIVATE();
                }
                if (!bAtomizer)
                {
                    ACTIVATE();
                    bAtomizer = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.CERAMIC_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bCeramicPistol)
                {
                    DEACTIVATE();
                }
                if (!bCeramicPistol)
                {
                    ACTIVATE();
                    bCeramicPistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.NAVY_REVOLVER)
            {
                if (bWEAPONHACKFLAG && !bNavyRevolver)
                {
                    DEACTIVATE();
                }
                if (!bNavyRevolver)
                {
                    ACTIVATE();
                    bNavyRevolver = true;
                }
            }
            #endregion

            //COMPLETE
            #region SMG
            if (CURRENT_WEAPON == (int)WeaponHash.MICRO_SMG)
            {
                if (bWEAPONHACKFLAG && !bMicroSMG)
                {
                    ACTIVATE();
                }
                if (bMicroSMG)
                {
                    DEACTIVATE();
                    bMicroSMG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SMG)
            {
                if (bWEAPONHACKFLAG && !bSMG)
                {
                    DEACTIVATE();
                }
                if (!bSMG)
                {
                    ACTIVATE();
                    bSMG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SMGMK2)
            {
                if (bWEAPONHACKFLAG && !bSMGMk2)
                {
                    DEACTIVATE();
                }
                if (!bSMGMk2)
                {
                    ACTIVATE();
                    bSMGMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.ASSAULT_SMG)
            {
                if (bWEAPONHACKFLAG && !bAssaultSMG)
                {
                    DEACTIVATE();
                }
                if (!bAssaultSMG)
                {
                    ACTIVATE();
                    bAssaultSMG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.COMBAT_PDW)
            {
                if (bWEAPONHACKFLAG && !bCombatPDW)
                {
                    DEACTIVATE();
                }
                if (!bCombatPDW)
                {
                    ACTIVATE();
                    bCombatPDW = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.MACHINE_PISTOL)
            {
                if (bWEAPONHACKFLAG && !bMachinePistol)
                {
                    DEACTIVATE();
                }
                if (!bMachinePistol)
                {
                    ACTIVATE();
                    bMachinePistol = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.MINI_SMG)
            {
                if (bWEAPONHACKFLAG && !bMiniSMG)
                {
                    DEACTIVATE();
                }
                if (!bMiniSMG)
                {
                    ACTIVATE();
                    bMiniSMG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.HELLBRINGER)
            {
                if (bWEAPONHACKFLAG && !bHellbringer)
                {
                    DEACTIVATE();
                }
                if (!bHellbringer)
                {
                    ACTIVATE();
                    bHellbringer = true;
                }
            }
            #endregion

            //COMPLETE
            #region ASSAULT RIFLE
            if (CURRENT_WEAPON == (int)WeaponHash.ASSAULT_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bAssaultRifle)
                {
                    DEACTIVATE();
                }
                if (!bAssaultRifle)
                {
                    ACTIVATE();
                    bAssaultRifle = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.ASSAULT_RIFLEMK2)
            {
                if (bWEAPONHACKFLAG && !bAssaultRifleMk2)
                {
                    DEACTIVATE();
                }
                if (!bAssaultRifleMk2)
                {
                    ACTIVATE();
                    bAssaultRifleMk2 = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.CARBINE_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bCarbineRifle)
                {
                    DEACTIVATE();
                }
                if (!bCarbineRifle)
                {
                    ACTIVATE();
                    bCarbineRifle = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.CARBINE_RIFLEMK2)
            {
                if (bWEAPONHACKFLAG && !bCarbineRifleMk2)
                {
                    DEACTIVATE();
                }
                if (!bCarbineRifleMk2)
                {
                    ACTIVATE();
                    bCarbineRifleMk2 = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.ADVANCED_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bAdvancedRifle)
                {
                    DEACTIVATE();
                }
                if (!bAdvancedRifle)
                {
                    ACTIVATE();
                    bAdvancedRifle = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.SPECIAL_CARBINE)
            {
                if (bWEAPONHACKFLAG && !bSpecialCarbine)
                {
                    DEACTIVATE();
                }
                if (!bSpecialCarbine)
                {
                    ACTIVATE();
                    bSpecialCarbine = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.SPECIAL_CARBINEMK2)
            {
                if (bWEAPONHACKFLAG && !bSpecialCarbineMk2)
                {
                    DEACTIVATE();
                }
                if (!bSpecialCarbineMk2)
                {
                    ACTIVATE();
                    bSpecialCarbineMk2 = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.BULLPUP_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bBullpupRifle)
                {
                    DEACTIVATE();
                }
                if (!bBullpupRifle)
                {
                    ACTIVATE();
                    bBullpupRifle = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.BULLPUP_RIFLEMK2)
            {
                if (bWEAPONHACKFLAG && !bBullpupRifleMk2)
                {
                    DEACTIVATE();
                }
                if (!bBullpupRifleMk2)
                {
                    ACTIVATE();
                    bBullpupRifleMk2 = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.MILITARY_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bMilitaryRifle)
                {
                    DEACTIVATE();
                }
                if (!bMilitaryRifle)
                {
                    ACTIVATE();
                    bMilitaryRifle = true;
                }

            }
            #endregion

            //COMPLETE
            #region MACHINE GUNS
            if (CURRENT_WEAPON == (int)WeaponHash.MG)
            {
                if (bWEAPONHACKFLAG && !bMG)
                {
                    DEACTIVATE();
                }
                if (!bMG)
                {
                    ACTIVATE();
                    bMG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.COMBATMG)
            {
                if (bWEAPONHACKFLAG && !bCombatMG)
                {
                    DEACTIVATE();
                }
                if (!bCombatMG)
                {
                    ACTIVATE();
                    bCombatMG = true;
                }

            }
            if (CURRENT_WEAPON == (int)WeaponHash.COMBATMGMK2)
            {
                if (bWEAPONHACKFLAG && !bCombatMGMk2)
                {
                    DEACTIVATE();
                }
                if (!bCombatMGMk2)
                {
                    ACTIVATE();
                    bCombatMGMk2 = true;
                }

            }
            #endregion

            //COMPLETE
            #region SHOTGUNS
            if (CURRENT_WEAPON == (int)WeaponHash.PUMP_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bPumpShotgun)
                {
                    DEACTIVATE();
                }
                if (!bPumpShotgun)
                {
                    ACTIVATE();
                    bPumpShotgun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.PUMP_SHOTGUNMK2)
            {
                if (bWEAPONHACKFLAG && !bPumpShotgunMk2)
                {
                    DEACTIVATE();
                }
                if (!bPumpShotgunMk2)
                {
                    ACTIVATE();
                    bPumpShotgunMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SAWN_OFF_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bSawnOffSG)
                {
                    DEACTIVATE();
                }
                if (!bSawnOffSG)
                {
                    ACTIVATE();
                    bSawnOffSG = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.ASSAULT_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bAssaultShotgun)
                {
                    DEACTIVATE();
                }
                if (!bAssaultShotgun)
                {
                    ACTIVATE();
                    bAssaultShotgun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.BULLPUP_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bBullpupShotgun)
                {
                    DEACTIVATE();
                }
                if (!bBullpupShotgun)
                {
                    ACTIVATE();
                    bBullpupShotgun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.MUSKET)
            {
                if (bWEAPONHACKFLAG && !bMusket)
                {
                    DEACTIVATE();
                }
                if (!bMusket)
                {
                    ACTIVATE();
                    bMusket = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.HEAVY_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bHeavyShotgun)
                {
                    DEACTIVATE();
                }
                if (!bHeavyShotgun)
                {
                    ACTIVATE();
                    bHeavyShotgun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.DOUBLE_BARREL)
            {
                if (bWEAPONHACKFLAG && !bDoubleBarrel)
                {
                    DEACTIVATE();
                }
                if (!bDoubleBarrel)
                {
                    ACTIVATE();
                    bDoubleBarrel = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.SWEEPER_SHOTGUN)
            {
                if (bWEAPONHACKFLAG && !bSweeperShotgun)
                {
                    DEACTIVATE();
                }
                if (!bSweeperShotgun)
                {
                    ACTIVATE();
                    bSweeperShotgun = true;
                }
            }
            #endregion

            //COMPLETE
            #region SNIPER RIFLES
            if (CURRENT_WEAPON == (int)WeaponHash.SNIPER_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bSniperRifle)
                {
                    DEACTIVATE();
                }
                if (!bSniperRifle)
                {
                    ACTIVATE();
                    bSniperRifle = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.HEAVY_SNIPER)
            {
                if (bWEAPONHACKFLAG && !bHeavySniper)
                {
                    DEACTIVATE();
                }
                if (!bHeavySniper)
                {
                    ACTIVATE();
                    bHeavySniper = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.HEAVY_SNIPERMK2)
            {
                if (bWEAPONHACKFLAG && !bHeavySniperMk2)
                {
                    DEACTIVATE();
                }
                if (!bHeavySniperMk2)
                {
                    ACTIVATE();
                    bHeavySniperMk2 = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.MARKSMAN_RIFLE)
            {
                if (bWEAPONHACKFLAG && !bMarksmanRifle)
                {
                    DEACTIVATE();
                }
                if (!bMarksmanRifle)
                {
                    ACTIVATE();
                    bMarksmanRifle = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.MARKSMAN_RIFLEMK2)
            {
                if (bWEAPONHACKFLAG && !bMarksmanRifleMk2)
                {
                    DEACTIVATE();
                }
                if (!bMarksmanRifleMk2)
                {
                    ACTIVATE();
                    bMarksmanRifleMk2 = true;
                }
            }
            #endregion

            //COMPLETE
            #region SPECIAL
            if (CURRENT_WEAPON == (int)WeaponHash.MINIGUN)
            {
                if (bWEAPONHACKFLAG && !bMiniGun)
                {
                    DEACTIVATE();
                }
                if (!bMiniGun)
                {
                    ACTIVATE();
                    bMiniGun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.RAILGUN)
            {
                if (bWEAPONHACKFLAG && !bRailGun)
                {
                    DEACTIVATE();
                }
                if (!bRailGun)
                {
                    ACTIVATE();
                    bRailGun = true;
                }
            }
            if (CURRENT_WEAPON == (int)WeaponHash.WIDOWMAKER)
            {
                if (bWEAPONHACKFLAG && !bWidowMaker)
                {
                    DEACTIVATE();
                }
                if (!bWidowMaker)
                {
                    ACTIVATE();
                    bWidowMaker = true;
                }
            }
            #endregion
            Thread.Sleep(100);
        }

        private void WEAPONHACK2()
        {
            while (true)
            {
                if (bPerfectWeapon)
                {
                    WEAPONHACK();
                }
            }
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
            keyMgr.AddKey(Keys.F8);         // CONTROLLER MODE
            keyMgr.AddKey(Keys.F9);         // ALL OFF
            keyMgr.AddKey(Keys.NumPad1);    // INFINITE AMMO
            keyMgr.AddKey(Keys.NumPad2);    // RPBooster
            keyMgr.AddKey(Keys.NumPad3);    // Perfect Weapon (SOCOM INSPIRED)
            keyMgr.AddKey(Keys.NumPad4);    // Vehicle Speed Modifier
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
                    this.bPerfectWeapon = !this.bPerfectWeapon;
                    break;
                case Keys.NumPad4:
                    this.bTuneCar = !this.bTuneCar;
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
                Thread WEAPONHACK = new Thread(WEAPONHACK2) { IsBackground = true };
                WEAPONHACK.Start();

                ///RasqueP , didn't want to delete any of the stuff you did as you can probably integrate my work seamlessly
                ///Instead I just commented some stuff out and left all your work in tact. Feel free to simplify all of this if you would like
                //Thread PERFECTWEAPONHACK = new Thread(weapon.WEAPONHACK) { IsBackground = true };
                //PERFECTWEAPONHACK.Start();
            }

            this.BackColor = System.Drawing.Color.Orange;
            this.TransparencyKey = System.Drawing.Color.Orange;
            this.Opacity = 0.69;
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
                DrawMenu(sender, e);

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
            SPEEDHACK();
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
        public void DrawMenu(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(System.Drawing.Color.Black);
            g.CompositingQuality = CompositingQuality.HighQuality;
            System.Drawing.Brush redBrush = new SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Brush whiteBrush = new SolidBrush(System.Drawing.Color.White);
            System.Drawing.Brush GreenBrush = new SolidBrush(System.Drawing.Color.LimeGreen);
            System.Drawing.Brush blackFill = new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0));
            System.Drawing.Font HeaderFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.Font InfoTextFont = new System.Drawing.Font("Arial", 10);

            #region DEBUG MENU
            //Menu Position 1 || TOP LEFT
            PointF HeaderTextPos = new PointF(0.0F, 25.0F);
            PointF MenuOption1Pos = new PointF(2.0F, 50.0F);
            PointF MenuOption2Pos = new PointF(2.0F, 70.0F);
            PointF MenuOption3Pos = new PointF(2.0F, 90.0F);
            PointF MenuOption4Pos = new PointF(2.0F, 120.0F);
            PointF MenuOption5Pos = new PointF(2.0F, 135.0F);
            PointF MenuOption6Pos = new PointF(2.0F, 150.0F);
            System.Drawing.Rectangle InfoBox = new System.Drawing.Rectangle(3, 26, 155, 140);
            //g.FillRectangle(blackFill, InfoBox);
            #endregion

            #region MENU OPTIONS
            //Menu Position 2 || BOTTOM RIGHT
            System.Drawing.Rectangle MENU = new System.Drawing.Rectangle(1750, 851, 150, 195);     //BOX POSITIION
            PointF HeaderTextPosTest = new PointF(1747.0F, 850.0F);     //NIGHTFYRETV
            PointF MenuOption1PosTest = new PointF(1749.0f, 875.0F);    //TRIGGERBOT
            PointF MenuOption2PosTest = new PointF(1749.0f, 890.0F);    //GODMODE
            PointF MenuOption3PosTest = new PointF(1749.0f, 905.0F);    //NEVERWANTED
            PointF MenuOption4PosTest = new PointF(1749.0f, 920.0F);    //CONTROLLER
            PointF MenuOption8PosTest = new PointF(1749.0f, 940.0f);    //INFINITE AMMO
            PointF MenuOption9PosTest = new PointF(1749.0f, 955.0f);    //RP BOOSTER
            PointF MenuOption10PosTest = new PointF(1749.0f, 970.0f);  //Perfect Weapon
            PointF MenuOption11PosTest = new PointF(1749.0f, 985.0f);  //Fast Car
            PointF MenuOption5PosTest = new PointF(1749.0f, 1000.0F);   //ALL OFF
            PointF MenuOption6PosTest = new PointF(1749.0f, 1015.0F);   //SHOW HIDE
            PointF MenuOption7PosTest = new PointF(1749.0f, 1030.0F);   //QUIT MENU

            g.FillRectangle(blackFill, MENU);
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
            if (!bPerfectWeapon)
            {
                g.DrawString("[3] PERFECT WEAPON", InfoTextFont, whiteBrush, MenuOption10PosTest);
            }
            else
            {
                g.DrawString("[3] PERFECT WEAPON", InfoTextFont, GreenBrush, MenuOption10PosTest);
            }
            //if (!weapon.bPerfectWeapon)
            //{
            //    g.DrawString("[3] PERFECT WEAPON", InfoTextFont, whiteBrush, MenuOption10PosTest);
            //}
            //else
            //{
            //    g.DrawString("[3] PERFECT WEAPON", InfoTextFont, GreenBrush, MenuOption10PosTest);
            //}
            if (!bTuneCar)
            {
                g.DrawString("[4] TUNE CAR", InfoTextFont, whiteBrush, MenuOption11PosTest);
            }
            else
            {
                g.DrawString("[4] TUNE CAR", InfoTextFont, GreenBrush, MenuOption11PosTest);
            }
            g.DrawString("ALL OFF [F9]", InfoTextFont, redBrush, MenuOption5PosTest);
            g.DrawString("SHOW / HIDE [INSERT]", InfoTextFont, redBrush, MenuOption6PosTest);
            g.DrawString("QUIT MENU [DELETE]", InfoTextFont, redBrush, MenuOption7PosTest);
            #endregion

            g.Dispose();
        }
        #endregion

    }
}
