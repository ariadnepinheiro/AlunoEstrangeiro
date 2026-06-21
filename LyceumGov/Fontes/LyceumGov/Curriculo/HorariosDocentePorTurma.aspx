<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HorariosDocentePorTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.HorariosDocentePorTurma" %>

<asp:Content ID="conHorariosDocenteporTurma" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <asp:ObjectDataSource ID="odsHorarioDocente" TypeName="Techne.Lyceum.Net.Curriculo.HorariosDocentePorTurma"
        runat="server" SelectMethod="Listar" DeleteMethod="Delete" UpdateMethod="Update"
        InsertMethod="Insert">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlDocentes" Name="docente" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="lblGradeID" Name="gradeid" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
        <asp:Label ID="lblGradeID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Panel ID="pnDadoscTurma" runat="server" GroupingText="Dados da Turma" Visible="true"
        Width="540px">
        <table>
            <tr>
               <td style="text-align: right;">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAno" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPeriodo" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurno" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblCurso" runat="server" Text="Curso:"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtCurso" runat="server" ReadOnly="true" Width="410px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtUnidadeEnsino" runat="server" ReadOnly="true" Width="410px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblSerie" runat="server" Text="Série:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSérie" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblTurma" runat="server" Text="Turma:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtTurma" runat="server" ReadOnly="true" Width="252px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Selecione o docente que deseja exibir horários:"
        Height="45px" Width="540px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocente" runat="server" Text="Docente*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDocentes" runat="server" AutoPostBack="True" DataTextField="nome"
                        DataValueField="num_func" OnSelectedIndexChanged="ddlDocentes_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDocente" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <br />
        <dxwgv:ASPxGridView ID="grdHorarrioDocente" runat="server" AutoGenerateColumns="False"
            SkinID="NoConfirmDelete" ClientInstanceName="grdHorarrioDocente" KeyFieldName="coordenadoria"
            DataSourceID="odsHorarioDocente">
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="1"
                    Width="100px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome" VisibleIndex="2" Width="250px"
                    ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Dia da Aula" FieldName="dia" VisibleIndex="8"
                    ReadOnly="true" Width="200">
                    <PropertiesComboBox ValueType="System.String" Width="200">
                        <Items>
                            <dxe:ListEditItem Text="Domingo" Value="1" />
                            <dxe:ListEditItem Text="Segunda-Feira" Value="2" />
                            <dxe:ListEditItem Text="Terça-Feira" Value="3" />
                            <dxe:ListEditItem Text="Quarta-Feira" Value="4" />
                            <dxe:ListEditItem Text="Quinta-Feira" Value="5" />
                            <dxe:ListEditItem Text="Sexta-Feira" Value="6" />
                            <dxe:ListEditItem Text="Sábado" Value="7" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataDateColumn Caption="Hora Inicial" FieldName="horainicial" VisibleIndex="9"
                    Width="100px" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="HH:mm">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Hora Final" FieldName="horafinal" VisibleIndex="9"
                    Width="100px" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="HH:mm">
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="disciplina" VisibleIndex="3"
                    Width="250px" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Inicial" FieldName="datainicial" VisibleIndex="9"
                    Width="100px" ReadOnly="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Final" FieldName="datafinal" VisibleIndex="9"
                    Width="100px" ReadOnly="true">
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
        </dxwgv:ASPxGridView>
</asp:Content>
