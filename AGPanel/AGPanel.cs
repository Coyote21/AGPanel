using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using KSP.UI.Screens;
using KSP.IO;
using ToolbarControl_NS;
using ClickThroughFix;



namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    // Want to learn why using this line, instead of KSPAddon, borks the KSP GUI so that I can't save games or Quit from the Pause Menu (Specifically in Flight Scene)
    //[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.EDITOR)]  
    
    // Need to move all this to attach to a module in CommandModule parts so can save settings per vessel.

    public class AGPanel : MonoBehaviour
    {
        ToolbarControl toolbarControl;
        public static bool visible = true;
        
        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";

        //Dictionary of current Action Group labels. Need to load this save if exist, else defaults.
        public static Dictionary<int, String> agLabelMap = new Dictionary<int, String> {
            { 1,  "custom01" },
            { 2,  "custom02" },
            { 3,  "custom03" },
            { 4,  "custom04" },
            { 5,  "custom05" },
            { 6,  "custom06" },
            { 7,  "custom07" },
            { 8,  "custom08" },
            { 9,  "custom09" },
            { 10, "custom10" },
            { 11, "Light" },
            { 12, "RCS" },
            { 13, "SAS" },
            { 14, "Brakes" },
            { 15, "Abort" },
            { 16, "Gear" },
        };

        //private void Awake()
        //{
        //    //Should be here or Start? does it matter?
        //    //AddToolbarButton();
        //}


        // Why VS2019 says this is unused but the AddToolButton(); is called so it must be used???
        void Start()
        {
            AddToolbarButton();
            Debug.Log("AGPanel.AGPanel: OnStart");
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
            Debug.Log("AGPanel.AGPanel: AddToolbarButton");
        }

        void WindowToggle()
        {
            visible = !visible;
            Debug.Log("AGPanel.AGPanel: WindowToggle");
        }
        
    }

}
