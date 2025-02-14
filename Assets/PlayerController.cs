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
    private GameObject Player;
    public float scale = 1;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    private float dashingCooldown;
    private bool isGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private float snow;
    private bool isrolling;


    public GameObject dash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        int layermask = 1 << ~3;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.down, .5f);
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
            StartCoroutine(Roll());
            if (direction > 0f)
            {
            player.linearVelocity = new Vector2(direction + speed, player.linearVelocity.y);
            }
            else if (direction < 0f)
            {
            player.linearVelocity = new Vector2(direction - speed, player.linearVelocity.y);
            }
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
            {
            Debug.Log("Jumped");
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpspeed);
            isGrounded = false;
            }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

    }

   private IEnumerator Roll() 
   {
    while (direction > .1f || direction < -.1f)
    {
        if (scale < 2.5)
        {
            snow += .01f;
            yield return new WaitForSeconds(1f);
            player.gravityScale += snow;
            scale += snow; 
            speed += snow;
            jumpspeed += snow;
            player.gravityScale += snow;
        }
    }
   }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalgravity = player.gravityScale;
        player.gravityScale = 0;
        player.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        GameObject dash_anim;
        dash_anim = Instantiate(dash, transform.position, Quaternion.identity);
        dash_anim.transform.localScale = new Vector2(player.transform.localScale.x / 2, Player.transform.localScale.y / 2);
        dash_anim.transform.position = new Vector2(dash_anim.transform.localPosition.x, dash_anim.transform.localPosition.y + 4);
        yield return new WaitForSeconds(dashingTime);
        Destroy(dash_anim);
        player.gravityScale = originalgravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}