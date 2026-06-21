<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoTransferencia.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AcompanhamentoTransferencia" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
       
        function controlarObservacao(comboBox, limparTexto) {
            var txtJustificativa = $("#" + $(comboBox).attr("txtJustificativa"));
            var observacao = $(comboBox).val();

            if (observacao == "Outros") {
                $(txtJustificativa).removeAttr("readonly");
                $(txtJustificativa).removeAttr("disabled");
                $(txtJustificativa).css("background-color", "");

                if (limparTexto) {
                    $(txtJustificativa).val("");
                }
            }
            else {
                $(txtJustificativa).attr("readonly", "readonly");
                $(txtJustificativa).attr("disabled", true);
                $(txtJustificativa).css("background-color", "Gainsboro");

                if (limparTexto) {
                    $(txtJustificativa).val("");
                }
            }
        }

        function controlarRecusa(radioButton, limparTexto) {
            var cmbObservacao = $("#" + $(radioButton).attr("cmbObservacao"));
            var txtJustificativa = $("#" + $(radioButton).attr("txtJustificativa"));

            $(txtJustificativa).attr("readonly", "readonly");
            $(txtJustificativa).attr("disabled", true);
            $(txtJustificativa).css("background-color", "Gainsboro");

            if (limparTexto) {
                $(txtJustificativa).val("");

                $(cmbObservacao).val("Selecione");
            }

            $(cmbObservacao).removeAttr("disabled");
        }

        function controlarAceite(radioButton) {
            var cmbObservacao = $("#" + $(radioButton).attr("cmbObservacao"));
            var txtJustificativa = $("#" + $(radioButton).attr("txtJustificativa"));

            $(txtJustificativa).attr("readonly", "readonly");
            $(txtJustificativa).attr("disabled", true);
            $(txtJustificativa).css("background-color", "Gainsboro");
            $(txtJustificativa).val("");

            $(cmbObservacao).attr("disabled", true);
            $(cmbObservacao).val("Selecione");
        }

        function abrirPopup() {
           
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function atualizarGrid() {
            $("input[id*='rbAceitar']:checked").each(function() {
                controlarAceite(this, true, false);
            });

            $("input[id*='rbRecusar']:checked").each(function() {
                controlarRecusa(this, false);
            });

            $("select[id*='cmbObservacao']").each(function() {
                controlarObservacao(this, false);
            });
        }

        $(document).ready(function() {
            $("input[id*='rbAceitar']").click(function() {
                controlarAceite(this);
            });

            $("input[id*='rbRecusar']").click(function() {
                controlarRecusa(this, true);
            });

            $("select[id*='cmbObservacao']").change(function() {
                controlarObservacao(this, true);
            });
        });
    </script>

    <dxtc:ASPxPageControl ID="pcTransferencia" runat="server" ActiveTabIndex="1" OnTabClick="pcTransferencia_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Solicitação de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Acompanhamento de Solicitações">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:">
                            <table>
                                <tr>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                            GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                                            MaxLength="10">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 20%">
                                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                            SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px"
                                            OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_antiga" Width="30%" />
											    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_atual" Width="30%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                                            ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnSolicitacao" runat="server" GroupingText="Solicitações realizadas pela Unidade">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStatusSolic" runat="server" Text="Exibir Solicitações: " SkinID="lblObrigatorio"></asp:Label>
                                                    <asp:DropDownList ID="cmbStatusSolic" runat="server" AppendDataBoundItems="True"
                                                        AutoPostBack="True" Height="16px">
                                                        <asp:ListItem Selected="True" Text="Pendentes" Value="Pendente"></asp:ListItem>
                                                        <asp:ListItem Text="Aceitas" Value="Aceita"></asp:ListItem>
                                                        <asp:ListItem Text="Recusadas" Value="Recusada"></asp:ListItem>
                                                        <asp:ListItem Text="Todas"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxwgv:ASPxGridView ID="grdSolicitacao" runat="server" AutoGenerateColumns="False" 
                                                        ClientInstanceName="grdSolicitacao" DataSourceID="odsSolicitacao" KeyFieldName="ID_TRANSFERENCIA"
                                                        OnCustomUnboundColumnData="grdSolicitacao_CustomUnboundColumnData" OnCommandButtonInitialize="grdSolicitacao_CommandButtonInitialize">
                                                        <SettingsBehavior ConfirmDelete="True" />
                                                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                        <Columns>
                                                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                                <DeleteButton Text="Remover" Visible="True">
                                                                    <Image Url="~/img/bt_exclui2.png" />
                                                                </DeleteButton>
                                                            </dxwgv:GridViewCommandColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_TRANSFERENCIA" ReadOnly="True"
                                                                VisibleIndex="1">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Aluno" UnboundType="String" FieldName="MATRICULA_ALUNO"
                                                                ReadOnly="true" VisibleIndex="2">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="ALUNO" ReadOnly="true" VisibleIndex="3"
                                                                Visible="false">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="NOME_ALUNO" ReadOnly="true" VisibleIndex="3"
                                                                Visible="false">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Origem" UnboundType="String" FieldName="CENSO_ESCOLA"
                                                                ReadOnly="true" VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="CENSO" ReadOnly="true" Visible="false" VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="NOME_ESCOLA" ReadOnly="true" Visible="false"
                                                                VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Data Solicitação" FieldName="DT_CADASTRO"
                                                                ReadOnly="True" VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Solicitante" FieldName="SOLICITANTE" ReadOnly="True"
                                                                VisibleIndex="5">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" ReadOnly="True"
                                                                VisibleIndex="6">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAOJUST" ReadOnly="True"
                                                                VisibleIndex="7">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Data Andamento" FieldName="DT_ALTERACAO" ReadOnly="True"
                                                                VisibleIndex="8">
                                                            </dxwgv:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />
                                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                    </dxwgv:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnAcompanhamento" runat="server" GroupingText="Solicitações feitas para a Unidade">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStatusAcomp" runat="server" Text="Exibir Solicitações: " SkinID="lblObrigatorio"></asp:Label>
                                                    <asp:DropDownList ID="cmbStatusAcomp" runat="server" AppendDataBoundItems="true"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxwgv:ASPxGridView ID="grdAcompanhamento" runat="server" AutoGenerateColumns="False"
                                                        ClientInstanceName="grdAcompanhamento" DataSourceID="odsAcompanhamento" KeyFieldName="ID_TRANSFERENCIA"
                                                        OnCustomUnboundColumnData="grdAcompanhamento_CustomUnboundColumnData" OnHtmlRowCreated="grdAcompanhamento_HtmlRowCreated">
                                                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                                        <SettingsPager Mode="ShowAllRecords" />                                                    
                                                        <Columns>
                                                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_TRANSFERENCIA" ReadOnly="true"
                                                                VisibleIndex="1">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Aluno" UnboundType="String" FieldName="MATRICULA_ALUNO"
                                                                ReadOnly="true" VisibleIndex="2">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="ALUNO" ReadOnly="true" VisibleIndex="3"
                                                                Visible="false">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="NOME_ALUNO" ReadOnly="true" VisibleIndex="3"
                                                                Visible="false">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Destino" UnboundType="String" FieldName="CENSO_ESCOLA"
                                                                ReadOnly="true" VisibleIndex="4">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="CENSO" ReadOnly="true" Visible="false" VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn FieldName="NOME_ESCOLA" ReadOnly="true" Visible="false"
                                                                VisibleIndex="4">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Data Solicitação" FieldName="DT_CADASTRO"
                                                                ReadOnly="true" Visible="true" VisibleIndex="5">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" ReadOnly="true"
                                                                VisibleIndex="7">
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Andamento *" Name="ANDAMENTO" VisibleIndex="8">
                                                                <DataItemTemplate>
                                                                    <asp:RadioButton ID="rbAceitar" runat="server" GroupName='<%# Bind("ID_TRANSFERENCIA") %>'
                                                                        Text="Aceitar" />
                                                                    <asp:RadioButton ID="rbRecusar" runat="server" GroupName='<%# Bind("ID_TRANSFERENCIA") %>'
                                                                        Text="Recusar" />
                                                                </DataItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Observação" FieldName="OBSERVACAO" Name="OBSERVACAO"
                                                                UnboundType="String">
                                                                <DataItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hfObservacao" Value='<%# Bind("OBSERVACAO") %>' />
                                                                    <asp:DropDownList ID="cmbObservacao" runat="server" DataSourceID="odsTabelaGeral"
                                                                        DataTextField="DESCR" DataValueField="ITEM" Width="200px">
                                                                    </asp:DropDownList>
                                                                </DataItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Justificativa" Name="JUSTIFICATIVA" FieldName="JUSTIFICATIVA">
                                                                <DataItemTemplate>
                                                                    <asp:TextBox ID="txtJustificativa" runat="server" Text='<%# Bind("JUSTIFICATIVA") %>' />
                                                                </DataItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Data Andamento" FieldName="DT_ALTERACAO" ReadOnly="true"
                                                                VisibleIndex="11">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Solicitante" FieldName="SOLICITANTE" ReadOnly="true"
                                                                VisibleIndex="12">
                                                                <PropertiesTextEdit>
                                                                    <ReadOnlyStyle>
                                                                        <Border BorderStyle="None"></Border>
                                                                    </ReadOnlyStyle>
                                                                </PropertiesTextEdit>
                                                            </dxwgv:GridViewDataTextColumn>
                                                            <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="MOTIVO" ReadOnly="True"
                                                                VisibleIndex="13">
                                                            </dxwgv:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />                                                      
                                                    </dxwgv:ASPxGridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar" 
                                                        OnClick="btnSalvar_Click" OnClientClick="this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:ObjectDataSource ID="odsTabelaGeral" runat="server" TypeName="Techne.Lyceum.RN.Cache.TabelaGeral"
                                        SelectMethod="SelecionarItens">
                                        <SelectParameters>
                                            <asp:Parameter Name="tabela" DefaultValue="ObservacaoTransf"></asp:Parameter>
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsAcompanhamento" TypeName="Techne.Lyceum.Net.Academico.AcompanhamentoTransferencia"
                            runat="server" SelectMethod="ListarTransferenciasDeOrigem">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                                    PropertyName="DBValue" />
                                <asp:ControlParameter ControlID="cmbStatusAcomp" PropertyName="SelectedValue" Name="status" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsSolicitacao" TypeName="Techne.Lyceum.Net.Academico.AcompanhamentoTransferencia"
                            runat="server" SelectMethod="ListarTransferenciasDeDestino" DeleteMethod="Delete"
                            OnDeleting="odsSolicitacao_Deleting">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                                    PropertyName="DBValue" />
                                <asp:ControlParameter ControlID="cmbStatusSolic" PropertyName="SelectedValue" Name="status" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false" EnableViewState="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" EnableAnimation="false" ShowPageScrollbarWhenModal="false"
        Width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" >
        <ClientSideEvents Init="OnInitASPxPopupControl" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMensagemPopup" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="lblMensagemPopupEletiva" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="trAceitos">
                        <td>
                            <asp:Label ID="lblAceitos" runat="server" Text="Aceitos:"></asp:Label>
                            <br />
                            <asp:BulletedList ID="blAceitos" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr runat="server" id="trRecusados">
                        <td>
                            <asp:Label ID="lblRecusados" runat="server" Text="Recusados:"></asp:Label>
                            <br />
                            <asp:BulletedList ID="blRecusados" runat="server">
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
                            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click" OnClientClick="this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
