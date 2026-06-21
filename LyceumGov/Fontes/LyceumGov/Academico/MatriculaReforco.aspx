<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MatriculaReforco.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MatriculaReforco" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="cTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <script type="text/javascript">

                var ASPxClientMenuBase;
                var ASPxClientPopupMenu;


                function Bloqueio() {

                    var divBloqueio = document.getElementById("dvbloqueioMatricula");
                    divBloqueio.className = "Bloqueado";
                }     
     
            </script>

            <asp:HiddenField ID="hdnSeriePrincipal" runat="server" />
            <div id="dvbloqueioMatricula" class="Desbloqueado">
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
                            Width="620px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMatriculaTSearch" runat="server" Text="Matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearch ID="tseAluno" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
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
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <div class="divEditBlock" style="width: 740px;">
                            <asp:Label runat="server" ID="lblBloco" Text="Enturmação de Reforço" SkinID="BcTitulo" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEletivas" runat="server" GroupingText="Dados Escolares" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel3" runat="server" Width="700px">
                                            <dxwgv:ASPxGridView ID="grdEletivas" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdEletivas"
                                                DataSourceID="odsEletivas" KeyFieldName="TURMA;TIPO;CURSO" OnAfterPerformCallback="grdEletivaso_AfterPerformCallback">
                                                <SettingsBehavior ConfirmDelete="True" />
                                                <SettingsEditing Mode="Inline" />
                                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                <Columns>
                                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                        <CancelButton Text="Cancelar">
                                                            <Image Url="~/img/bt_cancelar.png" />
                                                        </CancelButton>
                                                        <ClearFilterButton Text="Limpar" Visible="True">
                                                            <Image Url="~/img/bt_limpa.png" />
                                                        </ClearFilterButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" ReadOnly="true" VisibleIndex="1"
                                                        Width="100px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="TIPO" VisibleIndex="2" Width="200px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" VisibleIndex="3"
                                                        Width="200px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="4"
                                                        Width="150px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnExluirEletivas" runat="server" Text="Excluir Eletivas" Visible="false"
                                            OnClick="btnExluirEletivas_Click" OnClientClick="Bloqueio()" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsEletivas" TypeName="Techne.Lyceum.Net.Academico.MatriculaReforco"
                                runat="server" SelectMethod="ListarEletivas">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="pnlEletivas" PropertyName="Visible" Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaOptativaReforco" Visible="false" runat="server" GroupingText="Turma de Reforço"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Label ID="Label5" runat="server" Text="Unidade de Ensino:" Width="100px" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUnidadeEnsinoReforco" runat="server" Width="250px"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtFaculdade" ReadOnly="true" runat="server" Visible="false" Width="50px"></asp:TextBox>
                                        <asp:Label ID="lblAnoOptativaReforcoTitulo" Text="Ano: " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        <asp:Label ID="lblAnoOptativaReforco" runat="server" Width="50px"></asp:Label>
                                        <asp:DropDownList ID="ddlAno" runat="server" DataValueField="ano" Visible="false"
                                            AutoPostBack="true" DataTextField="ano" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblPeriodoOptativaReforcoTitulo" Text="Período: " Width="50px" runat="server"
                                            SkinID="lblObrigatorio"></asp:Label>
                                        <asp:Label ID="lblPeriodoOptativaReforco" runat="server" Width="50px"></asp:Label>
                                        <asp:DropDownList ID="ddlSemestre" runat="server" Visible="false" DataValueField="periodo"
                                            AutoPostBack="true" DataTextField="id_reduzida" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurnoOptativaReforcoTitulo" Text="Turno:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label6" Text="Componente Curricular:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <tweb:TSearchBox runat="server" ID="tsbDisciplina" AutoPostBack="true" MaxLength="30"
                                            Key="disciplina" SqlSelect="Select disciplina, nome from ly_disciplina" SqlWhere="(disciplina = 'FOCO_MAT_4_PRESENC' or disciplina = 'FOCO_MAT_6_REMOTO' or disciplina = 'FOCO_PORT_4_PRESENC' or disciplina = 'FOCO_PORT_6_REMOTO')"
                                            SqlOrder="nome" OnChanged="tsbDisciplina_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Componente Curricular" FieldName="disciplina" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurmaOptativaReforcoTitulo" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlTurma" runat="server" AutoPostBack="True" DataTextField="turma"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="left">
                                        <br />
                                        <asp:Button ID="btnSalvarTurmaOptativaReforco" runat="server" Text="Salvar" OnClick="btnSalvarTurmaOptativaReforco_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" runat="server">
                                            <dxwgv:ASPxGridView ID="grdOptativaReforco" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdOptativaReforco" DataSourceID="odsOptativaReforco" KeyFieldName="ALUNO;ANO;SEMESTRE;TURMA"
                                                OnAfterPerformCallback="grdOptativaReforco_AfterPerformCallback" OnStartRowEditing="grdOptativaReforco_StartRowEditing"
                                                OnInitNewRow="grdOptativaReforco_InitNewRow">
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
                                                        Visible="false">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Width="30px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                        Width="50px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" FieldName="NOME" VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="MAISEDUCACAO" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data de Início" FieldName="DT_MATRICULA" VisibleIndex="8">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsOptativaReforco" TypeName="Techne.Lyceum.Net.Academico.MatriculaReforco"
                                runat="server" SelectMethod="ListaMatriculaOptativaReforcoPor" OnDeleting="odsOptativaReforco_Deleting"
                                DeleteMethod="RemoveMatriculaOptativaReforco">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlSemestre" DefaultValue="" Name="semestre" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="pnlTurmaOptativaReforco" PropertyName="Visible"
                                        Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMensagem2" runat="server" SkinID="lblMensagem"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
