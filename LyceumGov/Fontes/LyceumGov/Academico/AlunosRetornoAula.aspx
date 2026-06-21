<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AlunosRetornoAula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AlunosRetornoAula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueioMatricula");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

   <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="dvbloqueioMatricula" class="Desbloqueado">
            </div>
            <asp:Panel ID="Panel2" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
                Width="800px">
                <table>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                runat="server" Text="Unidade de Ensino:*"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga,id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                                SqlWhere=" situacao = 'ESTADUAL' AND SITUACAO_FUNCIONAMENTO = 'EMATIVIDADE'" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                                SqlOrder="nome_comp">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                                Width="70px" AutoPostBack="True" AppendDataBoundItems="true" onchange="Bloqueio()" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                                Width="70px" AutoPostBack="True" AppendDataBoundItems="true" onchange="Bloqueio()" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="Label4" runat="server" Text="Data da Aula:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="dtAula" runat="server" MinDate="1901-01-01" Width="120px" EnableDefaultAppearance="true"
                                ClientInstanceName="dtAula" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje"
                                ClientEnabled="True" OnValueChanged="dtAula_ValueChanged" AutoPostBack="true"   >
                               
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            <asp:Label ID="Label3" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTurno" AutoPostBack="True" runat="server" DataTextField="descricao" 
                                DataValueField="turno" AppendDataBoundItems="true" onchange="Bloqueio()" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <asp:HiddenField ID="hdnErro" runat="server" />
            <br />
            <div id="divEdit" runat="server" class="divEditBlock" style="width: 800px;">
                <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click" OnClientClick="Bloqueio()"
                    ValidationGroup="SalvarForm" />
                <asp:Label runat="server" ID="lblBloco" Text="Acompanhamento Controle de Frequência - Retorno Presencial"
                    SkinID="BcTitulo" />
                <asp:ValidationSummary ID="vsEletivas" runat="server" EnableClientScript="true" ShowMessageBox="true"
                    ValidationGroup="SalvarForm" ShowSummary="false" />
            </div>
            <br />
            <div id="dvTurmas" runat="server" visible="false">
                <asp:Label ID="Label18" runat="server" Text="*" SkinID="lblMensagem"></asp:Label>
                <asp:Label ID="Label8" runat="server" Text="PORTARIA SEEDUC/SUGEN Nº 419 DE 27 DE SETEMBRO DE 2013. Art. 46 - Em qualquer nível/etapa de ensino, é assegurado ao educando que apresentar <br /> impedimento de frequência, amparado por legislação específica (enfermos, gestantes, militares e outros), o direito a tratamento especial, como forma alternativa <br /> de cumprimento da carga horária e das avaliações que atendam os mínimos exigidos para promoção."></asp:Label>
                <br />
                <asp:Label ID="Label11" runat="server" Text="**" SkinID="lblMensagem"></asp:Label>
                <asp:Label ID="Label81" runat="server" Text="Consideramos casos de afastamento por COVID-19: </br> a) caso detectado  </br> b) cumprimento de protocolo sanitário, conforme legislação vigente(Decreto Estadual N°47.801 de 19/10/2021)."></asp:Label>
                <br />
                <br />
                <table>
                    <asp:Repeater ID="rpTurmas" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTurma" runat="server" Text="Turma" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label2" runat="server" Text="Matriculados" SkinID="lblObrigatorio"
                                        Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label3" runat="server" Text="Presentes" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label9" runat="server" Text="Afastamento Amparo </br> Portaria 419"
                                        SkinID="lblObrigatorio" Style="text-align: center"></asp:Label><asp:Label ID="Label18"
                                            runat="server" Text="*" SkinID="lblMensagem"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label10" runat="server" Text="Afastamento </br> COVID-19" SkinID="lblObrigatorio"
                                        Style="text-align: center"></asp:Label><asp:Label ID="Label12" runat="server" Text="**"
                                            SkinID="lblMensagem"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label7" runat="server" Text="Ausentes" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label4" runat="server" Text="% Presentes" SkinID="lblObrigatorio"
                                        Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label1" runat="server" Text="% Ausentes" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label5" runat="server" Text="% Casos Amparo</br>Portaria 419" SkinID="lblObrigatorio"
                                        Style="text-align: center"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label6" runat="server" Text="% Afastamento </br> COVID-19" SkinID="lblObrigatorio"
                                        Style="text-align: center"></asp:Label>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTurma" runat="server" Text='<%#Eval("TURMA")%>' SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMatriculados" runat="server" MaxLength="2" Width="60px" SkinID="numerico"
                                        Enabled="false" ReadOnly="true" Style="text-align: center; background: Gainsboro" />
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txtPresente" runat="server" MaxLength="2" Width="60px" SkinID="numerico"
                                        Style="text-align: center" />
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txtCasosAmparo" runat="server" MaxLength="2" Width="60px" SkinID="numerico"
                                        Style="text-align: center; background-color: #ffff0063" />
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txtCovid" runat="server" MaxLength="2" Width="60px" SkinID="numerico"
                                        Style="text-align: center; background-color: #ffff0063" />
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txtAusente" runat="server" MaxLength="2" Width="60px" SkinID="numerico"
                                        Style="text-align: center; background: Gainsboro" Enabled="false" ReadOnly="true" />
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblFrequencia" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblInfrequencia" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblAmparo" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblCovid" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
