<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ReaberturaAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ReaberturaAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="750px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" OnTextChanged="tseAluno_Changed" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoReabertura"
                        AutoPostBack="true">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 750px;">
        <asp:ImageButton ID="btnReabrir" runat="server" SkinID="BcSalvar" OnClick="btnReabrir_Click"
            ValidationGroup="SalvarForm" Visible="false" />
    </div>
    <asp:Panel ID="pnlGeral" runat="server" Visible="false" Width="750px">
        <asp:Panel ID="pnlDadosAluno" GroupingText="Dados do Aluno" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblUniEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtUnidadeEnsino" runat="server" ReadOnly="true" Width="600px" />
                        <asp:HiddenField ID="hdnCenso" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblSituacao" runat="server" Text="Situação:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtSituacao" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurso" runat="server" ReadOnly="true" Visible="false" />
                        <asp:TextBox ID="txtNomeCurso" runat="server" Width="400px" ReadOnly="true" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTurno" runat="server" ReadOnly="true" Visible="false" />
                        <asp:TextBox ID="txtNomeTurno" runat="server" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCurriculo" runat="server" Text="Currículo: " Visible="false"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCurriculoEncerramento" runat="server" ReadOnly="true" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblSerie" runat="server" Text="Ano de Escolaridade:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNomeSerie" runat="server" ReadOnly="true" Width="600px" />
                        <asp:TextBox ID="txtSerie" runat="server" ReadOnly="true" Visible="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1" GroupingText="Dados Encerramento" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblAnoEncerramento" runat="server" Text="Ano: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAnoEncerramento" runat="server" ReadOnly="true" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblPeriodoEncerramento" runat="server" Text="Período: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPeriodoEncerramento" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Text="Data: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataEncerramento" runat="server" ReadOnly="true" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Motivo: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMotivoEncerramento" runat="server" ReadOnly="true" Visible="false" />
                        <asp:TextBox ID="txtDescMotivoEncerramento" runat="server" ReadOnly="true" Width="200px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlReabertura" runat="server" GroupingText="Informações Reabertura">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblMotivoReabertura" runat="server" Text="Motivo Reabertura:* " Visible="false"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMotivoReabertura" runat="server" Visible="false" DataTextField="descr"
                            DataValueField="item" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblDataReabertura" runat="server" Text="Data Reabertura:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtDataReabertura" runat="server" MinDate="1901-01-01" Width="120px"
                            ClientInstanceName="dtDataReabertura" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje" ReadOnly="false">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblAnoReabertura" runat="server" Text="Ano de Reabertura:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAnoReabertura" runat="server" DataValueField="ano" DataTextField="ano"
                            Width="120px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAnoReabertura_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblPeriodoReabertura" runat="server" Text="Período de Reabertura:* "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodoReabertura" runat="server" DataValueField="periodo"
                            AutoPostBack="True" DataTextField="periodo" Width="120px" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="ddlPeriodoReabertura_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label3" runat="server" Text="Curso:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO
                                                " ArgumentColumns="60" Columns="10" OnChanged="tseCurso_Changed"
                            MaxLength="20" GridWidth="800px" SqlOrder="nome">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Turno:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                            OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged" AutoPostBack="true" Width="200px"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label15" runat="server" Text="Série/Ano Escolar:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" DataValueField="serie"
                            OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged" AutoPostBack="true" Width="200px"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label25" runat="server" Text="Disciplinas Optativas:"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                            Enabled="false" />
                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                            Enabled="false" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnCurriculoReabertura" runat="server" />
        </asp:Panel>   
    </asp:Panel>
</asp:Content>
