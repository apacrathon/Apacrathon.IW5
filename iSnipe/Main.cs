﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InfinityScript;
using Newtonsoft.Json;

namespace Atlas
{
    public class AtlasiSnipe : BaseScript
    {
        // globals
        public static readonly string d_script = @"scripts\AtlasiSnipe";            // assume "../scripts" exists because it's required to load a script in the first place
        public static readonly string f_settings = d_script + @"\settings.txt";
        public static ScriptSettings ScriptConfiguration = new ScriptSettings();    // use default constructor to ensure values always exist
        public static AntiKnife antiKnife;

        private delegate void InitializeScriptDelegate();

        // entry point for InfinityScript
        public AtlasiSnipe() : base()
        {
            InitializeScriptDelegate initializeScriptDelegate = InitializeServerDvars;
            initializeScriptDelegate += InitializeScriptFiles;
            initializeScriptDelegate += InitializeScriptSettings;
            initializeScriptDelegate += InitializeEvents;
            initializeScriptDelegate += InitializeGameModifications;

            try
            {
                initializeScriptDelegate();    
            }
            catch (Exception e)
            {
                Log.Error(string.Format("An exception prevented the initialization of the assembly.\n{0}", e.ToString()));
                return;
            }
        }

        #region INIT
        private void InitializeServerDvars()
        {
            Utils.SetDvar("scr_game_playerwaittime", (Parameter)0.1);    //Only for development purposes so we can get the fuck started
            Utils.SetDvar("scr_game_matchstarttime", (Parameter)5);

            Utils.SetDvar("com_maxfps", 0);
            //Server netcode adjustments//
            Utils.SetDvar("com_maxfps", 0);
            //-IW5 server update rate-
            Utils.SetDevDvar("sv_network_fps", 200);
            //-Setup larger snapshot size and remove/lower delay-
            Utils.SetDvar("sv_hugeSnapshotSize", 10000);
            Utils.SetDvar("sv_hugeSnapshotDelay", 100);
            //-Remove ping degradation-
            Utils.SetDvar("sv_pingDegradation", 0);
            Utils.SetDvar("sv_pingDegradationLimit", 9999);
            //-Teak ping throttle-
            Utils.SetDvar("sv_acceptableRateThrottle", 9999);
            Utils.SetDvar("sv_newRateThrottling", 2);
            //-Tweak ping clamps-
            Utils.SetDvar("sv_minPingClamp", 50);
            //-Increase server think rate per frame-
            Utils.SetDvar("sv_cumulThinkTime", 1000);
            //End server netcode//

            //EXPERIMENTALS
            //-Lock CPU threads-
            Utils.SetDvar("sys_lockThreads", "all");
            //-Prevent game from attempting to slow time for frames-
            Utils.SetDvar("com_maxFrameTime", 1000);
            Utils.SetDvar("cg_drawCrosshair", 1);
            Utils.SetDvar("cl_demo_enabled", 0);
            Utils.SetDvar("cl_demo_recordPrivateMatches", 0);
            Utils.SetDvar("sv_voiceQuality", 9);
            Utils.SetDvar("maxVoicePacketsPerSec", 1000);
            Utils.SetDvar("maxVoicePacketsPerSecForServer", 200);
            Utils.SetDvar("cg_everyoneHearsEveryone", 1);
        }

        private void InitializeScriptFiles()
        {
            Log.Debug("Initializing script files...");
            // Create the working directory if it does not exist
            Directory.CreateDirectory(d_script);
        }

        private void InitializeScriptSettings()
        {
            if (File.Exists(f_settings))
            {
                // Initialize an empty string to store the contents of the settings file
                string serializedSettings = string.Empty;

                using (StreamReader reader = new StreamReader(f_settings))
                {
                    while (reader.Peek() > 0)
                    {
                        string currentLine = reader.ReadLine();
                        if (currentLine.StartsWith("//")) continue;
                        serializedSettings = string.Concat(serializedSettings, currentLine);
                    }
                }

                ScriptConfiguration = JsonConvert.DeserializeObject<ScriptSettings>(serializedSettings);
            }
            else
            {
                Utils.WriteFile(new List<string>() { JsonConvert.SerializeObject(ScriptConfiguration, Formatting.Indented) }, f_settings, false, false);
            }
        }

        private void InitializeEvents()
        {
            Log.Debug("Initializing events...");

            PlayerConnecting += AtlasiSnipe_PlayerConnecting;
            // PlayerConnected += AtlasiSnipe_PlayerConnected;
            // PlayerDisconnected += AtlasiSnipe_PlayerDisconnected;
            
        }

        private void InitializeGameModifications()
        {
            // bomb sites
            if (ScriptConfiguration.DeleteBombSites)
                AfterDelay(750, () => Utils.DeleteAllBombSites());

            // setup AntiKnife (SAT)
            antiKnife = new AntiKnife();
            antiKnife.SetupKnife();

            if (ScriptConfiguration.DisableMelee)
                antiKnife.DisableKnife();
            else
                antiKnife.EnableKnife();
        }
        #endregion

        #region CALLBACKS
        private void AtlasiSnipe_PlayerConnecting(Entity player)
        {
            if (ScriptConfiguration.DisableHardScope)
                CL_AntiHardScope(player);
            if (ScriptConfiguration.LimitSemiAutoRateOfFire)
                CL_AntiSniperSpam(player);
            if (ScriptConfiguration.DisableLightingEffects)
                CL_ResetLightingFogParticles(player);
            if (ScriptConfiguration.DisableFilmTweaks)
                CL_ResetFilmTweaks(player);   
        }
        #endregion

        #region iSNIPE
        private static void CL_AntiHardScope(Entity player)
        {
            if (ScriptConfiguration.DisableHardScope)
            {
                if (ScriptConfiguration.HardScopeLimit < 0f || ScriptConfiguration.HardScopeLimit > 1f)
                {
                    Log.Error("HardScopeLimit must be in the domain (0, 1).");  // this will spam the log, but fuck the server owner for being an idiot
                    return;
                }

                int currentAdsTime = 0;
                player.OnInterval(100, (_player) =>
                {
                    if (!player.IsAlive) { return true; }
                    if (player.Call<float>("playerads") >= 1) { currentAdsTime++; }
                    else { currentAdsTime = 0; }

                    if (currentAdsTime >= ScriptConfiguration.HardScopeLimit * 10)
                    {
                        currentAdsTime = 0;
                        if (Function.Call<string>("weaponclass", player.CurrentWeapon) == "sniper")
                        {
                            player.Call("allowads", false);
                            player.Call("iprintlnbold", ScriptConfiguration.FeedbackMessages["hardScope"]);
                            player.OnInterval(50, (__player) =>
                            {
                                if (player.Call<int>("adsbuttonpressed") == 1) { return true; }
                                player.Call("allowads", true);
                                return false;
                            });

                        }
                    }
                    return true;
                });
            }
        }

        private static void CL_AntiSniperSpam(Entity player)
        {
            player.OnNotify("weapon_fired", (_player, weapon) => {
                string currentWeapon = weapon.ToString();

                if (currentWeapon.StartsWith("iw5_barrett") ||
                    currentWeapon.StartsWith("iw5_dragonuv"))
                {
                    _player.Call("stunplayer", true);
                    _player.AfterDelay(330, (__player) => player.Call("stunplayer", false));
                }
            });
        }

        private static void CL_ResetLightingFogParticles(Entity player)
        {
            player.SetClientDvar("fx_enable", "1");
            player.SetClientDvar("r_fog", "1");
            player.SetClientDvar("fx_drawclouds", "1");
            // more to add
        }

        private static void CL_ResetFilmTweaks(Entity player)
        {
            player.SetClientDvar("r_filmusetweaks", "0");
            player.SetClientDvar("r_filmtweakenabled", "0");
            player.SetClientDvar("r_colorMap", "1");
            player.SetClientDvar("r_specularMap", "1");
            player.SetClientDvar("r_normalMap", "1");
            player.SetClientDvar("r_filmTweakInvert", "0");
            player.SetClientDvar("r_glowTweakEnable", "0");
            player.SetClientDvar("r_glowUseTweaks", "0");
            player.Call("thermalvisionoff");
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (ScriptConfiguration.DisableFallDamage && mod == "MOD_FALLING")
            {
                player.Health += damage;
                return;
            }

            if (attacker == null)
                return;

            float attackerAds = attacker.Call<float>("playerads");

            // Use else-if to avoid restoring extra health
            if (ScriptConfiguration.DisableNoScope && attackerAds <= 0.1f && Function.Call<string>("weaponclass", weapon) == "sniper")
            {
                attacker.Call("iprintlnbold", ScriptConfiguration.TryGetFeedbackMessage("noScope"));
                player.Health += damage;
                return;
            }
            else if (ScriptConfiguration.DisableHalfScope && attackerAds < ScriptConfiguration.QuickScopeLimit)
            {
                attacker.Call("iprintlnbold", ScriptConfiguration.TryGetFeedbackMessage("halfScope"));
                player.Health += damage;
            }
            else if (ScriptConfiguration.DisableDropShot && attacker.Call<string>("getstance") == "prone")
            {
                attacker.Call("iprintlnbold", ScriptConfiguration.TryGetFeedbackMessage("dropShot"));
                player.Health += damage;
                return;
            }
            
            base.OnPlayerDamage(player, inflictor, attacker, damage, dFlags, mod, weapon, point, dir, hitLoc);
        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            if (message[0] == '!')
            {
                CommandHandler.RunCommand(player, message.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                return EventEat.EatGame;
            }

            return base.OnSay3(player, type, name, ref message);
        }
        #endregion
    }
}
