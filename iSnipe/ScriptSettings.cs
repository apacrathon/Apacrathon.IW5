using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using InfinityScript;

namespace Apacrathon
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ScriptSettings
    {
        [JsonProperty]
        public bool EnableScript { get; set; }

        [JsonProperty]
        public string ServerName { get; set; }

        [JsonProperty]
        public string ServerMessage { get; set; }

        [JsonProperty]
        public bool DeleteBombSites { get; set; }

        [JsonProperty]
        public bool DisableGiveAmmo { get; set; }

        [JsonProperty]
        public bool DisableMelee { get; set; }

        [JsonProperty]
        public bool DisableFallDamage { get; set; }

        [JsonProperty]
        public bool DisableFilmTweaks { get; set; }

        [JsonProperty]
        public bool DisableLightingEffects { get; set; }

        [JsonProperty]
        public bool DisableDropShot { get; set; }

        [JsonProperty]
        public bool DisableNoScope { get; set; }

        [JsonProperty]
        public bool DisableHardScope { get; set; }

        [JsonProperty]
        public bool DisableHalfScope { get; set; }

        [JsonProperty]
        public float HardScopeLimit { get; set; }

        [JsonProperty]
        public float QuickScopeLimit { get; set; }

        [JsonProperty]
        public bool LimitSemiAutoRateOfFire { get; set; }

        [JsonProperty]
        public IDictionary<string, string> FeedbackMessages { get; set; }

        public ScriptSettings()
        {
            EnableScript = true;
            ServerName = "IW5 iSnipe";
            ServerMessage = string.Format("{0}\n{1}", ServerName, "Atlas iSnipe " + Utils.GetVersionString());
            DisableGiveAmmo = false;
            DeleteBombSites = true;
            DisableMelee = true;
            DisableFallDamage = true;
            DisableFilmTweaks = false;
            DisableLightingEffects = false;
            DisableDropShot = true;
            DisableNoScope = true;
            DisableHardScope = true;
            DisableHalfScope = true;
            HardScopeLimit = 0.30F;
            QuickScopeLimit = 0.80F;
            LimitSemiAutoRateOfFire = true;
            FeedbackMessages = new Dictionary<string, string>()
            {
                { "noScope", "no scopes are not allowed" },
                { "dropShot", "drop shots are not allowed" },
                { "halfScope", "half scopes are not allowed" },
                { "hardScope", "hard scopes are not allowed" },
                { "fxDisabled", "FX have been disabled." },
                { "fxEnabled", "FX have been enabled." },
                { "cmdGiveAmmoDisabled", "The give ammo command is currently disabled" },
                { "cmdFilmTweakDisabled", "The film tweak command is currently disabled." },
                { "cmdFxDisabled", "The FX command is currently disabled." },
            };
        }

        public ScriptSettings(bool EnableScript, string ServerName, string ServerMessage, bool DeleteBombSites, bool DisableGiveAmmo, bool DisableMelee, bool DisableFallDamage,
                              bool DisableFilmTweaks, bool DisableLightingEffects, bool DisableDropShot, bool DisableNoScope, bool DisableHardScope, bool DisableHalfScope,
                              float HardScopeLimit, float QuickScopeLimit, bool LimitSemiAutoRateOfFire, Dictionary<string, string> FeedbackMessages)
        {
            this.EnableScript = EnableScript;
            this.ServerName = ServerName;
            this.ServerMessage = ServerMessage;
            this.DeleteBombSites = DeleteBombSites;
            this.DisableGiveAmmo = DisableGiveAmmo;
            this.DisableMelee = DisableMelee;
            this.DisableFallDamage = DisableFallDamage;
            this.DisableFilmTweaks = DisableFilmTweaks;
            this.DisableLightingEffects = DisableLightingEffects;
            this.DisableDropShot = DisableDropShot;
            this.DisableNoScope = DisableNoScope;
            this.DisableHardScope = DisableHardScope;
            this.DisableHalfScope = DisableHalfScope;
            this.HardScopeLimit = HardScopeLimit;
            this.QuickScopeLimit = QuickScopeLimit;
            this.LimitSemiAutoRateOfFire = LimitSemiAutoRateOfFire;
            this.FeedbackMessages = FeedbackMessages;
        }

        public void Save()
        {
            List<string> contents = JsonConvert.SerializeObject(this, Formatting.Indented).Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            Utils.WriteFile(new List<string>() { JsonConvert.SerializeObject(this, Formatting.Indented) }, AtlasiSnipe.f_settings, false, false);
        }

        public string TryGetFeedbackMessage(string key)
        {
            string value = string.Empty;

            try
            {
                if (!FeedbackMessages.ContainsKey(key))
                    Log.Error(string.Format("FeedbackMessages does not contain the key {0}.", key));
                else
                    FeedbackMessages.TryGetValue(key, out value);
            }
            catch (ArgumentNullException exception)
            {
                Log.Error("Null key used to access FeedbackMessages.");
            }

            return value;
        }
    }
}
