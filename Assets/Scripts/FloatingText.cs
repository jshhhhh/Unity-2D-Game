using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    public Text text;

    private Vector3 vector;

    // Update is called once per frame
    void Update()
    {
        //Update(): 매 프레임마다 실행됨
        //Time.deltaTime: 1초에 실행되는 프레임 개수의 역수
        //결국 1초에 movespeed만큼 y 움직임
        vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);
        //벡터 대입(움직임)
        text.transform.position = vector;

        //1초에 1씩 감소하게
        destroyTime -= Time.deltaTime;

        if(destroyTime <= 0)
            Destroy(this.gameObject);
    }
}
