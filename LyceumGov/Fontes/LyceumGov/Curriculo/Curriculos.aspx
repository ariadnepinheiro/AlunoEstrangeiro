<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Curriculos.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.Curriculos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OngrdSeriesEndCallBack() {
            if (typeof (grdGrade) != 'undefined' && grdGrade != null) {
                var valor = 'grade';
                grdGrade.PerformCallback(valor);
            }
        }
        function OnCursoChanged(cmbCurso) {

            grdSeries.GetEditor("serie_seguinte").PerformCallback(cmbCurso.GetValue().toString());
        }

        function OnAnoSerieConcluinteChanged() {
            if (typeof (chkAnoSerieConcluinte) != 'undefined' && chkAnoSerieConcluinte != null) {
                if (typeof chkAnoSerieConcluinte.GetValue() != 'undefined' && chkAnoSerieConcluinte.GetValue() != null) {
                    if (chkAnoSerieConcluinte.GetValue().toString() == "S") {
                        curso_seguinte.SetEnabled(false);
                        serie_seguinte.SetEnabled(false);
                        curso_seguinte.SetText('');
                        serie_seguinte.ClearItems();

                    }
                    else if (chkAnoSerieConcluinte.GetValue().toString() == "N") {
                        curso_seguinte.SetEnabled(true);
                        serie_seguinte.SetEnabled(true);
                    }
                }
                else {
                    curso_seguinte.SetEnabled(true);
                    serie_seguinte.SetEnabled(true);
                }
            }
        }
        

     

    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por escolaridade e matriz curricular"
        Width="750px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseNivel" runat="server" Argument="descricao" Caption="" Key="tipo"
                        SqlOrder="descricao" SqlSelect="SELECT tipo, descricao FROM ly_tipo_curso" OnChanged="tseNivel_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="tipo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseModalidade" runat="server" Argument="descricao" Caption=""
                        Key="modalidade" SqlOrder="descricao" SqlSelect="SELECT modalidade, descricao FROM ly_modalidade_curso"
                        OnChanged="tseModalidade_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="modalidade" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Argument="nome" Caption="" Key="curso"
                        SqlOrder="nome" SqlSelect="SELECT curso, nome FROM ly_curso" SqlWhere="tipo = #tseNivel# and tipo is not null AND modalidade = #tseModalidade# and modalidade is not null"
                        OnChanged="tseCurso_Changed" Enabled="False">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lbltseTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseTurno" runat="server" Argument="descricao" Caption="" Key="turno"
                        SqlOrder="descricao" SqlSelect="SELECT turno, descricao FROM ly_turno" OnChanged="tseTurno_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="turno" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCurriculoTSearch" runat="server" Text="Matriz Curricular:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurriculo" runat="server" Caption="" Key="curriculo" SqlOrder="curriculo"
                        SqlSelect="SELECT distinct curriculo, curriculo AS curriculo1 FROM ly_curriculo"
                        Argument="curriculo1" SqlWhere="curso = isnull(#tseCurso#,'')  and turno = isnull(#tseTurno#,'')"
                        OnChanged="tseCurriculo_Changed" Enabled="False">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curriculo" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="curriculo1" Width="70%" />
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
    <asp:HiddenField ID="hdnEnsReligioso" runat="server" />
    <asp:HiddenField ID="hdnLinguaEstrangeira" runat="server" />
    <asp:ObjectDataSource ID="odsCurriculosSeries" runat="server" TypeName="Techne.Lyceum.RN.Serie"
        SelectMethod="ConsultarCurriculosSeries" InsertMethod="InsertCurriculosSerie"
        UpdateMethod="UpdateCurriculosSerie" DeleteMethod="DeleteCurriculosSerie" OnDeleting="odsCurriculosSeries_Deleting"
        OnInserting="odsCurriculosSeries_Inserting" OnUpdating="odsCurriculosSeries_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseCurso" Name="curso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseTurno" Name="turno" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseCurriculo" Name="curriculo" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsNivel" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_curso"
        SqlWhere="tipo = @tipo">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseNivel" Name="tipo" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsModalidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_modalidade_curso"
        SqlWhere="modalidade = @modalidade">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseModalidade" Name="modalidade" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsUnidadesFisicas" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_curriculo_unidade_fisica"
        SqlWhere="curso = @curso AND curriculo = @curriculo">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseCurso" Name="curso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseCurriculo" Name="curriculo" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource> 
    <techne:TTableDataSource ID="tdsCurriculo" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_curriculo"
        SqlWhere="curso = @curso AND curriculo = @curriculo">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseCurso" Name="curso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseCurriculo" Name="curriculo" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsFaculdade" runat="server" DataTableClassName="Techne.Lyceum.CR.vw_zzcro_unidade_fisica">
    </techne:TTableDataSource>
    <div class="divEditBlock" style="width: 90%;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Matriz curricular" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcCurriculos" runat="server" Width="90%" ActiveTabIndex="2">
        <tabpages>
            <dxtc:TabPage Name="tabDefinicao" Text="Definição">
                <ContentCollection>
                    <dxw:ContentControl ID="conDefinicao" runat="server">
                        <table id="tblDescr" runat="server">
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="ddlCurso" runat="server" Argument="nome" Caption="" Key="curso"
                                        SqlOrder="nome" SqlSelect="SELECT curso, nome FROM ly_curso" AutoPostBack="false"
                                        OnChanged="ddlCurso_Changed">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                    <asp:RequiredFieldValidator ID="rfvCurso" runat="server" ControlToValidate="ddlCurso"
                                        InitialValue="" ErrorMessage="Escolaridade: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTurno" runat="server" DataValueField="turno" DataTextField="descricao">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTurno" runat="server" ControlToValidate="ddlTurno"
                                        InitialValue="" ErrorMessage="Turno: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblCurriculo" runat="server" Text="Especificação:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurriculo" runat="server" MaxLength="100" Columns="80" Width="200px">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCurriculo" runat="server" ControlToValidate="txtCurriculo"
                                        InitialValue="" ErrorMessage="Especificação: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblAno_Ini" runat="server" Text="Ano de Início:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAno_Ini" runat="server" DataValueField="ano" OnSelectedIndexChanged="ddlAno_Ini_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno_Ini"
                                        InitialValue="" ErrorMessage="Ano de Inicío: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblSem_Ini" runat="server" Text="Período de Início:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem_Ini" runat="server" DataValueField="periodo">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPeriodo" runat="server" ControlToValidate="ddlSem_Ini"
                                        InitialValue="" ErrorMessage="Período de Início: Preenchimento obrigatório."
                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDt_Homolog" runat="server" Text="Data de Publicação D.O.:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDt_Homolog" runat="server" MinDate="1901-01-01">
                                    </dxe:ASPxDateEdit>
                                    <asp:RequiredFieldValidator ID="rfvDtHomolog" runat="server" ControlToValidate="dtDt_Homolog"
                                        InitialValue="" ErrorMessage="Data de Publicação D.O.: Preenchimento obrigatório."
                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDt_Extincao" runat="server" Text="Data de Extinção: "></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDt_Extinsao" runat="server" MinDate="1901-01-01">
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                            <td style="text-align: right">
                                    <asp:Label ID="Label1" runat="server" Text="Optativas: "></asp:Label>
                                </td>
                                <td>
                                   <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                                Enabled="false" />
                            <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                Enabled="false" />
                                 
                                </td>
                            </tr>
                         
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="tabSeries" Text="Anos de Escolaridade">
                <ContentCollection>
                    <dxw:ContentControl ID="conSeries" runat="server">
                        <asp:ObjectDataSource ID="odsCurso" runat="server" TypeName="Techne.Lyceum.RN.Curso"
                            SelectMethod="ConsultarEscolaridade"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdSeries" runat="server" AutoGenerateColumns="False" DataSourceID="odsCurriculosSeries"
                            KeyFieldName="CompositeKey" OnCellEditorInitialize="grdSeries_CellEditorInitialize"
                            ClientInstanceName="grdSeries" OnCustomUnboundColumnData="grdSeries_CustomUnboundColumnData"
                            OnRowDeleting="grdSeries_RowDeleting" OnRowUpdating="grdSeries_RowUpdating" Font-Names="Verdana"
                            Font-Size="Small" OnInitNewRow="grdSeries_InitNewRow" OnStartRowEditing="grdSeries_StartRowEditing"
                            Width="800px" OnRowValidating="grdSeries_RowValidating" OnRowInserting="grdSeries_RowInserting"
                            OnAfterPerformCallback="grdSeries_AfterPerformCallback">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <ClientSideEvents EndCallback="function(s, e) { OngrdSeriesEndCallBack(); }" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdSeries.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>                                
                                <dxwgv:GridViewDataComboBoxColumn Caption="Curso" FieldName="curso" VisibleIndex="1"
                                    Width="120px" Visible="False">
                                    <PropertiesComboBox DataSourceID="odsCurso" TextField="curso" ValueField="curso"
                                        ValueType="System.String">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>            
                                <dxwgv:GridViewDataComboBoxColumn Caption="Turno" FieldName="turno" VisibleIndex="1"
                                    Width="120px" Visible="False">
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Currículo" FieldName="curriculo" VisibleIndex="3"
                                    Width="100px" Visible="False">
                                    <PropertiesComboBox DataSourceID="tdsCurriculo" TextField="curriculo" ValueField="curriculo"
                                        ValueType="System.String">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano/Série*" FieldName="serie" VisibleIndex="4"
                                    Width="50px">
                                    <PropertiesTextEdit MaxLength="3" Width="100px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor informar o Ano/Série." />
                                            <RegularExpression ErrorText="Ano/Série deve ser um número positivo de até 3 dígitos." ValidationExpression="\d{1,3}" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e) { return MRAcceptNumber(s,e,3); }" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição*" FieldName="descricao" VisibleIndex="5"
                                    Width="150px">
                                    <PropertiesTextEdit MaxLength="100" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor informar a Descrição." />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Prefixo das Turmas" FieldName="complemento1" VisibleIndex="6"
                                    Width="50px">
                                    <PropertiesTextEdit MaxLength="11" Width="70px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tempos de Aula*" FieldName="complemento2"
                                    VisibleIndex="7" Width="120px">
                                    <PropertiesTextEdit MaxLength="2" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor informar os Tempos de Aula." />
                                            <RegularExpression ErrorText="Tempos de Aula só aceita números inteiros e positivos."
                                                ValidationExpression="^\d*$" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Idade Mínima de Ingresso" FieldName="idade_minima"
                                    VisibleIndex="8" Width="100px">
                                    <PropertiesTextEdit MaxLength="3" Width="100px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RegularExpression ErrorText="Idade Mínima de Ingresso deve ser um número de até 3 dígitos."
                                                ValidationExpression="\d{0,3}" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e) { return MRAcceptNumber(s,e,3); }" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dia Limite de Aniversário" FieldName="dia_aniv"
                                    VisibleIndex="9" Width="150px">
                                    <PropertiesTextEdit MaxLength="2" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RegularExpression ErrorText="Dia Limite de Aniversário deve ser um número de até 2 dígitos."
                                                ValidationExpression="\d{0,2}" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e) { return MRAcceptNumber(s,e,2); }" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Mês Limite de Aniversário" FieldName="mes_aniv"
                                    VisibleIndex="10" Width="100px">
                                    <PropertiesTextEdit MaxLength="2" Width="100px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RegularExpression ErrorText="O Mês deve ser um número de até 2 dígitos." ValidationExpression="\d{0,2}" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e) { return MRAcceptNumber(s,e,2); }" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn VisibleIndex="11" Caption="Desativado em" FieldName="dt_extincao">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="12" Caption="Curso Seguinte*" FieldName="curso_seguinte"
                                    Width="300px">
                                    <PropertiesComboBox TextField="nome" ValueField="curso" EnableSynchronization="False"
                                        EnableIncrementalFiltering="True" DataSourceID="odsCurso" ClientInstanceName="curso_seguinte">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCursoChanged(s); }"></ClientSideEvents>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="13" Caption="Série Seguinte*" FieldName="serie_seguinte">
                                    <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                        ClientInstanceName="serie_seguinte">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ano/Série Concluinte?" FieldName="ano_serie_concluinte"
                                    Name="ano_serie_concluinte" VisibleIndex="14">
                                    <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                                        DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkAnoSerieConcluinte"
                                        DisplayTextUndefined="" NullDisplayText="N">
                                        <ClientSideEvents CheckedChanged="function(s, e) { OnAnoSerieConcluinteChanged(s);}"
                                            Init="function(s, e) { OnAnoSerieConcluinteChanged(s);}" />
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Emite Certificação?" FieldName="emite_certificacao"
                                    Name="emite_certificacao" VisibleIndex="14">
                                    <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                                        DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkEmite_Certificacao"
                                        DisplayTextUndefined="" NullDisplayText="N">                                       
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                 <dxwgv:GridViewDataCheckColumn Caption="Oferece Eletiva?" FieldName="ofertaeletiva"
                                    Name="ofertaeletiva" VisibleIndex="15">
                                    <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                                        DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="chkOfereceEletiva"
                                        DisplayTextUndefined="" NullDisplayText="N">                                       
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="tabMatrizCurricular" Text="Componentes Curriculares" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <dxwgv:ASPxGridView ID="grdGrade" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdGrade"
                            KeyFieldName="CompositeKey" OnInitNewRow="grdGrade_InitNewRow" OnStartRowEditing="grdGrade_StartRowEditing"
                            OnRowDeleting="grdGrade_RowDeleting" OnRowInserting="grdGrade_RowInserting" OnCustomCallback="grdGrade_CustomCallback"
                            OnRowUpdating="grdGrade_RowUpdating" DataSourceID="odsGrade" OnCustomUnboundColumnData="grdGrade_CustomUnboundColumnData"
                            OnAfterPerformCallback="grdGrade_AfterPerformCallback" OnRowValidating="grdGrade_RowValidating"
                            OnHtmlEditFormCreated="grdGrade_HtmlEditFormCreated">
                            <SettingsEditing Mode="EditForm" />
                            <SettingsBehavior ConfirmDelete="true" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDisciplina" Text="Componente Curricular:*" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox ID="tseDisciplina" runat="server" Argument="nome" Caption="" AutoPostBack="false"
                                                            Key="disciplina" SqlOrder="disciplina" Value='<%# Bind("disciplina") %>' DataType="VarChar"
                                                            Enabled='<%# Container.Grid.IsNewRowEditing %>' SqlSelect="SELECT disciplina, nome FROM ly_disciplina"
                                                            MaxLength="20">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Componente Curricular" FieldName="disciplina" Width="30%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSerie" Text="Ano de Escolaridade:*" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxComboBox ID="cboSerie" runat="server" DataSourceID="odsEscolaridade" TextField="serie"
                                                            EnableViewState="false" ValueField="serie" Value='<%# Bind("serie_ideal") %>'
                                                            ValueType="System.Int32" AutoPostBack="False" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                            <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                <RequiredField ErrorText="Favor informar o Ano de Escolaridade." IsRequired="True" />
                                                            </ValidationSettings>--%>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <asp:
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMacro" Text="Macrocampo:" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxComboBox ID="cboMacro" runat="server" DataSourceID="odsMacro" TextField="nome"
                                                            EnableViewState="false" ValueField="id_macro_campos" Value='<%# Bind("macro_nome") %>'
                                                            ValueType="System.Int32" AutoPostBack="False" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                            <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                                <RequiredField ErrorText="Favor informar a Macro." IsRequired="True" />
                                                            </ValidationSettings>--%>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblObrigatoria" Text="Obrigatória:" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxCheckBox ID="chkObrigatoria" runat="server" Value='<%# Bind("obrigatoria") %>'
                                                            ValueType="System.String" ValueChecked="S" ValueUnchecked="N" Checked="true">
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPermiteGLP" Text="Permite GLP:" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxCheckBox ID="chkPermiteGLP" runat="server" Value='<%# Bind("permite_glp") %>'
                                                            ValueType="System.String" ValueChecked="S" ValueUnchecked="N" Checked="true">
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Visible="true">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                onclick="grdGrade.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="curso" VisibleIndex="1"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno" VisibleIndex="1"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Currículo" FieldName="curriculo" VisibleIndex="2"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano de Escolaridade" FieldName="serie_ideal"
                                    VisibleIndex="3" Width="80px">
                                    <PropertiesTextEdit Width="70px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="disciplina" VisibleIndex="4"
                                    Width="90px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" FieldName="nomedisciplina" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Carga Horária" FieldName="cargahoraria" VisibleIndex="6"
                                    Width="80px" ReadOnly="true">
                                    <PropertiesTextEdit Width="70px" DisplayFormatString="######.00">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Componente" FieldName="componente" ReadOnly="true"
                                    VisibleIndex="7" Width="200px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Macrocampo" FieldName="macro_nome" ReadOnly="true"
                                    VisibleIndex="8" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Área de Conhecimento" FieldName="area_conhecimento"
                                    ReadOnly="true" VisibleIndex="8" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Obrigatória" FieldName="obrigatoria" VisibleIndex="9"
                                    Width="110px">
                                    <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Permite GLP" FieldName="permite_glp" VisibleIndex="10"
                                    Width="110px">
                                    <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    VisibleIndex="11" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </tabpages>
    </dxtc:ASPxPageControl>
    <asp:ObjectDataSource ID="odsGrade" runat="server" SelectMethod="Listar" TypeName="Techne.Lyceum.Net.Curriculo.Curriculos"
        OnDeleting="odsGrade_Deleting" OnInserting="odsGrade_Inserting" OnUpdating="odsGrade_Updating"
        DeleteMethod="Delete" UpdateMethod="Update" InsertMethod="Insert">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseCurso" Name="tseCurso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseTurno" Name="turno" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseCurriculo" Name="tseCurriculo" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsEscolaridade" runat="server" SelectMethod="ListarEscolaridades"
        TypeName="Techne.Lyceum.Net.Curriculo.Curriculos">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseCurso" Name="tseCurso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseTurno" Name="tseTurno" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseCurriculo" Name="tseCurriculo" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMacro" runat="server" SelectMethod="ListarMacros" TypeName="Techne.Lyceum.Net.Curriculo.Curriculos">
    </asp:ObjectDataSource>
</asp:Content>
