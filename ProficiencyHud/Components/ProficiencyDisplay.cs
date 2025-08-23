using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Awaken.TG.Main.General.StatTypes;

namespace ProficiencyHUD;

public class ProficiencyDisplay : MonoBehaviour
{
    public Image proficiencyIcon;
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI xpLabel;
    private Sprite iconSprite;
    public RectTransform proficiencyDisplayTransform;
    private GameObject levelGameObject;
    private GameObject iconGameObject;
    private GameObject xpBarGameObject;
    private GameObject xpBarProgressGameObject;
    public ProfStatType profStatType;

    private Sprite xpBarSprite = ProficiencyHUD.assets.LoadAsset<Sprite>("ProgressBar");

    public RectTransform xpBarProgressTransform;
    public static int ProficiencyDisplayXSize = 42;
    public static int ProficiencyDisplayYSize = 30;

    public void Initialize(GameObject proficiencyHUDBase, ProfStatType profStatType, float progressToNextLevel, string level)
    {
        transform.SetParent(proficiencyHUDBase.transform, false);

        this.profStatType = profStatType;

        ConfigureProficiencyDisplay();
        ConfigureIcon();
        ConfigureLevel(level);
        ConfigureXPBar();
        ConfigureXPBarProgress(progressToNextLevel);

        xpBarProgressTransform = xpBarProgressGameObject.GetComponent<RectTransform>();
    }

    private void ConfigureProficiencyDisplay()
    {
        proficiencyDisplayTransform = GetComponent<RectTransform>();
        proficiencyDisplayTransform.sizeDelta = new Vector2(ProficiencyDisplayXSize, ProficiencyDisplayYSize);

        Image proficiencyBaseImage = gameObject.AddComponent<Image>();
        proficiencyBaseImage.color = new Color(6f/255f, 6f/255f, 6f/255f, 1f);
        proficiencyDisplayTransform.anchorMin = new Vector2(0, 1);
        proficiencyDisplayTransform.anchorMax = new Vector2(0, 1);
    }

    private void ConfigureIcon()
    {
        iconGameObject = new GameObject($"{profStatType.EnumName}Icon", typeof(RectTransform));

        // Attach to the parent
        iconGameObject.transform.SetParent(gameObject.transform, false);

        iconSprite = ProficiencyHUD.assets.LoadAsset<Sprite>(profStatType.EnumName);

        Image iconImage = iconGameObject.AddComponent<Image>();
        iconImage.sprite = iconSprite;

        RectTransform iconRect = iconGameObject.GetComponent<RectTransform>();

        // Set the anchor to the top left corner of its parent
        iconRect.anchorMin = new Vector2(0, 1);
        iconRect.anchorMax = new Vector2(0, 1);

        // Need to set the image size depending on the image
        iconRect.sizeDelta = new Vector2(24, 24);

        // To set the icon to be at the top left corner,
        // set the x of the anchoredPosition to half the width of the icon and the y to -half the inferred height
        iconRect.anchoredPosition = new Vector2(12, -12);
    }

    private void ConfigureLevel(string level)
    {
        levelGameObject = new GameObject($"{profStatType.EnumName}LevelLabel", typeof(RectTransform));

        // Attach to the parent
        levelGameObject.transform.SetParent(gameObject.transform, false);

        levelLabel = levelGameObject.AddComponent<TextMeshProUGUI>();
        levelLabel.fontSize = 12;
        levelLabel.color = Color.white;

        RectTransform levelLabelTransform = levelLabel.GetComponent<RectTransform>();
        levelLabelTransform.anchorMin = new Vector2(0, 1);
        levelLabelTransform.anchorMax = new Vector2(0, 1);

        // To set the label to be beside the icon,
        // set the x of the anchoredPosition to the width of the icon plus half the width of the label
        // and the y to minus half the height
        levelLabelTransform.sizeDelta = new Vector2(20, 20);
        levelLabelTransform.anchoredPosition = new Vector2(24 + 12, -20);

        levelLabel.text = level;
    }

    private void ConfigureXPBar()
    {
        xpBarGameObject = new GameObject($"{profStatType.EnumName}XPBar", typeof(RectTransform));

        // Attach to the parent
        xpBarGameObject.transform.SetParent(gameObject.transform, false);

        Image xpBarImage = xpBarGameObject.AddComponent<Image>();
        xpBarImage.sprite = xpBarSprite;

        RectTransform agilityXPBackgroundRect = xpBarGameObject.GetComponent<RectTransform>();

        // Set the anchor to the top left corner of its parent
        agilityXPBackgroundRect.anchorMin = new Vector2(0, 1);
        agilityXPBackgroundRect.anchorMax = new Vector2(0, 1);

        agilityXPBackgroundRect.sizeDelta = new Vector2(42, 5);

        // To set the icon to be at the top left corner,
        // Set the x of the anchoredPosition to half the width of the icon
        // and the y to minus the inferred height of the icon minus half the height
        agilityXPBackgroundRect.anchoredPosition = new Vector2(21f, -25f - 2.5f);
    }

    private void ConfigureXPBarProgress(float progressToNextLevel)
    {
        xpBarProgressGameObject = new GameObject($"{profStatType.EnumName}XPBarProgress", typeof(RectTransform));

        // Attach to the parent
        xpBarProgressGameObject.transform.SetParent(xpBarGameObject.transform, false);

        Image proficiencyBaseImage = xpBarProgressGameObject.AddComponent<Image>();
        proficiencyBaseImage.color = Color.white;

        xpBarProgressTransform = xpBarProgressGameObject.GetComponent<RectTransform>();
        xpBarProgressTransform.anchorMin = new Vector2(0, 1);
        xpBarProgressTransform.anchorMax = new Vector2(0, 1);
        xpBarProgressTransform.sizeDelta = new Vector2(progressToNextLevel * 42f, 5);
        xpBarProgressTransform.anchoredPosition = new Vector2(progressToNextLevel * 42f / 2, -2.5f);
    }
}