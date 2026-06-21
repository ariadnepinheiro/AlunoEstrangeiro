<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ExclusaoAlocacaoDocente.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.ExclusaoAlocacaoDocente" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conExcluirAlocDocentes" ContentPlaceHolderID="cphFormulario" runat="server">
    <%--    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />--%>

    <script type="text/javascript">
        $(document).ready(function() {
            Sys.Application.add_load(BlurTSearch8Caracteres);
        });

        function BlurTSearch8Caracteres() {
            //Chama evento BLUR quando o campo de código da TSearch atinge 8 caracteres ou mais
            $("#<%=tseDocente.ClientID %> input[originalValue]").keyup(function(e) {
                if ($(this).val().length >= 20)
                    $(this).blur();
            }).focus();
        }

        function confirmarExclusao(s, e) {
            var aulas = $("#<%=grdAulas.ClientID %> input:checkbox:checked").length;
            if (aulas == 0) {
                alert('Selecione pelo menos 1 alocação.');
                e.processOnServer = false;
                return;
            } else {
                var carencia = $("#<%=pnlAcao.ClientID %> input:radio:checked");
                if (carencia.length == 0) {
                    alert('Selecione Carência Real ou Carência Temporária.');
                    e.processOnServer = false;
                    return;
                }

                var msg = 'Deseja excluir ' +
                    ((aulas == 1) ? 'a alocação selecionada,\nsubstituindo-a por ' :
                                    'as ' + aulas + ' alocações selecionadas,\nsubstituindo-as por ') +
                    ((carencia.val() == '99999999') ? '"Carência Real"' : '"Carência Temporária"') + "?";
                e.processOnServer = confirm(msg);
                return;
            }
        }

        function selecionaCheckboxes(s, e, checked) {
            $("#<%=grdAulas.ClientID %> input:checkbox:enabled").attr('checked', checked);
            e.processOnServer = false;
        }    
    </script>

    <%--
    <asp:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="true" UpdateMode="Conditional"
        runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAlterarFuncao" EventName="Click" />            
        </Triggers> <ContentTemplate>--%>
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="Informe a Id/Vínculo ou o nome do docente e o ano das alocações"
        Height="65px" Width="710px">
        <asp:ObjectDataSource ID="odsAno" TypeName="Techne.Lyceum.RN.PeriodoLetivo" SelectMethod="ConsultarProximosAnos"
            runat="server" />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="Id/Vínculo do Docente*: "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocente" MaxLength="20" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente"
                        AutoPostBack="true" OnChanged="tseDocente_Changed" OnTextChanged="tseDocente_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAno" Text="Ano Letivo:" runat="server" SkinID="lblObrigatorio" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" AutoPostBack="true" DataSourceID="odsAno" DataTextField="ano"
                        DataValueField="ano" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" SkinID="lblMensagem" runat="server" />
    <br />
    <br />
    <asp:Panel ID="pnlDados" runat="server">
        <asp:Panel ID="pnlDocente" runat="server" GroupingText="Dados do docente" Width="584px">
            <table style="width: 92%">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNome" Text="Nome:" runat="server" />
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblMatricula" Text="Id/Vinculo:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtMatricula" runat="server" ReadOnly="true" Width="100%" />
                        <asp:HiddenField ID="hdnNumFunc" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblCPF" Text="CPF:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPF" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblCargo" Text="Cargo:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCargo" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblFuncao" Text="Função:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuncao" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr id="trDiscipIngressoSit" runat="server">
                    <td align="right">
                        <asp:Label ID="lblDisciplinaIngresso" Text="Disciplina de ingresso:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDisciplinaIngresso" ReadOnly="true" runat="server" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblSituacao" Text="Situação:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSituacao" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr id="trCH" runat="server">
                    <td align="right">
                        <asp:Label ID="lblCHIngresso" Text="CH de ingresso:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHIngresso" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right" style="width: 100px">
                        <asp:Label ID="lblCHTurma" Text="CH em turma:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHTurma" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr id="trSegundaMatricula" runat="server">
                    <td align="right">
                        <asp:Label ID="lblSegundaMatricula" Text="Segunda Matrícula:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSegundaMatricula" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlFuncao" runat="server" GroupingText="Função" Width="584px">
            <table style="width: 92%">
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseFuncaoLotacao" runat="server" Argument="descricao" ArgumentColumns="70"
                            FollowContainerMode="false" MaxLength="20" AutoPostBack="true" Columns="10" DataType="VarChar"
                            SqlWhere=" CAMPO_01 = 'S' AND ATIVO = 'S' " Key="funcao" OnChanged="tseFuncaoLotacao_Changed"
                            SqlOrder="descricao" SqlSelect="SELECT DISTINCT F.funcao, F.descricao FROM Ly_funcao F ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="width: 50px">
                        <dxe:ASPxButton ID="btnAlterarFuncao" Text="Alterar Função" runat="server" Wrap="False"
                            OnClick="btnAlterarFuncao_Click">
                            <ClientSideEvents Click="function(s,e) { e.processOnServer = confirm('Tem certeza que deseja alterar a função do docente?'); }" />
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="*  A troca da função não efetuará a desalocação automática do professor. A responsabilidade por manter a carga horária será da Coordenadoria de Gestão de Pessoas da Regional."
                            runat="server" SkinID="lblObrigatorio" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <dxwgv:ASPxGridView ID="grdAulas" DataSourceID="odsAulas" ClientInstanceName="grdAulas"
            runat="server" OnHtmlDataCellPrepared="grdAulas_HtmlDataCellPrepared">
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsText EmptyDataRow="O docente não possui alocações ativas." />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ano" Name="ano">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="semestre" Name="semestre">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="regional"
                    Name="regional">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="municipio_descr" Name="municipio_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ue_descr" Name="ue_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="ua_descr" Name="ua_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="censo" Name="censo" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="turma" Name="turma">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno_descr" Name="turno_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Dia da Aula" FieldName="dia_semana_descr"
                    Name="dia_semana_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Hora de Entrada" FieldName="hora_inicio" Name="hora_inicio">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Hora de Saída" FieldName="hora_fim" Name="hora_fim">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="disciplina_descr" Name="disciplina_descr">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" FieldName="DISCIPLINA_INGRESSO" Name="DISCIPLINA_INGRESSO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo Aula" FieldName="tipo" Name="tipo">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo GLP" FieldName="tipoglp" Name="tipoglp">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" Name="matricula">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="idvinculo" Name="idvinculo">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Excluir?" FieldName="pode_excluir" Name="pode_excluir">
                    <DataItemTemplate>
                        <asp:CheckBox ID="cbExcluir" runat="server" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <asp:ObjectDataSource ID="odsAulas" runat="server" SelectMethod="ObterAulasDoDocente"
            TypeName="Techne.Lyceum.RN.Turma">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnNumFunc" Name="numfunc" PropertyName="Value" />
                <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Panel ID="pnlAcao" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rbtnCarencia" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Text="Carência Real" Value="99999999" />
                            <asp:ListItem Text="Carência Temporária" Value="00000000"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width: 50px">
                        <dxe:ASPxButton ID="btnExcluir" Text="Excluir Alocações" runat="server" Wrap="False"
                            ClientSideEvents-Click="function(s,e) { confirmarExclusao(s,e);}" OnClick="btnExcluir_Click" />
                    </td>
                    <td align="right" style="width: 150px">
                        <dxe:ASPxButton ID="btnTodos" Text="Todos" ClientSideEvents-Click="function(s, e) { selecionaCheckboxes(s, e, true);}"
                            runat="server" />
                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnNenhum" Text="Nenhum" ClientSideEvents-Click="function(s, e) { selecionaCheckboxes(s, e, false);}"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <!--</ContentTemplate></asp:UpdatePanel>-->
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 263px;
        }
    </style>
</asp:Content>
