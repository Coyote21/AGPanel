using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ClickThroughFix;

namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class FlightPanel : MonoBehaviour
    {
        int flightWindowID;

        //const float WIDTH = 100;
        //const float HEIGHT = 200;
        //Rect flightWindowPos = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

        Rect flightWindowPos = new Rect();

        Vessel activeVessel;


        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        void Start()
        {
            activeVessel = FlightGlobals.ActiveVessel;

            flightWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (AGPanel.visible)
                flightWindowPos = ClickThruBlocker.GUILayoutWindow(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
            //flightWindowPos = GUILayout.Window(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");

        }

        void DrawFlightWindow(int id)
        {
            GUI.enabled = (FlightGlobals.ActiveVessel != null);

            GUILayout.BeginVertical();
            for (int i = 1; i < 5; i++)
            {
                if (GUILayout.Button(AGPanel.dictAGLabels[i]))
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
            FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(AGPanel.dictAG[agID]);
        }
    }
}
