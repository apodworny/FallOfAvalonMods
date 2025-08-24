using Awaken.TG.Main.Heroes;
using UnityEngine;
using Awaken.TG.Main.General.StatTypes;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;

namespace ProficiencyHUD;

public class ProficiencyHUD : MonoBehaviour
{
    public static AssetBundle assets;

    private Hero _hero;    
    private List<ProficiencyDisplay> _proficiencyDisplays = new List<ProficiencyDisplay>();
    private int _totalProficiencyLevel = 0;
    private Text totalLevelLabel;

    private const int ProficiencyHUDWidth = 156;
    private const int ProficiencyHUDHeight = 226;

    void Start()
    {
        LoadAssetBundle();
        GameObject proficiencyHUDBase = new GameObject("ProficiencyHUDBase", typeof(RectTransform));

        // Get the canvas from the hero
        Canvas canvas = GetComponentInParent<Canvas>();

        ConfigureProficiencyHUDBase(canvas, proficiencyHUDBase);
        InitProficiencyDisplays(proficiencyHUDBase);
        ConfigureTotalProficiencyLevel(proficiencyHUDBase);
        RepositionProficiencyDisplays();
    }

    public void SetHero(Hero hero)
    {
        _hero = hero;
    }

    public void UpdateProgress(ProfStatType typeofProfession, float progress)
    {
        int percentage = Mathf.FloorToInt(progress * 100f);

        ProficiencyDisplay proficiencyDisplay = _proficiencyDisplays.Find(display => display.profStatType == typeofProfession);

        float levelAfterXPApplied = typeofProfession._getter(_hero).BaseValue;
        float oldLevel = float.Parse(proficiencyDisplay.levelLabel.text);
        Plugin.Log.LogInfo($"Old Level: {oldLevel}, New Level: {levelAfterXPApplied}");

        // Update the total count of proficiency levels if the level increased
        if (levelAfterXPApplied > oldLevel)
        {
            Plugin.Log.LogInfo($"Adding new level");
            _totalProficiencyLevel += 1;

            if (totalLevelLabel != null)
            {
                totalLevelLabel.text = $"Total:\n{_totalProficiencyLevel}";
            }
            // I think this works to add a new level to _totalProficiencyLevel, but it just updates the number, not the label
        }

        proficiencyDisplay.levelLabel.text = typeofProfession._getter(_hero).BaseValue.ToString();

        // I have the percentage in an integer, but width of the background is 42, so it should be percentage of 42
        proficiencyDisplay.xpBarProgressTransform.sizeDelta = new Vector2(percentage * 0.42f, 5);
        proficiencyDisplay.xpBarProgressTransform.anchoredPosition = new Vector2(percentage * 0.42f / 2, -2.5f);
        proficiencyDisplay.FlashXPBar();
    }

    private void ConfigureProficiencyHUDBase(Canvas canvas, GameObject proficiencyHUDBase)
    {
        // Attach the proficiencyHUDBase to the hero's canvas
        proficiencyHUDBase.transform.SetParent(canvas.transform, false);

        RectTransform proficiencyHUDTransform = proficiencyHUDBase.GetComponent<RectTransform>();
        proficiencyHUDTransform.sizeDelta = new Vector2(ProficiencyHUDWidth, ProficiencyHUDHeight);

        Image proficiencyBaseImage = proficiencyHUDBase.AddComponent<Image>();
        proficiencyBaseImage.color = new Color(6f/255f, 6f/255f, 6f/255f, 1f);
        proficiencyHUDTransform.anchorMin = new Vector2(1, 0.35f);
        proficiencyHUDTransform.anchorMax = new Vector2(1, 0.35f);
        proficiencyHUDTransform.anchoredPosition = new Vector2(-(ProficiencyHUDWidth / 2), 0);
    }

    private void InitProficiencyDisplays(GameObject proficiencyHUDBase)
    {
        _proficiencyDisplays.Clear();

        foreach (ProfStatType proficiency in ProfStatType.HeroProficiencies)
        {
            string level = proficiency._getter(_hero).BaseValue.ToString();
            float progressToNextLevel = _hero.ProficiencyStats.GetProgressToNextLevel(proficiency);

            GameObject proficiencyDisplayGameObject = new GameObject($"{proficiency.EnumName}Display", typeof(RectTransform));
            proficiencyDisplayGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

            ProficiencyDisplay proficiencyDisplay = proficiencyDisplayGameObject.AddComponent<ProficiencyDisplay>();
            proficiencyDisplay.Initialize(proficiencyHUDBase, proficiency, progressToNextLevel, level);

            _proficiencyDisplays.Add(proficiencyDisplay);

            // Update the total proficiency level
            _totalProficiencyLevel += int.Parse(proficiencyDisplay.levelLabel.text);
        }
    }

    private void RepositionProficiencyDisplays()
    {
        // TODO: move 42 and 30 to consts folder? comes from ProficiencyDisplayXSize and ProficiencyDisplayYSize
        float horizontalSpacing = 42f + 6f;
        float verticalSpacing = 30f + 6f;
        int columns = 3;

        for (int i = 0; i < _proficiencyDisplays.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            _proficiencyDisplays[i].proficiencyDisplayTransform.anchoredPosition = new Vector2((ProficiencyDisplay.ProficiencyDisplayXSize + 19) / 2 + (col * horizontalSpacing), -(ProficiencyDisplay.ProficiencyDisplayYSize + 10) / 2 - (row * verticalSpacing));
        }
    }

    private void ConfigureTotalProficiencyLevel(GameObject proficiencyHUDBase)
    {
        GameObject totalProficiencyLevelGameObject = new GameObject("TotalProficiencyLevel", typeof(RectTransform));

        // Attach the totalProficiencyLevelGameObject to the proficiencyHUDBase
        totalProficiencyLevelGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

        totalLevelLabel = totalProficiencyLevelGameObject.AddComponent<Text>();
        totalLevelLabel.fontSize = 16;
        totalLevelLabel.color = Color.white;
        totalLevelLabel.font = assets.LoadAsset<Font>("runescape_uf");
        if (totalLevelLabel.font == null)
        {
            Plugin.Log.LogInfo("Font failed to load from asset bundle!");
        }
        totalLevelLabel.alignment = TextAnchor.MiddleCenter;

        RectTransform totalLevelLabelTransform = totalLevelLabel.GetComponent<RectTransform>();
        totalLevelLabelTransform.anchorMin = new Vector2(1, 0);
        totalLevelLabelTransform.anchorMax = new Vector2(1, 0);

        // To set the label to be beside the icon,
        // set the x of the anchoredPosition to the width of the icon plus half the width of the label
        // and the y to minus half the height
        totalLevelLabelTransform.sizeDelta = new Vector2(50, 50);
        totalLevelLabelTransform.anchoredPosition = new Vector2(-30, 23);

        totalLevelLabel.text = $"Total:\n{_totalProficiencyLevel}";
    }

    private void LoadAssetBundle()
    {
        if (assets == null)
        {
            string bundlePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "proficiencyhud");
            assets = AssetBundle.LoadFromFile(bundlePath);
        }
    }
}