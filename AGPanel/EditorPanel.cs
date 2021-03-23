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

        //public static List<String> labelList = new List<string> {
        //    "Custom01",
        //    "Custom02",
        //    "Custom03",
        //    "Custom04",
        //    "Custom05",
        //    "Custom06",
        //    "Custom07",
        //    "Custom08",
        //    "Custom09",
        //    "Custom10",
        //    "Light",
        //    "RCS",
        //    "SAS",
        //    "Brakes",
        //    "Abort",
        //    "Gear"
        //};

        public class LabelRec
        {
            public int ActionGroup;
            public String Label;
            public Boolean Visible = false;
            public BtnTypes ButtonType = BtnTypes.Plain;

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
                this.Visible = false;
                this.ButtonType = BtnTypes.Plain;
            }

            public String Serialise()
            {
                return (this.Visible ? "1" : "0") + this.ButtonType.ToString("d") + this.Label;
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

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        void Start()
        {
            editorWindowID = UnityEngine.Random.Range(1000, 20000000) + _AssemblyName.GetHashCode();
            GameEvents.onAboutToSaveShip.Add(StoreAGPData);
            GameEvents.onEditorLoad.Add(LoadAGPData);
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
            GUILayout.Label("V");               // Make button active/visible in flight panel
            //GUILayout.Label("T");               // Make button in flight panel a toggle (not implemented but planing ahead)
            //GUILayout.Label("S");               // Remove button from flight after 1st press, to be used for single use AG's
            GUILayout.EndHorizontal();

            foreach (LabelRec rec in labelList)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(String.Format("AG" + rec.ActionGroup + ": "));
                rec.Label = GUILayout.TextField(rec.Label, 25);
                rec.Visible = GUILayout.Toggle(rec.Visible, "");
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

                foreach (LabelRec rec in labelList)
                {
                    storageModule.Fields.SetValue("AG" + rec.ActionGroup, rec.Serialise());
                    //Debug.Log("AGPanel: EditorPanel: StoreAGPData: SetValue: AG" + rec.ActionGroup + " = " + rec.Serialise());
                }
            }
        }

        public void LoadAGPData(ShipConstruct s, KSP.UI.Screens.CraftBrowserDialog.LoadType loadType)
        {
            // This is not working, it is pulling default data from somewhere other than the .craft files, maybe its
            // holding on to the values from before the load?
            // I think I will need to change EditorLogic.RootPart for something else???
            
            
            AGPModule storageModule = EditorLogic.RootPart.Modules.GetModule<AGPModule>();

            for (int i = 0; i < labelList.Count; i++)
            {
                String value = storageModule.Fields.GetValue<String>("AG" + (i + 1));

                labelList[i].Visible = value.Substring(0, 1).Equals("1");
                labelList[i].ButtonType = (LabelRec.BtnTypes)Enum.Parse(typeof(LabelRec.BtnTypes), value.Substring(1, 1));
                labelList[i].Label = value.Substring(2);
                Debug.Log("AGPanel: EditorPanel: LoadAGPData: value = " + value);
                Debug.Log("AGPanel: EditorPanel: LoadAGPData: labeList[" + i + "].Label = " + value.Substring(2));
            }
        }
    }
}
