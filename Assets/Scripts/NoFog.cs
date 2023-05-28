using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFog : MonoBehaviour
{

    bool doWeHaveFogInScene;
    [SerializeField] GameObject player;
    [SerializeField] float height;

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x,player.transform.eulerAngles.y, this.transform.rotation.eulerAngles.z);
    }
    private void Start()
    {
        doWeHaveFogInScene = RenderSettings.fog;
    }

    private void OnPreRender()
    {
        RenderSettings.fog = false;
    }
    private void OnPostRender()
    {
        RenderSettings.fog = doWeHaveFogInScene;
    }
}
