using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProficiencyDisplay : MonoBehaviour
{
    public Image proficiencyIcon;
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI xpLabel;

    public void SetProficiencyDisplay(Sprite icon, int level, float xpPercent)
    {
        // if (proficiencyIcon != null)
        //     proficiencyIcon.sprite = icon;

        // if (levelLabel != null)
        //     levelLabel.text = $"Level: {level}";

        // if (xpLabel != null)
        //     xpLabel.text = $"XP: {Mathf.RoundToInt(xpPercent * 100)}%";
    }
}