using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Vector2 minPos;         // Clampç≈è¨íl
    [SerializeField] private Vector2 maxPos;         // Clampç≈ëÂíl
    [SerializeField] private GameObject _wall;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _wall.SetActive(true);
        GManager.Instance.CheckPoint(transform.position,minPos,maxPos); 
    }
}
