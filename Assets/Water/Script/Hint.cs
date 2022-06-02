using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{

    public GameObject messagePanel;


    public void OpenMessage(string text)
    {
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<Text>().text = text;
        return;
    }

    public void CloseMessage()
    {
        messagePanel.SetActive(false);
        return;
    }
}
