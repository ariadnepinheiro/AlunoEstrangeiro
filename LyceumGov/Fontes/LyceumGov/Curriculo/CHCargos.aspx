<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="CHCargos.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.CHCargos" %>

<asp:Content ID="cnCarteirinhas" ContentPlaceHolderID="cphFormulario" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
    <asp:PlaceHolder ID="plaConsulta" runat="server">
    
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Selecione o grupo" Width="800px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCargoTSearch" runat="server" Text="Grupo:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGrupo" runat="server" DataSourceID="odsGrupo" DataValueField="AGRUPAMENTOCARGOSID" DataTextField="DESCRICAO" AppendDataBoundItems="true" AutoPostBack="true">
                        <asp:ListItem Value="" />
                    </asp:DropDownList>                   
                 
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Button ID="btnNovo" runat="server" Text="Novo" style="position: relative; left: 700px; width: 100px;" OnClick="btnNovo_Click" />
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdChGlp" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdChGlp" DataSourceID="odsChGlp" KeyFieldName="CH_GLPID"
        Width="800px" OnRowInserted="grdChGlp_RowInserted" OnRowInserting="grdChGlp_RowInserting"
        OnRowDeleting="grdChGlp_RowDeleting"
        OnRowValidating="grdChGlp_RowValidating" OnInitNewRow="grdChGlp_InitNewRow"
        OnStartRowEditing="grdChGlp_StartRowEditing" OnAfterPerformCallback="grdChGlp_AfterPerformCallback"
        OnCellEditorInitialize="grdChGlp_CellEditorInitialize" Visible="true" SettingsEditing-Mode="Inline">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados" />
        <Settings ShowFilterRow="true" ShowFilterRowMenu="false" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="&nbsp;" Width="50px">
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                  <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataColumn FieldName="CH_GLPID" Caption="CH_GLPID" VisibleIndex="1" ReadOnly="True" Visible="false" Width="0px" />
            <dxwgv:GridViewDataColumn Caption="QTDE. MATRÍCULAS" FieldName="NR_MATRICULAS" VisibleIndex="2" Width="50px" />
            <dxwgv:GridViewDataComboBoxColumn Caption="GRUPO" FieldName="AGRUPAMENTOCARGOSID" VisibleIndex="3" Width="100px">
                <PropertiesComboBox ValueType="System.String" MaxLength="1" Width="70px" DropDownWidth="100px" DataSourceID="odsGrupo" ValueField="AGRUPAMENTOCARGOSID" TextField="DESCRICAO" />
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataColumn Caption="CH GRUPO" FieldName="CH_GRUPO" VisibleIndex="4" Width="50px" />
            <dxwgv:GridViewDataColumn Caption="FUNÇÃO DA MATRÍCULA" FieldName="FUNCAO_DESCRICAO" VisibleIndex="5" Width="100px">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="GRUPO (2)" FieldName="AGRUPAMENTOCARGOSID_2" VisibleIndex="6" Width="100px">
                <PropertiesComboBox ValueType="System.String" MaxLength="1" Width="70px" DropDownWidth="100px" DataSourceID="odsGrupo" ValueField="AGRUPAMENTOCARGOSID" TextField="DESCRICAO" />
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataColumn Caption="CH GRUPO (2)" FieldName="CH_GRUPO_2" VisibleIndex="7" Width="50px" />
            <dxwgv:GridViewDataColumn Caption="FUNÇÃO DA MATRÍCULA (2)" FieldName="FUNCAO_DESCRICAO_2" VisibleIndex="8" Width="100px">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="TOTAL DO GRUPO" FieldName="CH_TOTAL_GRUPO" VisibleIndex="9" Width="50px" />
            <dxwgv:GridViewDataColumn Caption="CH GLP" FieldName="CH_GLP" VisibleIndex="10" Width="50px" />
            <dxwgv:GridViewDataColumn Caption="CH FINAL COM A GLP" FieldName="CH_SEMANAL_TOTAL" VisibleIndex="11" Width="50px" />
        </Columns>
    </dxwgv:ASPxGridView>

    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="plaEdicao" runat="server" Visible="false">
    
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" style="position: relative; left: 600px; width: 100px;" OnClick="btnCancelar_Click" />
    <asp:Button ID="btnOk" runat="server" Text="OK" style="position: relative; left: 600px; width: 100px;" OnClick="btnOk_Click" />
    <br /><br />
    <asp:Panel ID="pnNrMatriculas" runat="server" GroupingText="Quantidade de Matrículas" Width="800px">
        <asp:RadioButtonList ID="rblNrMatriculas" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblNrMatriculas_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="1" Value="1" Selected="True"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
        </asp:RadioButtonList>
    </asp:Panel>
    <br /><br />
    <asp:Panel ID="pnMatricula1" runat="server" GroupingText="1ª Matrícula" Width="800px">
        Grupo: <asp:DropDownList ID="ddlGrupo1" runat="server" DataSourceID="odsGrupoAtivo" DataValueField="AGRUPAMENTOCARGOSID" DataTextField="DESCRICAO" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo1_SelectedIndexChanged">
            <asp:ListItem Value="" />
        </asp:DropDownList>
        C. H. do Grupo/Função: <asp:TextBox ID="txtChGrupo1" runat="server" Enabled="false"></asp:TextBox>
        <br /><br />
        Função: <tweb:TSearchBox ID="tseFuncao1" runat="server" Key="funcao" Argument="descricao" AutoPostBack="true" SqlOrder="descricao" SqlSelect="select distinct funcao, descricao from LY_FUNCAO" SqlWhere=" ATIVO = 'S' " >
            <gridcolumns>
                <tweb:TSearchBoxColumn Caption="Função" FieldName="funcao" Width="100%" />
                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="100%" />
            </gridcolumns>
        </tweb:TSearchBox>
    </asp:Panel>
    
    <asp:PlaceHolder ID="plaMatricula2" runat="server" Visible="false">
    
    <br /><br />
    <asp:Panel ID="pnMatricula2" runat="server" GroupingText="2ª Matrícula" Width="800px">
        Grupo: <asp:DropDownList ID="ddlGrupo2" runat="server" DataSourceID="odsGrupoAtivo" DataValueField="AGRUPAMENTOCARGOSID" DataTextField="DESCRICAO" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo2_SelectedIndexChanged">
            <asp:ListItem Value="" />
        </asp:DropDownList>
        C. H. do Grupo/Função: <asp:TextBox ID="txtChGrupo2" runat="server" Enabled="false"></asp:TextBox>
        <br /><br />
        Função: <tweb:TSearchBox ID="tseFuncao2" runat="server" Key="funcao" Argument="descricao" AutoPostBack="true" SqlOrder="descricao" SqlSelect="select distinct funcao, descricao from LY_FUNCAO" SqlWhere=" ATIVO = 'S' ">
            <gridcolumns>
                <tweb:TSearchBoxColumn Caption="Função" FieldName="funcao" Width="100%" />
                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="100%" />
            </gridcolumns>
        </tweb:TSearchBox>
    </asp:Panel>
    
    </asp:PlaceHolder>
    
    <br /><br />
    Total do Grupo + Função: <asp:TextBox ID="txtTotalGrupo" runat="server" Enabled="false" />
    + C. H. da GLP: <asp:TextBox ID="txtChGlp" runat="server" Enabled="false" />
    = C. H. final com GLP: <asp:TextBox ID="txtChSemanaTotal" runat="server" Enabled="false" />
    
    </asp:PlaceHolder>

    <% if (!string.IsNullOrEmpty(lblMensagem.Text)) { %>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <% } %>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:ObjectDataSource ID="odsChGlp" runat="server" TypeName="Techne.Lyceum.Net.Curriculo.CHCargos" SelectMethod="Lista" DeleteMethod="Remove">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlGrupo" PropertyName="SelectedValue" Name="AGRUPAMENTOCARGOSID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo" runat="server" SelectMethod="ListaGrupo" TypeName="Techne.Lyceum.Net.Curriculo.CHCargos"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupoAtivo" runat="server" SelectMethod="ListaGrupoAtivo" TypeName="Techne.Lyceum.Net.Curriculo.CHCargos"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsFuncao" runat="server" SelectMethod="ListaFuncao" TypeName="Techne.Lyceum.Net.Curriculo.CHCargos"></asp:ObjectDataSource>
    <asp:Button ID="Button1" runat="server" Text="Exportar" OnClick="Button1_Click_ExportarButton1_Click" />
</asp:Content>
