using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBodyController : MonoBehaviour
{
    public GameObject bodyParent;
    public GameObject[] bodyParts;

    public GameObject needle;
    public Button needleBtn;

    private float[] bodyLength;

    private void Start()
    {
        bodyParts = new GameObject[bodyParent.transform.childCount];
        bodyLength = new float[bodyParent.transform.childCount];
        for (int i = 0; i < bodyParent.transform.childCount; i++)
        {
            bodyParts[i] = bodyParent.transform.GetChild(i).gameObject;
        }
        for(int i = 0; i < bodyParts.Length; i++)
            bodyLength[i] = bodyParts[i].GetComponent<JointController>().maxDistance;
        needleBtn.onClick.AddListener(() =>
        {
            Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            var offset = transform.position - Camera.main.ScreenToWorldPoint(new
                Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            needle.transform.position = curPosition;
            needle.SetActive(true);
        });
    }

    private void OnMouseDown()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        foreach(var p in bodyParts) p.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
