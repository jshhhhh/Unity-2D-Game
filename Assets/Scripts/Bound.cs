using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
