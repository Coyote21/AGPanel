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
        
        //ToolbarControl toolbarControl;
        public static bool editorVisible = false;
        public static bool flightVisible = true;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        // Editor Panel GUIStyle constants
        const float EDITORWIDTH = 250;
        const float EDITORHEIGHT = 150;

        const float LABELHEADINGS_HEIGHT = 25.0f;
        const int HEADING_FONTSIZE = 14;

        const float LINE_HEIGHT = 30.0f;
        
        const int LABELAG_FONTSIZE = 14;
        const float LABELAG_WIDTH = 40.0f;
        const float LABELAG_HEIGHT = 20.0f;
        const int TEXTFIELD_FONTSIZE = 14;
        const float TEXTFIELD_WIDTH = 120f;
        const float TEXTFIELD_HEIGHT = 18.0f;
        const float TOGGLE_WIDTH = 30.0f;
        const float TOGGLE_HEIGHT = 20.0f;
        
        const float TOOLBARBUTTON_WIDTH = 20.0f;
        const float TOOLBARBUTTON_HEIGHT = 20.0f;

        const float BTYPE_SLIDER_WIDTH = 100.0f;
        const float BTYPE_SLIDER_HEIGHT = 8.0f;
        const float BTYPE_SLIDER_THUMB_WIDTH = 30.0f;
        const float BTYPE_SLIDER_THUMB_HEIGHT = 7.5f;
        const float BTYPE_DESC_HEIGHT = 7.0f;

        // Flight Panel GUIStyle constants
        const float BUTTON_WIDTH = 120.0f;
        const float BUTTON_HEIGHT = 20.0f;        
        const int BUTTON_FONTSIZE = 14;


        //Texture2D normal = GameDatabase.Instance.GetTexture("ScienceRelay/Resources/Relay_Normal", false);
        public static Texture2D redButtonHover = GameDatabase.Instance.GetTexture("AGPanel/Icon/RedButtonHover", false);
        public static Texture2D redButtonNormal = GameDatabase.Instance.GetTexture("AGPanel/Icon/RedButtonNormal", false);
        public static Texture2D redButtonActive = GameDatabase.Instance.GetTexture("AGPanel/Icon/RedButtonActive", false);
        

        public class AGPSettings
        {
            public float editorPanelX = Screen.width / 2 - EDITORWIDTH / 2 + 100;
            public float editorPanelY = Screen.height / 2 - EDITORHEIGHT / 2 - 100;
            public float flightPanelX = Screen.width / 2;
            public float flightPanelY = Screen.height / 2;
        }

        public static AGPSettings agpSettings = new AGPSettings();

        [Serializable]
        public class LabelRec 
        {
            public int ActionGroup;
            public String Label;
            public Boolean Active = false;
            public Boolean Visible = false;
            public int ButtonType = 0;
            public Boolean Critical = false;

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
                this.Critical = (actionGroup == 15);
            }

            public String Serialise()
            {
                return (this.Visible ? "1" : "0") + (this.Critical ? "1" : "0") + (this.Active ? "1" : "0") + this.ButtonType.ToString() + this.Label;
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

        [Serializable]
        public class Preset
        {
            public String name;
            public List<String> labels = new List<String>();

            public Preset (String s, List<LabelRec> lt)
            {
                this.name = s;

                foreach (LabelRec rec in lt)
                {
                    this.labels.Add(rec.Serialise());
                }
            }
        }

        void Start()
        {
            GameEvents.onGameSceneSwitchRequested.Add(UpdateSettingsIfNeeded);

            if (!File.Exists(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json"))
            {
                FileStream fs = File.Create(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json");
                fs.Close();
                SaveSettings();
            }

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

        public static void SaveSettings()
        {
            
            try
            {
                StreamWriter writer = new StreamWriter(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json", false);
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

            try
            {
                StreamReader reader = new StreamReader(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json");
                agpSettings = JsonUtility.FromJson<AGPSettings>(reader.ReadToEnd());
                reader.Close();
            }
            catch(Exception exp)
            {
                Debug.LogError(exp.Message);
            }
            
            
            //if (File.Exists(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json"))
            //{
            //    StreamReader reader = new StreamReader(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/AGPanelSettings.json");
            //    agpSettings = JsonUtility.FromJson<AGPSettings>(reader.ReadToEnd());
            //    reader.Close();
            //}
            //else
            //{
            //    SaveSettings();
            //}

            EditorPanel.editorWindowPos.x = agpSettings.editorPanelX;
            EditorPanel.editorWindowPos.y = agpSettings.editorPanelY;
            FlightPanel.flightWindowPos.x = agpSettings.flightPanelX;
            FlightPanel.flightWindowPos.y = agpSettings.flightPanelY;

        }

        [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
        public class EditorPanel : MonoBehaviour
        {
            int editorWindowID;
            public static Rect editorWindowPos = new Rect(agpSettings.editorPanelX, agpSettings.editorPanelX, EDITORWIDTH, EDITORHEIGHT);

            internal const string MODID = "AGPEditor";
            internal const string MODNAME = "AGP Editor";
            private const int Vnotwork = 4;
            ToolbarControl toolbarControl;

            GUIStyle editorHeadingAG;
            GUIStyle editorHeadingLabel;
            GUIStyle editorHeadingVis;
            GUIStyle editorHeadingBtnType;
            
            GUIStyle editorLabelAG;
            GUIStyle editorTextField;
            GUIStyle toggleLight;
            GUIStyle toolbarButtons;
            
            GUIStyle editorSlider;
            GUIStyle editorSliderThumb;
            GUIStyle editorBtnTypeDesc;

            void Start()
            {
                editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

                GameEvents.onGUIActionGroupShown.Add(ShowEditor);
                GameEvents.onGUIActionGroupFlightShown.Add(ShowEditor);
                GameEvents.onGUIActionGroupClosed.Add(HideEditor);

                AddToolbarButton();
                LoadSettings();
                SetGUIStyles(); 
            }

            void ShowEditor()
            {
                editorVisible = true;
            }

            void HideEditor()
            {
                editorVisible = false;
            }

            void SetGUIStyles()
            {

                editorHeadingAG = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = HEADING_FONTSIZE,
                    alignment = TextAnchor.LowerCenter,
                    fixedWidth = LABELAG_WIDTH,
                    fixedHeight = LABELHEADINGS_HEIGHT,
                    wordWrap = true
                };

                editorHeadingLabel = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = HEADING_FONTSIZE,
                    alignment = TextAnchor.LowerCenter,
                    fixedWidth = TEXTFIELD_WIDTH,
                    fixedHeight = LABELHEADINGS_HEIGHT
                };

                editorHeadingVis = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = HEADING_FONTSIZE,
                    alignment = TextAnchor.LowerCenter,
                    fixedWidth = TOGGLE_WIDTH,
                    fixedHeight = LABELHEADINGS_HEIGHT
                };

                editorHeadingBtnType = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = HEADING_FONTSIZE,
                    alignment = TextAnchor.LowerCenter,
                    fixedWidth = BTYPE_SLIDER_WIDTH,
                    fixedHeight = LABELHEADINGS_HEIGHT
                };

                editorLabelAG = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = LABELAG_FONTSIZE,
                    alignment = TextAnchor.UpperCenter,
                    fixedWidth = LABELAG_WIDTH,
                    fixedHeight = LINE_HEIGHT,
                    wordWrap = false
                };

                editorTextField = new GUIStyle(HighLogic.Skin.GetStyle("textField"))
                {
                    fontSize = TEXTFIELD_FONTSIZE,
                    fixedWidth = TEXTFIELD_WIDTH,
                    fixedHeight = TEXTFIELD_HEIGHT,
                    alignment = TextAnchor.MiddleLeft
                };

                toggleLight = new GUIStyle(HighLogic.Skin.GetStyle("toggle"))
                {
                    fixedWidth = TOGGLE_WIDTH,
                    fixedHeight = LINE_HEIGHT
                };

                toolbarButtons = new GUIStyle(HighLogic.Skin.GetStyle("button"))
                {
                    fixedWidth = TOOLBARBUTTON_WIDTH,
                    fixedHeight = TOOLBARBUTTON_HEIGHT
                };

                editorSlider = new GUIStyle(HighLogic.Skin.GetStyle("horizontalSlider"))
                {
                    fixedWidth = BTYPE_SLIDER_WIDTH,
                    fixedHeight = BTYPE_SLIDER_HEIGHT
                };

                editorSliderThumb = new GUIStyle(HighLogic.Skin.GetStyle("horizontalSliderThumb"))
                {
                    fixedWidth = BTYPE_SLIDER_THUMB_WIDTH,
                    fixedHeight = BTYPE_SLIDER_THUMB_HEIGHT
                };
                
                editorBtnTypeDesc = new GUIStyle(HighLogic.Skin.GetStyle("label"))
                {
                    fontSize = 10,
                    fixedWidth = BTYPE_SLIDER_WIDTH - 2,
                    fixedHeight = BTYPE_DESC_HEIGHT,
                    alignment = TextAnchor.UpperCenter,
                    wordWrap = false
                };

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
                // Needs a lot of work formatting the layout correctly and making pretty-er 
                
                GUI.enabled = true;

                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Action Group", editorHeadingAG);
                GUILayout.Label("\nLabel", editorHeadingLabel);
                GUILayout.Label("\nVis", editorHeadingVis);               // Make button visible in flight panel
                GUILayout.Label("\nType", editorHeadingBtnType);               // Make button Normal button, Toggle button or "One click, then remove" button
                GUILayout.Label("\nCrit", editorHeadingVis);               // Make button red in flight panel
                GUILayout.EndHorizontal();

                foreach (LabelRec rec in labelList)
                {
                    String currentLabel = rec.Label;
                    Boolean currentVis = rec.Visible;
                    GUILayout.BeginHorizontal();
                    
                    // AGx: Label
                    GUILayout.Label(String.Format("AG" + rec.ActionGroup + ": "), editorLabelAG);
                    
                    // Custom Label TextField (Text to appear on button in Flight Panel)
                    rec.Label = GUILayout.TextField(rec.Label, 15, editorTextField);
                    if (rec.Label != currentLabel) rec.Visible = true;      // Auto-toggle Visible if Label is edited
                    
                    // Visibility Toggle (Show in Flight Panel T/F)
                    rec.Visible = GUILayout.Toggle(rec.Visible, "", toggleLight);
                    if ((rec.Visible != currentVis) && !rec.Visible) FlightPanel.flightWindowPos.height -= (BUTTON_HEIGHT + 5f);
                    if (rec.Visible && !flightVisible) flightVisible = true;
                    
                    // Button Type - Normal, Toggle or Single Use.
                    GUILayout.BeginVertical();
                    rec.ButtonType = Convert.ToInt32(GUILayout.HorizontalSlider((float)rec.ButtonType, 0.0f, 2.0f, editorSlider, editorSliderThumb));
                    GUILayout.Label(rec.BtnTypes[rec.ButtonType], editorBtnTypeDesc);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();

                    // Possible alternative to slider
                    //String[] toolbarStrings = { "P", "T", "1" };
                    //rec.ButtonType = GUILayout.Toolbar(rec.ButtonType, toolbarStrings, toolbarButtons);

                    // Another alternative, try to do a dropdown selector?

                    // Critical Toggle (Show Red Flight Panel)
                    rec.Critical = GUILayout.Toggle(rec.Critical, "", toggleLight);

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Load Preset"))
                {
                    LoadPresetPopup();
                }
                if (GUILayout.Button("Save as Preset"))
                {
                    SavePresetPopup();
                }
                GUILayout.EndHorizontal();
               
                GUI.DragWindow();

            }

            void LoadPresetPopup()
            {
                // Need to prevent keyboard and mouse clicks from passing thru popup.

                if (!File.Exists(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.txt"))
                {
                    ScreenMessages.PostScreenMessage("Preset File Not Found!\nYou need to Save a preset first");
                    return;
                }

                ConfigNode rootNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.txt");

                List<DialogGUIBase> guiBaseList = new List<DialogGUIBase>();
                guiBaseList.Add(new DialogGUIFlexibleSpace());

                foreach (ConfigNode cn in rootNode.GetNodes())
                {
                    guiBaseList.Add(new DialogGUIButton(cn.name,
                                delegate
                                {
                                    LoadPreset(cn);
                                }, 140.0f, 30.0f, true));
                }

                guiBaseList.Add(new DialogGUIButton("Close", () => { }, 140.0f, 30.0f, true));

                DialogGUIVerticalLayout vertLayout = new DialogGUIVerticalLayout(guiBaseList.ToArray());
                

                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                    new MultiOptionDialog("LoadPreset",
                        "",
                        "Load Preset",
                        HighLogic.UISkin,
                        new Rect(0.5f, 0.5f, 150f, 30f),
                        new DialogGUIFlexibleSpace(),
                        vertLayout),
                    false,
                    HighLogic.UISkin);
            }

            void LoadPreset(ConfigNode node)
            {
                
                foreach (LabelRec rec in labelList)
                {
                    String value = node.GetValue("AG" + rec.ActionGroup);

                    if (value.Length > 0)
                    {
                        rec.Visible = value.Substring(0, 1).Equals("1");
                        rec.Critical = value.Substring(1, 1).Equals("1");
                        rec.Active = value.Substring(2, 1).Equals("1");
                        rec.ButtonType = (int.Parse(value.Substring(3, 1)));
                        rec.Label = value.Substring(4);
                    }
                }

                ScreenMessages.PostScreenMessage("Preset " + node.name + " Loaded");
            }

            void SavePresetPopup()
            {
                // Need to prevent keyboard and mouse clicks from passing thru popup.
                
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                    new MultiOptionDialog("SavePreset",
                        "",
                        "Preset Name",
                        HighLogic.UISkin,
                        new Rect(0.5f, 0.5f, 150f, 35f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIVerticalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUITextInput("Preset 1", false, 25, SavePreset, 25f)
                            )),
                    false,
                    HighLogic.UISkin);
            }

            String SavePreset(String s)
            {
                ConfigNode rootNode;

                if (!File.Exists(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.txt"))
                {
                    rootNode = new ConfigNode();
                }
                else
                {
                    rootNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.txt");
                }
                    
                if (rootNode.HasNode(s))
                {
                    ScreenMessages.PostScreenMessage("Preset " + s + " already exists!");
                } 
                else
                {
                    ConfigNode preset = new ConfigNode(s);
                    foreach (LabelRec rec in labelList)
                    {
                        preset.AddValue("AG" + rec.ActionGroup, rec.Serialise());
                    }
                    rootNode.AddNode(preset);
                    rootNode.Save(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.txt");

                    ScreenMessages.PostScreenMessage("Preset " + s + " Saved");
                    PopupDialog.ClearPopUps();
                }

                // Using JSON. Not working really b/c JsonUtility has issues with Lists and Arrays both of which I need when Saving and Loading
                //Preset preset = new Preset(s, labelList);
                //try
                //{
                //    StreamWriter writer = new StreamWriter(KSPUtil.ApplicationRootPath + "GameData/AGpanel/PluginsData/Presets.json", true);
                //    
                //    writer.WriteLine(JsonUtility.ToJson(preset, true));
                //    writer.Close();
                //}
                //catch (Exception exp)
                //{
                //    Debug.LogError(exp.Message);
                //}


                return s;
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
                        "AGPanel/Icon/AGPEditor-38",
                        "AGPanel/Icon/AGPEditor-24",
                        MODNAME
                        );
                }
            }

            void WindowToggle()
            {
                editorVisible = !editorVisible;
            }


        }

        [KSPAddon(KSPAddon.Startup.Flight, false)]
        public class FlightPanel : MonoBehaviour
        {
            int flightWindowID;
            public static Rect flightWindowPos = new Rect();
            GUIStyle flightButtons;
            GUIStyle flightButtonsCritical;


            internal const string MODID = "AGPFlight";
            internal const string MODNAME = "AGP Flight";
            ToolbarControl toolbarControl;

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

                flightWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();

                AddToolbarButton();

                //Load Windows pos
                LoadSettings();
                SetGUIStyles();
            }

            void SetGUIStyles()
            {
                flightButtons = new GUIStyle(HighLogic.Skin.GetStyle("button"))
                {
                    fontSize = BUTTON_FONTSIZE,
                    fixedWidth = BUTTON_WIDTH,
                    fixedHeight = BUTTON_HEIGHT
                };

                flightButtonsCritical = new GUIStyle(HighLogic.Skin.GetStyle("button"))
                {
                    fontSize = BUTTON_FONTSIZE,
                    fixedWidth = BUTTON_WIDTH,
                    fixedHeight = BUTTON_HEIGHT
                };

                flightButtonsCritical.normal.background = redButtonNormal;
                flightButtonsCritical.hover.background = redButtonHover;
                flightButtonsCritical.focused.background = redButtonHover;
                flightButtonsCritical.active.background = redButtonActive;
                flightButtonsCritical.onActive.background = redButtonActive;
            }

            

            void OnGUI()
            {
                GUI.skin = HighLogic.Skin;
                if (flightVisible)
                {
                    flightWindowPos = ClickThruBlocker.GUILayoutWindow(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
                    //flightWindowPos = GUILayout.Window(flightWindowID, flightWindowPos, DrawFlightWindow, "AG Flight Panel");
                }
            }

            void DrawFlightWindow(int id)
            {
                // Maybe add a large red button somewhere for abort AG? Or make possible to chose color of every button?

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
                            if (GUILayout.Toggle(rec.Active, rec.Label, (rec.Critical ? flightButtonsCritical : flightButtons)) != rec.Active)
                            {
                                rec.Active = !rec.Active;
                                ActivateActionGroup(rec.ActionGroup);
                            }
                        }
                        else
                        {
                            if (GUILayout.Button(rec.Label, (rec.Critical ? flightButtonsCritical : flightButtons)))     // Normal Button (Red Button for Abort AG)
                            {
                                ActivateActionGroup(rec.ActionGroup);
                                if (rec.ButtonType == 2)  // Click Once then Remove Button
                                {
                                    rec.Visible = false;
                                    visibleBtnCount--;
                                    flightWindowPos.height -= (BUTTON_HEIGHT + 5f);
                                }
                            }
                        }
                    }
                }
                GUILayout.EndVertical();
                if (visibleBtnCount == 0) flightVisible = false;
                GUI.DragWindow();
            }

            private static void ActivateActionGroup(int ag)
            {
                FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(dictAG[ag]);
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
                        "AGPanel/Icon/AGPFlight-38",
                        "AGPanel/Icon/AGPFlight-24",
                        MODNAME
                        );
                }
            }

            void WindowToggle()
            {
                flightVisible = !flightVisible;
            }
        }

    }
}
