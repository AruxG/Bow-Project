using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public Animator anim;

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            anim.Play("DoorAnimationR");
            GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            anim.Play("DoorAnimation");
            GetComponent<AudioSource>().Play();
        }
    }

}
