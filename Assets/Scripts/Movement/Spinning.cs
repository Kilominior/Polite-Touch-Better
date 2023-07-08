using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    private float defaultScale;
    public float rotateSpeed = 1.0f;
    public bool isClockwise = true;

    public bool withScale = false;
    public float minSize = 0.1f;//Õñ·ù
    public float HZ = 0.5f;//ÆµÂÊ

    private void Start()
    {
        defaultScale = transform.localScale.x;
    }

    void Update()
    {
        if(isClockwise) transform.Rotate(Vector3.forward, -rotateSpeed);
        else transform.Rotate(Vector3.up, rotateSpeed);
        if (withScale)
        {
            float scaleNow = (Mathf.Sin(Time.fixedTime * Mathf.PI * HZ)) * minSize + defaultScale;
            transform.localScale = new Vector3(scaleNow, scaleNow, scaleNow);
        }
    }
}
