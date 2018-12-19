/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using System.Text;

using Autodesk.Connectivity.WebServices;

namespace LifecycleEventEditor
{
    
    /// <summary>
    /// A container for an LfCycDef object which can be displayed in a list box
    /// </summary>
    public class LfCycDefListItem
    {
        public LfCycDef LfCycDef;

        public LfCycDefListItem(LfCycDef lfCycDef)
        {
            this.LfCycDef = lfCycDef;
        }

        public override string ToString()
        {
            return LfCycDef.DispName;
        }
    }

    /// <summary>
    /// A container for an LfCycState object which can be displayed in a list box
    /// </summary>
    public class LfCycStateListItem
    {
        public LfCycState LfCycState;

        public LfCycStateListItem(LfCycState lfCycState)
        {
            this.LfCycState = lfCycState;
        }

        public override string ToString()
        {
            return LfCycState.DispName;
        }
    }

    /// <summary>
    /// A container for an LfCycTrans object which can be displayed in a list box
    /// </summary>
    public class LfCycTransListItem
    {
        public string DispName;
        public LfCycTrans LfCycTrans;

        public LfCycTransListItem(LfCycTrans lfCycTrans)
        {
            this.LfCycTrans = lfCycTrans;
        }

        public override string ToString()
        {
            return this.DispName;
        }
    }

    /// <summary>
    /// A container for an Workflow object which can be displayed in a list box
    /// </summary>
    public class WorkflowListItem
    {
        public Workflow Workflow;

        public WorkflowListItem(Workflow workflowDef)
        {
            this.Workflow = workflowDef;
        }

        public override string ToString()
        {
            return Workflow.DispName;
        }

    }
    /// <summary>
    /// A container for an WorkflowState object which can be displayed in a list box
    /// </summary>
    public class WorkflowStateListItem
    {
        public WorkflowState WorkflowState;

        public WorkflowStateListItem(WorkflowState workflowState)
        {
            this.WorkflowState = workflowState;
        }

        public override string ToString()
        {
            return WorkflowState.DispName;
        }
    }
    /// <summary>
    /// A container for an Activity object which can be displayed in a list box
    /// </summary>
    public class WorkflowActivityListItem
    {
        public string DispName;
        public Activity Activity;

        public WorkflowActivityListItem(Activity activity)
        {
            this.Activity = activity;
        }

        public override string ToString()
        {
            return this.DispName;
        }
    }
}
