<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="CandidatoMigracao.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoMigracao" %>

<%@ Import Namespace="Techne.Lyceum.RN.ContratoTemporario" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="cCandidatoDocente" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../scripts/processoseletivo/funcoespopup.js"></script>

    <script type="text/javascript">

        function msgBox(sMessage) {
            alert(sMessage);
            window.location = 'CandidatoMigracao.aspx';
        }

        function Validamsg(sMessage) {
            alert(sMessage);

        }
        function FinalizarCadastro() {

            return false;
        }

        function SomenteNumeros(oEvent) {
            var keycode = (oEvent.which) ? oEvent.which : oEvent.keyCode;

            if ((keycode >= 48 && keycode <= 57) || (keycode == 8))
                return (true && (keycode != 46));

            return false;
        }

        function SomenteNumerosLetras(event) {
            var charCode = (event.which) ? event.which : event.keyCode

            if (charCode == 8)
                return true;

            if (charCode > 47 && charCode < 58)
                return true;

            if (charCode > 65 && charCode < 90)
                return true;

            if (charCode > 97 && charCode < 122)
                return true;

            return false;
        }

     
    </script>

    <asp:Panel ID="pnBusca" GroupingText="Faça uma busca por processo seletivo e número de inscrição"
        runat="server" Width="850px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBuscaConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao, indigena, ano FROM lY_concurso_docente"
                        ArgumentColumns="50" Columns="30" MaxLength="20" SqlWhere="TIPO = 'Migracao' "
                        SqlOrder=" ano desc" GridWidth="800px" OnChanged="tseConcursoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                            <tweb:TSearchBoxColumn Caption="Indigena" FieldName="indigena" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBuscaCandidato" runat="server" Text="Candidato:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidatoBusca" runat="server" Caption="" Key="idvinculo"
                        Argument="nome" SqlSelect="SELECT concurso,candidato FROM vw_ly_docente_candidato"
                        ArgumentColumns="50" Columns="30" MaxLength="20" SqlOrder="nome" GridWidth="800px"
                        SqlWhere=" concurso = #tseConcursoBusca# " OnChanged="tseCandidatoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID/Vinculo" FieldName="idvinculo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:HiddenField ID="hdnNumFunc" runat="server" />
    <asp:HiddenField ID="hdnSituacao" runat="server" />
    <asp:HiddenField ID="hdnDataConvocacao" runat="server" />
    <asp:Label ID="lblMensagem" runat="server" Text="" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right"
            Visible="false" />
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnEditarData" runat="server" SkinID="BcEditarDatas" OnClick="btnEditarData_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBlocoCandidatoDocente" Text="Ficha de Inscrição"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsCandidatoDocente" runat="server" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoMigracao"
        SelectMethod="ListaDocumento">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="candidato" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlDocenteTSearch" runat="server" GroupingText="Informe o Id/Vínculo ou o nome do docente"
        Width="700px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="Id/Vínculo do Docente:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente"
                        AutoPostBack="true" OnTextChanged="tseDocente_Changed" MaxLength="20">
                    </tweb:TSearch>
                    <asp:TextBox ID="txtPessoaHidden" Visible="false" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <dxtc:ASPxPageControl ID="pcCandidatoDocente" runat="server" ClientInstanceName="pcCandidatoDocente"
        ActiveTabIndex="0" Width="980px">
        <TabPages>
            <dxtc:TabPage Text="Dados da Inscrição">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDadosInscricao" runat="server">
                        <br />
                        <asp:Panel runat="server" ID="pnlNumeroInscricao" GroupingText="Número de Inscrição/Situação"
                            Visible="false">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCandidato" runat="server" Text="Número de Inscrição: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCandidato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hdnDocenteCandidatoId" runat="server" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Text="Status: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStatusCandidato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="Panel6" GroupingText="Lotação Atual">
                            <table>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label runat="server" ID="Label2" Text="Regional/Sede:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLotacaoRegionalSede" runat="server" Width="200px" ReadOnly="true"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hdnRegionalLotacao" runat="server" />
                                        <asp:HiddenField ID="hdnSedeLotacao" runat="server" />
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label runat="server" ID="Label5" Text="Municipio:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLotacaoMunicipio" runat="server" Width="200px" ReadOnly="true"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hdnMunicipioLotacao" runat="server" />
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label runat="server" ID="Label6" Text="Disciplina de Ingresso:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLotacaoDisciplinaIngresso" runat="server" Width="200px" ReadOnly="true"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hdnDiscIngresso" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="pnlRegionalDesejadaSede" GroupingText="Regional Desejada">
                            <table>
                                <td>
                                 <asp:DropDownList ID="ddlRegionaldesejada" runat="server" DataTextField="regional" DataValueField="id_regional"
                                            Width="200px" Height="20px" >
                                        </asp:DropDownList>
                                </td>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="pnlDadosPessoais" GroupingText="Dados Pessoais">
                            <table>
                                <tr>
                                    <td align="right" style="width: 150px">
                                        <asp:Label ID="lblNomeCompleto" runat="server" Text="Nome Completo:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeCompleto" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="300px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDataNasc" runat="server" Text="Data Nascimento:"></asp:Label>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="120px"
                                                        Enabled="false" ReadOnly="true" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNomeMae" runat="server" Text="Nome da Mãe:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="200px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNomePai" runat="server" Text="Nome do Pai:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="200px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="200px" Height="20px" ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNaturalidade" runat="server" Text="Naturalidade:"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseNaturalidade" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                            Columns="10" ArgumentColumns="30" MaxLength="10" OnChanged="tseNaturalidade_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:TextBox ID="txtNaturalidadeNasc" runat="server" MaxLength="20" Width="250px"
                                            Visible="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNaturalidadeUF" Visible="true" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNaturalidadeUF" runat="server" Width="150px" Height="20px" ReadOnly="true"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="Panel1" GroupingText="Endereço">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" ReadOnly="true"
                                            Enabled="false" AutoPostBack="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                            Text="Município:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" ReadOnly="true" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" Visible="true" runat="server" Text="Estado:"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" visible="true" runat="server" maxlength="20" class="txtInput"
                                            readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                            Text="Endereço:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                                        Width="400px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                            Text="N.º:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                            Text="Compl.:" ReadOnly="true" Enabled="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndCompl" ReadOnly="true" Enabled="false" runat="server" MaxLength="50"
                                            onkeypress="return endereco(event);" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                            Text="Bairro:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                            Width="400px" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="Panel2" GroupingText="Contato">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFone" onkeyup="return formataTelefoneDDD(this,event);" runat="server"
                                            onpaste="return false;" Width="100px" MaxLength="13" ReadOnly="true" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular" runat="server"
                                            Text="Celular:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"
                                            onpaste="return false;" Width="100px" MaxLength="14" ReadOnly="true" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                                            Text="E-mail:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="600px" MaxLength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label1" runat="server" Text="E-mail institucional:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailInstitucional" runat="server" Width="600px" ReadOnly="true"
                                            Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="pnlDocumentos" GroupingText="Documentos Pessoais">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                                            Text="Rg Número:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="15" Width="200px" SkinID="numeroDocumento"
                                            OnKeyPress="return SomenteNumerosLetras(event);" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
                                            Text="Rg Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                                            Width="150px" Height="20px" ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
                                            Text="Rg Órgão Emissor:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                                            Width="200px" Height="20px" ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
                                            Text="Rg Data de Expedição:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" ReadOnly="true" Enabled="false">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="50"
                                            Width="150px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPisPasep" runat="server" Text="PIS / PASEP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPisPasep" runat="server" MaxLength="11" Width="150px" SkinID="numerico"
                                            ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="Panel8" runat="server" GroupingText="Título de Eleitor">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblEleitorNum" Text="Número: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOC_Teleitor_Num" SkinID="numerico" runat="server" MaxLength="15"
                                                ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblTeleitorZona" Text="Zona: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOC_Teleitor_Zona" runat="server" MaxLength="15" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblEleitorSecao" Text="Seção: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOC_Teleitor_Secao" runat="server" MaxLength="15" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblEleitorUF" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEleitor_Uf" runat="server" DataTextField="sigla" DataValueField="sigla"
                                                Width="120px" Height="20px" ReadOnly="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel9" runat="server" GroupingText="Habilitação">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="Label3" Text="Número CNH: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumCNH" SkinID="numerico" runat="server" MaxLength="15" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="Label4" Text="Categoria: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCategoriaCNH" runat="server" Width="120px" Height="20px"
                                                ReadOnly="true" Enabled="false">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Selecione" Value="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="A">A</asp:ListItem>
                                                <asp:ListItem Value="B">B</asp:ListItem>
                                                <asp:ListItem Value="C">C</asp:ListItem>
                                                <asp:ListItem Value="D">D</asp:ListItem>
                                                <asp:ListItem Value="E">E</asp:ListItem>
                                                <asp:ListItem Value="AB">AB</asp:ListItem>
                                                <asp:ListItem Value="AB">AC</asp:ListItem>
                                                <asp:ListItem Value="AB">AD</asp:ListItem>
                                                <asp:ListItem Value="AB">AE</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="Label14" Text="Data de Validade: "></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtValidadeCNH" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                ReadOnly="true" Enabled="false" CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="Label13" Text="UF: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlUFCNH" runat="server" DataTextField="sigla" DataValueField="sigla"
                                                Width="120px" Height="20px" ReadOnly="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnCarteiraProfissional" runat="server" GroupingText="Carteira Profissional">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCprofNum" Text="Número:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCrpof_Num" runat="server" MaxLength="15" Width="120px" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblProfSerie" Text="Série:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCprof_Serie" runat="server" MaxLength="15" Width="120px" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCProf_UF" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddDlCprof_Uf" runat="server" DataTextField="sigla" DataValueField="sigla"
                                                Width="120px" Height="20px" ReadOnly="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnDocumentosMilitares_CertifReservista" runat="server" GroupingText="Certificado de Reservista">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrNum" Text="Número: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_Num" runat="server" MaxLength="17" SkinID="numerico"
                                                ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrSerie" Text="Série: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_Serie" runat="server" MaxLength="15" ReadOnly="true"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="Label8" Text="Estado:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCrUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                                                Width="120px" Height="20px" ReadOnly="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Titulações / Experiências" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccTitulacaoExperiencia" runat="server">
                        <asp:Panel ID="Panel3" runat="server" GroupingText="Titulação - Formação Acadêmica">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblTitulacao" runat="server" RepeatDirection="Vertical">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="Panel4" runat="server" GroupingText="Experiência exclusivamente em sala de aula nas escolas estaduais da SEEDUC/RJ">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblExperiencia" runat="server" RepeatDirection="Vertical">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlFuncaoDiretor" runat="server" GroupingText="Exercício de função - Diretor Geral ou Diretor Adjunto">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblFuncaoDiretor" runat="server" RepeatDirection="Horizontal"
                                            DataValueField="FuncaoDiretor" Width="150px">
                                            <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Dados Migração" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccLotacao" runat="server">
                        <asp:Panel ID="Panel12" runat="server" GroupingText="Dependentes">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDependentes" OnKeyPress="return SomenteNumeros(event);" runat="server"
                                            MaxLength="2"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="Panel11" runat="server" GroupingText="Possui Acumulação de Cargos públicos regularizados no Diário Oficial">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblAcumulacao" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlTempoGLP" runat="server" GroupingText="Quantidade de anos de atuação no regime de ampliação da jornada de trabalho por meio da Gratificação por Lotação Prioritária(GLP)">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtGLP" OnKeyPress="return SomenteNumeros(event);" runat="server"
                                            MaxLength="15" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlAnosGLP" runat="server" GroupingText="Gratificação por Lotação Prioritária (GLP)">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chlAnosGLP" runat="server" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Panel ID="Panel5" runat="server" GroupingText="Participações em migrações anteriores">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkMigracaoAnterior" runat="server" Text="Não efetivou migração de carga horária nos processos seletivos anteriores." />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="Panel10" runat="server" GroupingText="DECLARO, em atenção ao §1° do art. 4º do Decreto nº 49.026 de 02 de abril de 2024, minha opção por:">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblRubrica" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Text="Utilizar a rubrica referida no caput do art. 4° como parte integrante de minha remuneração de contribuição."
                                                Value="Sim"></asp:ListItem>
                                            <asp:ListItem Text="Não utilizar a rubrica referida no caput do art. 4° como parte integrante de minha remuneração de contribuição."
                                                Value="Nao"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlDataConvocacao" runat="server" GroupingText="Data D.O. Convocação/Migração"
                            Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtConvocacao" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                    </td>
                                    <td style="width: 50px">
                                        <asp:Button ID="btnAlterarData" runat="server" ValidationGroup="SalvarForm" Text="Alterar Data"
                                            OnClick="btnAlterarData_Click" OnClientClick="Bloqueio();" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Documentos" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDocumentos" runat="server">
                        <dxwgv:ASPxGridView runat="server" ID="grdDocumento" ClientInstanceName="grdDocumento"
                            AutoGenerateColumns="False" EnableCallBacks="false" Width="800" DataSourceID="odsDocumento"
                            KeyFieldName="DOCENTECANDIDATOARQUIVOID">
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Visible="false" FieldName="DOCENTECANDIDATOARQUIVOID"
                                    VisibleIndex="3" Width="1%">
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="4"
                                    Width="30%">
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Data Envio" FieldName="DATACADASTRO" VisibleIndex="5"
                                    Width="20%">
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Importar" Name="btnDetalhes" VisibleIndex="8"
                                    Width="100px">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <DataItemTemplate>
                                        <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("DOCENTECANDIDATOARQUIVOID") + "," + Eval("TIPODOCUMENTOID") %>'
                                            OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo">
                                        </asp:ImageButton>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Visualizar" Name="btnVisualizar" VisibleIndex="9"
                                    Width="100px">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <DataItemTemplate>
                                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("DOCENTECANDIDATOARQUIVOID") + "," + Eval("TIPOARQUIVO") %>'
                                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                            AlternateText="Visualizar Documento" Visible='<%# Eval("DOCENTECANDIDATOARQUIVOID") != DBNull.Value  %>'>
                                        </asp:ImageButton>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <dxpc:ASPxPopupControl ID="pucConfirmarArquivo" ClientInstanceName="pucConfirmarArquivo"
                            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
                            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
                            Width="580px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
                            HeaderText="Upload de Documentos">
                            <HeaderStyle HorizontalAlign="Center" />
                            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                            <ContentStyle VerticalAlign="Top">
                            </ContentStyle>
                            <SizeGripImage Height="12px" Width="12px" />
                            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                            <ContentCollection>
                                <dxpc:PopupControlContentControl>
                                    <table id="Table1" runat="server">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblArquivo" Style="margin-left: 23px;" Text="Documento:"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" Style="margin-left: 30px;" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btnImportar" runat="server" Text="Importar" OnClick="btnImportar_Click"
                                                    OnClientClick="pucConfirmarArquivo.Hide(); return true;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label Text="" ID="Statuslbl" runat="server" />
                                </dxpc:PopupControlContentControl>
                            </ContentCollection>
                        </dxpc:ASPxPopupControl>
                        <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
                            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
                            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                            CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Documento">
                            <HeaderStyle HorizontalAlign="Center" />
                            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                            <ContentStyle VerticalAlign="Top">
                            </ContentStyle>
                            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                            <ContentCollection>
                                <dxpc:PopupControlContentControl>
                                    <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                                        Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                                        ClientInstanceName="bimgArquivo">
                                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                                    </dxe:ASPxBinaryImage>
                                    <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
                                </dxpc:PopupControlContentControl>
                            </ContentCollection>
                        </dxpc:ASPxPopupControl>
                        <div style="visibility: hidden">
                            <asp:HiddenField ID="hdnDocenteCandidatoArquivoId" runat="server" />
                            <asp:HiddenField ID="hdnDocenteCandidatoTipoArquivoId" runat="server" />
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
