<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SerieSufixo.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.SerieSufixo" %>

<asp:Content ID="ctSerieSufixo" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnPesquisa" runat="server" Width="1020px" GroupingText="Informe os valores abaixo para filtrar">
        <table>
           
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCurso" runat="server"
                        Text="Escolaridade:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct c.curso as curso, nome FROM LY_CURSO c join LY_SERIE s on s.CURSO = c.CURSO"
                        ArgumentColumns="60" Columns="10" MaxLength="20" GridWidth="800px" SqlOrder="nome"
                        OnChanged="tseCurso_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Escolaridade: Preenchimento obrigatório."
                        ID="rfvCurso" runat="server" ControlToValidate="tseCurso" InitialValue="" ValidationGroup="SalvarForm">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTurno" runat="server"
                        Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                        DataSourceID="odsTurno" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged"
                        AutoPostBack="true" Width="200px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Turno: Preenchimento obrigatório." ID="rfvTurno"
                        runat="server" ControlToValidate="ddlTurno" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCurriculo" runat="server"
                        Text="Matriz Curricular:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCurriculo" runat="server" DataTextField="curriculo" DataValueField="curriculo"
                        DataSourceID="odsCurriculo" AutoPostBack="true" OnSelectedIndexChanged="ddlCurriculo_SelectedIndexChanged"
                        Width="200px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Matriz Curricular: Preenchimento obrigatório."
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCurriculo"
                        InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerie" runat="server"
                        Text="Ano de Escolaridade:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSerie" AutoPostBack="true" runat="server" DataTextField="descricao"
                        DataValueField="serie" DataSourceID="odsSerie" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged"
                        Width="200px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Ano de Escolaridade: Preenchimento obrigatório."
                        ID="rfvSerie" runat="server" ControlToValidate="ddlSerie" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="odsTurno" runat="server" TypeName="Techne.Lyceum.RN.Turno"
            SelectMethod="Consultar"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsCurriculo" runat="server" TypeName="Techne.Lyceum.RN.Curriculo"
            SelectMethod="Consultar">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseCurso" Name="pcurso" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="ddlTurno" Name="pturno" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsSerie" runat="server" TypeName="Techne.Lyceum.RN.Serie"
            SelectMethod="Consultar">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseCurso" Name="pcurso" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="ddlTurno" Name="pturno" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="ddlCurriculo" Name="pcurriculo" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdSerieSufixo" runat="server" AutoGenerateColumns="False"
        DataSourceID="tdsSufixoSerie" ClientInstanceName="grdSerieSufixo" KeyFieldName="curso;turno;curriculo;serie;sufixo"
        Width="1020px" Font-Names="Verdana" Font-Size="Small" OnAfterPerformCallback="grdSerieSufixo_AfterPerformCallback"
        OnRowInserting="grdSerieSufixo_RowInserting">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdSerieSufixo.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="curso" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Curriculo" FieldName="curriculo" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Serie" FieldName="serie" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Sufixo*" FieldName="sufixo" HeaderStyle-Font-Bold="true"
                VisibleIndex="1" Width="200px">
                <PropertiesTextEdit MaxLength="2" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Sufixo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                Width="720px" VisibleIndex="3">
                <PropertiesTextEdit MaxLength="100" Width="720px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsSufixoSerie" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_serie_sufixo"
        SqlWhere="curso = @curso and turno = @turno and curriculo = @curriculo and serie = @serie">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseCurso" Name="curso" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlTurno" Name="turno" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlCurriculo" Name="curriculo" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlSerie" Name="serie" PropertyName="SelectedValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
</asp:Content>
