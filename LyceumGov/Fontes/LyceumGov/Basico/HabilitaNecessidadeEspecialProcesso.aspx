<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HabilitaNecessidadeEspecialProcesso.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.HabilitaNecessidadeEspecialProcesso"
    Title="Habilita Necessidade Especial Processo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OngrdNecessidadeEndCallBack() {
            if (typeof (grdNecessidade) != 'undefined' && grdNecessidade != null) {
                var valor = 'grade';
                grdNecessidade.PerformCallback(valor);
            }
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o processo" Height="45px"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblProcesso" runat="server" Text="Processo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProcesso" runat="server" Caption="" SqlSelect="SELECT  FILTROPROCESSOID ,
                            DESCRICAO FROM    HADES.DBO.FILTROPROCESSO" ArgumentColumns="60" Columns="10"
                        DataType="Number" AutoPostBack="true" OnChanged="tseProcesso_Changed" MaxLength="5"
                        GridWidth="800px" SqlOrder="FILTROPROCESSOID">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="FILTROPROCESSOID" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="70%" />
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
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsNecessidade" runat="server" TypeName="Techne.Lyceum.RN.NecessidadeEspecial.NecessidadeEspecial"
            SelectMethod="ListaNecessidadeEspecialAtivaPor" UpdateMethod="Update" OnUpdating="odsNecessidade_Updating">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseProcesso" PropertyName="DBValue" Name="filtroProcessoId" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdNecessidade" runat="server" AutoGenerateColumns="False"
            DataSourceID="odsNecessidade" KeyFieldName="NECESSIDADEESPECIALID" OnAfterPerformCallback="grdNecessidade_AfterPerformCallback"
            ClientInstanceName="grdNecessidade" Font-Names="Verdana" Font-Size="Small" OnStartRowEditing="grdNecessidade_StartRowEditing"
            Width="800px">
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <EditButton Text="Editar" Visible="True">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <UpdateButton>
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="NECESSIDADEESPECIALID" FieldName="NECESSIDADEESPECIALID"
                    UnboundType="String" Visible="False" VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="ITEM" ReadOnly="True" UnboundType="String"
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" ReadOnly="True" UnboundType="String"
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Utilizar no Módulo? *" FieldName="HABILITADO"
                    Name="HABILITADO" VisibleIndex="3">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.String" ValueUnchecked="0"
                        DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkHabilitado"
                        DisplayTextUndefined="" NullDisplayText="N">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
