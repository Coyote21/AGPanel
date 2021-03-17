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

        
        void Awake()
        {

        }

        void Start()
        {
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
                toolbarControl.AddToAllToolbars(windowToggle, windowToggle,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    MODID,
                    "AGPanel",
                    "AGPanel/Icon/AGPanel-38",
                    "AGPanel/Icon/AGPanel-24",
                    MODNAME
                    );
            }
        }

        void windowToggle()
        {
            visible = !visible;
        }



        void DrawWindow(int id)
        {
            bool activeVessel = (FlightGlobals.ActiveVessel != null);

            bool b = false;
            GUI.enabled = activeVessel;

            GUILayout.BeginVertical();
            for (int i = 1; i < 5; i++)
            {
                if (GUILayout.Button("AG" + i))
                {
                    b = activateActionGroup(i);
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static bool activateActionGroup(int ag)
        {
            //Actiavte AG ag

            return true;
        }
    }

}
