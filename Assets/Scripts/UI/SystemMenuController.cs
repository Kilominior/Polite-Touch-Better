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
    public Slider MusicSlider;
    public Slider AudioSlider;

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
            MenuText1.text = "You Finished This Level! \nYou Have Received Pigment Supplies.";

            TitleText2.text = "He's Dead...";
            MenuText2.text = "Chameleons Need You! \nTry Again?";

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
            MenuText1.text = "你在补给点补充了颜料，\n继续前进吧！";

            TitleText2.text = "倒下...";
            MenuText2.text = "蜥裔的希望全在你身上了，\n打起精神再试一次吧？";

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
        if(isDead) deadMenu.SetActive(true);
        else finishMenu.SetActive(true);
    }
}

