using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   [SerializeField] private MainChar _mainCharacter;

    [Header("Camera Follow Settings")]
    private Camera _camMain;
    public float playerFollowRateX;
    public float playerVelocityXApproachRate;
    [SerializeField] private float _multiplier;
    [SerializeField] private float _approachRate;
    [SerializeField] private float _cur_Multiplier;

    [Header("Camera Position Settings")]
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    private Vector3 _temp = Vector3.zero;
    
    
    public enum CameraState
    {
        Idle,
        Started,
        EndPhase
    }

    private CameraState cameraState = CameraState.Idle;

    public void Init(MainChar mainChar)
    {
        this._mainCharacter = mainChar;
        cameraState = CameraState.Started;
    }
    private void Awake()
    {
        _camMain = Camera.main;
        cameraState = CameraState.Started;
    }
    void Start()
    {
        transform.position = _mainCharacter.transform.position;
        StartCoroutine(LateStart());
    }
    private void LateUpdate()
    {
        if (cameraState == CameraState.Started || cameraState == CameraState.EndPhase)
        {
            _temp = transform.position;
            _temp = new Vector3(Mathf.MoveTowards(_temp.x, (_mainCharacter.transform.position.x) * playerFollowRateX, playerVelocityXApproachRate), _temp.y, Mathf.MoveTowards(_temp.z, _mainCharacter.transform.position.z, _mainCharacter.VelocityZ * _cur_Multiplier));
            transform.position = _temp;

            if (cameraState == CameraState.EndPhase)
            {
                _camMain.transform.eulerAngles = new Vector3(Mathf.LerpAngle(_camMain.transform.eulerAngles.x, _endPos.eulerAngles.x, _approachRate), Mathf.LerpAngle(_camMain.transform.eulerAngles.y, _endPos.eulerAngles.y, _approachRate), Mathf.LerpAngle(_camMain.transform.eulerAngles.z, _endPos.eulerAngles.z, _approachRate));

                _camMain.transform.position = new Vector3(Mathf.Lerp(_camMain.transform.position.x, _endPos.position.x, _approachRate), Mathf.Lerp(_camMain.transform.position.y, _endPos.position.y, _approachRate), Mathf.Lerp(_camMain.transform.position.z, _endPos.position.z, _approachRate));
            }
        }
    }
    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        _camMain.transform.position = _startPos.position;
        _camMain.transform.rotation = _startPos.rotation;
    }

    public void EndPhaseStarted()
    {
        cameraState = CameraState.EndPhase;
    }
}
