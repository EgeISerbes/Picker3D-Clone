using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private RouteFollow _routeFollow;
    [SerializeField] private Transform _routeTR;
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _spawnable;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnDropOffset;
    private bool _canSpawn = false;
    private List<GameObject> _spawnedObjList = new List<GameObject>();
    [Header("Developer Settings")]
    [SerializeField] private float _maxSpawnRate;
    private void Awake()
    {
        _routeFollow = GetComponent<RouteFollow>();
        _routeFollow.Init(OnRouteEnd);
        for (int i = 0; i < _maxSpawnRate; i++)
        {
            var obj = Instantiate(_spawnable);
            obj.SetActive(false);
            _spawnedObjList.Add(obj);
        }
    }

    void OnRouteEnd()
    {
        _canSpawn = false;
    }
    IEnumerator SpawnObjects()
    {
        _canSpawn = true;
        while (_canSpawn)
        {
            var listLength = _spawnedObjList.Count;
            var objToDrop = _spawnedObjList[listLength - 1];
            objToDrop.SetActive(true);
            objToDrop.transform.position = new Vector3(transform.position.x, transform.position.y - _spawnDropOffset, transform.position.z);
            objToDrop.transform.rotation = transform.rotation;
            _spawnedObjList.RemoveAt(listLength - 1);
            yield return new WaitForSeconds(_spawnDelay);
        }
        Destroy(_routeTR.gameObject);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerEventTrigger"))
        {
            _routeTR.parent = null;
            _routeFollow.StartCoroutine(_routeFollow.GoByTheRoute());
            StartCoroutine(SpawnObjects());
        }
    }
}
