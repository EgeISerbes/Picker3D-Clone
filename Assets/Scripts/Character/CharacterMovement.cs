using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _targetPos;
    private Camera _mainCam;
    [SerializeField] private InputManager _inputs;
    [SerializeField] private bool _isAI;
    [Header("Character Settings")]
    public float velocityZ;
    [HideInInspector] public float currentVelocityZ;
    [Header("Character Settings(Not for AI)")]
    public float maxPositionX = 0.0f;
    public float maxForce;
    public float sideSpeed;
    public float maxVelocity;

    [SerializeField] private float _slowedAmount;

    [Header("End Phase Settings")]
    [HideInInspector] public Transform _endTR;
    [SerializeField] private float _endPhaseApproachRate;
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private float _pushForceStartAt;
    [SerializeField] private float _forceIncreaseRateByEveryInput;
    private bool _canTap = false;
    public enum CharState
    {
        Idle,
        Started,
        EndPhase,
        Restarted
    };

     public CharState charState = CharState.Idle;

    private void Awake()
    {
        _mainCam = Camera.main;
        _rb = gameObject.GetComponent<Rigidbody>();
        currentVelocityZ = velocityZ;
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        _targetPos = _rb.position;
    }
    private void Update()
    {
        if (_canTap && _inputs.IsPressedOnce)
        {
            _pushForceStartAt += _forceIncreaseRateByEveryInput;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        ControlKinematicState();
    }

    void ProcessKinematicInput(float velocityZ, float sideSpeed)
    {
        currentVelocityZ = velocityZ;
        if (_inputs.IsPressed)
        {
            currentVelocityZ = velocityZ * _slowedAmount;
        }

        var targetSideSpeed = sideSpeed * _inputs.InputVal.x;
        var currentVelocity = _rb.velocity;
        currentVelocity.x = Mathf.Clamp(Mathf.MoveTowards(currentVelocity.x, targetSideSpeed, maxForce * Time.fixedDeltaTime), -maxVelocity, maxVelocity);
        _targetPos = _rb.position + new Vector3(currentVelocity.x, 0, currentVelocityZ) * Time.deltaTime;
        _targetPos = new Vector3(Mathf.Clamp(_targetPos.x, -maxPositionX, maxPositionX), _targetPos.y, _targetPos.z);

    }

    public void ModifyCollider(bool val)
    {
        _meshCollider.convex = val;
        _rb.isKinematic = !val;
        if(val)
        {
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
         
        _canTap = val;
    }
    void ControlKinematicState()
    {
        if (charState == CharState.Started)
        {
            if (_isAI)
            {

                _targetPos.z = _rb.position.z + (velocityZ) * Time.deltaTime;
                _rb.MovePosition(_targetPos);


            }
            else
            {

                ProcessKinematicInput(velocityZ, sideSpeed);
                _rb.MovePosition(_targetPos);


            }
        }
        else if (charState == CharState.EndPhase)
        {
            //_targetPos = _rb.position + new Vector3(0, velocityZ * Mathf.Sin(15) * Time.deltaTime, velocityZ * Mathf.Cos(15) * Time.deltaTime);
            //_targetPos.z = _rb.position.z + (velocityZ) * Time.deltaTime;
            _rb.AddForce(new Vector3(0, 0, _pushForceStartAt));
            //_rb.MovePosition(_targetPos);
        }
        else if (charState == CharState.Restarted)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, _endTR.position, _endPhaseApproachRate);
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, _endTR.eulerAngles, _endPhaseApproachRate);
        }
    }
}
