<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PropostaTurnosVagas.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.PropostaTurnosVagas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dxeListBox_Office2003_Blue .dxeHD
        {
            font-weight: bold;
        }
        .dxeListBox_Blue .dxeHD
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <script type="text/javascript">
        function OngrdPropostaEndCallBack() {

            if (typeof (grdProposta) != 'undefined' && grdProposta != null) {
                var valor = 'grade';

                grdProposta.PerformCallback(valor);
            }
        }
        function OnCursoChanged(cmbCurso) {

            grdProposta.GetEditor("SERIE").PerformCallback(cmbCurso.GetValue().toString());
        }

        function OnAnoPeriodoChanged(cmbAnoPeriodo) {

            grdProposta.GetEditor("anoperiodoreferencia").PerformCallback(cmbAnoPeriodo.GetValue().toString());
        }
        function OnEscolaridadeChanged() {

            var descricao = cmbCurso.GetText().toString().split('|');

            txtNivel.SetText(descricao[2]);
            txtModalidade.SetText(descricao[1]);
            txtCodigo.SetText(cmbCurso.GetValue().toString());

            grdProposta.GetEditor("SERIE").PerformCallback(cmbCurso.GetValue().toString());
        }
       
    </script>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Ano e Escola"
        Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" 
                        Font-Bold="true">                                   
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList Height="20px" ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                        DataTextField="ano" DataValueField="ano" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRegional" runat="server" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblEscola" runat="server" Text="Unidade Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"  SqlWhere=" id_regional = #tseRegional# and municipio = #tseMunicipio#"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsProposta" runat="server" TypeName="Techne.Lyceum.Net.Basico.PropostaTurnosVagas"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
     <asp:ObjectDataSource ID="odsCurso" TypeName="Techne.Lyceum.Net.Basico.PropostaTurnosVagas"
        SelectMethod="ListaCurso" runat="server">
    </asp:ObjectDataSource>        
    <asp:Panel ID="pnlProposta" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdProposta" runat="server" DataSourceID="odsProposta" KeyFieldName="ID_PROPOSTA_SEEDUC"
            AutoGenerateColumns="false" ClientInstanceName="grdProposta" OnInitNewRow="grdProposta_InitNewRow"
            OnStartRowEditing="grdProposta_StartRowEditing" OnRowInserting="grdProposta_RowInserting"
            OnAfterPerformCallback="grdProposta_AfterPerformCallback"	
            OnRowUpdating="grdProposta_RowUpdating" OnCellEditorInitialize="grdProposta_CellEditorInitialize" Width="80%">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                onclick="grdProposta.AddNewRow();" />
                        </div>
                    </HeaderCaptionTemplate>
                    <CancelButton Visible="true" Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <EditButton Visible="True" Text="Editar">
                        <Image Url="../img/bt_editar.png" />
                    </EditButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <UpdateButton Visible="true" Text="Alterar">
                        <Image Url="../img/bt_salvar.png" />
                    </UpdateButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="0" FieldName="ID_PROPOSTA_SEEDUC"
                    Visible="false" >
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Ano*" FieldName="ANO" VisibleIndex="1"
                    Width="50px">
                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                        <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Período*" FieldName="PERIODO" VisibleIndex="1"
                    Width="50px">
                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="1" NumberFormat="Custom" NumberType="Integer">
                        <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataTextColumn Caption="Modalidade*" FieldName="MODALIDADE" VisibleIndex="2"
                    Width="110px" ReadOnly="true">
                    <PropertiesTextEdit ClientInstanceName="txtModalidade">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Segmento*" FieldName="NIVEL" VisibleIndex="3"
                    Width="150px" ReadOnly="true">
                    <PropertiesTextEdit ClientInstanceName="txtNivel">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CURSO" ReadOnly="True"
                    VisibleIndex="4">
                    <PropertiesTextEdit ClientInstanceName="txtCodigo">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Curso*" HeaderStyle-Font-Bold="true" FieldName="NOME"
                    VisibleIndex="5" Width="300px">
                    <PropertiesComboBox DataSourceID="odsCurso" EnableDefaultAppearance="false" ListBoxStyle-CssClass="dxeListBox"
                        CssPostfix="Office2003_Blue" ClientInstanceName="cmbCurso" TextFormatString="{1}                                                             |{2}|{3}"
                        ValueField="curso" ValueType="System.String" DropDownWidth="800px" Width="300px"
                        MaxLength="300">
                        <Columns>
                            <dxe:ListBoxColumn Caption="Código" FieldName="curso" Name="curso" Width="100px" />
                            <dxe:ListBoxColumn Caption="Escolaridade" FieldName="nome" Name="nome" Width="270px" />
                            <dxe:ListBoxColumn Caption="Modalidade" FieldName="modalidade" Name="modalidade"
                                Width="260px" />
                            <dxe:ListBoxColumn Caption="Nível" FieldName="nivel" Name="nivel" Width="190px" />
                        </Columns>
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ OnEscolaridadeChanged(); }" />
                        <ListBoxStyle CssClass="dxeListBox">
                        </ListBoxStyle>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="Favor escolher uma escolaridade." />
                        </ValidationSettings>
                    </PropertiesComboBox>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="6" Caption="Série*" FieldName="SERIE"
                    Width="70px">
                    <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                        ClientInstanceName="SERIE">
                    </PropertiesComboBox>
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Vagas Continuidade*" FieldName="VAGAS_CONTINUIDADE" VisibleIndex="7"
                    Width="70px">
                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="3" NumberFormat="Custom" NumberType="Integer">
                        <SpinButtons ShowIncrementButtons="False">
                        </SpinButtons>
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Vagas Novas*" FieldName="VAGAS_NOVAS" VisibleIndex="8"
                    Width="70px">
                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="3" NumberFormat="Custom" NumberType="Integer">
                        <SpinButtons ShowIncrementButtons="False">
                        </SpinButtons>
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TAXAREPROVACAO" Caption="Taxa de Reprovação (%)*" VisibleIndex="10" Width="100px">
                    <PropertiesTextEdit MaxLength="6" DisplayFormatString="00.00" >
                        <MaskSettings Mask="<0..999>,<0..99>" ErrorText="Taxa Reprovação 3 dígitos inteiros e 2 decimais." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
