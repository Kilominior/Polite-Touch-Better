using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }
        else
        {
            readingBoard.SetActive(false);
            speechBoard.SetActive(true);
        }
        StartCoroutine(nameof(speeching));
    }

    public IEnumerator speeching()
    {
        yield return new WaitForSecondsRealtime(speechTime);
        mainBody.FaceChange(0);
        speechText.gameObject.SetActive(false);
        speechBoard.SetActive(false);
        readingBoard.SetActive(false);
    }

}
