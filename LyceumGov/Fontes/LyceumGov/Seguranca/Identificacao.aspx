<%@ Page Language="c#" CodeBehind="Identificacao.aspx.cs" AutoEventWireup="True"
    MasterPageFile="~/Modulos/PublicMaster.Master" Inherits="Techne.Lyceum.Net.Seguranca.Identificacao" %>

<asp:Content ContentPlaceHolderID="cphFormulario" ID="Content1" runat="server">
    <asp:Panel ID="PanelLogin" runat="server" Width="100%">
        <div class="box-100">
            <!-- CARD DE LOGIN-->
            <div class="login">
                <!-- Titulo da area de Login -->
                <h3>
                    <img src="../Images/icone-card.png" alt="" width="50" height="50" align="absmiddle" />
                    <span class:"icone-card.png"></span>
                    Efetue
                    seu login no 
                    <br />
                    Conexão Educação:</h3>
                <!-- AREA dos campos de Login, Captcha e submit-->
                <div class="log_int">
                    <div class="row">
                        <!-- LABEL NOME DE USARIO -->
                        <label for="cUsuario">
                            Usuário:</label>
                        <asp:Label ID="Label1" TabIndex="-1" runat="server" CssClass="label-titulo" Font-Size="Larger"></asp:Label>
                        <!-- INPUT NOME DE USARIO -->
                        &#8203;<asp:TextBox ID="cUsuario" runat="server" CssClass="log_form"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ErrorMessage="*" ControlToValidate="cUsuario"></asp:RequiredFieldValidator>--%>
                        <!-- LABEL SENHA -->
                        <label>
                            Senha do Conexão Educação:</label>
                        <asp:Label ID="Label2" TabIndex="-1" runat="server"></asp:Label>
                        <!-- INPUT SENHA  -->
                        &#8203;<asp:TextBox ID="cSenha" runat="server" ToolTip="Senha" TextMode="Password"
                            CssClass="log_form" AutoComplete="Off"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvSenha" runat="server" ErrorMessage="*" ControlToValidate="cSenha"></asp:RequiredFieldValidator>--%>
                        <div class="input-enviar text-center">
                            <!-- IMAGEM DO CAPTCHA  -->
                            <asp:Image ID="imgChave" runat="server" ImageUrl="GeraChaveSeguranca.aspx" />
                            <!-- TROCAR CAPTCHA -->
                            <asp:LinkButton ID="btnAtualizaImagemCaptcha" Text="Trocar Imagem" runat="server" />
                            <!-- LABEL e CAMPO CHAVE DO CAPTCHA -->
                            <asp:Label ID="lblChave" runat="server" SkinID="lblObrigatorio" Text="Digite o código da imagem acima:"></asp:Label>
                            <asp:TextBox ID="txtChave" Visible="true" runat="server" SkinID="numerico" MaxLength="6"
                                Width="50"></asp:TextBox>
                            <!-- BOTÃO LOGIN SUBMIT-->
                            <asp:ImageButton ID="bt_Entrar" runat="server" ImageUrl="../Images/btn_EntrarBranco.png"
                                OnClick="bt_Entrar_Click"></asp:ImageButton>
                            <asp:LinkButton ID="lnkPedidoRedefinicaoSenha" runat="server" Text="Esqueci minha Senha"
                                OnClick="lnkPedidoRedefinicaoSenha_Click"></asp:LinkButton>
                            <asp:LinkButton ID="lnkPedidoUsuario" runat="server" Text="Esqueci meu Usuário" OnClick="lnkPedidoUsuario_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <p class="loginErroTexto">
                    <asp:Label ID="lblErroSenha" runat="server" Font-Bold="True" ForeColor="Red" CssClass="MsgError"></asp:Label>
                    <asp:Label ID="cMsg" TabIndex="-1" runat="server" Font-Bold="True" ForeColor="Red"
                        CssClass="MsgError"></asp:Label>
                </p>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelPedidoRedefinicaoSenha" runat="server" Width="100%" Visible="false">
        <div class="box-100">
            <div class="login" style="margin-top: 100px;">
                <h3>
                    &nbsp;<img src="../Images/icone-card.png" alt="" width="50" height="50" align="absmiddle" />Recupere a sua senha do <br />Conexão Educação</h3>
                <div class="log_int" id="divRedefinicaoSenha" runat="server">
                    <div class="row">
                        <label for="cUsuario">
                            Usuário:</label>
                        <asp:Label ID="Label3" TabIndex="-1" runat="server" CssClass="label-titulo" Font-Size="Larger"></asp:Label>
                        <asp:TextBox ID="cUsuarioRS" runat="server" CssClass="log_form"></asp:TextBox>
                        <div class="input-enviar text-center">
                            <!-- IMAGEM DO CAPTCHA  -->
                            <asp:Image ID="Image1" runat="server" ImageUrl="GeraChaveSeguranca.aspx" />
                            <!-- TROCAR CAPTCHA -->
                            <asp:LinkButton ID="LinkButton1" Text="Trocar Imagem" runat="server" />
                            <!-- LABEL e CAMPO CHAVE DO CAPTCHA -->
                            <asp:Label ID="Label5" runat="server" SkinID="lblObrigatorio" Text="Digite o código da imagem acima:"></asp:Label>
                            <asp:TextBox ID="txtChaveRS" Visible="true" runat="server" SkinID="numerico" MaxLength="6"
                                Width="50"></asp:TextBox>
                            <!-- BOTÃO LOGIN SUBMIT-->
                            <asp:ImageButton ID="bt_EntrarRS" runat="server" ImageUrl="../Images/btn_EntrarBranco.png"
                                OnClick="bt_EntrarRS_Click"></asp:ImageButton>
                        </div>
                    </div>
                </div>
                <p class="loginErroTextoRS">
                    <asp:Label ID="lblErroSenhaRS" runat="server" Font-Bold="True" ForeColor="Red" CssClass="MsgError"></asp:Label>
                    <asp:Label ID="cMsgRS" TabIndex="-1" runat="server" Font-Bold="True" ForeColor="Red"
                        CssClass="MsgError"></asp:Label>
                </p>
                <p style="text-align: center; width: 100%">
                    <asp:LinkButton ID="lnkRetornarLogin" runat="server" Text="Retornar para Login" OnClick="lnkRetornarLogin_Click"></asp:LinkButton>
                </p>
                <!-- Mensagem informativa -->
                <asp:panel runat= "server" ID="PnlAviso" visible = "false">
                <p style="text-align: center; width: 100%">
                    <asp:LinkButton ID="lnkEnviarNovamente" runat="server" Text="Enviar novamente" OnClick="lnkPedidoRedefinicaoSenha_Click" />
                </p>
                <p style="text-align: center; width: 100%; color: #FF0000; ForeColor="Red" font-size: 14px;">
                    Caso não tenha recebido a senha temporária, verifique a caixa de spam.
                    <br />
                    Se mesmo assim não encontrar, entre em contato através do site <a href="https://suporteti.educacao.rj.gov.br"
                        target="_blank">suporteti.educacao.rj.gov.br</a>
                </p>
                
                </asp:panel>
                
                
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelPedidoUsuario" runat="server" Width="100%" Visible="false">
        <div class="box-100">
            <div class="login" style="margin-top: 100px;">
                <h3>
                    &nbsp;<img src="../Images/icone-card.png" alt="" width="50" height="50" align="absmiddle" />Recupere o seu usuário do<br /> Conexão Educação</h3>
                <div class="log_int" id="divPedidoUsuario" runat="server">
                    <div class="row">
                        <label for="cUsuarioCPF">
                            CPF:</label>
                        <asp:Label ID="Label6" TabIndex="-1" runat="server" CssClass="label-titulo" Font-Size="Larger"></asp:Label>
                        <asp:TextBox ID="cUsuarioCPF" runat="server" CssClass="log_form"></asp:TextBox>
                        <div class="input-enviar text-center">
                            <!-- IMAGEM DO CAPTCHA  -->
                            <asp:Image ID="Image2" runat="server" ImageUrl="GeraChaveSeguranca.aspx" />
                            <!-- TROCAR CAPTCHA -->
                            <asp:LinkButton ID="LinkButton2" Text="Trocar Imagem" runat="server" />
                            <!-- LABEL e CAMPO CHAVE DO CAPTCHA -->
                            <asp:Label ID="Label7" runat="server" SkinID="lblObrigatorio" Text="Digite o código da imagem acima:"></asp:Label>
                            <asp:TextBox ID="txtChaveUSU" Visible="true" runat="server" SkinID="numerico" MaxLength="6"
                                Width="50"></asp:TextBox>
                            <!-- BOTÃO LOGIN SUBMIT-->
                            <asp:ImageButton ID="bt_EntrarUSU" runat="server" ImageUrl="../Images/btn_EntrarBranco.png"
                                OnClick="bt_EntrarUSU_Click"></asp:ImageButton>
                        </div>
                    </div>
                </div>
                <p class="loginErroTextoRS">
                    <asp:Label ID="lblErroSenhaUSU" runat="server" Font-Bold="True" ForeColor="Red" CssClass="MsgError"></asp:Label>
                    <asp:Label ID="cMsgUSU" TabIndex="-1" runat="server" Font-Bold="True" ForeColor="Red"
                        CssClass="MsgError"></asp:Label>
                </p>
                <p style="text-align: center; width: 100%">
                    <asp:LinkButton ID="LinkButton3" runat="server" Text="Retornar para Login" OnClick="lnkRetornarLogin_Click"></asp:LinkButton>
                </p>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelPassword" runat="server" Visible="False">
        <div class="resetSenha">
            <h3>
                &nbsp;<img src="../Images/sel.png" alt="" width="18" height="15" align="absmiddle" />&nbsp;Alteração
                de Senha:</h3>
            <div class="log_int">
                <strong>
                    <asp:Label ID="lbltitUsuario2" runat="server">Usu&aacute;rio:</asp:Label></strong><br />
                <asp:Label ID="lblUsuarioSenha" runat="server"></asp:Label><br>
                <br />
                <strong>
                    <asp:Label ID="lblSenhaAtual" runat="server">Senha Atual:</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaAtual" runat="server" TextMode="Password" CssClass="log_form"></asp:TextBox><br />
                <br />
                <strong>
                    <asp:Label ID="Label4" TabIndex="-1" runat="server">Senha Nova:</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaNova1" runat="server" ToolTip="Senha" TextMode="Password"
                    CssClass="log_form"></asp:TextBox><br />
                <br />
                <strong>
                    <asp:Label ID="lblSenhaNova2" runat="server">Senha Nova (Confirme):</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaNova2" runat="server" TextMode="Password" CssClass="log_form"></asp:TextBox><br />
                <asp:ImageButton ID="ib_ok" runat="server" ImageUrl="~/Images/btn_Confirmar.png"
                    OnClick="ib_ok_Click" CssClass="btn-confirmar"></asp:ImageButton>
                <asp:ImageButton ID="ib_cancelar" runat="server" ImageUrl="~/Images/btn_Cancelar.png"
                    OnClick="ib_cancelar_Click" CssClass="btn-Cancelar"></asp:ImageButton>
                <br />
                <br />
                <asp:Label ID="lblMsgAlteracao" runat="server" CssClass="azul1">A sua senha expirou. Digite uma nova senha</asp:Label>
                <br />
                <asp:Label ID="lblErroSenhaAlteracao" runat="server" Font-Bold="True" ForeColor="Red"
                    CssClass="MsgError"></asp:Label>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
