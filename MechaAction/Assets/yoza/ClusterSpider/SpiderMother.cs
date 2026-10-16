using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Cooltime;

public class SpiderMother : MonoBehaviour
{
    const float GENERATETIME = 5f;
    const int GENERATECOUNT = 3;

    CoolDown _coolDown = new CoolDown();
    
    
    static public int GenerateCount = 0;

    [SerializeField] private GameObject Bombspider;
    
    private Rigidbody _rb;
    private Transform _player;
    Vector3 _velocity;

    private int _dir;
    
    private float _moveSpeed = 5f;
    private float _time;
    private float _distance;
    [SerializeField]private float _ditection;       //���G�͈͂��m�肵����const�ɕύX
    [SerializeField] private float _stiffTime;      //�d�����Ԃ��m�肵����const�ɕύX

    private float _generateTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
        _coolDown.DiraySkill(5f);
    }

    private void FixedUpdate()
   {
        _distance = Vector3.Distance(_rb.position, _player.position);
        
        if (_distance<= _ditection)
        {
            if ((_time += Time.deltaTime) >= _stiffTime)
            {
                Move();
            }
        }
   }
    private void Move()
    {

        _velocity = _rb.linearVelocity;
        _generateTime +=Time.deltaTime;

        int dirBefore = _dir;

        _dir = (_rb.position.x > _player.position.x) ? 1 : -1;

        if (dirBefore != _dir)
        {
            _velocity.x = 0;
            _time = 0;
            return;
        }

        else
        {
            _velocity.x = _dir * _moveSpeed;
        }
        
        _rb.linearVelocity = _velocity;

        if (_generateTime>=GENERATETIME)
        {
            _generateTime = 0f;
            
            if (GenerateCount < GENERATECOUNT)
            {
                Generate();
                _time = 0;
                return;
            }
        }
    }
    private void Generate()
    {
        _rb.linearVelocity = Vector3.zero;
        var _Bombspider = Instantiate(Bombspider, transform.position, transform.rotation);
        GenerateCount++;
    }
   
}
