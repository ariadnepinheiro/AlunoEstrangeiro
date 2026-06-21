<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ValidaInfCertificacao.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.ValidaInfCertificacao" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        //add event handlers to the search UpdatePanel
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function startRequest(sender, e) {

            ppcLoading.Show();

            $('body').css('overflow', 'auto');

        }

        function endRequest(sender, e) {
            ppcLoading.Hide();
        }

        function atualizargrid2() {
            $("#<%=btBuscar.ClientID %>").click();
            // alert('Eu sou um alert!');
        }

        function scroll() {

            $('html, body').css({
                overflow: 'auto',
                height: 'auto'
            });
        }

        function popupItens(id) {
            //console.log(id); 
            var txtRow = document.getElementById('<%=txtRow.ClientID %>');
            txtRow.value = id;

            $("#<%=pnlLiberacao.ClientID %>").css("display", "none"); // esconde o painel , onde tem os botões de autorização e download

            pucItemHistorico.Show();
            $("#<%=btnAtualizar.ClientID %>").click();
        }

        /*
        DevExpress Workaround:
        https://www.devexpress.com/Support/Center/Question/Details/T555320/a-popup-is-not-shown-or-is-shown-at-an-incorrect-position-in-chrome-61-and-newer-versions
        A popup is not shown or is shown at an incorrect position in Chrome 61 and newer versions
                    
        Nas versões recentes do Chrome, o popup não estava aparecendo centralizado na tela visível do usuário. Ao invés disso, estava aparecendo centralizado como
        se o scrollTop estivesse zerado.
                    
        As funções abaixo corrigem o scrollTop para o que o componente do DevExpress realmente espera receber.
        */
        window.onload = function() {

            function _aspxGetDocumentScrollTop() {
                return document.documentElement.scrollTop || document.body.scrollTop
            }
            if (window._aspxGetDocumentScrollTop) {
                window._aspxGetDocumentScrollTop = _aspxGetDocumentScrollTop;
            }
            /* Begin -> Correct ScrollLeft */
            function _aspxGetDocumentScrollLeft() {
                return document.documentElement.scrollLeft || document.body.scrollLeft
            }
            if (window._aspxGetDocumentScrollLeft) {
                window._aspxGetDocumentScrollLeft = _aspxGetDocumentScrollLeft;
            }
            /* End -> Correct ScrollLeft */
        }

        function SelChanged(s, e) {
            if (navigator.userAgent.indexOf("Firefox") == -1)
                return;

            window.setTimeout(function() {
                // console.log("entrou");
                // console.log($("#ctl00_cphFormulario_pucItemHistorico_DXPWMB-1"));
                $("#ctl00_cphFormulario_pucItemHistorico_TCFix-1").css("position", "absolute");
                $("#ctl00_cphFormulario_pucItemHistorico_TCFix-1").css("width", "10px");
                $("#ctl00_cphFormulario_pucItemHistorico_TCFix-1").css("height", "10px");
            }, 1000);
        }
    </script>

    <asp:UpdatePanel ID="upnlMatriculas" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnBusca" runat="server" Width="100%">
                <br />
                <div style="height: 1px; width: 100%; border-bottom: inset 1px Blue;">
                </div>
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                            Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, ua_atual, ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                                            MaxLength="20" FieldName="Unidade de Ensino" GridWidth="850px" SqlOrder="nome_comp"
                                            AutoPostBack="false">
                                            <gridcolumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                            </gridcolumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTCurso" Text="Escolaridade:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseCurso2" runat="server" Argument="nome" Caption="" Key="curso"
                                            SqlSelect=" SELECT DISTINCT C.curso, nome FROM ly_curso C INNER JOIN LY_UNIDADE_ENSINO_CURSOS LC ON LC.CURSO = C.CURSO  "
                                            SqlOrder="nome" AutoPostBack="false" MaxLength="20" SqlWhere=" UNIDADE_ENS = #tseUnidadeEnsino# ">
                                            <gridcolumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                                            </gridcolumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAnoEscolar" Text="Ano Escolar:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnoEscolar" AutoPostBack="true" runat="server" Width="130px"
                                            DataTextField="AnoEscolar" DataValueField="AnoEscolar" OnSelectedIndexChanged="ddlAnoEscolar_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblSemestre" Text="Semestre:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSemestre" runat="server" DataValueField="periodo" AutoPostBack="false"
                                            DataTextField="id_reduzida" Width="130px" />
                                    </td>
                                    <td>
                                        <td align="right">
                                            <asp:ImageButton ID="btBuscar" runat="server" ImageUrl="~/Images/bot_buscar.png"
                                                OnClick="btBuscar_Click" />
                                        </td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Label ID="lblMensagem" runat="server" ClientInstancename="lblMensagem" SkinID="lblMensagem"></asp:Label>
                            <br />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <dxwgv:ASPxGridView ID="grdMeusAlunos" ClientInstanceName="grdMeusAlunos" AutoGenerateColumns="False"
                Visible="true" EnableViewState="false" runat="server" KeyFieldName="ID" DataSourceID="odsAlunos"
                EnableCallBacks="false" OnCustomUnboundColumnData="grdMeusAlunos_CustomUnboundColumnData">
                <settingsbehavior allowmultiselection="TRUE" allowfocusedrow="false" />
                <settings showfilterrow="true" showfilterrowmenu="true" />
                <settingsediting mode="Inline" />
                <settingspager pagesize="35" />
                <columns>
                    <dxwgv:GridViewDataHyperLinkColumn VisibleIndex="27" Caption="Detalhes" FieldName="detalhes"
                        Width="35px" UnboundType="Integer">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="javascript:popupItens({0});" Text="Detalhes">
                        </PropertiesHyperLinkEdit>
                    </dxwgv:GridViewDataHyperLinkColumn>
                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="28" Visible="false"
                        Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="ARQUIVO LIBERADO" FieldName="ARQUIVO" VisibleIndex="35"
                        Visible="false" Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="DOCUMENTOCERTID" FieldName="DOCUMENTOCERTID"
                        VisibleIndex="36" Visible="false" Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="TIPO_CONCLUSAO_ID" FieldName="TIPO_CONCLUSAO_ID"
                        VisibleIndex="37" Visible="false" Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="TIPO_DOCUMENTO_ID" FieldName="TIPO_DOCUMENTO_ID"
                        VisibleIndex="38" Visible="false" Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataCheckColumn Caption="Autorizado" FieldName="AUTORIZADO" Name="AUTORIZADO"
                        VisibleIndex="0" Width="15px">
                        <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                            ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="Não">
                        </PropertiesCheckEdit>
                    </dxwgv:GridViewDataCheckColumn>
                    <dxwgv:GridViewDataCheckColumn Caption="Download" FieldName="Download" Name="Download"
                        VisibleIndex="1" Visible="false" Width="05px">
                        <DataItemTemplate>
                            <asp:Button ID="btnBaixar2" Text="Baixar" runat="server" />
                        </DataItemTemplate>
                    </dxwgv:GridViewDataCheckColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Cad. Complt" FieldName="CADASTRO_COMPLETO"
                        VisibleIndex="2" Visible="false" Width="10px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="3"
                        Visible="true" Width="15px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Conclusão" FieldName="TIPO_CONCLUSAO"
                        VisibleIndex="8" Visible="true" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Certificado" FieldName="TIPO_DOCUMENTO"
                        VisibleIndex="7" Visible="true" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="NOMEDOALUNO" VisibleIndex="4"
                        Visible="true" Width="250px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="9"
                        Visible="true" Width="65px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" VisibleIndex="10"
                        Visible="false" Width="15px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nº" FieldName="NUMERO" VisibleIndex="11" Visible="false"
                        Width="05px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Fls" FieldName="FOLHAS" VisibleIndex="12"
                        Visible="false" Width="05px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Lvr" FieldName="LIVRO" VisibleIndex="13" Visible="false"
                        Width="5px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Org. Emissor" FieldName="ORGÃO_EMISSOR" VisibleIndex="15"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="UE. Ato" FieldName="UE_ATO" VisibleIndex="16"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="UE. Dt Ato" FieldName="UE_DT" VisibleIndex="30"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="N. Doc" FieldName="NUMERO_DOC" VisibleIndex="17"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="UF. Exped" FieldName="UF_EXPEDICAO" VisibleIndex="18"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tp. Doc" FieldName="TIPO_DOC_RG" VisibleIndex="19"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Dt. Nasc" FieldName="DATANASCIMENTO" VisibleIndex="20"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Munip. Nasc" FieldName="MUNICIPIO_NASC" VisibleIndex="21"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="UF. Nasc" FieldName="UF_NASC" VisibleIndex="22"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="País Nasc" FieldName="PAIS_NASC" VisibleIndex="23"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Mãe" FieldName="NOME_MAE" VisibleIndex="24"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Pai" FieldName="NOME_PAI" VisibleIndex="25"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Ato Curso" FieldName="ATO_CURSO" VisibleIndex="26"
                        Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Dt. Implant Curso" FieldName="DT_DO_CURSO"
                        VisibleIndex="27" Visible="false" Width="25px">
                    </dxwgv:GridViewDataTextColumn>
                </columns>
                <settings showfilterrow="True" showfilterrowmenu="true" />
                <clientsideevents selectionchanged="function(s, e) { SelChanged(s, e); }" />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsAlunos" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.ValidaInfCertificacao"
                InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete" SelectMethod="listarAlunos">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlAnoEscolar" Name="AnoEscolar" PropertyName="SelectedValue"
                        Type="String" />
                    <asp:ControlParameter ControlID="tseCurso2" Name="curso" PropertyName="DBValue" />
                    <asp:ControlParameter ControlID="tseUnidadeEnsino" Name="unidade_ens" PropertyName="DBValue" />
                    <asp:ControlParameter ControlID="ddlSemestre" Name="Semestre" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div style="visibility: hidden">
                <asp:Button ID="btnAtualizar" runat="server" OnClick="btnAtualizar_Click" />
                <input id="txtRow" type="hidden" runat="server" />
            </div>
            <dxpc:ASPxPopupControl ID="pucItemHistorico" runat="server" CloseAction="MouseOut"
                Width="784px" HeaderText="Itens do histórico" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalOffset="80" PopupVerticalAlign="Middle" AllowDragging="True" ClientInstanceName="pucItemHistorico"
                EnableAnimation="False" EnableViewState="False" ShowCloseButton="true">
                <border bordercolor="Gainsboro" borderstyle="Ridge" borderwidth="4px" />
                <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
                <contentcollection>
                    <dxpc:PopupControlContentControl ID="puccItemHistorico" runat="server" Visible="true">
                        <asp:Panel ID="Panel1" GroupingText="Dados do Aluno" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label3" runat="server" Text="Nome do Aluno: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeAluno" runat="server" ReadOnly="true" Width="400px" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblmatricula" runat="server"
                                            Text="Matricula: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtmatricula" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblNomemae" runat="server" Text="Nome da Mãe: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomemae" runat="server" MaxLength="200" ReadOnly="true" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblNomepai" runat="server" Text="Nome do Pai: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomepai" runat="server" MaxLength="200" ReadOnly="true" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblRg" runat="server" Text="Nº do RG: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNRg" runat="server" Width="98px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblOrgaoemissor" runat="server" Text="Orgão Emissor: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtorgaoemissor" runat="server" Width="181px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblufexpedicao" runat="server" Text="UF EXPEDIÇÃO: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtufexpedicao" runat="server" Width="23px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbldtnascimento" runat="server" Text="Data Nascimento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdtnascimento" runat="server" Width="60px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMunicipioNascimento" runat="server" Text="Município Nascimento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipioNascimento" runat="server" Width="98px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUFNascimento" runat="server" Text="UF Nascimento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUFNascimento" runat="server" Width="23px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblPaisNascimento" runat="server" Text="País Nascimento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaisNascimento" runat="server" Width="98px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnRepresentante" GroupingText="Unidade de Ensino" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblNomeue" runat="server"
                                            Text="Nome:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeue" runat="server" Width="300px" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblatocriacaoue" runat="server"
                                            Text="Ato de Criação:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtatocriacaoue" runat="server" Width="95px" MaxLength="14" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label1" runat="server" Text="Data do Ato:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdtatocriacaoue" runat="server" Width="58px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pncurso" GroupingText="Informação do Curso" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblnomecurso" runat="server"
                                            Text="Nome:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnomecurso" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblncurso" runat="server"
                                            Text="Nº:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnumerocurso" runat="server" SkinID="numerico" MaxLength="8" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                            Text="Ato de Criação:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtatodecriacaocurso" runat="server" MaxLength="20" Width="172px"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" runat="server" Text="Data do Ato:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdtAtodecriacaocurso" runat="server" Width="58px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlivro" GroupingText="Informações do Livro" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Tipo de Conclusão:"
                                            SkinID="lbltipoconclusao"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttipoconclusao" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="Tipo do Documento:"
                                            SkinID="lblTipoDocumento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTipoDocumento" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lbltipoconclusao" runat="server"
                                            Text="Nº:" SkinID="lbltipoconclusao"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumeroDocumento" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFolhaDocumento" runat="server"
                                            Text="Folha:" SkinID="lblfolhadocumento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFolhaDocumento" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lbllivrocumento" runat="server"
                                            Text="Livro:" SkinID="lbllivrocumento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLivroDocumento" runat="server" ReadOnly="true" Wrap="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlLiberacao" GroupingText="Autorizar e fazer Download" runat="server">
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label Font-Names="Verdana" Font-Size="Medium" ID="lblmsgautorizado" runat="server"
                                            Text="Para que o documento seja autorizado, todos os dados dessa janela devem estar completos."
                                            SkinID="lblInfAluno" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label Font-Names="Verdana" Font-Size="Small" ID="lblcadastroCompleto" runat="server"
                                            Text="" SkinID="lbllivrocumento"></asp:Label>
                                        <asp:Label Font-Names="Verdana" Font-Size="Small" ID="lblautorizado" runat="server"
                                            Text="Autorizado:" SkinID="lbllivrocumento"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAutorizar" Text="Autorizar Emissão" runat="server" OnClick="btnAutorizar_Click"
                                            OnClientClick="return confirm('Deseja prosseguir com a autorização para geração do documento de certificação para esse aluno?');" />
                                        <asp:Button ID="btnBaixar" Text="Fazer Download" runat="server" OnClick="btnBaixar_Click" />
                                        <asp:Button ID="btnsegundavia" Text="Solicitar 2ª via" runat="server" OnClick="btnsegundavia_Click"
                                            OnClientClick="return confirm('Deseja realmente solicitar a segunda via do documento? O documento atual será excluído para que possa ser gerado uma nova via com informações atualizadas.');" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label Font-Names="Verdana" Font-Size="Small" ID="lblautorizadomsg" runat="server"
                                            Text="" SkinID="lbllivrocumento"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlcorrecao" GroupingText="Corrigir informações" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label Font-Names="Verdana" Font-Size="Medium" ID="lblInfAluno" runat="server"
                                            Text="Para editar as informações acima, favor procurar os setores responsáveis. "
                                            SkinID="lblInfAluno"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxpc:PopupControlContentControl>
                </contentcollection>
            </dxpc:ASPxPopupControl>
            <dxpc:ASPxPopupControl ID="ppcLoading" ClientInstanceName="ppcLoading" runat="server"
                Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
                ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" DisappearAfter="60" Cursor="wait" EnableAnimation="false"
                Width="150px">
                <border bordercolor="Gainsboro" borderstyle="Ridge" borderwidth="4px" />
                <contentcollection>
                    <dxpc:PopupControlContentControl>
                        <div style="text-align: center">
                            <asp:Image ID="Image2_" ImageUrl="~/App_Themes/Blue/GridView/gvLoadingOnStatusBar.gif"
                                runat="server" Visible="false" />
                            <asp:Label ID="Label2" Text="Carregando dados..." Font-Bold="true" runat="server" />
                            <br />
                            <br />
                            <asp:Image ID="Image1" ImageUrl="~/App_Themes/Blue/GridView/Loading.gif" runat="server" />
                        </div>
                    </dxpc:PopupControlContentControl>
                </contentcollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
