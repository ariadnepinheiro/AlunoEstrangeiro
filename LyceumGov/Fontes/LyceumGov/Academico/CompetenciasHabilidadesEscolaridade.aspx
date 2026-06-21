<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CompetenciasHabilidadesEscolaridade.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.CompetenciasHabilidadesEscolaridade"
    Title="Competências/Habilidades por Disciplina e Escolaridade" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botão direito neste campo");
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function onlyNumbers() {
            if (event.keyCode < 48
                || event.keyCode > 57) {
                event.keyCode = 0;
            };
        }

        function SomenteNumeros(oEvent) {
            var keycode = (oEvent.which) ? oEvent.which : oEvent.keyCode;

            if ((keycode >= 48 && keycode <= 57) || (keycode == 8))
                return (true && (keycode != 46));

            return false;
        }

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }

        $(document).ready(function() {
            $('#<%=txtNomeGrupo.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtOrdemGrupo.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
            $('#<%=txtNomeCompetencia.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtOrdemComp.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
        });

    </script>

    <asp:Panel ID="pnBuscaEscolaridade" runat="server" GroupingText="Informe o código ou a descrição da escolaridade"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblGrupoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE "
                        ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                        SqlOrder="nome" OnChanged="tseCurso_Changed" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnGeral" runat="server" Width="800px" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAno" runat="server" Text="Ano / Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSerie" runat="server" Text="Série:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSerie" runat="server" AutoPostBack="True" DataTextField="serie"
                        DataValueField="serie" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo de currículo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipo" runat="server" AppendDataBoundItems="True" AutoPostBack="True" Height="16px">
                        <asp:ListItem Selected="True" Text="Selecione" Value="Selecione"></asp:ListItem>
                        <asp:ListItem Text="BÁSICO" Value="BÁSICO"></asp:ListItem>
                        <asp:ListItem Text="ESSENCIALIZADO" Value="ESSENCIALIZADO"></asp:ListItem>
                        <asp:ListItem Text="RECOMPOSIÇÃO" Value="RECOMPOSIÇÃO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblBimestre" runat="server" Text="Subperíodo letivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBimestre" runat="server" AutoPostBack="True" DataTextField="descricao"
                        DataValueField="SUBPERIODO" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlBimestre_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDisciplinaGrupo" runat="server" Text="Disciplina / Grupo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDisciplinaGrupo" runat="server" AutoPostBack="True" DataTextField="NOME_DISCIPLINA"
                        DataValueField="DISCIPLINA" OnSelectedIndexChanged="ddlDisciplinaGrupo_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
      <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="false">
        <dxtc:ASPxPageControl ID="pcCompHabil" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="false" OnTabClick="pcCompHabil_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Grupos">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNomeGrupo" runat="server" Text="Nome Grupo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeGrupo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOrdemGrupo" runat="server" Text="Ordem:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrdemGrupo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvarGrupo" runat="server" ValidationGroup="SalvarForm" Text="Incluir Grupo"
                                            OnClick="btnSalvarGrupo_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <dxwgv:ASPxGridView ID="grdGrupo" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdGrupo"
                                KeyFieldName="ID_COMPETENCIA_HABILIDADE_GRUPO" DataSourceID="odsGrupo" OnCellEditorInitialize="grdGrupo_CellEditorInitialize"
                                OnStartRowEditing="grdGrupo_StartRowEditing" OnAfterPerformCallback="grdGrupo_AfterPerformCallback"
                                OnRowValidating="grdGrupo_RowValidating">
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <EditButton Text="Editar" Visible="True">
                                            <Image Url="~/img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <UpdateButton Text="Salvar">
                                            <Image Url="~/img/bt_salvar.png" />
                                        </UpdateButton>
                                        <CancelButton Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_COMPETENCIA_HABILIDADE_GRUPO"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Curso" VisibleIndex="2" FieldName="CURSO" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Modalidade" VisibleIndex="3" FieldName="MODALIDADE" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Tipo_curso" VisibleIndex="4" FieldName="TIPO_CURSO" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="5" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Periodo" VisibleIndex="6" FieldName="PERIODO" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Bimestre" FieldName="SUBPERIODO" VisibleIndex="7" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" VisibleIndex="8" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" VisibleIndex="9" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nome Disciplina" FieldName="NOME_DISCIPLINA" Visible="false"
                                        VisibleIndex="10">
                                    </dxwgv:GridViewDataTextColumn>
                                     <dxwgv:GridViewDataTextColumn Caption="Tipo do currículo" VisibleIndex="11" FieldName="TIPO_CURRICULO"
                                        PropertiesTextEdit-MaxLength="200" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>                                   
                                    <dxwgv:GridViewDataTextColumn Caption="Grupo" VisibleIndex="12" FieldName="GRUPO"
                                        PropertiesTextEdit-MaxLength="200">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataSpinEditColumn Caption="Ordem" FieldName="ORDEM" VisibleIndex="13"
                                        Width="70px" PropertiesSpinEdit-MaxLength="5">
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
			                                                                if(iVal&lt;1)
			                                                                {
				                                                                e.isValid = false;
				                                                                e.errorText='O Ano de Início deve ser positivo';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a Ordem" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesSpinEdit>
                                    </dxwgv:GridViewDataSpinEditColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DT_CADASTRO" VisibleIndex="14" Visible="false">
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Competência/Habilidade">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGrupo" runat="server" Text="Grupo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ObjectDataSource ID="odsCompetencia" TypeName="Techne.Lyceum.Net.Academico.CompetenciasHabilidadesEscolaridade"
                                            runat="server" SelectMethod="ListarCompetencia" UpdateMethod="odsCompetencia_Update"
                                            DeleteMethod="odsCompetencia_Delete" OnDeleting="odsCompetencia_Deleting" OnUpdating="odsCompetencia_Updating">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlGrupo" DefaultValue="" Name="ID_COMPETENCIA_HABILIDADE_GRUPO"
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" DataTextField="GRUPO"
                                            DataValueField="ID_COMPETENCIA_HABILIDADE_GRUPO" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNomeCompetencia" runat="server" Text="Nome Competencia / Habilidade:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeCompetencia" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOrdemComp" runat="server" Text="Ordem:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrdemComp" runat="server"  ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvarComp" runat="server" ValidationGroup="SalvarForm" Text="Incluir Competência/Habilidades"
                                            OnClick="btnSalvarComp_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <dxwgv:ASPxGridView ID="grdCompetenciaHab" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdCompetenciaHab" KeyFieldName="ID_COMPETENCIA_HABILIDADE_ITEM"
                                DataSourceID="odsCompetencia" OnCellEditorInitialize="grdCompetenciaHab_CellEditorInitialize"
                                OnStartRowEditing="grdCompetenciaHab_StartRowEditing" OnAfterPerformCallback="grdCompetenciaHab_AfterPerformCallback"
                                OnRowValidating="grdCompetenciaHab_RowValidating">
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <EditButton Text="Editar" Visible="True">
                                            <Image Url="~/img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <UpdateButton Text="Salvar">
                                            <Image Url="~/img/bt_salvar.png" />
                                        </UpdateButton>
                                        <CancelButton Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_COMPETENCIA_HABILIDADE_ITEM"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID_GRUPO" FieldName="ID_COMPETENCIA_HABILIDADE_GRUPO"
                                        VisibleIndex="2" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="GRUPO" FieldName="GRUPO" VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataSpinEditColumn Caption="Ordem" FieldName="ORDEM" VisibleIndex="4"
                                        Width="70px" PropertiesSpinEdit-MaxLength="5">
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
			                                                                if(iVal&lt;1)
			                                                                {
				                                                                e.isValid = false;
				                                                                e.errorText='O Ano de Início deve ser positivo';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a Ordem" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesSpinEdit>
                                    </dxwgv:GridViewDataSpinEditColumn>                               
                                    <dxwgv:GridViewDataTextColumn Caption="Competencia Habilidade" VisibleIndex="5" FieldName="COMPETENCIA_HABILIDADE"
                                        PropertiesTextEdit-MaxLength="200">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DT_CADASTRO" VisibleIndex="6" Visible="false">
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
  
    <asp:ObjectDataSource ID="odsGrupo" TypeName="Techne.Lyceum.Net.Academico.CompetenciasHabilidadesEscolaridade"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" DeleteMethod="Delete"
        OnDeleting="odsGrupo_Deleting" OnUpdated="odsGrupo_Update" OnUpdating="odsGrupo_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlDisciplinaGrupo" DefaultValue="" Name="DISCIPLINA"
                PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlSerie" DefaultValue="" Name="serie" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlBimestre" DefaultValue="" Name="SUBPERIODO" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="tseCurso" DefaultValue="" Name="curso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlTipo" DefaultValue="" Name="tipo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
