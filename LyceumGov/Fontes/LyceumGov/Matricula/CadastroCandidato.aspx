<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CadastroCandidato.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.CadastroCandidato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="divPrincipal" runat="server">
        <br />
        <table style="width: 750px">
            <tr>
                <td>
                    <asp:Panel ID="pnDados" runat="server" GroupingText="Dados do Candidato" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Número Inscrição:* "></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNome" runat="server" Text="Nome do Candidato:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblData" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:* "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNumeroInscricao" runat="server" Width="100px" Enabled="false" />
                                    <asp:HiddenField ID="hdnInscricao" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNomeCompl" runat="server" Width="450px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtNascimento" runat="server" Enabled="false" Width="100px" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Sexo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Rede Origem:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtSexo" runat="server" Width="70px" Enabled="false" />
                                    </div>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEstadoCivil" runat="server" Width="120px" Enabled="false" />
                                </td>
                                <td style="width: 50px">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRedeOrigem" runat="server" Width="200px" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="lblObrigatorio" Text="UF de Nascimento:* "></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Município de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtNacionalidade" runat="server" Width="100px" Enabled="false" />
                                    </div>
                                </td>
                                <td style="width: 50px">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUFNascimento" runat="server" Width="110px" Enabled="false" />
                                </td>
                                <td style="width: 50px">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipioNascimento" runat="server" Width="200px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlFiliacao" runat="server" GroupingText="Filiação" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Nome da Mãe:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNomeMae" runat="server" Width="450px" Enabled="false" />
                                </td>
                                <td>
                                    <div>
                                        <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Enabled="false" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Nome do Pai:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNomePai" runat="server" Enabled="false" Width="450px" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp
                                </td>
                            </tr>
                        </table>
            <asp:Panel ID="pnlNaoPossuiIrmao" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkNaoPossuiIrmao" Text="Não possui irmão na rede ou se inscrevendo."
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlIrmaoRede" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkIrmaoRede" Text="Possui irmão menor de 18 anos matriculado na Rede SEEDUC"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label41" runat="server" Text="Número da matricula do irmão:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtMatriculaIrmao" runat="server" Width="450px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label43" runat="server" Text="DADOS DO SEU IRMÃO" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label44" runat="server" Text="Nome:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label45" runat="server" Text="Data Nascimento:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNomeIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataNascIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label46" runat="server" Text="Unidade Escolar:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label47" runat="server" Text="Série:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUEIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerieIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label48" runat="server" Text="Curso:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label49" runat="server" Text="Turno:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCursoIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTurnoIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlIrmaoForaRede" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkIrmaoForaRede" Text="Possui irmão realizando simultaneamente inscrição no Matrícula Fácil"
                                AutoPostBack="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label42" runat="server" Text="Número da inscrição do irmão:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnIdIrmao" runat="server" />
                            <asp:TextBox ID="txtInscricaoIrmao" runat="server" Width="200px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label38" runat="server" Text="DADOS DO SEU IRMÃO" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text="Nome:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label40" runat="server" Text="Data Nascimento:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNomeIrmaoForaRede" runat="server" Width="200px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataNascIrmaoForaRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:ObjectDataSource ID="odsOpcaoIrmaoForaRede" TypeName="Techne.Lyceum.Net.Matricula.ConsultaCandidato"
                                runat="server" SelectMethod="ListarOpcaoIrmaoForaRede">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnIdIrmao" DefaultValue="" Name="inscricaoAlunoId"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdOpcoesIrmaoForaRede" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdOpcoesIrmaoForaRede" DataSourceID="odsOpcaoIrmaoForaRede"
                                KeyFieldName="OPCAOINSCRICAOID">
                                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Opção" FieldName="OPCAO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="ESCOLA">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="DESCRICAOMODALIDADE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel4" runat="server" GroupingText="Endereço" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="CEP:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" SkinID="lblObrigatorio" Text="Logradouro:* "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCEP" Style="width: 110px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLogradouro" runat="server" Width="300px" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="Complemento:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNumero" runat="server" Style="width: 110px;" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComplemento" runat="server" Width="300px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="UF:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" SkinID="lblObrigatorio" Text="Município:*"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" SkinID="lblObrigatorio" Text="Bairro:*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUFEndereco" runat="server" Width="50px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipioEndereco" runat="server" Width="200px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBairro" runat="server" Width="200px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel5" runat="server" GroupingText="Documentos Candidato" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" Text="CPF:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Identidade:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Orgão Emissor:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="UF Emissor:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCPF" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgNumero" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgEmissor" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgUf" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel7" runat="server" GroupingText="Tipo Certidão">
                                        <asp:RadioButtonList ID="rblTipoCertidao" runat="server" RepeatDirection="Horizontal"
                                            Width="200px" Enabled="false">
                                            <asp:ListItem Text="Nascimento" Value="Nascimento"></asp:ListItem>
                                            <asp:ListItem Text="Casamento" Value="Casamento"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </asp:Panel>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Panel ID="Panel6" runat="server" GroupingText="Modelo Certidão">
                                        <asp:RadioButtonList ID="rblModeloCertidao" runat="server" RepeatDirection="Horizontal"
                                            Width="200px" Enabled="false">
                                            <asp:ListItem Text="Modelo Novo" Value="Modelo Novo"></asp:ListItem>
                                            <asp:ListItem Text="Modelo Antigo" Value="Modelo Antigo"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlAntigo" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCertNasc" runat="server" Text="Número do Termo: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDOC_CertNasc_Numero" runat="server" Enabled="false" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCertNascFolha" runat="server" Text="Folha: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDOC_CertNasc_Folha" runat="server" Enabled="false" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNascLivro" runat="server" Text="Livro: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDOC_CertNasc_Livro" runat="server" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlNovo" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNumMatricula" runat="server" Text="Número da matrícula: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNumMatriculaCertidao" Width="150px" runat="server" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel8" runat="server" GroupingText="Deficiência" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDeficiencia" Width="300px" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel9" runat="server" GroupingText="Dados do Responsável" Width="750px">
                        <table width="750px">
                            <tr>
                                <td colspan="4">
                                    <asp:RadioButtonList ID="rblResponsavel" runat="server" RepeatDirection="Horizontal"
                                        Enabled="false">
                                        <asp:ListItem Text="Pai" Value="Pai"></asp:ListItem>
                                        <asp:ListItem Text="Mãe" Value="Mãe"></asp:ListItem>
                                        <asp:ListItem Text="O próprio candidato" Value="proprio"></asp:ListItem>
                                        <asp:ListItem Text="Outros" Value="Outros"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label24" runat="server" Text="Nome do Responsável:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="Celular do Responsável:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="txtResponsavel" runat="server" Width="450px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCelularResponsavel" runat="server" Width="120px" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label21" runat="server" Text="Identidade:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label22" runat="server" Text="Orgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" Text="UF Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCPFResponsavel" Style="width: 120px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgResponsavelNumero" Style="width: 120px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgResponsavelEmissor" Style="width: 120px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgResponsavelUf" Style="width: 120px;" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <br />
</asp:Content>
