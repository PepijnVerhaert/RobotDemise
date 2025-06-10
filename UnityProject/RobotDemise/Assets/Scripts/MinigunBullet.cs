using UnityEngine;

public class MinigunBullet : BaseProjectile
{
    public Vector3 _direction;
    public float _speed;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _rigidbody.MovePosition(transform.position + (_direction * (_speed * Time.fixedDeltaTime)));
    }
}
