using System;
using System.Collections.Generic;
using DevComponents.DotNetBar.Validator;
using System.Windows.Forms;

namespace MichaelFramework.WinForm.UI.Foundation
{
    public enum HightlightFireAction
    {
        OnFocus,
        OnMouseEnter
    }

    public static class HighlightController
    {
        private static Dictionary<Control, Highlighter> dicContainerHighlighter = new Dictionary<Control, Highlighter>();
        private static Dictionary<Control, KeyValuePair<eHighlightColor,Control>> dicControlHighlighterColor =
            new Dictionary<Control, KeyValuePair<eHighlightColor,Control>>();
        public static void RegisterHightlightControls(Control containerControl,eHighlightColor color, HightlightFireAction action,params Control[] controls)
        {
            Highlighter highlighter = null;
            if (dicContainerHighlighter.ContainsKey(containerControl))
                highlighter = dicContainerHighlighter[containerControl];
            else
            {
                highlighter = new Highlighter();
                highlighter.ContainerControl = containerControl;
                dicContainerHighlighter.Add(containerControl, highlighter);
            }
            foreach (Control control in controls)
            {
                if (!dicControlHighlighterColor.ContainsKey(control))
                    dicControlHighlighterColor.Add(control, new KeyValuePair<eHighlightColor, Control>());
                dicControlHighlighterColor[control]=new KeyValuePair<eHighlightColor,Control>(color,containerControl);
                switch (action)
                {
                    case HightlightFireAction.OnFocus:
                        control.Enter -= new EventHandler(control_Highlight);
                        control.Leave -= new EventHandler(control_ClearHighlight);

                        control.Enter += new EventHandler(control_Highlight);
                        control.Leave += new EventHandler(control_ClearHighlight);
                        break;
                    case HightlightFireAction.OnMouseEnter:
                        control.MouseEnter -= new EventHandler(control_Highlight);
                        control.MouseLeave -= new EventHandler(control_ClearHighlight);

                        control.MouseEnter += new EventHandler(control_Highlight);
                        control.MouseLeave += new EventHandler(control_ClearHighlight);
                        break;
                }
            }
        }

        static void control_Highlight(object sender, EventArgs e)
        {
            dicContainerHighlighter[dicControlHighlighterColor[(Control)sender].Value].SetHighlightColor((Control)sender,
             dicControlHighlighterColor[(Control)sender].Key);
        }

        static void control_ClearHighlight(object sender, EventArgs e)
        {
            dicContainerHighlighter[dicControlHighlighterColor[(Control)sender].Value].SetHighlightColor((Control)sender, eHighlightColor.None);
        }

        public static void UnRegisterHightlightControl(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enter -= new EventHandler(control_Highlight);
                control.Leave -= new EventHandler(control_ClearHighlight);
                control.MouseEnter -= new EventHandler(control_Highlight);
                control.MouseLeave -= new EventHandler(control_ClearHighlight);
                dicControlHighlighterColor.Remove(control);
            }
        }
    }
}
