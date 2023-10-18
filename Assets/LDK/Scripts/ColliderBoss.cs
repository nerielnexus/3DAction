using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBoss : MonoBehaviour
{
    CapsuleCollider cp;
    MonsterAI bx;
    
    // Start is called before the first frame update
    void Start()
    {
        // bx = GetComponent<BoxCollider>();

        bx = GetComponentInParent<MonsterAI>();
        cp = GetComponent<CapsuleCollider>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //활성화 비활성화를 함수로 만들어서 AI코드로 보내자.
    public void OnFireCollder()
    {
        cp.enabled = true;
    }
    public void OnNotFireCollder()
    {
        cp.enabled = false;
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" &&cp.enabled==true)
        {

            Debug.Log("플레이어가 불타오름~~~~~~~~~~");
        }
     

    }
   */

  

}
