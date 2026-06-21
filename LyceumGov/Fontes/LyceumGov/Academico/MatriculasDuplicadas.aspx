<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MatriculasDuplicadas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MatriculasDuplicadas" %>

<asp:Content ID="contabelaGeral" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function endRequest(sender, e) {
            if (e.get_error()) {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = e.get_error().description.replace(e.get_error().name + ": ", "");
                e.set_errorHandled(true);
            }
            else {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = "";
            }
        }
                
    </script>

    <asp:ScriptManagerProxy ID="manager" runat="server" />
     <asp:Label ID="lblDiretor" runat="server" SkinID="lblMensagem" Text="Prezado(a) diretor(a), <br>Verificamos que alguns alunos da sua unidade de ensino encontram-se matriculados em mais de uma turma para o mesmo ano e período. <br>Pedimos que remova as matrículas das turmas que estão erradas, deixando apenas a turma correta para os alunos abaixo listados. <br>Ao remover a turma errada, notas e faltas lançadas para o aluno na mesma também serão removidas. <br>Desculpe-nos o transtorno."></asp:Label>
     <br />
     <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblSelecione" runat="server" Text="Selecione um aluno:"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dxwgv:ASPxGridView ID="grdAluno" runat="server" EnableRowsCache="False" DataSourceID="odsAlunos"
                EnableViewState="False" EnableCallBacks="false" ClientInstanceName="grdAluno"
                AutoGenerateColumns="False" KeyFieldName="aluno" OnFocusedRowChanged="grdTabela_FocusedRowChanged">
                <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" AllowSort="false" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewDataTextColumn FieldName="aluno" VisibleIndex="1" Caption="Matrícula"
                        Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="nome" VisibleIndex="2" Caption="Nome Completo"
                        Width="250">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <br />
            <br />
            <asp:Label ID="lblTabela" runat="server"></asp:Label>
            <asp:TextBox ID="txtAluno" runat="server" Visible="false" />
            <br />
            <br />
            <dxwgv:ASPxGridView runat="server" ID="grdTurmas" ClientInstanceName="detailGridView"
                DataSourceID="odsTurmas" AutoGenerateColumns="False" KeyFieldName="aluno;turma;ano;semestre"
                EnableCallBacks="false">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsText ConfirmDelete="Confirma a remoção desta turma?" EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn>
                        <EditButton Text="Editar" Visible="false">
                            <Image Url="~/img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Text="Excluir desta turma" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                    </dxwgv:GridViewCommandColumn>
                      <dxwgv:GridViewDataTextColumn FieldName="aluno" Caption="Turma" VisibleIndex="1"
                        Width="100px" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="turma" Caption="Turma" VisibleIndex="1"
                        HeaderStyle-Font-Bold="true" Width="150px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="ano" Caption="Ano" VisibleIndex="2" HeaderStyle-Font-Bold="true"
                        Width="50px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="semestre" Caption="Semestre" VisibleIndex="3"
                        HeaderStyle-Font-Bold="true" Width="50px">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsAlunos" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculasDuplicadas"
                SelectMethod="ConsultarAlunos">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtUnidade" PropertyName="Text" Name="unidade" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsTurmas" runat="server" TypeName="Techne.Lyceum.Net.Academico.MatriculasDuplicadas"
                SelectMethod="ConsultarTurmas" DeleteMethod="Deletar" 
                ondeleted="odsTurmas_Deleted">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtAluno" PropertyName="Text" Name="aluno" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:TextBox ID="txtUnidade" runat="server" Visible="false" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
