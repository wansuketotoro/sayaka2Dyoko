using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    public FloatingJoystick floatingJoystick;
    [SerializeField] float speedAdd;
    float speed;
    Rigidbody2D rd2;
    [SerializeField] float jumpPower = 400f;
    [SerializeField] GameManager gameManager;
    [SerializeField] PhysicsMaterial2D physicsMaterial2;
    [SerializeField] Animator animator;

    void Start()
    {
        rd2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        //Joyスティックによる向き判定 ***************
        float x = floatingJoystick.Horizontal;
        float y = floatingJoystick.Vertical;
        if (x == 0)
        {
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            direction = DIRECTION_TYPE.RIGHT;            
        }
        else if (x < 0)
        {
            direction = DIRECTION_TYPE.LEFT;
        }
        //-----------------------------------------------
  

        if (IsGround())
        {            
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
        }

    }

    private void FixedUpdate()
    {
        //DIrectionによって移動処理***************************************************
        switch(direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                animator.SetBool("Ranbool", false);
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = speedAdd;
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("Ranbool", true);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = speedAdd * -1;
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool("Ranbool", true);
                break;
        }
            rd2.velocity = new Vector2(speed, rd2.velocity.y);
        //----------------------------------------------------------------------------
    }
    public void Jump()
    {
        if (IsGround())
        {
            rd2.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
    }
    bool IsGround()
    {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rigthStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endStartPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endStartPoint);
        Debug.DrawLine(rigthStartPoint, endStartPoint);
        return Physics2D.Linecast(leftStartPoint, endStartPoint, blockLayer)
          || Physics2D.Linecast(rigthStartPoint, endStartPoint, blockLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            gameManager.GameOver();
        }
        if (collision.gameObject.tag == "Friction")
        {
            physicsMaterial2.friction = 50f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("JumpFall", false);
            Debug.Log("test");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Friction")
        {
            physicsMaterial2.friction = 0f;
        }
    }
    void JumpFall()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("JumpFall", true);
    }
    void JumpEnd()
    {        
        animator.SetBool("JumpFall", false);        
    }
}
