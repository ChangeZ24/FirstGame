using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    private AudioSource audio;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void Death()
    {
        //GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
    public void SwitchDeath()
    {
        audio.Play();
        anim.SetTrigger("death");
    }
}
