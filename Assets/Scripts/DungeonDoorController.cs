using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDoorController : MonoBehaviour
{
    bool loadBool;
    public int ID;
    public float FMSpeed;
    [SerializeField] Text InfoText;
    [SerializeField] GameObject myicon;
    bool isIn;
    // Update is called once per frame
    void Update()
    {
        if (loadBool)
        {
            Camera.main.GetComponent<AudioSource>().volume -= Time.deltaTime * FMSpeed;
        }
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
        if (ID == 1)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                GameObject.FindObjectOfType<ReloadControlController>().dungeonIcon = true;
                myicon.SetActive(true);
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            GameObject.FindObjectOfType<ReloadControlController>().doorID = ID;
            InfoText.gameObject.SetActive(true);
            isIn = true;
            InfoText.text = "Press E to enter";
            if (Input.GetKey(KeyCode.E))
            {
                loadBool = true;
                if (ID%2 == 1)
                {
                    StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Dwarf dungeon"));
                }else if (ID%2==0)
                {
                    StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Forest"));
                }
            }
        }
    }
    void OnTriggerExit()
    {
        isIn = false;
        InfoText.gameObject.SetActive(false);
    }
}
