<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Rota.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Rota" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsPrimeiroCEPIda.ClientID %>',
                cep: '<%=txtPrimeiroCepIda.ClientID %>',
                nomeLogradouro: '<%=txtPrimeiroEnderecoIda.ClientID %>',
                nomeMunicipio: '<%=txtPrimeiroMunicipioIda.ClientID %>',
                codigoMunicipio: '<%=hdnPrimeiroCodMunicipioIda.ClientID %>',
                uf: '<%=txtPrimeiroEstadoIda.ClientID %>'
            });

            preencherDadosPorCEP({ tscep: '<%=tsPrimeiroCEPVolta.ClientID %>',
                cep: '<%=txtPrimeiroCepVolta.ClientID %>',
                nomeLogradouro: '<%=txtPrimeiroEnderecoVolta.ClientID %>',
                nomeMunicipio: '<%=txtPrimeiroMunicipioVolta.ClientID %>',
                codigoMunicipio: '<%=hdnPrimeiroCodMunicipioVolta.ClientID %>',
                uf: '<%=txtPrimeiroEstadoVolta.ClientID %>'
            });

            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });      
    </script>

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
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,u.municipio,u.id_regional,ua_atual,ua_antiga, r.regional  from VW_UNIDADE_ENSINO_SITUACAO u left join TCE_REGIONAL r on u.ID_REGIONAL = r.ID_REGIONAL "
                        SqlWhere=" situacao = 'ESTADUAL' ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="30%" />
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
                        Columns="10" MaxLength="11" Key="codigo" DataType="VarChar" SqlWhere = " CENSO = #tseUnidadeFiltro# " >
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
    <br />
    <br />
    <div class="divEditBlock" style="width: 740px;">
        <asp:ImageButton ID="btnDesabilitar" runat="server" SkinID="BcDesabilitar" OnClick="btnDesabilitar_Click"
            ImageUrl="~/Images/bot_desabil.png" OnClientClick="return confirm('Confirma a desabilitação?');" />
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnIncluir" runat="server" SkinID="BcSalvar" OnClick="btnIncluir_Click"
            OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Rota" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="true">
        <dxtc:ASPxPageControl ID="pcRota" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="true">
            <TabPages>
                <dxtc:TabPage Name="DadosGerais" Text="Dados Gerais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDados" runat="server">
                            <asp:Panel ID="pnlDados" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: right; width: 5%">
                                            <asp:Label ID="lblRegional" runat="server" Text="Regional:"></asp:Label>
                                        </td>
                                        <td colspan="5">
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
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMuncipio" runat="server" Text="Município:"></asp:Label>
                                        </td>
                                        <td colspan="5">
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
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblEscola" runat="server" Text="Escola:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                                OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                                                SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, rf.descricao as regiaofinanceira,ua_atual,ua_antiga from vw_unidade_ensino_situacao ue left join gestaorede.regiaofinanceiramunicipio rm on ue.municipio = rm.municipioid left join gestaorede.regiaofinanceira rf on rm.regiaofinanceiraid = rf.regiaofinanceiraid ">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="8%" />
                                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="25%" />
                                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />                                                    
                                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="25%" />
                                                    <tweb:TSearchBoxColumn Caption="Regiao Geográfica" FieldName="regiaofinanceira" Width="25%" />
                                                    <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="9%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label2" runat="server" Text="CNPJ:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCnpj" runat="server"></asp:Label>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label1" runat="server" Text="Região Geográfica:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRegiaoFinanceira" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label3" runat="server" Text="Código:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCodigo" runat="server" Width="100px"></asp:Label>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label4" runat="server" Text="Situação:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSituacao" runat="server"></asp:Label>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label15" runat="server" Text="Ativo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkAtivo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTipoPagamento" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoCalculoPagamento" runat="server" DataTextField="DESCRICAO"
                                                DataValueField="TIPOCALCULOPAGAMENTOID" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTurno" runat="server" Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                                                AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                </table>
                                <br />
                                <asp:Panel ID="pnlIda" runat="server" GroupingText="Trajeto de Ida" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTipoContratacaoIda" runat="server" SkinID="lblObrigatorio" Text="Contratação:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoContratacaoIda" runat="server" DataTextField="DESCRICAO"
                                                    DataValueField="TIPOCONTRATACAOID" Width="200px" AppendDataBoundItems="true"
                                                    OnSelectedIndexChanged="ddlTipoContratacaoIda_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblValorRotaIda" runat="server" SkinID="lblObrigatorio" Text="Valor*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtValorRotaIda" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblQuantidadeKmIda" runat="server" SkinID="lblObrigatorio" Text="Quantidade Km:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantidadeKmIda" runat="server" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label26" runat="server" Text="Tempo(horas):"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTempoIda" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlPrestadorIda" runat="server" Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblPrestadorIda" runat="server" SkinID="lblObrigatorio" Text="Prestador:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tsePrestadorIda" runat="server" Argument="nome" ArgumentColumns="50"
                                                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="true" SqlWhere=" ativo = 1"
                                                        Key="prestadorid" SqlSelect="select distinct prestadorid,cnpj, cpf, nome, telefone from Transporte.prestador"
                                                        SqlOrder="nome">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="prestadorid" Width="10%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                                            <tweb:TSearchBoxColumn Caption="cnpj" FieldName="cnpj" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="cpf" FieldName="cpf" Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblCondutorIda" runat="server" SkinID="lblObrigatorio" Text="Condutor:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseCondutorIda" runat="server" SqlOrder="nome" SqlSelect="select distinct pc.condutorid,cpf,nome,numerocnh,pc.prestadorid from Transporte.prestadorcondutor pc inner join Transporte.condutor c on c.condutorid = pc.condutorid "
                                                        SqlWhere=" c.ativo = 1 and pc.prestadorid = #tsePrestadorIda# " GridWidth="600px"
                                                        ArgumentColumns="50" Argument="nome" Columns="10" MaxLength="11" Key="cpf" DataType="VarChar">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="condutorid" Width="10%" />
                                                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                                            <tweb:TSearchBoxColumn Caption="Número CNH" FieldName="numerocnh" Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblVeiculoIda" runat="server" SkinID="lblObrigatorio" Text="Veiculo:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseVeiculoIda" runat="server" Argument="nome" ArgumentColumns="50"
                                                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlWhere=" v.ativo = 1 and a.prestadorid = #tsePrestadorIda# and c.cpf = #tseCondutorIda# "
                                                        Key="placa" SqlSelect="select distinct a.veiculoid,placa,v.nome,anolicenciamento from Transporte.prestadorcondutorveiculo a inner join Transporte.VEICULO v on a.VEICULOID = v.VEICULOID inner join Transporte.CONDUTOR c on a.CONDUTORID = c.CONDUTORID"
                                                        SqlOrder="nome" DataType="VarChar">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="veiculoid" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Placa" FieldName="placa" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
                                                            <tweb:TSearchBoxColumn Caption="Ano do Licenciamento" FieldName="anolicenciamento"
                                                                Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPrimeiroEmbarqueIda" runat="server" GroupingText="Primeiro ponto de Embarque"
                                        Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPrimeiroCepIda" runat="server" SkinID="numerico" MaxLength="8"
                                                        AutoPostBack="false" />
                                                    <tweb:TSearch ID="tsPrimeiroCEPIda" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                                                        Modal="true" SkinID="CEP" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <input id="txtPrimeiroEstadoIda" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                                        Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hdnPrimeiroCodMunicipioIda" />
                                                    <asp:TextBox ID="txtPrimeiroMunicipioIda" runat="server" MaxLength="20" Width="300px"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                                        Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPrimeiroEnderecoIda" runat="server" MaxLength="50" Columns="50"
                                                        onkeypress="return endereco(event);" Width="550px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                                        Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroEndNumIda" runat="server" MaxLength="15" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                                        Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroBairroIda" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                                        Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label7" runat="server" Text="Latitude:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiraLatitudeIda" runat="server" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label8" runat="server" Text="Longitude:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroLongitudeIda" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlVolta" runat="server" GroupingText="Trajeto de Volta" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTipoContratacaoVolta" runat="server" SkinID="lblObrigatorio" Text="Contratação:*"></asp:Label>
                                                <td>
                                                    <asp:DropDownList ID="ddlTipoContratacaoVolta" runat="server" DataTextField="DESCRICAO"
                                                        DataValueField="TIPOCONTRATACAOID" Width="200px" AppendDataBoundItems="true"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTipoContratacaoVolta_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblValorRotaVolta" runat="server" SkinID="lblObrigatorio" Text="Valor:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtValorRotaVolta" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblQuantidadeKmVolta" runat="server" SkinID="lblObrigatorio" Text="Quantidade Km:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantidadeKmVolta" runat="server" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label27" runat="server" Text="Tempo(horas):"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTempoVolta" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlPrestadorVolta" runat="server" Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblPrestadorVolta" runat="server" SkinID="lblObrigatorio" Text="Prestador:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tsePrestadorVolta" runat="server" Argument="nome" ArgumentColumns="50"
                                                        DataType="Number" MaxLength="20" Columns="10" SqlWhere=" ativo = 1" Key="prestadorid"
                                                        SqlSelect="select distinct prestadorid,cnpj, cpf, nome, telefone from Transporte.prestador"
                                                        SqlOrder="nome">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="prestadorid" Width="10%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                                            <tweb:TSearchBoxColumn Caption="cnpj" FieldName="cnpj" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="cpf" FieldName="cpf" Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblCondutorVolta" runat="server" SkinID="lblObrigatorio" Text="Condutor:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseCondutorVolta" runat="server" SqlOrder="nome" SqlSelect="select distinct pc.condutorid,cpf,nome,numerocnh,pc.prestadorid from Transporte.prestadorcondutor pc inner join Transporte.condutor c on c.condutorid = pc.condutorid "
                                                        SqlWhere=" c.ativo = 1 and pc.prestadorid = #tsePrestadorVolta# " GridWidth="600px"
                                                        ArgumentColumns="50" Argument="nome" Columns="10" MaxLength="11" Key="cpf" DataType="VarChar">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="condutorid" Width="10%" />
                                                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                                            <tweb:TSearchBoxColumn Caption="Número CNH" FieldName="numerocnh" Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblVeiculoVolta" runat="server" SkinID="lblObrigatorio" Text="Veiculo:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseVeiculoVolta" runat="server" Argument="nome" ArgumentColumns="50"
                                                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlWhere=" v.ativo = 1 and a.prestadorid = #tsePrestadorVolta# and c.cpf = #tseCondutorVolta# "
                                                        Key="placa" SqlSelect=" select distinct a.veiculoid,placa,v.nome,anolicenciamento from Transporte.prestadorcondutorveiculo a inner join Transporte.VEICULO v on a.VEICULOID = v.VEICULOID inner join Transporte.CONDUTOR c on a.CONDUTORID = c.CONDUTORID "
                                                        SqlOrder="nome" DataType="VarChar">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="veiculoid" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Placa" FieldName="placa" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
                                                            <tweb:TSearchBoxColumn Caption="Ano do Licenciamento" FieldName="anolicenciamento"
                                                                Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPrimeiroEmbarqueVolta" runat="server" GroupingText="Primeiro ponto de Embarque"
                                        Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="CEP:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPrimeiroCepVolta" runat="server" SkinID="numerico" MaxLength="8"
                                                        AutoPostBack="false" />
                                                    <tweb:TSearch ID="tsPrimeiroCEPVolta" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                                                        Modal="true" SkinID="CEP" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label6" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <input id="txtPrimeiroEstadoVolta" runat="server" maxlength="20" class="txtInput"
                                                        readonly="readonly" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label9" runat="server" Text="Município:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hdnPrimeiroCodMunicipioVolta" />
                                                    <asp:TextBox ID="txtPrimeiroMunicipioVolta" runat="server" MaxLength="20" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label10" runat="server" Text="Endereço:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPrimeiroEnderecoVolta" runat="server" MaxLength="50" Columns="50"
                                                        onkeypress="return endereco(event);" Width="550px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label11" runat="server" Text="N.º:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroEndNumVolta" runat="server" MaxLength="15" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label12" runat="server" Text="Bairro:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroBairroVolta" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                                        Width="300px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label13" runat="server" Text="Latitude:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiraLatitudeVolta" runat="server" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label14" runat="server" Text="Longitude:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrimeiroLongitudeVolta" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="PontosEmbarque" Text="Pontos de Embarque">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccPontoEmbarque" runat="server">
                            <div class="divEditBlock" style="width: 740px;">
                                <asp:ImageButton ID="btnNovoEmbarque" runat="server" SkinID="BcNovo" OnClick="btnNovoEmbarque_Click" />
                                <asp:ImageButton ID="btnCancelaEmbarque" runat="server" SkinID="BcCancelar" OnClick="btnCancelaEmbarque_Click" />
                                <asp:ImageButton ID="btnIncluirEmbarque" runat="server" SkinID="BcSalvar" OnClick="btnIncluirEmbarque_Click"
                                    OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
                                <asp:Label runat="server" ID="Label24" Text="Embarque" SkinID="BcTitulo" />
                            </div>
                            <asp:HiddenField runat="server" ID="hdnRotaTrajetoIdIda" />
                            <asp:HiddenField runat="server" ID="hdnRotaTrajetoIdVolta" />
                            <asp:HiddenField runat="server" ID="hdnPontoEmbarque" />
                            <asp:HiddenField runat="server" ID="hdnRotaTrajetoId" />
                            <asp:Panel ID="pnlNovoEmbarque" runat="server" GroupingText="Escolha o Trajeto" Width="100%">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label35" runat="server" Text="Tipo:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblTrajetoEmbarque" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="rblTrajetoEmbarque_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Ida" Value="Ida"></asp:ListItem>
                                                <asp:ListItem Text="Volta" Value="Volta"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlDadosEmbarque" runat="server" GroupingText="Pontos de Embarque"
                                    Width="100%" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label16" runat="server" Text="CEP:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false" />
                                                <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                                                    Modal="true" SkinID="CEP" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label17" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label18" runat="server" Text="Município:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                                                <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="300px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label19" runat="server" Text="Endereço:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                                    Width="550px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label20" runat="server" Text="N.º:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label21" runat="server" Text="Bairro:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                                    Width="300px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label22" runat="server" Text="Latitude:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLatitude" runat="server" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label23" runat="server" Text="Longitude:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLongitude" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label25" runat="server" Text="Primeiro:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPrimeiro" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:ObjectDataSource ID="odsPontoEmbarqueIda" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Rota"
                                SelectMethod="ListarPontoEmbarqueIda" DeleteMethod="DeleteEmbarqueIda" UpdateMethod="UpdateEmbarqueIda">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnRotaTrajetoIdIda" Name="rotaTrajetoIdIda" PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdPontoEmbarqueIda" runat="server" DataSourceID="odsPontoEmbarqueIda"
                                KeyFieldName="PONTOEMBARQUEID" AutoGenerateColumns="false" ClientInstanceName="grdPontoEmbarqueIda"
                                OnInitNewRow="grdPontoEmbarqueIda_InitNewRow" OnStartRowEditing="grdPontoEmbarqueIda_StartRowEditing"
                                OnCustomButtonCallback="grdPontoEmbarqueIda_CustomButtonCallback" EnableCallBacks="False"
                                OnRowDeleting="grdPontoEmbarqueIda_RowDeleting" Width="700px" OnAfterPerformCallback="grdPontoEmbarqueIda_AfterPerformCallback">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="InLine" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <CustomButtons>
                                            <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="EditarIda" Visibility="AllDataRows"
                                                Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                            </dxwgv:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PONTOEMBARQUEID" Name="PONTOEMBARQUEID" VisibleIndex="1"
                                        FieldName="PONTOEMBARQUEID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ROTATRAJETOID" Name="ROTATRAJETOID" VisibleIndex="1"
                                        FieldName="ROTATRAJETOID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="IDA" Name="IDA" VisibleIndex="1" FieldName="IDA"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CEP" Name="CEP" ReadOnly="true" VisibleIndex="2"
                                        FieldName="CEP">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Logradouro" Name="LOGRADOURO" ReadOnly="true"
                                        VisibleIndex="3" FieldName="LOGRADOURO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Número" Name="NUMERO" ReadOnly="true" VisibleIndex="4"
                                        FieldName="NUMERO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Bairro" Name="BAIRRO" ReadOnly="true" VisibleIndex="5"
                                        FieldName="BAIRRO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="MUNICIPIO" Name="MUNICIPIO" ReadOnly="true"
                                        VisibleIndex="6" FieldName="MUNICIPIO" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Município" Name="DESCRICAOMUNICIPIO" ReadOnly="true"
                                        VisibleIndex="7" FieldName="DESCRICAOMUNICIPIO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Estado" Name="ESTADO" ReadOnly="true" VisibleIndex="8"
                                        FieldName="ESTADO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Latitude" Name="LATITUDE" ReadOnly="true"
                                        VisibleIndex="9" FieldName="LATITUDE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Longitude" Name="LONGITUDE" ReadOnly="true"
                                        VisibleIndex="10" FieldName="LONGITUDE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataCheckColumn Caption="Primeiro" Name="PRIMEIRO" VisibleIndex="11"
                                        FieldName="PRIMEIRO">
                                    </dxwgv:GridViewDataCheckColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <asp:ObjectDataSource ID="odsPontoEmbarqueVolta" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Rota"
                                SelectMethod="ListarPontoEmbarqueVolta" DeleteMethod="DeleteEmbarqueVolta" UpdateMethod="UpdateEmbarqueVolta">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnRotaTrajetoIdVolta" Name="rotaTrajetoIdVolta"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdPontoEmbarqueVolta" runat="server" DataSourceID="odsPontoEmbarqueVolta"
                                KeyFieldName="PONTOEMBARQUEID" AutoGenerateColumns="false" ClientInstanceName="grdPontoEmbarqueVolta"
                                OnInitNewRow="grdPontoEmbarqueVolta_InitNewRow" OnStartRowEditing="grdPontoEmbarqueVolta_StartRowEditing"
                                OnCustomButtonCallback="grdPontoEmbarqueVolta_CustomButtonCallback" EnableCallBacks="False"
                                OnRowDeleting="grdPontoEmbarqueVolta_RowDeleting" Width="700px" OnAfterPerformCallback="grdPontoEmbarqueVolta_AfterPerformCallback">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="InLine" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <CustomButtons>
                                            <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="EditarVolta" Visibility="AllDataRows"
                                                Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                            </dxwgv:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PONTOEMBARQUEID" Name="PONTOEMBARQUEID" VisibleIndex="1"
                                        FieldName="PONTOEMBARQUEID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ROTATRAJETOID" Name="ROTATRAJETOID" VisibleIndex="1"
                                        FieldName="ROTATRAJETOID" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="IDA" Name="IDA" VisibleIndex="1" FieldName="IDA"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CEP" Name="CEP" ReadOnly="true" VisibleIndex="2"
                                        FieldName="CEP">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Logradouro" Name="LOGRADOURO" ReadOnly="true"
                                        VisibleIndex="3" FieldName="LOGRADOURO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Número" Name="NUMERO" ReadOnly="true" VisibleIndex="4"
                                        FieldName="NUMERO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Bairro" Name="BAIRRO" ReadOnly="true" VisibleIndex="5"
                                        FieldName="BAIRRO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="MUNICIPIO" Name="MUNICIPIO" ReadOnly="true"
                                        VisibleIndex="6" FieldName="MUNICIPIO" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Município" Name="DESCRICAOMUNICIPIO" ReadOnly="true"
                                        VisibleIndex="7" FieldName="DESCRICAOMUNICIPIO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Estado" Name="ESTADO" ReadOnly="true" VisibleIndex="8"
                                        FieldName="ESTADO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Latitude" Name="LATITUDE" ReadOnly="true"
                                        VisibleIndex="9" FieldName="LATITUDE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Longitude" Name="LONGITUDE" ReadOnly="true"
                                        VisibleIndex="10" FieldName="LONGITUDE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataCheckColumn Caption="Primeiro" Name="PRIMEIRO" VisibleIndex="11"
                                        FieldName="PRIMEIRO">
                                    </dxwgv:GridViewDataCheckColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="AssociacaoAluno" Text="Associação Aluno">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccAluno" runat="server">
                            <div class="divEditBlock" style="width: 740px;">
                                <asp:ImageButton ID="btnNovaAssociacaoAluno" runat="server" SkinID="BcNovo" OnClick="btnNovaAssociacaoAluno_Click" />
                                <asp:ImageButton ID="btnCancelAssociacaoAluno" runat="server" SkinID="BcCancelar"
                                    OnClick="btnCancelAssociacaoAluno_Click" />
                                <asp:ImageButton ID="btnIncluirAssociacao" runat="server" SkinID="BcSalvar" OnClick="btnIncluirAssociacao_Click"
                                    OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
                                <asp:Label runat="server" ID="Label32" Text="Aluno" SkinID="BcTitulo" />
                            </div>
                            <asp:Panel ID="pnlNovaAssociacao" runat="server" GroupingText="Escolha o Trajeto"
                                Width="100%">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTrajeto" runat="server" Text="Tipo:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblTrajetoAssociacao" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="rblTrajetoAssociacao_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Ida" Value="Ida"></asp:ListItem>
                                                <asp:ListItem Text="Volta" Value="Volta"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlDadosNovaAssociacao" runat="server" Width="100%" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoAssociacaoTransporte"
                                                    AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                                    <QueryParameters>
                                                        <asp:ControlParameter Name="unidade_ens" ControlID="tseUnidadeResponsavel" PropertyName="DBValue" />
                                                        <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                                    </QueryParameters>
                                                </tweb:TSearch>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label33" runat="server" SkinID="lblObrigatorio" Text="Data Início:* "></asp:Label>
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
                                                <asp:Label ID="Label34" runat="server" SkinID="lblObrigatorio" Text="Data Fim:* "></asp:Label>
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
                            <br />
                            <asp:ObjectDataSource ID="odsRotaAlunoIda" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Rota"
                                SelectMethod="ListarRotaAlunoIda" UpdateMethod="UpdateRotaAlunoIda">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnRotaTrajetoIdIda" DefaultValue="" Name="trajetoIda"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdRotaAlunoIda" runat="server" DataSourceID="odsRotaAlunoIda"
                                KeyFieldName="ROTAALUNOID" AutoGenerateColumns="false" ClientInstanceName="grdRotaAlunoIda"
                                OnInitNewRow="grdRotaAlunoIda_InitNewRow" OnStartRowEditing="grdRotaAlunoIda_StartRowEditing"
                                OnRowUpdating="grdRotaAlunoIda_RowUpdating" Width="600px" OnAfterPerformCallback="grdRotaAlunoIda_AfterPerformCallback">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="InLine" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                                        FieldName="ALUNO" Width="200px">
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
                            <asp:ObjectDataSource ID="odsRotaAlunoVolta" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Rota"
                                SelectMethod="ListarRotaAlunoVolta" UpdateMethod="UpdateRotaAlunoVolta">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnRotaTrajetoIdVolta" DefaultValue="" Name="trajetoVolta"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdRotaAlunoVolta" runat="server" DataSourceID="odsRotaAlunoVolta"
                                KeyFieldName="ROTAALUNOID" AutoGenerateColumns="false" ClientInstanceName="grdRotaAlunoVolta"
                                OnInitNewRow="grdRotaAlunoVolta_InitNewRow" OnStartRowEditing="grdRotaAlunoVolta_StartRowEditing"
                                OnRowUpdating="grdRotaAlunoVolta_RowUpdating" Width="600px" OnAfterPerformCallback="grdRotaAlunoVolta_AfterPerformCallback">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="InLine" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                                        FieldName="ALUNO" Width="200px">
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
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
