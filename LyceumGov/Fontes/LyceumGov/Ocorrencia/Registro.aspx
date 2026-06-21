<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Registro.aspx.cs" Inherits="Techne.Lyceum.Net.Ocorrencia.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.css" rel="stylesheet">
    <link href="../Styles/Ocorrencia.css" rel="stylesheet" type="text/css">
    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarCancelamento.Show();
            }, 1000);
        }


        function fecharPopup() {
            window.setTimeout(function() {

                pucConfirmarCancelamento.Hide();
            }, 1000);
        }


        function VerificaTamanhoArquivo() {
            var fi = document.getElementById('<%= FileUpload1.ClientID %>');
            var maxFileSize = 1048576; // 1MB -> 1048576

            if (fi.files.length > 0) {

                for (var i = 0; i <= fi.files.length - 1; i++) {

                    var fsize = fi.files.item(i).size;

                    if (fsize > maxFileSize) {
                        alert("Os arquivos devem ter tamanho com até 1 MB");
                        fi.value = null;
                    }

                }
            }
        }

        function VerificaTamanhoArquivoEncaminhamento() {
            var fi = document.getElementById('<%= FileUpload2.ClientID %>');
            var maxFileSize = 1048576; // 1MB -> 1048576

            if (fi.files.length > 0) {

                for (var i = 0; i <= fi.files.length - 1; i++) {

                    var fsize = fi.files.item(i).size;

                    if (fsize > maxFileSize) {
                        alert("Os arquivos devem ter tamanho com até 1 MB");
                        fi.value = null;
                    }

                }
            }
        }

        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueioRegistro");
            divBloqueio.className = "Bloqueado";
        }   
       
    </script>

    <div id="dvbloqueioRegistro" class="Desbloqueado">
    </div>
    <asp:Label ID="lblTipoOperacao" runat="server" SkinID="lblNomePagina"></asp:Label>&nbsp;
    <br />
    <br />
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Características da unidade selecionada"
        Width="70%">
        <table width="70%">
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Regional:*" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: left; width: 50%">
                    <asp:Label Font-Names="Verdana" ID="lblRegional" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Município:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:*"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: left; width: 50%">
                    <asp:Label Font-Names="Verdana" ID="lblEscola" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    <asp:HiddenField ID="hdnCenso" runat="server" />
                </td>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Bairro:*" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: left; width: 50%">
                    <asp:Label Font-Names="Verdana" ID="lblBairro" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    <asp:HiddenField ID="HiddenField5" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Ano:*" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: left; width: 50%">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
            <td align="left" colspan="4">
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <asp:Label ID="lblMensagemFinalizada" runat="server" SkinID="lblMensagem"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hdnQueryString" runat="server" />
    <asp:HiddenField ID="hdnOcorrenciaId" runat="server" />
    <asp:HiddenField ID="hdnPerfil" runat="server" />
    <div class="divEditBlock" style="width: 70%;">
        <asp:ImageButton ID="btnNovaConsulta" runat="server" ImageAlign="Right" ImageUrl="~/Images/bt_nova_consulta.png"
            OnClick="btnNovaConsulta_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" ImageAlign="Right" ImageUrl="~/Images/bt_salvar.png"
            OnClick="btnSalvar_Click" OnClientClick="Bloqueio()" />
        <asp:ImageButton ID="btnExcluirOcorrencia" SkinID="BcDeletar" runat="server" OnClick="btnExcluirOcorrencia_Click"
            OnClientClick="Bloqueio()" />
        <asp:ImageButton ID="btnArquivar" runat="server" ImageAlign="Right" ImageUrl="~/Images/bt_arquivado.png"
            OnClick="btnArquivar_Click" CausesValidation="false" OnClientClick="Bloqueio()" />
        <asp:ImageButton ID="btnCancel" runat="server" ImageAlign="Right" SkinID="Voltar"
            OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Ocorrência" SkinID="BcTitulo" />
    </div>
    <asp:HiddenField ID="hdnArquivoId" runat="server" />
    <asp:Panel runat="server" ID="pnlEncaminhamento" GroupingText="Encaminhamentos" Width="70%"
        Visible="false">
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="txtEncaminhamento" runat="server" MaxLength="5000" TextMode="MultiLine"
                        Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload2" runat="server" onchange="Javascript: VerificaTamanhoArquivoEncaminhamento();" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Button ID="btnAdicionarEncaminhamento" runat="server" OnClientClick="Bloqueio()"
                        Text="Adicionar" OnClick="btnAdicionarEncaminhamento_Click" />
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="odsEncaminhamento" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
            SelectMethod="ListaEncaminhamento" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                    Name="id" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ClientInstanceName="grdEncaminhamento" ID="grdEncaminhamento"
            runat="server" Width="80%" DataSourceID="odsEncaminhamento" KeyFieldName="OCORRENCIAENCAMINHAMENTOID"
            EnableCallBacks="false" OnCustomButtonCallback="grdEncaminhamento_CustomButtonCallback">
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
            <SettingsCookies Enabled="false" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px" Caption="Excluir">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluirEncaminhamento" Text="Excluir"
                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="OCORRENCIAENCAMINHAMENTOID" FieldName="OCORRENCIAENCAMINHAMENTOID"
                    VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="OCORRENCIAENCAMINHAMENTOARQUIVOID" FieldName="OCORRENCIAENCAMINHAMENTOARQUIVOID"
                    VisibleIndex="2" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Encaminhamento" FieldName="ENCAMINHAMENTO"
                    VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Arquivo" FieldName="NOMEARQUIVO" VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ARQUIVO" FieldName="ARQUIVO" VisibleIndex="10"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TIPOARQUIVO" FieldName="TIPOARQUIVO" VisibleIndex="11"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Visualizar" Name="btnVisualizar" VisibleIndex="9"
                    Width="100px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <DataItemTemplate>
                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("OCORRENCIAENCAMINHAMENTOARQUIVOID") + "," + Eval("TIPOARQUIVO") %>'
                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                            AlternateText="Visualizar Encaminhamento" Visible='<%# Eval("OCORRENCIAENCAMINHAMENTOARQUIVOID") != DBNull.Value && Eval("OCORRENCIAENCAMINHAMENTOARQUIVOID") != DBNull.Value %>'>
                        </asp:ImageButton>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Encaminhamento">
            <HeaderStyle HorizontalAlign="Center" />
            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
            <ContentCollection>
                <dxpc:PopupControlContentControl>
                    <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                        Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                        ClientInstanceName="bimgArquivo">
                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                    </dxe:ASPxBinaryImage>
                    <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInformacoes" GroupingText="Informações Gerais" Width="70%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblData" SkinID="lblObrigatorio" runat="server"
                        Text="Data da ocorrência:*"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataOcorrencia" runat="server" Width="100px" Enabled="true"
                        AutoPostBack="true" EnableDefaultAppearance="true" ClientInstanceName="dtDataOcorrencia"
                        CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje"
                        OnDateChanged="dtDataOcorrencia_DateChanged">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblClasse" runat="server" Font-Names="Verdana" Text="Classe:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseClasse" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                        OnChanged="tseClasse_Changed" Key="classeid" SqlSelect="select distinct CLASSEID, DESCRICAO,ORDEM from [Ocorrencias].[CLASSE]"
                        SqlWhere=" ATIVO = 1 " SqlOrder="ORDEM">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CLASSEID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Text="SubClasse:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseSubClasse" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" Caption="" OnChanged="tseSubClasse_Changed"
                        Key="subclasseid" SqlSelect="select distinct SUBCLASSEID, DESCRICAO,ORDEM, CLASSEID from [Ocorrencias].[SUBCLASSE]"
                        SqlOrder="ORDEM" SqlWhere=" ATIVO = 1 AND CLASSEID = #tseClasse# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="SUBCLASSEID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="DESCRICAO" Width="80%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="CLASSEID" Width="80%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label11" runat="server" Font-Names="Verdana" Text="Meio:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMeio" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" Caption="" OnChanged="tseMeio_Changed"
                        Key="MEIOID" SqlSelect="select distinct DESCRICAO,ORDEM from [Ocorrencias].[MEIO]"
                        SqlWhere=" ATIVO = 1 " SqlOrder="ORDEM">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="MEIOID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Meio" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label ID="lblfCritica" runat="server" Text="Uso de Arma?:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblUsoArma" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblUsoArma_IndexChanged">
                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:CheckBoxList ID="chkUsoArma" runat="server" Visible="false" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Branca</asp:ListItem>
                        <asp:ListItem Value="2">Fogo</asp:ListItem>
                        <asp:ListItem Value="3">Artefato</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Text="Batalhão de Polícia:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseBatalhao" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                        OnChanged="tseBatalhao_Changed" Key="BATALHAOID" SqlSelect="select distinct  DESCRICAO from [Ocorrencias].[BATALHAO]"
                        SqlWhere=" ATIVO = 1 " SqlOrder="batalhaoid">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="BATALHAOID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Batalhão de Polícia" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label7" runat="server" Font-Names="Verdana" Text="Delegacia:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseDelegacia" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                        OnChanged="tseDelegacia_Changed" Key="delegaciaid" SqlSelect="select distinct  DESCRICAO from [Ocorrencias].[DELEGACIA]"
                        SqlWhere=" ATIVO = 1 AND BATALHAOID = #tseBatalhao#" SqlOrder="descricao, delegaciaid">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="DELEGACIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Delegacia" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label10" runat="server" Text="R.O.:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRO" runat="server" Width="100px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDescricao" runat="server"
                        Text="Descrição do ocorrido:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="500px" MaxLength="2000" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label9" runat="server" Text="Observação:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtObservacao" runat="server" Width="500px" MaxLength="5000" TextMode="MultiLine" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnProsseguir" Text="Prosseguir" runat="server" OnClientClick="Bloqueio()"
                        OnClick="btnProsseguir_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlProsseguir" Visible="false">
        <asp:Panel runat="server" ID="pnlInterrupcao" GroupingText="Interrupção" Width="70%">
            <table>
                <tr>
                    <td>
                        <asp:Label Font-Names="Verdana" ID="Label13" runat="server" Text="O fato acima ocasionou interrupção de aulas?*"
                            SkinID="lblObrigatorio"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblInterrupcao" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblInterrupcao_SelectedIndexChanged">
                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSimInterrupcao" Width="70%" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="Label14" runat="server" Text="Data:*" SkinID="lblObrigatorio"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="Label15" runat="server" Text="Turno:*" SkinID="lblObrigatorio"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxDateEdit ID="dtInterrupcao" runat="server">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chkTurno" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="M">Manhã</asp:ListItem>
                                <asp:ListItem Value="T">Tarde</asp:ListItem>
                                <asp:ListItem Value="N">Noite</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAdicionarInterrupcao" runat="server" Text="Adicionar Interrupção"
                                OnClick="btnAdicionarInterrupcao_Click" OnClientClick="Bloqueio()" />
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsInterrupcao" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
                            SelectMethod="ListaInterrupcao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                                    Name="id" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdInterrupcao" runat="server" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="grdInterrupcao" DataSourceID="odsInterrupcao"
                            KeyFieldName="OCORRENCIAINTERRUPCAOID" OnCustomButtonCallback="grdInterrupcao_CustomButtonCallback">
                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirInterrupcao"
                                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png" Image-Height="15px"
                                            Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="OCORRENCIAINTERRUPCAOID"
                                    ReadOnly="true" VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Interrupção" FieldName="DATAINTERRUPCAO"
                                    VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="MANHA" Caption="Manhã" ReadOnly="true" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TARDE" Caption="Tarde" ReadOnly="true" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="NOITE" Caption="Noite" ReadOnly="true" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlVitima" GroupingText="Alvo" Width="70%">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblTipoVitima" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblTipoVitima" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblTipoVitima_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Aluno" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Servidor" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Unidade" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Outro" Value="4"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlVitimaAluno" runat="server" GroupingText="Informe a matrícula ou o nome do aluno">
                <table>
                    <tr>
                        <td>
                            <tweb:TSearch ID="tseAlunoVitima" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoOcorrencia"
                                AutoPostBack="true" OnTextChanged="tseAlunoVitima_Changed">
                            </tweb:TSearch>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoVitimaAluno" Text="Desconhecido"
                                Width="140px" AutoPostBack="true" OnCheckedChanged="chkDesconhecidoVitimaAluno_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblIdadeVitima" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlVitimaServidor" runat="server" GroupingText="Informe ID/Vínculo do Servidor ou o nome do servidor">
                <table>
                    <tr>
                        <td>
                            <tweb:TSearch ID="tseServidorVitima" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionarioOcorrencia"
                                AutoPostBack="true" OnTextChanged="tseServidorVitima_Changed">
                                <QueryParameters>
                                    <asp:ControlParameter Name="DataOcorrencia" ControlID="dtDataOcorrencia" PropertyName="Date" />
                                </QueryParameters>
                            </tweb:TSearch>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoVitimaServidor" Text="Desconhecido"
                                Width="140px" AutoPostBack="true" OnCheckedChanged="chkDesconhecidoVitimaServidor_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblCargoFuncaoVitima" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlVitimaOutro" runat="server" GroupingText="Informe os dados">
                <table>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblNomeVitima" runat="server" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNomeVitima" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);">
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoVitima" Text="Desconhecido" Width="140px"
                                AutoPostBack="true" OnCheckedChanged="chkDesconhecidoVitima_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label8" runat="server" Text="Data de Nascimento: "></asp:Label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtNascimentoVitima" runat="server">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblCPFVitima" runat="server" Text="CPF: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCPFVitima" runat="server" onkeyup="formataCPF(this,event)" MaxLength="14"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGTipoPessoaVitima" runat="server" Text="Tipo: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGTipoPessoaVitima" runat="server" DataValueField="item"
                                DatatTextField="descr" OnSelectedIndexChanged="ddlRGTipoPessoaVitima_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGNumPessoaVitima" runat="server" Text="Número: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRGNumPessoaVitima" runat="server" MaxLength="15" SkinID="numeroDocumento"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGUFPessoaVitima" runat="server" Text="Estado: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGUFPessoaVitima" runat="server" DataValueField="sigla"
                                DatatTextField="sigla">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGEmissorPessoaVitima" runat="server" Text="Órgão Emissor:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGEmissorPessoaVitima" runat="server" DataValueField="item"
                                DatatTextField="item">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGDataExpPessoaVitima" runat="server" Text="Data de Expedição: "></asp:Label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dteRGDataExpPessoaVitima" runat="server">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAdicionarVitima" Text="Adicionar Alvo" runat="server" OnClientClick="Bloqueio()"
                            OnClick="btnAdicionarVitima_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsVitima" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
                            SelectMethod="ListaVitima">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                                    Name="id" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdVitima" runat="server" AutoGenerateColumns="False" Width="100%"
                            ClientInstanceName="grdVitima" DataSourceID="odsVitima" KeyFieldName="VITIMAID"
                            OnCustomButtonCallback="grdVitima_CustomButtonCallback">
                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirVitima" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="VITIMAID" ReadOnly="true"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TIPODESCRICAO" Caption="Tipo" ReadOnly="true"
                                    VisibleIndex="2">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ALUNO" Caption="Aluno" ReadOnly="true" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="IDFUNCIONAL" Caption="ID Funcional" ReadOnly="true"
                                    VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="NOME" Caption="Nome" ReadOnly="true" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Nascimento" FieldName="DATANASCIMENTO"
                                    VisibleIndex="6">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="IDADE" Caption="Idade" ReadOnly="true" VisibleIndex="7">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="CARGO" Caption="Cargo" ReadOnly="true" VisibleIndex="8">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="DESCRICAOFUNCAO" Caption="Função" ReadOnly="true"
                                    VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlAcusado" GroupingText="Autor" Width="70%">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblTipoAcusado" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblTipoAcusado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblTipoAcusado_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Aluno" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Servidor" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Outro" Value="4"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlAcusadoAluno" runat="server" GroupingText="Informe a matrícula ou o nome do aluno">
                <table>
                    <tr>
                        <td>
                            <tweb:TSearch ID="tseAlunoAcusado" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoOcorrencia"
                                AutoPostBack="true" OnTextChanged="tseAlunoAcusado_Changed">
                            </tweb:TSearch>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoAcusadoAluno" Text="Desconhecido"
                                Width="140px" AutoPostBack="true" OnCheckedChanged="chkDesconhecidoAcusadoAluno_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblIdadeAcusado" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlAcusadoServidor" runat="server" GroupingText="Informe ID/Vínculo do Servidor ou o nome do servidor">
                <table>
                    <tr>
                        <td>
                            <tweb:TSearch ID="tseServidorAcusado" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionarioOcorrencia"
                                AutoPostBack="true" OnTextChanged="tseServidorAcusado_Changed">
                                <QueryParameters>
                                    <asp:ControlParameter Name="DataOcorrencia" ControlID="dtDataOcorrencia" PropertyName="Date" />
                                </QueryParameters>
                            </tweb:TSearch>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoAcusadoServidor" Text="Desconhecido"
                                Width="140px" AutoPostBack="true" OnCheckedChanged="chkDesconhecidoAcusadoServidor_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblCargoFuncaoAcusado" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlAcusadoOutro" runat="server" GroupingText="Informe os dados">
                <table>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblNomeAcusado" runat="server" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNomeAcusado" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);">
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDesconhecidoAcusado" Text="Desconhecido" Width="140px"
                                AutoPostBack="true" OnCheckedChanged="chkDesconhecidoAcusado_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label12" runat="server" Text="Data de Nascimento: "></asp:Label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtNascimentoAcusado" runat="server">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblCPFAcusado" runat="server" Text="CPF: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCPFAcusado" runat="server" onkeyup="formataCPF(this,event)" MaxLength="14"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGTipoPessoaAcusado" runat="server" Text="Tipo: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGTipoPessoaAcusado" runat="server" DataValueField="item"
                                DatatTextField="descr" OnSelectedIndexChanged="ddlRGTipoPessoaAcusado_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGNumPessoaAcusado" runat="server" Text="Número:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRGNumPessoaAcusado" runat="server" MaxLength="15" SkinID="numeroDocumento"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGUFPessoaAcusado" runat="server" Text="Estado: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGUFPessoaAcusado" runat="server" DataValueField="sigla"
                                DatatTextField="sigla">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGEmissorPessoaAcusado" runat="server" Text="Órgão Emissor:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRGEmissorPessoaAcusado" runat="server" DataValueField="item"
                                DatatTextField="item">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblRGDataExpPessoaAcusado" runat="server" Text="Data de Expedição: "></asp:Label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dteRGDataExpPessoaAcusado" runat="server">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAdicionarAcusado" Text="Adicionar Autor" runat="server" OnClientClick="Bloqueio()"
                            OnClick="btnAdicionarAcusado_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsAcusado" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
                            SelectMethod="ListaAcusado">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                                    Name="id" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdAcusado" runat="server" AutoGenerateColumns="False" Width="100%"
                            ClientInstanceName="grdAcusado" DataSourceID="odsAcusado" KeyFieldName="ACUSADOID"
                            OnCustomButtonCallback="grdAcusado_CustomButtonCallback">
                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirAcusado" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ACUSADOID" ReadOnly="true"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TIPODESCRICAO" Caption="Tipo" ReadOnly="true"
                                    VisibleIndex="2">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ALUNO" Caption="Aluno" ReadOnly="true" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="IDFUNCIONAL" Caption="ID Funcional" ReadOnly="true"
                                    VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="NOME" Caption="Nome" ReadOnly="true" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Nascimento" FieldName="DATANASCIMENTO"
                                    VisibleIndex="6">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="IDADE" Caption="Idade" ReadOnly="true" VisibleIndex="7">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="CARGO" Caption="Cargo" ReadOnly="true" VisibleIndex="8">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="DESCRICAOFUNCAO" Caption="Função" ReadOnly="true"
                                    VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlTratamento" GroupingText="Tratamento" Width="70%">
            <tweb:TSearchBox ID="tseTratamento" runat="server" Argument="descricao" ArgumentColumns="50"
                DataType="Number" MaxLength="20" Columns="10" Caption="" OnChanged="tseTratamento_Changed"
                Key="tratamentoid" SqlSelect="select distinct TRATAMENTOID, DESCRICAO,ORDEM from [Ocorrencias].[TRATAMENTO]"
                SqlWhere=" ATIVO = 1 " SqlOrder="ORDEM">
                <GridColumns>
                    <tweb:TSearchBoxColumn Caption="Código" FieldName="TRATAMENTOID" Width="20%" />
                    <tweb:TSearchBoxColumn Caption="Tratamento" FieldName="DESCRICAO" Width="80%" />
                </GridColumns>
            </tweb:TSearchBox>
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar Tratamento" OnClientClick="Bloqueio()"
                            OnClick="btnAdicionar_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsTratamento" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
                            SelectMethod="ListaTratamento">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                                    Name="id" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdTratamento" runat="server" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="grdTratamento" DataSourceID="odsTratamento"
                            KeyFieldName="OCORRENCIATRATAMENTOID" OnCustomButtonCallback="grdTratamento_CustomButtonCallback">
                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirTratamento"
                                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png" Image-Height="15px"
                                            Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="OCORRENCIATRATAMENTOID"
                                    ReadOnly="true" VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Tratamento" ReadOnly="true"
                                    VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblQtdSolicitacoes" runat="server" SkinID="lblMensagem" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlDocumento" GroupingText="Documentos" Width="70%"
            Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="Javascript: VerificaTamanhoArquivo();" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="text-align: right;">
                        <asp:Button ID="btnAnexar" runat="server" Text="Anexar arquivo" OnClientClick="Bloqueio()"
                            OnClick="btnAnexar_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="Panel2" runat="server" GroupingText="Arquivos" Font-Names="Verdana"
                Width="100%">
                <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Registro"
                    SelectMethod="ListaDocumento" DeleteMethod="Delete">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnOcorrenciaId" DefaultValue="" PropertyName="Value"
                            Name="id" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <dxwgv:ASPxGridView ClientInstanceName="grdDocumento" ID="grdDocumento" runat="server"
                    Width="100%" DataSourceID="odsDocumento" KeyFieldName="ARQUIVOOCORRENCIAID" EnableCallBacks="false"
                    OnCustomButtonCallback="grdDocumento_CustomButtonCallback">
                    <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                    <SettingsCookies Enabled="false" />
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px" Caption="Excluir">
                            <CustomButtons>
                                <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluirDocumento" Text="Excluir"
                                    Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png">
                                </dxwgv:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="ARQUIVOOCORRENCIAID" FieldName="ARQUIVOOCORRENCIAID"
                            VisibleIndex="1" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="OCORRENCIAID" FieldName="OCORRENCIAID" VisibleIndex="2"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Arquivo" FieldName="NOMEARQUIVO" VisibleIndex="8">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="ARQUIVO" FieldName="ARQUIVO" VisibleIndex="10"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="TIPOARQUIVO" FieldName="TIPOARQUIVO" VisibleIndex="11"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
                <asp:Repeater ID="repCarrossel" runat="server" DataSourceID="odsDocumento" OnItemDataBound="repCarrossel_ItemDataBound">
                    <HeaderTemplate>
                        <div class="glider-contain" style="width: 800px;">
                            <p style="text-align: center; color: red; font-size: 14px;">
                                Clique nos círculos abaixo para navegar no carrossel de documentos abaixo</p>
                            <div role="tablist" class="dots">
                            </div>
                            <div class="glider">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="text-align: center">
                            <div style="font-size: 16px; text-align: center; font-weight: bold; color: black;">
                                <p>
                                    <%# DataBinder.Eval(Container.DataItem, "NOMEARQUIVO")%></p>
                            </div>
                            <asp:PlaceHolder ID="plaTipoPDF" runat="server" Visible="false">
                                <object data="../Util/FileCS.ashx?Tabela=ArquivoOcorrencia&Id=<%# Eval("ARQUIVOOCORRENCIAID") %>"
                                    type="application/pdf" width="800px" height="1170px">
                                    <iframe src="../Util/FileCS.ashx?Tabela=ArquivoOcorrencia&Id=<%# Eval("ARQUIVOOCORRENCIAID") %>"
                                        width="100%" height="100%" style="border: none;">
                                        <p>
                                            Your browser does not support PDFs.<a href="../Util/FileCS.ashx?Tabela=ArquivoOcorrencia&Id=16">Download
                                                the PDF</a>.</p>
                                    </iframe>
                                </object>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plaTipoImagem" runat="server" Visible="false">
                                <img src="../Util/FileCS.ashx?Tabela=ArquivoOcorrencia&Id=<%# Eval("ARQUIVOOCORRENCIAID") %>"
                                    style="width: 700px; height: 900px;" />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plaSemArquivo" runat="server" Visible="false">
                                <div style="font-size: 12px; text-align: center; font-weight: bold; color: black;">
                                    <p>
                                        (Nenhum arquivo enviado)</p>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div> </div>

                        <script src="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.js"></script>

                        <script language="javascript" type="text/javascript">
                                    var glider = new Glider(document.querySelector('.glider'), {
                                      slidesToShow: 1,
                                      slidesToScroll: 1,
                                      draggable: true,
                                      dots: ".dots",
                                      arrows: {
                                        prev: ".prev",
                                        next: ".next",
                                      },
                                    });
                        </script>

                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucConfirmarCancelamento" ClientInstanceName="pucConfirmarCancelamento"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowHeader="true" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Cancelamento de Ocorrência">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label Font-Names="Verdana" ID="lblMotivo" runat="server" Text="Motivo Cancelamento:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMotivoCancelamento" runat="server" DataTextField="DESCRICAO"
                                DataValueField="MOTIVOCANCELAMENTOID" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnConfirmarCancelamento" runat="server" Text="Cancelar Ocorrência"
                                OnClick="btnConfirmarCancelamento_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltarCancelamento" runat="server" Text="Voltar" OnClick="btnVoltarCancelamento_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:Label ID="lblMensagemCancelamento" runat="server" SkinID="lblMensagem"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
