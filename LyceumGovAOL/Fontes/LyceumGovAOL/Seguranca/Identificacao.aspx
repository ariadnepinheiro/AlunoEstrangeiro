<%@ Page Language="c#" CodeBehind="Identificacao.aspx.cs" AutoEventWireup="True"
    MasterPageFile="~/Modulos/PublicMaster.Master" Inherits="Techne.Lyceum.Net.Seguranca.Identificacao" %>

<asp:Content ContentPlaceHolderID="cphFormulario" ID="Content1" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="../Scripts/Themes/jquery-ui.css">
    <script>
        $(function() {
        $("#<%= calendario.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
            });
        });
        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    
    <asp:Panel ID="PanelLogin" runat="server" Width="100%">
        <div class="login"> 
            <h3 style="padding: 10px;">
                <img src="../Images/icone-aluons-site-04.png" alt="" width="60" height="60" align="middle" />&nbsp;Efetue
                seu login no 
                <br>
                Aluno Online</h3>
                
                
            <div class="log_int">
                <div class="box-200">
                
                    <div class="flex-coluna">
                        <!-- LABEL ANO LETIVO -->
                        <asp:Label ID="lblAnoLetivo" runat="server" Font-Bold="True" Text="Ano letivo:"></asp:Label>
                   
                        <asp:DropDownList ID="cmbAnoLetivo" CssClass="select-login" runat="server" DataTextField="ano" DataValueField="ano"
                             AutoPostBack="false" Enabled="true">
                        </asp:DropDownList>
                    </div>
                    
                    <div class="flex-coluna">
                        <!-- LABEL PERIODO LETIVO -->
                        <asp:Label ID="lblPeriodoLetivo" runat="server" Font-Bold="True" Text="Periodo letivo:">
                            Periodo letivo:
                            <input type="checkbox" id="bt_duvida">
                            <label for="bt_duvida"><a class="icone-Modal" href="#abrirModal">Dúvida?</a></label>
                           <!-- <i class="far fa-question-circle"></i>-->
                        </asp:Label>
                   
                        <!-- INPUT PERIODO  -->
                        <asp:DropDownList ID="cmbPeriodoLetivo" CssClass="select-login" runat="server" AutoPostBack="false" Enabled="true">
                            <asp:ListItem Selected="True" Text="<Nenhum>" Value=""></asp:ListItem>
                            <asp:ListItem Text="Anual" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1o. semestre" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2o. semestre" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="flex-coluna">
                        <!-- LABEL MATRICULA -->
                        <asp:Label ID="lblUsuario" runat="server" Font-Bold="True" Text="Matrícula:"></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ErrorMessage="*Obrigatório." ControlToValidate="txtUsuario" />--%>
                  
                        <!-- INPUT -->
                        <asp:TextBox ID="txtUsuario" CssClass="inputs-login" runat="server" onkeypress="return OnlyNumericEntry(event)"
                            MaxLength="15"></asp:TextBox>
                    </div>
                    
                    <div class="flex-coluna"> 
                        <!-- LABEL  -->
                        <asp:Label ID="lblDataNasc" runat="server" Font-Bold="True" Text="Data de Nascimento:"></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="rfvDataNasc" runat="server" ErrorMessage="*Obrigatório." ControlToValidate="dtDataNasc" />--%>
                        <!-- input -->
                        <asp:TextBox ID="calendario" CssClass="inputs-login" runat="server" MaxLength="10" onkeyup="formataData(this,event)"></asp:TextBox>
                    </div>
                    
                    <div class="flex-coluna">
                        <td colspan="2" align="center">
                            <asp:Image ID="imgChave" runat="server" ImageUrl="GeraChaveSeguranca.aspx" />
                        </td>
                    </div>
                    
                    <div class="flex-coluna"> 
                        <td colspan="2" align="center">
                            <asp:LinkButton ID="btnAtualizaImagemCaptcha" Text="Trocar Imagem" runat="server"
                                TabIndex="-1" />
                        </td>
                    </div>
                    
                    <div class="flex-coluna">
                        <asp:Label ID="lblChave" runat="server" Font-Bold="True" Text="Digite o código da imagem acima:"></asp:Label>
                   
                        <asp:TextBox ID="txtChave" CssClass="inputs-login" Visible="true" runat="server" onkeypress="return OnlyNumericEntry(event)"
                            MaxLength="6"></asp:TextBox>
                    </div>
                    
                    <div class="flex-coluna">
                        <asp:ImageButton ID="btnEntrar" runat="server" ImageUrl="~/Images/btn_EntrarBranco.png"
                            OnClick="btnEntrar_Click"></asp:ImageButton>
                    </div>
                </div> <!-- FIM BOX-200-->    
                <div class="flex-coluna">
                        <asp:Label ID="lblMensagem" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                </div>           
            </div> <!-- FIM LOGIN_INT-->
            
            
            
            
            <!-- Modal Aqui -->
            <div id="abrirModal" class="modal">
                <div>
                      <!-- conteúdo do modal aqui -->
                      <a href="#fechar" title="Fechar" class="fechar">x</a>
                      <h3><%--&nbsp;<img src="../Images/sel.png" alt="" height="" align="middle" />&nbsp;--%>
                        Observações sobre Período Letivo:
                      </h3>
                      <p class="loginAvisoTexto"><b>Anual</b> - Refere-se a cursos com duração de quatro bimestres.</p>
                      <p class="loginAvisoTexto"><b>1º Semestre</b> - Refere-se a cursos com duração de dois bimestres.</p>
                      <p class="loginAvisoTexto"><b>2º Semestre</b> - Refere-se a cursos com duração de dois bimestres. </p>  
                </div> 
            </div><!-- FIM MODAL -->
            
        </div>
    </asp:Panel>
</asp:Content>
