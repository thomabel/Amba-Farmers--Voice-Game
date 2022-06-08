using UnityEngine;

/// <summary>
/// Used to store common variables as an asset that can be accessed by many objects
/// simultaneously. See this talk for more information about the use case for these:
/// https://youtu.be/raQ3iHhE_Kk
/// </summary>
[CreateAssetMenu (
    menuName = "SO Variables/Float",
    fileName = "float var"
    )]
public class FloatVariable : ScriptableObject
{
    public float Value;
}
