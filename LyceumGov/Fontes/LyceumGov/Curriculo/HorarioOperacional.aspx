<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HorarioOperacional.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.HorarioOperacional" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script>
        function ValidaHora(txtHora) {
            if (txtHora.GetValue() == null) return false;
            {
                var horaIni = txtHora.GetValue();
                var tO = [];
                var intminute;
                var hora;
                var minute;

                tO = horaIni.split(':');
                if ((tO[0] == "") || (tO[0] == " ") || (tO[0] == "  ")) return false;
                if ((tO[1] == "") || (tO[1] == " ") || (tO[1] == "  ")) return false;
                hora = parseInt(tO[0]);
                minute = parseInt(tO[1]);

                if (hora == NaN) return false;
                if ((hora < 0) || (hora > 23)) return false;
                if (minute == NaN) return false;
                if ((minute < 0) || (minute > 59)) return false;
                return true;
            }
        }
        function ChangeHoraFim(txtHoraIni, e) {
            var duracao = document.getElementById("<%=ddlDuracao.ClientID %>")
            var horaFim = '';
            var ret = true;

            //Mudar o valor da hora
            if (parseInt(duracao.value) == 0 || duracao.value == '') {
                alert("Favor informar uma duração para as aulas.");
                return;
            }

            var textFim = eval("txtBoxFim" + txtHoraIni.nullText.toString());

            if (txtHoraIni.GetValue().trim() == ":") {
                textFim.SetText("");
                return;
            }

            if (grdHorarioOperacional != null) {
                if (!ValidaHora(txtHoraIni)) {
                    alert("Favor informar um horário válido (hh:mm)");
                    txtHoraIni.SetFocus();
                    ret = false;
                }
                else {
                    var horaIni = txtHoraIni.GetValue();
                    var tO = [];
                    var intminute;
                    var hora;
                    var minute;
                    tO = horaIni.split(':');
                    if (tO[0].substr(0, 1) == '0')
                        tO[0] = tO[0].substr(1, 1)
                    if (tO[1] != "") {
                        horaFim = parseInt(tO[0]) * 60 + parseInt(tO[1]) + parseInt(duracao.value);
                        tmpV = Math.floor(horaFim / 60);
                        intminute = horaFim % 60

                        if (tmpV == 24)
                            tmpV = 0;

                        if (tmpV < 10)
                            hora = 0 + tmpV.toString();
                        else
                            hora = tmpV;

                        if (intminute < 10)
                            minute = 0 + intminute.toString();
                        else
                            minute = intminute;

                        horaFim = hora + ':' + minute;

                    }
                }
                textFim.SetText(horaFim);

            }
            return ret;
        }
    </script>

    <asp:LinkButton ID="btnPagHorOper"  Text="Voltar para a página da turma" runat="server" style="padding:15px" OnClick="btnVoltarTurma_Click"/>
                        
    <asp:Panel ID="pnDadosBusca" runat="server" GroupingText="Faça uma busca pelos campos abaixo"
        Width="700" style="margin-top:10px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                        Key="id_regional" SqlSelect="SELECT id_regional, regional FROM TCE_REGIONAL"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ID="rfvRegionalPesquisa" runat="server" ControlToValidate="tseRegional"
                        ErrorMessage="Regional: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
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
                <td align="right">
                    <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                        Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,id_regional,municipio, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO"
                        GridWidth="850px" MaxLength="20" FieldName="Unidade de Ensino" SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# "
                        ArgumentColumns="60" Columns="10" OnChanged="tseUnidadeEnsino_Changed" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />						    
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Unidade de Ensino: Preenchimento obrigatório."
                        ID="rfvUnidadeEnsino" runat="server" ControlToValidate="tseUnidadeEnsino" InitialValue=""
                        ValidationGroup="Pesquisar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblUnidadeFisica" runat="server" Text="Unidade Física:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeFisica" runat="server" Argument="nome_comp" ArgumentColumns="60"
                        MaxLength="20" nomeUnidade="" Columns="10" DataType="VarChar" Key="unidade_fis"
                        SqlSelect="SELECT f.unidade_fis, nome_comp FROM vw_zzcro_unidade_fisica f inner join ly_unidades_associadas a on a.unidade_fis = f.unidade_fis"
                        OnChanged="tseUnidadeFisica_Changed" SqlWhere="a.unidade_ens = #tseUnidadeEnsino#"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Unidade Física: Preenchimento obrigatório."
                        ID="rfvUnidadeFisica" runat="server" ControlToValidate="tseUnidadeFisica" InitialValue=""
                        ValidationGroup="Pesquisar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome, tc.descricao FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO"
                        ArgumentColumns="60" Columns="10" MaxLength="20" SqlWhere="unidade_ens = isnull(#tseUnidadeEnsino#,'')"
                        SqlOrder="nome" GridWidth="800px" OnChanged="tseCurso_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Escolaridade: Preenchimento obrigatório."
                        ID="rfvCurso" runat="server" ControlToValidate="tseCurso" InitialValue="" ValidationGroup="Pesquisar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTurno" DataTextField="descricao" DataValueField="turno"
                                    AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ErrorMessage="Turno: Preenchimento obrigatório." ID="RequiredFieldValidator1"
                                    runat="server" ControlToValidate="ddlTurno" InitialValue="" ValidationGroup="Pesquisar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblCurriculo" runat="server" SkinID="lblObrigatorio" Text="Matriz Curricular:* "></asp:Label>
                                <asp:DropDownList ID="ddlCurriculo" runat="server" AutoPostBack="true" DataTextField="CURRICULO"
                                    DataValueField="CURRICULO_ANO" OnSelectedIndexChanged="ddlCurriculo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCurriculo" runat="server" ControlToValidate="ddlCurriculo"
                                    ErrorMessage="Matriz Curricular: Preenchimento obrigatório." InitialValue=""
                                    ValidationGroup="Pesquisar"><img 
                                    src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblSerie" runat="server" Text="Ano de Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSerie" DataTextField="descricao" DataValueField="SERIE"
                                    AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ErrorMessage="Ano de Escolaridade: Preenchimento obrigatório."
                                    ID="rfvSerie" runat="server" ControlToValidate="ddlSerie" InitialValue="" ValidationGroup="Pesquisar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Pesquisar" ImageUrl="~/Images/bot_buscar.png"
                        OnClick="btnPesquisar_Click" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="lblDuracao" Visible="false" runat="server" Text="Duração:* " SkinID="lblObrigatorio"></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlDuracao" Visible="false" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDuracao_SelectedIndexChanged" DataTextField="duracao"
                                DataValueField="duracao" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <p>
                            </p>
                        </td>
                        <td>
                            <p>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <dxwgv:ASPxGridView ID="grdHorarioOperacional" runat="server" AutoGenerateColumns="False"
        Width="400px" SkinID="SkinHorarioOperacional" ClientInstanceName="grdHorarioOperacional"
        KeyFieldName="faculdade;turno;dia_semana;aula;ordem" Visible="False">
        <SettingsBehavior ConfirmDelete="True" AllowSort="False" />
        <SettingsEditing Mode="Inline" />
        <SettingsPager AlwaysShowPager="false" PageSize="50">
        </SettingsPager>
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem tempos cadastrados." />
        <Columns>
            <dxwgv:GridViewDataTextColumn FieldName="ORDEM" VisibleIndex="1" ReadOnly="True"
                Caption="Aula">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aula" FieldName="AULA" Name="AULA" ReadOnly="True"
                Visible="False" VisibleIndex="2" Width="80px">
                <PropertiesTextEdit MaxLength="10">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="Aula deve ser um valor inteiro." ValidationExpression="^\d+$" />
                        <RequiredField ErrorText="Favor informar a Aula." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Hora Início*" FieldName="HORAINI_AULA" Name="HORAINI_AULA"
                VisibleIndex="3" Width="70px">
                <PropertiesTextEdit ValidationSettings-Display="Dynamic">
                    <ValidationSettings Display="Dynamic">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dxe:ASPxTextBox ID="txtBox" runat="server" NullText='<%# Container.VisibleIndex.ToString() %>'
                        ClientInstanceName='<%# "txtBox"+Container.VisibleIndex.ToString() %>' Value='<%# this.FormataHora(Eval("HORAINI_AULA")) %>'
                        Width="60px" MaxLength="5" DisplayFormatString="HH:mm">
                        <MaskSettings Mask="##:##" />
                        <ClientSideEvents LostFocus="function(s, e) { ChangeHoraFim(s,e); }" />
                    </dxe:ASPxTextBox>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Hora Fim*" HeaderStyle-Font-Bold="true" FieldName="HORAFIM_AULA"
                ReadOnly="false" VisibleIndex="4" Width="70px">
                <PropertiesTextEdit ValidationSettings-Display="Dynamic">
                    <ValidationSettings Display="Dynamic">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dxe:ASPxTextBox ID="txtBoxFim" runat="server" NullText='<%# Container.VisibleIndex.ToString() %>'
                        ClientInstanceName='<%# "txtBoxFim"+Container.VisibleIndex.ToString() %>' Value='<%# this.FormataHora(Eval("HORAFIM_AULA")) %>'
                        Width="60px" MaxLength="5" DisplayFormatString="HH:mm" ReadOnly="true">
                        <MaskSettings Mask="##:##" />
                    </dxe:ASPxTextBox>
                </DataItemTemplate>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="4"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Faculdade" FieldName="FACULDADE" VisibleIndex="6"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Dia Semana" FieldName="DIA_SEMANA" VisibleIndex="4"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <br />
    <table>
        <tr>
            <td>
                <div>
                    <dxe:ASPxButton ID="btnSalvar" runat="server" Text="Salvar" Visible="false" AutoPostBack="true"
                        OnClick="btnSalvar_Click">
                    </dxe:ASPxButton>
                </div>
            </td>
            <td>
                <div>
                    <dxe:ASPxButton ID="btnExcluir" runat="server" Text="Excluir" Visible="false" AutoPostBack="true"
                        OnClick="btnExcluir_Click">
                        <ClientSideEvents Click="function(s,e) { e.processOnServer = confirm('Tem certeza que deseja excluir o horario operacional?'); }" />
                    </dxe:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <br />

    <br />
</asp:Content>
