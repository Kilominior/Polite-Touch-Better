using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTitle : MonoBehaviour
{
    public Vector3 trans1;//记录原位置
    Vector2 trans2;//简谐运动变化的位置，计算得出

    public float zhenFu = 10f;//振幅
    public float HZ = 1f;//频率

    public bool aliveWhilePause = false;
    public bool axisY = true;

    private void Awake()
    {
        if (!aliveWhilePause) trans1 = transform.position;
    }

    private void Update()
    {
        if (Time.timeScale == 0 && !aliveWhilePause) return;
        trans2 = trans1;
        if(axisY) trans2.y = Mathf.Sin(Time.realtimeSinceStartup * Mathf.PI * HZ) * zhenFu + trans1.y;
        else trans2.x = Mathf.Sin(Time.realtimeSinceStartup * Mathf.PI * HZ) * zhenFu + trans1.x;

        if(aliveWhilePause) transform.localPosition = trans2;
        else transform.position = trans2;
    }
}
