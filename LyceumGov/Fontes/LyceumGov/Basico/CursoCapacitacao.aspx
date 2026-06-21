<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CursoCapacitacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CursoCapacitacao"
    Title="Curso de Capacitação" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
    
<asp:Content ID="conCursoCapacitacao" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) { alert("Proibido utilizar o botao direito neste campo"); }
        }

        //function onlyNumbers()
        //{ if (event.keyCode < 48 || event.keyCode > 57) event.keyCode = 0; }       

        function isNumberKey(evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

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

    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão / Consulta:"
        Width="800px">
        <table>
            <tr>
                <td colspan="2">
                    <p>
                        &nbsp;<asp:Panel ID="pnTipoCurso" runat="server" Enabled="true" GroupingText="Tipo de Curso:*"
                            top="100" Visible="true">
                            <asp:CheckBoxList ID="chkListTipoCurso" runat="server" DataSourceID="odsTipoCurso"
                                DataTextField="DESCRICAO" DataValueField="TIPOCURSOCAPACITACAOID" RepeatColumns="2"
                                RepeatDirection="Vertical" RepeatLayout="Table" Width="100%">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:ObjectDataSource ID="odsTipoCurso" TypeName="Techne.Lyceum.RN.TipoCursoCapacitacao"
                            SelectMethod="Listar" runat="server"></asp:ObjectDataSource>
                        <p>
                        </p>
                    </p>
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2">
                    <asp:TextBox ID="txtCursoCapacitacaoID" runat="server" Width="100px" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none">
                <td style="text-align: right; width: 25%">
                    <asp:Label ID="lblOferecidoSEEDUC" runat="server" Text="Oferecido pela SEEDUC:*"
                        SkinID="lblObrigatorio" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rbtListOferecidoSEEDUC" runat="server" RepeatDirection="Horizontal"
                        Visible="false">
                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 25%">
                    <asp:Label ID="lblAreaConhecimento" runat="server" Text="Área de Conhecimento:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAreaConhecimento" runat="server" DataTextField="DESCRICAO"
                        DataValueField="AREACONHECIMENTOID" AppendDataBoundItems="true" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCursoCapacitacao" runat="server" Text="Curso/Capacitação:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeCursoCapacitacao" runat="server" Width="500px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNomeInstituicao" runat="server" Text="Nome da Instituição:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeInstituicao" runat="server" Width="500px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga Horária:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCargaHoraria" runat="server" Width="80px" MaxLength="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDataInicio" runat="server" Text="Data Início:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dteDataInicio" runat="server" MinDate="1901-01-01">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDataConclusao" runat="server" Text="Data de Conclusão:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dteDataConclusao" runat="server" MinDate="1901-01-01">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSalvarCursoCapacitacao" runat="server" ValidationGroup="SalvarForm"
                        Text="Incluir Capacitação" OnClick="btnSalvarCursoCapacitacao_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsCursoCapacitacao" TypeName="Techne.Lyceum.Net.Basico.CursoCapacitacao"
        runat="server" DeleteMethod="DeleteCursoCapacitacao" SelectMethod="Listar" OldValuesParameterFormatString="{0}"
        OnDeleting="odsCursoCapacitacao_Deleting"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAreaConhecimento" TypeName="Techne.Lyceum.RN.AreaConhecimento"
        SelectMethod="Listar" runat="server"></asp:ObjectDataSource>
    <asp:Panel ID="pnGrid" runat="server">
        <dxwgv:ASPxGridView ID="grdCursoCapacitacao" runat="server" AutoGenerateColumns="False"
            EnableCallBacks="False" ClientInstanceName="grdCursoCapacitacao" KeyFieldName="CURSOCAPACITACAOID"
            DataSourceID="odsCursoCapacitacao" OnAfterPerformCallback="grdCursoCapacitacao_AfterPerformCallback"
            OnCustomButtonCallback="grdCursoCapacitacao_CustomButtonCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="Editar" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                    <DeleteButton Text="Remover" Visible="True">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="CURSOCAPACITACAOID" VisibleIndex="0"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="IDAREA" FieldName="AREACONHECIMENTOID" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipos de Curso" FieldName="TIPOSCURSO" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Área de Conhecimento" VisibleIndex="3"
                    FieldName="AREACONHECIMENTO" Width="200px">
                    <PropertiesComboBox DataSourceID="odsAreaConhecimento" MaxLength="20" TextField="DESCRICAO"
                        ValueField="AREACONHECIMENTOID" ValueType="System.String" Width="90%" EnableIncrementalFiltering="true">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Curso/Capacitação" FieldName="NOMECURSO" VisibleIndex="4" Width="200px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome da Instituição" FieldName="NOMEINSTITUICAO"
                    VisibleIndex="5" Width="200px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Carga Horária" FieldName="CARGAHORARIA" VisibleIndex="6" Width="70px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="7" Width="100px">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data de Conclusão" FieldName="DATACONCLUSAO"
                    VisibleIndex="8" Width="100px">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Oferecido SEEDUC" FieldName="OFERECIDOSEEDUC"
                    VisibleIndex="9" Visible="false">
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
