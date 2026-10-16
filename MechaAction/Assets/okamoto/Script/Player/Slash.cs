
using UnityEngine;


public class Slash : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] DamageEffectSO _damageEffectSO;
    [SerializeField] private float _speed;

    private int _damage;
    private int _knockback;
    private int _dir;
    private string _effectname;
    private string _audioname;
    private bool _electslash;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage, int knockback, int dir, string effectname,string audioname ,bool electslash)
    {
        _damage = damage;
        _knockback = knockback;
        _effectname = effectname;
        _audioname = audioname;
        _dir = dir;

        _electslash = electslash;
    }

    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void FixedUpdate()
    {
        velocity = _rb.linearVelocity;
        velocity.x = _dir * _speed;
        _rb.linearVelocity = velocity;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("��������");

        if (other.CompareTag("Enemy"))
        {
            if(_electslash)//audio�͌�ɒ���
            {
                var Interface_E = other.GetComponent<IDamage>();
                if (Interface_E != null)
                {
                    Interface_E.TakeElectDamage(_damage, _knockback, _dir,5f, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾

                    //var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//�����_�`��AI�Œm����
                    //if (attackData != null && attackData.HitEffect != null)
                    //{
                    //    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    //    Destroy(effect, 0.2f);
                    //}
                }

                return;
            }

            var Interface = other.GetComponent<IDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir,_audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//�����_�`��AI�Œm����
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    //Destroy(effect, 0.2f);
                }
            }
        }
    }

}
