using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Drone_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private DroneEnemy _enemy;

    private int _clear;
    [SerializeField] private Transform _muzzlepoint;
    public GameObject _guneffect;

    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private int _dir;
    private string _effectname;
    private string _audioname;

    private void Awake()
    {
        _enemy = GetComponent<DroneEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("BurstEnemy");
        if (attackData != null)
        {
            _damage = (int)(attackData.Damage + (_clear * 10));
            _knockback = (int)(attackData.Knockback + (_clear * 2));
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }
    private void Update()
    {
        _dir = _enemy.dir;
    }

    public IEnumerator Attack()
    {
        Debug.Log("beem");
        var G_effect = Instantiate(_guneffect, _muzzlepoint.position, Quaternion.identity);
        Destroy(G_effect, 2f); // アニメーションの長さに合わせて

        yield return new WaitForSeconds(2f);

        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, transform.forward, out RaycastHit hit, Quaternion.identity, 30f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var Interface = hit.collider.GetComponent<IPlayerDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
                }
            }
        }

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(transform.position + Vector3.one * 30f, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
            }
        }
    }
}