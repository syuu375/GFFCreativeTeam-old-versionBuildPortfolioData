using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GunHitbox : MonoBehaviour
{
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] DamageEffectSO _damageEffectSO;

    [SerializeField] GameObject _bulletPrefab;
    public Transform _bulletPosition;
    [SerializeField] private Transform _muzzlepoint;
    public GameObject _guneffect;
    public LayerMask ignoreLayer;

    private int _damage;
    private int _knockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    private int _GUN = 0;
    private float _RAY = 0f;
    private int _SHOTGUN = 0;
    private float _SHOTGUNRADIUS = 0f;
    private int _RIFLE = 0;
    private int _KNOCKP = 0;

    private void Start()
    {
        ApplySkillUpgrades();
    }

    private void ApplySkillUpgrades()
    {
        if (SkillManager.Instance.HasSkill(SkillType.GUN1))
        {
            _GUN += 20;
            _RAY += 1f;
            Debug.Log("銃アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GUN2))
        {
            _GUN += 20;
            _RAY += 1f;
            Debug.Log("銃アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GUN3))
        {
            _GUN += 30;
            _RAY += 1f;
            Debug.Log("銃アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.SHOTGUN))
        {
            _SHOTGUN += 30;
            _SHOTGUNRADIUS += 2f;
            Debug.Log("ショットガンアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.RIFLE))
        {
            _RIFLE += 30;
            Debug.Log("ライフルアップ！");
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

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[3].Damage + _GUN;
        _knockback = _playerAttackSO.playerAttackList[3].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[3].EffectName;
        _audioname = _playerAttackSO.playerAttackList[3].AudioName;
        _dir = dir;

        var G_effect = Instantiate(_guneffect, _muzzlepoint.position, Quaternion.identity);
        Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

        AudioManager.Instance.PlaySound("leftgun");

        float dis = 10f + _RAY;
        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, dis,~ignoreLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var Interface = hit.collider.GetComponent<IDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir, _audioname);//敵のインターフェース<IDamage>取得

                    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                    if (attackData != null && attackData.HitEffect != null)
                    {
                        var effect = Instantiate(attackData.HitEffect, hit.transform.position, Quaternion.identity);
                        Destroy(effect, 0.2f);
                    }
                }
            }
        }
    }

    public void ShotGun(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[4].Damage  + _SHOTGUN;
        _knockback = _playerAttackSO.playerAttackList[4].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[4].EffectName;
        _audioname = _playerAttackSO.playerAttackList[4].AudioName;
        _dir = dir;

        var G_effect = Instantiate(_guneffect, _muzzlepoint.position, Quaternion.identity);
        Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

        AudioManager.Instance.PlaySound("shotgun");

        float dis = 3f + _RAY;
        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        if (Physics.BoxCast(transform.position, Vector3.one * (0.5f + _SHOTGUNRADIUS), transform.forward, out RaycastHit hit,Quaternion.identity, dis,~ignoreLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var Interface = hit.collider.GetComponent<IDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir, _audioname);//敵のインターフェース<IDamage>取得

                    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                    if (attackData != null && attackData.HitEffect != null)
                    {
                        var effect = Instantiate(attackData.HitEffect, hit.transform.position, Quaternion.identity);
                        Destroy(effect, 0.2f);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 halfExtents = Vector3.one * (0.5f + _SHOTGUNRADIUS);
        float dis = 3f + _RAY; // ← BoxCastのdisと合わせる
        Vector3 direction = transform.forward;

        // 始点と終点の中心を描画
        Vector3 start = transform.position;
        Vector3 end = start + direction * dis;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(start, halfExtents * 2); // 始点（Boxのサイズ）

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(end, halfExtents * 2);   // 終点（Boxのサイズ）
    }

    public void Rifle(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[5].Damage  + _RIFLE;
        _knockback = _playerAttackSO.playerAttackList[5].Knockback + _KNOCKP;
        _effectname = _playerAttackSO.playerAttackList[5].EffectName;
        _audioname = _playerAttackSO.playerAttackList[5].AudioName;
        _dir = dir;

        AudioManager.Instance.PlaySound("rifle");


        GameObject bullet = Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(_damage, _knockback, _dir, _effectname, _audioname);
    }
}
