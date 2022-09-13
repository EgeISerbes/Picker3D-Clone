using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndPhasePiece : MonoBehaviour
{
    [SerializeField] TextMeshPro _text;
    [SerializeField] private int _val;
    private void Awake()
    {
        _text.SetText(_val.ToString());
    }
}
