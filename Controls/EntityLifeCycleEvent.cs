using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace LifecycleEventEditor.Controls
{
    public partial class EntityLifeCycleEvent : UserControl, ILifeCycleEvent
    {
        private Dictionary<long, LfCycState> m_idsToStates;
        private Dictionary<long, StringArray> m_transIdsToEvents;
        private Dictionary<long, StringArray> m_pendingChanges;

        public VDF.Vault.Currency.Connections.Connection Connection;

        // If we are doing work on a thread, then this uniquely identifies the current thread
        Guid m_currentThreadId = Guid.Empty;

        bool m_committingChagnes = false;

        public EntityLifeCycleEvent()
        {
            InitializeComponent();
        }

        #region WORKER_THREADS

        

        public void PopulateUIWorker(object o)
        {
            LfCycDef[] lcDefs = null;
            try
            {
                // get all the lifecycle information
               lcDefs = Connection.WebServiceManager.LifeCycleService.GetAllLifeCycleDefinitions();
            }
            catch (Exception ex)
            {
                // Show the error on the UI Thread
                this.BeginInvoke((MethodInvoker)delegate
                {
                    MessageBox.Show(ex.ToString());
                });
            }

            // Process the results on the UI Thread
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (lcDefs != null)
                {
                    foreach (LfCycDef lcDef in lcDefs)
                    {
                        m_lifecycleDefinitionComboBox.Items.Add(new LfCycDefListItem(lcDef));
                    }
                }
            });
            
        }

        private void UpdateStatesWorker(object o)
        {
            object[] inputs = o as object[];
            Guid threadId = (Guid)inputs[0];
            LfCycDefListItem lcDef = inputs[1] as LfCycDefListItem;
            LfCycState[] states = null;

            if (lcDef.LfCycDef != null)
                states = lcDef.LfCycDef.StateArray;

            try
            {
                if (states != null)
                    GetTransitionEvents(lcDef.LfCycDef);
            }
            catch (Exception ex)
            {
                // Show the error on the UI Thread
                this.BeginInvoke((MethodInvoker)delegate
                {
                    MessageBox.Show(ex.ToString());
                });
            }

            // Process the results on the UI Thread
            this.BeginInvoke((MethodInvoker)delegate
            {
                // only update the UI if we are the current thread
                if (threadId.Equals(m_currentThreadId))
                {
                    if (states != null)
                    {
                        foreach (LfCycState state in states)
                        {
                            m_lifecycleStatesListBox.Items.Add(new LfCycStateListItem(state));
                            m_idsToStates.Add(state.Id, state);
                        }
                    }

                    m_currentThreadId = Guid.Empty;
                }
            });
        }

        #endregion

        #region HANDLE
        private void m_lifecycleDefinitionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill out the states combo box
            m_lifecycleStatesListBox.Items.Clear();
            m_lifecycleTransitionsListBox.Items.Clear();
            m_jobTypesListBox.Items.Clear();
            m_idsToStates.Clear();

            m_currentThreadId = Guid.NewGuid();
            ThreadPool.QueueUserWorkItem(UpdateStatesWorker, 
                new object [] {m_currentThreadId, m_lifecycleDefinitionComboBox.SelectedItem});
        }

        private void m_lifecycleStatesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill out the transitions combo box
            m_lifecycleTransitionsListBox.Items.Clear();
            m_jobTypesListBox.Items.Clear();

            LfCycDefListItem lcDefItem = m_lifecycleDefinitionComboBox.SelectedItem as LfCycDefListItem;
            LfCycStateListItem stateItem = m_lifecycleStatesListBox.SelectedItem as LfCycStateListItem;

            LfCycDef lcDef = lcDefItem.LfCycDef;
            LfCycState state = stateItem.LfCycState;
            if (lcDef != null && state != null)
            {
                LfCycTrans[] transitions = lcDef.TransArray;
                if (transitions == null || transitions.Length == 0)
                    return;

                // pass 1: going from the selected state
                foreach (LfCycTrans transition in transitions)
                {
                    if (transition.FromId == state.Id)
                    {
                        LfCycTransListItem transItem = new LfCycTransListItem(transition);
                        transItem.DispName = state.DispName + " -> " + m_idsToStates[transition.ToId].DispName;
                        m_lifecycleTransitionsListBox.Items.Add(transItem);
                    }
                }

                // pass 2: going to the selected state
                foreach (LfCycTrans transition in transitions)
                {
                    if (transition.ToId == state.Id)
                    {
                        LfCycTransListItem transItem = new LfCycTransListItem(transition);
                        transItem.DispName = state.DispName + " <- " + m_idsToStates[transition.FromId].DispName;
                        m_lifecycleTransitionsListBox.Items.Add(transItem);
                    }
                }
            }
        }

        private void m_lifecycleTransitionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_jobTypesListBox.Items.Clear();

            LfCycTransListItem transition = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
            if (transition == null || transition.LfCycTrans == null)
                return;

            if (m_transIdsToEvents.ContainsKey(transition.LfCycTrans.Id))
            {
                StringArray jobArray = m_transIdsToEvents[transition.LfCycTrans.Id];

                if (jobArray != null && jobArray.Items != null)
                {
                    foreach (string job in jobArray.Items)
                    {
                        m_jobTypesListBox.Items.Add(job);
                    }
                }
            }
        }

        private void m_jobTypesListBox_DoubleClick(object sender, EventArgs e)
        {
            AddJobEvent();
        }

        private void m_jobTypesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                DeleteJobEvent();
        }

        #endregion

        #region Private methods
        private void GetTransitionEvents(LfCycDef lcDef)
        {
            LfCycTrans[] transitions = lcDef.TransArray;
            if (transitions == null || transitions.Length == 0)
                return;

            // check to see if we have the information in the cache already
            if (m_transIdsToEvents.ContainsKey(transitions[0].Id))
                return;

            List<long> transIds = new List<long>();
            foreach (LfCycTrans transition in transitions)
            {
                transIds.Add(transition.Id);
            }

            StringArray[] jobArrayArray = null;

            jobArrayArray = Connection.WebServiceManager.LifeCycleService.GetJobTypesByLifeCycleStateTransitionIds(
                transIds.ToArray());

            int i = 0;
            foreach (long id in transIds)
            {
                if (jobArrayArray != null)
                    m_transIdsToEvents.Add(id, jobArrayArray[i]);
                else
                    m_transIdsToEvents.Add(id, null);

                i++;
            }
        }

        private void UpdatePendingChanges()
        {
            LfCycTransListItem transitionItem = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
            if (transitionItem == null)
                return;

            LfCycTrans transition = transitionItem.LfCycTrans;
            if (m_pendingChanges.ContainsKey(transition.Id))
                m_pendingChanges.Remove(transition.Id);

            StringArray jobTypes = new StringArray();

            if (m_jobTypesListBox.Items.Count == 0)
                jobTypes.Items = null;
            else
            {
                jobTypes.Items = new string[m_jobTypesListBox.Items.Count];

                int i = 0;
                foreach (string job in m_jobTypesListBox.Items)
                {
                    jobTypes.Items[i] = job;
                    i++;
                }
            }

            m_pendingChanges.Add(transition.Id, jobTypes);

            if (m_transIdsToEvents.ContainsKey(transition.Id))
                m_transIdsToEvents[transition.Id] = jobTypes;
            else
                m_transIdsToEvents.Add(transition.Id, jobTypes);

        }

        #endregion

        #region ILifeCycleEvent implement

        public void PopulateUI()
        {
            if (m_idsToStates == null)
            {
                m_idsToStates = new Dictionary<long, LfCycState>();
                m_transIdsToEvents = new Dictionary<long, StringArray>();
                m_pendingChanges = new Dictionary<long, StringArray>();

                ThreadPool.QueueUserWorkItem(PopulateUIWorker);
            }
        }


        /// <summary>
        /// Commits the changes.  Do not call from the UI thread.
        /// </summary>
        public void CommitChanges()
        {
            if (m_pendingChanges.Count == 0 || m_committingChagnes)
                return;

            m_committingChagnes = true;

            try
            {
                Dictionary<long, StringArray>.Enumerator enumerator = m_pendingChanges.GetEnumerator();
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    Connection.WebServiceManager.LifeCycleService.UpdateLifeCycleStateTransitionJobTypes(
                        enumerator.Current.Key,
                        enumerator.Current.Value.Items);
                }

                m_pendingChanges.Clear();
            }
            catch (Exception ex)
            {
                // Show the error on the UI Thread
                this.BeginInvoke((MethodInvoker)delegate
                {
                    MessageBox.Show(ex.ToString());
                });
            }

            m_committingChagnes = false;
        }

        public void AddJobEvent()
        {
            if (m_committingChagnes)
            {
                MessageBox.Show(Properties.Resources.IDS_COMMIT_RUNNING);
                return;
            }

            LfCycTransListItem transition = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
            if (transition == null)
                return;

            TextBoxDialog textBoxDialog = new TextBoxDialog("Job Type:", "Add Job Type");
            DialogResult result = textBoxDialog.ShowDialog();
            if (result == DialogResult.OK && textBoxDialog.Value.Length > 0)
            {
                m_jobTypesListBox.Items.Add(textBoxDialog.Value);
                UpdatePendingChanges();
            }
        }

        public void DeleteJobEvent()
        {
            if (m_committingChagnes)
            {
                MessageBox.Show(Properties.Resources.IDS_COMMIT_RUNNING);
                return;
            }

            LfCycTransListItem transition = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
            if (transition == null || m_jobTypesListBox.SelectedItem == null)
                return;

            m_jobTypesListBox.Items.RemoveAt(m_jobTypesListBox.SelectedIndex);
            UpdatePendingChanges();
        }

        public bool HasPendingChanges
        {
            get { return (m_pendingChanges != null && m_pendingChanges.Count > 0); }
        }

        public bool IsRunning
        {
            get { return !m_currentThreadId.Equals(Guid.Empty); }
        }

        public bool CanAddJobEvent
        {
            get
            {
                LfCycTransListItem transition = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
                if (transition == null)
                    return false;
                else
                    return true;
            }
        }

        public bool CanDeleteJobEvent
        {
            get
            {
                LfCycTransListItem transition = m_lifecycleTransitionsListBox.SelectedItem as LfCycTransListItem;
                if (transition == null || m_jobTypesListBox.SelectedItem == null)
                    return false;
                else
                    return true;
            }
        }

        #endregion
    }
}
