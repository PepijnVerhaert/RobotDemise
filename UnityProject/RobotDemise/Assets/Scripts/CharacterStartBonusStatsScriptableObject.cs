using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStartBonusStatsScriptableObject", menuName = "ScriptableObjects/CharacterStartBonusStatsScriptableObject", order = 2)]
public class CharacterStartBonusStatsScriptableObject : ScriptableObject
{
    public float characterSpeed = 1f;
    public float haste = 1f;
    public float damage = 1f;
    public float size = 1f;
    public float health = 0f;
    public float regeneration = 0f;
    public float defence = 0f;
    public float experience = 1f;
    public int projectileCount = 0;
    public float luck = 0f;
    public float duration = 1f;
    public float projectileSpeed = 1f;
    public int projectilePiercing = 0;
    public float pickupRadius = 1f;
    public int passiveSlots = 0;
    public int weaponSlots = 0;
}
