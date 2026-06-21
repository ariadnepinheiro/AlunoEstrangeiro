<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AnaliseLiberacaoLancamento.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AnaliseLiberacaoLancamento"
    Title="Liberação de Lançamento de Notas/Faltas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="conAnaliseLiberacaoLancamento" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdAnaliseSolHabilitacao.cpMessage != null)
                lblMensagem.innerHTML = grdAnaliseSolHabilitacao.cpMessage;
        }
      

    </script>

    <asp:ObjectDataSource ID="odsAno" runat="server" TypeName="Techne.Lyceum.RN.PeriodoLetivo"
        SelectMethod="ListarAnos"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsPeriodo" runat="server" TypeName="Techne.Lyceum.RN.PeriodoLetivo"
        SelectMethod="ConsultarPeriodo">
          <SelectParameters>
             <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano"
                PropertyName="SelectedValue" />
        </SelectParameters>
        </asp:ObjectDataSource>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por coordenadoria e unidade administrativa"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" Width="70px" DataSourceID="odsAno" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                 <td align="left">
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                         Width="100px" AutoPostBack="True" >
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvPeriodo" runat="server" ControlToValidate="ddlPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvPeriodoPesquisa" runat="server" ControlToValidate="ddlPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                     <tweb:TSearchBox ID="tseRegional" runat="server" Argument="descricao" ArgumentColumns="50" DataType="Number"
                                    MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                    Key="id_regional" SqlSelect="select distinct ID_REGIONAL, descricao from (select distinct ue.ID_REGIONAL, n.regional as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                join TCE_REGIONAL n on n.ID_REGIONAL = ue.ID_REGIONAL) as tabela" SqlOrder="descricao, id_regional">
                                    <gridcolumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="80%" />
                                    </gridcolumns>
                                </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect=" select distinct ue.unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio, ua_atual, ua_antiga,id_regional from [VW_SOLICITACAO_ALTERACAO_NOTA_AGUARDANDO] df inner join [LYCEUM].[dbo].[VW_UNIDADE_ENSINO_SITUACAO] ue  on df.UNIDADE_ENS = ue.UNIDADE_ENS "
                        OnChanged="tseUnidade_Ensino_Changed" GridWidth="850px" SqlOrder="nome_comp"
                        SqlWhere= " ue.ID_REGIONAL = #tseRegional# " >
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="15%" />					    
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="15%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    Status do Pedido:
                </td>
                <td>
                    <asp:DropDownList ID="cmbStatus" runat="server" OnSelectedIndexChanged="cmbStatus_SelectedIndexChanged" AutoPostBack ="true" >
                        <asp:ListItem Selected="True" Value="Aguardando">Aguardando</asp:ListItem>
                        <asp:ListItem Value="Aprovado">Aprovado</asp:ListItem>
                        <asp:ListItem Value="Reprovado">Reprovado</asp:ListItem>
                        <asp:ListItem Value="Todas">Todas</asp:ListItem>
                    </asp:DropDownList>
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
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdAnaliseLiberacaoNotas" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsAnaliseLiberacaoLancamento" Visible="False" ClientInstanceName="grdAnaliseLiberacaoNotas"
                        KeyFieldName="ID_SOLICITACAO_ALTERACAO_NOTA" OnCustomJSProperties="grdAnaliseLiberacaoNotas_CustomJSProperties">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grdAnaliseLiberacaoNotas.SelectAllRowsOnPage(this.checked);"
                                        title="Select/Unselect all rows on the page" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_SOLICITACAO_ALTERACAO_NOTA"
                                ReadOnly="true" VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" ReadOnly="true"
                                Visible="false" VisibleIndex="2">
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
                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" ReadOnly="true"
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
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" ReadOnly="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turma" VisibleIndex="7" FieldName="TURMA"
                                ReadOnly="true">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Disciplina" VisibleIndex="8" FieldName="DISC_NOME"
                                ReadOnly="true">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Bimestre/Trimestre" VisibleIndex="9" FieldName="SUBPERIODO"
                               >
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Solicitação" FieldName="DT_SOLICITACAO"
                                 VisibleIndex="10">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" ReadOnly="true"
                                VisibleIndex="11">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" 
                                VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Status" FieldName="DT_STATUS" Visible="false"
                                VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matrícula Aprovador" ReadOnly="true" FieldName="MATRICULA_APROVADOR"
                                VisibleIndex="14">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Limite" FieldName="DT_LIMITE" ReadOnly="true"
                                VisibleIndex="15">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" ReadOnly="true"
                                VisibleIndex="16">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                    <asp:Panel ID= "pnlBotoes" runat = "server" >
                            <asp:Button ID="btnAprovar" runat="server" Text="Aprovar" OnClick="btnAprovar_Click" />
                            <asp:Button ID="btnReprovar" runat="server" Text="Reprovar" OnClick="btnReprovar_Click" />
                   </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="odsAnaliseLiberacaoLancamento" TypeName="Techne.Lyceum.Net.Academico.AnaliseLiberacaoLancamento"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="regional" PropertyName="DBValue" />
             <asp:ControlParameter ControlID="cmbStatus" DefaultValue="" Name="status"
                PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano"
                PropertyName="SelectedValue" />
                  <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo"
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
