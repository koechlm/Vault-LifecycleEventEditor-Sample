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
using System.Windows.Forms;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace LifecycleEventEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            VDF.Vault.Library.Initialize();
            VDF.Vault.Forms.Settings.LoginSettings settings = new VDF.Vault.Forms.Settings.LoginSettings();
            VDF.Vault.Currency.Connections.Connection connection = VDF.Vault.Forms.Library.Login(settings);

            if (connection == null)
                return;

            MainForm mainForm = new MainForm(connection);
            mainForm.ShowDialog();

            VDF.Vault.Library.ConnectionManager.LogOut(connection);
        }
    }
}