using UnityEngine;
using ToolbarControl_NS;

namespace AGPanel
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class RegisterToolbar : MonoBehaviour
	{
		void Start()
		{
			ToolbarControl.RegisterMod(AGPanel.EditorPanel.MODID, AGPanel.EditorPanel.MODNAME);
			ToolbarControl.RegisterMod(AGPanel.FlightPanel.MODID, AGPanel.FlightPanel.MODNAME);
		}
	}
}
