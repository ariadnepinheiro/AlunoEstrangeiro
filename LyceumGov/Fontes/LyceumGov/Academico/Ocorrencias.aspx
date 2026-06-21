<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Ocorrencias.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Ocorrencias" %>

<asp:Content ID="conOcorrencias" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function OnValueChanged(s, e) {
            Page_ClientValidate("");
        }
        function validaDataMenorAtual(s, e) {
            var data = e1.GetDate();
            var hoje = new Date();
            if (data > hoje)
                e.IsValid = false;
        }
    </script>

    <div>
        <asp:Label ID="lblTipoOperacao" runat="server" SkinID="lblNomePagina"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    </div>
    <div class="divEditBlock" style="width: 742px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');"  />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" ValidationGroup="SalvarForm" />
        <asp:ImageButton ID="btnConfirmar" runat="server" SkinID="BcConfirmar" OnClick="btnConfirmar_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Ocorrências" SkinID="BcTitulo" />
    </div>

    <div>
        <dxp:ASPxPanel ID="pnOcorrencias" runat="server" Width="742px" OnPreRender="pnOcorrencias_PreRender">
            <PanelCollection>
                <dxp:PanelContent runat="server">
                    <table id="tbDisciplina" runat="server">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblAluno" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAluno" runat="server" CausesValidation="True"></asp:TextBox>
                                <asp:TextBox ID="txtNomeAluno" runat="server" CausesValidation="True" 
                                    Width="537px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblUsuario" runat="server" Text="Usuário:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipo" runat="server" DataTextField="descr" DataValueField="item"
                                    CausesValidation="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblData" runat="server" Text="Data:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxDateEdit ID="tdData" runat="server" ClientInstanceName="e1" MinDate="01/01/1900">
                                    <ClientSideEvents ValueChanged="OnValueChanged"/>
                                    <ValidationSettings CausesValidation="True">
                                    </ValidationSettings>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="rfvData" runat="server" ErrorMessage="Data: Preenchimento obrigatório."
                                    InitialValue="" ValidationGroup="SalvarForm" ControlToValidate="tdData"><img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvData" runat="server" ClientValidationFunction="validaDataMenorAtual"
                                    ErrorMessage="A data não pode ser maior que a data de hoje." ControlToValidate="tdData"
                                    ValidationGroup="SalvarForm">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="lblOrdem" runat="server" Text="Ordem: " Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrdem" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblDescricao" runat="server" Text="Descrição:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescricao" runat="server" Height="298px" TextMode="MultiLine"
                                    Width="667px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Descrição: Preenchimento obrigatório."
                                    InitialValue="" ValidationGroup="SalvarForm" ControlToValidate="txtDescricao"><img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano Letivo: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                    DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblPeriodo" runat="server" Text="Período Letivo: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblDisciplina" runat="server" Text="Disciplina: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDisciplina" runat="server" AutoPostBack="True" DataTextField="nome"
                                    DataValueField="disciplina" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblTurma" runat="server" Text="Turma: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurma" runat="server" DataTextField="turma" DataValueField="turma">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </dxp:PanelContent>
            </PanelCollection>
        </dxp:ASPxPanel>
    </div>
    <div>
    </div>
</asp:Content>
