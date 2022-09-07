using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsController : MonoBehaviour
{
    private List<Ball> _ballList;

    [Header("Propel Settings")]
   [SerializeField] private float _launchForce;
    public int BallCount
    {
        get => _ballList.Count;
    }
    public void Launch()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<Ball>();
            _ballList.Add(ball);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<Ball>();
            _ballList.Remove(ball);
        }
    }
}
