<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MaeAnalise.aspx.cs" Inherits="Techne.Lyceum.Net.Cadastros.MaeAnalise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para consulta:"
        Width="623px">
        <asp:HiddenField runat="server" ID="hdnIdControle" />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="2">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                        MaxLength="10">
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
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server" GroupingText="Lista Responsáveis" Visible="false">
                    <br />
                    <dxwgv:ASPxGridView ID="grdAnalise" runat="server" AutoGenerateColumns="False" EnableCallBacks="false"
                        ClientInstanceName="grdAnalise" DataSourceID="odsAnalise" KeyFieldName="MAE_INSCRICAOID"
                        OnAfterPerformCallback="grdAnalise_AfterPerformCallback" OnCustomButtonCallback="grdAnalise_CustomButtonCallback"
                        OnCustomButtonInitialize="grdAnalise_CustomButtonInitialize">
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton Text="Confirmar" ID="btnConfirmar" Visibility="AllDataRows">
                                        <Image Url="../Images/bot_confirmar.png" Height="16px" AlternateText="Confirmar">
                                        </Image>
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="MAE_INSCRICAOID" FieldName="MAE_INSCRICAOID"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="6" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn VisibleIndex="9" Caption="Data de Nascimento" Name="DATANASCIMENTO"
                                FieldName="DATANASCIMENTO" Width="100px" ReadOnly="true">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                    <ValidationSettings>
                                        <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                    </ValidationSettings>
                                </PropertiesDateEdit>
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="E-mail" Name="EMAIL" FieldName="EMAIL"
                                Width="100px" ReadOnly="true">
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Telefone Alternativo" FieldName="FIXOCELULAR"
                                VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Experiência de Trabalho" FieldName="EXPERIENCIATRABALHO"
                                VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Habilitado" FieldName="HABILITADO" VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="MOTIVO" VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Dados Inscrição" Name="btnVisualizarDados"
                                VisibleIndex="16" Width="35px">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="btnVisualizarDados" runat="server" EnableViewState="false" CommandArgument='<%# Eval("CPF") %>'
                                        OnClick="hplVisualizarDados_Click" Text="Cadastro"></asp:LinkButton>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsAnalise" TypeName="Techne.Lyceum.Net.Cadastros.MaeAnalise"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
