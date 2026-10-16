using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreGrounded : MonoBehaviour
{
    [SerializeField] private Collider _ground;
    private Collider _col;


    private void OnTriggerEnter(Collider other)
    {
        _col = other.GetComponent<Collider>();

        Physics.IgnoreCollision(_ground, _col ,true);
    }

    private void OnTriggerExit(Collider other)
    {
        _col = other.GetComponent<Collider>();

        Physics.IgnoreCollision(_ground, _col, false);
    }
}
