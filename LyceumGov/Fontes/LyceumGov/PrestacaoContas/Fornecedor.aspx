<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Fornecedor.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.Fornecedor" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
    
<%@ Register src="FornecedorPopup.ascx" tagname="FornecedorPopup" tagprefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 60px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <script>
        subinsblogla = 0;
        
        function loadingPanel() {
            console.log(document.readyState);
            if (document.readyState != 'complete') {
                document.documentElement.style.overflow = "hidden";
                var subinsblog = document.createElement("div");
                subinsblog.id = "subinsblogldiv";
                var polu = 99 * 99 * 99999999 * 999999999;
                subinsblog.style.zIndex = polu;
                subinsblog.style.background = "#ededed url('../Images/updateProgress.gif') 50% 50% no-repeat";
                subinsblog.style.opacity = "0.7";
                subinsblog.style.backgroundPositionX = "50%";
                subinsblog.style.backgroundPositionY = "50%";
                subinsblog.style.position = "absolute";
                subinsblog.style.right = "0px";
                subinsblog.style.left = "0px";
                subinsblog.style.top = "0px";
                subinsblog.style.bottom = "0px";
                if (subinsblogla == 0) {
                    document.documentElement.appendChild(subinsblog);
                    subinsblogla = 1;
                }
            } else if (document.getElementById('subinsblogldiv') != null) {
                document.getElementById('subinsblogldiv').style.display = "none";
                document.documentElement.style.overflow = "auto";
                clearInterval(subinsbloglaInterval);
            }
        }

        var subinsbloglaInterval = setInterval(loadingPanel, 100);
    </script>

    <link href="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.css" rel="stylesheet">
    
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

    <script type="text/javascript">

        $("#<%= this.txtMunicipio.ClientID %>").attr("readonly", "readonly");

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

        function abrirPopupConfirmar() {
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function formataFixoCelularDDD(b, a) {

            vr = b.value = filtraNumeros(filtraCampo(b));
            tam = vr.length;
            if (tam < 10)
                return;

            if (tam == 11) {
                formataCelularDDD(b, a);
            }
            if (tam == 10) {
                formataTelefoneDDD(b, a);

            }
        }

        function grdDocumento_OnBeginCallback(s, e) {
            switch (e.command) {
                case "STARTEDIT":
                case "UPDATEEDIT":
                case "CANCELEDIT":
                    pucLoading.Show();
                    break;
            }
        }

        function grdDocumento_OnEndCallback(s, e) {
            pucLoading.Hide();
        }
    </script>

    <script type="text/javascript">

        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });

            txtCnpj = $("#<%= txtCnpj.ClientID %>");
            if (txtCnpj.length > 0) {
                $("#hplCNPJ").css("display", "inline");
                $("#hplCNPJ").on("click", function(evt) {
                    window.open($(evt.target).attr("data-url") + "?cnpj=" + $("#<%= txtCnpj.ClientID %>").val(), "_blank");
                });
            }
            else
                $("#hplCNPJ").css("display", "none");
        });
 
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por fornecedor"
        Width="617px">
        <table style="width: 600px">
            <tr>
                <td align="left">
                    <asp:Label ID="lblForncedor" runat="server" Text="Fornecedor:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseFornecedorPrestacao" runat="server" Key="IDFORNECEDOR" Argument="RAZAOSOCIAL"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="TRUE" SqlSelect="SELECT CNPJ, INSCRICAOESTADUAL, INSCRICAOMUNICIPAL, DATACADASTRO, FORNECEDORID, SITUACAO from [PrestacaoContas].[VW_TSEARCH_FORNECEDOR]"
                        GridWidth="850px" SqlOrder="FORNECEDORID" OnChanged="tseFornecedorPrestacao_Changed"
                        OnLoad="tseFornecedorPrestacao_Load">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="IDFORNECEDOR" Width="0%" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="FORNECEDORID" Width="6%" />                            
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="CNPJ" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="RAZAOSOCIAL" Width="35%" />
                            <tweb:TSearchBoxColumn Caption="Insc. Estadual" FieldName="INSCRICAOESTADUAL" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Insc. Municipal" FieldName="INSCRICAOMUNICIPAL" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="25%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcNovoEditar" OnClick="btnEditar_Click" OnClientClick="" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovoNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcNovoCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click" />
        <asp:ImageButton ID="btnEnviarAnalise" runat="server" ImageUrl="~/Images/bot_EnviarAnalise.png"
            align="right" OnClick="btnEnviarAnalise_Click" ToolTip="Enviar para Análise"
            SkinID="BcEnviar" />
        <asp:ImageButton ID="btnAnalisar" runat="server" align="right" OnClientClick="pucConfirmar.Show(); return false;"
            OnPreRender="btnAnalisar_PreRender" ToolTip="Analisar" SkinID="BcAnalisar" />
        <asp:Label runat="server" ID="lblBlocoFornecedor" Text="Fornecedor" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:HiddenField ID="hdnFornecedorId" runat="server" />
    <asp:HiddenField ID="hdnFinalizado" runat="server" />
    <asp:HiddenField ID="hdnEnviado" runat="server" />
    <dxtc:ASPxPageControl ID="pcFornecedor" runat="server" ActiveTabIndex="0" OnTabClick="pcFornecedor_TabClick"
        Width="50%">
        <tabpages>
            <dxtc:TabPage Text="Informações Gerais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label7" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="Situação:*"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="lblSituacao" runat="server" Font-Names="Verdana" SkinID="lblMensagem"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblTipo" runat="server" SkinID="lblObrigatorio" Text="Tipo:* "></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:RadioButtonList ID="rblTipo" runat="server" RepeatDirection="Horizontal" Width="350px"
                                        OnSelectedIndexChanged="rblTipo_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Pessoa Jurídica" Value="Pessoa Jurídica" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Pessoa Física" Value="Pessoa Física"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="CNPJ/CPF:*"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtCnpj" runat="server" MaxLength="19" Width="140px" onkeyup="formataCNPJ(this,event)" />
                                    <asp:TextBox ID="txtCPF" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                                        onkeyup="formataCPF(this,event)" Visible="false" />
                                    <a id="hplCNPJ" style="display: none;" href="javascript: void(0)" data-url="http://servicos.receita.fazenda.gov.br/Servicos/cnpjreva/Cnpjreva_Solicitacao.asp">
                                        Consulta CNPJ</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label6" runat="server" Text="Razão Social:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRazaoSocial" runat="server" Width="400px" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkGrandePorte" Text="Cadastro Simplificado" Width="140px" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkEventual" Text="Eventual" Width="140px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblInscricaoEstadual" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="Inscrição Estadual:*"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInscricao" runat="server" Width="140px" SkinID="numerico" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblInscricaoMunicipal" runat="server" Text="Inscrição Municipal:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtInscricaoMunicipal" runat="server" Width="140px" SkinID="numerico" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtCEP" runat="server" SkinID="numerico" MaxLength="8" />
                                    <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP"
                                        Modal="true" SkinID="CEP" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Visible="false"></asp:TextBox>
                                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                        GridWidth="600px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
                                        MaxLength="10" OnLoad="tseMunicipio_Load">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <input id="txtEstado" runat="server" maxlength="2" class="txtInput" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                        Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                        Width="400px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                        Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                        Text="Compl.:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndCompl" runat="server" MaxLength="50" onkeypress="return endereco(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                        Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                        Width="400px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label14" runat="server" Font-Names="Verdana" 
                                        Text="E-mail:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="400px" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label15" runat="server" Text="Telefone:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtTelefone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                                        MaxLength="14" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblAlteracaoSenha" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="Para enviar ao usuário o E-mail de Redefinição de Senha, "></asp:Label>
                                    <asp:LinkButton ID="lnkAlteracaoSenha" runat="server" OnClick="lnkAlteracaoSenha_Click"
                                        Visible="false">CLIQUE AQUI</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmacaoCadastro" runat="server" SkinID="lblObrigatorio" Text="Para enviar ao usuário o E-mail de Confirmação de Cadastro, "></asp:Label>
                                    <asp:LinkButton ID="lnkConfirmacaoCadastro" runat="server" OnClick="lnkConfirmacaoCadastro_Click"
                                        Visible="false">CLIQUE AQUI</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Habilitação">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl6" runat="server">
                        <asp:ObjectDataSource ID="odsProdutoServicoGrupo" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                            SelectMethod="ListaProdutoServicoGrupo"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsFornecedorProdutoServicoGrupo" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                            SelectMethod="ListaFornecedorProdutoServicoGrupo" InsertMethod="InsertFornecedorProdutoServicoGrupo"
                            UpdateMethod="UpdateFornecedorProdutoServicoGrupo" DeleteMethod="DeleteFornecedorProdutoServicoGrupo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseFornecedorPrestacao" PropertyName="Value" Name="IDFORNECEDOR"
                                    Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView runat="server" ID="grdFornecedorProdutoServicoGrupo" DataSourceID="odsFornecedorProdutoServicoGrupo"
                            ClientInstanceName="grdFornecedorProdutoServicoGrupo" AutoGenerateColumns="False"
                            KeyFieldName="FORNECEDOR__PRODUTOSERVICOGRUPOID" OnAfterPerformCallback="grdFornecedorProdutoServicoGrupo_AfterPerformCallback"
                            OnInitNewRow="grdFornecedorProdutoServicoGrupo_InitNewRow" OnStartRowEditing="grdFornecedorProdutoServicoGrupo_StartRowEditing"
                            OnRowInserting="grdFornecedorProdutoServicoGrupo_RowInserting" OnRowUpdating="grdFornecedorProdutoServicoGrupo_RowUpdating"
                            OnRowDeleting="grdFornecedorProdutoServicoGrupo_RowDeleting">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="100px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                onclick="grdFornecedorProdutoServicoGrupo.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Visible="True" Text="Remover">
                                        <Image Url="../img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Grupo" FieldName="PRODUTOSERVICOGRUPOID"
                                    VisibleIndex="1" Width="600px">
                                    <PropertiesComboBox DataSourceID="odsProdutoServicoGrupo" TextField="DESCRICAO" ValueField="PRODUTOSERVICOGRUPOID"
                                        Width="600px" ValueType="System.String" DropDownWidth="600px">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico de Razão Social ">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:ObjectDataSource ID="odsHistoricoRazaoSocial" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                                        SelectMethod="ListaRazaoSocial" InsertMethod="InsertRazaoSocial" UpdateMethod="UpdateRazaoSocial"
                                        DeleteMethod="DeleteRazaoSocial">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="tseFornecedorPrestacao" PropertyName="Value" Name="IDFORNECEDOR"
                                                Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <dxwgv:ASPxGridView runat="server" ID="grdHistoricoRazaoSocial" ClientInstanceName="grdHistoricoRazaoSocial"
                                        DataSourceID="odsHistoricoRazaoSocial" AutoGenerateColumns="False" KeyFieldName="FORNECEDORRAZAOSOCIALID"
                                        EnableCallBacks="false" OnCellEditorInitialize="grdHistoricoRazaoSocial_CellEditorInitialize"
                                        OnInitNewRow="grdHistoricoRazaoSocial_InitNewRow" OnStartRowEditing="grdHistoricoRazaoSocial_StartRowEditing"
                                        OnRowUpdating="grdHistoricoRazaoSocial_RowUpdating" OnAfterPerformCallback="grdHistoricoRazaoSocial_AfterPerformCallback">
                                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="100px">
                                                <CancelButton Visible="true" Text="Cancelar">
                                                    <Image Url="~/img/bt_cancelar.png" />
                                                </CancelButton>
                                                <EditButton Visible="True" Text="Editar">
                                                    <Image Url="../img/bt_editar.png" />
                                                </EditButton>
                                                <ClearFilterButton Text="Limpar" Visible="True">
                                                    <Image Url="~/img/bt_limpa.png" />
                                                </ClearFilterButton>
                                                <UpdateButton Visible="true" Text="Alterar">
                                                    <Image Url="../img/bt_salvar.png" />
                                                </UpdateButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="FORNECEDORRAZAOSOCIALID"
                                                Name="FORNECEDORRAZAOSOCIALID" VisibleIndex="1" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Razão Social" FieldName="DESCRICAO" VisibleIndex="3"
                                                Width="400">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                                </CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataDateColumn VisibleIndex="9" Caption="Data Início" Name="DATAINICIO"
                                                FieldName="DATAINICIO" Width="100px" Visible="true" SortOrder="Descending">
                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                                    </ValidationSettings>
                                                </PropertiesDateEdit>
                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                </CellStyle>
                                            </dxwgv:GridViewDataDateColumn>
                                            <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Fim" Name="DATAFIM"
                                                FieldName="DATAFIM" Width="100px" Visible="true">
                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                                </PropertiesDateEdit>
                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                </CellStyle>
                                            </dxwgv:GridViewDataDateColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Representante Legal">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <asp:ObjectDataSource ID="odsRepresentante" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                            SelectMethod="ListaRepresentanteLegal" InsertMethod="InsertRepresentanteLegal"
                            UpdateMethod="UpdateRepresentanteLegal" DeleteMethod="DeleteRepresentanteLegal">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseFornecedorPrestacao" PropertyName="Value" Name="IDFORNECEDOR"
                                    Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdRepresentante" runat="server" DataSourceID="odsRepresentante"
                            ClientInstanceName="grdRepresentante" AutoGenerateColumns="false" KeyFieldName="FORNECEDORREPRESENTANTELEGALID"
                            EnableCallBacks="false" OnCommandButtonInitialize="grdRepresentante_CommandButtonInitialize"
                            OnInitNewRow="grdRepresentante_InitNewRow" OnStartRowEditing="grdRepresentante_StartRowEditing"
                            OnAfterPerformCallback="grdRepresentante_AfterPerformCallback" OnRowInserting="grdRepresentante_RowInserting"
                            OnRowUpdating="grdRepresentante_RowUpdating" OnRowDeleting="grdRepresentante_RowDeleting"
                            Width="700px">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="100px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                onclick="grdRepresentante.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Visible="True" Text="Remover">
                                        <Image Url="../img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome*" Name="NOME" FieldName="NOME" VisibleIndex="1"
                                    Width="400px">
                                    <PropertiesTextEdit MaxLength="100">
                                        <ClientSideEvents KeyPress="function (s, e){ SomentePermitirLetrasEspaco(s, e.htmlEvent); }" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CPF*" Name="CPF" FieldName="CPF" VisibleIndex="2"
                                    Width="200px">
                                    <PropertiesTextEdit MaxLength="11">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início*" Name="DATAINICIO" FieldName="DATAINICIO"
                                    VisibleIndex="3" Width="100px">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" Name="DATAFIM" FieldName="DATAFIM"
                                    VisibleIndex="4" Width="100px">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Documentos">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl4" runat="server">
                        <asp:Panel ID="pnlDocumentos" runat="server">
                            <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                                SelectMethod="ListaDocumento" UpdateMethod="UpdateDocumento">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseFornecedorPrestacao" PropertyName="Value" Name="IDFORNECEDOR"
                                        Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
                                    <asp:ControlParameter ControlID="rblTipo" Name="tipo" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ClientInstanceName="grdDocumento" ID="grdDocumento" runat="server"
                                Width="1000px" DataSourceID="odsDocumento" KeyFieldName="DOCUMENTOSNECESSARIOSFORNECEDORID"
                                AutoGenerateColumns="false" EnableCallBacks="true" OnCellEditorInitialize="grdDocumento_CellEditorInitialize"
                                OnRowUpdating="grdDocumento_RowUpdating">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                <ClientSideEvents BeginCallback="grdDocumento_OnBeginCallback" EndCallback="grdDocumento_OnEndCallback" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="100px">
                                        <CancelButton Visible="true" Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                        <EditButton Visible="True" Text="Editar">
                                            <Image Url="../img/bt_editar.png" />
                                        </EditButton>
                                        <ClearFilterButton Text="Limpar" Visible="True">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                        <UpdateButton Visible="true" Text="Alterar">
                                            <Image Url="../img/bt_salvar.png" />
                                        </UpdateButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="DOCUMENTOSFORNECEDORID" FieldName="DOCUMENTOSFORNECEDORID"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="DOCUMENTOSNECESSARIOSFORNECEDORID" FieldName="DOCUMENTOSNECESSARIOSFORNECEDORID"
                                        VisibleIndex="2" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="" FieldName="PERIODICIDADE_MESES" VisibleIndex="2"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="3"
                                        Width="300px">
                                        <EditItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "DESCRICAO")%></EditItemTemplate>
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Periodicidade" FieldName="PERIODICIDADE" VisibleIndex="4"
                                        Width="100px">
                                        <EditItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "PERIODICIDADE")%></EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                                        </CellStyle>
                                        <EditCellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                                        </EditCellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="5"
                                        Width="100px">
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                            </ValidationSettings>
                                        </PropertiesDateEdit>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Fim" Name="DATAFIM" FieldName="DATAFIM"
                                        VisibleIndex="6" Width="100px">
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                        </PropertiesDateEdit>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Enviado?" FieldName="ENVIADO" VisibleIndex="7"
                                        Width="100px">
                                        <DataItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "DOCUMENTOSFORNECEDORID") != DBNull.Value ? DataBinder.Eval(Container.DataItem, "ENVIADO") : "" %></DataItemTemplate>
                                        <EditItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "DOCUMENTOSFORNECEDORID") != DBNull.Value ? DataBinder.Eval(Container.DataItem, "ENVIADO") : ""%></EditItemTemplate>
                                        <EditCellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Importar" Name="btnDetalhes" VisibleIndex="8"
                                        Width="100px">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("DOCUMENTOSFORNECEDORID") + "," + Eval("DOCUMENTOSNECESSARIOSFORNECEDORID") %>'
                                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo"
                                                Visible='<%# Eval("DOCUMENTOSFORNECEDORID") != DBNull.Value %>'></asp:ImageButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" Name="btnVisualizar" VisibleIndex="9"
                                        Width="100px">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("DOCUMENTOSFORNECEDORID") + "," + Eval("TIPOARQUIVO") %>'
                                                OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento" Visible='<%# Eval("DOCUMENTOSFORNECEDORID") != DBNull.Value && Eval("FORNECEDORDOCUMENTOARQUIVOID") != DBNull.Value %>'>
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                        <div style="visibility: hidden">
                            <asp:HiddenField ID="hdnFornecedorTipoDocumentoId" runat="server" />
                            <asp:HiddenField ID="hdnFornecedorDocumentoId" runat="server" />
                        </div>
                        <br />
                        <br />
                        <br />
                        
                        <asp:Repeater ID="repCarrossel" runat="server" DataSourceID="odsDocumento" OnItemDataBound="repCarrossel_ItemDataBound">
                            <HeaderTemplate>
                                <div class="glider-contain" style="width: 800px;">
                                    <p style="text-align: center; color: red; font-size: 14px;">Clique nos círculos abaixo para navegar no carrossel de documentos abaixo</p>
                                    <div role="tablist" class="dots"></div>
                                    <div class="glider">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <div style="font-size: 16px; text-align: center; font-weight: bold; color: black;">
                                        <p><%# DataBinder.Eval(Container.DataItem, "DESCRICAO")%></p>
                                    </div>
                                    
                                    <asp:PlaceHolder ID="plaTipoPDF" runat="server" Visible="false">
                                    <object data="FileCS.ashx?Tabela=FornecedorDocumentoArquivo&Id=<%# Eval("DOCUMENTOSFORNECEDORID") %>" type="application/pdf" width="800px" height="1170px">
	                                    <iframe src="FileCS.ashx?Tabela=FornecedorDocumentoArquivo&Id=<%# Eval("DOCUMENTOSFORNECEDORID") %>" width="100%" height="100%" style="border: none;">
		                                    <p>Your browser does not support PDFs.<a href="FileCS.ashx?Tabela=FornecedorDocumentoArquivo&Id=16">Download the PDF</a>.</p>
	                                    </iframe>
                                    </object>
                                    </asp:PlaceHolder>
                                    
                                    <asp:PlaceHolder ID="plaTipoImagem" runat="server" Visible="false">
                                    <img src="FileCS.ashx?Tabela=FornecedorDocumentoArquivo&Id=<%# Eval("DOCUMENTOSFORNECEDORID") %>" />
                                    </asp:PlaceHolder>
                                    
                                    <asp:PlaceHolder ID="plaSemArquivo" runat="server" Visible="false">
                                    <div style="font-size: 12px; text-align: center; font-weight: bold; color: black;">
                                        <p>(Nenhum arquivo enviado)</p>
                                    </div>
                                    </asp:PlaceHolder>
                                    
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </div>
                                </div>
                                
                                <script src="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.js"></script>
    
                                <script language="javascript">
                                    var glider = new Glider(document.querySelector('.glider'), {
                                      slidesToShow: 1,
                                      slidesToScroll: 1,
                                      draggable: true,
                                      dots: ".dots",
                                      arrows: {
                                        prev: ".prev",
                                        next: ".next",
                                      },
                                    });
                                </script>
                            </FooterTemplate>
                        </asp:Repeater>
                        
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
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico de Análise">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl5" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:ObjectDataSource ID="odsHistoricoAnalise" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.Fornecedor"
                                        SelectMethod="ListaHistoricoAnalise">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="tseFornecedorPrestacao" PropertyName="Value" Name="IDFORNECEDOR"
                                                Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <dxwgv:ASPxGridView runat="server" ID="grdHistoricoAnalise" ClientInstanceName="grdHistoricoAnalise"
                                        DataSourceID="odsHistoricoAnalise" AutoGenerateColumns="False" KeyFieldName="FORNECEDORANALISEID"
                                        EnableCallBacks="false">
                                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        <SettingsEditing Mode="Inline" />
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="FORNECEDORANALISEID" Name="FORNECEDORANALISEID"
                                                VisibleIndex="1" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="3"
                                                Width="400">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                                </CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataColumn Caption="Motivo Reprovação" FieldName="MOTIVOREPROVACAO" VisibleIndex="3" Width="400">
                                                <DataItemTemplate>
                                                    <%# Eval("MOTIVOREPROVACAO").ToString().Replace(Environment.NewLine, "<br />")%>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataDateColumn VisibleIndex="9" Caption="Data Análise" Name="DATAANALISE"
                                                FieldName="DATAANALISE" Width="100px" Visible="true" SortOrder="Descending">
                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                                    </ValidationSettings>
                                                </PropertiesDateEdit>
                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                </CellStyle>
                                            </dxwgv:GridViewDataDateColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </tabpages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="410px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Analisar Fornecedor">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <sizegripimage height="12px" width="12px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <asp:Panel ID="pnlConfirmação" runat="server" GroupingText="Confirmação" Width="80%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="false" Width="201px">
                                    <asp:ListItem Text="Aprovado" Value="Aprovado"></asp:ListItem>
                                    <asp:ListItem Text="Reprovado" Value="Reprovado"></asp:ListItem>
                                </asp:RadioButtonList>
                                
                                <script language="javascript">
                                    $(() => {
                                        $(document.getElementsByName("<%= rblConfirmacao.UniqueID %>")).on("click", (evt) => {
                                            let pnlMotivo = $("#<%= pnlMotivo.ClientID %>");
                                            if (evt.target.value === "Reprovado") {
                                                pnlMotivo.css("display", "block");
                                            }
                                            else {
                                                pnlMotivo.css("display", "none");
                                            }
                                        });
                                    });
                                </script>
                            </td>
                            <td>
                                <asp:Panel ID="pnlMotivo" runat="server" Visible="true" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Motivo Reprovação Fornecedor:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlMotivoReprovacaoFornecedor" runat="server" DataTextField="DESCRICAO"
                                                    DataValueField="MOTIVOREPROVACAOFORNECEDORID" AppendDataBoundItems="true" Width="201px">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hidMotivosSelecionados" runat="server" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnAdicionarMotivo" value="+" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="overflow: auto;height: 100px;width: 100%;">
                                                    <table id="tableMotivos">
                                                        <tbody></tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <script language="javascript">
                                        $(() => {
                                            $("#btnAdicionarMotivo").on("click", (evt) => {
                                                let text = $("#<%= ddlMotivoReprovacaoFornecedor.ClientID %>").find(":selected").text();
                                                let value = $("#<%= ddlMotivoReprovacaoFornecedor.ClientID %>").find(":selected").val();
                                                if (value != '') {
                                                    if ($('#tableMotivos > tbody > tr[data-value=\'' + value + '\']').length == 0) {
                                                        $("#tableMotivos > tbody").append('<tr data-value="' + value + '"><td><input type="button" class="btnRemoverMotivo" value="X" data-value=' + value + '></td><td>' + text + '</td></tr>');
                                                    }
                                                }
                                                
                                                $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                if ($('#tableMotivos > tbody > tr').length > 0) {
                                                    $('#tableMotivos > tbody > tr').each((index, el) => {
                                                        if (index > 0) {
                                                            $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + ";");
                                                        }
                                                        $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + $(el).attr("data-value"));
                                                    });
                                                }
                                                else {
                                                    $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                }
                                            });
                                        
                                            $("#tableMotivos > tbody").delegate(".btnRemoverMotivo", "click", (evt) => {
                                                let value = $(evt.target).attr("data-value");
                                                $('#tableMotivos > tbody > tr[data-value=\'' + value + '\']').remove();
                                                
                                                $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                if ($('#tableMotivos > tbody > tr').length > 0) {
                                                    $('#tableMotivos > tbody > tr').each((index, el) => {
                                                        if (index > 0) {
                                                            $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + ";");
                                                        }
                                                        $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + $(el).attr("data-value"));
                                                    });
                                                }
                                                else {
                                                    $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                }
                                            });
                                        });
                                    </script>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
                <table id="Table2" runat="server">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnConfirmarAnalise" runat="server" Text="Analisar" OnClick="btnConfirmarAnalise_Click"
                                OnClientClick="pucConfirmar.Hide(); return true;" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
    
    <uc1:FornecedorPopup ID="FornecedorPopup1" runat="server" />
</asp:Content>
