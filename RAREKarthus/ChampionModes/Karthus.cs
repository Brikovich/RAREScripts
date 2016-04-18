#region copyright

// Copyright (c) KyonLeague 2016
// If you want to copy parts of the code, please inform the author and give appropiate credits
// File: Karthus.cs
// Author: KyonLeague
// Contact: "cryz3rx" on Skype 

#endregion

#region usage

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK.Core.UI.IMenu;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using SharpDX;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

#endregion

namespace RAREKarthus.ChampionModes
{
    class Karthus : Program
    { 
       
        public static void Init()
        {
            #region skills setup
            // initializing all skills with all attributes
            Utilities.Q = new Spell(SpellSlot.Q, 875)
                .SetSkillshot(1f, 160, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Utilities.W = new Spell(SpellSlot.W, 1000)
                .SetSkillshot(.5f, 70, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Utilities.E = new Spell(SpellSlot.E, 505)
                .SetSkillshot(1f, 505, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Utilities.R = new Spell(SpellSlot.R)
                .SetSkillshot(3f, float.MaxValue, float.MaxValue, false, SkillshotType.SkillshotCircle);
            // setting up there damage type
            Utilities.Q.DamageType = Utilities.W.DamageType = Utilities.E.DamageType = Utilities.R.DamageType = 
                DamageType.Magical;
            // setting up the standart hitchange for Q
            Utilities.Q.MinHitChance = HitChance.High;
            #endregion

            // init the menu for karthus, maybe more champs soon.
            ChampionMenu();

            //game update events - combo etc.
            Game.OnUpdate += Game_OnUpdate;
            // drawing event to draw something => lower refresh rate than game update.
            Drawing.OnDraw += Drawing_OnDraw;

        }

        // drawing Ranges Fappa
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Utilities.Player.IsDead)
            {
                return;
            }

            if (Utilities.MainMenu["Draw"]["Q"] && Utilities.Q.Level > 0)
            {
                Render.Circle.DrawCircle(Utilities.Player.Position,Utilities.Q.Range, Color.AntiqueWhite, 2);
            }

            if (Utilities.MainMenu["Draw"]["W"] && Utilities.W.Level > 0)
            {
                Render.Circle.DrawCircle(Utilities.Player.Position, Utilities.W.Range, Color.ForestGreen, 2);
            }

            if (Utilities.MainMenu["Draw"]["E"] && Utilities.E.Level > 0)
            {
                Render.Circle.DrawCircle(Utilities.Player.Position, Utilities.E.Range, Color.OrangeRed, 2);
            }

            // no need for ult, because it's global Fappa

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            //checking if UltKS is enabled.
            if (Utilities.MainMenu["R"]["KS"])
                //start ultks method
                UltKs();

            // check if ping notifying is enabled.
            if (Utilities.MainMenu["Utilities"]["NotifyPing"])
                // for each player, who's killable with R gets pinged.
                foreach (var enemy in GameObjects.EnemyHeroes.Where(
                            t =>ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready &&
                                t.IsValidTarget() && Utilities.R.GetDamage(t) > t.Health &&
                                t.Distance(ObjectManager.Player.Position) > Utilities.Q.Range))
                {
                    Game.ShowPing(PingCategory.Fallback, enemy);
                }

            switch (Variables.Orbwalker.GetActiveMode())
            {
                case OrbwalkingMode.Combo:
                    Variables.Orbwalker.SetAttackState(Utilities.MainMenu["Modes"]["useaa"] || ObjectManager.Player.Mana < 100);
                    //if no mana, allow auto attacks!
                    Variables.Orbwalker.SetMovementState(Utilities.MainMenu["Modes"]["usemm"]);
                    Combo();
                    break;
                case OrbwalkingMode.Hybrid:
                    Variables.Orbwalker.SetAttackState(true);
                    Variables.Orbwalker.SetMovementState(Utilities.MainMenu["Modes"]["usemm"]);
                    Harass();
                    break;
                case OrbwalkingMode.LaneClear:
                    Variables.Orbwalker.SetAttackState(Utilities.MainMenu["Modes"]["useaa"] || ObjectManager.Player.Mana < 100);
                    Variables.Orbwalker.SetMovementState(Utilities.MainMenu["Modes"]["usemm"]);
                    LaneClear();
                    break;
                case OrbwalkingMode.LastHit:
                    Variables.Orbwalker.SetAttackState(true);
                    Variables.Orbwalker.SetMovementState(Utilities.MainMenu["Modes"]["usemm"]);
                    LastHit();
                    break;
                default:
                    Variables.Orbwalker.SetAttackState(true);
                    Variables.Orbwalker.SetMovementState(true);
                    RegulateEState();
                    break;
            }
        }

        private static void RegulateEState()
        {
        }

        private static void UltKs()
        {
            
        }

        private static void Combo()
        {
            
        }

        private static void Harass()
        {

        }

        // lane clear with Q / E on enabled.
        private static void LaneClear()
        {

        }

        // last hitting method with Q
        public static void LastHit()
        {
            // look up if enabled and Q is ready and isNot in passive form. (before dead)
            if (!Utilities.MainMenu["Q"]["LastQ"] )
                return;

            // get all minions with costum equalations
            var e_minions = GameObjects.EnemyMinions.Where(i => i.IsMinion && Utilities.Q.IsInRange(i) && i.Health <= (Utilities.Q.GetDamage(i)*1.10)                                              && !i.IsDead);
            IEnumerable<Obj_AI_Minion> aiMinions = e_minions as IList<Obj_AI_Minion> ?? e_minions.ToList();
            
            // if minions is NOT empty or if there are any minions
            if (aiMinions.Any())
            {
                var minion = aiMinions.OrderBy(i => i.Health).FirstOrDefault(); // => get first one or default.
                Core.CastQ(minion, Utilities.MainMenu["Q"]["LastMana"]); // => CastQ on it, while refering on your mana.
            }
        }

        // initialisation of the champion menu.
        public static void ChampionMenu()
        {
            var modesSettings = Utilities.MainMenu.Add(new Menu("Modes", "Modes"));
            {
                modesSettings.Bool("useaa", "Use AutoAttacks in Modes");
                modesSettings.Bool("usemm", "Able to Move in Modes");
            }

            var qMenu = Utilities.MainMenu.Add(new Menu("Q", "Q spell"));
            {
                qMenu.Separator("Combo");
                qMenu.Bool("ComboQ", "Use Q");
                qMenu.Separator("Harass");
                qMenu.Bool("HarassQ", "Use Q");
                qMenu.Separator("Farm");
                qMenu.Bool("FarmQ", "Use Q");
                qMenu.Separator("LastHit");
                qMenu.Bool("LastQ", "Use Q");
                qMenu.Slider("LastMana", "min. mana x%", 50);
            }

            var wMenu = Utilities.MainMenu.Add(new Menu("W", "W spell"));
            {
                wMenu.Separator("Combo");
                wMenu.Bool("ComboW", "Use W");
                wMenu.Separator("Harass");
                wMenu.Bool("HarassW", "Use W");
            }

            var eMenu = Utilities.MainMenu.Add(new Menu("E", "E spell"));
            {
                eMenu.Separator("Combo");
                eMenu.Bool("ComboE", "Use E");
                eMenu.Separator("Harass");
                eMenu.Bool("HarassE", "Use E");
            }

            var rMenu = Utilities.MainMenu.Add(new Menu("R", "R spell"));
            {
                rMenu.Separator("Combo");
                rMenu.Bool("ComboR", "Use R");
                rMenu.Separator("Others");
                rMenu.Bool("KS", "KS kill/Save on low life");
            } 

            var comboMenu = Utilities.MainMenu.Add(new Menu("Utilities", "Utilities"));
            {
                comboMenu.Bool("NotifyPing", "On killable ping");
            }

            var drawMenu = Utilities.MainMenu.Add(new Menu("Draw", "Draw"));
            {
                drawMenu.Bool("Q", "Draws Q");
                drawMenu.Bool("W", "Draws W");
                drawMenu.Bool("E", "Draws E");
                drawMenu.Bool("Farm", "Draw Farm-Focus");
            }
        }
    }

    internal class Core
    {

        public static void CastQ(Obj_AI_Base target, int minManaPercent = 0)
        {
            if (!Utilities.Q.IsReady() || !(GetManaPercent() >= minManaPercent))
                return;
            if (target == null)
                return;

            //Utilities.Q.Width = GetDynamicQWidth(target);
            Utilities.Q.Cast(target);

        }

        public static void CastW(Obj_AI_Base target, int minManaPercent = 0)
        {
            if (!Utilities.W.IsReady() || !(GetManaPercent() >= minManaPercent))
                return;
            if (target == null)
                return;
            //Utilities.W.Width = GetDynamicWWidth(target);
            Utilities.W.Cast(target);
        }

        public static float GetManaPercent()
        {
            return (Utilities.Player.Mana / Utilities.Player.MaxMana) * 100f;
        }

        // if Karthus is in his dead form. 
        public static bool IsInPassiveForm()
        {
            return Utilities.Player.IsZombie;
        }
    }
}