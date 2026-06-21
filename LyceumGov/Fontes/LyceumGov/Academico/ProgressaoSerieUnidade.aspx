<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProgressaoSerieUnidade.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ProgressaoSerieUnidade"
    EnableEventValidation="false" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="ConProgressaoSerieUnidade" ContentPlaceHolderID="cphFormulario"
    runat="server">
    <asp:ScriptManagerProxy ID="manager" runat="server" />
    <asp:Panel ID="pnBuscaProgressaoSerieUnidade" runat="server" GroupingText="Informe os dados para pesquisar: " Height="60px" Width="650px">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblUnidadeEnsinoTSearch" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT UNIDADE_ENS, NOME_COMP FROM LY_UNIDADE_ENSINO "
                            GridWidth="650px" OnChanged="tseUnidadeEnsino_Changed"
                            SqlOrder="nome_comp" AutoPostBack="true">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            </gridcolumns>
                        </tweb:TSearchBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvtseUnidadeEnsino" runat="server" ControlToValidate="tseUnidadeEnsino"
                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm"><img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" />
                         </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModalidade" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbModalidade" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE"
                            AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbModalidade_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNivel" runat="server" Text="Nível/Segmento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbNivel" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO"
                            AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbNivel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCurso" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" Key="curso"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome" 
                            SqlSelect="SELECT DISTINCT C.CURSO, C.NOME 
                                         FROM LY_CURSO C 
                                        INNER JOIN LY_MODALIDADE_CURSO M ON C.MODALIDADE = M.MODALIDADE
                                        INNER JOIN LY_TIPO_CURSO T ON C.TIPO = T.TIPO
                                        INNER JOIN LY_UNIDADE_ENSINO_CURSOS U ON C.CURSO = U.CURSO" 
                            GridWidth="650px"
                            OnChanged="tseCurso_Changed" SqlOrder="nome">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Curso" FieldName="nome" Width="30%" />
                            </gridcolumns>
                        </tweb:TSearchBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvtseCurso" runat="server" ControlToValidate="tseCurso"
                            ErrorMessage="Curso: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm"><img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" />
                         </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSerie" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="DESCRICAO" DataValueField="SERIE"
                            AutoPostBack="false" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Panel ID="pnBuscaProximoCursoSerie" runat="server" GroupingText="Dados Próximo Curso/Série para Unidade de Ensino: "
        Height="60px" Width="650px">
        <div>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblModalidadeProximoCursoSerie" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbModalidadeProximoCursoSerie" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE"
                            AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbModalidadeProximoCursoSerie_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNivelProximoCursoSerie" runat="server" Text="Nível/Segmento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbNivelProximoCursoSerie" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO"
                            AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbNivelProximoCursoSerie_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblCursoProximoCursoSerie" runat="server" Text="Próximo Curso:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCursoProximoCursoSerie" runat="server" Caption="" Key="curso"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome" 
                            SqlSelect="SELECT DISTINCT C.CURSO, C.NOME 
                                         FROM LY_CURSO C 
                                        INNER JOIN LY_MODALIDADE_CURSO M ON C.MODALIDADE = M.MODALIDADE
                                        INNER JOIN LY_TIPO_CURSO T ON C.TIPO = T.TIPO
                                        INNER JOIN LY_UNIDADE_ENSINO_CURSOS U ON C.CURSO = U.CURSO"
                            GridWidth="650px"
                            OnChanged="tseCursoProximoCursoSerie_Changed" SqlOrder="nome">
                            <gridcolumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Curso" FieldName="nome" Width="30%" />
                            </gridcolumns>
                        </tweb:TSearchBox>
 
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvtseCursoProximoCursoSerie" runat="server" ControlToValidate="tseCursoProximoCursoSerie"
                            ErrorMessage="Curso próximo Curso/Série: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm"><img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" />
                         </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSerieProximoCursoSerie" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbSerieProximoCursoSerie" runat="server" DataTextField="DESCRICAO" DataValueField="SERIE"
                            AutoPostBack="false" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPreferencial" runat="server" Text="Preferencial">
                        </asp:CheckBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" ValidationGroup="ConfirmarForm"/>
    <asp:UpdatePanel ID="uppProgressaoSerieUnidade" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel ID="pnGridProgressaoSerieUnidade" runat="server" Visible="true">
                <dxwgv:ASPxGridView ID="grdProgressaoSerieUnidade" runat="server" AutoGenerateColumns="False"
                    Visible="true" ClientInstanceName="grdProgressaoSerieUnidade" DataSourceID="odsProgressaoSerieUnidade"
                    KeyFieldName="PROGRESSAOSERIE_UNIDADEENSINO_ID" OnStartRowEditing="grdProgressaoSerieUnidade_StartRowEditing"
                    OnAfterPerformCallback="grdProgressaoSerieUnidade_AfterPerformCallback" 
                    OnCellEditorInitialize="grdProgressaoSerieUnidade_CellEditorInitialize"
                    OnRowUpdating="grdProgressaoSerieUnidade_RowUpdating"
                    OnRowDeleting="grdProgressaoSerieUnidade_RowDeleting">
                    <SettingsBehavior ConfirmDelete="True" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                            <EditButton Text="Editar" Visible="True">
                                <Image Url="~/img/bt_editar.png" />
                            </EditButton>
                            <DeleteButton Text="Remover" Visible="True">
                                <Image Url="~/img/bt_exclui2.png" />
                            </DeleteButton>
                            <CancelButton Text="Cancelar">
                                <Image Url="~/img/bt_cancelar.png" />
                            </CancelButton>
                            <UpdateButton Text="Alterar">
                                <Image Url="~/img/bt_salvar.png" />
                            </UpdateButton>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROGRESSAOSERIE_UNIDADEENSINO_ID" Caption="ProgressaoSerieUnidadeId"
                            VisibleIndex="1" Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="COD_CURSO" Caption="Código" VisibleIndex="2"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="NOME_CURSO" Caption="Curso" VisibleIndex="3"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="COD_NIVEL" Caption="Código Nível" VisibleIndex="4"
                            Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="NOME_NIVEL" Caption="Nível" VisibleIndex="5"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="COD_SERIE" Caption="Código Série" VisibleIndex="6"
                            Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="SERIE" Caption="Série" VisibleIndex="7"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROX_COD_CURSO" Caption="Código" VisibleIndex="8"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROX_NOME_CURSO" Caption="Prox. Curso" VisibleIndex="9"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROX_COD_NIVEL" Caption="Código Tipo" VisibleIndex="10"
                            Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROX_NOME_NIVEL" Caption="Tipo" VisibleIndex="11"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="PROX_SERIE" Caption="Prox. Série" VisibleIndex="12"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataCheckColumn FieldName="PREFERENCIAL" Caption="Preferencial" VisibleIndex="13"
                            Width="120px" Visible="true">
                            <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                            </PropertiesCheckEdit>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataDateColumn FieldName="DATACADASTRO" Caption="Data" VisibleIndex="14"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="USUARIOID" Caption="Usuário" VisibleIndex="15"
                            Visible="true" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="UNIDADEENSINOID" Caption="Unidade de Ensino" VisibleIndex="16"
                            Visible="false" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <asp:ObjectDataSource ID="odsProgressaoSerieUnidade" TypeName="Techne.Lyceum.Net.Academico.ProgressaoSerieUnidade"
                runat="server" SelectMethod="ListaProgressaoSerieUnidade" OnDeleting="odsProgressaoSerieUnidade_Deleting"
                DeleteMethod="Delete">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseUnidadeEnsino" Name="UnidadeEnsinoId" PropertyName="Value" />
                    <asp:ControlParameter ControlID="tseCurso" Name="CursoId" PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
