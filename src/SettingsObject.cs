using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPG_SP_2024
{
    public class SettingsObject
    {
        public int scenario;
        public bool colorMap;
        public bool gridShown;
        public int gridX;
        public int gridY;
        public SettingsObject(int scenario, int gridX, int gridY) 
        {
            this.scenario = scenario;
            this.colorMap = false;
            this.gridShown = false;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}
