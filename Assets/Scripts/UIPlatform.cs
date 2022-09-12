using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPlatform : MonoBehaviour
{

    [SerializeField] private int _targetValue;
    [SerializeField] private BallPlatform _ballPlatform;
    [SerializeField] private Collider _invBarrier;
    [SerializeField] private MoraleBoostUI _moraleText;
    [SerializeField] private GateRotator _gateRotator;

    [Header("Platform Settings")]
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _p_approachRate;
    [SerializeField] private float _waitSeconds;
    private Vector3 _targetPos;
    private bool _canMove = false;
    private System.Action OnExit;

    private void Awake()
    {
        _targetPos = transform.position;
        _invBarrier = GetComponent<Collider>();
        _ballPlatform.Init(LowerWall, _targetValue);
    }
    public float GetTargetVal()
    {
        return _targetValue;
    }
    public void SetListener(System.Action OnExit)
    {
        this.OnExit = OnExit;
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
        _moraleText.gameObject.SetActive(true);
        _moraleText.SetMessageActive();
        _gateRotator.RotateGates();
        StartCoroutine(ExitTrigger());
        
    }

    IEnumerator ExitTrigger()
    {
        yield return new WaitForSeconds(_waitSeconds);
        _invBarrier.isTrigger = false;
        OnExit();
    }
}
