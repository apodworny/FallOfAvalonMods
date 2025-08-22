using Awaken.TG.Main.Heroes;
using Awaken.TG.MVC.Elements;
using UnityEngine;
using Awaken.TG.Assets;
using TMPro;
using Awaken.TG.MVC;
using Awaken.TG.Main.General.StatTypes;
using UnityEngine.UI;

namespace ProficiencyHUD;

public class ProficiencyHUD : MonoBehaviour
{
    private TextMeshProUGUI agilityLabel;
    private Hero hero;

    private int sneakPercentage;
    private int agilityPercentage;

    private Sprite sneakSprite;
    private Sprite agilityIconSprite;
    private Sprite agilityXPBarSprite;

    RectTransform agilityXPBarTransform;

    void Start()
    {
        GameObject proficiencyHUDBase = new GameObject("ProficiencyHUDBase", typeof(RectTransform));
        GameObject agilityLabelGameObject = new GameObject("AgilityLabel", typeof(RectTransform));
        GameObject agilityIconGameObject = new GameObject("AgilityIcon", typeof(RectTransform));
        GameObject agilityXPBackgroundGameObject = new GameObject("AgilityXPBackground", typeof(RectTransform));
        GameObject agilityXPBarGameObject = new GameObject("AgilityXPBar", typeof(RectTransform));

        loadAssets();

        // Get the canvas from the hero
        Canvas canvas = GetComponentInParent<Canvas>();

        configureProficiencyHUDBase(canvas, proficiencyHUDBase);
        configureAgilityIcon(proficiencyHUDBase, agilityIconGameObject);
        configureAgilityLabel(proficiencyHUDBase, agilityLabelGameObject);
        configureAgilityXP(proficiencyHUDBase, agilityXPBackgroundGameObject);
        configureAgilityXPBar(agilityXPBackgroundGameObject, agilityXPBarGameObject);
    }

    public void SetHero(Hero hero)
    {
        this.hero = hero;
    }

    public void UpdateProgress(ProfStatType typeofProfession, float progress)
    {
        int percentage = Mathf.FloorToInt(progress * 100f);
        if (typeofProfession == ProfStatType.Acrobatics)
        {
            agilityPercentage = percentage;
        }
        else if (typeofProfession == ProfStatType.Sneak)
        {
            sneakPercentage = percentage;
        }

        // if (agilityLabel != null)
        // {
        //     agilityLabel.text = $"{hero.ProficiencyStats.Acrobatics.BaseValue}, {agilityPercentage}%\n\n{hero.ProficiencyStats.Sneak.BaseValue}, {sneakPercentage}%";
        // }

        if (agilityLabel != null)
        {
            agilityLabel.text = hero.ProficiencyStats.Acrobatics.BaseValue.ToString();

            // I have the percentage in an integer, but width of the background is 35, so it should be percentage of 35
            agilityXPBarTransform.sizeDelta = new Vector2(agilityPercentage * 0.35f, 5);
            agilityXPBarTransform.anchoredPosition = new Vector2(agilityPercentage * 0.35f / 2, -2.5f);
        }
    }

    private void loadAssets()
    {
        string bundlePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "proficiencyhud");
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

        sneakSprite = bundle.LoadAsset<Sprite>("sneak");
        agilityIconSprite = bundle.LoadAsset<Sprite>("agility");
        agilityXPBarSprite = bundle.LoadAsset<Sprite>("progressbar");
    }

    private void configureProficiencyHUDBase(Canvas canvas, GameObject proficiencyHUDBase)
    {
        // Attach the proficiencyHUDBase to the hero's canvas
        proficiencyHUDBase.transform.SetParent(canvas.transform, false);

        RectTransform proficiencyHUDTransform = proficiencyHUDBase.GetComponent<RectTransform>();
        proficiencyHUDTransform.sizeDelta = new Vector2(200, 200);

        Image proficiencyBaseImage = proficiencyHUDBase.AddComponent<Image>();
        proficiencyBaseImage.color = Color.black;
        proficiencyHUDTransform.anchorMin = new Vector2(1, 0.5f);
        proficiencyHUDTransform.anchorMax = new Vector2(1, 0.5f);
        proficiencyHUDTransform.anchoredPosition = new Vector2(-20, 0);
    }

    private void configureAgilityIcon(GameObject proficiencyHUDBase, GameObject agilityIconGameObject)
    {
        agilityIconGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

        Image agilityIconImage = agilityIconGameObject.AddComponent<Image>();
        agilityIconImage.sprite = agilityIconSprite;
        agilityIconImage.preserveAspect = true;

        RectTransform agilityIconRect = agilityIconGameObject.GetComponent<RectTransform>();

        // Set the anchor to the top left corner of its parent
        agilityIconRect.anchorMin = new Vector2(0, 1);
        agilityIconRect.anchorMax = new Vector2(0, 1);

        agilityIconRect.sizeDelta = new Vector2(24, 24);

        // To set the icon to be at the top left corner,
        // set the x of the anchoredPosition to half the width of the icon and the y to -half the height
        agilityIconRect.anchoredPosition = new Vector2(12, -12);
    }

    private void configureAgilityLabel(GameObject proficiencyHUDBase, GameObject agilityLabelGameObject)
    {
        // Attach the agilityLabelGameObject to the proficiencyHUDBase
        agilityLabelGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

        agilityLabel = agilityLabelGameObject.AddComponent<TextMeshProUGUI>();
        agilityLabel.fontSize = 12;
        agilityLabel.color = Color.white;

        RectTransform agilityLabelTransform = agilityLabel.GetComponent<RectTransform>();
        agilityLabelTransform.anchorMin = new Vector2(0, 1);
        agilityLabelTransform.anchorMax = new Vector2(0, 1);

        // To set the label to be beside the icon,
        // set the x of the anchoredPosition to the width of the icon plus half the width of the label
        // and the y to minus half the height
        agilityLabelTransform.sizeDelta = new Vector2(20, 20);
        agilityLabelTransform.anchoredPosition = new Vector2(24 + 10, -10);
    }

    private void configureAgilityXP(GameObject proficiencyHUDBase, GameObject agilityXPBackgroundGameObject)
    {
        // Attach the agilityXPBackgroundGameObject to the proficiencyHUDBase
        agilityXPBackgroundGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

        Image agilityXPBackgroundImage = agilityXPBackgroundGameObject.AddComponent<Image>();
        agilityXPBackgroundImage.sprite = agilityXPBarSprite;

        RectTransform agilityXPBackgroundRect = agilityXPBackgroundGameObject.GetComponent<RectTransform>();

        // Set the anchor to the top left corner of its parent
        agilityXPBackgroundRect.anchorMin = new Vector2(0, 1);
        agilityXPBackgroundRect.anchorMax = new Vector2(0, 1);

        agilityXPBackgroundRect.sizeDelta = new Vector2(35, 5);

        // To set the icon to be at the top left corner,
        // Set the x of the anchoredPosition to half the width of the icon
        // and the y to minus the height of the icon minus half the height
        agilityXPBackgroundRect.anchoredPosition = new Vector2(17.5f, -24 - 2.5f);
    }

    private void configureAgilityXPBar(GameObject agilityXPBackgroundGameObject, GameObject agilityXPBarGameObject)
    {
        // Attach the agilityXPBackgroundGameObject to the agilityXPBackgroundGameObject
        agilityXPBarGameObject.transform.SetParent(agilityXPBackgroundGameObject.transform, false);

        agilityXPBarTransform = agilityXPBarGameObject.GetComponent<RectTransform>();
        agilityXPBarTransform.sizeDelta = new Vector2(0, 5);

        Image proficiencyBaseImage = agilityXPBarGameObject.AddComponent<Image>();
        proficiencyBaseImage.color = Color.white;
        agilityXPBarTransform.anchorMin = new Vector2(0, 1);
        agilityXPBarTransform.anchorMax = new Vector2(0, 1);
        agilityXPBarTransform.anchoredPosition = new Vector2(0, -2.5f);
    }
}