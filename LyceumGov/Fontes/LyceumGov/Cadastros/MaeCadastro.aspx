<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MaeCadastro.aspx.cs" Inherits="Techne.Lyceum.Net.Cadastros.MaeCadastro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnSituacaoChanged(ddlMotivo, txtOutroMotivo) {

            if (typeof (ddlMotivo) != 'undefined' && ddlMotivo != null) {
                var motivo = ddlMotivo.GetValue();
               
                if (motivo != null && motivo != "") {

                    if (motivo == "1") {
                        txtOutroMotivo.SetEnabled(true);

                    } else {
                        txtOutroMotivo.SetText("");
                        txtOutroMotivo.SetEnabled(false);
 
                    }      
                }
            }            
        }     
       
    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para consulta:"
        Width="750px">
        <table style="width: 500px">
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblCPF" runat="server" Text="CPF*:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCPF" runat="server" SqlOrder="nome" SqlSelect=" select usuarioid FROM Cadastros.MAE_INSCRICAO"
                        GridWidth="600px" ArgumentColumns="50" OnChanged="tseCPF_Changed" Columns="10"
                        Key="CPF" Argument="NOME" MaxLength="11">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="CPF" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="nome" FieldName="nome" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div id="divPrincipal" runat="server" visible="false">
        <br />
        <table style="width: 750px">
            <tr>
                <td>
                    <asp:Panel ID="pnDados" runat="server" GroupingText="Dados Iniciais" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Número Inscrição:"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNome" runat="server" Text="Nome do Responsável Legal:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblData" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNumeroInscricao" runat="server" Width="100px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNomeCompl" runat="server" Width="450px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtNascimento" runat="server" Enabled="false" Width="100px" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Sexo:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Responsável legal tomou a vacina?" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtSexo" runat="server" Width="70px" Enabled="false" />
                                    </div>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtTomouVacina" runat="server" Width="300px" Enabled="false" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="lblObrigatorio" Text="Desempregado há mais de quatro meses:"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Recebendo seguro desemprego:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>                               
                                <td>
                                    <asp:Label ID="Label9" runat="server" SkinID="lblObrigatorio" Text="Possui carga horária livre de 6 horas diárias:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDesempregado" runat="server" Width="70px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSeguro" runat="server" Width="70px" Enabled="false" />
                                </td>                                
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCHLivre" runat="server" Width="70px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" GroupingText="Contato" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label21" runat="server" SkinID="lblObrigatorio" Text="E-mail:"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label22" runat="server" Text="Telefone Celular:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" SkinID="lblObrigatorio" Text="Telefone Alternativo:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="350px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefone" runat="server" Width="150px" Enabled="false" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelAlternativo" runat="server" Enabled="false" Width="150px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel5" runat="server" GroupingText="Documentos Candidato" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" Text="CPF:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Identidade:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Orgão Emissor:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="UF Emissor:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCPF" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgNumero" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgEmissor" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRgUf" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel4" runat="server" GroupingText="Endereço" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="CEP:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" SkinID="lblObrigatorio" Text="Logradouro:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCEP" Style="width: 110px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLogradouro" runat="server" Width="300px" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="Número:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="Complemento:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNumero" runat="server" Style="width: 110px;" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComplemento" runat="server" Width="300px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="UF:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" SkinID="lblObrigatorio" Text="Município:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" SkinID="lblObrigatorio" Text="Bairro:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUFEndereco" runat="server" Width="50px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipioEndereco" runat="server" Width="200px" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBairro" runat="server" Width="200px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Informações Complementares" Width="750px">
                        <table width="750px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Escolaridade:" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" SkinID="lblObrigatorio" Text="Possui experiência de trabalho com criança e jovens?"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label24" runat="server" SkinID="lblObrigatorio" Text="Turno que deseja trabalhar:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEscolaridade" Style="width: 250px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExprienciaTrabalho" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTurnoTrabalho" Style="width: 145px;" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <asp:ObjectDataSource ID="odsAlunoResponsabilidade" runat="server" TypeName="Techne.Lyceum.Net.Cadastros.MaeCadastro"
            SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseCPF" DefaultValue="" Name="cpf" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdAlunoResponsabilidade" runat="server" DataSourceID="odsAlunoResponsabilidade"
            KeyFieldName="MAE_INSCRICAOALUNOID" AutoGenerateColumns="false" ClientInstanceName="grdAlunoResponsabilidade"
            OnInitNewRow="grdAlunoResponsabilidade_InitNewRow" OnStartRowEditing="grdAlunoResponsabilidade_StartRowEditing"
            Width="700px">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="MAE_INSCRICAOALUNOID"
                    Visible="false" Width="700px">
                    <PropertiesTextEdit MaxLength="200">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Aluno" Name="Aluno" VisibleIndex="2" FieldName="Aluno"
                    Width="400px">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME_COMPL" VisibleIndex="2" FieldName="NOME_COMPL"
                    Width="400px">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Vinculo" Name="Vinculo" VisibleIndex="2" FieldName="VINCULODESCRICAO"
                    Width="400px">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Outro Vínculo" Name="OUTROVINCULO" VisibleIndex="2"
                    FieldName="OUTROVINCULO" Width="400px">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <asp:ObjectDataSource ID="odsLotacao" TypeName="Techne.Lyceum.Net.Cadastros.MaeCadastro"
            runat="server" SelectMethod="ListaLotacao" UpdateMethod="Update" InsertMethod="Insert">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseCPF" DefaultValue="" Name="cpf" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdLotacao" runat="server" EnableCallBacks="false" ClientInstanceName="grdLotacao"
            AutoGenerateColumns="False" DataSourceID="odsLotacao" KeyFieldName="MAE_LOTACAOID" OnCommandButtonInitialize="grdLotacao_CommandButtonInitialize"
            OnInitNewRow="grdLotacao_InitNewRow" OnRowUpdating="grdLotacao_RowUpdating" OnCellEditorInitialize="grdLotacao_CellEditorInitialize"
            OnStartRowEditing="grdLotacao_StartRowEditing" OnAfterPerformCallback="grdLotacao_AfterPerformCallback"
            OnRowInserting="grdLotacao_RowInserting" Width="1500px" OnHtmlRowCreated="grdLotacao_HtmlRowCreated"
            OnCancelRowEditing="grdLotacao_CancelRowEditing">
            <SettingsEditing Mode="EditForm" />
            <Templates>
                <EditForm>
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseUnidade" runat="server" SqlSelect=" select DISTINCT UE.UNIDADE_ENS, UE.NOME_COMP
                                                                                                     from [Cadastros].[MAE_INSCRICAOALUNO] M 
                                                                                                     INNER JOIN Cadastros.MAE_INSCRICAO I ON I.MAE_INSCRICAOID = M.MAE_INSCRICAOID
                                                                                                     INNER JOIN LY_ALUNO A ON M.ALUNO = A.ALUNO
                                                                                                     INNER JOIN VW_UNIDADE_ENSINO_SITUACAO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO"
                                        SqlOrder="NOME_COMP" MaxLength="20" GridWidth="850px" DataType="Varchar" SqlWhere=" I.CPF = #tseCPF# "
                                        Value='<%# Bind("CENSO") %>'>
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="UNIDADE_ENS" FieldName="UNIDADE_ENS" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="NOME_COMP" FieldName="NOME_COMP" Width="40%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBanco" runat="server" Text="Banco:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseBanco" runat="server" Key="banco" Argument="nome" SqlSelect="SELECT banco, nome from BANCOS (nolock)"
                                        SqlOrder="nome" MaxLength="20" GridWidth="850px" DataType="Number" SqlWhere=" banco ='237'"
                                        Value='<%# Bind("BANCO") %>'>
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAgencia" runat="server" Text="Agência:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tsAgencia" runat="server" Key="agencia" Argument="nome" MaxLength="20"
                                        GridWidth="850px" SqlSelect="select agencia, banco, nome from AGENCIAS (nolock) "
                                        Value='<%# Bind("AGENCIA") %>' SqlWhere=" banco ='237'">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Agência" FieldName="agencia" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" runat="server" Text="Conta Corrente:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConta" runat="server" Width="100px" MaxLength="15" Text='<%# Bind("CONTACORRENTE") %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDataInicio" runat="server" Text="Data início:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit runat="server" ID="DATAINICIO" Value='<%# Bind("DATAINICIO") %>'
                                        Width="110px">
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:Label ID="lblDataFim" runat="server" Text="Data Fim: "></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAFIM" ID="ASPxGridViewTemplateReplacement6"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                                <td>
                                    <asp:Label ID="Label27" runat="server" Text="Motivo Desligamento: "></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="MAE_MOTIVODESLIGAMENTOID" ID="ASPxGridViewTemplateReplacement11"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                                <td>
                                    <asp:Label ID="Label28" runat="server" Text="Outro Motivo:"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtOutroMotivo" runat="server" Width="250px" ClientInstanceName="txtOutroMotivo"
                                      MaxLength="200" TextMode="MultiLine" Text='<%# Bind("DESCRICAOOUTROS") %>'>
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                    </div>
                </EditForm>
            </Templates>
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                onclick="grdLotacao.AddNewRow();" />
                        </div>
                    </HeaderCaptionTemplate>
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
                <dxwgv:GridViewDataTextColumn Caption="MAE_LOTACAOID" FieldName="MAE_LOTACAOID" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="PODEALTERAR" FieldName="PODEALTERAR" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="MAE_INSCRICAOID" FieldName="MAE_INSCRICAOID"
                    VisibleIndex="2" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="5">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn FieldName="DATAFIM" Caption="Data Fim" VisibleIndex="6">
                    <PropertiesDateEdit Width="110px" EditFormat="Date">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Motivo" FieldName="MAE_MOTIVODESLIGAMENTOID"
                    VisibleIndex="7" Width="300px">
                    <PropertiesComboBox ValueType="System.String" DataSourceID="odsMotivo" TextField="DESCRICAO"
                        ClientInstanceName="ddlMotivo" ValueField="MAE_MOTIVODESLIGAMENTOID" Width="300px">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnSituacaoChanged(ddlMotivo,txtOutroMotivo);}" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Outro Motivo" FieldName="DESCRICAOOUTROS"
                    VisibleIndex="8">
                    <PropertiesTextEdit Width="210px" ClientInstanceName="txtOutroMotivo" MaxLength="200">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código Banco" FieldName="BANCO" VisibleIndex="9">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Banco" FieldName="BANCODESCRICAO" VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código Agência" FieldName="AGENCIA" VisibleIndex="11">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Agência" FieldName="AGENCIADESCRICAO" VisibleIndex="12">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Conta Corrente" FieldName="CONTACORRENTE"
                    VisibleIndex="13">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <asp:ObjectDataSource ID="odsMotivo" runat="server" SelectMethod="ListaAtivo" TypeName="Techne.Lyceum.RN.Cadastros.MaeMotivoDesligamento">
        </asp:ObjectDataSource>
    </div>
</asp:Content>
