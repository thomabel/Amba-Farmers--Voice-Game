using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour
{
    public GameObject volumeBar;
    public GameObject fill;

    public void OpenBar(bool index, float volume)
    {
        volumeBar.SetActive(true);
        if (index)
        {
            fill.GetComponent<Image>().color = new Color32(103, 143, 241, 255);

            
        }
        else
        {
            fill.GetComponent<Image>().color = new Color32(203, 200, 95, 255);

 
        }
        volumeBar.GetComponent<Slider>().value = volume/5.0f;
    }

    public void CloseBar()
    {
        volumeBar.SetActive(false);
        return;
    }
}
