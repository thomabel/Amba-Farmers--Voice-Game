
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        StartButton = root.Q<Button>("Start_Button");
        StartButton.clicked += StartButtonPressed;

    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("Farm");
    }
}
