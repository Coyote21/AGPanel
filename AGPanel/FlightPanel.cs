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
    public class FlightPanel : MonoBehaviour
    {
        int flightWindowID;

        Rect flightWindowPos = new Rect();

        Vessel activeVessel;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        //Dictionary of KSP Action Groups
        public static Dictionary<int, KSPActionGroup> dictAG = new Dictionary<int, KSPActionGroup> {
            { 0,  KSPActionGroup.Custom01 },
            { 1,  KSPActionGroup.Custom02 },
            { 2,  KSPActionGroup.Custom03 },
            { 3,  KSPActionGroup.Custom04 },
            { 4,  KSPActionGroup.Custom05 },
            { 5,  KSPActionGroup.Custom06 },
            { 6,  KSPActionGroup.Custom07 },
            { 7,  KSPActionGroup.Custom08 },
            { 8,  KSPActionGroup.Custom09 },
            { 9, KSPActionGroup.Custom10 },
            { 10, KSPActionGroup.Light },
            { 11, KSPActionGroup.RCS },
            { 12, KSPActionGroup.SAS },
            { 13, KSPActionGroup.Brakes },
            { 14, KSPActionGroup.Abort },
            { 15, KSPActionGroup.Gear }
        };

        public static List<String> labelList = new List<string> {
            "Custom01",
            "Custom02",
            "Custom03",
            "Custom04",
            "Custom05",
            "Custom06",
            "Custom07",
            "Custom08",
            "Custom09",
            "Custom10",
            "Light",
            "RCS",
            "SAS",
            "Brakes",
            "Abort",
            "Gear"
        };

        public static List<Boolean> visibleList = new List<Boolean> {
             false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };
        public static List<Boolean> toggleList = new List<Boolean> {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };     
        public static List<Boolean> oneDoneList = new List<Boolean> {
             false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };


        void Start()
        {
            activeVessel = FlightGlobals.ActiveVessel;

            flightWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

            //Load data from root part
            LoadAGPData();
        }

        public void LoadAGPData()
        {
            AGPModule storageModule = activeVessel.rootPart.Modules.GetModule<AGPModule>();
            labelList = storageModule.labelMap.Split('~').ToList();
            visibleList = DeserializeBoolList(storageModule.visibleList);
            
            
            //for (int i=0; i < storageModule.visibleList.Length; i++)
            //{
            //    visibleList[i] = ((int) storageModule.visibleList[i] > 0);
            //}
        }

        private List<Boolean> DeserializeBoolList(String s)
        {
            List<Boolean> list = new List<Boolean>();

            for (int i = 0; i < s.Length; i++)
            {
                //if (s[i].Equals("1"))
                //    list.Add(true);
                //else
                //    list.Add(false);

                list.Add(s[i].Equals("1"));
                
                
                Debug.Log("AGPanel.FlightPanel : Deserialize : " + (s[i].Equals("1")));
            }
            
            return list;
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
            //Loop through the buttons-action groups that are set to be shown
            for (int i = 0; i < labelList.Count; i++)
            {
                if (visibleList[i])
                {
                    if (GUILayout.Button(labelList[i]))
                    {
                        ActivateActionGroup(i);
                    }
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();

        }

        private static void ActivateActionGroup(int ag)
        {
            FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(dictAG[ag]);
        }
    }
}
