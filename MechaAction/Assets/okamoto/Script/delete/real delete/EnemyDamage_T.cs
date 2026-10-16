using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage_T : MonoBehaviour
{
    [SerializeField] public float damageAmount = 20f;

    // ColliderのIs Triggerにチェックが入っている場合、他のColliderと接触すると呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // 衝突した相手のオブジェクトが「Player」タグを持っているか確認
        if (other.gameObject.CompareTag("Player"))
        {
            // 衝突した相手からPlayerHealthSimpleコンポーネントを直接取得
            //PlayerHP_T playerHealth = other.GetComponent<PlayerHP_T>();

            // PlayerHealthSimpleコンポーネントがアタッチされていれば
            //if (playerHealth != null)
            //{
                // ダメージを与える
                //playerHealth.TakeDamage(damageAmount);

                // 弾丸などの場合は、ダメージを与えた後、自身を破壊する
                //Destroy(gameObject);
            //}
        }
    }
}
