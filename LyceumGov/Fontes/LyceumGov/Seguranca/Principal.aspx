<%@ Page language="c#" Codebehind="Principal.aspx.cs" AutoEventWireup="True" inherits="Techne.Lyceum.Net.Seguranca.Principal" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../LyceumNet.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="fonline" method="post" runat="server">
			<P>&nbsp;</P>
			<P><asp:panel id="PanelLogin" runat="server" Width="98%">
					<TABLE style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(../Images/fnd_login.gif); WIDTH: 209px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 224px"
						cellSpacing="1" cellPadding="1" width="209" align="left" border="0">
						<TR>
							<TD align="center">
								<TABLE style="WIDTH: 184px; HEIGHT: 163px; BACKGROUND-COLOR: transparent" cellSpacing="1"
									cellPadding="1" width="184" align="center" border="0">
									<TR>
										<TD style="WIDTH: 45px; HEIGHT: 31px" colSpan="2">
											<P>
												<asp:label id="Label1" tabIndex="-1" runat="server" Width="54px" Font-Bold="True" Font-Names="Verdana"
													Font-Size="XX-Small" CssClass="azul1" Height="12px">Usuário:</asp:label></P>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 45px; HEIGHT: 18px" colSpan="2">
											<asp:textbox id="txtnumero_matricula" runat="server" Width="142px" Font-Names="Verdana" Font-Size="XX-Small"
												Height="22px"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 45px" colSpan="2">
											<P align="left">
												<asp:label id="Label2" tabIndex="-1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
													CssClass="azul1">Senha:</asp:label></P>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 45px" colSpan="2">
											<asp:textbox id="txtsenha" runat="server" Width="144px" Font-Names="Verdana" Font-Size="XX-Small"
												Height="21px" ToolTip="Senha" TextMode="Password"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 45px" align="right" colSpan="2">
											<P class="MsgError" align="right">&nbsp;</P>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 45px" colSpan="2">
											<P class="MsgError" align="right">&nbsp;</P>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 79px"></TD>
										<TD style="WIDTH: 45px">
											<P class="MsgError" align="right">&nbsp;</P>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 79px"></TD>
										<TD style="WIDTH: 45px">
											<P class="MsgError" align="center">
												<asp:imagebutton id="image1" runat="server" ImageUrl="../Images/Entrar.gif" onclick="bt_Entrar_Click"></asp:imagebutton></P>
										</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
					<TABLE id="Table1" style="WIDTH: 729px; HEIGHT: 238px" cellSpacing="1" cellPadding="1"
						width="729" border="0">
						<TR>
							<TD vAlign="top">
								<P style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif">Por 
									favor identifique-se para prosseguir.</P>
								<P class="MsgError" align="left">
									<asp:label id="cMsg" tabIndex="-1" runat="server" Font-Bold="True" CssClass="MsgError"></asp:label></P>
							</TD>
						</TR>
					</TABLE>
				</asp:panel></P>
			<asp:panel id="PanelPassword" runat="server" Visible="False">
				<TABLE style="WIDTH: 439px; HEIGHT: 8px" cellSpacing="1" cellPadding="1" width="439" align="center"
					border="0">
					<TR>
						<TD style="HEIGHT: 14px">
							<asp:Label id="lblMsgAlteracao" runat="server" CssClass="azul1">A sua senha expirou. Digite uma nova senha</asp:Label>.</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="lblErroSenha" runat="server" CssClass="MsgError"></asp:Label></TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
					<TR>
						<TD>
							<TABLE class="Griditem" style="BORDER-RIGHT: #000099 3px double; BORDER-TOP: #000099 3px double; BORDER-LEFT: #000099 3px double; BORDER-BOTTOM: #000099 3px double"
								cellSpacing="1" cellPadding="1" align="center" border="0">
								<TR>
									<TD>
										<TABLE cellSpacing="0" cellPadding="1" width="300" border="0">
											<TR>
												<TD align="right"></TD>
												<TD style="WIDTH: 163px"></TD>
											</TR>
											<TR>
												<TD align="right">
													<asp:Label id="lbltitUsuario2" runat="server">Usuário:</asp:Label></TD>
												<TD style="WIDTH: 163px">
													<asp:Label id="lblUsuarioSenha" runat="server"></asp:Label></TD>
											</TR>
											<TR>
												<TD style="HEIGHT: 2px" align="right"></TD>
												<TD style="WIDTH: 163px; HEIGHT: 2px"></TD>
											</TR>
											<TR>
												<TD align="right">
													<asp:Label id="lblSenhaAtual" runat="server">Senha Atual:</asp:Label></TD>
												<TD style="WIDTH: 163px">
													<asp:TextBox id="txtSenhaAtual" runat="server" TextMode="Password"></asp:TextBox></TD>
											</TR>
											<TR>
												<TD align="right">
													<asp:Label id="lblSenhaNova1" runat="server">Senha Nova:</asp:Label></TD>
												<TD style="WIDTH: 163px">
													<asp:TextBox id="txtSenhaNova1" runat="server" TextMode="Password"></asp:TextBox></TD>
											</TR>
											<TR>
												<TD align="right">
													<asp:Label id="lblSenhaNova2" runat="server">Senha Nova (Confirme):</asp:Label></TD>
												<TD style="WIDTH: 163px">
													<asp:TextBox id="txtSenhaNova2" runat="server" TextMode="Password"></asp:TextBox></TD>
											</TR>
											<TR>
												<TD>&nbsp;
												</TD>
												<TD style="WIDTH: 163px">&nbsp;
												</TD>
											</TR>
											<TR>
												<TD>&nbsp;</TD>
												<TD style="WIDTH: 163px">
													<asp:ImageButton id="ib_ok" runat="server" ImageUrl="../Images/Ok.gif" onclick="ib_ok_Click"></asp:ImageButton>
													<asp:ImageButton id="ib_cancelar" runat="server" ImageUrl="../Images/Cancel.gif" onclick="ib_cancelar_Click"></asp:ImageButton></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
