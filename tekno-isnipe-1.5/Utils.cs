using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InfinityScript;

namespace Atlas
{
    public static class Utils
    {
        public static string GetVersionString()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1)
                                    .AddDays(version.Build).AddSeconds(version.Revision * 2);
            return string.Format("{0}, built on {1}", version, buildDate);
        }

        public static void WriteFile(List<string> contents, string file, bool appendLine, bool appendFile)
        {
            if (!appendFile) { File.WriteAllLines(file, contents); return; }
            foreach (string line in contents)
            {
                using (StreamWriter Writer = new StreamWriter(file, append: appendLine)) { Writer.WriteLine(line); }
            }
        }

        public static Entity GetBombs(string name) =>
            GSCFunctions.GetEnt(name, "targetname");

        public static Entity GetBombTarget(Entity bomb) =>
            GSCFunctions.GetEnt(bomb.Target, "targetname");
        

        public static void DeleteBombCol()
        {
            Entity col = null;
            for (int i = 18; i < 100; i++)
            {
                Entity ent = Entity.GetEntity(i);
                if (ent == null) continue;

                if (ent.GetField<string>("classname") == "script_brushmodel")
                {
                    if (ent.GetField<int>("spawnflags") == 1)
                    {
                        col = ent;
                        break;
                    }
                }
            }
            if (col != null) col.Delete();
        }

        public static void DeleteAllBombSites()
        {        
            if (GSCFunctions.GetDvar("g_gametype") != "sd") return;

            Entity bomb = GetBombs("bombzone");
            Entity bomb1 = GetBombTarget(GetBombTarget(bomb));//Trigger
            if (bomb1 != null) bomb1.Delete();
            bomb1 = GetBombTarget(GetBombs("bombzone"));//model
            if (bomb1 != null) bomb1.Delete();
            bomb1 = GetBombs("bombzone");//plant trigger
            if (bomb1 != null) bomb1.Delete();

            Entity bomb2 = GetBombTarget(GetBombTarget(GetBombs("bombzone")));//Trigger
            if (bomb2 != null) bomb2.Delete();
            bomb2 = GetBombTarget(GetBombs("bombzone"));//model
            if (bomb2 != null) bomb2.Delete();
            bomb2 = GetBombs("bombzone");//plant trigger
            if (bomb2 != null) bomb2.Delete();

            DeleteBombCol();//Collision
            DeleteBombCol();//Collision

            GetBombs("sd_bomb_pickup_trig").Delete();//Bomb pickup trigger
            GetBombs("sd_bomb").Delete();//bomb pickup model

            HudElem bombIcon = HudElem.GetHudElem(65536);
            HudElem bombIcon_enemy = HudElem.GetHudElem(65537);//Unknown?
            HudElem aSite_planting = HudElem.GetHudElem(65538);
            HudElem aSite_defusing = HudElem.GetHudElem(65539);
            HudElem bSite_planting = HudElem.GetHudElem(65540);
            HudElem bSite_defusing = HudElem.GetHudElem(65541);

            bombIcon.Destroy();
            bombIcon_enemy.Destroy();
            aSite_defusing.Destroy();
            aSite_planting.Destroy();
            bSite_planting.Destroy();
            bSite_defusing.Destroy();
        }
    }
}
