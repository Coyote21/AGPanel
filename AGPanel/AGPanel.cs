﻿using System;
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

    public class AGPanel : PartModule
    {
        ToolbarControl toolbarControl;
        public static bool visible = true;
        
        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";

        
        //Dictionary of KSP Action Groups
        public static Dictionary<int, KSPActionGroup> dictAG = new Dictionary<int, KSPActionGroup> {
            { 0,  KSPActionGroup.None },
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

        //Dictionary of current Action Group labels. Need to load this save if exist, else defaults.
        public static Dictionary<int, String> dictAGLabels = new Dictionary<int, String> {
            { 0,  "Stage" },
            { 1,  "Custom01" },
            { 2,  "Custom02" },
            { 3,  "Custom03" },
            { 4,  "Custom04" },
            { 5,  "Custom05" },
            { 6,  "Custom06" },
            { 7,  "Custom07" },
            { 8,  "Custom08" },
            { 9,  "Custom09" },
            { 10, "Custom10" },
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
            visible = !visible;
        }

        
    }

}
