using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public GameObject prefab_Floating_text;
    public GameObject parent;
    public GameObject effect;

    public string atkSound;

    private PlayerStat thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            int dmg = collision.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            //AudioManager.instance.Play(atkSound);

            //충돌한 객체(몬스터) 위에 prefab을 띄움
            Vector3 vector = collision.transform.position;

            Instantiate(effect, vector, Quaternion.Euler(Vector3.zero));
            
            vector.y += 60;

            //prefab clone 생성
            GameObject clone = Instantiate(prefab_Floating_text, vector, Quaternion.Euler(Vector3.zero));
            //Floating 안의 text 변수의 text 속성에 dmg를 넣어줌
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            //text color를 red로
            clone.GetComponent<FloatingText>().text.color = Color.white;
            //text의 font size를 25로
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}
