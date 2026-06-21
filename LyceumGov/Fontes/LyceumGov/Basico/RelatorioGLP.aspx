<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RelatorioGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.RelatorioGLP" ResponseEncoding="UTF-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        hr { width: 800px; margin: 20px 0; }
        
        #conteudo { height: 565px; }
        
        #tblEscolaAno { width: 800px; }
        #tblEscolaAno td:nth-child(1) { width: 50px; text-align: right; }
        #<%= ddlRegional.ClientID %> { width: 90%; }
        #tblEscolaAno td:nth-child(2) { width: 350px; }
        #<%= ddlMunicipio.ClientID %> { width: 90%; }
        #tblEscolaAno td:nth-child(3) { width: 50px; text-align: right; }
        #<%= ddlEscola.ClientID %>{ width: 90%; }
        #tblEscolaAno td:nth-child(4) { width: 350px; }
        
        #tblModalidade { width: 800px; }
        #tblModalidade td:nth-child(1) { width: 50px; }
        #tblModalidade td:nth-child(2) { width: 350px; }
        #tblModalidade td:nth-child(3) { width: 400px; }
        
        #tblDisciplinaTurno { width: 800px; }
        #tblDisciplinaTurno td:nth-child(1) { width: 50px; text-align: right; }
        #tblDisciplinaTurno td:nth-child(2) { width: 350px; }
        #tblDisciplinaTurno td:nth-child(3) { width: 50px; text-align: right; }
        #tblDisciplinaTurno td:nth-child(4) { width: 350px; }
        #<%= ddlDisciplina.ClientID %> { width: 90%; }
                
        #<%= pnlLotacaoProfessorInscrito.ClientID %> { width: 800px; }
        #<%= pnlIngressoProfessorInscrito.ClientID %> { width: 800px; }
        #<%= pnlSem65Horas.ClientID %> { width: 800px; }
        
        #tblClassificacaoDisciplina { width: 400px; }
        #tblClassificacaoDisciplina td { width: 200px; }
        
        #tblLotacaoProfessorInscrito { width: 400px; }
        #tblLotacaoProfessorInscrito td { width: 200px; }
        
        #tblIngressoProfessorInscrito { width: 775px; }
        #tblIngressoProfessorInscrito td { width: 775px; }
        
        #<%= btnLimpar.ClientID %> { position: relative; left: 595px; }
        #<%= btnFiltrar.ClientID %> { position: relative; left: 600px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">    

   <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
    
    <asp:PlaceHolder ID="plaFiltro" runat="server">
    
    <table id="tblEscolaAno">
        <tr>
            <td>Regional:</td>
            <td>
                <asp:DropDownList ID="ddlRegional" runat="server" AutoPostBack="true" DataSourceID="odsRegional" DataTextField="REGIONAL" DataValueField="ID_REGIONAL" />
                <asp:ObjectDataSource ID="odsRegional" runat="server" TypeName="Techne.Lyceum.Net.Basico.RelatorioGLP" SelectMethod="ListaRegional"></asp:ObjectDataSource>
                 
            </td>
            <td>Município:</td>
            <td>
                <asp:DropDownList ID="ddlMunicipio" runat="server" AutoPostBack="true" DataSourceID="odsMunicipio" DataTextField="NOME" DataValueField="CODIGO" />
                <asp:ObjectDataSource ID="odsMunicipio" runat="server" TypeName="Techne.Lyceum.Net.Basico.RelatorioGLP" SelectMethod="ListaMunicipio">
                    <SelectParameters>
                        <asp:ControlParameter Name="ID_REGIONAL" ControlID="ddlRegional" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>Escola:</td>
            <td>
                <asp:DropDownList ID="ddlEscola" runat="server" DataSourceID="odsEscola" DataTextField="NomeComp" DataValueField="UnidadeEns" />
                <asp:ObjectDataSource ID="odsEscola" runat="server" TypeName="Techne.Lyceum.Net.Basico.RelatorioGLP" SelectMethod="ListaEscola">
                    <SelectParameters>
                        <asp:ControlParameter Name="ID_REGIONAL" ControlID="ddlRegional" PropertyName="SelectedValue" />
                        <asp:ControlParameter Name="MUNICIPIO" ControlID="ddlMunicipio" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td>Ano:</td>
            <td>
                <asp:DropDownList ID="ddlAno" runat="server" AppendDataBoundItems="true" DataSourceID="odsAno" DataTextField="Ano" DataValueField="Ano">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsAno" runat="server" TypeName="Techne.Lyceum.Net.Basico.RelatorioGLP" SelectMethod="ListaAno"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    
    <hr />
    
    <table id="tblModalidade">
        <tr>
            <td>Modalidade:</td>
            <td>
                <asp:CheckBox ID="chkModalidadeMedio" runat="server" Text="Médio / Ensino Fundamental Anos Finais / Semipresencial" data-value="Médio,Ensino Fundamental Anos Finais,Semipresencial" />
            </td>
            <td>
                <asp:CheckBox ID="chkModalidadeFundamental" runat="server" Text="Ensino Fundamental Anos Iniciais" data-value="Ensino Fundamental Anos Iniciais" />
            </td>
        </tr>
    </table>
   
    <br />
    
    <table id="tblDisciplinaTurno">
        <tr>
            <td>Disciplina:</td>
            <td>
                <asp:DropDownList ID="ddlDisciplina" runat="server" DataSourceID="odsDisciplina" DataTextField="DESCRICAO" DataValueField="AGRUPAMENTO" />
                <asp:ObjectDataSource ID="odsDisciplina" runat="server" TypeName="Techne.Lyceum.Net.Basico.RelatorioGLP" SelectMethod="ListaDisciplina">                    
                </asp:ObjectDataSource>
            </td>
            <td>Turno:</td>
            <td>
                <asp:CheckBox ID="chkTurnoManha" runat="server" Text="Manhã" data-value="M" />
                <asp:CheckBox ID="chkTurnoTarde" runat="server" Text="Tarde" data-value="T" />
                <asp:CheckBox ID="chkTurnoNoite" runat="server" Text="Noite" data-value="N" />
            </td>
        </tr>
    </table>
    
    <br />
    
    <asp:Panel ID="pnlLotacaoProfessorInscrito" runat="server" GroupingText="Lotação do Professor Inscrito">
        <table id="tblLotacaoProfessorInscrito">
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblLotacao" runat="server" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="2">
                            <asp:ListItem Text="Todos" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Lotado na mesma escola" Value="MesmaEscola"></asp:ListItem>
                            <asp:ListItem Text="Lotado na mesma Regional" Value="MesmaRegional"></asp:ListItem>  
                            <asp:ListItem Text="Lotado no mesmo Município" Value="MesmoMunicipio"></asp:ListItem>  
                            <asp:ListItem Text="Lotado em outra Regional" Value="OutraRegional"></asp:ListItem>                                   
                      </asp:RadioButtonList>
                  </td>
            </tr>           
        </table>
    </asp:Panel>
    
    <br />
    
    <asp:Panel ID="pnlIngressoProfessorInscrito" runat="server" GroupingText="Ingresso do Professor Inscrito">
        <table id="tblIngressoProfessorInscrito">
            <tr>
                <td>
                 <asp:RadioButtonList ID="rbIngresso" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Text="Todos" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Professores com a mesma disciplina de ingresso (considerar o Docente II em regime de aproveitamento)" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Professores com disciplina diferente a do ingresso" Value="0"></asp:ListItem>                                    
                  </asp:RadioButtonList>
                 </td>
            </tr>           
        </table>
    </asp:Panel>
    
    <br />
    
    <asp:Panel ID="pnlSem65Horas" runat="server" GroupingText="65 Horas">
        <table id="Table1">
            <tr>
                <td><asp:CheckBox ID="chkSem65Horas" runat="server" Text="Excluir professores que já possuem o limite de 65 horas" /></td>
            </tr>
        </table>
    </asp:Panel>
    
    <br />
    
    <asp:Button ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click" Width="100px" />
    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" Width="100px" />
    
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="plaReport" runat="server" Visible="false">
    
    <asp:Button ID="btnVoltar" runat="server" Text="Voltar para o Filtro" OnClick="btnVoltar_Click" Width="150px" />
    
    <br /><br />
    
    <rsweb:ReportViewer ID="rvwDisponibilidade" runat="server" Width="90%" Height="500px">
    </rsweb:ReportViewer>
    
    </asp:PlaceHolder>

       </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFiltrar" />
            <asp:PostBackTrigger ControlID="btnVoltar" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>
