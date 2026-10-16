using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoxDrone_Move : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _clear;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;


    private Transform _player;
    private Rigidbody _rb;
    private float chaseRange = 15f;          // ���F����
    private float moveSpeed = 8f;           // �ǐՑ��x
    private float velocitySmoothTime = 0.9f; // �����E�����̊��炩��
    private int _dir;

    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;

        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("BoxDrone_Move");
        if (attackData != null)
        {
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//�E
            _dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//��
            _dir = -1;
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            _rb.linearVelocity = Vector3.zero;
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Vector3 targetVelocity = direction * moveSpeed;

        // ���炩�ɑ��x��ω�������
        _rb.linearVelocity = Vector3.SmoothDamp(_rb.linearVelocity, targetVelocity, ref currentVelocity, velocitySmoothTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var Interface = other.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾
            }
        }
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Move;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(2f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
}
