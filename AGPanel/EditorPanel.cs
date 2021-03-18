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

        const float WIDTH = 100;
        const float HEIGHT = 200;
        Rect editorWindowPos = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

        Vessel activeVessel;


        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        void Start()
        {
            activeVessel = FlightGlobals.ActiveVessel;

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
          
            GUILayout.BeginVertical();
            for (int i = 1; i < 5; i++)
            {
                if (GUILayout.Button("AG" + i))
                {
                    UpdateActionGroupLabel(i, "AG" + i);
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static void UpdateActionGroupLabel(int agID, String label)
        {
            AGPanel.dictAGLabels[agID] = label;
        }
    }
}
