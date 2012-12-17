using System;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace MichaelFramework.WinForm.UI.Foundation.Frame
{
    public partial class EditorContainer : UserControl
    {
        public static EditorContainer Current { get; private set; }

        # region 外部接口：属性

        public event EventHandler<CurrentEditorChangingEventArgs> CurrentEditorChanging;

        public event EventHandler<CurrentEditorChangedEventArgs> CurrentEditorChanged;

        public event EventHandler<EditorClosingEventArgs> EditorClosing;

        public Editor CurrentEditor
        {
            get
            {
                EditorTab tab = this.tabControl1.SelectedTab as EditorTab;
                return tab == null ? null : tab.Editor;
            }
        }

        # endregion

        # region 外部接口：方法

        public Editor NavigateTo(Type editorType, object editContent, string Id, params object[] args)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                foreach (EditorTab tab in tabControl1.Tabs)
                {
                    if (tab.Editor.GetType() == editorType && tab.Editor.ID == Id)
                    {
                        tabControl1.SelectedTab = tab;
                        if (tabControl1.SelectedTab == tab)
                            return tab.Editor;
                        return null;
                    }
                }
            }
            else
            {
                foreach (EditorTab tab in tabControl1.Tabs)
                {
                    if (tab.Editor.GetType() == editorType &&
                        (tab.Editor.SingleInstance || tab.Editor.Content != null && tab.Editor.Content.Equals(editContent) ||
                        tab.Editor.Content == null && editContent == null))
                    {
                        tabControl1.SelectedTab = tab;
                        if (tabControl1.SelectedTab == tab)
                            return tab.Editor;
                        return null;
                    }
                }
            }
            Editor editor = addEditor(-1,editorType, editContent, Id, args);
            return editor;

        }

        public Editor NavigateTo(Type editorType, object editContent, params object[] args)
        {
            return NavigateTo(editorType, editContent, "", args);
        }

        public Editor NavigateTo(Type editorType, params object[] args)
        {
            return NavigateTo(editorType, default(object), args);
        }

        public Editor NavigateTo<T>(object editContent, params object[] args)
           where T : Editor
        {

            return NavigateTo(typeof(T), editContent, args);
        }

        public Editor NavigateTo<T>(params object[] args) where T : Editor
        {
            return NavigateTo(typeof(T), default(object), args);
        }

        public Editor ReplaceCurrentEditor<T>(object editContent,params object[]args) where T : Editor
        {
            int i = CurrentEditor != null ? tabControl1.Tabs.IndexOf(CurrentEditor.EditorTab) : 0;
            Editor editor = null;
            if (CurrentEditor == null || CurrentEditor.Close())
                editor = addEditor(i, typeof(T), editContent, null, args);
            return editor;
        }

        public bool Save()
        {
            EditorTab tab = tabControl1.SelectedTab as EditorTab;
            if (tab == null) return false;
            if (tab.Text.EndsWith("*") && !tab.Editor.CanSave || !tab.Editor.Save())
            {
                MessageBoxEx.Show(string.Format("页面“{0}”编辑不完善，无法保存！",
                    tab.Editor.Title), "警告", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public bool SaveAll()
        {
            foreach (EditorTab tab in this.tabControl1.Tabs)
            {
                if (!tab.Editor.CanSave || !tab.Editor.Save())
                    return false;
            }
            return true;
        }

        public bool CloseAll()
        {
            for (int i = this.tabControl1.Tabs.Count - 1; i >= 0;i-- )
            {
                if (!(this.tabControl1.Tabs[i] as EditorTab).Editor.Close())
                    return false;
            }
            return true;
        }

        public bool Close(object editContent)
        {
            foreach (EditorTab tab in tabControl1.Tabs)
            {
                if (tab.Editor.Content == editContent)
                {
                    tab.AttachedControl.Dispose();
                    tabControl1.Refresh();
                    return true;
                }
            }
            return false;
        }

        # endregion

        # region 初始化

        public EditorContainer()
        {
            InitializeComponent();
            Current = this;
        }

        # endregion

        # region 内部变量和方法

        private Editor addEditor(int insertIndex, Type editorType, object editContent, string id, params object[] args)
        {
            Editor editor = Activator.CreateInstance(editorType, args) as Editor;
            editor.ID = id;
            editor.Content = editContent;
            editor.Dock = DockStyle.Fill;
            if (this.tabControl1.Tabs.Count == 0)
            {
                CurrentEditorChangingEventArgs e = new CurrentEditorChangingEventArgs()
                {
                    OldEditor = null,
                    NewEditor = editor,
                    Cancel = false
                };
                OnCurrentEditorChanging(e);
                if (e.Cancel)
                    return null;
            }
            EditorTab newTab = new EditorTab(editor);
            newTab.MouseDown += new MouseEventHandler(Tab_MouseDown);
            if (insertIndex < 0)
                this.tabControl1.Tabs.Add(newTab);
            else
                this.tabControl1.Tabs.Insert(insertIndex, newTab);
            this.tabControl1.Controls.Add(newTab.AttachedControl);
            this.tabControl1.SelectedTab = newTab;
            OnCurrentEditorChanged(new CurrentEditorChangedEventArgs()
            {
                OldEditor = null,
                NewEditor = editor
            });
            return editor;
        }

        protected virtual void OnCurrentEditorChanging(CurrentEditorChangingEventArgs e)
        {
            if (CurrentEditorChanging != null)
                CurrentEditorChanging(this, e);
        }

        protected virtual void OnCurrentEditorChanged(CurrentEditorChangedEventArgs e)
        {
            if (CurrentEditorChanged != null)
                CurrentEditorChanged(this, e);
        }

        # endregion

        # region 右键菜单

        void Tab_MouseDown(object sender, MouseEventArgs e)
        {
            tabControl1.SelectedTab = sender as TabItem;
            if (e.Button == MouseButtons.Right)
                contextMenuBar1.SetContextMenuEx(tabControl1, menu1);
        }

        private void menu1_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            btnSaveTab.Enabled = tabControl1.SelectedTab != null &&
                (tabControl1.SelectedTab as EditorTab).Editor.CanSave;
            btnCloseTab.Visible = CurrentEditor.CanClose;
            btnCloseAllTabsExceptThis.Enabled = tabControl1.Tabs.Count > 1;
        }

        private void menu1_PopupClose(object sender, EventArgs e)
        {
            contextMenuBar1.SetContextMenuEx(tabControl1, null);
        }

        private void btnSaveTab_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab as EditorTab).Editor.Save();
        }

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            EditorClosingEventArgs ex = new EditorClosingEventArgs() { Editor = CurrentEditor };
            if (EditorClosing != null) EditorClosing(this, ex);
            if (!CurrentEditor.Close()) return;
            tabControl1.Refresh();
        }

        private void btnCloseAllTabsExceptThis_Click(object sender, EventArgs e)
        {
            bool b = false;
            for (int i = tabControl1.Tabs.Count - 1; i >= 0; i--)
            {
                EditorTab tab = tabControl1.Tabs[i] as EditorTab;
                if (tab != tabControl1.SelectedTab)
                {
                    if (tab.Text.EndsWith("*"))
                    {
                        b = true;
                        break;
                    }
                    //tabControl1.Tabs.Remove(tab);
                }
            }
            DialogResult d = DialogResult.None;
            if (b)
                d = MessageBoxEx.Show("有已修改的页面，是否保存？",
                    "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (d == DialogResult.Cancel) return;
            for (int i = tabControl1.Tabs.Count - 1; i >= 0; i--)
            {
                EditorTab tab = tabControl1.Tabs[i] as EditorTab;
                if (tab != tabControl1.SelectedTab)
                {
                    if (tab.Text.EndsWith("*") && d == DialogResult.Yes)
                    {
                        if (tab.Editor.CanSave && !tab.Editor.Save())
                        {
                            MessageBoxEx.Show("保存失败！", "失败",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                    EditorClosingEventArgs ex = new EditorClosingEventArgs() { Editor = tab.Editor};
                    if (EditorClosing != null) EditorClosing(this, ex);
                    if (ex.Cancel) return;
                    tab.AttachedControl.Dispose();

                }
            }
            tabControl1.Refresh();
        }

        # endregion

        # region 当前tab页改变事件处理

        private void tabControl1_SelectedTabChanging(object sender, TabStripTabChangingEventArgs e)
        {
            if (e.OldTab == null ||!tabControl1.Tabs.Contains(tabControl1.SelectedTab)) return;

            CurrentEditorChangingEventArgs ex = new CurrentEditorChangingEventArgs()
            {
                OldEditor = e.OldTab == null ? null : (e.OldTab as EditorTab).Editor,
                NewEditor = e.NewTab == null ? null : (e.NewTab as EditorTab).Editor,
                Cancel = false
            };
            OnCurrentEditorChanging(ex);
            e.Cancel = ex.Cancel;
        }

        private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            OnCurrentEditorChanged(new CurrentEditorChangedEventArgs()
            {
                OldEditor = e.OldTab == null ? null : (e.OldTab as EditorTab).Editor,
                NewEditor = e.NewTab == null ? null : (e.NewTab as EditorTab).Editor,
            });
        }

        # endregion
    }

    public class CurrentEditorChangedEventArgs : EventArgs
    {
        public Editor OldEditor { get; set; }
        public Editor NewEditor { get; set; }
    }

    public class CurrentEditorChangingEventArgs : EventArgs
    {
        public Editor OldEditor { get; set; }
        public Editor NewEditor { get; set; }

        public bool Cancel { get; set; }
    }

     public class EditorClosingEventArgs : EventArgs
    {
         public Editor Editor { get; set; }

        public bool Cancel { get; set; }
    }
}
