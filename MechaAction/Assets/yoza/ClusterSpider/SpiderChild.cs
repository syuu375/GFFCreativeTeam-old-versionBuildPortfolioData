using System.Collections;
using UnityEngine;
using Cooltime;


[RequireComponent(typeof(Rigidbody))]
public class SpiderChild : MonoBehaviour
{
    private CoolDown coolDown = new CoolDown();
    private Rigidbody _rb;
    Vector3 _velocity;
    private Transform _player;
    [SerializeField]private Vector3 _PerentPos;
    private int _dir;
    private float _moveSpeed = 5f;
    private float _limittime;
    private int tmp;
    [SerializeField]private bool isbomb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        _limittime = 0.5f;
    }


    

    private void FixedUpdate()
    {
        coolDown.DiraySkill(1f);
            Move();
        if (Vector3.Distance(_rb.position, _PerentPos) >= 10)
            Boom();
    }
    private void Move()  
    {
 

       tmp = _dir;

       _dir =  (_rb.position.x < _player.position.x )? 1 : -1;

        if (tmp != _dir)
        {
            _velocity.x = 0;
            return;
        }
        else
        {
            _velocity.x = _dir * _moveSpeed;
        }
        
        if (Vector3.Distance(_player.position, _rb.position) < 3f)
        {
            if((_limittime -= Time.deltaTime) <0)
            {
                Boom();
            }
        }
        else
        {
            _limittime = 0.5f;
        }

        if (Vector3.Distance(_player.position, _rb.position) < 1.15f)
        {
            _velocity = Vector3.zero;
        }
        _rb.linearVelocity = _velocity;
    }

    Coroutine _thiscoroutine;
    private void Boom()
    {
        if (_thiscoroutine == null)
        {
            _thiscoroutine =
            StartCoroutine(coolDown.Skill(callback => { _thiscoroutine = callback; }, 3f,boom));
        }
        
    }

    private void boom()
    {
        Debug.Log("BOOOOOOOM");
        Collider[] hitcolliders = Physics.OverlapSphere(_rb.position, 3f);

        foreach (var hit in hitcolliders)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("3�_���[�W");
            }
        }

        SpiderMother.GenerateCount--;
        Destroy(gameObject, 0.5f);
    }
}
    




