using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    //左右的行动边界
    public Transform leftPoint,rightPoint;
    private bool isFaceLeft = true;
    public float speed,jumpForce;
    private float leftx, rightx;//用于保存左右边界的x值
    private Collider2D coll;
    public LayerMask ground;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }
    //敌人移动
    void Movement()
    {
        if (isFaceLeft)
        {
            if (coll.IsTouchingLayers(ground)) {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            if (transform.position.x < leftx)
            {
                //控制敌人面向右边，即掉头
                transform.localScale = new Vector3(-1, 1, 1);
                isFaceLeft = false;
            }
            
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFaceLeft = true;
            }
        }
    }
    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground)&& anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

}
