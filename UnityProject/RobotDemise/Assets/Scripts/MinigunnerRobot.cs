using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class MinigunnerRobotClass : BaseRobot
{
    [SerializeField] private CharacterBaseStatsScriptableObject _baseStats;
    [SerializeField] private BaseWeapon _startWeapon;
    private int _maxLevel = 5;

    private CharacterController _characterController;

    private InputAction _moveAction;
    private InputAction _basicAbilityAction;
    private InputAction _ultimateAbilityAction;

    private float _basicAbilityCooldown = 5f;
    private float _basicAbilityTimer = 0f;
    private bool _isUsingBasicAbility = false;
    private float _cancelBasicAbilityCooldown = .5f;
    private float _basicAbilityDuration = 5f;

    private float _ultimateAbilityCooldown = 5f;
    private float _ultimateAbilityTimer = 0f;
    private bool _isUsingUltimateAbility = false;
    private float _cancelUltimateAbilityCooldown = .5f;
    private float _ultimateAbilityDuration = 15f;

    private bool _limitedAbilityDuration = true;

    #region Inherited
    public override void AddPassive(string name)
    {
        if(_passives.ContainsKey(name))
        {
            if (_passives[name] >= _maxLevel)
            {
                Debug.Log("[MinigunnerRobot.cs][AddPassive] passive already at max level");
                return;
            }
            _passives[name] += 1;
            //actually add stats
        }
        else
        {
            _passives.Add(name, 1);
        }
    }

    public override void AddWeapon(string name)
    {
        if (_weapons.ContainsKey(name))
        {
            if (_weapons[name]._level >= _maxLevel)
            {
                Debug.Log("[MinigunnerRobot.cs][AddWeapon] weapon already at max level");
                return;
            }
            _weapons[name].ActivateEffect("LevelUp");
        }
        else
        {
            _weapons.Add(name, null);
            //actually add weapon
        }
    }

    public override List<string> GetAvailablePassives()
    {
        var availablePassives = new List<string>();
        foreach (var pair in _passives)
        {
            if(pair.Value < _maxLevel) availablePassives.Add(pair.Key);
        }
        return availablePassives;
    }

    public override List<string> GetAvailableWeapons()
    {
        var availableWeapons = new List<string>();
        foreach (var pair in _weapons)
        {
            if (pair.Value._level < _maxLevel) availableWeapons.Add(pair.Key);
        }
        return availableWeapons;
    }

    public override int GetFreePassiveSlotCount()
    {
        return _stats.passiveSlots - _passives.Count;
    }

    public override int GetFreeWeaponSlotCount()
    {
        return _stats.weaponSlots - _passives.Count;
    }

    public override List<string> GetMaxedPassives()
    {
        var maxedPassives = new List<string>();
        foreach (var pair in _passives)
        {
            if (pair.Value == _maxLevel) maxedPassives.Add(pair.Key);
        }
        return maxedPassives;
    }

    public override List<string> GetMaxedWeapons()
    {
        var maxedWeapons = new List<string>();
        foreach (var pair in _weapons)
        {
            if (pair.Value._level == _maxLevel) maxedWeapons.Add(pair.Key);
        }
        return maxedWeapons;
    }

    public override void SetStartBonusStats(CharacterStartBonusStatsScriptableObject startStats)
    {
        _stats.characterSpeed *= startStats.characterSpeed;
        _stats.haste += startStats.haste;
        _stats.damage *= startStats.damage;
        _stats.size *= startStats.size;
        _stats.health += startStats.health;
        _stats.regeneration += startStats.regeneration;
        _stats.defence += startStats.defence;
        _stats.experience *= startStats.experience;
        _stats.projectileCount += startStats.projectileCount;
        _stats.luck += startStats.luck;
        _stats.duration *= startStats.duration;
        _stats.projectileSpeed *= startStats.projectileSpeed;
        _stats.projectilePiercing += startStats.projectilePiercing;
        _stats.pickupRadius *= startStats.pickupRadius;
        _stats.passiveSlots += startStats.passiveSlots;
        _stats.weaponSlots += startStats.weaponSlots;
    }

    protected override void SetStats()
    {
        _stats.characterSpeed = _baseStats.characterSpeed;
        _stats.haste = _baseStats.haste;
        _stats.damage = _baseStats.damage;
        _stats.size = _baseStats.size;
        _stats.health = _baseStats.health;
        _stats.regeneration = _baseStats.regeneration;
        _stats.defence = _baseStats.defence;
        _stats.experience = _baseStats.experience;
        _stats.projectileCount = _baseStats.projectileCount;
        _stats.luck = _baseStats.luck;
        _stats.duration = _baseStats.duration;
        _stats.projectileSpeed = _baseStats.projectileSpeed;
        _stats.projectilePiercing = _baseStats.projectilePiercing;
        _stats.pickupRadius = _baseStats.pickupRadius;
        _stats.passiveSlots = _baseStats.passiveSlots;
        _stats.weaponSlots = _baseStats.weaponSlots;
    }

    protected override void SetInput()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _basicAbilityAction = InputSystem.actions.FindAction("BasicAbility");
        _ultimateAbilityAction = InputSystem.actions.FindAction("UltimateAbility");

        _basicAbilityAction.performed += _basicAbilityAction_performed;
        _ultimateAbilityAction.performed += _ultimateAbilityAction_performed;
    }

    public override void SetCharacterController(CharacterController controller)
    {
        _characterController = controller;
    }

    #endregion

    void Start()
    {
        SetInput();
        SetStats();

        //init start weapon
        _stats.weaponSlots += 1;
        _weapons.Add(_startWeapon._name, _startWeapon);
    }

    void Update()
    {
        _basicAbilityTimer += Time.deltaTime;
        if (_limitedAbilityDuration && _isUsingBasicAbility && _basicAbilityTimer >= (_basicAbilityDuration * _stats.duration))
        {
            ActivateBasicAbility(false);
        }
        _ultimateAbilityTimer += Time.deltaTime;
        if (_limitedAbilityDuration && _isUsingUltimateAbility && _ultimateAbilityTimer >= (_ultimateAbilityDuration * _stats.duration))
        {
            ActivateUltimateAbility(false);
        }

        if (!_isUsingUltimateAbility)
        {
            var moveActionValue = _moveAction.ReadValue<Vector2>();
            Vector3 simpleMove = new Vector3(moveActionValue.x * _stats.characterSpeed, 0f, moveActionValue.y * _stats.characterSpeed);
            if (_isUsingBasicAbility) simpleMove *= 2f;

            _characterController.SimpleMove(simpleMove);
        }
    }

    private void _basicAbilityAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("pressed basic");
        UsedBasicAbility();
    }
    private void _ultimateAbilityAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("pressed ult");
        UsedUltimateAbility();
    }

    private void UsedBasicAbility()
    {
        if(!_isUsingBasicAbility)
        {
            if (_basicAbilityTimer < (_basicAbilityCooldown * _stats.haste)) return;
            ActivateBasicAbility(true);
        }
        else
        {
            if (_basicAbilityTimer < _cancelBasicAbilityCooldown) return;
            ActivateBasicAbility(false);
        }
    }

    private void UsedUltimateAbility()
    {
        if (!_isUsingUltimateAbility)
        {
            if (_ultimateAbilityTimer < (_ultimateAbilityCooldown * _stats.haste)) return;
            ActivateUltimateAbility(true);
        }
        else
        {
            if (_ultimateAbilityTimer < _cancelUltimateAbilityCooldown) return;
            ActivateUltimateAbility(false);
        }
    }

    private void ActivateBasicAbility(bool activate)
    {
        if (activate == _isUsingBasicAbility) return;

        _isUsingBasicAbility = activate;
        _basicAbilityTimer = 0f;

        if (activate) _stats.haste *= 2f;
        else _stats.haste *= .5f;
    }

    private void ActivateUltimateAbility(bool activate)
    {
        if (activate == _isUsingUltimateAbility) return;

        if(activate) ActivateBasicAbility(false);

        _isUsingUltimateAbility = activate;
        _ultimateAbilityTimer = 0f;

        if (activate) _stats.haste *= .5f;
        else _stats.haste *= 2f;
    }
}
