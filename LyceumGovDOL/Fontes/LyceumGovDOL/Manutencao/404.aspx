<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Techne.Lyceum.Net.Manutencao._04"
    MasterPageFile="~/Modulos/PublicMaster.Master" %>

<asp:Content ID="conErro" ContentPlaceHolderID="cphFormulario" runat="server">
    <h3>
        Erro de Acesso:</h3>
    <p>
        <img src="../images/img_interrog.png" width="128" height="128" align="left" /><strong
            style="text-decoration: underline;">Aconteceu o Seguinte Erro de Navegabilidade:</strong></p>
    <p>
        A página não existe, ou não está disponível no momento.<br />
        Por favor, verifique o erro na <strong>Lista de Ajuda </strong>ou entre em contato
        com o <strong>Suporte Técnico ou Adminstrador do Sistema</strong>.</p>
    <p>
        <b><a href="javascript:history.back(-1);">Clique aqui para voltar</a></b></p>
    <p>
        Atenciosamente,
        <br />
        <strong>Conexão Educação Gestão.</strong></p>
    <p>
    </p>
</asp:Content>
