using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sceleton : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Transform _targetPosition;
    
    private void Update()
    {
        gameObject.transform.LookAt(_targetPosition);
        
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, _targetPosition.position) < 0.1f)
        {
            Debug.Log("Цель достигнута!");
        }
    }

    public void GetTargetPosition(Transform point)
    {
        _targetPosition = point;
    }
}
