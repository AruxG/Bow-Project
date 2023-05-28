using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    #region Variables
    Transform Parent;
    GameObject origen;
    GameObject player;
    Rigidbody rb;
    AnimationCurve aux;
    AudioSource asource;
    public ParticleSystem ps;
    public bool shot;
    public bool done;

    bool hit = false;
    private Quaternion rot;
    private Vector3 dir;
    const float maxDistance = 1000.0f;
    float cont;
    #endregion
    #region Methods
    void Awake()
    {
        done = false;
        shot = false;
        Parent = GameObject.FindGameObjectWithTag("Arms").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        origen = GameObject.FindGameObjectWithTag("Origen");
        rb = GetComponent<Rigidbody>();
        asource = this.GetComponent<AudioSource>();
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        if (GameObject.Find("bow range") != null)
        {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameObject.Find("bow range").GetComponent<Collider>(), true);
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("bow range").GetComponent<Collider>(), true);
        }
    }
    void RotatetoMouse (Vector3 Destination)
    {
        dir = Destination - this.transform.position;
        rot = Quaternion.LookRotation(dir);
        transform.localRotation = Quaternion.Lerp(transform.rotation,rot,1);
    }
    void Update()
    {
        
        if (shot == false)
        {
            RaycastHit hit;
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maxDistance))
            {
                RotatetoMouse(hit.point);
            }
            else
            {
                var pos = rayMouse.GetPoint(maxDistance);
                RotatetoMouse(pos);
            }
            if (player.GetComponent<Animator>().GetBool("Shooting"))
            {
                cont += Time.deltaTime;
                transform.position = Parent.position;
                transform.rotation = player.transform.rotation;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<Transform>().SetParent(this.transform);
            Shoot();
            Destroy(this.gameObject, 15.0f);
        }
    }
    void Shoot()
    {
        shot = true;
        GetComponent<BoxCollider>().enabled = true;
        //Revisar si forward o vector a hitpoint, buscar tutorial en historial

        //rb.velocity = dir.normalized  * Mathf.Min(Mathf.Pow(7,cont), 70.0f);
        rb.velocity = transform.forward  * Mathf.Min(Mathf.Pow(5,cont), 45.0f);//forward, 5,45
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Player") && !collision.gameObject.tag.Equals("Bow") && !collision.gameObject.tag.Equals("Arrow")) 
        {
            GetComponent<CapsuleCollider>().enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            if (!hit) { asource.Play(); }
            gameObject.GetComponent<Transform>().SetParent(collision.collider.gameObject.transform);
            if (!collision.gameObject.tag.Equals("Enemy"))
            {
                done = true;
            }
            hit = true;
            StartCoroutine(TurnOffRoutine());
        }
        
    }
    IEnumerator TurnOffRoutine()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
    }
    #endregion
}
