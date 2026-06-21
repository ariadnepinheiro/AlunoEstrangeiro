<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="LiberacaoConfirmacaoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.LiberacaoConfirmacaoMatricula" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
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
                <dxwgv:ASPxGridView ClientInstanceName="grdConfirmacao" ID="grdConfirmacao" runat="server"
                    AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"
                    Visible="false" OnCustomButtonInitialize="grdConfirmacao_CustomButtonInitialize"
                    EnableCallBacks="false" OnCustomButtonCallback="grdConfirmacao_CustomButtonCallback"
                    OnPageIndexChanged="grdConfirmacao_PageIndexChanged">
                    <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="1" ButtonType="Link" Width="50px" Caption="Liberar">
                            <CustomButtons>
                                <dxwgv:GridViewCommandColumnCustomButton ID="btnLiberar" Text="Liberar">
                                </dxwgv:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONFIRMACAO_MATRICULA"
                            Name="ID_CONFIRMACAO_MATRICULA" VisibleIndex="0" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="ALUNO" FieldName="ALUNO" Name="ALUNO" VisibleIndex="2"
                            Width="20%" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Ano Letivo" FieldName="ANO" VisibleIndex="3">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Período Letivo" FieldName="PERIODO" VisibleIndex="4">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO"
                            VisibleIndex="5">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Modalidade / Segmento / Curso" FieldName="MOD_SEG_CURSO"
                            VisibleIndex="6">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Série/Ano Escolar" FieldName="SERIE" VisibleIndex="7">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="8">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn VisibleIndex="9" Caption="Data Sugerida" Name="DT_SUGERIDA"
                            FieldName="DT_SUGERIDA" Width="100px" Visible="true" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                <ValidationSettings>
                                    <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Ensino Religioso" FieldName="ENSINO_RELIGIOSO"
                            Name="ENSINO_RELIGIOSO" VisibleIndex="10">
                            <DataItemTemplate>
                                <asp:CheckBox ID="chkEnsinoReligioso" Enabled="false" runat="server" Checked='<%# this.VerificarCheck(Eval("ENSINO_RELIGIOSO")) %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Língua Estrangeira Facultativa" FieldName="LINGUA_ESTRANGEIRA_FACULTATIVA"
                            Name="LINGUA_ESTRANGEIRA_FACULTATIVA" VisibleIndex="11">
                            <DataItemTemplate>
                                <asp:CheckBox ID="chkLinguaEstrangeira" Enabled="false" runat="server" Checked='<%# this.VerificarCheck(Eval("LINGUA_ESTRANGEIRA_FACULTATIVA")) %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Progr. de aceleração de estudos (Proj. Autonomia)"
                            FieldName="PROJETO_AUTONOMIA" Name="PROJETO_AUTONOMIA" VisibleIndex="12">
                            <DataItemTemplate>
                                <asp:CheckBox ID="chkProjetoAutonomia" Enabled="false" runat="server" Checked='<%# this.VerificarCheck(Eval("PROJETO_AUTONOMIA")) %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Situação" Name="DT_ALTERACAO"
                            FieldName="DT_ALTERACAO" Width="100px" Visible="true" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                <ValidationSettings>
                                    <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" VisibleIndex="14">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </td>
        </tr>
    </table>
    <dxpc:ASPxPopupControl ID="pcMatriculaTurmas" ClientInstanceName="pcMatriculaTurmas"
        runat="server" Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableAnimation="true"
        Width="700px">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppDisciplinasMatricula" runat="server">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Para liberar a confirmação será necessário cancelar a(s) matrícula(s) associada(s) abaixo:"
                                runat="server" SkinID="lblMensagem"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <dxwgv:ASPxGridView ID="grdMatriculaTurmas" runat="server" EnableRowsCache="false"
                                EnableViewState="false" ClientInstanceName="grdMatriculaTurmas" AutoGenerateColumns="False"
                                Width="95%" Font-Names="Verdana" Font-Size="Small">
                                <SettingsText EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewDataColumn FieldName="FACULDADE" Caption="Unidade" VisibleIndex="0"
                                        Visible="false" />
                                    <dxwgv:GridViewDataColumn FieldName="UNIDADE" Caption="Unidade" VisibleIndex="0" />
                                    <dxwgv:GridViewDataColumn FieldName="MOD_SEG_CURSO" Caption="Curso" VisibleIndex="1" />
                                    <dxwgv:GridViewDataColumn FieldName="SERIE" Caption="Serie" VisibleIndex="2" />
                                    <dxwgv:GridViewDataColumn FieldName="TURNO" Caption="Turno" VisibleIndex="3" />
                                    <dxwgv:GridViewDataColumn FieldName="TURMA" Caption="Turma" VisibleIndex="4" />
                                    <dxwgv:GridViewDataColumn FieldName="TIPO_MATRICULA" Caption="Tipo de Matricula"
                                        VisibleIndex="5" />
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ImageButton ID="btnConfirmarLiberacao" SkinID="BcSalvar" runat="server" OnClick="btnConfirmarLiberacao_Click" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelaLiberacao" runat="server" SkinID="BcCancelar" OnClick="btnCancelaLiberacao_Click"
                                Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
</asp:Content>
