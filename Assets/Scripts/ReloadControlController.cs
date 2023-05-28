using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadControlController : MonoBehaviour
{
    Vector3[] positions = new Vector3[9];
    Vector3[] rotations = new Vector3[9];
    Transform player;

    public int doorID = 0;
    public bool fireIcon;
    public bool lumberIcon;
    public bool signIcon;
    public bool dungeonIcon;
    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ReloadControl");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        positions[0] = new Vector3(116, 0.05f, 164);
        rotations[0] = new Vector3(0, 0, 0);

        positions[1] = new Vector3(0,0,-3.15f);
        rotations[1] = new Vector3(0,0,0);

        positions[2] = new Vector3(229.59f, 0.05f, 351.57f);
        rotations[2] = new Vector3(0, 136.1f, 0);

        positions[3] = new Vector3(-34.71f, 0.05f, 9.26f);
        rotations[3] = new Vector3(0, 0, 0);

        positions[4] = new Vector3(162.85f, 0.05f, 280f);
        rotations[4] = new Vector3(0, 88.743f, 0);

        positions[5] = new Vector3(116, 0.05f, 155);
        rotations[5] = new Vector3(0, 180, 0);

        positions[6] = new Vector3(184.88f, 0.05f, 51.1f);
        rotations[6] = new Vector3(0, 90, 0);

        positions[7] = new Vector3(247, 0.05f, 8);
        rotations[7] = new Vector3(0, 160, 0);

        positions[8] = new Vector3(240, 0.05f, 340);
        rotations[8] = new Vector3(0, -45, 0);

        fireIcon = true;
    }
    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("MainMenu")) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            player.position = positions[doorID];
            player.rotation = Quaternion.Euler(rotations[doorID]);
        }
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(0.3f);
        //Debug.Log("He cargado el bosque");
    }
}
