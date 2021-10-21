using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //카메라가 따라갈 대상
    public GameObject target;
    //카메라의 속도
    public float moveSpeed;
    //대상의 현재 위치값
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //대상이 존재한다면
        if(target.gameObject != null)
        {
            //대상의 현재 위치값
            targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);

            //카메라를 움직임(Lerp: A값과 B값까지 t의 속도로 움직임)
            //1초에 moveSpeed만큼 이동
            //Time.deltaTime: 1초에 60프레임이 실행된다면, 60분의 1값을 지님
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        
    }
}
