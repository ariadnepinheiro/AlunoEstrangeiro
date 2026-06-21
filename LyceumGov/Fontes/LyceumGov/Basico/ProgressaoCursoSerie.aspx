<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProgressaoCursoSerie.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ProgressaoCursoSerie"
    Title="Progressão de Curso e Série" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe Curso para pesquisar"  Width="70%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="descricao" DataValueField="modalidade"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlModalidade_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Modalidade: Preenchimento obrigatório."
                        ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlModalidade"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="descricao" DataValueField="tipo"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Nível: Preenchimento obrigatório." ID="RequiredFieldValidator6"
                        runat="server" ControlToValidate="ddlNivel" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE "
                        ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                        OnChanged="tseCurso_Changed" SqlOrder="nome" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Curso: Preenchimento obrigatório." ID="RequiredFieldValidator5"
                        runat="server" ControlToValidate="tseCurso" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerie" runat="server"
                        Text="Série:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="serie" DataValueField="serie"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Série: Preenchimento obrigatório." ID="RequiredFieldValidator8"
                        runat="server" ControlToValidate="cmbSerie" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnDados" runat="server" GroupingText="Dados Próximo Curso/Série"
        Width="70%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeProx" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlModalidadeProx" runat="server" DataTextField="descricao"
                        DataValueField="modalidade" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlModalidadeProx_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Modalidade: Preenchimento obrigatório."
                        ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlModalidadeProx"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearchProx" runat="server" Text="Nível/Segmento:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlNivelProx" runat="server" DataTextField="descricao" DataValueField="tipo"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlNivelProx_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Nível/Segmento: Preenchimento obrigatório."
                        ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlNivelProx"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearchProx" runat="server" Text="Próximo Curso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <tweb:TSearchBox ID="tseCursoProx" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE "
                        ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                        OnChanged="tseCursoProx_Changed" SqlOrder="nome" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Próximo Curso: Preenchimento obrigatório."
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="tseCursoProx"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr colspan="4">
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerieProx" runat="server"
                        Text="Próxima Série:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerieProx" runat="server" DataTextField="serie" DataValueField="serie" />
                    <asp:RequiredFieldValidator ErrorMessage="Série: Preenchimento obrigatório." ID="RequiredFieldValidator1"
                        runat="server" ControlToValidate="cmbSerieProx" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>            
        </table>
        <br />
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <br />
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server" Width="90%">
                    <dxwgv:ASPxGridView ID="grdProgressao" runat="server" AutoGenerateColumns="False" 
                        Visible="False" ClientInstanceName="grdProgressao" OnRowDeleting="grdProgressao_RowDeleting"
                        KeyFieldName="IdProgressaoSerie" OnStartRowEditing="grdProgressao_StartRowEditing"
                        OnAfterPerformCallback="grdProgressao_AfterPerformCallback">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <DeleteButton Visible="True" Text="Remover">
                                    <Image Url="../img/bt_exclui2.png" />
                                </DeleteButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="IdProgressaoSerie" Visible="false"
                                ReadOnly="true" VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="Curso" ReadOnly="true" VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Segmento" FieldName="TipoCurso" ReadOnly="true"
                                Visible="true" VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="Serie" ReadOnly="true" Visible="true"
                                VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Próx. Curso" FieldName="ProxCurso" ReadOnly="true"
                                Visible="true" VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Próx. Segmento" FieldName="ProxTipoCurso"
                                ReadOnly="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Próx. Série" FieldName="ProxSerie" ReadOnly="true"
                                Visible="true" VisibleIndex="7">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>                           
                            <dxwgv:GridViewDataTextColumn Caption="Data" FieldName="DataCadastro" ReadOnly="true"
                                VisibleIndex="10">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="Matricula" ReadOnly="true" 
                                VisibleIndex="11">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table> 
</asp:Content>
