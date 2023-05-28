using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinController : MonoBehaviour
{

    [SerializeField] GameObject myicon;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            GameObject.FindObjectOfType<ReloadControlController>().lumberIcon = true;
            myicon.SetActive(true);
        }
    }
}
