<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="QuestaoAvaliacaoCurriculoMinimo.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.QuestaoAvaliacaoCurriculoMinimo"
    Title="Questão Avaliação Currículo Mínimo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button ==
    2) { alert("Proibido utilizar o botao direito neste campo"); }
        }

        function onlyNumbers()
        { if (event.keyCode < 48 || event.keyCode > 57) event.keyCode = 0; }

        function blocTexto(campo, qtde) {
            var quant = qtde;

            var valor = $.trim($(campo).val());
            //            var valor = campo.value;

            var total = valor.length;

            if (total <= quant) {
                var resto = quant - total;

            }
            else {
                $(campo).val(valor.substr(0, quant));
            }
        }


        $(document).ready(function() {
        $('#<%=txtQuestao.ClientID %>').bind('keyup', function() { blocTexto(this, 150) });
            $('#<%=txtOrdem.ClientID %>').bind('keyup', function() { blocTexto(this, 5) });
        });

    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão / Consulta:"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano / Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblBimestre" runat="server" Text="Bimestre:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBimestre" runat="server" AutoPostBack="True" DataTextField="descricao"
                        DataValueField="SUBPERIODO" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblOrdem" runat="server" Text="Ordem:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOrdem" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblQuestao" runat="server" Text="Questão:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtQuestao" runat="server" Width="600"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="Habilitada" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Desabilitada" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvarAvaliacao" runat="server" ValidationGroup="SalvarForm" Text="Incluir Avaliação"
                        OnClick="btnSalvarAvaliacao_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnGrid" runat="server">
        <dxwgv:ASPxGridView ID="grdAvaliacao" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdAvaliacao" KeyFieldName="ID_AVALIACAO_CM" DataSourceID="odsAvaliacao"
            OnCellEditorInitialize="grdAvaliacao_CellEditorInitialize" OnStartRowEditing="grdAvaliacao_StartRowEditing"
            OnAfterPerformCallback="grdAvaliacao_AfterPerformCallback" OnRowValidating="grdAvaliacao_RowValidating">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <EditButton Text="Editar" Visible="True">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <DeleteButton Text="Remover" Visible="True">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                    <UpdateButton Text="Salvar">
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_AVALIACAO_CM" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Bimestre" FieldName="SUBPERIODO" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ordem" VisibleIndex="4" FieldName="ORDEM" PropertiesTextEdit-MaxLength="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Questao" VisibleIndex="5" FieldName="AVALIACAO" PropertiesTextEdit-MaxLength="100">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Habilitado?" FieldName="HABILITADO"
                    VisibleIndex="7" Width="120px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DT_CADASTRO" VisibleIndex="8" Visible = "false" >
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsAvaliacao" TypeName="Techne.Lyceum.Net.Academico.QuestaoAvaliacaoCurriculoMinimo"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" DeleteMethod="Delete"
        OnDeleting="odsAvaliacao_Deleting" OnUpdating="odsAvaliacao_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
