<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MaeAssinaturaDigital.aspx.cs" Inherits="Techne.Lyceum.Net.Cadastros.MaeAssinaturaDigital" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <script src="../Scripts/SignerDigital-1.0.0.min.js" type="text/javascript"></script>
    
    <style type="text/css">
        #divTelaBloqueada 
        {
            display: none; 
            position: fixed; 
            width: 99%; 
            height: 80%; 
            background-color: #000; 
            opacity: 0.5;
            filter: alpha(opacity=50);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <div id="divTelaBloqueada"></div>
    
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
                <dxwgv:ASPxGridView ID="grdAssinaturaDigital" runat="server" AutoGenerateColumns="False" EnableCallBacks="false"
                    ClientInstanceName="grdAssinaturaDigital" DataSourceID="odsAssinaturaDigital" KeyFieldName="MAE_INSCRICAOID"
                    OnAfterPerformCallback="grdAssinaturaDigital_AfterPerformCallback" OnCustomButtonCallback="grdAssinaturaDigital_CustomButtonCallback"
                    OnCustomButtonInitialize="grdAssinaturaDigital_CustomButtonInitialize">
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="grdAssinaturaDigital.SelectAllRowsOnPage(this.checked);" title="Select/Unselect all rows on the page" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="MAE_INSCRICAOID" VisibleIndex="0" Visible="false">
                        </dxwgv:GridViewDataTextColumn>                             
                        <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="2" Width="300px">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="3" Width="100px">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="RG" FieldName="NUMERORG" VisibleIndex="4" Width="100px">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="5" Width="50px">
                            <DataItemTemplate>
                                <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("MAE_INSCRICAOID") + "," + Eval("TIPOARQUIVO") %>'
                                    OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                    AlternateText="Visualizar Documento" Visible='<%# Eval("MAE_INSCRICAOID") != DBNull.Value && Eval("MAE_FORMULARIOBANCOARQUIVOID") != DBNull.Value %>'></asp:ImageButton>
                            </DataItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                </dxwgv:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td height="50">
                <label for="<%= txtQRCode.ClientID %>"><b>URL base para QRCode:</b></label><br />
                <asp:TextBox ID="txtQRCode" runat="server" Text="" Width="530px" />
            </td>
        </tr>
        <tr>
            <td height="50">
                <label for="<%= txtURLBusca.ClientID %>"><b>URL Raiz para tela de busca:</b></label><br />
                <asp:TextBox ID="txtURLBusca" runat="server" Text="" Width="530px" />
                <asp:Button ID="btnSalvarURLBusca" runat="server" Text="Salvar URL" Width="80px" OnClick="btnSalvarURLBusca_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnGerarPDF" runat="server" Text="Gerar PDF" Width="80px" OnClientClick="return downloadPDF();" />
            </td>
        </tr>
    </table>
    <br />
    
    <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Arquivos">
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
    
    <asp:ObjectDataSource ID="odsAssinaturaDigital" TypeName="Techne.Lyceum.Net.Cadastros.MaeAssinaturaDigital" runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script language="javascript">
    
        function createPDF(contador, rows, cert) {
            var id = rows[contador][0];
            var nome = rows[contador][1];
            var rg = rows[contador][2];
            var cpf = rows[contador][3];
            
            $.post("CreatePDF.ashx", { 
                "Certificado": cert.Cert,
                "MaeInscricaoId": id,
                "Nome": nome,
                "RG": rg,
                "CPF": cpf
            })
            .done((hash) => {
                SignerDigital.signPdfHash(hash.Hash, cert.CertThumbPrint, "SHA-256").then(hashAssinado => {
                    let formData = new FormData();
                    formData.append("MaeInscricaoId", id);
                    formData.append("Id", hash.Id);
                    formData.append("HashAssinado", hashAssinado);
                    //formData.append("HashAssinado", "");
                    getDataUrl("SignPDF.ashx", formData).then(dataurl => { 
                        if (++contador < rows.length) {
                            createPDF(contador, rows, cert);
                        } else {
                            $("#divTelaBloqueada").css("display", "none");
                            grdAssinaturaDigital.Refresh();
                        }
                    });
                }, erro => {
                    console.error(erro);
                    $("#divTelaBloqueada").css("display", "none");
                });
            });
        }
    
        function downloadPDF() {
            $("#divTelaBloqueada").css("display", "block");
            grdAssinaturaDigital.GetSelectedFieldValues("MAE_INSCRICAOID;NOME;CPF;NUMERORG", (rows) => { 
                SignerDigital.getSelectedCertificate().then(certificate => {
                    let cert = JSON.parse(certificate);
                    if (rows.length > 0) {
                        var contador = 0;
                        var row = rows[contador];
                        createPDF(contador, rows, cert);
                    }
                    else {
                        $("#divTelaBloqueada").css("display", "none");
                    }
                }, erro => {
                    console.error(erro);
                    $("#divTelaBloqueada").css("display", "none");
                });
            });
            return false;
        }
        
        async function getDataUrl(url, data) {
            let blob = await fetch(url, { 
                'method': 'POST',
                'body': data
            }).then(r => r.blob());
            let dataUrl = await new Promise(resolve => {
              let reader = new FileReader();
              reader.onload = () => resolve(reader.result);
              reader.readAsDataURL(blob);
            });
            return dataUrl;
        }
    </script>

</asp:Content>
