using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleBirth : MonoBehaviour
{
    public GameObject needle;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        needle.transform.position = ray.GetPoint(10);
    }

    private void OnMouseDown()
    {
        Debug.Log("444");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        needle.transform.position = ray.GetPoint(10);
        needle.SetActive(true);
    }
}
