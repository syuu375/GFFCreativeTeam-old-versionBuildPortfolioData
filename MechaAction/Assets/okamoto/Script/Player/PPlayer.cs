using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _moveVector;

    private Vector2 _inputVector;

    private bool _isGrounded;
    private bool _isJump = false;
    private bool _isSecondJump;
    private bool _isRun = false;
    private int _Runcount = 0;
    private int _lookDir;
    private float prevHorizontal = 0f;
    private bool _isDash = false;
    private bool _canDash = true;//�󒆂łQ��ڃ_�b�V����h������

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    private float _fallTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // �}�E�X�̃X�N���[�����W���擾
        Vector3 mousePos = Input.mousePosition;

        //Debug.Log($"Screen Position: X={mousePos.x}, Y={mousePos.y}");

        // ���[���h���W�ɕϊ��i�J�����K�{�j
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y,10f )//Camera.main.nearClipPlane
        );
        

        if(transform.position.x  < worldPos.x && !_isDash)
        {
            _lookDir = 1;
        }
        else if (transform.position.x > worldPos.x && !_isDash)
        {
            _lookDir = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;
    
        }


        if (prevHorizontal == 0f && _inputVector.x == 1)//_inputVector�̉����u�Ԃ��擾���邽��
        {
            if(_Runcount <= 0)
            {
                _Runcount = 150;
            }
            else
            {
                _isRun = true;
                //Debug.Log("Dash!");
            }
        }

        if (prevHorizontal == 0f && _inputVector.x == -1)//_inputVector�̉����u�Ԃ��擾���邽��
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
        if(_inputVector.x == 0)
        {
            _isRun = false;
            //Debug.Log("nodash");
        }

        prevHorizontal = _inputVector.x;

        if(Input.GetMouseButtonDown(1) && _canDash)
        {
            _isDash = true;
            _anim.SetFloat("Speed",3);
            StartCoroutine(_Dash());
            _canDash = false;
        }
         
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        if (_isDash)
        {
            return;
        }

        Vector3 velocity = _rb.linearVelocity; //��x�ϐ��ɃR�s�[���Ă���ҏW
        _moveVector.x = _inputVector.x; //�����ɏ������Ƃŋ󒆂ō��E�Ɉړ��\
        
        
        if(_moveVector.x == 0)//�����A�j���[�V����
        {
           // _anim.SetFloat("Speed", 0);

        }

        if (velocity.x >= -1 && velocity.x <= 1)
        {
            _anim.SetFloat("Speed", 0);
            //Debug.Log("Speed0");
        }
        else if((velocity.x > 1 && velocity.x <= 4)|| (velocity.x < 1 && velocity.x >= -4))
        {
            _anim.SetFloat("Speed", 1);
            //Debug.Log("Speed1");
        }
        else
        {
            _anim.SetFloat("Speed", 2);
            //Debug.Log("Speed2");
        }

        if (_lookDir == 1)
        {
            if (_isRun && _moveVector.x > 0f)
            {
                velocity.x = _moveVector.x * _moveSpeed;
            }
            else
            {
                velocity.x = _moveVector.x * _moveSpeed * 0.5f;
            }
        }
        else if (_lookDir == -1)
        {
            if (_isRun && _moveVector.x < 0f)
            {
                velocity.x = _moveVector.x * _moveSpeed;
            }
            else
            {
                velocity.x = _moveVector.x * _moveSpeed * 0.5f;
            }
        }


        if (_isGrounded)
        {

            if( _isJump)
            {
                _rb.AddForce(0f, _jumpPower, 0f,ForceMode.Impulse);
                _isJump = false;
            }
            _isSecondJump = true;
            _canDash = true;
        }
        else
        {
            if (_isSecondJump && _isJump)
            {
                velocity.y = 0f;
                _fallTime = 0f;
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isSecondJump = false;
            }
            if(_isJump)
            {
                _isJump = false;
            }

            _fallTime += Time.deltaTime;

            //_moveVector.y = Physics.gravity.y * _FallTime * 2.0f;�@//���ڒl��ς��Ă��܂��̂Ŏ��̃t���[���łO�ɖ߂��Ă��܂�
            float _fallSpeed = Physics.gravity.y * _fallTime * 2f*2f; //Unity�̕W���d�͂ɔC�������Ȃ� fallSpeed �͕s�v

            //Debug.Log(_fallSpeed);

            velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y���x�ɏ��X�ɉ��Z
                                                           //Time.fixedDeltaTime �������Z���t���[�����[�g�Ɉˑ������Ȃ����ߕK�{
            if(velocity.y < -20f)//�������x�̐���
            {
                velocity.y = -20f;
            }
        }


        //_rb.velocity = _moveSpeed * _moveVector;
        //Debug.Log(velocity.x);
        _rb.linearVelocity = velocity; //�ҏW�����l��߂���rigidbody�Ŏ��s
    }

    private IEnumerator _Dash()
    {
        _isDash = true;
        Vector3 velocity = _rb.linearVelocity;
        _fallTime = 0f;

        float t = 0f;
        float duration = 0.2f;
        while(t < duration)
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
        _isDash = false;
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
}
