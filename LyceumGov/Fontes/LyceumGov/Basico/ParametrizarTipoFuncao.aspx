<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ParametrizarTipoFuncao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ParametrizarTipoFuncao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnFuncaoChanged() {

            var descricao = cmbFuncao.GetText().toString();
            var hidden = document.getElementById("<%=hdnDescricaoFuncao.ClientID %>");
            hidden.value = descricao;
            txtDescricao.SetText(cmbFuncao.GetText().toString());
        }
       
    </script>

    <asp:HiddenField ID="hdnDescricaoFuncao" runat="server" />
    <dxwgv:ASPxGridView ID="grdTipoFuncao" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdTipoFuncao" KeyFieldName="FUNCAOID" DataSourceID="odsParametrizarTipoFuncao"
        OnRowValidating="grdTipoFuncao_RowValidating" Width="600px" OnRowDeleting="grdTipoFuncao_RowDeleting"
        OnRowInserting="grdTipoFuncao_RowInserting" OnStartRowEditing="grdTipoFuncao_StartRowEditing">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdTipoFuncao.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="1" Caption="Função*" Name="FUNCAOID"
                FieldName="FUNCAOID" Width="400" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsFuncao" TextField="DESCRICAO" ValueField="FUNCAO"
                    ValueType="System.String" ClientInstanceName="cmbFuncao">
                    <ClientSideEvents SelectedIndexChanged="function(s,e){ OnFuncaoChanged(); }" />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="DESCRICAO" FieldName="DESCRICAO" VisibleIndex="2"
                Width="150px" ReadOnly="true" Visible="false">
                <PropertiesTextEdit ClientInstanceName="txtDescricao">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="TipoFuncaoId" FieldName="TIPOFUNCAOID" VisibleIndex="2"
                Width="150px" ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="3" Caption="Tipo*" Name="TIPOFUNCAO"
                FieldName="TIPOFUNCAO" Width="200" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsTipoFuncao" TextField="TipoFuncao" ValueField="TipoFuncaoId"
                    ValueType="System.String">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsParametrizarTipoFuncao" TypeName="Techne.Lyceum.Net.Basico.ParametrizarTipoFuncao"
        runat="server" SelectMethod="Listar" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsFuncao" runat="server" TypeName="Techne.Lyceum.RN.Funcao"
        SelectMethod="Lista"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoFuncao" runat="server" TypeName="Techne.Lyceum.RN.TipoFuncao"
        SelectMethod="ListaTipoFuncao"></asp:ObjectDataSource>
</asp:Content>
