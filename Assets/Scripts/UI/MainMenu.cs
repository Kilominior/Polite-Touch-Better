using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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

    private void Start()
    {
        levelChoosePage.SetActive(false);
        startBtn.onClick.AddListener(() =>
        {
            levelChoosePage.SetActive(true);
        });

        ExitBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//如果是在unity编译器中
#else
            Application.Quit();//否则在打包文件中
#endif
        });

        if(GameManager.language == GameManager.Language.CH)
        {
            startText0.text = "关卡选择";
            startText1.text = "关卡选择";
            exitText0.text = "退出游戏";
            exitText1.text = "退出游戏";
            languageText0.text = "中文/English";
            languageText1.text = "中文/English";
        }
        else if(GameManager.language == GameManager.Language.EN)
        {
            startText0.text = "Levels";
            startText1.text = "Levels";
            exitText0.text = "Exit";
            exitText1.text = "Exit";
            languageText0.text = "English/中文";
            languageText1.text = "English/中文";
        }

        languageBtn.onClick.AddListener(() =>
        {
            if(GameManager.language == GameManager.Language.EN)
                GameManager.language = GameManager.Language.CH;
            else GameManager.language = GameManager.Language.EN;
            SceneManager.LoadScene(0);
        });

        level0Btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        level1Btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });
        level2Btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(3);
        });
        level3Btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(4);
        });
    }
}
