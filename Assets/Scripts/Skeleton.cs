using System;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private WaitForSeconds _waitForSeconds;
    private Vector3 _direction;

    public event Action<Skeleton> Triggered;

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Cube cube))
        {
            Triggered?.Invoke(this);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public void RotateToDirection()
    {
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}
