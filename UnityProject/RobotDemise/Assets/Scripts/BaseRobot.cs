using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BaseRobot : MonoBehaviour
{
    public Vector3 _mouseTarget { get; protected set; } = new Vector3();
    public LayerMask _floorLayer;

    protected Dictionary<string, int> _passives;
    protected Dictionary<string, BaseWeapon> _weapons;
    protected float _currentHealth;

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
    public abstract bool GetHit(float damage); //return was lethal
    protected abstract void SetStats();
    protected abstract void SetInput();

    protected void UpdateMouseTarget()
    {
        RaycastHit hit = new RaycastHit();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 1000f, _floorLayer);
        _mouseTarget = hit.point;
    }
}
