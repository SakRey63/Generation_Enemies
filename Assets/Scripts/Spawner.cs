using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private GameObject _skeleton;
    [SerializeField] private float _repeateTime = 2.0f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSkeleton),0,  _repeateTime);
    }

    private void SpawnSkeleton()
    {
        _skeleton.GetComponent<Sceleton>().GetTargetPosition(_points[GetRandomPoint()]);
        
        Instantiate(_skeleton, _points[GetRandomPoint()]);
    }

    private int GetRandomPoint()
    {
        int firstPoint = 0;

        int randomIndexPoint = Random.Range(firstPoint, _points.Length);

        return randomIndexPoint;
    }
}