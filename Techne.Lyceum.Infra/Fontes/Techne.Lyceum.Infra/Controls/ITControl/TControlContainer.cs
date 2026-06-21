namespace Techne.Controls
{
    /// <summary>
    ///   Summary description for TControlContainer.
    /// </summary>
    internal class TControlContainer : System.Web.UI.WebControls.Panel
    {
        public void Validate()
        {
            foreach (var c in TControl.GetChildTControls(this))
            {
                if (!(c is ITControlEditable))
                {
                    continue;
                }

                var msg = ((ITControlEditable)c).GetValueError();
                if (msg != null && msg.Trim() != string.Empty)
                {
                    c.Msg = msg;
                }
            }
        }
    }
}