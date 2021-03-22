﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGPanel
{
    public class AGPModule : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String placeHolder = "TY Diazo, Got, DRVeyl, LGG, DarthPointer";
        //Diazo says empty ConfigNodes behave wierd so this just gives KSP something to think about, to distract it.

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String labelMapList = "";

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String toggleList = "000000000000000";

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String onehitList = "000000000000000";

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
        public String lightList = "000000000000000";

    }
}