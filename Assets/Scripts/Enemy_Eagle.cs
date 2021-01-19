using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    public Transform upPoint, downPoint;
    private float upy, downy;
    public float speed;
    private bool facedUp;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        upy = upPoint.position.y;
        downy = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        if (facedUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > upy)
                facedUp = false;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < downy)
                facedUp = true;
        }
    }
}
