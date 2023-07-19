using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCollideController : MonoBehaviour
{
    public string TextCH;
    public string TextEN;
    public bool touched;

    private GameManager.Language language;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        language = GameManager.language;
        touched = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (touched) return;
        if (collision.tag == "Player")
        {
            if(GameManager.language == GameManager.Language.CH)
                transform.parent.GetComponent<SpeechController>().speech(TextCH);
            else transform.parent.GetComponent<SpeechController>().speech(TextEN);
            touched = true;
        }
    }
}
