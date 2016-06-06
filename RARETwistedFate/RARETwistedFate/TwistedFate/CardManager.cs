#region copyrights

//  Copyright 2016 Marvin Piekarek
//  CardManager.cs is part of RARETwistedFate.
//  RARETwistedFate is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RARETwistedFate is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with RARETwistedFate. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using LeagueSharp.SDK.Utils;

#endregion

namespace RARETwistedFate.TwistedFate
{
    public enum Cards
    {
        Disabled = -1,
        OffCard = 0,
        BlueCard = 1, // mana card
        GoldCard = 2, // stun card
        RedCard = 3 // aoe-damage card
    }

    internal class CardManager
    {
        public CardsFrame CardFrame;
        public Spell SpellW;

        public CardManager(TwistedFate tf)
        {
            CardFrame = new CardsFrame();
            SpellW = new Spell(SpellSlot.W, tf.Player.GetRealAutoAttackRange());
        }

        public Cards GetActiveCardCombo()
        {
            return CardFrame.CardCombo.ActiveCard;
        }

        public Cards GetActiveCardFarm()
        {
            return CardFrame.CardFarm.ActiveCard;
        }

        public bool IsOn(OrbwalkingMode orbMode)
        {
            switch (orbMode)
            {
                case OrbwalkingMode.Combo:
                    return MenuTwisted.MainMenu["W"]["ComboW"];
                case OrbwalkingMode.LastHit:
                    return MenuTwisted.MainMenu["W"]["FarmW"];
                case OrbwalkingMode.Hybrid:
                    return MenuTwisted.MainMenu["W"]["HybridW"];
                case OrbwalkingMode.LaneClear:
                    return MenuTwisted.MainMenu["W"]["HybridW"];
            }

            return false;
        }

        public bool IsButtonActive()
        {
            return MenuTwisted.MainMenu["Q"]["ShowButton"];
        }

        public Cards GetCardfromString(string name)
        {
            if (name == Cards.BlueCard.ToString())
            {
                return Cards.BlueCard;
            }
            if (name == Cards.GoldCard.ToString())
            {
                return Cards.GoldCard;
            }
            if (name == Cards.RedCard.ToString())
            {
                return Cards.RedCard;
            }
            if (name == Cards.OffCard.ToString())
            {
                return Cards.OffCard;
            }
            if (name == Cards.Disabled.ToString())
            {
                return Cards.Disabled;
            }

            return Cards.Disabled;
        }

        private void InstaPickCardOnUlt()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard")
            {
                SpellW.Cast();
                var castedCard = true;
                while (castedCard)
                {
                    castedCard = !PickCard(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name,
                        GetCardfromString(MenuTwisted.MainMenu["R"]["ActiveCard"].ToString()));
                }

            }
        }


        public void HandleCards(OrbwalkingMode orbMode, bool urgent)
        {
            var counth = GameObjects.EnemyHeroes.Count(x => GameObjects.Player.Distance(x) <= SpellW.Range + 200);

            var countm = GameObjects.EnemyMinions.Count(y => GameObjects.Player.Distance(y) <= SpellW.Range + 200) +
                         GameObjects.EnemyTurrets.Count(t => GameObjects.Player.Distance(t) <= SpellW.Range + 200);

            var countj = GameObjects.Jungle.Count(z => GameObjects.Player.Distance(z) <= SpellW.Range + 200);

            if (urgent && orbMode == OrbwalkingMode.Combo)
            {
                InstaPickCardOnUlt();
            }

            if (orbMode == OrbwalkingMode.Combo && IsOn(orbMode) && counth > 0)
            {
                PickCard(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name, GetActiveCardCombo());
            }
            else if ((orbMode == OrbwalkingMode.Hybrid || orbMode == OrbwalkingMode.LaneClear) && IsOn(orbMode) &&
                     (countj > 0 || countm > 0))
            {
                PickCard(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name, GetActiveCardFarm());
            }
            else if (orbMode == OrbwalkingMode.LastHit && IsOn(orbMode) && countm > 0)
            {
                PickCard(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name, GetActiveCardFarm());
            }
        }

        private bool PickCard(string name, Cards card)
        {
            if (name == "PickACard" && card != Cards.OffCard && card != Cards.Disabled)
            {
                SpellW.Cast();
                return true;
            }
            if (name == "RedCardLock" && card == Cards.RedCard)
            {
                SpellW.Cast();
                return true;
            }
            if (name == "BlueCardLock" && card == Cards.BlueCard)
            {
                SpellW.Cast();
                return true;
            }
            if (name == "GoldCardLock" && card == Cards.GoldCard)
            {
                SpellW.Cast();
                return true;
            }

            return false;
        }
    }
}