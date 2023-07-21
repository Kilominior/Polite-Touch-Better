using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject loadMask;
    public Button startBtn;
    public Text startText0;
    public Text startText1;
    public Button ExitBtn;
    public Text exitText0;
    public Text exitText1;
    public Button languageBtn;
    public Text languageText0;
    public Text languageText1;
    public GameObject levelChoosePage;
    public Button level0Btn;
    public Button level1Btn;
    public Button level2Btn;
    public Button level3Btn;
    public Image titleImage;
    public Sprite titleCH;
    public Sprite titleEN;
    public GameObject face;
    private List<Button> btns;
    private List<Vector3> btnPos;

    public AudioSource sfxSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        loadMask.SetActive(true);
        DOTween.Init();
        StartCoroutine(nameof(maskFade));
    }

    private IEnumerator maskFade()
    {
        loadMask.transform.localScale = Vector3.one;
        yield return new WaitForSecondsRealtime(1.0f);
        loadMask.transform.DOScale(60.0f, 1.0f).OnComplete(() => loadMask.SetActive(false));
    }

    private void Start()
    {
        btns = new List<Button>(4);
        btns.Add(level0Btn);
        btns.Add(level1Btn);
        btns.Add(level2Btn);
        btns.Add(level3Btn);
        btnPos = new List<Vector3>(4);
        foreach (var b in btns)
        {
            b.GetComponent<FloatingTitle>().trans1 = b.transform.localPosition;
            btnPos.Add(b.transform.localPosition);
        }

        audioClips = Resources.LoadAll<AudioClip>("Audio/SFX");

        levelChoosePage.SetActive(false);
        startBtn.onClick.AddListener(() =>
        {
            if (levelChoosePage.activeInHierarchy)
            {
                sfxPlay(7);
                for (int i = 0; i < 4; i++)
                {
                    btns[i].GetComponent<FloatingTitle>().enabled = false;
                    int q = i;
                    btns[q].transform.DOLocalMove(Vector3.zero, .4f).OnComplete(() =>
                    levelChoosePage.SetActive(false));
                }
            }
            else
            {
                sfxPlay(4);
                for (int i = 0; i < 4; i++)
                {
                    btns[i].transform.localPosition = Vector3.zero;
                    btns[i].GetComponent<FloatingTitle>().enabled = false;
                }
                levelChoosePage.SetActive(true);
                for (int i = 0; i < 4; i++)
                {
                    int q = i;
                    btns[q].transform.DOLocalMove(btnPos[q], .4f).OnComplete(() =>
                    btns[q].GetComponent<FloatingTitle>().enabled = true);
                }
            }
        });

        ExitBtn.onClick.AddListener(() =>
        {
            sfxPlay(0);
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//如果是在unity编译器中
#else
            Application.Quit();//否则在打包文件中
#endif
            });
        });

        if(GameManager.language == GameManager.Language.CH)
        {
            startText0.text = "关卡选择";
            startText1.text = "关卡选择";
            exitText0.text = "退出游戏";
            exitText1.text = "退出游戏";
            languageText0.text = "中文/English";
            languageText1.text = "中文/English";
            titleImage.sprite = titleCH;
        }
        else if(GameManager.language == GameManager.Language.EN)
        {
            startText0.text = "Levels";
            startText1.text = "Levels";
            exitText0.text = "Exit";
            exitText1.text = "Exit";
            languageText0.text = "English/中文";
            languageText1.text = "English/中文";
            titleImage.sprite = titleEN;
        }

        languageBtn.onClick.AddListener(() =>
        {
            sfxPlay(4);
            if (GameManager.language == GameManager.Language.EN)
                GameManager.language = GameManager.Language.CH;
            else
                GameManager.language = GameManager.Language.EN; 
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
                SceneManager.LoadScene(0);
            });
        });

        if(GameManager.levelProgress == 1)
        {
            level1Btn.interactable = false;
            level2Btn.interactable = false;
            level3Btn.interactable = false;
            level1Btn.transform.GetChild(0).gameObject.SetActive(false);
            level2Btn.transform.GetChild(0).gameObject.SetActive(false);
            level3Btn.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (GameManager.levelProgress == 2)
        {
            level2Btn.interactable = false;
            level3Btn.interactable = false;
            level2Btn.transform.GetChild(0).gameObject.SetActive(false);
            level3Btn.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (GameManager.levelProgress == 3)
        {
            level3Btn.interactable = false;
            level3Btn.transform.GetChild(0).gameObject.SetActive(false);
        }

        level0Btn.onClick.AddListener(() =>
        {
            sfxPlay(0);
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
                SceneManager.LoadScene(1);
            });
        });
        level1Btn.onClick.AddListener(() =>
        {
            sfxPlay(0);
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
                SceneManager.LoadScene(2);
            });
        });
        level2Btn.onClick.AddListener(() =>
        {
            sfxPlay(0);
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
                SceneManager.LoadScene(3);
            });
        });
        level3Btn.onClick.AddListener(() =>
        {
            sfxPlay(0);
            loadMask.SetActive(true);
            loadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
                SceneManager.LoadScene(4);
            });
        });

        face.GetComponent<Button>().onClick.AddListener(() =>
        {
            sfxPlay(Random.Range(4, 7));
        });


        GetComponent<AudioSource>().volume = GameManager.audioVolume;
        sfxSource.volume = GameManager.sfxVolume;
    }

    private void Update()
    {
        face.GetComponent<LineRenderer>().SetPosition(2, Vector3.zero);
        face.GetComponent<LineRenderer>().SetPosition(1, (Vector3.zero + 
            titleImage.transform.GetChild(0).transform.position) / 2);
        face.GetComponent<LineRenderer>().SetPosition(0, titleImage.transform.GetChild(0).transform.position);
        foreach(var b in btns)
        {
            b.GetComponent<LineRenderer>().SetPosition(2, Vector3.zero);
            b.GetComponent<LineRenderer>().SetPosition(1, (Vector3.zero +
                b.transform.position) / 2);
            b.GetComponent<LineRenderer>().SetPosition(0, b.transform.position);
        }
    }

    private void sfxPlay(int sfxID)
    {
        sfxSource.clip = audioClips[sfxID];
        sfxSource.Play();
    }
}
