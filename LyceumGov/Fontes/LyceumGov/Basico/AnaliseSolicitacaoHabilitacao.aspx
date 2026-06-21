<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="AnaliseSolicitacaoHabilitacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AnaliseSolicitacaoHabilitacao" Title="Análise Solicitação de Habilitação" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="conAnaliseSolicitacao" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdAnaliseSolHabilitacao.cpMessage != null)
                lblMensagem.innerHTML = grdAnaliseSolHabilitacao.cpMessage;
        }
      

    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por coordenadoria e unidade administrativa"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblNucleo" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" SqlSelect="SELECT id_regional, regional FROM TCE_REGIONAL"
                        SqlOrder="regional" ColumnName="id_regional" Caption="" MaxLength="20" DataType="Number"
                        OnChanged="tseRegional_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="regional" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server"
                        ArgumentColumns="50" ColumnName="unidade_ens" Caption="" MaxLength="8" DataType="Varchar" 
                        SqlSelect="SELECT distinct ue.unidade_ens, ue.nome_comp, ue.setor, ue.cgc, ue.nucleo,ua_atual,ua_antiga , ue.ID_REGIONAL from LY_UNIDADE_ENSINO ue inner join HADES..VW_SETOR s on ue.SETOR = s.SETOR inner join LY_UNIDADES_ASSOCIADAS uas on uas.UNIDADE_ENS = ue.UNIDADE_ENS inner join LY_USUARIO_UNIDADE_FIS uuf on uuf.UNIDADE_FIS = uas.UNIDADE_FIS inner join dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE hd on hd.UNIDADE_ENS = ue.UNIDADE_ENS" 
                        OnChanged="tseUnidade_Ensino_Changed" GridWidth="850px" SqlOrder="nome_comp" SqlWhere="ue.id_regional = #tseRegional# and status='Aguardando'">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="40%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="UA_ATUAL" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />    
        <table>
		<tr>
<td><asp:Panel ID="pnGrid" runat="server">    
    <dxwgv:ASPxGridView ID="grdAnaliseSolHabilitacao" runat="server" AutoGenerateColumns="False"
        Visible="False" ClientInstanceName="grdAnaliseSolHabilitacao" DataSourceID="odsSolicitacaoHabilitacao"
        KeyFieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE" 
        OnCustomJSProperties="grdAnaliseSolHabilitacao_CustomJSProperties" >
<SettingsEditing Mode="Inline" />
        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />

        <Columns>
            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                <HeaderTemplate>
                    <input type="checkbox" onclick="grdAnaliseSolHabilitacao.SelectAllRowsOnPage(this.checked);" title="Select/Unselect all rows on the page" />
                </HeaderTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewCommandColumn>

            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE" ReadOnly="true" VisibleIndex="1" >
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Coordenadoria" FieldName="COORDENADORIA"  ReadOnly="true" Visible ="true" VisibleIndex="2">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="MUNICIPIO"  ReadOnly="true" Visible ="true" VisibleIndex="2">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADE_ENS" ReadOnly="true"
                VisibleIndex="3" Visible="false">
               <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA"  ReadOnly="true"
                VisibleIndex="4">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" ReadOnly="true"
                VisibleIndex="5">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="IDVINCULO" ReadOnly="true"
                VisibleIndex="5">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" ReadOnly="true"
                VisibleIndex="6">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cargo" VisibleIndex="7" FieldName ="CARGO" ReadOnly="true">
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" VisibleIndex="8" FieldName ="FUNCAO" ReadOnly="true">
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Em aula?" VisibleIndex="9" FieldName ="em_aula" ReadOnly="true">
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" VisibleIndex="10" FieldName ="DISCIPLINA_INGRESSO" Visible ="false" >
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="SEGMENTO_ATUACAO" Visible ="false"
                VisibleIndex="11">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina para habilitar" FieldName="DISCIPLINA_HABILITAR" ReadOnly="true"
                VisibleIndex="12">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina " FieldName="AGRUPAMENTO" visible ="false"
                VisibleIndex="13">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Habilitar para:" FieldName="HABILITACAO_MATRICULA_GLP" ReadOnly="true"
                VisibleIndex="14">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>

            <dxwgv:GridViewDataHyperLinkColumn FieldName="MATRICULA_SUBSTITUIDA" PropertiesHyperLinkEdit-ClientInstanceName="DocumentHyperLink" 
            PropertiesHyperLinkEdit-EnableClientSideAPI="True" Caption="Matricula para substituir" VisibleIndex="15">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="../Relatorio/PageViewer.aspx?report=chdocenteSolHab&grp=QHI&matricula={0}"
                        Target="_blank" TextField="MATRICULA_SUBSTITUIDA">
                    </PropertiesHyperLinkEdit>
                    <Settings SortMode="DisplayText" />
            </dxwgv:GridViewDataHyperLinkColumn>
            
            <dxwgv:GridViewDataTextColumn Caption="Tipo de Substituição" ReadOnly="true"
                FieldName="TIPO_SUBSTITUICAO" VisibleIndex="16">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUS" ReadOnly="true"
                VisibleIndex="17">
                  <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data do Pedido" FieldName="DATA_CADASTRO" VisibleIndex="18" ReadOnly="true">
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data de Análise" FieldName="DATA_ANALISE" VisibleIndex="19" ReadOnly="true">
              <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
        <dxwgv:GridViewDataColumn Caption="Motivo" FieldName="MOTIVO" VisibleIndex="20">
            <DataItemTemplate>
                <asp:DropDownList ID="cmbMotivoReprovacao" runat="server" DataSourceID="odsMotivoReprovacao"
                    DataTextField="DESCR" DataValueField="ITEM" Width="200px"  >
                    
                </asp:DropDownList>
            </DataItemTemplate>
        </dxwgv:GridViewDataColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:Button ID="btnAprovar" runat="server" Text="Aprovar" 
        onclick="btnAprovar_Click" />
    <asp:Button ID="btnReprovar" runat="server" Text="Reprovar" 
        onclick="btnReprovar_Click" />
    </asp:Panel></td>
            </tr>
        </table>
   <techne:TTableDataSource ID="tdsMotivoReprovacao" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
       SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'MotivoReprovSolHab'" SqlOrder="DESCR">
    </techne:TTableDataSource>
    
     <asp:ObjectDataSource ID="odsMotivoReprovacao" TypeName="Techne.Lyceum.Net.Basico.AnaliseSolicitacaoHabilitacao"
        runat="server" SelectMethod="ListaMotivoReprovacao" >
        </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="odsSolicitacaoHabilitacao" TypeName="Techne.Lyceum.Net.Basico.AnaliseSolicitacaoHabilitacao"
        runat="server" SelectMethod="ListarPor" >
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
             <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="id_regional"
                PropertyName="DBValue" />
        
        </SelectParameters>
    </asp:ObjectDataSource>    
    <br />   
    

</asp:Content>
