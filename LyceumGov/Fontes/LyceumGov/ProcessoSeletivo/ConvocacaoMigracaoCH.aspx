<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConvocacaoMigracaoCH.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.ConvocacaoMigracaoCH" %>

<asp:Content ID="conConvocacao" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>

    <script type="text/javascript">
        function printpage() { window.print(); }
    </script>

    <script type="text/javascript">
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

    <asp:ValidationSummary ID="vsConvocacao" runat="server" ShowMessageBox="true" ValidationGroup="Selecionar"
        ShowSummary="false" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por:" Width="740px">
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcurso" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
                        ArgumentColumns="50" Columns="30" GridWidth="800px" MaxLength="20" SqlSelect="select LY_CONCURSO_DOCENTE.concurso as concurso, descricao from LY_CONCURSO_DOCENTE"
                        SqlWhere="tipo = 'Migracao'" OnChanged="tseConcurso_Changed" SqlOrder=" ano desc">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" SqlSelect="SELECT ID_REGIONAL,REGIONAL FROM TCE_REGIONAL "
                        SqlOrder="Regional" ArgumentColumns="50" Columns="30" DataType="Number" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Codigo" FieldName="ID_REGIONAL" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="REGIONAL" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox runat="server" ID="tseMunicipioProc" SqlSelect="SELECT DISTINCT MN.CODIGO,MN.NOME FROM MUNICIPIO MN
					INNER JOIN LY_UNIDADE_ENSINO UE ON MN.CODIGO = UE.MUNICIPIO
					INNER JOIN VW_UNIDADE_DE_ENSINO_ATIVAS UEA ON UEA.UNIDADE_ENS = UE.UNIDADE_ENS
					INNER JOIN REGIONAL RG ON RG.POLO = UE.ID_REGIONAL " SqlWhere=" SIT_FUNCIONAMENTO='EmAtividade' and RG.POLO = #tseRegional#"
                        MaxLength="20" ArgumentColumns="50" Columns="30" SqlOrder="NOME" DataType="Varchar" AutoPostBack="true" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Município: Preenchimento obrigatório."
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="tseMunicipioProc"
                        InitialValue="" ValidationGroup="SalvarForm" Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
         
            <tr>
                <td align="right">
                    <asp:Label ID="lblDisciplina" runat="server" Text="Disciplina de Ingresso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseDisciplina" runat="server" Key="agrupamento" Argument="descricao"
                        MaxLength="50" SqlSelect="SELECT DISTINCT gh.AGRUPAMENTO as agrupamento, gh.DESCRICAO as descricao FROM LY_GRUPO_HABILITACAO gh inner join RecursosHumanos.DOCENTECANDIDATO ldh on gh.AGRUPAMENTO = ldh.DISCIPLINAINGRESSO "
                        SqlWhere="ldh.Concurso = #tseConcurso# " SqlOrder=" DESCRICAO"
                        ArgumentColumns="50" Columns="30" OnChanged="tseDisciplina_Changed" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="agrupamento" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
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
    <asp:Panel ID="pnCampos" runat="server" GroupingText="Dados do Processo Seletivo"
        Width="570px" Visible="false">
        <table>            
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDisponivel" runat="server" Text="Inscrições Disponíveis: "></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDisponivel" runat="server" MaxLength="3" Width="50px" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblQuantidade" runat="server" Text="Quantidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtQuantidade" runat="server" MaxLength="3" Width="50px" SkinID="numerico" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvQuantidade" runat="server" ControlToValidate="txtQuantidade"
                                    InitialValue="" ErrorMessage="Quantidade: Preenchimento obrigatório." ValidationGroup="Selecionar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
               <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Data da Publicação da Convocação:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtPublicacaoConvocacao" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                           <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtPublicacaoConvocacao"
                                    InitialValue="" ErrorMessage="Data da Publicação da Convocação: Preenchimento obrigatório."
                                    ValidationGroup="Selecionar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDataApresent" runat="server" Text="Data da Apresentação:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtdataApresent" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvDataApresent" runat="server" ControlToValidate="dtdataApresent"
                                    InitialValue="" ErrorMessage="Data da Apresentação: Preenchimento obrigatório."
                                    ValidationGroup="Selecionar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblHoraApresent" runat="server" Text="Horário da Apresentação:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxTextBox runat="server" ID="dtHoraApresent" MaskSettings-Mask="00:00" NullText=""
                                    Width="50">
                                </dxe:ASPxTextBox>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="vedtHoraApresent" ControlToValidate="dtHoraApresent"
                                    ValidationExpression="([0-1][0-9]|2[0-3]):[0-5][0-9]" ErrorMessage="Hora inválida."
                                    Display="Dynamic" SetFocusOnError="true" runat="server">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtHoraApresent"
                                    InitialValue="" ErrorMessage="Horário da Apresentação: Preenchimento obrigatório." ValidationGroup="Selecionar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="style1">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnSelecionar" runat="server" Text="Selecionar" OnClick="btnSelecionar_Click"
                                    ValidationGroup="Selecionar" Enabled="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnConvocar" runat="server" Text="Convocar" OnClick="btnConvocar_Click"
                                    OnClientClick="Bloqueio();" Enabled="false" />
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnCorreios" runat="server" Text="Correios" Height="24px" Enabled="false"
                                    AutoPostBack="true" Visible="false">
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnCoordenadoria" runat="server" Text="Coordenadoria" Enabled="false"
                                    Visible="false">
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsSelecao" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.ConvocacaoMigracaoCH"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" Name="tseRegional" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseConcurso" Name="tseConcurso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseDisciplina" Name="tseDisciplina" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="txtQuantidade" Name="qtd" PropertyName="Text" />
            <asp:ControlParameter ControlID="dtHoraApresent" Name="dtHoraApresent" PropertyName="Text" />
            <asp:ControlParameter ControlID="dtdataApresent" Name="dtdataApresent" PropertyName="Date" />
            <asp:ControlParameter ControlID="tseMunicipioProc" Name="tseMunicipioProc" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnSelecao" runat="server" GroupingText="Seleção*" Visible="false"
        Width="700px">
        <dxwgv:ASPxGridView ID="grdSelecao" runat="server" AutoGenerateColumns="False" SkinID="NoConfirmDelete"
            DataSourceID="odsSelecao" ClientInstanceName="grdSelecao" KeyFieldName="DOCENTECANDIDATOID"
            Width="700px">
            <SettingsEditing Mode="Inline" />
            <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Inscrição" FieldName="CANDIDATO" VisibleIndex="1"
                    Width="90px" ReadOnly="true">
                    <PropertiesTextEdit MaxLength="8" Width="90px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="2" Width="200px"
                    ReadOnly="true">
                    <PropertiesTextEdit MaxLength="100" Width="200px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data de Nascimento" FieldName="DATANASC" VisibleIndex="3"
                    Width="90px" ReadOnly="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Pontuação" FieldName="PONTUACAO" VisibleIndex="4"
                    Width="100px" CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit MaxLength="10" Width="100px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="5"
                    Width="250">
                    <PropertiesComboBox>
                        <Items>
                            <dxe:ListEditItem Text="Inscrito" Value="1" />
                            <dxe:ListEditItem Text="Convocado" Value="2" />
                            <dxe:ListEditItem Text="Aprovado" Value="3" />
                            <dxe:ListEditItem Text="Faltoso" Value="4" />
                            <dxe:ListEditItem Text="Desistente" Value="5" />
                            <dxe:ListEditItem Text="Desclassificado " Value="6" />
                            <dxe:ListEditItem Text="Migração Concluída" Value="7" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <br /> 
    </asp:Panel>
</asp:Content>
<asp:Content ID="conEstilos" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            height: 70px;
        }
    </style>
</asp:Content>
