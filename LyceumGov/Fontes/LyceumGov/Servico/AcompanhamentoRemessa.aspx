<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoRemessa.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.AcompanhamentoRemessa" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlFiltro" runat="server" GroupingText="Filtros" Width="720px">
                <table>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno: "></asp:Label>
                        </td>
                        <td colspan="4">
                            <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoRemessaCartao" 
                            AutoPostBack="true" OnTextChanged="tseAluno_Changed" >
                            </tweb:TSearch>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                MaxLength="20" Columns="10" Caption="" OnChanged="tseRegional_Changed" Key="id_regional"
                                SqlSelect="SELECT id_regional, regional FROM TCE_REGIONAL" DataType="Number">
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
                        <td colspan="4">
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
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server"></asp:Label>
                        </td>
                        <td colspan="4">
                            <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,id_regional,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                                GridWidth="850px" MaxLength="20" FieldName="Unidade de Ensino" SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# "
                                ArgumentColumns="60" Columns="10" OnChanged="tseUnidadeEnsino_Changed" SqlOrder="nome_comp">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />									
                                    <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Visible="false" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label10" runat="server" Text="Data da Leitura:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td style="width: 50px; text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label1" runat="server" Text=" Inicial:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtEnvioInicial" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" Width="140px" OnValueChanged="dtEnvioInicial_ValueChanged"
                                            AutoPostBack="true">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="width: 50px; text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label2" runat="server" Text=" Final:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtEnvioFinal" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" Width="140px" OnValueChanged="dtEnvioFinal_ValueChanged"
                                            AutoPostBack="true">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblfCritica" runat="server" Text="Possui Crítica?:"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblPossuiCriticas" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblPossuiCriticas_IndexChanged">
                                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Não" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Em Processamento" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Todos" Value="0" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td colspan="3" align="right">
                            <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Pesquisar" ImageUrl="~/Images/bot_buscar.png"
                                OnClick="btnPesquisar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnlMensagem" runat="server">
        <ContentTemplate>
            <br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <dxwgv:ASPxGridView ClientInstanceName="grdProcessamentoRemessa" ID="grdProcessamentoRemessa"
                runat="server" AutoGenerateColumns="False" KeyFieldName="RemessaId" Width="100%"
                EnableCallBacks="false" OnPageIndexChanged="grdProcessamentoRemessa_PageIndexChanged" Visible="false">
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsCookies Enabled="false" />
                <SettingsText EmptyDataRow="Não existem dados." />                                
                <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle"/>
                <Columns>           
                    <dxwgv:GridViewDataTextColumn Caption="Detalhes" Name="btnDetalhes" VisibleIndex="0" Width="90px">
                        <DataItemTemplate>
                            <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("RemessaId") %>'
                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/bt_busca.png" Height="15px" AlternateText="Visualizar Detalhes da Remessa">
                            </asp:ImageButton>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Remessa" FieldName="RemessaId" VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Solicitação" FieldName="CodSolicitacao" VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>                    
                    <dxwgv:GridViewDataTextColumn Caption="Operadora" FieldName="NomeOperadora" VisibleIndex="3">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="Aluno" VisibleIndex="4" Width="10%">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome do Aluno" FieldName="Nome_Compl" VisibleIndex="5">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="Unidade_Ens" VisibleIndex="6">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Data Disponibilização" FieldName="DataInclusaoRemessa" VisibleIndex="7">
                    </dxwgv:GridViewDataTextColumn>                    
                    <dxwgv:GridViewDataTextColumn Caption="Data da Leitura pela Operadora" FieldName="DataEnvio" VisibleIndex="8">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Situação do Retorno" FieldName="SituacaoProcessamento" VisibleIndex="9">
                        <DataItemTemplate>
                            <%# Eval("SituacaoProcessamento") == null ? String.Empty : Techne.Lyceum.Net.Util.Utils.GetEnumDescription((Techne.Lyceum.RN.CartaoEstudante.Enum.SituacaoProcessamentoEnum)Convert.ToInt32(Eval("SituacaoProcessamento")))%>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>                    
                    <dxwgv:GridViewDataTextColumn Caption="Data do Último Retorno" FieldName="DataInclusaoRetorno" VisibleIndex="10">
                    </dxwgv:GridViewDataTextColumn>                                        
                </Columns>
            </dxwgv:ASPxGridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPesquisar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="tseAluno" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="tseRegional" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="tseMunicipio" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="tseUnidadeEnsino" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="rblPossuiCriticas" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="dtEnvioInicial" EventName="ValueChanged" />
            <asp:AsyncPostBackTrigger ControlID="dtEnvioFinal" EventName="ValueChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <dxpc:ASPxPopupControl ID="ppcDetalhesRemessa" ClientInstanceName="ppcDetalhesRemessa"
                runat="server" Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false"
                ShowCloseButton="true" ShowFooter="false" ShowHeader="true" HeaderText="Detalhes"
                ShowSizeGrip="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                EnableAnimation="true" AutoUpdatePosition="true" Width="960px" Height="480px">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ClientSideEvents Init="OnInitASPxPopupControl" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="ppDetalhesRemessa" runat="server">
                        <div class="scrollLargePopUp">
                            <contentcollection>              
                                <dxtc:ASPxPageControl ID="pcProcessamentoRemessa" runat="server" ActiveTabIndex="0" Height="470px" Width="935px" ClientInstanceName="pcProcessamentoRemessa">
                                    <TabPages>
                                        <dxtc:TabPage Text="Dados da Remessa">
                                            <ContentCollection> 
                                            <dxw:ContentControl ID="ccDadosEnvio" runat="server">               
                                                <asp:Panel ID="pntabDadosRemessa" runat="server" Visible="true">
                                                    <asp:Panel ID="pnRemessa" GroupingText="Remessa" runat="server" Visible="true">
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCodigoRemessa" runat="server" Text="Remessa" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCodigoRemessa" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLoteRemessa" runat="server" Text="Lote" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLote" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>                                                                  
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSolicitacao" runat="server" Text="Solicitação" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSolicitacao" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>                                                            
                                                            </tr>
                                                            <tr>     
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblOperadora" runat="server" Text="Operadora" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOperadora" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>                                                                                                                 
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataInclusao" runat="server" Text="Data Disponibilização" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDataInclusaoRemessa" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>                                                             
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataEnvio" runat="server" Text="Data da Leitura pela Operadora" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDataEnvioLogRemessa" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                </td>                                                                                                                              
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>                                                
                                                    <asp:Panel ID="pnAluno" GroupingText="Dados do Aluno" runat="server">
                                                        <asp:Panel ID="pnInfoPessoais" GroupingText="Informações Pessoais" runat="server">                                                    
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <dxe:ASPxBinaryImage ID="bimgFotoPessoa" runat="server" AlternateText="sem foto"
                                                                                        ClientInstanceName="bimgFotoPessoa" Height="150px" StoreContentBytesInViewState="True"
                                                                                        Width="150px">
                                                                                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                                                                                    </dxe:ASPxBinaryImage>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblMatricula" runat="server" Width="100px" Text="Matrícula: "></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtMatricula" runat="server" MaxLength="100" ReadOnly="true" Width="150px" />
                                                                                </td>                                                                            
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblNome" runat="server" Text="Nome: "></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNomeAluno" runat="server" MaxLength="100" ReadOnly="true" Width="250px" />
                                                                                </td>                                                                               
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCPF" runat="server" MaxLength="50" ReadOnly="true" Width="150px" />
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    &nbsp&nbsp&nbsp<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataUltimaAtualizacao" runat="server" Text="Data da última atualização" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDataUltimaAtualizacao" runat="server" ReadOnly="true" MaxLength="100"/>
                                                                                </td>                                                                                  
                                                                            </tr>                                                                            
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label ID="lblDtNasc" runat="server" Text="Data de Nascimento: " />
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtDataNascimento" runat="server" ReadOnly="true" MaxLength="20"
                                                                                        Width="160px" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Nome da Mãe:" />
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtNomeMae" runat="server" Width="250px" MaxLength="100" ReadOnly="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label6" runat="server" Text="Nome do Pai:" />
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtNomePai" runat="server" Width="250px" MaxLength="100" ReadOnly="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right; width: 50px;">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                                                                                        Text="E-mail Interno:"></asp:Label>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtEmailInterno" ReadOnly="true" Style="text-transform: lowercase;"
                                                                                        runat="server" Width="400px" MaxLength="100" />
                                                                                </td>
                                                                            </tr>                                                                             
                                                                        </table>
                                                                    </td>
                                                                </tr>                                                               
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnDocumento" GroupingText="Documentos" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="lblRGNum" runat="server" Text="RG:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRGNum" runat="server" ReadOnly="true" MaxLength="20" Width="160px" />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRGUF" runat="server" Text="Estado:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRGUF" runat="server" ReadOnly="true" MaxLength="20" Width="160px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: right">
                                                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRGEmissor" runat="server"
                                                                            Text="Órgão Emissor:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRGEmissor" runat="server" ReadOnly="true" MaxLength="20" Width="160px" />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRGDataEmissao" runat="server"
                                                                            Text="Data de Expedição:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRGDataEmissao" runat="server" ReadOnly="true" MaxLength="20"
                                                                            Width="160px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlLoginOperadora" GroupingText="Dados da operadora de cartões" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: right">
                                                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLogin" runat="server"
                                                                            Text="Login" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLoginCartao" runat="server" Enabled="false" ReadOnly="true" Width="400px"
                                                                            MaxLength="100" Style="text-transform: lowercase" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>                                                                                                                
                                                        <asp:Panel ID="pnEndereco" GroupingText="Endereço" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLogradouro" runat="server" Text="Endereço:" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLogradouro" runat="server" ReadOnly="true" MaxLength="50" Columns="50" Width="400px"/>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnderecoNumero" runat="server" Text="N.º:" />
                                                                                </td> 
                                                                                <td>
                                                                                    <asp:TextBox ID="txtEnderecoNumero" runat="server" MaxLength="15" Width="30px" ReadOnly="true" />
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblComplemento" runat="server"
                                                                                        Text="Complemento:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtComplemento" runat="server" MaxLength="150" Width="150px" ReadOnly="true" />
                                                                                </td>                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="6">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="text-align: right">
                                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server" Text="Bairro:" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" ReadOnly="true" Width="100px" />
                                                                                            </td>
                                                                                            <td style="text-align: right">
                                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndMunicipio" runat="server" Text="Município:" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMunicipio" runat="server" ReadOnly="true" MaxLength="20" Width="100px" />
                                                                                            </td>
                                                                                            <td style="text-align: right">
                                                                                                <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtEstado" runat="server" Width="15px" Readonly="true" />
                                                                                            </td>
                                                                                            <td style="text-align: right">
                                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCep" runat="server" ReadOnly="true" Width="50px" MaxLength="8" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pntabDadosEscolares" runat="server">
                                                                <asp:Panel ID="pnDadosEscolares" GroupingText="Dados Escolares" runat="server">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="text-align: right">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label3" runat="server" Text="Unidade de Ensino:" ></asp:Label>
                                                                            </td>
                                                                            <td style="width: auto">
						                                                        <asp:TextBox ID="txtUnidadeEnsino" runat="server" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTurno" runat="server" Text="Turno:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtTurno" runat="server" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerie" runat="server" Text="Série:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtSerie" runat="server" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTurma" runat="server" Text="Turma:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtTurma" runat="server" ReadOnly="true" MaxLength="50" />
                                                                            </td>
                                                                        </tr>				
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnTransporte" GroupingText="Transporte" runat="server" Visible="true">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="text-align: right; width:5%">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblGratuidade" runat="server" Text="Gratuidade:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtGratuidade" runat="server" Width="10px" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right; width:5%">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTrem" runat="server" Text="Trem:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtTrem" runat="server" Width="10px" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right; width:5%">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblOnibus" runat="server" Text="Ônibus:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtOnibus" runat="server" Width="10px" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right; width:5%">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMetro" runat="server" Text="Metrô:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtMetro" runat="server" Width="10px" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                            <td style="text-align: right; width:5%">
                                                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBarcas" runat="server" Text="Barcas:"></asp:Label>
                                                                            </td>
                                                                            <td>
						                                                        <asp:TextBox ID="txtBarcas" runat="server" Width="10px" ReadOnly="true" MaxLength="8" />
                                                                            </td>
                                                                        </tr>				
                                                                    </table>
                                                                </asp:Panel>
                                                        </asp:Panel>                                                        
                                                    </asp:Panel>
                                                </asp:Panel>                                  
                                            </dxw:ContentControl>
                                            </ContentCollection>
                                        </dxtc:TabPage>
                                        <dxtc:TabPage Text="Dados de Retorno">
                                            <ContentCollection> 
                                            <dxw:ContentControl ID="ccDadosRetorno" runat="server">               
                                                <asp:Panel ID="pnDadosRetorno" runat="server" GroupingText="Retorno" Visible="true">
                                                    <asp:Panel ID="pnRetorno" GroupingText="Dados do último retorno" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBeneficiario" runat="server" Text="Beneficiário:" />
                                                                </td>
                                                                <td>
						                                            <asp:TextBox ID="txtBeneficiario" runat="server" Width="50px" ReadOnly="true" MaxLength="8" />
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSituacaoProcessamento" runat="server" Text="Situação de Processamento:" />
                                                                </td>
                                                                <td>
						                                            <asp:TextBox ID="txtSituacaoProcessamento" runat="server"  Width="200px" ReadOnly="true" MaxLength="8" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataInclusaoRetorno" runat="server" Text="Data do Último Retorno" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDataInclusaoRetorno" Width="80px" runat="server" ReadOnly="true" MaxLength="30"/>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataProcessamentoRetorno" runat="server" Text="Data de Processamento da Operadora" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDataProcessamentoRetorno" Width="80px" runat="server" ReadOnly="true" MaxLength="30"/>
                                                                </td>                                                             
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnTabDadosRetornoCritica" GroupingText="Críticas" runat="server">
                                                        <br />
                                                        <asp:Label ID="lblMensagemGridRetornoCritica" runat="server" SkinID="lblMensagem"></asp:Label>
                                                        <br />
                                                        <dxwgv:ASPxGridView ClientInstanceName="grdRetornoCritica" ID="grdRetornoCritica"
                                                            runat="server" AutoGenerateColumns="False" KeyFieldName="RetornoCriticaId" Width="100%"
                                                            EnableCallBacks="false" Visible="True">
                                                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                                                            <SettingsText EmptyDataRow="Não existem dados." />
                                                            <Columns>                          
                                                                <dxwgv:GridViewDataTextColumn Caption="Código da Crítica" FieldName="CodigoCritica" VisibleIndex="2">
                                                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                                    </CellStyle>
                                                                </dxwgv:GridViewDataTextColumn>                                                                
                                                                <dxwgv:GridViewDataTextColumn Caption="Descrição da Critica" FieldName="Descricao" VisibleIndex="3">
                                                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                                    </CellStyle>
                                                                </dxwgv:GridViewDataTextColumn>
                                                                <dxwgv:GridViewDataTextColumn Caption="Data do Último Retorno" FieldName="DataInclusao" VisibleIndex="4">
                                                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                                                    </CellStyle>
                                                                </dxwgv:GridViewDataTextColumn>                                                                
                                                            </Columns>
                                                        </dxwgv:ASPxGridView>
                                                    </asp:Panel>                                                    
                                                    <br />
                                                </asp:Panel>                                  
                                            </dxw:ContentControl>
                                            </ContentCollection>
                                        </dxtc:TabPage>                             
                                    </TabPages>
                                </dxtc:ASPxPageControl>
                            </contentcollection>
                        </div>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
        <%-- <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDetalhes" EventName="Click" />
        </Triggers>--%>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Carregando..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
