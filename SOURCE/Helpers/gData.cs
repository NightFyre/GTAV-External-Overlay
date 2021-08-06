namespace Simple_GTAV_External_Trainer.Helpers
{
    class gData
    {
        //Triggerbot Info
        public const string ENEMY_IN_CROSSHAIRS = "GTA5.exe+1FB2380";
        public const string IS_ZOOMED = "GTA5.exe+1FB23A4";
        public const string ENTITY = "GTA5.exe+1FB2375";

        //GodMode Info
        public const string GODMODE = "GTA5.exe+25333D8,0x8,0x189";

        //Never Wanted Info
        public const string WANTED_LEVEL = "GTA5.exe+25333D8,0x8,0x10C8,0x888";

        //Infinite Ammo
        public const string INFINITE_AMMO = "GTA5.exe+25333D8,0x8,0x10D0,0x78";

        //Perfect Weapon 
        public const string WEAPON_DAMAGE = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0xB0";
        public const string WEAPON_SPREAD = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0x7C";
        public const string WEAPON_BPENETRATION = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0x110";
        public const string WEAPON_MVELOCITY = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0x11C";
        public const string WEAPON_RANGE = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0x28C";
        public const string WEAPON_RECOIL = "GTA5.exe+25333D8,0x8,0x10D8,0x20,0x2F4";

        //Weapon Data Base Address
        public const string WeaponID = "GTA5.exe+25333D8,0x8,0x10D8,0x20";
        public const int ImpactType = 0x20;     //NEW
        public const int ImpactExplode = 0x24;  //NEW
        public const int Spread = 0x7C;         //NEW
        public const int Spread2 = 0x74;      //CORRECT ADDRESS , NEED TO COLLECT DATA WITH NEW ADDRESS
        public const int Damage = 0xB0;
        public const int Penetration = 0x110;
        public const int Velocity = 0x11C;
        public const int ReloadSpeed = 0x134;   //NEW
        public const int Range = 0x28C;
        public const int Recoil = 0x2f4;        //NEW

        //Modded Weapon Data
        public const string pDamage = "150";
        public const string pSpread = "0";
        public const string pPenetration = "1";
        public const string pVelocity = "5000";
        public const string pRange = "1500";
        public const string pRecoil = "0";      //NEW

        //Vehicle Data
        public const string VehicleID = "GTA5.exe+25333D8,0x8,0xD30";
        public const string VehicleState = "GTA5.exe+25333D8,0x8,0x1477";
        public const string VehicleDirt = "GTA5.exe+25333D8,0x8,0xD30,0x9F8";
        public const string VehicleHealth = "GTA5.exe+25333D8,0x8,0xD30,280";
        public const string EngineHealth = "GTA5.exe+25333D8,0x8,0xD30,908";
        public const string VehicleGravity = "GTA5.exe+25333D8,0x8,0xD30,C5C";

        //Vehicle Handling Data
        public const string VehicleAcceleration = "GTA5.exe+25333D8,0x8,0xD30,0x938,0x4C";
        public const string VehBrakeForce = "GTA5.exe+25333D8,0x8,0xD30,0x938,0x6C";
        public const string VehHandbrakeforce = "GTA5.exe+25333D8,0x8,0xD30,0x938,0x7C";
        public const string VehDamageMultiplier = "GTA5.exe+25333D8,0x8,0xD30,0x938,0xF0";
        public const string VehCollisionMultiplier = "GTA5.exe+25333D8,0x8,0xD30,0x938,0xF8";
    }
    
     /// <summary>
    /// WEAPON DATA
    /// </summary>
    #region WEAPON

    namespace Weapon
    {
        public class Stats
        {
            public readonly float Damage;
            public readonly float Spread;
            public readonly float Penetration;
            public readonly float Velocity;

            public readonly float Range;
            // public readonly float Recoil;

            private Stats(float damage, float spread, float penetration, float velocity, float range)
            {
                Damage = damage;
                Spread = spread;
                Penetration = penetration;
                Velocity = velocity;
                Range = range;
                // Recoil = recoil;
            }

            #region HANDS

            public static readonly Stats HandsData = new Stats(0, 2, 0, 2000, 30);

            #endregion

            #region PISTOLS

            public static readonly Stats PistolData = new Stats(26, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats CombatPistolData = new Stats(27, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats HeavyRevolverData = new Stats(26, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats ApPistolData = new Stats(25, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats FlaraGunData = new Stats(10, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats AtomizerData = new Stats(10, 2, 0.009999999776f, 2000, 120);

            #endregion

            #region SMG's

            public static readonly Stats MicroSMGData = new Stats(21, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats SMGData = new Stats(22, 2, 0.009999999776f, 2000, 120);

            #endregion

            #region Assault Rifles

            public static readonly Stats AssaultRifleData = new Stats(30, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats SpecialCarbineData = new Stats(32, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats BullpupRifleData = new Stats(32, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats MilitaryRifleData = new Stats(37.5f, 2, 0.1000000015f, 2000, 120);

            #endregion

            #region SNIPER RIFLES

            public static readonly Stats SniperRifleData = new Stats(101, 2, 1, 5000, 1500);
            public static readonly Stats MarksmanRifleData = new Stats(65, 2, 1, 5000, 1000);
            public static readonly Stats HeavySniperData = new Stats(230, 2, 1, 5000, 1500);

            #endregion

            #region SHOTGUNS

            public static readonly Stats PumpShotgunData = new Stats(29, 2, 0.009999999776f, 2000, 40);
            public static readonly Stats HeavyShotgunData = new Stats(117, 2, 0.009999999776f, 2000, 50);

            #endregion

            #region Explosives

            public static readonly Stats GrenadeData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats StickyBombData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats RPGData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats GrenadeLauncherData = new Stats(0, 2, 0, 2000, 150);
            public static readonly Stats HomingLauncherData = new Stats(0, 2, 0, 2000, 300);

            #endregion

            public static readonly Stats KarbineData = new Stats(33, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats MiniGunData = new Stats(30, 2, 0.009999999776f, 2000, 120);

        }
    }

    #endregion

    class KarbineData
    {
        public const string Damage = "33";
        public const string Spread = "2";
        public const string Penetration = "0.009999999776";
        public const string Velocity = "2000";
        public const string Range = "120";
    }


    class MiniGunData
    {
        public const string Damage = "30";
        public const string Spread = "2";
        public const string Penetration = "0.009999999776";
        public const string Velocity = "2000";
        public const string Range = "120";
    }
    
    
    /// <summary>
    /// WEAPON DATA EDIT "RasqueP"
    /// </summary>
    /// Not yet implemented

    #region WEAPON

    namespace Weapon
    {
        public class Stats
        {
            public readonly float ImpactType;
            public readonly float ImpactExplosion;
            public readonly float Damage;
            public readonly float Spread;
            public readonly float Penetration;
            public readonly float ReloadSpeed;
            public readonly float Velocity;
            public readonly float Recoil;

            public readonly float Range;
            // public readonly float Recoil;

            private Stats(float damage, float spread, float penetration, float velocity, float range)
            {
                // ImpactType = impact;
                // ImpactExplosion = explosion;
                Damage = damage;
                Spread = spread;
                Penetration = penetration;
                // ReloadSpeed = reloadspeed;
                Velocity = velocity;
                Range = range;
                // Recoil = recoil;
            }

            #region HANDS

            public static readonly Stats HandsData = new Stats(0, 2, 0, 2000, 30);

            #endregion

            #region PISTOLS

            public static readonly Stats PistolData = new Stats(26, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats CombatPistolData = new Stats(27, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats HeavyRevolverData = new Stats(26, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats ApPistolData = new Stats(25, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats FlaraGunData = new Stats(10, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats AtomizerData = new Stats(10, 2, 0.009999999776f, 2000, 120);

            #endregion

            #region SMG's

            public static readonly Stats MicroSMGData = new Stats(21, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats SMGData = new Stats(22, 2, 0.009999999776f, 2000, 120);

            #endregion

            #region Assault Rifles

            public static readonly Stats AssaultRifleData = new Stats(30, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats SpecialCarbineData = new Stats(32, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats BullpupRifleData = new Stats(32, 2, 0.1000000015f, 2000, 120);
            public static readonly Stats MilitaryRifleData = new Stats(37.5f, 2, 0.1000000015f, 2000, 120);

            #endregion

            #region SNIPER RIFLES

            public static readonly Stats SniperRifleData = new Stats(101, 2, 1, 5000, 1500);
            public static readonly Stats MarksmanRifleData = new Stats(65, 2, 1, 5000, 1000);
            public static readonly Stats HeavySniperData = new Stats(230, 2, 1, 5000, 1500);

            #endregion

            #region SHOTGUNS

            public static readonly Stats PumpShotgunData = new Stats(29, 2, 0.009999999776f, 2000, 40);
            public static readonly Stats HeavyShotgunData = new Stats(117, 2, 0.009999999776f, 2000, 50);

            #endregion

            #region Explosives

            public static readonly Stats GrenadeData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats StickyBombData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats RPGData = new Stats(117, 2, 0.009999999776f, 2000, 50);
            public static readonly Stats GrenadeLauncherData = new Stats(0, 2, 0, 2000, 150);
            public static readonly Stats HomingLauncherData = new Stats(0, 2, 0, 2000, 300);

            #endregion

            public static readonly Stats KarbineData = new Stats(33, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats MiniGunData = new Stats(30, 2, 0.009999999776f, 2000, 120);

        }
    }

    #endregion
}
