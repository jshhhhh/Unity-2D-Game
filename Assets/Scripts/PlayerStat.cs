using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string dmgSound;

    //Floating Text prefab
    public GameObject prefab_Floating_text;
    //prefab의 부모 객체(Canvas)
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHP = hp;
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
        
    }
}
