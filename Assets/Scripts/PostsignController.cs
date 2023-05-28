using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostsignController : MonoBehaviour
{
    [SerializeField] Text InfoText;
    [SerializeField] GameObject myicon;
    bool isIn;
    void Update()
    {
        if (Time.timeScale == 1 && isIn)
        {
            InfoText.gameObject.SetActive(true);
        }
        else if (Time.timeScale == 0 && isIn)
        {
            InfoText.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            isIn = true;
            GameObject.FindObjectOfType<ReloadControlController>().signIcon = true;
            myicon.SetActive(true);
            InfoText.gameObject.SetActive(true);
            InfoText.text = "Press E to read";
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                InfoText.text = "North - Dwarf Ruins \n South - Lumberjack camp";
            }
        }
    }
    void OnTriggerExit()
    {
        isIn = false;
        InfoText.gameObject.SetActive(false);
    }
}
