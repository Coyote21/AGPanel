using UnityEngine;
using ToolbarControl_NS;

namespace AGPanel
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class RegisterToolbar : MonoBehaviour
	{
		void Start()
		{
			ToolbarControl.RegisterMod(AGPanel.MODID, AGPanel.MODNAME);
		}
	}
}
