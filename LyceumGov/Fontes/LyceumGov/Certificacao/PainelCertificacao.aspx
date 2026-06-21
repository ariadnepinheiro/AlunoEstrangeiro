<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PainelCertificacao.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.Certificacao.PainelCertificacao" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateEditWithoutBorder
        {
            width: 100%;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
        }
    </style>
    
    <style>
        table.filtro {
            table-layout: fixed;
        }

        table.filtro td.titulo {
            text-align: right;
            padding-top: 5px;
            padding-right: 10px;
            padding-bottom: 5px;
        }

        table.filtro  td.titulo > span {
            color: red;
        }

        table.filtro td.botao {
            padding-bottom: 5px;
            text-align: right;
            vertical-align: bottom;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/js/min/jquery.maskedinput.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function OnEndCallBack(source) {

        }

        $(document).ready(function() {

            $("input[id*='rblistSituacao']:checked").each(function() {
                controlarSituacao(this, true);
            });

            $("input[id*='rblistSituacao']").click(function() {
                controlarSituacao(this, true);
            });

        });

        function controlarSituacao(radioButton, limparTexto) {

            var situacao = $(radioButton).val();
            var cmbMotivo = $("#" + $(radioButton).attr("cmbMotivo"));

            $(cmbMotivo).attr("readonly", "readonly");
            $(cmbMotivo).attr("disabled", true);
            $(cmbMotivo).css("background-color", "red");

            if (situacao == "4") //INDEFERIDO
            {
                // alert('indefirido');
                $(cmbMotivo).removeAttr("disabled");
                $(cmbMotivo).removeAttr("readonly");
                $(cmbMotivo).css("background-color", "Gainsboro");
            }
            else {
                // alert(situacao);

            }
        }

    </script>

    <asp:HiddenField runat="server" ID="hdnIdTermo" />
    <asp:HiddenField runat="server" ID="hdnDadosPublicacao" />
    <asp:HiddenField runat="server" ID="hdnAba" />
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Dados da Solicitação" Width="90%">
        <table class="filtro" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                    <td width="75"></td>
                </tr>

                <tr>
                    <td colspan="2" class="titulo">
                        <b>Status Solicitação:</b> <span>*</span>
                    </td>
                    <td colspan="8" class="campo">
                        <asp:RadioButtonList ID="rbStatusSituacao" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Text="Todos"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Solicitado"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Emitido"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Entregue"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Indeferido"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    
                    <td colspan="2" rowspan="5" class="botao">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" OnClientClick="return Valida();" />
                        <asp:Button ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click" />
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="titulo">
                        Unidade Certificadora:
                    </td>
                    <td colspan="8" class="campo">
                        <asp:DropDownList ID="ddlUnidade" runat="server" DataTextField="UNIDADE" DataValueField="UNIDADECERTIFICADORAID" AppendDataBoundItems="true" AutoPostBack="false" Width="557px">
                    </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="titulo">Nome Aluno:</td>
                    <td colspan="8" class="campo">
                        <asp:TextBox ID="txtNome" runat="server" MaxLength="800" Width="550px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="titulo">CPF:</td>
                    <td colspan="2" class="campo">
                        <asp:TextBox ID="txtCpf" onkeyup="formataCPF(this,event)" runat="server" MaxLength="800" TextMode="SingleLine" Width="135px"></asp:TextBox>
                    </td>
                    <td class="titulo">Data Ini.:</td>
                    <td colspan="2" class="campo">
                        <dxe:ASPxDateEdit ID="dtDataIni" runat="server" Width="100px" Enabled="true"
                            EnableDefaultAppearance="true" ClientInstanceName="dtDataIni" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td class="titulo">Data Fim:</td>
                    <td colspan="2" class="campo">
                        <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="100px" Enabled="true"
                            EnableDefaultAppearance="true" ClientInstanceName="dtDataIni" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="titulo">Número Protocolo:</td>
                    <td colspan="8">
                        <asp:TextBox ID="txtNumeroProtocolo" runat="server" MaxLength="800" Width="135px"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsMotivo" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.PainelCertificacao"
        SelectMethod="ListarMotivo"></asp:ObjectDataSource>
    <asp:Panel ID="pnAbas" runat="server" Width="100%" Visible="true">
        <asp:Panel ID="pnlDocumentos" runat="server">
            <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.PainelCertificacao"
                SelectMethod="ListaAluno">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtNome" DefaultValue="" Name="nome" PropertyName="Text" />
                    <asp:ControlParameter ControlID="txtCpf" DefaultValue="" Name="cpf" PropertyName="Text" />
                    <asp:ControlParameter ControlID="txtNumeroProtocolo" DefaultValue="" Name="numeroProtocolo"
                        PropertyName="Text" />
                    <asp:ControlParameter ControlID="rbStatusSituacao" DefaultValue="" Name="statusSolicitacao"
                        PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="ddlUnidade" DefaultValue="" Name="polo" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="dtDataIni" DefaultValue="" Name="dataIni" PropertyName="Value" />
                    <asp:ControlParameter ControlID="dtDataFim" DefaultValue="" Name="dataFim" PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div style="visibility: hidden">
                <asp:TextBox runat="server" ID="txtWebAuthUser"></asp:TextBox>
            </div>
            <dxwgv:ASPxGridView ClientInstanceName="grdDocumento" ID="grdDocumento" EnableCallBacks="false"
                DataSourceID="odsDocumento" KeyFieldName="ENCCEJAALUNOID" runat="server" OnCustomButtonCallback="grdDocumento_CustomButtonCallback"
                OnRowDataBound="grdDocumento_RowDataBound" OngrdDocumento_AfterPerformCallback="grdDocumento_AfterPerformCallback"
                OnHtmlRowCreated="grdDocumento_HtmlRowCreated" OnCustomColumnDisplayText="grdDocumento_CustomColumnDisplayText"
                OnCustomButtonInitialize="grdDocumento_CustomButtonInitialize">
                <Settings ShowFilterRow="false" ShowFilterRowMenu="true" />
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsCookies Enabled="false" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="48px" Caption="Editar"
                        Name="Editar">
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton ID="btnSalvarGrid" Text="Salvar" Image-Width="18px"
                                Visibility="AllDataRows" Image-Url="../img/bt_salvar.png">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="TIPOCERTIFICACAO" VisibleIndex="1" Caption="Tipo"
                        Width="130px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="POSSUISEGUNDAVIA" VisibleIndex="1" Caption="2ª Via"
                        CellStyle-ForeColor="Red" CellStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="UNIDADE" VisibleIndex="1" Caption="Unidade"
                        Width="130px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="LOGRADOURO" VisibleIndex="1" Caption="LOGRADOURO"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="NUMERO" VisibleIndex="1" Caption="NUMERO"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="BAIRRO" VisibleIndex="1" Caption="BAIRRO"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="COMPLEMENTO" VisibleIndex="1" Caption="COMPLEMENTO"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="MUNICIPIO" VisibleIndex="1" Caption="MUNICIPIO"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="MOTIVOINDEFERIDODESCRICAO" VisibleIndex="1"
                        Caption="MOTIVOINDEFERIDODESCRICAO" CellStyle-HorizontalAlign="Center" Width="30"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="MOTIVOINDEFERIDOID" VisibleIndex="1" Caption="MOTIVOINDEFERIDOID"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="ENCCEJAREQUERIMENTOID" VisibleIndex="1"
                        Caption="ENCCEJAREQUERIMENTOID" CellStyle-HorizontalAlign="Center" Width="30"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn> 
                    <dxwgv:GridViewDataTextColumn FieldName="SITUACAOENCCEJAREQUERIMENTOID" VisibleIndex="1"
                        Caption="SITUACAOENCCEJAREQUERIMENTOID" CellStyle-HorizontalAlign="Center" Width="30"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nº Protocolo" FieldName="PROTOCOLO" VisibleIndex="2"
                        Visible="true" Width="100px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="3" Visible="true"
                        Width="200px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="4" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Mãe" FieldName="NOMEMAE" VisibleIndex="5"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Pai" FieldName="NOMEPAI" VisibleIndex="6"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Cpf" FieldName="CPF" VisibleIndex="7" Visible="true"
                        Width="50px" Name="CPF">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Rg" FieldName="RG" VisibleIndex="8" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn VisibleIndex="8" Caption="Nascimento" Name="DATANASCIMENTO"
                        FieldName="DATANASCIMENTO" Width="100px" Visible="false" ReadOnly="true">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                            </ValidationSettings>
                        </PropertiesDateEdit>
                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                        </CellStyle>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="FIXOCELULAR" VisibleIndex="9"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="10"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Email" FieldName="EMAIL" VisibleIndex="11"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn VisibleIndex="15" Caption="Dt. Solicitação" FieldName="DATASOLICITACAO"
                        Width="100px" Visible="true" ReadOnly="true">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                            </ValidationSettings>
                        </PropertiesDateEdit>
                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                        </CellStyle>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn Caption="Situação *" Name="Situacao" VisibleIndex="16">
                        <DataItemTemplate>
                            <asp:RadioButtonList runat="server" ID="rblistSituacao" SelectedValue='<%# Bind("SITUACAOENCCEJAREQUERIMENTOID") %>'
                                RepeatDirection="Horizontal">
                                <asp:ListItem Value="2" Text="Emitido" />
                                <asp:ListItem Enabled="false" Value="3" Text="Entregue" />
                                <asp:ListItem Value="4" Text="Indeferido" />
                                <asp:ListItem Enabled="false" Value="1" Text="Solicitado" />
                            </asp:RadioButtonList>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Motivo " FieldName="DESCRICAO" Name="MOTIVO" UnboundType="String"
                        VisibleIndex="17">
                        <DataItemTemplate>
                            <asp:TextBox ID="txtMotivo" runat="server" ReadOnly="true" Enabled="false" Text='<%# Bind("MOTIVOINDEFERIDODESCRICAO") %>'></asp:TextBox>
                            <asp:HiddenField ID="hfMotivo" runat="server" Value='<%# Bind("MOTIVOINDEFERIDOID") %>' />
                            <asp:DropDownList ID="cmbMotivo" runat="server" DataSourceID="odsMotivo" DataTextField="DESCRICAO"
                                DataValueField="MOTIVOINDEFERIDOID" Width="100px">
                            </asp:DropDownList>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Dt. Situação" FieldName="DATAVERIFICACAO"
                        VisibleIndex="18" Visible="true">
                        <DataItemTemplate>
                            <dxe:ASPxDateEdit ID="dtVerificacao" runat="server" Width="120px" Enabled="true"
                                EnableDefaultAppearance="true" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataColumn Caption="Dt. Entrega" FieldName="DATAENTREGA" Name="DTENTREGA"
                        UnboundType="String" VisibleIndex="20">
                        <DataItemTemplate>
                            <dxe:ASPxDateEdit ID="dtEntrega" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Dados Cadastro" Name="btnVisualizarDados"
                        VisibleIndex="21" Width="35px">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <DataItemTemplate>
                            <asp:LinkButton ID="lkDadosCadastro" runat="server" Text="Dados Cadastro" OnCommand="lkDadosCadastro_Command"
                                CommandArgument='<%# Eval("[NOME]") + "," + Eval("[NOMEMAE]") + "," + Eval("[NOMEPAI]") + "," + Eval("CPF") + "," + Eval("[RG]")+ "," + Eval("[FIXOCELULAR]")+ "," + Eval("[CELULAR]")+ "," + Eval("[EMAIL]")+ "," + Eval("[DATANASCIMENTO]")+ "," + Eval("[LOGRADOURO]")+ "," + Eval("[NUMERO]")+ "," + Eval("[BAIRRO]")+ "," + Eval("[COMPLEMENTO]")+ "," + Eval("[MUNICIPIO]")%>' />
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Anexos" Name="btnVisualizar" VisibleIndex="22"
                        Width="35px">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <DataItemTemplate>
                            <asp:LinkButton ID="lkImgRg" runat="server" Text="Visualizar" OnCommand="lkImg_Command"
                                CommandArgument='<%# Eval("[ENCCEJAREQUERIMENTOID]") + ",1" %>' />
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    
                    <dxwgv:GridViewDataTextColumn FieldName="ENDERECOUNIDADE" VisibleIndex="23" Width="0" Visible="false" />
                    <dxwgv:GridViewDataTextColumn FieldName="NUMEROUNIDADE" VisibleIndex="24" Width="0" Visible="false" />
                    <dxwgv:GridViewDataTextColumn FieldName="BAIRROUNIDADE" VisibleIndex="25" Width="0" Visible="false" />
                    <dxwgv:GridViewDataTextColumn FieldName="COMPLEMENTOUNIDADE" VisibleIndex="26" Width="0" Visible="false" />
                    <dxwgv:GridViewDataTextColumn FieldName="MUNICIPIOUNIDADE" VisibleIndex="27" Width="0" Visible="false" />
                    
                </Columns>
            </dxwgv:ASPxGridView>
            <dxpc:ASPxPopupControl ID="pucArquivos" runat="server" CloseAction="CloseButton"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pucInfoAluno" HeaderText="Anexos" AllowDragging="True" Width="550px"
                EnableAnimation="True" EnableViewState="False">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl>
                        <asp:HiddenField ID="hdnEnccejaRequirimentoId" runat="server" />
                        <asp:Label ID="lblTipoArquivo" Text="Arquivos RG enviados:" runat="server" />
                        <ul>
                            <asp:Repeater ID="rpAruivosRG" runat="Server">
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="hlAruivoRG" runat="server" Text='<%# Eval("NOMEARQUIVO") %>' OnCommand="lkArquivo_Command"
                                            CommandArgument='<%# Eval("[ENCCEJADOCUMENTOARQUIVOID]") + ",1,"  + Eval("TIPOARQUIVO") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <asp:Label ID="Label18" Text="Arquivos CPF enviados:" runat="server" />
                        <ul>
                            <asp:Repeater ID="rpAruivosCPF" runat="Server">
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="hlAruivoCPF" runat="server" Text='<%# Eval("NOMEARQUIVO") %>' OnCommand="lkArquivo_Command"
                                            CommandArgument='<%# Eval("[ENCCEJADOCUMENTOARQUIVOID]") + ",2,"  + Eval("TIPOARQUIVO") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <asp:Label ID="Label19" Text="Arquivos BOLETIM enviados:" runat="server" />
                        <ul>
                            <asp:Repeater ID="rpAruivosBOLETIM" runat="Server" >
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="hlAruivoBOLETIM" runat="server" Text='<%# Eval("NOMEARQUIVO") %>' OnCommand="lkArquivo_Command"
                                            CommandArgument='<%# Eval("[ENCCEJADOCUMENTOARQUIVOID]") + ",3,"  + Eval("TIPOARQUIVO") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <asp:Label ID="Label20" Text="Arquivos HISTÓRICO enviados:" runat="server" />
                        <ul>
                            <asp:Repeater ID="rpAruivosHISTORICO" runat="Server" >
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="hlAruivoHISTORICO" runat="server" Text='<%# Eval("NOMEARQUIVO") %>' OnCommand="lkArquivo_Command"
                                            CommandArgument='<%# Eval("[ENCCEJADOCUMENTOARQUIVOID]") + ",4,"  + Eval("TIPOARQUIVO") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
            <dxpc:ASPxPopupControl ID="pucDadosCadastro" runat="server" CloseAction="CloseButton"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pucInfoAluno" HeaderText="Detalhes do Requerimento" AllowDragging="True"
                Width="550px" EnableAnimation="True" EnableViewState="False">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="pucccDadosCadastro" runat="server">
                        <asp:Panel runat="server" ID="Panel1" GroupingText="Dados Pessoais" Width="100%">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" Text="Nome: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNome" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Text="Nome da Mãe: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNomeMae" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Text="Nome do Pai: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNomePai" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" Text="CPF: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCpf" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" Text="RG: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRg" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" Text="Telefone: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTelefone" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Text="Celular: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCelular" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Text="E-mail: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" Text="Data de nascimento: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDataNasc" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel2" GroupingText="Dados do Endereço" Width="100%">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label13" Text="Logradouro: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLogradouro" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label14" Text="Número: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNumero" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" Text="Bairro: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBairro" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label15" Text="Complemento: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblComplemento" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label17" Text="Município: " Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMunicipio" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
            <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
                runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
                ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Img RG">
                <ContentStyle VerticalAlign="Middle" HorizontalAlign="Center">
                </ContentStyle>
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl runat="server">
                        <dxe:ASPxBinaryImage ID="bimgArquivo" runat="server" AlternateText="sem foto" ClientInstanceName="bimgArquivo"
                            Height="350px" StoreContentBytesInViewState="True" Visible="False" Width="350px">
                            <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                        </dxe:ASPxBinaryImage>
                        <asp:Literal ID="ltEmbed" runat="server" Visible="False" />
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Center" />
                <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
            </dxpc:ASPxPopupControl>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
