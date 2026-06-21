<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DesvinculaAlunoRota.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.DesvinculaAlunoRota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para consulta:"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeFiltro" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                        OnChanged="tseUnidadeFiltro_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" situacao = 'ESTADUAL' ">
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
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRota" runat="server" Font-Names="Verdana" Text="Rota:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRota" runat="server" SqlOrder="codigo" SqlSelect="select rotaid, codigo, nome, rotatrajetoidida, rotatrajetoidvolta, tipocalculopagamento,censo from [transporte].[vw_rota]"
                        GridWidth="600px" ArgumentColumns="50" Argument="nome" OnChanged="tseRota_Changed"
                        Columns="10" MaxLength="11" Key="codigo" DataType="VarChar" SqlWhere=" CENSO = #tseUnidadeFiltro# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Tipo" FieldName="tipocalculopagamento" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Chave" FieldName="rotaid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Ida" FieldName="rotatrajetoidida" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Volta" FieldName="rotatrajetoidvolta" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField runat="server" ID="hdnRotaId" />
    <br />
    <br />
    <div class="divEditBlock" style="width: 740px;">
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Desvincular Aluno da Rota" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="true">
        <dxtc:ASPxPageControl ID="pcRota" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="true">
            <TabPages>
                <dxtc:TabPage Name="tbAlunosIda" Text="Ida">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDados" runat="server">
                            <asp:Panel ID="pnlDados" runat="server" Visible="false">
                                <asp:ObjectDataSource ID="odsRotaAlunoIda" runat="server" TypeName="Techne.Lyceum.Net.Transporte.DesvinculaAlunoRota"
                                    SelectMethod="ListarRotaAlunoIda" DeleteMethod="DeleteIda">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnRotaId" DefaultValue="" Name="rotaTrajeto" PropertyName="Value" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdRotaAlunoIda" runat="server" DataSourceID="odsRotaAlunoIda"
                                    KeyFieldName="ROTAALUNOID" AutoGenerateColumns="false" ClientInstanceName="grdRotaAlunoIda"
                                    OnInitNewRow="grdRotaAlunoIda_InitNewRow" OnStartRowEditing="grdRotaAlunoIda_StartRowEditing"
                                    OnRowDeleting="grdRotaAlunoIda_RowDeleting" Width="700px" OnAfterPerformCallback="grdRotaAlunoIda_AfterPerformCallback">
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsEditing Mode="InLine" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ROTAALUNOID" Name="ROTAALUNOID" VisibleIndex="1"
                                            FieldName="ROTAALUNOID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ROTATRAJETOID" Name="ROTATRAJETOID" VisibleIndex="1"
                                            FieldName="ROTATRAJETOID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PESSOA" Name="PESSOA" VisibleIndex="1" FieldName="PESSOA"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Aluno" Name="ALUNO" ReadOnly="true" VisibleIndex="2"
                                            FieldName="ALUNO" Width="150px">
                                            <PropertiesTextEdit>
                                                <ReadOnlyStyle>
                                                    <Border BorderStyle="None"></Border>
                                                </ReadOnlyStyle>
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" ReadOnly="true" VisibleIndex="3"
                                            FieldName="NOME" Width="200px">
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
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="tbAlunosVolta" Text="Volta">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:ObjectDataSource ID="odsRotaAlunoVolta" runat="server" TypeName="Techne.Lyceum.Net.Transporte.DesvinculaAlunoRota"
                                    SelectMethod="ListarRotaAlunoVolta" DeleteMethod="DeleteVolta">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnRotaId" DefaultValue="" Name="rotaTrajeto" PropertyName="Value" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdRotaAlunoVolta" runat="server" DataSourceID="odsRotaAlunoVolta"
                                    KeyFieldName="ROTAALUNOID" AutoGenerateColumns="false" ClientInstanceName="grdRotaAlunoVolta"
                                    OnInitNewRow="grdRotaAlunoVolta_InitNewRow" OnStartRowEditing="grdRotaAlunoVolta_StartRowEditing"
                                    OnRowDeleting="grdRotaAlunoVolta_RowDeleting" Width="700px" OnAfterPerformCallback="grdRotaAlunoVolta_AfterPerformCallback">
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsEditing Mode="InLine" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ROTAALUNOID" Name="ROTAALUNOID" VisibleIndex="1"
                                            FieldName="ROTAALUNOID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ROTATRAJETOID" Name="ROTATRAJETOID" VisibleIndex="1"
                                            FieldName="ROTATRAJETOID" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PESSOA" Name="PESSOA" VisibleIndex="1" FieldName="PESSOA"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Aluno" Name="ALUNO" ReadOnly="true" VisibleIndex="2"
                                            FieldName="ALUNO" Width="150px">
                                            <PropertiesTextEdit>
                                                <ReadOnlyStyle>
                                                    <Border BorderStyle="None"></Border>
                                                </ReadOnlyStyle>
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" ReadOnly="true" VisibleIndex="3"
                                            FieldName="NOME" Width="200px">
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
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
