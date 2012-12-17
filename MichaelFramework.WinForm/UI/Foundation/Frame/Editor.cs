using System;
using System.ComponentModel;
using System.Windows.Forms;
using MichaelFramework.Model;
using DevComponents.DotNetBar;
using System.Collections.Generic;
using System.Drawing;

namespace MichaelFramework.WinForm.UI.Foundation.Frame
{
    public partial class Editor : UserControl
    {
        # region  外部接口

        public virtual string ID { get; set; }

        public virtual bool SingleInstance { get { return false; } }

        public virtual bool UnSaved
        {
            get
            {
                return EditorTab.Text.EndsWith("*");
            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorTab EditorTab { get; set; }

        private object content = null;
        /// <summary>
        /// 要编辑的内容
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object Content
        {
            get { return content; }
            set
            {
                content = value;
            }
        }

        private string title = "未命名";
        public virtual string Title
        {
            get { return title; }
            set { EditorTab.Text = title = value; }
        }

        public virtual bool CanSave { get { return false; } }

        public virtual bool Save()
        {
            if (this.EditorTab != null)
                this.EditorTab.Text = Title;
            OnSaved();
            return true;
        }

        public virtual bool CanClose { get { return true; } }

        public virtual bool Close()
        {
            if (UnSaved)
            {
                DialogResult d = MessageBoxEx.Show("内容已更改，是否保存？", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (d == DialogResult.Cancel) return false;
                if (d == DialogResult.Yes)
                {
                    if (!Save()) return false;
                }
            }
            EditorTab.AttachedControl.Dispose();
            return true;
        }

        public event EventHandler ContentChanged;

        public event EventHandler<EditorSavingEventArgs> Saving;

        public event EventHandler Saved;

        # endregion

        # region 系统接口

        protected virtual void OnContentChanged()
        {
            if (ContentChanged != null)
                ContentChanged(this, EventArgs.Empty);
        }

        protected virtual void OnSaving(EditorSavingEventArgs e)
        {
            if (Saving != null) Saving(this, e);
        }

        protected virtual void OnSaved()
        {
            if (Saved != null) Saved(this, EventArgs.Empty);
        }

        # endregion

        # region 初始化

        public Editor()
        {
            InitializeComponent();
        }

        # endregion
    }

    public class EditorSavingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
