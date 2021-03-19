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
        //Rect editorWindowPos = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        
        //Make window dynamic? not really needed here?
        Rect editorWindowPos = new Rect();

        //Vessel activeVessel;

        private static Dictionary<int, String> dictNewAGLabels = new Dictionary<int, String>();
        
        // Change these to a single dictionary dictOptions with simple int (bit value) entries?
        
        private static Dictionary<int, bool> dictToggles = new Dictionary<int, bool> {
            { 0,  false },
            { 1,  false },
            { 2,  false },
            { 3,  false },
            { 4,  false },
            { 5,  false },
            { 6,  false },
            { 7,  false },
            { 8,  false },
            { 9,  false },
            { 10, false },
            { 11, false },
            { 12, false },
            { 13, false },
            { 14, false },
            { 15, false },
            { 16, false },
        };

        private static Dictionary<int, bool> dictSinglePress = new Dictionary<int, bool> {
            { 0,  false },
            { 1,  false },
            { 2,  false },
            { 3,  false },
            { 4,  false },
            { 5,  false },
            { 6,  false },
            { 7,  false },
            { 8,  false },
            { 9,  false },
            { 10, false },
            { 11, false },
            { 12, false },
            { 13, false },
            { 14, false },
            { 15, false },
            { 16, false },
        };

        private static Dictionary<int, bool> dictVisible = new Dictionary<int, bool> {
            { 0,  false },
            { 1,  false },
            { 2,  false },
            { 3,  false },
            { 4,  false },
            { 5,  false },
            { 6,  false },
            { 7,  false },
            { 8,  false },
            { 9,  false },
            { 10, false },
            { 11, false },
            { 12, false },
            { 13, false },
            { 14, false },
            { 15, false },
            { 16, false },
        };

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        void Start()
        {
            //activeVessel = FlightGlobals.ActiveVessel;

            editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (AGPanel.visible)
                editorWindowPos = ClickThruBlocker.GUILayoutWindow(editorWindowID, editorWindowPos, DrawEditorWindow, "AG Editor Panel");
                //editorWindowPos = GUILayout.Window(editorWindowID, editorWindowPos, DrawEditorWindow, "AG Editor Panel");

        }

        void DrawEditorWindow(int id)
        {
            //Needs a lot of work formatting the layout correctly and making pretty-er 


            GUI.enabled = true;
            //GUILayout.Button("HERE");
            GUILayout.BeginVertical();
    
            GUILayout.BeginHorizontal();
            GUILayout.Label("Action Group");
            GUILayout.Label("T");               // Make button in flight panel a toggle (not implemented but planing ahead)
            GUILayout.Label("S");               // Remove button from flight after 1st press, to be used for single use AG's
            GUILayout.Label("V");               // Make button active/visible in flight panel
            GUILayout.EndHorizontal();

            for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(String.Format("AG" + i + ": "));
                AGPanel.agLabelMap[i] = GUILayout.TextField(AGPanel.agLabelMap[i], 25);
                if(GUILayout.Toggle(dictToggles[i], ""))
                {
                    dictToggles[i] = !dictToggles[i];
                }
                if (GUILayout.Toggle(dictSinglePress[i], ""))
                {
                    dictSinglePress[i] = !dictSinglePress[i];
                }
                if (GUILayout.Toggle(dictVisible[i], ""))
                {
                    dictVisible[i] = !dictVisible[i];
                }
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
            //GUILayout.Button("GONE");

            GUI.DragWindow();
        }

        private static void UpdateActionGroupLabel(int agID, String label)
        {
            AGPanel.agLabelMap[agID] = label;
        }
    }
}
