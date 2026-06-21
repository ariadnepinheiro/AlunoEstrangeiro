<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CadHistoricoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.CadHistoricoMatricula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(s) {            
            if (s.cpAtualizar != undefined) {
                if (s.cpAtualizar != null) {
                    $("#<%= this.lblSituacaoFinal.ClientID %>").text(s.cpAtualizar);
                    s.cpAtualizar = null;
                }
                if (s.cpAtualizarFreq != null) {
                    $("#<%= this.lblFrequenciaGlobal.ClientID %>").text(s.cpAtualizarFreq);
                    s.cpAtualizarFreq = null;
                }
            }
        }
    </script>

    <style type="text/css">
        textarea
        {
            resize: none;
        }
    </style>
    <asp:HiddenField ID="hdnSerieConcluinte" runat="server" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Aluno, Ano, Período e  Turma"
        Width="700px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        GridWidth="800" AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="true" Width="115px" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                        DataValueField="ano" DataTextField="ano" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" AutoPostBack="true" Width="65px"
                        OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" DataValueField="SEMESTRE"
                        DataTextField="SEMESTRE" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTurma" runat="server" AutoPostBack="true" Width="115px"
                        OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged" DataValueField="turma"
                        DataTextField="turma" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblTextoSitFinal" runat="server" Text="Situação Final do Aluno:" SkinID="lblObrigatorio"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSituacaoFinal" runat="server" SkinID="lblObrigatorio" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTextoFreq" runat="server" Text="Frequência Global:" SkinID="lblObrigatorio"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFrequenciaGlobal" runat="server" SkinID="lblObrigatorio" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSituacao" runat="server" Visible="false">
                        <asp:ListItem Text="Aprovado" Value="Aprovado"> </asp:ListItem>
                        <asp:ListItem Text="Promovido com continuidade curricular" Value="Promovido"> </asp:ListItem>
                        <asp:ListItem Text="Retido" Value="Retido"> </asp:ListItem>
                        <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnSalvarSituacao" runat="server" Text="Alterar Situação" Visible="false"
                        OnClick="btnSalvarSituacao_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblInformativo" runat="server" SkinID="lblObrigatorio" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsCadHistMatricula" runat="server" SelectMethod="ListarHist"
        TypeName="Techne.Lyceum.Net.Academico.CadHistoricoMatricula" UpdateMethod="Update"
        OnUpdating="odsHistMatricula_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" DefaultValue="" Name="aluno" PropertyName="Text" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlTurma" DefaultValue="" Name="turma" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnGridHistorico" runat="server" Visible="true">
        <dxwgv:ASPxGridView ID="grdHistorico" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdHistorico" Visible="False" DataSourceID="odsCadHistMatricula"
            KeyFieldName="aluno;ordem;ano;semestre;disciplina" OnAfterPerformCallback="grdHistorico_AfterPerformCallback"
            OnCellEditorInitialize="grdHistorico_CellEditorInitialize" OnInitNewRow="grdHistorico_InitNewRow"
            OnStartRowEditing="grdHistorico_StartRowEditing" OnRowValidating="grdHistorico_RowValidating"
            OnRowUpdating="grdHistorico_RowUpdating" OnRowUpdated="grdHistorico_OnRowUpdated"
            OnCommandButtonInitialize="grdHistorico_CommandButtonInitialize" EnableCallBacks="true">
            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
            <SettingsEditing Mode="Inline" />
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                    <EditButton Text="Editar" Visible="True">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <UpdateButton Text="Salvar">
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn FieldName="aluno" Caption="Aluno" VisibleIndex="1"
                    Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="ordem" Caption="Ordem" VisibleIndex="2"
                    Width="100px" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="unidade_ensino" Caption="Censo" VisibleIndex="3"
                    Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="nome_comp03" Caption="Instituição" VisibleIndex="4"
                    Width="100px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="outras" Caption="Outra Instituição" VisibleIndex="5"
                    Width="100px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="disciplina" Caption="Cód. Disciplina" VisibleIndex="6"
                    Width="100px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="nomedisciplina" Caption="Disciplina" VisibleIndex="7"
                    Width="200px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="turma" Caption="Turma" VisibleIndex="8"
                    ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano de Escolaridade" FieldName="serie" VisibleIndex="9"
                    HeaderStyle-Font-Bold="true" Width="100px" ReadOnly="true">
                    <PropertiesTextEdit MaxLength="3" Width="100px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RegularExpression ErrorText="Ano de escolaridade deve ser um número com até 3 dígitos."
                                ValidationExpression="\d{1,3}" />
                            <RequiredField ErrorText="Favor informar o ano de escolaridade." IsRequired="true" />
                        </ValidationSettings>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="nota_final" Caption="Total de Pontos" VisibleIndex="10">
                    <PropertiesTextEdit MaxLength="6" DisplayFormatString="000.00">
                        <MaskSettings Mask="<0..99>,<0..9>" ErrorText="Total de Pontos deve ter no máximo 2 dígitos inteiros e um decimal." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Situação da Disciplina" FieldName="situacao_hist"
                    VisibleIndex="11" Width="110px">
                    <PropertiesComboBox ValueType="System.String" Width="110px">
                        <Items>
                        
                            <dxe:ListEditItem Text="Aprovado" Value="APROVADO" />
                            <dxe:ListEditItem Text="Aprovado por Conselho de Classe" Value="APROVADO CONSELHO" />
                            <dxe:ListEditItem Text="Cancelado" Value="CANCELADO" />
                            <dxe:ListEditItem Text="Reprovado por Nota" Value="REP NOTA" />
                            <dxe:ListEditItem Text="Reprovado por Frequência" Value="REP FREQ" />
                            <dxe:ListEditItem Text="Promovido com continuidade curricular" Value="Promovido" />
                            <dxe:ListEditItem Text="Retido" Value="Retido" />
                        </Items>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar a situação final." IsRequired="true" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="observacao" Caption="Observação" VisibleIndex="12"
                    Width="200px">
                    <PropertiesMemoEdit Height="30px" Width="200px" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataTextColumn FieldName="falta_final" Caption="Total Falta" VisibleIndex="13">
                    <PropertiesTextEdit MaxLength="3" Width="100px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RegularExpression ErrorText="Total de falta deve ser um número com até 3 dígitos."
                                ValidationExpression="\d{1,3}" />
                            <RequiredField ErrorText="Favor informar o total de faltas." IsRequired="true" />
                        </ValidationSettings>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="aulas_dadas" Caption="Aulas dadas" VisibleIndex="14">
                    <PropertiesTextEdit MaxLength="3" Width="100px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RegularExpression ErrorText="Aulas dadas deve ser um número com até 3 dígitos."
                                ValidationExpression="\d{1,3}" />
                            <RequiredField ErrorText="Favor informar as aulas dadas." IsRequired="true" />
                        </ValidationSettings>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DEPENDENCIA" Caption="Observação" VisibleIndex="15"
                    ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="SERIE_REFERENCIA" Caption="Série Referência"
                    VisibleIndex="16" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DISCIPLINA_REFERENCIA" Caption="Disciplina Referência"
                    VisibleIndex="17" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="creditos" Caption="Créditos" VisibleIndex="18"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="nivel_presenca" Caption="Nível presença"
                    VisibleIndex="19" Visible="false">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
