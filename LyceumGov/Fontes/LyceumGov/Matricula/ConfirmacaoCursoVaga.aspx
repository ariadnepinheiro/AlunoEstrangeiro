<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConfirmacaoCursoVaga.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ConfirmacaoCursoVaga" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 212px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnFiltro" GroupingText="Informe os dados para pesquisar a confirmação de turnos e vagas">
        <div>
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                            Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                            SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                            DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                            ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga,id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                            SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Label ID="lblMensagemPeriodo" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Label ID="lblMensagemFinalizacao" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div id="divEdit" runat="server" class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnFinalizar" runat="server" SkinID="BcNovoFinalizar" OnClick="btnFinalizar_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Confirmação Vagas Continuidade Após Escolha dos Itinerários Formativos"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsVagas" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlOrientaçoes" runat="server" GroupingText="ORIENTAÇÕES" Width="80%">
        <ol>
            <li>Essa tela tem como objetivo servir de base para o Planejamento de Turnos e Vagas,
                sendo o mais fidedigna à realidade da unidade escolar. Neste momento o Diretor escolar
                irá ratificar o quantitativo de alunos com MATRÍCULA ATIVA, que irá cursar a 2ª
                Série do Novo Ensino Médio ou o Módulo III da EJA Novo Ensino Médio no próximo ano
                letivo, que efetuaram a escolha das trilhas formativas (por série/curso),
                permitindo assim a projeção de vagas de continuidade necessárias para a Confirmação
                de Turnos e Vagas. </li>
            <br />
            <li>Nos casos em que parte do quantitativo total de alunos matriculado, que irá cursar
                a 2ª Série do Novo Ensino Médio ou o Módulo III da EJA Novo Ensino Médio,
                não tiver realizado a escolha da trilha formativa o Diretor deverá consultar os
                alunos não optantes. O número de vagas de continuidade deverá ser IGUAL ao nº de
                alunos, mesmo que não haja 100% na escolha da trilha formativa pelos discentes.
            </li>
            <br />
            <li>Havendo a necessidade de ajuste das vagas (Caso não tenham sido alcançados os 100%
                de escolhas dos itinerários) o Diretor deverá fazer a complementação no “Quadro
                de Lançamento de Vagas de Continuidade”. </li>
            <br />
            <li>O Diretor somente conseguirá concluir a confirmação após alcançar 100% do quantitativo
                dos alunos optantes.</li>
            <br />
            <li>O Diretor deverá estar atento à confirmação nesta tela a fim de que TODOS os alunos
                que realizaram suas escolhas de trilha formativa tenham sua vaga garantida durante
                o processo de Confirmação de Turnos e Vagas, e posteriormente no momento da Renovação
                de Matrícula.</li>
            <br />
            <li>Durante a Confirmação de Turnos e Vagas, para os casos de turmas já existentes possuírem
                espaço ocioso, esse poderá ser preenchido por vagas novas, a serem ofertadas durante
                a Matrícula Informatizada.</li>
            <br />
            <i><center><b><u>ATENÇÃO</u></b></center><br />
            <center>O período para a realização desta ação é de <asp:Label ID="lblInicio" runat="server"></asp:Label> a <asp:Label ID="lblFim" runat="server"></asp:Label>. NÂO havendo possibilidade de prorrogação.</center></i>
        </ol>
        <%--<div style="text-align: center">
            <i><u><b>ATENÇÃO</b></u></i>
        </div>
        <br />
        <div style="text-align: center">
            <i>O período para a realização desta ação é , NÃO havendo possibilidade
                de prorrogação.</i>
        </div>--%>
    </asp:Panel>
    <br />
    <div style="width: 50%; text-align: center">
        <asp:Panel ID="pnlResumo" runat="server" Width="30%">
            <table style="border-style: solid; border-spacing: 0; border-width: 0; width: 597px;">
                <tr>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold"
                        colspan="8">
                        RESUMO POR MODALIDADE
                    </td>
                </tr>
                <tr>
                    <td style="border-style: solid; border-width: 1px" class="style1" rowspan="2">
                        &nbsp; &nbsp;
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold"
                        colspan="5">
                        MATRICULADOS SÉRIE ANTERIOR
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold"
                        rowspan="2">
                        OPTANTES
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold"
                        rowspan="2">
                        % &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold">
                        Manhã
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold">
                        Tarde
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold">
                        Noite
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold">
                        Integral
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px; font-weight: bold">
                        Total
                    </td>
                </tr>
                <tr>
                    <td class="style1" style="border-style: solid; border-width: 1px">
                        <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text="ENSINO MÉDIO"></asp:Label>
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEnsMedioM" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSMANHA")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEnsMedioT" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSTARDE")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEnsMedioN" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSNOITE")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEnsMedioI" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSINTEGRAL")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtTotal" runat="server" MaxLength="2" Width="50px" SkinID="numerico"
                            Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtOptEnsMedio" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            SkinID="numerico" Style="text-align: center; background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtPorEnsMedio" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            SkinID="numerico" Style="text-align: center; background: Gainsboro" Width="50px" />
                    </td>
                </tr>
                <tr>
                    <td style="border-style: solid; border-width: 1px" class="style1">
                        <asp:Label ID="Label8" runat="server" Text="EDUCAÇÃO JOVENS ADULTOS" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEJAM" runat="server" MaxLength="2" Width="50px" SkinID="numerico"
                            Text='<%#Eval("MATRICULADOSMANHA")%>' Enabled="false" ReadOnly="true" Style="text-align: center;
                            background: Gainsboro" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEJAT" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSTARDE")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEJAN" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSNOITE")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtMatEJAI" runat="server" Enabled="false" MaxLength="2" ReadOnly="true"
                            Text='<%#Eval("MATRICULADOSINTEGRAL")%>' SkinID="numerico" Style="text-align: center;
                            background: Gainsboro" Width="50px" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtTotalEJA" runat="server" MaxLength="2" Width="50px" SkinID="numerico"
                            Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtOptEJA" runat="server" MaxLength="2" Width="50px" SkinID="numerico"
                            Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro" />
                    </td>
                    <td align="center" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtPorEJA" runat="server" MaxLength="2" Width="50px" SkinID="numerico"
                            Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="center" style="font-weight: bold; font-size: medium">
                                Quadro de Lançamento de Vagas de Continuidade
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <asp:Repeater ID="rpModalidade" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblModalidade" runat="server" Text='<%#Eval("MODALIDADECURSO")%>'
                                            SkinID="lblObrigatorio"></asp:Label>
                                        <asp:HiddenField ID="hdnCursoReferencia" runat="server" />
                                        <asp:HiddenField ID="hdnSerieReferencia" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 800px">
                                                    <asp:Label ID="Label7" runat="server" Text="Itinerário Formativo" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label5" runat="server" Text="Série" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label29" runat="server" Text="Manhã" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label3" runat="server" Text="Tarde" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label4" runat="server" Text="Noite" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label28" runat="server" Text="Integral" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label6" runat="server" Text="Optantes" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                            </tr>
                                            <asp:Repeater ID="rpVagas" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCurso" runat="server" Text='<%#Eval("CURSO")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("NOMECURSO")%>'></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblSerie" runat="server" Text='<%#Eval("SERIE")%>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:TextBox ID="txtManha" Text='<%#Eval("VAGASMANHA")%>' runat="server" MaxLength="3"
                                                                Width="50px" SkinID="numerico" Style="text-align: center" Enabled='<%#Eval("HABILITAMANHA")%>' />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:TextBox ID="txtTarde" Text='<%#Eval("VAGASTARDE")%>' runat="server" MaxLength="3"
                                                                Width="50px" SkinID="numerico" Style="text-align: center" Enabled='<%#Eval("HABILITATARDE")%>' />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:TextBox ID="txtNoite" Text='<%#Eval("VAGASNOITE")%>' runat="server" MaxLength="3"
                                                                Width="50px" SkinID="numerico" Style="text-align: center" Enabled='<%#Eval("HABILITANOITE")%>' />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:TextBox ID="txtIntegral" Text='<%#Eval("VAGASINTEGRAL")%>' runat="server" MaxLength="3"
                                                                Width="50px" SkinID="numerico" Style="text-align: center" Enabled='<%#Eval("HABILITAINTEGRAL")%>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOptantes" runat="server" MaxLength="3" Width="50px" SkinID="numerico"
                                                                Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro"
                                                                Text='<%#Eval("QUANTIDADEOPTANTES")%>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Vagas lançadas
                                        <asp:Label ID="lblPorcentagem" runat="server" SkinID="lblObrigatorio"> </asp:Label>
                                        %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
