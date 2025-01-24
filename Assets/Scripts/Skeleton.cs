using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _finishTime = 2.0f;
    
    private Transform _targetPosition;
    private Animator _animator;

    public event Action<Skeleton> Triggered;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        AchievedTarget();
    }

    private IEnumerator MakesPoseWinner()
    {
        WaitForSeconds wait = new(_finishTime);
        
        _animator.SetBool(PlayerAnimatorData.Params.isJump, true);
            
        yield return wait;
        
        Triggered?.Invoke(this);
    }

    private void AchievedTarget()
    {
        if (transform.position == _targetPosition.position)
        {
            StartCoroutine(MakesPoseWinner());
        }
    }
    
    private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);
    
            _animator.SetBool(PlayerAnimatorData.Params.isRun, true);
        }
    
    public void TargetPosition(Transform point)
    {
        _targetPosition = point;
                 
        gameObject.transform.LookAt(point);
    }
}
