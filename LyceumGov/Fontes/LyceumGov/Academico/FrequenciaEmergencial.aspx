<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FrequenciaEmergencial.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.FrequenciaEmergencial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para consulta:"
        Width="623px">
        <asp:HiddenField runat="server" ID="hdnIdControle" />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                        MaxLength="10">
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
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        onchange="Bloqueio()" Width="70px" AutoPostBack="True" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        AutoPostBack="True" Width="70px" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblTurma" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTurma" runat="server" DataTextField="turma" Width="150px"
                        AppendDataBoundItems="true" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Text="Mês de Referência:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMes" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                        <asp:ListItem Text="Fevereiro" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Março" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Maio" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Junho" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Julho" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Setembro" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Outubro" Value="10"></asp:ListItem>
                        <asp:ListItem Text="Novembro" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Dezembro" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3" style="font-weight: bold" class="style2">
                    * Todos os campos são obrigátorios para a realização da busca
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"
                        ValidationGroup="SalvarForm" Style="margin-left: 0px" Width="100px" OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblMensagemLancamento" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div id="divEdit" runat="server" class="divEditBlock" style="width: 950px">
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" ValidationGroup="SalvarForm"
            OnClick="btnSalvar_Click" OnClientClick="return confirm('Confirma a frequência do(s) aluno(s)?');" />
        <asp:Label runat="server" ID="lblBloco" Text="Controle de Frequência Emergencial"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsFrequencia" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlGridMatriculas" runat="server">
        <asp:ObjectDataSource ID="odsMatriculas" runat="server" TypeName="Techne.Lyceum.Net.Academico.FrequenciaEmergencial"
            SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
                <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
                <asp:ControlParameter ControlID="ddlTurma" PropertyName="SelectedValue" Name="turma" />
                <asp:ControlParameter ControlID="ddlMes" PropertyName="SelectedValue" Name="mes" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label ID="lblMensagemFixa" runat="server" SkinID="lblMensagem" ></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="DNL = DIA NÃO LETIVO"></asp:Label>
        <br />
        <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Text="S = SÁBADO"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" SkinID="lblObrigatorio" Text="D = DOMINGO"></asp:Label>
        <br />
        <dxwgv:ASPxGridView ID="grdFrequencia" ClientInstanceName="grdMatriculas" KeyFieldName="aluno"
            DataSourceID="odsMatriculas" runat="server" EnableCallBacks="false" OnHtmlDataCellPrepared="grdFrequencia_HtmlDataCellPrepared">
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                AllowGroup="false" AllowSort="false" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" Name="aluno" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" Name="nome_compl" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="1" Name="1" Width="100px" VisibleIndex="3" FieldName="DIA1">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia1" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA1")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="2" Name="2" Width="100px" VisibleIndex="4" FieldName="DIA2">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia2" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA2")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="3" Name="3" Width="100px" VisibleIndex="5" FieldName="DIA3">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia3" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA3")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="4" Name="4" Width="100px" VisibleIndex="6" FieldName="DIA4">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia4" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA4")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="5" Name="5" Width="100px" VisibleIndex="7" FieldName="DIA5">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia5" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA5")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="6" Name="6" Width="100px" VisibleIndex="8" FieldName="DIA6">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia6" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA6")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="7" Name="7" Width="100px" VisibleIndex="9" FieldName="DIA7">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia7" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA7")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="8" Name="8" Width="100px" VisibleIndex="10" FieldName="DIA8">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia8" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA8")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="9" Name="9" Width="100px" VisibleIndex="11" FieldName="DIA9">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia9" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA9")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="10" Name="10" Width="100px" VisibleIndex="12"
                    FieldName="DIA10">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia10" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA10")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="11" Name="11" Width="100px" VisibleIndex="13"
                    FieldName="DIA11">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia11" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA11")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="12" Name="12" Width="100px" VisibleIndex="14"
                    FieldName="DIA12">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia12" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA12")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="13" Name="13" Width="100px" VisibleIndex="15"
                    FieldName="DIA13">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia13" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA13")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="14" Name="14" Width="100px" VisibleIndex="16"
                    FieldName="DIA14">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia14" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA14")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="15" Name="15" Width="100px" VisibleIndex="17"
                    FieldName="DIA15">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia15" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA15")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="16" Name="16" Width="100px" VisibleIndex="18"
                    FieldName="DIA16">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia16" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA16")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="17" Name="17" Width="100px" VisibleIndex="19"
                    FieldName="DIA17">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia17" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA17")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="18" Name="18" Width="100px" VisibleIndex="20"
                    FieldName="DIA18">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia18" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA18")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="19" Name="19" Width="100px" VisibleIndex="21"
                    FieldName="DIA19">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia19" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA19")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="20" Name="20" Width="100px" VisibleIndex="22"
                    FieldName="DIA20">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia20" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA20")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="21" Name="21" Width="100px" VisibleIndex="23"
                    FieldName="DIA21">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia21" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA21")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="22" Name="22" Width="100px" VisibleIndex="24"
                    FieldName="DIA22">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia22" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA22")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="23" Name="23" Width="100px" VisibleIndex="25"
                    FieldName="DIA23">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia23" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA23")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="24" Name="24" Width="100px" VisibleIndex="26"
                    FieldName="DIA24">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia24" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA24")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="25" Name="25" Width="100px" VisibleIndex="27"
                    FieldName="DIA25">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia25" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA25")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="26" Name="26" Width="100px" VisibleIndex="28"
                    FieldName="DIA26">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia26" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA26")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="27" Name="27" Width="100px" VisibleIndex="29"
                    FieldName="DIA27">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia27" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA27")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="28" Name="28" Width="100px" VisibleIndex="30"
                    FieldName="DIA28">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia28" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA28")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="29" Name="29" Width="100px" VisibleIndex="31"
                    FieldName="DIA29">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia29" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA29")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="30" Name="30" Width="100px" VisibleIndex="32"
                    FieldName="DIA30">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia30" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA30")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="31" Name="31" Width="100px" VisibleIndex="33"
                    FieldName="DIA31">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkFrequencia31" runat="server" Checked='<%# this.VerificarCheck(Eval("DIA31")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
