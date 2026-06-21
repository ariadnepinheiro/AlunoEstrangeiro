<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ReposicaoAulas.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.ReposicaoAulas"
    Title="Reposição de Aulas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar turma">
        <div>
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:DropDownList ID="ddlAno" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" DataTextField="ano" DataValueField="ano"
                            Width="70px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="ReadOnlyField" DataTextField="id_reduzida"
                            AutoPostBack="True" DataValueField="periodo" Width="100px" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPeriodoPesquisa" runat="server" ControlToValidate="ddlPeriodo"
                            ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Coordenadoria:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseCoordenadoria" runat="server" Argument="descricao" ArgumentColumns="50"
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseCoordenadoria_Changed"
                            Key="nucleo" SqlSelect="SELECT nucleo, descricao FROM ly_nucleo">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="nucleo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere=" nucleo = #tseCoordenadoria# " GridWidth="600px" ArgumentColumns="50"
                            OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" nucleo = #tseCoordenadoria# AND municipio = #tseMunicipio# " GridWidth="850px"
                            OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblPrefixoUnidade" runat="server" Text="Prefixo Unidade:"
                            Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrefixoUnidade" runat="server" ReadOnly="true" CssClass="ReadOnlyField"
                            Width="10%" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Turma:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:DropDownList ID="ddlTurma" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged" DataTextField="grade"
                            AppendDataBoundItems="true" DataValueField="grade">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTurma"
                            ErrorMessage="Turma: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        &nbsp;
                    </td>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdReposicao" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdReposicao" KeyFieldName="CompositeKey" DataSourceID="odsReposicao"
        OnRowValidating="grdReposicao_RowValidating" OnStartRowEditing="grdReposicao_StartRowEditing"
        OnCellEditorInitialize="grdReposicao_CellEditorInitialize" OnAfterPerformCallback="grdReposicao_AfterPerformCallback"
        OnRowUpdating="grdReposicao_RowUpdating" OnCustomUnboundColumnData="grdReposicao_CustomUnboundColumnData"
        OnRowDeleting="grdReposicao_RowDeleting">
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
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
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                VisibleIndex="0" Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo_disciplina" FieldName="TIPO_DISCIPLINA"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="CODIGO" VisibleIndex="2" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano" VisibleIndex="3" FieldName="ANO" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Num_Func" VisibleIndex="4" FieldName="NUM_FUNC"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" VisibleIndex="5" FieldName="TURNO"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="FACULDADE" VisibleIndex="6"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Dia" VisibleIndex="7" FieldName="DIA_SEMANA"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aula" FieldName="AULA" VisibleIndex="8" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="disciplina" FieldName="DISCIPLINA" VisibleIndex="9"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="turma" FieldName="TURMA" VisibleIndex="10"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="11"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="data_inicio" VisibleIndex="12" FieldName="DATA_INICIO"
                Visible="false" PropertiesTextEdit-MaxLength="100">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" VisibleIndex="13" FieldName="MATRICULA">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID/VÍnculo" VisibleIndex="14" FieldName="IDVINCULO">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" VisibleIndex="15" FieldName="NOME_COMPL">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Horário" VisibleIndex="16" FieldName="HORARIO">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Dia da Semana" VisibleIndex="17" FieldName="DIA">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" VisibleIndex="18" FieldName="NOME_DISCIPLINA">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo Aula" VisibleIndex="19" FieldName="TIPO">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="Matrícula Substituição" FieldName="MATRICULA_SUBSTITUTO"
                VisibleIndex="20">
            </dxwgv:GridViewDataSpinEditColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID/VÍnculo Substituição*" VisibleIndex="21"
                FieldName="IDVINCULO2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" VisibleIndex="22" FieldName="NOME_SUBSTITUTO">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início*" HeaderStyle-Font-Bold="true"
                FieldName="DATA_INICIO_SUBSTITUICAO" VisibleIndex="23" Width="15%">
                <PropertiesDateEdit Width="90%">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim*" HeaderStyle-Font-Bold="true" FieldName="DATA_FIM_SUBSTITUICAO"
                VisibleIndex="24" Width="15%">
                <PropertiesDateEdit Width="90%">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data Fim." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="false" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <br />
    <asp:ObjectDataSource ID="odsReposicao" TypeName="Techne.Lyceum.Net.Curriculo.ReposicaoAulas"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="ddlTurma" PropertyName="SelectedValue" Name="turma" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" PropertyName="DBValue" Name="unidade" />
            <asp:ControlParameter ControlID="hdDataInicio" PropertyName="Value" Name="dtinicio" />
            <asp:ControlParameter ControlID="hdDataFim" PropertyName="Value" Name="dtfim" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdDataInicio" runat="server" Value="01/08/2011" />
    <asp:HiddenField ID="hdDataFim" runat="server" Value="30/08/2011" />
    <asp:HiddenField ID="hdValorGLP" runat="server" />
    <br />
    <br />
</asp:Content>
