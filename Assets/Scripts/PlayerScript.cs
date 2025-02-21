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
    public float gravityModifier;
    public float HealSpeed;

    [Header("Debug")]
    private Rigidbody2D rb;
    public Animator anim;
    private bool CanIJump = false;
    public LayerMask groundlayer;
    private float coyoteeTimeCounter;
    private float jumpBufferCounter;
    private bool JustGrounded = true;
    public float jumpTimeCounter;
    public bool startedJumping = true;
    public float damaged;

    [Header("Fields")]
    [SerializeField] private GameObject myBottomParticles;

    [Header("JumpChecks")]
    [SerializeField] private Transform JumpCheck1;
    [SerializeField] private Transform JumpCheck2;
    [SerializeField] private Transform JumpCheck3;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        anim.SetBool("AmIWalking", false);
    }
    public IEnumerator regen()
    {
        while (damaged > 0)
        {
            //effect
            damaged -= Time.deltaTime * HealSpeed;
            yield return null;
        }
        damaged = 0;
    }
    public void hit()
    {
        if (damaged <= 0)
        {
            damaged++;
            StartCoroutine(regen());
        }
        else
        {
            damaged++;
        }
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

    void Jump()
    {
        rb.AddForce(transform.up * jump * Time.deltaTime);
    }

    private void Update()
    {
        CanIJump = IsGrounded();

        if (CanIJump)
        {

            if (!JustGrounded)
            {
                if (jumpBufferCounter > 0f)
                {
                    Jump();
                }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
            if (coyoteeTimeCounter > 0f)
            {
                startedJumping = true;
                Jump();
            }
            jumpTimeCounter = jumpTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0f)
        {
            if (startedJumping)
            {
                jumpTimeCounter -= Time.deltaTime;
                Jump();
            }
        }
        else
        {
            startedJumping = false;
        }

    }

    void FixedUpdate()
    {
        float dirX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);

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

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - gravityModifier);
        }
    }
}