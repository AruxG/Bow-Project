using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    float speed;
    Animator anim;
    CharacterController enemy;
    public CharacterController player;
    const float maxDistance = 5f;
    float distance;
    Vector3 movement;
    bool enter;
    public bool dead;
    public bool fade;
    [SerializeField] Collider[] colliders;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<CharacterController>();
        enter = false;
        fade = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (!enter)
            {
                speed = 2.0f;
                movement = transform.forward;
                movement.y -= 10f;
                distance += Time.deltaTime;
                if (distance >= maxDistance)
                {
                    distance = 0;
                    transform.Rotate(transform.rotation.x, transform.rotation.y+90, transform.rotation.z);
                }
                anim.SetBool("walk", true);
                movement.y -= 10f;
            }
            else
            {
                speed = 0.3f;
                movement = player.transform.position - transform.position;
                movement.y -= 10f;
                transform.LookAt(player.transform);
            }
            enemy.Move(movement * Time.deltaTime * speed);
        }
        else
        {
            enemy.enabled = false;
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }
            
            StartCoroutine(Die());
            if (fade == true)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");
                Color oColor = GetComponentInChildren<SkinnedMeshRenderer>().material.color;
                float fadeAmount = oColor.a - (0.2f * Time.deltaTime);
                oColor = new Color(oColor.r, oColor.g, oColor.b, fadeAmount);
                GetComponentInChildren<SkinnedMeshRenderer>().material.color = oColor;
                if (oColor.a <= 0)
                {
                    fade = false;
                }
            }
        }
        
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        var t = transform.position;
        t.y += 3*Time.deltaTime;
        transform.position= t;
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
    void OnCollisionEnter(Collision col)
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y + 15, transform.rotation.z);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            enter = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            enter = false;
        }
    }
}
