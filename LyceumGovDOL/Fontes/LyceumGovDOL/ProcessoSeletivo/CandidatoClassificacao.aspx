<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CandidatoClassificacao.aspx.cs"
    Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoClassificacao" MasterPageFile="~/Modulos/ProcessoSeletivoMaster.Master" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="cCandidatoClassificacao" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" GroupingText="Faça uma busca por:" runat="server" Width="1000px"
        CssClass="Panel" Visible="true">
        <table border="0">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblProcessoSeletivo" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao FROM lY_concurso_docente cd "
                        SqlWhere="tipo = 'Contrato'" SqlOrder="concurso" ArgumentColumns="100" GridWidth="800">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblRegional" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" SqlSelect="SELECT DISTINCT  RE.ID_REGIONAL id,RE.REGIONAL  FROM LY_CONCURSO_DOCENTE CD INNER JOIN LY_CONCURSO_DOC_HABILITACAO CH ON CD.CONCURSO = CH.CONCURSO INNER JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = CH.REGIONALID "
                        SqlOrder="regional" Caption="" ArgumentColumns="100" Key="id_regional" Argument="regional"
                        GridWidth="600" SqlWhere="CD.CONCURSO = #tseConcursoBusca#" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ID_REGIONAL" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="REGIONAL" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblMunicipio" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlSelect="SELECT DISTINCT MN.CODIGO,MN.NOME, cd.REGIONALID FROM LY_CANDIDATO_DOCENTE CD INNER JOIN LY_CONCURSO_DOC_HABILITACAO CH ON CD.CONCURSO = CH.CONCURSO INNER JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = Cd.REGIONALID INNER JOIN HADES.DBO.MUNICIPIO MN ON MN.CODIGO =  Cd.MUNICIPIO_PROC "
                        ArgumentColumns="100" GridWidth="600" SqlOrder="NOME" OnChanged="tseMunicipio_Changed"
                        SqlWhere=" cd.REGIONALID= #tseRegional# AND CD.CONCURSO = #tseConcursoBusca#">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <dxe:ASPxButton ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnlResultadoConsulta" runat="server" GroupingText="Seleção" CssClass="Panel"
        Visible="true" Width="900px">
        <asp:ObjectDataSource ID="odsSelecao" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoClassificacao"
            runat="server" SelectMethod="Listar">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="tseRegional"
                    PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseConcursoBusca" Name="tseConcursoBusca" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseMunicipio" Name="tseMunicipio" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdSelecao" runat="server" AutoGenerateColumns="False" DataSourceID="odsSelecao"
            ClientInstanceName="grdSelecao" KeyFieldName="candidato" Width="900px">
            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome" VisibleIndex="1" Width="200px"
                    ReadOnly="true">
                    <PropertiesTextEdit MaxLength="100" Width="200px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data de Nascimento" FieldName="datanasc" VisibleIndex="2"
                    Width="90px" ReadOnly="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Pontuação" FieldName="pontuacao" VisibleIndex="3"
                    Width="100px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit MaxLength="10" Width="100px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data de Inscrição" FieldName="dtinscricao"
                    VisibleIndex="4" Width="90px" ReadOnly="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Situação" FieldName="STATUS" VisibleIndex="5"
                    Width="250">
                    <PropertiesComboBox>
                        <Items>
                            <dxe:ListEditItem Text="Aguardando" Value="1" />
                            <dxe:ListEditItem Text="Inscrito" Value="26" />
                            <dxe:ListEditItem Text="Convocado" Value="2" />
                            <dxe:ListEditItem Text="Admitido" Value="24" />
                            <dxe:ListEditItem Text="Desistente" Value="5" />
                            <dxe:ListEditItem Text="Faltoso" Value="21" />
                            <dxe:ListEditItem Text="Desclassificado" Value="22" />
                            <dxe:ListEditItem Text="Aguardando avaliação CGP" Value="23" />                            
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Disciplina de convocação" FieldName="disciplina"
                    VisibleIndex="6" Width="200px" ReadOnly="true">
                    <PropertiesTextEdit MaxLength="100" Width="200px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Regime de Cota " FieldName="cota" VisibleIndex="7"
                    Width="200px" ReadOnly="true">
                    <PropertiesTextEdit MaxLength="100" Width="200px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
