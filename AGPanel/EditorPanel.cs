using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ClickThroughFix;

namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    class EditorPanel : MonoBehaviour
    {
        int editorWindowID;

        const float WIDTH = 300;
        const float HEIGHT = 300;
        Rect editorWindowPos = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

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

        public static int toggleInt = 0;
        public static int visibleInt = 0;
        public static int oneDoneInt = 0;


        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        void Start()
        {
            editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();
            GameEvents.onAboutToSaveShip.Add(StoreAGPData);
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (AGPanel.visible)
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
            GUILayout.Label("T");               // Make button in flight panel a toggle (not implemented but planing ahead)
            GUILayout.Label("S");               // Remove button from flight after 1st press, to be used for single use AG's
            GUILayout.Label("V");               // Make button active/visible in flight panel
            GUILayout.EndHorizontal();

            for (int i = 0; i < labelList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(String.Format("AG" + (i + 1) + ": "));
                labelList[i] = GUILayout.TextField(labelList[i], 25);
                if(GUILayout.Toggle(toggleList[i], ""))
                {
                    toggleList[i] = !toggleList[i];
                }
                if (GUILayout.Toggle(oneDoneList[i], ""))
                {
                    oneDoneList[i] = !oneDoneList[i];
                }
                if (GUILayout.Toggle(visibleList[i], ""))
                {
                    visibleList[i] = !visibleList[i];
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();

            GUI.DragWindow();

        }

        public void StoreAGPData(ShipConstruct s)
        {
            AGPModule storageModule;

            if (EditorLogic.RootPart.Modules.Contains("AGPModule"))
            {
                storageModule = EditorLogic.RootPart.Modules.GetModule<AGPModule>();

                String serializedLabelList = "";

                foreach (String label in labelList)
                {
                    serializedLabelList += label + "~";
                }
                storageModule.labelMap = serializedLabelList.TrimEnd('~');
                
                storageModule.visibleList = ConvertBoolList2String(visibleList);


            }

        }

        private String ConvertBoolList2String(List<Boolean> list)
        {
            String s = "";
            foreach (Boolean b in list)
            {
                s += (b ? "1" : "0");
            }
            return s;
        }
    }
}
