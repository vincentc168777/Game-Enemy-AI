using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region cameraLook
    [SerializeField] private Camera playerCam;
    [SerializeField] private float lookSensitivity;
    private float xRotation;
    private float yRotation;
    #endregion

    #region movement
    private CharacterController playerCont;
    private Rigidbody playerRB;

    [SerializeField] private float playerWalkSpeed;
    [SerializeField] private float playerRunSpeed;
    //walking input will be processed into this vector
    private Vector3 moveVec;

    #region stamina
    [SerializeField] private float maxStamina;
    [SerializeField] private float sprintCost;
    [SerializeField] private float staminaRechargeWait;
    [SerializeField] private float staminaRechargeRate;
    [SerializeField] private Image staminaBar;
    [SerializeField] private float currStamina;
    [SerializeField] private float currSpeed;
    private bool isStamRecharging;
    private Coroutine stamRechCoroutine;
    private bool canRun;
    #endregion

 
   
    

    #region jumping
    [SerializeField] private float jumpForce;

    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
   
        playerRB = GetComponent<Rigidbody>();
        playerRB.freezeRotation = true;


        currStamina = maxStamina;

        isStamRecharging = false;
        canRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInputs();
        moveInputs();
        SpeedControl();
        manageStamina();
    }

    private void FixedUpdate()
    {
        movePlayer();
        physicsJump();
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
        Vector3 moveDir = new Vector3(moveX, 0f, moveY);
        /* transformDirections takes the local direction we want to go in 
         * and converts it to a world space equivalent
         */
        moveVec = transform.TransformDirection(moveDir).normalized;
        
    }

    private void movePlayer()
    {
        /*
         * drag should only active if grounded
         * if is active while falling, the fall will be super slow
         */
        if (isGrounded())
        {
            playerRB.drag = 5;
        }
        else
        {
            playerRB.drag = 0;
        }

        playerRB.AddForce(moveVec * currSpeed * 10, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 currMoveVec = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);

        if(currMoveVec.magnitude > currSpeed)
        {
            Vector3 controlledSpeedVec = currMoveVec.normalized * currSpeed;
            playerRB.velocity = new Vector3(controlledSpeedVec.x, playerRB.velocity.y, controlledSpeedVec.z);
        }
    }

    #region stamina code
    private void manageStamina()
    {
        if (currStamina <= 0)
        {
            Debug.LogError("cant run");
            canRun = false;
        }
        else
        {
            canRun = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            stopStaminaRecharge();

            currSpeed = playerRunSpeed;

            currStamina -= sprintCost * Time.deltaTime;
            staminaBar.fillAmount = currStamina / maxStamina;

        }
        else
        {
            currSpeed = playerWalkSpeed;

            if (currStamina < maxStamina)
            {
                if (!isStamRecharging && !Input.GetKey(KeyCode.LeftShift))// edge case where stamina bar is fully depleted but we are still holding shift
                {
                    stamRechCoroutine = StartCoroutine(rechargeStamina());
                    isStamRecharging = true;
                }

            }
        }

        currStamina = Mathf.Clamp(currStamina, 0f, maxStamina);
    }

    private IEnumerator rechargeStamina()
    {
        yield return new WaitForSeconds(staminaRechargeWait);
        Debug.Log("recharging");
        while (currStamina < maxStamina)
        {
            currStamina += staminaRechargeRate * Time.deltaTime;
            currStamina = Mathf.Clamp(currStamina, 0f, maxStamina);
            staminaBar.fillAmount = currStamina / maxStamina;
            yield return null;
        }
        stopStaminaRecharge();
    }

    private void stopStaminaRecharge()
    {
        if (stamRechCoroutine != null)
        {
            StopCoroutine(stamRechCoroutine);
        }
        isStamRecharging = false;
    }
    #endregion

    #region jump code
    private void physicsJump()
    {
        if(isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            //reset y velocity so you always jump same height
            //playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
            playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }


    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.3f);
    }

    #endregion
}
