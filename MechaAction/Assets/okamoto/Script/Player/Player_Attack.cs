using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using Cooltime;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private GameObject Sword;
    [SerializeField] private GameObject HandGun;
    [SerializeField] private GameObject ShotGun;
    [SerializeField] private SwordHitbox _sword;
    [SerializeField] private GunHitbox _gun;
    private SkillCoolTimeUI _ui;

    private Animator _anim;
    private Player _player;
    private int _dir;

    private CoolDown coolDown = new CoolDown();

    private Coroutine _tatakitukecoroutine;
    private Coroutine _shotguncoroutine;
    private Coroutine _slashcoroutine;
    private Coroutine _riflecoroutine;


    private enum PlayerAttackType {
        Sowd,
        Gun
    }

    private PlayerAttackType _state = PlayerAttackType.Sowd;

    private float _SKILL = 0f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _ui = FindFirstObjectByType<SkillCoolTimeUI>();
    }

    private void Start()
    {
        _sword.enabled = false;
        ApplySkillUpgrades();
    }

    private void ApplySkillUpgrades()
    {
        if (SkillManager.Instance.HasSkill(SkillType.HP1))
        {
            _SKILL += 0.1f;
            Debug.Log("スキルアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP2))
        {
            _SKILL += 0.2f;
            Debug.Log("スキルアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP3))
        {
            _SKILL += 0.3f;
            Debug.Log("スキルアップ！");
        }
    }

    private void Update()
    {
        if(_player.IsDead) return;

        switch (_state)
        {
            case PlayerAttackType.Sowd:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _state = PlayerAttackType.Gun;
                    HandGun.SetActive(true);
                    Sword.SetActive(false);
                    //Debug.Log("GunMode");
                }

                break;
            case PlayerAttackType.Gun:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _state = PlayerAttackType.Sowd;
                    Sword.SetActive(true);
                    HandGun.SetActive(false);
                    ShotGun.SetActive(false);
                    //Debug.Log("SowdMode");
                }

                break;
        }

        _dir = _player._lookDir;

        if(Input.GetMouseButtonDown(0))
        {
            LeftAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            tatakituke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            slash();
        }
    }

    public void LeftAttack()
    {
        if(!_player.CanMove) return;

        _player._ChangeState(PlayerState.Attack);

        if(_state == PlayerAttackType.Sowd)
        {

            _sword.enabled = true;

            _anim.SetInteger("AttackType", 0);
            _anim.SetTrigger("Attack");
            //_anim.ResetTrigger("Attack");

            _sword.leftAttack(_dir);

        }
        else if(_state == PlayerAttackType.Gun)
        {
            HandGun.SetActive(true);
            ShotGun.SetActive(false);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");
            _gun.leftAttack(_dir);

            _player._ReturnNormal();//シグナルできなかった...
        }
    }

    //[SerializeField]bool test = false;


    public void tatakituke()
    {
        if (_state == PlayerAttackType.Sowd)
        {
            if (!_player.CanMove || _tatakitukecoroutine != null) return;
            _player._ChangeState(PlayerState.Attack);

            float cooltime = 3f - _SKILL;
            _ui.GroundSkillCoolTime(cooltime);

            //test = false;
            _sword.enabled = true;
            _anim.SetInteger("AttackType", 1);
            _anim.SetTrigger("Attack");
            _tatakitukecoroutine = StartCoroutine(
                coolDown.Skill(callback => { _tatakitukecoroutine = callback; },
                cooltime,
                null,
                _sword.tatakitukeAttack,
                0,
                _dir));
            // Debug.Log((int)skill.Current);
            //_sword.tatakitukeAttack(_dir);
            return;
        }
        else if(_state == PlayerAttackType.Gun)
        {
            if (!_player.CanMove || _shotguncoroutine != null) return;
            _player._ChangeState(PlayerState.Attack);
            HandGun.SetActive(false);
            ShotGun.SetActive(true);

            float cooltime = 3f - _SKILL;
            _ui.ShotgunSkillCoolTime(cooltime);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            _shotguncoroutine = StartCoroutine(
               coolDown.Skill(callback => { _shotguncoroutine = callback; },
               cooltime,
               null,
               _gun.ShotGun,
               0,
               _dir));

            //_gun.ShotGun(_dir);
            _player._ReturnNormal();
        }  
    }

    public void slash()
    {
        if (_state == PlayerAttackType.Sowd)
        {
            if (!_player.CanMove || _slashcoroutine != null) return;
            _player._ChangeState(PlayerState.Attack);

            float cooltime = 3f - _SKILL;
            _ui.SlashSkillCoolTime(cooltime);

            _sword.enabled = true;
            _anim.SetTrigger("Attack");
            _anim.SetInteger("AttackType", 2);
            _slashcoroutine = StartCoroutine(
                coolDown.Skill(callback => { _slashcoroutine = callback; },
                cooltime,
                null,
                _sword.slashAttack,
                0,
                _dir));
            //_sword.slashAttack(_dir);
        }
        else if (_state == PlayerAttackType.Gun)
        {
            if (!_player.CanMove || _riflecoroutine != null) return;
            _player._ChangeState(PlayerState.Attack);
            HandGun.SetActive(false);
            ShotGun.SetActive(true);

            float cooltime = 3f - _SKILL;
            _ui.RifleSkillCoolTime(cooltime);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            _riflecoroutine = StartCoroutine(
               coolDown.Skill(callback => { _riflecoroutine= callback; },
               cooltime,
               null,
               _gun.Rifle,
               0,
               _dir));
            //_gun.Rifle(_dir);
            _player._ReturnNormal();
        }
    }

    /*private IEnumerator Enabled()
    {
        sword.enabled = true;
        yield return new WaitForSeconds(2f);
        sword.enabled = false;
        yield break;
    }*/
    public void _Enabletfalse()//animationシグナルで呼ぶ
    {
        _sword.ColliderEnabled();//collider false
        //Debug.Log("falswe");
        _sword.enabled = false;

        _player._ReturnNormal();//最後に呼ぶ
    }

    public void GroundAttack()
    {
        _sword.GroundAttackSignal();
    }

    //public void _Enabletrue()//animationシグナルで呼ぶ
    //{
    //    _sword.enabled = true;//意味ないかも
    //}
}