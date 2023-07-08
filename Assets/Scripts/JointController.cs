using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointController : MonoBehaviour
{
    private GameObject connectedPart;
    private float mistake = 0.5f;
    public float maxDistance = 6.0f;

    private void Start()
    {
        connectedPart = GetComponent<SpringJoint2D>().connectedBody.gameObject;
    }

    private void Update()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, (transform.position + connectedPart.transform.position) / 2);
        GetComponent<LineRenderer>().SetPosition(2, connectedPart.transform.position);
        if ((connectedPart.transform.position - transform.position).magnitude >= maxDistance)
        {
            StartCoroutine(nameof(SpeedDown));
        }
    }

    private IEnumerator SpeedDown()
    {
        while ((connectedPart.transform.position - transform.position).magnitude >= maxDistance
            && (GetComponent<Rigidbody2D>().velocity - Vector2.zero).magnitude >= 5)
        {
            GetComponent<Rigidbody2D>().velocity /= 10;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "Needle" && collision.gameObject.tag != "Joint")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    private void OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new
            Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

        // 如果鼠标点击位置在误差范围内，就松手
        if((transform.position - curPosition).magnitude < mistake)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
