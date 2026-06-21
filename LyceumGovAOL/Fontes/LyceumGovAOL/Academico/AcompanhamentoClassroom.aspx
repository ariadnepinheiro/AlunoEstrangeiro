<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoClassroom.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AcompanhamentoClassroom" %>

<%@ Import Namespace="System.Linq" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    
    td{font-family:Tahoma, Geneva, sans-serif;font-size:11px;color:black;}
    h2{margin:3px 0px 0px 0px;font-family:Verdana, Geneva, sans-serif;font-size:11px;font-weight:bold;color:white;}
    .tabela-padrao table{border-collapse:collapse;}
    .tabela-padrao table td{border-collapse:collapse;border:1px solid #C0C0C0;padding:4px;}
    .tabela-padrao table thead tr{background-color:#BFD7F3;font-weight:bold; text-align:center;}
    .tabela-padrao table{width:100%;}
    .tabela-padrao thead td{border-color:#BFD7F3;}
    .Desbloqueado{display:none;background-image:url(http://localhost:52447/Imagens/loader.gif);}
    .cabecalho{position:fixed;top:0px;left:0px;width:100%;height:90px;background:white url('http://localhost:52447/Imagens/bk_top-inf.gif') repeat-x;z-index:10000;}
    .cabecalho .top{position:relative;height:90px;min-width:780px;background:url('http://localhost:52447/Imagens/bk_top.gif') no-repeat;}
    .cabecalho .ico_sair{border-style:none;border-color:inherit;border-width:0px;float:right;margin:1px 5px 1px 5px;background-image:url('http://localhost:52447/Imagens/bt_sair.png');background-repeat:no-repeat;width:60px;height:18px;}
    .cabecalho .ico_sair:hover{background-position:-60px;cursor:pointer;}
    .cabecalho .ico_inicio{border-style:none;border-color:inherit;border-width:0px;float:right;margin:1px 5px 1px 5px;background-image:url('http://localhost:52447/Imagens/ico_inicio.png');background-repeat:no-repeat;width:25px;height:18px;cursor:pointer;}
    .cabecalho .ico_inicio:hover{background-position:-25px;}
    .cabecalho .ico_info{border-style:none;border-color:inherit;border-width:0px;float:right;margin:1px 5px 1px 5px;background-repeat:no-repeat;width:25px;height:18px;cursor:pointer;background-image:url('http://localhost:52447/Imagens/ico_info.png');}
    .cabecalho .ico_info:hover{background-position:-25px;}
    .cabecalho .top_logo{position:absolute;left:11px;top:1px;width:175px;height:50px;}
    .cabecalho .tit_sist{position:absolute;top:5px;left:295px;height:20px;width:530px;}
    .cabecalho .botoes-menu-superior{position:absolute;top:35px;right:10px;height:20px;width:300px;}
    .cabecalho .tit_pag{position:absolute;left:230px;width:500px;top:35px;height:20px;}
    .cabecalho .saudacao{position:absolute;right:10px;top:6px;width:460px;height:15px;color:white;font-size:10px;font-weight:bold;}
    .cabecalho .menu-topo{position:absolute;top:60px;width:1150px;height:20px;display:inline;white-space:nowrap;margin:0px;left:70px;padding:0px;}
    .cabecalho .menu-topo li a{float:left;line-height:18px;height:20px;padding:0px 1px;background-image:url(http://localhost:52447/Imagens/menu_topo_els.gif);color:#FFF;text-decoration:none;}
    .cabecalho .menu-topo li a:hover{text-decoration:underline;}
    .cabecalho .menu-topo li .menu-aba-esquerda{width:10px;height:20px;display:inline-block;background-image:url(http://localhost:52447/Imagens/menu_topo_esqdir.png);background-repeat:no-repeat;background-position:left;float:left;}
    .cabecalho .menu-topo li .menu-aba-direita{width:10px;height:20px;display:inline-block;background-image:url(http://localhost:52447/Imagens/menu_topo_esqdir.png);background-repeat:no-repeat;background-position:right;float:left;}
    .cabecalho .menu-topo li{list-style:none;float:left;margin-right:2px;}
    .rodape{position:fixed;bottom:0px;left:0px;width:100%;height:45px;background:url('http://localhost:52447/Imagens/bk_bas-inf.gif') repeat-x;z-index:10000;}
    .rodape .bas{position:relative;min-width:640px;height:20px;background:url('http://localhost:52447/Imagens/bk_bas.gif') right no-repeat;padding:20px 140px 5px 20px;font-size:10px;color:white;text-transform:uppercase;}
    .logo-impressao{border-width:0px;}
    .topo-padrao{padding-top:15px;}
    .tabela-padrao{width:auto;margin:auto;margin-top:5em;}
    .tabela-padrao table{background: #ffffff;width:200px;border-radius: 5%;box-shadow: 0 2.8px 2.2px rgb(0 0 0 / 3%), 0 6.7px 5.3px rgb(0 0 0 / 5%), 0 12.5px 10px rgb(0 0 0 / 6%), 0 22.3px 17.9px rgb(0 0 0 / 7%), 0 41.8px 33.4px rgb(0 0 0 / 9%), 0 100px 80px rgb(0 0 0 / 12%);}  
    .tabela-padrao table thead tr:first-child{font-weight:bold;font-family:Calibri;font-size:12pt;background-color:#6495ed;text-align:center;}
    .tabela-padrao table thead tr:first-child td{color:White;}
    .tabela-padrao table tbody td:first-child{font-weight:bold;width:250px;}
    @media screen{
    .cabecalho-impressao{display:none;}
    }
    @media print{
    .top_inf{display:none;}
    .cabecalho{display:none;}
    .top{display:none;}
    .rodape{display:none;}
    .cabecalho-impressao{display:block;}
    .topo-padrao{page-break-inside:avoid;-webkit-region-break-inside:avoid;}
    }
    .titulo-tabela{font-weight:bold;font-size:14px;color:#2b6ab3;background: #ffffff;text-align: center;font-size:22px;padding: 6px;border-radius: 20px;box-shadow: 0 2.8px 2.2px rgb(0 0 0 / 3%), 0 6.7px 5.3px rgb(0 0 0 / 5%), 0 12.5px 10px rgb(0 0 0 / 6%), 0 22.3px 17.9px rgb(0 0 0 / 7%), 0 41.8px 33.4px rgb(0 0 0 / 9%), 0 100px 80px rgb(0 0 0 / 12%);}
    .tabela-dados-docente{width:600px!important;}
    .campo-email-titulo{width:50px!important;}
    .campo-email-valor{width:550px!important;}
    .tabela-turmas{width:300px!important;}
    .campo-unidadeescolar-titulo{width:150px!important;}
    .campo-turma-titulo{width:150px!important;}
    .campo-unidadeescolar-valor{width:150px!important;font-weight:normal!important;}
    .campo-turma-valor{width:150px!important;}
    .tabela-ultimosacessos{width:200px!important;}
    .campo-datalogin-titulo{width:150px!important;font-size:18px!important;}
    .campo-datalogin-valor{width:150px!important;font-weight:normal!important;font-size: 16px!important;}
    
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">    
    <br />
    <asp:Label ID="lblMensagem" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    <br />
    
    <div class="tabela-padrao">
        <div class="topo-padrao">

            <p class="titulo-tabela">Últimos Acessos</p>

            <table class="tabela-ultimosacessos">
                <thead>
                    <tr>
                        <td class="campo-datalogin-titulo">Data Login</td>
                    </tr>
                </thead>
                <tbody>
                    <% if (_acessos.Any()) { %>
                    <% foreach (var ultimoAcesso in _acessos) { %>
                    <tr>
                        <td align="center" class="campo-datalogin-valor"><%= ultimoAcesso.ToString("dd/MM/yyyy") %></td>
                    </tr>
                    <% } %>
                    <% } else { %>
                    <tr><td>Sem acessos registrados</td></tr>
                    <% } %>
                </tbody>      
            </table>
        </div>
    </div>
</asp:Content>
