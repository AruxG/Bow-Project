using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject Dragon;
    // Start is called before the first frame update
    void Start()
    {
        Dragon.GetComponentInChildren<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter()
    {
        if (PuzzleController.solved == false)
        {
            GetComponent<Animator>().Play("Press");
            Dragon.GetComponent<AudioSource>().Play();
            Dragon.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(StopParticles());
            if (Dragon.transform.localRotation.z == 0.7071068f)
            {
                Dragon.GetComponent<Animator>().Play("DragonRot180");
            }else if (Dragon.transform.localRotation.w == 0.5f)
            {
                Dragon.GetComponent<Animator>().Play("DragonRot90");
            }
            else if (Dragon.transform.localRotation.z==0)
            {
                Dragon.GetComponent<Animator>().Play("DragonRot0");
            }
            else if (Dragon.transform.localRotation.w == -0.5f)
            {
                Dragon.GetComponent<Animator>().Play("DragonRot270");
            }
        }
    }
    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(0.6f);
        Dragon.GetComponentInChildren<ParticleSystem>().Stop();
    }
}
