using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] DamageEffectSO _damageEffectSO;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _slashPrefab;
    [SerializeField] private Transform _groundpoint;
    public GameObject _groundeffect;

    private Collider _collider;
    public Transform _slashPosition;
    private int _damage;
    private int _knockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    private bool _groundattack = false;

    private int _ATTACK= 0;
    private int _SLASH = 0;
    private bool _ELECTSLASH = false;
    private int _GROUND = 0;
    private float _GROUNDRADIUS = 0;
    private int _KNOCKP = 0;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _collider.enabled = false; // スクリプトより先に物理無効化
        ApplySkillUpgrades();
    }

    private void ApplySkillUpgrades()
    {
        if (SkillManager.Instance.HasSkill(SkillType.ATTACK1))
        {
            _ATTACK += 20;
            Debug.Log("攻撃力アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.ATTACK2))
        {
            _ATTACK += 20;
            Debug.Log("攻撃力アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.ATTACK3))
        {
            _ATTACK += 30;
            Debug.Log("攻撃力アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GROUND))
        {
            _SLASH += 30;
            _ELECTSLASH = true;
            Debug.Log("スラッシュアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GROUND))
        {
            _GROUND += 30;
            _GROUNDRADIUS += 1f;
            Debug.Log("グラウンドアタックアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKP1))
        {
            _KNOCKP += 2;
            Debug.Log("ノックバックアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKP2))
        {
            _KNOCKP += 2;
            Debug.Log("ノックバックアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKP3))
        {
            _KNOCKP += 2;
            Debug.Log("ノックバックアップ！");
        }
    }

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        hitTargets.Clear();
        _collider.enabled = true;
        _groundattack = false;
    }

    public void ColliderEnabled()
    {
        _collider.enabled = false;
    }

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[0].Damage + _ATTACK;
        _knockback = _playerAttackSO.playerAttackList[0].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[0].EffectName;
        _audioname = _playerAttackSO.playerAttackList[0].AudioName;
        _dir = dir;

        AudioManager.Instance.PlaySound("leftattack");
    }

    //問題　連続した攻撃の際、アニメーションのシグナルか何かでClear();を呼ぶ必要あり

    public void tatakitukeAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[1].Damage  + _GROUND;
        _knockback = _playerAttackSO.playerAttackList[1].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[1].EffectName;
        _audioname = _playerAttackSO.playerAttackList[1].AudioName;
        _dir = dir;
        _groundattack = true;
        AudioManager.Instance.PlaySound("groundattack");//音がおそいから

    }

    public void slashAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[2].Damage  + _SLASH;
        _knockback = _playerAttackSO.playerAttackList[2].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[2].EffectName;
        _audioname = _playerAttackSO.playerAttackList[2].AudioName;
        AudioManager.Instance.PlaySound("slash");

        _dir = dir;

        var slash = Instantiate(_slashPrefab, _slashPosition.position, Quaternion.identity);
        if(_dir < 0)
        {
            slash.transform.localScale = new Vector3(_dir*0.1f, 0.1f, 0.1f);
        }
        slash.GetComponent<Slash>().Initialize(_damage, _knockback, _dir, _effectname,_audioname,_ELECTSLASH);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            var Interface = other.GetComponent<IDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir , _audioname);//敵のインターフェース<IDamage>取得
                hitTargets.Add(other.gameObject);

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                if (attackData != null && attackData.HitEffect != null)
                {
                    var rot = (_dir < 0) ? 180f : 0f;
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.Euler(0, 0, rot));
                    //Destroy(effect, 0.2f);
                }
            }
        }

        if (other.CompareTag("Grounded") && _groundattack)
        {
            //Debug.Log("地面");
            //var G_effect = Instantiate(_groundeffect, _groundpoint.position, Quaternion.identity);
            //if (_dir < 0)
            //{
            //    G_effect.transform.localScale = new Vector3(_dir * 0.1f, 0.1f, 0.1f);
            //}
            //Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

            //Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f + _GROUNDRADIUS);
            //foreach (var hit in hits)
            //{
            //    if (hit.CompareTag("Enemy"))
            //    {
            //        // IDamageable 実装クラスへダメージ
            //        var Interface = hit.GetComponent<IDamage>();
            //        if (Interface != null) Interface.TakeDamage(_damage,_knockback,_dir, _audioname);

            //        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
            //        if (attackData != null && attackData.HitEffect != null)
            //        {
            //            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            //            Destroy(effect, 0.2f);
            //        }
            //    }
            //}
            //_groundattack = false;
        }
    }

    public void GroundAttackSignal()//グラウンドアタックをPlayerAttackシグナルでよぶ
    {
        Debug.Log("地面");
        

        var G_effect = Instantiate(_groundeffect, _groundpoint.position, Quaternion.identity);
        if (_dir < 0)
        {
            G_effect.transform.localScale = new Vector3(_dir * 0.1f, 0.1f, 0.1f);
        }
        Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

        Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f + _GROUNDRADIUS);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // IDamageable 実装クラスへダメージ
                var Interface = hit.GetComponent<IDamage>();
                if (Interface != null) Interface.TakeDamage(_damage, _knockback, _dir, _audioname);

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 0.2f);
                }
            }
        }
        _groundattack = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}