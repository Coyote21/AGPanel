using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using KSP.UI.Screens;
using KSP.IO;
using ToolbarControl_NS;
using ClickThroughFix;



namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class AGPanel : MonoBehaviour
    {
        ToolbarControl toolbarControl;
        bool visible = true;
        int baseWindowID;
        const float WIDTH = 100;
        const float HEIGHT = 200;

        Rect windowPosition = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";

        public static Dictionary<int, KSPActionGroup> dictAG = new Dictionary<int, KSPActionGroup> {
            { 0,  KSPActionGroup.None },
            { 1,  KSPActionGroup.Custom01 },
            { 2,  KSPActionGroup.Custom02 },
            { 3,  KSPActionGroup.Custom03 },
            { 4,  KSPActionGroup.Custom04 },
            { 5,  KSPActionGroup.Custom05 },
            { 6,  KSPActionGroup.Custom06 },
            { 7,  KSPActionGroup.Custom07 },
            { 8,  KSPActionGroup.Custom08 },
            { 9,  KSPActionGroup.Custom09 },
            { 10, KSPActionGroup.Custom10 },
            { 11, KSPActionGroup.Light },
            { 12, KSPActionGroup.RCS },
            { 13, KSPActionGroup.SAS },
            { 14, KSPActionGroup.Brakes },
            { 15, KSPActionGroup.Abort },
            { 16, KSPActionGroup.Gear }
        };

        public static Dictionary<int, String> dictAGLabels = new Dictionary<int, String> {
            { 0,  "Stage" },
            { 1,  "Custom01" },
            { 2,  "Custom02" },
            { 3,  "Custom03" },
            { 4,  "Custom04" },
            { 5,  "Custom05" },
            { 6,  "Custom06" },
            { 7,  "Custom07" },
            { 8,  "Custom08" },
            { 9,  "Custom09" },
            { 10, "Custom10" },
            { 11, "Light" },
            { 12, "RCS" },
            { 13, "SAS" },
            { 14, "Brakes" },
            { 15, "Abort" },
            { 16, "Gear" },
        };

        void Awake()
        {

        }

        void Start()
        {
            Vessel activeVessel = FlightGlobals.ActiveVessel;

            baseWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();
            AddToolbarButton();
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (visible)
                windowPosition = ClickThruBlocker.GUILayoutWindow(baseWindowID, windowPosition, DrawWindow, "AGPanel");
                //windowPosition = GUILayout.Window(baseWindowID, windowPosition, DrawWindow, "AGPanel");

        }


        void AddToolbarButton()
        {
            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(WindowToggle, WindowToggle,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    MODID,
                    "AGPanel",
                    "AGPanel/Icon/AGPanel-38",
                    "AGPanel/Icon/AGPanel-24",
                    MODNAME
                    );
            }
        }

        void WindowToggle()
        {
            visible = !visible;
        }



        void DrawWindow(int id)
        {
            //bool b = false;
            GUI.enabled = (FlightGlobals.ActiveVessel != null);

            GUILayout.BeginVertical();
            for (int i = 1; i < 5; i++)
            {
                if (GUILayout.Button("AG" + i))
                {
                    ActivateActionGroup(i);
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static void ActivateActionGroup(int agID)
        {
            //Actiavte Action Group agID
            FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(dictAG[agID]);
        }
    }

}
