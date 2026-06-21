<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RegistroConfirmacaoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.RegistroConfirmacaoMatricula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="700px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfirmacaoMatricula"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAno_Ingresso" runat="server"
                        Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSem_Ingresso" runat="server"
                        Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" AutoPostBack="true"
                        Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Label ID="lblMensagemAluno" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnCurriculo" runat="server" />
    <br />
    <asp:Panel ID="pnlGeral" runat="server" Visible="false">
        <asp:Panel ID="pnlDados" GroupingText="Dados da Confirmação" runat="server" Width="700px">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNivel" runat="server" Text="Nível/Segmento*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblModalidade" runat="server" Text="Modalidade*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlModalidade_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCurso" runat="server"
                            Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome, pc.curso as pccurso FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO
                                                left join LY_EVENTO_GERAL pc on pc.CURSO = uec.CURSO and pc.TIPO_FILTRO = 'Bloqueio_Cadastro_Aluno' and CONVERT(date,GetDate()) between pc.DT_INICIO and DT_FIM"
                            ArgumentColumns="60" Columns="10" OnChanged="tseCurso_Changed" MaxLength="20"
                            GridWidth="800px" SqlOrder="nome" SqlWhere="pc.curso is null">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                            Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoCurso" runat="server" Visible="false">
                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Concomitante" Value="Concomitante"></asp:ListItem>
                            <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTurno" runat="server"
                            Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                            OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged" AutoPostBack="true" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerie" runat="server"
                            Text="Série/Ano Escolar:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="SERIE" DataValueField="serie"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
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
            <br />
            <table>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfirmar" runat="server" Text="Confirma" OnClick="btnConfirmar_Click"
                            OnClientClick="Bloqueio()" />
                    </td>
                    <td align="left">
                        <asp:Button ID="btnLimpar" runat="server" Text="Limpar Campos" OnClick="btnLimpar_Click" />
                    </td>
                    <td align="left">
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ClientInstanceName="grdConfirmacao" ID="grdConfirmacao" runat="server"
                        AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"
                        Visible="false" EnableCallBacks="false">
                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
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
    </asp:Panel>
</asp:Content>
