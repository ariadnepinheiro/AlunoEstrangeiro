<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CancelarRegres.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.CancelarRegres" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
         Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="70px" AppendDataBoundItems="true" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoCancelamentoRegres"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    
    <asp:ObjectDataSource ID="odsCancelamento" runat="server" TypeName="Techne.Lyceum.Net.Matricula.CancelarRegres"
        SelectMethod="Lista" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdCancelamento" runat="server" DataSourceID="odsCancelamento"
        KeyFieldName="MATRICULAESPECIALDISCIPLINAID" AutoGenerateColumns="false" ClientInstanceName="grdCancelamento"
        OnRowDeleting="grdCancelamento_RowDeleting" Width="600px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma o cancelamento?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <DeleteButton Visible="True" Text="Remover">
                    <Image Url="../img/bt_exclui2.png" />
                </DeleteButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="MATRICULAESPECIALDISCIPLINAID"
                Visible="false" Width="700px">            
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" Name="Disciplina" VisibleIndex="2"
                FieldName="NOMEDISCIPLINA" Width="400px">                
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="NOMETURNO" VisibleIndex="3"
                Width="120px">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
