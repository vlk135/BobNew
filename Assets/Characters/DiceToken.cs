using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Characters
{
    public class DiceToken
    {
        private SideEnum Side { get; set; }
        private int Pips { get; set; }
        private string Action { get; set; }

        public DiceToken(SideEnum side, int pips, string action)
        {
            Side = side;
            Pips = pips;
            Action = action;
        }
        public string GetAction()
        {
            return Action;
        }
        public string getPips()
        {
            return Pips.ToString();
        }
        public int getPipsint()
        {
            return Pips;
        }
    }
}
