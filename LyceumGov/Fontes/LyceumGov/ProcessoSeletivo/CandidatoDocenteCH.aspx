<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="CandidatoDocenteCH.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoDocenteCH" %>

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



        }

        function msgBox(sMessage) {
            alert(sMessage);
            window.location = 'CandidatoDocenteCH.aspx';
        }

        function Validamsg(sMessage) {
            alert(sMessage);

        }
        function FinalizarCadastro() {


            return false;
        }

        function OnGridExperienciaEndCallback(sender) {



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
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
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
        runat="server" Width="750px">
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
                    <asp:Label ID="lblBuscaCandidato" runat="server" Text="Número de Inscrição:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidatoBusca" runat="server" Caption="" Key="candidato"
                        Argument="nome" SqlSelect="SELECT concurso,DESCRICAOSITUACAO FROM vw_ly_docente_candidato"
                        ArgumentColumns="50" Columns="30" MaxLength="20" SqlWhere=" SITUACAO NOT IN (1,8) and concurso = #tseConcursoBusca# "
                        SqlOrder="nome" GridWidth="800px" OnChanged="tseCandidatoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="concurso" FieldName="concurso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="DESCRICAOSITUACAO" Width="20%" />
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
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
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
    <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.RN.RecursosHumanos.DocenteCandidatoArquivo"
        SelectMethod="ListaDocumento">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="candidato" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsSituacao" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocenteCH"
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
    <asp:ObjectDataSource ID="odsCandidatoTitulacoes" runat="server" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoDocenteCH"
        SelectMethod="ListaCandidatoTitulacoes">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="Concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="Candidato" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsCandidatoExperiencia" runat="server" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoDocenteCH"
        SelectMethod="ListaCandidatoExperiencia">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseConcursoBusca" PropertyName="DBValue" Name="Concurso" />
            <asp:ControlParameter ControlID="tseCandidatoBusca" PropertyName="DBValue" Name="Candidato" />
        </SelectParameters>
    </asp:ObjectDataSource>
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
                                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                            GridWidth="600px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
                                            MaxLength="20">
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
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                            AutoPostBack="true" MaxLength="20" Columns="10" Caption="" Key="id_regional"
                                            SqlSelect="SELECT DISTINCT  RE.ID_REGIONAL,RE.REGIONAL  FROM LY_CONCURSO_DOCENTE CD INNER JOIN LY_CONCURSO_DOC_HABILITACAO CH ON CD.CONCURSO = CH.CONCURSO INNER JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = CH.REGIONALID "
                                            SqlOrder="regional" OnChanged="tseRegional_Changed" DataType="Number">
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
                                        <asp:Label ID="lblNomePai" runat="server" Text="Nome do Pai:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Nome do Pai: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNomePai" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
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
                                        <tweb:TSearchBox runat="server" ID="tseMunicipioProc" SqlSelect="select DISTINCT m.CODIGO ,M.NOME from LY_UNIDADE_ENSINO UE inner join MUNICIPIO M ON UE.MUNICIPIO = M.CODIGO inner join LY_CONCURSO_DOC_HABILITACAO cdh on cdh.MUNICIPIO_PROC=m.CODIGO "
                                            SqlWhere=" SIT_FUNCIONAMENTO='EmAtividade'" MaxLength="20" SqlOrder="NOME" DataType="Varchar"
                                            OnChanged="tseMunicipioProc_Changed" AutoPostBack="true">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Município: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="tseMunicipioProc"
                                            InitialValue="" ValidationGroup="SalvarForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
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
                                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"
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
                                            Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                                            Width="150px" OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true"
                                            Height="20px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Tipo: Preenchimento obrigatório." ID="RequiredFieldValidator6"
                                            runat="server" ControlToValidate="ddlRGTipoPessoa" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                                            Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="15" Width="200px" SkinID="numeroDocumento"
                                            OnKeyPress="return SomenteNumerosLetras(event);" />
                                        <asp:RequiredFieldValidator ErrorMessage="Número: Preenchimento obrigatório." ID="RequiredFieldValidator7"
                                            runat="server" ControlToValidate="txtRGNum" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cmbRGEmissor"
                                            InitialValue="" ErrorMessage="Órgão Emissor: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
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
            <dxtc:TabPage Text="Titulações / Experiências" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccTitulacaoExperiencia" runat="server">
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
                                        EnableCallBacks="true" ClientInstanceName="grdTitulacao" Width="678px" DataSourceID="odsCandidatoTitulacoes"
                                        OnCellEditorInitialize="grdTitulacao_CellEditorInitialize" KeyFieldName="concurso;candidato;TITULACAO"
                                        OnCustomJSProperties="grdTitulacao_CustomJSProperties">
                                        <SettingsEditing Mode="Inline" />
                                        <Settings ShowFooter="true" />
                                        <ClientSideEvents EndCallback="function(s,e){OnGridTitulacaoEndCallback(s);}" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn>
                                                <HeaderCaptionTemplate>
                                                </HeaderCaptionTemplate>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Titulação" FieldName="TITULACAO" VisibleIndex="1"
                                                ReadOnly="True" Width="250px">
                                                <PropertiesComboBox ValueType="System.String" ClientInstanceName="cmbTitulacao" EnableAnimation="False">
                                                    <DropDownButton Visible="False">
                                                    </DropDownButton>
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
                                        EnableCallBacks="true" DataSourceID="odsCandidatoExperiencia" ClientInstanceName="grdExperiencia"
                                        Width="678px" OnCellEditorInitialize="grdExperiencia_CellEditorInitialize" OnCustomJSProperties="grdExperiencia_CustomJSProperties"
                                        KeyFieldName="concurso;candidato;EXPERIENCIA" OnCustomColumnDisplayText="grdExperiencia_CustomColumnDisplayText">
                                        <Settings ShowFooter="true" />
                                        <ClientSideEvents EndCallback="function(s,e){OnGridExperienciaEndCallback(s);}" />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn>
                                                <HeaderCaptionTemplate>
                                                </HeaderCaptionTemplate>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Experiência" FieldName="EXPERIENCIA" VisibleIndex="1"
                                                Width="250px" ReadOnly="True">
                                                <PropertiesComboBox ValueType="System.String" ClientInstanceName="cmbExperiencia"
                                                    EnableAnimation="False">
                                                    <DropDownButton Visible="False">
                                                    </DropDownButton>
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
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Lotação" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccLotacao" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label2" Text="Regional/Sede:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLotacaoRegionalSede" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label5" Text="Municipio:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLotacaoMunicipio" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label6" Text="Disciplina de Ingresso:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLotacaoDisciplinaIngresso" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" colspan="5">
                                    <asp:Label runat="server" ID="Label7" Text="Possui Acumulação de Cargos públicos regularizados no Diário Oficial"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblLotacao" runat="server" RepeatDirection="Horizontal"
                                        DataValueField="PossuiAcumulacao" Width="150px">
                                        <asp:ListItem Text="Sim" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" colspan="5">
                                    <asp:Label runat="server" ID="Label8" Text="Quantidade de anos de atuação no regime de ampliação da jornada de trabalho por meio da Gratificação por Lotação Prioritária(GLP):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtLotacaoGLP" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlFuncDiretor" runat="server">
                            <table>
                                <tr>
                                    <td align="left" colspan="5">
                                        <asp:Label runat="server" ID="Label13" Text="Exerceu a função de Diretor Geral ou de Diretor Adjunto em alguma das unidades escolares que compõem a estrutura da SEEDUC?"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RdlFuncaoDiretor" runat="server" RepeatDirection="Horizontal"
                                            DataValueField="FuncaoDiretor" Width="150px">
                                            <asp:ListItem Text="Sim" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="False"></asp:ListItem>
                                        </asp:RadioButtonList>
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
                                <dxwgv:GridViewDataTextColumn Caption="Baixar" Name="btnBaixar" VisibleIndex="9"
                                    Width="100px">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <DataItemTemplate>
                                        <asp:ImageButton ID="btnBaixar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("DOCENTECANDIDATOARQUIVOID") + "," + Eval("TIPOARQUIVO") %>'
                                            OnCommand="btnBaixar_Click" ImageUrl="~/img/bt_copiar.png" Height="15px" AlternateText="Baixar Documento"
                                            Visible='<%# Eval("DOCENTECANDIDATOARQUIVOID") != DBNull.Value && Eval("DOCENTECANDIDATOARQUIVOID") != DBNull.Value %>'>
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
                                            AlternateText="Visualizar Documento" Visible='<%# Eval("DOCENTECANDIDATOARQUIVOID") != DBNull.Value && Eval("DOCENTECANDIDATOARQUIVOID") != DBNull.Value %>'>
                                        </asp:ImageButton>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
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
                        <asp:ImageButton ID="btnExportarPDF" runat="server" ImageAlign="Right" ToolTip="Export"
                            OnClick="btnExportarPDF_Click" ImageUrl="~/Images/bot_PDF.png" />
                        <emptyimage alternatetext="sem foto" url="~/Images/semfoto.jpg" />
                        </dxe:ASPxBinaryImage>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Processo seletivo" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ccProcessoSeletivo" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label9" Text="Data de Apresentação"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDataApresentacao" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label12" Text="Situação"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="ddlTipo" runat="server" ValueType="System.String" Width="200px">
                                        <Items>
                                            <dxe:ListEditItem Selected="True" Text="Selecione" Value="Selecione" />
                                            <dxe:ListEditItem Text="Em processo" Value="7" />
                                            <dxe:ListEditItem Text="Aprovado" Value="3" />
                                            <dxe:ListEditItem Text="Faltoso" Value="4" />
                                            <dxe:ListEditItem Text="Desistente" Value="5" />
                                            <dxe:ListEditItem Text="Desclassificado " Value="6" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label10" Text="Hora de Apresentação"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtHoraApresentacao" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="Label11" Text="Pontuação:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPontuacao" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnAnalisar" runat="server" align="right" OnClientClick="pucConfirmar.Show(); return false;"
                                        ToolTip="Analisar" SkinID="BcAnalisar" />
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="true"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="true" Width="100%" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirmar">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ModalBackgroundStyle BackColor="White" Opacity="0" />
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Label ID="lblPergunta" runat="server" Text="Tem certeza que deseja executar essa ação?" />
                        <br />
                        <table id="Table1" runat="server">
                            <tr>
                                <td style="text-align: right;" class="style5">
                                    <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click"
                                        OnClientClick="pucConfirmar.Hide(); return true;" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); " />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
