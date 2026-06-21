<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistorioEscolarAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Consulta.HistorioEscolarAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Aluno" Width="80%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="Label1" SkinID="lblObrigatorio" Text="Aluno*:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnDadosAluno" runat="server" Visible="false" Width="80%">
        <div>
            <table style="width: 100%;">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Image ID="Image1" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                                        ImageUrl="~/Images/logo.gif" Style="text-align: right" />
                                </td>
                                <td align="center" style="font-size: 13px">
                                    GOVERNO DO ESTADO DO RIO DE JANEIRO - SECRETARIA DE EDUCAÇÃO<br />
                                    <strong>CONSULTA DE HISTÓRICO ESCOLAR DO ALUNO</strong>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>DADOS DO ALUNO</strong></div>
            <hr id="tDadosPessoais" style="margin: 1px" />
            <asp:Panel ID="pnAluno" runat="server" Width="100%">
                <table>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Matricula" Text="Mastrícula:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblMatricula"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Nome" Text="Nome do aluno:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblNome"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Mae" Text="Nome da Mãe:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblMae"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="DataNascimento" Text="Data Nascimento:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblDataNascimento"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Status" Text="Status Matrícula:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblStatus"></asp:Label></strong>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div>
                <strong>DADOS DO HISTORICO ESCOLAR</strong></div>
            <hr id="Hr1" style="margin: 1px" />
            <asp:Panel ID="pnHistorico" runat="server" Visible="false" Width="100%">
                <br />
                <dxwgv:ASPxGridView ID="grdHistorico" runat="server" AutoGenerateColumns="False"
                    Width="100%" Visible="true" ClientInstanceName="grdHistorico" DataSourceID="odsHistorio"
                    KeyFieldName="ID_SITUACAO_FINAL_ALUNO" OnCustomButtonCallback="grdHistorico_CustomButtonCallback"
                    EnableCallBacks="false">
                    <SettingsBehavior ConfirmDelete="True" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                            <ClearFilterButton Text="Limpar" Visible="True">
                                <Image Url="~/img/bt_limpa.png" />
                            </ClearFilterButton>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Id" FieldName="ID_SITUACAO_FINAL_ALUNO" VisibleIndex="0"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" VisibleIndex="0"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="1"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="MUNICIPIO" VisibleIndex="2"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="3"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Unidade" FieldName="ESCOLA" VisibleIndex="4"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="5" Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="6"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="7"
                            Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Situação Final" FieldName="SITUACAO_FINAL"
                            VisibleIndex="8" Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Frêquencia Global" FieldName="FREQUENCIA_GLOBAL"
                            VisibleIndex="9" Visible="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewCommandColumn VisibleIndex="10" ButtonType="Link" Width="50px" Caption="Disciplinas"
                            Name="Avaliar">
                            <CustomButtons>
                                <dxwgv:GridViewCommandColumnCustomButton ID="btnDisciplinas" Text="Ver disciplinas"
                                    Image-Width="50px" Visibility="AllDataRows">
                                </dxwgv:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxwgv:GridViewCommandColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                </dxwgv:ASPxGridView>
                <asp:ObjectDataSource ID="odsHistorio" TypeName="Techne.Lyceum.Net.Consulta.HistorioEscolarAluno"
                    runat="server" SelectMethod="ListaTurmasHistorico">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:Panel>
            <asp:Label ID="lblAviso" runat="server" SkinID="lblMensagem" ClientInstanceName="lblAviso"></asp:Label>
        </div>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucDisciplinas" ClientInstanceName="pucDisciplinas" runat="server"
        Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="400px">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <dxwgv:ASPxGridView ID="grdDisciplina" runat="server" AutoGenerateColumns="False"
                                EnableCallBacks="false" DataSourceID="" ClientInstanceName="grdDisciplina" KeyFieldName="DISCIPLINA"
                                Width="450px">
                                <SettingsPager AlwaysShowPager="false" Mode="ShowAllRecords">
                                </SettingsPager>
                                <SettingsBehavior AllowFocusedRow="false" AutoExpandAllGroups="true" AllowGroup="false"
                                    AllowSort="false" AllowDragDrop="false" />
                                <Columns>
                                    <dxwgv:GridViewDataColumn Caption="Disciplina" FieldName="DISCIPLINANOME" VisibleIndex="1">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="Rendimento" FieldName="SITUACAO_HIST" VisibleIndex="2">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="% Frequência" FieldName="PERC_PRESENCA" VisibleIndex="3">
                                    </dxwgv:GridViewDataColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucDisciplinas.Hide();" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
