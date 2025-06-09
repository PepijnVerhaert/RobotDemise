using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private CharacterStartBonusStatsScriptableObject _startBonusStats;
    [SerializeField] private BaseRobot _robot;
    [SerializeField] private CharacterController _controller;

    private void Start()
    {
        if(_robot != null) InitRobot();
    }

    public void SetRobot(BaseRobot robotClass)
    {
        InitRobot();
    }

    private void InitRobot()
    {
        _robot.SetStartBonusStats(_startBonusStats);
        _robot.SetCharacterController(_controller);
    }
}