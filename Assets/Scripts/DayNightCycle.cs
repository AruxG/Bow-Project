using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float speed=30;
    [SerializeField] GameObject[] nightlights;
    [SerializeField] GameObject[] nightfires;
    Color day= new Color(0.8196079f, 0.8196079f, 0.8196079f, 1);
    Color night = new Color(0.2f, 0.2f, 0.2f, 1);
    Color sunset= new Color(1, 0.2f, 0, 1);
    Color noon= new Color(1, 0.8084262f, 0.4198113f, 1);
    Light l;
    // Start is called before the first frame update
    void Start()
    {
        l= GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.back,speed*Time.deltaTime);
        if (transform.position.y <1)
        {
            l.intensity -= 1*Time.deltaTime;
            
            if (this.name.Equals("Moon"))
            {
                RenderSettings.fogColor = Color.Lerp(night, day, 1-l.intensity);
                Camera.main.backgroundColor = RenderSettings.fogColor;
                for (int i = 0; i < nightlights.Length; i++) { nightlights[i].GetComponent<Light>().intensity = l.intensity * 3; }
                for (int i = 0; i < nightlights.Length; i++) { nightfires[i].GetComponent<ParticleSystem>().Stop(); }
                
                if (RenderSettings.ambientIntensity <1)
                {
                    RenderSettings.ambientIntensity += Time.deltaTime;
                }
            }
        }
        else if (transform.position.y >-1)
        {
            if (l.intensity < 1f)
            {
                l.intensity += 1 * Time.deltaTime;
            }
            if (this.name.Equals("Moon"))
            {
                RenderSettings.fogColor = Color.Lerp(day, night, l.intensity);
                Camera.main.backgroundColor = RenderSettings.fogColor;
                for (int i = 0; i < nightlights.Length; i++) { nightlights[i].GetComponent<Light>().intensity = l.intensity * 3; }
                for (int i = 0; i < nightlights.Length; i++) { nightfires[i].GetComponent<ParticleSystem>().Play(); }
                if (RenderSettings.ambientIntensity >0.2f)
                {
                    RenderSettings.ambientIntensity -= Time.deltaTime;
                }
            }
            else
            {
                GetComponent<Light>().color = Color.Lerp(sunset,noon,this.transform.position.y/500);
            }
        }
        transform.LookAt(Vector3.zero);
    }
}
