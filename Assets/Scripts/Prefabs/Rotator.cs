using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private bool _canRotate = false;
    [SerializeField] private List<Transform> _objects = new List<Transform>();
    [SerializeField] private Vector3 _targetRotationVal;
    private  Vector3[] _tempTargetRotationVal;
    private Vector3 _initialRotationVal;
    [SerializeField] private float _approachRate;
    // Update is called once per frame
    private int index = 0;

    private void Awake()
    {
        index = 0;
        _tempTargetRotationVal = new Vector3[_objects.Count];
        foreach(Transform obj in _objects)
        {
            _initialRotationVal = obj.transform.eulerAngles;
            _tempTargetRotationVal[index] = _targetRotationVal + _initialRotationVal;
            index++;
        }
        index = 0;
    }
    void Update()
    {
        if (_canRotate)
        {
            index = 0;
            foreach (Transform gate in _objects)
            {
                gate.eulerAngles = Vector3.Lerp(gate.eulerAngles, _tempTargetRotationVal[index], _approachRate);
                index++;
            }
           
        }
    }

    public void RotateObjects()
    {
        _canRotate = true;
    }
    public void StopRotating()
    {
        _canRotate = false;
    }
    public void SetInfinite()
    {
        index = 0;
        foreach(Transform obj in _objects)
        {
            _tempTargetRotationVal[index] = new Vector3(obj.transform.eulerAngles.x, Mathf.Infinity, obj.transform.eulerAngles.z);
            index++;
        }
        index = 0;

    }
}
