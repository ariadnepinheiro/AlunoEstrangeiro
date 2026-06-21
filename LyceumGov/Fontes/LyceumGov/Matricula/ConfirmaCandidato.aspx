<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConfirmaCandidato.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ConfirmaCandidato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnCurso" runat="server" />
    <asp:HiddenField ID="hdnCurriculo" runat="server" />
    <asp:HiddenField ID="hdnControleVagaId" runat="server" />
    <br />
    <div class="divEditBlock" style="width: 90%;">
        <asp:ImageButton ID="btnVoltar" runat="server" ImageAlign="Right" SkinID="Voltar"
            OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Confirmação de Candidato" SkinID="BcTitulo" />
    </div>
    <div id="divPrincipal" runat="server" visible="false">
        <br />
        <table width="80%">
            <tr>
                <td rowspan="2" width="150px">
                    <dxe:ASPxBinaryImage ID="bimgFotoPessoa" runat="server" AlternateText="sem foto"
                        Width="150px" Height="150px" ClientInstanceName="bimgFotoPessoa" StoreContentBytesInViewState="True">
                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                    </dxe:ASPxBinaryImage>
                </td>
                <td>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Dados do Candidato" Width="100%">
                        <table>
                            <tr>
                                <td width="10%" align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Nome:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Enabled="false" Width="600px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Número Inscrição:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroInscricao" runat="server" ReadOnly="true" Enabled="false" />
                                    <asp:HiddenField ID="hdnInscricao" runat="server" />
                                    <asp:HiddenField ID="hdnFase" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" GroupingText="Dados da Vaga Escolhida" Width="100%">
                        <table>
                            <tr>
                                <td width="10%" align="right">
                                    <asp:Label ID="Label8" runat="server" Text="Unidade:"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtUnidade" runat="server" Enabled="false" ReadOnly="true" Width="600px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label9" runat="server" Text="Ano:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAno" runat="server" Enabled="false" ReadOnly="true" Width="67px" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label10" runat="server" Text="Período:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPeriodo" runat="server" Enabled="false" ReadOnly="true" Width="89px" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label11" runat="server" Text="Modalidade:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtModalidade" runat="server" Enabled="false" ReadOnly="true" Width="277px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label12" runat="server" Text="Segmento:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtSegmento" runat="server" Enabled="false" ReadOnly="true" Width="277px" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label13" runat="server" Text="Curso:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurso" runat="server" Enabled="false" ReadOnly="true" Width="277px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label14" runat="server" Text="Série:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSerie" runat="server" Enabled="false" ReadOnly="true" Width="89px" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label15" runat="server" Text="Turno:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTurno" runat="server" Enabled="false" ReadOnly="true" Width="89px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlFiliacao" runat="server" GroupingText="Filiação" Width="100%">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Nome da Mãe:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                        Enabled="false" Width="450px" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" AutoPostBack="true"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Nome do Pai:"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                        Width="450px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" AutoPostBack="true"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                &nbsp
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
        </table>
        <asp:Panel ID="pnlConfirmação" runat="server" GroupingText="Confirmação" Width="80%">
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                            onchange="Bloqueio()" AutoPostBack="true" Width="201px" OnSelectedIndexChanged="rblConfirmacao_SelectedIndexChanged">
                            <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Panel ID="pnlMotivo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMotivo" runat="server" DataTextField="DESCRICAO" DataValueField="MOTIVOREJEICAOINSCRICAOID"
                                            AppendDataBoundItems="true" Width="201px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="pnlDisciplinaOptativas" runat="server" Visible="false" GroupingText="Disciplinas Optativas">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Enabled="false"
                                            Width="200px" />
                                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                            Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnTransporte" GroupingText="Transporte" runat="server" Visible="false"
            Width="80%">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblGratuidade" runat="server"
                            Text="Utiliza Transporte?*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGratuidade" runat="server" OnSelectedIndexChanged="ddlGratuidade_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="Sim" Value="S"> </asp:ListItem>
                            <asp:ListItem Text="Não" Value="N"> </asp:ListItem>
                            <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblPoderPublTransp" runat="server"
                            Text="Poder público responsável pelo transporte escolar:*" SkinID="lblObrigatorio"></asp:Label><asp:DropDownList
                                ID="ddlPoderPublicoTransp" runat="server">
                                <asp:ListItem Text="Estadual" Value="Estadual"> </asp:ListItem>
                                <asp:ListItem Text="Municipal" Value="Municipal"> </asp:ListItem>
                                <asp:ListItem Text="Nenhum" Value=""> </asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblModais" runat="server"
                            Text="Modal:"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkModais" runat="server" DataTextField="descr" DataValueField="item"
                            Width="300" OnSelectedIndexChanged="chkModais_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRodoviario" runat="server"
                            Text="Rodoviário:" Visible="false"></asp:Label><asp:CheckBoxList ID="chkRodoviario"
                                runat="server" Visible="false" RepeatDirection="Horizontal">
                                <asp:ListItem>Vans/Kombis</asp:ListItem>
                                <asp:ListItem>Microônibus</asp:ListItem>
                                <asp:ListItem>Ônibus</asp:ListItem>
                                <asp:ListItem>Bicicleta</asp:ListItem>
                                <asp:ListItem>Outro tipo de veículo</asp:ListItem>
                            </asp:CheckBoxList>
                        <br />
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAquaviario" runat="server"
                            Text="Aquaviário/embarcação:" Visible="false"></asp:Label><asp:CheckBoxList ID="chkAquaviario"
                                runat="server" Visible="false" AutoPostBack="true" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="chkAquaviario_SelectedIndexChanged">
                                <asp:ListItem>Capacidade de até 5 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade entre 5 e 15 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade entre 15 e 35 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade acima de 35 pessoas</asp:ListItem>
                                <asp:ListItem>Não utiliza transporte Aquaviário</asp:ListItem>
                            </asp:CheckBoxList>
                        <br />
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblOnibus" runat="server"
                            Text="Operadora (selecionar ATÉ DUAS operadoras apenas):" Visible="false"></asp:Label><asp:CheckBoxList
                                ID="chkOnibus" runat="server" Visible="false" RepeatDirection="Horizontal">
                                <asp:ListItem>RioCard</asp:ListItem>
                                <asp:ListItem>Jaé</asp:ListItem>
                                <asp:ListItem>SindPass</asp:ListItem>
                                <asp:ListItem>Outros</asp:ListItem>
                            </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDescFamilia" GroupingText="Comunidade quilombola ou indígena" runat="server" Visible="false" Width="80%">
            <table>
                <tr>
                    <td style="text-align: left" colspan="4">
                        <asp:Label ID="lblDescFamilia" runat="server" Text="O(a) aluno(a) se declara descendente de família integrante de comunidade quilombola, indígena ou caiçara?* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblDescFamilia" runat="server" RepeatDirection="Horizontal"
                            DataValueField="DescFamilia" Width="200px">
                            <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Sim, quilombola." Value="Q"></asp:ListItem>
                            <asp:ListItem Text="Sim, indígena." Value="I"></asp:ListItem>
                            <asp:ListItem Text="Sim, caiçara." Value="C"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar Confirmação"
                        OnClick="btnSalvar_Click" Visible="false" OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
