using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    //다른 곳에서 호출할 수 있게 public static
    public static PlayerStat instance;

    public int character_Lv;
    public int[] needEXP;
    public int currentEXP;

    public int hp;
    public int currentHP;
    public int mp;
    public int currentMP;

    public int atk;
    public int def;

    //1분 단위이지만 효과를 확인하기 위해 1초로 구현
    public int recover_hp;
    public int recover_mp;

    public string dmgSound;

    public float time;
    private float current_time;

    //Floating Text prefab
    public GameObject prefab_Floating_text;
    //prefab의 부모 객체(Canvas)
    public GameObject parent;

    public Slider hpSlider;
    public Slider mpSlider;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHP = hp;
        currentMP = mp;
        current_time = time;
    }

    public void Hit(int _enemyAtk)
    {
        int dmg;

        if(def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;

        currentHP -= dmg;

        if(currentHP <= 0)
            Debug.Log("체력 0 미만, 게임 오버");

        AudioManager.instance.Play(dmgSound);

        //floating text를 띄울 위치(좌표값)
        Vector3 vector = this.transform.position;
        //Player의 머리 위에 띄우기 위해
        vector.y += 60;

        //prefab clone 생성
        GameObject clone = Instantiate(prefab_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        //Floating 안의 text 변수의 text 속성에 dmg를 넣어줌
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        //text color를 red로
        clone.GetComponent<FloatingText>().text.color = Color.red;
        //text의 font size를 25로
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
        //연속으로 피격당할 수 있으므로 이전 코루틴 취소
        StopAllCoroutines();

        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        //Player의 sprite를 가져와서 알파값을 조절
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;

        hpSlider.value = currentHP;
        mpSlider.value = currentMP;
        
        //레벨업
        if(currentEXP >= needEXP[character_Lv])
        {
            character_Lv++;
            hp += character_Lv * 2;
            mp += character_Lv + 2;

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
        }

        //일정 시간이 되면 회복 알고리즘
        current_time -= Time.deltaTime;

        if(current_time <= 0)
        {
            if(recover_hp > 0)
            {
                if(currentHP + recover_hp <= hp)
                    currentHP += recover_hp;
                else
                    currentHP = hp;
            }
            current_time = time;
        }
    }
}
