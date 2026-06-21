<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Servidores.aspx.cs" EnableEventValidation="false" Inherits="Techne.Lyceum.Net.Curriculo.Servidores" %>

<asp:Content ID="conDocentesNaoRegentes" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnSituacaoChanged(source) {
            if (typeof (cmbSituacao) != 'undefined' && cmbSituacao != null) {
                var temmotivo = $("table[temmotivo]").attr("temmotivo");

                if (temmotivo == "False") {
                    var situacao = cmbSituacao.GetValue();
                    if (typeof (situacao) != 'undefined' && situacao != null) {
                        if (situacao == "") {
                            dataini.SetEnabled(false);
                            dataini.SetText('');
                            datafim.SetEnabled(false);
                            datafim.SetText('');
                        }
                        else {
                            var possui_dt_fim = situacao.split('|')[1];
                            if (possui_dt_fim == "N") {
                                datafim.SetEnabled(false);
                                datafim.SetText('');
                            }
                            else
                                datafim.SetEnabled(true);
                            dataini.SetEnabled(true);
                        }
                    }
                    else {
                        dataini.SetEnabled(false);
                        datafim.SetEnabled(false);
                    }
                }

                if (temmotivo == "True") {

                    var situacao = cmbSituacao.GetValue();
                    if (typeof (situacao) != 'undefined' && situacao != null) {
                        cmbSituacao.SetEnabled(false);
                        dataini.SetEnabled(false);
                        var possui_dt_fim = situacao.split('|')[1];
                        if (possui_dt_fim == "N") {
                            datafim.SetEnabled(false);
                            datafim.SetText('');
                            datafim.ReadOnly = false;
                        }
                        else {
                            datafim.SetEnabled(true);
                            datafim.ReadOnly = true;
                        }
                    }


                }

            }
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdServidores.cpMessage != null)
                lblMensagem.innerHTML = grdServidores.cpMessage;
        }

        function OnReadaptadoChanged() {
            if (typeof (chkReadaptado) != 'undefined' && chkReadaptado != null) {
                if (typeof chkReadaptado.GetValue() != 'undefined' && chkReadaptado.GetValue() != null) {

                    if (chkReadaptado.GetValue().toString() == "S") {
                        if ($("table [Readaptado=False]").length) {
                            var currentTime = new Date();
                            data_nomeacao.SetEnabled(true);
                            data_desativacao.SetEnabled(true);
                            data_nomeacao.SetValue(currentTime);
                            data_desativacao.SetText('');
                        }
                        else {
                            data_nomeacao.SetEnabled(false);
                            data_desativacao.SetEnabled(true);
                        }

                    } else if (chkReadaptado.GetValue().toString() == "N") {
                        data_nomeacao.SetEnabled(false);
                        data_desativacao.SetEnabled(false);
                        data_nomeacao.SetText('');
                        data_desativacao.SetText('');
                    }
                }
                else {
                    data_nomeacao.SetEnabled(false);
                    data_desativacao.SetEnabled(false);
                }
            }
        }

        function OnReducaoChChanged() {
            if (typeof (chkReducao) != 'undefined' && chkReducao != null) {
                if (typeof chkReducao.GetValue() != 'undefined' && chkReducao.GetValue() != null) {

                    if (chkReducao.GetValue().toString() == "S") {

                        var currentTime = new Date();
                        data_inireducao.SetEnabled(false);
                        data_fimreducao.SetEnabled(true);

                        if (!data_inireducao.GetValue()) {
                            data_inireducao.SetEnabled(true);
                        }

                    } else if (chkReducao.GetValue().toString() == "N") {
                        data_inireducao.SetEnabled(false);
                        data_fimreducao.SetEnabled(false);
                        data_inireducao.SetText('');
                        data_fimreducao.SetText('');
                    }

                } else {
                    data_inireducao.SetEnabled(false);
                    data_fimreducao.SetEnabled(false);
                }
            }
        }



    </script>

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisa"
        Width="650px">
        <div>
            <table>
                <tr>
                    <td style="text-align: right; width: 20%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseRegional" runat="server" MaxLength="2" AutoPostBack="True"
                            ArgumentColumns="50" OnChanged="tseRegional_Changed" SqlOrder="regional" Key="id_regional"
                            SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                            DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 20%">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla,id_regional from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere=" id_regional = #tseRegional# " OnChanged="tseMunicipio_Changed" MaxLength="8"
                            ArgumentColumns="50" AutoPostBack="true">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                  <td style="text-align: right; width: 20%">
                        <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Situação de Funcionamento:*"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseSituacaoFuncionamento" runat="server" Argument="descr" ArgumentColumns="50"
                            MaxLength="50" Columns="10" Caption="" AutoPostBack="true" SqlOrder="descr" Key="item"
                            SqlSelect="SELECT DISTINCT item, descr  from itemtabela" SqlWhere=" tab = 'SitFuncionamentoUE'"
                            DataType="VarChar" OnChanged="tseSituacaoFuncionamento_Changed">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="item" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="descr" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 20%">
                        <asp:Label ID="lblUnidadeEnsinoTSearch" runat="server" Text="Unidade de Ensino*: "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                            SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, municipio, id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL "
                            SqlWhere=" SITUACAO_FUNCIONAMENTO = #tseSituacaoFuncionamento# and municipio = #tseMunicipio# and id_regional = #tseRegional# " MaxLength="8"
                            ArgumentColumns="50" OnChanged="tseUnidade_Ensino_Changed" AutoPostBack="true"
                            SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsServidores" TypeName="Techne.Lyceum.Net.Curriculo.Servidores"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" OnUpdating="odsServidores_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidade_Ensino" Name="tseUnidade_Ensino" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsFuncao" TypeName="Techne.Lyceum.RN.Funcao" SelectMethod="PreencherComboFuncao"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtUsuarioHidden" Name="usuario" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsSituacao" TypeName="Techne.Lyceum.RN.Licencas" SelectMethod="PreencherComboLicenca"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtUsuarioHidden" Name="usuario" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:TextBox ID="txtUsuarioHidden" runat="server" Visible="false" Text=" " />
    <dxwgv:ASPxGridView ID="grdServidores" runat="server" AutoGenerateColumns="False"
        Visible="False" SkinID="NoConfirmDelete" ClientInstanceName="grdServidores" DataSourceID="odsServidores"
        KeyFieldName="matricula" OnStartRowEditing="grdServidores_StartRowEditing" OnCellEditorInitialize="grdServidores_CellEditorInitialize"
        OnAfterPerformCallback="grdServidores_AfterPerformCallback" OnCustomColumnDisplayText="grdServidores_CustomColumnDisplayText"
        OnRowUpdating="grdServidores_RowUpdating" OnCustomJSProperties="grdServidores_CustomJSProperties"
        OnCustomButtonCallback="grdServidores_CustomButtonCallback">
        <ClientSideEvents EndCallback="function(s, e) {OnSituacaoChanged(s);}" />
        <SettingsEditing Mode="Inline" />
        <Styles>
            <CommandColumn Wrap="False" />
        </Styles>
        <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="1" ButtonType="Link" Width="50px">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluirFunc" Text="Excluir<br>Função"
                        Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="1" ButtonType="Link" Width="50px">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluirSit" Text="Excluir<br>Situação"
                        Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="2"
                Width="90px" ReadOnly="true" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="8" Width="90px">
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="idvinculo" VisibleIndex="3"
                Width="90px" ReadOnly="true" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="8" Width="90px">
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome" VisibleIndex="4" Width="300px"
                ReadOnly="true" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="100" Width="300px">
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cargo" FieldName="cargo" ReadOnly="true" VisibleIndex="5"
                CellStyle-Wrap="False" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina Ingresso" FieldName="disciplina"
                VisibleIndex="6" Width="200px" ReadOnly="true" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit Width="200px">
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Função" FieldName="funcao" VisibleIndex="7"
                Width="200px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsFuncao" TextField="descricao"
                    ValueField="funcao" ClientInstanceName="ddlFuncao" Width="200px">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Readaptação" FieldName="dt_inicio_readaptacao"
                VisibleIndex="9" Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="data_nomeacao">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Readaptação" FieldName="dt_fim_readaptacao"
                VisibleIndex="10" Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="data_desativacao">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataCheckColumn FieldName="readaptado" Caption="Readaptado" VisibleIndex="8"
                Width="100px">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkReadaptado"
                    DisplayTextUndefined=" " NullDisplayText=" ">
                    <ClientSideEvents CheckedChanged="function(s, e) { OnReadaptadoChanged(s);}" Init="function(s, e) { OnReadaptadoChanged(s);}" />
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Situação" FieldName="motivo" VisibleIndex="11"
                Width="200px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsSituacao" TextField="descricao"
                    ClientInstanceName="cmbSituacao" ValueField="motivo" Width="200px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnSituacaoChanged();}" />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="ordemLicenca" FieldName="ordemLicenca" Visible="false"
                ReadOnly="true" VisibleIndex="18">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Situação" FieldName="dataini"
                VisibleIndex="12" Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="dataini">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Situação" FieldName="datafim" VisibleIndex="13"
                Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="datafim">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            
            <dxwgv:GridViewDataTextColumn 
                Caption="Pessoa"
                FieldName="pessoa"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn FieldName="reducaoch" Caption="Redução de CH" VisibleIndex="14"
                Width="100px">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkReducao"
                    DisplayTextUndefined=" " NullDisplayText=" ">
                    <ClientSideEvents CheckedChanged="function(s, e) { OnReducaoChChanged(s);}" Init="function(s, e) { OnReducaoChChanged(s);}" />
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início CH" FieldName="dtinich"
                VisibleIndex="15" Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="data_inireducao">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim CH" FieldName="dtfimch"
                VisibleIndex="16" Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="data_fimreducao">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            
            <dxwgv:GridViewDataTextColumn Caption="Aulas Alocadas" FieldName="aulas_alocadas"
                ReadOnly="true" VisibleIndex="17" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aulas Alocadas em GLP" FieldName="aulas_alocadas_glp"
                ReadOnly="true" VisibleIndex="18" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Funcionário" FieldName="num_func" Visible="false"
                VisibleIndex="19">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="pessoa" Visible="false"
                VisibleIndex="20">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="setor" FieldName="setor" Visible="false" ReadOnly="true"
                VisibleIndex="21">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
