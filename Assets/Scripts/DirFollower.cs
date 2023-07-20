using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirFollower : MonoBehaviour
{
    public GameObject DirPrefab;

    public GameObject DirParent;

    public GameObject[] dirs;

    public MainBodyController mainBodyController;

    public GameObject[] followingObjs;

    private Vector2 cusorDir;

    private void Start()
    {
        followingObjs = mainBodyController.bodyParts;
        dirs = new GameObject[followingObjs.Length];
        for(int i = 0; i< followingObjs.Length; i++)
        {
            dirs[i] = Instantiate(DirPrefab, DirParent.transform.position,
                Quaternion.identity, DirParent.transform);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        for(int i = 0; i < dirs.Length; i++)
        {
            if (!followingObjs[i].activeInHierarchy && dirs[i].activeInHierarchy)
            {
                dirs[i].SetActive(false);
                continue;
            }
            if (followingObjs[i].activeInHierarchy && !dirs[i].activeInHierarchy)
                dirs[i].SetActive(true);
            dirs[i].transform.position = followingObjs[i].transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            cusorDir = dirs[i].transform.position - ray.GetPoint(10);
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, cusorDir);
            dirs[i].transform.rotation = quaternion;
        }
    }
}
