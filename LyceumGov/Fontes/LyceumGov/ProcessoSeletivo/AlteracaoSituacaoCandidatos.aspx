<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
	CodeBehind="AlteracaoSituacaoCandidatos.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.AlteracaoSituacaoCandidatos" %>

<asp:Content ID="conAlteracaoSituacaoCandidatos" ContentPlaceHolderID="cphFormulario"
	runat="server">
	<asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por processo seletivo e número de inscrição"
		Width="650px">
		<table width="100%" runat="server">
			<tr>
				<td align="right" style="width: 40%">
					<asp:Label ID="lblConcursoTSearch" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
				</td>
				<td style="width: 60%">
					<tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
						OnChanged="tseConcurso_Changed" MaxLength="20" SqlSelect="SELECT concurso, descricao from LY_CONCURSO_DOCENTE cd"
						ArgumentColumns="50" Columns="20" GridWidth="850px" SqlWhere="tipo = 'Contrato' and cd.DT_INICIO <= getdate() and cd.DT_FIM >= getdate()">
						<GridColumns>
							<tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="20%" />
							<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
						</GridColumns>
					</tweb:TSearchBox>
				</td>
			</tr>
			<tr>
				<td align="right" style="width: 40%">
					<asp:Label ID="lblInscricaoTSearch" runat="server" Text="Número de Inscrição:* "
						SkinID="lblObrigatorio"></asp:Label>
				</td>
				<td style="width: 60%">
					<tweb:TSearchBox ID="tseInscricao" runat="server" Key="candidato" Argument="nome"
						OnChanged="tseInscricao_Changed" MaxLength="20" SqlSelect="SELECT cd.candidato, cd.nome, cd.cpf, reg.regional FROM LY_CANDIDATO_DOCENTE cd LEFT JOIN TCE_REGIONAL reg ON CD.REGIONALID = REG.ID_REGIONAL"
						ArgumentColumns="50" Columns="20" GridWidth="850px" SqlWhere="cd.CONCURSO = #tseConcurso# AND CD.STATUS IN('1','2','5','21','22','26') ">
						<GridColumns>
							<tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
							<tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
							<tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="10%" />
							<tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="20%" />
						</GridColumns>
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
	<div class="divEditBlock" id="divTitulo" style="width: 900px;" visible="false" runat="server">
		<asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
		<asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancelar_Click" />
		<asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
			ValidationGroup="SalvarForm" />
		<asp:ValidationSummary ID="vsAlteracaoSituacaoCandidatos" runat="server" EnableClientScript="true"
			ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
		<asp:Label runat="server" ID="lblBloco" Text="Alteração da Situação de Candidatos"
			SkinID="BcTitulo" />
	</div>
	<dxtc:ASPxPageControl ID="pcAlteracaoSituacaoCandidatos" runat="server" ActiveTabIndex="1"
		Height="348px" Width="900px">
		<TabPages>
			<dxtc:TabPage Text="Dados do Candidato">
				<ContentCollection>
					<dxw:ContentControl ID="conDadosCandidato" runat="server">
						<asp:Panel ID="pnDados" GroupingText="Dados pessoais" runat="server">
							<table>
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
										<asp:TextBox ID="txtCelular" runat="server" Width="100px" ReadOnly="true" />
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
											Enabled="false" CalendarProperties-TodayButtonText="Hoje">
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
										<asp:TextBox ID="txtPISPASEP" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
									</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="pnlCarteiraProfissional" runat="server" GroupingText="Carteira Profissional">
							<table>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCProfNum" runat="server" Text="Número: "></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCProfNum" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblCProfSerie" runat="server" Text="Série: "></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCProfSerie" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td style="text-align: right">
										<asp:Label ID="lblCProfDtExp" runat="server" Text="Data de Expedição: "></asp:Label>
									</td>
									<td>
										<dxe:ASPxDateEdit ID="dteCProfDtExp" runat="server" MinDate="1901-01-01" Enabled="false">
											<CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
											</CalendarProperties>
										</dxe:ASPxDateEdit>
									</td>
									<td style="text-align: right">
										<asp:Label ID="lblCProfUF" runat="server" Text="Estado: "></asp:Label>
									</td>
									<td>
										<asp:TextBox ID="txtCProfUF" runat="server" Enabled="false" ReadOnly="true" MaxLength="2">
										</asp:TextBox>
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
						<asp:Panel ID="pnFolhaDePagamento" runat="server" GroupingText="">
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
														MaxLength="20" SqlSelect="select distinct LY_CONCURSO_DOC_HABILITACAO.NUCLEO as nucleo, descricao from LY_CONCURSO_DOC_HABILITACAO Join LY_NUCLEO On LY_CONCURSO_DOC_HABILITACAO.NUCLEO = LY_NUCLEO.NUCLEO"
														GridWidth="850px" SqlWhere="LY_CONCURSO_DOC_HABILITACAO.CONCURSO = #tseConcurso#">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="30%" />
															<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
														</GridColumns>
													</tweb:TSearchBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:Label ID="Label1" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
									</td>
									<td colspan="3">
										<tweb:TSearchBox runat="server" ID="tseMunicipioProc" SqlSelect="select DISTINCT m.CODIGO ,M.NOME from LY_UNIDADE_ENSINO UE inner join MUNICIPIO M ON UE.MUNICIPIO = M.CODIGO inner join LY_CONCURSO_DOC_HABILITACAO cdh on cdh.MUNICIPIO_PROC=m.CODIGO "
											Value='<%# Bind("MUNICIPIO_PROC") %>' SqlWhere="cdh.Concurso = #tseConcurso# AND cdh.nucleo= #tseCoordenadoria#"
											MaxLength="20" SqlOrder="NOME" DataType="Varchar">
											<GridColumns>
												<tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
												<tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="80%" />
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
									<td align="left">
										<table>
											<tr>
												<td colspan="3">
													<tweb:TSearchBox ID="tseDisciplina" runat="server" Key="agrupamento" Argument="descricao"
														MaxLength="50" SqlSelect="select DISTINCT C.agrupamento, descricao from LY_CONCURSO_DOC_HABILITACAO C Join LY_GRUPO_HABILITACAO G On C.AGRUPAMENTO = G.AGRUPAMENTO"
														GridWidth="850px" SqlWhere="C.CONCURSO = #tseConcurso# and C.NUCLEO = #tseCoordenadoria#">
														<GridColumns>
															<tweb:TSearchBoxColumn Caption="Disciplina" FieldName="agrupamento" Width="30%" />
															<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
														</GridColumns>
													</tweb:TSearchBox>
												</td>
											</tr>
										</table>
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
													<asp:DropDownList ID="ddlSituacao" runat="server" DataValueField="statusid" DataTextField="descricao"
														AutoPostBack="false" Width="150px">
													</asp:DropDownList>
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
		</TabPages>
	</dxtc:ASPxPageControl>
</asp:Content>
