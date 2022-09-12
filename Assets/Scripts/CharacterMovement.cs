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
    public enum CharState
    {
        Idle,
        Started,
        EndPhase,
        Restarted
    };

    [HideInInspector] public CharState charState = CharState.Idle;

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
        //GetInputs();
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
            _targetPos.z = _rb.position.z + velocityZ * Time.deltaTime;
            _rb.MovePosition(_targetPos);
        }
        else if (charState == CharState.Restarted)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, _endTR.position, _endPhaseApproachRate);
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, _endTR.eulerAngles, _endPhaseApproachRate);
        }
    }
}
