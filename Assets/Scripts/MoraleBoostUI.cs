using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoraleBoostUI : MonoBehaviour
{
    [Header("String Settings")]
    [SerializeField] private List<string> _coolThingsToSay;
    private string _targetText;
    [SerializeField] private TextMeshPro _text;

    [Header("Target Pos and Size Settings")]
    [SerializeField] private Transform _targetTR;

    [Header("Approaching Settings")]
    [SerializeField] private float _posApproachValue;
    [SerializeField] private float _sizeApproachValue;
    private bool _canApproach = false;
   [SerializeField] private float _destroyTime = 1f;
    private float _timer;

    //[SerializeField] private float _holdDuration;
    [SerializeField] private GameObject _partRef;

    private void Awake()
    {
        int randomIndex = Random.Range(0, _coolThingsToSay.Count);
        _targetText = _coolThingsToSay[randomIndex];
        _text.SetText(_targetText);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canApproach)
        {
            transform.position = Vector3.Lerp(transform.position, _targetTR.position, _posApproachValue);
            transform.localScale = Vector3.Lerp(transform.localScale, _targetTR.localScale, _sizeApproachValue);
            _timer += Time.deltaTime;
            if(_timer >= _destroyTime)
            {
                if (_partRef != null) Instantiate(_partRef, transform.position, transform.rotation);
                Destroy(gameObject);
            }
               
            
        }
    }
    public void SetMessageActive()
    {
        _canApproach = true;
    }
}
