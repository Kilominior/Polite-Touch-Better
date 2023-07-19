using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    public GameObject speechBoard;
    public Text speechText;
    private float speechTime = 4.0f;

    // 说话方法
    public void speech(string sText)
    {
        StopCoroutine(nameof(speeching));
        speechText.text = sText;
        speechBoard.SetActive(true);
        speechText.gameObject.SetActive(true);
        StartCoroutine(nameof(speeching));
    }

    public IEnumerator speeching()
    {
        yield return new WaitForSecondsRealtime(speechTime);
        speechText.gameObject.SetActive(false);
        speechBoard.SetActive(false);
    }

}
