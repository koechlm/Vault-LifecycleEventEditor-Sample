namespace LifecycleEventEditor.Controls
{
    partial class ChangeOrderLifeCycleEvent
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeOrderLifeCycleEvent));
            this.label4 = new System.Windows.Forms.Label();
            this.m_jobTypesListBox = new System.Windows.Forms.ListBox();
            this.m_workflowActivitiesListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_workflowStatesListBox = new System.Windows.Forms.ListBox();
            this.m_workflowComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_jobTypesListBox
            // 
            resources.ApplyResources(this.m_jobTypesListBox, "m_jobTypesListBox");
            this.m_jobTypesListBox.FormattingEnabled = true;
            this.m_jobTypesListBox.Name = "m_jobTypesListBox";
            this.m_jobTypesListBox.DoubleClick += new System.EventHandler(this.m_jobTypesListBox_DoubleClick);
            this.m_jobTypesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_jobTypesListBox_KeyDown);
            // 
            // m_workflowActivitiesListBox
            // 
            resources.ApplyResources(this.m_workflowActivitiesListBox, "m_workflowActivitiesListBox");
            this.m_workflowActivitiesListBox.FormattingEnabled = true;
            this.m_workflowActivitiesListBox.Name = "m_workflowActivitiesListBox";
            this.m_workflowActivitiesListBox.SelectedIndexChanged += new System.EventHandler(this.m_workflowActivitiesListBox_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_workflowStatesListBox
            // 
            resources.ApplyResources(this.m_workflowStatesListBox, "m_workflowStatesListBox");
            this.m_workflowStatesListBox.FormattingEnabled = true;
            this.m_workflowStatesListBox.Name = "m_workflowStatesListBox";
            this.m_workflowStatesListBox.SelectedIndexChanged += new System.EventHandler(this.m_workflowStatesListBox_SelectedIndexChanged);
            // 
            // m_workflowComboBox
            // 
            resources.ApplyResources(this.m_workflowComboBox, "m_workflowComboBox");
            this.m_workflowComboBox.FormattingEnabled = true;
            this.m_workflowComboBox.Name = "m_workflowComboBox";
            this.m_workflowComboBox.SelectedIndexChanged += new System.EventHandler(this.m_workflowComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ChangeOrderLifeCycleEvent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_jobTypesListBox);
            this.Controls.Add(this.m_workflowActivitiesListBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_workflowStatesListBox);
            this.Controls.Add(this.m_workflowComboBox);
            this.Controls.Add(this.label1);
            this.Name = "ChangeOrderLifeCycleEvent";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox m_jobTypesListBox;
        private System.Windows.Forms.ListBox m_workflowActivitiesListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox m_workflowStatesListBox;
        private System.Windows.Forms.ComboBox m_workflowComboBox;
        private System.Windows.Forms.Label label1;
    }
}
