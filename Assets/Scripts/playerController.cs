using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;

    public Collider2D coll;
    public Collider2D disColl;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public int cherry = 0;
    public int gem = 0;
    public Text CherryNumber;
    public Text GemNumber;
    [SerializeField]private bool isHurt;

    public Transform head;

    public AudioSource jumpAudio,hurtAudio,collectAudio;


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

    private void Update()
    {
        Jump();
        Crouch();
        //CherryNumber.text = cherry.ToString();
    }
    void movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed*Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(faceDirection)); 
        }
        if(faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        
        //Crouch();
        
    }
    //动画变换
    void switchAnim()
    {
        //anim.SetBool("idle", false);
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
                //anim.SetBool("idle", true);
            }
        }else if (coll.IsTouchingLayers(ground))//下降到地面动画切换
        {
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
    }
    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag.Equals("Cherry"))
        {
            collectAudio.Play();
            //collision.GetComponent<Animation>().Play("isGot");
            cherry++;
            CherryNumber.text = cherry.ToString();
            Destroy(collision.gameObject);
        }else if (collision.tag.Equals("Gem"))
        {
            collectAudio.Play();
            gem++;
            GemNumber.text = gem.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.tag.Equals("DeadLine"))
        {
            //关闭所有音频
            GetComponent<AudioSource>().enabled = false;
            //延迟2s触发restart函数
            Invoke("Restart", 2f);
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
                //调用Enemy_Frog类的方法切换敌人的动画
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.SwitchDeath();

                //增加消灭后再次小跳的效果
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {//增加受伤反弹的效果
                hurtAudio.Play();
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
    //趴下
    void Crouch()
    {
        //角色趴下
        if (!Physics2D.OverlapCircle(head.position, 0.2f, ground))
        {//当头顶没有东西的时候才能站起来，否则在爬行过程中无法站立
            if (Input.GetButton("Crouch"))
            {
                //趴下时取消头部的碰撞器，使之可以在地面爬行
                disColl.enabled = false;
                anim.SetBool("crouching", true);

            }
            else
            {
                disColl.enabled = true;
                anim.SetBool("crouching", false);
            }
        }
    }
    void Jump()
    {
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            anim.SetBool("jumping", true);
        }
    }
    //重启
    void Restart()
    {
        //重新加载scene--死亡后重启
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //public void CherryCount()
    //{
    //    cherry++;
    //}
}
    