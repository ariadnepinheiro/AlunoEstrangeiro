<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DocenteFuncaoGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.DocenteFuncaoGLP" %>

<asp:Content ID="conFuncaoGLP" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdDocenteFuncaoGLP.cpMessage != null)
                lblMensagem.innerHTML = grdDocenteFuncaoGLP.cpMessage;
        }

        function grid_SelectionChanged(s, e) {
            s.GetSelectedFieldValues("CONTAGEMCARENCIAS", GetSelectedFieldValuesCallback);
        }

        function GetSelectedFieldValuesCallback(values) {

            var total = 0;

            for (var i = 0; i < values.length; i++) {
                total = total + values[i];
            }

            document.getElementById("<%=txtQtdGLP.ClientID %>").value = parseInt(total);
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
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect="select distinct ue.SETOR, NOMESETOR,UE.UNIDADE_ENS,UA_ATUAL, ua_antiga from LY_UNIDADE_ENSINO ue inner join HADES..VW_SETOR s on ue.SETOR = s.SETOR inner join LY_UNIDADES_ASSOCIADAS uas on uas.UNIDADE_ENS = ue.UNIDADE_ENS inner join LY_USUARIO_UNIDADE_FIS uuf on uuf.UNIDADE_FIS = uas.UNIDADE_FIS"
                        ColumnName="SETOR" Caption="" MaxLength="15" DataType="Varchar" OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="SETOR" Width="10%" /> 
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="UA_ATUAL" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                             
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="NOMESETOR" Width="55%" />
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="UNIDADE_ENS" Width="15%" />
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
                    <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente"
                        AutoPostBack="true" OnChanged="tseDocente_Changed" ValidateText="true">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT distinct ue.unidade_ens, ue.nome_comp, ue.setor, ue.cgc,ua_atual,ua_antiga from VW_ZZCRO_UNIDADE_ENSINO ue"
                        GridWidth="850px" SqlOrder="nome_comp" SqlWhere=" ue.SETOR = #tseUnidadeAdministrativa# "
                        AutoPostBack="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="55%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCargo" runat="server" Text="Segmento de Atuação:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategoriaCurso" runat="server" AutoPostBack="false">
                        <asp:ListItem Text="Selecione" Value="" Selected="True">
                        </asp:ListItem>
                        <asp:ListItem Text="Ensino Fundamental Anos Iniciais" Value="Ensino Fundamental Anos Iniciais">
                        </asp:ListItem>
                        <asp:ListItem Text="Ensino Fundamental Anos Finais / Ensino Médio" Value="Ensino Fundamental Anos Finais / Ensino Médio">
                        </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblDisciplina" runat="server" Text="Disciplina:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDisciplina" runat="server" DataTextField="descricao" DataValueField="agrupamento"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged" onchange="Bloqueio()">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlGridTurmas" Visible="false" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsTurmaCarencia" TypeName="Techne.Lyceum.Net.Basico.DocenteFuncaoGLP"
                            runat="server" SelectMethod="ListarTurmaCarencia">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidadeEnsino"
                                    PropertyName="DBValue" />
                                <asp:ControlParameter ControlID="ddlDisciplina" DefaultValue="" Name="agrupamentoDisciplina"
                                    PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdTurmaCarencia" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdTurmaCarencia" DataSourceID="odsTurmaCarencia" KeyFieldName="CompositeKey"
                            OnAfterPerformCallback="grdTurmaCarencia_AfterPerformCallback" OnCustomUnboundColumnData="grdTurmaCarencia_CustomUnboundColumnData">
                            <SettingsEditing Mode="Inline" />
                            <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" />
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" Visible="False"
                                    VisibleIndex="0" UnboundType="String">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="1"
                                    Width="50">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" ReadOnly="true"
                                    VisibleIndex="2" Width="50">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" ReadOnly="true" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" ReadOnly="true"
                                    VisibleIndex="3" Width="90" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome Disciplina" FieldName="NOMEDISCIPLINA"
                                    ReadOnly="true" VisibleIndex="3" Visible="true" Width="90">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="C.H." FieldName="CONTAGEMCARENCIAS" ReadOnly="true"
                                    VisibleIndex="3" Width="50">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Cârencia" FieldName="TIPOCARENCIA" ReadOnly="true"
                                    VisibleIndex="3" Width="150">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NUM_FUNC_CARENCIA" FieldName="NUM_FUNC" ReadOnly="true"
                                    VisibleIndex="3" Width="150" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblQtdGLP" runat="server" Text="CH Solicitada:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQtdGLP" runat="server" MaxLength="2" SkinID="numerico" Width="30"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td align="left">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Incluir Solicitação"
                        OnClick="btnSalvar_Click" Visible="false" OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsDocenteFuncaoGLP" TypeName="Techne.Lyceum.Net.Basico.DocenteFuncaoGLP"
        runat="server" SelectMethod="Listar" DeleteMethod="Delete" OnDeleting="odsDocenteFuncaoGLP_Deleting">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeAdministrativa" DefaultValue="" Name="unidadeAdm"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdDocenteFuncaoGLP" runat="server" AutoGenerateColumns="False"
        Visible="false" ClientInstanceName="grdDocenteFuncaoGLP" DataSourceID="odsDocenteFuncaoGLP"
        KeyFieldName="id_docente_funcao_glp" OnAfterPerformCallback="grdDocenteFuncaoGLP_AfterPerformCallback"
        OnCustomJSProperties="grdDocenteFuncaoGLP_CustomJSProperties" OnCustomColumnDisplayText="grdDocenteFuncaoGLP_CustomColumnDisplayText">
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
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="id_docente_funcao_glp" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="unidade_ens"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="nome_comp" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="IDVINCULO" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome_compl" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="agrupamento" VisibleIndex="5">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="funcao_glp" VisibleIndex="7"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="segmento"
                VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="GLP Solicitada" FieldName="glp_solicitada"
                VisibleIndex="9">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="GLP Usada" FieldName="glp_usada" VisibleIndex="10">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="GLP Cancelada" FieldName="glp_cancelada" VisibleIndex="11">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="status" VisibleIndex="12">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="data" VisibleIndex="13">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Detalhes" Name="btnDetalhes" VisibleIndex="17"
                Width="50px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <DataItemTemplate>
                    <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("id_docente_funcao_glp") %>'
                        OnCommand="btnDetalhes_Command" ImageAlign="Middle" ImageUrl="~/img/bt_busca.png"
                        Height="15px" AlternateText="Visualizar Detalhes do Pedido"></asp:ImageButton>
                </DataItemTemplate>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsTurmasPedido" TypeName="Techne.Lyceum.Net.Basico.AnaliseGLP"
        runat="server" SelectMethod="ListarTurmaPedido">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtRow" Name="id_docente_funcao_glp" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div style="visibility: hidden">
        <input id="txtRow" type="hidden" runat="server" />
    </div>
    <dxpc:ASPxPopupControl ID="pucTurmaPedido" runat="server" CloseAction="CloseButton"
        HeaderText="Turmas do Pedido" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="90%" Height="90%" AllowDragging="True"
        ClientInstanceName="pucTurmaPedido" EnableAnimation="False" EnableViewState="False"
        ShowCloseButton="true">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,18000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="puccItemHistorico" runat="server">
                <dxwgv:ASPxGridView ID="grdTurmasPedido" runat="server" EnableRowsCache="false" DataSourceID="odsTurmasPedido"
                    EnableViewState="false" ClientInstanceName="grdTurmasPedido" AutoGenerateColumns="False"
                    KeyFieldName="ID_DOCENTE_FUNCAO_GLP" Width="95%" Font-Names="Verdana" Font-Size="Small">
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewDataColumn FieldName="ANO" Caption="Ano" VisibleIndex="1">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="PERIODO" Caption="Período" VisibleIndex="2">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="TURMA" Caption="Turma" VisibleIndex="3" />
                        <dxwgv:GridViewDataColumn FieldName="DISCIPLINA" Caption="Disciplina" VisibleIndex="4" />
                        <dxwgv:GridViewDataColumn FieldName="NOMEDISCIPLINA" Caption="Disciplina" VisibleIndex="5" />
                        <dxwgv:GridViewDataColumn FieldName="CARGAHORARIA" Caption="C.H." VisibleIndex="6">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="ID_DOCENTE_FUNCAO_GLP" VisibleIndex="5" Visible="false">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
