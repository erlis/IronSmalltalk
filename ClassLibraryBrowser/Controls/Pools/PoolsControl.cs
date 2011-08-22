/*
 * **************************************************************************
 *
 * Copyright (c) The IronSmalltalk Project. 
 *
 * This source code is subject to terms and conditions of the 
 * license agreement found in the solution directory. 
 * See: $(SolutionDir)\License.htm ... in the root of this distribution.
 * By using this source code in any fashion, you are agreeing 
 * to be bound by the terms of the license agreement.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **************************************************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools
{
    public partial class PoolsControl : UserControl, IMainFormMenuConsumer
    {
        private ValueHolder<SystemImplementation> _system;

        [Browsable(false)]
        public ValueHolder<SystemImplementation> SystemImplementationHolder
        {
            get
            {
                return this._system;
            }
            set
            {
                if (this._system != null)
                    this._system.Changed -= new ValueChangedEventHandler<SystemImplementation>(this.SystemImplementationChanged);
                this._system = value;
                if (this._system != null)
                    this._system.Changed += new ValueChangedEventHandler<SystemImplementation>(this.SystemImplementationChanged);
                this.poolDefinitionControl.SystemImplementationHolder = value;
            }
        }

        public SystemImplementation SystemImplementation
        {
            get
            {
                if (this.SystemImplementationHolder == null)
                    return null;
                return this.SystemImplementationHolder.Value;
            }
        }

        private ValueHolder<Pool> _pool;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Pool> PoolHolder
        {
            get
            {
                return this._pool;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._pool != null)
                    this._pool.Changed -= new ValueChangedEventHandler<Pool>(this.PoolChanged);
                this._pool = value;
                this._pool.Changed += new ValueChangedEventHandler<Pool>(this.PoolChanged);
                this.poolDefinitionControl.PoolHolder = value;
                this.poolValuesControl.PoolHolder = value;
            }
        }

        public Pool Pool
        {
            get
            {
                if (this.PoolHolder == null)
                    return null;
                return this.PoolHolder.Value;
            }
        }

        private ValueHolder<PoolValue> _poolValue;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<PoolValue> PoolValueHolder
        {
            get
            {
                return this._poolValue;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._poolValue != null)
                    this._poolValue.Changed -= new ValueChangedEventHandler<PoolValue>(this.PoolValueChanged);
                this._poolValue = value;
                this._poolValue.Changed += new ValueChangedEventHandler<PoolValue>(this.PoolValueChanged);
                this.poolValuesControl.PoolValueHolder = value;
            }
        }

        public PoolValue PoolValue
        {
            get
            {
                if (this.PoolValueHolder == null)
                    return null;
                return this.PoolValueHolder.Value;
            }
        }

        public PoolsControl()
        {
            InitializeComponent();
            this.PoolHolder = new ValueHolder<Pool>();
            this.PoolValueHolder = new ValueHolder<PoolValue>();
        }

        private void SystemImplementationChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            this.PoolHolder.Value = null;
            if (this.PoolsMenu != null)
                this.PoolsMenu.Enabled = (this.SystemImplementation != null) & this.Visible;
            if (this.CreateNewPoolMenuItem != null)
                this.CreateNewPoolMenuItem.Enabled = (this.SystemImplementation != null);
        }

        private void PoolChanged(object sender, ValueChangedEventArgs<Pool> e)
        {
            this.PoolValueHolder.Value = null;
            if (this.SavePoolMenuItem != null)
                this.SavePoolMenuItem.Enabled = (this.Pool != null);
            if (this.DeletePoolMenuItem != null)
                this.DeletePoolMenuItem.Enabled = (this.Pool != null);
            if (this.CreateNewPoolValueMenuItem != null)
                this.CreateNewPoolValueMenuItem.Enabled = (this.Pool != null);
        }

        private void PoolValueChanged(object sender, ValueChangedEventArgs<PoolValue> e)
        {
            if (this.SavePoolValueMenuItem != null)
                this.SavePoolValueMenuItem.Enabled = (this.Pool != null);
            if (this.DeletePoolValueMenuItem != null)
                this.DeletePoolValueMenuItem.Enabled = (this.Pool != null);
        }

        #region Menues

        private ToolStripMenuItem PoolsMenu;
        private ToolStripMenuItem CreateNewPoolMenuItem;
        private ToolStripMenuItem SavePoolMenuItem;
        private ToolStripMenuItem DeletePoolMenuItem;
        private ToolStripMenuItem CreateNewPoolValueMenuItem;
        private ToolStripMenuItem SavePoolValueMenuItem;
        private ToolStripMenuItem DeletePoolValueMenuItem;

        void IMainFormMenuConsumer.AddMainMenuItems(MenuStrip mainMenu)
        {
            this.PoolsMenu = new ToolStripMenuItem();
            this.PoolsMenu.MergeIndex = 80;
            this.PoolsMenu.MergeAction = MergeAction.Insert;
            this.PoolsMenu.Name = "PoolsMenu";
            this.PoolsMenu.Text = "Pools";
            this.PoolsMenu.Enabled = false;
            mainMenu.Items.Add(this.PoolsMenu);

            this.CreateNewPoolMenuItem = new ToolStripMenuItem();
            this.CreateNewPoolMenuItem.Name = "CreateNewPoolMenuItem";
            this.CreateNewPoolMenuItem.Text = "Create New Pool";
            this.CreateNewPoolMenuItem.Enabled = false;
            this.CreateNewPoolMenuItem.MergeIndex = 10;
            this.CreateNewPoolMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewPoolMenuItem.Click += new EventHandler(this.CreateNewPoolMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.CreateNewPoolMenuItem);

            this.SavePoolMenuItem = new ToolStripMenuItem();
            this.SavePoolMenuItem.Name = "SavePoolMenuItem";
            this.SavePoolMenuItem.Text = "Save Pool";
            this.SavePoolMenuItem.Enabled = false;
            this.SavePoolMenuItem.MergeIndex = 20;
            this.SavePoolMenuItem.MergeAction = MergeAction.Insert;
            this.SavePoolMenuItem.Click += new EventHandler(this.SavePoolMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.SavePoolMenuItem);

            this.DeletePoolMenuItem = new ToolStripMenuItem();
            this.DeletePoolMenuItem.Name = "DeletePoolMenuItem";
            this.DeletePoolMenuItem.Text = "Delete Pool";
            this.DeletePoolMenuItem.Enabled = false;
            this.DeletePoolMenuItem.MergeIndex = 30;
            this.DeletePoolMenuItem.MergeAction = MergeAction.Insert;
            this.DeletePoolMenuItem.Click += new EventHandler(this.DeletePoolMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.DeletePoolMenuItem);

            ToolStripSeparator tsp = new ToolStripSeparator();
            tsp.Name = "ToolStripSeparatorPools";
            tsp.MergeIndex = 100;
            tsp.MergeAction = MergeAction.Insert;
            this.PoolsMenu.DropDownItems.Add(tsp);

            this.CreateNewPoolValueMenuItem = new ToolStripMenuItem();
            this.CreateNewPoolValueMenuItem.Name = "CreateNewPoolValueMenuItem";
            this.CreateNewPoolValueMenuItem.Text = "Create New Pool Value";
            this.CreateNewPoolValueMenuItem.Enabled = false;
            this.CreateNewPoolValueMenuItem.MergeIndex = 110;
            this.CreateNewPoolValueMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewPoolValueMenuItem.Click += new EventHandler(this.CreateNewPoolValueMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.CreateNewPoolValueMenuItem);

            this.SavePoolValueMenuItem = new ToolStripMenuItem();
            this.SavePoolValueMenuItem.Name = "SavePoolValueMenuItem";
            this.SavePoolValueMenuItem.Text = "Save Pool Value";
            this.SavePoolValueMenuItem.Enabled = false;
            this.SavePoolValueMenuItem.MergeIndex = 120;
            this.SavePoolValueMenuItem.MergeAction = MergeAction.Insert;
            this.SavePoolValueMenuItem.Click += new EventHandler(this.SavePoolValueMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.SavePoolValueMenuItem);

            this.DeletePoolValueMenuItem = new ToolStripMenuItem();
            this.DeletePoolValueMenuItem.Name = "DeletePoolValueMenuItem";
            this.DeletePoolValueMenuItem.Text = "Delete Pool Value";
            this.DeletePoolValueMenuItem.Enabled = false;
            this.DeletePoolValueMenuItem.MergeIndex = 130;
            this.DeletePoolValueMenuItem.MergeAction = MergeAction.Insert;
            this.DeletePoolValueMenuItem.Click += new EventHandler(this.DeletePoolValueMenuItem_Click);
            this.PoolsMenu.DropDownItems.Add(this.DeletePoolValueMenuItem);

        }

        private void CreateNewPoolMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemImplementation == null)
                return;
            Pool pool = new Pool(this.SystemImplementation);
            pool.Name = "";
            if (!this.PoolHolder.SetValue(pool))
                return;
            this.poolDefinitionControl.SetPrimaryFocus();
        }

        private void SavePoolMenuItem_Click(object sender, EventArgs e)
        {
            this.poolDefinitionControl.Save();
        }

        private void DeletePoolMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Pool == null)
                return;
            if (this.SystemImplementation == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete pool '{0}'?", this.Pool.Name),
                "Delete Pool", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.SystemImplementation.GlobalItems.Remove(this.Pool);
            this.SystemImplementationHolder.TriggerChanged(this.SystemImplementation, this.SystemImplementation);
            this.PoolHolder.Value = null;
        }

        private void CreateNewPoolValueMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Pool == null)
                return;
            if (this.SystemImplementation == null)
                return;
            PoolValue value = new PoolValue(this.Pool);
            value.Name = "";
            if (!this.PoolValueHolder.SetValue(value))
                return;
            this.poolValuesControl.SetPrimaryFocus();
        }

        private void SavePoolValueMenuItem_Click(object sender, EventArgs e)
        {
            this.poolValuesControl.Save();
        }

        public void DeletePoolValueMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Pool == null)
                return;
            if (this.PoolValue == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete pool value '{0}'?", this.PoolValue.Name),
                "Delete Pool Value", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.Pool.Values.Remove(this.PoolValue);
            this.PoolHolder.TriggerChanged(this.Pool, this.Pool);
            this.PoolValueHolder.Value = null;
        }

        public void VisibilityChanged(bool visible)
        {
            if (this.PoolsMenu != null)
                this.PoolsMenu.Enabled = visible && (this.SystemImplementation != null);
        }

        #endregion
    }
}
