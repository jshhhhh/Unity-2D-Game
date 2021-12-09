using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemID;
    public int _count;
    public string pickUpSound;

    private void OnTriggerStay2D(Collider2D collision) {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //AudioManager가 static public이기 때문에 어디서나 접근 가능
            //자주 호출이 이루어진다면 private AudioManager theAudio처럼 변수로 만들어주는 편이 좋음(해당 변수는 단 한 번만 쓰이고 버려지므로 괜찮음)
            AudioManager.instance.Play(pickUpSound);
            //인벤토리 추가
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }
    }
}
