using Simple_GTAV_External_Trainer.Helpers;
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
        private bool bControllerMode = false;
        private bool bInfiniteAmmo = false;
        private bool bRPBoost = false;
        private bool bPerfectWeapon = false;

        ///Weapondata
        private bool bPistol = false, bCombatPistol = false;
        private bool bMicroSMG = false, bSMG = false;
        private bool bAssaultRifle = false, bSpecialCarbine = false, bBullpupRifle = false, bMilitaryRifle = false;
        private bool bSniperRifle = false, bMarksmanRifle = false;
        private bool bHeavyShotgun = false;

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

        private void WEAPONHACK()
        {
            while (true)
            {
                if (bPerfectWeapon)
                {
                    #region ENABLE

                    //PISTOL
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(PistolData.Damage)) && 
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(PistolData.Spread)) && 
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(PistolData.Velocity)) && 
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(PistolData.Range)))
                    {
                        bPistol = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //Combat Pistol
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(CombatPistolData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(CombatPistolData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(CombatPistolData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(CombatPistolData.Range)))
                    {
                        bCombatPistol = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //MicroSMG
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(MicroSMGData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(MicroSMGData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(MicroSMGData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(MicroSMGData.Range)))
                    {
                        bMicroSMG = true;

                    }

                    //SMG
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(SMGData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(SMGData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(SMGData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(SMGData.Range)))
                    {
                        bSMG = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //AssaultRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(AssaultRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(AssaultRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(AssaultRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(AssaultRifleData.Range)))
                    {
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
                        bMilitaryRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //MarksmanRifle
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(MarksmanRifleData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(MarksmanRifleData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(MarksmanRifleData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(MarksmanRifleData.Range)))
                    {
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
                        bSniperRifle = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }

                    //HeavyShotgun
                    if ((m.ReadFloat(gData.WEAPON_DAMAGE) == Convert.ToInt32(HeavyShotgunData.Damage)) &&
                        (m.ReadFloat(gData.WEAPON_SPREAD) == Convert.ToInt32(HeavyShotgunData.Spread)) &&
                        (m.ReadFloat(gData.WEAPON_MVELOCITY) == Convert.ToInt32(HeavyShotgunData.Velocity)) &&
                        (m.ReadFloat(gData.WEAPON_RANGE) == Convert.ToInt32(HeavyShotgunData.Range)))
                    {
                        bHeavyShotgun = true;
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", "150");
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", "0");
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", "1");
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", "5000");
                        m.WriteMemory(gData.WEAPON_RANGE, "float", "1500");
                    }
                    #endregion
                }
                else if (!bPerfectWeapon)
                {
                    #region DISABLE
                    if (bPistol)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", PistolData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", PistolData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", PistolData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", PistolData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", PistolData.Range);
                        bPistol = false;
                    }
                    if (bCombatPistol)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", CombatPistolData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", CombatPistolData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", CombatPistolData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", CombatPistolData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", CombatPistolData.Range);
                        bCombatPistol = false;
                    }
                    if (bMicroSMG)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", MicroSMGData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", MicroSMGData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MicroSMGData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MicroSMGData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", MicroSMGData.Range);
                        bMicroSMG = false;
                    }
                    if (bSMG)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", SMGData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", SMGData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SMGData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SMGData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", SMGData.Range);
                        bSMG = false;
                    }
                    if (bAssaultRifle)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", AssaultRifleData.Damage); ;
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", AssaultRifleData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", AssaultRifleData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", AssaultRifleData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", AssaultRifleData.Range);
                        bAssaultRifle = false;
                    }
                    if (bSpecialCarbine)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", SpecialCarbineData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", SpecialCarbineData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SpecialCarbineData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SpecialCarbineData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", SpecialCarbineData.Range);
                        bSpecialCarbine = false;
                    }
                    if (bBullpupRifle)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", BullpupRifleData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", BullpupRifleData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", BullpupRifleData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", BullpupRifleData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", BullpupRifleData.Range);
                        bBullpupRifle = false;
                    }
                    if (bMilitaryRifle)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", MilitaryRifleData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", MilitaryRifleData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MilitaryRifleData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MilitaryRifleData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", MilitaryRifleData.Range);
                        bMilitaryRifle = false;
                    }
                    if (bMarksmanRifle)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", MarksmanRifleData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", MarksmanRifleData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MarksmanRifleData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MarksmanRifleData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", MarksmanRifleData.Range);
                        bMarksmanRifle = false;
                    }
                    if (bSniperRifle)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", SniperRifleData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", SniperRifleData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SniperRifleData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SniperRifleData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", SniperRifleData.Range);
                        bSniperRifle = false;
                    }
                    if (bHeavyShotgun)
                    {
                        m.WriteMemory(gData.WEAPON_DAMAGE, "float", HeavyShotgunData.Damage);
                        m.WriteMemory(gData.WEAPON_SPREAD, "float", HeavyShotgunData.Spread);
                        m.WriteMemory(gData.WEAPON_BPENETRATION, "float", HeavyShotgunData.Penetration);
                        m.WriteMemory(gData.WEAPON_MVELOCITY, "float", HeavyShotgunData.Velocity);
                        m.WriteMemory(gData.WEAPON_RANGE, "float", HeavyShotgunData.Range);
                        bHeavyShotgun = false;
                    }
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
            if (bPerfectWeapon && bAllOff)
            {
                if (bPistol)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", PistolData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", PistolData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", PistolData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", PistolData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", PistolData.Range);
                    bPistol = false;
                }
                if (bCombatPistol)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", CombatPistolData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", CombatPistolData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", CombatPistolData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", CombatPistolData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", CombatPistolData.Range);
                    bCombatPistol = false;
                }
                if (bMicroSMG)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", MicroSMGData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", MicroSMGData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MicroSMGData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MicroSMGData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", MicroSMGData.Range);
                    bMicroSMG = false;
                }
                if (bSMG)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", SMGData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", SMGData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SMGData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SMGData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", SMGData.Range);
                    bSMG = false;
                }
                if (bAssaultRifle)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", AssaultRifleData.Damage); ;
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", AssaultRifleData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", AssaultRifleData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", AssaultRifleData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", AssaultRifleData.Range);
                    bAssaultRifle = false;
                }
                if (bSpecialCarbine)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", SpecialCarbineData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", SpecialCarbineData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SpecialCarbineData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SpecialCarbineData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", SpecialCarbineData.Range);
                    bSpecialCarbine = false;
                }
                if (bBullpupRifle)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", BullpupRifleData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", BullpupRifleData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", BullpupRifleData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", BullpupRifleData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", BullpupRifleData.Range);
                    bBullpupRifle = false;
                }
                if (bMilitaryRifle)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", MilitaryRifleData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", MilitaryRifleData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MilitaryRifleData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MilitaryRifleData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", MilitaryRifleData.Range);
                    bMilitaryRifle = false;
                }
                if (bMarksmanRifle)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", MarksmanRifleData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", MarksmanRifleData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MarksmanRifleData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MarksmanRifleData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", MarksmanRifleData.Range);
                    bMarksmanRifle = false;
                }
                if (bSniperRifle)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", SniperRifleData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", SniperRifleData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SniperRifleData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SniperRifleData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", SniperRifleData.Range);
                    bSniperRifle = false;
                }
                if (bHeavyShotgun)
                {
                    m.WriteMemory(gData.WEAPON_DAMAGE, "float", HeavyShotgunData.Damage);
                    m.WriteMemory(gData.WEAPON_SPREAD, "float", HeavyShotgunData.Spread);
                    m.WriteMemory(gData.WEAPON_BPENETRATION, "float", HeavyShotgunData.Penetration);
                    m.WriteMemory(gData.WEAPON_MVELOCITY, "float", HeavyShotgunData.Velocity);
                    m.WriteMemory(gData.WEAPON_RANGE, "float", HeavyShotgunData.Range);
                    bHeavyShotgun = false;
                }
                bPerfectWeapon = false;
            }
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
            if (bPistol)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", PistolData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", PistolData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", PistolData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", PistolData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", PistolData.Range);
                bPistol = false;
            }
            if (bCombatPistol)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", CombatPistolData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", CombatPistolData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", CombatPistolData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", CombatPistolData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", CombatPistolData.Range);
                bCombatPistol = false;
            }
            if (bMicroSMG)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", MicroSMGData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", MicroSMGData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MicroSMGData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MicroSMGData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", MicroSMGData.Range);
                bMicroSMG = false;
            }
            if (bSMG)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", SMGData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", SMGData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SMGData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SMGData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", SMGData.Range);
                bSMG = false;
            }
            if (bAssaultRifle)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", AssaultRifleData.Damage); ;
                m.WriteMemory(gData.WEAPON_SPREAD, "float", AssaultRifleData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", AssaultRifleData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", AssaultRifleData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", AssaultRifleData.Range);
                bAssaultRifle = false;
            }
            if (bSpecialCarbine)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", SpecialCarbineData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", SpecialCarbineData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SpecialCarbineData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SpecialCarbineData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", SpecialCarbineData.Range);
                bSpecialCarbine = false;
            }
            if (bBullpupRifle)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", BullpupRifleData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", BullpupRifleData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", BullpupRifleData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", BullpupRifleData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", BullpupRifleData.Range);
                bBullpupRifle = false;
            }
            if (bMilitaryRifle)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", MilitaryRifleData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", MilitaryRifleData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MilitaryRifleData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MilitaryRifleData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", MilitaryRifleData.Range);
                bMilitaryRifle = false;
            }
            if (bMarksmanRifle)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", MarksmanRifleData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", MarksmanRifleData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", MarksmanRifleData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", MarksmanRifleData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", MarksmanRifleData.Range);
                bMarksmanRifle = false;
            }
            if (bSniperRifle)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", SniperRifleData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", SniperRifleData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", SniperRifleData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", SniperRifleData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", SniperRifleData.Range);
                bSniperRifle = false;
            }
            if (bHeavyShotgun)
            {
                m.WriteMemory(gData.WEAPON_DAMAGE, "float", HeavyShotgunData.Damage);
                m.WriteMemory(gData.WEAPON_SPREAD, "float", HeavyShotgunData.Spread);
                m.WriteMemory(gData.WEAPON_BPENETRATION, "float", HeavyShotgunData.Penetration);
                m.WriteMemory(gData.WEAPON_MVELOCITY, "float", HeavyShotgunData.Velocity);
                m.WriteMemory(gData.WEAPON_RANGE, "float", HeavyShotgunData.Range);
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
