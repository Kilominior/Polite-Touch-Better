using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandFollower : MonoBehaviour
{
    public float touchVelocity;
    private Vector2 distance;
    public bool autoFollowing { private set; get; }
    public Transform waitingPos;
    private DirFollower dirFollower;
    private Sprite[] DirSprites;

    public Button modeChangeBtn;
    public Sprite mode1;
    public Sprite mode2;
    public Text modeNowText;
    public Text modeNowText1;

    public SystemMenuController systemMenu;
    public MainBodyController mainBody;
    GameManager.Language gameLanguage;

    private void Start()
    {
        gameLanguage = GameManager.language;
        DirSprites = new Sprite[2];
        DirSprites[0] = Resources.Load<Sprite>("LevelObjects/Dir");
        DirSprites[1] = Resources.Load<Sprite>("LevelObjects/DirSoft");
        dirFollower = GetComponent<DirFollower>();
        keepFollowing();
        modeChangeBtn.onClick.AddListener(() =>
        {
            if (autoFollowing)
            {
                systemMenu.AudioPlay(0);
                waitOutside();
            }
            else
            {
                systemMenu.AudioPlay(1);
                keepFollowing();
            }
        });
    }

    private void Update()
    {
        //if (Time.timeScale == 0) waitOutside();
        if (Input.GetMouseButtonDown(1))
        {
            if (autoFollowing)
            {
                systemMenu.AudioPlay(0);
                waitOutside();
            }
            else
            {
                systemMenu.AudioPlay(1);
                keepFollowing();
            }
        }
        if (autoFollowing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.GetPoint(10);
        }
    }

    public void keepFollowing()
    {
        autoFollowing = true;
        mainBody.isRudeTouch = true;
        modeChangeBtn.image.sprite = mode1;
        if (gameLanguage == GameManager.Language.CH)
        {
            modeNowText.text = "无理乱碰：幻肢弹飞";
            modeNowText1.text = "无理乱碰：幻肢弹飞";
        }
        else
        {
            modeNowText.text = "Rude Touch: Bounce";
            modeNowText1.text = "Rude Touch: Bounce";
        }
        foreach (var d in dirFollower.dirs)
            d.GetComponent<SpriteRenderer>().sprite = DirSprites[0];
    }

    public void waitOutside()
    {
        autoFollowing = false;
        mainBody.isRudeTouch = false;
        transform.position = waitingPos.position;
        modeChangeBtn.image.sprite = mode2;
        if (gameLanguage == GameManager.Language.CH)
        {
            modeNowText.text = "诚意轻抚：幻肢松开";
            modeNowText1.text = "诚意轻抚：幻肢松开";
        }
        else
        {
            modeNowText.text = "Polite Touch: Let Go";
            modeNowText1.text = "Polite Touch: Let Go";
        }
        foreach (var d in dirFollower.dirs)
            d.GetComponent<SpriteRenderer>().sprite = DirSprites[1];
    }

    /*//��ק���ܵ�ʵ�֣��ú�����Ӧ�õ��κνű���
    IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//��ά��������ת��Ļ����
                                                                                 //�������Ļ����תΪ��ά���꣬�ټ�������λ�������֮��ľ���
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
        if (collision.GetComponent<MainBodyController>())
        {
            collision.GetComponent<MainBodyController>().TouchedCry(1);
            collision.GetComponent<Rigidbody2D>().velocity = touchVelocity * distance / 6.0f;
        }
        else if(collision.GetComponent<JointController>())
        {
            collision.GetComponent<SpringJoint2D>().connectedBody.GetComponent<MainBodyController>().TouchedCry(1);
            collision.GetComponent<Rigidbody2D>().velocity = touchVelocity * distance;
        }
    }
}
