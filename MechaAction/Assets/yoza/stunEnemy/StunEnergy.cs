using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))] 
public class StunEnergy : MonoBehaviour
{
    private Rigidbody _rb;
    private float _speed = 3f;

    private float _bantime;
    private string _effectname;
    private string _audioname;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.right * _speed;

        
    }

    public void Initialize(float bantime, string effectname, string audioname)
    {
        _bantime = bantime;
        _effectname = effectname;
        _audioname = audioname;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var Interface = other.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeBanDamage(_bantime, _effectname, _audioname);//�G�̃C���^�[�t�F�[�X<IDamage>�擾
            }
        }
    }
}