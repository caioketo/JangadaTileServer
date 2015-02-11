using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Utils
{
    class Stats
    {
        public enum STAT
        {
            STR,
            INT,
            DEX,
            CONS,
            WIS
        }

        int STR;
        int INT;
        int DEX;
        int CONS;
        int WIS;

        public Stats()
        {
            STR = 10;
            INT = 10;
            DEX = 10;
            CONS = 10;
            WIS = 10;
        }

        public int Get(STAT stat)
        {
            switch (stat)
            {
                case STAT.STR:
                    return STR;
                case STAT.INT:
                    return INT;
                case STAT.DEX:
                    return DEX;
                case STAT.CONS:
                    return CONS;
                case STAT.WIS:
                    return WIS;
                default:
                    return 0;
            }
        }
    }
}
