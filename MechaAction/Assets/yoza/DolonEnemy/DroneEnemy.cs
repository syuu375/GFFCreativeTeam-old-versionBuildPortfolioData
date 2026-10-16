using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;


[RequireComponent(typeof(Rigidbody))]
public class DroneEnemy : MonoBehaviour, IEnemy
{
    enum EnemyState
    { 
        DITECTION,
        MOVE,
        ATTACK,
        STIFFNESS,
        DAMAGE//�����Ȃ�
    };

    private Drone_Attack _attack;
    private Rigidbody _rb;
    private Transform _playerTransform;
    private EnemyState _state;
   [SerializeField] private float _distance;
    private Vector3 _velocity;
    private float _moveSpeed = 5f;
    public int dir;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _attack = GetComponent<Drone_Attack>();
        _state = EnemyState.DITECTION;
    }

    private void Update()
    {
        if (_rb.position.x < _playerTransform.position.x)
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
        
        _distance = Vector3.Distance(_rb.position, _playerTransform.position);
        switch (_state)
        {
            case EnemyState.DITECTION:
                Ditection();
                break;

            case EnemyState.MOVE:
                Move();
                break;
                
            case EnemyState.ATTACK:
                Attack();
                break;

            case EnemyState.STIFFNESS:
                {
                    IsStiff = Stiffness();
                    if (IsStiff == false)
                    {
                        _state = EnemyState.DITECTION;
                        //Debug.Log("false");
                    }
                    break;
                }
        }

    }

    private bool IsStiff = true;
    private void Ditection()
    {
        if(_distance <= 50f)
        {
            _state = EnemyState.MOVE;
        }
    }

    private void Move()
    {
        if(_distance <= 10f)
        {
            
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.ATTACK;
            return;
        }
        _velocity.x = _moveSpeed;
        _rb.linearVelocity = (_rb.position.x < _playerTransform.position.x) ? (_velocity) : (-_velocity); 
    }

    private void Attack()
    {
        Debug.Log("Attack");
        StartCoroutine(_attack.Attack());
        _state = EnemyState.STIFFNESS;
    }

    private float stifftime;
    private bool Stiffness()
    {
        stifftime += Time.deltaTime;
        if (stifftime > 3f)
        {
            stifftime = 0;
            return false;
        }
        else
            return true;
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.DITECTION;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(1.0f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(electtime));
    }
}