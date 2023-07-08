using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startBtn;
    public Button ExitBtn;
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
