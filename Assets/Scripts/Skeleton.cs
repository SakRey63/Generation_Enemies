using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _finishTime = 2.0f;
    [SerializeField] private float _distanceToTarget = 0.1f;
    
    private Transform _targetPosition;
    private Animator _animator;
    private bool _isJump;
    private bool _isRun;

    public event Action<Skeleton> Triggered;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private IEnumerator MakesPoseWinner()
    {
        _isJump = true;
        _isRun = false;
        
        Setup(_isRun, _isJump);
            
        yield return new WaitForSeconds(_finishTime);
        
        Triggered?.Invoke(this);
    }
    
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);

        _isJump = false;
        _isRun = true;
        
        Setup(_isRun, _isJump);
        
        if (IsTargetReached())
        {
            StartCoroutine(MakesPoseWinner());
        }
    }
    
    private bool IsTargetReached()
    {
        return transform.position.IsEnoughClose(_targetPosition.position, _distanceToTarget);
    }
    
    private void Setup(bool isRun, bool isJump)
    {
        _animator.SetBool(PlayerAnimatorData.Params.isRun, isRun);
        _animator.SetBool(PlayerAnimatorData.Params.isJump, isJump);
    }
    
    public void GettingNewPosition(Transform point)
    {
        _targetPosition = point;
                 
        gameObject.transform.LookAt(point);
    }
}
