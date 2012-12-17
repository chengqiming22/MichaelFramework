using System;

namespace MichaelFramework.Config
{
    public partial class PaneStateTypeConverter
    {
        private global::MichaelFramework.Utils.PaneState ConvertFromStringToPaneState(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, string value)
        {
            MichaelFramework.Utils.PaneState ps = new MichaelFramework.Utils.PaneState();
            string[] s = value.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            ps.Visible = bool.Parse(s[1].Trim());
            ps.Expanded = bool.Parse(s[3].Trim());
            ps.WidthOrHeight = int.Parse(s[5].Trim());
            return ps;
        }

        private string ConvertToPaneStateFromString(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, global::MichaelFramework.Utils.PaneState value, global::System.Type type)
        {
            return string.Format("Visible={0},Expanded={1},WidthOrHeight={2}", value.Visible, value.Expanded, value.WidthOrHeight);
        }
    }
}
