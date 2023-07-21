using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainBodyController : MonoBehaviour
{
    public GameObject bodyParent;
    public GameObject[] bodyParts;
    public int bodyExistCount { get; private set; }
    private bool[] bodyExist;
    public GameObject spritePrefab;

    public float damagedVelocity;

    private float cryingTime = 0.4f;
    private Sprite defaultSprite;
    private Sprite cryingSprite;
    private Sprite smilingSprite;
    private int faceNow;

    private float[] bodyLength;

    private float invincibleTime = 3.0f;
    private bool invincible;
    public GameObject invincibleCircle;

    private bool dead;

    public SystemMenuController systemMenu;
    public SpeechController speech;

    public bool isRudeTouch;

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
        faceNow = 0;

        dead = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (isRudeTouch) return;
        TouchedCry(2);
        GetComponent<Rigidbody2D>().isKinematic = false;
        foreach (var p in bodyParts)
        {
            if(p.activeSelf) p.GetComponent<JointController>().releaseHand();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dead) return;
        if (collision.gameObject.tag == "Respawn")
        {
            FaceChange(1);
            dead = true;
            systemMenu.OnDead(true);
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
            if(GameManager.language == GameManager.Language.EN)
            {
                if (bodyExistCount <= 1) speech.speech("I can't believe this! But it's real! There is still hope!", 2);
                if (bodyExistCount == 2) speech.speech("Here comes my rebirth! Having more body parts is so nice!", 2);
                if (bodyExistCount == 3) speech.speech("We are returning to the best! Having more body parts is sooo nice!", 2);
                else if (bodyExistCount == 4) speech.speech("I'm getting back to my peak state! Thank you very much!", 2);
            }
            if (GameManager.language == GameManager.Language.CH)
            {
                if (bodyExistCount <= 1) speech.speech("这...这......绝境逢生啊！还有希望！", 2);
                if (bodyExistCount == 2) speech.speech("重获新生！幻肢多的感觉就是好啊！", 2);
                if (bodyExistCount == 3) speech.speech("渐回佳境！幻肢多的感觉就是好啊！", 2);
                else if(bodyExistCount == 4) speech.speech("回到我的巅峰状态啦！真是多亏了你啊！", 2);
            }
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
        if (bodyExistCount == -1)
        {
            FaceChange(1);
            dead = true;
            systemMenu.OnDead(false);
        }
        else
        {
            invincible = true;
            if (GameManager.language == GameManager.Language.EN)
            {
                if (bodyExistCount == 4) speech.speech("It seems that I can't fully recover now but... I know you can do it!", 1);
                else if (bodyExistCount == 3) speech.speech("To be honest, less body parts means better control, right?", 1);
                else if (bodyExistCount == 2) speech.speech("Ohhh! Take it easy friend! I... I want my mom!", 1);
                else if (bodyExistCount == 1) speech.speech("It's difficult to stand with one last body part, maybe the only choice now is to swing...", 1);
                else speech.speech("A drop in the ocean, drifting in the vast universe... Perhaps we can only start over again...", 1);
            }
            if (GameManager.language == GameManager.Language.CH)
            {
                if (bodyExistCount == 4) speech.speech("看来这下没法全身而退了...但，我知道你可以的！", 1);
                else if (bodyExistCount == 3) speech.speech("说实话，幻肢少点更好操作，是吧......", 1);
                else if (bodyExistCount == 2) speech.speech("嘿！悠着点啊！我...我开始害怕了......", 1);
                else if (bodyExistCount == 1) speech.speech("独木难支，接下来...可能只能用摆荡行动了......", 1);
                else speech.speech("沧海一粟，漂游在这茫茫宇宙......也许咱们只能重来了...", 1);
            }
            StartCoroutine(nameof(damageWait));
            StartCoroutine(nameof(invincibleShine));
            List<int> disappearIndex = new List<int>(bodyExistCount + 1);
            for (int i = 0; i < bodyParts.Length; i++)
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
        if (faceNow != 0) return;
        if(status == 1) GetComponent<SpriteRenderer>().sprite = cryingSprite;
        if(status == 2) GetComponent<SpriteRenderer>().sprite = smilingSprite;
        StartCoroutine(nameof(touchedCrying));
    }

    public void FaceChange(int status)
    {
        faceNow = status;
        if (status == 0) GetComponent<SpriteRenderer>().sprite = defaultSprite;
        if (status == 1) GetComponent<SpriteRenderer>().sprite = cryingSprite;
        if (status == 2) GetComponent<SpriteRenderer>().sprite = smilingSprite;
    }

    private IEnumerator touchedCrying()
    {
        yield return new WaitForSecondsRealtime(cryingTime);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
