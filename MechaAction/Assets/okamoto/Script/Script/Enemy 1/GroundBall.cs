using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBall : MonoBehaviour
{
    private ElectricEnemy _enemy;
    private Rigidbody _rb;

    private Vector3 velocity;

    private float _movespeed = 5f;
    private int _dir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    public void Initialize(ElectricEnemy enemy)
    {
        _enemy = enemy;
        _dir = _enemy._direction;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        velocity = _rb.linearVelocity;

        velocity.x = _movespeed * _dir;

        _rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //null

            //TakeDamage
            //electricdamage
        }

        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            Destroy(gameObject);
        }
    }
}
