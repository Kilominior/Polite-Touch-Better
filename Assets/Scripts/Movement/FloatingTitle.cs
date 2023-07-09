using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTitle : MonoBehaviour
{
    Vector3 trans1;//��¼ԭλ��
    Vector2 trans2;//��г�˶��仯��λ�ã�����ó�

    public float zhenFu = 10f;//���
    public float HZ = 1f;//Ƶ��

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
