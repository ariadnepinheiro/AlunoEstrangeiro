<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="OpcoesAssunto.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.OpcoesAssunto" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlCampanha" runat="server" GroupingText="Caracteristicas das Opções de Resposta">
        <table>
            <tr>
                <td align="left">
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
                <td align="left">
                    <asp:Label SkinID="lblObrigatorio" ID="lblTituloCampanha" runat="server" Text="Campanha:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTituloCampanha" runat="server" DataTextField="TITULO" DataValueField="CAMPANHAID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlTituloCampanha_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label SkinID="lblObrigatorio" ID="lblGrupo" runat="server" Text="Grupo:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGrupo" runat="server" DataTextField="DESCRICAO" AutoPostBack="true"
                        DataValueField="GRUPOID" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label SkinID="lblObrigatorio" ID="lblAssunto" runat="server" Text="Perguntas:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssunto" runat="server" DataTextField="ASSUNTO" AutoPostBack="true"
                        DataValueField="AssuntoID" OnSelectedIndexChanged="ddlAssunto_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label SkinID="lblMensagem" ID="lblobsResposta" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:ObjectDataSource ID="odsOPAssunto" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.OpcoesAssunto"
        InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete" SelectMethod="ListarAssuntoOpcoes">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAssunto" Name="AssuntoID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdOPAssunto" runat="server" KeyFieldName="OPCOESASSUNTOID"
        DataSourceID="odsOPAssunto" AutoGenerateColumns="false" ClientInstanceName="grdOPAssunto"
        OnRowInserting="grdOPAssunto_RowInserting" OnRowUpdating="grdOPAssunto_RowUpdating"
        OnRowDeleting="grdOPAssunto_RowDeleting" Width="100%" Visible="false">
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
                            onclick="grdOPAssunto.AddNewRow();" />
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
            <dxwgv:GridViewDataColumn FieldName="OPCOESASSUNTOID" Visible="false" VisibleIndex="1">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="ASSUNTO" VisibleIndex="2"
                FieldName="DESCRICAO" Width="20px">
                <PropertiesTextEdit MaxLength="500">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ordem*" Name="ORDEM" VisibleIndex="3" FieldName="ORDEM"
                UnboundType="Integer" Width="100px">
                <PropertiesTextEdit MaxLength="3">
                    <ValidationSettings ErrorText="">
                        <RegularExpression ErrorText="A ordem só aceita valores numéricos e inteiros." ValidationExpression="\d+" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Possui Ação de Direção" FieldName="ACAODEDIRECAO"
                Name="ACAODEDIRECAO" VisibleIndex="4" Width="75px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Restritivo" FieldName="RESTRITIVO" Name="RESTRITIVO"
                VisibleIndex="5" Width="75px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
