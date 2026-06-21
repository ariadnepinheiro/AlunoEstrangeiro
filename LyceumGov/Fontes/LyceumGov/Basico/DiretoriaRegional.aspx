<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DiretoriaRegional.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.DiretoriaRegional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 15%;
        }
        .style2
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function onlyNumbers() {
            if (event.keyCode < 48
                 || event.keyCode > 57) {
                event.keyCode = 0;
            }
        }

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }

        $(document).ready(function() {
            $("#<%= this.txtEnd_Num.ClientID %>").numeric();
            $("#<%= this.txtEndereco.ClientID %>").attr("readonly", "readonly");
            $("#<%= this.txtMunicipio.ClientID %>").attr("readonly", "readonly");
            $('#<%=txtEndereco.ClientID %>').bind('keyup', function() {
                blocTexto(this, 100);
            });

            $('#<%=txtNome.ClientID %>').bind('keyup', function() {
                blocTexto(this, 50);
            });


        });

    </script>

    <script type="text/javascript">

        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });

            AddEvents();
        });
        function AddEvents() {

        }

 
    </script>
<asp:HiddenField ID="hdnIdRegional" runat="server"/>
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Diretoria Regional" Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Text="Código da Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" SqlOrder="regional" SqlSelect=" select distinct  id_regional ,regional from tce_regional m "
                        GridWidth="600px" ArgumentColumns="50" Columns="10" MaxLength="10" OnChanged="tseRegional_Changed"
                        DataType="Number" Key="id_regional">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlInformacoes" runat="server" GroupingText="Informe os dados para inclusão:"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label7" runat="server" Text="Nome:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtNome" MaxLength="50" Width="400px"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label8" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" SkinID="numerico" >
                    </asp:TextBox>
                    <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP"
                        Modal="true" SkinID="CEP" />
                        <asp:RequiredFieldValidator ErrorMessage="CEP: Preenchimento obrigatório." ID="rfvCEP"
                        runat="server" ControlToValidate="txtCEP" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revCEP" ControlToValidate="txtCEP" ValidationExpression="^.{8}$"
                        runat="server" ErrorMessage="CEP: Preenchimento de oito números obrigatório."
                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RegularExpressionValidator>
                </td>               
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="380px"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblUF" runat="server" Text="UF: "></asp:Label>
                </td>
                <td>
                    <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Width="380px" onkeypress="return endereco(event);" />
                    <asp:RequiredFieldValidator ID="rfvEndereco" runat="server" ControlToValidate="txtEndereco"
                        InitialValue="" ErrorMessage="Endereço: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblEnd_Num" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEnd_Num" runat="server" MaxLength="15" SkinID="numerico">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNum" runat="server" ControlToValidate="txtEnd_Num"
                        InitialValue="" ErrorMessage="Número: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEnd_Num" runat="server" ControlToValidate="txtEnd_Num"
                        ValidationExpression="^[+]?\d*$" ErrorMessage="Número: Só é possível inserir número."
                        ValidationGroup="SalvarForm">
                                            <img src="../Images/AlertaMens.gif" alt="Só é possível inserir número"/>
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEnd_Compl" runat="server" Text="Complemento:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEnd_Compl" runat="server" MaxLength="50" Width="380px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBairro" runat="server" Width="380px" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ControlToValidate="txtBairro"
                        InitialValue="" ErrorMessage="Bairro: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnSalvar" runat="server" OnClick="btnSalvar_Click" Text="Salvar"
                        ValidationGroup="SalvarForm" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdRegional" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdRegional"
                        DataSourceID="odsRegional" KeyFieldName="ID_REGIONAL" OnCellEditorInitialize="grdRegional_CellEditorInitialize"
                        OnStartRowEditing="grdRegional_StartRowEditing" OnInitNewRow="grdRegional_InitNewRow">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                               
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_REGIONAL" ReadOnly="true"
                                VisibleIndex="1" Visible="FALSE">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="REGIONAL" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>                           
                            <dxwgv:GridViewDataSpinEditColumn Caption="CEP" FieldName="CEP" VisibleIndex="4">
                            </dxwgv:GridViewDataSpinEditColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="NOME" VisibleIndex="6"
                                ReadOnly="TRUE">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Logradouro" FieldName="LOGRADOURO" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataSpinEditColumn Caption="Número" FieldName="NUMERO" VisibleIndex="8"
                                Width="70px">
                            </dxwgv:GridViewDataSpinEditColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Complemento" FieldName="COMPLEMENTO" VisibleIndex="9">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Bairro" FieldName="BAIRRO" VisibleIndex="10">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsRegional" TypeName="Techne.Lyceum.Net.Basico.DiretoriaRegional"
        runat="server" SelectMethod="Listar" OnDeleting="odsRegional_Deleting" DeleteMethod="Delete"
       >
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" PropertyName="DBValue" Name="regional" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
