using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float jump = 1;
    public float coyoteeTime = 0.2f;
    public float jumpBufferTime = 0.3f;
    public float jumpTime;

    [Header("Debug")]
    private Rigidbody2D rb;
    private Animator anim;
    private bool CanIJump = false;
    public LayerMask groundlayer;
    private float coyoteeTimeCounter;
    private float jumpBufferCounter;
    private bool JustGrounded = true;
    public float jumpTimeCounter;

    [Header("Fields")]
    [SerializeField] private GameObject myBottomParticles;

    [Header("JumpChecks")]
    [SerializeField] private Transform JumpCheck1;
    [SerializeField] private Transform JumpCheck2;
    [SerializeField] private Transform JumpCheck3;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        anim.SetBool("AmIWalking", false);
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(JumpCheck1.position, -transform.up, 0.1f, groundlayer) || Physics2D.Raycast(JumpCheck2.position, -transform.up, 0.1f, groundlayer) || Physics2D.Raycast(JumpCheck3.position, -transform.up, 0.1f, groundlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        CanIJump = IsGrounded();

        if (CanIJump)
        {
            if (!JustGrounded)
            {
                JustGrounded = true;
                myBottomParticles.SetActive(true);
            }
            coyoteeTimeCounter = coyoteeTime;
        }
        else
        {
            JustGrounded = false;
            coyoteeTimeCounter -= Time.deltaTime;
        }


        float dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

        if (Mathf.Abs(dirX) > 0.2f)
        {
            if (dirX < 0.2f && transform.rotation != Quaternion.Euler(0, 180, 0) && transform.rotation != Quaternion.Euler(0, -180, 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (dirX > 0.2f && transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
            if (jumpBufferCounter > 0f && coyoteeTimeCounter > 0f)
            {
                rb.AddForce(transform.up * jump * Time.timeScale);
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
}