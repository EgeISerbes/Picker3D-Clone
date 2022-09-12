using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BallPlatform : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private float _targetVal;
    private float _currentVal;
    System.Action OnBallsAreStacked;
    private string[] _textArr;
    private string _targetSTR;

    public void Init(System.Action OnBallsAreStacked, int targetVal)
    {
        _targetVal = targetVal;
        this.OnBallsAreStacked = OnBallsAreStacked;
        _textArr = _text.text.ToString().Split('/');
        _textArr[1] = targetVal.ToString();
        SetValueToUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            ball.Explode();
            ModifyText();

        }
    }

    void ModifyText()
    {
        _currentVal += 1;
        if(_currentVal>=_targetVal)
        {
            OnBallsAreStacked();
        }
        _textArr[0] = _currentVal.ToString();
        SetValueToUI();
    }

    void SetValueToUI()
    {
        _targetSTR = _textArr[0] + '/' + _textArr[1];
        _text.SetText(_targetSTR);
    }
}
