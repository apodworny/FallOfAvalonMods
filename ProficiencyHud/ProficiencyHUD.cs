using Awaken.TG.Main.Heroes;
using UnityEngine;
using Awaken.TG.Main.General.StatTypes;
using UnityEngine.UI;
using System.Collections.Generic;
using Awaken.TG.MVC;
using Awaken.TG.Main.AudioSystem.Notifications;
using Awaken.TG.Main.Scenes.SceneConstructors;

namespace ProficiencyHUD;

public class ProficiencyHUD : MonoBehaviour
{
    public static AssetBundle assets;

    public static readonly Color BackgroundColor = new(6f / 255f, 6f / 255f, 6f / 255f, 1f);

    private Hero _hero;    
    private List<ProficiencyDisplay> _proficiencyDisplays = new List<ProficiencyDisplay>();
    private int _totalProficiencyLevel = 0;
    private Text totalLevelLabel;
    
    private const float ProficiencyHUDWidth = 176f;
    private const float ProficiencyHUDHeight = 246f;

    void Start()
    {
        // Destroy any existing ProficiencyHUDBase GameObject
        var existingHUD = GameObject.Find("ProficiencyHUDBase");
        if (existingHUD != null)
        {
            Destroy(existingHUD);
        }

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

        // Update the total count of proficiency levels if the level increased
        if (levelAfterXPApplied > oldLevel)
        {
            _totalProficiencyLevel += 1;

            // Play a notification sound when leveling up a proficiency
            NotificationSoundEvent soundEvent = CommonReferences.Get.AudioConfig.ProficiencyAudio;
            NotificationsAudioService audioService = World.Services.Get<NotificationsAudioService>();
            audioService?.PlayNotificationSound(soundEvent);

            totalLevelLabel.text = $"Total:\n{_totalProficiencyLevel}";
        }

        proficiencyDisplay.levelLabel.text = typeofProfession._getter(_hero).BaseValue.ToString();

        // Percentage is in an integer, but width of the background is 42, so it should be percentage of 42
        proficiencyDisplay.xpBarProgressTransform.sizeDelta = new Vector2(percentage * 0.42f, 5f);
        proficiencyDisplay.xpBarProgressTransform.anchoredPosition = new Vector2(percentage * 0.42f / 2, -2.5f);

        // If experience gain animation is enabled, run the animation
        if (Plugin.PluginConfig.ExpGainAnimationEnabled.Value)
        {
            proficiencyDisplay.FlashXPBar();
        }
    }

    private void ConfigureProficiencyHUDBase(Canvas canvas, GameObject proficiencyHUDBase)
    {
        // Attach the proficiencyHUDBase to the hero's canvas
        proficiencyHUDBase.transform.SetParent(canvas.transform, false);

        // Set the opacity
        CanvasGroup canvasGroup = proficiencyHUDBase.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = proficiencyHUDBase.AddComponent<CanvasGroup>();

        canvasGroup.alpha = Plugin.PluginConfig.HUDOpacity.Value;

        RectTransform proficiencyHUDTransform = proficiencyHUDBase.GetComponent<RectTransform>();
        proficiencyHUDTransform.sizeDelta = new Vector2(ProficiencyHUDWidth, ProficiencyHUDHeight);

        Image proficiencyBaseImage = proficiencyHUDBase.AddComponent<Image>();
        Sprite sprite = assets.LoadAsset<Sprite>("bg2");
        proficiencyBaseImage.sprite = sprite;
        proficiencyHUDTransform.anchorMin = new Vector2(1f, Plugin.PluginConfig.ProficiencyHUDVerticalPosition.Value);
        proficiencyHUDTransform.anchorMax = new Vector2(1f, Plugin.PluginConfig.ProficiencyHUDVerticalPosition.Value);
        proficiencyHUDTransform.anchoredPosition = new Vector2(-(ProficiencyHUDWidth / 2f), 0f);
    }

    private void InitProficiencyDisplays(GameObject proficiencyHUDBase)
    {
        _proficiencyDisplays.Clear();

        ProfStatType[] proficiencyOrder =
        [
            ProfStatType.OneHanded,
            ProfStatType.TwoHanded,
            ProfStatType.Archery,

            ProfStatType.Magic,
            ProfStatType.Unarmed,
            ProfStatType.Shield,

            ProfStatType.LightArmor,
            ProfStatType.MediumArmor,
            ProfStatType.HeavyArmor,

            ProfStatType.Athletics,
            ProfStatType.Evasion,
            ProfStatType.Acrobatics,

            ProfStatType.Alchemy,
            ProfStatType.Handcrafting,
            ProfStatType.Cooking,

            ProfStatType.Sneak,
            ProfStatType.Theft
        ];

        foreach (ProfStatType proficiency in proficiencyOrder)
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
        float horizontalSpacing = 42f + 6f;
        float verticalSpacing = 30f + 6f;
        int columns = 3;

        for (int i = 0; i < _proficiencyDisplays.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            _proficiencyDisplays[i].proficiencyDisplayTransform.anchoredPosition = new Vector2((ProficiencyDisplay.ProficiencyDisplayXSize + 39) / 2f + (col * horizontalSpacing), -(ProficiencyDisplay.ProficiencyDisplayYSize + 30) / 2f - (row * verticalSpacing));
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
        totalLevelLabelTransform.anchorMin = new Vector2(1f, 0f);
        totalLevelLabelTransform.anchorMax = new Vector2(1f, 0f);

        // To set the label to be beside the icon,
        // set the x of the anchoredPosition to the width of the icon plus half the width of the label
        // and the y to minus half the height
        totalLevelLabelTransform.sizeDelta = new Vector2(50f, 50f);
        totalLevelLabelTransform.anchoredPosition = new Vector2(-40f, 33f);

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