using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCollideController : MonoBehaviour
{
    public string TextCH;
    public string TextEN;
    public string TextCH1 = "";
    public string TextEN1 = "";
    public int faceChangeTo = 0;
    public bool toBeRead = false;
    public bool isReading;
    public bool touched;

    private GameManager.Language language;

    private void Start()
    {
        isReading = false;
        if (!toBeRead) GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        language = GameManager.language;
        touched = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (touched) return;
        if (isReading) return;
        if (collision.tag == "Player")
        {
            if(TextCH1 != "")
            {
                if(collision.GetComponent<MainBodyController>().bodyExistCount < 5)
                {
                    if (GameManager.language == GameManager.Language.CH)
                        transform.parent.GetComponent<SpeechController>().speech(TextCH1, 1);
                    else transform.parent.GetComponent<SpeechController>().speech(TextEN1, 1);
                }
                else
                {
                    if (GameManager.language == GameManager.Language.CH)
                        transform.parent.GetComponent<SpeechController>().speech(TextCH, 2);
                    else transform.parent.GetComponent<SpeechController>().speech(TextEN, 2);
                }
            }
            else
            {
                if (GameManager.language == GameManager.Language.CH)
                    transform.parent.GetComponent<SpeechController>().speech(TextCH, faceChangeTo, toBeRead);
                else transform.parent.GetComponent<SpeechController>().speech(TextEN, faceChangeTo, toBeRead);
            }
            if (!toBeRead) touched = true;
            else StartCoroutine(nameof(readWait));
        }
    }

    private IEnumerator readWait()
    {
        isReading = true;
        yield return new WaitForSecondsRealtime(4.0f);
        isReading = false;
    }
}
