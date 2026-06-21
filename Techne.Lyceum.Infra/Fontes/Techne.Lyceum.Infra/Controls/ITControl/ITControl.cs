using System.Web.UI.WebControls;

namespace Techne.Controls
{
    public interface ITControl
    {
        string Caption { get; set; }

        string ColumnName { get; set; }

        ControlMessageType ControlMessageType { get; set; }

        DbObject DBValue { get; set; }

        DbType DataType { get; set; }

        string Format { get; set; }

        IContainerManager Manager { get; }

        string MessageImageUrl { get; set; }

        string Msg { get; set; }

        string ReadOnlyCssClass { get; set; }

        void CopyProperties(WebControl target);

        string GetCaption(string idioma);

        void ResetValue();
    }

    public interface ITControlEditable : ITControl
    {
        event ChangedEventHandler Changed;

        bool FollowContainerMode { get; set; }

        bool KeepValueAfterSave { get; set; }

        string MaximumValue { get; set; }

        string MinimumValue { get; set; }

        ControlMode Mode { get; set; }

        bool ReadOnly { get; set; }

        string GetValueError();
    }
}