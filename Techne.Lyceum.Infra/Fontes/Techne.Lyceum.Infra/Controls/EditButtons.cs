using System;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal enum EditButtonType
    {
        Ok, 
        Cancel, 
        New, 
        Edit, 
        Delete, 
        Paging, 
        Return, 
        Undelete, 
        Other
    }

    internal class EditButtonClickEventArgs : EventArgs
    {
        public EditButtonClickEventArgs(EditButtons item, EditButtonType type)
        {
            this.Item = item;
            this.ButtonType = type;
        }

        public EditButtonType ButtonType { get; set; }

        public EditButtons Item { get; set; }
    }

    [Designer(typeof (Design.EditButtonsDesigner))]
    internal class EditButtons : WebControl, INamingContainer
    {
        // (em ordem alfabética)
        public const string DeleteButtonImageUrl_Def = "~/images/EditButtonsDelete.gif";

        public const string EditButtonImageUrl_Def = "~/images/EditButtonsEdit.gif";

        public const string NewButtonImageUrl_Def = "~/images/EditButtonsNew.gif";

        private const string AccessKeyCancelButton_Def = "C";

        private const string AccessKeyDeleteButton_Def = "A";

        private const string AccessKeyEditButton_Def = "E";

        private const string AccessKeyNewButton_Def = "N";

        private const string AccessKeyOkButton_Def = "S";

        private const string AccessKeyPagingButton_Def = "P";

        private const string AccessKeyReturnButton_Def = "V";

        private const string AccessKeyUndeleteButton_Def = "R";

        private const ControlMessageType AssociatedMethodsMessageType_Def = ControlMessageType.Icon;

        private const string CancelButtonImageUrlFocus_Def = "~/images/EditButtonsCancelFocus.gif";

        private const string CancelButtonImageUrl_Def = "~/images/EditButtonsCancel.gif";

        private const string CancelButtonToolTip_Def = "Cancela a operação corrente";

        private const string DeleteButtonImageUrlFocus_Def = "~/images/EditButtonsDeleteFocus.gif";

        private const string DeleteButtonToolTip_Def = "Remove o registro apresentado";

        private const string EditButtonImageUrlFocus_Def = "~/images/EditButtonsEditFocus.gif";

        private const string EditButtonToolTip_Def = "Edita o registro apresentado";

        private const bool EnableViewState_Def = false;

        private const string NewButtonImageUrlFocus_Def = "~/images/EditButtonsNewFocus.gif";

        private const string NewButtonToolTip_Def = "Adiciona novo registro";

        private const string OkButtonImageUrlFocus_Def = "~/images/EditButtonsOkFocus.gif";

        private const string OkButtonImageUrl_Def = "~/images/EditButtonsOk.gif";

        private const string OkButtonToolTip_Def = "Efetiva a operação corrente";

        private const string ReturnButtonImageUrlFocus_Def = "~/images/EditButtonsReturnFocus.gif";

        private const string ReturnButtonImageUrl_Def = "~/images/EditButtonsReturn.gif";

        private const string ReturnButtonToolTip_Def = "Retorna à página anterior";

        private const string UndeleteButtonImageUrlFocus_Def = "~/images/EditButtonsUndeleteFocus.gif";

        private const string UndeleteButtonImageUrl_Def = "~/images/EditButtonsUndelete.gif";

        private const string UndeleteButtonToolTip_Def = "Reinsere o registro apresentado";

        private ImageLink CancelButton;

        private ImageLink DeleteButton;

        private ImageLink EditButton;

        private ImageLink NewButton;

        private ImageLink OkButton;

        private ImageLink ReturnButton;

        private ImageLink UndeleteButton;

        private TLinkButton[] methodButtons;

        /// <summary>
        ///   Deve ser utilizado somente através da propriedade Mgr.
        /// </summary>
        private IContainerManager mgr;

        private ImageLink pagingButton;

        private string pvAccessKeyCancelButton = AccessKeyCancelButton_Def;

        private string pvAccessKeyDeleteButton = AccessKeyDeleteButton_Def;

        private string pvAccessKeyEditButton = AccessKeyEditButton_Def;

        private string pvAccessKeyNewButton = AccessKeyNewButton_Def;

        private string pvAccessKeyOkButton = AccessKeyOkButton_Def;

        private string pvAccessKeyPagingButton = AccessKeyPagingButton_Def;

        private string pvAccessKeyReturnButton = AccessKeyReturnButton_Def;

        private string pvAccessKeyUndeleteButton = AccessKeyUndeleteButton_Def;

        private string pvCancelButtonImageUrlFocus = CancelButtonImageUrlFocus_Def;

        private string pvDeleteButtonImageUrlFocus = DeleteButtonImageUrlFocus_Def;

        private string pvEditButtonImageUrlFocus = EditButtonImageUrlFocus_Def;

        private string pvNewButtonImageUrlFocus = NewButtonImageUrlFocus_Def;

        private string pvOkButtonImageUrlFocus = OkButtonImageUrlFocus_Def;

        private string pvPageOffButtonImageUrlFocus = GridButtons.SinglePageImageFocus_Def;

        private string pvPageOnButtonImageUrlFocus = GridButtons.MultiPageImageFocus_Def;

        private string pvReturnButtonImageUrlFocus = ReturnButtonImageUrlFocus_Def;

        private string pvUndeleteButtonImageUrlFocus = UndeleteButtonImageUrlFocus_Def;

        public EditButtons()
        {
            this.EnableReturnButton = true;
            this.EnableNewButton = true;
            this.EnableEditButton = true;
            this.EnableDeleteButton = true;
            this.EnableUndeleteButton = true;
            this.EnablePagingButton = true;

            this.CancelButtonImageUrl = CancelButtonImageUrl_Def;
            this.OkButtonImageUrl = OkButtonImageUrl_Def;
            this.ReturnButtonImageUrl = ReturnButtonImageUrl_Def;
            this.DeleteButtonImageUrl = DeleteButtonImageUrl_Def;
            this.NewButtonImageUrl = NewButtonImageUrl_Def;
            this.EditButtonImageUrl = EditButtonImageUrl_Def;
            this.UndeleteButtonImageUrl = UndeleteButtonImageUrl_Def;
            this.PageOnButtonImageUrl = GridButtons.MultiPageImage_Def;
            this.PageOffButtonImageUrl = GridButtons.SinglePageImage_Def;

            this.CancelButtonImageUrlFocus = CancelButtonImageUrlFocus_Def;
            this.OkButtonImageUrlFocus = OkButtonImageUrlFocus_Def;
            this.ReturnButtonImageUrlFocus = ReturnButtonImageUrlFocus_Def;
            this.DeleteButtonImageUrlFocus = DeleteButtonImageUrlFocus_Def;
            this.NewButtonImageUrlFocus = NewButtonImageUrlFocus_Def;
            this.EditButtonImageUrlFocus = EditButtonImageUrlFocus_Def;
            this.UndeleteButtonImageUrlFocus = UndeleteButtonImageUrlFocus_Def;
            this.PageOnButtonImageUrlFocus = GridButtons.MultiPageImageFocus_Def;
            this.PageOffButtonImageUrlFocus = GridButtons.SinglePageImageFocus_Def;

            this.CancelButtonToolTip = CancelButtonToolTip_Def;
            this.OkButtonToolTip = OkButtonToolTip_Def;
            this.ReturnButtonToolTip = ReturnButtonToolTip_Def;
            this.DeleteButtonToolTip = DeleteButtonToolTip_Def;
            this.NewButtonToolTip = NewButtonToolTip_Def;
            this.EditButtonToolTip = EditButtonToolTip_Def;
            this.UndeleteButtonToolTip = UndeleteButtonToolTip_Def;
            this.PageOnButtonToolTip = GridButtons.SinglePageToolTip_Def;
            this.PageOffButtonToolTip = GridButtons.MultiPageToolTip_Def;

            this.AssociatedMethodsMessageType = AssociatedMethodsMessageType_Def;
            this.EnableViewState = EnableViewState_Def;
            this.Manager = string.Empty;
        }

        internal event EditButtonClickHandler Click;

        [
            Category("Behavior"), 
            DefaultValue(EnableViewState_Def), 
        ]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }

            set
            {
                base.EnableViewState = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyCancelButton_Def), 
            Description("Tecla de atalho do botão \"Cancelar\" ."), 
        ]
        public string AccessKeyCancelButton
        {
            get
            {
                return this.pvAccessKeyCancelButton;
            }

            set
            {
                this.pvAccessKeyCancelButton = value == null ? string.Empty : value;
            }
        }

        [Description("Tecla de atalho do bot\x00e3o \"Apagar\" ."), DefaultValue("A"), Category("Techne")]
        public string AccessKeyDeleteButton
        {
            get
            {
                return this.pvAccessKeyDeleteButton;
            }

            set
            {
                this.pvAccessKeyDeleteButton = value == null ? string.Empty : value;
            }
        }

        [DefaultValue("E"), Description("Tecla de atalho do bot\x00e3o \"Editar\" ."), Category("Techne")]
        public string AccessKeyEditButton
        {
            get
            {
                return this.pvAccessKeyEditButton;
            }

            set
            {
                this.pvAccessKeyEditButton = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyNewButton_Def), 
            Description("Tecla de atalho do botão \"Novo\" ."), 
        ]
        public string AccessKeyNewButton
        {
            get
            {
                return this.pvAccessKeyNewButton;
            }

            set
            {
                this.pvAccessKeyNewButton = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyOkButton_Def), 
            Description("Tecla de atalho do botão \"OK\" ."), 
        ]
        public string AccessKeyOkButton
        {
            get
            {
                return this.pvAccessKeyOkButton;
            }

            set
            {
                this.pvAccessKeyOkButton = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyPagingButton_Def), 
            Description("Tecla de atalho do botão \"Paginar\" ."), 
        ]
        public string AccessKeyPagingButton
        {
            get
            {
                return this.pvAccessKeyPagingButton;
            }

            set
            {
                this.pvAccessKeyPagingButton = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyReturnButton_Def), 
            Description("Tecla de atalho do botão \"Voltar\" ."), 
        ]
        public string AccessKeyReturnButton
        {
            get
            {
                return this.pvAccessKeyReturnButton;
            }

            set
            {
                this.pvAccessKeyReturnButton = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(AccessKeyUndeleteButton_Def), 
            Description("Tecla de atalho do botão \"Reinserir\" ."), 
        ]
        public string AccessKeyUndeleteButton
        {
            get
            {
                return this.pvAccessKeyUndeleteButton;
            }

            set
            {
                this.pvAccessKeyUndeleteButton = value == null ? string.Empty : value;
            }
        }

        [Category("Techne"), DefaultValue(AssociatedMethodsMessageType_Def), Description("Modo de apresentação de mensagens de erro nos botões gerados para os AssociatedMethods do manager."),]
        public ControlMessageType AssociatedMethodsMessageType { get; set; }

        [
            Category("Image"), 
            DefaultValue(CancelButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string CancelButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["CancelButtonImageUrl"];
            }

            set
            {
                this.ViewState["CancelButtonImageUrl"] = value == null ? CancelButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(CancelButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string CancelButtonImageUrlFocus
        {
            get
            {
                return this.pvCancelButtonImageUrlFocus;
            }

            set
            {
                this.pvCancelButtonImageUrlFocus = value == null ? CancelButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(CancelButtonToolTip_Def), 
        ]
        public string CancelButtonToolTip
        {
            get
            {
                return (string)this.ViewState["CancelButtonToolTip"];
            }

            set
            {
                this.ViewState["CancelButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(DeleteButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string DeleteButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["DeleteButtonImageUrl"];
            }

            set
            {
                this.ViewState["DeleteButtonImageUrl"] = value == null ? DeleteButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(DeleteButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string DeleteButtonImageUrlFocus
        {
            get
            {
                return this.pvDeleteButtonImageUrlFocus;
            }

            set
            {
                this.pvDeleteButtonImageUrlFocus = value == null ? DeleteButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(DeleteButtonToolTip_Def), 
        ]
        public string DeleteButtonToolTip
        {
            get
            {
                return (string)this.ViewState["DeleteButtonToolTip"];
            }

            set
            {
                this.ViewState["DeleteButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(EditButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string EditButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["EditButtonImageUrl"];
            }

            set
            {
                this.ViewState["EditButtonImageUrl"] = value == null ? EditButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(EditButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string EditButtonImageUrlFocus
        {
            get
            {
                return this.pvEditButtonImageUrlFocus;
            }

            set
            {
                this.pvEditButtonImageUrlFocus = value == null ? EditButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(EditButtonToolTip_Def), 
        ]
        public string EditButtonToolTip
        {
            get
            {
                return (string)this.ViewState["EditButtonToolTip"];
            }

            set
            {
                this.ViewState["EditButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Apagar\" permitindo delete do registro corrente."), 
        ]
        public bool EnableDeleteButton
        {
            get
            {
                return (bool)this.ViewState["EnableDeleteButton"];
            }

            set
            {
                this.ViewState["EnableDeleteButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Editar\" permitindo updates no registro corrente."), 
        ]
        public bool EnableEditButton
        {
            get
            {
                return (bool)this.ViewState["EnableEditButton"];
            }

            set
            {
                this.ViewState["EnableEditButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Novo\" permitindo inserts na tabela à qual o manager está associado."), 
        ]
        public bool EnableNewButton
        {
            get
            {
                return (bool)this.ViewState["EnableNewButton"];
            }

            set
            {
                this.ViewState["EnableNewButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Paginar\" se o manager for TDataGrid."), 
        ]
        public bool EnablePagingButton
        {
            get
            {
                return (bool)this.ViewState["EnablePagingButton"];
            }

            set
            {
                this.ViewState["EnablePagingButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Voltar\" se o manager permitir (o atributo NavReturn do método de navegação é true)."), 
        ]
        public bool EnableReturnButton
        {
            get
            {
                return (bool)this.ViewState["EnableReturnButton"];
            }

            set
            {
                this.ViewState["EnableReturnButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
            Description("Mostra o botão \"Reinserir\" se a tabela à qual o manager está associado é historificada e o registro corrente foi removido."), 
        ]
        public bool EnableUndeleteButton
        {
            get
            {
                return (bool)this.ViewState["EnableUndeleteButton"];
            }

            set
            {
                this.ViewState["EnableUndeleteButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            TypeConverter(typeof (RecordManagerConverter))
        ]
        public string Manager
        {
            get
            {
                return (string)this.ViewState["Manager"];
            }

            set
            {
                this.ViewState["Manager"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(NewButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string NewButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["NewButtonImageUrl"];
            }

            set
            {
                this.ViewState["NewButtonImageUrl"] = value == null ? NewButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(NewButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string NewButtonImageUrlFocus
        {
            get
            {
                return this.pvNewButtonImageUrlFocus;
            }

            set
            {
                this.pvNewButtonImageUrlFocus = value == null ? NewButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(NewButtonToolTip_Def), 
        ]
        public string NewButtonToolTip
        {
            get
            {
                return (string)this.ViewState["NewButtonToolTip"];
            }

            set
            {
                this.ViewState["NewButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(OkButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string OkButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["OkButtonImageUrl"];
            }

            set
            {
                this.ViewState["OkButtonImageUrl"] = value == null ? OkButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(OkButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string OkButtonImageUrlFocus
        {
            get
            {
                return this.pvOkButtonImageUrlFocus;
            }

            set
            {
                this.pvOkButtonImageUrlFocus = value == null ? OkButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(OkButtonToolTip_Def), 
        ]
        public string OkButtonToolTip
        {
            get
            {
                return (string)this.ViewState["OkButtonToolTip"];
            }

            set
            {
                this.ViewState["OkButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(GridButtons.SinglePageImage_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string PageOffButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["PageOffButtonImageUrl"];
            }

            set
            {
                this.ViewState["PageOffButtonImageUrl"] = value == null ? GridButtons.SinglePageImage_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(GridButtons.SinglePageImageFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string PageOffButtonImageUrlFocus
        {
            get
            {
                return this.pvPageOffButtonImageUrlFocus;
            }

            set
            {
                this.pvPageOffButtonImageUrlFocus = value == null ? GridButtons.SinglePageImageFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(GridButtons.MultiPageToolTip_Def), 
        ]
        public string PageOffButtonToolTip
        {
            get
            {
                return (string)this.ViewState["PageOffButtonToolTip"];
            }

            set
            {
                this.ViewState["PageOffButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(GridButtons.MultiPageImage_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string PageOnButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["PageOnButtonImageUrl"];
            }

            set
            {
                this.ViewState["PageOnButtonImageUrl"] = value == null ? GridButtons.MultiPageImage_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(GridButtons.MultiPageImageFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string PageOnButtonImageUrlFocus
        {
            get
            {
                return this.pvPageOnButtonImageUrlFocus;
            }

            set
            {
                this.pvPageOnButtonImageUrlFocus = value == null ? GridButtons.MultiPageImageFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(GridButtons.SinglePageToolTip_Def), 
        ]
        public string PageOnButtonToolTip
        {
            get
            {
                return (string)this.ViewState["PageOnButtonToolTip"];
            }

            set
            {
                this.ViewState["PageOnButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(ReturnButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string ReturnButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["ReturnButtonImageUrl"];
            }

            set
            {
                this.ViewState["ReturnButtonImageUrl"] = value == null ? ReturnButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(ReturnButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string ReturnButtonImageUrlFocus
        {
            get
            {
                return this.pvReturnButtonImageUrlFocus;
            }

            set
            {
                this.pvReturnButtonImageUrlFocus = value == null ? ReturnButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(ReturnButtonToolTip_Def), 
        ]
        public string ReturnButtonToolTip
        {
            get
            {
                return (string)this.ViewState["ReturnButtonToolTip"];
            }

            set
            {
                this.ViewState["ReturnButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(UndeleteButtonImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string UndeleteButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["UndeleteButtonImageUrl"];
            }

            set
            {
                this.ViewState["UndeleteButtonImageUrl"] = value == null ? DeleteButtonImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Image"), 
            DefaultValue(UndeleteButtonImageUrlFocus_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string UndeleteButtonImageUrlFocus
        {
            get
            {
                return this.pvUndeleteButtonImageUrlFocus;
            }

            set
            {
                this.pvUndeleteButtonImageUrlFocus = value == null ? DeleteButtonImageUrlFocus_Def : value.Trim();
            }
        }

        [
            Category("Message"), 
            DefaultValue(UndeleteButtonToolTip_Def), 
        ]
        public string UndeleteButtonToolTip
        {
            get
            {
                return (string)this.ViewState["UndeleteButtonToolTip"];
            }

            set
            {
                this.ViewState["UndeleteButtonToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        private IContainerManager Mgr
        {
            get
            {
                if (this.mgr == null)
                {
                    Control control = null;

                    // if(Manager.Length == 0)
                    // throw new InvalidOperationException("A propriedade " + ID + ".Manager não foi setada.");
                    control = this.NamingContainer.FindControl(this.Manager);

                    // if(control == null)
                    // throw new Exception("Manager informado na propriedade " + UniqueID + ".Manager não encontrado: " + Manager + ".");
                    if (control != null && !(control is IContainerManager))
                    {
                        throw new Exception("O controle " + this.Manager + " informado na propriedade " + this.UniqueID + ".Manager não é do tipo " + typeof (IContainerManager).FullName + ".");
                    }

                    this.mgr = (IContainerManager)control;
                }

                return this.mgr;
            }
        }

        protected virtual void OnClick(EditButtonClickEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            // CancelButton
            this.CancelButton = new ImageLink();
            this.CancelButton.ID = "CancelButton";
            this.CancelButton.Click += this.ButtonClick;
            this.CancelButton.AccessKey = this.AccessKeyCancelButton;
            this.Controls.Add(this.CancelButton);

            // DeleteButton
            this.DeleteButton = new ImageLink();
            this.DeleteButton.ID = "DeleteButton";
            this.DeleteButton.Click += this.ButtonClick;
            this.DeleteButton.AccessKey = this.AccessKeyDeleteButton;
            this.Controls.Add(this.DeleteButton);

            // EditButton
            this.EditButton = new ImageLink();
            this.EditButton.ID = "Edit";
            this.EditButton.Click += this.ButtonClick;
            this.EditButton.AccessKey = this.AccessKeyEditButton;
            this.Controls.Add(this.EditButton);

            // NewButton
            this.NewButton = new ImageLink();
            this.NewButton.ID = "NewButton";
            this.NewButton.Click += this.ButtonClick;
            this.NewButton.AccessKey = this.AccessKeyNewButton;
            this.Controls.Add(this.NewButton);

            // OkButton
            this.OkButton = new ImageLink();
            this.OkButton.ID = "OkButton";
            this.OkButton.Click += this.ButtonClick;
            this.OkButton.AccessKey = this.AccessKeyOkButton;
            this.Controls.Add(this.OkButton);

            // pagingButton
            this.pagingButton = new ImageLink();
            this.pagingButton.ID = "paging";
            this.pagingButton.Click += this.ButtonClick;
            this.pagingButton.AccessKey = this.AccessKeyPagingButton;
            this.Controls.Add(this.pagingButton);

            // ReturnButton
            this.ReturnButton = new ImageLink();
            this.ReturnButton.ID = "ReturnButton";
            this.ReturnButton.AccessKey = this.AccessKeyReturnButton;
            this.Controls.Add(this.ReturnButton);

            // UndeleteButton
            this.UndeleteButton = new ImageLink();
            this.UndeleteButton.ID = "UndeleteButton";
            this.UndeleteButton.Click += this.ButtonClick;
            this.UndeleteButton.AccessKey = this.AccessKeyUndeleteButton;
            this.Controls.Add(this.UndeleteButton);

            if (this.Mgr != null)
            {
                var count = this.Mgr.AssociatedMethods.Count;
                this.methodButtons = new TLinkButton[count];
                for (var i = 0; i < count; i++)
                {
                    this.methodButtons[i] = new TLinkButton();
                    this.methodButtons[i].ID = "method_" + i;
                    this.methodButtons[i].Click += this.ExecuteMethod;
                    this.methodButtons[i].ControlMessageType = this.AssociatedMethodsMessageType;
                    this.Controls.Add(this.methodButtons[i]);
                }
            }
        }

        protected override void OnPreRender(EventArgs args)
        {
            HistoryLib.RegisterHistoryScript(this.Page);

            base.OnPreRender(args);

            var canCommit = ContainerManagerLib.CanCommit(this.Mgr) && this.Mgr.IsRoot;

            // CancelButton
            this.CancelButton.ImageUrl = this.CancelButtonImageUrl;
            this.CancelButton.FocusImageUrl = this.CancelButtonImageUrlFocus;
            this.CancelButton.ToolTip = this.CancelButtonToolTip;
            if (this.Mgr != null && this.Mgr.DataEntry && ContainerManagerLib.AllContainersInMode(this.Mgr, RecordManagerMode.New))
            {
                this.CancelButton.Url = this.Mgr.GetReturnUrl();
            }

            this.CancelButton.Visible = canCommit;

            // DeleteButton
            this.DeleteButton.ImageUrl = this.DeleteButtonImageUrl;
            this.DeleteButton.FocusImageUrl = this.DeleteButtonImageUrlFocus;
            this.DeleteButton.ToolTip = this.DeleteButtonToolTip;
            this.DeleteButton.Visible = this.EnableDeleteButton && ContainerManagerLib.CanDelete(this.Mgr) && this.Mgr.IsRoot;

            // EditButton

            
            var enableEdit = false;
            if (this.EnableEditButton)
            {
                if (ContainerManagerLib.CanEnterEdit(this.Mgr))
                {
                    enableEdit = true;
                }
                else if (ContainerManagerLib.CanEnterAddNew(this.Mgr))
                {
                    // Se pode entrar em modo AddNew e pelo menos um container
                    // pode entrar em modo Edit, habilita o botão Edit.
                    foreach (var childManager in TControl.GetChildManagers((Control)this.Mgr))
                    {
                        if (ContainerManagerLib.CanEnterEdit(childManager))
                        {
                            enableEdit = true;
                            break;
                        }
                    }
                }
            }

            

            this.EditButton.ImageUrl = this.EditButtonImageUrl;
            this.EditButton.FocusImageUrl = this.EditButtonImageUrlFocus;
            this.EditButton.ToolTip = this.EditButtonToolTip;
            this.EditButton.Visible = enableEdit && ((this.Mgr != null && this.Mgr.IsRoot) || (this.Mgr == null && this.DesignMode));

            // NewButton
            this.NewButton.ImageUrl = this.NewButtonImageUrl;
            this.NewButton.FocusImageUrl = this.NewButtonImageUrlFocus;
            this.NewButton.ToolTip = this.NewButtonToolTip;
            this.NewButton.Visible = this.EnableNewButton && ((this.Mgr != null && ContainerManagerLib.CanEnterAddNew(this.Mgr) && this.Mgr.IsRoot) || (this.Mgr == null && this.DesignMode));

            // OkButton
            if (this.Mgr != null)
            {
                this.OkButton.Attributes.Add("OnClick", "return TControl_OnSubmitValidate('" + ((Control)this.Mgr).ClientID + "');");
            }

            this.OkButton.ImageUrl = this.OkButtonImageUrl;
            this.OkButton.FocusImageUrl = this.OkButtonImageUrlFocus;
            this.OkButton.ToolTip = this.OkButtonToolTip;
            this.OkButton.Visible = canCommit && ((this.Mgr != null && this.Mgr.IsRoot) || (this.Mgr == null && this.DesignMode));

            // pagingButton
            if (this.Mgr is TDataGrid)
            {
                var grid = (TDataGrid)this.Mgr;
                this.pagingButton.ImageUrl = grid.AllowPaging ? this.PageOffButtonImageUrl : this.PageOnButtonImageUrl;
                this.pagingButton.FocusImageUrl = grid.AllowPaging ? this.PageOffButtonImageUrlFocus : this.PageOnButtonImageUrlFocus;
                this.pagingButton.ToolTip = grid.AllowPaging ? this.PageOffButtonToolTip : this.PageOnButtonToolTip;
                this.pagingButton.Visible = this.EnablePagingButton &&
                                            (grid.AllowPaging && grid.PageCount > 1 || !grid.AllowPaging && grid.RowCount >= grid.PageSize) &&
                                            ContainerManagerLib.AllContainersInMode(this.Mgr, RecordManagerMode.View);
            }
            else
            {
                this.pagingButton.Visible = false;
            }

            // ReturnButton
            var returnUrl = this.Mgr != null ? this.Mgr.GetReturnUrl() : string.Empty;
            this.ReturnButton.ToolTip = this.ReturnButtonToolTip;
            this.ReturnButton.ImageUrl = this.ReturnButtonImageUrl;
            this.ReturnButton.FocusImageUrl = this.ReturnButtonImageUrlFocus;
            this.ReturnButton.Url = returnUrl;
            this.ReturnButton.Visible = this.EnableReturnButton && ((this.Mgr != null &&
                                                                     ContainerManagerLib.AllContainersInMode(this.Mgr, RecordManagerMode.View) &&
                                                                     returnUrl != null && this.Mgr.IsRoot) ||
                                                                    (this.Mgr == null && this.DesignMode));

            // UndeleteButton
            this.UndeleteButton.ImageUrl = this.UndeleteButtonImageUrl;
            this.UndeleteButton.FocusImageUrl = this.UndeleteButtonImageUrlFocus;
            this.UndeleteButton.ToolTip = this.UndeleteButtonToolTip;
            this.UndeleteButton.Visible = this.EnableUndeleteButton && ((this.Mgr != null &&
                                                                         ContainerManagerLib.CanUndelete(this.Mgr) &&
// Não permite undeletes quando o manager não estiver em view
                                                                         ContainerManagerLib.AllContainersInMode(this.Mgr, RecordManagerMode.View)) ||
                                                                        (this.Mgr == null && this.DesignMode));

            if (this.Mgr != null)
            {
                var associatedMethods = this.Mgr.AssociatedMethods;
                for (var i = 0; i < associatedMethods.Count; i++)
                {
                    try
                    {
                        this.methodButtons[i].ImageUrl = associatedMethods[i].GetImageUrl();
                        this.methodButtons[i].Text = associatedMethods[i].GetControlText();
                        this.methodButtons[i].ToolTip = associatedMethods[i].GetToolTip();

// Faz o texto ficar alinhado com o texto dos botões antigos
                        this.methodButtons[i].Style["position"] = "relative";
                        this.methodButtons[i].Style["top"] = "-2px";
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("Existe um problema com o método '" + associatedMethods[i].ExecuteMethod + "' de " + ((Control)this.Mgr).ID + ".AssociatedMethods: " + exc.Message);
                    }
                }
            }
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            // Este override garante a ordem de renderização dos botões
            var space = new LiteralControl("&nbsp;&nbsp;");

            this.ReturnButton.RenderControl(writer);
            if (this.ReturnButton.Visible)
            {
                space.RenderControl(writer);
            }

            this.DeleteButton.RenderControl(writer);
            this.UndeleteButton.RenderControl(writer);
            this.NewButton.RenderControl(writer);
            this.EditButton.RenderControl(writer);
            if (this.DeleteButton.Visible || this.UndeleteButton.Visible || this.NewButton.Visible || this.EditButton.Visible)
            {
                space.RenderControl(writer);
            }

            this.pagingButton.RenderControl(writer);
            if (this.pagingButton.Visible)
            {
                space.RenderControl(writer);
            }

            this.CancelButton.RenderControl(writer);
            this.OkButton.RenderControl(writer);

            if (this.Mgr != null && ContainerManagerLib.AllContainersInMode(this.Mgr, RecordManagerMode.View) && this.Mgr.RowCount > 0)
            {
                // Renderiza os botões AssociatedMethods somente se o container estiver em View.
                var associatedMethods = this.Mgr.AssociatedMethods;
                if (associatedMethods.Count > 0)
                {
                    space.RenderControl(writer);
                    for (var i = 0; i < associatedMethods.Count; i++)
                    {
                        this.methodButtons[i].RenderControl(writer);
                    }
                }
            }
        }

        private void ButtonClick(object sender, EventArgs args)
        {
            var buttonType = EditButtonType.Other;

            if (sender == this.EditButton)
            {
                buttonType = EditButtonType.Edit;
            }
            else if (sender == this.NewButton)
            {
                buttonType = EditButtonType.New;
            }
            else if (sender == this.DeleteButton)
            {
                buttonType = EditButtonType.Delete;
            }
            else if (sender == this.OkButton)
            {
                buttonType = EditButtonType.Ok;
            }
            else if (sender == this.CancelButton)
            {
                buttonType = EditButtonType.Cancel;
            }
            else if (sender == this.UndeleteButton)
            {
                buttonType = EditButtonType.Undelete;
            }
            else if (sender == this.pagingButton)
            {
                buttonType = EditButtonType.Paging;
            }
            else if (sender == this.ReturnButton)
            {
                buttonType = EditButtonType.Return;
            }

            var e = new EditButtonClickEventArgs(this, buttonType);
            this.OnClick(e);

            if (this.Mgr != null)
            {
                if (sender == this.EditButton)
                {
                    this.Mgr.EnterEditMode();
                }
                else if (sender == this.NewButton)
                {
                    this.Mgr.EnterAddNewMode();
                }
                else if (sender == this.DeleteButton)
                {
                    this.Mgr.Delete();
                }
                else if (sender == this.OkButton)
                {
                    this.Mgr.Commit();
                }
                else if (sender == this.CancelButton)
                {
                    this.Mgr.Rollback();
                }
                else if (sender == this.UndeleteButton)
                {
                    this.Mgr.Undelete();
                }
                else if (sender == this.pagingButton)
                {
                    ((TDataGrid)this.Mgr).TogglePaging();
                }
            }
        }

        private void ExecuteMethod(object sender, EventArgs args)
        {
            var methodButton = (TLinkButton)sender;

            var index = -1;
            for (var i = 0; i < this.methodButtons.Length; i++)
            {
                if (this.methodButtons[i] == methodButton)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                throw new ApplicationException();
            }

            methodButton.MessageCssClass = TLinkMethod.SuccessMsgCssClass_Def;
            RetVal result;
            if (this.Mgr != null)
            {
                result = this.Mgr.ExecAssociatedMethod(index);
                methodButton.Msg = result.Message;
            }
            else
            {
                methodButton.Msg = string.Empty;
            }
        }

        internal delegate void EditButtonClickHandler(object sender, EditButtonClickEventArgs e);
    }
}

namespace Techne.Controls.Design
{
    internal class EditButtonsDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);
            var control = (EditButtons)this.Component;
            var empty = true;

            if (control.EnableReturnButton)
            {
                new Image { ImageUrl = TUtil.TranslateRelativeUrl(control.ReturnButtonImageUrl, control) }.RenderControl(writer);
                empty = false;
            }

            if (control.EnableDeleteButton)
            {
                new Image { ImageUrl = TUtil.TranslateRelativeUrl(control.DeleteButtonImageUrl, control) }.RenderControl(writer);
                empty = false;
            }

            if (control.EnableNewButton)
            {
                new Image { ImageUrl = TUtil.TranslateRelativeUrl(control.NewButtonImageUrl, control) }.RenderControl(writer);
                empty = false;
            }

            if (control.EnableEditButton)
            {
                new Image { ImageUrl = TUtil.TranslateRelativeUrl(control.EditButtonImageUrl, control) }.RenderControl(writer);
                empty = false;
            }

            if (!empty)
            {
                return writer.ToString();
            }

            return "[" + control.ID + "]";
        }
    }
}