using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private Skeleton _skeleton;
    [SerializeField] private float _repeateTime = 2.0f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Skeleton> _pool;
    
    private void Awake()
    {
        _pool = new ObjectPool<Skeleton>(
            createFunc: () => Instantiate(_skeleton),
            actionOnGet: (skeleton) => GetAction(skeleton),
            actionOnRelease: (skeleton) => skeleton.gameObject.SetActive(false),
            actionOnDestroy: (skeleton) => Destroy(skeleton),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void GetAction(Skeleton skeleton)
    {
        skeleton.TriggerEnter += SkeletonRelease;

        int indexPointSpawn = GetRandomPoint();
        
        skeleton.transform.position = _points[indexPointSpawn].transform.position;
        
        skeleton.gameObject.SetActive(true);
        
        GetTargetPositionSkeleton(skeleton, indexPointSpawn);
    }
    
    private void SkeletonRelease(Skeleton skeleton)
    {
        skeleton.TriggerEnter -= SkeletonRelease;
        
        _pool.Release(skeleton);
    }
    
    private void Start()
    {
        StartCoroutine(SpawnerSkeletons());
    }

    private IEnumerator SpawnerSkeletons()
    {
        WaitForSeconds wait = new(_repeateTime);

        while (true)
        {
            SpawnSkeleton();

            yield return wait;
        }
    }

    private void SpawnSkeleton()
    {
        _pool.Get();
    }

    private void GetTargetPositionSkeleton(Skeleton skeleton, int indexSpawn)
    {
        int indexTargetPoint = GetRandomPoint();

        if (indexSpawn == indexTargetPoint)
        {
            if (indexTargetPoint == _points.Length - 1)
            {
                skeleton.GetTargetPosition(_points[indexTargetPoint - 1]);
            }
            else
            {
                skeleton.GetTargetPosition(_points[indexTargetPoint + 1]);
            }
        }
        else
        {
            skeleton.GetTargetPosition(_points[indexTargetPoint]);
        }
    }

    private int GetRandomPoint()
    {
        int firstPoint = 0;

        int randomIndexPoint = Random.Range(firstPoint, _points.Length);

        return randomIndexPoint;
    }
}