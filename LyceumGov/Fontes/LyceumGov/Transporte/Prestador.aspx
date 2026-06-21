<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Prestador.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Prestador" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function formataFixoCelularDDD(b, a) {
            //lert(b);
            vr = b.value = filtraNumeros(filtraCampo(b));
            tam = vr.length;
            if (tam < 10)
                return;

            if (tam == 11) {
                formataCelularDDD(b, a);
            }
            if (tam == 10) {
                formataTelefoneDDD(b, a);

            }
        }

        
    </script>

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para consulta:"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPrestador" runat="server" Font-Names="Verdana" Text="Prestador:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePrestador" runat="server" SqlOrder="nome" SqlSelect="select distinct prestadorid,cpf,cnpj,nome from transporte.prestador"
                        SqlWhere=" ativo = 1 " GridWidth="600px" ArgumentColumns="50" Argument="nome"
                        OnChanged="tsePrestador_Changed" Columns="10" MaxLength="11" Key="prestadorid"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="prestadorid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cnpj" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 740px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnIncluir" runat="server" SkinID="BcSalvar" OnClick="btnIncluir_Click"
            OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Prestador" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="true">
        <dxtc:ASPxPageControl ID="pcPrestador" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="true" OnTabClick="pcPrestador_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Dados Gerais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDados" runat="server">
                            <asp:Panel ID="pnlDados" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblCNPJ_CPF" runat="server" Text="Tipo:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblCNPJ_CPF" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="rblCNPJ_CPF_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Pessoa Jurídica" Value="CNPJ" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Pessoa Física" Value="CPF"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblCNPJ" runat="server" Text="CNPJ:* " SkinID="lblObrigatorio"></asp:Label>
                                            <asp:Label ID="lblCPF" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="19" onkeyup="formataCNPJ(this,event)"
                                                Width="150px">                                                                        
                                            </asp:TextBox>
                                            <asp:TextBox ID="txtCPF" runat="server" MaxLength="15" onkeyup="formataCPF(this,event)"
                                                Width="150px">                                                                        
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNome" runat="server" Text="Nome/Razão Social:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNome" runat="server" MaxLength="200" ReadOnly="false" Width="400px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 50px">
                                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone:"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td style="width: 415px">
                                            <asp:TextBox ID="txtFone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                                                MaxLength="14" Width="100px" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="pnlNovaVigencia" runat="server" Visible="false" GroupingText="Vigência">
                                    <table>
                                        <tr>
                                            <td style="text-align: right; width: 15%">
                                                <asp:Label ID="lblRegionalNova" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearchBox ID="tseRegionalNova" runat="server" Argument="regional" ArgumentColumns="50"
                                                    MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegionalNova_Changed"
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
                                            <td style="text-align: right; width: 15%">
                                                <asp:Label Font-Names="Verdana" ID="lblMunicipioNova" runat="server" Text="Município:"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearchBox ID="tseMunicipioNova" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                                    SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegionalNova# " GridWidth="600px"
                                                    ArgumentColumns="50" OnChanged="tseMunicipioNova_Changed" Columns="10" MaxLength="10">
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
                                                <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavelNova" SkinID="lblObrigatorio"
                                                    runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearchBox ID="tseUnidadeResponsavelNova" runat="server" Caption="" Key="unidade_ens"
                                                    MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                                    OnChanged="tseUnidadeResponsavelNova_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                                                    SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                                    SqlWhere="  situacao = 'ESTADUAL' and id_regional = #tseRegionalNova# AND municipio = #tseMunicipioNova# ">
                                                    <GridColumns>
                                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />                                                        
                                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="30%" />
                                                    </GridColumns>
                                                </tweb:TSearchBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label9" runat="server" SkinID="lblObrigatorio" Text="Data Início:* "></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDataInicioNova" runat="server" Width="120px" Enabled="true"
                                                    EnableDefaultAppearance="true" ClientInstanceName="dtDataInicioNova" CalendarProperties-ClearButtonText="Limpar"
                                                    CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text="Data Fim:* "></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDataFimNova" runat="server" Width="120px" Enabled="true"
                                                    EnableDefaultAppearance="true" ClientInstanceName="dtDataFimNova" CalendarProperties-ClearButtonText="Limpar"
                                                    CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Vígência Unidade Escolar">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccVigencia" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
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
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
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
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                                runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                                OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                                                SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                                SqlWhere=" situacao = 'ESTADUAL' and id_regional = #tseRegional# AND municipio = #tseMunicipio# ">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />                                                    
													<tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
													<tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Data Início:* "></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                                ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Text="Data Fim:* "></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                                ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnIncluirVigencia" runat="server" Text="Incluir Vigência" OnClick="btnIncluirVigencia_Click"
                                                OnClientClick="Bloqueio()" />
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCancelarVigencia" runat="server" Text="Cancelar" OnClick="btnCancelarVigencia_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:ObjectDataSource ID="odsPrestadorVigencia" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Prestador"
                                SelectMethod="ListarVigenciaPrestador" UpdateMethod="UpdatePrestador" DeleteMethod="DeletePrestador">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tsePrestador" DefaultValue="" Name="prestadorId"
                                        PropertyName="DBValue" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdPrestadorVigencia" runat="server" DataSourceID="odsPrestadorVigencia"
                                KeyFieldName="PRESTADORVIGENCIAID" AutoGenerateColumns="false" ClientInstanceName="grdPrestadorVigencia"
                                OnInitNewRow="grdPrestadorVigencia_InitNewRow" OnStartRowEditing="grdPrestadorVigencia_StartRowEditing"
                                OnRowUpdating="grdPrestadorVigencia_RowUpdating" OnRowDeleting="grdPrestadorVigencia_RowDeleting"
                                Width="600px" OnCustomColumnDisplayText="grdPrestadorVigencia_CustomColumnDisplayText"
                                OnAfterPerformCallback="grdPrestadorVigencia_AfterPerformCallback" OnCommandButtonInitialize="grdPrestadorVigencia_CommandButtonInitialize">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="InLine" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                        <CancelButton Visible="true" Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                        <EditButton Visible="True" Text="Editar">
                                            <Image Url="../img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Visible="True" Text="Remover">
                                            <Image Url="../img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <ClearFilterButton Text="Limpar" Visible="True">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                        <UpdateButton Visible="true" Text="Alterar">
                                            <Image Url="../img/bt_salvar.png" />
                                        </UpdateButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PRESTADORVIGENCIAID" Name="PRESTADORVIGENCIAID"
                                        VisibleIndex="1" FieldName="PRESTADORVIGENCIAID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PRESTADORID" Name="PRESTADORID" VisibleIndex="1"
                                        FieldName="PRESTADORID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                     <dxwgv:GridViewDataTextColumn Caption="Regional" Name="REGIONAL" ReadOnly="true" VisibleIndex="2"
                                        FieldName="REGIONAL" Width="200px">
                                        <PropertiesTextEdit>
                                            <ReadOnlyStyle>
                                                <Border BorderStyle="None"></Border>
                                            </ReadOnlyStyle>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                     <dxwgv:GridViewDataTextColumn Caption="Município" Name="MUNICIPIODESCRICAO" ReadOnly="true" VisibleIndex="3"
                                        FieldName="MUNICIPIODESCRICAO" Width="200px">
                                        <PropertiesTextEdit>
                                            <ReadOnlyStyle>
                                                <Border BorderStyle="None"></Border>
                                            </ReadOnlyStyle>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Censo" Name="CENSO" VisibleIndex="4" FieldName="CENSO"
                                        Width="100px" ReadOnly="true">
                                        <PropertiesTextEdit>
                                            <ReadOnlyStyle>
                                                <Border BorderStyle="None"></Border>
                                            </ReadOnlyStyle>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Escola" Name="ESCOLA" ReadOnly="true" VisibleIndex="5"
                                        FieldName="ESCOLA" Width="200px">
                                        <PropertiesTextEdit>
                                            <ReadOnlyStyle>
                                                <Border BorderStyle="None"></Border>
                                            </ReadOnlyStyle>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Início*" FieldName="DATAINICIO" VisibleIndex="6"
                                        Width="100px">
                                        <PropertiesDateEdit Width="100px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Fim*" FieldName="DATAFIM" VisibleIndex="7"
                                        Width="100px">
                                        <PropertiesDateEdit Width="100px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Condutor">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="pnlCondutor" runat="server">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label Font-Names="Verdana" ID="lblCondutor" runat="server" Text="Condutor:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseCondutor" runat="server" SqlOrder="nome" SqlSelect="select distinct c.condutorid, cpf, nome, numerocnh from Transporte.condutor c "
                                                SqlWhere=" ativo = 1 " GridWidth="600px" ArgumentColumns="50" Argument="nome"
                                                OnChanged="tseCondutor_Changed" Columns="10" MaxLength="11" Key="cpf" DataType="VarChar">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="condutorid" Width="10%" />
                                                    <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                                    <tweb:TSearchBoxColumn Caption="Número CNH" FieldName="numerocnh" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnIncluirCondutor" runat="server" Text="Associar Condutor" OnClick="btnIncluirCondutor_Click"
                                                OnClientClick="Bloqueio()" />
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCancelarCondutor" runat="server" Text="Cancelar" OnClick="btnCancelarCondutor_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:ObjectDataSource ID="odsCondutor" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Prestador"
                                    SelectMethod="ListarCondutor" DeleteMethod="Delete">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="tsePrestador" DefaultValue="" Name="prestadorId"
                                            PropertyName="DBValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdCondutor" runat="server" DataSourceID="odsCondutor" KeyFieldName="CompositeKey"
                                    AutoGenerateColumns="false" ClientInstanceName="grdCondutor" OnInitNewRow="grdCondutor_InitNewRow"
                                    OnStartRowEditing="grdCondutor_StartRowEditing" OnRowDeleting="grdCondutor_RowDeleting"
                                    Width="700px" OnCustomColumnDisplayText="grdCondutor_CustomColumnDisplayText"
                                    OnAfterPerformCallback="grdCondutor_AfterPerformCallback" OnCustomUnboundColumnData="grdCondutor_CustomUnboundColumnData">
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsEditing Mode="InLine" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior ConfirmDelete="true" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <DeleteButton Visible="True" Text="Remover">
                                                <Image Url="../img/bt_exclui2.png" />
                                            </DeleteButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PRESTADORID" Name="PRESTADORID" VisibleIndex="1"
                                            FieldName="PRESTADORID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CONDUTORID" Name="CONDUTORID" VisibleIndex="1"
                                            FieldName="CONDUTORID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CPF" Name="CPF" VisibleIndex="2" FieldName="CPF"
                                            Width="100px" ReadOnly="true">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" ReadOnly="true" VisibleIndex="3"
                                            FieldName="NOME" Width="200px">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                            VisibleIndex="9" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
