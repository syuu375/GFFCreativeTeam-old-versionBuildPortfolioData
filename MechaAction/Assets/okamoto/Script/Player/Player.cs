using System.Collections;

using UnityEngine;


public enum PlayerState
{
    Standing,    //�����A�����A�W�����v
    Dash,
    Attack,
    Knockback,
    Other,//�M�~�b�N�A�d�C�A�W�Q
    Dead,       //���񂾂��ԑJ�ږh�~
    Respawn     //�B��Dead�����蔲����
}

public class Player : MonoBehaviour
{

    private PlayerState _state = PlayerState.Standing;
    private PlayerState state
    {
        get => _state;
        set
        {
            if (IsDead&& value != PlayerState.Respawn)//Dead����Respawn�ȊO
            {
                return;
            }
                _state = value;
        }
    }

    public bool IsDead => _state == PlayerState.Dead;

    public bool CanMove => _state == PlayerState.Standing;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private Animator _anim;

    private Vector2 _moveVector;
    private Vector2 _inputVector;

    private Vector3 velocity;

    private bool _isGrounded;
    private bool _isJump = false;
    private bool _isSecondJump;
    private bool _isRun = false;
    private int _Runcount = 0;
    public int _lookDir;
    private float prevHorizontal = 0f;
    private bool _isDash = false;//�_�b�V�����������ς��Ȃ��悤��
    private bool _canDash = true;//�󒆂łQ��ڃ_�b�V����h������
    public bool _isBan= false;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    Vector3 origin;

    private float _fallTime;

    private bool _isRight = false;
    private bool _isLeft = false;

    private float _SPEED = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ApplySkillUpgrades();
    }
    private void ApplySkillUpgrades()
    {
        if (SkillManager.Instance.HasSkill(SkillType.SHOTGUN))
        {
            _SPEED += 0.1f;
            Debug.Log("�X�s�[�h�A�b�v�I");
        }
        if (SkillManager.Instance.HasSkill(SkillType.RIFLE))
        {
            _SPEED += 0.2f;
            Debug.Log("�X�s�[�h�A�b�v�I");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKP1))
        {
            _SPEED += 0.2f;
            Debug.Log("�X�s�[�h�A�b�v�I");
        }
    }

    private void Update()
    {
        _MousePosition();//�}�E�X�̈ʒu�擾
        
        _InputDetection();//�_�b�V���̂Q����͌��m

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;

        }

        if (Input.GetMouseButtonDown(1) && _canDash)
        {
            _anim.SetFloat("Speed", 3);
            StartCoroutine(_Dash());
            _canDash = false;
        }

        Vector2 bounds = _col.bounds.size;
        RaycastHit hit;
        origin = transform.position + Vector3.down * (bounds.y / 2);
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1.5f, LayerMask.GetMask("Grounded"));

        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        //Debug.Log(velocity.x);
        //Debug.Log(_isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1.5f, 0.4f);
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        if (_state == PlayerState.Dash) return;

        velocity = _rb.linearVelocity; //��x�ϐ��ɃR�s�[���Ă���ҏW
        _moveVector.x = _inputVector.x; //�����ɏ������Ƃŋ󒆂ō��E�Ɉړ��\

        _Move();

        _Jump();

        #region�@�����A�j���[�V����
        if(_isGrounded)
        {
            //Debug.Log("moveanimation");
            if (velocity.x >= -1 && velocity.x <= 1)
            {
                _anim.SetFloat("Speed", 0);
                //Debug.Log("Speed0");
            }
            else if ((velocity.x > 1 && velocity.x <= 4) || (velocity.x < 1 && velocity.x >= -4))
            {
                _anim.SetFloat("Speed", 1);
                //Debug.Log("Speed1");
            }
            else
            {
                _anim.SetFloat("Speed", 2);
                //Debug.Log("Speed2");
            }
        }
        else
        {
            //if(velocity.y > 0)
            //{
            //    _anim.SetInteger("JumpSpeed", 0);
            //}
            //else
            //{
            //    _anim.SetInteger("JumpSpeed", 1);
            //}
        }

        #endregion

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0f;
        }

        _rb.linearVelocity = velocity; //�ҏW�����l��߂���rigidbody�Ŏ��s
    }

    public void _ChangeState(PlayerState newState)
    {
        state = newState;
        //Debug.Log("state��" + newState + "�ɕς����");
        if(newState == PlayerState.Attack && _isGrounded)
        {
            //Debug.Log("chengestate�������~�߂�");
            _rb.linearVelocity = Vector3.zero;
        }
    }

    public void _ReturnNormal()
    {
        state = PlayerState.Standing;
    }

    private void _MousePosition()
    {
        // �}�E�X�̃X�N���[�����W���擾
        Vector3 mousePos = Input.mousePosition;

        //Debug.Log($"Screen Position: X={mousePos.x}, Y={mousePos.y}");

        // ���[���h���W�ɕϊ��i�J�����K�{�j
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, 10f)//Camera.main.nearClipPlane
        );


        if (transform.position.x < worldPos.x && !_isDash)
        {
            _lookDir = 1;
            _isRight = true;
        }
        else if (transform.position.x > worldPos.x && !_isDash)
        {
            _lookDir = -1;
            _isLeft = true;
        }
    }

    private void _InputDetection()
    {
        if (prevHorizontal == 0f && (_inputVector.x == 1 || _inputVector.x == -1))//_inputVector�̉����u�Ԃ��擾���邽��
        {
            if (_Runcount <= 0)
            {
                _Runcount = 150;
            }
            else
            {
                _isRun = true;
                //Debug.Log("Dash!");
            }
        }

        if (_Runcount > 0)
        {
            _Runcount--;
        }
        if (_inputVector.x == 0)
        {
            _isRun = false;
            //Debug.Log("nodash");
        }

        prevHorizontal = _inputVector.x;
    }

    private void _Move()
    {
        if(!CanMove) return;

        if (_lookDir == 1)
        {
            if (_isRun && _moveVector.x > 0f)
            {
                velocity.x = _moveVector.x * (_moveSpeed + _SPEED);
            }
            else
            {
                velocity.x = _moveVector.x * (_moveSpeed + _SPEED) * 0.5f;
            }
        }
        else if (_lookDir == -1)
        {
            if (_isRun && _moveVector.x < 0f)
            {
                velocity.x = _moveVector.x * (_moveSpeed + _SPEED);
            }
            else
            {
                velocity.x = _moveVector.x * (_moveSpeed + _SPEED) * 0.5f;
            }
        }
        
    }

    private void _Jump()
    {
        if (!CanMove) return;
        if (_isBan) return;//PlayerHP�ŊǗ�����bool

        if (_isGrounded)
        {
            if (_isJump)
            {
                _anim.SetTrigger("Jump");
                _fallTime = 0f;
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isJump = false;
            }
            _isSecondJump = true;
            _canDash = true;
        }
        else
        {
            if (_isSecondJump && _isJump)
            {
                velocity.y = 0f;//��i�ڂŒ��ˏオ��h��
                _fallTime = 0f;
                _anim.SetTrigger("Jump");
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isSecondJump = false;
            }
            if (_isJump)
            {
                _isJump = false;
            }
        }
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        //_moveVector.y = Physics.gravity.y * _FallTime * 2.0f;�@//���ڒl��ς��Ă��܂��̂Ŏ��̃t���[���łO�ɖ߂��Ă��܂�
        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unity�̕W���d�͂ɔC�������Ȃ� fallSpeed �͕s�v

        //Debug.Log(_fallSpeed);

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y���x�ɏ��X�ɉ��Z
                                                        //Time.fixedDeltaTime �������Z���t���[�����[�g�Ɉˑ������Ȃ����ߕK�{
        if (velocity.y < -20f)//�������x�̐���
        {
            velocity.y = -20f;
        }
    }

    private IEnumerator _Dash()
    {
        if(!CanMove) yield break;

        _ChangeState(PlayerState.Dash);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        _isDash = true;
        Vector3 velocity = _rb.linearVelocity;
        _fallTime = 0f;

        float t = 0f;
        float duration = 0.2f;
        while (t < duration)
        {
            velocity = _rb.linearVelocity;
            if (_lookDir == 1)
            {
                velocity.x = _lookDir * 15f;
                velocity.y = 0f;
            }
            else if (_lookDir == -1)
            {
                velocity.x = _lookDir * 15f;
                velocity.y = 0f;
            }
            //Debug.Log(velocity.x);
            _rb.linearVelocity = velocity;
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();  //�R���[�`������FixedUpdate�ł����AI�Œm����
        }
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), false);
        _ReturnNormal();
        _isDash = false;
        yield break;
    }

    public void SKnockBack(int dir,int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _anim.SetTrigger("SKnock");
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _anim.SetTrigger("BKnock");
    }

    public void Stun()//�d���_���[�W�ŌĂ�
    {
        _rb.linearVelocity = Vector3.zero;
    }


    public void Dead()
    {
        _rb.linearVelocity = Vector3.zero;
        _anim.SetInteger("Dead", 1);
    }

    public void Respawn()
    {
        _anim.SetInteger("Dead", 0);

    }

    private IEnumerator Gimmick()
    {
        yield return new WaitForSeconds(0.8f);
        _ReturnNormal();

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpGimmick"))
        {
            _fallTime = 0f;
            _rb.linearVelocity = Vector3.zero;
            _ChangeState(PlayerState.Other);
            StartCoroutine(Gimmick());
            _rb.AddForce(0f,23f,0f, ForceMode.Impulse);
        }

        //if (collision.gameObject.CompareTag("Grounded"))
        //{
        //    Debug.Log("ground");
        //    _fallTime = 0f;
        //    _isGrounded = true;
        //}
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Grounded"))
    //    {
    //        _fallTime = 0f;
    //        _isGrounded = false;
    //    }
    //}
}
