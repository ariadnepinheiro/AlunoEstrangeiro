<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Techne.Lyceum.Net.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>SEEDUC - Governo do Rio de Janeiro</title>
    <link href="./site.css" type="text/css" rel="stylesheet" />
    <link href="./LyceumNet4.css" type="text/css" rel="stylesheet" />
    <link href="./Scripts/themes/basic/grid.css" rel="stylesheet" type="text/css" />
    <link href="./Scripts/themes/jqModal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="./Images/favicon.ico" type="image/x-icon" />
    
    <link rel='stylesheet' type='text/css' href="./Scripts/themes/calendar.css" />
    <script type='text/javascript' src='./Scripts/js/calendar.js'></script>
    
    <!-- google fonts e fontsawesome-->
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300;0,400;0,600;0,700;0,800;1,300;1,400;1,600;1,700;1,800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.css" integrity="sha512-3icgkoIO5qm2D4bGSUkPqeQ96LS8+ukJC7Eqhl1H5B2OJMEnFqLmNDxXVmtV/eq5M65tTDkUYS/Q0P4gvZv+yA==" crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/css/fontawesome.min.css" integrity="sha384-wESLQ85D6gbsF459vf1CiZ2+rr+CsxRY0RpiF1tLlQpDnAgg6rwdsUF1+Ics2bni" crossorigin="anonymous">
</head>

<body onload="moveRelogio();" approot='<%=Page.ResolveClientUrl("~/") %>'>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/jquery-1.7.1.min.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/jquery.jqGrid.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/js/jqModal.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/MaskTypes.js") %>'></script>
    
    
    
    <!-- HEADER - Barra superior -->
    <div class="top_inf2">
        <div class="top">
        
            <!-- Logo do Estado -->
            <div class="top_logo" align="center">
                <asp:Image ID="Image2" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                    ImageUrl="~/Images/logo-educacao.png" />
            </div>
            
            <!-- Icone menu responsivo -->
            <input type="checkbox" id="bt_menu">
            <label for="bt_menu">&#9776;</label>
            
            <div class="menu-responsivo">
                <!-- LOGO - CONEXÃO EDUCAÇÃO -->
                <div class="tit_sist">
                    <%--<asp:Image runat="server" ImageUrl="~/Images/logo_CEG.png" border="0" />--%> 
                    <asp:Image ID="Image3" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                    ImageUrl="~/Images/logo-processo-seletivo-azul.png" />
                </div>
                <!-- DATA HORA -->
                <div class="user_saud" align="right">
                    <asp:Literal ID="lblSaudacoes" runat="server" />
                    <div id="divHoraData">
                    </div>
                </div>
                 <!--Menu com as opções HOME -->
                <div class="tit_pag2">
                    <asp:HyperLink ID="hlInicio" runat="server"  CssClass="ocultar-asp"
                         ToolTip="Início">Inicio</asp:HyperLink>
                    <a class="icon-externo" href="https://www.seeduc.rj.gov.br/" target="_blank">Site Seeduc</a>     
                    <h2>&nbsp;</h2>                       
                </div>
            </div><!-- fim menu responsivo -->           
        </div>
    </div>
        
    <form id="form1" runat="server" submitdisabledcontrols="True">
       
    <!-- Miolo da página aqui !!! -->
    <div id="conteudo" class="conteudo">
        <div style="background: url('./Images/imagem.jpg');background-size: cover;height: calc(100vh - 155px);min-height: 457px;margin-top: 60px;">
               
                 
                 
            <table style="margin: 0 auto; height: 86%">
                <tr>
                    <th>
                       <div>
				    <a href="~/ProcessoSeletivo/CandidatoDocenteFicha.aspx" runat="server" class="link-circulo" style="text-decoration: none; color: #246da7">
					    <h3>Realizar <br />Inscrição</h3>					   
				    </a>
                    </th>
                     <th>
                        <div style="margin: 0 35px 0 35px;">
				    <a href="~/ProcessoSeletivo/CandidatoClassificacao.aspx" runat="server" class="link-circulo" style="text-decoration: none; color: #246da7">
					    <h3>Consultar Inscrição</h3>					   
				    </a>
			    </div>
                    </th>
                     <th>
                      <div>
				    <a href="~/ProcessoSeletivo/ReimpressaoFichaInscricao.aspx" runat="server" class="link-circulo" style="text-decoration: none; color: #246da7">
					    <h3>Reimprimir Comprovante <br />de Inscrição</h3>					    
				    </a>
			    </div>
                    </th>
                </tr>
            </table>
                 
               
                 
                 
                 
        </div>                   
    </div>
    
    
     <!--Rodapé -->
        <footer>
            <div class="container-footer">   
                <!-- Conexão e Redes sociais-->
                <div class="top-footer">
                   <!-- A Esquerda - Logo Conexão educação-->
                   
                   <div class="logo-conexao" style="margin-top: 10px;">
                       <%--<h2> Conexão Educação</h2>
                       <p>Gestão</p>--%>
                       <asp:Image ID="Image4" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                    ImageUrl="~/Images/logo-conexao.png" />
                   </div>
                   <!-- A Direita - Redes sociais -->
                   <div class="redes-sociais">
                      <div class="icones-redes">
                          <a href="https://www.facebook.com/seeducRJ" target="_blank"><i class="fab fa-facebook-f fa-2x"></i></a>
                          <a href="https://www.instagram.com/seeducrj/" target="_blank"><i class="fab fa-instagram fa-2x"></i></a>
                          <a href="https://www.youtube.com/channel/UCzhk54RUJtn1kwqU4LTNP-Q" target="_blank"><i class="fab fa-youtube fa-2x"></i></a>
                      </div>
                   </div>                
               </div>
               <!-- Copyright--> 
               <div class="copyright-footer">
                    <hr/>
                   &copy; Todos os direitos reservados SEEDUC - RJ -
                   <%= DateTime.Now.Year %>
                   | versão
                   <asp:Literal ID="lblVersao" runat="server" />
               </div>
            </div><!-- Fim do Rodapé -->
        </footer><!-- Fim do Rodapé -->
    
    
    </form>
</body>
</html>
