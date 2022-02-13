namespace Simple_GTAV_External_Trainer.Helpers
{
    class gData
    {
        //POINTER AoB's
        //WorldPTR = 48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07
        //BlipPTR = 4C 8D 05 ? ? ? ? 0F B7 C1
        //ReplayInterfacePTR = 48 8D 0D ? ? ? ? 48 8B D7 E8? ? ? ? 48 8D 0D ? ? ? ? 8A D8 E8
        //LocalScriptsPTR = 48 8B 05 ? ? ? ? 8B CF 48 8B 0C C8 39 59 68)
        //GlobalPTR = 4C 8D 05 ? ? ? ? 4D 8B 08 4D 85 C9 74 11
        //PlayerCountPTR = 48 8B 0D ? ? ? ? E8? ? ? ? 48 8B C8 E8? ? ? ? 48 8B CF
        //PickupDataPTR = 48 8B 05 ? ? ? ? 48 8B 1C F8 8B
        //SettingsPTR = 44 39 05 ? ? ? ? 75 0D

        //Social Club Edition
        //- WORLDPTR = GTA5.exe+252DCD8
        //

        //Epic Games Edition
        // WORLDPTR = GTA5.exe+252DCD8

        //STEAM Edition
        // WORLDPTR = GTA5.exe+25333D8
        
        /// Patch 1.57 Offsets . Not current
        //WEAPON HASH
        public const string CWEAPON_HASH = "GTA5.exe+1D04F0C";

        //Social Club
        public const string EPIC_ENEMY_IN_CROSSHAIRS = "GTA5.exe+1FACDF0";
        public const string EPIC_ENTITY = "GTA5.exe+1FACDE5";
        public const string EPIC_IS_ZOOMED = "GTA5.exe+1FACE14";
        
        /// Patch 1.58 Updated

        //Triggerbot Info
        public const string ENEMY_IN_CROSSHAIRS = "GTA5.exe+1FE9560";
        public const string IS_ZOOMED = "GTA5.exe+1FE9584";
        public const string ENTITY = "GTA5.exe+1FE9555";
        
        //GodMode Info
        public const string GODMODE = "GTA5.exe+256A878,0x8,0x189";

        //Never Wanted Info
        public const string WANTED_LEVEL = "GTA5.exe+256A878,0x8,0x10C8,0x888";

        //Infinite Ammo
        public const string INFINITE_AMMO = "GTA5.exe+256A878,0x8,0x10D0,0x78";

        //Perfect Weapon 
        public const string WEAPON_DAMAGE = "GTA5.exe+256A878,0x8,0x10D8,0x20,0xB0";
        public const string WEAPON_SPREAD = "GTA5.exe+256A878,0x8,0x10D8,0x20,0x74";
        public const string WEAPON_BPENETRATION = "GTA5.exe+256A878,0x8,0x10D8,0x20,0x110";
        public const string WEAPON_MVELOCITY = "GTA5.exe+256A878,0x8,0x10D8,0x20,0x11C";
        public const string WEAPON_RANGE = "GTA5.exe+256A878,0x8,0x10D8,0x20,0x28C";
        public const string WEAPON_RECOIL = "GTA5.exe+256A878,0x8,0x10D8,0x20,0x2F4";

        //Weapon Data Base Address
        public const string WeaponID = "GTA5.exe+256A878,0x8,0x10D8,0x20";
        public const int ImpactType = 0x20;
        public const int ImpactExplode = 0x24;
        public const int Spread = 0x74;
        public const int Damage = 0xB0;
        public const int Penetration = 0x110;
        public const int Velocity = 0x11C;
        public const int ReloadSpeed = 0x134;
        public const int Range = 0x28C;
        public const int Recoil = 0x2f4;

        //Modded Weapon Data
        public const string pDamage = "150";
        public const string pSpread = "0";
        public const string pPenetration = "1";
        public const string pVelocity = "5000";
        public const string pRange = "1500";
        public const string pRecoil = "0";

        //Vehicle Data
        public const string VehicleID = "GTA5.exe+256A878,0x8,0xD30";
        public const string VehicleState = "GTA5.exe+256A878,0x8,0x1477";
        public const string VehicleDirt = "GTA5.exe+256A878,0x8,0xD30,0x9F8";
        public const string VehicleHealth = "GTA5.exe+256A878,0x8,0xD30,280";
        public const string EngineHealth = "GTA5.exe+256A878,0x8,0xD30,908";
        public const string VehicleGravity = "GTA5.exe+256A878,0x8,0xD30,C5C";

        //Vehicle Handling Data
        public const string VehicleAcceleration = "GTA5.exe+256A878,0x8,0xD30,0x938,0x4C";
        public const string VehBrakeForce = "GTA5.exe+256A878,0x8,0xD30,0x938,0x6C";
        public const string VehHandbrakeforce = "GTA5.exe+256A878,0x8,0xD30,0x938,0x7C";
        public const string VehDamageMultiplier = "GTA5.exe+256A878,0x8,0xD30,0x938,0xF0";
        public const string VehCollisionMultiplier = "GTA5.exe+256A878,0x8,0xD30,0x938,0xF8";
    }

    /// <summary>
    /// WEAPON DATA
    /// </summary>
    #region WEAPON

    namespace Weapon
    {

        enum WeaponHash 
        {
            HANDS = -1569615261,
            CELL_PHONE = 966099553,

            #region PISTOLS
            PISTOL = 453432689, 
            PISTOL_MK2 = -1075685676,
            COMBAT_PISTOL = 1593441988,
            AP_PISTOL = 584646201,
            STUN_GUN = 911657153,
            PISTOL_50 = -1716589765,
            SNS_PISTOL = -1076751822,
            SNS_PISTOLMK2 = -2009644972,
            HEAVY_PISTOL = -771403250,
            VINTAGE_PISTOL = 137902532,
            FLARE_GUN = 1198879012,
            MARKSMAN_PISTOL = -598887786,
            REVOLVER = -1045183535,
            REVOLVERMK2 = -879347409,
            DOUBLE_ACTION = -1746263880,
            ATOMIZER = -1355376991,
            CERAMIC_PISTOL = 727643628,
            NAVY_REVOLVER = -1853920116,
            #endregion

            #region SMG
            MICRO_SMG = 324215364,
            SMG = 736523883,
            SMGMK2 = 2024373456,
            ASSAULT_SMG = -270015777,
            COMBAT_PDW = 171789620,
            MACHINE_PISTOL = -619010992,
            MINI_SMG = -1121678507,
            HELLBRINGER = 1198256469,

            #endregion

            #region MACHINE GUNS
            MG = -1660422300,
            COMBATMG = 2144741730,
            COMBATMGMK2 = -608341376,
            #endregion

            #region SHOTGUNS
            PUMP_SHOTGUN = 487013001,
            PUMP_SHOTGUNMK2 = 1432025498,
            SAWN_OFF_SHOTGUN = 2017895192,
            ASSAULT_SHOTGUN = -494615257,
            BULLPUP_SHOTGUN = -1654528753,
            MUSKET = -1466123874,
            HEAVY_SHOTGUN = 984333226,
            DOUBLE_BARREL = -275439685,
            SWEEPER_SHOTGUN = 317205821,
            #endregion

            #region ASSAULT RIFLES
            ASSAULT_RIFLE = -1074790547,
            ASSAULT_RIFLEMK2 = 961495388,
            CARBINE_RIFLE = -2084633992,
            CARBINE_RIFLEMK2 = -86904375,
            ADVANCED_RIFLE = -1357824103,
            SPECIAL_CARBINE = -1063057011,
            SPECIAL_CARBINEMK2 = -1768145561,
            BULLPUP_RIFLE = 2132975508,
            BULLPUP_RIFLEMK2 = -2066285827,
            MILITARY_RIFLE = -1074790547,
            #endregion

            #region SNIPER RIFLES
            SNIPER_RIFLE = 100416529,
            HEAVY_SNIPER = 205991906,
            HEAVY_SNIPERMK2 = 177293209,
            MARKSMAN_RIFLE = -952879014,
            MARKSMAN_RIFLEMK2 = 1785463520,
            #endregion

            #region EXPLOSIVES
            RPG = -1312131151,
            GRENADE_LAUNCHER = -1568386805,
            #endregion

            MINIGUN = 1119849093,
            RAILGUN = 1834241177,
            WIDOWMAKER = -1238556825
        }

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
            public static readonly Stats RPGData = new Stats(0, 2, 0f, 2000, 85);
            public static readonly Stats GrenadeLauncherData = new Stats(0, 2, 0, 2000, 150);
            public static readonly Stats HomingLauncherData = new Stats(0, 2, 0, 2000, 300);

            #endregion

            public static readonly Stats KarbineData = new Stats(33, 2, 0.009999999776f, 2000, 120);
            public static readonly Stats MiniGunData = new Stats(30, 2, 0.009999999776f, 2000, 120);

        }
    }

    #endregion
}
