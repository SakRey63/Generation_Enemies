using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _finishTime = 2.0f;
    
    private Transform _targetPosition;
    private Animator _animator;

    public event Action<Skeleton> TriggerEnter;
    
    public void GetTargetPosition(Transform point)
        {
            _targetPosition = point;
            
            gameObject.transform.LookAt(point);
        }

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        MoveSkeleton();
    }

    private void MoveSkeleton()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);

        _animator.SetBool("isRun", true);
    }

    private IEnumerator WinnersPose()
    {
        WaitForSeconds wait = new(_finishTime);
        
        _animator.SetBool("isJump", true);
            
        yield return wait;
        
        TriggerEnter?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position == _targetPosition.position)
        {
            StartCoroutine(WinnersPose());
        }
    }
}
