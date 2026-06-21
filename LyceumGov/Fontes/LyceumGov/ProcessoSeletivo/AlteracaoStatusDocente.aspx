<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="AlteracaoStatusDocente.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.AlteracaoStatusDocente" %>

<asp:Content ID="conAlteracaoSituacaoCandidatos" ContentPlaceHolderID="cphFormulario"
	runat="server">
	<script>
    document.addEventListener("DOMContentLoaded", function () {
        const input = document.getElementById("<%= tseCandidato.ClientID %>");
        if (input) {
            input.addEventListener("keydown", function (e) {
                e.preventDefault();
                });
            }
        });
    </script>

	<asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por processo seletivo e número de inscrição ou ID Vinculo"
		Width="651px">
		<asp:ObjectDataSource 
            ID="odsAno" 
            TypeName="Techne.Lyceum.RN.PeriodoLetivo" 
            SelectMethod="ConsultarProximosAnos"
            runat="server" />
    
		<table id="Table1" width="100%" runat="server">
			<tr>
				<td align="right" style="width: 20%; white-space: nowrap;">
					<asp:Label ID="lblConcursoTSearch" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
				</td>
				<td style="width: 80%">
					<tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
						OnChanged="tseConcurso_Changed" MaxLength="20" SqlSelect="SELECT concurso, descricao from LY_CONCURSO_DOCENTE cd"
						ArgumentColumns="59" Columns="16" GridWidth="850px" SqlWhere="tipo = 'Migracao'">
						<GridColumns>
							<tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="20%" />
							<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
						</GridColumns>
					</tweb:TSearchBox>
				</td>
			</tr>
			<tr>
				<td align="right" style="width: 20%; white-space: nowrap;">
					<asp:Label ID="lblInscricaoTSearch" runat="server" Text="Número de Inscrição: "
						SkinID="lblObrigatorio"></asp:Label>
				</td>
				<td style="width: 80%">
					<tweb:TSearchBox ID="tseCandidato" Enabled="false" runat="server" Caption="" Key="candidato"
                        Argument="nome" SqlSelect="SELECT concurso,DESCRICAOSITUACAO, IDVINCULO FROM vw_ly_docente_candidato"
                        ArgumentColumns="59" Columns="16" GridWidth="850px" MaxLength="20" SqlWhere="concurso = #tseConcurso# AND SITUACAO <> (1)"
                        SqlOrder="nome" OnChanged="tseCandidato_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Id/Vínculo" FieldName="IDVINCULO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="DESCRICAOSITUACAO" Width="20%" />
                            
                        </GridColumns>
                    </tweb:TSearchBox>
				</td>
			</tr>
			
			 <tr>
                <td align="right">
                    <asp:Label ID="lblAno" Text="Ano Letivo:" runat="server" SkinID="lblObrigatorio" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" AutoPostBack="true" DataSourceID="odsAno" DataTextField="ano"
                        DataValueField="ano" runat="server" />
                </td>
            </tr>
		</table>
	</asp:Panel>
	<br />
	<asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
	<br />
	<asp:Panel ID="pnGrupo" runat="server" Visible=false>
	    <asp:Panel ID="Panel1" runat="server" GroupingText="Dados Atuais do Docente"
		    Width="650px">
		    <table id="Table2" width="100%" runat="server">
		        <tr>
				    <td align="right" style="width:19%;">
					    <asp:Label ID="Label1" runat="server" Text="ID/Vínculo "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtIdvinculo" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    
			    <tr id="trDataApresentacao">
				    <td align="right" style="width:19%;">
					    <asp:Label ID="lblDataApresentacao" runat="server" Text="Data de Apresentação "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtDataApresentacao" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    <tr id="trHoraApresentacao">
				    <td align="right">
					    <asp:Label ID="lblHoraApresentacao" runat="server" Text="Hora de Apresentação "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtHoraApresentacao" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    <tr id="trPontuacao">
				    <td align="right">
					    <asp:Label ID="lblPontuacao" runat="server" Text="Pontuação "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtPontuacao" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    <tr id="trCargo">
				    <td align="right" style="width:19%;">
					    <asp:Label ID="lblCargo" runat="server" Text="Cargo "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtCargo" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    <tr id="trSituacao">
				    <td align="right">
					    <asp:Label ID="lblSituacao" runat="server" Text="Situação "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtSituacao" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>  
			    <tr id="trChRegencia">
				    <td align="right">
					    <asp:Label ID="lblChRegencia" runat="server" Text="CH de regência"
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtChRegencia" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    <tr id="trChNormal">
				    <td align="right">
					    <asp:Label ID="lblChNormal" runat="server" Text="CH Normal"
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtChNormal" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    
			    <tr id="trChGLP">
				    <td align="right">
					    <asp:Label ID="lblChGlp" runat="server" Text="CH GLP"
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtChGlp" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
			    
			    <tr id="trFuncao">
				    <td align="right">
					    <asp:Label ID="lblFuncao" runat="server" Text="Função "
						    SkinID="lblObrigatorio"></asp:Label>
				    </td>
				    <td>
                        <asp:TextBox ID="txtFuncao" runat="server" MaxLength="100" Width="150px" ReadOnly="true" />
				    </td>
			    </tr>
		    </table>
	    </asp:Panel>
    	
	    <asp:Panel ID="Panel2" runat="server" GroupingText="Alteração Status do Docente"
		    Width="650px">
		    <table id="Table3" width="100%" runat="server">
                <tr id="trCargoAlterar">
                    <td style="text-align: right; width:15%;">
                        <asp:Label ID="lblCategoria" runat="server" Text="Cargo:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseCategoria" AutoPostBack="true" runat="server" Key="categoria"
                            Argument="nome" Caption="" MaxLength="20" GridWidth="850px" Columns="20" ArgumentColumns="60"
                            SqlSelect="SELECT CATEGORIA, NOME FROM LY_CATEGORIA_DOCENTE"
                            SqlOrder="nome"
                            SqlWhere="CATEGORIA IN ('PROF DOC I C REF 3','PROF DOC I C REF 4','PROF DOC I C REF 5','PROF DOC I C REF 6','PROF DOC I C REF 7','PROF DOC I C REF 8','PROF DOC I D REF 4','PROF DOC I D REF 5','PROF DOC I D REF 6','PROF DOC I D REF 7','PROF DOC I D REF 8','PROF DOC I D REF 9')">
                            
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="categoria" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Cargo" FieldName="nome" Width="30%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                     </td>
                </tr>
                <tr id="trFuncaoAlterar">
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="Label22" runat="server" Font-Names="Verdana" Text="Função:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseFuncaoLotacao" runat="server" Argument="descricao" ArgumentColumns="70"
                            FollowContainerMode="false" MaxLength="20" AutoPostBack="true" Columns="10" DataType="VarChar"
                            Key="funcao" SqlOrder="descricao" SqlSelect="SELECT DISTINCT F.funcao, F.descricao FROM Ly_funcao F ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                
                <tr id="trObservacao">
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblObservacao" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio" Text="Observação: "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtObservacao" runat="server" 
                            TextMode="MultiLine" 
                            Rows="4" 
                            Width="97%" 
                            MaxLength="500">
                        </asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lbl" runat="server" Font-Names="Verdana" Text="Situação: *" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSituacao" runat="server" DataValueField="statusid" DataTextField="descricao"
							AutoPostBack="false" Width="150px">
						</asp:DropDownList>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="4" style="text-align: right;">
                       <asp:Button ID="btnAlterar" runat="server" Text="Alterar Status" OnClick="btnAlterar_Click" />
                    </td>
                </tr>
                
		    </table>
	    </asp:Panel>
	</asp:Panel>
	
	    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" EnableViewState="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" ShowPageScrollbarWhenModal="false"
        Width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirmar">
        <HeaderStyle HorizontalAlign="Center" />
        <ClientSideEvents Init="OnInitASPxPopupControl" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr runat="server" id="trMsgPopup">
                        <td>
                            <asp:Label ID="lblMsgPopup" runat="server" style="font-size:.9rem" Text="Tem certeza que deseja executar essa ação?"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click"/>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
