using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrone_Normal : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _clear;
    private int _hitdamage;
    private int _hitknockback;
    private int _dir;
    private string _effectname;
    private string _audioname;

    private float _horizontalSpeed = 3f;   // ���ւ̈ړ����x
    private float _verticalAmplitude = 5f; // �㉺�̐U�ꕝ
    private float _verticalSpeed = 3f; // �㉺�̑���

    private Transform _player;
    private Rigidbody _rb;
    private float _startY;
    private float _timeOffset;
    private int dir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("BoxDrone_Normal");
        if (attackData != null)
        {
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }

        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//�E
            dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//��
            dir = -1;
        }

        _startY = transform.position.y;
        _timeOffset = Random.Range(0f, Mathf.PI * 2f);  // �G���Ƃɓ��������炷
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

        if (!CanMove) return;
        float newY = _startY + Mathf.Sin(Time.time * _verticalSpeed + _timeOffset) * _verticalAmplitude;
        Vector3 newVelocity = new Vector3(_horizontalSpeed * dir, (newY - transform.position.y) / Time.fixedDeltaTime, 0f);
        _rb.linearVelocity = newVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var Interface = other.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾
            }
        }
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Move;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(2f));
        //anim
    }

    public void ElectStun(int dir, int knockback,float electtime)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
}
