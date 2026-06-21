<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ChamadoAnatel.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.ChamadoAnatel" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa e contrato"
        Width="700px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect=" SELECT DISTINCT s.setor, nome, ue.UNIDADE_ENS, ua_atual, ua_antiga FROM VW_ZZCRO_UNIDADE_ADMINSTRATIVA S inner join HADES..VW_SETOR se on S.SETOR = se.SETOR inner join LYCEUM.FiscalizacaoLink.CONTRATOSETOR CS ON CS.SETORID=S.SETOR left join LY_UNIDADE_ENSINO ue on S.SETOR = ue.SETOR "
                        SqlOrder="setor" ColumnName="setor" Caption="" MaxLength="6" DataType="Varchar"
                        OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />                           
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="UNIDADE_ENS" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="Contrato:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseContrato" runat="server" SqlSelect="select DISTINCT numero , descricao, C.contratoid  from [LYCEUM].[FiscalizacaoLink].[CONTRATO] C inner join [LYCEUM].[FiscalizacaoLink].[CONTRATOSETOR] CS ON C.CONTRATOID=CS.CONTRATOID "
                        SqlOrder="numero" ColumnName="numero" Key="numero" Argument="descricao" Caption=""
                        MaxLength="15" SqlWhere=" SETORID=#tseUnidadeAdministrativa# " DataType="VarChar"
                        OnChanged="tseContrato_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número" FieldName="numero" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="Circuito:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCircuito" runat="server" SqlSelect="select DISTINCT  CI.CIRCUITOSETORID AS CODIGO, (T.DESCRICAO + '/' + CONVERT(VARCHAR(10),V.VALOR)) AS DESCRICAO   from [LYCEUM].[FiscalizacaoLink].[CONTRATO] C inner join [LYCEUM].[FiscalizacaoLink].[CONTRATOSETOR] CS ON C.CONTRATOID=CS.CONTRATOID inner join [LYCEUM].[FiscalizacaoLink].CIRCUITOSETOR CI ON CI.CONTRATOSETORID = CS.CONTRATOSETORID
                        INNER JOIN [LYCEUM].[FiscalizacaoLink].VELOCIDADE V ON V.VELOCIDADEID = CI.VELOCIDADEID
                        INNER JOIN [LYCEUM].[FiscalizacaoLink].TECNOLOGIA T ON T.TECNOLOGIAID = CI.TECNOLOGIAID"
                        SqlOrder="CIRCUITOSETORID" ColumnName="CIRCUITOSETORID" Key="CIRCUITOSETORID"
                        Argument="descricao" Caption="" MaxLength="15" SqlWhere=" SETORID=#tseUnidadeAdministrativa# AND C.NUMERO = #tseContrato# "
                        DataType="Number" OnChanged="tseCircuito_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CIRCUITOSETORID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <br />
        <table align="right" >
            <tr>
                <td >
                    <asp:Button ID="btnLimpar" runat="server" Text="Limpar Campos" OnClick="btnLimpar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsChamadoAnatel" runat="server" TypeName="Techne.Lyceum.Net.Interconectividade.ChamadoAnatel"
        SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete" InsertMethod="Insert">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseCircuito" Name="circuitoSetor" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdChamadoAnatel" runat="server" DataSourceID="odsChamadoAnatel"
        Visible="false" KeyFieldName="CHAMADOANATELID" OnRowDeleting="grdChamadoAnatel_RowDeleting"
        AutoGenerateColumns="false" ClientInstanceName="grdChamadoAnatel" OnInitNewRow="grdChamadoAnatel_InitNewRow"
        OnInit="grdChamadoAnatel_Init" OnCellEditorInitialize="grdChamadoAnatel_CellEditorInitialize"
        OnStartRowEditing="grdChamadoAnatel_StartRowEditing" Width="850px" OnRowUpdating="grdChamadoAnatel_RowUpdating"
        OnRowInserting="grdChamadoAnatel_RowInserting">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsBehavior ConfirmDelete="False" />
       <%-- <ClientSideEvents EndCallback="grdChamadoAnatel_EndCallback" />--%>
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdChamadoAnatel.AddNewRow();" />
                    </div>
                </HeaderCaptionTemplate>
                <CancelButton Visible="true" Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <EditButton Visible="True" Text="Editar">
                    <Image Url="../img/bt_editar.png" />
                </EditButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <UpdateButton Visible="true" Text="Alterar">
                    <Image Url="../img/bt_salvar.png" />
                </UpdateButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CHAMADOANATELID"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="" Name="CIRCUITOSETORID" VisibleIndex="2"
                FieldName="CIRCUITOSETORID" Visible="false">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nº Chamado Operadora*" Name="numerooperadora"
                VisibleIndex="2" FieldName="NUMEROOPERADORA" Width="150px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Chamado Operadora*" FieldName="DATAOPERADORA"
                VisibleIndex="3" Width="130px">
                <EditItemTemplate>
                    <table style="width: 110px">
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtOperadora" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATAOPERADORA") %>'
                                    EnableDefaultAppearance="true" ClientInstanceName="dtOperadora" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje" >
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nº Chamado Anatel*" Name="Chamado" VisibleIndex="4"
                FieldName="NUMEROANATEL" Width="150px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Chamado Anatel*" FieldName="DATAANATEL"
                VisibleIndex="5" Width="130px">
                <EditItemTemplate>
                    <table style="width: 110px">
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtAnatel" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATAANATEL") %>'
                                    EnableDefaultAppearance="true" ClientInstanceName="dtAnatel" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Resolução" FieldName="DATARESOLUCAO"
                VisibleIndex="6" Width="110px">
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtResolucao" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATARESOLUCAO") %>'
                                    EnableDefaultAppearance="true" ClientInstanceName="dtResolucao" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataColumn Caption="Severidade*" Name="SEVERIDADE" VisibleIndex="7"
                FieldName="SEVERIDADE" CellStyle-Border-BorderStyle="None">
                <EditItemTemplate>
                    <dxe:ASPxComboBox ID="ddlSeveridade" runat="server" Value='<%# Bind("SEVERIDADE") %>'
                        ValueType="System.String" Width="200px">
                        <Items>
                            <dxe:ListEditItem Text="Baixa" Value="Baixa" />
                            <dxe:ListEditItem Text="Média" Value="Média" />
                            <dxe:ListEditItem Text="Alta" Value="Alta" />
                        </Items>
                    </dxe:ASPxComboBox>
                </EditItemTemplate>
            </dxwgv:GridViewDataColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
