using UnityEngine;

[CreateAssetMenu(fileName = "CharacterBaseStats", menuName = "ScriptableObjects/CharacterBaseStatsScriptableObject", order = 1)]
public class CharacterBaseStatsScriptableObject : ScriptableObject
{
    public float characterSpeed = 1f;
    public float haste = 1f;
    public float damage = 1f;
    public float size = 1f;
    public float health = 100f;
    public float regeneration = 1f;
    public float defence = 0f;
    public float experience = 1f;
    public int projectileCount = 1;
    public float luck = 10f;
    public float duration = 1f;
    public float projectileSpeed = 1f;
    public int projectilePiercing = 0;
    public float pickupRadius = 1f;
    public int passiveSlots = 4;
    public int weaponSlots = 3;
}
