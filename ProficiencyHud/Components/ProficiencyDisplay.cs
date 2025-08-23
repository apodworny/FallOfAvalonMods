using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Awaken.TG.Main.General.StatTypes;
using System.Collections;

namespace ProficiencyHUD;

public class ProficiencyDisplay : MonoBehaviour
{
    public Image proficiencyIcon;
    public Text levelLabel;
    public RectTransform proficiencyDisplayTransform;
    public ProfStatType profStatType;
    public RectTransform xpBarProgressTransform;

    private Sprite _iconSprite;
    private GameObject _levelGameObject;
    private GameObject _iconGameObject;
    private GameObject _xpBarGameObject;
    private GameObject _xpBarProgressGameObject;
    private Image _xpBarProgressImage;
    private float _lastFlashTime = -1f;

    private Sprite _xpBarSprite = ProficiencyHUD.assets.LoadAsset<Sprite>("ProgressBar");
    private Font _rsFont = ProficiencyHUD.assets.LoadAsset<Font>("runescape_uf");

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

        xpBarProgressTransform = _xpBarProgressGameObject.GetComponent<RectTransform>();
    }
    
    public void FlashXPBar()
    {
        // Set a debounce
        float now = Time.unscaledTime;
        if (now - _lastFlashTime < 0.3f)
            return;

        _lastFlashTime = now;
        StartCoroutine(FlashXPBarCoroutine());
    }

    private IEnumerator FlashXPBarCoroutine()
    {
        _xpBarProgressImage.color = new Color(1f, 0.84f, 0f, 1f);
        yield return new WaitForSecondsRealtime(0.2f);
        _xpBarProgressImage.color = Color.white;
    }

    private void ConfigureProficiencyDisplay()
    {
        proficiencyDisplayTransform = GetComponent<RectTransform>();
        proficiencyDisplayTransform.sizeDelta = new Vector2(ProficiencyDisplayXSize, ProficiencyDisplayYSize);

        Image proficiencyBaseImage = gameObject.AddComponent<Image>();
        proficiencyBaseImage.color = new Color(6f / 255f, 6f / 255f, 6f / 255f, 1f);
        proficiencyDisplayTransform.anchorMin = new Vector2(0, 1);
        proficiencyDisplayTransform.anchorMax = new Vector2(0, 1);
    }

    private void ConfigureIcon()
    {
        _iconGameObject = new GameObject($"{profStatType.EnumName}Icon", typeof(RectTransform));

        // Attach to the parent
        _iconGameObject.transform.SetParent(gameObject.transform, false);

        _iconSprite = ProficiencyHUD.assets.LoadAsset<Sprite>(profStatType.EnumName);

        Image iconImage = _iconGameObject.AddComponent<Image>();
        iconImage.sprite = _iconSprite;

        RectTransform iconRect = _iconGameObject.GetComponent<RectTransform>();

        // Set the anchor to the top left corner of its parent
        iconRect.anchorMin = new Vector2(0, 1);
        iconRect.anchorMax = new Vector2(0, 1);

        // Need to set the image size depending on the image
        iconRect.sizeDelta = new Vector2(18, 18);

        // To set the icon to be at the top left corner,
        // set the x of the anchoredPosition to half the width of the icon and the y to -half the inferred height
        iconRect.anchoredPosition = new Vector2(12, -13);
    }

    private void ConfigureLevel(string level)
    {
        _levelGameObject = new GameObject($"{profStatType.EnumName}LevelLabel", typeof(RectTransform));

        // Attach to the parent
        _levelGameObject.transform.SetParent(gameObject.transform, false);

        levelLabel = _levelGameObject.AddComponent<Text>();
        levelLabel.fontSize = 16;
        levelLabel.color = Color.white;
        levelLabel.font = _rsFont;

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
        _xpBarGameObject = new GameObject($"{profStatType.EnumName}XPBar", typeof(RectTransform));

        // Attach to the parent
        _xpBarGameObject.transform.SetParent(gameObject.transform, false);

        Image xpBarImage = _xpBarGameObject.AddComponent<Image>();
        xpBarImage.sprite = _xpBarSprite;

        RectTransform agilityXPBackgroundRect = _xpBarGameObject.GetComponent<RectTransform>();

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
        _xpBarProgressGameObject = new GameObject($"{profStatType.EnumName}XPBarProgress", typeof(RectTransform));

        // Attach to the parent
        _xpBarProgressGameObject.transform.SetParent(_xpBarGameObject.transform, false);

        _xpBarProgressImage = _xpBarProgressGameObject.AddComponent<Image>();
        _xpBarProgressImage.color = Color.white;

        xpBarProgressTransform = _xpBarProgressGameObject.GetComponent<RectTransform>();
        xpBarProgressTransform.anchorMin = new Vector2(0, 1);
        xpBarProgressTransform.anchorMax = new Vector2(0, 1);
        xpBarProgressTransform.sizeDelta = new Vector2(progressToNextLevel * 42f, 5);
        xpBarProgressTransform.anchoredPosition = new Vector2(progressToNextLevel * 42f / 2, -2.5f);
    }
}