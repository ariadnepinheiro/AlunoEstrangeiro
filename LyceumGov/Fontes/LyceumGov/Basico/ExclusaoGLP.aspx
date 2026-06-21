<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ExclusaoGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ExclusaoGLP" %>

<asp:Content ID="conFuncaoGLP" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdDocenteFuncaoGLP.cpMessage != null)
                lblMensagem.innerHTML = grdDocenteFuncaoGLP.cpMessage;
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Selecione os dados para busca:"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" SqlSelect="select id_regional,regional from TCE_REGIONAL"
                        SqlOrder="regional" ColumnName="id_regional" Caption="" MaxLength="20" DataType="Number"
                        OnChanged="tseRegional_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT distinct ue.unidade_ens, ue.nome_comp, ue.setor, ue.cgc, ue.id_regional,ua_atual,ua_antiga from VW_ZZCRO_UNIDADE_ENSINO ue"
                        GridWidth="850px" SqlOrder="nome_comp" SqlWhere="ue.ID_REGIONAL = #tseRegional#"
                        OnChanged="tseUnidade_Ensino_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
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
    <br />
    <asp:ObjectDataSource ID="odsDocenteFuncaoGLP" TypeName="Techne.Lyceum.Net.Basico.ExclusaoGLP"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="idregional" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidade_ensino"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdDocenteFuncaoGLP" runat="server" AutoGenerateColumns="False"
        Visible="false" ClientInstanceName="grdDocenteFuncaoGLP" DataSourceID="odsDocenteFuncaoGLP"
        KeyFieldName="id_docente_funcao_glp" OnAfterPerformCallback="grdDocenteFuncaoGLP_AfterPerformCallback"
        OnCustomJSProperties="grdDocenteFuncaoGLP_CustomJSProperties" OnCustomButtonCallback="grdDocenteFuncaoGLP_CustomButtonCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="13" ButtonType="Link" Width="50px" Caption="Excluir">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluir" Text="Excluir" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_DOCENTE_FUNCAO_GLP" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENS"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="IDVINCULO" VisibleIndex="2">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" VisibleIndex="5"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CH Solicitada" FieldName="glp_solicitada"
                VisibleIndex="7">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="segmento_atuacao"
                VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUS" VisibleIndex="9">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DATA" VisibleIndex="10">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="100px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Mês" FieldName="MES" VisibleIndex="11">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="12">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
