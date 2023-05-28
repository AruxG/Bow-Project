using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region Variables
    float xAxisClamp = 0;
    float rotY = 0;
    private Transform playerTransform;
    private Animator anim;
    Vector3 normal = new Vector3(0.05f, 1.7f, -1.5f);
    Vector3 shoot = new Vector3(0.26f, 1.55f, 0f);//0.26,1.75,0
    [SerializeField]
    float sensibility= 1.0f;
    #endregion
    #region Methods
    void Start()
    {
        transform.localPosition = normal;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }
    void Update()
    {
        
        if (anim.GetBool("Shooting"))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            Invoke("Auxiliar2", 0.2f);
            RotateCamera(1f);
        }
        if (!anim.GetBool("Shooting"))
        {
            Invoke("Auxiliar", 0.3f);
            xAxisClamp = 0;
            //transform.Rotate(-Input.GetAxis("Mouse Y") * 25f * Time.deltaTime,0,0); //más bonito pero sin clamp
            if (Time.timeScale == 1)
            {
                rotY += Input.GetAxis("Mouse Y");
                rotY = Mathf.Clamp(rotY, -30f, 10f);
                transform.localRotation = Quaternion.Euler(-rotY, 0, 0);
            }
        }
        
    }
    private void Auxiliar()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, normal, 2);
    }
    private void Auxiliar2()
    {
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, shoot, 2);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RotateCamera(float magnitude)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float rotAmountX = mouseX * sensibility;
        float rotAmountY = mouseY * sensibility;

        xAxisClamp -= rotAmountY;

        Vector3 rotPlayer = playerTransform.transform.rotation.eulerAngles;

        rotPlayer.x -= rotAmountY;
        rotPlayer.z = 0;
        rotPlayer.y += rotAmountX;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            rotPlayer.x = 90;
        }else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            rotPlayer.x = 270;
        }

        playerTransform.rotation = Quaternion.Euler(rotPlayer);
    }
#endregion
}
