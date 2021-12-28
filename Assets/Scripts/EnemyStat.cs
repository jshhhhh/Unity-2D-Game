using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour
{

    public int hp;
    public int currentHP;
    public int atk;
    public int def;
    public int exp;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
        //기본값으로 체력이 가득 차게
        healthBarFilled.fillAmount = 1f;
    }

    public int Hit(int _playerAtk)
    {
        int playerAtk = _playerAtk;
        int dmg;
        //방어력만큼 데미지 감소
        if(def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;
        
        currentHP -= dmg;

        //체력이 0이 되면 몬스터 객체를 Destroy함
        if(currentHP <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.currentEXP += exp;
        }

        //현재 체력의 비율만큼 체력바 표시
        //fillAmount: 0~1f만큼의 값을 가짐
        //둘 다 int이므로 float으로 명시적 변환
        healthBarFilled.fillAmount = (float)currentHP / hp;
        healthBarBackground.SetActive(true);
        //때릴 때마다 WaitCoroutine 정지시킴
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());

        return dmg;
    }

    //일정 시간이 지나면 체력바가 사라짐
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        healthBarBackground.SetActive(false);
    }
}