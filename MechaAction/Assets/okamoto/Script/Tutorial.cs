using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _trueUI;
    [SerializeField] private GameObject _falseUI;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        
        if(_falseUI != null) _falseUI.SetActive(false);
        if(_trueUI != null) _trueUI.SetActive(true);
        Destroy(gameObject);
    }
}