using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowController : MonoBehaviour
{
    Animator anim;
    public GameObject Parent;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Parent.GetComponent<Controller>().numArrows>0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                anim.SetFloat("Shoot", 1);
            }
            if (Input.GetMouseButtonUp(1))
            {
                anim.SetFloat("Shoot", -1);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                anim.SetFloat("Shoot", -1);
            }
        }
    }
}
