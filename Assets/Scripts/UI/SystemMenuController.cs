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
        OnPause();
        finishMenu.SetActive(true);
    }

    public void OnDead()
    {
        StartCoroutine(nameof(deadWait));
    }

    private IEnumerator deadWait()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        OnPause();
        deadMenu.SetActive(true);
    }
}

