<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConfirmacaoCandidato.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ConfirmacaoCandidato" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

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
                <td>
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
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbAno" runat="server" DataTextField="ano" DataValueField="ano"
                        onchange="Bloqueio()" Width="70px" AutoPostBack="True" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="cmbAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        Width="70px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Text="Fase:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlFase" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                        Height="16px">
                        <asp:ListItem Selected="True" Text="Selecione" Value=""></asp:ListItem>
                        <asp:ListItem Text="1ª Fase" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2ª Fase" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" style="font-weight: bold" class="style2">
                    * Todos os campos são obrigátorios para a realização da busca
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                        ValidationGroup="SalvarForm" Style="margin-left: 0px" Width="100px" OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server" GroupingText="Lista de Candidatos" Visible="false">
                    <dxwgv:ASPxGridView ID="grdControle" runat="server" AutoGenerateColumns="False" EnableCallBacks="false"
                        ClientInstanceName="grdControle" DataSourceID="odsControle" KeyFieldName="OPCAOINSCRICAOID"
                        OnCustomButtonCallback="grdControle_CustomButtonCallback">
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
                            <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Inscriçao" FieldName="INSCRICAOALUNOID" VisibleIndex="6"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Número Inscrição" FieldName="NUMEROINSCRICAO"
                               CellStyle-HorizontalAlign= "Center" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome do Candidato" FieldName="NOME" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome da Mãe" FieldName="NOMEMAE" VisibleIndex="8">
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
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" CellStyle-HorizontalAlign= "Center" VisibleIndex="10">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" CellStyle-HorizontalAlign= "Center" VisibleIndex="11">
                            </dxwgv:GridViewDataTextColumn>                            
                            <dxwgv:GridViewDataTextColumn Caption="Nome Curso" FieldName="NOMECURSO"  VisibleIndex="11">
                            </dxwgv:GridViewDataTextColumn>                            
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" CellStyle-HorizontalAlign= "Center" VisibleIndex="12">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Convocação" Name="DATACONVOCACAO"
                                FieldName="DATACONVOCACAO" Width="100px" ReadOnly="true">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                    <ValidationSettings>
                                        <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                    </ValidationSettings>
                                </PropertiesDateEdit>
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Prazo Resposta" FieldName="PRAZORESPOSTA"
                                VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Dados Candidato" Name="btnVisualizarDados"
                                VisibleIndex="16" Width="35px">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="btnVisualizarDados" runat="server" EnableViewState="false" CommandArgument='<%# Eval("INSCRICAOALUNOID") %>'
                                        OnClick="hplVisualizarDados_Click" Text="Cadastro"></asp:LinkButton>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PRECADASTROALUNOID" FieldName="PRECADASTROALUNOID"
                                VisibleIndex="15" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CONTROLEVAGAID" FieldName="CONTROLEVAGAID"
                                VisibleIndex="15" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PESSOAID" FieldName="PESSOAID" VisibleIndex="15"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CENSO" FieldName="CENSO" VisibleIndex="15"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="15" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsControle" TypeName="Techne.Lyceum.Net.Matricula.ConfirmacaoCandidato"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="cmbAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlFase" DefaultValue="" Name="fase" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
