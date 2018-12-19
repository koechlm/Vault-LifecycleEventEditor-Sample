namespace LifecycleEventEditor
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addJobToTransitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteJobFromTransitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitJobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_coTabPage = new System.Windows.Forms.TabPage();
            this.m_changeOrderLifeCycleEvent = new LifecycleEventEditor.Controls.ChangeOrderLifeCycleEvent();
            this.m_entityTabPage = new System.Windows.Forms.TabPage();
            this.m_entityLifeCycleEvent = new LifecycleEventEditor.Controls.EntityLifeCycleEvent();
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.m_coTabPage.SuspendLayout();
            this.m_entityTabPage.SuspendLayout();
            this.m_tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addJobToTransitionToolStripMenuItem,
            this.deleteJobFromTransitionToolStripMenuItem,
            this.commitJobsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            resources.ApplyResources(this.actionsToolStripMenuItem, "actionsToolStripMenuItem");
            this.actionsToolStripMenuItem.Click += new System.EventHandler(this.actionsToolStripMenuItem_Click);
            // 
            // addJobToTransitionToolStripMenuItem
            // 
            this.addJobToTransitionToolStripMenuItem.Name = "addJobToTransitionToolStripMenuItem";
            resources.ApplyResources(this.addJobToTransitionToolStripMenuItem, "addJobToTransitionToolStripMenuItem");
            this.addJobToTransitionToolStripMenuItem.Click += new System.EventHandler(this.addJobToTransitionToolStripMenuItem_Click);
            // 
            // deleteJobFromTransitionToolStripMenuItem
            // 
            this.deleteJobFromTransitionToolStripMenuItem.Name = "deleteJobFromTransitionToolStripMenuItem";
            resources.ApplyResources(this.deleteJobFromTransitionToolStripMenuItem, "deleteJobFromTransitionToolStripMenuItem");
            this.deleteJobFromTransitionToolStripMenuItem.Click += new System.EventHandler(this.deleteJobFromTransitionToolStripMenuItem_Click);
            // 
            // commitJobsToolStripMenuItem
            // 
            this.commitJobsToolStripMenuItem.Name = "commitJobsToolStripMenuItem";
            resources.ApplyResources(this.commitJobsToolStripMenuItem, "commitJobsToolStripMenuItem");
            this.commitJobsToolStripMenuItem.Click += new System.EventHandler(this.commitJobsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // m_coTabPage
            // 
            this.m_coTabPage.Controls.Add(this.m_changeOrderLifeCycleEvent);
            resources.ApplyResources(this.m_coTabPage, "m_coTabPage");
            this.m_coTabPage.Name = "m_coTabPage";
            // 
            // m_changeOrderLifeCycleEvent
            // 
            resources.ApplyResources(this.m_changeOrderLifeCycleEvent, "m_changeOrderLifeCycleEvent");
            this.m_changeOrderLifeCycleEvent.Name = "m_changeOrderLifeCycleEvent";
            // 
            // m_entityTabPage
            // 
            this.m_entityTabPage.Controls.Add(this.m_entityLifeCycleEvent);
            resources.ApplyResources(this.m_entityTabPage, "m_entityTabPage");
            this.m_entityTabPage.Name = "m_entityTabPage";
            // 
            // m_entityLifeCycleEvent
            // 
            resources.ApplyResources(this.m_entityLifeCycleEvent, "m_entityLifeCycleEvent");
            this.m_entityLifeCycleEvent.Name = "m_entityLifeCycleEvent";
            // 
            // m_tabControl
            // 
            resources.ApplyResources(this.m_tabControl, "m_tabControl");
            this.m_tabControl.Controls.Add(this.m_entityTabPage);
            this.m_tabControl.Controls.Add(this.m_coTabPage);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Tag = "File";
            this.m_tabControl.SelectedIndexChanged += new System.EventHandler(this.m_tabControl_SelectedIndexChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.m_coTabPage.ResumeLayout(false);
            this.m_entityTabPage.ResumeLayout(false);
            this.m_tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addJobToTransitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteJobFromTransitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commitJobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabPage m_coTabPage;
        private Controls.ChangeOrderLifeCycleEvent m_changeOrderLifeCycleEvent;
        private System.Windows.Forms.TabPage m_entityTabPage;
        private Controls.EntityLifeCycleEvent m_entityLifeCycleEvent;
        private System.Windows.Forms.TabControl m_tabControl;
    }
}

