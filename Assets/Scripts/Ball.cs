using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameObject _partRef;
    public void Explode()
    {
        var obj = Instantiate(_partRef, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
