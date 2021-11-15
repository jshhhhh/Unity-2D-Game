using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    //플레이어가 바라보는 방향을 알기 위해
    private PlayerManager thePlayer;
    
    //회전(각도)를 담당하는 Vector4(x, y, z, w)
    private Vector2 vector;

    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어의 좌표를 손전등에 넘겨줌
        this.transform.position = thePlayer.transform.position;

        //플레이어가 바라보는 방향 불러옴
        vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));

        //오른쪽을 바라볼 때
        if(vector.x == 1f)
        {
            //Euler 함수로 각도 설정
            rotation = Quaternion.Euler(0, 0, 90);
            //이 값을 넘겨줌
            this.transform.rotation = rotation;
            //벡터로는 넘기기 불가
            // this.transform.rotation = new Vector4(0, 0, 90, 0);
        }
        //왼쪽을 바라볼 때
        else if(vector.x == -1f)
        {
            //Euler 함수로 각도 설정
            rotation = Quaternion.Euler(0, 0, -90);
            //이 값을 넘겨줌
            this.transform.rotation = rotation;
        }
        //위를 바라볼 때
        else if(vector.y == 1f)
        {
            //Euler 함수로 각도 설정
            rotation = Quaternion.Euler(0, 0, 180);
            //이 값을 넘겨줌
            this.transform.rotation = rotation;
        }
        //아래를 바라볼 때
        else if(vector.y == -1f)
        {
            //Euler 함수로 각도 설정
            rotation = Quaternion.Euler(0, 0, 0);
            //이 값을 넘겨줌
            this.transform.rotation = rotation;
        }
    }
}
