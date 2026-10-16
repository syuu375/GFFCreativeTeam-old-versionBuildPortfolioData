using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.UI.Image;

public class BurstEnemy : MonoBehaviour, IEnemy
{
    private enum EnemyState { 
        Look,          //�T��
        Move,          //�ǐ�
        Wait,          //���˗p��(����Ȃ�)
        Attack,         //����
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private CapsuleCollider _col;
    private Rigidbody _rb;
    private Animator _anim;
    private Burst_Attack _attack;
    private float _moveSpeed = 1.0f;
    private bool _moveStop = false;
    public int _direction = -1;
    private float _attacktime = 0;
    private float _fallTime;
    Vector3 velocity;
    Vector3 origin;
    private bool _isGrounded;

    private bool _isRight = false;
    private bool _isLeft = true;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<CapsuleCollider>();
        _attack = GetComponent<Burst_Attack>();
    }

    private void Start()
    {
        _isLeft = true;
    }

    private void Update()
    {
        if (_isRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _isRight = false;
        }
        else if (_isLeft)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            _isLeft = false;
        }

        Vector2 bounds = _col.bounds.size;
        RaycastHit hit;
        origin = transform.position + Vector3.down * (bounds.y / 2);
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.4f);
    }

    private void FixedUpdate()
    {
        switch(_state)
        {
            case EnemyState.Look:
                Look();
                _attacktime = 0.0f;
                if (Vector3.Distance(transform.position, _player.position) < 15f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                _attacktime += Time.deltaTime;
                Move();
                
                if (Vector3.Distance(transform.position, _player.position) > 20f)
                {
                    _state = EnemyState.Look;
                }
                else if (_attacktime > 2f)
                {
                    _state = EnemyState.Attack;
                }
                break;

            case EnemyState.Wait:
                Wait();
                break;

            case EnemyState.Attack:
                Attack();
                _attacktime = 0.0f;
                _state = EnemyState.Move;
                break;
        }

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0f;
        }
    }

    private void Look()
    {
        _anim.SetInteger("Speed", 0);

    }

    private void Move()
    {
        velocity = _rb.linearVelocity;

        if (_moveStop)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, _rb.linearVelocity.z);
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if(_direction == -1)
            {
                _isRight = true;
                StartCoroutine(Waitturn(1));
            }
            _direction = 1;
        }
        else
        {
            if (_direction == 1)
            {
                _isLeft = true;
                StartCoroutine(Waitturn(-1));
            }
            _direction = -1;
        }
        _anim.SetInteger("Speed", 1);
        velocity.x = _direction * _moveSpeed;

        _rb.linearVelocity = velocity;
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unity�̕W���d�͂ɔC�������Ȃ� fallSpeed �͕s�v

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y���x�ɏ��X�ɉ��Z
                                                        //Time.fixedDeltaTime �������Z���t���[�����[�g�Ɉˑ������Ȃ����ߕK�{
        if (velocity.y < -20f)//�������x�̐���
        {
            velocity.y = -20f;
        }
    }

    //�U��Ԃ�Ƃ��������܂�
    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _direction = _newdirection;
        _moveStop = false;

        yield break;
    }

    private void Wait()
    {

    }

    private void Attack()
    {
        _attack.GunAttack();
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Look;
        yield break ;
    }

    public void SKnockBack(int dir,int knockback)
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
