using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(int amount, int knockback, int dir, string audioname);

    void TakeElectDamage(int amout,int knockback, int dir,float electtime, string audioname);
    void Heal(int amount);
    void Die();
}
