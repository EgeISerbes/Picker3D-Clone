using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsController : MonoBehaviour
{
    private List<Ball> _ballList = new List<Ball>();

    [Header("Ball Push Settings")]
    private float _velocityZ;
   [SerializeField] private float _pushOffset;

    [Header("Propel Settings")]
    [SerializeField] private float _launchForce;
    public int BallCount
    {
        get => _ballList.Count;
    }
    public void Launch()
    {
        foreach (Ball ball in _ballList)
        {
            ball.rb.AddForce(Vector3.forward * _launchForce);
        }
    }

    public void Init(float velocityZ)
    {
        _velocityZ = velocityZ;
    }
    private void FixedUpdate()
    {
        foreach (Ball ball in _ballList)
        {
            ball.rb.AddForce(0, 0, _velocityZ*Time.deltaTime*_pushOffset);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<Ball>();
            _ballList.Add(ball);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<Ball>();
            _ballList.Remove(ball);
            
        }
    }
}
