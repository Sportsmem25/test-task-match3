using TMPro;
using UnityEngine;

public class ScoreLabelController : MonoBehaviour
{
    public TextMeshProUGUI label;

    public void SetScoreText(string value)
    {
        if(label == null)
        {
            label = GetComponent<TextMeshProUGUI>();
        }

        label.text = value;
    }
}