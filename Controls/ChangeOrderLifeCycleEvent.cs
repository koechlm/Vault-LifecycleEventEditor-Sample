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
    public partial class ChangeOrderLifeCycleEvent : UserControl, ILifeCycleEvent
    {
        private Dictionary<long, WorkflowState> m_idsToStates;
        private Dictionary<long, Activity> m_idsToActivities;
        private Dictionary<long, Dictionary<long, StringArray>> m_stateIdsToEvents;
        private Dictionary<long, Dictionary<long, StringArray>> m_pendingChanges;

        // If we are doing work on a thread, then this uniquely identifies the current thread
        Guid m_currentThreadId = Guid.Empty;

        bool m_committingChagnes = false;

        public VDF.Vault.Currency.Connections.Connection Connection;

        public ChangeOrderLifeCycleEvent()
        {
            InitializeComponent();
        }

        #region Workers

        public void UpdateActivitiesWorker(object o)
        {
            object[] inputs = o as object[];
            Guid threadId = (Guid)inputs[0];
            WorkflowStateListItem state = inputs[1] as WorkflowStateListItem;
            WorkflowActivityListItem activity = inputs[2] as WorkflowActivityListItem;

            long stateId = -1;
            long activityId = -1;

            try
            {
                if (state != null && activity != null &&
                    state.WorkflowState != null && activity.Activity != null)
                {
                    GetTransitionEvents(state.WorkflowState);

                    stateId = state.WorkflowState.Id;
                    activityId = activity.Activity.Id;
                }
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
                    if (m_stateIdsToEvents.ContainsKey(stateId) && m_stateIdsToEvents[stateId].ContainsKey(activityId))
                    {
                        StringArray jobArray = m_stateIdsToEvents[stateId][activityId];

                        if (jobArray != null && jobArray.Items != null)
                        {
                            foreach (string job in jobArray.Items)
                            {
                                m_jobTypesListBox.Items.Add(job);
                            }
                            
                        }
                    }
                }

                m_currentThreadId = Guid.Empty;
            });
   
        }

        public void UpdateStatesWorker(object o)
        {
            object[] inputs = o as object[];
            Guid threadId = (Guid)inputs[0];
            WorkflowListItem workflow = inputs[1] as WorkflowListItem;

            if (workflow.Workflow != null)
            {
                long workflowId = workflow.Workflow.Id;
                WorkflowInfo workflowInfo = Connection.WebServiceManager.ChangeOrderService.GetWorkflowInfo(workflowId);

                // Workflow States
                WorkflowState[] states = workflowInfo.WorkflowStateArray;
                Activity[] activities = null;
                if (states != null)
                {
                    activities = workflowInfo.ActivityArray;

                    if (activities != null)
                    {
                        foreach (Activity activity in activities)
                        {
                            m_idsToActivities.Add(activity.Id, activity);
                        }
                    }
                }

                // Process the results on the UI Thread
                this.BeginInvoke((MethodInvoker)delegate
                {
                    // only update the UI if we are the current thread
                    if (threadId.Equals(m_currentThreadId))
                    {
                        foreach (WorkflowState state in states)
                        {
                            m_workflowStatesListBox.Items.Add(new WorkflowStateListItem(state));
                            m_idsToStates.Add(state.Id, state);
                        }
                        m_currentThreadId = Guid.Empty;
                    }
                });
            }
        }
        #endregion

        #region HANDLE
        private void m_workflowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill out the states combo box
            m_workflowStatesListBox.Items.Clear();
            m_workflowActivitiesListBox.Items.Clear();
            m_jobTypesListBox.Items.Clear();
            m_idsToStates.Clear();

            m_currentThreadId = Guid.NewGuid();
            ThreadPool.QueueUserWorkItem(UpdateStatesWorker, new object [] 
                {m_currentThreadId, m_workflowComboBox.SelectedItem});
        }

        private void m_workflowStatesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill out the states combo box
            m_workflowActivitiesListBox.Items.Clear();
            m_jobTypesListBox.Items.Clear();

            WorkflowStateListItem state = m_workflowStatesListBox.SelectedItem as WorkflowStateListItem;
            if (state.WorkflowState != null)
            {
                long[] activityIds = state.WorkflowState.SuccessorActivityIdArray;

                if (activityIds == null)
                    return;

                foreach (long activityId in activityIds)
                {
                    Activity activity = m_idsToActivities[activityId];
                    WorkflowActivityListItem transItem = new WorkflowActivityListItem(activity);
                    transItem.DispName = m_idsToStates[state.WorkflowState.Id].DispName + " -> " + activity.DispName;
                    m_workflowActivitiesListBox.Items.Add(transItem);
                }
            }
        }

        private void m_workflowActivitiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_jobTypesListBox.Items.Clear();

            m_currentThreadId = Guid.NewGuid();
            ThreadPool.QueueUserWorkItem(UpdateActivitiesWorker,
                new object[] { m_currentThreadId, m_workflowStatesListBox.SelectedItem, m_workflowActivitiesListBox.SelectedItem });
        }

        

        private void m_jobTypesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                DeleteJobEvent();
        }

        private void m_jobTypesListBox_DoubleClick(object sender, EventArgs e)
        {
            AddJobEvent();
        }

        #endregion

        #region Private methods

        private void UpdatePendingChanges()
        {
            WorkflowStateListItem state = m_workflowStatesListBox.SelectedItem as WorkflowStateListItem;
            WorkflowActivityListItem activity = m_workflowActivitiesListBox.SelectedItem as WorkflowActivityListItem;
            if (state == null || activity == null ||
                state.WorkflowState == null || activity.Activity == null)
                return;

            long stateId = state.WorkflowState.Id;
            long activityId = activity.Activity.Id;
            if (m_pendingChanges.ContainsKey(stateId) && m_pendingChanges[stateId].ContainsKey(activityId))
                m_pendingChanges[stateId].Remove(activityId); 

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

            if (!m_pendingChanges.ContainsKey(stateId))
                m_pendingChanges.Add(stateId, new Dictionary<long, StringArray>());

            m_pendingChanges[stateId].Add(activityId, jobTypes);

            if (m_stateIdsToEvents.ContainsKey(stateId) && m_stateIdsToEvents[stateId].ContainsKey(activityId))
                m_stateIdsToEvents[stateId][activityId] = jobTypes;
            else
            {
                if (!m_stateIdsToEvents.ContainsKey(stateId))
                    m_stateIdsToEvents.Add(stateId, new Dictionary<long, StringArray>());

                m_stateIdsToEvents[stateId].Add(activityId, jobTypes);
            }
        }

        private void GetTransitionEvents(WorkflowState state)
        {
            long[] activityIds = state.SuccessorActivityIdArray;
            if (activityIds == null || activityIds.Length == 0)
                return;

            // check to see if we have the information in the cache already
            if (m_stateIdsToEvents.ContainsKey(state.Id))
                return;

            long[] stateIds = new long[activityIds.Length];
            for (int i = 0; i < activityIds.Length; i++)
                stateIds[i] = state.Id;

            StringArray[] jobArrayArray = null;
            jobArrayArray = Connection.WebServiceManager.ChangeOrderService.GetJobTypesByChangeOrderStateTransitionIds(stateIds, activityIds);

            if (jobArrayArray == null)
                return;

            for (int i = 0; i < activityIds.Length;i++ )
            {
                if (!m_stateIdsToEvents.ContainsKey(state.Id))
                    m_stateIdsToEvents.Add(state.Id, new Dictionary<long, StringArray>());
                m_stateIdsToEvents[state.Id].Add(activityIds[i], jobArrayArray[i]);
            }
        }

        #endregion

        #region ILifeCycleEvent method

        public void PopulateUI()
        {
            if (m_idsToStates == null)
            {
                m_idsToStates = new Dictionary<long, WorkflowState>();
                m_idsToActivities = new Dictionary<long, Activity>();
                m_stateIdsToEvents = new Dictionary<long, Dictionary<long, StringArray>>();
                m_pendingChanges = new Dictionary<long, Dictionary<long, StringArray>>();

                // get all the workflow information

                Workflow[] workflows = Connection.WebServiceManager.ChangeOrderService.GetAllActiveWorkflows();
                if (workflows != null)
                {
                    foreach (Workflow workflow in workflows)
                    {
                        this.m_workflowComboBox.Items.Add(new WorkflowListItem(workflow));
                    }
                }
            }
        }


        /// <summary>
        /// Commits the changes.  Do not call from the UI thread.
        /// </summary>
        public void CommitChanges()
        {
            if (m_pendingChanges.Count == 0)
                return;

            m_committingChagnes = true;
            try
            {
                long[] transIds = new long[m_pendingChanges.Count];
                StringArray[] jobs = new StringArray[m_pendingChanges.Count];
                foreach (long stateId in m_pendingChanges.Keys)
                {
                    foreach (long activityId in m_pendingChanges[stateId].Keys)
                    {
                        Connection.WebServiceManager.ChangeOrderService.UpdateChangeOrderStateTransitionJobTypes(
                            stateId, activityId, m_pendingChanges[stateId][activityId].Items);
                    }
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

            WorkflowActivityListItem activity = m_workflowActivitiesListBox.SelectedItem as WorkflowActivityListItem;

            if (activity == null)
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

            WorkflowActivityListItem activity = m_workflowActivitiesListBox.SelectedItem as WorkflowActivityListItem;

            if (activity == null ||
                m_jobTypesListBox.SelectedItem == null)
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
                WorkflowActivityListItem activity = m_workflowActivitiesListBox.SelectedItem as WorkflowActivityListItem;
                if (activity == null)
                    return false;
                else
                    return true;
            }
        }

        public bool CanDeleteJobEvent
        {
            get
            {
                WorkflowActivityListItem activity = m_workflowActivitiesListBox.SelectedItem as WorkflowActivityListItem;

                if (activity == null ||
                    m_jobTypesListBox.SelectedItem == null)
                    return false;
                else
                    return true;
            }
        }

        #endregion

    }
}
