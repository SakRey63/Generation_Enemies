using System;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private WaitForSeconds _waitForSeconds;
    private Transform _targetPosition;

    public event Action<Skeleton> Triggered;

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Cube cube))
        {
            Triggered?.Invoke(this);
        }
    }
}
