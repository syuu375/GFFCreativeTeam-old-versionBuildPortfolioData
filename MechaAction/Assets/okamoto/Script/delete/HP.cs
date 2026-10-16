using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    private float currentHP;
    private float maxHP;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _HP();
        }
    }

    private void _HP()
    {
        currentHP--;
    }
}
