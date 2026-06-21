<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="Techne.Lyceum.Net.Menu.Config"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<%@ Register Src="SiteMapControl.ascx" TagName="SiteMapControl" TagPrefix="uc1" %>


<asp:Content ID="conAlunos" ContentPlaceHolderID="cphFormulario" runat="server">

<style>
    #modalFundo {
        display: none;
        position: fixed;
        top: 0; left: 0;
        width: 100%; height: 100%;
        background: rgba(0,0,0,0.6);
        z-index: 9999;
        animation: fadeIn 0.3s ease;
    }

    @keyframes fadeIn {
        from { opacity: 0; }
        to   { opacity: 1; }
    }

    #modalCaixa {
        background: white;
        width: 90%;
        max-width: 480px;
        margin: 120px auto;
        border-radius: 12px;
        box-shadow: 0 0 15px rgba(0,0,0,0.4);
        position: relative;
        overflow: hidden;
        animation: subir 0.3s ease;
    }

    @keyframes subir {
        from { transform: translateY(-30px); opacity: 0; }
        to   { transform: translateY(0); opacity: 1; }
    }

    #btnFechar {
        position: absolute;
        top: 8px;
        right: 12px;
        font-size: 22px;
        font-weight: bold;
        cursor: pointer;
        color: #444;
        transition: 0.2s;
    }

    #btnFechar:hover {
        color: #e60000;
        transform: scale(1.2);
    }

    #tituloModal {
        text-align: center;
        font-size: 22px;
        font-weight: bold;
        color: #1E74D9;        /* azul */
        margin-top: 20px;
        margin-bottom: 15px;
        font-family: Arial;
    }

    #modalConteudo {
        padding: 0 20px 20px 20px;
        text-align: center;
    }

    #modalConteudo img {
        width: 100%;
        max-width: 420px;
        border-radius: 8px;
    }
</style>

<script>
    function abrirModal() {
        document.getElementById("modalFundo").style.display = "block";
    }

    function fecharModal() {
        document.getElementById("modalFundo").style.display = "none";
    }


    window.onload = function() {

        document.getElementById("btnFechar").onclick = fecharModal;
        abrirModal();
    };
</script>

<!-- Modal -->
<div id="modalFundo">
    <div id="modalCaixa">
        <span id="btnFechar">×</span>


        <div id="tituloModal">Fique Ligado!!</div>

        <div id="modalConteudo">
            <img src="../Images/popup_segundachance.png" alt="Segunda Chance" />
        </div>
    </div>
</div>

    <uc1:SiteMapControl ID="SiteMapControl1" runat="server" />
</asp:Content>
