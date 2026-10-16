using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamage 
{
    void TakeDamage(int amount, int knockback, int dir, string effectname, string audioname);

    void TakeElectDamage(int amount,float electtime, string effectname, string audioname);

    void TakeBanDamage(float bantime, string effectname, string audioname);

    void Heal(int amount, string effectname, string audioname);
    void Die();
}
