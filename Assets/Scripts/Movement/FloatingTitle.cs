using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTitle : MonoBehaviour
{
    Vector3 trans1;//记录原位置
    Vector2 trans2;//简谐运动变化的位置，计算得出

    public float zhenFu = 10f;//振幅
    public float HZ = 1f;//频率

    public bool axisY = true;

    private void Awake()
    {
        trans1 = transform.position;
    }

    private void Update()
    {
        trans2 = trans1;
        if(axisY) trans2.y = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + trans1.y;
        else trans2.x = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + trans1.x;

        transform.position = trans2;
    }
}
