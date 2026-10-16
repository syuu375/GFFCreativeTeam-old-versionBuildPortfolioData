using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp")]
public class PowerUpSO : ScriptableObject
{
    [SerializeField] private float damagemultiplier = 1.2f;
    [SerializeField] private int hp = 50;

    public float DamageMult { get => damagemultiplier; }
    public int Hp { get => hp; }


}
