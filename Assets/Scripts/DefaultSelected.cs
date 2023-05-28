using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultSelected : MonoBehaviour
{
    public string key;
    public EventSystem even;
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(key))
        {
            even.SetSelectedGameObject(
                     this.gameObject);
        }
    }

}

