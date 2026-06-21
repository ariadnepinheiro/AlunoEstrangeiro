<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="ConsultarMatriculasDuplicadas.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.ConsultarMatriculasDuplicadas" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <script type="text/javascript">
        
        function SelChanged(s, e) {
            url = "";        
            if (e.buttonID == 'btAluno') {                
                url = '../Academico/Alunos.aspx?Chave=';
                grdDuplicidade.GetRowValues(e.visibleIndex, 'ALUNO', OnGridSelectionComplete);
            }
            if (e.buttonID == 'btMatricula') {
                url = '../Academico/Matricula.aspx?Chave=';
                grdDuplicidade.GetRowValues(e.visibleIndex, 'ALUNO', OnGridSelectionComplete);
            }
            if (e.buttonID == 'btEncerramento') {
                url = '../Academico/ListarEncerramentoAluno.aspx?Chave=';
                grdDuplicidade.GetRowValues(e.visibleIndex, 'ALUNO', OnGridSelectionComplete);
            }
        }

        function OnGridSelectionComplete(values) {
            var str = 'aluno=' + values;
            str = Base64.encode(str);
            window.open(url + str, '_blank');
        }
    </script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno" Width="640px">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno: "></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryMatriculasDuplicadas" AutoPostBack="true"
                        OnTextChanged="tseAluno_Changed">
                        </tweb:TSearch>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed" 
                            Key="id_regional" SqlSelect="SELECT distinct R.id_regional, R.regional FROM TCE_REGIONAL R inner join VW_UNIDADE_ENSINO_SITUACAO UE ON ue.id_regional = R.id_regional"
                            SqlOrder="r.id_regional" DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                            Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,id_regional,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                            GridWidth="850px" MaxLength="20" FieldName="Unidade de Ensino" SqlWhere=" id_regional = #tseRegional#"
                            ArgumentColumns="60" Columns="10" OnChanged="tseUnidadeEnsino_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                                
                                <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Visible="false" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Pesquisar" ImageUrl="~/Images/bot_buscar.png"
                            OnClick="btnPesquisar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    <br />
    <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />    
    </ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <dxwgv:ASPxGridView ClientInstanceName="grdDuplicidade" ID="grdDuplicidade" runat="server"
            AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"
            EnableCallBacks="false" OnPageIndexChanged="grdDuplicidade_PageIndexChanged"
            OnAfterPerformCallback="grdDuplicidade_AfterPerformCallback">
            <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="false"/>
            <SettingsText EmptyDataRow="Não existem dados." />
            <Styles Cell-HorizontalAlign = "Center"></Styles>
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" Caption="Aluno" VisibleIndex="0">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btAluno">
                            <Image Url="../img/bt_copiar.png" />
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewCommandColumn ButtonType="Image" Caption="Matrícula" VisibleIndex="1">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btMatricula">
                            <Image Url="../img/bt_copiar.png" />
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>             
                <dxwgv:GridViewCommandColumn ButtonType="Image" Caption="Encerramento" VisibleIndex="2">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btEncerramento">
                            <Image Url="../img/bt_copiar.png" />
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>                          
                <dxwgv:GridViewDataTextColumn Caption="Operadora" FieldName="NOMEOPERADORA" Name="NOMEOPERADORA" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Matricula Aluno" FieldName="ALUNO" Name="ALUNO" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome do Aluno" FieldName="NOME_COMPL" Name="NOME_COMPL" VisibleIndex="5" Width="20%">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação do Aluno" FieldName="SIT_ALUNO" Name="SIT_ALUNO" VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código do Beneficiário" FieldName="IDBENEFICIARIO" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Matrícula Principal" FieldName="FLAGMATRICULAPRINCIPAL" VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data da Inclusão" FieldName="DATAINCLUSAO" VisibleIndex="9">
                    <PropertiesDateEdit EditFormat="Date"> 
                    <%-- DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy">--%>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Disponibilização Operadora" FieldName="DATAATUALIZACAO" VisibleIndex="10">
                    <PropertiesDateEdit EditFormat="Date">
                    </PropertiesDateEdit>                
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            <ClientSideEvents CustomButtonClick="function(s, e) { SelChanged(s, e); }" />            
        </dxwgv:ASPxGridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnPesquisar" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>            
</asp:Content>
