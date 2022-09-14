using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class MainChar : MonoBehaviour
{
   [SerializeField] private CharacterMovement _characterMovement;
   [SerializeField] private BallsController _ballsController;
   [SerializeField] private InfRotator _upgradedVersion;
    [SerializeField] private TriggerInteractions _trigger;
    System.Action<bool,int> GameFinished;
    private bool _hasEntered = false;
    private bool _isFinishing = false;
    public bool IsFinishing
    {
        set => _isFinishing = value;
        get => _isFinishing;
    }
    public float VelocityZ
    {
        get => _characterMovement.currentVelocityZ;
    }
    private float _tempVal ;
    public void Init(System.Action<bool,int> GameFinished)
    {
        this.GameFinished = GameFinished;
        _ballsController.Init(VelocityZ);

    }
    private void Awake()
    {
        //_characterMovement = GetComponent<CharacterMovement>();
        //_characterMovement.charState = CharacterMovement.CharState.Started;
        _ballsController.Init(VelocityZ);
        _upgradedVersion.gameObject.SetActive(false);
        _trigger.Init(OnTriggerInteractionEnter, OnTriggerInteractionExit);
    }
    public void StartState()
    {
        _characterMovement.charState = CharacterMovement.CharState.Started;
    }
    public void PauseState()
    {
        _characterMovement.charState = CharacterMovement.CharState.Idle;
    }
    private void OnTriggerInteractionEnter(GameObject other)
    {
        if(other.CompareTag("UIPlatform")&& !_hasEntered)
        {
            _tempVal = _characterMovement.velocityZ;
            _hasEntered = true;
            _characterMovement.velocityZ = 0f;
            var uiPlatform = other.GetComponent<UIPlatform>();
            uiPlatform.SetListener(OnExit);
            _ballsController.Launch();
            _upgradedVersion.gameObject.SetActive(false);
            _upgradedVersion.MakeItAvailable(false);

            var targetCount = uiPlatform.GetTargetVal();
            if (_ballsController.BallCount<targetCount)
            {   
                GameFinished(false,0);
            }
           
        }
        else if (other.CompareTag("EndPhaseTrigger"))
        {
            _characterMovement.charState = CharacterMovement.CharState.EndPhase;
            gameObject.transform.eulerAngles = other.transform.eulerAngles;
            _characterMovement.ModifyCollider(true);
            _upgradedVersion.gameObject.SetActive(false);
            _upgradedVersion.MakeItAvailable(false);
        }
        else if (other.CompareTag("EndPhasePiece")&&!_isFinishing)
        {
            _isFinishing = true;
            var eP = other.GetComponent<EndPhasePiece>();
            GameFinished(true, eP.GetPoints());
            

        }
        else if(other.CompareTag("Upgrader"))
        {
            Destroy(other.gameObject);
            _upgradedVersion.gameObject.SetActive(true);
            _upgradedVersion.MakeItAvailable(true);
        }
    }
    private void OnTriggerInteractionExit(GameObject other)
    {
        if(other.gameObject.CompareTag("EndPhaseTrigger"))
        {
            _characterMovement.charState = CharacterMovement.CharState.Idle;
        }
    }
    private void OnExit()
    {
        _characterMovement.velocityZ = _tempVal;
        _hasEntered = false;

    }

    public void SetTargetPos(Transform tr)
    {
        _characterMovement._endTR = tr;
    }
    public void SetRestartState()
    {
        _characterMovement.ModifyCollider(false);
        _characterMovement.charState = CharacterMovement.CharState.Restarted;
        
    }
}
