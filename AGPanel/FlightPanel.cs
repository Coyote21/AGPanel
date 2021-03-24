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
        //float flightWindowX = 0f;
        //float flightWindowY = 0f;


        public static Rect flightWindowPos = new Rect();

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        //Dictionary of KSP Action Groups
        public static Dictionary<int, KSPActionGroup> dictAG = new Dictionary<int, KSPActionGroup> {
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
 
       //public static List<AGPanel.LabelRec> flightLabelList = new List<AGPanel.LabelRec> {
       //     new AGPanel.LabelRec(1, "Custom01"),
       //     new AGPanel.LabelRec(2, "Custom02"),
       //     new AGPanel.LabelRec(3, "Custom03"),
       //     new AGPanel.LabelRec(4, "Custom04"),
       //     new AGPanel.LabelRec(5, "Custom05"),
       //     new AGPanel.LabelRec(6, "Custom06"),
       //     new AGPanel.LabelRec(7, "Custom07"),
       //     new AGPanel.LabelRec(8, "Custom08"),
       //     new AGPanel.LabelRec(9, "Custom09"),
       //     new AGPanel.LabelRec(10, "Custom10"),
       //     new AGPanel.LabelRec(11, "Light"),
       //     new AGPanel.LabelRec(12, "RCS"),
       //     new AGPanel.LabelRec(13, "SAS"),
       //     new AGPanel.LabelRec(14, "Brakes"),
       //     new AGPanel.LabelRec(15, "Abort"),
       //     new AGPanel.LabelRec(16, "Gear"),
       // };

        void Start()
        {
            AGPanel.activeVessel = FlightGlobals.ActiveVessel;

            flightWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

            //Load Windows pos
            AGPanel.LoadSettings();
            
            //Load data from root part
            LoadAGPDataFromPartModule();
        }

        public void LoadAGPDataFromPartModule()
        {
            AGPModule storageModule = AGPanel.activeVessel.rootPart.Modules.GetModule<AGPModule>();

            for (int i = 0; i < AGPanel.labelList.Count; i++)
            {
                String value = storageModule.Fields.GetValue<String>("AG" + (i + 1));

                AGPanel.labelList[i].Visible = value.Substring(0, 1).Equals("1");
                AGPanel.labelList[i].Visible = value.Substring(1, 1).Equals("1");
                AGPanel.labelList[i].ButtonType = (int.Parse(value.Substring(2, 1)));
                AGPanel.labelList[i].Label = value.Substring(3);
            }
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (AGPanel.flightVisible)
                flightWindowPos = ClickThruBlocker.GUILayoutWindow(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
            //flightWindowPos = GUILayout.Window(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
            
        }

        void DrawFlightWindow(int id)
        {
            //Needs to be smaller, prettyer etc and maybe add a large red button somewhere for abort AG?
           
            GUI.enabled = (FlightGlobals.ActiveVessel != null);
            
            //Need to repalce with pos loaded from file
            //flightWindowPos.x = 100;
            //flightWindowPos.y = 100;
            
            GUILayout.BeginVertical();

            //Loop through the buttons-action groups that are set to be shown
            foreach (AGPanel.LabelRec rec in AGPanel.labelList)
            {
                if (rec.Visible)
                {
                    if (rec.ButtonType == 1)
                    {
                        if(GUILayout.Toggle(rec.Active, rec.Label, HighLogic.Skin.button) != rec.Active)
                        {
                            rec.Active = !rec.Active;
                            ActivateActionGroup(rec.ActionGroup);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(rec.Label))
                        {
                            ActivateActionGroup(rec.ActionGroup);
                            if (rec.ButtonType == 2)
                            {
                                rec.Visible = false;
                                OnGUI();
                            }
                        }
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
