using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _targetUpdateInterval = 1f;
    [SerializeField] private float _baseSpeed = 3.5f;
    private float _updateTimer = 0f;
    private BaseEnemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<BaseEnemy>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_target.transform.position);
        _agent.speed = _baseSpeed;
    }

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        if(_updateTimer > _targetUpdateInterval)
        {
            _updateTimer = 0f;
            _agent.SetDestination(_target.transform.position);
        }
    }
}
