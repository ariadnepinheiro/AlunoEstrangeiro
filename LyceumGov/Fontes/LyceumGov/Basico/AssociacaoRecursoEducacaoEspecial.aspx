<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AssociacaoRecursoEducacaoEspecial.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AssociacaoRecursoEducacaoEspecial" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }
       
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o CPF ou o nome do recurso"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblTSearch" runat="server" Text="Recurso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseRecurso" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryRecurso"
                        AutoPostBack="true" OnTextChanged="tseRecurso_Changed" MaxLength="11">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcAssociacao" runat="server" Width="90%" ActiveTabIndex="0"
        Visible="false">
        <TabPages>
            <dxtc:TabPage Name="tabCuidador" Text="Cuidador">
                <ContentCollection>
                    <dxw:ContentControl ID="conCuidador" runat="server">
                        <asp:UpdatePanel ID="upCuidador" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearch ID="tseAlunoCuidador" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoNecessidadeEspecial"
                                                AutoPostBack="true" OnTextChanged="tseAlunoCuidador_Changed">
                                            </tweb:TSearch>
                                        </td>
                                    </tr>
                                    <tr id="trVigenciaCuidador" runat="server">
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblVigenciaCuidador" runat="server" Text="Vigência:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtInicioCuidador" runat="server" Width="120px" Enabled="true"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtInicioCuidador" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            a
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtFimCuidador" runat="server" Width="120px" Enabled="true"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtFimCuidador" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="width: 45%">
                                            &nbsp
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <table>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnAssociarCuidador" runat="server" ValidationGroup="SalvarForm"
                                        Text="Associar Aluno" OnClick="btnAssociarCuidador_Click" OnClientClick="Blooqueio()" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <dxwgv:ASPxGridView ID="grdAssociacaoCuidador" runat="server" AutoGenerateColumns="False"
                            Visible="true" ClientInstanceName="grdAssociacaoCuidador" DataSourceID="odsAssociacaoCuidador"
                            KeyFieldName="CUIDADORALUNOID" OnStartRowEditing="grdAssociacaoCuidador_StartRowEditing"
                            OnAfterPerformCallback="grdAssociacaoCuidador_AfterPerformCallback" OnRowUpdating="grdAssociacaoCuidador_RowUpdating">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton Text="Alterar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="CUIDADORALUNOID" Caption="Id" VisibleIndex="1"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ALUNOID" Caption="Aluno" VisibleIndex="2"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="NOME_COMPL" Caption="Nome" VisibleIndex="3"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TURNO" Caption="Turno" VisibleIndex="4"
                                    Visible="true" ReadOnly="true">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="5"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAINICIO">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="6"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAFIM">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsAssociacaoCuidador" TypeName="Techne.Lyceum.Net.Basico.AssociacaoRecursoEducacaoEspecial"
                            runat="server" SelectMethod="ListaAssociacaoCuidador" OnUpdating="odsAssociacaoCuidador_Updating"
                            UpdateMethod="Update">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseRecurso" Name="recursoId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="tabLedor" Text="Ledor">
                <ContentCollection>
                    <dxw:ContentControl ID="conLedor" runat="server">
                        <asp:UpdatePanel ID="upLedor" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblUnidadeEnsinoTSearch" runat="server" Text="Unidade de Ensino:* "
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearchBox ID="tseUnidadeEnsinoLedor" runat="server" Caption="" Key="unidade_ens"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                                GridWidth="850px" OnChanged="tseUnidadeEnsinoLedor_Changed" SqlOrder="nome_comp">
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
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblAno" runat="server" Text="Ano Letivo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                                                Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblPeriodo" runat="server" SkinID="lblObrigatorio" Text="Período Letivo:*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSemestre" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                                DataTextField="periodo" DataValueField="periodo" OnSelectedIndexChanged="ddlSemestre_SelectedIndexChanged"
                                                Width="70px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblCurso" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" Key="curso" MaxLength="20"
                                                ArgumentColumns="50" Columns="10" Argument="nome" SqlSelect="SELECT distinct c.curso as curso, nome,mc.DESCRICAO AS modalidade,tc.DESCRICAO AS segmento FROM LY_CURSO C INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO      "
                                                GridWidth="650px" OnChanged="tseCurso_Changed" SqlOrder="nome">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Curso" FieldName="nome" Width="30%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTurno" runat="server" SkinID="lblObrigatorio" Text="Turno:* "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurno" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                                DataTextField="descricao" DataValueField="turno" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblSerie" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="SERIE" DataValueField="SERIE"
                                                AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label1" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurma" runat="server" DataTextField="TURMA" DataValueField="TURMA"
                                                AutoPostBack="TRUE" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label2" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearch ID="tseAlunoLedor" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoEnturmadoNecessidadeEspecial"
                                                AutoPostBack="true" OnTextChanged="tseAlunoLedor_Changed" ValueField="aluno">
                                                <QueryParameters>
                                                    <asp:ControlParameter ControlID="tseUnidadeEnsinoLedor" Name="unidade_ens" PropertyName="DBValue" />
                                                    <asp:ControlParameter ControlID="ddlTurma" Name="turma" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="ddlSemestre" Name="semestre" PropertyName="SelectedValue" />
                                                </QueryParameters>
                                            </tweb:TSearch>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label3" runat="server" Text="Vigência:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtInicioLedor" runat="server" Width="120px" Enabled="true"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtInicioLedor" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            a
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtFimLedor" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                                ClientInstanceName="dtFimLedor" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="width: 45%">
                                            &nbsp
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <table>
                            <tr>
                                <td colspan="4" style="text-align: right;">
                                    <asp:Button ID="btnAssociarLedor" runat="server" ValidationGroup="SalvarForm" Text="Associar Aluno"
                                        OnClick="btnAssociarLedor_Click" OnClientClick="Blooqueio()" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <dxwgv:ASPxGridView ID="grdAssociacaoLedor" runat="server" AutoGenerateColumns="False"
                            Visible="true" ClientInstanceName="grdAssociacaoLedor" DataSourceID="odsAssociacaoLedor"
                            KeyFieldName="LEDORALUNOID" OnStartRowEditing="grdAssociacaoLedor_StartRowEditing"
                            OnAfterPerformCallback="grdAssociacaoLedor_AfterPerformCallback" OnRowUpdating="grdAssociacaoLedor_RowUpdating">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton Text="Alterar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="LEDORALUNOID" Caption="Id" VisibleIndex="1"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ALUNOID" Caption="Aluno" VisibleIndex="2"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="NOME_COMPL" Caption="Nome" VisibleIndex="3"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ANO" Caption="Ano" VisibleIndex="4" Visible="true"
                                    ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="SEMESTRE" Caption="Semestre" VisibleIndex="5"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TURMA" Caption="Turma" VisibleIndex="6"
                                    Visible="true" ReadOnly="true">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TURNO" Caption="Turno" VisibleIndex="7"
                                    Visible="true" ReadOnly="true">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="8"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAINICIO">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="9"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAFIM">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsAssociacaoLedor" TypeName="Techne.Lyceum.Net.Basico.AssociacaoRecursoEducacaoEspecial"
                            runat="server" SelectMethod="ListaAssociacaoLedor" OnUpdating="odsAssociacaoLedor_Updating"
                            UpdateMethod="UpdateLedor">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseRecurso" Name="recursoId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="tabInterprete" Text="Intérprete">
                <ContentCollection>
                    <dxw:ContentControl ID="conInterprete" runat="server">
                        <asp:UpdatePanel ID="upInterprete" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label4" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearchBox ID="tseUnidadeEnsinoInterprete" runat="server" Caption="" Key="unidade_ens"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                                                GridWidth="850px" OnChanged="tseUnidadeEnsinoInterprete_Changed" SqlOrder="nome_comp">
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
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label5" runat="server" Text="Ano Letivo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAnoInterprete" runat="server" DataTextField="ano" DataValueField="ano"
                                                Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAnoInterprete_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label6" runat="server" SkinID="lblObrigatorio" Text="Período Letivo:*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSemestreInterprete" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="True" DataTextField="periodo" DataValueField="periodo" OnSelectedIndexChanged="ddlSemestreInterprete_SelectedIndexChanged"
                                                Width="70px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label7" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <tweb:TSearchBox ID="tseCursoInterprete" runat="server" Caption="" Key="curso" MaxLength="20"
                                                ArgumentColumns="50" Columns="10" Argument="nome" SqlSelect="SELECT distinct c.curso as curso, nome,mc.DESCRICAO AS modalidade,tc.DESCRICAO AS segmento FROM LY_CURSO C INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO      "
                                                GridWidth="650px" OnChanged="tseCursoInterprete_Changed" SqlOrder="nome">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Curso" FieldName="nome" Width="30%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label8" runat="server" SkinID="lblObrigatorio" Text="Turno:* "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurnoInterprete" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="True" DataTextField="descricao" DataValueField="turno" OnSelectedIndexChanged="ddlTurnoInterprete_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label9" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSerieInterprete" runat="server" DataTextField="SERIE" DataValueField="SERIE"
                                                AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSerieInterprete_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label10" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurmaInterprete" runat="server" DataTextField="TURMA" DataValueField="TURMA"
                                                AutoPostBack="TRUE" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr2" runat="server">
                                        <td style="text-align: right;">
                                            <asp:Label ID="Label12" runat="server" Text="Vigência:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtInicioInterprete" runat="server" Width="120px" Enabled="true"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtInicioInterprete" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            a
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtFimInterprete" runat="server" Width="120px" Enabled="true"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtFimInterprete" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="width: 60%">
                                            &nbsp
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <table>
                            <tr>
                                <td colspan="4" style="text-align: right;">
                                    <asp:Button ID="btnAssociarInterprete" runat="server" ValidationGroup="SalvarForm"
                                        Text="Associar Turma" OnClick="btnAssociarInterprete_Click" OnClientClick="Blooqueio()"/>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <dxwgv:ASPxGridView ID="grdAssociacaoInterprete" runat="server" AutoGenerateColumns="False"
                            Visible="true" ClientInstanceName="grdAssociacaoInterprete" DataSourceID="odsAssociacaoInterprete"
                            KeyFieldName="INTERPRETETURMAID" OnStartRowEditing="grdAssociacaoInterprete_StartRowEditing"
                            OnAfterPerformCallback="grdAssociacaoInterprete_AfterPerformCallback" OnRowUpdating="grdAssociacaoInterprete_RowUpdating">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton Text="Alterar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="INTERPRETETURMAID" Caption="Id" VisibleIndex="1"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="ANO" Caption="Ano" VisibleIndex="4" Visible="true"
                                    ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="SEMESTRE" Caption="Semestre" VisibleIndex="5"
                                    Visible="true" ReadOnly="true">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TURMA" Caption="Turma" VisibleIndex="6"
                                    Visible="true" ReadOnly="true">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="TURNO" Caption="Turno" VisibleIndex="7"
                                    Visible="true" ReadOnly="true">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="8"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAINICIO">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="9"
                                    Width="110px">
                                    <PropertiesDateEdit Width="110px" ClientInstanceName="DATAFIM">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsAssociacaoInterprete" TypeName="Techne.Lyceum.Net.Basico.AssociacaoRecursoEducacaoEspecial"
                            runat="server" SelectMethod="ListaAssociacaoInterprete" OnUpdating="odsAssociacaoInterprete_Updating"
                            UpdateMethod="UpdateInterprete">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseRecurso" Name="recursoId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
