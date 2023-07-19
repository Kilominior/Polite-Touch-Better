using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemMenuController: MonoBehaviour
{
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

    public GameObject finishMenu;

    public Text TitleText1;
    public Text MenuText1;
    public Button BackMainMenuBtn1;
    public Button RetryBtn1;
    public Button NextLevelBtn;

    public GameObject deadMenu;

    public Text TitleText2;
    public Text MenuText2;
    public Button BackMainMenuBtn2;
    public Button RetryBtn2;

    void Start()
    {
        Debug.Log("<场景加载>:当前场景为" + SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        finishMenu.SetActive(false);
        deadMenu.SetActive(false);


        PauseBtn.onClick.AddListener(() =>
        {
            if (pauseMenu.activeSelf)
            {
                OnResume();
                pauseMenu.SetActive(false);
                ScreenMask.SetActive(false);
                return;
            }
            OnPause();
            pauseMenu.SetActive(true);
            ScreenMask.SetActive(true);
        });

        ReturnBtn.onClick.AddListener(() =>
        {
            OnResume();
            pauseMenu.SetActive(false);
            ScreenMask.SetActive(false);
        });


        if (GameManager.language == GameManager.Language.EN)
        {
            TitleText0.text = "MENU";
            musicText.text = "music";
            audioText.text = "sfx";

            TitleText1.text = "Congratulations!";
            MenuText1.text = "You Finished Level" + SceneManager.GetActiveScene().buildIndex
                + "! \nHe Should Be Grateful!";

            TitleText2.text = "Failed...";
            MenuText2.text = "He Needs Your Help! \nTry Again?";

            BackMainMenuBtn0.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn1.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";
            BackMainMenuBtn2.transform.GetChild(0).GetComponent<Text>().text = "Main Menu";

            RetryBtn0.transform.GetChild(0).GetComponent<Text>().text = "Try Again";
            RetryBtn1.transform.GetChild(0).GetComponent<Text>().text = "Try Again";
            RetryBtn2.transform.GetChild(0).GetComponent<Text>().text = "Try Again";

            ReturnBtn.transform.GetChild(0).GetComponent<Text>().text = "Continue";
            NextLevelBtn.transform.GetChild(0).GetComponent<Text>().text = "Next Level";
        }
        else if (GameManager.language == GameManager.Language.CH)
        {
            TitleText0.text = "菜单";
            musicText.text = "音乐";
            audioText.text = "音效";

            TitleText1.text = "过关！";
            MenuText1.text = "恭喜通过第" + SceneManager.GetActiveScene().buildIndex
                + "关！\n就知道你是真心想帮忙！";

            TitleText2.text = "失败...";
            MenuText2.text = "他需要你的帮助，\n再试一次吧？";

            BackMainMenuBtn0.transform.GetChild(0).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn1.transform.GetChild(0).GetComponent<Text>().text = "主菜单";
            BackMainMenuBtn2.transform.GetChild(0).GetComponent<Text>().text = "主菜单";

            RetryBtn0.transform.GetChild(0).GetComponent<Text>().text = "重试";
            RetryBtn1.transform.GetChild(0).GetComponent<Text>().text = "重试";
            RetryBtn2.transform.GetChild(0).GetComponent<Text>().text = "重试";

            ReturnBtn.transform.GetChild(0).GetComponent<Text>().text = "继续游戏";
            NextLevelBtn.transform.GetChild(0).GetComponent<Text>().text = "下一关";
        }


        // 暂停弹窗
        //TitleText0 = "";

        BackMainMenuBtn0.onClick.AddListener(() =>
        {
            ToMainMenu();
        });

        RetryBtn0.onClick.AddListener(() =>
        {
            OnRestart();
        });


        // 过关弹窗
        //TitleText1 = "";
        //MenuText1 = "";

        BackMainMenuBtn1.onClick.AddListener(() =>
        {
            ToMainMenu();
        });

        RetryBtn1.onClick.AddListener(() =>
        {
            OnRestart();
        });

        NextLevelBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });


        // 死亡弹窗
        BackMainMenuBtn2.onClick.AddListener(() =>
        {
            ToMainMenu();
        });

        RetryBtn2.onClick.AddListener(() =>
        {
            OnRestart();
        });


        AudioSlider.value = GameManager.audioVolume;
        SfxSlider.value = GameManager.sfxVolume;
        audioSource.volume = GameManager.audioVolume;
        //sfxSource.volume = GameManager.sfxVolume;

        AudioSlider.onValueChanged.AddListener((float value) =>
        {
            GameManager.audioVolume = value;
            audioSource.volume = value;
        });

        SfxSlider.onValueChanged.AddListener((float value) =>
        {
            GameManager.sfxVolume = value;
            //sfxSource.volume = value;
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
        SceneManager.LoadScene(0);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void OnFinish()
    {
        StartCoroutine(nameof(popWait), false);
    }

    public void OnDead()
    {
        StartCoroutine(nameof(popWait), true);
    }

    private IEnumerator popWait(bool isDead)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        OnPause();
        if (isDead) deadMenu.SetActive(true);
        else
        {
            if (GameManager.levelProgress <= SceneManager.GetActiveScene().buildIndex)
                GameManager.levelProgress++;
            finishMenu.SetActive(true);
        }
    }
}

