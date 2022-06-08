using UnityEngine;
using TMPro;

/// <summary>
/// Test class for use with TickCounter that shows the counter value.
/// </summary>
public class UpdateText : MonoBehaviour
{
    public TMP_Text text;
    public FloatReference time;

    private void Update()
    {
        text.text = time.Value.ToString("F2");
    }
}
