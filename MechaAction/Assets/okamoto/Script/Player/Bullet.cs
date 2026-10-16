using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] DamageEffectSO _damageEffectSO;

    [SerializeField] private float _speed;
    
    private int _damage;
    private int _knockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage, int knockback, int dir, string effectname, string audioname)
    {
        _damage = damage;
        _knockback = knockback;
        _effectname = effectname;
        _audioname = audioname;
        _dir = dir;
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    private void FixedUpdate()
    {
        velocity = _rb.linearVelocity;
        velocity.x = _dir * _speed;
        _rb.linearVelocity = velocity;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("��������");

        if (other.CompareTag("Enemy"))
        {
            var Interface = other.GetComponent<IDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//�����_�`��AI�Œm����
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 0.2f);
                }
            }
        }
    }
}
