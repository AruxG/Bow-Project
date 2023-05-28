using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    [SerializeField] Text InfoText;
    bool open;
    bool isIn;
    void Update()
    {
        if (Time.timeScale == 1&&isIn)
        {
            InfoText.gameObject.SetActive(true);
        }else if(Time.timeScale==0 && isIn)
        {
            InfoText.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            isIn = true;
            InfoText.gameObject.SetActive(true);
            InfoText.text = "Press E to open";
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            if (Input.GetKey(KeyCode.E)&&!open)
            {
                InfoText.gameObject.SetActive(false);
                GetComponent<Animator>().SetBool("Open", true);
                GetComponent<AudioSource>().Play();
                open = true;
                //mostrar UI inventario
            }
        }
    }
    void OnTriggerExit()
    {
        if (open)
        {
            GetComponent<Animator>().SetBool("Open", false);
            GetComponent<AudioSource>().Play();
            //GetComponent<Animator>().PlayInFixedTime("Open");
            //GetComponent<Animation>().Play();
        }
        else
        {
            InfoText.gameObject.SetActive(false);
        }
        isIn = false;
        open = false;
    }
}
