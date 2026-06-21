<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeriodoLetivo.aspx.cs"
    Inherits="Techne.Lyceum.Net.Basico.PeriodoLetivo" MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conPeriodoLetivo" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPeriodoLetivo" runat="server" TypeName="Techne.Lyceum.Net.Basico.PeriodoLetivo"
        SelectMethod="Lista" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPeriodoLetivo" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdPeriodoLetivo" DataSourceID="odsPeriodoLetivo" OnCustomUnboundColumnData="grdPeriodoLetivo_CustomUnboundColumnData"
        KeyFieldName="CompositeKey" OnCellEditorInitialize="grdPeriodoLetivo_CellEditorInitialize"
        OnInitNewRow="grdPeriodoLetivo_InitNewRow" OnStartRowEditing="grdPeriodoLetivo_StartRowEditing"
        OnAfterPerformCallback="grdPeriodoLetivo_AfterPerformCallback" OnRowInserting="grdPeriodoLetivo_RowInserting"
        OnRowUpdating="grdPeriodoLetivo_RowUpdating" OnRowDeleting="grdPeriodoLetivo_RowDeleting">
        <settingsbehavior confirmdelete="true" />
        <settingsediting mode="EditForm" />
        <settingstext confirmdelete="Confirma a remoção?" emptydatarow="Não existem dados." />
        <templates>
            <EditForm>
                <div style="text-align: left; padding: 2px 2px 2px 2px">
                    <dxwgv:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                    <dxwgv:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAno" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ano" ID="ASPxGridViewTemplateReplacement1"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblPeriodo" runat="server" Text="Período:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="periodo" ID="ASPxGridViewTemplateReplacement2"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataInicio" runat="server" Text="Data Início:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="dt_inicio" ID="ASPxGridViewTemplateReplacement3"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataFim" runat="server" Text="Data Fim:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="dt_fim" ID="ASPxGridViewTemplateReplacement4"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataInicioAula" runat="server" Text="Data Início Aula:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="dt_inicio_aula" ID="ASPxGridViewTemplateReplacement5"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataFimAula" runat="server" Text="Data Fim Aula:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="dt_fim_aula" ID="ASPxGridViewTemplateReplacement6"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataInicioDocente" runat="server" Text="Data Início Docente: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_inicio_docente" ID="ASPxGridViewTemplateReplacement9"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataFimDocente" runat="server" Text="Data Fim Docente: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_fim_docente" ID="ASPxGridViewTemplateReplacement10"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Data Início Indicação Eletiva: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_inicio_indicacao_eletiva" ID="ASPxGridViewTemplateReplacement7"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Data Fim Indicação Eletiva: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_fim_indicacao_eletiva" ID="ASPxGridViewTemplateReplacement8"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Data Início Distribuição Eletiva: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_inicio_distribuicao_eletiva" ID="ASPxGridViewTemplateReplacement11"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Data Fim Distribuição Eletiva: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="data_fim_distribuicao_eletiva" ID="ASPxGridViewTemplateReplacement15"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDescricao" runat="server" Text="Especificação: "></asp:Label>
                            </td>
                            <td colspan="3">
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="descricao" ID="ASPxGridViewTemplateReplacement12"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblProximoAno" runat="server" Text="Próximo Ano: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="per_ano" ID="ASPxGridViewTemplateReplacement13"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblProximoPeriodo" runat="server" Text="Próximo Ano Letivo: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="per_periodo" ID="ASPxGridViewTemplateReplacement14"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                         <td>
                          <asp:Label ID="Label5" runat="server" Text="Quantidade de Ciclos Avaliativos:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="qtde_subperiodo" ID="ASPxGridViewTemplateReplacement25"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                    </table>
                </div>
            </EditForm>
        </templates>
        <columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="4%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPeriodoLetivo.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                VisibleIndex="1" Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ano" VisibleIndex="1" Width="150px"
                Settings-FilterMode="Value">
                <PropertiesTextEdit MaxLength="4" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Ano." IsRequired="True" />
                        <RegularExpression ErrorText="O campo Ano só aceita anos entre 1900 e 2999." ValidationExpression="^([1]+[9]+[0-9]+[0-9]|[2]+[0-9]+[0-9]+[0-9])*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="periodo" VisibleIndex="2"
                Width="150px">
                <PropertiesTextEdit MaxLength="2" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Período." IsRequired="True" />
                        <RegularExpression ErrorText="O campo Período só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="dt_inicio" VisibleIndex="3">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="dt_fim" VisibleIndex="4">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Fim." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Aula" FieldName="dt_inicio_aula"
                VisibleIndex="5">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Início Aula." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Aula" FieldName="dt_fim_aula" VisibleIndex="6">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Fim Aula." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
             <dxwgv:GridViewDataTextColumn Caption="Quantidade Ciclos Avaliativos" FieldName="qtde_subperiodo" VisibleIndex="7"
                Width="80px">
                <PropertiesTextEdit MaxLength="2" Width="80px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Quantidade Ciclos Avaliativos." IsRequired="True" />
                        <RegularExpression ErrorText="O campo Quantidade Ciclos Avaliativos só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Docente" FieldName="data_inicio_docente"
                VisibleIndex="7">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Docente" FieldName="data_fim_docente"
                VisibleIndex="8">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Indicação Eletiva" FieldName="data_inicio_indicacao_eletiva"
                VisibleIndex="9">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Indicação Eletiva" FieldName="data_fim_indicacao_eletiva"
                VisibleIndex="10">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Distribuição Eletiva" FieldName="data_inicio_distribuicao_eletiva"
                VisibleIndex="11">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Distribuição Eletiva" FieldName="data_fim_distribuicao_eletiva"
                VisibleIndex="12">
                <PropertiesDateEdit Width="150px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Especificação" FieldName="descricao" VisibleIndex="12"
                Width="400px">
                <PropertiesTextEdit Width="720px" MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Próximo Ano" FieldName="per_ano" VisibleIndex="13">
                <PropertiesTextEdit MaxLength="4" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="O campo Ano só aceita anos entre 1900 e 2999." ValidationExpression="^([1]+[9]+[0-9]+[0-9]|[2]+[0-9]+[0-9]+[0-9])*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Próximo Ano Letivo" FieldName="per_periodo"
                VisibleIndex="14">
                <PropertiesTextEdit MaxLength="2" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="O campo Próximo Ano Letivo só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            
        </columns>
        <settings showfilterrow="True" showfilterrowmenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
