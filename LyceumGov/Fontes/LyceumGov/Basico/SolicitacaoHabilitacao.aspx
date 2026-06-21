<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SolicitacaoHabilitacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.SolicitacaoHabilitacao"
    Title="Solicitação de Habilitação" %>

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
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hdnUnidadeAdministrativa" />
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect="select distinct S.UA_ATUAL, NOMESETOR, ue.SETOR, S.ua_antiga from LY_UNIDADE_ENSINO ue inner join HADES..VW_SETOR s on ue.SETOR = s.SETOR inner join LY_UNIDADES_ASSOCIADAS uas on uas.UNIDADE_ENS = ue.UNIDADE_ENS inner join LY_USUARIO_UNIDADE_FIS uuf on uuf.UNIDADE_FIS = uas.UNIDADE_FIS"
                        ColumnName="UA_ATUAL" Caption="" MaxLength="15" DataType="Varchar" OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="SETOR" Width="10%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="UA_ATUAL" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="NOMESETOR" Width="70%" />                    
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnPrincipal" runat="server" GroupingText="Informe os dados para inclusão da solicitação:"
        Width="800px" Visible="false">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblDocente" runat="server" Text="Id/Vínculo do Docente:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>                
                    <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteLotacaoAtiva"
                        AutoPostBack="true" OnChanged="tseDocente_Changed" ValidateText="true" Width="485px">
                        <QueryParameters>
                            <asp:ControlParameter ControlID="tseUnidadeAdministrativa" Name="UA_ATUAL" PropertyName="DBValue" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT distinct ue.unidade_ens, ue.nome_comp, ue.setor, ue.cgc,ue.ua_atual,ue.ua_antiga from VW_ZZCRO_UNIDADE_ENSINO ue inner join ly_nucleo n  on ue.NUCLEO = n.NUCLEO inner join HADES..VW_SETOR sn on n.SETOR = sn.SETOR "
                        GridWidth="850px" SqlOrder="nome_comp" SqlWhere="(ue.ua_atual = #tseUnidadeAdministrativa# or sn.UA_ATUAL = #tseUnidadeAdministrativa#)"
                        AutoPostBack="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="40%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCargo" runat="server" Text="Cargo: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCargo" runat="server" Enabled="false" Width="515px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblFuncao" runat="server" Text="Função: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFuncao" runat="server" Enabled="false" Width="515px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSituacao" runat="server" Enabled="false" Width="515px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblDisciplinaIngresso" runat="server" Text="Disciplina de Ingresso: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDiscIngresso" runat="server" Enabled="false" Width="515px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblSegmento" runat="server" Text="Segmento de Atuação:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategoriaCurso" runat="server" AutoPostBack="false">
                        <asp:ListItem Text="<Nenhum>" Value="" Selected="True">
                        </asp:ListItem>
                        <asp:ListItem Text="Ensino Fundamental Anos Iniciais" Value="Ensino Fundamental Anos Iniciais">
                        </asp:ListItem>
                        <asp:ListItem Text="Ensino Fundamental Anos Finais / Ensino Médio" Value="Ensino Fundamental Anos Finais / Ensino Médio">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCategoriaCurso" runat="server" ControlToValidate="ddlCategoriaCurso"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblGrupoTSearch" runat="server" Text="Grupo de disciplinas:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAgrupamento" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDisciplinasParaHabilitacao"
                        AutoPostBack="true" ValidateText="true">
                        <QueryParameters>
                            <asp:ControlParameter ControlID="tseDocente" Name="matricula" PropertyName="DBValue" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblHabilitar" runat="server" Text="Habilitar para: "></asp:Label>
                </td>
                <td>
                    <asp:CheckBoxList ID="cblHabilitar" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblHabilitar_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Value="Matricula">Matrícula</asp:ListItem>
                        <asp:ListItem Value="GLP">GLP</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlTipoSubs" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="text-align: right; width: 15%">
                                    <asp:Label ID="lblTipoSubst" runat="server" Text="Tipo de Substituição: "></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblTipoSubstituicao" runat="server" Enabled="false" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rblTipoSubstituicao_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="Matricula">Matricula</asp:ListItem>
                                        <%--desabilitado temporariamente (dia 16-05-2011) a pedido do quadro de horarios
                    <asp:ListItem Value="GLP">GLP</asp:ListItem>
                    <asp:ListItem Value="CT">Contrato</asp:ListItem>--%>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDocenteSubs" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="text-align: right; width: 15%">
                                    <asp:Label ID="lblDocenteSubst" runat="server" Text="Matrícula que irá substituir:* "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearch ID="TSDocenteSubst" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente">
                                    </tweb:TSearch>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Incluir Solicitação"
                        OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnConsulta" runat="server" GroupingText="Consulta de Solicitação cadastradas">
        <table>
            <tr>
                <td>
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
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnGrid" runat="server">
                        <dxwgv:ASPxGridView ID="grdSolHabilitacao" runat="server" AutoGenerateColumns="False"
                            Visible="False" ClientInstanceName="grdSolHabilitacao" DataSourceID="odsSolicitacaoHabilitacao"
                            KeyFieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE" OnAfterPerformCallback="grdSolHabilitacao_AfterPerformCallback"
                            OnCustomJSProperties="grdSolHabilitacao_CustomJSProperties" OnCommandButtonInitialize="grdSolHabilitacao_CommandButtonInitialize">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <EditButton Text="Editar" Visible="False">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <UpdateButton Text="Salvar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_SOLICITACAO_HABILITACAO_DOCENTE"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="REGIONAL" FieldName="REGIONAL" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADE_ENS" VisibleIndex="3"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                 <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="IDVINCULO" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cargo" VisibleIndex="7" FieldName="CARGO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Função" VisibleIndex="8" FieldName="FUNCAO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Em aula?" VisibleIndex="9" FieldName="em_aula"
                                    ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" VisibleIndex="10"
                                    FieldName="DISCIPLINA_INGRESSO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="SEGMENTO_ATUACAO"
                                    VisibleIndex="11">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina para habilitar" FieldName="DISCIPLINA_HABILITAR"
                                    VisibleIndex="12">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina " FieldName="AGRUPAMENTO" Visible="false"
                                    VisibleIndex="13">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Habilitar para:" FieldName="HABILITACAO_MATRICULA_GLP"
                                    ReadOnly="true" VisibleIndex="14">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Matricula para substituir" VisibleIndex="15"
                                    FieldName="MATRICULA_SUBSTITUIDA">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo de Substituição" FieldName="TIPO_SUBSTITUICAO"
                                    VisibleIndex="16">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUS" VisibleIndex="17">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Motivo" VisibleIndex="18" FieldName="MOTIVO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DATA_CADASTRO" VisibleIndex="19">
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsSolicitacaoHabilitacao" TypeName="Techne.Lyceum.Net.Basico.SolicitacaoHabilitacao"
        runat="server" SelectMethod="Listar" DeleteMethod="Delete" OnDeleting="odsSolicitacaoHabilitacao_Deleting">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnUnidadeAdministrativa" Name="SETOR" PropertyName="Value" />
            <asp:ControlParameter ControlID="cmbStatus" DefaultValue="" Name="STATUS" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 33%;
        }
        .style2
        {
            width: 86px;
        }
    </style>
</asp:Content>
