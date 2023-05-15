using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Characters
{
    internal class CharacterAction
    {
        public bool DoAction(int pips, Character sender, Character target, string action)
        {
            switch (action)
            {
                case "Shield":
                    if (sender.getHeroStatus() == target.getHeroStatus())
                    {
                        target.GetShield(pips);
                        return true;
                    }
                    break;
                case "Hit":
                    if (sender.getHeroStatus() != target.getHeroStatus())
                    {
                        target.GetHit(pips);
                        return true;
                    }
                    break;
                case "Nothink":
                    return true;
                case "Heal":
                    if (sender.getHeroStatus() == target.getHeroStatus())
                    {
                        target.Heal(pips);
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
