using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI
{
    public Label createLabel(ref Label label, string classList, Texture2D picture, string name, string text)
    {
        label = new Label();
        label.AddToClassList(classList);
        label.style.backgroundImage = picture;
        if(name != null)
            label.name = name;
        label.text = text;

        return label;
    }

    public Button createButton(ref Button button, string classList, Texture2D picture, string name, string text)
    {
        button = new Button();
        button.AddToClassList(classList);
        button.style.backgroundImage = picture;
        if (name != null)
            button.name = name;
        button.text = text;

        return button;
    }

    public VisualElement createVisualElement(ref VisualElement visualElement, string classList, Texture2D picture, string name, string text)
    {
        visualElement = new VisualElement();
        visualElement.AddToClassList(classList);
        visualElement.style.backgroundImage = picture;
        if (name != null)
            visualElement.name = name;

        return visualElement;
    }
    public TextField createTextField(ref TextField textfield, string classList, int maxlength, string value, string name)
    {
        textfield = new TextField();
        textfield.AddToClassList(classList);
        textfield.maxLength = maxlength;
        textfield.value = value;
        textfield.name = name;
        return textfield;
    }

    public void ShowOrHideVisualElements(ref VisualElement ShowElement, ref VisualElement HideElement)
    {
        ShowElement.style.display = DisplayStyle.Flex;
        HideElement.style.display = DisplayStyle.None;

    }
}
