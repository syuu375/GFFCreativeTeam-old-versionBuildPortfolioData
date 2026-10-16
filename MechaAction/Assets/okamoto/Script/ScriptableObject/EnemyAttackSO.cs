using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public List<EnemyAttack> enemyAttackList = new List<EnemyAttack>();
    private Dictionary<string, EnemyAttack> _effectDictionary;

    [System.Serializable]
    public class EnemyAttack
    {
        [SerializeField] string enemyname;
        [SerializeField] int damage;
        [SerializeField] int knockback;
        [SerializeField] int hitdamage;
        [SerializeField] int hitknockback;
        [SerializeField] float electtime;
        [SerializeField] float bantime;
        [SerializeField] string effectname;
        [SerializeField] string audioname;

        public string Enemyname { get => enemyname; }
        public int Damage { get => damage; }
        public int Knockback { get => knockback; }
        public int Hitdamage { get => hitdamage; }
        public int Hitknockback { get => hitknockback; }
        public float Electtime { get => electtime; }
        public float Bantime { get => bantime; }
        public string EffectName { get => effectname; }
        public string AudioName { get => audioname; }
    }
    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _effectDictionary = new Dictionary<string, EnemyAttack>();
        foreach (var data in enemyAttackList)
        {
            _effectDictionary[data.Enemyname] = data;
        }
    }

    public EnemyAttack GetEffect(string name)
    {
        if (_effectDictionary.TryGetValue(name, out var data))
            return data;
        return null;
    }
}
