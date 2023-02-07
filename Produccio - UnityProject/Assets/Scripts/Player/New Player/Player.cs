using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerClass {Templar, Nun};
    public PlayerClass currentClass;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    //[Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Player Health")]
    public Slider healthbar;
    public float maxHP;
    public float currentHP;
    public float regenRate;
    private float damageDelay = 15.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        switch(currentClass)
        {
            case PlayerClass.Templar:
                maxHP = 100f;
                moveSpeed = 6;
                groundDrag = 5;
                jumpForce = 6;
                airMultiplier = 0.4f;
                playerHeight = 2;
                break;
            case PlayerClass.Nun:
                maxHP = 85f;
                moveSpeed = 8;
                groundDrag = 5;
                jumpForce = 8;
                airMultiplier = 0.4f;
                playerHeight = 2;
                break;
        }
        currentHP = maxHP;
        healthbar.maxValue = maxHP;
        healthbar.value = healthbar.maxValue;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }


    public void takeDamage(float damage)
    {
        if (currentHP <= 0)
        {
            Die();
        }
        currentHP -= damage;
        healthbar.value = currentHP - Time.time;
        //StartCoroutine(DamageDelayer(damageDelay));
    }
    /*private IEnumerator DamageDelayer(float damageDelay)
    {
        yield return damageDelay;
        if (currentHP <= 0)
        {
            Die();
        }
    }*/

    private void Die()
    {
        //deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}