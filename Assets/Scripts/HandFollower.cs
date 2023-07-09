using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandFollower : MonoBehaviour
{
    public float touchVelocity;
    private Vector2 distance;
    private bool autoFollowing;
    public Transform waitingPos;

    public Button modeChangeBtn;
    public Sprite mode1;
    public Sprite mode2;
    public Text modeNowText;

    private void Start()
    {
        waitOutside();
        modeChangeBtn.onClick.AddListener(() =>
        {
            if (autoFollowing) waitOutside();
            else
            {
                autoFollowing = true;
                modeChangeBtn.image.sprite = mode1;
                modeNowText.text = "刺激: 触点应激弹飞";
            }
        });
    }

    private void Update()
    {
        if (Time.timeScale == 0) waitOutside();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (autoFollowing) waitOutside();
            else
            {
                autoFollowing = true;
                modeChangeBtn.image.sprite = mode1;
                modeNowText.text = "刺激: 触点应激弹飞";
            }
        }
        if (autoFollowing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.GetPoint(10);
        }
    }

    public void waitOutside()
    {
        autoFollowing = false;
        transform.position = waitingPos.position;
        modeChangeBtn.image.sprite = mode2;
        modeNowText.text = "安抚: 触点放弃抓握";
    }

    /*//拖拽功能的实现，该函数可应用到任何脚本中
    IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标
                                                                                 //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
        //gameObject.SetActive(false);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        distance = collision.transform.position - transform.position;
        collision.GetComponent<Rigidbody2D>().isKinematic = false;
        if(collision.GetComponent<MainBodyController>())
            collision.GetComponent<Rigidbody2D>().velocity = touchVelocity * distance / 6.0f;
        else collision.GetComponent<Rigidbody2D>().velocity = touchVelocity * distance;
    }
}
