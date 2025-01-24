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
    
    private void Start()
    {
        StartCoroutine(SpawnerSkeletons());
    }
    
    private IEnumerator SpawnerSkeletons()
        {
            while (true)
            {
                SpawnSkeleton();
    
                yield return new WaitForSeconds(_repeateTime);
            }
        }
    
    private void GetAction(Skeleton skeleton)
    {
        skeleton.Triggered += SkeletonRelease;

        int indexPointSpawn = GetRandomPoint();
        
        skeleton.transform.position = _points[indexPointSpawn].transform.position;
        
        skeleton.gameObject.SetActive(true);
        
        SpecifyingNewPosition(skeleton, indexPointSpawn);
    }
    
    private void SkeletonRelease(Skeleton skeleton)
    {
        skeleton.Triggered -= SkeletonRelease;
        
        _pool.Release(skeleton);
    }

    private void SpawnSkeleton()
    {
        _pool.Get();
    }

    private void SpecifyingNewPosition(Skeleton skeleton, int indexSpawn)
    {
        int indexTargetPoint = GetRandomPoint();

        if (indexSpawn == indexTargetPoint)
        {
            if (indexTargetPoint == _points.Length - 1)
            {
                skeleton.GettingNewPosition(_points[indexTargetPoint - 1]);
            }
            else
            {
                skeleton.GettingNewPosition(_points[indexTargetPoint + 1]);
            }
        }
        else
        {
            skeleton.GettingNewPosition(_points[indexTargetPoint]);
        }
    }

    private int GetRandomPoint()
    {
        int firstPoint = 0;

        int randomIndexPoint = Random.Range(firstPoint, _points.Length);

        return randomIndexPoint;
    }
}