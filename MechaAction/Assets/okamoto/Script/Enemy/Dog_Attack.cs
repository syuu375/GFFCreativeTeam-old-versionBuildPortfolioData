using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _clear;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private DogEnemy _enemy;
    public LayerMask ignoreLayer;

    private int _dir;

    private void Awake()
    {
        _enemy = GetComponent<DogEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("DogEnemy");
        if (attackData != null)
        {
            _damage = attackData.Damage;
            _knockback = attackData.Knockback;
            _hitdamage = attackData.Hitdamage;
            _hitknockback = attackData.Hitknockback;
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        //transform.position += transform.forward * 3f * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        _dir = _enemy._direction;
    }

    public void GunAttack()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f,~ignoreLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var Interface = hit.collider.GetComponent<IPlayerDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir,_effectname, _audioname);//敵のインターフェース<IDamage>取得

                }
            }
        }
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
