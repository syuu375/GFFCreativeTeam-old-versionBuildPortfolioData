using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DogEnemy : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Look,          //�T��
        Move,          //�ǐ�
        Wait,          //���˗p��(����Ȃ�)
        Attack,         //����
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private Rigidbody _rb;
    private Dog_Attack _attack;

    private float _jumpPower = 7f;

    private bool _isGrounded;
    private bool _moveStop = false;

    public int _direction = 0;

    private bool _iswait;//wait�R���[�`���̏d����h��
    private bool _ismove;//move�R���[�`���̏d����h��
    private bool _isattack;//attack�R���[�`���̏d����h��

    private float _fallTime;
    private float _fallSpeed;

    private bool _isRight = false;
    private bool _isLeft = false;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Dog_Attack>();
    }

    private void FixedUpdate()
    {
        if(_isRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _isRight = false;
        }
        else if(_isLeft)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            _isLeft = false;
        }
        

        Vector3 velocity = _rb.linearVelocity;
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 13f)
                {
                    Debug.Log("����");
                    _state = EnemyState.Wait;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) > 17f)
                {
                    _state = EnemyState.Look;
                }
                Direction();
                if(!_ismove)//�R���[�`���̏d���h��
                    StartCoroutine(Move());
                break;

            case EnemyState.Wait:
                Direction();
                if(!_iswait)//�R���[�`���̏d���h��
                    StartCoroutine(Wait());//���΂炭�҂���Move��
                break;

            case EnemyState.Attack:
                if(!_isattack)//�R���[�`���̏d���h��
                    StartCoroutine(Attack());
                _state = EnemyState.Wait;
                break;
        }

        if (!_isGrounded)
        {
            _fallTime += Time.deltaTime;

            _fallSpeed = Physics.gravity.y * _fallTime * 5f * 5f; //Unity�̕W���d�͂ɔC�������Ȃ� fallSpeed �͕s�v

            velocity.y += _fallSpeed * Time.fixedDeltaTime;
        }
        else
        {
            //velocity.x = 0f;
        }
            _rb.linearVelocity = velocity;
    }

    private void Look()
    {
        _direction = 0;
    }

    private void Direction()
    {
        if(!_isGrounded)
        {
            return;
        }

        if (_moveStop)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_direction == -1 || _direction == 0)
            {
                _isRight = true;
                Debug.Log("right");
                StartCoroutine(Waitturn(1));
            }
            _direction = 1;
        }
        else
        {
            if (_direction == 1 || _direction == 0)
            {
                _isLeft = true;
                Debug.Log("left");
                StartCoroutine(Waitturn(-1));
            }
            
            _direction = -1;
        }

    }


    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);
        _direction = _newdirection;
        _moveStop = false;

        yield break;
    }

    private IEnumerator Move()
    {
        _ismove = true;
        //int _rand = Random.Range(1, 4);
        if (Vector3.Distance(transform.position, _player.position) > 7f)
        {
            Debug.Log("frontjump");
            _rb.AddForce(_direction * 13f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.4f);

            
            Debug.Log("�҂�");
            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                       //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.Attack;
            
            
        }
        else if(Vector3.Distance(transform.position, _player.position) <= 7f &&
            Vector3.Distance(transform.position, _player.position) > 3f)
        {
            _state = EnemyState.Attack;
        }
        else
        {
            Debug.Log("backjump");
            _rb.AddForce(_direction * -9f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.4f);

            Debug.Log("�҂�");
            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                       //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.Attack;
        }

        _ismove = false;
        yield break;
    }

    private IEnumerator Wait()
    {
        _iswait = true;
        yield return new WaitForSeconds(2f);
        _state = EnemyState.Move;
        _iswait = false;
        yield break;
    }

    private IEnumerator Attack()
    {
        _isattack = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attack");
        _attack.GunAttack();
        _isattack = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _fallTime = 0f;
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _fallTime = 0f;
            _isGrounded = false;
        }
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Look;
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
