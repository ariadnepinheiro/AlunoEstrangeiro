<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SerieEntrada.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.SerieEntrada" 
    EnableEventValidation="false"%>

<asp:Content ID="ConSerieEntrada" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ScriptManagerProxy ID="manager" runat="server" />
    <asp:Panel ID="pnBuscaSerieEntrada" runat="server" GroupingText="Informe os dados: " Height="60px" Width="650px">
        <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Nivel:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseNivel" runat="server" Caption="" Key="tipo"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="descricao" 
                            SqlSelect="select TIPO, DESCRICAO from LY_TIPO_CURSO" 
                            GridWidth="650px" OnChanged="tseNivel_Changed" SqlOrder="descricao">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código Nível" FieldName="tipo" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="30%" />
                            </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseModalidade" runat="server" Caption="" Key="modalidade"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="descricao" 
                            SqlSelect="select MODALIDADE, DESCRICAO from LY_MODALIDADE_CURSO" 
                            GridWidth="650px" OnChanged="tseModalidade_Changed" SqlOrder="descricao">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código Modalidade" FieldName="modalidade" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="descricao" Width="30%" />
                            </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEscolaridadeTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseEscolaridade" runat="server" Caption="" Key="curso"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome" 
                            SqlSelect="Select CURSO, NOME From LY_CURSO" 
                            GridWidth="650px" OnChanged="tseEscolaridade_Changed" SqlOrder="nome" AutoPostBack="true">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código Escolaridade" FieldName="curso" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Escolaridade" FieldName="nome" Width="30%" />
                            </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSerie" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="DESCRICAO" DataValueField="SERIE"
                        AutoPostBack="true" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:UpdatePanel ID="uppSerieEntrada" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel ID="pnGridSerieEntrada" runat="server" Visible="true">
                <dxwgv:ASPxGridView ID="grdSerieEntrada" runat="server" AutoGenerateColumns="False"
                    Visible="true" ClientInstanceName="grdSerieEntrada" DataSourceID="odsSerieEntrada"
                    KeyFieldName="CURSO;SERIE" OnStartRowEditing="grdSerieEntrada_StartRowEditing"
                    OnAfterPerformCallback="grdSerieEntrada_AfterPerformCallback"
                    OnCellEditorInitialize="grdSerieEntrada_CellEditorInitialize"
                    OnRowUpdating="grdSerieEntrada_RowUpdating">
                    <SettingsBehavior ConfirmDelete="True" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                            <EditButton Text="Editar" Visible="true">
                                <Image Url="~/img/bt_editar.png" />
                            </EditButton>
                            <UpdateButton Text="Atualizar">
                                <Image Url="~/img/bt_salvar.png" />
                            </UpdateButton>
                            <CancelButton Text="Cancelar">
                                <Image Url="~/img/bt_cancelar.png" />
                            </CancelButton>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="CURSO" Caption="SerieEntrada" VisibleIndex="1"
                            Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="SERIE" Caption="Código" VisibleIndex="2"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Descrição" VisibleIndex="3"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataCheckColumn FieldName="ENTRADA" Caption="Entrada?" VisibleIndex="4"
                            Width="120px" Visible="true" ReadOnly="true">
                            <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                            </PropertiesCheckEdit>
                        </dxwgv:GridViewDataCheckColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <asp:ObjectDataSource ID="odsSerieEntrada" TypeName="Techne.Lyceum.Net.Academico.SerieEntrada"
                runat="server" SelectMethod="ListaSerieEntrada" OnUpdating="odsSerieEntrada_Updating" UpdateMethod="Update">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseEscolaridade" Name="CURSO" PropertyName="Value" />
                    <asp:ControlParameter ControlID="cmbSerie" Name="SERIE" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
