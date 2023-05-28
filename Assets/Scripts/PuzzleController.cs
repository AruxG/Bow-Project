using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public static bool solved = false;
    public GameObject DragonParent;
    public GameObject DragonL;
    public GameObject DragonR;
    public GameObject DragonC;
    public GameObject Wall;
    public ParticleSystem myParticles;
    void Start()
    {
        myParticles.Stop();
        if (solved)
        {
            Wall.GetComponent<Animator>().Play("Open");
            DragonParent.GetComponent<Animator>().Play("Solved");
        }
    }
    void Update()
    {
        if (DragonC.transform.localRotation.w == 0.5f && DragonL.transform.localRotation.z == 0.7071068f && DragonR.transform.localRotation.z == 0.7071068f&& solved==false)
        {
            Wall.GetComponent<AudioSource>().Play();
            solved = true;
            Wall.GetComponent<Animator>().Play("Open");
            myParticles.Play();
            StartCoroutine(StopParticles(myParticles ,0.005f));
            DragonParent.GetComponent<Animator>().Play("Solved");
            DragonC.GetComponent<AudioSource>().Play();
            DragonC.GetComponentInChildren<ParticleSystem>().Play();
            DragonR.GetComponent<AudioSource>().Play();
            DragonR.GetComponentInChildren<ParticleSystem>().Play();
            DragonL.GetComponent<AudioSource>().Play();
            DragonL.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(StopParticles(DragonC.GetComponentInChildren<ParticleSystem>(), 0.7f));
            StartCoroutine(StopParticles(DragonR.GetComponentInChildren<ParticleSystem>(), 0.7f));
            StartCoroutine(StopParticles(DragonL.GetComponentInChildren<ParticleSystem>(), 0.7f));
        }
    }
    IEnumerator StopParticles(ParticleSystem ps,float t)
    {
        yield return new WaitForSeconds(t);
        ps.Stop();
    }
}
