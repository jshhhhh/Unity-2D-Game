using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;
    //bound의 이름 기록 목적
    public string boundName;
    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        //bound의 정보와 카메라의 정보를 가져옴
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
        //새로운 씬에서의 bound를 넘겨줌
        theCamera.SetBound(bound);
    }

    public void SetBound()
    {
        //카메라가 있을 경우
        if(theCamera != null)
        {
            theCamera.SetBound(bound);
        }
    }
}
