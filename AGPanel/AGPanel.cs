using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using KSP.UI.Screens;
using ToolbarControl_NS;
using ClickThroughFix;
using System.IO;


namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
   
    public class AGPanel : MonoBehaviour
    {
        
        ToolbarControl toolbarControl;
        public static bool editorVisible = true;
        public static bool flightVisible = true;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        const float EDITORWIDTH = 300;
        const float EDITORHEIGHT = 150;
        const float BUTTON_WIDTH = 120.0f;
        const float BUTTON_HEIGHT = 20.0f;
        const float TOGGLE_HEIGHT = 20.0f;
        const float TOGGLE_WIDTH = 20.0f;
        const float BTYPE_SLIDER_WIDTH = 100.0f;
        const float BTYPE_SLIDER_HEIGHT = 10.0f;
        const float BTYPE_SLIDER_THUMB_WIDTH = 30.0f;
        const float BTYPE_SLIDER_THUMB_HEIGHT = 10.0f;
        const float LABEL_WIDTH = 40.0f;
        const int TEXTFIELD_FONTSIZE = 12;
        const int BUTTON_FONTSIZE = 14;
        const int LABEL_FONTSIZE = 12;
        const float TEXTFIELD_WIDTH = 100f;
        const float TEXTFIELD_HEIGHT = 20.0f;
        static readonly RectOffset PADDING = new RectOffset(0,0,0,0);
        static readonly RectOffset MARGINS = new RectOffset(0, 0, 0, 0);


        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";
 
        public class AGPSettings
        {
            public float editorPanelX = Screen.width / 2 - EDITORWIDTH / 2 + 100;
            public float editorPanelY = Screen.height / 2 - EDITORHEIGHT / 2 - 100;
            public float flightPanelX = Screen.width / 2;
            public float flightPanelY = Screen.height / 2;
        }

        public static AGPSettings agpSettings = new AGPSettings();

        //public static Vessel activeVessel;

        public class LabelRec
        {
            public int ActionGroup;
            public String Label;
            public Boolean Active = false;
            public Boolean Visible = false;
            public int ButtonType = 0;

            public List<String> BtnTypes = new List<String>
            {
                "Plain",
                "Toggle",
                "Single Use"
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
                return (this.Visible ? "1" : "0") + (this.Active ? "1" : "0") + this.ButtonType.ToString() + this.Label;
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

            //InvokeRepeating("UpdateSettingsIfNeeded", 5.0f, 5.0f);
            GameEvents.onGameSceneSwitchRequested.Add(UpdateSettingsIfNeeded);

            //LoadSettings();
        }

        void UpdateSettingsIfNeeded(GameEvents.FromToAction<GameScenes, GameScenes> data)
        {
            if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
            {
                bool doSave = false;
                if (EditorPanel.editorWindowPos.x != agpSettings.editorPanelX || EditorPanel.editorWindowPos.y != agpSettings.editorPanelY)
                {
                    agpSettings.editorPanelX = EditorPanel.editorWindowPos.x;
                    agpSettings.editorPanelY = EditorPanel.editorWindowPos.y;
                    doSave = true;
                }
                if (FlightPanel.flightWindowPos.x != agpSettings.flightPanelX || FlightPanel.flightWindowPos.y != agpSettings.flightPanelY)
                {
                    agpSettings.flightPanelX = FlightPanel.flightWindowPos.x;
                    agpSettings.flightPanelY = FlightPanel.flightWindowPos.y;
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

        public static void SaveSettings()
        {
            
            Debug.Log("AGPanel.SaveSettings: Write JSON");

            try
            {
                StreamWriter writer = new StreamWriter(KSPUtil.ApplicationRootPath + "GameData/AGpanel/Plugins/AGPanelSettings.json", false);
                writer.WriteLine(JsonUtility.ToJson(agpSettings, true));
                writer.Close();
            }
            catch(Exception exp)
            {
                Debug.LogError(exp.Message);
            }

        }

        public static void LoadSettings()
        {

            Debug.Log("AGPanel.LoadSettings: Read JSON");

            if (File.Exists(KSPUtil.ApplicationRootPath + "GameData/AGpanel/Plugins/AGPanelSettings.json"))
            {
                StreamReader reader = new StreamReader(KSPUtil.ApplicationRootPath + "GameData/AGpanel/Plugins/AGPanelSettings.json");
                agpSettings = JsonUtility.FromJson<AGPSettings>(reader.ReadToEnd());
                reader.Close();
            }

            EditorPanel.editorWindowPos.x = agpSettings.editorPanelX;
            EditorPanel.editorWindowPos.y = agpSettings.editorPanelY;
            FlightPanel.flightWindowPos.x = agpSettings.flightPanelX;
            FlightPanel.flightWindowPos.y = agpSettings.flightPanelY;

        }

        [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
        class EditorPanel : MonoBehaviour
        {
            int editorWindowID;            
            public static Rect editorWindowPos = new Rect(agpSettings.editorPanelX, agpSettings.editorPanelX, EDITORWIDTH, EDITORHEIGHT);

            void Start()
            {
                editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

                LoadSettings();
                editorWindowPos.x = agpSettings.editorPanelX;

                if (HighLogic.LoadedSceneIsEditor)
                {
                    //GameEvents.onAboutToSaveShip.Add(StoreAGPDataEditor);
                    //GameEvents.onEditorLoad.Add(LoadAGPDataEditor);
                }
                else if (HighLogic.LoadedSceneIsFlight)
                {
                    //activeVessel = FlightGlobals.ActiveVessel;
                    //May not need this line....test
                    //GameEvents.onAboutToSaveShip.Remove(StoreAGPDataEditor);
                }
                //LoadSettings();
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
                HighLogic.Skin.label.fontSize = LABEL_FONTSIZE;
                HighLogic.Skin.label.alignment = TextAnchor.MiddleCenter;
                HighLogic.Skin.label.fixedWidth = LABEL_WIDTH;
                HighLogic.Skin.button.fontSize = BUTTON_FONTSIZE;
                HighLogic.Skin.textField.fontSize = TEXTFIELD_FONTSIZE;
                HighLogic.Skin.textField.fixedWidth = TEXTFIELD_WIDTH;
                HighLogic.Skin.textField.fixedHeight = TEXTFIELD_HEIGHT;
                HighLogic.Skin.button.fixedHeight = BUTTON_HEIGHT;
                HighLogic.Skin.button.fixedWidth = BUTTON_WIDTH;
                HighLogic.Skin.horizontalSlider.fixedWidth = BTYPE_SLIDER_WIDTH;
                HighLogic.Skin.horizontalSlider.fixedHeight = BTYPE_SLIDER_HEIGHT;
                HighLogic.Skin.horizontalSliderThumb.fixedWidth = BTYPE_SLIDER_THUMB_WIDTH;
                HighLogic.Skin.horizontalSliderThumb.fixedHeight = BTYPE_SLIDER_THUMB_HEIGHT;
                //HighLogic.Skin.textField.padding = PADDING;
                //HighLogic.Skin.textField.margin = MARGINS;
                HighLogic.Skin.label.wordWrap = true;

                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Action Group");
                GUILayout.Label("Vis");               // Make button visible in flight panel
                GUILayout.Label("Type");               // Make button Normal button, Toggle button or "One click, then remove" button
                GUILayout.EndHorizontal();

                foreach (LabelRec rec in labelList)
                {
                    String currentLabel = rec.Label;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(String.Format("AG" + rec.ActionGroup + ": "));
                    rec.Label = GUILayout.TextField(rec.Label, 15);
                    if (rec.Label != currentLabel) rec.Visible = true;      // Auto-toggle Visible if Label is edited
                    rec.Visible = GUILayout.Toggle(rec.Visible, "");

                    GUILayout.BeginVertical();
                    //Checkout Trajectories for adding label to end of slider indicating value
                    rec.ButtonType = Convert.ToInt32(GUILayout.HorizontalSlider((float)rec.ButtonType, 0.0f, 2.0f));
                    HighLogic.Skin.label.fontSize = 10;
                    HighLogic.Skin.label.fixedWidth = BTYPE_SLIDER_WIDTH;
                    RectOffset temp23 = HighLogic.Skin.label.margin;
                    RectOffset temp24 = HighLogic.Skin.label.padding;
                    HighLogic.Skin.label.margin = MARGINS;
                    HighLogic.Skin.label.padding = PADDING;
                    HighLogic.Skin.label.wordWrap = false;
                    GUILayout.Label(rec.BtnTypes[rec.ButtonType]);
                    HighLogic.Skin.label.fontSize = 12;
                    HighLogic.Skin.label.fixedWidth = LABEL_WIDTH;
                    HighLogic.Skin.label.margin = temp23;
                    HighLogic.Skin.label.padding = temp24;
                    HighLogic.Skin.label.wordWrap = true;
                    GUILayout.EndVertical();
                    
                    //Possible alternative if can make btns small enough
                    //rec.ButtonType = GUILayout.Toolbar(rec.ButtonType, Enum.GetNames(typeof(LabelRec.BtnTypes)));
               
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUI.DragWindow();

            }

            
        }

        [KSPAddon(KSPAddon.Startup.Flight, false)]
        public class FlightPanel : MonoBehaviour
        {
            int flightWindowID;

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

            void Start()
            {
                //activeVessel = FlightGlobals.ActiveVessel;

                flightWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

                //Load Windows pos
                LoadSettings();
            
            }

            

            void OnGUI()
            {
                GUI.skin = HighLogic.Skin;
                if (flightVisible)
                    flightWindowPos = ClickThruBlocker.GUILayoutWindow(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
                //flightWindowPos = GUILayout.Window(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
                               
            }

            void DrawFlightWindow(int id)
            {
                //Needs to be smaller, prettyer etc and maybe add a large red button somewhere for abort AG?

                int visibleBtnCount = 0;

                GUI.enabled = (FlightGlobals.ActiveVessel != null);

                GUILayout.BeginVertical();

                //Loop through the buttons that are set to be shown
                foreach (LabelRec rec in labelList)
                {
                    if (rec.Visible)
                    {
                        visibleBtnCount++;
                        if (rec.ButtonType == 1) //Toggle Button
                        {
                            if (GUILayout.Toggle(rec.Active, rec.Label, HighLogic.Skin.button) != rec.Active)
                            {
                                rec.Active = !rec.Active;
                                ActivateActionGroup(rec.ActionGroup);
                                Debug.Log("AGPanel.DrawFlightWindow: Button: " + rec.Label + " is " + rec.Active);
                            }
                        }
                        else
                        {
                            if (GUILayout.Button(rec.Label)) // Normal Button
                            {
                                ActivateActionGroup(rec.ActionGroup);
                                if (rec.ButtonType == 2)  // Click Once then Remove Button
                                {
                                    rec.Visible = false;
                                    visibleBtnCount--;
                                    flightWindowPos.height -= HighLogic.Skin.button.lineHeight + 20.0f;
                                }
                            }
                        }
                    }
                }

                GUILayout.EndVertical();
                flightVisible = visibleBtnCount > 0;
                GUI.DragWindow();
            }

            private static void ActivateActionGroup(int ag)
            {
                FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(dictAG[ag]);
            }
        }

    }
}
