<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ControleVagasContNova.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ControleVagasContNova" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#<%= this.txtVagasLiberadas.ClientID %>").numeric();
            $("#<%= this.txtVagasUtilizadas.ClientID %>").numeric(); 
            $("#<%= this.txtVagasDisponiveis.ClientID %>").numeric();        
            $("#<%= this.txtVagasContinuidade.ClientID %>").numeric();
            $("#<%= this.txtVagasNova.ClientID %>").numeric();

            $("#<%= this.txtVagasContinuidade.ClientID %>").keyup(function() {
                calcularVagasLiberada();
            });
            $("#<%= this.txtVagasNova.ClientID %>").keyup(function() {
                calcularVagasLiberada();
            });
            $("#<%= this.txtVagasLiberadas.ClientID %>").keyup(function() {
                calcularVagasDisponiveis();
            });

            calcularVagasDisponiveis();
        });

        function calcularVagasDisponiveis() {
            try {
                var vagasLiberadas = parseInt($("#<%= this.txtVagasLiberadas.ClientID %>").val());
                var vagasContinuidade = parseInt($("#<%= this.txtVagasContinuidade.ClientID %>").val());
                var vagasNova = parseInt($("#<%= this.txtVagasNova.ClientID %>").val());

                var vagasUtilizadas = parseInt($("#<%= this.txtVagasUtilizadas.ClientID %>").val());

           
                var vagasDisponiveis = vagasLiberadas - vagasUtilizadas;
//                var vagasDisponiveisContinuidade = vagasContinuidade - vagasUtilizadasContinuidade;
//                var vagasDisponiveisNovas = vagasNova - vagasUtilizadasNovas;

                if (vagasDisponiveis >= 0) {
                    $("#<%= this.txtVagasDisponiveis.ClientID %>").val(vagasDisponiveis);
                }
               
          
            } catch (e) {
                $("#<%= this.txtVagasDisponiveis.ClientID %>").val("");
             
            }
        }

        function calcularVagasLiberada() {
            try {

                var vagasContinuidade = $("#<%= this.txtVagasContinuidade.ClientID %>").val();
                var vagasNova = $("#<%= this.txtVagasNova.ClientID %>").val();
                

                var vagasUtilizadas = parseInt($("#<%= this.txtVagasUtilizadas.ClientID %>").val());
             
              

                if (vagasContinuidade != "")
                    vagasContinuidade = parseInt($("#<%= this.txtVagasContinuidade.ClientID %>").val());
                else
                    vagasContinuidade = 0;

                if (vagasNova != "")
                    vagasNova = parseInt($("#<%= this.txtVagasNova.ClientID %>").val());
                else
                    vagasNova = 0;


                var vagasLiberadas = vagasContinuidade + vagasNova;

                var vagasDisponiveis = vagasLiberadas - vagasUtilizadas;
            

                if (vagasLiberadas >= 0) {
                    $("#<%= this.txtVagasLiberadas.ClientID %>").val(vagasLiberadas);
                    $("#<%= this.txtVagasDisponiveis.ClientID %>").val(vagasDisponiveis);
                 
                }
                else {
                    $("#<%= this.txtVagasLiberadas.ClientID %>").val("");
                }
            } catch (e) {
                $("#<%= this.txtVagasLiberadas.ClientID %>").val("");
            }
        }

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function OnEndCallBack(s, e) {
            $("#<%= this.lblMensagem.ClientID %>").html("");
        }
    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
        Width="800px">
        <asp:HiddenField runat="server" ID="hdnIdControle" />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                        MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                        ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                        ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbAno_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="cmbAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbPeriodo_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cmbPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbModalidade" runat="server" DataTextField="descricao" DataValueField="modalidade"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbModalidade_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbNivel" runat="server" DataTextField="descricao" DataValueField="tipo"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbNivel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbEscolaridade" runat="server" AutoPostBack="True" DataTextField="nome"
                        DataValueField="curso" OnSelectedIndexChanged="cmbEscolaridade_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="Label1" runat="server" Text="Serie:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerie" AutoPostBack="True" runat="server" DataTextField="serie"
                        DataValueField="serie" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbTurno" AutoPostBack="True" runat="server" DataTextField="descricao"
                        DataValueField="turno" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbTurno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label2" runat="server" Text="Vagas Continuidade:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagasContinuidade" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label3" runat="server" Text="Vagas Nova:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagasNova" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblVagasLiberadas" runat="server" Text="Total de Vagas:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagasLiberadas" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblVagasUtilizadas" runat="server" Text="Total Vagas Utilizadas:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagasUtilizadas" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                </td>
            </tr>        
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblVagasDisponiveis" runat="server" Text="Saldo Vagas Disponíveis:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagasDisponiveis" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label5" runat="server" Text="Vagas Planejada 2ª Fase:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVagaPlanejada" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblParticipaMatriculaFacil" runat="server" Text="Participa Matrícula Fácil:*"  SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkParticipaMatriculaFacil" runat="server" />
                </td>
            </tr>
             <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblOfereceVagaFase1" runat="server" Text="Oferece Vagas 1ª Fase Matrícula Fácil:*"  SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkOfereceVagaFase1" runat="server" />
                </td>
            </tr>
             <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label4" runat="server" Text="Visualiza Consulta Vagas:*"  SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkVisualizaVagas" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdControle" runat="server" AutoGenerateColumns="False" Visible="False"
                        ClientInstanceName="grdControle" DataSourceID="odsControle" KeyFieldName="ID_CONTROLE_VAGA;CENSO;ANO;PERIODO;CURSO;TURNO"
                        OnCustomUnboundColumnData="grdControle_CustomUnboundColumnData" OnRowValidating="grdControle_RowValidating"
                        OnAfterPerformCallback="grdControle_AfterPerformCallback" OnHtmlDataCellPrepared="grdControle_HtmlDataCellPrepared">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <EditButton Text="Editar" Visible="True">
                                    <Image Url="~/img/bt_editar.png" />
                                </EditButton>
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <UpdateButton>
                                    <Image Url="~/img/bt_salvar.png" />
                                </UpdateButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONTROLE_VAGA" ReadOnly="true"
                                VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" ReadOnly="true" VisibleIndex="2"
                                Visible="false">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" ReadOnly="true"
                                VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" ReadOnly="true"
                                Visible="true" VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Segmento" FieldName="SEGMENTO" ReadOnly="true"
                                Visible="true" VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="NOME_CURSO" ReadOnly="true"
                                VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" VisibleIndex="7">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="NOME_TURNO" ReadOnly="true"
                                VisibleIndex="8">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataSpinEditColumn Caption="Vagas Continuidade" FieldName="VAGAS_CONTINUIDADE"
                                VisibleIndex="9" Width="70px">
                                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                    <ClientSideEvents Validation="function(s, e) {
	                                                                        var strVal=e.value;
	                                                                        var iVal=null;
	                                                                        e.isValid = true;
	                                                                        try
	                                                                        {
		                                                                        if(strVal!=null)
		                                                                        {
			                                                                        iVal=parseInt(strVal);
			                                                                        if (strVal != 0)
			                                                                        {
			                                                                             if(iVal&lt;1)
			                                                                             {
				                                                                             e.isValid = false;
				                                                                             e.errorText='Vagas de Continuidade deve ser um número';
			                                                                             }
			                                                                        }
		                                                                        }
	                                                                        }
	                                                                        catch(ex)
	                                                                        {
		                                                                        e.isValid = false;
		                                                                        e.errorText=ex;
	                                                                        }
                                                                        }" />
                                </PropertiesSpinEdit>
                            </dxwgv:GridViewDataSpinEditColumn>
                            <dxwgv:GridViewDataSpinEditColumn Caption="Vagas Nova" FieldName="VAGAS_NOVAS" VisibleIndex="10"
                                Width="70px">
                                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                    <ClientSideEvents Validation="function(s, e) {
	                                                                        var strVal=e.value;
	                                                                        var iVal=null;
	                                                                        e.isValid = true;
	                                                                        try
	                                                                        {
		                                                                        if(strVal!=null)
		                                                                        {
			                                                                        iVal=parseInt(strVal);
			                                                                        if (strVal != 0)
			                                                                        {
			                                                                             if(iVal&lt;1)
			                                                                             {
				                                                                             e.isValid = false;
				                                                                             e.errorText='Vagas Nova deve ser um número';
			                                                                             }
			                                                                        }
		                                                                        }
	                                                                        }
	                                                                        catch(ex)
	                                                                        {
		                                                                        e.isValid = false;
		                                                                        e.errorText=ex;
	                                                                        }
                                                                        }" />
                                </PropertiesSpinEdit>
                            </dxwgv:GridViewDataSpinEditColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Total de Vagas " FieldName="VAGAS_LIBERADAS"
                                ReadOnly="true" VisibleIndex="11">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>      
                            <dxwgv:GridViewDataTextColumn Caption="Total Vagas Utilizadas" FieldName="VAGAS_UTILIZADAS"
                                ReadOnly="true" VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>     
                            <dxwgv:GridViewDataTextColumn Caption="Saldo Vagas Disponíveis" UnboundExpression="" Name="VAGAS_DISPONIVEIS"
                                FieldName="VAGAS_DISPONIVEIS" UnboundType="Integer" ReadOnly="true" VisibleIndex="13">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                              <dxwgv:GridViewDataTextColumn Caption="Vaga planejada 2ª Fase" FieldName="VAGAPLANEJADA"
                                ReadOnly="true" VisibleIndex="14">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn><dxwgv:GridViewDataTextColumn Caption="Inscritos MF" FieldName="TOTALALUNO"
                                ReadOnly="true" VisibleIndex="15">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Fila de espera" FieldName="FILAESPERA"
                                ReadOnly="true" VisibleIndex="16">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>                            
                            <dxwgv:GridViewDataCheckColumn Caption="Participa Matrícula Fácil" FieldName="PARTICIPAMATRICULAFACIL" 
								VisibleIndex="17" Width="120px">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Oferece Vaga 1ª Fase" FieldName="OFERECEVAGAFASE1" VisibleIndex="18"
                                Width="120px">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Matrícula Fácil Paralisada" FieldName="PARALISAMATRICULAFACIL" 
								VisibleIndex="19" Width="120px" ReadOnly="true">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Visualiza Consulta Vagas" FieldName="VISUALIZAVAGA" VisibleIndex="20"
                                Width="120px">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Cadastro" FieldName="DT_CADASTRO" ReadOnly="true"
                                VisibleIndex="21">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Alteração" FieldName="DT_ALTERACAO" ReadOnly="true"
                                VisibleIndex="22">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário Responsável" FieldName="MATRICULA"
                                ReadOnly="true" VisibleIndex="23">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="ANO" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Periodo" FieldName="PERIODO" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsControle" TypeName="Techne.Lyceum.Net.Academico.ControleVagasContNova"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" OnUpdating="odsControle_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="cmbAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
