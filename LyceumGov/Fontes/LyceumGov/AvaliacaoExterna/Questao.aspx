<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Questao.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Questao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function numberToLetter(number) {
            if (number < 1 || number > 5)
                return null;

            return String.fromCharCode(96 + number).toUpperCase();
        }

        function letterToNumber(letter) {
            if (letter.length != 1)
                return;

            letter = letter.toUpperCase();
            if (letter != "A" && letter != "B" && letter != "C" && letter != "D" && letter != "E")
                return null;

            return letter.charCodeAt(0) - 64;
        }

        function validateLetter(evt) {
            evt = evt || window.event;
            var charCode = evt.keyCode || evt.which;
            var charStr = String.fromCharCode(charCode);
            return letterToNumber(charStr) != null;
        }

        function keypress(evt, obj) {
            return validateLetter(evt);
        }

        function keyup(evt, obj) {
            var isValid = validateLetter(evt);
            if (!isValid)
                return;

            obj.value = obj.value.toUpperCase();
            var inputs = $(obj).closest('form').find(':input');
            inputs.eq(inputs.index(obj) + 1).focus();
        }

        function click(evt, obj) {
            obj.setSelectionRange(0, obj.value.length);
        }

        function abrirPopup() {
            window.setTimeout(function() {
                pucConfirmarItinerario.Show();
            }, 1000);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:HiddenField ID="hdnIdQuestao" runat="server" />
    <asp:ObjectDataSource ID="objAno" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="ListaAnos" />
    <asp:ObjectDataSource ID="objAvaliacao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="ListaAvaliacao">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DbType="Int32" PropertyName="SelectedValue"
                Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsProva" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="ListaProva">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAvaliacao" DbType="Int32" PropertyName="SelectedValue"
                Name="avaliacaoId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsComponente" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="ListaComponente"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsHabilidade" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="ListaHabilidade"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsQuestao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Questao"
        runat="server" SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlProva" DbType="Int32" PropertyName="SelectedValue"
                Name="provaId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel runat="server" GroupingText="Selecione a Avaliação, a Etapa e o Ano de Escolaridade:"
        Width="775px">
        <table style="width: 750px;">
            <tr>
                <td style="width: 70px; text-align: right;">
                    <asp:Label ID="lblAvaliacao" runat="server" SkinID="lblObrigatorio" Text="Avaliação:* "></asp:Label>
                </td>
                <td style="width: 680px;" colspan="3">
                    <asp:DropDownList ID="ddlAno" runat="server" DataSourceID="objAno" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                        DataTextField="ANO" DataValueField="ANO" Width="50px" AutoPostBack="true" />
                    <asp:DropDownList ID="ddlAvaliacao" runat="server" DataSourceID="objAvaliacao" DataTextField="DESCRICAO" OnSelectedIndexChanged="ddlAvaliacao_SelectedIndexChanged"
                        DataValueField="AVALIACAOID" Width="377px" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td style="width: 70px; text-align: right;">
                    <asp:Label ID="lblProva" runat="server" SkinID="lblObrigatorio" Text="Prova:* "></asp:Label>
                </td>
                <td style="width: 305px;">
                    <asp:DropDownList ID="ddlProva" runat="server" DataSourceID="odsProva" DataTextField="DESCRICAO" OnSelectedIndexChanged="ddlProva_SelectedIndexChanged"
                        DataValueField="PROVAID" Width="300px" AutoPostBack="true" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>    
    <asp:PlaceHolder ID="plaGrid" runat="server">       
        <br />
        <dxwgv:ASPxGridView ID="grdQuestao" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdQuestao"
            EnableCallBacks="false" DataSourceID="odsQuestao" KeyFieldName="QUESTAOID" OnAfterPerformCallback="grdQuestao_AfterPerformCallback"
            OnStartRowEditing="grdQuestao_StartRowEditing" OnInitNewRow="grdQuestao_InitNewRow" 
            OnCustomButtonCallback="grdQuestao_CustomButtonCallback" OnRowInserting="grdQuestao_RowInserting"
            OnRowUpdating="grdQuestao_RowUpdating" OnCellEditorInitialize="grdQuestao_CellEditorInitialize" Width="80%">
            <Settings ShowFilterRow="false" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />                   
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                onclick="grdQuestao.AddNewRow();" />
                        </div>
                    </HeaderCaptionTemplate>
                    <CancelButton Visible="true" Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <EditButton Visible="True" Text="Editar">
                        <Image Url="../img/bt_editar.png" />
                    </EditButton>                    
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <UpdateButton Visible="true" Text="Alterar">
                        <Image Url="../img/bt_salvar.png" />
                    </UpdateButton>
                    <CustomButtons>                                 
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluir" Text="Delete" Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir" /> 
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="QUESTAOID" VisibleIndex="0"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Prova" Name="PROVA" VisibleIndex="1" FieldName="PROVA"
                    ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Questão*" VisibleIndex="2" FieldName="NUMERO">
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Componente*" FieldName="COMPONENTEID"
                    VisibleIndex="3">
                    <PropertiesComboBox DataSourceID="odsComponente" TextField="DESCRICAO" ValueField="COMPONENTEID"
                        Width="355px" ValueType="System.String" DropDownWidth="355px" ClientInstanceName="ddlComponenteEtapa"
                        EnableSynchronization="False" EnableIncrementalFiltering="True">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Campo COMPONENTE é obrigatório." IsRequired="True" />
                        </ValidationSettings>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { ddlHabilidade.PerformCallback(); }" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Habilidade*" FieldName="HABILIDADEID"
                    VisibleIndex="3">
                    <PropertiesComboBox DataSourceID="odsHabilidade" TextField="CODIGODESCRICAO" ValueField="HABILIDADEID"
                        Width="355px" ValueType="System.String" DropDownWidth="355px" ClientInstanceName="ddlHabilidade"
                        EnableSynchronization="False" EnableIncrementalFiltering="True">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Campo HABILIDADE é obrigatório." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                    <EditItemTemplate>
                        <dxe:ASPxComboBox ID="ddlHabilidade" ClientInstanceName="ddlHabilidade" runat="server"
                            TextField="CODIGODESCRICAO" ValueField="HABILIDADEID" OnLoad="ddlHabilidade_Load">
                        </dxe:ASPxComboBox>
                    </EditItemTemplate>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Índice Dificuldade*" VisibleIndex="4"
                    FieldName="INDICEDIFICULDADE">
                    <PropertiesSpinEdit AllowNull="false" MaxValue="1.000" MinValue="0.000" NumberType="Float"
                        DecimalPlaces="3" Increment="0.100" NumberFormat="Number">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataColumn Caption="Nível Dificuldade*" VisibleIndex="5" FieldName="NIVELDIFICULDADE">
                    <EditItemTemplate>
                        <dxe:ASPxTextBox />
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Alternativas*" VisibleIndex="6" FieldName="QUANTIDADEALTERNATIVAS">
                    <PropertiesSpinEdit AllowNull="false" MaxValue="5" MinValue="2" NumberType="Integer"
                        NumberFormat="Number">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Gabarito*" VisibleIndex="7" FieldName="ALTERNATIVACORRETA">
                    <PropertiesComboBox>
                        <Items>
                            <dxe:ListEditItem Text="A" Value="1" />
                            <dxe:ListEditItem Text="B" Value="2" />
                            <dxe:ListEditItem Text="C" Value="3" />
                            <dxe:ListEditItem Text="D" Value="4" />
                            <dxe:ListEditItem Text="E" Value="5" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plaZero" runat="server">
        <br />       
        <br />
        <h2 style="width: 775px; text-align: center; color: Black;">
            <b>Por favor selecione um componente no painel acima</b></h2>
    </asp:PlaceHolder>
    <dxpc:ASPxPopupControl ID="pucConfirmarItinerario" ClientInstanceName="pucConfirmarItinerario"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label1" Text="Este item será excluído e o resultado recalcuado." runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="lblRespostas" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <br />Confirma a exclusão?
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
