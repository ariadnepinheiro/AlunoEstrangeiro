using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class TLinkButton : TLinkButtonBase, IPostBackEventHandler
    {
        private const bool BlockMultipleSubmit_Def = true;

        private const string CommandArgument_Def = "";

        private const string CommandName_Def = "";

        private static readonly object ClickEvent = new object();

        private static readonly object CommandEvent = new object();

        public TLinkButton()
        {
            this.BlockMultipleSubmit = BlockMultipleSubmit_Def;
            this.CommandArgument = CommandArgument_Def;
            this.CommandName = CommandName_Def;
        }

        [
            Category("Action"), 
        ]
        public event EventHandler Click
        {
            add
            {
                this.Events.AddHandler(ClickEvent, value);
            }

            remove
            {
                this.Events.RemoveHandler(ClickEvent, value);
            }
        }

        [
            Category("Action"), 
        ]
        public event CommandEventHandler Command
        {
            add
            {
                this.Events.AddHandler(CommandEvent, value);
            }

            remove
            {
                this.Events.RemoveHandler(CommandEvent, value);
            }
        }

        [Description("Bloqueia m\x00faltiplos submits. S\x00f3 \x00e9 v\x00e1lido se a Url for vazia"), DefaultValue(true), Category("Behavior")]
        public bool BlockMultipleSubmit
        {
            get
            {
                return (bool)this.ViewState["BlockMultipleSubmit"];
            }

            set
            {
                this.ViewState["BlockMultipleSubmit"] = value;
            }
        }

        [Category("Behavior"), DefaultValue("")]
        public string CommandArgument
        {
            get
            {
                return (string)this.ViewState["CommandArgument"];
            }

            set
            {
                this.ViewState["CommandArgument"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Behavior"), 
            DefaultValue(CommandName_Def), 
        ]
        public string CommandName
        {
            get
            {
                return (string)this.ViewState["CommandName"];
            }

            set
            {
                this.ViewState["CommandName"] = value == null ? string.Empty : value;
            }
        }

        protected override void OnClick(EventArgs args)
        {
            var clickEventDelegate = (EventHandler)this.Events[ClickEvent];
            if (clickEventDelegate != null)
            {
                clickEventDelegate(this, args);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // InternalControl só será LinkButton quando Enabled for true,
            // por isso o tipo tem que ser verificado.
            if (!InDesignMode(this) && this.InternalControl is LinkButton)
            {
                var lnk = (LinkButton)this.InternalControl;
                lnk.CommandName = this.CommandName;
                lnk.CommandArgument = this.CommandArgument;
                if (!this.BlockMultipleSubmit)
                {
                    lnk.Attributes.Add("bypassCheck", "true");
                }
            }
        }

        protected override void RaisePostBackEvent(string eventArgument)
        {
            base.RaisePostBackEvent(eventArgument);
            this.OnCommand(new CommandEventArgs(this.CommandName, this.CommandArgument));
        }

        protected void OnCommand(CommandEventArgs args)
        {
            var commandEventDelegate = (CommandEventHandler)this.Events[CommandEvent];
            if (commandEventDelegate != null)
            {
                commandEventDelegate(this, args);
            }

            this.RaiseBubbleEvent(this, args);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }
    }
}