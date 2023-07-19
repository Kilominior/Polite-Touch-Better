using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainBodyController : MonoBehaviour
{
    public GameObject bodyParent;
    public GameObject[] bodyParts;
    private int bodyExistCount;
    private bool[] bodyExist;
    public GameObject spritePrefab;

    public float damagedVelocity;

    private float cryingTime = 0.4f;
    private Sprite defaultSprite;
    private Sprite cryingSprite;
    private Sprite smilingSprite;

    private float[] bodyLength;

    private float invincibleTime = 3.0f;
    private bool invincible;
    public GameObject invincibleCircle;

    public SystemMenuController systemMenu;

    private void Awake()
    {
        invincibleCircle.SetActive(false);
        invincible = false;
        bodyParts = new GameObject[bodyParent.transform.childCount];
        bodyLength = new float[bodyParent.transform.childCount];
        bodyExist = new bool[bodyParts.Length];
        bodyExistCount = bodyParts.Length;

        for (int i = 0; i < bodyExist.Length; i++)
        {
            bodyExist[i] = true;
        }

        for (int i = 0; i < bodyParent.transform.childCount; i++)
        {
            bodyParts[i] = bodyParent.transform.GetChild(i).gameObject;
        }

        for(int i = 0; i < bodyParts.Length; i++)
            bodyLength[i] = bodyParts[i].GetComponent<JointController>().maxDistance;

        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        cryingSprite = Resources.Load<Sprite>("Characters/0/h-1");
        smilingSprite = Resources.Load<Sprite>("Characters/0/h-2");
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        TouchedCry(2);
        GetComponent<Rigidbody2D>().isKinematic = false;
        foreach (var p in bodyParts)
        {
            if(p.activeSelf) p.GetComponent<JointController>().releaseHand();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            systemMenu.OnDead();
        }
        if(collision.gameObject.tag == "Reward")
        {
            onRewarded(collision.gameObject);
        }
        if (invincible) return;
        if(collision.gameObject.tag == "Enemy")
        {
            onDamaged(collision.bounds.ClosestPoint(transform.position));
        }
    }

    public void onRewarded(GameObject reward)
    {
        if (bodyExistCount == 5) return;
        else
        {
            reward.SetActive(false);
            bodyExistCount++;
            List<int> getIndex = new List<int>(6 - bodyExistCount);
            for (int i = 0; i < bodyParts.Length; i++)
            {
                if (!bodyExist[i])
                {
                    getIndex.Add(i);
                }
            }
            int getOne = getIndex[Random.Range(0, 6 - bodyExistCount)];
            bodyParts[getOne].transform.position = transform.position;
            bodyParts[getOne].SetActive(true);
            bodyParts[getOne].GetComponent<JointController>().mySpriteLayer.SetActive(true);
            bodyExist[getOne] = true;
        }
    }

    public void onDamaged(Vector3 collidePos)
    {
        foreach (var p in bodyParts)
        {
            p.GetComponent<Rigidbody2D>().isKinematic = false;
            p.GetComponent<Rigidbody2D>().velocity = damagedVelocity * (transform.position - p.transform.position);
        }
        GetComponent<Rigidbody2D>().velocity = damagedVelocity * (transform.position - collidePos);
        bodyExistCount--;
        if(bodyExistCount == -1) systemMenu.OnDead();
        else
        {
            invincible = true;
            StartCoroutine(nameof(damageWait));
            StartCoroutine(nameof(invincibleShine));
            List<int> disappearIndex = new List<int>(bodyExistCount + 1);
            for(int i = 0; i < bodyParts.Length; i++)
            {
                if (bodyExist[i])
                {
                    disappearIndex.Add(i);
                }
            }
            int disappearOne = disappearIndex[Random.Range(0, bodyExistCount + 1)];
            if (bodyParts[disappearOne].transform.parent != bodyParent.transform)
                bodyParts[disappearOne].GetComponent<JointController>().releaseFromConnectedObj(bodyParent.transform);
            bodyParts[disappearOne].SetActive(false);
            bodyParts[disappearOne].GetComponent<JointController>().mySpriteLayer.SetActive(false);
            bodyExist[disappearOne] = false;
        }
    }

    private IEnumerator damageWait()
    {
        yield return new WaitForSecondsRealtime(invincibleTime);
        invincible = false;
    }

    private IEnumerator invincibleShine()
    {
        while (invincible)
        {
            if (invincibleCircle.activeSelf)
                invincibleCircle.SetActive(false);
            else invincibleCircle.SetActive(true);
            yield return new WaitForSecondsRealtime(0.4f);
        }
        invincibleCircle.SetActive(false);
    }

    public void TouchedCry(int status)
    {
        StopCoroutine(nameof(touchedCrying));
        if(status == 1) GetComponent<SpriteRenderer>().sprite = cryingSprite;
        if(status == 2) GetComponent<SpriteRenderer>().sprite = smilingSprite;
        StartCoroutine(nameof(touchedCrying));
    }

    private IEnumerator touchedCrying()
    {
        yield return new WaitForSecondsRealtime(cryingTime);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
