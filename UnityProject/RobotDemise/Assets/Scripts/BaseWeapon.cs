using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public string _name { get; protected set; }
    public BaseRobot _robot;
    public int _level { get; protected set; }
    public bool _isUpgraded { get; protected set; }
    public string _passiveForUpgrade { get; protected set; }
    public abstract void ActivateEffect(string effectName);
}
