using System.Collections;

using UnityEngine;

public class PlayerHP : MonoBehaviour ,IPlayerDamage
{
    private Player _player;
    [SerializeField] DamageEffectSO _damageEffectSO;
    private bool _isDeadArea = false;

    private int maxHP = 100;
    private int currentHP;

    // 現在のHPを他のスクリプト（UIなど）から取得するための公開プロパティ
    public int CurrentHP => currentHP; //UIHPゲージで使う　ゲームマネージャーでmaxにするためだめかも
    public int MaxHP => maxHP;//こっちもまあ使うか

    private int _HP = 0;
    private int _KNOCKS = 0;
    private float _UNB = 0f;
    public void Awake()
    {
        _player = GetComponent<Player>();
        
        
    }

    private void Start()
    {
        ApplySkillUpgrades();
        maxHP += _HP;
        currentHP = maxHP;
        Debug.Log("<color=green>" + gameObject.name + "のHPが初期化されました: " + currentHP);
    }

    private void ApplySkillUpgrades()
    {
        if (SkillManager.Instance.HasSkill(SkillType.HP1))
        {
            _HP += 20;
            Debug.Log("HPアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP2))
        {
            _HP += 30;
            Debug.Log("HPアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP3))
        {
            _HP += 50;
            Debug.Log("HPアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKS1))
        {
            _KNOCKS += 2;
            Debug.Log("ノック減アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKS2))
        {
            _KNOCKS += 2;
            Debug.Log("ノック減アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.KNOCKS3))
        {
            _KNOCKS += 2;
            Debug.Log("ノック減アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GUN1))
        {
            _UNB += 0.1f;
            Debug.Log("無敵アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GUN2))
        {
            _UNB += 0.2f;
            Debug.Log("無敵アップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.GUN3))
        {
            _UNB += 0.2f;
            Debug.Log("無敵アップ！");
        }
    }

    //ダメージを受け、HPを減少させる。
    public void TakeDamage(int damage, int knockback, int dir, string effectname, string audioname)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        AudioManager.Instance.PlaySound(audioname);

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        knockback -= _KNOCKS;
        knockback = Mathf.Max(knockback, 0);

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == effectname);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Debug.Log("<color=green>" + gameObject.name + "が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        // HPが0以下になったかチェック
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        if (damage >= 10)
        {
            if (knockback >= 0 && knockback < 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.SKnockBack(dir, knockback);
                StartCoroutine(_DamageTime(1f));
                //StartCoroutine(_StateNormal(0.5f));//しぐなる
            }
            if (knockback >= 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.BKnockBack(dir, knockback);
                StartCoroutine(_DamageTime(1.5f));
                //StartCoroutine(_StateNormal(1f));
            }
        }
        
    }

    public void TakeElectDamage(int damage, float electtime, string effectname, string audioname)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        AudioManager.Instance.PlaySound(audioname);

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == effectname);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, electtime);
        }

        Debug.Log("<color=green>" + gameObject.name + "が" + damage + "ダメージ受けました。残りHP:" + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

        _player._ChangeState(PlayerState.Other);
        _player.Stun();//vector3.zero
        StartCoroutine(_StateNormal(electtime));

        
    }

    public void TakeBanDamage(float bantime, string effectname, string audioname)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        AudioManager.Instance.PlaySound(audioname);

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == effectname);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, bantime);
        }

        StartCoroutine(_BanTime(bantime));
    }

    private IEnumerator _StateNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _player._ReturnNormal();
        yield break;
    }

    private IEnumerator _BanTime(float bantime)//ジャンプ制限
    {
        _player._isBan = true;
        yield return new WaitForSeconds(bantime);
        _player._isBan = false;
        yield break;
    }

    private IEnumerator _DamageTime(float time)
    {
        GManager.Instance.OnPlayerHit();//カメラ揺らす

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(time + _UNB);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), false);

        yield break;
    }

    // HPを回復させる。
    public void Heal(int healAmount, string effectname, string audioname)
    {
        AudioManager.Instance.PlaySound("recover");
        currentHP += healAmount;

        currentHP = Mathf.Min(currentHP, maxHP);

        Debug.Log(gameObject.name + "のHPが" + healAmount + "回復しました。残りHP: " + currentHP);
    }


    // HPが0になったときの死亡処理。（プレイヤー専用）
    public void Die()
    {
        Debug.Log("<color=green>" + gameObject.name + "は倒されました。ゲームオーバー！");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        _player._ChangeState(PlayerState.Dead);
        _player.Dead();//animとか
        StartCoroutine(GManager.Instance.DiePlayer());

        // ここにプレイヤー入力の無効化などの処理を追加
        // プレイヤーオブジェクトを非アクティブ化
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeadArea") && !_isDeadArea)//落下死亡isTrigger
        {
            _isDeadArea = true;//死亡判定2回チェック防止
            Debug.Log("Die");
            GManager.Instance.OnPlayerHit();//カメラ揺らす　GMでもどす
            currentHP = 0;
            Die();
        }
    }

    public IEnumerator ResetHP()//GManagerから復活の命令
    {
        _isDeadArea = false;
        currentHP = MaxHP;
        _player.Respawn();
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
        //   LayerMask.NameToLayer("Enemy"), false);
        //yield return new WaitForSeconds(1f);
        //_player.Dead();//Vector3.zeroにするため　後消す
        //_player._ChangeState(PlayerState.Respawn);//一旦respawnに
        //StartCoroutine(_StateNormal(1f));//standingにもどす
        yield break ;
    }
}
