<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CandidatoDocente.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoDocente" %>

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

        $().ready(function() {
            trataCep({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });

        function OnChangeExperiencia() {
            if (grdExperiencia != null) {
                if (cmbExperiencia.GetValue() != null) {
                    cmbExperienciaPontuacao.PerformCallback(cmbExperiencia.GetValue().toString());
                }
            }
        }

        function OnChangeTitulacao() {
            if (grdTitulacao != null) {
                if (cmbTitulacao.GetValue() != null) {
                    cmbTitulacaoPontuacao.PerformCallback(cmbTitulacao.GetValue().toString());
                }
            }
        }

        function OnGridTitulacaoEndCallback(sender) {

            var txtTotalPont = document.getElementById("<%=txtTotalPont.ClientID %>");
            if (typeof (sender) != 'undefined' && sender != null) {
                if (typeof (sender.cpTotal) != 'undefined' && sender.cpTotal != null) {
                    if (typeof (txtTotalPont) != 'undefined' && txtTotalPont != null) {
                        txtTotalPont.innerHTML = sender.cpTotal;
                        grdProcessoSeletivo.PerformCallback();
                    }
                }
            }

        }

        function FinalizarCadastro() {
            var experiencia = $('#<%= this.txtTotalPont.ClientID %>').val();
            var titulacao = $('#<%= this.txtTotalPont.ClientID %>').val();

            if (titulacao == 0) {
                alert("É obrigatória a seleção de uma titulação");
                return false;
            }
            if (confirm("O candidato possui " + titulacao + " titulação(ões) e " + experiencia + " experiência(s) selecionados.Após esta ação, não será mais possível a alteração do cadastro. É obrigatório proceder a impressão como comprovante de inscrição. Deseja continuar?")) {
                return true;
            }
            return false;
        }

        function OnGridExperienciaEndCallback(sender) {

            var txtTotalPont = document.getElementById("<%=txtTotalPont.ClientID %>");
            if (typeof (sender) != 'undefined' && sender != null) {
                if (typeof (sender.cpTotal) != 'undefined' && sender.cpTotal != null) {
                    if (typeof (txtTotalPont) != 'undefined' && txtTotalPont != null) {
                        txtTotalPont.innerHTML = sender.cpTotal;
                        grdProcessoSeletivo.PerformCallback();
                    }
                }
            }
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

        function DesabilitarSubmitPopup() {
            $('#btnSalvarDisciplinas').click(function(e) { this.disabled = true; });
        }
    </script>

    <dxpc:ASPxPopupControl ID="ppcMensagem" runat="server" CloseAction="CloseButton"
        HeaderText="Disciplinas" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        AllowDragging="true" ClientInstanceName="pucItensDespesa" EnableAnimation="true"
        EnableViewState="false" ShowCloseButton="true" Width="520px" Height="380px" HeaderStyle-BackColor="#BFD7F3"
        HeaderStyle-Font-Bold="true" ContentStyle-Font-Bold="true">

<HeaderStyle BackColor="#BFD7F3" Font-Bold="True"></HeaderStyle>

        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
<ContentStyle Font-Bold="True"></ContentStyle>

        <ClientSideEvents Init="OnInitASPxPopupControl" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppcccMensagem" runat="server">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" style="width: 35%">
                            <div class="BottomPadding">
                                <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Disciplinas:" Font-Bold="true"
                                    ForeColor="#004A80"></asp:Label>
                            </div>
                            <br />
                            <dxe:ASPxListBox ID="listFrom" TextField="descricao" ValueField="agrupamento" runat="server"
                                ClientInstanceName="lbAvailable" Width="100%" Height="240px" SelectionMode="CheckColumn">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }" />
                            </dxe:ASPxListBox>
                        </td>
                        <td valign="middle" align="center" style="padding: 10px; width: 30%">
                            <div>
                                <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                                    AutoPostBack="False" Text=">" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Adicionar items selecionados">
                                    <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div class="TopPadding">
                                <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                                    AutoPostBack="False" Text=">>" Width="130px" Height="23px" ToolTip="Adiciona todos os items">
                                    <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div style="height: 32px">
                            </div>
                            <div>
                                <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                                    AutoPostBack="False" Text="<" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Remover items selecionados">
                                    <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div class="TopPadding">
                                <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                                    AutoPostBack="False" Text="<<" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Remover todos os Items">
                                    <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                        </td>
                        <td valign="top" style="width: 35%">
                            <div class="BottomPadding">
                                <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Disciplinas de habilitação:"
                                    Font-Bold="true" ForeColor="#004A80"></asp:Label>
                            </div>
                            <br />
                            <dxe:ASPxListBox ID="listTo" runat="server" ClientInstanceName="lbChoosen" Width="100%"
                                Height="240px" SelectionMode="CheckColumn">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                                </ClientSideEvents>
                            </dxe:ASPxListBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <br />
                    <dxe:ASPxButton ClientInstanceName="btnSalvarDisciplinas" AutoPostBack="true" UseSubmitBehavior="false"
                        ID="btnSalvarDisciplinas" OnClick="btnSalvarDisciplinas_Click" runat="server"
                        Text="Salvar">
                    </dxe:ASPxButton>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <asp:HiddenField ID="hdnTitulacao" runat="server" />
    <asp:HiddenField ID="hdnExperiencia" runat="server" />
    <asp:Panel ID="pnBusca" GroupingText="Faça uma busca por processo seletivo e número de inscrição"
        runat="server" Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBuscaConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao, indigena, ano FROM lY_concurso_docente"
                        SqlWhere="tipo = 'Contrato'" ArgumentColumns="50" Columns="30" MaxLength="20" SqlOrder= " ano desc" GridWidth="800px"
                        OnChanged="tseConcursoBusca_Changed">
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
                    <asp:Label ID="lblBuscaCandidato" runat="server" Text="Número de Inscrição:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidatoBusca" runat="server" Caption="" SqlSelect="SELECT candidato, nome, nucleo FROM LY_CANDIDATO_DOCENTE"
                        SqlWhere="concurso = #tseConcursoBusca#" ArgumentColumns="50" Columns="30" MaxLength="20"
                        SqlOrder="nome" GridWidth="800px" OnChanged="tseCandidatoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right"
            Visible="false" />
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnValidarInscricao" runat="server" SkinID="BcValidarFicha"
            OnClick="btnValidarInscricao_Click" Visible="false" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" OnClientClick="return confirm('Confirma a inscrição?\nNão será possível editar os dados.') " />
        <asp:Label runat="server" ID="lblBlocoCandidatoDocente" Text="Ficha de Inscrição"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsCandidatoDocente" runat="server" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:ObjectDataSource ID="odsProcessoSeletivo" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
        SelectMethod="ConsultarProcessoSeletivo">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="candidato" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsSituacao" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
        SelectMethod="ConsultarSituacaoAvaliacao"></asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsSitAvaliacao" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_candidato_doc_sit_avaliacao"
        SqlWhere="candidato = @candidato and concurso = @concurso">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="candidato" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsCandidatoTitulacoes" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_candidato_doc_titulacoes"
        SqlWhere="Ly_candidato_doc_titulacoes.candidato = @candidato and Ly_candidato_doc_titulacoes.concurso = @concurso"
        SqlOrder="pontuacao">
        <SqlWhereParameters>
            <asp:SessionParameter Name="concurso" SessionField="SSconcurso" />
            <asp:SessionParameter Name="candidato" SessionField="SScandidato" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsCandidatoExperiencia" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_candidato_doc_experiencias"
        SqlWhere="Ly_candidato_doc_experiencias.candidato = @candidato and Ly_candidato_doc_experiencias.concurso = @concurso"
        SqlOrder="pontuacao">
        <SqlWhereParameters>
            <asp:SessionParameter Name="concurso" SessionField="SSconcurso" />
            <asp:SessionParameter Name="candidato" SessionField="SScandidato" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <dxtc:ASPxPageControl ID="pcCandidatoDocente" runat="server" ClientInstanceName="pcCandidatoDocente"
        ActiveTabIndex="0" Width="980px">
        <TabPages>
            <dxtc:TabPage Text="Dados da Inscrição">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDadosInscricao" runat="server">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblCandidato" runat="server" Text="Número de Inscrição: "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblMsgCandidato" runat="server" Text="Número gerado pelo sistema após salvar os dados."></asp:Label>
                                    <asp:TextBox ID="txtCandidato" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtStatusCandidato" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlDadosInscricao" GroupingText="Dados da Inscrição">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseConcurso" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao,indigena FROM lY_concurso_docente"
                                            MaxLength="20" SqlOrder="descricao" GridWidth="800px" OnChanged="tseConcurso_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                                                <tweb:TSearchBoxColumn Caption="Indigena" FieldName="indigena" Visible="false" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:RequiredFieldValidator ToolTip="Processo Seletivo: Preenchimento obrigatório."
                                            ID="rfvConcurso" runat="server" ControlToValidate="tseConcurso" InitialValue=""
                                            ValidationGroup="SalvarForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox runat="server" ID="tseMunicipioProc" SqlSelect="select DISTINCT m.CODIGO ,M.NOME from LY_UNIDADE_ENSINO UE inner join MUNICIPIO M ON UE.MUNICIPIO = M.CODIGO inner join LY_CONCURSO_DOC_HABILITACAO cdh on cdh.MUNICIPIO_PROC=m.CODIGO "
                                            Value='<%# Bind("MUNICIPIO_PROC") %>' SqlWhere=" SIT_FUNCIONAMENTO='EmAtividade' and cdh.Concurso = #tseConcurso#"
                                            MaxLength="20" SqlOrder="NOME" DataType="Varchar" OnChanged="tseMunicipioProc_Changed"
                                            AutoPostBack="true">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Município: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="tseMunicipioProc"
                                            InitialValue="" ValidationGroup="SalvarForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"
                                            ></asp:Label>
                                    </td>
                                    <td>
                                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                AutoPostBack="true" MaxLength="20" Columns="10" Caption="" Key="id_regional" SqlSelect="SELECT DISTINCT  RE.ID_REGIONAL,RE.REGIONAL  FROM LY_CONCURSO_DOCENTE CD INNER JOIN LY_CONCURSO_DOC_HABILITACAO CH ON CD.CONCURSO = CH.CONCURSO INNER JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = CH.REGIONALID " SqlOrder="regional"
                                  SqlWhere="ch.Concurso = #tseConcurso# and ch.MUNICIPIO_PROC = #tseMunicipioProc#"
                                OnChanged="tseRegional_Changed" DataType="Number">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="REGIONAL" FieldName="regional" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                                    
                                      <%--  <tweb:TSearchBox ID="tseCoordenadoria" runat="server" Argument="descricao" Caption=""
                                            Key="nucleo" SqlSelect="SELECT distinct n.nucleo as nucleo, n.descricao as descricao FROM dbo.MUNICIPIO_NUCLEO mn INNER JOIN ly_nucleo n ON mn.nucleoid=n.nucleo inner join LY_CONCURSO_DOC_HABILITACAO ldh on n.nucleo = ldh.nucleo"
                                            SqlWhere="ldh.Concurso = #tseConcurso# and ldh.MUNICIPIO_PROC = #tseMunicipioProc#"
                                            OnChanged="tseCoordenadoria_Changed" Visible="false">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="nucleo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>--%>
                                        <asp:HiddenField runat="server" ID="hdnNucleo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblHabilitacao" runat="server" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbCargo" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO"
                                            Enabled="true" Width="521px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="pnlDadosPessoais" GroupingText="Dados Pessoais">
                            <table>
                                <tr>
                                    <td align="right" style="width: 150px">
                                        <asp:Label ID="lblNomeCompleto" runat="server" Text="Nome Completo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeCompleto" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNomeComl" runat="server" ControlToValidate="txtNomeCompleto"
                                            InitialValue="" ErrorMessage="Nome Completo: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDataNasc" runat="server" Text="Data Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="120px"
                                                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ErrorMessage="Data Nascimento: Preenchimento obrigatório."
                                                        ID="rfvDtNasc" runat="server" ControlToValidate="dtDataNasc" InitialValue=""
                                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblSexo" runat="server" Text="Sexo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td style="height: 30px">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
                                                        Width="150px">
                                                        <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                                        <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ErrorMessage="Sexo: Preenchimento obrigatório." ID="rfvSexo"
                                                        runat="server" ControlToValidate="rblSexo" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNecessidadeEspecial" runat="server" Text="Necessidade Especial:*"
                                            Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbNecessidadeEspecial" runat="server" DataValueField="NECESSIDADEESPECIALID"
                                            AutoPostBack="true" DataTextField="DESCRICAO" Width="200px" OnSelectedIndexChanged="AtualizarDadosCota">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvNecEspecial" runat="server" ControlToValidate="cmbNecessidadeEspecial"
                                            InitialValue="" ErrorMessage="Necessidade Especial: Preenchimento obrigatório."
                                            ValidationGroup="SalvarForm"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label runat="server" ID="lblCorRaca" Text="Etnia:*" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCorRaca" runat="server" Width="100px" Height="20px" DataTextField="nome"
                                            DataValueField="etniaid" OnSelectedIndexChanged="AtualizarDadosCota" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Etnia: Preenchimento obrigatório." ID="rfvEtnia"
                                            runat="server" ControlToValidate="ddlCorRaca" InitialValue="" Display="Dynamic"
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNomeMae" runat="server" Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Nome da Mãe: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNomeMae" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNomePai" runat="server" Text="Nome do Pai:*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="200px"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Nome do Pai: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNomePai" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="200px" Height="20px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Estado Civil: Preenchimento obrigatório."
                                            ID="rfvEst_Civil" runat="server" ControlToValidate="ddlEst_Civil" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPaisNasc" Text="País de Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaisNasc" runat="server" DataTextField="nome" DataValueField="codigo"
                                            AutoPostBack="true" Width="200px" Height="20px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="País de Nascimento: Preenchimento obrigatório."
                                            ID="rfvPaisNasc" runat="server" ControlToValidate="ddlPaisNasc" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                                            AutoPostBack="false" Height="20px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Nacionalidade: Preenchimento obrigatório."
                                            ID="rfvNacionalidade" runat="server" ControlToValidate="ddlNacionalidade" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNaturalidade" runat="server" Text="Naturalidade:*" SkinID="lblObrigatorio"></asp:Label>
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
                                        <asp:RequiredFieldValidator ErrorMessage="Naturalidade: Preenchimento obrigatório."
                                            ID="rfvNaturalidade" runat="server" ControlToValidate="tseNaturalidade" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNaturalidadeUF" Visible="true" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNaturalidadeUF" ReadOnly="true" runat="server" Width="150px"
                                            Height="20px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlCota" runat="server" GroupingText="Cotas Disponíveis">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Cota:*" Font-Names="Verdana" Font-Size="Smaller"
                                            ID="lblCota" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCotas" DataValueField="COTAID" DataTextField="SIGLA">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Cota: Preenchimento obrigatório." ID="rfvCota"
                                            runat="server" ControlToValidate="ddlCotas" InitialValue="" ValidationGroup="SalvarForm">
											<img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="Panel1" GroupingText="Endereço">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false" />
                                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                            Modal="true" SkinID="CEP" />
                                        <asp:RegularExpressionValidator ID="revCEP" ControlToValidate="txtCEP" ValidationExpression="^.{8}$"
                                            runat="server" ErrorMessage="CEP: Preenchimento de oito números obrigatório."
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ErrorMessage="CEP: Preenchimento obrigatório." ID="rfvCEP"
                                            runat="server" ControlToValidate="txtCEP" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                            Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                            GridWidth="600px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
                                            MaxLength="10">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="250px" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Município: Preenchimento obrigatório."
                                            ID="rfvMunicipio" runat="server" ControlToValidate="tseMunicipio" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" Visible="true" runat="server" Text="Estado:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" visible="true" runat="server" maxlength="20" class="txtInput"
                                            readonly="readonly" />
                                        <asp:RequiredFieldValidator ErrorMessage="Estado: Preenchimento obrigatório." ID="rfvEstador"
                                            runat="server" Enabled="false" ControlToValidate="txtEstado" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                            Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                                        Width="400px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEndereco" runat="server" ControlToValidate="txtEndereco"
                                                        InitialValue="" ErrorMessage="Endereço: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                            Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                        <asp:RequiredFieldValidator ErrorMessage="Número: Preenchimento obrigatório." ID="rfvEndNum"
                                            runat="server" ControlToValidate="txtEndNum" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
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
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                            Width="400px" />
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
                                    <td style="width: 415px">
                                        <asp:TextBox ID="txtFone" onkeyup="return formataTelefoneDDD(this,event);" runat="server"
                                            onpaste="return false;" Width="100px" MaxLength="13">
                                        </asp:TextBox>
                                       
                                    </td>
                                    <td align="right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular" runat="server"
                                            Text="Celular:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)"  runat="server"
                                            onpaste="return false;" Width="100px" MaxLength="14">
                                        </asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                                            Text="E-mail:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="600px" MaxLength="100" />
                                        <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        <img src="../Images/AlertaMens.gif" alt="E-mail inválido" />
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel runat="server" ID="pnlDocumentos" GroupingText="Documentos">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Tipo" runat="server"
                                            Text="Tipo:*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                                            Width="150px" OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true"
                                            Height="20px">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Tipo: Preenchimento obrigatório." ID="RequiredFieldValidator6"
                                            runat="server" ControlToValidate="ddlRGTipoPessoa" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                                            Text="Número:*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="15" Width="200px" SkinID="numeroDocumento"
                                            OnKeyPress="return SomenteNumerosLetras(event);" />
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Número: Preenchimento obrigatório." ID="RequiredFieldValidator7"
                                            runat="server" ControlToValidate="txtRGNum" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
                                            Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                                            Width="150px" Height="20px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Estado: Preenchimento obrigatório." ID="rfvEstadoRG"
                                            Enabled="false" runat="server" ControlToValidate="cmbRGUF" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
                                            Text="Órgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                                            Width="200px" Height="20px">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cmbRGEmissor"
                                            InitialValue="" ErrorMessage="Órgão Emissor: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
                                            Text="Data de Expedição:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ErrorMessage="Data de Expedição: Preenchimento obrigatório."
                                                        ID="rfvDataExp" Enabled="false" runat="server" ControlToValidate="dtDataExped"
                                                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCPF" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="50"
                                            Width="150px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCPF"
                                            InitialValue="" ErrorMessage="CPF: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPisPasep" runat="server" Text="PIS / PASEP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPisPasep" runat="server" MaxLength="11" Width="150px" SkinID="numerico" />
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
                                        <asp:TextBox ID="txtCrpof_Num" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblProfSerie" Text="Série:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCprof_Serie" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCProf_DtExpedicao" Text="Data de Expedição:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dboCprof_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCProf_UF" Text="Estado:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDlCprof_Uf" runat="server" DataTextField="sigla" DataValueField="sigla"
                                            Width="120px" Height="20px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Disciplinas de Habilitação" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDisciplinasHabilitacao" runat="server">
                        <asp:ObjectDataSource ID="odsDisciplinasHabilitacao" runat="server" TypeName="Techne.Lyceum.RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao"
                            SelectMethod="ListaDisciplinasHabilitacao" UpdateMethod="AtualizaDisciplinasHabilitacao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                                <asp:ControlParameter ControlID="txtCandidato" PropertyName="Text" Name="candidato" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="habilitado" Type="Boolean" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <table>
                            <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdDisciplinasHabilitacao" runat="server" AutoGenerateColumns="true"
                                        ClientInstanceName="grdDisciplinasHabilitacao" DataSourceID="odsDisciplinasHabilitacao"
                                        KeyFieldName="CANDIDATODOCENTE_GRUPOHABILITACAOID" OnStartRowEditing="grdDisciplinasHabilitacao_StartRowEditing"
                                        OnAfterPerformCallback="grdDisciplinasHabilitacao_AfterPerformCallback" OnCellEditorInitialize="grdDisciplinasHabilitacao_CellEditorInitialize"
                                        OnRowUpdating="grdDisciplinasHabilitacao_RowUpdating" OnCommandButtonInitialize="grdDisciplinasHabilitacao_CommandButtonInitialize"
                                        EnableCallBacks="true">
                                        <SettingsEditing Mode="Inline" />
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px" Name="teste">
                                                <HeaderCaptionTemplate>
                                                    <div style="text-align: center">
                                                        <input type="image" id="btnInserirDisciplinas" src="../img/bt_novo.png" style="cursor: pointer"
                                                            onserverclick="HabilitaPopUpInsercao" runat="server" />
                                                    </div>
                                                </HeaderCaptionTemplate>
                                                <EditButton Text="Editar" Visible="True">
                                                    <Image Url="~/img/bt_editar.png" />
                                                </EditButton>
                                                <CancelButton Text="Cancelar">
                                                    <Image Url="~/img/bt_cancelar.png" />
                                                </CancelButton>
                                                <UpdateButton Text="Alterar">
                                                    <Image Url="~/img/bt_salvar.png" />
                                                </UpdateButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataTextColumn FieldName="CANDIDATODOCENTE_GRUPOHABILITACAOID" Caption="DisciplinasHabilitacaoId"
                                                VisibleIndex="1" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn FieldName="CANDIDATO" Caption="Candidato" VisibleIndex="1"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn FieldName="AGRUPAMENTO" Caption="Agrupamento" VisibleIndex="1"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Disciplinas" VisibleIndex="2"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataCheckColumn FieldName="HABILITADO" Caption="Habilitadas" VisibleIndex="3"
                                                Width="120px" Visible="true">
                                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                                </PropertiesCheckEdit>
                                            </dxwgv:GridViewDataCheckColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Titulações / Experiências" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccTitulacaoExperiencia" runat="server">
                        <asp:ObjectDataSource ID="odsTitulacao" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                            SelectMethod="ConsultarTitulacao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsExperiencia" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                            SelectMethod="ConsultarExperiencia">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblObservacaoHabilitacao" ForeColor="Red" runat="server" Text="Obs.: É obrigatório ter a titulação 'Licenciatura Plena em Pedagogia' com Habilitação para lecionar nos anos iniciais do Ensino Fundamental e/ou do Ensino Médio na modalidade normal para poder participar do processo seletivo para Professor para atuar nos anos iniciais do Ensino Fundamental."></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdTitulacao" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="true" ClientInstanceName="grdTitulacao" Width="678px" DataSourceID="tdsCandidatoTitulacoes"
                                        OnCellEditorInitialize="grdTitulacao_CellEditorInitialize" KeyFieldName="concurso;candidato;titulacao"
                                        OnRowInserting="grdTitulacao_RowInserting" OnCustomJSProperties="grdTitulacao_CustomJSProperties"
                                        OnAfterPerformCallback="grdTitulacao_AfterPerformCallback">
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Settings ShowFooter="true" />
                                        <ClientSideEvents EndCallback="function(s,e){OnGridTitulacaoEndCallback(s);}" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="70px">
                                                <HeaderCaptionTemplate>
                                                    <div style="text-align: center">
                                                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                            onclick="grdTitulacao.AddNewRow();" alt="Novo" />
                                                    </div>
                                                </HeaderCaptionTemplate>
                                                <EditButton Text="Editar" Visible="True">
                                                    <Image Url="~/img/bt_editar.png" />
                                                </EditButton>
                                                <DeleteButton Text="Remover" Visible="True">
                                                    <Image Url="~/img/bt_exclui2.png" />
                                                </DeleteButton>
                                                <CancelButton Text="Cancelar">
                                                    <Image Url="~/img/bt_cancelar.png" />
                                                </CancelButton>
                                                <UpdateButton>
                                                    <Image Url="~/img/bt_salvar.png" />
                                                </UpdateButton>
                                                <ClearFilterButton Text="Limpar" Visible="True">
                                                    <Image Url="~/img/bt_limpa.png" />
                                                </ClearFilterButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Titulação" FieldName="titulacao" VisibleIndex="1"
                                                Width="250px">
                                                <PropertiesComboBox DataSourceID="odsTitulacao" TextField="descricao" ValueField="titulacao"
                                                    ValueType="System.String" ClientInstanceName="cmbTitulacao">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnChangeTitulacao(s); }" />
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor selecionar a titulação." IsRequired="True" />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Pontuação" FieldName="pontuacao" ReadOnly="True"
                                                VisibleIndex="2">
                                                <PropertiesComboBox ValueType="System.String" ClientInstanceName="cmbTitulacaoPontuacao"
                                                    EnableAnimation="False">
                                                    <DropDownButton Visible="False">
                                                    </DropDownButton>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                        </Columns>
                                        <TotalSummary>
                                            <dxwgv:ASPxSummaryItem FieldName="pontuacao" SummaryType="Sum" DisplayFormat="Total Titulação: {0}" />
                                        </TotalSummary>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdExperiencia" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="true" DataSourceID="tdsCandidatoExperiencia" ClientInstanceName="grdExperiencia"
                                        Width="678px" OnCellEditorInitialize="grdExperiencia_CellEditorInitialize" OnCustomJSProperties="grdExperiencia_CustomJSProperties"
                                        OnRowInserting="grdExperiencia_RowInserting" KeyFieldName="concurso;candidato;experiencia"
                                        OnAfterPerformCallback="grdExperiencia_AfterPerformCallback" OnCustomColumnDisplayText="grdExperiencia_CustomColumnDisplayText">
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Settings ShowFooter="true" />
                                        <ClientSideEvents EndCallback="function(s,e){OnGridExperienciaEndCallback(s);}" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="70px">
                                                <HeaderCaptionTemplate>
                                                    <div style="text-align: center">
                                                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                            onclick="grdExperiencia.AddNewRow();" alt="Novo" />
                                                    </div>
                                                </HeaderCaptionTemplate>
                                                <EditButton Text="Editar" Visible="True">
                                                    <Image Url="~/img/bt_editar.png" />
                                                </EditButton>
                                                <DeleteButton Text="Remover" Visible="True">
                                                    <Image Url="~/img/bt_exclui2.png" />
                                                </DeleteButton>
                                                <CancelButton Text="Cancelar">
                                                    <Image Url="~/img/bt_cancelar.png" />
                                                </CancelButton>
                                                <UpdateButton>
                                                    <Image Url="~/img/bt_salvar.png" />
                                                </UpdateButton>
                                                <ClearFilterButton Text="Limpar" Visible="True">
                                                    <Image Url="~/img/bt_limpa.png" />
                                                </ClearFilterButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Experiência" FieldName="experiencia" VisibleIndex="1"
                                                Width="250px">
                                                <PropertiesComboBox DataSourceID="odsExperiencia" TextField="descricao" ValueField="experiencia"
                                                    ValueType="System.String" ClientInstanceName="cmbExperiencia">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnChangeExperiencia(); }" />
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor selecionar a experiência." IsRequired="True" />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Pontuação" FieldName="pontuacao" ReadOnly="True"
                                                VisibleIndex="2">
                                                <PropertiesComboBox ValueType="System.String" ClientInstanceName="cmbExperienciaPontuacao"
                                                    EnableAnimation="False">
                                                    <DropDownButton Visible="False">
                                                    </DropDownButton>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Concurso" FieldName="concurso" ReadOnly="True"
                                                Visible="False" VisibleIndex="3">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Candidato" FieldName="candidato" ReadOnly="True"
                                                Visible="False" VisibleIndex="3">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <TotalSummary>
                                            <dxwgv:ASPxSummaryItem FieldName="pontuacao" SummaryType="Sum" DisplayFormat="Total Experiência: {0}" />
                                        </TotalSummary>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <table align="right">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblTotalPont" runat="server" Text="Pontuação Total: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtTotalPont" runat="server" SkinID="lblNomePagina" Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Processo seletivo" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccProcessoSeletivo" runat="server">
                        <dxwgv:ASPxGridView ID="grdProcessoSeletivo" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdProcessoSeletivo" DataSourceID="odsProcessoSeletivo" OnCustomCallback="grdProcessoSeletivo_CustomCallback"
                            OnAfterPerformCallback="grdProcessoSeletivo_AfterPerformCallback" EnableCallBacks="true">
                            <SettingsEditing Mode="Inline" />
                            <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Pontuação" FieldName="SOMA_PONTUACAO" VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Apresentação" FieldName="DT_APRESENTACAO"
                                    VisibleIndex="2">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Hora Apresentação" FieldName="HORA_APRESENTACAO"
                                    VisibleIndex="3">
                                    <PropertiesDateEdit DisplayFormatString="HH:mm">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início Contrato" FieldName="DT_INICIO_CONTRATO"
                                    VisibleIndex="4">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Término Contrato" FieldName="DT_FIM_CONTRATO"
                                    VisibleIndex="5">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
