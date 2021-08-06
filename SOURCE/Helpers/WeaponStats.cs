using System;
using System.Collections.Generic;
using System.Threading;
using Memory;
using Simple_GTAV_External_Trainer.Helpers;

namespace Simple_GTAV_External_Trainer
{
    public class Weapon
    {
        private readonly Mem _m;

        private class WeaponDefaultValue
        {
            public string damage;
            public string spread;
            public string penetration;
            public string velocity;
            public string range;
            public string recoil;

            public WeaponDefaultValue(string damage, string spread, string penetration, string velocity, string range, string recoil)
            {
                this.damage = damage;
                this.spread = spread;
                this.penetration = penetration;
                this.velocity = velocity;
                this.range = range;
                this.recoil = recoil;
            }
        }

        public bool bPerfectWeapon = false;

        private Dictionary<long, WeaponDefaultValue> weaponsSave = new Dictionary<long, WeaponDefaultValue>();

        public Weapon(Mem m)
        {
            this._m = m;
        }

        private static Func<Helpers.Weapon.Stats, bool> IsCurentWeaponBuildFunc(float weaponDamage, float weaponSpread, float weaponMvelocity, float weaponRange)
        {
            return (weaponStats) => weaponDamage == weaponStats.Damage &&
                                    weaponSpread == weaponStats.Spread &&
                                    weaponMvelocity == weaponStats.Velocity &&
                                    weaponRange == weaponStats.Range;
        }

        private bool DoCurentWeaponShootBullet(float weaponDamage, float weaponSpread, float weaponMvelocity, float weaponRange)
        {
            Func<Helpers.Weapon.Stats, bool> isCurentWeapon = IsCurentWeaponBuildFunc(weaponDamage, weaponSpread, weaponMvelocity, weaponRange);

            return !(isCurentWeapon(Helpers.Weapon.Stats.HandsData) &&
                     isCurentWeapon(Helpers.Weapon.Stats.GrenadeData) &&
                     isCurentWeapon(Helpers.Weapon.Stats.StickyBombData) &&
                     isCurentWeapon(Helpers.Weapon.Stats.RPGData) &&
                     isCurentWeapon(Helpers.Weapon.Stats.GrenadeLauncherData) &&
                     isCurentWeapon(Helpers.Weapon.Stats.HomingLauncherData));
        }

        public void ResetStats()
        {
            foreach (var weaponSave in weaponsSave) {
                _m.WriteMemory((weaponSave.Key + gData.Damage).ToString("X"), "float", weaponSave.Value.damage);
                _m.WriteMemory((weaponSave.Key + gData.Spread).ToString("X"), "float", weaponSave.Value.spread);
                _m.WriteMemory((weaponSave.Key + gData.Penetration).ToString("X"), "float", weaponSave.Value.penetration);
                _m.WriteMemory((weaponSave.Key + gData.Velocity).ToString("X"), "float", weaponSave.Value.velocity);
                _m.WriteMemory((weaponSave.Key + gData.Range).ToString("X"), "float", weaponSave.Value.range);
                _m.WriteMemory((weaponSave.Key + gData.Recoil).ToString("X"), "float", weaponSave.Value.recoil);
            }
            weaponsSave.Clear();
        }

        //public void WEAPONHACK()
        //{
        //    while (true)
        //    {
        //        if (bPerfectWeapon)
        //        {
        //            //Store unique gun addresses so that we can disable the patch later
        //            long baseAddr = _m.ReadLong("GTA5.exe+25333D8,0x8,0x10D8,0x20");

        //            if (!weaponsSave.ContainsKey(baseAddr))
        //            {
        //                float weaponDamage = _m.ReadFloat(gData.WEAPON_DAMAGE);
        //                float weaponSpread = _m.ReadFloat(gData.WEAPON_SPREAD);
        //                float weaponPenetration = _m.ReadFloat(gData.WEAPON_BPENETRATION);
        //                float weaponVelocity = _m.ReadFloat(gData.WEAPON_MVELOCITY);
        //                float weaponRange = _m.ReadFloat(gData.WEAPON_RANGE);
        //                float weaponRecoil = _m.ReadFloat(gData.WEAPON_RECOIL);

        //                #region ENABLE

        //                if (DoCurentWeaponShootBullet(weaponDamage, weaponSpread, weaponVelocity, weaponRange))
        //                {
        //                    #region EDIT_BULLET_GUN

        //                    //TODO: When other weapon type is implement move the SAVE_DEFAULT_STATS out of the if
        //                    #region SAVE_DEFAULT_STATS

        //                    weaponsSave.Add(baseAddr, new WeaponDefaultValue(weaponDamage.ToString(), weaponSpread.ToString(),
        //                        weaponPenetration.ToString(), weaponVelocity.ToString(), weaponRange.ToString(), weaponRecoil.ToString()));

        //                    #endregion

        //                    _m.WriteMemory(gData.WEAPON_DAMAGE, "float", gData.pDamage);
        //                    _m.WriteMemory(gData.WEAPON_SPREAD, "float", gData.pSpread);
        //                    _m.WriteMemory(gData.WEAPON_BPENETRATION, "float", gData.pPenetration);
        //                    _m.WriteMemory(gData.WEAPON_MVELOCITY, "float", gData.pVelocity);
        //                    _m.WriteMemory(gData.WEAPON_RANGE, "float", gData.pRange);
        //                    _m.WriteMemory(gData.WEAPON_RECOIL, "float", gData.pRecoil);

        //                    #endregion
        //                }

        //                #endregion
        //            }
        //        }
        //        else
        //        {
        //            #region DISABLE

        //            ResetStats();

        //            #endregion
        //        }
        //        Thread.Sleep(100);
        //    }
        //}
    }
}