using Awaken.TG.Main.Heroes.Crosshair;
using HarmonyLib;

namespace RemoveCrosshair;

[HarmonyPatch]
public class CrosshairPatch
{
    [HarmonyPatch(typeof(HeroCrosshair), nameof(HeroCrosshair.RefreshLayer))]
    [HarmonyPostfix]
    public static bool Prefix(HeroCrosshair __instance, CrosshairLayer layer)
    {
        // Loop through parts of the crosshair
        foreach (CrosshairPart part in __instance.Elements<CrosshairPart>())
        {
            // Player is crouching if we are looking at the CrouchCrosshairPart.
            // We only want to remove individual images of the crouching crosshair
            bool isCrouching = part is CrouchCrosshairPart;

            VCrouchCrosshairPart vCrouch = part.MainView.GetComponent<VCrouchCrosshairPart>();

            if (vCrouch != null)
            {
                // Removes the undesirable crouching crosshair images
                foreach (UnityEngine.UI.Image img in part.MainView.GetComponentsInChildren<UnityEngine.UI.Image>(true))
                {
                    if (img.name == "left" || img.name == "right")
                        img.enabled = false;
                }

                // Removes the ornament image from the detection crosshair
                if (vCrouch.detectionOrnamentImage != null)
                    vCrouch.detectionOrnamentImage.enabled = false;
            }

            // Disable the part being looped through, unless it's the crouching part
            part.SetActive(isCrouching);
        }

        // Prevent the original method being patched from running
        return false;
    }
}
