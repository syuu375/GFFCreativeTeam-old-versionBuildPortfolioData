using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;

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

    public void Initialize(int damage ,int knockback ,int dir,string effectname,string audioname)
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
        //Debug.Log(_dir);
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

        var Interface = other.GetComponent<IPlayerDamage>();
        if (Interface != null)
        {
            Interface.TakeDamage(_damage, _knockback, _dir, _effectname, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾
        }
    }
}
