<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="UnidadesFisicas_SomenteDepEquip.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.UnidadesFisicas_SomenteDepEquip" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="conUnidadesFisicas" ContentPlaceHolderID="cphFormulario" runat="server">
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

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function onlyNumbers()
        { if (event.keyCode < 48 || event.keyCode > 57) event.keyCode = 0; }

        function blocTexto(campo, qtde) {
            var quant = qtde;

            var valor = $.trim($(campo).val());

            var total = valor.length;

            if (total <= quant) {
                var resto = quant - total;

            }
            else {
                $(campo).val(valor.substr(0, quant));
            }
        }

        function OnEdificacaoChanged(cmbEdificacao) {
            grdDependencias.GetEditor("pavimento").PerformCallback(cmbEdificacao.GetValue().toString());
        }


        function abrirPopup() {
            window.setTimeout(function() {
                pucConfirmarEquipamentos.Show();
            }, 1000);
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidadeFisicaTSearch" runat="server" Text="Unidade Física:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadesFisicas" runat="server" Argument="nome_comp" Key="unidade_fis"
                        SqlSelect="SELECT unidade_fis, nome_comp,salaclimatizada,salaacessibilidade,salaCantinhoLeitura FROM vw_zzcro_unidade_fisica" OnChanged="tseUnidadesFisicas_Changed"
                        MaxLength="8" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="80%" />
                            <tweb:TSearchBoxColumn Caption="Climatizada" Visible="false" FieldName="salaclimatizada" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Acessibilidade" Visible="false" FieldName="salaacessibilidade" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Cantinho da Leitura" Visible="false" FieldName="salaCantinhoLeitura" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGeral" runat="server">
        <dxtc:ASPxPageControl ID="pcUnidadesFisicas" runat="server" ActiveTabIndex="0" Width="1300px">
            <TabPages>
                <dxtc:TabPage Text="Demais Dependências">
                    <ContentCollection>
                        <dxw:ContentControl ID="conDemaisDependencias" runat="server">
                            <asp:Panel ID="pnlDependencia" runat="server" GroupingText="Demais Dependências" />
                            <br />
                            <asp:Panel ID="pnlSalaAlternativa" runat="server" GroupingText="Sala Alternativa" />
                            <br />
                            <asp:Panel ID="pnlCondicoesSala" runat="server" GroupingText="Condições das salas de aula utilizadas na escola (dentro e fora do prédio)">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:TextBox ID="txtQtdSalaClimatizada" runat="server" Width="20px" MaxLength="3"
                                                SkinID="numerico">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Salas de aula climatizadas">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:TextBox ID="txtQtdSalaAcessibilidade" runat="server" Width="20px" MaxLength="3"
                                                SkinID="numerico">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Salas de aula com acessibilidade para pessoas
com deficiência ou mobilidade reduzida">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                    <td style="text-align: right">
                                        <asp:TextBox ID="txtQtdCantinhoLeitura" runat="server" Width="20px" MaxLength="3" SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label38" runat="server" Text="Salas de aula com Cantinho de Leitura para Educação Infantil e o Ensino Fundamental(Anos Iniciais)">
                                        </asp:Label>
                                    </td>
                                </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Button ID="btnSalvarQtdDependencias" runat="server" ValidationGroup="SalvarForm"
                                Text="Salvar Informações" OnClick="btnSalvarQtdDependencias_Click" Visible="false" />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Equipamentos na Unidade">
                    <ContentCollection>
                        <dxw:ContentControl ID="conEquipamentosUnidade" runat="server">
                            <asp:Panel ID="pnlEquipamentos" runat="server" GroupingText="Equipamentos Unidade" />
                            <br />
                            <asp:Button ID="btnSalvarQtdEquipamentos" runat="server" ValidationGroup="SalvarForm"
                                Text="Salvar Informações" OnClick="btnSalvarQtdEquipamentos_Click" />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Internet">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="Panel4" runat="server" GroupingText="Possui Internet Banda Larga?">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblInternetBandaLarga" AutoPostBack="true" runat="server"
                                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rblInternetBandaLarga_SelectedIndexChanged">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlDadosInternet" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" GroupingText="Acesso à internet">
                                                <asp:CheckBoxList ID="chkAcessoInternet" OnSelectedIndexChanged="chkAcessoInternet_SelectedIndexChanged"
                                                    AutoPostBack="true" RepeatColumns="2" CellSpacing="2" runat="server" RepeatDirection="vertical"
                                                    Style="text-align: left">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Panel3" runat="server" GroupingText="Equipamentos que os alunos usam para acessar a Internet da escola" Enabled="false">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <dxe:ASPxCheckBox ID="chkEquipamentoEscola" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="true" Text="Computadores de mesa, portáteis e tablets da escola (laboratório de informática, biblioteca, sala de aula, etc)">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkEquipamentoPessoal" ValueChecked="S" ValueUnchecked="N"
                                                    ValueType="System.String" runat="server" Checked="false" Text="Dispositivos pessoais (computadores portáteis, celulares, tablets, etc)">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Panel2" runat="server" GroupingText="Rede Local de interligação de computadores" Enabled="false">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <dxe:ASPxCheckBox ID="chkRedeCabo" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="true" Text="A Cabo">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxCheckBox ID="chkRedeWireless" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Wireless">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxCheckBox ID="chkSemRedeComputador" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    AutoPostBack="true" runat="server" Checked="false" Text="Não há rede local interligando computadores"
                                                    OnCheckedChanged="chkSemRedeComputador_CheckedChanged">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                            <br />
                            <asp:Button ID="btnSalvarInternet" runat="server" ValidationGroup="SalvarForm" Text="Salvar Informações"
                                OnClick="btnSalvarInternet_Click" />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Pedagógicos">
                    <ContentCollection>
                        <dxw:ContentControl ID="conPedagogicos" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlMaterialPedagogico" runat="server" GroupingText="Instrumentos, materiais socioculturais e/ou pedagógicos em uso na escola para o desenvolvimento de atividades                           
                                        
de ensino aprendizagem">
                                            <asp:CheckBoxList ID="chkMaterialPedagogico" OnSelectedIndexChanged="chkMaterialPedagogico_SelectedIndexChanged"
                                                AutoPostBack="true" RepeatColumns="2" CellSpacing="10" runat="server" RepeatDirection="vertical"
                                                Style="text-align: left">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlOrgaosColegiados" runat="server" GroupingText="Órgãos colegiados em funcionamento na escola"
                                            Width="100%">
                                            <asp:CheckBoxList ID="chkOrgaoColegiado" OnSelectedIndexChanged="chkOrgaoColegiado_SelectedIndexChanged"
                                                AutoPostBack="true" runat="server" RepeatDirection="vertical" Style="text-align: left"
                                                RepeatColumns="2" CellSpacing="10">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="  A escola possui site ou blog ou página em redes sociais para comunicação institucional?"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblPossuiSite" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblPossuiSite_SelectedIndexChanged">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSiteBlog" runat="server" Text="Site/Blog:* " SkinID="lblObrigatorio"
                                            Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtSiteBlog" runat="server" MaxLength="500" Width="250px" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="A escola usa espaços e equipamentos do entorno escolar para atividades regulares
                                        com os(as) alunos(as)?" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblEspacoEquipamentoEntorno" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="A escola possui projeto político pedagógico ou a proposta pedagógica da escola (conforme
                                        art. 12 da LDB)?" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblPossuiProjeto" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblPossuiProjeto_SelectedIndexChanged">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlCumpriuProjeto" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="O projeto político pedagógico ou a proposta pedagógica da escola (conforme art.
                                                        12 da LDB) foi atualizada nos últimos 12 meses até a data de referência?" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblCumpriuProjetoPedagogico" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="A escola compartilha espaços para atividades de integração escola-comunidade"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblCompartilhaEspacoComunidade" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnSalvarPedagogicos" runat="server" ValidationGroup="SalvarForm"
                                Text="Salvar Informações" OnClick="btnSalvarPedagogicos_Click" />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucConfirmarEquipamentos" ClientInstanceName="pucConfirmarEquipamentos"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            Alguma(s) quantidade(s) precisa(m) de verificação.
                            <br />
                            Confirma a gravação de:
                        </td>
                    </tr>
                    <tr runat="server" id="trMaximos">
                        <td>
                            <asp:BulletedList ID="blMaximos" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConfirmar" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnConfirmarEquipamentos" runat="server" Text="Confirmar" OnClick="btnConfirmarEquipamentos_Click" />
                            <asp:Button ID="btnCancelarEquipamentos" runat="server" Text="Cancelar" OnClientClick="pucConfirmarEquipamentos.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
