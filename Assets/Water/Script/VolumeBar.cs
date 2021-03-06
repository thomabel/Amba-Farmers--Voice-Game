using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour
{
    public GameObject volumeBar;
    public GameObject fill;

    private GameObject player;
    private LiquidTypeLabel liquid;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Update()
    {
        if (player.GetComponentInChildren<LiquidTypeLabel>())
        {
            liquid = player.GetComponentInChildren<LiquidTypeLabel>();
            if (liquid != null && liquid.Type == Base.LiquidType.Urine)
            {

                float volume = player.GetComponentInChildren<BucketWithUrine>().getCurrentUrineVolume();
                OpenBar(false, volume);
            }
            else if (liquid != null && liquid.Type == Base.LiquidType.Water)
            {
                float volume = player.GetComponentInChildren<BucketWithWater>().getCurrentWaterVolume();

                OpenBar(true, volume);
            }
            else
            {
                CloseBar();
            }

        }
        else
        {
           CloseBar();
        }

    }

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
