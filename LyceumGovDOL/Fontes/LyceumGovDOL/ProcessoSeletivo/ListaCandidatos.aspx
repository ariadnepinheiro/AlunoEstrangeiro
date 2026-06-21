<%@ Page Language="c#" CodeBehind="ListaCandidatos.aspx.cs" AutoEventWireup="True"
    MasterPageFile="~/Modulos/LyceumConexaoMaster.Master" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.ListaCandidatos" %>

<asp:Content ID="conListaCandidatos" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por:" Width="700px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcursoBusca" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Key="concurso" Argument="descricao"
                        MaxLength="20" SqlSelect="select cd.concurso as concurso, cd.descricao as descricao from LY_CONCURSO_DOCENTE cd"
                        ArgumentColumns="60" Columns="30" GridWidth="850px" OnChanged="tseConcursoBusca_Changed"
                        SqlWhere="cd.DT_INI_CONSULTA <= getdate() and cd.DT_FIM_CONSULTA >= getdate()">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCoordenadoriaBusca" runat="server" Text="Coordenadoria:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="right">
                    <tweb:TSearchBox ID="tseCoordenadoriaBusca" runat="server" Key="nucleo" Argument="descricao"
                        MaxLength="20" SqlSelect="select distinct LY_CONCURSO_DOC_HABILITACAO.NUCLEO as nucleo, descricao from LY_CONCURSO_DOC_HABILITACAO Join LY_NUCLEO On LY_CONCURSO_DOC_HABILITACAO.NUCLEO = LY_NUCLEO.NUCLEO"
                        GridWidth="850px" SqlWhere="LY_CONCURSO_DOC_HABILITACAO.CONCURSO = #tseConcursoBusca#"
                        ArgumentColumns="60" Columns="30">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblHabilitacaoBusca" runat="server" Text="Função:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cmbCargoBusca" runat="server" Value='<%# Bind("categoria") %>'
                        AutoPostBack="true" ValueType="System.String" Width="521px" OnValueChanged="cmbCargoBusca_Changed"
                        Enabled="false">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDisciplinaBusca" runat="server" Text="Disciplina de Ingresso:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseDisciplinaBusca" runat="server" Key="agrupamento" Argument="descricao"
                        MaxLength="50" SqlSelect="SELECT DISTINCT gh.AGRUPAMENTO as agrupamento, gh.DESCRICAO as descricao FROM LY_GRUPO_HABILITACAO gh inner join LY_CONCURSO_DOC_HABILITACAO ldh on gh.AGRUPAMENTO = ldh.AGRUPAMENTO"
                        ArgumentColumns="60" Columns="30" OnChanged="tseDisciplinaBusca_Changed" GridWidth="850px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="agrupamento" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNomeCandidatoBusca" runat="server" Text="Nome: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseNomeCandidatoBusca" runat="server" Key="candidato" Argument="nome"
                        SqlSelect="SELECT candidato, nome, nucleo FROM LY_CANDIDATO_DOCENTE" SqlWhere="concurso = #tseConcursoBusca# and nucleo = #tseCoordenadoriaBusca# and agrupamento_ingresso = #tseDisciplinaBusca#"
                        ArgumentColumns="60" Columns="30" MaxLength="20" SqlOrder="nome" GridWidth="850px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnListaCandidatos" runat="server" GroupingText="" Visible="false"
        Width="700px">
        <asp:ObjectDataSource ID="odsListaCandidatos" runat="server" TypeName="Techne.Lyceum.RN.ProcessoSeletivo"
            SelectMethod="ConsultarListaCandidatos">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="concurso" />
                <asp:ControlParameter ControlID="tseCoordenadoriaBusca" PropertyName="DBValue" Name="nucleo" />
                <asp:ControlParameter ControlID="cmbCargoBusca" PropertyName="SelectedItem.Value"
                    Name="categoria" />
                <asp:ControlParameter ControlID="tseDisciplinaBusca" PropertyName="DBValue" Name="agrupamento" />
                <asp:ControlParameter ControlID="tseNomeCandidatoBusca" PropertyName="DBValue" Name="candidato" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdListaCandidatos" runat="server" AutoGenerateColumns="False"
            EnableCallBacks="true" ClientInstanceName="grdListaCandidatos" Width="700px"
            DataSourceID="odsListaCandidatos" KeyFieldName="candidato" OnAfterPerformCallback="grdListaCandidatos_AfterPerformCallback"
            OnCustomColumnDisplayText="grdListaCandidatos_CustomColumnDisplayText" OnHtmlDataCellPrepared="grdListaCandidatos_HtmlDataCellPrepared">
            <SettingsEditing Mode="Inline" />
            <SettingsBehavior AllowMultiSelection="False" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Settings ShowFooter="true" />
            <Columns>
                <dxwgv:GridViewDataColumn Caption="Número de Inscrição" FieldName="candidato" VisibleIndex="1">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Nome" FieldName="nome" VisibleIndex="2">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Pontuação" FieldName="pontuacao" VisibleIndex="3"
                    CellStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Situação" FieldName="situacao" VisibleIndex="4">
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <div>
            <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right"
                Visible="true" />
        </div>
    </asp:Panel>
</asp:Content>
