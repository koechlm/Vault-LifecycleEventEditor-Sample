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
using System.Text;

namespace LifecycleEventEditor
{
    public interface ILifeCycleEvent
    {
        void CommitChanges();
        void AddJobEvent();
        void DeleteJobEvent();
        void PopulateUI();

        bool HasPendingChanges { get; }
        bool CanAddJobEvent { get; }
        bool CanDeleteJobEvent { get; }
        bool IsRunning { get; }
    }
}
