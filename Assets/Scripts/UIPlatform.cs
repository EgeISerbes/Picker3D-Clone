using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPlatform : MonoBehaviour
{

    [SerializeField] private int _targetValue;
    [SerializeField] private BallPlatform _ballPlatform;
    [SerializeField] private GameObject _invWall;

    [Header("Platform Settings")]
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _p_approachRate;
    private Vector3 _targetPos;
    private bool _canMove = false;

    private void Awake()
    {
        _targetPos = transform.position;
        _ballPlatform.Init(LowerWall, _targetValue);
    }
    public float GetTargetVal()
    {
        return _targetValue;
    }

    private void Update()
    {
        if (_canMove)
        {
            _platform.transform.position = Vector3.Lerp(_platform.transform.position, _targetPos, _p_approachRate);
        }
    }

    public void LowerWall()
    {
        _platform.SetActive(true);
        _canMove = true;
        _invWall.SetActive(false);
    }
}
