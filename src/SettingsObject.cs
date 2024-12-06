using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    public static class SettingsObject
    {
        /// <summary>
        /// cislo prave otevreneho scenare
        /// </summary>
        public static int scenario = 0;

        /// <summary>
        /// ma se vykreslit barevna mapa?
        /// </summary>
        public static bool colorMap = false;

        /// <summary>
        /// ma se vykreslit mrizka?
        /// </summary>
        public static bool gridShown = false;

        /// <summary>
        /// roztec mrizky na ose X v pixelech
        /// </summary>
        public static int gridX = 50;

        /// <summary>
        /// roztec mrizky na ose Y v pixelech
        /// </summary>
        public static int gridY = 50;

        /// <summary>
        /// instance kresliciho panelu
        /// </summary>
        public static DrawingPanel drawingPanel;

        /// <summary>
        /// instance ovladaciho panelu
        /// </summary>
        public static ControlPanel controlPanel;

        /// <summary>
        /// rohy vykreslovane plochy
        /// </summary>
        public static float[] corners;

        /// <summary>
        /// instance okna s grafy
        /// </summary>
        public static GraphForm graphForm;

        /// <summary>
        /// seznam sond ve scenari
        /// </summary>
        public static List<IProbe> probes;

        /// <summary>
        /// polovina sirky vykresleneho sveta
        /// </summary>
		public static float halfWidth;

        /// <summary>
        /// polovina vysky vykresleneho sveta
        /// </summary>
		public static float halfHeight;

        /// <summary>
        /// bod na stredu kreslicho panelu
        /// </summary>
		public static PointF worldCenter;

        /// <summary>
        /// prodleva mezi snimky
        /// </summary>
        public static int tickLen;

        /// <summary>
        /// zacatek simulace
        /// </summary>
        public static float startTime;

        /// <summary>
        /// prave otevreny soubor
        /// </summary>
        public static string openFile;

        /// <summary>
        /// maximalni pocet sond
        /// </summary>
        public static int maxProbes;

        /// <summary>
        /// skalovani vykresleni sveta
        /// </summary>
        public static float scale;

        /// <summary>
        /// instace okna s tabulkou naboju
        /// </summary>
        public static ChargeTable chargeForm;

        /// <summary>
        /// instance okna s tabulkou sond
        /// </summary>
        public static ProbeTable probeForm;
    }
}
