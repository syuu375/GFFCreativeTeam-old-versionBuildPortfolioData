using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using static Enemy;

public class StunEnemy : MonoBehaviour
{
    const float ATTACKTIME = 5f;

   // [SerializeField] private float test;
    //private enum EnemyState
    //{ 
    //DITECTION,
    //LOOK,
    // //WAIT,
    //ATTACK
    //}

    [SerializeField] private GameObject energyBallPrefab;
    private GameObject _myEnergyBall;   

    // private EnemyState _state = EnemyState.DITECTION;
    private Rigidbody _rb;
    private Transform _playerTransform;
    private bool _ATTACK;

    private float _attackTime;

    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _clear;
    private float _bantime;
    private string _effectname;
    private string _audioname;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransform = GameObject.FindWithTag("Player").transform;;
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("StunEnemy");
        if (attackData != null)
        {

            _bantime = attackData.Bantime;
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_playerTransform.position, _rb.position) < 20f)
        {
            Look();
        }
        else
        {
            _attackTime = 0f;
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

    }

    private void Look()
    {
        _attackTime += Time.deltaTime;

        float Angle = GetAngle(transform.position,_playerTransform.position);
        Debug.Log(Angle);
        transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        if (_attackTime >= ATTACKTIME)
        {
            _attackTime = 0f;
            Attack();
        }
        
    }

    private void Attack()
    {
        _myEnergyBall = Instantiate(energyBallPrefab,transform.position,transform.rotation);
        _myEnergyBall.GetComponent<StunEnergy>().Initialize(_bantime, _effectname, _audioname);
    }

    float GetAngle(Vector3 my,Vector3 target)
    {
        Vector3 dt = target - my;
        float rad =Mathf.Atan2(dt.y,dt.x);
        float degree= rad *Mathf.Rad2Deg;

        return degree;
    }

}
