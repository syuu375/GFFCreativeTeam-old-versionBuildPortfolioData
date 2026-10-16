using System;
using System.Collections;
using System.Threading;
using UnityEngine;


namespace Cooltime
{
    public class CoolDown
    {
        const int MS = 1000;



       

        public void DiraySkill(float coolTime = 0, Action<int> dirskill = null, Action skill = null, int dir = 0)
        {
            Thread.Sleep((int)(coolTime * MS));
            skill();
            dirskill(dir);
            return;
        }

        public IEnumerator Skill(Action<Coroutine> callback,float cooltime = 0, Action skill = null, Action<int> dirskill = null, float diray = 0, int dir = 0)
        {
            yield return new WaitForSeconds(diray);
            if(skill != null) skill();
            if (dirskill != null) dirskill(dir);
            yield return new WaitForSeconds(cooltime);
            callback(null);
        }
    }
}

namespace Critical
{

    class CriticalDamage
    {
        const float PERCENT = 0.01f;
        public double damage(int ATK = 0, float critical = 5f, float criticalDamage = 50f)
        {
            float Crit = UnityEngine.Random.Range(1f, 100f);

            return (Crit < critical) ? ATK + (ATK * (criticalDamage * PERCENT)) : ATK;

        }

    }

}


