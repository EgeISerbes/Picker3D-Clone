using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfRotator : MonoBehaviour
{
    [SerializeField] private List<Transform> _objects = new List<Transform>();
    private bool _canRotate = false;
    [SerializeField] private float _approachRate;
    private void Update()
    {
        if(_canRotate)
        {
            foreach(Transform obj in _objects)
            {
                obj.eulerAngles = Vector3.Lerp(obj.eulerAngles, obj.eulerAngles + new Vector3(0,10,0), _approachRate);
            }
        }
    }

    public void MakeItAvailable(bool val)
    {
        _canRotate = val;
    }
}
