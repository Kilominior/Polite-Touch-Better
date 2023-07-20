using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpeechController : MonoBehaviour
{
    public GameObject speechBoard;
    public GameObject readingBoard;
    public Text speechText;
    public MainBodyController mainBody;
    private float speechTime = 4.0f;

    // 说话方法
    public void speech(string sText, int faceChangeTo = 0, bool isReading = false)
    {
        StopCoroutine(nameof(speeching));
        mainBody.FaceChange(faceChangeTo);
        speechText.text = sText;
        speechText.gameObject.SetActive(true);
        if (isReading)
        {
            speechBoard.SetActive(false);
            readingBoard.SetActive(true);
            readingBoard.transform.DOShakeScale(.1f, 0.5f, 8, 45.0f).OnComplete(() =>
            readingBoard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f));
        }
        else
        {
            readingBoard.SetActive(false);
            speechBoard.SetActive(true);
            speechBoard.transform.DOShakeScale(.1f, 0.5f, 8, 45.0f).OnComplete(() =>
            speechBoard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f));
        }
        StartCoroutine(nameof(speeching));
    }

    public IEnumerator speeching()
    {
        bool waitPause = false;
        yield return new WaitForSecondsRealtime(speechTime);
        while(Time.timeScale == 0)
        {
            waitPause = true;
            yield return new WaitForFixedUpdate();
        }
        if(waitPause) yield return new WaitForSecondsRealtime(speechTime);
        mainBody.FaceChange(0);
        speechText.gameObject.SetActive(false);
        speechBoard.SetActive(false);
        readingBoard.SetActive(false);
    }

}
