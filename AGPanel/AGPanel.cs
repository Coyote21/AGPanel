using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using KSP.UI.Screens;
using ToolbarControl_NS;
using ClickThroughFix;


namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
   
    public class AGPanel : MonoBehaviour
    {
        //Debug Statements
        public static bool attemptSave = true;
        
        
        
        ToolbarControl toolbarControl;
        public static bool editorVisible = true;
        public static bool flightVisible = true;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        const float EDITORWIDTH = 350;
        const float EDITORHEIGHT = 300;
        public static float editorX = Screen.width / 2 - EDITORWIDTH / 2 + 100;
        public static float editorY = Screen.height / 2 - EDITORHEIGHT / 2 - 100;
        public static float flightX = 0f;
        public static float flightY = 0f;

        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";

        public static ConfigNode settingsNode;

        public static Vessel activeVessel;

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

            public String Serialise()
            {
                return (this.Visible ? "1" : "0") + (this.Visible ? "1" : "0") + this.ButtonType.ToString() + this.Label;
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
            AddToolbarButton();

            InvokeRepeating("UpdateSettingsIfNeeded", 5.0f, 5.0f);

            //LoadSettings();
        }

        void UpdateSettingsIfNeeded()
        {
            if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
            {
                bool doSave = false;
                if (EditorPanel.editorWindowPos.x != editorX || EditorPanel.editorWindowPos.y != editorY)
                {
                    editorX = EditorPanel.editorWindowPos.x;
                    editorY = EditorPanel.editorWindowPos.y;
                    doSave = true;
                }
                if (FlightPanel.flightWindowPos.x != flightX || FlightPanel.flightWindowPos.y != flightY)
                {
                    flightX = FlightPanel.flightWindowPos.x;
                    flightY = FlightPanel.flightWindowPos.y;
                    doSave = true;
                }

                if (doSave) SaveSettings();
            }
        }

        void AddToolbarButton()
        {
            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(WindowToggle, WindowToggle,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
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
            editorVisible = !editorVisible;
            flightVisible = !flightVisible;
        }

        public void OnSave()
        {
            //SaveSettings();
        }

        public static void SaveSettings()
        {
            //Throwaway Node b/c ConfigNode saves only the sub-nodes and not the calling Node itself. 
            ConfigNode throwAway = new ConfigNode();

            //ConfigNode settingsNode = new ConfigNode("AGPanelSettings");
            //ConfigNode editorPanelNode = new ConfigNode("EditorPosition");
            //ConfigNode flightPanelNode = new ConfigNode("FlightPosition");

            //ConfigNode editorPanelNode = settingsNode.GetNode("AGPanelSettings").GetNode("EditorPositon");
            //ConfigNode flightPanelNode = settingsNode.GetNode("AGPanelSettings").GetNode("FlightPosition");

            ConfigNode[] nodeRay = settingsNode.GetNodes();
            ConfigNode editorPanelNode = nodeRay[0];
            ConfigNode flightPanelNode = nodeRay[1];

            editorPanelNode.SetValue("Xpos", editorX, true);
            editorPanelNode.SetValue("Ypos", editorY, true);
            //settingsNode.AddNode(editorPanelNode);

            flightPanelNode.SetValue("Xpos", flightX, true);
            flightPanelNode.SetValue("Ypos", flightY, true);
            //settingsNode.AddNode(flightPanelNode);

            Debug.LogError("AGPanel.SaveSetings: settingsNode: " + settingsNode.ToString());

            throwAway.AddNode(settingsNode);

            throwAway.Save(KSPUtil.ApplicationRootPath + "GameData/AGpanel/Plugins/AGPanel.cfg", "AGPanel Settings File");
        }

        public static void LoadSettings()
        {
            
            ConfigNode rootNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/AGpanel/Plugins/AGPanel.cfg");
            
            settingsNode = rootNode.GetNode("AGPanelSettings");

            ConfigNode[] nodeRay = settingsNode.GetNodes();
            ConfigNode editorPanelNode = nodeRay[0];
            ConfigNode flightPanelNode = nodeRay[1];
            
            //_ = new ConfigNode();
            //ConfigNode editorPanelNode = settingsNode.GetNode("EditorPositon");
            editorX = float.Parse(editorPanelNode.GetValue("Xpos"));
            editorY = float.Parse(editorPanelNode.GetValue("Ypos"));
            
            //_ = new ConfigNode();
            //ConfigNode flightPanelNode = settingsNode.GetNode("FlightPosition");
            flightX = float.Parse(flightPanelNode.GetValue("Xpos"));
            flightY = float.Parse(flightPanelNode.GetValue("Ypos"));

            Debug.Log("AGPanel.LoadSettings: editorX = " + editorX);
            Debug.Log("AGPanel.LoadSettings: editorY = " + editorY);

            EditorPanel.editorWindowPos.x = editorX;
            EditorPanel.editorWindowPos.y = editorY;
        }

        [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
        class EditorPanel : MonoBehaviour
        {
            int editorWindowID;            
            public static Rect editorWindowPos = new Rect(editorX, editorY, EDITORWIDTH, EDITORHEIGHT);

            void Start()
            {
                editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

                if (HighLogic.LoadedSceneIsEditor)
                {
                    GameEvents.onAboutToSaveShip.Add(StoreAGPDataInPartModule);
                    GameEvents.onEditorLoad.Add(LoadAGPDataInEditor);
                }
                else if (HighLogic.LoadedSceneIsFlight)
                {
                    activeVessel = FlightGlobals.ActiveVessel;
                }
                LoadSettings();
            }

            void OnGUI()
            {
                GUI.skin = HighLogic.Skin;
                if (editorVisible)
                    editorWindowPos = ClickThruBlocker.GUILayoutWindow(editorWindowID, editorWindowPos, DrawEditorWindow, "Action Group Label Editor");
                //editorWindowPos = GUILayout.Window(editorWindowID, editorWindowPos, DrawEditorWindow, "AG Editor Panel");
               
            }

            void DrawEditorWindow(int id)
            {
                //Needs a lot of work formatting the layout correctly and making pretty-er 

                GUI.enabled = true;

                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Action Group");
                GUILayout.Label("Vis");               // Make button active/visible in flight panel
                GUILayout.Label("Type");               // Make button in flight panel a toggle (not implemented but planing ahead)
                GUILayout.EndHorizontal();

                foreach (LabelRec rec in labelList)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(String.Format("AG" + rec.ActionGroup + ": "));
                    rec.Label = GUILayout.TextField(rec.Label, 25);
                    rec.Visible = GUILayout.Toggle(rec.Visible, "");

                    //Checkout Trajectories for adding label to end of slider indicating value
                    rec.ButtonType = Convert.ToInt32(GUILayout.HorizontalSlider((float)rec.ButtonType, 0.0f, 2.0f));
                    //Possible alternative if can make btns small enough
                    //rec.ButtonType = GUILayout.Toolbar(rec.ButtonType, Enum.GetNames(typeof(LabelRec.BtnTypes)));
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUI.DragWindow();

            }

            public void StoreAGPDataInPartModule(ShipConstruct s)
            {
                AGPModule storageModule;

                if (EditorLogic.RootPart.Modules.Contains("AGPModule"))
                {
                    storageModule = EditorLogic.RootPart.Modules.GetModule<AGPModule>();

                    foreach (LabelRec rec in labelList)
                    {
                        storageModule.Fields.SetValue("AG" + rec.ActionGroup, rec.Serialise());
                        //Debug.Log("AGPanel: EditorPanel: StoreAGPData: SetValue: AG" + rec.ActionGroup + " = " + rec.Serialise());
                    }
                }
            }

            public void LoadAGPDataInEditor(ShipConstruct s, KSP.UI.Screens.CraftBrowserDialog.LoadType loadType)
            {
                // This is not working, it is pulling default data from somewhere other than the .craft files, maybe its
                // holding on to the values from before the load?
                // I think I will need to change EditorLogic.RootPart for something else???


                AGPModule storageModule = EditorLogic.RootPart.Modules.GetModule<AGPModule>();

                for (int i = 0; i < labelList.Count; i++)
                {
                    String value = storageModule.Fields.GetValue<String>("AG" + (i + 1));

                    labelList[i].Visible = value.Substring(0, 1).Equals("1");
                    //labelList[i].Active = value.Substring(1, 1).Equals("1");
                    labelList[i].Active = false;
                    labelList[i].ButtonType = (int.Parse(value.Substring(2, 1)));
                    labelList[i].Label = value.Substring(3);
                    //Debug.Log("AGPanel: EditorPanel: LoadAGPData: value = " + value);
                    //Debug.Log("AGPanel: EditorPanel: LoadAGPData: labeList[" + i + "].Label = " + value.Substring(3));
                }
            }
        }

    }

    
}
