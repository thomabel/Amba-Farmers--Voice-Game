using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    public TMP_Text text;
    public FloatReference time;

    private void Update()
    {
        text.text = time.Value.ToString("F2");
    }
}
