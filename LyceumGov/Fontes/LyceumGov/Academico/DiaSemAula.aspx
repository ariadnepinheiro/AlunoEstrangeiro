<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DiaSemAula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.DiaSemAula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }       

    </script>

</asp:Content>
<asp:Content ID="conReposicaoAula" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div>
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    </div>
    <br />
    <div class="divEditBlock" style="width: 742px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Reposição Aula" SkinID="BcTitulo" />
    </div>
    <div>
        <asp:Panel ID="pnlReposicao" runat="server" Width="60%" GroupingText="Dados da Reposição Aula">
            <br />
            <table id="tbReposicao" runat="server">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblData" runat="server" Text="Dia sem Aula:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtDataSemAula" runat="server" Width="120px" Enabled="true"
                            EnableDefaultAppearance="true" ClientInstanceName="dtDataSemAula" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Processo SEI*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProcessoSEI" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblMotivo" runat="server" Text="Motivo Suspensão da Aula:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMotivo" runat="server" DataTextField="DESCRICAO" DataValueField="MOTIVODIASEMAULAID"
                            CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblJustificativa" runat="server" Text="Justificativa:* "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtJustificativa" runat="server" Height="100px" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Data Reposição:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtReposicao" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                            ClientInstanceName="dtReposicao" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
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
    </div>
    <div>
        <br />
        <br />
        <asp:ObjectDataSource ID="odsMotivoSuspensao" runat="server" SelectMethod="Lista"
            TypeName="Techne.Lyceum.RN.Turmas.MotivoDiaSemAula"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsReposicao" runat="server" TypeName="Techne.Lyceum.Net.Academico.DiaSemAula"
            SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidade" PropertyName="DBValue" Name="unidade" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdReposicao" runat="server" DataSourceID="odsReposicao" EnableCallBacks="false"
            KeyFieldName="DIASEMAULAID" AutoGenerateColumns="false" ClientInstanceName="grdReposicao"
            OnInitNewRow="grdReposicao_InitNewRow" OnStartRowEditing="grdReposicao_StartRowEditing"
            OnCustomButtonCallback="grdReposicao_CustomButtonCallback" Width="700px" OnAfterPerformCallback="grdReposicao_AfterPerformCallback">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows"
                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="DIASEMAULAID"
                    Visible="false" Width="700px">
                    <PropertiesTextEdit MaxLength="200">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="2" Caption="Dia Sem Aula" Name="DATA"
                    FieldName="DATA" Width="100px" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" Name="TURNO" VisibleIndex="3" FieldName="TURNO"
                    Width="400px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turma" Name="TURMA" VisibleIndex="4" FieldName="TURMA"
                    Width="400px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" Name="COMPONENTECURRICULAR"
                    VisibleIndex="5" FieldName="COMPONENTECURRICULAR" Width="400px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tempo/Aula" Name="TEMPOAULA" VisibleIndex="6"
                    FieldName="TEMPOAULA" Width="400px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Processo SEI" Name="PROCESSOSEI" VisibleIndex="6"
                    FieldName="PROCESSOSEI" Width="400px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Motivo Suspensão Aula" HeaderStyle-Font-Bold="true"
                    FieldName="MOTIVODIASEMAULAID" VisibleIndex="7" Width="110px">
                    <PropertiesComboBox DataSourceID="odsMotivoSuspensao" TextField="DESCRICAO" ValueField="MOTIVODIASEMAULAID"
                        ValueType="System.String">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar o motivo." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Justificativa" Name="JUSTIFICATIVA" VisibleIndex="8"
                    FieldName="JUSTIFICATIVA" Width="400px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Reposição" Name="DATAREPOSICAO"
                    FieldName="DATAREPOSICAO" Width="100px" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
    <asp:HiddenField ID="hdnIdDiaSemAula" runat="server" />
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma a exclusão da Reposição da Aula?<br />
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
