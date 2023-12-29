using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float gravity;
    private float xRotation;
    private float yRotation;
    private CharacterController playerCont;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCont = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseInputs();
        moveInputs();
        playerGravity();
    }

    private void mouseInputs()
    {
        float camX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * lookSensitivity;
        float camY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * lookSensitivity;
        
        /*
         * when you move your mouse up, it returns positive 1 for camY
         * but when we rotate an object around the x axis, adding positve angles
         * means the object will rotate downwards, so we have to subtract the postive value
         * from xRotation to make camera rotate up when we move our mouse up
         */
        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += camX;



        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        playerCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void moveInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        /* makes sure that diagonal move has vector magnitude of 1
         * and that when we multiply it by speed it will always be 
         * the same result
         */
        Vector3 moveDir = new Vector3(moveX, 0f, moveY).normalized;
        /* transformDirections takes the local direction we want to go in 
         * and converts it to a world space equivalent
         */
        moveDir = transform.TransformDirection(moveDir);
        playerCont.Move(moveDir * Time.deltaTime * playerSpeed);
    }

    private void playerGravity()
    {

        Vector3 gravVec = new Vector3(0, gravity, 0);
        
        playerCont.Move(gravVec * Time.deltaTime);
    }
}
