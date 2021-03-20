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
                //node.ClearValues();
                String tmpString = "blah";
                for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
                {
                    
                    if (node.TryGetValue("ActionGroup_" + i, ref tmpString)) 
                    {
                        node.SetValue("ActionGroup_" + i, (AGPanel.agLabelMap[i].Length > 0 ? AGPanel.agLabelMap[i] : "badness"));
                        Debug.Log("AGPanel.AGPVessel: VSMSave: Set:" + AGPanel.agLabelMap[i] + " to ActionGroup_" + i);
                    }
                    else
                    {
                        node.AddValue("ActionGroup_" + i, (AGPanel.agLabelMap[i].Length > 0 ? AGPanel.agLabelMap[i] : "badness"));
                        Debug.Log("AGPanel.AGPVessel: VSMSave: Added:" + AGPanel.agLabelMap[i] + " to ActionGroup_" + i);
                    }
                }
            }
            
            
            return node;
        }

        public override void VSMLoad(ConfigNode node)
        {
            if ( validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel )
            {
                Debug.Log("AGPanel.AGPVessel: VSMLoad: vesselType:" + this.Vessel.vesselType.ToString() + " and isActive = " + this.vessel.isActiveVessel);
                //String tmpString, 
                String logString;
                String tmpString = "blkah";

                Debug.Log("VSMLoad: node:" + node.ToString());
        
                for (int i = 1; i < AGPanel.agLabelMap.Count; i++)
                {
                    logString = AGPanel.agLabelMap[i];
                    Debug.Log("Attempting to get Value ActionGroup_" + i + " from node " + this.vessel.id);
                    //tmpString = node.GetValue("ActionGroup_" + i);
                    if (node.TryGetValue("ActionGroup_" + i, ref tmpString)) {
                        AGPanel.agLabelMap[i] = tmpString;
                    }
                    //AGPanel.agLabelMap[i] = (tmpString.Length > 0 ? tmpString : AGPanel.agLabelMap[i]);
                    Debug.Log("dictUpdate: index:" + i + " changed from " + logString + " to " + AGPanel.agLabelMap[i]);
                }
            }
        }

        public override void VSMDestroy()
        {
            base.VSMDestroy();
            //Remove node from file
        }

        public override void OnUnloadVessel()
        {
            base.OnUnloadVessel();
            //Reset LabelMap Dictionary?

            // Not seen this called, tested active vessel to space centre.

            if (validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel)
                Debug.Log("AGPanel.AGPVessel: OnUnLoadVessel: this.vessel.id = " + this.vessel.id.ToString());
        }

        public override void OnLoadVessel()
        {
            base.OnLoadVessel();
            //Put stuff here instead of using VSMLoad?
            // tested! this method is called correctly when loading vessel

            if (validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel)
                Debug.Log("AGPanel.AGPVessel: OnLoadVessel: this.vessel.id = " + this.vessel.id.ToString());

        }

        protected override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            // Tested! this method is called correctly and regularly

            if (validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel)
                Debug.Log("AGPanel.AGPVessel: OnSave: this.vessel.id = " + this.vessel.id.ToString());

        }

        protected override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            // Never seen called (tested load persistent.sfs, new vessel, recover vessel.)

            if (validVesselTypes.Contains(this.Vessel.vesselType) && this.vessel.isActiveVessel)
                Debug.Log("AGPanel.AGPVessel: OnLoad: this.vessel.id = " + this.vessel.id.ToString());
        }

    }
}
