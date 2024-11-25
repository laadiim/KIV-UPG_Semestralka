﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    public static class SettingsObject
    {
        public static int scenario = 0;
        public static bool colorMap = false;
        public static bool gridShown = false;
        public static int gridX = 50;
        public static int gridY = 50;
        public static DrawingPanel drawingPanel;
        public static ControlPanel controlPanel;
        public static float[] corners;
        public static GraphPanel graphPanel;
    }
}
