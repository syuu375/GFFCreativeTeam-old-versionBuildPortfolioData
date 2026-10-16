using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DamageEffect")]
public class DamageEffectSO : ScriptableObject
{
    public List<DamageEffect> damageEffectList = new List<DamageEffect>();

    [System.Serializable]
    public class DamageEffect
    {
        [SerializeField] string effectname;
        [SerializeField] GameObject hiteffect;


        public string EffectName { get => effectname; }
        public GameObject HitEffect { get => hiteffect; }
    }
}
