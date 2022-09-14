using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private GameObject _partRef;
    public Rigidbody rb;
    [SerializeField] private float _explodeTimer;
    public bool hasExploded = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Explode()
    {
        //var obj = Instantiate(_partRef, transform.position, transform.rotation);
        hasExploded = true;
        StartCoroutine(Explosion());
        
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(_explodeTimer);
        //var obj = Instantiate(_partRef, transform.position, transform.rotation);
        Destroy(gameObject);
    }

   
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Barrier"))
    //    {
    //        Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
    //    }
    //}
}
