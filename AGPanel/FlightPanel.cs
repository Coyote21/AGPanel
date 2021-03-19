﻿using System;
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

        //For dynamic sized winodow
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
            //Needs to be smaller, prettyer etc and maybe add a large red button somewhere for abort AG?
            

            GUI.enabled = (FlightGlobals.ActiveVessel != null);
            
            //First attempt at kind of a toggle state for buttons. Dont know how to activate?
            var style = new GUIStyle(GUI.skin.button);
            style.active.textColor = Color.green;
            //needs to be style.active.background = Sunken Image;

            GUILayout.BeginVertical();
            //Load which buttons-action groups are set to be shown then loop through them
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
