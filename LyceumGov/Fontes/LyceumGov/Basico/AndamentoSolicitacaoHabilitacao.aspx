<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AndamentoSolicitacaoHabilitacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AndamentoSolicitacaoHabilitacao"
    Title="Andamento da Solicitação de Habilitação" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="conSolicHabil" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdSolHabilitacao.cpMessage != null)
                lblMensagem.innerHTML = grdSolHabilitacao.cpMessage;
        }
        
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed" DataType="Number"
                        Key="id_regional" SqlSelect="select id_regional,regional from TCE_REGIONAL" SqlOrder="regional">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere=" id_regional = #tseRegional# " GridWidth="600px" ArgumentColumns="50"
                        Key="codigo" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
             <td style="text-align: right; width: 20%">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect="select distinct ue.setor, nomesetor, UE.id_regional, ue.municipio, ua_atual, ua_antiga from LY_UNIDADE_ENSINO ue inner join HADES..VW_SETOR s on ue.SETOR = s.SETOR inner join LY_UNIDADES_ASSOCIADAS uas on uas.UNIDADE_ENS = ue.UNIDADE_ENS inner join LY_USUARIO_UNIDADE_FIS uuf on uuf.UNIDADE_FIS = uas.UNIDADE_FIS"
                        ArgumentColumns="50" ColumnName="setor" Caption="" MaxLength="15" DataType="Varchar"
                        OnChanged="tseUnidadeAdministrativa_Changed" SqlWhere=" ue.id_regional = #tseRegional# AND ue.municipio = #tseMunicipio# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome." FieldName="nomesetor" Width="70%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="id_regional" FieldName="id_regional" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            
               
            <tr>
                <td style="text-align: right; width: 20%">
                    Status do Pedido:
                </td>
                <td>
                    <asp:DropDownList ID="cmbStatus" runat="server" OnSelectedIndexChanged="cmbStatus_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Selected="True" Value="Aguardando">Aguardando</asp:ListItem>
                        <asp:ListItem Value="Aprovado">Aprovado</asp:ListItem>
                        <asp:ListItem Value="Reprovado">Reprovado</asp:ListItem>
                        <asp:ListItem Value="">Todas</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdSolHabilitacao" runat="server" AutoGenerateColumns="False"
                        Visible="False" ClientInstanceName="grdSolHabilitacao" DataSourceID="odsSolicitacaoHabilitacao"
                        KeyFieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE" OnAfterPerformCallback="grdSolHabilitacao_AfterPerformCallback"
                        OnCustomJSProperties="grdSolHabilitacao_CustomJSProperties">
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE"
                                VisibleIndex="1" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADE_ENS" VisibleIndex="2"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                             <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="IDVINCULO" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Cargo" VisibleIndex="6" FieldName="CARGO">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Função" VisibleIndex="7" FieldName="FUNCAO">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Em aula?" VisibleIndex="8" FieldName="em_aula"
                                ReadOnly="true">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" VisibleIndex="9" FieldName="DISCIPLINA_INGRESSO">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="SEGMENTO_ATUACAO"
                                VisibleIndex="10">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Disciplina para habilitar" FieldName="DISCIPLINA_HABILITAR"
                                VisibleIndex="11">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Disciplina " FieldName="AGRUPAMENTO" Visible="false"
                                VisibleIndex="12">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Habilitar para:" FieldName="HABILITACAO_MATRICULA_GLP"
                                ReadOnly="true" VisibleIndex="13">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matricula para substituir" VisibleIndex="14"
                                FieldName="MATRICULA_SUBSTITUIDA">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tipo de Substituição" FieldName="TIPO_SUBSTITUICAO"
                                VisibleIndex="15">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUS" VisibleIndex="16">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Motivo" VisibleIndex="17" FieldName="MOTIVO">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DATA_CADASTRO" VisibleIndex="18">
                            </dxwgv:GridViewDataDateColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="odsSolicitacaoHabilitacao" TypeName="Techne.Lyceum.Net.Basico.AndamentoSolicitacaoHabilitacao"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="id_regional"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseMunicipio" DefaultValue="" Name="municipio" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseUnidadeAdministrativa" DefaultValue="" Name="setor"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="cmbStatus" DefaultValue="" Name="status" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
