<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="InformacoesAdicionaisAAE.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.InformacoesAdicionaisAAE" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 60px;
        }
        .myPopup
        {
            top: 15%;
            left: 20%;
            position: fixed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">       

        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function mostrarResultado(box, num_max, spContador) {
            var contagem_carac = box.length;
            if (contagem_carac != 0) {
                document.getElementById(spContador).innerHTML = contagem_carac + " caracteres digitados";
                if (contagem_carac == 1) {
                    document.getElementById(spContador).innerHTML = contagem_carac + " caracter digitado";
                }
                if (contagem_carac >= num_max) {
                    document.getElementById(spContador).innerHTML = "Limite de caracteres excedido!";
                }
            } else {
                document.getElementById(spContador).innerHTML = "Limite de " + num_max + " caracteres";
            }
        }
        function contarCaracteres(box, valor, spContador, campoMult) {

            var conta = valor - box.length;
            document.getElementById(spContador).innerHTML = "Você ainda pode digitar " + conta + " caracteres";
            if (box.length >= valor) {
                document.getElementById(spContador).innerHTML = "Limite excedido.";
                campoMult.value = campoMult.value.substr(0, valor);
            }
        }
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }
      
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informações da Unidade de Ensino"
        Width="700px">
        <table style="width: 700px">
            <tr>
                <td align="left">
                    <asp:Label ID="lblForncedor" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        EnableViewState="true" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio, id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                        ArgumentColumns="75" Columns="10" OnChanged="tseUnidadeResponsavel_Changed" GridWidth="850px"
                        SqlOrder="nome_comp" OnLoad="tseUnidadeResponsavel_Load">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnMandatoId" runat="server" />
    <br />
    <dxtc:ASPxPageControl ID="pcFornecedor" runat="server" ActiveTabIndex="0" OnTabClick="pcFornecedor_TabClick"
        Width="50%">
        <tabpages>
            <dxtc:TabPage Text="Dados da Unidade">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label8" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtRegional" runat="server" Width="400px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEndereco" runat="server" Font-Names="Verdana" Text="Endereço:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="19" Width="140px" ReadOnly="true"
                                        onkeyup="formataCNPJ(this,event)" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                        Text="Número:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                        Text="Complemento:" ReadOnly="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndCompl" runat="server" ReadOnly="true" MaxLength="50" onkeypress="return endereco(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtBairro" runat="server" Width="400px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblMunicipio" runat="server" Text="Municipio:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtMunicipio" runat="server" Width="400px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblTelefone" runat="server" Font-Names="Verdana" Text="Telefone:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtTelefone" runat="server" Width="140px" SkinID="numerico" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                                        Text="Email:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtEmail" runat="server" SkinID="numerico" MaxLength="8" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDiretor" runat="server" Text="Diretor: "></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtDiretor" runat="server" MaxLength="20" Visible="true" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblCenso" runat="server" Font-Names="Verdana" Text="Censo:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtCenso" runat="server" Width="400px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNumeroAlunos" runat="server" Font-Names="Verdana" Text="Número de Alunos:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtNumeroAlunos" runat="server" Width="400px" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Dados da AAE">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl6" runat="server">
                        <asp:Panel ID="pnlResolucao" runat="server" GroupingText="Presidente da AAE" Font-Names="Verdana"
                            Width="100%">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNomePresAAE" runat="server" Text="Nome:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtNomePresAAE" runat="server" Width="100%" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblRgPresAAE" runat="server" Text="Rg:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRgPresAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCpfPresAAE" runat="server" Text="Cpf:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCpfPresAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEnderecoPresAAE" runat="server" Text="Endereço:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnderecoPresAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNumeroPresAAE" runat="server" Text="Número:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumeroPresAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblComplementoPresAAE" runat="server" Text="Complemento:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComplementoPresAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblBairroPresAAE" runat="server" Text="Bairro:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtBairroPresAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblMunicipioPresidenteAAE" runat="server" Text="Municipio:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtMunicipioPresidenteAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEmailPresAAE" runat="server" Text="E-mail:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailPresAAE" runat="server" Width="100%" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTelefonePresAAE" runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefonePresAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblIdFuncionalPresAAE" runat="server" Text="Id Funcional:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIdFuncionalPresAAE" runat="server" Text="Id Funcional:" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblMatriculaPresAAE" runat="server" Text="Matricula:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtMatriculaPresAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlGrupoProduto" runat="server" GroupingText="Tesoureiro da AAE" Font-Names="Verdana"
                            Width="100%">
                            <table>
                                <tr>
                                    <td colspan="6">
                                        <asp:CheckBox ID="chkPossuiIdFunc" runat="server" Text="Possui ID Funcional" AutoPostBack="true"
                                            OnCheckedChanged="chkPossuiIdFunc_Clicked" />
                                        <asp:CheckBox ID="chkNaoPossuiIdFunc" runat="server" Text="Não Possui ID Funcional"
                                            AutoPostBack="true" OnCheckedChanged="chkNaoPossuiIdFunc_Clicked" />
                                        <asp:Label ID="Label3" runat="server" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNomeTesAAE" runat="server" Text="Nome:" />
                                    </td>
                                    <td colspan="6">
                                        <tweb:TSearchBox ID="tseTesoureiro" runat="server" Argument="nome_compl" Key="pessoa"
                                            MaxLength="9" DataType="VarChar" OnChanged="tseTesoureiro_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="pessoa" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_compl" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label4" runat="server" Text="RG:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRgTesoureiroAAE" runat="server" Text="" ReadOnly="true" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label6" runat="server" Text="CPF:" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCpfTesoureiroAAE" runat="server" Text="" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEnderecoTesoureiroAAE" runat="server" Text="Endereço:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnderecoTesoureiroAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNumeroTesoureiroAAE" runat="server" Text="Número:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumeroTesoureiroAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblComplementoTesoureiroAAE" runat="server" Text="Complemento:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComplementoTesoureiroAAE" runat="server" Text="" Width="100%"
                                            SkinID="lblObrigatorio" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblBairroTesoureiroAAE" runat="server" Text="Bairro:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtBairroTesoureiroAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblMunicipioTesoureiroAAE" runat="server" Text="Municipio:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtMunicipioTesoureiroAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEmailTesoureiroAAE" runat="server" Text="E-mail:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailTesoureiroAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTelefoneTesoureiroAAE" runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefoneTesoureiroAAE" runat="server" Text="" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblIdFuncionalTesoureiroAAE" runat="server" Text="Id Funcional:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtIdFuncionalTesoureiroAAE" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Dados Mandato AAE Vigente" Font-Names="Verdana"
                            Width="100%">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Data do início da vigência do mandato da AAE*:"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="txtInicioMandato" runat="server" Width="120px" Enabled="true"
                                            EnableDefaultAppearance="true" ClientInstanceName="txtInicioMandato" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" OnValueChanged="txtInicioMandato_ValueChanged"
                                            AutoPostBack="true">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label19" runat="server" Text="Quantidade de meses do Mandato:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        <asp:TextBox ID="txtMandato" MaxLength="4" runat="server" AutoPostBack="true" OnTextChanged="txtMandato_TextChanged"
                                            SkinID="numerico"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Text="Data do fim da vigência do mandato da AAE*:"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFinalDoMandato" runat="server" Text="" SkinID="lblObrigatorio"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="FileUpload2" runat="server" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkVisualizarRelatorio" runat="server" OnClick="lnkVisualizarRelatorio_Click">Visualizar Ata</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="Histórico de Mandatos da AAE"
                            Font-Names="Verdana" Width="100%">
                            <asp:ObjectDataSource ID="odsMandatosAae" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.InformacoesAdicionaisAAE"
                                SelectMethod="ListaMandatosAae" DeleteMethod="Delete">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="DBValue" Name="censo" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ClientInstanceName="grdMandatosAae" ID="grdMandatosAae" OnHtmlRowCreated="grdMandatosAae_HtmlRowCreated"
                                runat="server" Width="100%" DataSourceID="odsMandatosAae" KeyFieldName="MANDATOAAEID" EnableCallBacks="false"
                                OnCustomButtonCallback="grdMandatosAae_CustomButtonCallback">
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                                <SettingsCookies Enabled="false" />
                                <SettingsText EmptyDataRow="Não existem dados." />
                                <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                 <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px" Caption="Excluir">
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluir" Text="Excluir" Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png">
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                                  
                                    <dxwgv:GridViewDataTextColumn Caption="MANDATOAAEID" FieldName="MANDATOAAEID" VisibleIndex="1"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ARQUIVOAAEID" FieldName="ARQUIVOAAEID" VisibleIndex="2"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Início Vigência" FieldName="DATAINICIOMANDATO"
                                        VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Fim Vigência" FieldName="DATAFIMMANDATO" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Quantidade Meses" FieldName="MANDATO" VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Tesoureiro" FieldName="TESOUREIRO" VisibleIndex="6">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CPF do Tesoureiro" FieldName="CPFTESOUREIRO"
                                        VisibleIndex="7">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Arquivo" FieldName="NOMEARQUIVO" VisibleIndex="8">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" Name="btnVisualizarMandato" VisibleIndex="9"
                                        Width="35px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizarMandato" runat="server" EnableViewState="false" CommandArgument='<%# Eval("ARQUIVOAAEID") + "," + Eval("TIPOARQUIVO")%>'
                                                OnCommand="btnVisualizarMandato_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento"></asp:ImageButton>
                                        </DataItemTemplate>
                                    </dxwgv:GridViewDataTextColumn>                                  
                                    <dxwgv:GridViewDataTextColumn Caption="TIPOARQUIVO" FieldName="TIPOARQUIVO" VisibleIndex="11"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Dados Bancários">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBanco" runat="server" Text="Banco:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBanco" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAgencia" runat="server" Text="Agência:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAgencia" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblContaCorrente" runat="server" Text="Conta Corrente:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContaCorrente" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMotivoImpedimento" runat="server" Text="Motivo Impedimento:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMotivoImpedimento" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Obrigações Fiscais" Visible="true">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl4" runat="server">
                        <asp:Panel ID="pnlObrigacoesFiscais" runat="server">
                            <asp:ObjectDataSource ID="odsObrigacoesFiscais" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.InformacoesAdicionaisAAE"
                                SelectMethod="ListaObrigacoesFiscais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="DBValue" Name="censo" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ClientInstanceName="grdObrigacoesFiscais" ID="grdObrigacoesFiscais"
                                OnHtmlRowCreated="grdObrigacoesFiscais_HtmlRowCreated" runat="server" Width="100%"
                                DataSourceID="odsObrigacoesFiscais" KeyFieldName="DECLARACAOAAEID">
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                                <SettingsCookies Enabled="false" />
                                <SettingsText EmptyDataRow="Não existem dados." />
                                <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="DECLARACAOAAEID" FieldName="DECLARACAOAAEID"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="OBRIGACAOFISCALAAEID" FieldName="OBRIGACAOFISCALAAEID"
                                        VisibleIndex="2" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="DECLARACAOFISCALARQUIVOID" FieldName="DECLARACAOFISCALARQUIVOID"
                                        VisibleIndex="3" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Ano Base" FieldName="ANOBASE" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Mês" FieldName="MES" VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Declaração" FieldName="DESCRICAO" VisibleIndex="6"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Periodicidade" FieldName="PERIODICIDADE" VisibleIndex="7">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Enviado" FieldName="ENVIADO" VisibleIndex="8">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Importar" Name="btnDetalhes" VisibleIndex="9"
                                        Width="35px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("OBRIGACAOFISCALAAEID") + "," + Eval("DECLARACAOAAEID") %>'
                                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo">
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" Name="btnVisualizarObrigacoes"
                                        VisibleIndex="10" Width="35px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizarObrigacoes" runat="server" EnableViewState="false"
                                                CommandArgument='<%# Eval("OBRIGACAOFISCALAAEID") + "," + Eval("TIPOARQUIVO")%>'
                                                OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento"></asp:ImageButton>
                                        </DataItemTemplate>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Obrigatório" FieldName="OBRIGATORIO" VisibleIndex="11">
                                    </dxwgv:GridViewDataTextColumn>
                                  <%--  <dxwgv:GridViewDataTextColumn Caption="ARQUIVO" FieldName="ARQUIVO" VisibleIndex="12"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>--%>
                                    <dxwgv:GridViewDataTextColumn Caption="TIPOARQUIVO" FieldName="TIPOARQUIVO" VisibleIndex="13"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="NOMEARQUIVO" FieldName="NOMEARQUIVO" VisibleIndex="14"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                        <div style="visibility: hidden">
                            <asp:HiddenField ID="hdnDeclaracaoAaeId" runat="server" />
                            <asp:HiddenField ID="hdnObrigacaoFiscalAaeId" runat="server" />
                            <asp:HiddenField ID="hdnMandatoAae" runat="server" />
                        </div>
                        <dxpc:ASPxPopupControl ID="pucConfirmarArquivo" ClientInstanceName="pucConfirmarArquivo"
                            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false" CssClass="myPopup"
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
                                                <asp:Label runat="server" ID="Label1" Style="margin-left: 23px;" Text="Ano Base:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="Label5" Style="margin-left: 23px;" Text="Mês:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMes" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
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
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </tabpages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        CssClass="myPopup" ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False"
        EnableAnimation="false" Top="200" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Visualizar Documento">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,18000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                    Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                    ClientInstanceName="bimgArquivo">
                    <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                </dxe:ASPxBinaryImage>
                <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
     <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirma exclusão do mandato?">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <sizegripimage height="12px" width="12px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Sim" OnClick="btnExcluir_Click"
                                    OnClientClick="popup.Hide(); return true;" />
                            </td>
                            <td>
                                <asp:Button ID="btnNao" runat="server" Text="Não" OnClientClick="popup.Hide();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
