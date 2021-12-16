using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{

    public int hp;
    public int currentHP;
    public int atk;
    public int def;
    public int exp;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
    }

    public int Hit(int _playerAtk)
    {
        int playerAtk = _playerAtk;
        int dmg;
        if(def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;
        
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.currentEXP += exp;
        }

        return dmg;
    }
}