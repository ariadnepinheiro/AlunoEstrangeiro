<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AnaliseGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AnaliseGLP" %>

<asp:Content ID="conFuncaoGLP" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdDocenteFuncaoGLP.cpMessage != null)
                lblMensagem.innerHTML = grdDocenteFuncaoGLP.cpMessage;
        }
      
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa"
        Width="700px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:*"
                        SkinID="lblObrigatorio"></asp:Label>
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
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
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
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT distinct ue.unidade_ens, ue.nome_comp, ue.setor, ue.cgc, ue.nucleo,ue.id_regional,ue.municipio,ua_atual,ua_antiga, m.NOME,r.REGIONAL from VW_ZZCRO_UNIDADE_ENSINO ue
                            inner join LY_DOCENTE_FUNCAO_GLP df on df.UNIDADE_ENS = ue.UNIDADE_ENS inner join municipio m on ue.municipio = m.CODIGO INNER JOIN TCE_REGIONAL R ON ue.ID_REGIONAL=R.ID_REGIONAL" OnChanged="tseUnidade_Ensino_Changed"
                        GridWidth="750px" SqlOrder="r.REGIONAL, m.NOME,nome_comp" SqlWhere="ue.id_regional = #tseRegional# and ue.municipio = #tseMunicipio#  and df.STATUS = 'Aguardando' and df.ano = DATEPART(YEAR ,GETDATE()) and df.mes = DATEPART(MONTH ,GETDATE()) " >
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                            
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
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
    <asp:ObjectDataSource ID="odsDocenteFuncaoGLP" TypeName="Techne.Lyceum.Net.Basico.AnaliseGLP"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="id_regional"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidade_ensino"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlAnaliseGLP" runat="server" Visible="false">    
    <dxwgv:ASPxGridView ID="grdDocenteFuncaoGLP" runat="server" AutoGenerateColumns="False"
        Width="90%" Visible="false" ClientInstanceName="grdDocenteFuncaoGLP" DataSourceID="odsDocenteFuncaoGLP"
        KeyFieldName="ID_DOCENTE_FUNCAO_GLP" OnAfterPerformCallback="grdDocenteFuncaoGLP_AfterPerformCallback"
        OnCustomJSProperties="grdDocenteFuncaoGLP_CustomJSProperties" OnCustomButtonCallback="grdDocenteFuncaoGLP_CustomButtonCallback"
        EnableCallBacks="false"  OnCustomColumnDisplayText="grdDocenteFuncaoGLP_CustomColumnDisplayText">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="15" ButtonType="Link" Width="50px" Caption="Aceitar">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnAceitar" Text="Aceitar" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="16" ButtonType="Link" Width="50px" Caption="Reprovar">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnReprovar" Text="Reprovar" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_DOCENTE_FUNCAO_GLP" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENS"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Setor" FieldName="SETOR" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Munícipio" FieldName="MUNICIPIO" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="IDVINCULO" VisibleIndex="2">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="2">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="AGRUPAMENTO" VisibleIndex="4"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DESCRICAO_DISCIPLINA"
                VisibleIndex="5">
            </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="SEGMENTO"
                VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="C.H. Solicitada" FieldName="GLP_SOLICITADA"
                VisibleIndex="8">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUS" VisibleIndex="9">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data" FieldName="DATA" VisibleIndex="10" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Mês" FieldName="MES" VisibleIndex="11" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="12" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="FUNCAO_GLP" VisibleIndex="13"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CH Livre" FieldName="CH_LIVRE" VisibleIndex="14">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CH Livre no Município" FieldName="CH_LIVRE_MUNICIPIO" VisibleIndex="14">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Detalhes" Name="btnDetalhes" VisibleIndex="17"
                Width="50px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <DataItemTemplate>
                    <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("ID_DOCENTE_FUNCAO_GLP") %>'
                        OnCommand="btnDetalhes_Command" ImageAlign="Middle" ImageUrl="~/img/bt_busca.png"
                        Height="15px" AlternateText="Visualizar Detalhes do Pedido"></asp:ImageButton>
                </DataItemTemplate>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsMotivo" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'MotivoReprovarGLP'" SqlOrder="DESCR">
    </techne:TTableDataSource>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="300px">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblConfirmar" runat="server" Text="Selecione o motivo da reprovação:"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:DropDownList ID="cmbMotivo" runat="server" DataValueField="item" DataTextField="descr"
                                DataSourceID="tdsMotivo">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btConfirma" runat="server" Text="Confirma" OnClick="btConfirma_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide();" />
                            <asp:Label ID="hID" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hQtde" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <asp:ObjectDataSource ID="odsTurmasPedido" TypeName="Techne.Lyceum.Net.Basico.AnaliseGLP"
        runat="server" SelectMethod="ListarTurmaPedido">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtRow" Name="id_docente_funcao_glp" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div style="visibility: hidden">
        <input id="txtRow" type="hidden" runat="server" />
    </div>
    <dxpc:ASPxPopupControl ID="pucTurmaPedido" runat="server" CloseAction="CloseButton"
        HeaderText="Turmas do Pedido" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="90%" Height="90%" AllowDragging="True"
        ClientInstanceName="pucTurmaPedido" EnableAnimation="False" EnableViewState="False"
        ShowCloseButton="true">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,18000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="puccItemHistorico" runat="server">
                <dxwgv:ASPxGridView ID="grdTurmasPedido" runat="server" EnableRowsCache="false" DataSourceID="odsTurmasPedido"
                    EnableViewState="false" ClientInstanceName="grdTurmasPedido" AutoGenerateColumns="False"
                    KeyFieldName="ID_DOCENTE_FUNCAO_GLP" Width="95%" Font-Names="Verdana" Font-Size="Small">
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewDataColumn FieldName="ANO" Caption="Ano" VisibleIndex="1">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="PERIODO" Caption="Período" VisibleIndex="2">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="TURMA" Caption="Turma" VisibleIndex="3" />
                        <dxwgv:GridViewDataColumn FieldName="DISCIPLINA" Caption="Disciplina" VisibleIndex="4" />
                        <dxwgv:GridViewDataColumn FieldName="NOMEDISCIPLINA" Caption="Disciplina" VisibleIndex="5" />
                        <dxwgv:GridViewDataColumn FieldName="CARGAHORARIA" Caption="C.H." VisibleIndex="6">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn FieldName="ID_DOCENTE_FUNCAO_GLP" VisibleIndex="5" Visible="false">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    </asp:Panel>
</asp:Content>
