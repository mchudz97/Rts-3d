using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Step
    {

        public int resourcesToBuild { get; set; }
        public int barracksToBuild { get; set; }
        public int towersToBuild { get; set; }
        public int soldiersToCreate { get; set; }
        public int tanksToCreate { get; set; }


        public Step(int res, int barr, int tow, int sold, int tanks)
        {

            this.resourcesToBuild = res;
            this.barracksToBuild = barr;
            this.towersToBuild = tow;
            this.soldiersToCreate = sold;
            this.tanksToCreate = tanks;

        }

        public bool StepPassed(int currRes, int currBarr, int currTow, int currSold, int currTanks)
        {

            if(this.resourcesToBuild == currRes && this.barracksToBuild == currBarr 
                && this.towersToBuild == currTow && this.soldiersToCreate == currSold && this.tanksToCreate == currTanks)
            {
                return true;
            }

            return false;

        }

    }
}
