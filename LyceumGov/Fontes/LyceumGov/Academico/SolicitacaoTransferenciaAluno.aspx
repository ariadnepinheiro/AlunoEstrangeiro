<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SolicitacaoTransferenciaAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.SolicitacaoTransferenciaAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<asp:HiddenField ID="hdnCompartilhada" runat="server" />
<asp:HiddenField ID="hdnSerieCompartilhada" runat="server" />

    <dxtc:ASPxPageControl ID="pcTransferencia" runat="server" ActiveTabIndex="0" Width="800px"
        OnTabClick="pcTransferencia_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Solicitação de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:HiddenField runat="server" ID="hdnPeriodoOrigem" />
                        <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização Atual do Aluno:"
                            Width="800px">
                            <table>
                                <tr>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from municipio m "
                                            GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                                            SqlWhere="uf_sigla='RJ'" MaxLength="10">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, nucleo,municipio from LY_UNIDADE_ENSINO "
                                            SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                                            SqlOrder="nome_comp">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                               
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                                            ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoTransf"
                                            AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                            <QueryParameters>
                                                <asp:ControlParameter ControlID="tseUnidadeResponsavel" Name="unidade_ens" PropertyName="DBValue" />
                                            </QueryParameters>
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Dados da Transferência:">
                            <table>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                                            Text="Unidade de Ensino de destino:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUnidadeEnsinoDestino" runat="server" Caption="" Key="unidade_ens"
                                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect="select distinct ue.UNIDADE_ENS,ue.NOME_COMP,uas.UNIDADE_FIS FROM VW_UNIDADE_ENSINO_SITUACAO ue INNER JOIN LY_UNIDADES_ASSOCIADAS uas ON uas.UNIDADE_ENS = ue.UNIDADE_ENS"
                                            GridWidth="850px" SqlOrder="nome_comp" OnChanged="tseUnidadeEnsinoDestino_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Unidade Fisica" FieldName="UNIDADE_FIS" Width="30%"
                                                    Visible="false" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tseUnidadeEnsinoDestino"
                                            ErrorMessage="Unidade de Ensino de Destino: Preenchimento obrigatório." InitialValue=""
                                            ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="lblAno" runat="server" Text="Ano Letivo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbAno" runat="server" DataTextField="ano" DataValueField="ano"
                                            Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbAno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="cmbAno"
                                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblPeriodo" runat="server" SkinID="lblObrigatorio" Text="Período Letivo:*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbPeriodo" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                            DataTextField="periodo" DataValueField="periodo" OnSelectedIndexChanged="cmbPeriodo_SelectedIndexChanged"
                                            Width="70px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="Label5" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct c.curso as curso, nome,mc.DESCRICAO AS modalidade,tc.DESCRICAO AS segmento FROM LY_CURSO C INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO      "
                                            ArgumentColumns="60" Key="curso" Columns="10" OnChanged="tseCurso_Changed" SqlOrder="nome"
                                            MaxLength="20" GridWidth="800px">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="25%" />
                                                <tweb:TSearchBoxColumn Caption="Segmento" FieldName="segmento" Width="25%" />
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="40%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblTurno" runat="server" SkinID="lblObrigatorio" Text="Turno:* "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbTurno" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                            DataTextField="descricao" DataValueField="turno" OnSelectedIndexChanged="cmbTurno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                                            SkinID="lblObrigatorio" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbTipoCurso" runat="server" Visible="false">
                                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Concomitante" Value="Concomitante"></asp:ListItem>
                                            <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text="Série/Ano:* "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbSerie" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                            DataTextField="serie" DataValueField="serie" OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="Label3" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbTurma" AutoPostBack="True" runat="server" DataTextField="turma"
                                            DataValueField="turma" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbTurma_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="Label4" runat="server" SkinID="lblObrigatorio" Text="Motivo:* "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="cmbMotivoTransf" runat="server" AppendDataBoundItems="true"
                                            AutoPostBack="True" DataTextField="descr" DataValueField="descr">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right;">
                                        <asp:Label ID="Label8" runat="server" Text="Disciplinas Optativas: " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                                            Enabled="false" />
                                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                            Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkAtesto" runat="server" Text="Atesto que recebi a declaração de transferência do aluno*"
                                            SkinID="lblObrigatorio" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: right;">
                                        <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Solicitar Transferência"
                                            OnClick="btnSalvar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <asp:Label ID="lblMensagemBloqueio" runat="server" SkinID="lblMensagem"></asp:Label>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Acompanhamento de Solicitações">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
