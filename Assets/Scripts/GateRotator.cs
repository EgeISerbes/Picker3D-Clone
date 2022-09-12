using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRotator : MonoBehaviour
{
    private bool _canRotate = false;
    [SerializeField] private List<Transform> _gates = new List<Transform>();
   [SerializeField] private float _targetRotationVal;
    [SerializeField] private float _approachRate;
    

    // Update is called once per frame
    void Update()
    {
        if(_canRotate)
        {
            foreach(Transform gate in _gates)
            {
                gate.eulerAngles = Vector3.Lerp(gate.eulerAngles, new Vector3(gate.eulerAngles.x, gate.eulerAngles.y, _targetRotationVal), _approachRate);
            }
        }
    }

    public void RotateGates()
    {
        _canRotate = true;
    }
}
