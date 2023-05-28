using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button resumeButton;
    public GameObject controls;
    public Button[] buttons = new Button[4];
    public GameObject MainImage;
    public GameObject blur;
    float time;
    bool loadBool;
    public AudioSource menuAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loadBool)
        {
            menuAudio.volume -= Time.deltaTime*0.5f;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ShowControls()
    {
        for(int i=0; i< buttons.Length; i++)
        {
            //buttons[i].gameObject.SetActive(false);
            buttons[i].interactable = false;
        }
        resumeButton.gameObject.SetActive(true);
        controls.gameObject.SetActive(true);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Resume()
    {
        resumeButton.gameObject.SetActive(false);
        controls.GetComponentInChildren<Scrollbar>().value = 1;
        controls.gameObject.SetActive(false);
        blur.SetActive(false);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void Back()
    {
        resumeButton.gameObject.SetActive(false);
        controls.GetComponentInChildren<Scrollbar>().value = 1;
        controls.gameObject.SetActive(false);
        for (int i = 0; i < buttons.Length; i++)
        {
            //buttons[i].gameObject.SetActive(true);
            buttons[i].interactable = true;
        }
    }
    public void LoadGame()
    {
        loadBool = true;
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Forest"));
    }
    public void LoadDungeon()
    {
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Dwarf dungeon"));
    }
    void Load()
    {
        StartCoroutine(LoadLevelAfterDelay(1f));
        this.GetComponent<AudioSource>().volume -= Time.deltaTime*0.5f;
        
    }
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Forest");
    }
}
