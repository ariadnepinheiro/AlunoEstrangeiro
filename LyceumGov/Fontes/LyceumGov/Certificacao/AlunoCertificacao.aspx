<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AlunoCertificacao.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.AlunoCertificacao" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botão direito neste campo");
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        
        function onlyNumbers(event) {
                let charCode = event.which || event.keyCode;

                // Permite apenas números de 0 a 9
                if (charCode < 48 || charCode > 57) {
                    event.preventDefault();
                    return false;
                }
                return true;
            }

       function aplicarSomenteNumeros() {
            document.querySelectorAll(".only-numbers").forEach(function (input) {
                
                // Bloqueia teclas não numéricas
                input.addEventListener("keypress", function (event) {
                    let charCode = event.which || event.keyCode;
                    if (charCode < 48 || charCode > 57) {
                        event.preventDefault();
                    }
                });

                // Bloqueia colagem inválida
                input.addEventListener("paste", function (event) {
                    let pasted = (event.clipboardData || window.clipboardData).getData("text");
                    if (!/^\d+$/.test(pasted)) {
                        event.preventDefault();
                    }
                });

                // Limpa caracteres inválidos (failsafe)
                input.addEventListener("input", function () {
                alert('ok');
                    this.value = this.value.replace(/\D/g, "");
                });
            });
            }
            
       aplicarSomenteNumeros();

        function SomenteNumeros(oEvent) {
            var keycode = (oEvent.which) ? oEvent.which : oEvent.keyCode;

            if ((keycode >= 48 && keycode <= 57) || (keycode == 8))
                return (true && (keycode != 46));

            return false;
        }

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }
     

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="Informe o CPF ou o nome do aluno."
        Width="70%">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoCPFTSearch" runat="server" Text="CPF do Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAlunoCPF" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoCertificacaoNaoCadastrado"
                        AutoPostBack="true" OnTextChanged="tseAlunoCPF_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoCPF" runat="server" Text="Para efetuar a busca é necessário ter o CPF do(a) aluno(a)."
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:HiddenField ID="hdnAlunoCertificacaoId" runat="server" />
    <asp:HiddenField ID="hdnAlunoDocumentoId" runat="server" />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstancename="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 70%;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right"
            Visible="false" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Aluno" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Panel ID="pnAbas" runat="server" Width="100%">
        <dxtc:ASPxPageControl ID="pcCertificacao" runat="server" ActiveTabIndex="0" Width="100%"
            OnTabClick="pcCertificacao_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Dados Pessoais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="pnlDados" GroupingText="Dados do Aluno" runat="server" Width="70%"
                                Visible="true">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNomeAluno" runat="server" Text="Nome do Aluno:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNomeAluno" runat="server" MaxLength="100" Width="495px" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblCPF" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCPF" runat="server" Width="170px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNomeMae" runat="server" Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" Width="495px" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Width="140px"
                                                AutoPostBack="true" OnCheckedChanged="chkNaoDeclarMae_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNomePai" runat="server" Text="Nome do Pai:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" Width="495px" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" Width="140px"
                                                AutoPostBack="true" OnCheckedChanged="chkNaoDeclarPai_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMunicipioNascimento" runat="server" Text="Município de Nascimento:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseNaturalidade" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                                Columns="10" ArgumentColumns="30" AutoPostBack="true" Key="codigo" MaxLength="10"
                                                OnChanged="tseNaturalidade_Changed">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                    <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                            <asp:TextBox ID="txtMunicipioNaturalidade" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblDataNascimento" runat="server" Text="Data Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                                                AutoPostBack="true" OnSelectedIndexChanged="cmbNacionalidade_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblNRg" runat="server" Text="Nº do RG:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="txtNRg" runat="server" Width="160px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right" class="style2">
                                            <asp:Label ID="lblRGEmissor" runat="server" Text="Orgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td class="style8">
                                            <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                                                Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right" class="style2">
                                            <asp:Label ID="lblRGUF" runat="server" Text="UF Expedição:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td class="style8">
                                            <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                                                Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSalvarDadosPessoais" Text="Salvar Dados Pessoais" runat="server"
                                                OnClick="btnSalvarDadosPessoais_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Dados Escolares">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <asp:Panel ID="pnlNovoDadosEscolares" runat="server" Width="100%">
                                <asp:Panel ID="pnlEnsino" GroupingText="Unidade de Ensino" runat="server" Width="70%">
                                    <table>
                                        <tr>
                                            <td style="width: auto">
                                                <tweb:TSearchBox ID="tseUnidadeEns" runat="server" Caption="" Key="unidade_ens" MaxLength="20"
                                                    ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,  municipio,id_regional, ua_atual,ua_antiga,regional, v.NOME as nomemunicipio  from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL u inner join MUNICIPIO v on u.MUNICIPIO = v.CODIGO "
                                                    GridWidth="850px" OnLoad="tseUnidadeEns_Load" SqlOrder="nome_comp">
                                                    <GridColumns>
                                                        <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                                        <tweb:TSearchBoxColumn Caption="Município" FieldName="municipio" Width="18%" />
                                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="18%" />
                                                    </GridColumns>
                                                </tweb:TSearchBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlCurso" GroupingText="Informação do Curso" runat="server" Width="70%">
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label runat="server" ID="lblTipoConclusao" SkinID="lblObrigatorio" Text="Tipo de Conclusão*:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoConclusao" runat="server" DataTextField="DESCRICAO"
                                                    DataValueField="TIPOCONCLUSAOID">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label runat="server" ID="Label1" Text="Modalidade*:" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlNivelModalidade" runat="server">
                                                    <asp:ListItem Text="Ensino Fundamental Regular" Value="EFR"> </asp:ListItem>
                                                    <asp:ListItem Text="Ensino Médio Regular" Value="EMR"> </asp:ListItem>
                                                    <asp:ListItem Text="Ensino Fundamental EJA" Value="EFE"> </asp:ListItem>
                                                    <asp:ListItem Text="Ensino Médio EJA" Value="EME"> </asp:ListItem>
                                                    <asp:ListItem Text="Educação Profissional Integrada" Value="EPI"> </asp:ListItem>
                                                    <asp:ListItem Text="Educação Profissional Concomitante" Value="EPC"> </asp:ListItem>
                                                    <asp:ListItem Text="Educação Profissional Subsequente" Value="EPS"> </asp:ListItem>
                                                    <asp:ListItem Text="Curso Normal" Value="CN"> </asp:ListItem>
                                                    <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTipo" runat="server" SkinID="lblObrigatorio" Text="Tipo Documento"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoDocumento" runat="server">
                                                    <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                                                    <asp:ListItem Text="Certificado" Value="3"> </asp:ListItem>
                                                    <asp:ListItem Text="Diploma" Value="4"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblCurso" runat="server" Text="Nome do Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtCurso" runat="server" MaxLength="100" Width="300px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblAtoAutoriza" runat="server" Text="Ato Autorizativo:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAtoAutoriza" runat="server" MaxLength="100" Width="100px" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDtAto" runat="server" Text="Data do Ato:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDataAto" runat="server" ClientInstanceName="dtAto" CalendarProperties-ClearButtonText="Limpar"
                                                    CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTotalHAula" runat="server" Text="Total Hora-Aula:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalHAula" runat="server" MaxLength="50" Width="60px" class="only-numbers" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTotalHRelogio" runat="server" Text="Total Hora-Relógio:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalHRelogio" runat="server" MaxLength="50" Width="60px" class="only-numbers" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDtConclusao" runat="server" Text="Data de Conclusão:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDataConclusao" runat="server" ClientInstanceName="dtConclusao"
                                                    CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlObservacao" GroupingText="Observação" runat="server" Width="70%">
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblNumeroLivro" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumLivro" runat="server" MaxLength="10" Width="50px" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblFolhaLivro" runat="server" Text="Folha:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFolhaLivro" runat="server" MaxLength="10" Width="50px" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblLivro" runat="server" Text="Livro:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLivro" runat="server" MaxLength="10" Width="50px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblObservacao" runat="server" Text="Observação:"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtObservacao" runat="server" MaxLength="100" Width="600px" TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSalvar" Text="Salvar e Gerar Documento" runat="server" OnClick="btnSalvar_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                            <br />
                            <asp:ObjectDataSource ID="odsDadosEscolares" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.AlunoCertificacao"
                                SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnAlunoCertificacaoId" Name="alunoCertificacaoId"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdDadosEscolares" runat="server" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="grdDadosEscolares" DataSourceID="odsDadosEscolares"
                                OnAfterPerformCallback="grdDadosEscolares_AfterPerformCallback" KeyFieldName="ALUNODOCUMENTOID"
                                EnableCallBacks="false" OnCustomButtonCallback="grdDadosEscolares_CustomButtonCallback">
                                <SettingsEditing Mode="Inline" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="30px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center" id="dvteste">
                                                <input type="image" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                    title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                                            </div>
                                        </HeaderCaptionTemplate>
                                        <CustomButtons>
                                            <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                                Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                            </dxwgv:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ALUNODOCUMENTOID" ReadOnly="true"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="UNIDADEENSINO" VisibleIndex="3" Caption="NUM_FUNC"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="TIPOCONCLUSAOID" VisibleIndex="4" Caption="TIPOCONCLUSAOID"
                                        CellStyle-HorizontalAlign="Center" Width="40px" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataColumn Caption="Tipo de Conclusão" FieldName="DESCRICAOTIPOCONCLUSAO"
                                        VisibleIndex="5" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center"
                                        Visible="true">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="MODALIDADE" VisibleIndex="6" Caption="MODALIDADE"
                                        Visible="false" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="DESCRICAOMODALIDADE" VisibleIndex="7" Caption="Modalidade"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="DOCUMENTOID" VisibleIndex="8" Caption="DOCUMENTOID"
                                        Visible="false" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="DESCRICAODOCUMENTO" VisibleIndex="9" Caption="Tipo de Documento"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="ESCOLA" VisibleIndex="10" Caption="Escola"
                                        CellStyle-HorizontalAlign="Justify">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="NOMECURSO" VisibleIndex="11" Caption="Curso"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="ATOAUTORIZA" VisibleIndex="12" Caption="Ato Autorizativo"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data do Ato" FieldName="DATAAUTORIZA" VisibleIndex="13"
                                        Width="110px" CellStyle-HorizontalAlign="Center">
                                        <PropertiesDateEdit Width="110px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="TOTALHORASRELOGIO" VisibleIndex="14" Caption="Total Horas Relógio"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="TOTALHORASAULA" VisibleIndex="15" Caption="Total Horas Aula"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data de Conclusão" FieldName="DATACONCLUSAO"
                                        VisibleIndex="16" Width="110px" CellStyle-HorizontalAlign="Center">
                                        <PropertiesDateEdit Width="110px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="NUMEROLIVRO" VisibleIndex="17" Caption="Número"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="LIVRO" VisibleIndex="18" Caption="Livro"
                                        Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Folha" FieldName="FOLHALIVRO" Name="FOLHALIVRO"
                                        VisibleIndex="19" CellStyle-HorizontalAlign="Center">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" Name="OBSERVACAO"
                                        VisibleIndex="20">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <table>
        <tr>
            <td>
                <asp:Button ID="btnImprimirCert" Text="Certificado em Word" runat="server" OnClick="btnImprimirCert_Click" />
                <asp:Button ID="btnImprimirDipl" Text="Diploma em Word" runat="server" OnClick="btnImprimirDipl_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
