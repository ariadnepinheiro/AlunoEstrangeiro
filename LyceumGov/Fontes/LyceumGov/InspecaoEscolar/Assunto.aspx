<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Assunto.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.Assunto" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsAssunto" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Assunto"
        InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete" SelectMethod="ListaAssunto">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlGrupo" Name="grupoID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAssuntoPai" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Assunto"
        SelectMethod="ListaAssuntoPai" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlGrupo" Name="GRUPOID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoAssunto" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Assunto"
        SelectMethod="ListaTipoAssunto"></asp:ObjectDataSource>
    <asp:Panel ID="pnlCampanha" runat="server" GroupingText="Caracteristicas da campanha">
        <table>
            <tr>
                <td align="right">
                    <asp:Label SkinID="lblObrigatorio" ID="lblAno" runat="server" Text="Ano:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ANO" DataValueField="ANO"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblSemestre" SkinID="lblObrigatorio" runat="server" Text="Semestre:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemestre" runat="server" AutoPostBack="true" DataTextField="SEMESTRE"
                        DataValueField="SEMESTRE" OnSelectedIndexChanged="ddlSemestre_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label SkinID="lblCampanha" ID="lblTituloCampanha" runat="server" Text="Campanha:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTituloCampanha" runat="server" DataTextField="TITULO" DataValueField="CAMPANHAID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlTituloCampanha_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label SkinID="lblGrupo" ID="lblGrupo" runat="server" Text="Grupo:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGrupo" runat="server" DataTextField="DESCRICAO" AutoPostBack="true"
                        DataValueField="GRUPOID" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <dxwgv:ASPxGridView ID="grdAssunto" runat="server" KeyFieldName="ASSUNTOID" AutoGenerateColumns="false"
        DataSourceID="odsAssunto" ClientInstanceName="grdAssunto" OnInitNewRow="grdAssunto_InitNewRow"
        OnHtmlDataCellPrepared="grdAssunto_HtmlDataCellPrepared" OnStartRowEditing="grdAssunto_StartRowEditing"
        OnRowInserting="grdAssunto_RowInserting" OnHtmlRowCreated="grdAssunto_HtmlRowCreated"
        OnAfterPerformCallback="grdAssunto_AfterPerformCallback" OnRowUpdating="grdAssunto_RowUpdating"
        OnCellEditorInitialize="grdAssunto_CellEditorInitialize" OnRowDeleting="grdAssunto_RowDeleting"
        Width="100%" Visible="false">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados.." />
        <SettingsPager PageSize="15" />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdAssunto.AddNewRow();" />
                    </div>
                </HeaderCaptionTemplate>
                <CancelButton Visible="true" Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <EditButton Visible="True" Text="Editar">
                    <Image Url="../img/bt_editar.png" />
                </EditButton>
                <DeleteButton Visible="True" Text="Remover">
                    <Image Url="../img/bt_exclui2.png" />
                </DeleteButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <UpdateButton Visible="true" Text="Alterar">
                    <Image Url="../img/bt_salvar.png" />
                </UpdateButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataColumn FieldName="ASSUNTOID" Visible="false" VisibleIndex="1">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataTextColumn Caption="Assunto*" Name="ASSUNTO" VisibleIndex="2"
                FieldName="ASSUNTO" Width="150px">
                <PropertiesTextEdit MaxLength="500">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ordem*" Name="ORDEM" VisibleIndex="3" FieldName="ORDEM"
                UnboundType="Integer" Width="10px">
                <PropertiesTextEdit MaxLength="3">
                    <ValidationSettings ErrorText="">
                        <RegularExpression ErrorText="A ordem só aceita valores numéricos e inteiros." ValidationExpression="\d+" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn FieldName="GRUPOID" Visible="false" VisibleIndex="4">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataTextColumn Caption="GRUPO*" Name="GRUPO" Visible="false" VisibleIndex="5"
                FieldName="GRUPO" Width="150px">
                <PropertiesTextEdit MaxLength="50">
                </PropertiesTextEdit>
                <%--<EditItemTemplate>
                 <asp:Label ID="labelgrupo" runat="server" Text='<%# Eval("GRUPO") %>' Enabled="false" ></asp:Label>  
                </EditItemTemplate>--%>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn Caption="Tipo Assunto*" FieldName="TIPOASSUNTOID" Visible="false" VisibleIndex="6">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="7" Caption="TIPOASSUNTO*" Name="ddlTIPOASSUNTO"
                FieldName="TIPOASSUNTOID">
                <PropertiesComboBox DataSourceID="odsTipoAssunto" TextField="TIPOASSUNTO" ValueField="TIPOASSUNTOID"
                    ValueType="System.String">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                            var valor = s.GetValue();
                            var grid = grdAssunto;
                            var restricao = grid.GetEditor('RESTRICAO');
                            if (restricao) {
                                if (valor == 4) {
                                    restricao.SetVisible(true);
                                } else {
                                    restricao.SetVisible(false);
                                }
                            }
                        }" />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Restrição*" VisibleIndex="7" FieldName="RESTRICAO"
                Name="ddlRESTRICAO">
                <PropertiesComboBox>
                    <Items>
                        <dxe:ListEditItem Text="Sem restrição" Value="0" />
                        <dxe:ListEditItem Text="Apenas números" Value="1" />
                        <dxe:ListEditItem Text="Data" Value="2" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Possui Ação de Direção" FieldName="ACAODEDIRECAO"
                Name="ACAODEDIRECAO" VisibleIndex="8" Width="75px">
                <%--  <EditItemTemplate>
                    <asp:CheckBox ID="chkacaodedirecao"   runat="server"   Checked='<%# this.VerificarCheck(Eval("ACAODEDIRECAO")) %>' />
                </EditItemTemplate>--%>
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <%--<dxwgv:GridViewDataComboBoxColumn VisibleIndex="9" Caption="Assunto Pai*" 
             Name="ddlPAI_ASSUNTO" FieldName="ASSUNTOPAIID" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsAssuntoPai" TextField="ASSUNTOPAIID" ValueField="ASSUNTOPAI" ValueType="System.String" >
                </PropertiesComboBox>
                
            </dxwgv:GridViewDataComboBoxColumn>--%>
            <dxwgv:GridViewDataTextColumn Caption="Assunto Pai*" Name="PAI_ASSUNTO" VisibleIndex="11"
                FieldName="ASSUNTOPAI" Width="150px">
                <PropertiesTextEdit MaxLength="50">
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <%#Eval("AssuntoPAI") == DBNull.Value ? "(VAZIO)" : Eval("AssuntoPAI")%></DataItemTemplate>
                <EditItemTemplate>
                    <dxe:ASPxComboBox ID="ddlIDPAIASSUNTO" ClientInstanceName="ddlIDPAIASSUNTO" runat="server"
                        DataSourceID="odsAssuntoPai" TextField="ASSUNTOPAI" ValueField="ASSUNTOPAIID"
                        CallbackPageSize="15" ValueType="System.String" Value='<%# Eval("ASSUNTOPAI") %>'>
                    </dxe:ASPxComboBox>
                </EditItemTemplate>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn Caption="IDPAI_ASSUNTO" FieldName="ASSUNTOPAIID" Visible="false"
                VisibleIndex="10">
            </dxwgv:GridViewDataColumn>
            <%--  
              a.ASSUNTOID,a.DESCRICAO ASSUNTO ,a.ORDEM, A.GRUPOID,
              g.DESCRICAO GRUPO,A.TIPOASSUNTOID,ta.DESCRICAO TIPOASSUNTO,
              a.ACAODEDIRECAO,z.DESCRICAO PAI_ASSUNTO,
              A.IDPAIASSUNTOID IDPAI_ASSUNTO
              --%>
            <%--<dxwgv:GridViewDataComboBoxColumn VisibleIndex="4" Caption="TIPOASSUNTO*" Name="ddlTIPOASSUNTO" FieldName="TIPOASSUNTOID" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsTipoAssunto" TextField="DESCRICAO" ValueField="TIPOASSUNTOID" ValueType="System.String">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>--%>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
