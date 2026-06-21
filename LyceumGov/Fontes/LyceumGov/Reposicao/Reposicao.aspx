<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Reposicao.aspx.cs" Inherits="Techne.Lyceum.Net.Reposicao.Reposicao" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarReposicao.Show();
            }, 1000);
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function endRequest(sender, e) {
            if (e.get_error()) {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = e.get_error().description.replace(e.get_error().name + ": ", "");
                e.set_errorHandled(true);
            }
            else {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = "";
            }
        }

        function OnlyNumericEntry(e) {
            alert('ok');
            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        
        

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar"
        Width="70%">
        <div>
            <table width="60%">
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Período Referência:*"
                            SkinID="lblObrigatorio" Font-Bold="true">                                   
                        </asp:Label>
                    </td>
                    <td width="20%" colspan="3">
                        <asp:DropDownList Height="20px" ID="ddlPeriodoReferencia" runat="server" DataTextField="DESCRICAO"
                            AutoPostBack="true" DataValueField="PERIODOLANCAMENTOID" Width="200px" OnSelectedIndexChanged="ddlPeriodoReferencia_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label Font-Names="Verdana" ID="lblDataGreve" SkinID="lblObrigatorio" runat="server"
                            Text="Data da Greve:*"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtDataGreve" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                            AutoPostBack="true" ClientInstanceName="dtDataGreve" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje" OnDateChanged="dtDataGreve_DateChanged">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td>
                        <asp:Label ID="lblDiaSemana" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"
                            Font-Bold="true">                                   
                        </asp:Label>
                    </td>
                    <td width="20%">
                        <asp:DropDownList Height="20px" ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                            DataTextField="ano" DataValueField="ano" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                            Width="70px" AppendDataBoundItems="true">
                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="descricao" ArgumentColumns="50"
                            DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                            OnChanged="tseRegional_Changed" Key="id_regional" SqlSelect="select distinct ID_REGIONAL, descricao from (select distinct ue.ID_REGIONAL, n.regional as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                join TCE_REGIONAL n on n.ID_REGIONAL = ue.ID_REGIONAL) as tabela" SqlOrder="descricao, id_regional">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere=" id_regional = #tseRegional# " GridWidth="600px" ArgumentColumns="50"
                            OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
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
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td width="20%" colspan="3">
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual, bairro, ua_antiga    from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                            OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="Bairro" FieldName="bairro" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="id_regional" FieldName="id_regional" Width="18%"
                                    Visible="false" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="Label3" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlTurma" runat="server" AutoPostBack="true" Width="115px"
                            DataValueField="DADOSTURMA" DataTextField="TURMA" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                     <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblNomeTurno" runat="server"  Text="Turno:  "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTurno" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblNomeCurso" runat="server" Text="Curso: "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCurso" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>                   
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <table>
        <tr>
            <td align="left" colspan="4">
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hdnTempoPendente" runat="server" />
    <asp:Panel runat="server" ID="pnlProfessores" GroupingText="Professores licenciados - Greve"
        Width="70%">
        <asp:ObjectDataSource ID="odsProfessores" runat="server" TypeName="Techne.Lyceum.Net.Reposicao.Reposicao"
            SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlPeriodo" Name="periodo" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlPeriodoReferencia" Name="periodoReferencia" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlTurma" Name="turma" PropertyName="Text" />
                <asp:ControlParameter ControlID="dtDataGreve" Name="dataGreve" PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdProfessor" runat="server" KeyFieldName="NUM_FUNC;TURMA;DISCIPLINA;DATAAULA"
            DataSourceID="odsProfessores" ClientInstanceName="grdProfessor" AutoGenerateColumns="False"
            OnFocusedRowChanged="grdProfessor_FocusedRowChanged" Width="70%" EnableRowsCache="False"
            EnableViewState="False" EnableCallBacks="false" OnHtmlDataCellPrepared="grdProfessor_HtmlDataCellPrepared"
            OnHtmlRowCreated="grdProfessor_HtmlRowCreated">
            <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <ClientSideEvents Init="function(s) {if(s.cpUpdateError) s.ShowError(s.cpUpdateError);}" />
            <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            <Columns>
                <dxwgv:GridViewDataTextColumn FieldName="NUM_FUNC" VisibleIndex="1" Caption="NUM_FUNC"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="MATRICULA" VisibleIndex="1" Caption="Matrícula"
                    CellStyle-HorizontalAlign="Center" Width="30">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="IDVINCULO" VisibleIndex="1" Caption="ID Vínculo"
                    CellStyle-HorizontalAlign="Center" Width="40px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2"
                    CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center" Width="200px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TURMA" VisibleIndex="3" Caption="Turma"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DISCIPLINA" VisibleIndex="3" Caption="DISCIPLINA"
                    Visible="false" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="NOMEDISCIPLINA" VisibleIndex="5" Caption="Disciplina"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TIPO_AULA" VisibleIndex="6" Caption="Tipo"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAAULA" VisibleIndex="7" Caption="Data Aula"
                    CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TEMPOS" VisibleIndex="8" Caption="Tempos Total"
                    CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn FieldName="PENDENTES" VisibleIndex="9" Caption="Tempos Pendentes"
                    CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="SITUACAO" VisibleIndex="10" Caption="Situação"
                    CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <br />
    <asp:Label ID="lblProfessor" runat="server" Font-Size="Medium" ForeColor="red"></asp:Label>
    <br />
    <asp:Label ID="lblMsgInformativa" runat="server" Font-Size="Medium" ForeColor="red"></asp:Label>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnlReposicao" GroupingText="Reposição de Carga Horária - Greve"
        Width="70%">
        <br />
        <asp:Panel ID="pnlNovaReposicao" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="CH Reposição PENDENTE:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCHPendente" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Recusado" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkRecusado" AutoPostBack="true" OnCheckedChanged="chkRecusado_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="CH Reposição:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHReposicao" MaxLength="3" runat="server" onkeypress="return OnlyNumericEntry(event)"
                            SkinID="numerico"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataReposicao" runat="server" Text="Data Reposição:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit runat="server" ID="dtDataReposicao" Width="110px" OnDateChanged="DATAREPOSICAO_DateChanged"
                            AutoPostBack="true">
                        </dxe:ASPxDateEdit>
                    </td>
                    <td>
                        <asp:Label ID="lblDiaSemanaReposicao" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                            OnClick="btnSalvar_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:HiddenField ID="hdnNumFunc" runat="server" />
        <asp:HiddenField ID="hdnTipoAula" runat="server" />
        <asp:HiddenField ID="hdnDisciplina" runat="server" />
        <asp:HiddenField ID="hdnSituacao" runat="server" />
        <asp:HiddenField ID="hdnIdReposicao" runat="server" />
        <asp:ObjectDataSource ID="odsReposicao" runat="server" TypeName="Techne.Lyceum.Net.Reposicao.Reposicao"
            SelectMethod="ListaReposicao" InsertMethod="InsertReposicao" UpdateMethod="Update">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlPeriodo" Name="periodo" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlPeriodoReferencia" Name="periodoReferencia" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlTurma" Name="turma" PropertyName="Text" />
                <asp:ControlParameter ControlID="dtDataGreve" Name="dataGreve" PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdReposicao" runat="server" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="grdReposicao" DataSourceID="odsReposicao" KeyFieldName="REPOSICAOID"
            EnableCallBacks="false" OnCustomButtonCallback="grdReposicao_CustomButtonCallback">
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="30px">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center" id="dvteste">
                            <input type="image" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                        </div>
                    </HeaderCaptionTemplate>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarReposicao" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirReposicao"
                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png" Image-Height="15px"
                            Image-AlternateText="Excluir">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                    <HeaderStyle HorizontalAlign="Center" />
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="REPOSICAOID" ReadOnly="true"
                    VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="NUM_FUNC" VisibleIndex="3" Caption="NUM_FUNC"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="MATRICULA" VisibleIndex="1" Caption="Matrícula"
                    CellStyle-HorizontalAlign="Center" Width="30px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="IDVINCULO" VisibleIndex="1" Caption="ID Vínculo"
                    CellStyle-HorizontalAlign="Center" Width="40px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2"
                    CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center" Width="200px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TURMA" VisibleIndex="3" Caption="Turma"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DISCIPLINA" VisibleIndex="3" Caption="DISCIPLINA"
                    Visible="false" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TIPO_AULA" VisibleIndex="4" Caption="Tipo"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="NOMEDISCIPLINA" VisibleIndex="5" Caption="Disciplina"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DATAREPOSICAO" VisibleIndex="6"
                    Width="110px" CellStyle-HorizontalAlign="Center">
                    <PropertiesDateEdit Width="110px" EditFormat="Date">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn FieldName="CHEXIBICAO" VisibleIndex="5" Caption="CH Reposição"
                    Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="CH Reposição" FieldName="CHREPOSICAO"
                    VisibleIndex="7" Width="70px" CellStyle-HorizontalAlign="Center" Visible="FALSE">
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
				                                                                             e.errorText='CH deve ser um número';
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
                <dxwgv:GridViewDataCheckColumn Caption="Recusado" FieldName="RECUSADO" Name="RECUSADO"
                    VisibleIndex="10">
                </dxwgv:GridViewDataCheckColumn>
            </Columns>
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucConfirmarReposicao" ClientInstanceName="pucConfirmarReposicao"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma a exclusão da Reposição?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
