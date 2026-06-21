<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EncerramentoAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.EncerramentoAluno" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:HiddenField ID="hdnCompartilhada" runat="server" />
    <asp:HiddenField ID="hdnAnoComp" runat="server" />
    <asp:HiddenField ID="hdnPeriodoComp" runat="server" />
    <asp:HiddenField ID="hdnCursoComp" runat="server" />
    <asp:HiddenField ID="hdnSerieComp" runat="server" />
    <asp:HiddenField ID="hdnPessoa" runat="server" />
    <dxpc:ASPxPopupControl ID="pcPreMatricula" runat="server" Modal="True" Width="600"
        CloseAction="CloseButton" Height="450" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ClientInstanceName="pcPreMatricula" HeaderText="Confirma" AllowDragging="True"
        EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppPreMatricula" runat="server">
                <dxp:ASPxPanel ID="pnlWindow" runat="server" DefaultButton="btnRemovePreMatricula">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent1" runat="server">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right" valign="middle">
                                        <asp:Label ID="lblPerguntaPreMatricula" Text="Aluno possui Pré-Matrícula. Remover os dados da pré-matricula? "
                                            runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <br />
                                <br />
                                <tr>
                                    <td align="right">
                                        <asp:ImageButton ID="btnRemovePreMatricula" SkinID="BcSalvar" runat="server" OnClick="btnRemovePreMatricula_Click"
                                            CssClass="cursorImagem" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnCancelaRemovePreMatricula" runat="server" SkinID="BcCancelar"
                                            OnClick="btnCancelaRemovePreMatricula_Click" CssClass="cursorImagem" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
    <br />
    <dxpc:ASPxPopupControl ID="pcDisciplinasMatricula" runat="server" Modal="True" Width="600"
        Height="450" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcDisciplinasMatricula" HeaderText="Disciplinas Ativas"
        AllowDragging="True" EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppDisciplinasMatricula" runat="server">
                <dxp:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnRemoveDisciplinasMatricula">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent3" runat="server">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="2" align="center">
                                        <dxwgv:ASPxGridView ID="grdDisciplinasAtivas" runat="server" EnableRowsCache="false"
                                            DataSourceID="odsDisciplinasAtivas" EnableViewState="false" ClientInstanceName="grdDisciplinasAtivas"
                                            AutoGenerateColumns="False" KeyFieldName="CompositeKey" OnCustomUnboundColumnData="grdDisciplinasAtivas_CustomUnboundColumnData"
                                            Width="95%" Font-Names="Verdana" Font-Size="Small">
                                            <SettingsText EmptyDataRow="Não existem dados." />
                                            <Columns>
                                                <dxwgv:GridViewDataColumn FieldName="disciplina" Caption="Disciplina" VisibleIndex="0"
                                                    Visible="false" />
                                                <dxwgv:GridViewDataColumn FieldName="nome_disciplina" Caption="Disciplina" VisibleIndex="0" />
                                                <dxwgv:GridViewDataColumn FieldName="turma" Caption="Turma" VisibleIndex="1" />
                                                <dxwgv:GridViewDataColumn FieldName="ano" Caption="Ano" VisibleIndex="2" />
                                                <dxwgv:GridViewDataColumn FieldName="semestre" Caption="Período" VisibleIndex="3" />
                                                <dxwgv:GridViewDataColumn FieldName="sit_matricula" Caption="Situação" VisibleIndex="4" />
                                                <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="5" UnboundType="String"
                                                    Visible="False">
                                                </dxwgv:GridViewDataColumn>
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="middle">
                                        <asp:Label ID="Label1" Text="Deseja encerrar? " runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:ImageButton ID="btnRemoveDisciplinasMatricula" SkinID="BcSalvar" runat="server"
                                            OnClick="btnRemoveDisciplinasMatricula_Click" CssClass="cursorImagem" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnCancelaRemovaDisciplinasMatricula" runat="server" SkinID="BcCancelar"
                                            OnClick="btnCancelaRemovaDisciplinasMatricula_Click" CssClass="cursorImagem" />
                                    </td>
                                </tr>
                            </table>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
    <br />
    <dxpc:ASPxPopupControl ID="pcConfirmaDados" runat="server" Modal="True" Width="800"
        CloseAction="CloseButton" Height="450" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ClientInstanceName="pcConfirmaDados" HeaderText="Confirmar dados encerramento"
        AllowDragging="True" EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents PopUp="function(s,e){ OnInitASPxPopupControlSize(s,e,20000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppConfirmaDados" runat="server">
                <asp:Label ID="lblConfMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <br />
                <dxp:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btnSalva">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent2" runat="server">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblConfAnoEncerramento" runat="server" Text="Ano de Encerramento:* "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlConfAnoEncerramento" runat="server" DataValueField="ano"
                                            DataTextField="ano" Width="120px" OnSelectedIndexChanged="ddlConfAnoEncerramento_SelectedIndexChanged"
                                            Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblConfPeriodoEncerramento" runat="server" Text="Período de Encerramento:* "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlConfPeriodoEncerramento" runat="server" DataValueField="periodo"
                                            DataTextField="periodo" Width="120px" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblConfMotivo" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlConfMotivo" runat="server" DataValueField="motivosaida"
                                            AutoPostBack="true" DataTextField="descricao" Width="320px" Enabled="true" OnSelectedIndexChanged="ddlConfMotivo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblConfInstituicao" runat="server"
                                            Text="Instituição:"></asp:Label>
                                    </td>
                                    <td style="width: auto" colspan="3">
                                        <tweb:TSearchBox ID="tseConfInstituicao" AutoPostBack="false" runat="server" Key="outra_faculdade"
                                            Argument="nome_comp" SqlSelect="SELECT outra_faculdade, nome_comp from ly_instituicao"
                                            Caption="" MaxLength="20" GridWidth="850px" Enabled="false">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Instituição" FieldName="outra_faculdade" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblConfCausa" runat="server" Text="Causa: " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlConfCausa" runat="server" DataValueField="causa_encerr"
                                            DataTextField="descricao" Width="400px" Enabled="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblCondDataColacao" runat="server" Text="Data da Colação:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtConfDataColacao" runat="server" ReadOnly="false" MinDate="1901-01-01"
                                            Width="120px" ClientInstanceName="dtConfDataColacao" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" Enabled="false">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" colspan="2">
                                        <asp:Label ID="lblConfDataDiploma" runat="server" Text="Data do Diploma:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtConfDataDiploma" runat="server" ReadOnly="false" MinDate="1901-01-01"
                                            Width="120px" ClientInstanceName="dtConfDataDiploma" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" Enabled="false">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td colspan="2" align="right">
                                        <dxe:ASPxButton ID="btnConfSalvar" runat="server" Text="Salvar" SkinID="BcSalvar"
                                            OnClick="btnConfSalvar_Click" ValidationGroup="SalvarForm" AutoPostBack="true"
                                            ClientSideEvents-Click="function(s,e) { Bloqueio(); } ">
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <dxe:ASPxButton ID="btnConfCancelar" runat="server" SkinID="BcCancelar" Text="Cancelar"
                                            OnClick="btnConfCancelar_Click" AutoPostBack="true">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsDisciplinasAtivas" TypeName="Techne.Lyceum.Net.Academico.EncerramentoAluno"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="tseAluno" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnDados" GroupingText="Dados do Aluno" runat="server">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblUniEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtUniEnsino" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
                    <asp:HiddenField ID="hdnCenso" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSituacao" runat="server" MaxLength="15" Width="600px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurso" runat="server" MaxLength="20" Width="200px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeCurso" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurno" runat="server" MaxLength="20" Width="200px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeTurno" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurriculo" runat="server" Text="Currículo: " Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCurriculo" runat="server" MaxLength="20" Width="600px" ReadOnly="true"
                        Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSerie" runat="server" Text="Ano de Escolaridade:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSerie" runat="server" MaxLength="3" Width="600px" ReadOnly="true" />
                    <asp:TextBox ID="txtCodigoSerie" runat="server" MaxLength="3" Width="600px" ReadOnly="true"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnEncerramentos" runat="server" GroupingText="Encerramentos do Aluno">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncCurso" runat="server" Text="Escolaridade: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEncCurso" runat="server" MaxLength="20" Width="320px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncTurno" runat="server" Text="Turno: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEncTurno" runat="server" MaxLength="20" Width="320px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncCurriculo" runat="server" Text="Currículo: " Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEncCurriculo" runat="server" MaxLength="20" Width="600px" ReadOnly="true"
                        Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncAnoIngresso" runat="server" Text="Ano de Ingresso: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEncAnoIngresso" runat="server" MaxLength="20" Width="120px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncPeriodoIngresso" runat="server" Text="Período de Ingresso: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEncPeriodoIngresso" runat="server" MaxLength="20" Width="120px"
                        ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncDataEncerramento" runat="server" Text="Data de Encerramento do Aluno:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtEncDataEncerramento" runat="server" Width="120px" ClientInstanceName="dtEncDataEncerramento"
                        CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje"
                        ReadOnly="true" Enabled="false">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ErrorMessage="Data de Encerramento: Preenchimento obrigatório para encerrar."
                        ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtEncDataEncerramento"
                        InitialValue="" ValidationGroup="EncerrarForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" 
                        alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncDataReabertura" runat="server" Text="Data de Reabertura:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtEncDataReabertura" runat="server" MinDate="1901-01-01" Width="120px"
                        ClientInstanceName="dtEncDataReabertura" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" ReadOnly="false">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ErrorMessage="Data de Reabertura: Preenchimento obrigatório para reabrir."
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtEncDataReabertura"
                        InitialValue="" ValidationGroup="ReabrirForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" 
                        alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncMotivo" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEncMotivo" runat="server" DataValueField="motivosaida" DataTextField="descricao"
                        Width="400px" OnSelectedIndexChanged="ddlEncMotivo_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lbltseInstituicao" runat="server"
                        Text="Instituição:"></asp:Label>
                </td>
                <td style="width: auto">
                    <tweb:TSearchBox ID="tseEncInstituicao" AutoPostBack="false" runat="server" Key="outra_faculdade"
                        Argument="nome_comp" SqlSelect="SELECT outra_faculdade, nome_comp from ly_instituicao"
                        Caption="" MaxLength="20" GridWidth="850px" Enabled="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Instituição" FieldName="outra_faculdade" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncDataColacao" runat="server" Text="Data da Colação:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtEncDataColacao" runat="server" MinDate="1901-01-01" Width="120px"
                        ClientInstanceName="dtEncDataColacao" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" ReadOnly="true" Enabled="false">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncDataDiploma" runat="server" Text="Data do Diploma:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtEncDataDiploma" runat="server" MinDate="1901-01-01" Width="120px"
                        ClientInstanceName="dtEncDataDiploma" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" ReadOnly="true" Enabled="false">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncAno" runat="server" Text="Ano de Encerramento:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEncAnoEncerramento" runat="server" DataValueField="ano"
                        AutoPostBack="true" DataTextField="ano" Width="120px" OnSelectedIndexChanged="ddlEncAnoEncerramento_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncPeriodoEncerramento" runat="server" Text="Período de Encerramento:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEncPeriodoEncerramento" runat="server" DataValueField="periodo"
                        DataTextField="periodo" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlEncPeriodoEncerramento_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEncCausa" runat="server" Text="Causa: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEncCausa" runat="server" DataValueField="causa_encerr" DataTextField="descricao"
                        Width="400px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlReabertura" runat="server" GroupingText="Informações Reabertura"
                        Visible="false">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblMotivoReabertura" runat="server" Text="Motivo Reabertura:* " Visible="false"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMotivoReabertura" runat="server" Visible="false" DataTextField="descr"
                                        DataValueField="item" Width="400px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblAnoReabertura" runat="server" Text="Ano de Reabertura:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAnoReabertura" runat="server" DataValueField="ano" DataTextField="ano"
                                        Width="120px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAnoReabertura_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblPeriodoReabertura" runat="server" Text="Período de Reabertura:* "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPeriodoReabertura" runat="server" DataValueField="periodo"
                                        AutoPostBack="True" DataTextField="periodo" Width="120px" AppendDataBoundItems="true"
                                        OnSelectedIndexChanged="ddlPeriodoReabertura_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label2" runat="server" Text="Curso:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO
                                                " ArgumentColumns="60" Columns="10" OnChanged="tseCurso_Changed" MaxLength="20"
                                        GridWidth="800px" SqlOrder="nome">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label3" runat="server" Text="Turno:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                                        OnSelectedIndexChanged="cmbTurno_SelectedIndexChanged" AutoPostBack="true" Width="200px"
                                        AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="Série/Ano Escolar:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="serie" DataValueField="serie"
                                        OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged" AutoPostBack="true" Width="200px"
                                        AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Disciplinas Optativas:"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                                        Enabled="false" />
                                    <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                        Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <asp:Panel ID="pnlConfirmacao" runat="server" GroupingText="Existe algum aluno para colocar na vaga que está sendo liberada?(Somente para o Curso/Série/Turno que participa da matrícula fácil)"
                        Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblConfirmacao_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlPermuta" runat="server" GroupingText="Informe a matrícula ou o nome do aluno para colocar na vaga"
                                        Width="650px" Visible="false">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label10" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearch ID="tseAlunoPermuta" runat="server" OnTextChanged="tseAlunoPermuta_Changed"
                                                        SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoTransfTurma" AutoPostBack="true">
                                                        <QueryParameters>
                                                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                                        </QueryParameters>
                                                    </tweb:TSearch>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label11" runat="server" Text="Disciplinas Optativas: "></asp:Label>
                                                </td>
                                                <td colspan="5">
                                                    <asp:CheckBox ID="chkEnsReligiosoPermuta" runat="server" Text="Ensino Religioso"
                                                        Width="140px" Enabled="false" />
                                                    <asp:CheckBox ID="chkLinEstrangeiraPermuta" runat="server" Text="Língua Estrangeira Facultativa"
                                                        Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label12" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMotivoPermuta" runat="server" DataValueField="motivo_transf"
                                                        DataTextField="descricao" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnCurriculoReabertura" runat="server" />
        <table>
            <tr>
                <td>
                    <dxe:ASPxButton ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click"
                        ValidationGroup="EncerrarForm">
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnReabrir" runat="server" Text="Reabrir" OnClick="btnReabrir_Click"
                        ValidationGroup="ReabrirForm">
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
