﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] _controlPoints;
    private Vector3 _gizmosPos;

    private void OnDrawGizmos()
    {
        for (float t =0; t <= 1; t+= .05f)
        {
            _gizmosPos = Mathf.Pow(1 - t, 3) * _controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[2].position +
                Mathf.Pow(t, 3) * _controlPoints[3].position;
            Gizmos.DrawSphere(_gizmosPos, .25f);

        }
        Gizmos.DrawLine(_controlPoints[0].position, _controlPoints[1].position);
        Gizmos.DrawLine(_controlPoints[2].position, _controlPoints[3].position);
    }
}
