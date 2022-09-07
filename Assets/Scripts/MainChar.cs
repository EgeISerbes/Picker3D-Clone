using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class MainChar : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private BallsController _ballsController;
    System.Action<bool> GameFinished;
    public float VelocityZ
    {
        get => _characterMovement.currentVelocityZ;
    }

    public void Init(System.Action<bool> GameFinished)
    {
        this.GameFinished = GameFinished;
    }
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterMovement.charState = CharacterMovement.CharState.Started;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("UIPlatform"))
        {
            var uiPlatform = other.gameObject.GetComponent<UIPlatform>();
            _ballsController.Launch();
            var targetCount = uiPlatform.GetTargetVal();
            if (_ballsController.BallCount<targetCount)
            {
                GameFinished(false);
            }
           
        }
    }
}
