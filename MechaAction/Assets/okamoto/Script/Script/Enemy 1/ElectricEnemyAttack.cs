using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemyAttack : MonoBehaviour
{
    //private int _damage = 20;
    //private int _knockback = 0;

    private void Update()

    {

        //transform.position += transform.forward * 3f * Time.deltaTime;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

    }

    public void ElectricAttack()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            GameObject target = hit.collider.gameObject;

            if (target.tag == "Player")
            {
                Debug.Log("playerにアタック");
            }
        }
    }
}
