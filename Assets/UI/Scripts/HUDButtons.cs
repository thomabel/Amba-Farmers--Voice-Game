using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDButtons : MonoBehaviour
{
    [SerializeField]
    private RawImage ToolbuttonImage;

    [SerializeField]
    private RawImage ItembuttonImage;

    [SerializeField]
    private Market market;

    //These functions get triggered when a Tool or Item
    //has been set to equipment slots

    //We have set a new tool, this function gets called
    //and we replace Tool image on Tool control button
    public void AddToolImage(GameObject obj)
    {
        MarketWrapper value;
        if (obj == null)
        {
            ToolbuttonImage.gameObject.SetActive(false);
            return;
        }

        if (market.Comparator.TryGetValue(obj.GetComponent<TypeLabel>().Type, out value))
        {
            ToolbuttonImage.gameObject.SetActive(true);
            ToolbuttonImage.texture = value.picture;
            
            //useToolButton.style.backgroundImage = new StyleBackground(value.picture);
        }
        else
            ToolbuttonImage.gameObject.SetActive(false);
            
        
            
    }
    //We have set a new Item, this function gets called
    //and we replace Item image on Item control button
    public void AddItemImage(GameObject obj)
    {
        if (obj == null)
        {
            ItembuttonImage.gameObject.SetActive(false);
            return;
        }
        MarketWrapper value;
        if (market.Comparator.TryGetValue(obj.GetComponent<TypeLabel>().Type, out value))
        {
            ItembuttonImage.gameObject.SetActive(true);
            ItembuttonImage.texture = value.picture;
        }
        else
            ItembuttonImage.gameObject.SetActive(false);
    }
}
