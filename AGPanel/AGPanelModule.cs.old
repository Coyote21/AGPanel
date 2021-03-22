using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AGPanel
{
    public class AGPanelModule : PartModule
    {

        ConfigNode agpNode = new ConfigNode("AGPNODE");

        public void OnStart()
        {
            for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
            {
                agpNode.AddValue("Custom0" + i + "_label", AGPanel.agLabelMap[i]);
            }
            Debug.Log("AGPanel.AGPPart: OnStart");
        }


        //public void OnSave(ConfigNode node)
        //{
        //    Debug.Log("AGPanel.AGPPart: OnSave");
        //
        //    node.AddNode(agpNode);
        //    
        //    for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
        //    {
        //        node.AddValue("Custom0" + i + "_label", AGPanel.agLabelMap[i]);
        //    }
        //}


        public void Onload(ConfigNode node)
        {
            for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
            {
                AGPanel.agLabelMap[i] = node.GetValue("Custom0" + i + "_label");
            }
            string myValue2 = node.GetValue("myValue");
            Debug.Log("AGPanel.AGPPart: " + myValue2);
        
        }

    }
}
