using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyState { Look, Chase, Return, Attack }

    [SerializeField] private float _lookRange = 3f;    // �p�g���[���͈�
    [SerializeField] private float _chaseRange = 5f;   // �T�m�͈�
    [SerializeField] private float _attackRange = 1f;  // �U���͈�
    [SerializeField] private float _moveSpeed = 4.0f;    // �ړ����x
    [SerializeField] private float _chaseSpeed = 6.0f;    // Chase���x
    [SerializeField] private float _waitTime = 1.0f;   // �[�ő҂���

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;
    private Transform _player;
    private Rigidbody _rb;

    private int _lookDirection = 1; // 1 = �E, -1 = ��
    private bool _isWaiting = false;

    private void Awake()
    {
        _spawnPos = transform.position;
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(_rb.position, _player.position) < _chaseRange)
                    _state = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                Chase();
                if (Vector3.Distance(_rb.position, _player.position) < _attackRange)
                    _state = EnemyState.Attack;
                else if (Vector3.Distance(_rb.position, _player.position) > _chaseRange)
                    _state = EnemyState.Return;
                break;

            case EnemyState.Return:
                ReturnToSpawn();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void Look()
    {
        if (_isWaiting)
        {
            _rb.linearVelocity = Vector3.zero; // �ҋ@���͒�~
            return;
        }

        // ���݂̑��x���R�s�[
        Vector3 velocity = _rb.linearVelocity;
        velocity.x = _lookDirection * _moveSpeed;

        _rb.linearVelocity = velocity;// Rigidbody �ɔ��f

        // �X�|�[���ʒu����̋������m�F
        float distanceFromSpawn = _rb.position.x - _spawnPos.x;

        if (distanceFromSpawn > _lookRange)
        {
            StartCoroutine(WaitAndTurn(-1));
        }
        else if (distanceFromSpawn < -_lookRange)
        {
            StartCoroutine(WaitAndTurn(1));
        }
    }

    private IEnumerator WaitAndTurn(int newDirection)
    {
        _isWaiting = true;
        //_rb.velocity = Vector3.zero; // ��~
        yield return new WaitForSeconds(_waitTime); // �����҂�
        _lookDirection = newDirection; // �����]��

        // ���������[���牟���߂��i���t���[���Œ[��������Ȃ��悤�Ɂj
        _rb.position += new Vector3(_lookDirection * 0.1f, 0f, 0f);
        _isWaiting = false;
    }

    private void Chase()
    {
        //��x�ϐ��ɃR�s�[���Ă���ҏW
        Vector3 velocity = _rb.linearVelocity;

        // X�������v���C���[�����ɓ�����
        float _directionX = Mathf.Sign(_player.position.x - _rb.position.x); // �E������
        velocity.x = _directionX * _chaseSpeed;


        _rb.linearVelocity = velocity;//�ҏW�����l��߂���rigidbody�Ŏ��s

    }

    private void ReturnToSpawn()
    {
        Vector3 toSpawn = (_spawnPos - _rb.position);
        if (toSpawn.magnitude < 0.1f)
        {
            // �X�|�[���ɖ߂����珄��ĊJ
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.Look;
            return;
        }

        Vector3 velocity = toSpawn.normalized * _moveSpeed;
        _rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, 0f);
    }

    private void Attack()
    {
        _rb.linearVelocity = Vector3.zero;
        Debug.Log("�U���I");
        //�U���A�j���[�V�����ⓖ���蔻��������Ŏ���
        _state = EnemyState.Chase;
    }
}