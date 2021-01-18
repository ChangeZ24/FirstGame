using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;

    public Collider2D coll;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public int cherry = 0;
    public int gem = 0;
    public Text CherryNumber;
    public Text GemNumber;
    [SerializeField]private bool isHurt;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
         
    void FixedUpdate()
    {
        if(!isHurt)
            movement(); 
        switchAnim();
    }
    void movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed*Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(faceDirection)); 
        }
        if(faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            anim.SetBool("jumping", true);
        }
        //if (Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)) {
        //    anim.SetBool("crouching", true);
        //    if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        //    {
        //        anim.SetBool("crouching", false);
        //    }
        //}
    }
    //动画变换
    void switchAnim()
    {
        anim.SetBool("idle", false);
        //保证没有跳跃的情况下，从高处下落仍然切换falling动画
        if(rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {//y<0.1f即没有向上的速度
            anim.SetBool("falling", true);
        }
        //跳跃到下落动画切换
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);//保证受伤后处于静止状态而不是跑步状态
            if (Mathf.Abs(rb.velocity.x) < 0.1)
            {//判断角色是否已经反弹结束处于静止状态
                isHurt = false;
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
            }
        }else if (coll.IsTouchingLayers(ground))//下降到地面动画切换
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }
    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Cherry"))
        {
            cherry++;
            CherryNumber.text = cherry.ToString();
            Destroy(collision.gameObject);
        }else if (collision.tag.Equals("Gem"))
        {
            gem++;
            GemNumber.text = gem.ToString();
            Destroy(collision.gameObject);
        }
    }
    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)//当该刚体碰撞另一个刚体时调用的函数
    {
        if (collision.gameObject.tag.Equals("Enemies"))
        { //函数代表的是整个碰撞器/刚体，故需要.gameObject获其角色类
          //消灭敌人
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                //增加消灭后再次小跳的效果
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {//增加受伤反弹的效果
                rb.velocity = new Vector2(-5, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}
    