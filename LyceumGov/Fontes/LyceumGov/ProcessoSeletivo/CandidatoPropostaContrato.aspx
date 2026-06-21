<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="CandidatoPropostaContrato.aspx.cs"
    Inherits="Techne.Lyceum.Net.ProcessoSeletivo.CandidatoPropostaContrato" %>

<asp:Content ID="conCandidatoPropostaContrato" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <script type="text/javascript">
        function somenteNumeros(oEvent) {
            var keycode = (oEvent.which) ? oEvent.which : oEvent.keyCode;

            if ((keycode >= 48 && keycode <= 57) || (keycode == 8))
                return (true && (keycode != 46));

            return false;
        }

        function validarIdentidadeFuncional(obj) {
            var btnSalvar = document.getElementById("<%=btnSalvar.ClientID %>");

            if ((btnSalvar) && (obj.value.length > 0)) {
                if ((obj.value > 2147483647) || (obj.value <= 0)) {
                    alert("Número de Identidade Funcional inválido !");
                    obj.focus();
                    return false;
                }
            }

            return true;
        }       
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por processo seletivo e número de inscrição"
        Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcursoTSearch" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
                        OnChanged="tseConcurso_Changed" MaxLength="20" SqlSelect="SELECT concurso, descricao from LY_CONCURSO_DOCENTE cd"
                         SqlWhere="tipo = 'Contrato'" ArgumentColumns="50" Columns="30" GridWidth="850px">
                        <gridcolumns>
							<tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="20%" />
							<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
						</gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblInscricaoTSearch" runat="server" Text="Número de Inscrição:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseInscricao" runat="server" Key="candidato" Argument="nome"
                        OnChanged="tseInscricao_Changed" MaxLength="20" SqlSelect="SELECT CD.CANDIDATO, CD.NOME, CD.CPF, CDS.DESCRICAO AS SITUACAO, RG.REGIONAL, NC.NUCLEO FROM ly_candidato_docente CD INNER JOIN LY_CANDIDATO_DOCENTE_STATUS CDS WITH (NOLOCK) ON CDS.STATUSID = CD.STATUS LEFT JOIN TCE_REGIONAL RG WITH (NOLOCK) ON RG.ID_REGIONAL = CD.REGIONALID LEFT JOIN LY_NUCLEO NC WITH (NOLOCK) ON NC.NUCLEO = CD.NUCLEO "
                        ArgumentColumns="50" Columns="30" GridWidth="850px" SqlWhere="CDS.STATUSID IN ('2','24','25') AND CD.CONCURSO = #tseConcurso# "
                        SqlOrder="CD.NOME">
                        <gridcolumns>
							<tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="10%" />
							<tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
							<tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="10%" />
							<tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="15%" />
							<tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="15%" Visible="false" />
						</gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancelar_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:ValidationSummary ID="vsCandidatoContratoPorposta" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
        <asp:Label runat="server" ID="lblBloco" Text="Proposta de Contrato Temporário" SkinID="BcTitulo" />
    </div>
    <div visible="false">
        <asp:TextBox ID="txtPessoa" runat="server" MaxLength="100" ReadOnly="true" Visible="false" />
        <asp:TextBox ID="txtNumFunc" runat="server" MaxLength="100" ReadOnly="true" Visible="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcCandidatoPropostaContrato" runat="server" ActiveTabIndex="1"
        Height="348px" Width="900px">
        <tabpages>
			<dxtc:TabPage Text="Dados do Candidato">
				<ContentCollection>
					<dxw:ContentControl ID="conDadosCandidato" runat="server">
						<asp:Panel ID="pnDados" GroupingText="Dados pessoais" runat="server">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblMatricula" runat="server" Text="Matrícula:"  />
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtMatricula" runat="server" MaxLength="10" Width="150px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblIdentidadeFuncional" runat="server" Text="Identidade Funcional:*" SkinID="lblObrigatorio" />
									</td>
									<td >
										<asp:TextBox ID="txtIdentidadeFuncional" runat="server" MaxLength="8" Width="150px"
											ReadOnly="true" />
									</td>
									<td style="text-align: right">
										<asp:Label ID="Label2" runat="server" Text="Vínculo:*" SkinID="lblObrigatorio" />
									</td>
									<td >
										<asp:TextBox ID="txtVinculo" runat="server" MaxLength="2" Width="150px"
											ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblNome_Comp" runat="server" Text="Nome completo:"></asp:Label>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtNome_Comp" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblDataNasc" runat="server" Text="Data de nascimento:"></asp:Label>
									</td>
									<td>
										<dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="120px"
											ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje"
											ReadOnly="true">
											<CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
											</CalendarProperties>
										</dxe:ASPxDateEdit>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblSexo" runat="server" Text="Sexo:"></asp:Label>
									</td>
									<td style="height: 30px">
										<table>
											<tr>
												<td>
													<asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
														Width="150px" Enabled="false">
														<asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
														<asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
													</asp:RadioButtonList>
												</td>
											</tr>
										</table>
									</td>
									<td align="right">
										<asp:Label runat="server" ID="lblCorRaca" Text="Etnia: "></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlCorRaca" runat="server" Width="100px" Height="20px" DataTextField="nome"
											DataValueField="etniaid" Enabled="false">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblNomeMae" runat="server" Text="Nome mãe:"></asp:Label>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblNomePai" runat="server" Text="Nome pai:"></asp:Label>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblNecessidadeEspecial" runat="server" Text="Necessidade Especial: "></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlNecessidadeEspecial" runat="server" DataValueField="NECESSIDADEESPECIALID"
											DataTextField="DESCRICAO" Width="200px" Enabled="false">
										</asp:DropDownList>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil: "></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item"
											Enabled="false">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label runat="server" ID="lblPaisNasc" Text="País de Nascimento:"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlPaisNasc" runat="server" DataTextField="nome" DataValueField="codigo"
											Enabled="false">
										</asp:DropDownList>
									</td>
									<td style="text-align: right">
										<asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
											Enabled="false">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblNaturalidade" runat="server" Text="Naturalidade: "></asp:Label>
									</td>
									<td>
										<tweb:TSearchBox ID="tseNaturalidade" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
											Columns="10" ArgumentColumns="30" MaxLength="10">
											<GridColumns>
												<tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
												<tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
												<tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
											</GridColumns>
										</tweb:TSearchBox>
										<asp:TextBox ID="txtNaturalidadeNasc" runat="server" MaxLength="20" Width="250px"></asp:TextBox>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblNaturalidadeUF" runat="server" Text="Estado: "></asp:Label>
									</td>
									<td>
										<input id="txtNaturalidadeUF" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
									</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnEndereco" GroupingText="Endereço" runat="server">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblPais" runat="server" Text="País:"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlPaises" runat="server" DataTextField="nome" DataValueField="codigo"
											Width="250px" Enabled="false">
										</asp:DropDownList>
									</td>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCep" runat="server" MaxLength="8" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
											Text="Município:"></asp:Label>
									</td>
									<td>
										<tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
											GridWidth="600px" ArgumentColumns="30" Columns="10" MaximumValue="10">
											<GridColumns>
												<tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
												<tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
												<tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
											</GridColumns>
										</tweb:TSearchBox>
										<asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="250px" ReadOnly="true"></asp:TextBox>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
									</td>
									<td>
										<input id="txtEstado" runat="server" maxlength="20" class="txtInput" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
											Text="Endereço:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" Width="400px"
											ReadOnly="true"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
											Text="N.º:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" ReadOnly="true" />
									</td>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
											Text="Compl.:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtEndCompl" runat="server" MaxLength="50" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
											Text="Bairro:"></asp:Label>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtBairro" runat="server" MaxLength="50" Width="400px" ReadOnly="true" />
									</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnContato" GroupingText="Contato" runat="server">
							<table>
								<tr>
									<td style="text-align: right; width: 50px">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone:"></asp:Label>
									</td>
									<td style="width: 415px">
										<asp:TextBox ID="txtFone" runat="server" Width="100px" ReadOnly="true" />
									</td>
									<td style="text-align: right; width: 50px">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular" runat="server"
											Text="Celular:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCelular" runat="server" Width="100px" ReadOnly="true" onkeyup="formataCelularDDD(this,event)"  />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
											Text="E-mail:"></asp:Label>
									</td>
									<td colspan="3">
										<asp:TextBox ID="txtEmail" runat="server" Width="600px" MaxLength="100" ReadOnly="true" />
									</td>
								</tr>
							</table>
						</asp:Panel>
					</dxw:ContentControl>
				</ContentCollection>
			</dxtc:TabPage>
			<dxtc:TabPage Text="Documentos" Name="Documentos">
				<ContentCollection>
					<dxw:ContentControl ID="conDocumentos" runat="server">
						<asp:Panel ID="pnDocumento" GroupingText="Documento" runat="server">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="50"
											Width="150px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Tipo" runat="server"
											Text="Tipo:"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddlRGTipo" runat="server" DataValueField="item" DatatTextField="descr"
											Width="150px" Enabled="false">
										</asp:DropDownList>
									</td>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
											Text="Número:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtRGNum" runat="server" MaxLength="20" Width="200px" ReadOnly="true"
											SkinID="numeroDocumento" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
											Text="Estado:"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtRGUF" runat="server" Width="150px" Enabled="false" ReadOnly="true">
										</asp:TextBox>
									</td>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
											Text="Órgão Emissor:"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
											Width="200px" Enabled="false">
										</asp:DropDownList>
									</td>
									<td style="text-align: right">
										<asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
											Text="Data de Expedição:"></asp:Label>
									</td>
									<td>
										<dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
											Enabled="true" CalendarProperties-TodayButtonText="Hoje">
											<CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
											</CalendarProperties>
										</dxe:ASPxDateEdit>
									</td>
									
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnlOutrosDocumentos" runat="server" GroupingText="PIS/PASEP">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblPISPASEP" runat="server" Text="PIS/PASEP: "></asp:Label>
									</td>
									<td style="text-align: right">
										<asp:TextBox ID="txtPISPASEP" runat="server" MaxLength="11" SkinID="numerico" ></asp:TextBox>
									</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnlCarteiraProfissional" runat="server" GroupingText="Carteira Profissional">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCProfNum" runat="server" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCProfNum" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblCProfSerie" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCProfSerie" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCProfDtExp" runat="server" Text="Data de Expedição:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td>
										<dxe:ASPxDateEdit ID="dteCProfDtExp" runat="server" MinDate="1901-01-01" Enabled="false">
											<CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
											</CalendarProperties>
										</dxe:ASPxDateEdit>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblCProfUF" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td>
										<asp:DropDownList ID="ddDlCprof_Uf" runat="server" DataTextField="sigla" DataValueField="sigla"
											Width="120px" Height="20px">
										</asp:DropDownList>
									</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnlAcumulacao" runat="server" Font-Names="Verdana" GroupingText="Acumulação">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label19" runat="server" Text="Acumulação:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblAcumulacao" OnSelectedIndexChanged="rblAcumulacao_SelectedIndexChanged"
                                            AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Sim</asp:ListItem>
                                            <asp:ListItem Value="0">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label16" runat="server" Text="Matrícula:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMatriculaAcumulacao" runat="server" MaxLength="20" SkinID="numerico"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label17" runat="server" Text="Órgão:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrgaoAcumulacao" MaxLength="120" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label18" runat="server" Text="Nº de Processo:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumProcessoAcumulacao" MaxLength="25" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
					</dxw:ContentControl>
				</ContentCollection>
			</dxtc:TabPage>
			<dxtc:TabPage Text="Folha de pagamento" Name="Folha de pagamento">
				<ContentCollection>
					<dxw:ContentControl ID="conFolhaDePagamento" runat="server">
						<asp:Panel ID="pnFolhaDePagamento" runat="server" GroupingText="Folha de Pagamento">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCoordenadoria" runat="server" Text="Coordenadoria: "></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<tweb:TSearchBox ID="tseCoordenadoria" runat="server" Key="nucleo" Argument="descricao"
														MaxLength="20" SqlSelect="select distinct LY_CONCURSO_DOC_HABILITACAO.NUCLEO as nucleo, descricao 
                                                                   from LY_CONCURSO_DOC_HABILITACAO 
                                                                   Join LY_NUCLEO On LY_CONCURSO_DOC_HABILITACAO.NUCLEO = LY_NUCLEO.NUCLEO"
														GridWidth="850px" SqlWhere="LY_CONCURSO_DOC_HABILITACAO.CONCURSO = #tseConcurso#">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="30%" />
															<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
														</GridColumns>
													</tweb:TSearchBox>
													<asp:Table ID="tblRegional" runat="server" CellPadding="2" CellSpacing="0">
														<asp:TableRow>
															<asp:TableCell>
																<asp:TextBox ID="txtPolo" Width="118" runat="server" ReadOnly="true" />
															</asp:TableCell>
															<asp:TableCell>
																<asp:TextBox ID="txtDescricaoRegional" Width="364" runat="server" ReadOnly="true" />
															</asp:TableCell>
														</asp:TableRow>
													</asp:Table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:Label ID="Label1" runat="server" Text="Município:"></asp:Label>
									</td>
									<td colspan="3">
										<tweb:TSearchBox runat="server" ID="tseMunicipioProc" SqlSelect="select DISTINCT m.codigo, m.nome from MUNICIPIO m inner join LY_CANDIDATO_DOCENTE c ON c.MUNICIPIO_PROC = m.CODIGO "
											AutoPostBack="true" MaxLength="20" SqlOrder="nome" ArgumentColumns="30" Columns="10"
											DataType="VarChar">
											<GridColumns>
												<tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
												<tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
											</GridColumns>
										</tweb:TSearchBox>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblHabilitacao" runat="server" Text="Função: "></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<dxe:ASPxComboBox ID="cmbCargo" runat="server" Value='<%# Bind("categoria") %>' AutoPostBack="true"
														ValueType="System.String" Width="498px" Enabled="false">
													</dxe:ASPxComboBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblDisciplinaIngresso" runat="server" Text="Disciplina de Ingresso: "></asp:Label>
									</td>
									<td colspan="3">
										<tweb:TSearchBox ID="tseDisciplina" runat="server" GridWidth="850px" SqlSelect="SELECT GH.AGRUPAMENTO as agrupamento, GH.DESCRICAO as descricao FROM LY_GRUPO_HABILITACAO GH INNER JOIN LY_CANDIDATO_DOCENTE CD ON GH.AGRUPAMENTO = CD.AGRUPAMENTO_INGRESSO"
											Key="agrupamento" Argument="descricao" AutoPostBack="true" DataType="VarChar"
											ArgumentColumns="30" Columns="10">
											<GridColumns>
												<tweb:TSearchBoxColumn Caption="Disciplina" FieldName="agrupamento" Width="20%" />
												<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
											</GridColumns>
										</tweb:TSearchBox>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCota" runat="server" Text="Cota Convocação:  " />
									</td>
									<td align="left">
										<asp:TextBox ID="txtCota" runat="server" Width="142px" ReadOnly="true" />
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblSituacao" runat="server" Text="Situação:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td>
													<asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="false" Width="150px"
														Height="20px">
													</asp:DropDownList>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblUnidadeTSearch" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
														AutoPostBack="false" MaxLength="20" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
														GridWidth="850px">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
															<tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
															<tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
															<tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />															
															<tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
															<tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
														</GridColumns>
													</tweb:TSearchBox>
												</td>
												
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label runat="server" ID="lblCH" Text="Carga Horária:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="2">
													<asp:DropDownList ID="ddlCargaHoraria" runat="server" DataTextField="descr" DataValueField="descr"
														AutoPostBack="true" Height="20px" OnSelectedIndexChanged="ddlCargaHoraria_SelectedIndexChanged">
													</asp:DropDownList>												
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:Label runat="server" ID="lblCargo" Text="Cargo: " ></asp:Label>
									</td>
									<td colspan="2">
										<asp:UpdatePanel ID="updCarga" runat="server">
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="ddlCargaHoraria" EventName="SelectedIndexChanged" />
											</Triggers>
											<ContentTemplate>
												<asp:TextBox runat="server" ID="txtCargo" ReadOnly ="true"></asp:TextBox>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:Label ID="lblBanco" runat="server" Text="Banco:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<tweb:TSearchBox ID="tseBanco" runat="server" Key="banco" Argument="nome" SqlSelect="SELECT banco, nome from BANCOS (nolock)"
														SqlOrder="nome" AutoPostBack="true" OnChanged="tseBanco_Changed" MaxLength="20"
														GridWidth="850px" DataType="Number" SqlWhere=" banco ='237'">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
															<tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
														</GridColumns>
													</tweb:TSearchBox>
												</td>
											</tr>										
										</table>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:Label ID="lblAgencia" runat="server" Text="Agência:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<tweb:TSearchBox ID="tsAgencia" runat="server" Key="agencia" Argument="nome" OnChanged="tsAgencia_Changed"
														MaxLength="20" GridWidth="850px" SqlSelect="select agencia, banco, nome from AGENCIAS (nolock) "
														SqlWhere=" banco ='237'">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Agência" FieldName="agencia" Width="20%" />
															<tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
															<tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
														</GridColumns>
													</tweb:TSearchBox>
												</td>
												<td>
													<asp:RequiredFieldValidator ErrorMessage="Agência: Preenchimento obrigatório." ID="rfvAgencia"
														runat="server" ControlToValidate="tsAgencia" InitialValue="" ValidationGroup="SalvarForm"
														Visible="false" Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
													<asp:TextBox ID="txtBanco" runat="server" Visible="true" Width="0" BorderColor="White"
														ForeColor="White" BorderWidth="0"></asp:TextBox>
													<asp:TextBox ID="txtAgencia" runat="server" Visible="true" Width="0" BorderColor="White"
														ForeColor="White" BorderWidth="0"></asp:TextBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblConta" runat="server" Text="Conta:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td>
													<asp:TextBox ID="txtConta" runat="server" Width="100px" MaxLength="15"></asp:TextBox>
												</td>											
											</tr>
										</table>
									</td>
								</tr
								<tr>
									<td align="right">
										<asp:Label ID="lblDataAdmissao" runat="server" Text="Data de Admissão:* " SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td align="left">
										<table>
											<tr>
												<td>
													<dxe:ASPxDateEdit ID="dtAdmissao" runat="server" MinDate="1901-01-01" Width="120px"
														ClientInstanceName="dtAdmissao" CalendarProperties-ClearButtonText="Limpar">
														<CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje" >
														</CalendarProperties>
													</dxe:ASPxDateEdit>
												</td>
												
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</dxw:ContentControl>
				</ContentCollection>
			</dxtc:TabPage>
		</tabpages>
    </dxtc:ASPxPageControl>
</asp:Content>
