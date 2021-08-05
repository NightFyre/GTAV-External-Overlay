using Simple_GTAV_External_Trainer.Helpers;
using System.Runtime.InteropServices;
using GTAV_External_Trainer.Helpers;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Memory;
using System;
using System.Collections.Generic;

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
        private bool bControllerMode = false;
        private bool bInfiniteAmmo = false;
        private bool bRPBoost = false;
        private bool bPerfectWeapon = false;

        ///Weapondata
        private bool bPistol = false, bCombatPistol = false;
        private bool bMicroSMG = false, bSMG = false;
        private bool bAssaultRifle = false, bSpecialCarbine = false, bBullpupRifle = false, bMilitaryRifle = false;
        private bool bSniperRifle = false, bMarksmanRifle = false;
        private bool bPumpShotgun = false, bHeavyShotgun = false;

        Dictionary<string, string> data = new Dictionary<string, string>();

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

        #region WEAPON HACK
        private void WEAPONHACK()
        {
            while (true)
            {
                if (bPerfectWeapon)
                {
                    #region ENABLE

                    #region PISTOLS

                    //PISTOL
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(PistolData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(PistolData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(PistolData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(PistolData.Range)))
                    {
                        //Store unique gun addresses so that we can disable the patch later
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("PistolBase", baseAddr.ToString("X"));
                        data.Add("PistolWD", (baseAddr + gData.Damage).ToString("X"));
                        data.Add("PistolWS", (baseAddr + gData.Spread).ToString("X"));
                        data.Add("PistolWP", (baseAddr + gData.Penetration).ToString("X"));
                        data.Add("PistolWV", (baseAddr + gData.Velocity).ToString("X"));
                        data.Add("PistolWR", (baseAddr + gData.Range).ToString("X"));
                        data.Add("PistolWRe", (baseAddr + gData.Recoil).ToString("X"));

                        //Write patched values
                        bPistol = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", gData.pDamage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", gData.pSpread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", gData.pPenetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", gData.pVelocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", gData.pRange);
                        m.WriteMemory(gData.WEAPON_RECOIL, "float", gData.pRecoil);
                    }

                    //Combat Pistol
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(CombatPistolData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(CombatPistolData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(CombatPistolData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(CombatPistolData.Range)))
                    {
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("CPistolBase", baseAddr.ToString("X"));
                        data.Add("CPistolWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("CPistolWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("CPistolWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("CPistolWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("CPistolWR", (baseAddr + 0x28C).ToString("X"));

                        bCombatPistol = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", gData.pDamage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", gData.pSpread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", gData.pPenetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", gData.pVelocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", gData.pRange);
                    }
                    #endregion

                    #region SMG's
                    //MicroSMG
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(MicroSMGData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(MicroSMGData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(MicroSMGData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(MicroSMGData.Range)))
                    {
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("MSMGBase", baseAddr.ToString("X"));
                        data.Add("MSMGWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("MSMGWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("MSMGWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("MSMGWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("MSMGWR", (baseAddr + 0x28C).ToString("X"));
                        
                        bMicroSMG = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");

                    }

                    //SMG
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(SMGData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(SMGData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(SMGData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(SMGData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("SMGBase", baseAddr.ToString("X"));
                        data.Add("SMGWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("SMGWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("SMGWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("SMGWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("SMGWR", (baseAddr + 0x28C).ToString("X"));

                        bSMG = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    #endregion

                    #region ASSAULT RIFLES

                    //AssaultRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(AssaultRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(AssaultRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(AssaultRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(AssaultRifleData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("ARBase", baseAddr.ToString("X"));
                        data.Add("ARWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("ARWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("ARWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("ARWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("ARWR", (baseAddr + 0x28C).ToString("X"));

                        bAssaultRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //SpecialCarbine
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(SpecialCarbineData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(SpecialCarbineData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(SpecialCarbineData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(SpecialCarbineData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("SCBase", baseAddr.ToString("X"));
                        data.Add("SCWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("SCWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("SCWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("SCWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("SCWR", (baseAddr + 0x28C).ToString("X"));

                        bSpecialCarbine = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //BullpupRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(BullpupRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(BullpupRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(BullpupRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(BullpupRifleData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("BPRBase", baseAddr.ToString("X"));
                        data.Add("BPRWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("BPRWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("BPRWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("BPRWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("BPRWR", (baseAddr + 0x28C).ToString("X"));

                        bBullpupRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //MilitaryRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == 37.5) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(MilitaryRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(MilitaryRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(MilitaryRifleData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("MiRBase", baseAddr.ToString("X"));
                        data.Add("MiRWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("MiRWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("MiRWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("MiRWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("MiRWR", (baseAddr + 0x28C).ToString("X"));

                        bMilitaryRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    #endregion

                    #region SNIPER RIFLES

                    //MarksmanRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(MarksmanRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(MarksmanRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(MarksmanRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(MarksmanRifleData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("MaRBase", baseAddr.ToString("X"));
                        data.Add("MaRWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("MaRWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("MaRWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("MaRWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("MaRWR", (baseAddr + 0x28C).ToString("X"));

                        bMarksmanRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //SniperRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(SniperRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(SniperRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(SniperRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(SniperRifleData.Range)))
                    {
                        //We need to store the addresses that contain the weapon data
                        var baseAddr = m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");
                        data.Add("SnRBase", baseAddr.ToString("X"));
                        data.Add("SnRWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("SnRWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("SnRWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("SnRWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("SnRWR", (baseAddr + 0x28C).ToString("X"));

                        bSniperRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    #endregion

                    #region SHOTGUNS
                    //Pump Shotgun
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(PumpShotgunData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(PumpShotgunData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(PumpShotgunData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(PumpShotgunData.Range)))
                    {
                        var baseAddr = m.ReadLong(gData.WeaponID);
                        data.Add("PumpSGBase", baseAddr.ToString("X"));
                        data.Add("PumpSGWD", (baseAddr + gData.Damage).ToString("X"));
                        data.Add("PumpSGWS", (baseAddr + gData.Spread).ToString("X"));
                        data.Add("PumpSGWP", (baseAddr + gData.Penetration).ToString("X"));
                        data.Add("PumpSGWV", (baseAddr + gData.Velocity).ToString("X"));
                        data.Add("PumpSGWR", (baseAddr + gData.Range).ToString("X"));

                        bPumpShotgun = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", gData.pDamage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", gData.pSpread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", gData.pPenetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", gData.pVelocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", gData.pRange);
                    }
                    //HeavyShotgun
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(HeavyShotgunData.Damage)) &&
                    (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(HeavyShotgunData.Spread)) &&
                    (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(HeavyShotgunData.Velocity)) &&
                    (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(HeavyShotgunData.Range)))
                    {
                        var baseAddr = m.ReadLong(gData.WeaponID);
                        data.Add("HSGBase", baseAddr.ToString("X"));
                        data.Add("HSGWD", (baseAddr + 0xB0).ToString("X"));
                        data.Add("HSGWS", (baseAddr + 0x7C).ToString("X"));
                        data.Add("HSGWP", (baseAddr + 0x110).ToString("X"));
                        data.Add("HSGWV", (baseAddr + 0x11C).ToString("X"));
                        data.Add("HSGWR", (baseAddr + 0x28C).ToString("X"));

                        bHeavyShotgun = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    #endregion

                    #region EXPLOSIVES
                    #endregion

                    #endregion
                }
                else if (!bPerfectWeapon)
                {
                    #region DISABLE

                    #region PISTOLS
                    if (bPistol)
                    {
                        string Damage = data["PistolWD"];
                        string Spread = data["PistolWS"];
                        string Penetration = data["PistolWP"];
                        string Velocity = data["PistolWV"];
                        string Range = data["PistolWR"];
                        string Recoil = data["PistolWRe"];
                        m.WriteMemory(Damage, "float", PistolData.Damage);
                        m.WriteMemory(Spread, "float", PistolData.Spread);
                        m.WriteMemory(Penetration, "float", PistolData.Penetration);
                        m.WriteMemory(Velocity, "float", PistolData.Velocity);
                        m.WriteMemory(Range, "float", PistolData.Range);
                        m.WriteMemory(Recoil, "float", PistolData.Recoil);
                        data.Remove("PistolBase");
                        data.Remove("PistolWD");
                        data.Remove("PistolWS");
                        data.Remove("PistolWP");
                        data.Remove("PistolWV");
                        data.Remove("PistolWR");
                        bPistol = false;
                    }
                    if (bCombatPistol)
                    {
                        string Damage = data["CPistolWD"];
                        string Spread = data["CPistolWS"];
                        string Penetration = data["CPistolWP"];
                        string Velocity = data["CPistolWV"];
                        string Range = data["CPistolWR"];
                        m.WriteMemory(Damage, "float", CombatPistolData.Damage);
                        m.WriteMemory(Spread, "float", CombatPistolData.Spread);
                        m.WriteMemory(Penetration, "float", CombatPistolData.Penetration);
                        m.WriteMemory(Velocity, "float", CombatPistolData.Velocity);
                        m.WriteMemory(Range, "float", CombatPistolData.Range);
                        data.Remove("CPistolBase");
                        data.Remove("CPistolWD");
                        data.Remove("CPistolWS");
                        data.Remove("CPistolWP");
                        data.Remove("CPistolWV");
                        data.Remove("CPistolWR");
                        bCombatPistol = false;
                    }
                    #endregion

                    #region SMG's
                    if (bMicroSMG)
                    {
                        string Damage = data["MSMGWD"];
                        string Spread = data["MSMGWS"];
                        string Penetration = data["MSMGWP"];
                        string Velocity = data["MSMGWV"];
                        string Range = data["MSMGWR"];
                        m.WriteMemory(Damage, "float", MicroSMGData.Damage);
                        m.WriteMemory(Spread, "float", MicroSMGData.Spread);
                        m.WriteMemory(Penetration, "float", MicroSMGData.Penetration);
                        m.WriteMemory(Velocity, "float", MicroSMGData.Velocity);
                        m.WriteMemory(Range, "float", MicroSMGData.Range);
                        data.Remove("MSMGBase");
                        data.Remove("MSMGWD");
                        data.Remove("MSMGWS");
                        data.Remove("MSMGWP");
                        data.Remove("MSMGWV");
                        data.Remove("MSMGWR");
                        bMicroSMG = false;
                    }
                    if (bSMG)
                    {
                        string Damage = data["SMGWD"];
                        string Spread = data["SMGWS"];
                        string Penetration = data["SMGWP"];
                        string Velocity = data["SMGWV"];
                        string Range = data["SMGWR"];
                        m.WriteMemory(Damage, "float", SMGData.Damage);
                        m.WriteMemory(Spread, "float", SMGData.Spread);
                        m.WriteMemory(Penetration, "float", SMGData.Penetration);
                        m.WriteMemory(Velocity, "float", SMGData.Velocity);
                        m.WriteMemory(Range, "float", SMGData.Range);
                        data.Remove("SMGBase");
                        data.Remove("SMGWD");
                        data.Remove("SMGWS");
                        data.Remove("SMGWP");
                        data.Remove("SMGWV");
                        data.Remove("SMGWR");
                        bSMG = false;
                    }
                    #endregion

                    #region ASSAULT RIFLES
                    if (bAssaultRifle)
                    {
                        string Damage = data["ARWD"];
                        string Spread = data["ARWS"];
                        string Penetration = data["ARWP"];
                        string Velocity = data["ARWV"];
                        string Range = data["ARWR"];
                        m.WriteMemory(Damage, "float", AssaultRifleData.Damage); ;
                        m.WriteMemory(Spread, "float", AssaultRifleData.Spread);
                        m.WriteMemory(Penetration, "float", AssaultRifleData.Penetration);
                        m.WriteMemory(Velocity, "float", AssaultRifleData.Velocity);
                        m.WriteMemory(Range, "float", AssaultRifleData.Range);
                        data.Remove("ARBase");
                        data.Remove("ARWD");
                        data.Remove("ARWS");
                        data.Remove("ARWP");
                        data.Remove("ARWV");
                        data.Remove("ARWR");
                        bAssaultRifle = false;
                    }
                    if (bSpecialCarbine)
                    {
                        string Damage = data["SCWD"];
                        string Spread = data["SCWS"];
                        string Penetration = data["SCWP"];
                        string Velocity = data["SCWV"];
                        string Range = data["SCWR"];
                        m.WriteMemory(Damage, "float", SpecialCarbineData.Damage);
                        m.WriteMemory(Spread, "float", SpecialCarbineData.Spread);
                        m.WriteMemory(Penetration, "float", SpecialCarbineData.Penetration);
                        m.WriteMemory(Velocity, "float", SpecialCarbineData.Velocity);
                        m.WriteMemory(Range, "float", SpecialCarbineData.Range);
                        data.Remove("SCBase");
                        data.Remove("SCWD");
                        data.Remove("SCWS");
                        data.Remove("SCWP");
                        data.Remove("SCWV");
                        data.Remove("SCWR");
                        bSpecialCarbine = false;
                    }
                    if (bBullpupRifle)
                    {
                        string Damage = data["BPRWD"];
                        string Spread = data["BPRWS"];
                        string Penetration = data["BPRWP"];
                        string Velocity = data["BPRWV"];
                        string Range = data["BPRWR"];
                        m.WriteMemory(Damage, "float", BullpupRifleData.Damage);
                        m.WriteMemory(Spread, "float", BullpupRifleData.Spread);
                        m.WriteMemory(Penetration, "float", BullpupRifleData.Penetration);
                        m.WriteMemory(Velocity, "float", BullpupRifleData.Velocity);
                        m.WriteMemory(Range, "float", BullpupRifleData.Range);
                        data.Remove("BPRBase");
                        data.Remove("BPRWD");
                        data.Remove("BPRWS");
                        data.Remove("BPRWP");
                        data.Remove("BPRWV");
                        data.Remove("BPRWR");
                        bBullpupRifle = false;
                    }
                    if (bMilitaryRifle)
                    {
                        string Damage = data["MiRWD"];
                        string Spread = data["MiRWS"];
                        string Penetration = data["MiRWP"];
                        string Velocity = data["MiRWV"];
                        string Range = data["MiRWR"];
                        m.WriteMemory(Damage, "float", MilitaryRifleData.Damage);
                        m.WriteMemory(Spread, "float", MilitaryRifleData.Spread);
                        m.WriteMemory(Penetration, "float", MilitaryRifleData.Penetration);
                        m.WriteMemory(Velocity, "float", MilitaryRifleData.Velocity);
                        m.WriteMemory(Range, "float", MilitaryRifleData.Range);
                        data.Remove("MiRBase");
                        data.Remove("MiRWD");
                        data.Remove("MiRWS");
                        data.Remove("MiRWP");
                        data.Remove("MiRWV");
                        data.Remove("MiRWR");
                        bMilitaryRifle = false;
                    }
                    #endregion

                    #region SNIPER RIFLES
                    if (bMarksmanRifle)
                    {
                        string Damage = data["MaRWD"];
                        string Spread = data["MaRWS"];
                        string Penetration = data["MaRWP"];
                        string Velocity = data["MaRWV"];
                        string Range = data["MaRWR"];
                        m.WriteMemory(Damage, "float", MarksmanRifleData.Damage);
                        m.WriteMemory(Spread, "float", MarksmanRifleData.Spread);
                        m.WriteMemory(Penetration, "float", MarksmanRifleData.Penetration);
                        m.WriteMemory(Velocity, "float", MarksmanRifleData.Velocity);
                        m.WriteMemory(Range, "float", MarksmanRifleData.Range);
                        data.Remove("MaRBase");
                        data.Remove("MaRWD");
                        data.Remove("MaRWS");
                        data.Remove("MaRWP");
                        data.Remove("MaRWV");
                        data.Remove("MaRWR");
                        bMarksmanRifle = false;
                    }
                    if (bSniperRifle)
                    {
                        string Damage = data["SnRWD"];
                        string Spread = data["SnRWS"];
                        string Penetration = data["SnRWP"];
                        string Velocity = data["SnRWV"];
                        string Range = data["SnRWR"];
                        m.WriteMemory(Damage, "float", SniperRifleData.Damage);
                        m.WriteMemory(Spread, "float", SniperRifleData.Spread);
                        m.WriteMemory(Penetration, "float", SniperRifleData.Penetration);
                        m.WriteMemory(Velocity, "float", SniperRifleData.Velocity);
                        m.WriteMemory(Range, "float", SniperRifleData.Range);
                        data.Remove("SnRBase");
                        data.Remove("SnRWD");
                        data.Remove("SnRWS");
                        data.Remove("SnRWP");
                        data.Remove("SnRWV");
                        data.Remove("SnRWR");
                        bSniperRifle = false;
                    }
                    #endregion

                    #region SHOTGUNS
                    if (bPumpShotgun)
                    {
                        string Damage = data["PumpSGWD"];
                        string Spread = data["PumpSGWS"];
                        string Penetration = data["PumpSGWP"];
                        string Velocity = data["PumpSGWV"];
                        string Range = data["PumpSGWR"];
                        m.WriteMemory(Damage, "float", PumpShotgunData.Damage);
                        m.WriteMemory(Spread, "float", PumpShotgunData.Spread);
                        m.WriteMemory(Penetration, "float", PumpShotgunData.Penetration);
                        m.WriteMemory(Velocity, "float", PumpShotgunData.Velocity);
                        m.WriteMemory(Range, "float", PumpShotgunData.Range);
                        data.Remove("PumpSGBase");
                        data.Remove("PumpSGWD");
                        data.Remove("PumpSGWS");
                        data.Remove("PumpSGWP");
                        data.Remove("PumpSGWV");
                        data.Remove("PumpSGWR");
                        bPumpShotgun = false;
                    }
                    if (bHeavyShotgun)
                    {
                        string Damage = data["HSGWD"];
                        string Spread = data["HSGWS"];
                        string Penetration = data["HSGWP"];
                        string Velocity = data["HSGWV"];
                        string Range = data["HSGWR"];
                        m.WriteMemory(Damage, "float", HeavyShotgunData.Damage);
                        m.WriteMemory(Spread, "float", HeavyShotgunData.Spread);
                        m.WriteMemory(Penetration, "float", HeavyShotgunData.Penetration);
                        m.WriteMemory(Velocity, "float", HeavyShotgunData.Velocity);
                        m.WriteMemory(Range, "float", HeavyShotgunData.Range);
                        data.Remove("HSGBase");
                        data.Remove("HSGWD");
                        data.Remove("HSGWS");
                        data.Remove("HSGWP");
                        data.Remove("HSGWV");
                        data.Remove("HSGWR");
                        bHeavyShotgun = false;
                    }
                    #endregion

                    #region EXPLOSIVES
                    #endregion

                    #endregion
                }
                Thread.Sleep(100);
            }
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
            if (bPerfectWeapon && bAllOff)
            {
                #region PISTOLS
                if (bPistol)
                {
                    string Damage = data["PistolWD"];
                    string Spread = data["PistolWS"];
                    string Penetration = data["PistolWP"];
                    string Velocity = data["PistolWV"];
                    string Range = data["PistolWR"];
                    string Recoil = data["PistolWRe"];
                    m.WriteMemory(Damage, "float", PistolData.Damage);
                    m.WriteMemory(Spread, "float", PistolData.Spread);
                    m.WriteMemory(Penetration, "float", PistolData.Penetration);
                    m.WriteMemory(Velocity, "float", PistolData.Velocity);
                    m.WriteMemory(Range, "float", PistolData.Range);
                    m.WriteMemory(Recoil, "float", PistolData.Recoil);
                    data.Remove("PistolBase");
                    data.Remove("PistolWD");
                    data.Remove("PistolWS");
                    data.Remove("PistolWP");
                    data.Remove("PistolWV");
                    data.Remove("PistolWR");
                    bPistol = false;
                }
                if (bCombatPistol)
                {
                    string Damage = data["CPistolWD"];
                    string Spread = data["CPistolWS"];
                    string Penetration = data["CPistolWP"];
                    string Velocity = data["CPistolWV"];
                    string Range = data["CPistolWR"];
                    m.WriteMemory(Damage, "float", CombatPistolData.Damage);
                    m.WriteMemory(Spread, "float", CombatPistolData.Spread);
                    m.WriteMemory(Penetration, "float", CombatPistolData.Penetration);
                    m.WriteMemory(Velocity, "float", CombatPistolData.Velocity);
                    m.WriteMemory(Range, "float", CombatPistolData.Range);
                    data.Remove("CPistolBase");
                    data.Remove("CPistolWD");
                    data.Remove("CPistolWS");
                    data.Remove("CPistolWP");
                    data.Remove("CPistolWV");
                    data.Remove("CPistolWR");
                    bCombatPistol = false;
                }
                #endregion

                if (bMicroSMG)
                {
                    string Damage = data["MSMGWD"];
                    string Spread = data["MSMGWS"];
                    string Penetration = data["MSMGWP"];
                    string Velocity = data["MSMGWV"];
                    string Range = data["MSMGWR"];
                    m.WriteMemory(Damage, "float", MicroSMGData.Damage);
                    m.WriteMemory(Spread, "float", MicroSMGData.Spread);
                    m.WriteMemory(Penetration, "float", MicroSMGData.Penetration);
                    m.WriteMemory(Velocity, "float", MicroSMGData.Velocity);
                    m.WriteMemory(Range, "float", MicroSMGData.Range);
                    data.Remove("MSMGBase");
                    data.Remove("MSMGWD");
                    data.Remove("MSMGWS");
                    data.Remove("MSMGWP");
                    data.Remove("MSMGWV");
                    data.Remove("MSMGWR");
                    bMicroSMG = false;
                }
                if (bSMG)
                {
                    string Damage = data["SMGWD"];
                    string Spread = data["SMGWS"];
                    string Penetration = data["SMGWP"];
                    string Velocity = data["SMGWV"];
                    string Range = data["SMGWR"];
                    m.WriteMemory(Damage, "float", SMGData.Damage);
                    m.WriteMemory(Spread, "float", SMGData.Spread);
                    m.WriteMemory(Penetration, "float", SMGData.Penetration);
                    m.WriteMemory(Velocity, "float", SMGData.Velocity);
                    m.WriteMemory(Range, "float", SMGData.Range);
                    data.Remove("SMGBase");
                    data.Remove("SMGWD");
                    data.Remove("SMGWS");
                    data.Remove("SMGWP");
                    data.Remove("SMGWV");
                    data.Remove("SMGWR");
                    bSMG = false;
                }
                if (bAssaultRifle)
                {
                    string Damage = data["ARWD"];
                    string Spread = data["ARWS"];
                    string Penetration = data["ARWP"];
                    string Velocity = data["ARWV"];
                    string Range = data["ARWR"];
                    m.WriteMemory(Damage, "float", AssaultRifleData.Damage); ;
                    m.WriteMemory(Spread, "float", AssaultRifleData.Spread);
                    m.WriteMemory(Penetration, "float", AssaultRifleData.Penetration);
                    m.WriteMemory(Velocity, "float", AssaultRifleData.Velocity);
                    m.WriteMemory(Range, "float", AssaultRifleData.Range);
                    data.Remove("ARBase");
                    data.Remove("ARWD");
                    data.Remove("ARWS");
                    data.Remove("ARWP");
                    data.Remove("ARWV");
                    data.Remove("ARWR");
                    bAssaultRifle = false;
                }
                if (bSpecialCarbine)
                {
                    string Damage = data["SCWD"];
                    string Spread = data["SCWS"];
                    string Penetration = data["SCWP"];
                    string Velocity = data["SCWV"];
                    string Range = data["SCWR"];
                    m.WriteMemory(Damage, "float", SpecialCarbineData.Damage);
                    m.WriteMemory(Spread, "float", SpecialCarbineData.Spread);
                    m.WriteMemory(Penetration, "float", SpecialCarbineData.Penetration);
                    m.WriteMemory(Velocity, "float", SpecialCarbineData.Velocity);
                    m.WriteMemory(Range, "float", SpecialCarbineData.Range);
                    data.Remove("ARBase");
                    data.Remove("ARWD");
                    data.Remove("ARWS");
                    data.Remove("ARWP");
                    data.Remove("ARWV");
                    data.Remove("ARWR");
                    bSpecialCarbine = false;
                }
                if (bBullpupRifle)
                {
                    string Damage = data["BPRWD"];
                    string Spread = data["BPRWS"];
                    string Penetration = data["BPRWP"];
                    string Velocity = data["BPRWV"];
                    string Range = data["BPRWR"];
                    m.WriteMemory(Damage, "float", BullpupRifleData.Damage);
                    m.WriteMemory(Spread, "float", BullpupRifleData.Spread);
                    m.WriteMemory(Penetration, "float", BullpupRifleData.Penetration);
                    m.WriteMemory(Velocity, "float", BullpupRifleData.Velocity);
                    m.WriteMemory(Range, "float", BullpupRifleData.Range);
                    data.Remove("BPRBase");
                    data.Remove("BPRWD");
                    data.Remove("BPRWS");
                    data.Remove("BPRWP");
                    data.Remove("BPRWV");
                    data.Remove("BPRWR");
                    bBullpupRifle = false;
                }
                if (bMilitaryRifle)
                {
                    string Damage = data["MiRWD"];
                    string Spread = data["MiRWS"];
                    string Penetration = data["MiRWP"];
                    string Velocity = data["MiRWV"];
                    string Range = data["MiRWR"];
                    m.WriteMemory(Damage, "float", MilitaryRifleData.Damage);
                    m.WriteMemory(Spread, "float", MilitaryRifleData.Spread);
                    m.WriteMemory(Penetration, "float", MilitaryRifleData.Penetration);
                    m.WriteMemory(Velocity, "float", MilitaryRifleData.Velocity);
                    m.WriteMemory(Range, "float", MilitaryRifleData.Range);
                    data.Remove("MiRBase");
                    data.Remove("MiRWD");
                    data.Remove("MiRWS");
                    data.Remove("MiRWP");
                    data.Remove("MiRWV");
                    data.Remove("MiRWR");
                    bMilitaryRifle = false;
                }
                if (bMarksmanRifle)
                {
                    string Damage = data["MaRWD"];
                    string Spread = data["MaRWS"];
                    string Penetration = data["MaRWP"];
                    string Velocity = data["MaRWV"];
                    string Range = data["MaRWR"];
                    m.WriteMemory(Damage, "float", MarksmanRifleData.Damage);
                    m.WriteMemory(Spread, "float", MarksmanRifleData.Spread);
                    m.WriteMemory(Penetration, "float", MarksmanRifleData.Penetration);
                    m.WriteMemory(Velocity, "float", MarksmanRifleData.Velocity);
                    m.WriteMemory(Range, "float", MarksmanRifleData.Range);
                    data.Remove("MaRBase");
                    data.Remove("MaRWD");
                    data.Remove("MaRWS");
                    data.Remove("MaRWP");
                    data.Remove("MaRWV");
                    data.Remove("MaRWR");
                    bMarksmanRifle = false;
                }
                if (bSniperRifle)
                {
                    string Damage = data["SnRWD"];
                    string Spread = data["SnRWS"];
                    string Penetration = data["SnRWP"];
                    string Velocity = data["SnRWV"];
                    string Range = data["SnRWR"];
                    m.WriteMemory(Damage, "float", SniperRifleData.Damage);
                    m.WriteMemory(Spread, "float", SniperRifleData.Spread);
                    m.WriteMemory(Penetration, "float", SniperRifleData.Penetration);
                    m.WriteMemory(Velocity, "float", SniperRifleData.Velocity);
                    m.WriteMemory(Range, "float", SniperRifleData.Range);
                    data.Remove("SnRBase");
                    data.Remove("SnRWD");
                    data.Remove("SnRWS");
                    data.Remove("SnRWP");
                    data.Remove("SnRWV");
                    data.Remove("SnRWR");
                    bSniperRifle = false;
                }
                if (bPumpShotgun)
                {
                    string Damage = data["PumpSGWD"];
                    string Spread = data["PumpSGWS"];
                    string Penetration = data["PumpSGWP"];
                    string Velocity = data["PumpSGWV"];
                    string Range = data["PumpSGWR"];
                    m.WriteMemory(Damage, "float", PumpShotgunData.Damage);
                    m.WriteMemory(Spread, "float", PumpShotgunData.Spread);
                    m.WriteMemory(Penetration, "float", PumpShotgunData.Penetration);
                    m.WriteMemory(Velocity, "float", PumpShotgunData.Velocity);
                    m.WriteMemory(Range, "float", PumpShotgunData.Range);
                    data.Remove("PumpSGBase");
                    data.Remove("PumpSGWD");
                    data.Remove("PumpSGWS");
                    data.Remove("PumpSGWP");
                    data.Remove("PumpSGWV");
                    data.Remove("PumpSGWR");
                    bPumpShotgun = false;
                }
                if (bHeavyShotgun)
                {
                    string Damage = data["HSGWD"];
                    string Spread = data["HSGWS"];
                    string Penetration = data["HSGWP"];
                    string Velocity = data["HSGWV"];
                    string Range = data["HSGWR"];
                    m.WriteMemory(Damage, "float", HeavyShotgunData.Damage);
                    m.WriteMemory(Spread, "float", HeavyShotgunData.Spread);
                    m.WriteMemory(Penetration, "float", HeavyShotgunData.Penetration);
                    m.WriteMemory(Velocity, "float", HeavyShotgunData.Velocity);
                    m.WriteMemory(Range, "float", HeavyShotgunData.Range);
                    data.Remove("HSGBase");
                    data.Remove("HSGWD");
                    data.Remove("HSGWS");
                    data.Remove("HSGWP");
                    data.Remove("HSGWV");
                    data.Remove("HSGWR");
                    bHeavyShotgun = false;
                }
                bPerfectWeapon = false;
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

            #region PISTOLS
            if (bPistol)
            {
                string Damage = data["PistolWD"];
                string Spread = data["PistolWS"];
                string Penetration = data["PistolWP"];
                string Velocity = data["PistolWV"];
                string Range = data["PistolWR"];
                string Recoil = data["PistolWRe"];
                m.WriteMemory(Damage, "float", PistolData.Damage);
                m.WriteMemory(Spread, "float", PistolData.Spread);
                m.WriteMemory(Penetration, "float", PistolData.Penetration);
                m.WriteMemory(Velocity, "float", PistolData.Velocity);
                m.WriteMemory(Range, "float", PistolData.Range);
                m.WriteMemory(Recoil, "float", PistolData.Recoil);
                data.Remove("PistolBase");
                data.Remove("PistolWD");
                data.Remove("PistolWS");
                data.Remove("PistolWP");
                data.Remove("PistolWV");
                data.Remove("PistolWR");
                bPistol = false;
            }
            if (bCombatPistol)
            {
                string Damage = data["CPistolWD"];
                string Spread = data["CPistolWS"];
                string Penetration = data["CPistolWP"];
                string Velocity = data["CPistolWV"];
                string Range = data["CPistolWR"];
                m.WriteMemory(Damage, "float", CombatPistolData.Damage);
                m.WriteMemory(Spread, "float", CombatPistolData.Spread);
                m.WriteMemory(Penetration, "float", CombatPistolData.Penetration);
                m.WriteMemory(Velocity, "float", CombatPistolData.Velocity);
                m.WriteMemory(Range, "float", CombatPistolData.Range);
                data.Remove("CPistolBase");
                data.Remove("CPistolWD");
                data.Remove("CPistolWS");
                data.Remove("CPistolWP");
                data.Remove("CPistolWV");
                data.Remove("CPistolWR");
                bCombatPistol = false;
            }
            #endregion

            if (bMicroSMG)
            {
                string Damage = data["MSMGWD"];
                string Spread = data["MSMGWS"];
                string Penetration = data["MSMGWP"];
                string Velocity = data["MSMGWV"];
                string Range = data["MSMGWR"];
                m.WriteMemory(Damage, "float", MicroSMGData.Damage);
                m.WriteMemory(Spread, "float", MicroSMGData.Spread);
                m.WriteMemory(Penetration, "float", MicroSMGData.Penetration);
                m.WriteMemory(Velocity, "float", MicroSMGData.Velocity);
                m.WriteMemory(Range, "float", MicroSMGData.Range);
                data.Remove("MSMGBase");
                data.Remove("MSMGWD");
                data.Remove("MSMGWS");
                data.Remove("MSMGWP");
                data.Remove("MSMGWV");
                data.Remove("MSMGWR");
                bMicroSMG = false;
            }
            if (bSMG)
            {
                string Damage = data["SMGWD"];
                string Spread = data["SMGWS"];
                string Penetration = data["SMGWP"];
                string Velocity = data["SMGWV"];
                string Range = data["SMGWR"];
                m.WriteMemory(Damage, "float", SMGData.Damage);
                m.WriteMemory(Spread, "float", SMGData.Spread);
                m.WriteMemory(Penetration, "float", SMGData.Penetration);
                m.WriteMemory(Velocity, "float", SMGData.Velocity);
                m.WriteMemory(Range, "float", SMGData.Range);
                data.Remove("SMGBase");
                data.Remove("SMGWD");
                data.Remove("SMGWS");
                data.Remove("SMGWP");
                data.Remove("SMGWV");
                data.Remove("SMGWR");
                bSMG = false;
            }
            if (bAssaultRifle)
            {
                string Damage = data["ARWD"];
                string Spread = data["ARWS"];
                string Penetration = data["ARWP"];
                string Velocity = data["ARWV"];
                string Range = data["ARWR"];
                m.WriteMemory(Damage, "float", AssaultRifleData.Damage); ;
                m.WriteMemory(Spread, "float", AssaultRifleData.Spread);
                m.WriteMemory(Penetration, "float", AssaultRifleData.Penetration);
                m.WriteMemory(Velocity, "float", AssaultRifleData.Velocity);
                m.WriteMemory(Range, "float", AssaultRifleData.Range);
                data.Remove("ARBase");
                data.Remove("ARWD");
                data.Remove("ARWS");
                data.Remove("ARWP");
                data.Remove("ARWV");
                data.Remove("ARWR");
                bAssaultRifle = false;
            }
            if (bSpecialCarbine)
            {
                string Damage = data["SCWD"];
                string Spread = data["SCWS"];
                string Penetration = data["SCWP"];
                string Velocity = data["SCWV"];
                string Range = data["SCWR"];
                m.WriteMemory(Damage, "float", SpecialCarbineData.Damage);
                m.WriteMemory(Spread, "float", SpecialCarbineData.Spread);
                m.WriteMemory(Penetration, "float", SpecialCarbineData.Penetration);
                m.WriteMemory(Velocity, "float", SpecialCarbineData.Velocity);
                m.WriteMemory(Range, "float", SpecialCarbineData.Range);
                data.Remove("ARBase");
                data.Remove("ARWD");
                data.Remove("ARWS");
                data.Remove("ARWP");
                data.Remove("ARWV");
                data.Remove("ARWR");
                bSpecialCarbine = false;
            }
            if (bBullpupRifle)
            {
                string Damage = data["BPRWD"];
                string Spread = data["BPRWS"];
                string Penetration = data["BPRWP"];
                string Velocity = data["BPRWV"];
                string Range = data["BPRWR"];
                m.WriteMemory(Damage, "float", BullpupRifleData.Damage);
                m.WriteMemory(Spread, "float", BullpupRifleData.Spread);
                m.WriteMemory(Penetration, "float", BullpupRifleData.Penetration);
                m.WriteMemory(Velocity, "float", BullpupRifleData.Velocity);
                m.WriteMemory(Range, "float", BullpupRifleData.Range);
                data.Remove("BPRBase");
                data.Remove("BPRWD");
                data.Remove("BPRWS");
                data.Remove("BPRWP");
                data.Remove("BPRWV");
                data.Remove("BPRWR");
                bBullpupRifle = false;
            }
            if (bMilitaryRifle)
            {
                string Damage = data["MiRWD"];
                string Spread = data["MiRWS"];
                string Penetration = data["MiRWP"];
                string Velocity = data["MiRWV"];
                string Range = data["MiRWR"];
                m.WriteMemory(Damage, "float", MilitaryRifleData.Damage);
                m.WriteMemory(Spread, "float", MilitaryRifleData.Spread);
                m.WriteMemory(Penetration, "float", MilitaryRifleData.Penetration);
                m.WriteMemory(Velocity, "float", MilitaryRifleData.Velocity);
                m.WriteMemory(Range, "float", MilitaryRifleData.Range);
                data.Remove("MiRBase");
                data.Remove("MiRWD");
                data.Remove("MiRWS");
                data.Remove("MiRWP");
                data.Remove("MiRWV");
                data.Remove("MiRWR");
                bMilitaryRifle = false;
            }
            if (bMarksmanRifle)
            {
                string Damage = data["MaRWD"];
                string Spread = data["MaRWS"];
                string Penetration = data["MaRWP"];
                string Velocity = data["MaRWV"];
                string Range = data["MaRWR"];
                m.WriteMemory(Damage, "float", MarksmanRifleData.Damage);
                m.WriteMemory(Spread, "float", MarksmanRifleData.Spread);
                m.WriteMemory(Penetration, "float", MarksmanRifleData.Penetration);
                m.WriteMemory(Velocity, "float", MarksmanRifleData.Velocity);
                m.WriteMemory(Range, "float", MarksmanRifleData.Range);
                data.Remove("MaRBase");
                data.Remove("MaRWD");
                data.Remove("MaRWS");
                data.Remove("MaRWP");
                data.Remove("MaRWV");
                data.Remove("MaRWR");
                bMarksmanRifle = false;
            }
            if (bSniperRifle)
            {
                string Damage = data["SnRWD"];
                string Spread = data["SnRWS"];
                string Penetration = data["SnRWP"];
                string Velocity = data["SnRWV"];
                string Range = data["SnRWR"];
                m.WriteMemory(Damage, "float", SniperRifleData.Damage);
                m.WriteMemory(Spread, "float", SniperRifleData.Spread);
                m.WriteMemory(Penetration, "float", SniperRifleData.Penetration);
                m.WriteMemory(Velocity, "float", SniperRifleData.Velocity);
                m.WriteMemory(Range, "float", SniperRifleData.Range);
                data.Remove("SnRBase");
                data.Remove("SnRWD");
                data.Remove("SnRWS");
                data.Remove("SnRWP");
                data.Remove("SnRWV");
                data.Remove("SnRWR");
                bSniperRifle = false;
            }
            if (bPumpShotgun)
            {
                string Damage = data["PumpSGWD"];
                string Spread = data["PumpSGWS"];
                string Penetration = data["PumpSGWP"];
                string Velocity = data["PumpSGWV"];
                string Range = data["PumpSGWR"];
                m.WriteMemory(Damage, "float", PumpShotgunData.Damage);
                m.WriteMemory(Spread, "float", PumpShotgunData.Spread);
                m.WriteMemory(Penetration, "float", PumpShotgunData.Penetration);
                m.WriteMemory(Velocity, "float", PumpShotgunData.Velocity);
                m.WriteMemory(Range, "float", PumpShotgunData.Range);
                data.Remove("PumpSGBase");
                data.Remove("PumpSGWD");
                data.Remove("PumpSGWS");
                data.Remove("PumpSGWP");
                data.Remove("PumpSGWV");
                data.Remove("PumpSGWR");
                bPumpShotgun = false;
            }
            if (bHeavyShotgun)
            {
                string Damage = data["HSGWD"];
                string Spread = data["HSGWS"];
                string Penetration = data["HSGWP"];
                string Velocity = data["HSGWV"];
                string Range = data["HSGWR"];
                m.WriteMemory(Damage, "float", HeavyShotgunData.Damage);
                m.WriteMemory(Spread, "float", HeavyShotgunData.Spread);
                m.WriteMemory(Penetration, "float", HeavyShotgunData.Penetration);
                m.WriteMemory(Velocity, "float", HeavyShotgunData.Velocity);
                m.WriteMemory(Range, "float", HeavyShotgunData.Range);
                data.Remove("HSGBase");
                data.Remove("HSGWD");
                data.Remove("HSGWS");
                data.Remove("HSGWP");
                data.Remove("HSGWV");
                data.Remove("HSGWR");
                bHeavyShotgun = false;
            }
            #endregion

            bAutoShoot = false;
            bGodMode = false;
            bNeverWanted = false;
            bControllerMode = false;
            bInfiniteAmmo = false;
            bPerfectWeapon = false;
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
                    this.bPerfectWeapon = !this.bPerfectWeapon;
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
                Thread PERFECTWEAPONHACK = new Thread(WEAPONHACK) { IsBackground = true };
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
            if (!bPerfectWeapon)
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
