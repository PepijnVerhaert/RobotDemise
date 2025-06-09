using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRobot : MonoBehaviour
{
    protected Dictionary<string, int> _passives;
    protected Dictionary<string, BaseWeapon> _weapons;
    public struct Stats
    {
        public float characterSpeed;
        public float haste;
        public float damage;
        public float size;
        public float health;
        public float regeneration;
        public float defence;
        public float experience;
        public int projectileCount;
        public float luck;
        public float duration;
        public float projectileSpeed;
        public int projectilePiercing;
        public float pickupRadius;
        public int passiveSlots;
        public int weaponSlots;
    }
    protected Stats _stats;

    public Stats GetStats() { return _stats; }

    public abstract void SetCharacterController(CharacterController controller);
    public abstract void AddPassive(string name);
    public abstract void AddWeapon(string name);
    public abstract int GetFreePassiveSlotCount();
    public abstract int GetFreeWeaponSlotCount();
    public abstract List<string> GetMaxedPassives();
    public abstract List<string> GetMaxedWeapons();
    public abstract List<string> GetAvailablePassives();
    public abstract List<string> GetAvailableWeapons();
    public abstract void SetStartBonusStats(CharacterStartBonusStatsScriptableObject startStats);
    protected abstract void SetStats();
    protected abstract void SetInput();
}
