using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using KSP.UI.Screens;
using ToolbarControl_NS;


namespace AGPanel
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
   
    public class AGPanel : MonoBehaviour
    {
        ToolbarControl toolbarControl;
        public static bool visible = true;
        
        internal const string MODID = "AGPanel";
        internal const string MODNAME = "AGPanel";

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
