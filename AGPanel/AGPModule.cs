
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AGPanel
{
    public class AGPModule : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String placeHolder = "TY Diazo, Got, DRVeyl, Standecco, LGG, DarthPointer";
        //Diazo says empty ConfigNodes behave wierd so this just gives KSP something to think about, to distract it.
        //Source: AGExt source code

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG1 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG2 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG3 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG4 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG5 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG6 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG7 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG8 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG9 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG10 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG11 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG12 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG13 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG14 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG15 = "";
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String AG16 = "";


        public override void OnSave(ConfigNode node)
        {
            foreach (AGPanel.LabelRec rec in AGPanel.labelList)
            {
                node.SetValue("AG" + rec.ActionGroup, rec.Serialise(), true);
                this.Fields.SetValue("AG" + rec.ActionGroup, rec.Serialise());
                //Debug.Log("AGPanel.AGPModule: OnSave: Save: AG" + rec.ActionGroup + " = " + rec.Serialise());
            }
            base.OnSave(node);
        }

        public override void OnLoad(ConfigNode node)
        {
            foreach (AGPanel.LabelRec rec in AGPanel.labelList)
            {
                String value = node.GetValue("AG" + rec.ActionGroup);

                if (value.Length > 0)
                {
                    //Debug.Log("AGPanel.AGPModule: OnLoad: value = " + value);
                    rec.Visible = value.Substring(0, 1).Equals("1");
                    rec.Active = value.Substring(1, 1).Equals("1");
                    rec.ButtonType = (int.Parse(value.Substring(2, 1)));
                    rec.Label = value.Substring(3);
                }
            }
            base.OnLoad(node);
        }
    }
}
