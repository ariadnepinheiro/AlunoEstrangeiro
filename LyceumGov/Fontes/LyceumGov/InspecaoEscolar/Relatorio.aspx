<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Relatorio.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.Relatorio" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <%--<link href="../Styles/InspecaoEscolar.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">

        function somenteNumeros(e) {
            var charCode = (e.which) ? e.which : e.keyCode;

            if (e.target.value.length >= 10) return false;

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function mascaraData(campo) {
            var v = campo.value;
            if (v.length >= 10) return false;
            if (v.length == 2 || v.length == 5) {
                campo.value = v + '/';
            }
            return true;
        }
        
        function validarCampoData(campo) {
            if (campo.value && !dataValida(campo.value)) {
                alert("Data inválida!");
                campo.value = "";
                campo.focus();
                return false;
            }
            return true;
        }

        function dataValida(data) {
            const regex = /^\d{2}\/\d{2}\/\d{4}$/;
            if (!regex.test(data)) return false;

            const partes = data.split("/");
            const dia = parseInt(partes[0], 10);
            const mes = parseInt(partes[1], 10);
            const ano = parseInt(partes[2], 10);

            if (mes < 1 || mes > 12) return false;

            const diasMes = [31, (ehBissexto(ano) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

            if (dia < 1 || dia > diasMes[mes - 1]) return false;

            return true;
        }

        function ehBissexto(ano) {
            return (ano % 4 === 0 && ano % 100 !== 0) || (ano % 400 === 0);
        }

        $(document).on("blur", ".campo-data", function () {
            if (this.value && !dataValida(this.value)) {
                alert("Data inválida!");
                this.value = "";
                this.focus();
            }
        });

        function desmarcartodos(obj) {
            var nome = $(obj).attr('data-codigo');

            if (confirm('Tem certeza que deseja limpar todas as opções da dependência ' + nome + ' ?')) {
                // console.log(obj);
                // $('#ctl00_cphFormulario_pcRelatorio_rpSalasdeAulas_ctl00_Button2').attr('data-codigo');


                // alert(nome);

                var DivID = '#LINHA_' + nome + ' TD';
                //  alert(DivID);

                //$(DivID).find("select").prop("selectedIndex", 2);
                //$(DivID).find("select").val('5');
            }
            else {

                //$('#LINHA_SL-11 TD').find("select").prop("selectedIndex", 2);

                // Make sure function has run all the way to the end
                // alert('Cancelado');

            }
            return false;
        }

        function marcar(sala, opcaoassuntoid, resposta, respostadepid, respostadepopcid) {

            //CPID17 &OPASTIDID23


            var DivID = '#LINHA_' + sala + ' TD'; //passo a sala
            // console.log(DivID);

            var teste = $(DivID).find("select[data-opcaoassuntoid='" + opcaoassuntoid + "']"); //procuro o dropdown pelo opcaoassuntoid

            teste.val(resposta); //passo a resposta
            teste.attr('data-respostadepid', respostadepid);
            teste.attr('data-respostadepopcid', respostadepopcid);

            console.log(teste.length);

            return false;
        }

        function setarNameNoControle(controle) {
            controle.attr("name", controle.data("guid"));
        }
        
    </script>

    <%--<script language="javascript">
        $(document).ready(function() {
            $("select[data-guid!='']").each(function(i, el) {
                setarNameNoControle($(el));
                //console.log(el);
            });
        });
    </script>--%>

    <script language="javascript" type="text/javascript">
        function addBands() {

            /*
            Obs.: o ID da coluna no HTML de resposta da página muda quando se altera quaisquer propriedades do grdTranscrição.
            Ex.: "#ctl00_cphFormulario_grdTranscricao_DXTcol" -> o "DXT" do nome, que vem antes do "col", é conforme as propriedades
            do grid, então essa parte do nome pode mudar.
            */

            window.setTimeout(function() {

                $("#ctl00_cphFormulario_grdTranscricao_DXHeadersRow")
                            .clone()
                            .attr("id", "topHeader")
                            .css("height", "400px")
                            .insertBefore("#ctl00_cphFormulario_grdTranscricao_DXHeadersRow");

                for (i = 0; i <= 3; i++)
                    $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + i).html("&nbsp;");

                var ceId = [];
                var ceDescricao = [];
                var ceColspan = [];
                var ceStartIndex = [];
                var oldCeId = 0;
                var ceIndex = -1;
                for (i = 4; i <= grdTranscricao.columns.length - 1; i++) {
                    var control = $(grdTranscricao.GetRow(0).cells[i]).find(":input");
                    var componenteEtapaId = control.attr("data-assunto-id");
                    var descricaoComponente = control.attr("data-assunto-descricao");
                    if (componenteEtapaId) {
                        if (oldCeId == componenteEtapaId) {
                            ceColspan[ceIndex]++;
                        } else {
                            oldCeId = componenteEtapaId;
                            ceId.push(componenteEtapaId);
                            ceDescricao.push(descricaoComponente);
                            ceColspan.push(1);
                            ceStartIndex.push(i);
                            ceIndex = ceId.length - 1;
                        }
                    }
                }

                for (i = 0; i < ceId.length; i++) {
                    ceColspan[i] += 2;
                }

                var colspan = 4;
                for (i = 0; i < ceId.length; i++) {
                    for (j = ceStartIndex[i]; j < ceStartIndex[i] + ceColspan[i]; j++) {
                        if (j == ceStartIndex[i]) {
                            $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + j)
                                        .attr("colspan", ceColspan[i])
                                        .css("text-align", "left")
                                        .html(ceDescricao[i]);
                        }
                        else
                            $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + j).remove();
                    }
                }

            }, 100);
        }
    </script>

    <script language="javascript" type="text/javascript">

        function desmarcarTodos(obj) {

            if (confirm('Tem certeza que deseja marcar todas as opções desta dependência como "Não se Aplica"?')) {
                $(obj).parent().parent().find("select[data-nao-se-aplica='1']").val("5");
            }
            return false;
        }

        function desmarcarTodosEmDemaisDependencias(obj) {


            if ($(obj).val() == "")
                return;

            if ($(obj).val() == "-1")
                return;

            //            if ($(obj).val() == "6") {
            //                alert($(obj).val());
            //            }
            var valor = $(obj).val();
            // alert(valor);

            if (valor == "6") {

                if (confirm('Tem certeza que deseja marcar todas as opções desta dependência como "Espaço Inexistente"?')) {
                    $(obj).parent().parent().parent().find("select").val("6");
                }
                else {
                    $(obj).val("-1");
                }
            }
            else if (valor == "5") {

                if (confirm('Tem certeza que deseja marcar todas as opções desta dependência como "Não se Aplica"?')) {
                    $(obj).parent().parent().parent().find("select").val("5");
                }
                else {
                    $(obj).val("-1");
                }

            }


            return false;
        }

        function desmarcarNaoSeAplicaEmDemaisDependencias(obj) {

            //pega o valor do não se aplica selecionado
            var valornaoseaplica = $(obj).parent().parent().parent().find("select[data-nao-se-aplicaa='1']").val();
            //pega o valor da opção escolhida
            var valor = $(obj).val();

            if (valornaoseaplica == "-1")
                return false;

            if (valornaoseaplica == "5") {

                if ($(obj).val() != "5") {

                    $(obj).parent().parent().parent().find("select[data-nao-se-aplicaa='1']").val("-1");
                }

            }


            if (valornaoseaplica == "6") {

                if ($(obj).val() != "6") {

                    $(obj).parent().parent().parent().find("select[data-nao-se-aplicaa='1']").val("-1");
                }

            }

        }


        function desmarcarTodosEmDemaisDependenciasParaCheckbox(obj) {
            if ($(obj).val() == "")
                return;

            if ($(obj).prop("checked") == false) {
                $(obj).prop("checked", false);
                return;
            }

            if (confirm('Tem certeza que deseja desmarcar as opções anteriores desta pergunta?')) {
                $(obj).parent().parent().parent().parent().find("input[type='checkbox']").prop("checked", false);
            }
            else {
                $(obj).prop("checked", false);
                return false;

            }


            $(obj).prop("checked", true);
            return false;
        }

        function desmarcarNaoSeAplicaEmCheckbox(obj) {
            $(obj).parent().parent().parent().parent()
                .find("span[data-nao-se-aplica='1']")
                .find("input[type='checkbox']")
                .prop("checked", false);

        }

    </script>

    <asp:UpdatePanel ID="UpdateDados" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                                OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                                GridWidth="850px" SqlOrder="nome_comp" AutoPostBack="true">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                                    <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCampanha" runat="server" Text="Campanha:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseCampanha" runat="server" Key="campanhaid" Argument="titulo"
                                OnChanged="tseCampanha_Changed" MaxLength="8" SqlSelect="SELECT campanhaid ,titulo, Convert(varchar(4), ano) ano, Convert(varchar(1), semestre)semestre from inspecaoescolar.campanha"
                                GridWidth="850px" SqlOrder="titulo" DataType="Number" AutoPostBack="true">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="codigo" FieldName="campanhaid" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Titulo" FieldName="titulo" Width="70%" />
                                    <tweb:TSearchBoxColumn Caption="Ano" FieldName="ano" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Semestre" FieldName="semestre" Width="10%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <div class="divEditBlock">
                <asp:Label runat="server" ID="lblBloco" Text="Relatório de Trabalho de Infraestrutura"
                    SkinID="BcTitulo" />
                <asp:ImageButton ID="btnFinalizar" runat="server" SkinID="BcFinalizar" OnClick="btnFinalizar_Click"
                    ToolTip="Finalizar Campanha" OnClientClick="return confirm('Após a finalização do Relatório de Trabalho de Infraestrutura não será possível efetuar alterações. Deseja prosseguir com a finalização?');" />
                <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
                <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancelar_Click" />
            </div>
            <div>
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <asp:HiddenField ID="hdnConsideracoesGerais" runat="server" />
            </div>
            <br />
            <asp:HiddenField ID="hdnCampanhaEscolaId" runat="server" />
            <asp:HiddenField ID="hdnAcervoId" runat="server" />
            <dxtc:ASPxPageControl ID="pcRelatorio" runat="server" ActiveTabIndex="0" Visible="true"
                Width="100%">
                <TabPages>
                    <%-- Sala de Aula--%>
                    <dxtc:TabPage Text="Sala de Aula">
                        <ContentCollection>
                            <dxw:ContentControl ID="ContentControl1" runat="server">
                                <asp:Panel ID="pnlSalaAula" runat="server">
                                    <dxwgv:ASPxGridView ID="grdTranscricao" runat="server" AutoGenerateColumns="False"
                                        ClientInstanceName="grdTranscricao" KeyFieldName="ALUNO" Width="100%" EnableViewState="false"
                                        OnPreRender="grdTranscricao_PreRender" OnLoad="grdTranscricao_Load">
                                        <Settings ShowFilterRow="false" ShowFilterRowMenu="false" UseFixedTableLayout="true"
                                            ShowHorizontalScrollBar="true" ShowVerticalScrollBar="false" ShowColumnHeaders="true" />
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                                            AllowGroup="false" AllowSort="false" />
                                        <SettingsPager Mode="ShowAllRecords" />
                                        <Columns>
                                            <dxwgv:GridViewDataColumn FieldName="RESPOSTADEPENDENCIAID" Visible="false" />
                                            <dxwgv:GridViewDataColumn Caption="Bloco" FieldName="EDIFICACAO" VisibleIndex="1"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="Andar" FieldName="PAVIMENTO" VisibleIndex="2"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="Sala" FieldName="DEPENDENCIA" VisibleIndex="3"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="" FieldName="" VisibleIndex="4" Width="105px"
                                                Settings-AllowSort="False">
                                                <DataItemTemplate>
                                                    <asp:Button ID="btnNaoSeAplica" runat="server" data-codigo=' <%# Eval("DEPENDENCIA") %>'
                                                        Text="NÃO SE APLICA" OnClientClick="return desmarcarTodos(this);" />
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                        Visible="true" />
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Banheiro--%>
                    <dxtc:TabPage Text="Banheiros e Vestiários">
                        <ContentCollection>
                            <dxw:ContentControl ID="ContentControl2" runat="server">
                                <asp:Panel ID="pnlBanheiroVestiario" runat="server">
                                    <dxwgv:ASPxGridView ID="grdBanheiro" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdBanheiro"
                                        KeyFieldName="ALUNO" Width="100%" EnableViewState="false" OnPreRender="grdBanheiro_PreRender"
                                        OnLoad="grdBanheiro_Load">
                                        <Settings ShowFilterRow="false" ShowFilterRowMenu="false" UseFixedTableLayout="true"
                                            ShowHorizontalScrollBar="true" ShowVerticalScrollBar="false" ShowColumnHeaders="true" />
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                                            AllowGroup="false" AllowSort="false" />
                                        <SettingsPager Mode="ShowAllRecords" />
                                        <Columns>
                                            <dxwgv:GridViewDataColumn Caption="Bloco" FieldName="EDIFICACAO" VisibleIndex="0"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="Andar" FieldName="PAVIMENTO" VisibleIndex="1"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="Banheiro " FieldName="DEPENDENCIA" VisibleIndex="2"
                                                Width="50px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="Tipo Banheiro" FieldName="TIPO_BANHEIRO" VisibleIndex="3"
                                                Width="150px" Settings-AllowSort="False" />
                                            <dxwgv:GridViewDataColumn Caption="" FieldName="" VisibleIndex="3" Width="105px"
                                                Settings-AllowSort="False">
                                                <DataItemTemplate>
                                                    <asp:Button ID="btnNaoSeAplica" runat="server" data-codigo=' <%# Eval("DEPENDENCIA") %>'
                                                        Text="NÃO SE APLICA" OnClientClick="return desmarcarTodos(this);" />
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                    <asp:Button ID="btnSalvar_BanheiroeVestiarios" runat="server" Text="Salvar" OnClick="btnSalvar_BanheiroeVestiarios_Click"
                                        Visible="true" />
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba1 Condições de Acesso 01--%>
                    <dxtc:TabPage Text="Condições de Acesso 01">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A1" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A1" runat="server">
                                    <div id="dDemaisDependnenciasG1A1">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A1" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A1_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A1" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A1" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A1_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A1" runat="server" OnClick="btnDemaisDependenciasG1A1_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba2 Condições de Acesso 02--%>
                    <dxtc:TabPage Text="Condições de Acesso 02">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A2" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A2" runat="server">
                                    <div id="dDemaisDependnenciasG1A2">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A2" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A2" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A2" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A2_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A2" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A2" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A2" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A2" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A2" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A2_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A2" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A2" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A2" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A2" runat="server" OnClick="btnDemaisDependenciasG1A2_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba3 Condições de Acesso 03 --%>
                    <dxtc:TabPage Text="Condições de Acesso 03">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A3" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A3" runat="server">
                                    <div id="dDemaisDependnenciasG1A3">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A3" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A3" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A3" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A3" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A3_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A3" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A3" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A3" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A3" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A3" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A3" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A3_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A3" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A3" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A3" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A3" runat="server" OnClick="btnDemaisDependenciasG1A3_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba4 Condições de Acesso 04 --%>
                    <dxtc:TabPage Text="Condições de Acesso 04">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A4" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A4" runat="server">
                                    <div id="dDemaisDependnenciasG1A4">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A4" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A4" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A4" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A4" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A4_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A4" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A4" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A4" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A4" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A4" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A4" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A4_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A4" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A4" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A4" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A4" runat="server" OnClick="btnDemaisDependenciasG1A4_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba5 Condições de Acesso 05--%>
                    <dxtc:TabPage Text="Condições de Acesso 05">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A5" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A5" runat="server">
                                    <div id="dDemaisDependnenciasG1A5">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A5" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A5" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A5" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A5" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A5_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A5" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A5" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A5" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A5" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A5" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A5" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A5_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A5" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A5" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A5" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A5" runat="server" OnClick="btnDemaisDependenciasG1A5_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo1 - Aba6 Condições de Acesso 06--%>
                    <dxtc:TabPage Text="Condições de Acesso 06">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG1A6" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG1A6" runat="server">
                                    <div id="dDemaisDependnenciasG1A6">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG1A6" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG1A6" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG1A6" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG1A6" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG1A6_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG1A6" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG1A6" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG1A6" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG1A6" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG1A6" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG1A6" runat="Server" OnItemDataBound="rpAcaodeDirecaoG1A6_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG1A6" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG1A6" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG1A6" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG1A6" runat="server" OnClick="btnDemaisDependenciasG1A6_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo2- Aba1 Alimentação Escolar --%>
                    <dxtc:TabPage Text="Alimentação Escolar">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG2A1" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG2A1" runat="server">
                                    <div id="dDemaisDependnenciasG2A1">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG2A1" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG2A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG2A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG2A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG2A1_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG2A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG2A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG2A1" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG2A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG2A1" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG2A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG2A1_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG2A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG2A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG2A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG2A1" runat="server" OnClick="btnDemaisDependenciasG2A1_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo3 - Aba1 Tecnologia da Informação 01 --%>
                    <dxtc:TabPage Text="Tecnologia da Informação 01">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG3A1" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG3A1" runat="server">
                                    <div id="dDemaisDependnenciasG3A1">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG3A1" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG3A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG3A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG3A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG3A1_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG3A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG3A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG3A1" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG3A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG3A1" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG3A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG3A1_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG3A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG3A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG3A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG3A1" runat="server" OnClick="btnDemaisDependenciasG3A1_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo3 - Aba2  Tecnologia da Informação 02--%>
                    <dxtc:TabPage Text="Tecnologia da Informação 02">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG3A2" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG3A2" runat="server">
                                    <div id="dDemaisDependnenciasG3A2">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG3A2" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG3A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG3A2" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG3A2" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG3A2_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG3A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG3A2" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG3A2" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG3A2" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG3A2" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG3A2" runat="Server" OnItemDataBound="rpAcaodeDirecaoG3A2_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG3A2" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG3A2" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG3A2" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG3A2" runat="server" OnClick="btnDemaisDependenciasG3A2_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo4 - Situações Excepcionais 01 --%>
                    <dxtc:TabPage Text="Situações Excepcionais 01">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG4A1" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG4A1" runat="server">
                                    <div id="dDemaisDependnenciasG4A1">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG4A1" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG4A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG4A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG4A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG4A1_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG4A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG4A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG4A1" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG4A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG4A1" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG4A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG4A1_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG4A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG4A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG4A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG4A1" runat="server" OnClick="btnDemaisDependenciasG4A1_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo4 - Aba2 Situações Excepcionais 02 --%>
                    <dxtc:TabPage Text="Situações Excepcionais 02">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG4A2" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG4A2" runat="server">
                                    <div id="dDemaisDependnenciasG4A2">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG4A2" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG4A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG4A2" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG4A2" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG4A2_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG4A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG4A2" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG4A2" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG4A2" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG4A2" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG4A2" runat="Server" OnItemDataBound="rpAcaodeDirecaoG4A2_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG4A2" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG4A2" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG4A2" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG4A2" runat="server" OnClick="btnDemaisDependenciasG4A2_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- ACERVO --%>
                    <dxtc:TabPage Text="Inspeção Escolar" runat="server">
                        <ContentCollection>
                            <dxw:ContentControl ID="ContentControl3" runat="server">
                                <asp:Panel ID="pnlAcervo" runat="server" GroupingText="ACERVO DE ESCOLA EXTINTA OU PARALISADA">
                                    <asp:Panel ID="pnlPossuiAcervo" runat="server">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Existe na Unidade de Ensino acervo de outra(s) unidades?* "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblPossuiAcervo" runat="server" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="rblPossuiAcervo_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                                                        <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDadosAcervo" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblTipoInstituicao" runat="server" Text="Tipo da Unidade:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTipoInstituicao" runat="server" AutoPostBack="True" DataTextField="descr"
                                                        AppendDataBoundItems="true" DataValueField="descr" OnSelectedIndexChanged="ddlTipoInstituicao_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblInstituicao" runat="server" Text="Nome da Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearch ID="tseInstituicao" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryInstituicao"
                                                        AutoPostBack="true" OnTextChanged="tseInstituicao_Changed">
                                                        <QueryParameters>
                                                            <asp:ControlParameter ControlID="ddlTipoInstituicao" Name="TIPO_ORIGEM" PropertyName="SelectedValue" />
                                                        </QueryParameters>
                                                    </tweb:TSearch>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblSituacao" runat="server" SkinID="lblObrigatorio" Text="Situação da Unidade encontrada:* "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSituacao" runat="server">
                                                        <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Extinta" Value="Extinta"></asp:ListItem>
                                                        <asp:ListItem Text="Paralisada" Value="Paralisada"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAto" runat="server" Text="Ato de Extinção / Paralisação:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAto" runat="server" MaxLength="100" Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblVolume" runat="server" Text="Volume do Acervo:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVolume" runat="server" MaxLength="3" Width="100px" SkinID="numerico" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblMedida" runat="server" Text="Medida Utilizada:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMedida" runat="server" DataValueField="MEDIDAID" AutoPostBack="FALSE"
                                                        DataTextField="DESCRICAO" Width="300px" Height="16px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <br />
                                    <asp:Panel ID="pnlBotao" runat="server">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAcervo" runat="server" Text="Salvar" OnClick="btnAcervo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlGridAcervo" runat="server">
                                        <dxwgv:ASPxGridView ID="grdAcervo" runat="server" DataSourceID="odsAcervo" KeyFieldName="ACERVOID"
                                            AutoGenerateColumns="false" ClientInstanceName="grdAcervo" OnInitNewRow="grdAcervo_InitNewRow"
                                            OnStartRowEditing="grdAcervo_StartRowEditing" OnRowDeleting="grdAcervo_RowDeleting"
                                            OnRowUpdating="grdAcervo_RowUpdating" Width="800px" OnAfterPerformCallback="grdAcervo_AfterPerformCallback">
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                            <SettingsEditing Mode="InLine" />
                                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                            <SettingsBehavior ConfirmDelete="true" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                                    <DeleteButton Visible="True" Text="Remover">
                                                        <Image Url="../img/bt_exclui2.png" />
                                                    </DeleteButton>
                                                    <EditButton Text="Editar" Visible="True">
                                                        <Image Url="~/img/bt_editar.png" />
                                                    </EditButton>
                                                    <UpdateButton Text="Salvar">
                                                        <Image Url="~/img/bt_salvar.png" />
                                                    </UpdateButton>
                                                    <CancelButton Text="Cancelar">
                                                        <Image Url="~/img/bt_cancelar.png" />
                                                    </CancelButton>
                                                    <ClearFilterButton Text="Limpar" Visible="true">
                                                        <Image Url="~/img/bt_limpa.png" />
                                                    </ClearFilterButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="ACERVOID" Name="ACERVOID" VisibleIndex="1"
                                                    FieldName="ACERVOID" Visible="false">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="INSTITUICAOID" Name="INSTITUICAOID" VisibleIndex="1"
                                                    FieldName="INSTITUICAOID" Visible="false">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Tipo da Unidade" Name="TIPO_ORIGEM" VisibleIndex="1" ReadOnly="true"
                                                    FieldName="TIPO_ORIGEM">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Nome da Unidade" Name="NOME_COMPL" ReadOnly="true"
                                                    VisibleIndex="2" FieldName="UNIDADE" Width="300px">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataComboBoxColumn Caption="Situação da Unidade encontrada*" HeaderStyle-Font-Bold="true"
                                                    FieldName="SITUACAO" VisibleIndex="2" Width="110px">
                                                    <PropertiesComboBox ValueType="System.String" Width="110px">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Extinta" Value="Extinta" />
                                                            <dxe:ListEditItem Text="Paralisada" Value="Paralisada" />
                                                        </Items>
                                                    </PropertiesComboBox>
                                                </dxwgv:GridViewDataComboBoxColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Ato de Extinção / Paralisação*" Name="Ato"
                                                    VisibleIndex="4" FieldName="ATO" Width="200px">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Volume do Acervo*" FieldName="VOLUME" VisibleIndex="5"
                                                    Width="70px">
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                    <PropertiesTextEdit MaxLength="3" Style-HorizontalAlign="Left" Width="150px">
                                                        <ClientSideEvents KeyPress="function(s,e){ SomentePermitirNumeros(s, e.htmlEvent); } " />
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                            <RegularExpression ErrorText="Volume deve ser um número de até 5 dígitos." ValidationExpression="\d{0,5}" />
                                                        </ValidationSettings>
                                                        <Style HorizontalAlign="Left">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataComboBoxColumn Caption="Medida Utilizada*" HeaderStyle-Font-Bold="true"
                                                    FieldName="MEDIDAID" VisibleIndex="6" Width="110px">
                                                    <PropertiesComboBox DataSourceID="odsMedida" TextField="DESCRICAO" ValueField="MEDIDAID"
                                                        ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                </dxwgv:GridViewDataComboBoxColumn>
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </asp:Panel>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Grupo5 - Aba1 Sala de Recursos Multifuncionais --%>
                    <dxtc:TabPage Text="Sala de Recursos Multifuncionais">
                        <ContentCollection>
                            <dxw:ContentControl ID="cDemaisDependenciasG5A1" runat="server">
                                <asp:Panel ID="pnlDemaisDependenciasG5A1" runat="server">
                                    <div id="dDemaisDependnenciasG5A1">
                                        <asp:Repeater ID="rpdemaisdependenciasGrupoG5A1" runat="server">
                                            <ItemTemplate>
                                                <br></br>
                                                <%--Grupo--%>
                                                <asp:Label ID="lblGrupoDemaisG5A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                    Style="font-weight: bold; color: #000000;"></asp:Label>
                                                <br></br>
                                                <asp:HiddenField ID="hdnGrupoIdG5A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                                                <br></br>
                                                <asp:Repeater ID="rpdemaisdependenciasAssuntoG5A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG5A1_ItemDataBound"
                                                    DataSource='<%#Eval("ListaAssunto")%>'>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%-- Assunto--%>
                                                                    <asp:Label ID="lblAssuntoDemaisG5A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                                        Style="font-weight: bold; color: #000000;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnAssuntoIdDemaisG5A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                                        runat="server" />
                                                                    <asp:TextBox ID="txtRespostaDemaisG5A1" runat="server" MaxLength="500" Width="600px"
                                                                        Height="30px"></asp:TextBox>
                                                                    <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG5A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                                        DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                    </asp:RadioButtonList>
                                                                    <asp:CheckBoxList ID="chkRespostaDemaisG5A1" runat="server" DataTextField="Descricao"
                                                                        DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                                                    </asp:CheckBoxList>
                                                                    <tr>
                                                                        <asp:Repeater ID="rpAcaodeDirecaoG5A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG5A1_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAcaoDirecaoDemaisG5A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 135px;">
                                                                                        <asp:HiddenField ID="hdnOpAssuntoIdDemaisG5A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                                        <asp:DropDownList ID="ddlPerguntaDemaisG5A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                                            DataValueField="CODIGO" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tr>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Button ID="btnDemaisDependenciasG5A1" runat="server" OnClick="btnDemaisDependenciasG5A1_Click"
                                            Text="Salvar" />
                                    </div>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%-- Considerações Finais --%>
                    <dxtc:TabPage Text="Considerações Finais">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccConsideracao" runat="server">
                                <asp:Panel ID="pnlConsideracao" runat="server">
                                    <asp:Repeater ID="rpGrupoConsideracao" runat="server" OnItemDataBound="rpGrupoConsideracao_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("Descricao")%>' Style="font-weight: bold;
                                                color: #000000;"></asp:Label>
                                            <asp:HiddenField ID="hdnGrupoConsideracao" Value='<%# Eval("GrupoId") %>' runat="server" />
                                            <asp:Repeater ID="rpAssuntoConsideracao" runat="server" OnItemDataBound="rpAssuntoConsideracao_ItemDataBound"
                                                DataSource='<%#Eval("ListaAssunto")%>'>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Descricao")%>' Style="font-weight: bold"></asp:Label>
                                                                <asp:HiddenField ID="hdnTipoAssuntoTipo" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId") %>'
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtResposta" runat="server" TextMode="MultiLine" MaxLength="500"
                                                                    Width="600px" Height="100px"></asp:TextBox>
                                                                <asp:RadioButtonList runat="server" ID="rdGrupo" DataSource='<%#Eval("ListaOpcao")%>'
                                                                    DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                                                </asp:RadioButtonList>
                                                                <asp:CheckBoxList ID="chkResposta" runat="server" DataTextField="Descricao" DataValueField="valor"
                                                                    DataSource='<%#Eval("ListaOpcao")%>'>
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Button ID="btnConsideracao" runat="server" OnClick="btnConsideracao_Click" Text="Salvar" />
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                </TabPages>
            </dxtc:ASPxPageControl>
            <asp:ObjectDataSource ID="odsCampanhaEscola" TypeName="Techne.Lyceum.Net.InspecaoEscolar.CampanhaEscola"
                runat="server" SelectMethod="ListarCampanhaEscola" DeleteMethod="Deletar">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseCampanha" Name="campanhaid" DefaultValue="" PropertyName="DBValue"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade_ens" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="OdsAcaodeDirecao" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Relatorio"
                runat="server" SelectMethod="ListarAcaoDirecaoDemaisDependencias"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsAcervo" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Relatorio"
                SelectMethod="ListarAcervo" DeleteMethod="Delete" UpdateMethod="Update">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnCampanhaEscolaId" DefaultValue="" Name="campanhaEscolaId"
                        PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsMedida" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Relatorio"
                SelectMethod="ListarMedida"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
