using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    /*
    데이터베이스가 필요한 이유
    1. 지난 씬의 이벤트를 재발동하고 싶지 않지만 씬을 넘어가면 객체가 파괴되고 재생성되기 때문에 지난 씬을 재방문하면 변경된 내용이 전부 초기화됨(이벤트 재발동됨). 그래서 데이터베이스 스크립트를 일종의 전역 변수로 활용하여 위 현상을 방지
    2. 세이브와 로드를 기록
    3. 아이템(이름, id값, 설명)을 데이터베이스에 기술해놓고 가져다 쓰기만 하면 편함    
    */

    static public DatabaseManager instance;

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

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
        
    }
}
