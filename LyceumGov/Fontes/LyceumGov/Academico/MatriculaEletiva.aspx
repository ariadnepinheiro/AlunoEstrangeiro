<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MatriculaEletiva.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MatriculaEletiva" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvGeral" runat="server">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPeriodo" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="tseAluno" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="tseRegional" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="tseUnidadeEnsino" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="tseCurso" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="rbPrimeiraOpcao" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="rbSegundaOpcao" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlTurno" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlSerie" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlTurma" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="grdAluno" />
                <asp:AsyncPostBackTrigger ControlID="btnPesquisar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnBusca" runat="server" GroupingText="Escolha o tipo de busca que deseja"
                    Width="950px">
                    <table width="100%">
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                                    Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                                    Width="70px" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="height: 1px; width: 100%; border-bottom: inset 1px Blue;">
                    </div>
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="40px" align="center">
                                <asp:RadioButton ID="rbPrimeiraOpcao" GroupName="Busca" runat="server" Text=" " AutoPostBack="True"
                                    OnCheckedChanged="rbPrimeiraOpcao_CheckedChanged" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td align="right" width="114px">
                                            <asp:Label ID="lblAluno" Text="Aluno:* " SkinID="lblObrigatorio" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoEletiva"
                                                AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                                <QueryParameters>
                                                    <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                                </QueryParameters>
                                            </tweb:TSearch>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="height: 1px; width: 100%; border-bottom: inset 1px Blue;">
                    </div>
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="40px" align="center">
                                <asp:RadioButton ID="rbSegundaOpcao" Checked="true" GroupName="Busca" runat="server"
                                    Text=" " AutoPostBack="True" OnCheckedChanged="rbSegundaOpcao_CheckedChanged" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCoordenadoria" Text="Regional:" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                                MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                                SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                                DataType="Number">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                                OnChanged="tseUnidadeResponsavel_Changed" Argument="nome_comp" ColumnName="Faculdade"
                                                SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,ua_atual,ua_antiga,id_regional from VW_UNIDADE_ENSINO_SITUACAO"
                                                MaxLength="20" FieldName="Unidade de Ensino" GridWidth="850px" SqlOrder="nome_comp"
                                                SqlWhere=" ID_REGIONAL = #tseRegional#">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblTCurso" Text="Escolaridade:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" MaxLength="20" ArgumentColumns="50"
                                                Columns="10" SqlSelect=" SELECT DISTINCT C.CURSO,  C.NOME, TP.DESCRICAO,MC.DESCRICAO AS MODALIDADE,C.ITINERARIOFORMATIVO,C.TIPO, C.MODALIDADE AS CODMODALIDADE
                                                                                                        FROM    LY_UNIDADE_ENSINO_CURSOS uc
                                                                                                                JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                                                                                                                INNER JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                                                                                                                INNER JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO  "
                                                SqlWhere=" c.OFERTAELETIVA = 'S' AND C.CURSO <> '9999.80' AND UNIDADE_ENS = #tseUnidadeEnsino#"
                                                GridWidth="850px" OnChanged="tseCurso_Changed" SqlOrder="nome">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Curso" FieldName="curso" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Nome do Curso" FieldName="nome" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Itinerário Formativo" FieldName="itinerarioformativo"
                                                        Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="codmodalidade"
                                                        Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Tipo" FieldName="tipo"
                                                        Width="12%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblTurno" Text="Turno:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurno" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged"
                                                runat="server" AutoPostBack="true" Width="200px" DataTextField="descricao" DataValueField="turno">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblAnoEscolar" Text="Série:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSerie" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged"
                                                AutoPostBack="true" runat="server" Width="200px" DataTextField="serie" DataValueField="serie">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblTurmaTse" runat="server" Text="Turma:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurma" runat="server" DataTextField="turma" DataValueField="turma"
                                                AutoPostBack="true" Width="200px" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table align="right">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Buscar" ImageUrl="~/Images/bot_buscar.png"
                                    OnClick="btnPesquisar_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <br />
                <div id="divEdit" runat="server" class="divEditBlock" style="width: 950px">
                    <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" ValidationGroup="SalvarForm"
                        OnClick="btnSalvar_Click" />
                    <asp:Label runat="server" ID="lblBloco" Text="Enturmação Eletiva" SkinID="BcTitulo" />
                    <asp:ValidationSummary ID="vsEletivas" runat="server" EnableClientScript="true" ShowMessageBox="true"
                        ValidationGroup="SalvarForm" ShowSummary="false" />
                </div>
                <br />
                <div id="dvTurmas" runat="server" visible="false">
                    <dxwgv:ASPxGridView ID="grdAluno" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdAluno"
                        DataSourceID="odsAluno" KeyFieldName="ALUNO" OnHtmlRowCreated="grdAluno_HtmlRowCreated">
                        <clientsideevents endcallback="function(s, e) { OnEndCallBack(s); }" />
                        <settingspager mode="ShowAllRecords" />
                        <columns>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" ReadOnly="true"
                                VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Eletiva 1" FieldName="TURMAELETIVA1" Name="TURMAELETIVA1"
                                UnboundType="String">
                                <DataItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnTurma1" Value='<%# Bind("TURMAELETIVA1") %>' />
                                    <asp:TextBox runat="server" Width="290px" ID="txtTurmaDisciplina1" Enabled="false" Visible="false" Text='<%# Bind("TURMADISCIPLINAELETIVA1") %>' />
                                    <asp:DropDownList ID="ddlGrupo1" runat="server" DataSourceID="odsGrupo1" DataTextField="DISPLINATURMA"
                                        DataValueField="TURMA" Width="300px">
                                    </asp:DropDownList>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Eletiva 2" FieldName="TURMAELETIVA2" Name="TURMAELETIVA2"
                                UnboundType="String">
                                <DataItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnTurma2" Value='<%# Bind("TURMAELETIVA2") %>' />
                                    <asp:TextBox runat="server" Width="290px" ID="txtTurmaDisciplina2" Enabled="false" Visible="false" Text='<%# Bind("TURMADISCIPLINAELETIVA2") %>' />
                                    <asp:DropDownList ID="ddlGrupo2" runat="server" DataSourceID="odsGrupo2" DataTextField="DISPLINATURMA"
                                        DataValueField="TURMA" Width="300px">
                                    </asp:DropDownList>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Eletiva 3" FieldName="TURMAELETIVA3" Name="TURMAELETIVA3"
                                UnboundType="String">
                                <DataItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnTurma3" Value='<%# Bind("TURMAELETIVA3") %>' />
                                    <asp:TextBox runat="server" Width="290px" ID="txtTurmaDisciplina3" Enabled="false" Visible="false" Text='<%# Bind("TURMADISCIPLINAELETIVA3") %>' />
                                    <asp:DropDownList ID="ddlGrupo3" runat="server" DataSourceID="odsGrupo3" DataTextField="DISPLINATURMA"
                                        DataValueField="TURMA" Width="300px">
                                    </asp:DropDownList>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                        </columns>
                        <settings showfilterrow="true" showfilterrowmenu="true" />
                    </dxwgv:ASPxGridView>
                    <asp:HiddenField runat="server" ID="hdnUnidade" />
                    <asp:HiddenField runat="server" ID="hdnCurso" />
                    <asp:HiddenField runat="server" ID="hdnSerie" />
                    <asp:HiddenField runat="server" ID="hdnTurno" />
                    <asp:HiddenField runat="server" ID="hdnItinerario" />
                    <asp:HiddenField runat="server" ID="hdnModalidade" />
                    <asp:HiddenField runat="server" ID="hdnTipo" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ObjectDataSource ID="odsAluno" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculaEletiva"
        SelectMethod="Lista">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" DefaultValue="" Name="aluno" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="ddlTurma" PropertyName="SelectedValue" Name="turma" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo1" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculaEletiva"
        SelectMethod="ListarGrupo1">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnUnidade" DefaultValue="" Name="unidade_ens" PropertyName="Value" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="hdnCurso" DefaultValue="" Name="curso" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnSerie" PropertyName="Value" Name="serie" />
            <asp:ControlParameter ControlID="hdnTurno" PropertyName="Value" Name="turno" />
            <asp:ControlParameter ControlID="hdnItinerario" PropertyName="Value" Name="itinerario" />
            <asp:ControlParameter ControlID="hdnTipo" PropertyName="Value" Name="tipo" />
            <asp:ControlParameter ControlID="hdnModalidade" PropertyName="Value" Name="modalidade" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo2" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculaEletiva"
        SelectMethod="ListarGrupo2">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnUnidade" DefaultValue="" Name="unidade_ens" PropertyName="Value" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="hdnCurso" DefaultValue="" Name="curso" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnSerie" PropertyName="Value" Name="serie" />
            <asp:ControlParameter ControlID="hdnTurno" PropertyName="Value" Name="turno" />
            <asp:ControlParameter ControlID="hdnItinerario" PropertyName="Value" Name="itinerario" />
            <asp:ControlParameter ControlID="hdnTipo" PropertyName="Value" Name="tipo" />
            <asp:ControlParameter ControlID="hdnModalidade" PropertyName="Value" Name="modalidade" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo3" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculaEletiva"
        SelectMethod="ListarGrupo3">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnUnidade" DefaultValue="" Name="unidade_ens" PropertyName="Value" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="hdnCurso" DefaultValue="" Name="curso" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnSerie" PropertyName="Value" Name="serie" />
            <asp:ControlParameter ControlID="hdnTurno" PropertyName="Value" Name="turno" />
            <asp:ControlParameter ControlID="hdnItinerario" PropertyName="Value" Name="itinerario" />
            <asp:ControlParameter ControlID="hdnTipo" PropertyName="Value" Name="tipo" />
            <asp:ControlParameter ControlID="hdnModalidade" PropertyName="Value" Name="modalidade" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
