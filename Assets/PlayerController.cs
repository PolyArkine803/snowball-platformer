using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed = 7;
    public float jumpspeed = 17;
    private float direction;
    private Rigidbody2D player;
    [SerializeField] public Transform Player;
    public float scale = 1f;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    private float dashingCooldown;
    private bool isGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private float snow = 1;
    private bool isrolling;

    private IEnumerator Roll() 
    {
                    snow += .01f;
                    player.gravityScale += snow;
                    scale += snow; 
                    speed += snow;
                    jumpspeed += snow;
                    player.gravityScale += snow;
                    yield return new WaitForSeconds(.3f); 
                    if (scale < 2.5f)
                    {
                     StopCoroutine(Roll());  
                    }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalgravity = player.gravityScale;
        player.gravityScale = 0;
        player.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        player.gravityScale = originalgravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public GameObject dash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
          if (Input.GetButtonDown("Jump"))
            {
            Debug.Log("Jumped");
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpspeed);
            isGrounded = false;
            }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
        
        if (player.linearVelocity.x > .2f || player.linearVelocity.x < -.2f)
        {
            if (scale < 2.5f)
            {
                StartCoroutine(Roll());
                Debug.Log(player.linearVelocity.x);
            }
        }    


        int layermask = 1 << ~3;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.down, (1f+ snow)/2);
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if (ray == true)
        {
            
            Debug.Log("Somethine was hit");
            Debug.Log(ray.distance);
            isGrounded = true;
        }

        float originalgravity = player.gravityScale;
        if (player.linearVelocity.y < -1)
        {
            player.gravityScale = originalgravity + .05f;
        }
        else if (player.linearVelocity.y >= 0)
        {
            player.gravityScale = 3.5f;
        }
        if (isDashing)
        {
            return;
        }
       
        direction = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            if (direction > 0f)
            {
            player.linearVelocity = new Vector2(direction + speed, player.linearVelocity.y);
            Player.transform.localScale = new Vector2(scale, scale);
            }
            else if (direction < 0f)
            {
            player.linearVelocity = new Vector2(direction - speed, player.linearVelocity.y);
            Player.transform.localScale = new Vector2(scale, scale);
            }
        }
    }      
}