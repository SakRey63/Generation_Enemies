using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private Skeleton _skeleton;
    [SerializeField] private float _repeateTime = 2.0f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Skeleton> _pool;
    private WaitForSeconds _waitForSeconds;
    
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
        _waitForSeconds = new WaitForSeconds(_repeateTime);
        
        while (true)
        {
            SpawnSkeleton();
    
            yield return _waitForSeconds;
        }
    }
    
    private void GetAction(Skeleton skeleton)
    {
        skeleton.Triggered += SkeletonRelease;

        int indexPointSpawn = GetRandomIndexPoint();
        
        skeleton.transform.position = _points[indexPointSpawn].transform.position;
        skeleton.gameObject.SetActive(true);
        skeleton.SetDirection(GenerateDirection());
        skeleton.RotateToDirection();
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
    
    private Vector3 GenerateDirection()
    {
        float angleDirection = Random.Range(0f, Mathf.PI * 2);

        float vectorX = Mathf.Cos(angleDirection);
        float vectorZ = Mathf.Sin(angleDirection);

        return new Vector3(vectorX, 0, vectorZ);
    }

    private int GetRandomIndexPoint()
    {
        int firstPoint = 0;

        return Random.Range(firstPoint, _points.Length);
    }
}