using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SystemMenuController: MonoBehaviour
{
    public GameObject LoadMask;

    public Button PauseBtn;

    public GameObject ScreenMask;

    public GameObject pauseMenu;

    public Text TitleText0;
    public Text musicText;
    public Text audioText;
    public Button BackMainMenuBtn0;
    public Button RetryBtn0;
    public Button ReturnBtn;
    public Slider AudioSlider;
    public Slider SfxSlider;
    public AudioSource audioSource;
    public AudioSource sfxSource;
    public AudioSource speechSource;
    public GameObject musicBoard;
    public GameObject speechBoard0;

    public GameObject finishMenu;

    public Text TitleText1;
    public Text MenuText1;
    public Button BackMainMenuBtn1;
    public Button RetryBtn1;
    public Button NextLevelBtn;
    public GameObject speechBoard1;

    public GameObject deadMenu;

    public Text TitleText2;
    public Text MenuText2;
    public Button BackMainMenuBtn2;
    public Button RetryBtn2;
    public GameObject speechBoard2;

    private List<Button> btns;
    private List<Vector3> btnPos;

    private bool gameOver;

    public AudioClip[] audioClips;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        LoadMask.SetActive(true);
        StartCoroutine(nameof(maskFade));
    }

    private IEnumerator maskFade()
    {
        yield return new WaitForSecondsRealtime(.5f);
        LoadMask.transform.DOScale(60.0f, 1.0f).OnComplete(() => LoadMask.SetActive(false));
    }

    void Start()
    {
        Debug.Log("<场景加载>:当前场景为" + SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        gameOver = false;
        pauseMenu.SetActive(false);
        finishMenu.SetActive(false);
        deadMenu.SetActive(false);

        btns = new List<Button>(8);
        btns.Add(BackMainMenuBtn0);
        btns.Add(BackMainMenuBtn1);
        btns.Add(BackMainMenuBtn2);
        btns.Add(RetryBtn0);
        btns.Add(RetryBtn1);
        btns.Add(RetryBtn2);
        btns.Add(ReturnBtn);
        btns.Add(NextLevelBtn);

        btnPos = new List<Vector3>(8);
        foreach (var b in btns)
        {
            b.GetComponent<FloatingTitle>().trans1 = b.transform.localPosition;
            btnPos.Add(b.transform.localPosition);
        }

        audioClips = Resources.LoadAll<AudioClip>("Audio/SFX");

        PauseBtn.onClick.AddListener(() =>
        {
            if (gameOver) return;
            if (pauseMenu.activeSelf)
            {
                AudioPlay(1);
                OnResume();
                musicBoard.transform.DOLocalMoveY(-1000.0f, .1f).OnComplete(() => pauseMenu.SetActive(false));
                return;
            }

            AudioPlay(0);
            musicBoard.transform.localPosition = new(0.0f, -1000.0f);
            for (int i = 0; i < 8; i++)
            {
                btns[i].transform.localPosition = Vector3.zero;
                btns[i].GetComponent<FloatingTitle>().enabled = false;
            }
            pauseMenu.SetActive(true);
            for (int i = 0; i < 8; i++)
            {
                int q = i;
                btns[q].transform.DOLocalMove(btnPos[q], .1f).OnComplete(() =>
                btns[q].GetComponent<FloatingTitle>().enabled = true);
            }
            speechBoard0.transform.DOShakeScale(.1f, 0.5f, 8, 45.0f);
            musicBoard.transform.DOLocalMoveY(-500.0f, .1f).OnComplete(() => OnPause());
        });

        ReturnBtn.onClick.AddListener(() =>
        {
            AudioPlay(1);
            OnResume();
            musicBoard.transform.DOLocalMoveY(-1000.0f, .1f).OnComplete(() => pauseMenu.SetActive(false));
        });


        if (GameManager.language == GameManager.Language.EN)
        {
            TitleText0.text = "Pause!\nI will always be here, waiting for your coming back!";
            musicText.text = "music";
            audioText.text = "sfx";

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    TitleText1.text = "Level 1 clear!\nReady to face more challanges?";
                    break;
                case 2:
                    TitleText1.text = "Level 2 clear!\nJust move on!";
                    break;
                case 3:
                    TitleText1.text = "Level 3 clear!\nNext is the last level, Come on!";
                    break;
                case 4:
                    TitleText1.text = "Levels all clear!\nThank you for playing the game! Soon there will be more levels!";
                    break;
            }
            //MenuText1.text = "You Finished Level" + SceneManager.GetActiveScene().buildIndex
            //    + "! \nHe Should Be Grateful!";

            TitleText2.text = "Failed...";
            //MenuText2.text = "He Needs Your Help! \nTry Again?";

            BackMainMenuBtn0.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn1.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn2.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn0.transform.GetChild(1).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn1.transform.GetChild(1).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn2.transform.GetChild(1).GetComponent<Text>().text = "Main Menu";

            RetryBtn0.transform.GetChild(0).GetComponent<Text>().text = "Try Again";
            RetryBtn1.transform.GetChild(0).GetComponent<Text>().text = "Try Again";
            RetryBtn2.transform.GetChild(0).GetComponent<Text>().text = "Try Again";
            RetryBtn0.transform.GetChild(1).GetComponent<Text>().text = "Try Again";
            RetryBtn1.transform.GetChild(1).GetComponent<Text>().text = "Try Again";
            RetryBtn2.transform.GetChild(1).GetComponent<Text>().text = "Try Again";

            ReturnBtn.transform.GetChild(0).GetComponent<Text>().text = "Continue";
            NextLevelBtn.transform.GetChild(0).GetComponent<Text>().text = "Next Level";
            ReturnBtn.transform.GetChild(1).GetComponent<Text>().text = "Continue";
            NextLevelBtn.transform.GetChild(1).GetComponent<Text>().text = "Next Level";
        }
        else if (GameManager.language == GameManager.Language.CH)
        {
            TitleText0.text = "暂停啦！\n我一直在呢，什么时候回来都行！";
            musicText.text = "音乐";
            audioText.text = "音效";

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    TitleText1.text = "过关啦！\n以后的关卡也多多关照啦！";
                    break;
                case 2:
                    TitleText1.text = "过关啦！\n我们继续前进吧！";
                    break;
                case 3:
                    TitleText1.text = "过关啦！\n接下来就是最后的一关了，加油哇！";
                    break;
                case 4:
                    TitleText1.text = "到这里就通关啦！\n感谢游玩！后续还会有更多关卡加入，敬请期待！";
                    break;
            }
            //MenuText1.text = "恭喜通过第" + SceneManager.GetActiveScene().buildIndex
            //    + "关！\n就知道你是真心想帮忙！";

            TitleText2.text = "失败...";
            //MenuText2.text = "他需要你的帮助，\n再试一次吧？";

            BackMainMenuBtn0.transform.GetChild(0).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn1.transform.GetChild(0).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn2.transform.GetChild(0).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn0.transform.GetChild(1).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn1.transform.GetChild(1).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn2.transform.GetChild(1).GetComponent<Text>().text = "主菜单";

            RetryBtn0.transform.GetChild(0).GetComponent<Text>().text = "重试";
            RetryBtn1.transform.GetChild(0).GetComponent<Text>().text = "重试";
            RetryBtn2.transform.GetChild(0).GetComponent<Text>().text = "重试";
            RetryBtn0.transform.GetChild(1).GetComponent<Text>().text = "重试";
            RetryBtn1.transform.GetChild(1).GetComponent<Text>().text = "重试";
            RetryBtn2.transform.GetChild(1).GetComponent<Text>().text = "重试";

            ReturnBtn.transform.GetChild(0).GetComponent<Text>().text = "继续游戏";
            NextLevelBtn.transform.GetChild(0).GetComponent<Text>().text = "下一关";
            ReturnBtn.transform.GetChild(1).GetComponent<Text>().text = "继续游戏";
            NextLevelBtn.transform.GetChild(1).GetComponent<Text>().text = "下一关";
        }


        // 暂停弹窗
        //TitleText0 = "";

        BackMainMenuBtn0.onClick.AddListener(() =>
        {
            AudioPlay(1);
            ToMainMenu();
        });

        RetryBtn0.onClick.AddListener(() =>
        {
            AudioPlay(0);
            OnRestart();
        });


        // 过关弹窗
        //TitleText1 = "";
        //MenuText1 = "";

        BackMainMenuBtn1.onClick.AddListener(() =>
        {
            AudioPlay(1);
            ToMainMenu();
        });

        RetryBtn1.onClick.AddListener(() =>
        {
            AudioPlay(0);
            OnRestart();
        });

        NextLevelBtn.onClick.AddListener(() =>
        {
            AudioPlay(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });


        // 死亡弹窗
        BackMainMenuBtn2.onClick.AddListener(() =>
        {
            AudioPlay(1);
            ToMainMenu();
        });

        RetryBtn2.onClick.AddListener(() =>
        {
            AudioPlay(0);
            OnRestart();
        });


        AudioSlider.value = GameManager.audioVolume;
        SfxSlider.value = GameManager.sfxVolume;
        audioSource.volume = GameManager.audioVolume;
        sfxSource.volume = GameManager.sfxVolume;
        speechSource.volume = GameManager.sfxVolume;

        AudioSlider.onValueChanged.AddListener((float value) =>
        {
            GameManager.audioVolume = value;
            audioSource.volume = value;
        });

        SfxSlider.onValueChanged.AddListener((float value) =>
        {
            GameManager.sfxVolume = value;
            sfxSource.volume = value;
            speechSource.volume = value;
        });
    }

    public void OnPause()
    {
        ScreenMask.SetActive(true);
        //playerInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
    }

    public void OnResume()
    {
        ScreenMask.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        LoadMask.SetActive(true);
        LoadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
            SceneManager.LoadScene(0);
        });
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        if (LoadMask.activeInHierarchy)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        LoadMask.SetActive(true);
        LoadMask.transform.DOScale(1.0f, 1.0f).OnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    public void OnFinish()
    {
        gameOver = true;
        AudioPlay(9, true);
        StartCoroutine(nameof(popWait), false);
    }

    public void OnDead(bool isFallen)
    {
        gameOver = true;
        AudioPlay(8, true);
        if (isFallen)
        {
            if(GameManager.language == GameManager.Language.CH)
                TitleText2.text = "不好！越界了...吗......";
            else
                TitleText2.text = "OHHH NOOOOOO!";
        }
        else
        {
            if (GameManager.language == GameManager.Language.CH)
                TitleText2.text = "头疼......但还想...重来...";
            else
                TitleText2.text = "Oh my head! I want to... try again...";
        }
        StartCoroutine(nameof(popWait), true);
    }

    private IEnumerator popWait(bool isDead)
    {
        for (int i = 0; i < 8; i++)
        {
            btns[i].transform.localPosition = Vector3.zero;
            btns[i].GetComponent<FloatingTitle>().enabled = false;
        }
        if (isDead)
        {
            deadMenu.SetActive(true);
            speechBoard2.transform.DOShakeScale(.2f, 0.5f, 8, 45.0f);
        }
        else
        {
            if (GameManager.levelProgress <= SceneManager.GetActiveScene().buildIndex)
                GameManager.levelProgress++;
            finishMenu.SetActive(true);
            speechBoard1.transform.DOShakeScale(.2f, 0.5f, 8, 45.0f);
        }
        for (int i = 0; i < 8; i++)
        {
            int q = i;
            btns[q].transform.DOLocalMove(btnPos[q], .4f).OnComplete(() =>
            btns[q].GetComponent<FloatingTitle>().enabled = true);
        }
        yield return new WaitForSecondsRealtime(0.6f);
        OnPause();
    }

    private IEnumerator gameOverMenuClear()
    {
        while (gameOver)
        {
            pauseMenu.SetActive(false);
            yield return new WaitForFixedUpdate();
        }
    }


    /// <summary>
    /// 根据索引播放音乐，索引参见Resources/Audio/SFX文件夹
    /// </summary>
    public void AudioPlay(int musicIndex, bool isSpeech = false)
    {
        if (isSpeech)
        {
            speechSource.clip = audioClips[musicIndex];
            speechSource.Play();
            return;
        }
        if(musicIndex == 2)
        {
            sfxSource.clip = audioClips[Random.Range(10, 14)];
        }
        else sfxSource.clip = audioClips[musicIndex];
        sfxSource.Play();
    }
}

