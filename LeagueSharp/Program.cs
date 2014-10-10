using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace BaseTemplate
{
    class Program
    {
        public static string ChampName = "ChampName"; //Needs Champ Name
        public static Orbwalking.Orbwalker Orbwalker;
        public static Obj_AI_Base Player = ObjectManager.Player; // Instead of typing ObjectManager.Player you can just type Player
        public static Spell Q, W, E, R;
        public static Items.Item DFG;

        public static Menu TPGG;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.BaseSkinName != ChampName) return;

            Q = new Spell(SpellSlot.Q, 0);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 0);
            R = new Spell(SpellSlot.R, 0);
            //Base menu
            TPGG = new Menu("TPGG" + ChampName, ChampName, true);
            //Orbwalker and menu
            TPGG.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(TPGG.SubMenu("Orbwalker"));
            //Target selector and menu
            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            TPGG.AddSubMenu(ts);
            //Combo menu
            TPGG.AddSubMenu(new Menu("Combo", "Combo"));
            TPGG.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            TPGG.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W?").SetValue(true));
            TPGG.SubMenu("Combo").AddItem(new MenuItem("useE", "Use E?").SetValue(true));
            TPGG.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R?").SetValue(true));
            TPGG.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));
            //Exploits
            TPGG.AddItem(new MenuItem("PC", "Packet Cast").SetValue(true));
            //Make the menu visible
            TPGG.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw; // Add onDraw
            Game.OnGameUpdate += Game_OnGameUpdate; // adds OnGameUpdate (Same as onTick in bol)

            Game.PrintChat("THGG" + ChampName + " loaded! By ThePharaohGG");
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (TPGG.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
        }

        public static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical); //Physical or Magical
            if (target == null) return;

            if (target.IsValidTarget(DFG.Range) && DFG.IsReady())
                DFG.Cast(target);

            if (target.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(target, TPGG.Item("PC").GetValue<bool>());

            }
            if (target.IsValidTarget(W.Range) && W.IsReady())
            {
                W.Cast(target, TPGG.Item("PC").GetValue<bool>());
            }
            if (target.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(target, TPGG.Item("PC").GetValue<bool>());
            }
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                R.Cast(target, TPGG.Item("PC").GetValue<bool>());
            }
        }
    }
}