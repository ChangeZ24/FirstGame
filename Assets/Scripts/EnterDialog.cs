using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;

    //角色碰撞到出现对话的位置，触发函数
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            enterDialog.SetActive(true);
        }   
    }
    //角色离开出现对话的位置，触发函数
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            enterDialog.SetActive(false);
        }
    }
}
