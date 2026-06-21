<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master" CodeBehind="Teste.aspx.cs" Inherits="Techne.Lyceum.Net.Cadastros.Teste" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <script src="../Scripts/SignerDigital-1.0.0.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    
    <asp:ObjectDataSource ID="odsCertificado" runat="server" TypeName="Techne.Lyceum.Net.Cadastros.Teste" SelectMethod="Lista"></asp:ObjectDataSource>
    
    <dxwgv:ASPxGridView ID="grdCertificado" runat="server" AutoGenerateColumns="True" ClientInstanceName="grdCertificado" DataSourceID="odsCertificado" KeyFieldName="Id" Width="775px">
        <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
        <SettingsEditing Mode="Inline" />
    </dxwgv:ASPxGridView>
    
    <p>
        <asp:Button ID="btnGerarPDF" runat="server" Text="Gerar PDF" OnClientClick="return downloadPDF();" />
    </p>
    
    <script language="javascript">
    
        function createPDF(contador, cert, nome, rg, cpf) {
            
            $.ajax({
                url: "CreatePDF.ashx",
                contentType: "application/json: charset=utf-8",
                data: { 
                    "Certificado": cert.Cert,
                    "Nome": nome,
                    "RG": rg,
                    "CPF": cpf
                },
                success: function (hash) {
                    SignerDigital.signPdfHash(hash.Hash, cert.CertThumbPrint, "SHA-256").then(hashAssinado => {
                        getDataUrl("SignPDF.ashx?Id=" + hash.Id + "&HashAssinado=" + encodeURIComponent(hashAssinado)).then(dataurl => { 
                            console.log("=================== INICIO: " + nome + " ===================");
                            console.log(dataurl);
                            console.log("==================== FIM: " + nome + " =====================");
                            
                            if (++contador < grdCertificado.GetVisibleRowsOnPage())
                                grdCertificado.GetRowValues(contador, "Id;Nome;RG;CPF", (values) => createPDF(contador, cert, values[1], values[2], values[3]));
                        });
                    });
                },
                error: function (erro) {
                    console.log("===================== INI ERRO: " + nome + " ===================");
                    console.error(erro);
                    console.log("===================== FIM ERRO: " + nome + " ====================");
                }
            });

        }
    
        function downloadPDF() {
            SignerDigital.getSelectedCertificate().then(certificate => {
                let objCertificate = JSON.parse(certificate);
                if (grdCertificado.GetVisibleRowsOnPage() > 0) {
                    var contador = 0;
                    grdCertificado.GetRowValues(contador, "Id;Nome;RG;CPF", (values) => createPDF(contador, objCertificate, values[1], values[2], values[3]));
                }    
            });
            return false;
        }
        
        async function getDataUrl(url) {
            let blob = await fetch(url).then(r => r.blob());
            let dataUrl = await new Promise(resolve => {
              let reader = new FileReader();
              reader.onload = () => resolve(reader.result);
              reader.readAsDataURL(blob);
            });
            return dataUrl;
        }
    </script>
    
</asp:Content>