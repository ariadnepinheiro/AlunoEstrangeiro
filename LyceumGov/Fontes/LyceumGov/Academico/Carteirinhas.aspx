<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Carteirinhas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Carteirinhas" %>
<asp:Content ID="Content" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdatePanelBusca" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
                Width="580px">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                                GridWidth="800" AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                            </tweb:TSearch>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnlMensagem" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="updPnlGrid" runat="server">
        <ContentTemplate>             
             <dxwgv:ASPxGridView ClientInstanceName="grdCarteirinha" ID="grdCarteirinha" runat="server"
                        AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"
                        Visible="false" EnableCallBacks="false">
                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                        <Styles Header-HorizontalAlign = "Center"  Cell-HorizontalAlign = "Center"></Styles>
                        <SettingsText EmptyDataRow="Não existem dados." />                    
                <Columns>
                <dxwgv:GridViewDataDateColumn Caption="Número do Cartão" FieldName="NUMEROCARTAO" VisibleIndex="1"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Número do Chip *" HeaderStyle-Font-Bold="true"
                    FieldName="COD_BARRAS_CARTEIRINHA" VisibleIndex="2" Width="150px">
                    <PropertiesTextEdit MaxLength="30" Width="150px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar o código de barras." IsRequired="True" />
                            <RegularExpression ErrorText="Código de barras deve conter apenas letras e números."
                                ValidationExpression="^[+]?[\w]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Situação Cartão *" HeaderStyle-Font-Bold="true"
                    FieldName="SIT_CARTEIRINHA" VisibleIndex="3" Width="110px">
                    <PropertiesComboBox ValueType="System.String" Width="110px">
                        <Items>
                            <dxe:ListEditItem Text="Ativa " Value="Ativa" />
                            <dxe:ListEditItem Text="Bloqueada" Value="Bloqueada" />
                            <dxe:ListEditItem Text="Cancelada" Value="Cancelada" />
                        </Items>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor selecionar a situação do cartão do aluno." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Motivo do Cancelamento" FieldName="MOTIVO" VisibleIndex="4"
                    Width="250px">
                    <PropertiesTextEdit MaxLength="100" Width="250px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Solicitação do Cartão" FieldName="DT_SOLICITACAO"
                    VisibleIndex="5" Width="100px">
                    <PropertiesDateEdit Width="100px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor selecionar uma data." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Impressão" FieldName="DT_IMPRESSAO" VisibleIndex="6"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Local de Impressão" FieldName="LOCALIMPRESSAO" VisibleIndex="7"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>            
                <dxwgv:GridViewDataDateColumn Caption="Lote de Entrega" FieldName="NUMEROLOTE" VisibleIndex="8"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Entrega Lote" FieldName="DATAENTREGALOTE" VisibleIndex="9"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Confir. Entrega" FieldName="DATACONFIRMACAOENTREGA" VisibleIndex="10"
                    ReadOnly="True" Width="100px">
                    <PropertiesDateEdit Width="100px">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                    Visible="False" VisibleIndex="11">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
