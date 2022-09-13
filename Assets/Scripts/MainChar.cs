using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class MainChar : MonoBehaviour
{
   [SerializeField] private CharacterMovement _characterMovement;
   [SerializeField] private BallsController _ballsController;
    System.Action<bool> GameFinished;
    private bool _hasEntered = false;
    public float VelocityZ
    {
        get => _characterMovement.currentVelocityZ;
    }
    private float _tempVal ;
    public void Init(System.Action<bool> GameFinished)
    {
        this.GameFinished = GameFinished;
        _ballsController.Init(VelocityZ);

    }
    private void Awake()
    {
        //_characterMovement = GetComponent<CharacterMovement>();
        //_characterMovement.charState = CharacterMovement.CharState.Started;
        _ballsController.Init(VelocityZ);

    }
    public void StartState()
    {
        _characterMovement.charState = CharacterMovement.CharState.Started;
    }
    public void PauseState()
    {
        _characterMovement.charState = CharacterMovement.CharState.Idle;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("UIPlatform")&& !_hasEntered)
        {
            _tempVal = _characterMovement.velocityZ;
            _hasEntered = true;
            _characterMovement.velocityZ = 0f;
            var uiPlatform = other.gameObject.GetComponent<UIPlatform>();
            uiPlatform.SetListener(OnExit);
            _ballsController.Launch();
            
            var targetCount = uiPlatform.GetTargetVal();
            if (_ballsController.BallCount<targetCount)
            {
                GameFinished(false);
            }
           
        }
        else if (other.gameObject.CompareTag("EndPhaseTrigger"))
        {
            _characterMovement.charState = CharacterMovement.CharState.EndPhase;
            _characterMovement.ModifyCollider();
        }
        else if (other.gameObject.CompareTag("EndPhasePiece"))
        {

        }
    }
    private void OnTriggerExit(Collider other)
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
}
