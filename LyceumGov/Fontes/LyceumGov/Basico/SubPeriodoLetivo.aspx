<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubPeriodoLetivo.aspx.cs"
    Inherits="Techne.Lyceum.Net.Basico.SubPeriodoLetivo" MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conSubPeriodoLetivo" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnAnoChanged(cmbAno) {
            if (grdSubPeriodoLetivo != null) {
                if (cmbAno.GetValue() != null) {
                    grdSubPeriodoLetivo.GetEditor("periodo").PerformCallback(cmbAno.GetValue().toString());
                }
            }
        }
    </script>

    <asp:ObjectDataSource ID="odsSubPeriodoLetivo" runat="server" TypeName="Techne.Lyceum.RN.PeriodoLetivo"
        SelectMethod="ConsultarSubPeriodoLetivo" UpdateMethod="UpdateSubPeriodo" InsertMethod="InsertSubPeriodo"
        DeleteMethod="DeleteSubPeriodo" OnDeleting="odsSubPeriodoLetivo_Deleting" OnInserting="odsSubPeriodoLetivo_Inserting"
        OnUpdating="odsSubPeriodoLetivo_Updating"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAnoPeriodo" runat="server" SelectMethod="ConsultarAnoPeriodo"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsPeriodo" runat="server" SelectMethod="ConsultarPeriodo"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo">
        <SelectParameters>
            <asp:Parameter Name="ano" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsPeriodoTodos" runat="server" SelectMethod="ConsultarPeriodo"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdSubPeriodoLetivo" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdSubPeriodoLetivo" DataSourceID="odsSubPeriodoLetivo" KeyFieldName="CompositeKey"
        OnCustomUnboundColumnData="grdSubPeriodoLetivo_CustomUnboundColumnData" OnRowDeleting="grdSubPeriodoLetivo_RowDeleting"
        OnRowUpdating="grdSubPeriodoLetivo_RowUpdating" OnCellEditorInitialize="grdSubPeriodoLetivo_CellEditorInitialize"
        Font-Names="Verdana" Font-Size="Small" Width="877px" OnRowValidating="grdSubPeriodoLetivo_RowValidating"
        OnInitNewRow="grdSubPeriodoLetivo_InitNewRow" OnStartRowEditing="grdSubPeriodoLetivo_StartRowEditing"
        OnBeforeGetCallbackResult="grdSubPeriodoLetivo_BeforeGetCallbackResult" OnRowInserting="grdSubPeriodoLetivo_RowInserting"
        OnAfterPerformCallback="grdSubPeriodoLetivo_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdSubPeriodoLetivo.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="anoperiodo" VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsAnoPeriodo" TextField="anoperiodo" ValueField="anoperiodo"
                    ValueType="System.String" DropDownWidth="120px" Width="120px" MaxLength="4">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o ano letivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código*" HeaderStyle-Font-Bold="true" FieldName="subperiodo"
                VisibleIndex="3" Width="70px">
                <PropertiesTextEdit MaxLength="2" Width="70px">
                    <ClientSideEvents KeyPress="function (s, e){ SomentePermitirNumeros(s, e.htmlEvent); }" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o código." IsRequired="True" />
                        <RegularExpression ErrorText="O campo Código só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                VisibleIndex="4" Width="250px">
                <PropertiesTextEdit MaxLength="100" Width="350px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início Período Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="dt_inicio" VisibleIndex="5" Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o início do período letivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Término Período Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="dt_fim" VisibleIndex="6" Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o término do período letivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="Dias Letivos" FieldName="dias_letivos"
                VisibleIndex="7" Width="70px">
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="3" NumberFormat="Custom" NumberType="Integer"
                    Width="70px">
                    <SpinButtons ShowIncrementButtons="False">
                    </SpinButtons>
                    <ClientSideEvents Validation="function(s, e) {
	var strVal=e.value;
	var iVal=null;
	e.isValid = true;
	try
	{
		if(strVal!=null)
		{
			iVal=parseInt(strVal);
			if(iVal&lt;1)
			{
				e.isValid = false;
				e.errorText='Valor dos Dias Letivos deve ser positivo';
			}
		}
	}
	catch(ex)
	{
		e.isValid = false;
		e.errorText=ex;
	}
}" />
                </PropertiesSpinEdit>
            </dxwgv:GridViewDataSpinEditColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite para Lançamento de Notas" HeaderStyle-Font-Bold="true"
                FieldName="dt_lancamento" VisibleIndex="8" Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data limite para lançamento de notas/faltas"
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite para Lançamento de Faltas" HeaderStyle-Font-Bold="true"
                FieldName="dt_limite_frequencia" VisibleIndex="8" Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data limite para lançamento de Frequência"
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Limite para Lançamento do Currículo Mínimo"
                HeaderStyle-Font-Bold="true" FieldName="dt_curriculo_minimo" VisibleIndex="8"
                Width="120px">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data limite para lançamento do Currículo Mínimo"
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                Visible="False" VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
