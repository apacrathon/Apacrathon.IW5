using System;
using System.Collections.Generic;
using System.Linq;
using InfinityScript;

namespace Atlas
{
    public class CommandHandler
    {
        public static Dictionary<string, Action<Entity, string[]>> ScriptCommands = new Dictionary<string, Action<Entity, string[]>>
        {
            { "fx", FX },
            { "ft", FilmTweak },
            { "ga", GiveAmmo }
        };

        public static void RunCommand(Entity sender, string[] message)
        {
            if (sender == null || message.Length <= 0) return;
            string command = message[0].Remove(0, 1).ToLowerInvariant();
            string[] arguments = message.Where((source, index) => index != 0).ToArray();

            if (!ScriptCommands.TryGetValue(command, out Action<Entity, string[]> cmdAction))
            {
                Utilities.RawSayTo(sender, string.Format("The command {0} does not exist.", command));
                return;
            }

            cmdAction(sender, arguments);
        }

        private static void FX(Entity sender, string[] args)
        {
            if (AtlasiSnipe.ScriptConfiguration.DisableLightingEffects)
            {
                sender.IPrintLnBold(AtlasiSnipe.ScriptConfiguration.TryGetFeedbackMessage("cmdFXDisabled"));
                return;
            }

            if (args.Length == 1)
            {
                if (args[0].ToLower().Equals("on") || args[0].Equals("1"))
                {
                    sender.SetClientDvar("fx_enable", "1");
                    sender.SetClientDvar("r_fog", "1");
                    sender.SetClientDvar("fx_drawclouds", "1");
                    sender.IPrintLnBold("FX are now enabled.");
                }
                else if (args[0].ToLower().Equals("off") || args[0].Equals("0"))
                {
                    sender.SetClientDvar("fx_enable", "0");
                    sender.SetClientDvar("r_fog", "0");
                    sender.SetClientDvar("fx_drawclouds", "0");
                    sender.IPrintLnBold("FX are now disabled.");
                }
                else
                {
                    sender.IPrintLnBold("Syntax: !fx <on/off/1/0>");
                }
            }
            else
            {
                sender.IPrintLnBold("Syntax: !fx <on/off/1/0>");
            }
        }

        private static void FilmTweak(Entity sender, string[] args)
        {
            if (AtlasiSnipe.ScriptConfiguration.DisableFilmTweaks)
            {
                sender.IPrintLnBold(AtlasiSnipe.ScriptConfiguration.TryGetFeedbackMessage("cmdFilmTweakDisabled"));
                return;
            }

            if (args.Length < 1) { Utilities.RawSayTo(sender, "Syntax: !ft <0-25>, reset, random"); return; }

            switch (args[0])
            {
                case "0":
                    sender.SetClientDvar("r_filmusetweaks", "0");
                    sender.SetClientDvar("r_filmtweakenabled", "0");
                    sender.SetClientDvar("r_colorMap", "1");
                    sender.SetClientDvar("r_specularMap", "1");
                    sender.SetClientDvar("r_normalMap", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0");
                    sender.SetClientDvar("r_glowTweakEnable", "0");
                    sender.SetClientDvar("r_glowUseTweaks", "0");
                    sender.ThermalVisionOff();
                    break;
                case "1":
                    sender.SetClientDvar("r_filmtweakdarktint", "0.65 0.7 0.8");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.3");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.15");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "1.8 1.8 1.8");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "2":
                    sender.SetClientDvar("r_filmtweakdarktint", "1.15 1.1 1.3");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.6");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.2");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "1.35 1.3 1.25");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "3":
                    sender.SetClientDvar("r_filmtweakdarktint", "0.8 0.8 1.1");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.3");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.48");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "1 1 1.4");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "4":
                    sender.SetClientDvar("r_filmtweakdarktint", "1.8 1.8 2");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.25");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.02");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "0.8 0.8 1");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "5":
                    sender.SetClientDvar("r_filmtweakdarktint", "1 1 2");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.5");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.07");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "1 1.2 1");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "6":
                    sender.SetClientDvar("r_filmtweakdarktint", "1.5 1.5 2");
                    sender.SetClientDvar("r_filmtweakcontrast", "1");
                    sender.SetClientDvar("r_filmtweakbrightness", "0.0.4");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0");
                    sender.SetClientDvar("r_filmusetweaks", "1");
                    sender.SetClientDvar("r_filmtweaklighttint", "1.5 1.5 1");
                    sender.SetClientDvar("r_filmtweakenable", "1");
                    break;
                case "7":
                    sender.SetClientDvar("r_specularMap", "2");
                    sender.SetClientDvar("r_normalMap", "0");
                    break;
                case "8":
                    sender.SetClientDvar("cg_drawFPS", "1");
                    sender.SetClientDvar("cg_fovScale", "1.5");
                    break;
                case "9":
                    sender.SetClientDvar("r_debugShader", "1");
                    break;
                case "10":
                    sender.SetClientDvar("r_colorMap", "3");
                    break;
                case "11":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakLightTint", "0 0.2 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 0.125 1");
                    sender.SetClientDvar("r_filmtweakbrightness", "0");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "5");
                    sender.SetClientDvar("r_glowTweakBloomIntensity0", "0.5");
                    break;
                case "12":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "1");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 0 0");
                    sender.ThermalVisionOn();
                    break;
                case "26":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "1");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 0 0");
                    sender.ThermalVisionOff();
                    break;
                case "13":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "1 1 0");
                    break;
                case "14":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "1 1 1");
                    break;
                case "15":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "1 0 1");
                    break;
                case "16":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 1 1");
                    break;
                case "17":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 0 1");
                    break;
                case "18":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0 1 0");
                    break;
                case "19":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "1");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "10");
                    sender.SetClientDvar("r_filmTweakContrast", "3");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 0");
                    sender.SetClientDvar("r_filmTweakDarkTint", "1 0 0");
                    break;
                case "20":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.5");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "2");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.5 0.7 0.25");
                    break;
                case "21":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.5");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "2");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.25 0.7 0.5");
                    break;
                case "22":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.5");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "2");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.7 0.25 0.5");
                    break;
                case "23":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.5");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "2");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.7 0.7 0.35");
                    break;
                case "24":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.5");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "2");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "1 0.125 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.85 0.7 0.1");
                    break;
                case "25":
                    sender.SetClientDvar("r_filmUseTweaks", "1");
                    sender.SetClientDvar("r_filmTweakEnable", "1");
                    sender.SetClientDvar("r_filmTweakDesaturation", "1");
                    sender.SetClientDvar("r_filmTweakDesaturationDark", "1");
                    sender.SetClientDvar("r_filmTweakInvert", "0.6");
                    sender.SetClientDvar("r_glowTweakEnable", "1");
                    sender.SetClientDvar("r_glowUseTweaks", "1");
                    sender.SetClientDvar("r_glowTweakRadius0", "15");
                    sender.SetClientDvar("r_filmTweakContrast", "4");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.5");
                    sender.SetClientDvar("r_filmTweakLightTint", "0.4 0.12 1");
                    sender.SetClientDvar("r_filmTweakDarkTint", "0.15 0.5 0.8");
                    break;
                case "night":
                    sender.VisionSetNightForPlayer("paris_ac130_night");
                    break;
                case "reset":
                default:
                    sender.VisionSetNakedForPlayer(GSCFunctions.GetDvar("mapname"));
                    sender.SetClientDvar("r_filmtweakdarktint", "0.7 0.85 1");
                    sender.SetClientDvar("r_filmtweakcontrast", "1.4");
                    sender.SetClientDvar("r_filmtweakdesaturation", "0.2");
                    sender.SetClientDvar("r_filmusetweaks", "0");
                    sender.SetClientDvar("r_filmtweaklighttint", "1.1 1.05 0.85");
                    sender.SetClientDvar("cg_scoreboardpingtext", "1");
                    sender.SetClientDvar("waypointIconHeight", "13");
                    sender.SetClientDvar("waypointIconWidth", "13");
                    sender.SetClientDvar("cl_maxpackets", "100");
                    sender.SetClientDvar("r_fog", "0");
                    sender.SetClientDvar("fx_drawclouds", "0");
                    sender.SetClientDvar("r_distortion", "0");
                    sender.SetClientDvar("r_dlightlimit", "0");
                    sender.SetClientDvar("cg_brass", "0");
                    sender.SetClientDvar("snaps", "30");
                    sender.SetClientDvar("com_maxfps", "100");
                    sender.SetClientDvar("clientsideeffects", "0");
                    sender.SetClientDvar("r_filmTweakBrightness", "0.2");
                    break;
            }

            sender.IPrintLnBold(string.Format("Film Tweak {0} applied.", args[0]));
        }

        private static void GiveAmmo(Entity sender, string[] args)
        {
            if (AtlasiSnipe.ScriptConfiguration.DisableGiveAmmo)
            {
                sender.IPrintLnBold(AtlasiSnipe.ScriptConfiguration.TryGetFeedbackMessage("cmdGiveAmmoDisabled"));
                return;
            }

            if (args.Length != 0)
            {
                sender.IPrintLnBold("Syntax: !ga");
                return;
            }

            sender.GiveMaxAmmo(sender.CurrentWeapon);
            sender.IPrintLnBold("You have been given maximum ammo for " + sender.CurrentWeapon);
        }
    }
}
