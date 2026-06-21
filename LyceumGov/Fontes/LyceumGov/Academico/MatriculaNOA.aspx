<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MatriculaNOA.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MatriculaNOA" %>

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

            <asp:HiddenField ID="hdnSegmentoPrincipal" runat="server" />
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
                            <asp:Label runat="server" ID="lblBloco" Text="Enturmação NOA" SkinID="BcTitulo" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMatriculas" runat="server" GroupingText="Dados Escolares" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel3" runat="server" Width="700px">
                                            <dxwgv:ASPxGridView ID="grdMatriculas" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdMatriculas" DataSourceID="odsMatriculas" KeyFieldName="TURMA;TIPO;CURSO"
                                                OnAfterPerformCallback="grdMatriculas_AfterPerformCallback">
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
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsMatriculas" TypeName="Techne.Lyceum.Net.Academico.MatriculaNOA"
                                runat="server" SelectMethod="ListarMatriculas">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="pnlMatriculas" PropertyName="Visible" Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaNOA" Visible="false" runat="server" GroupingText="Turma de NOA"
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
                                        <asp:Label ID="lblAnoNOATitulo" Text="Ano: " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        <asp:Label ID="lblAnoNOA" runat="server" Width="50px"></asp:Label>
                                        <asp:DropDownList ID="ddlAno" runat="server" DataValueField="ano" Visible="false"
                                            AutoPostBack="true" DataTextField="ano" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblPeriodoNOATitulo" Text="Período: " Width="50px" runat="server"
                                            SkinID="lblObrigatorio"></asp:Label>
                                        <asp:Label ID="lblPeriodoNOA" runat="server" Width="50px"></asp:Label>
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
                                        <asp:Label ID="lblTurnoNOATitulo" Text="Turno:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlTurno" runat="server" Width="150px" Enabled="false">
                                            <asp:ListItem Value="I" Text="INTEGRAL" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurmaNOATitulo" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
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
                                        <asp:Button ID="btnSalvarTurmaNOA" runat="server" Text="Salvar" OnClick="btnSalvarTurmaNOA_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" runat="server">
                                            <dxwgv:ASPxGridView ID="grdNOA" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdNOA"
                                                DataSourceID="odsNOA" KeyFieldName="ALUNO;ANO;SEMESTRE;TURMA" OnAfterPerformCallback="grdNOA_AfterPerformCallback"
                                                OnStartRowEditing="grdNOA_StartRowEditing" OnInitNewRow="grdNOA_InitNewRow">
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
                            <asp:ObjectDataSource ID="odsNOA" TypeName="Techne.Lyceum.Net.Academico.MatriculaNOA"
                                runat="server" SelectMethod="ListaMatriculaNOAPor" OnDeleting="odsNOA_Deleting"
                                DeleteMethod="RemoveMatriculaNOA">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlSemestre" DefaultValue="" Name="semestre" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="pnlTurmaNOA" PropertyName="Visible" Name="painel" />
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
