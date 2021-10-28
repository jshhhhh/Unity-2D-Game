using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;
    //카메라가 따라갈 대상
    public GameObject target;
    //카메라의 속도
    public float moveSpeed;
    //대상의 현재 위치값
    private Vector3 targetPosition;

    public BoxCollider2D bound;
    //박스 콜라이더 영역의 최소 최대 xyz값을 지님
    private Vector3 minBound;
    private Vector3 maxBound;

    //카메라가 맵의 끝에 다다르면 필요없는 부분까지 비추기 되므로
    //반너비, 반높이만큼 가감할 때 필요한 변수
    private float halfWidth;
    private float halfHeight;

    //카메라의 반높이값을 구할 속성을 이용하기 위한 변수
    private Camera theCamera;


    //Start 함수보다 더 먼저 실행되는 함수
    private void Awake() {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
        //이 오브젝트를 다른 씬을 불러올 때마다 파괴시키지 말라는 명령어
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        //bound 변수의 bounds(영역)의 최솟값, 최댓값을 대입
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        //카메라의 orthographicSize(반너비)
        halfHeight = theCamera.orthographicSize;
        //반너비를 구하는 공식
        halfWidth = halfHeight * Screen.width / Screen.height;
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

            //Clamp(): (값, 최솟값, 최댓값), 값이 최솟값과 최댓값 사이에 있으면 값이 리턴되고, 최솟값보다 아래면 최솟값, 최댓값보다 위면 최댓값이 리턴됨
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            //카메라 좌표
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }        
    }

    //맵을 넘어가면 기존의 bound를 newBound로 교체함
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        //newBound의 새로운 값을 넣어줌
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
