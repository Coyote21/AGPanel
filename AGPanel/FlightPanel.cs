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

        public class LabelRec
        {
            public int ActionGroup;
            public String Label;
            public Boolean Active = false;
            public Boolean Visible = false;
            public int ButtonType = 0;

            public enum BtnTypes
            {
                Plain,
                Toggle,
                OneAndDone
            };

            public LabelRec(int actionGroup, string label)
            {
                this.ActionGroup = actionGroup;
                this.Label = label;
                this.Active = false;
                this.Visible = false;
                this.ButtonType = 0;
            }

        }

        public static List<LabelRec> labelList = new List<LabelRec> {
            new LabelRec(1, "Custom01"),
            new LabelRec(2, "Custom02"),
            new LabelRec(3, "Custom03"),
            new LabelRec(4, "Custom04"),
            new LabelRec(5, "Custom05"),
            new LabelRec(6, "Custom06"),
            new LabelRec(7, "Custom07"),
            new LabelRec(8, "Custom08"),
            new LabelRec(9, "Custom09"),
            new LabelRec(10, "Custom10"),
            new LabelRec(11, "Light"),
            new LabelRec(12, "RCS"),
            new LabelRec(13, "SAS"),
            new LabelRec(14, "Brakes"),
            new LabelRec(15, "Abort"),
            new LabelRec(16, "Gear"),
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

            for (int i = 0; i < labelList.Count; i++)
            {
                String value = storageModule.Fields.GetValue<String>("AG" + (i + 1));

                labelList[i].Visible = value.Substring(0, 1).Equals("1");
                labelList[i].ButtonType = (int.Parse(value.Substring(1, 1)));
                labelList[i].Label = value.Substring(2);
            }
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
            foreach (LabelRec rec in labelList)
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
