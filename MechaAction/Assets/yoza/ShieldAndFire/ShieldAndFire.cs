
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShieldAndFire : MonoBehaviour
{

    const float ATTACKTIME = 3f;
    const float SHIELDTIME = 3f;
    const float GRAVITY = -9.81f;

    private enum EnemyState
    {
        DITECTION,
        MOVE,
        ATTACK,
        SHIELD,
        DESTROY,
    };

    private Rigidbody _rb;
    private Transform _playerTransfrom;
    private float _distance;
    private EnemyState state;
    private int _dir;
    private float _moveSpeed = 5f;
    private float _gravityTime;
    private bool _isGround;
    [SerializeField] Vector3 _velocity;
    [SerializeField] private GameObject FlameThrower;
    [SerializeField] private GameObject _player;




    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransfrom = GameObject.FindWithTag("Player").transform;

    }
    private void Start()
    {
        state = EnemyState.DITECTION;
        _velocity = _rb.linearVelocity;
        _gravityTime = 0;
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_playerTransfrom.position, _rb.position);

        if (!(_isGround))
        {
            _gravityTime += Time.deltaTime;
            _velocity.y = GRAVITY * _gravityTime;

        }
        else
        {
            _gravityTime = 0;
            _velocity.y = 0;

            switch (state)
            {
                case EnemyState.DITECTION:
                    Ditection();
                    break;

                case EnemyState.MOVE:
                    Move();
                    break;

                case EnemyState.ATTACK:
                    _ATKtime += Time.deltaTime;
                    Attack();
                    if (_ATKtime >= ATTACKTIME)
                    {
                        _ATKtime = 0;
                            state = _distance >= 4f ? EnemyState.MOVE : EnemyState.SHIELD;
                    }
                    break;

                case EnemyState.SHIELD:
                    _SHItime += Time.deltaTime;
                    Shield();
                    if(_SHItime >= ATTACKTIME)
                    {
                        _SHItime = 0;
                        state = _distance >= 4f ? EnemyState.MOVE : EnemyState.ATTACK;
                    }
                    break;

                case EnemyState.DESTROY:
                    break;
            }
        }
    }

    private void Ditection()
    {
        if (_distance < 20)
        {
            state = EnemyState.MOVE;
            return;
        }
    }

    private void Move()
    {
        

        _dir = (_rb.position.x < _playerTransfrom.position.x) ? 1 : -1;
        
        
            _velocity.x = _dir * _moveSpeed;

        if (_distance < 4f)
        {
            _velocity.x = 0;
                state = EnemyState.ATTACK;
        }

        _rb.linearVelocity = _velocity;

    }

    private float _ATKtime;
    private void Attack()
    {
        if (_ATKtime <= ATTACKTIME)
            Debug.Log("FlameThrower");
        else
            return;
    }

    private float _SHItime;
    private void Shield()
    {
        if (_SHItime <= ATTACKTIME)
            Debug.Log("SHIELD");
        else
            return;
    }

    private void Destroy()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _isGround = true;
            Debug.Log("isGround");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _isGround = false;
        }
    }

    
}
