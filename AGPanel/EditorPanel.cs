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
        Rect editorWindowPos = new Rect();

        //Vessel activeVessel;

        private static Dictionary<int, String> dictNewAGLabels = new Dictionary<int, String>();
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

        private static Dictionary<int, bool> dictVisible = new Dictionary<int, bool>();

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

            GUI.enabled = true;
            GUILayout.Button("HERE");
            GUILayout.BeginVertical();
            for (int i = 1; i < 5; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(String.Format("AG" + i + ": "));
                AGPanel.dictAGLabels[i] = GUILayout.TextField(AGPanel.dictAGLabels[i], 25);
                if(GUILayout.Toggle(dictToggles[i], ""))
                {
                    dictToggles[i] = !dictToggles[i];
                }
                //dictToggles[i] = GUILayout.Toggle(dictToggles[i], "T");
                //dictVisible[i] = GUILayout.Toggle(dictVisible[i], "V");
                //if (GUILayout.Button("S"))
                //{
                //    //UpdateActionGroupLabel(i, dictNewAGLabels[i]);
                //}
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.Button("GONE");
            if(GUILayout.Toggle(false, "T"))
            {
                //dictToggles[i] = !dictToggles[i];
            }
            GUI.DragWindow();
        }

        private static void UpdateActionGroupLabel(int agID, String label)
        {
            AGPanel.dictAGLabels[agID] = label;
        }
    }
}
