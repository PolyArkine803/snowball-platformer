using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
    



public class PlayerController : MonoBehaviour
{
    
    [SerializeField] public Transform Player;
    
    private float snow = 0f;
    

    public float speed = 7f;
    public float jumpspeed = 17f;
    public float scale = 1f;

    private float newspeed = 7f;
    private float newjumpspeed = 17f;
    private float newscale = 1f;

    private float direction;
    private Rigidbody2D player;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    private float dashingCooldown;
    private bool isGrounded;
    public GameObject dash;
    IEnumerator Roll()
    {
        yield return new WaitForSeconds(.5f);
        while (true)
        {
            
            if (player.linearVelocity.x > 1 || player.linearVelocity.x < -1)
            {
                if (newscale < 2.5f)
                {
                    if (isGrounded)
                    {
                        snow = snow + -.006f * player.linearVelocity.y;
                        newspeed = speed + (snow*2);
                        newscale = scale + snow;
                        newjumpspeed = jumpspeed + (snow*10);
                        yield return new WaitForSeconds(.1f);
                    }
                }
            }
           yield return new WaitForSeconds(.01f); 
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        StartCoroutine(Roll());
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Horizontal");
    
        Player.transform.localScale = new Vector2(newscale, newscale);
          if (Input.GetButtonDown("Jump") && isGrounded)
            {
                player.gravityScale = 3.5f;
                player.linearVelocity = new Vector2(player.linearVelocity.x, newjumpspeed);
                isGrounded = false;
            }
            if (snow > 1f)
            {
                if (direction > 0f)
                {
                    player.linearVelocity = new Vector2(direction + newspeed+(snow*3), player.linearVelocity.y);
                    Debug.Log("launched");
                }
                else if (direction < 0f)
                {
                    player.linearVelocity = new Vector2(direction - newspeed+(snow*3), player.linearVelocity.y);
                    Debug.Log("launched");
                }   
                snow = snow*.75f;
                newspeed = speed + (snow*2);
                newscale = scale + snow;
                newjumpspeed = jumpspeed + (snow*10);
            }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }  
        int layermask = 1 << ~3;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.down, 1f+snow);
        Debug.DrawRay(transform.position, Vector3.down, Color.green, 1f+snow);
        if (ray == true)
        {
            
            Debug.Log("Somethine was hit");
            Debug.Log(ray.distance);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        float originalgravity = player.gravityScale;
        if (player.linearVelocity.y < -1)
        {
            player.gravityScale = originalgravity + .05f;
            Debug.Log("1");
        }
        else if (player.linearVelocity.y > 1f && isGrounded)
        {
            player.gravityScale = originalgravity + .1f;
            Debug.Log("2");
        }
        if (player.linearVelocity.y > -.05f && player.linearVelocity.y < .05f)
        {
            player.gravityScale = 3.5f;
            Debug.Log("3");
        }
        
       

        if (isGrounded)
        {
            if (direction > 0f)
            {
            player.linearVelocity = new Vector2(direction + newspeed, player.linearVelocity.y);
            }
            else if (direction < 0f)
            {
            player.linearVelocity = new Vector2(direction - newspeed, player.linearVelocity.y);
            }
        }
    }      
}
