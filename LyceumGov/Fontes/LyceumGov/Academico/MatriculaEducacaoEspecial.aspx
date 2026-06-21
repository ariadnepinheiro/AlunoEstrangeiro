<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MatriculaEducacaoEspecial.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MatriculaEducacaoEspecial" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="cTurma" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        var ASPxClientMenuBase;

        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueioMatricula");
            divBloqueio.className = "Bloqueado";
        }
    </script>

    <table>
        <tr>
            <td>
                <%-- Informe a matrícula ou o nome do aluno --%>
                <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
                    Width="620px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMatriculaTSearch" runat="server" Text="Matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearch ID="tseAluno" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoNecessidadeEspecial"
                                    AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                    <QueryParameters>
                                        <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                    </QueryParameters>
                                </tweb:TSearch>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <%-- Mensagens --%>
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <div class="divEditBlock" style="width: 740px;">
                    <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
                    <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
                        OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
                    <asp:Label runat="server" ID="lblBloco" Text="Matrículas" SkinID="BcTitulo" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <%-- Quadro Dados Escolares --%>
                <asp:Panel ID="pnSerieTurma" GroupingText="Dados Escolares" runat="server" Visible="false"
                    Width="740px">
                    <table>
                        <tr>
                            <td align="right" style="width: 100px">
                                <asp:Label ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurso" Enabled="false" runat="server" Width="150px" Visible="false"></asp:TextBox><asp:TextBox
                                    ID="txtNomeCurso" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="lblCurriculo" runat="server" Text="Matriz Curricular: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurriculo" ReadOnly="true" Width="230px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTurno" ReadOnly="true" runat="server" Width="150px" Visible="false"></asp:TextBox><asp:TextBox
                                    ID="txtNomeTurno" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblSerie" runat="server" Text="Ano Escolar: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSerie" ReadOnly="true" Width="230px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblUnidadeFisica" runat="server" Text="Unidade Física: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnidadeFisica" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtUnidadeFisicaDescr" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblCoordenadoria" runat="server" Text="Regional: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNucleo" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDecricaoNucleo" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblFaculdade" runat="server" Text="Unidade de Ensino: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFaculdade" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFaculdadeDescr" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <%-- Turma --%>
                <asp:Panel ID="pnAnoPeriodo" runat="server" GroupingText="Turma" Visible="false"
                    Enabled="false" Width="740px">
                    <table>
                        <tr>
                            <td align="right" style="width: 100px">
                                <asp:Label ID="lblAno" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAno" runat="server" DataValueField="ano" AutoPostBack="true"
                                    DataTextField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" Width="250px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                                    InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="lblSemestre" runat="server" Text="Período:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSemestre" runat="server" DataValueField="periodo" AutoPostBack="true"
                                    DataTextField="id_reduzida" OnSelectedIndexChanged="ddlSemestre_SelectedIndexChanged"
                                    Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSemestre" runat="server" ControlToValidate="ddlSemestre"
                                    InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="Label2" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" Key="curso" ArgumentColumns="60"
                                    Columns="10" OnChanged="tseCurso_Changed" MaxLength="20" Argument="nome" GridWidth="800px">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="60%" />
                                        <tweb:TSearchBoxColumn Caption="Tipo Curso" FieldName="tipo_curso" Width="20%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="Label3" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="True" DataTextField="descricao"
                                    DataValueField="turno" Width="100px" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTurno"
                                    InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="Label4" runat="server" Text="Ano de escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" AutoPostBack="true"
                                    DataValueField="serie" Width="100px" onchange="Bloqueio()" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSerie"
                                    InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTurma" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurma" runat="server" AutoPostBack="true" DataValueField="turma"
                                    DataTextField="turma" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged"
                                    Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvTurma" runat="server" ControlToValidate="ddlTurma"
                                    InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                <asp:LinkButton ID="lnkVisualizarDisciplina" runat="server" OnClick="lnkVisualizarDisciplina_Click"
                                    Visible="false">Visualizar Disciplinas</asp:LinkButton>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                                    Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoCurso" runat="server" Visible="false">
                                    <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <%-- Turma AEE - Atendimento Especializado --%>
                <asp:Panel ID="PnlAtendimentoEspecializado" runat="server" GroupingText="Turma AEE - Atendimento Especializado"
                    Visible="False" Width="740px">
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label5" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblUnidadeEnsinoAtendimentoEspecializado" runat="server" Width="55px"></asp:Label>
                                -
                                <asp:Label ID="lblNomeUnidadeEnsinoAtendimentoEspecializado" runat="server" Width="250px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label8" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAnoAtendimentoEspecializado" ReadOnly="true" runat="server" Width="250px"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label7" Text="Período:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPeriodoAtendimentoEspecializado" runat="server" Width="150px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label11" Text="Turno:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurnoAtendimentoEspecializado" runat="server" AutoPostBack="true"
                                    DataTextField="descricao" DataValueField="turno" OnSelectedIndexChanged="ddlTurnoAtendimentoEspecializado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label12" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurmaAtendimentoEspecializado" runat="server" DataTextField="turma"
                                    DataValueField="turma" AutoPostBack="true" OnSelectedIndexChanged="ddlTurmaAtendimentoEspecializado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblProfessorAtendimento" runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSalvarAtendimentoEspecializado" runat="server" Text="Salvar Turma"
                                    OnClick="btnSalvarAtendimentoEspecializado_Click" OnClientClick="Bloqueio()" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:ObjectDataSource ID="odsAtendEspecializado" TypeName="Techne.Lyceum.Net.Academico.MatriculaEducacaoEspecial"
                        runat="server" SelectMethod="ListaAtendEspecializado" OnDeleting="odsAtendEspecializado_Deleting"
                        DeleteMethod="DeleteAtendEspecializado">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlSemestre" DefaultValue="" Name="semestre" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="PnlAtendimentoEspecializado" PropertyName="Visible"
                                Name="painel" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <dxwgv:ASPxGridView ID="grdAtendEspecializado" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdAtendEspecializado" DataSourceID="odsAtendEspecializado"
                        KeyFieldName="ALUNO;TURMA;ANO;SEMESTRE;SIT_MATRICULA" OnAfterPerformCallback="grdAtendEspecializado_AfterPerformCallback"
                        OnCommandButtonInitialize="grdAtendEspecializado_CommandButtonInitialize">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                Visible="FALSE">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Width="30px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                Width="50px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SIT_MATRICULA" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <%-- Turma AEE - Sala de Recurso --%>
                <asp:Panel ID="pnlTurmaSalaRecurso" runat="server" GroupingText="Turma AEE - Sala de Recurso"
                    Visible="False" Width="740px">
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label6" Text="Ano:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAnoEducacaoEspecial" runat="server" Width="150px"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label9" Text="Período:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPeriodoEducacaoEspecial" runat="server" Width="150px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 11px; width: 110px">
                                <asp:Label ID="Label1" runat="server" Text="Regional:* "></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseRegionalSalaRecurso" runat="server" Argument="regional" ArgumentColumns="50"
                                    MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegionalSalaRecurso_Changed"
                                    SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                    DataType="Number">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td style="text-align: right; font-weight: bold; font-size: 11px; width: 110px">
                                <asp:Label ID="Label10" runat="server" Text="Unidade de Ensino:* "></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidadeEnsinoSalaRecurso" runat="server" Caption="" Key="FACULDADE"
                                    MaxLength="20" Columns="10" ArgumentColumns="50" Argument="nome_comp" AutoPostBack="True"
                                    OnChanged="tseUnidadeEnsinoSalaRecurso_Changed" SqlOrder="nome_comp" SqlWhere="  ATIVA = 'S'  AND d.TIPO_DEPEND = 'SALAAEE' AND u.id_regional IS NOT NULL and u.id_regional = #tseRegionalSalaRecurso# "
                                    SqlSelect=" select DISTINCT u.id_regional,u.NOME_COMP  FROM    LY_DEPENDENCIA d INNER JOIN dbo.LY_UNIDADE_FISICA uf ON d.FACULDADE = uf.UNIDADE_FIS  LEFT JOIN dbo.LY_UNIDADE_FISICA_EDIFICACAO ufe ON uf.UNIDADE_FIS = ufe.UNIDADE_FIS  AND d.PAVIMENTO = ufe.PAVIMENTO  AND d.EDIFICACAO = ufe.EDIFICACAO inner join VW_UNIDADE_ENSINO_SITUACAO u on uf.UNIDADE_FIS = u.UNIDADE_ENS ">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="FACULDADE" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label15" Text="Turno:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurnoEducEspecial" runat="server" AutoPostBack="True" DataTextField="descricao"
                                    DataValueField="turno" Width="150px" OnSelectedIndexChanged="ddlTurnoEducEspecial_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblDscSerieEducEspecial" Text="Ano de Escolaridade:* " SkinID="lblObrigatorio"
                                    runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSerieEducEspecial" runat="server" DataTextField="serie"
                                    AutoPostBack="true" DataValueField="serie" OnSelectedIndexChanged="ddlSerieEducEspecial_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label16" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurmaEducEspecial" runat="server" AutoPostBack="True" DataTextField="turma"
                                    DataValueField="turma">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSalvarTurmaEducacaoEspecial" runat="server" Text="Salvar Turma"
                                    OnClick="btnSalvarTurmaEducacaoEspecial_Click" OnClientClick="Bloqueio()" />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:ObjectDataSource ID="odsSalaRecurso" TypeName="Techne.Lyceum.Net.Academico.MatriculaEducacaoEspecial"
                        runat="server" SelectMethod="ListaSalaRecurso" OnDeleting="odsSalaRecurso_Deleting"
                        DeleteMethod="DeleteSalaRecurso">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlSemestre" DefaultValue="" Name="semestre" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="pnlTurmaSalaRecurso" PropertyName="Visible" Name="painel" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <dxwgv:ASPxGridView ID="grdSalaRecurso" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdSalaRecurso" DataSourceID="odsSalaRecurso" KeyFieldName="ALUNO;TURMA;ANO;SEMESTRE;SIT_MATRICULA"
                        OnAfterPerformCallback="grdSalaRecurso_AfterPerformCallback" OnCommandButtonInitialize="grdSalaRecurso_CommandButtonInitialize">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                Visible="FALSE">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="4" Width="30px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="5"
                                Width="50px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="6"
                                Width="30px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SIT_MATRICULA" VisibleIndex="8">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMensagem2" runat="server" SkinID="lblMensagem"></asp:Label>
            </td>
        </tr>
    </table>
    <dxpc:ASPxPopupControl ID="pcDisciplinasMatricula" runat="server" Modal="True" Width="600"
        Height="350" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcDisciplinasMatricula" HeaderText="Disciplinas Ativas"
        AllowDragging="True" EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppDisciplinasMatricula" runat="server">
                <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <dxp:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnRemoveDisciplinasMatricula">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent3" runat="server">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <dxwgv:ASPxGridView ID="grdDisciplinasAtivas" runat="server" KeyFieldName="DISCIPLINA"
                                                    ClientInstanceName="grdDisciplinasAtivas" EnableCallBacks="false" AutoGenerateColumns="False"
                                                    Width="95%" Font-Names="Verdana" Font-Size="Small" OnPageIndexChanged="grdDisciplinasAtivas_PageIndexChanged">
                                                    <SettingsText EmptyDataRow="Não existem dados." />
                                                    <SettingsPager PageSize="15" />
                                                    <Columns>
                                                        <dxwgv:GridViewDataColumn FieldName="DISCIPLINA" Caption="Código" VisibleIndex="0" />
                                                        <dxwgv:GridViewDataColumn FieldName="NOME" Caption="Disciplina" VisibleIndex="0" />
                                                    </Columns>
                                                </dxwgv:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
</asp:Content>
