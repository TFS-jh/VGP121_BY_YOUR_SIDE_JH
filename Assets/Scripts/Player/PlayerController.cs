using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseJump = 10f;
    [SerializeField] private float basePogoBoost = 7f;
    [SerializeField] private GameObject respawnPoint;

    [SerializeField] public int maxJumpCount = 1;
    [SerializeField] public float baseSpeed = 5f;
    
    [HideInInspector] public bool win;

    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private LayerCheck check;

    private int jumpCount = 0;
    private bool isFalling;

    #region Coins
    private int coinCount = 0;

    public void IncrementCoinCounter(int amount)
    {
        coinCount += amount;
        Debug.Log("Coins: " + coinCount.ToString());
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        check = GetComponent<LayerCheck>();

        check.Init(col, rb);
        rb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        bool isGroundedThisFrame = check.CheckGround();
        bool isDeadThisFrame = check.CheckDeath();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool fireInput = Input.GetButtonDown("Fire1");
        bool fireHeld = Input.GetButton("Fire1");

        float moveX = horizontalInput * baseSpeed;
        
        // Allows for movement
        rb.linearVelocityX = moveX;

        // jump along y axis
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;
                rb.linearVelocityY = 0;
                rb.AddForceY(baseJump, ForceMode2D.Impulse);
            }
        }

        if (isGroundedThisFrame){ 
            if (rb.linearVelocityY <= 0)
            {
                jumpCount = 0;
            }
            if (clipInfo != null && clipInfo.Length > 0 && clipInfo[0].clip != null) {
                if (clipInfo[0].clip.name == "pogo")
                {
                    jumpCount++;
                    rb.linearVelocityY = 0;
                    rb.AddForceY(basePogoBoost, ForceMode2D.Impulse);
                }
                if (clipInfo[0].clip.name == "stabHold")
                {
                    rb.linearVelocityX = 0;
                }
            }
        }

        if (isDeadThisFrame && rb.linearVelocityY <= 0) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = respawnPoint.transform.position;
        }

        #region my own fall check
        if (rb.linearVelocityY < 0)
        {
            isFalling = true;
        }
        else if (rb.linearVelocityY >= 0)
        {
            isFalling = false;
        }
        #endregion

        SpriteFlip(horizontalInput);

        //Update animator parameters
        anim.SetBool("isGrounded", isGroundedThisFrame);
        anim.SetFloat("horizontalInput", MathF.Abs(horizontalInput));
        anim.SetFloat("verticalInput", (verticalInput));
        //anim.SetBool("hasWon", win);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("stabHold", fireHeld);

        // Controller bools DIRECTLY trigger animations rather than triggering an Animator variable
        if (fireInput) anim.SetTrigger("stabInput");
    }

    //Flips Sprite when moving left/right in one line
    //private void SpriteFlip(float horizontalInput) => sr.flipX = (horizontalInput < 0);
    private void SpriteFlip(float horizontalInput)
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (!sr.flipX && horizontalInput < 0 || sr.flipX && horizontalInput > 0)
        {
            sr.flipX = !sr.flipX;
        }
    }
}
