using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : MonoBehaviour
{
    private int _heal = 30;
    private string _effectname = "DamageEffect";
    private string _audioname;


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
        if (Interface != null)
        {
            Interface.Heal(_heal, _effectname, _audioname);//敵のインターフェース<IDamage>取得
        }
        Destroy(gameObject);
    }
}
