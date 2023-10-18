using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;




public class UIBtype : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{

    public Transform ButtonScale;
    public BTNType currentType;
    Vector3 defausltScale;

    private void Start()
    {
        defausltScale = ButtonScale.localScale;
    }

    public void  OnBtnClick()
    {

        switch(currentType)
        {
            case BTNType.START:

                Debug.Log("새게임");
                SceneManager.LoadScene("LM");
                break;

            case BTNType.OPENION:
                GameObject.Find("Canvas").transform.Find("설명").gameObject.SetActive(true);
                Debug.Log("옵션");
                break;

            case BTNType.BACK:
                GameObject.Find("Canvas").transform.Find("설명").gameObject.SetActive(false);

                break;

            case BTNType.EXIT:

                break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonScale.localScale = defausltScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonScale.localScale = defausltScale;
    }


    // Start is called before the first frame update

}
