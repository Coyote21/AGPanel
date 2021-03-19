using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using System.IO;
using VesselModuleSaveFramework;


namespace AGPanel
{
    public class AGPVessel : VesselModuleSave
    {

        static List<VesselType> validVesselTypes = new List<VesselType>()
            {
                VesselType.Ship,
                VesselType.Base,
                VesselType.Lander,
                VesselType.Plane,
                VesselType.Probe,
                VesselType.Relay,
                VesselType.Rover,
                VesselType.Station
            };

        public override void VSMStart()
        {
                        
        }

        public override ConfigNode VSMSave(ConfigNode node)
        {
            if( validVesselTypes.Contains(this.Vessel.vesselType) && this.Vessel.isActiveVessel )
            {
                Debug.Log("AGPanel.AGPVessel: VSMSave: vesselType:" + this.Vessel.vesselType.ToString() + " and isActive = " + this.vessel.isActiveVessel);
                node.ClearValues();
                for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
                {
                    node.AddValue("Action Group " + i, (AGPanel.agLabelMap[i].Length > 0 ? AGPanel.agLabelMap[i] : "badness"));
                    Debug.Log("AGPanel.AGPVessel: VSMSave: Saved:" + AGPanel.agLabelMap[i] + " to Action Group " + i);
                }
            }
            
            
            return node;
        }

        //public override void VSMLoad(ConfigNode node)
        //{
        //    if ( validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel )
        //    {
        //        Debug.Log("AGPanel.AGPVessel: VSMLoad: vesselType:" + this.Vessel.vesselType.ToString() + " and isActive = " + this.vessel.isActiveVessel);
        //        String tmpString, logString;
        //        ConfigNode workNode = new ConfigNode();
        //
        //        Debug.Log("VMSLoad: node:" + node.ToString());
        //
        //        
        //        //for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
        //        //{
        //        //    logString = AGPanel.agLabelMap[i];
        //        //    Debug.Log("Attempting to get Value Action Group " + i + " from node " + this.vessel.id);
        //        //    tmpString = node.GetValue("Action Group " + i);
        //        //    AGPanel.agLabelMap[i] = (tmpString.Length > 0 ? tmpString : AGPanel.agLabelMap[i]);
        //        //    Debug.Log("dictUpdate: index:" + i + " changed from " + logString + " to " + AGPanel.agLabelMap[i]);
        //        //}
        //    }
        //}

    }
}
