using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    void SKnockBack(int dir,int knockback);

    void BKnockBack(int dir, int knockback);

    void ElectStun(int dir,int knockback,float electtime);
}
