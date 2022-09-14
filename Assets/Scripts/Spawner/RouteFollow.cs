using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFollow : MonoBehaviour
{
    [SerializeField] private Transform[] _controlPoints;
    
    private float _tParam;
    private Vector3 _objPos;
   [SerializeField] private float _speedModifier;
    private bool _coroutineAllowed;
    [SerializeField] private float _destroyWaitSeconds;
    private System.Action OnRouteEnd;
    public bool CoRoutineAllowed
    {
        get => _coroutineAllowed;
    }
    void Start()
    {
        _tParam = 0f;
        //_speedModifier = .5f;
        _coroutineAllowed = true;

    }

    public void Init(System.Action OnRouteEnd)
    {
        this.OnRouteEnd = OnRouteEnd;
    }


   public IEnumerator GoByTheRoute()
    {
        _coroutineAllowed = false;
        while(_tParam <1)
        {
            _tParam += Time.deltaTime * _speedModifier;
            _objPos = Mathf.Pow(1 - _tParam, 3) * _controlPoints[0].position +
                3 * Mathf.Pow(1 - _tParam, 2) * _tParam * _controlPoints[1].position +
                3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * _controlPoints[2].position +
                Mathf.Pow(_tParam, 3) * _controlPoints[3].position;
            transform.position = _objPos;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(_destroyWaitSeconds);
        OnRouteEnd();
    }
}
