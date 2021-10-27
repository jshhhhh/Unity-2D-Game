using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    //맵이 이동되면 플레이어가 시작될 위치
    public string startPoint;

    //Player의 currentMapName을 꺼내오기 위해
    private MovingObject thePlayer;

    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<MovingObject>();

        if(startPoint == thePlayer.currentMapName)
        {
            
            //카메라 순간이동
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
            //플레이어의 위치를 스타트 위치로 이동시킴
            thePlayer.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
