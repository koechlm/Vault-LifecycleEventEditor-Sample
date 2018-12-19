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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Resources;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace LifecycleEventEditor
{
    public partial class MainForm : Form
    {
        private VDF.Vault.Currency.Connections.Connection m_connection;
        private List<ILifeCycleEvent> m_lifecycleEvents;

        Guid m_currentThreadId = Guid.Empty;

        public MainForm(VDF.Vault.Currency.Connections.Connection connection)
        {
            InitializeComponent();

            m_connection = connection;

            m_entityLifeCycleEvent.Connection = m_connection;
            m_changeOrderLifeCycleEvent.Connection = m_connection;

            m_lifecycleEvents = new List<ILifeCycleEvent>() { m_entityLifeCycleEvent, m_changeOrderLifeCycleEvent };

            PopulateUI();

            addJobToTransitionToolStripMenuItem.Enabled = false;
            deleteJobFromTransitionToolStripMenuItem.Enabled = false;
            commitJobsToolStripMenuItem.Enabled = false;
        }

        #region UI_events
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lifecycleEvents.Any(n => n.HasPendingChanges))
            {
                string msgCaption = Properties.Resources.IDS_EXIT_CAPTION;
                string messageBody = Properties.Resources.IDS_HAS_PENDING_CHANGES;
                DialogResult result = MessageBox.Show(
                    messageBody, msgCaption, MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;
            }

            DialogResult = DialogResult.OK;
        }

        private void commitJobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CommitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CommitChanges()
        {
            if (!m_lifecycleEvents.Any(n => n.HasPendingChanges))
                return;

            // disable UI
            this.UseWaitCursor = true;
            m_tabControl.Enabled = false;
            this.menuStrip1.Enabled = false;

            m_currentThreadId = Guid.NewGuid();
            ThreadPool.QueueUserWorkItem(CommitChangesWorker, m_currentThreadId);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!m_currentThreadId.Equals(Guid.Empty))
            {
                MessageBox.Show(Properties.Resources.IDS_COMMIT_RUNNING);
                e.Cancel = true;
            }
        }

        private void CommitChangesWorker(object o)
        {
            Guid threadId = (Guid)o;

            foreach (ILifeCycleEvent lifeCycleEvent in m_lifecycleEvents)
            {
                if (lifeCycleEvent.HasPendingChanges)
                {
                    lifeCycleEvent.CommitChanges();
                }
            }

            // wait for the commits to complete
            while (m_lifecycleEvents.Any(n => n.IsRunning))
            {
                Thread.Sleep(100);
            }

            // Process the results on the UI Thread
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (threadId.Equals(m_currentThreadId))
                {
                    // re-enable UI
                    this.UseWaitCursor = false;
                    m_tabControl.Enabled = true;
                    this.menuStrip1.Enabled = true;

                    string messageBody = Properties.Resources.IDS_OPERATION_COMPLETE;
                    MessageBox.Show(messageBody);

                    m_currentThreadId = Guid.Empty;
                }
            });
        }

        private void addJobToTransitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleLifeCycleEvent.AddJobEvent();
        }

        private void deleteJobFromTransitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleLifeCycleEvent.DeleteJobEvent();
        }

        private void actionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetToolStripMenuItemEnbled();
        }

        private void m_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisibleLifeCycleEvent.PopulateUI();
            GetToolStripMenuItemEnbled();
        }
        #endregion // UI_events

        public ILifeCycleEvent VisibleLifeCycleEvent
        {
            get
            {
                ILifeCycleEvent iEvent = m_entityLifeCycleEvent;
                switch (m_tabControl.SelectedIndex)
                {
                    case 0:
                        iEvent = m_entityLifeCycleEvent;
                        break;
                    case 1:
                        iEvent = m_changeOrderLifeCycleEvent;
                        break;
                    default:
                        break;
                }

                return iEvent;
            }
        }

        private void GetToolStripMenuItemEnbled()
        {
            addJobToTransitionToolStripMenuItem.Enabled = VisibleLifeCycleEvent.CanAddJobEvent;
            deleteJobFromTransitionToolStripMenuItem.Enabled = VisibleLifeCycleEvent.CanDeleteJobEvent;
            commitJobsToolStripMenuItem.Enabled = m_lifecycleEvents.Any(n => n.HasPendingChanges);
        }

        private void PopulateUI()
        {
            // get supported products
            Product[] products = m_connection.WebServiceManager.InformationService.GetSupportedProducts();

            bool supportProductStream = false;
            foreach (Product product in products)
            {
                if (product.ProductName.Contains("Autodesk.Productstream"))
                    supportProductStream = true;
            }

            // remove ChangeOrder tabpages if server doesn't support.
            if (!supportProductStream)
            {
                m_tabControl.TabPages.Remove(m_coTabPage);
            }

            VisibleLifeCycleEvent.PopulateUI();
        }
    }
}
