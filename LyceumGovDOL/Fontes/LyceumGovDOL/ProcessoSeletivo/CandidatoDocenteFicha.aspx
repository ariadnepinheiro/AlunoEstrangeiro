<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/ProcessoSeletivoMaster.Master"
    AutoEventWireup="true" CodeBehind="CandidatoDocenteFicha.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoDocenteFicha" %>

<%@ MasterType VirtualPath="~/Modulos/ProcessoSeletivoMaster.Master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="cCandidatoDocente" ContentPlaceHolderID="cphFormulario" runat="server">
 
    <script type="text/javascript">

        $(document).ready(function() {

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

    <asp:HiddenField ID="hdnTitulacao" runat="server" />
    <asp:HiddenField ID="hdnExperiencia" runat="server" />
    <br />
    <br />
    <div>
       <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    </div>
    <br />
    <br />
    <div style="width: 850px;">
        <asp:ImageButton ID="btnImprimir" OnClick="Imprimir_Click" runat="server" SkinID="Imprimir"
            ImageAlign="Right" Visible="true" />
    </div>
    <br />
    <asp:ObjectDataSource ID="odsSituacao" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
        SelectMethod="ConsultarSituacaoAvaliacao"></asp:ObjectDataSource>
    <dxtc:ASPxPageControl ID="pcCandidatoDocente" runat="server" ClientInstanceName="pcCandidatoDocente"
        ActiveTabIndex="1" Width="980px" style="margin-bottom:50px; margin-top:-40px">
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
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseConcurso" runat="server" SqlSelect="SELECT distinct CD.concurso, CD.descricao,CD.indigena FROM lY_concurso_docente CD INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD ON CD.CONCURSO = CDCD.CONCURSOID INNER JOIN LY_CONCURSO_DOC_TITULACOES CDT ON CD.CONCURSO = CDT.CONCURSO "
                                        SqlWhere="tipo = 'Contrato' and CONVERT(DATE,GETDATE()) BETWEEN CONVERT(DATE,CD.DT_INI_INSCR ) AND CONVERT(DATE,CD.DT_FIM_INSCR)"
                                        MaxLength="20" SqlOrder="descricao" GridWidth="800px" OnChanged="tseConcurso_Changed">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                                            <tweb:TSearchBoxColumn Caption="Indigena" FieldName="indigena" Visible="false" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                    <asp:RequiredFieldValidator ErrorMessage="Processo Seletivo: Preenchimento obrigatório."
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
                                    <asp:Label ID="lblCoordMunic" runat="server" Text="Regional:*" SkinID="lblObrigatorio"
                                        Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" Caption=""
                                        AutoPostBack="true" Key="id_regional" SqlSelect="SELECT distinct regionalid FROM LY_CONCURSO_DOCENTE CD INNER JOIN LY_CONCURSO_DOC_HABILITACAO CH ON CD.CONCURSO = CH.CONCURSO INNER JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = CH.REGIONALID "
                                        SqlWhere="ch.Concurso = #tseConcurso# and ch.MUNICIPIO_PROC = #tseMunicipioProc#"
                                        OnChanged="tseRegional_Changed" Visible="true" DataType="Number" >
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="regional" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                    <asp:HiddenField runat="server" ID="hdnNucleo" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblHabilitacao" runat="server" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbCargo" runat="server" AutoPostBack="true" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO" OnSelectedIndexChanged="cmbCargo_Changed" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Panel ID="PnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
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
                                                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="200px"
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
                                                        Width="200px">
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
                                            AutoPostBack="true" DataTextField="DESCRICAO" Width="200px" OnSelectedIndexChanged="cmbNecessidadeEspecial_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvNecEspecial" runat="server" ControlToValidate="cmbNecessidadeEspecial"
                                            InitialValue="" ErrorMessage="Necessidade Especial: Preenchimento obrigatório."
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblEtnia" Text="Etnia:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEtnia" runat="server" Width="200px" DataTextField="NOME"
                                            DataValueField="ETNIAID" AutoPostBack="True" OnSelectedIndexChanged="ddlEtnia_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEtnia" runat="server" ControlToValidate="ddlEtnia"
                                            InitialValue="" ErrorMessage="Etnia: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNomeMae" runat="server" Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="193px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Nome da Mãe: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNomeMae" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNomePai" runat="server" Text="Nome do Pai:" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                            Width="193px"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ErrorMessage="Nome do Pai: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNomePai" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator> --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="200px">
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
                                            AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlPaisNasc_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="País de Nascimento: Preenchimento obrigatório."
                                            ID="rfvPaisNasc" runat="server" ControlToValidate="ddlPaisNasc" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" Width="200px"
                                            DataValueField="nacionalidade" AutoPostBack="false">
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
                                        <asp:RequiredFieldValidator ErrorMessage="Naturalidade: Preenchimento obrigatório."
                                            ID="rfvNaturalidade" runat="server" ControlToValidate="tseNaturalidade" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNaturalidadeUF" Visible="true" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNaturalidadeUF" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>
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
                                            runat="server" ControlToValidate="ddlCotas" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlEndereco" runat="server" GroupingText="Endereço">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCep" Width="200px" runat="server" SkinID="numerico" MaxLength="8"
                                            AutoPostBack="false" />
                                        <tweb:TSearch ID="tsCEP" runat="server" Width="200px" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                            Modal="true" SkinID="CEP">
                                            <GridColumns>
                                                <tweb:TSearchColumn FieldName="cep" Caption="CEP" Width="15%"></tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="nomeLogradouro" Caption="Logradouro" Width="30%">
                                                </tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="nomeMunicipio" Caption="Munic&#237;pio" Width="30%">
                                                </tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="nomeBairro" Caption="Bairro" Width="15%"></tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="uf" Caption="Estado" Width="10%"></tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="codigoLogradouro" Caption="codigoLogradouro" Visible="False"
                                                    Width="0%"></tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="codigoMunicipio" Caption="codigoMunicipio" Visible="False"
                                                    Width="0%"></tweb:TSearchColumn>
                                                <tweb:TSearchColumn FieldName="codigoBairro" Caption="codigoBairro" Visible="False"
                                                    Width="0%"></tweb:TSearchColumn>
                                            </GridColumns>
                                            <GridFilterParameters>
                                                <tweb:TSearchParameter Caption="Munic&#237;pio" MaxLength="100" ParameterName="municipio"
                                                    ShowInFilterPanel="True"></tweb:TSearchParameter>
                                                <tweb:TSearchParameter Caption="Logradouro" MaxLength="20" ParameterName="logradouro"
                                                    ShowInFilterPanel="True"></tweb:TSearchParameter>
                                                <tweb:TSearchParameter Caption="CEP" MaxLength="20" ParameterName="cep" ShowInFilterPanel="True">
                                                </tweb:TSearchParameter>
                                            </GridFilterParameters>
                                            <Messages KeyNotFound="CEP inv&#225;lida" QueryFailure="Ocorreu uma falha durante a busca dos registros. Execute a busca novamente."
                                                TooManyRows="Foram encontrados mais de {0} registros. Mostrando os {0} primeiros.">
                                            </Messages>
                                        </tweb:TSearch>
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
                                            GridWidth="500px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
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
                                            readonly="readonly" /><asp:RequiredFieldValidator ErrorMessage="Estado: Preenchimento obrigatório."
                                                ID="rfvEstador" runat="server" Enabled="false" ControlToValidate="txtEstado"
                                                InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
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
                                                        Width="300px"></asp:TextBox>
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
                                            Width="300px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlContatos" runat="server" GroupingText="Contatos">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td style="width: 415px">
                                        <asp:TextBox ID="txtFone" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                            Width="200px" />
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Telefone: Preenchimento obrigatório." ID="rfvFone"
                                            runat="server" ControlToValidate="txtFone" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right">
                                        <asp:Label Font-Bold="true" Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular"
                                            runat="server" Text="Celular:*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" runat="server" Width="200px" MaxLength="14" name="txtCelular" onkeyup="formataCelularDDD(this,event)"  />
                                        <asp:RequiredFieldValidator ErrorMessage="Celular: Preenchimento obrigatório." ID="rfvCelular"
                                            runat="server" ControlToValidate="txtCelular" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Bold="true" Font-Size="Smaller" ID="lblEmail"
                                            runat="server" Text="E-mail:*"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="300px" MaxLength="100" />
                                        <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><img src="../Images/AlertaMens.gif" alt="E-mail inválido" /></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ErrorMessage="E-mail: Preenchimento obrigatório." ID="rfvEmail"
                                            runat="server" ControlToValidate="txtEmail" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlDocumentos" runat="server" GroupingText="Documentos">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Tipo" runat="server"
                                            Text="Tipo:" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                                            Width="200px" OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Tipo: Preenchimento obrigatório." ID="RequiredFieldValidator6"
                                            runat="server" ControlToValidate="ddlRGTipoPessoa" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                                            Text="Número:" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="15" Width="200px" OnKeyPress="return SomenteNumerosLetras(event);" />
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Número: Preenchimento obrigatório." ID="RequiredFieldValidator7"
                                            runat="server" ControlToValidate="txtRGNum" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator> --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
                                            Text="Estado:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                                            Width="200px">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Estado: Preenchimento obrigatório." ID="rfvEstadoRG"
                                            Enabled="false" runat="server" ControlToValidate="cmbRGUF" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator> --%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
                                            Text="Órgão Emissor:" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                                            Width="200px">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cmbRGEmissor"
                                            InitialValue="" ErrorMessage="Órgão Emissor: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator> --%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
                                            Text="Data de Expedição:" ></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataExped" runat="server" Width="150px" MinDate="1901-01-01"
                                            CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="Data de Expedição: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtDataExped" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif"  /></asp:RequiredFieldValidator> --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCPF" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="14"
                                            Width="200px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCPF"
                                            InitialValue="" ErrorMessage="CPF: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblPisPasep" runat="server" Text="PIS/PASEP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPisPasep" runat="server" MaxLength="11" Width="200px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnSalvarCandidato" runat="server" Text="Avançar" OnClick="btnSalvarCandidato_Click"
                                            ValidationGroup="SalvarForm" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Disciplinas de Habilitação" ToolTip="Disciplinas a serem habilitadas">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDisciplinas" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlResultadoDisciplinas" CssClass="Panel">
                                        <asp:ObjectDataSource ID="odsDisciplina" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                                            SelectMethod="obtemHabilitacaoProcessoPor">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                                                <asp:ControlParameter ControlID="tseMunicipioProc" PropertyName="DBValue" Name="municipio" />
                                                <asp:ControlParameter ControlID="tseRegional" PropertyName="DBValue" Name="regional" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <dxwgv:ASPxGridView ID="grdDisciplina" KeyFieldName="agrupamento" runat="server"
                                            AutoGenerateColumns="False" DataSourceID="odsDisciplina">
                                            <SettingsEditing Mode="Inline" />
                                            <Columns>
                                                <dxwgv:GridViewDataTextColumn Caption="" HeaderStyle-Font-Bold="true" FieldName="agrupamento"
                                                    VisibleIndex="0" Width="380px" Visible="false">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" HeaderStyle-Font-Bold="true" FieldName="descricao"
                                                    VisibleIndex="1" Width="380px">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="2">
                                                </dxwgv:GridViewCommandColumn>
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button runat="server" ID="btnAvancarDisciplina" Text="Avançar" OnClick="btnAvancarDisciplina_Click" />
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Titulações / Experiências" ToolTip="Seção liberada após a inclusão do formulário">
                <ContentCollection>
                    <dxw:ContentControl ID="ccTitulacaoExperiencia" runat="server">
                        <asp:ObjectDataSource ID="odsTitulacao" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                            SelectMethod="ConsultarTitulacao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsExperiencia" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                            SelectMethod="ConsultarExperienciaSeeduc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsExperienciaFora" runat="server" TypeName="Techne.Lyceum.RN.CandidatoDocente"
                            SelectMethod="ConsultarExperienciaFora">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseConcurso" PropertyName="DBValue" Name="concurso" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblObservacaoHabilitacao" ForeColor="Red" runat="server" Text="É obrigatório ter a titulação 'Licenciatura Plena na disciplina pretendida para a contratação temporária' como Habilitação para lecionar nos anos finais do Ensino Fundamental,Ensino Médio e Educação Profissional de Nível Médio para poder participar do processo seletivo de Professor Docente I."></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdTitulacao" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="true" ClientInstanceName="grdTitulacao" Width="678px" DataSourceID="odsTitulacao"
                                        OnCellEditorInitialize="grdTitulacao_CellEditorInitialize" KeyFieldName="titulacao"
                                        OnRowInserting="grdTitulacao_RowInserting">
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Settings ShowFooter="true" />
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="" HeaderStyle-Font-Bold="true" FieldName="titulacao"
                                                VisibleIndex="0" Width="380px" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Titulação" HeaderStyle-Font-Bold="true" FieldName="descricao"
                                                VisibleIndex="1" Width="380px" Visible="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Pontuação" HeaderStyle-Font-Bold="true" FieldName="pontuacao"
                                                VisibleIndex="2" Width="200px">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewCommandColumn Caption="" Name="titulacaoSelecionada" ShowSelectCheckbox="true"
                                                VisibleIndex="3">
                                            </dxwgv:GridViewCommandColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdExperiencia" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="true" DataSourceID="odsExperiencia" ClientInstanceName="grdExperiencia"
                                        Width="678px" OnCellEditorInitialize="grdExperiencia_CellEditorInitialize" OnRowInserting="grdExperiencia_RowInserting"
                                        KeyFieldName="experiencia">
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Settings ShowFooter="true" />
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="" HeaderStyle-Font-Bold="true" FieldName="experiencia"
                                                VisibleIndex="0" Width="380px" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Experiência" HeaderStyle-Font-Bold="true"
                                                FieldName="descricao" VisibleIndex="1" Width="380px" Visible="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Pontuação" HeaderStyle-Font-Bold="true" FieldName="pontuacao"
                                                VisibleIndex="2" Width="200px">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewCommandColumn Caption="" Name="experienciaSelecionada" ShowSelectCheckbox="true"
                                                VisibleIndex="3">
                                            </dxwgv:GridViewCommandColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <dxwgv:ASPxGridView ID="grdExperienciaFora" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="true" DataSourceID="odsExperienciaFora" ClientInstanceName="grdExperienciaFora"
                                        Width="678px" OnCellEditorInitialize="grdExperiencia_CellEditorInitialize" OnRowInserting="grdExperiencia_RowInserting"
                                        KeyFieldName="experiencia">
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Settings ShowFooter="true" />
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="" HeaderStyle-Font-Bold="true" FieldName="experiencia"
                                                VisibleIndex="0" Width="380px" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Experiência" HeaderStyle-Font-Bold="true"
                                                FieldName="descricao" VisibleIndex="1" Width="380px" Visible="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Pontuação" HeaderStyle-Font-Bold="true" FieldName="pontuacao"
                                                VisibleIndex="2" Width="200px">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewCommandColumn Caption="" Name="experienciaSelecionada" ShowSelectCheckbox="true"
                                                VisibleIndex="3">
                                            </dxwgv:GridViewCommandColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAviso" runat="server" Font-Bold="True" Text="Observei com atenção todos os dados preenchidos e afirmo que as 
									informações estão corretas.Estou ciente que ao clicar em Finalizar o cadastro será 
									gravado em definitivo, não sendo mais possível alterá-lo.">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <table align="right">
                                        <tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Image ID="imgChave" runat="server" ImageUrl="~/Seguranca/GeraChaveSeguranca.aspx" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnAtualizaImagemCaptcha" Text="Trocar Imagem" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblChave" runat="server" SkinID="lblObrigatorio" Text="Digite o código da imagem acima:"></asp:Label>
                                        <asp:TextBox ID="txtChave" Visible="true" runat="server" SkinID="numerico" MaxLength="6"
                                            Width="50"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <br />
                                        <asp:Label ID="lblMensagem2" runat="server" SkinID="lblMensagem"></asp:Label>
                                        <br />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnFinalizar" runat="server" Text="Salvar Inscrição" OnClientClick="return FinalizarCadastro();"
                                            OnClick="btnFinalizar_Click" ValidationGroup="SalvarForm" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
