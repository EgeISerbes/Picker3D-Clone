using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputValues _inputValues =new InputValues();
    private Camera _mainCam;
    public float inputCamOffset = 1;
    private int _tempFingerID = -1;
    private Vector3 _prevPos = Vector3.zero;
    private Vector3 _tempMousePos;
    private Vector3 _tempWorldPos = Vector3.zero;
    private Vector3 _diff = Vector3.zero;
   [SerializeField] private float _sensitivity;
    public bool IsPressed
    { get => _inputValues.isPressed; }
    public bool IsReleased
    { get => _inputValues.isReleased; }
    public bool IsPressedOnce
    {
        get => _inputValues.isPressedOnce;
    }
    public Vector3 InputVal
    {
        get => _inputValues.inputPos;
    }
    public Vector3 WorldPos
    {
        get => _inputValues.worldPos;
    }

    private void Awake()
    {
        _mainCam = Camera.main;
    }
    void Update()
    {
        GetInputs();
    }
    public class InputValues
    {
        public bool isPressed;
        public bool isReleased;
        public bool isPressedOnce;
        public Vector3 inputPos;
        public Vector3 worldPos;
        public InputValues()
        {
            inputPos = Vector3.zero;
            worldPos = Vector3.zero;
            isPressed = false;
            isReleased = false;
            isPressedOnce = false;
        }
    }
    void GetInputs()
    {
        _inputValues.isReleased = false; //Always set isReleased false on running
        _inputValues.isPressedOnce = false;
        if (Application.isEditor)
        {
            //_inputValues.isPressed = (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) ? true : false;
            //_inputValues.isReleased = (Input.GetMouseButtonUp(0)) ? true : false;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                _inputValues.isPressed = true;
                _tempMousePos = Input.mousePosition + new Vector3(0, 0, _mainCam.nearClipPlane + inputCamOffset);
                _tempWorldPos = _mainCam.ScreenToWorldPoint(_tempMousePos);
                if(Input.GetMouseButtonDown(0))
                {
                    _prevPos = _tempWorldPos;
                    _inputValues.isPressedOnce = true;
                }
                
                 _diff = (_tempWorldPos - _prevPos)*_sensitivity;
                 _inputValues.inputPos = Vector3.LerpUnclamped(_inputValues.inputPos,_diff,(1 - Mathf.Pow(0.8f, Time.deltaTime * 60)));
                
                _prevPos = _tempWorldPos;
                _inputValues.worldPos = _tempWorldPos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _inputValues.isReleased = true;
                _inputValues.isPressed = false;
                _inputValues.inputPos = Vector3.zero;
            }

            //Debug.Log(_inputValues.worldPos);
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (_tempFingerID == -1)
                {
                    foreach (Touch inputs in Input.touches)
                    {
                        if (inputs.phase == TouchPhase.Began)
                        {
                            _tempFingerID = inputs.fingerId;
                            _inputValues.isPressed = true;
                            _inputValues.isReleased = false;
                            _inputValues.isPressedOnce = true;
                            _tempMousePos = new Vector3(inputs.position.x, inputs.position.y, 0) + new Vector3(0, 0, _mainCam.nearClipPlane + inputCamOffset);
                            _tempWorldPos = _mainCam.ScreenToWorldPoint(_tempMousePos);
                            _prevPos = _tempWorldPos;
                            _inputValues.worldPos = _tempWorldPos;
                        }
                    }
                }
                else
                {


                    foreach (Touch inputs in Input.touches)
                    {
                        if (inputs.fingerId == _tempFingerID)
                        {
                            if (inputs.phase == TouchPhase.Moved || inputs.phase == TouchPhase.Stationary)
                            {
                                _inputValues.isPressed = true;
                                _tempMousePos = Input.mousePosition + new Vector3(0, 0, _mainCam.nearClipPlane + inputCamOffset);
                                _tempWorldPos = _mainCam.ScreenToWorldPoint(_tempMousePos);
                                if (inputs.phase == TouchPhase.Moved)
                                {
                                    _inputValues.inputPos = _tempWorldPos - _prevPos;
                                }
                                _prevPos = _tempWorldPos;
                                _inputValues.worldPos = _tempWorldPos;
                            }
                            else if (inputs.phase == TouchPhase.Ended || inputs.phase == TouchPhase.Canceled)
                            {
                                _inputValues.isReleased = true;
                                _inputValues.isPressed = false;
                                _inputValues.inputPos = Vector3.zero;
                                _tempFingerID = -1;
                            }
                            return;
                        }

                    }
                    _inputValues.isPressed = false;
                    _inputValues.isReleased = true;
                    _tempFingerID = -1;
                }
            }
        }
    }
}
