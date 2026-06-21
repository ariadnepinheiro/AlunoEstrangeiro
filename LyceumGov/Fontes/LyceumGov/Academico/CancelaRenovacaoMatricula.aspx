<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CancelaRenovacaoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.CancelaRenovacaoMatricula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="50%">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label1" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" Width="70px" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                        Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnRenovacao" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsRenovacao" TypeName="Techne.Lyceum.Net.Academico.CancelaRenovacaoMatricula"
            runat="server" SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseAluno" DefaultValue="" Name="aluno" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ClientInstanceName="grdRenovacaoMatricula" ID="grdRenovacaoMatricula"
            OnAfterPerformCallback="grdRenovacaoMatricula_AfterPerformCallback" runat="server"
            EnableCallBacks="False" OnCustomButtonInitialize="grdRenovacaoMatricula_CustomButtonInitialize"
            AutoGenerateColumns="False" KeyFieldName="RENOVACAOID" Width="100%" DataSourceID="odsRenovacao"
            OnCustomButtonCallback="grdRenovacaoMatricula_CustomButtonCallback">
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Código" Visible="false" FieldName="RENOVACAOID"
                    Name="RENOVACAOID" VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código" Visible="false" FieldName="COD_SITUACAO_RENOVACAOID"
                    Name="COD_SITUACAO_RENOVACAOID" VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ALUNO" FieldName="ALUNO" Name="ALUNO" VisibleIndex="1"
                    Width="20%" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano Letivo" FieldName="ANO" VisibleIndex="2">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Período Letivo" FieldName="PERIODO" VisibleIndex="3">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO"
                    VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Modalidade / Segmento / Curso" FieldName="MOD_SEG_CURSO"
                    VisibleIndex="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Série/Ano Escolar" FieldName="SERIE" VisibleIndex="6">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="7">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Ensino Religioso" FieldName="ENSINO_RELIGIOSO"
                    Name="ENSINO_RELIGIOSO" VisibleIndex="9">
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkEnsinoReligioso" runat="server" Enabled="false" Checked='<%# this.VerificarCheck(Eval("ENSINO_RELIGIOSO")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Língua Estrangeira Facultativa" FieldName="LINGUA_ESTRANGEIRA_FACULTATIVA"
                    Name="LINGUA_ESTRANGEIRA_FACULTATIVA" VisibleIndex="10">
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Enabled="false" Checked='<%# this.VerificarCheck(Eval("LINGUA_ESTRANGEIRA")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn Caption="Situação" FieldName="SITUACAO_RENOVACAOID" Name="SITUACAO_RENOVACAOID"
                    VisibleIndex="12" Width="150px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Cadastro" Name="DATA_CADASTRO"
                    FieldName="DATA_CADASTRO" Width="100px" Visible="false" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="14" Caption="Data Alteração" Name="DATA_ALTERACAO"
                    FieldName="DATA_ALTERACAO" Width="100px" Visible="false" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo Vaga" FieldName="TIPO_VAGA" VisibleIndex="15">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" VisibleIndex="14"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Codigo Curso" FieldName="COD_CURSO" VisibleIndex="15"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" VisibleIndex="16"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewCommandColumn VisibleIndex="17" ButtonType="Link" Width="50px" Caption="Cancelar">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnCancelar" Text="Cancelar" Visibility="AllDataRows">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <br />
</asp:Content>
