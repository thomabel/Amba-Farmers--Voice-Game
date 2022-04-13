using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Card", menuName = "Shop/Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public int cost;
    public int quantity = 1;
    public Texture2D picture;
}
