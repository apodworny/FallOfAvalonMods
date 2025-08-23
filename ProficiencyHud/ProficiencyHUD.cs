using Awaken.TG.Main.Heroes;
using UnityEngine;
using Awaken.TG.Main.General.StatTypes;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;

namespace ProficiencyHUD;

public class ProficiencyHUD : MonoBehaviour
{
    private Hero hero;

    public static AssetBundle assets;
    private List<ProficiencyDisplay> proficiencyDisplays = new List<ProficiencyDisplay>();

    private const int ProficiencyHUDWidth = 148;
    private const int ProficiencyHUDHeight = 220;

    void Start()
    {
        LoadAssetBundle();
        GameObject proficiencyHUDBase = new GameObject("ProficiencyHUDBase", typeof(RectTransform));

        // Get the canvas from the hero
        Canvas canvas = GetComponentInParent<Canvas>();

        ConfigureProficiencyHUDBase(canvas, proficiencyHUDBase);
        InitProficiencyDisplays(proficiencyHUDBase);

        RepositionProficiencyDisplays();
    }

    public void SetHero(Hero hero)
    {
        this.hero = hero;
    }

    public void UpdateProgress(ProfStatType typeofProfession, float progress)
    {
        int percentage = Mathf.FloorToInt(progress * 100f);

        ProficiencyDisplay proficiencyDisplay = proficiencyDisplays.Find(display => display.profStatType == typeofProfession);

        proficiencyDisplay.levelLabel.text = typeofProfession._getter(hero).BaseValue.ToString();

        // I have the percentage in an integer, but width of the background is 42, so it should be percentage of 42
        proficiencyDisplay.xpBarProgressTransform.sizeDelta = new Vector2(percentage * 0.42f, 5);
        proficiencyDisplay.xpBarProgressTransform.anchoredPosition = new Vector2(percentage * 0.42f / 2, -2.5f);
    }

    private void ConfigureProficiencyHUDBase(Canvas canvas, GameObject proficiencyHUDBase)
    {
        // Attach the proficiencyHUDBase to the hero's canvas
        proficiencyHUDBase.transform.SetParent(canvas.transform, false);

        RectTransform proficiencyHUDTransform = proficiencyHUDBase.GetComponent<RectTransform>();
        proficiencyHUDTransform.sizeDelta = new Vector2(ProficiencyHUDWidth, ProficiencyHUDHeight);

        Image proficiencyBaseImage = proficiencyHUDBase.AddComponent<Image>();
        proficiencyBaseImage.color = new Color(6f/255f, 6f/255f, 6f/255f, 1f);
        proficiencyHUDTransform.anchorMin = new Vector2(1, 0.5f);
        proficiencyHUDTransform.anchorMax = new Vector2(1, 0.5f);
        proficiencyHUDTransform.anchoredPosition = new Vector2(-(ProficiencyHUDWidth / 2) - 5, 0);
    }

    private void InitProficiencyDisplays(GameObject proficiencyHUDBase)
    {
        proficiencyDisplays.Clear();

        foreach (ProfStatType proficiency in ProfStatType.HeroProficiencies)
        {
            string level = proficiency._getter(hero).BaseValue.ToString();
            float progressToNextLevel = hero.ProficiencyStats.GetProgressToNextLevel(proficiency);

            GameObject proficiencyDisplayGameObject = new GameObject($"{proficiency.EnumName}Display", typeof(RectTransform));
            proficiencyDisplayGameObject.transform.SetParent(proficiencyHUDBase.transform, false);

            ProficiencyDisplay proficiencyDisplay = proficiencyDisplayGameObject.AddComponent<ProficiencyDisplay>();
            proficiencyDisplay.Initialize(proficiencyHUDBase, proficiency, progressToNextLevel, level);

            proficiencyDisplays.Add(proficiencyDisplay);
        }
    }

    private void RepositionProficiencyDisplays()
    {
        // TODO: move 42 and 30 to consts folder? comes from ProficiencyDisplayXSize and ProficiencyDisplayYSize
        float horizontalSpacing = 42f + 6f;
        float verticalSpacing = 30f + 6f;
        int columns = 3;

        for (int i = 0; i < proficiencyDisplays.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            proficiencyDisplays[i].proficiencyDisplayTransform.anchoredPosition = new Vector2((ProficiencyDisplay.ProficiencyDisplayXSize + 11) / 2 + (col * horizontalSpacing), -(ProficiencyDisplay.ProficiencyDisplayYSize + 10) / 2 - (row * verticalSpacing));
        }
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