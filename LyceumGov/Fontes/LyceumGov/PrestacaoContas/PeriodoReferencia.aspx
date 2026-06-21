<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PeriodoReferencia.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.PeriodoReferencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="conPeriodoReferencia" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnAnoChanged(cmbAno) {
            if (grdPeriodoReferencia != null) {
                if (cmbAno.GetValue() != null) {
                    grdPeriodoReferencia.GetEditor("periodo").PerformCallback(cmbAno.GetValue().toString());
                }
            }
        }
    </script>

    <asp:ObjectDataSource ID="odsPeriodoReferencia" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.PeriodoReferencia"
        SelectMethod="Lista" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListarAnos" TypeName="Techne.Lyceum.RN.PeriodoLetivo">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMes" runat="server" SelectMethod="ListaMes" TypeName="Techne.Lyceum.RN.Util.Utils">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPeriodoReferencia" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdPeriodoReferencia" DataSourceID="odsPeriodoReferencia"
        KeyFieldName="PERIODOREFERENCIAID" OnRowDeleting="grdPeriodoReferencia_RowDeleting"
        OnRowUpdating="grdPeriodoReferencia_RowUpdating" Font-Names="Verdana" Font-Size="Small"
        OnCellEditorInitialize="grdPeriodoReferencia_CellEditorInitialize" Width="877px"
        OnInitNewRow="grdPeriodoReferencia_InitNewRow" OnStartRowEditing="grdPeriodoReferencia_StartRowEditing"
        OnRowInserting="grdPeriodoReferencia_RowInserting" OnAfterPerformCallback="grdPeriodoReferencia_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPeriodoReferencia.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano*" HeaderStyle-Font-Bold="true" FieldName="ANO"
                VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsAno" TextField="ANO" ValueField="ANO" ValueType="System.Int32"
                    DropDownWidth="120px" Width="120px" MaxLength="4">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Mês Inicial*" HeaderStyle-Font-Bold="true" FieldName="MESINICIAL"
                VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsMes" TextField="DESCRICAO" ValueField="CODIGO"
                    ValueType="System.Int32" DropDownWidth="120px" Width="120px" MaxLength="2">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Mês Final*" HeaderStyle-Font-Bold="true" FieldName="MESFINAL"
                VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsMes" TextField="DESCRICAO" ValueField="CODIGO"
                    ValueType="System.Int32" DropDownWidth="120px" Width="120px" MaxLength="2">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Referência" FieldName="REFERENCIA"
                    VisibleIndex="1" Width="110px">                    
                    <PropertiesComboBox ValueType="System.String" Width="110px">
                        <Items>
                            <dxe:ListEditItem Text="Mensal" Value="Mensal" />
                            <dxe:ListEditItem Text="Bimestral" Value="Bimestral" />
                            <dxe:ListEditItem Text="Trimestral" Value="Trimestral" />
                            <dxe:ListEditItem Text="Semestral" Value="Semestral" />
                            <dxe:ListEditItem Text="Anual" Value="Anual" />
                        </Items>                       
                    </PropertiesComboBox>                   
                </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano do período Anterior*" HeaderStyle-Font-Bold="true"
                FieldName="ANOANTERIOR" VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsAno" TextField="ANO" ValueField="ANO" ValueType="System.Int32"
                    DropDownWidth="120px" Width="120px" MaxLength="4">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Mês Final do período Anterior*" HeaderStyle-Font-Bold="true"
                FieldName="MESANTERIOR" VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsMes" TextField="DESCRICAO" ValueField="CODIGO"
                    ValueType="System.Int32" DropDownWidth="120px" Width="120px" MaxLength="2">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite Prestação*" HeaderStyle-Font-Bold="true"
                FieldName="DATALIMITEPRESTACAOCONTAS" VisibleIndex="5" Width="120px">
                <PropertiesDateEdit Width="120px">
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite Análise*" HeaderStyle-Font-Bold="true"
                FieldName="DATALIMITEANALISE" VisibleIndex="6" Width="120px">
                <PropertiesDateEdit Width="120px">
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite Despesa*" HeaderStyle-Font-Bold="true"
                FieldName="DATALIMITEDESPESAS" VisibleIndex="7" Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings RequiredField-IsRequired="true"
                        RequiredField-ErrorText="Campo obrigatório" />
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
