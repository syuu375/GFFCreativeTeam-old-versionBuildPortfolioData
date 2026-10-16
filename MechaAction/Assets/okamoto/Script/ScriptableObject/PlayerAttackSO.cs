using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerAttackType")]
public class PlayerAttackSO : ScriptableObject
{
    public List<PlayerAttackType> playerAttackList = new List<PlayerAttackType>();

    [System.Serializable]
    public class PlayerAttackType
    {
        [SerializeField] string Attackname;
        [SerializeField] Weapontype type;
        [SerializeField] int damage;
        [SerializeField] int knockback;
        [SerializeField] string effectname;
        [SerializeField] string audioname;


        public enum Weapontype
        {
            Sowd,
            Gun
        }

        public int Damage { get => damage;}
        public int Knockback { get => knockback;}
        public string EffectName { get => effectname;}
        public string AudioName { get => audioname;}
    }
}
