using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire_Drone : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    [SerializeField] private Transform _tireObj;
    private Tire _tire;
    private Rigidbody _childrb;
    private Rigidbody _rb;
    private Transform _player;

    private int _clear;
    private float _moveSpeed = 10f;  // �E�ւ̈ړ����x
    private bool _isdrop = false;
    public int dir;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _childrb = _tireObj.GetComponent<Rigidbody>();
        _tire = _tireObj.GetComponent<Tire>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("Tire");
        if (attackData != null)
        {
            _hitdamage = attackData.Hitdamage + (_clear * 10);
            _hitknockback = attackData.Hitknockback + (_clear * 2);
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }

        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//�E
            dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//��
            dir = -1;
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        if(!CanMove) return;
        velocity = _rb.linearVelocity;

        velocity.x = _moveSpeed * dir;

        _rb.linearVelocity = velocity;
    }

    private void Update()
    {
        if(!_isdrop && Vector3.Distance(transform.position, _player.transform.position) < 10f)
        {
            Drop();
        }
    }

    private void Drop()
    {
        _isdrop = true;
        _childrb.transform.parent = null;
        _tire._FallStart(_hitdamage, _hitknockback, dir, _effectname, _audioname);
        _childrb.isKinematic = false;
        _childrb.AddForce(transform.forward * 6f, ForceMode.Impulse);
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
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1.0f));
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
