using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage_T : MonoBehaviour
{
    [Header("好きに入れて")]
    [SerializeField] public float damageAmount = 20f;

    // 衝突やトリガー発動時に呼ばれる関数 (今回は例としてトリガーを使用)
    private void OnTriggerEnter(Collider other)
    {
        // 衝突した相手からkeiyakuインターフェースを実装したコンポーネントを取得
        IDamage targetHealth = other.GetComponent<IDamage>();

        // keiyakuを持っている（HPを持つべき）オブジェクトであれば
        if (targetHealth != null)
        {
            // 抽象的なIHealth型を通してPlayerDamageを呼ぶ
            //targetHealth.PlayerDamage(damageAmount);

            // 弾丸などの場合は、ここで自身を破壊する
            // Destroy(gameObject);
        }
    }

    // 衝突判定にColliderが使えない場合や、直接メソッドを呼ぶ場合のためのテスト関数
    /* public void DealDamage(GameObject target)
     {
         keiyaku targetHealth = target.GetComponent<keiyaku>();
         if (targetHealth != null)
         {
             targetHealth.PlayerDamage(damageAmount);
         }
     }*/
}
