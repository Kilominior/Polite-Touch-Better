using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JointController : MonoBehaviour
{
    private GameObject connectedPart;
    public GameObject mySpriteLayer;
    public float maxDistance = 6.0f;
    private bool isReleasing;
    private float rleaseTime = 0.3f;
    private bool connectedToMovingObj;

    private void Start()
    {
        isReleasing = false;
        connectedPart = GetComponent<SpringJoint2D>().connectedBody.gameObject;
        mySpriteLayer = Instantiate(connectedPart.GetComponent<MainBodyController>().spritePrefab,
            transform.position, Quaternion.identity, transform.parent);
        mySpriteLayer.GetComponent<SpriteRenderer>().sortingOrder =
            gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        mySpriteLayer.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        connectedToMovingObj = false;
    }

    private void Update()
    {
        if(!mySpriteLayer.activeInHierarchy) mySpriteLayer.SetActive(true);
        transform.localScale = Vector3.one;
        mySpriteLayer.transform.position = transform.position;

        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, (transform.position + connectedPart.transform.position) / 2);
        GetComponent<LineRenderer>().SetPosition(2, connectedPart.transform.position);

        Vector3 rotateDir = connectedPart.transform.position - transform.position;

        float DistanceRate = rotateDir.magnitude / maxDistance;
        if (DistanceRate < 1.0f) GetComponent<LineRenderer>().widthMultiplier = 1.5f - DistanceRate;
        else releaseHand();

        Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, rotateDir);
        mySpriteLayer.transform.rotation = quaternion;

        if ((connectedPart.transform.position - transform.position).magnitude >= maxDistance)
        {
           // GetComponent<Rigidbody2D>().velocity *= -1.0f;
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
        if(!isReleasing && collision.name != "Needle" && collision.gameObject.tag != "Joint"
             && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Reward"
             && collision.gameObject.tag != "Speech" && collision.gameObject.tag != "Respawn")
        {
            connectedPart.GetComponent<MainBodyController>().systemMenu.AudioPlay(2);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.GetComponent<FloatingTitle>() || collision.GetComponent<Spinning>())
            {
                transform.SetParent(collision.transform);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                connectedToMovingObj = true;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name != "Needle" && collision.gameObject.tag != "Joint"
             && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Reward"
             && collision.gameObject.tag != "Speech" && collision.gameObject.tag != "Respawn")
        {
            if(connectedToMovingObj && collision.transform == transform.parent)
            {
                transform.SetParent(connectedPart.
                    GetComponent<MainBodyController>().bodyParent.transform);
                //releaseHand();
                connectedToMovingObj = false;
            }
        }
    }

    public void releaseFromConnectedObj(Transform originParent)
    {
        transform.SetParent(originParent);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        connectedToMovingObj = false;
    }

    private void OnMouseDown()
    {
        if (connectedPart.GetComponent<MainBodyController>().isRudeTouch) return;
        connectedPart.GetComponent<MainBodyController>().TouchedCry(2);
        releaseHand();
    }

    public void releaseHand()
    {
        isReleasing = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        StopCoroutine(nameof(releaseWait));
        StartCoroutine(nameof(releaseWait));
    }

    private IEnumerator releaseWait()
    {
        yield return new WaitForSecondsRealtime(rleaseTime);
        isReleasing = false;
    }
}
