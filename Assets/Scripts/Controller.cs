using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    #region Variables
    CharacterController player;
    public Animator anim;
    Rigidbody rb;
    ParticleSystem ps;
    GameObject newArrow;
    List<Color> colors = new List<Color>();
    public CanvasGroup HUD;
    public GameObject blur;
    public Light lhLight;
    public float numArrows = 30;
    public GameObject arrow;
    public GameObject crosshair;
    public Text contador;
    const float maxspeed = 4.5f;
    const float minspeed = 0.5f;
    float speed;
    const float maxRspeed = 45f;
    float rotationSpeed;

    private Vector3 movement = Vector3.zero;

    public float maxHealth = 100;
    public float health = 100;
    public GameObject H;

    public GameObject stamineBar;
    public GameObject staminePanel;
    public GameObject SightFog;
    public float stamineMax = 100;
    public float stamine = 100;
    bool coldown;
    bool restaure;
    public AudioSource[] audios;
    public bool overweight;
    #endregion

    #region Methods
    
    void Awake()
    {
        player = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = maxspeed;
        rotationSpeed = maxRspeed;
        contador.text = numArrows.ToString();
        colors.Add(new Color(0.8207547f, 0.6498762f, 0.08904416f, 0.5f));
        colors.Add(new Color(0.09019608f, 0.8196079f, 0.3843137f, 0.5f));
        colors.Add(new Color(0.09019608f, 0.1019608f, 0.8196079f, 1));
        if (GameObject.Find("bow range") != null)
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("bow range").GetComponent<Collider>(), true);
    }
    IEnumerator StamineColdown()
    {
        coldown = true;
        staminePanel.GetComponent<CanvasGroup>().alpha = 0.4f;
        yield return new WaitForSeconds(3);
        restaure = true;
        staminePanel.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(1);
        coldown = false;
        restaure = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth) { health += Time.deltaTime; } else { health = maxHealth; }
        H.transform.localScale= new Vector3(Mathf.Max(health/maxHealth, 0), 1 ,1);

        SightFog.GetComponent<Image>().material.SetFloat("_Transparency", 1 - Mathf.Sqrt(stamine / stamineMax));
        SightFog.GetComponent<Image>().material.color=(Color.Lerp(Color.white, new Color(1, 0.95f, 0.65f, 1), 1 - Mathf.Sqrt(stamine / stamineMax)));//1,0.95,0.65,1
        audios[1].volume = (1 - Mathf.Sqrt(stamine / stamineMax))*0.7f;
        if (stamine < stamineMax)
        {
            if (Time.timeScale == 1)
            {
                staminePanel.GetComponent<Image>().enabled = true;
                foreach (Image i in staminePanel.GetComponentsInChildren<Image>())
                {
                    i.enabled = true;
                }
            }
            
        }
        else
        {
            if (Time.timeScale == 1)
            {
                staminePanel.GetComponent<Image>().enabled = false;
                foreach (Image i in staminePanel.GetComponentsInChildren<Image>())
                {
                    i.enabled = false;
                }
            }
        }
        stamineBar.transform.localScale = new Vector3(Mathf.Max(stamine / stamineMax, 0), 1, 1);
        if (restaure)
        {
            stamine += Time.deltaTime * 100f;
        }
        ControlPlayer();
    }
    private void ControlPlayer()
    {
        if (Time.timeScale == 1)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            movement = Camera.main.transform.TransformDirection(direction);
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0);
            anim.SetFloat("x", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("y", Input.GetAxisRaw("Vertical"));
            movement.y -= 10f;// * Time.deltaTime;
            player.Move(movement * speed * Time.deltaTime);
            if (anim.GetBool("Shooting") == false)
            {
                HUD.alpha = 1;
                anim.SetBool("Moving", (movement.x != 0 || movement.z != 0));
                if (anim.GetBool("Moving"))
                {
                    if (Input.GetKey(KeyCode.LeftShift) && !coldown &&!overweight)
                    {
                        anim.SetBool("Running", true);
                        stamine -= Time.deltaTime * 5f;
                        if (stamine <= 0)
                        {
                            stamine = 0;
                            StartCoroutine(StamineColdown());
                        }
                        speed = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 ? maxspeed : maxspeed*1.5f;
                        rotationSpeed = maxRspeed * 1.5f;
                    }
                    else
                    {
                        anim.SetBool("Running", false);
                        if (stamine <= stamineMax && !coldown) { stamine += Time.deltaTime * 10f; }
                        speed = Mathf.Abs(Input.GetAxisRaw("Horizontal"))>0? maxspeed*.5f:maxspeed;
                        rotationSpeed = maxRspeed;
                    }
                }
                else
                {
                    anim.SetBool("Running", false);
                    if (stamine <= stamineMax && !coldown) { stamine += Time.deltaTime * 20f; }
                    speed = maxspeed;
                    rotationSpeed = maxRspeed;
                }
                
            }
            if (Input.GetMouseButtonDown(1) && numArrows > 0)
            {
                anim.SetBool("Shooting", true);
                HUD.alpha = 0.4f;
                crosshair.gameObject.SetActive(true);
                newArrow = Instantiate(arrow);
                ps = newArrow.GetComponentInChildren<ParticleSystem>();
                ps.Stop();
            }
            

            if (anim.GetBool("Shooting") == true)
            {
                speed = 0.0f;
                rotationSpeed = 0.0f;
                if (stamine <= stamineMax&&!coldown) { stamine += Time.deltaTime * 10f; }
                var main = ps.main;
                if (Input.GetMouseButtonDown(0))// && numArrows > 0 && anim.GetBool("Shooting"))
                {
                    anim.SetBool("Shooting", false);
                    numArrows--;
                    contador.text = numArrows.ToString();
                    crosshair.gameObject.SetActive(false);
                    Invoke("reSpawn", 0.3f);
                }
                if (Input.GetMouseButtonUp(1))
                {
                    anim.SetBool("Cancel", true);
                    anim.SetBool("Shooting", false);
                    anim.SetFloat("Reverse", -0.1f);
                    Destroy(newArrow);
                    crosshair.gameObject.SetActive(false);
                    Invoke("reSpawn", 0.3f);
                }
                if (Input.GetKeyDown("4"))
                {
                    ps.Play();
                    main.startColor = new ParticleSystem.MinMaxGradient(colors[2]);

                    lhLight.color = colors[2];
                    lhLight.intensity = 5;
                    Invoke("disableLight", 0.3f);
                }
                else if (Input.GetKeyDown("2"))
                {
                    ps.Play();
                    main.startColor = new ParticleSystem.MinMaxGradient(colors[0]);

                    lhLight.color = colors[0];
                    lhLight.intensity = 5;
                    Invoke("disableLight", 0.3f);
                }
                else if (Input.GetKeyDown("3"))
                {
                    ps.Play();
                    main.startColor = new ParticleSystem.MinMaxGradient(colors[1]);

                    lhLight.color = colors[1];
                    lhLight.intensity = 5;
                    Invoke("disableLight", 0.3f);
                }
                else if (Input.GetKeyDown("1"))
                {
                    ps.Stop();
                    lhLight.intensity = 0;
                }
            }
        }
    }
    void disableLight()
    {
        lhLight.intensity = 0; 
    }
    private void reSpawn()
    {
        speed = maxspeed;
        rotationSpeed = maxRspeed;

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        anim.SetBool("Cancel", false);
        anim.SetFloat("Reverse", 1.0f);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Arrow") && collision.gameObject.GetComponent<ArrowController>().shot==true){
            Destroy(collision.gameObject);
            numArrows++;
            contador.text = numArrows.ToString();
        }
    }
    private void Step()
    {
        audios[0].volume = Random.Range(0.25f,0.35f);
        audios[0].pitch = Random.Range(0.45f,0.6f);
        audios[0].PlayOneShot(GetComponent<AudioSource>().clip);
    }
    #endregion
}
