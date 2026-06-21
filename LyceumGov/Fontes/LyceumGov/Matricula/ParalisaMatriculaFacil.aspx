<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ParalisaMatriculaFacil.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ParalisaMatriculaFacil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para Consulta:"
        Width="60%">
        <table width="100%">
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="100px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        Width="100px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCurso" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE "
                        ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                        SqlOrder="nome" GridWidth="800px" OnChanged="tseCurso_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnGrid" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <b>Quantitativos de Censo / Série / Turno para o curso:</b>
                </td>
            </tr>
            <tr>
                <td>
                    . Cadastrados no controle de vagas:
                    <asp:Label ID="lblQtdeCursos" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    . Com Participa Matrícula Fácil:
                    <asp:Label ID="lblQtdeParticipaMatricula" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    . Paralisados:
                    <asp:Label ID="lblQtdeParalisado" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    . Não paralisados:
                    <asp:Label ID="lblQtdeNãoParalisados" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    . Candidatos na fila de Espera:
                    <asp:Label ID="lblFila" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr> 
                <td>
                    <asp:Label ID="lblMotivo" runat="server" Text="Altera situação:*" SkinID="lblObrigatorio"></asp:Label>               
					<asp:DropDownList ID="ddlMotivo" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="True" Height="16px">
                        <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                        <asp:ListItem Text="HISTÓRICO - TÉRMINO DO 1º BIMESTRE/TRIMESTRE" Value="27"></asp:ListItem>
                        <asp:ListItem Text="HISTÓRICO - TÉRMINO DO 2º BIMESTRE/TRIMESTRE" Value="28"></asp:ListItem>
                        <asp:ListItem Text="HISTÓRICO - TÉRMINO DO ANO LETIVO" Value="26"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnParalisar" runat="server"  OnClientClick="return confirm('Confirma a paralisação de todas as opões desse curso?');" Text="Paralisar Todos" OnClick="btnParalisar_Click" />
                    <asp:Button ID="btnRetiraParalisar" OnClientClick="return confirm('Confirma a retirada da paralisação de todas as opões desse curso?');" runat="server" Text="Retira Paralisar de Todos"
                        OnClick="btnRetiraParalisar_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxwgv:ASPxGridView ID="grdCurso" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdCurso"
                        DataSourceID="odsControle" KeyFieldName="ID_CONTROLE_VAGA" OnCustomUnboundColumnData="grdCurso_CustomUnboundColumnData"
                        OnAfterPerformCallback="grdCurso_AfterPerformCallback" OnRowUpdating="grdCurso_RowUpdating">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
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
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONTROLE_VAGA" VisibleIndex="1"
                                Visible="false">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="MUNICIPIO_NOME" VisibleIndex="2"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="3"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="4"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" VisibleIndex="5"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Segmento" FieldName="SEGMENTO" VisibleIndex="6"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="NOME_CURSO" VisibleIndex="7"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" VisibleIndex="8"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="NOME_TURNO" VisibleIndex="9"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Vagas Continuidade" FieldName="VAGAS_CONTINUIDADE"
                                ReadOnly="true" VisibleIndex="10">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Vagas Nova" FieldName="VAGAS_NOVAS" VisibleIndex="11"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Total de Vagas " FieldName="VAGAS_LIBERADAS"
                                ReadOnly="true" VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Total Vagas Utilizadas" FieldName="VAGAS_UTILIZADAS"
                                ReadOnly="true" VisibleIndex="13">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Saldo Vagas Disponíveis" UnboundExpression=""
                                ReadOnly="true" Name="VAGAS_DISPONIVEIS" FieldName="VAGAS_DISPONIVEIS" UnboundType="Integer"
                                VisibleIndex="14">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Vaga planejada 2ª Fase" FieldName="VAGAPLANEJADA"
                                ReadOnly="true" VisibleIndex="15">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Participa Matrícula Fácil" FieldName="PARTICIPAMATRICULAFACIL"
                                VisibleIndex="16">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Fila de espera" FieldName="FILAESPERA"
                                ReadOnly="true" VisibleIndex="17">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Oferece Vaga 1ª Fase" FieldName="OFERECEVAGAFASE1"
                                ReadOnly="true" VisibleIndex="18">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Matrícula Fácil Paralisada" FieldName="PARALISAMATRICULAFACIL"
                                VisibleIndex="19" Width="120px">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Visualiza Consulta Vagas" FieldName="VISUALIZAVAGA"
                                ReadOnly="true" VisibleIndex="20">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Alteração" FieldName="DT_ALTERACAO" VisibleIndex="21"
                                ReadOnly="true">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário Responsável" FieldName="MATRICULA"
                                VisibleIndex="22">
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsControle" TypeName="Techne.Lyceum.Net.Matricula.ParalisaMatriculaFacil"
        runat="server" SelectMethod="Listar" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="tseCurso" DefaultValue="" Name="curso" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
