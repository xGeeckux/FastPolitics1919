using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Battle
    {
        public List<Unit> Attackers { get; set; }
        public List<Unit> Defenders { get; set; }

        private List<Unit> FallenAttackers { get; set; }
        private List<Unit> FallenDefenders { get; set; }

        private static Random Random = new Random();

        public Battle(List<Unit> attacker, List<Unit> defender)
        {
            FallenAttackers = new List<Unit>();
            FallenDefenders = new List<Unit>();
            Attackers = attacker;
            Defenders = defender;

            foreach (Unit unit in Attackers)
                unit.IsFighting = true;
            foreach (Unit unit in Defenders)
                unit.IsFighting = true;
        }

        public bool Update()
        {
            if (IsOver())
                return true;

            //Log.Write("Angreifer am Zug");
            Attack(Attackers, Defenders);
            //Console.ReadLine();
            //Log.Write("Verteidiger am Zug");
            Attack(Defenders, Attackers);
            //Console.ReadLine();
            //Log.Write("Überprüfe gefallene");
            CheckFallen();
            Status();
            //Console.ReadLine();
            //Console.Clear();
            return false;
        }

        //- Attack
        private void Attack(List<Unit> attackers, List<Unit> defenders)
        {
            int n = 0;
            foreach (Unit unit in attackers)
            {
                n = Random.Next(0, defenders.Count);
                Attack(unit, defenders[n]);
            }
        }
        private void CalcDamage(Unit attacker, Unit defender, double modifier)
        {
            double damage = GetDamage(attacker.BattleValues.SoftAttack * modifier) * (1 + GetUnitModifiers(attacker));

            double org_damage = damage;
            double man_damage = -damage * 2d;
            if (defender.BattleValues.SoftDefence <= 0)
                man_damage *= 4;

            defender.AddOrganisation(-org_damage);
            defender.AddStrength((int)man_damage);
            //- Debug off
            //defender.BattleValues.SoftDefence -= org_damage;


            //Log.Write("\t\t" + attacker.Name + " verursacht: " + org_damage + " Org.-Schaden und " + (int)man_damage + " Mann-Schaden.");
            //WriteStatus(defender);
        }
        private void Attack(Unit attacker, Unit defender)
        {
            if (attacker.BattleValues.SoftAttack > defender.BattleValues.SoftDefence)
                CalcDamage(attacker, defender, 0.9);
            else
                CalcDamage(attacker, defender, 0.6);

            Unit new_att = defender;
            Unit new_def = attacker;
            if (new_att.BattleValues.SoftAttack > new_def.BattleValues.SoftAttackDefence)
                CalcDamage(attacker, defender, 0.8);
            else
                CalcDamage(attacker, defender, 0.5);
        }
        private double GetDamage(double value)
        {
            value /= 10;
            int extra = (int)value;
            int r = Random.Next(0, 6 + 1);
            if (r == 6) r += 1;
            return value + (extra / 2) + (r / 2);
        }
        private double GetUnitModifiers(Unit unit)
        {
            double modf = 0;

            //- Commander Bonus
            if (unit.Commander != null)
                modf += 0.5;

            //- Nation Bonus
            if (unit.Owner != null && unit.Owner is Country)
                modf += 0.1;

            return modf;
        }

        //- Fallen
        private void CheckFallen()
        {
            List<Unit> new_attackers = new List<Unit>();
            foreach (Unit unit in Attackers)
            {
                if (unit.CurOrganisation <= 0 || unit.CurStrength <= (unit.MaxStrength * 0.1d))
                    FallenAttackers.Add(unit);
                else
                    new_attackers.Add(unit);
            }
            Attackers = new_attackers;

            List<Unit> new_defenders = new List<Unit>();
            foreach (Unit unit in Defenders)
            {
                if (unit.CurOrganisation <= 0 || unit.CurStrength <= (unit.MaxStrength * 0.1d))
                    FallenDefenders.Add(unit);
                else
                    new_defenders.Add(unit);
            }
            Defenders = new_defenders;
        }

        //- Status
        private void Status()
        {
            Log.Write("");
            Log.Write("<-- Alive -->");
            Log.Write("");
            Log.Write("Attackers:");
            foreach (Unit unit in Attackers)
                WriteStatus(unit);
            Log.Write("");
            Log.Write("Defenders:");
            foreach (Unit unit in Defenders)
                WriteStatus(unit);
            Log.Write("");
            Log.Write("<-- Fallen -->");
            Log.Write("");
            Log.Write("Fallen (Attackers):");
            foreach (Unit unit in FallenAttackers)
                WriteStatus(unit);
            Log.Write("");
            Log.Write("Fallen (Defenders):");
            foreach (Unit unit in FallenDefenders)
                WriteStatus(unit);
            Log.Write("");
        }
        private void WriteStatus(Unit unit)
        {
            Log.Write(unit.Name + " Org:[" + unit.CurOrganisation + "/" + unit.MaxOrganisation + "]" + " Mann:[" + unit.BattleValues.CurStrength + "/" + unit.BattleValues.MaxStrength + "]");
        }

        //- Check Status
        private bool IsOver()
        {

            if (Attackers.Count == 0)
            {
                Log.Write("Kampf beendet, Verteidiger haben gewonnen !");
                BattleOver();
                Retreat(FallenAttackers);
                return true;
            }
            if (Defenders.Count == 0)
            {
                Log.Write("Kampf beendet, Angreifer haben gewonnen !");
                BattleOver();
                Retreat(FallenDefenders);
                return true;
            }
            return false;
        }

        //- If Lost
        private void Retreat(List<Unit> units)
        {
            if (units.Count == 0)
                return;
            Tile cur = units[0].Location;
            List<Tile> neighbours = cur.GetNeighbours();
            int i = Random.Next(0, neighbours.Count);
            
            foreach (Unit unit in units)
            {
                if (neighbours.Count == 0)
                {
                    unit.KillEveryone();
                    Engine.Game.Units.Remove(unit.ID);
                }
                else
                    unit.MoveTo(neighbours[i]);
            }
        }

        private void ResetValues(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                unit.IsFighting = false;
                if (unit.BattleValues.CurOrganisation < 0)
                    unit.BattleValues.CurOrganisation = 1;
                if (unit.BattleValues.CurStrength <= 0)
                {
                    unit.KillEveryone();
                    Engine.Game.Units.Remove(unit.ID);
                }
                if (unit.BattleValues.CurStrength > 0)
                    unit.AddStrength((int)(unit.BattleValues.MaxStrength * 0.4d));
            }
        }
        private void BattleOver()
        {
            ResetValues(Attackers);
            ResetValues(FallenAttackers);
            ResetValues(Defenders);
            ResetValues(FallenDefenders);
        }
    }
}
