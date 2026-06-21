<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PeriodoConfMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.PeriodoConfMatricula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarInicial.Show();
            }, 1000);
        }

        function abrirPopupCursoSerie() {
            window.setTimeout(function() {
                pucConfirmarCursoSerie.Show();
            }, 1000);
        }     

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Período de Liberação de Confirmação de Matrícula"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsLiberacao" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcLiberacao" runat="server" ActiveTabIndex="0" Width="800px"
        OnTabClick="pcLiberacao_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Período">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:Panel ID="pnAbaInicial" runat="server" Visible="false">
                            <asp:HiddenField ID="hdnIdLiberacao" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                            DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblItiner" runat="server" Text="Data Início:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Data Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
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
                        <asp:ObjectDataSource ID="odsInicial" runat="server" TypeName="Techne.Lyceum.Net.Basico.PeriodoConfMatricula"
                            SelectMethod="Lista" DeleteMethod="Delete"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdInicial" runat="server" DataSourceID="odsInicial" OnAfterPerformCallback="grdInicial_AfterPerformCallback"
                            EnableCallBacks="false" KeyFieldName="PERIODOCONFIRMACAOID" AutoGenerateColumns="false"
                            ClientInstanceName="grdInicial" OnInitNewRow="grdInicial_InitNewRow" OnStartRowEditing="grdInicial_StartRowEditing"
                            OnCustomButtonCallback="grdInicial_CustomButtonCallback">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center" id="dvteste">
                                            <input type="image" id="btnNovoGridInicial" src="../img/bt_novo.png" style="cursor: pointer"
                                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOCONFIRMACAOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CATEGORIAITINERARIOFORMATIVOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano*" Name="ANO" VisibleIndex="2" FieldName="ANO"
                                    Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Período*" Name="PERIODO" VisibleIndex="3"
                                    FieldName="PERIODO" Width="600px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início*" Name="DATAINICIO" VisibleIndex="4"
                                    FieldName="DATAINICIO" Width="400px">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim*" Name="DATAFIM" VisibleIndex="4"
                                    FieldName="DATAFIM" Width="400px">
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Curso/Série">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:HiddenField ID="hdnIdCursoSerie" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; width: 128px">
                                    <asp:Label ID="lblItinerario" runat="server" Text="Ano/Período:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseAnoPeriodo" runat="server" Caption="" SqlSelect="SELECT DISTINCT ANO,PERIODO, DATAINICIO,DATAFIM FROM [Pedagogico].[VW_PERIODOLIBERACAOCONFIRMACAOMAT] "
                                        ArgumentColumns="60" Columns="10" MaxLength="20" Key="PERIODOCONFIRMACAOID" Argument="ANOPERIODO"
                                        DataType="Number" SqlWhere=" CONVERT(DATE, GETDATE()) BETWEEN DATAINICIO AND DATAFIM "
                                        SqlOrder="ANOPERIODO" GridWidth="800px">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PERIODOCONFIRMACAOID" Width="10%" />
                                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="50%" />
                                            <tweb:TSearchBoxColumn Caption="Período" FieldName="PERIODO" Width="50%" />
                                            <tweb:TSearchBoxColumn Caption="Início" FieldName="DATAINICIO" Width="50%" />
                                            <tweb:TSearchBoxColumn Caption="Fim" FieldName="DATAFIM" Width="50%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlAbaCurso" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbCurso" Text="Curso Reprovação<br/>(do Ano/Periodo anterior):*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO"
                                            ArgumentColumns="60" Columns="10" MaxLength="20" GridWidth="800px" SqlOrder="nome"
                                            OnChanged="tseCurso_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbSerie" Text="Série:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" DataValueField="serie"
                                            Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarCursoSerie" runat="server" ValidationGroup="SalvarForm"
                                            Text="Salvar" OnClick="btnSalvarCursoSerie_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:ObjectDataSource ID="odsCursoSerie" runat="server" TypeName="Techne.Lyceum.Net.Basico.PeriodoConfMatricula"
                            SelectMethod="ListaCursoSerie" DeleteMethod="DeleteCursoSerie">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseAnoPeriodo" DefaultValue="" Name="anoperiodo"
                                    PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:Label ID="Label1" Text="Cursos do ano/periodo anterior onde será permitido criação de registro de confirmação pelo Diretor, para alunos reprovados nestes cursos / série." runat="server" SkinID="lblObrigatorio"></asp:Label>
                        <dxwgv:ASPxGridView ID="grdCursoSerie" runat="server" DataSourceID="odsCursoSerie"
                            KeyFieldName="PERIODOCONFIRMACAOCURSOID" AutoGenerateColumns="false" ClientInstanceName="grdCursoSerie"
                            OnInitNewRow="grdCursoSerie_InitNewRow" OnAfterPerformCallback="grdCursoSerie_AfterPerformCallback"
                            EnableCallBacks="false" OnStartRowEditing="grdCursoSerie_StartRowEditing" OnCustomButtonCallback="grdCursoSerie_CustomButtonCallback">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center" id="dvteste">
                                            <input type="image" id="btnNovoGridCursoSerie" src="../img/bt_novo.png" style="cursor: pointer"
                                                title="Novo" onserverclick="HabilitaPnlNovaCursoSerie" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirCursoSerie"
                                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png" Image-Height="15px"
                                            Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOCONFIRMACAOCURSOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOCONFIRMACAOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curso Reprovação<br/>(Ano/Periodo anterior)*" Name="CURSO" VisibleIndex="2" FieldName="CURSO"
                                    Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome Curso*" Name="NOMECURSO" VisibleIndex="3"
                                    FieldName="NOMECURSO" Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Série*" Name="SERIE" VisibleIndex="4" FieldName="SERIE"
                                    Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarInicial" ClientInstanceName="pucConfirmarInicial"
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
                            Confirma a exclusão do Ano/Período de Liberação?<br />
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
    <dxpc:ASPxPopupControl ID="pucConfirmarCursoSerie" ClientInstanceName="pucConfirmarCursoSerie"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma a exclusão do Curso/Série?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSimCursoSerie" runat="server" Text="Sim" OnClick="btnSimCursoSerie_Click" />
                            <asp:Button ID="btnNaoCursoSerie" runat="server" Text="Não" OnClick="btnNaoCursoSerie_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
