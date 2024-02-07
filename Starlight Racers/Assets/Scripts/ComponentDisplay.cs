using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComponentDisplay : MonoBehaviour
{
    public Image SelectorImage;
    
    // Start is called before the first frame update
    void Start()
    {
       SelectorImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject selectedCard = EventSystem.current.currentSelectedGameObject;

        if (selectedCard == gameObject)
        {
            SelectorImage.gameObject.SetActive(true);
        }
        else
        {
            SelectorImage.gameObject.SetActive(false);
        }
    }
}
