using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBonesChecker : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Arrow"))
        {
            Destroy(col.gameObject, 0.1f);
            if (col.gameObject.GetComponent<ArrowController>().done == false)
            {
                transform.GetComponentInParent<Animator>().SetBool("Die", true);
                transform.GetComponentInParent<enemyController>().dead = true;
                transform.GetComponentInParent<enemyController>().fade = true;
            }
        }
    }
}
