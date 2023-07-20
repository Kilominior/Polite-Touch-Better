using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public SystemMenuController systemMenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            systemMenu.OnFinish();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
