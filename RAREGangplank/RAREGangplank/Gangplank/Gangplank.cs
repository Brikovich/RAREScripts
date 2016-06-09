using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace RAREGangplank.Gangplank
{
    class Gangplank
    {
        /// <summary>
        /// Equals the current player with all his data
        /// </summary>
        internal static Obj_AI_Hero Player => ObjectManager.Player;

        /// <summary>
        /// Constant that holds the Championname
        /// </summary>
        private const string HERONAME = "Gangplank";

        /// <summary>
        /// Variable for holding the BarrelSpell
        /// </summary>
        private Barrel barrelSpell;

        private Citrus citrusSpell;

        private Shot shotSpell;

        private Spell RSpell;

        /// <summary>
        /// Loading method for initilaziation of our whole assembly
        /// </summary>
        public void Load()
        {
            if (Player.CharData.BaseSkinName != HERONAME)
                return;

            // init spells
            barrelSpell = new Barrel();
            citrusSpell = new Citrus();
            shotSpell = new Shot();
            RSpell = new Spell(SpellSlot.R);

            // init Menu
            GMenu.Init();

            // subscribe to events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
             
        }

        /// <summary>
        /// OnUpdate Event (ticks every 10ms) 
        /// </summary>
        /// <param name="args"></param>
        private void Game_OnUpdate(EventArgs args)
        {
            // implement champion logic :klappa:

        }

        /// <summary>
        /// The Drawing on Draw event for every drawing we make.
        /// Has lowered refresh rates than GameOnUpdate 
        /// </summary>
        /// <param name="args"></param>
        private void Drawing_OnDraw(EventArgs args)
        {
            var drawQ = GMenu.MainMenu.Item("drawQ").GetValue<Circle>();
            var drawW = GMenu.MainMenu.Item("drawW").GetValue<Circle>();
            var drawE = GMenu.MainMenu.Item("drawE").GetValue<Circle>();

            if (shotSpell.Level >= 1 && drawQ.Active)
                Render.Circle.DrawCircle(Player.Position, shotSpell.Range, Color.DarkGoldenrod);

            if (barrelSpell.Level >= 1 && drawW.Active)
                Render.Circle.DrawCircle(Player.Position, barrelSpell.Range, Color.Firebrick);

        }

        private float GetComboDamage(Obj_AI_Base target)
        {
            var damage = Player.GetComboDamage(target, GetSpellCombo());
            return (float) (damage + Player.GetAutoAttackDamage(target));
        }

        private List<SpellSlot> GetSpellCombo()
        {
            var spelllist = new List<SpellSlot>();

            if(shotSpell.IsReady())
                spelllist.Add(shotSpell.Slot);
            if(barrelSpell.IsReady())
                spelllist.Add(barrelSpell.Slot);
            if(citrusSpell.IsReady())
                spelllist.Add(citrusSpell.Slot);
            if(RSpell.IsReady())
                spelllist.Add(RSpell.Slot);
                
            return spelllist;

        }

    }
}
