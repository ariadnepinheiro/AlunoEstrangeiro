<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alunos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Alunos"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="conAlunos" ContentPlaceHolderID="cphFormulario" runat="server">
    <style>
        #video
        {
            border: 1px solid black;
            box-shadow: 2px 2px 3px black;
            width: 400px;
            height: 400px;
        }
        #photo
        {
            border: 1px solid black;
            box-shadow: 2px 2px 3px black;
            width: 400px;
            height: 400px;
        }
        #canvas
        {
            display: none;
        }
        .camera
        {
            width: 400px;
            display: inline-block;
        }
        .output
        {
            width: 340px;
            display: inline-block;
        }
        .contentarea
        {
            font-size: 16px;
            font-family: "Lucida Grande" , "Arial" , sans-serif;
            width: 760px;
        }
    </style>

    <script type="text/javascript">
	(function() {
      // The width and height of the captured photo. We will set the
      // width to the value defined here, but the height will be
      // calculated based on the aspect ratio of the input stream.

      var width = 320;    // We will scale the photo width to this
      var height = 0;     // This will be computed based on the input stream

      // |streaming| indicates whether or not we're currently streaming
      // video from the camera. Obviously, we start at false.

      var streaming = false;

      // The various HTML elements we need to configure or control. These
      // will be set by the startup() function.

      var video = null;
      var canvas = null;
      var photo = null;
      var startbutton = null;

      function startup() {
        video = document.getElementById('video');
        canvas = document.getElementById('canvas');
        photo = document.getElementById('photo');
        startbutton = document.getElementById('startbutton');
        
        if (navigator.mediaDevices != undefined){
        navigator.mediaDevices.getUserMedia({video: true, audio: false})
        .then(function(stream) {
          video.srcObject = stream;
          video.play();
        })
        .catch(function(err) {
          //console.log("An error occurred: " + err);
        });
        }
        

        video.addEventListener('canplay', function(ev){
          if (!streaming) {
            height = video.videoHeight / (video.videoWidth/width);
          
            // Firefox currently has a bug where the height can't be read from
            // the video, so we will make assumptions if this happens.
          
            if (isNaN(height)) {
              height = width / (4/3);
            }
          
            video.setAttribute('width', width);
            video.setAttribute('height', height);
            canvas.setAttribute('width', width);
            canvas.setAttribute('height', height);
            streaming = true;
          }
        }, false);

        startbutton.addEventListener('click', function(ev){
          takepicture();
          ev.preventDefault();
        }, false);
        
        clearphoto();
      }

      // Fill the photo with an indication that none has been
      // captured.
     
      function clearphoto() {
        var context = canvas.getContext('2d');
        context.fillStyle = "#AAA";
        context.fillRect(0, 0, canvas.width, canvas.height);

        var data = canvas.toDataURL('image/png');
        photo.setAttribute('src', data);
      }
      
      // Capture a photo by fetching the current contents of the video
      // and drawing it into a canvas, then converting that to a PNG
      // format data URL. By drawing it on an offscreen canvas and then
      // drawing that to the screen, we can change its size and/or apply
      // other changes before drawing it.

      function takepicture() {
        var context = canvas.getContext('2d');
        if (width && height) {
          canvas.width = width;
          canvas.height = height;
          context.drawImage(video, 0, 0, width, height);
        
          var data = canvas.toDataURL('image/jpeg');
            photo.setAttribute('src', data);
            $("#<%= this.hdnFotoUrl.ClientID %>").val(data);
            
        } else {
          clearphoto();
        }
      }

      // Set up our event listener to run the startup process
      // once loading is complete.
      window.addEventListener('load', startup, false);
    })();
    </script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/conversorImagem.js") %>'></script>

    <script src='<%=ResolveUrl("~/Scripts/jquery.webcam.js") %>' type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });

            AddEvents();

            $("#<%= this.txtMunicipio.ClientID %>").attr("readonly", "readonly");

            $("#<%= this.txtEmailConfirmacao.ClientID %>").bind("cut copy paste", function(e) {
                e.preventDefault();
            });

            $("#<%= this.txtEmail.ClientID %>").bind("cut copy paste", function(e) {
                e.preventDefault();
            });

            MudouEmail();
        });

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total <= quant) {
                var resto = quant - total;
            }
            else {
                $(campo).val(valor.substr(0, quant));
            }
        }

        function AddEvents() {
            $("input[id*='chkEntrega']").click(function() {
                controlRelatedFields(this);
            });

            $("input[id*='chkEntrega']").each(function() {
                controlRelatedFields(this);
            });

            $("input[id*='txtDataEntrega']").each(function() {
                $(this).mask("99/99/9999");
            });
        }

        function controlRelatedFields(field) {
            var txtDataEntrega = $("#" + $(field).attr("entrega"));
            var isChecked = $(field).is(':checked');

            if (isChecked) {
                $(txtDataEntrega).removeAttr("readonly");
                $(txtDataEntrega).removeAttr("disabled");
                $(txtDataEntrega).css("background-color", "");
            }
            else {
                $(txtDataEntrega).val("");
                $(txtDataEntrega).attr("readonly", "readonly");
                $(txtDataEntrega).attr("disabled", true);
                $(txtDataEntrega).css("background-color", "Gainsboro");
            }
        }

        function ShowLoginWindow() {
            //            teste
            $("#<%= this.hdnFotoUrl.ClientID %>").val("S");
            //            teste

            $("#<%= this.hdnFoto.ClientID %>").val("S");
            document.forms['<%=Form.ClientID %>'].submit();
            pcFoto.Show();
        }

        function fecharPopupFotos() {
            $("#<%= this.hdnFoto.ClientID %>").val("N");
            pcFoto.Hide();
        }

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function onlyNumbers() {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.keyCode = 0;
            }
        }


        function CheckedChangedMae(objeto, objetoDeclaracao) {
            var check = window.document.getElementById(objeto).checked;

            if (check) {
                if (window.confirm('Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome da mãe.'))
                    window.document.getElementById(objetoDeclaracao).checked = check;
                else {
                    window.document.getElementById(objetoDeclaracao).checked = (!check);
                }
            }
            else {
                window.document.getElementById(objetoDeclaracao).visible = check;
            }
        }

        function CheckedNecessidadeEspecial(objetoDeclaracao) {
            var check = window.document.getElementById(objetoDeclaracao).checked;

            if (check) {
                if (window.confirm('Senhor(a) Diretor(o), confirma estar de posse do Laudo Médico ou Parecer Pedagógico?')) {
                    window.document.getElementById(objetoDeclaracao).checked = check;
                }
                else {
                    window.document.getElementById(objetoDeclaracao).checked = (!check);
                }
            }
            else {
                window.document.getElementById(objetoDeclaracao).visible = check;
            }
        }

        function CheckedChangedPai(objeto, objetoDeclaracao) {
            var check = window.document.getElementById(objeto).checked;

            if (check) {
                if (window.confirm('Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome do pai.'))
                    window.document.getElementById(objetoDeclaracao).checked = check;
                else {
                    window.document.getElementById(objetoDeclaracao).checked = (!check);
                }
            }
            else {
                window.document.getElementById(objetoDeclaracao).visible = check;
            }
        }

        function abrirPopupAlunoNovo() {
            window.setTimeout(function() {
                pcNovoAluno.Show();
            }, 1000);
        }

        function MarcaMapa() {
            var hdnMarcarMapa = document.getElementById("<%=hdnMarcarMapa.ClientID %>");
            hdnMarcarMapa.value = "MarcarMapa";
        };

        function fecharPopupAlunoNovo() {
            window.setTimeout(function() {

                pcNovoAluno.Hide();
            }, 1000);
        }

        function abrirPopupConfirmarTransf() {
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function abrirPopupAEDH() {
            window.setTimeout(function() {
                pucConfirmarAEDH.Show();
            }, 1000);
        }

        function nomeSemNum(b) {
            var a;

            if (window.event) {
                a = window.event.keyCode
            }
            else {
                if (event) {
                    a = event.keyCode
                }
                else {
                    if (b) {
                        a = b.which
                    }
                    else {
                        return true
                    }
                }
            }

            if ((a >= 65 && a <= 90) || (a >= 97 && a <= 122) || (a >= 192 && a <= 255) || (a == 32) || (a == 45) || (a == 39)) {
                return true
            }
            else {
                return false
            }

            return true
        }


        function removeEspacosDuplicados(a) {
            a = a.replace(/\s{2,}/g, ' ');
        }

        function primeiraLinhaPaginaCorrente() {
            primeiraLinhaPaginaCorrente = App.PagingToolbar1.getPageData().fromRecord;
        }

        function retiraSelecaoCheckRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo) {
            var checkNenhum = document.getElementById(strCheckNenhum);

            if (checkNenhum.checked) {
                var chkRecursoAplicacaoProva = document.getElementById(strRecursoAplicacaoProva).getElementsByTagName("input");
                for (i = 0; i < chkRecursoAplicacaoProva.length; i++) {
                    if (chkRecursoAplicacaoProva[i].type == "checkbox") {
                        chkRecursoAplicacaoProva[i].checked = false;
                    }
                }

                var rblRecursoAplicaProvaExclusivo = document.getElementById(strRecursoAplicaProvaExclusivo).getElementsByTagName("input");
                for (i = 0; i < rblRecursoAplicaProvaExclusivo.length; i++) {
                    if (rblRecursoAplicaProvaExclusivo[i].type == "radio") {
                        rblRecursoAplicaProvaExclusivo[i].checked = false;
                    }
                }
            }
            else {
                retiraSelecaoCheckNenhumRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo);
            }
        }

        function retiraSelecaoCheckNenhumRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo) {
            var habilitaNenhum = true;

            var chkRecursoAplicacaoProva = document.getElementById(strRecursoAplicacaoProva).getElementsByTagName("input");
            for (i = 0; i < chkRecursoAplicacaoProva.length; i++) {
                if (chkRecursoAplicacaoProva[i].type == "checkbox") {
                    if (chkRecursoAplicacaoProva[i].checked) {
                        habilitaNenhum = false;
                        break;
                    }
                }
            }

            var rblRecursoAplicaProvaExclusivo = document.getElementById(strRecursoAplicaProvaExclusivo).getElementsByTagName("input");
            for (i = 0; i < rblRecursoAplicaProvaExclusivo.length; i++) {
                if (rblRecursoAplicaProvaExclusivo[i].type == "radio") {
                    if (rblRecursoAplicaProvaExclusivo[i].checked) {
                        habilitaNenhum = false;
                        break;
                    }
                }
            }

            document.getElementById(strCheckNenhum).checked = habilitaNenhum;
        }

        function MudouEmail() {
            var emailAntigo = $("#<%= this.hddTxtEmail.ClientID %>").val();
            var emailNovo = $("#<%= this.txtEmail.ClientID %>").val();

            if (emailAntigo != emailNovo) {
                $("#<%= this.txtEmailConfirmacao.ClientID %>").removeAttr("disabled");
            }
            else {
                $("#<%= this.txtEmailConfirmacao.ClientID %>").val("");
                $("#<%= this.txtEmailConfirmacao.ClientID %>").attr("disabled", true);

            }
        }

        function txtEmailTextChanged() {
            MudouEmail();
        }

        function txtEmailTextBlur() {
            var emailAntigo = $("#<%= this.hddTxtEmail.ClientID %>").val();
            var emailNovo = $("#<%= this.txtEmail.ClientID %>").val();
            var emailConfirma = $("#<%= this.txtEmailConfirmacao.ClientID %>").val();

            if ((emailAntigo != emailNovo) && (emailConfirma === "")) {
                $("#<%= this.txtEmailConfirmacao.ClientID %>").focus();
            }
        }

        function EmailEmptyValidator(source, args) {

            var emailAntigo = $("#<%= this.hddTxtEmail.ClientID %>").val();
            var emailNovo = $("#<%= this.txtEmail.ClientID %>").val();
            var emailConfirma = $("#<%= this.txtEmailConfirmacao.ClientID %>").val();
            var gratuidade = $("#<%= this.ddlGratuidade.ClientID %>").val();

            if (gratuidade == "S") {
                if ((emailAntigo != emailNovo) && (emailConfirma == "")) {
                    var msg = "O E-mail deverá ser confirmado antes de salvar o cadastro";

                    args.IsValid = false;
                    source.innerHTML = "<span><img src='../Images/AlertaMens.gif' alt='" + msg + "' />" + msg + "</span>";

                    return;
                }
            }
        }
        var pageUrl = '<%=ResolveUrl("../Academico/Alunos.aspx") %>';
        $(function() {
            var foto = $("#<%= this.hdnFoto.ClientID %>").val();
            if (foto == "S") {
                jQuery("#webcam").webcam({
                    width: 400,
                    height: 400,
                    mode: "save",
                    swffile: '<%=ResolveUrl("../Scripts/jscam.swf") %>',
                    onSave: function(data) {
                        $.ajax({
                            type: "POST",
                            url: pageUrl + "/GetCapturedImage",
                            data: '',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function(r) {

                                $("#<%= this.hdnFoto.ClientID %>").val("N");
                            },
                            failure: function(response) {
                                alert(response.d);
                            }
                        });
                    },
                    onCapture: function() {
                        webcam.save(pageUrl);
                    }
                });
            }
        });

        function Capture() {
            webcam.capture();
            return false;
        }

        function ConfirmaFoto() {
            pcFoto.Hide();

            return false;
        }


        function formataFixoCelularDDD(b, a) {

            vr = b.value = filtraNumeros(filtraCampo(b));
            tam = vr.length;
            if (tam < 10)
                return;

            if (tam == 11) {
                formataCelularDDD(b, a);
            }
            if (tam == 10) {
                formataTelefoneDDD(b, a);

            }
        }

        
    </script>

    <script type="text/javascript">
        window.onload = function() {
            if (!abriu) {
                var elem = document.getElementById('divPopupBloqueado');
                if (typeof elem != 'undefined' && elem != null) {
                    elem.style.display = 'block';
                }
            }
        }
    </script>

    <!-- Leaflet.js - OpenStreetMap -->
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>

    <script type="text/javascript">
    var map = null;
    var pin = null;

    // Destrói e reinicializa o mapa. Chamada pelo code-behind via RegisterStartupScript.
    function InicializarMapa(lat, lng) {
        // Destrói instância anterior para evitar o bug do mapa preso
        if (map) {
            map.remove();
            map = null;
            pin = null;
        }

        var centerLat = (lat !== null && !isNaN(lat)) ? lat : -22.9068;
        var centerLng = (lng !== null && !isNaN(lng)) ? lng : -43.1729;

        map = L.map('mapa').setView([centerLat, centerLng], 15);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
            maxZoom: 19
        }).addTo(map);

        // Se já tem coordenadas salvas, coloca o pin
        if (lat !== null && !isNaN(lat) && lng !== null && !isNaN(lng)) {
            AdicionarPin(lat, lng);
        }
    }

    function AdicionarPin(lat, lng) {
        if (pin) { map.removeLayer(pin); }
        pin = L.marker([lat, lng], { draggable: true }).addTo(map);
        map.setView([lat, lng], 15);
        pin.on('dragend', function (e) {
            var pos = e.target.getLatLng();
            PreencherLatitudeLongitude(pos.lat, pos.lng);
            alert('Coordenadas atualizadas. Para voltar à posição inicial do endereço, clique novamente no botão Obter Coordenadas.');
        });
    }

    function MarcaMapa() {
        var hdnMarcarMapa = document.getElementById('<%= hdnMarcarMapa.ClientID %>');
        hdnMarcarMapa.value = 'MarcarMapa';
        BuscarEndereco();
        return false;
    }

    function BuscarEndereco() {
        var endereco  = document.getElementById('<%= txtEndereco.ClientID %>');
        var numero    = document.getElementById('<%= txtEndNum.ClientID %>');
        var bairro    = document.getElementById('<%= txtBairro.ClientID %>');
        var municipio = document.getElementById('<%= txtMunicipio.ClientID %>');
        var uf        = document.getElementById('<%= txtEstado.ClientID %>')
                     || document.querySelector('[id$="txtEstado"]');

        if (!endereco || !endereco.value || !municipio || !municipio.value) return;

        var partes = [endereco.value.trim()];
        if (numero    && numero.value.trim())    partes.push(numero.value.trim());
        if (bairro    && bairro.value.trim())    partes.push(bairro.value.trim());
        partes.push(municipio.value.trim());
        if (uf        && uf.value.trim())        partes.push(uf.value.trim());
        partes.push('Brasil');

        var url = 'https://nominatim.openstreetmap.org/search?format=json&limit=1&q='
                  + encodeURIComponent(partes.join(', '));

        fetch(url, { headers: { 'Accept-Language': 'pt-BR', 'User-Agent': 'ConexaoEducacaoRJ/1.0' } })
        .then(function(r) { return r.json(); })
        .then(function(data) {
            if (data && data.length > 0) {
                var lat = parseFloat(data[0].lat);
                var lng = parseFloat(data[0].lon);
                if (!map) InicializarMapa(lat, lng);
                AdicionarPin(lat, lng);
                PreencherLatitudeLongitude(lat, lng);
            }
            // Sem resultado: mapa permanece na posição atual, sem alert intrusivo
        })
        .catch(function(err) { console.error('Erro geocodificação:', err); });
    }

    function PreencherLatitudeLongitude(lat, lng) {
        document.getElementById('<%= txtLatitude.ClientID %>').value = lat;
        document.getElementById('<%= txtLongitude.ClientID %>').value = lng;
    }
</script>

    <asp:HiddenField ID="hdnDataInicio" runat="server" />
    <asp:HiddenField ID="hdnDataFim" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <dxpc:ASPxPopupControl ID="pcFoto" runat="server" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ClientInstanceName="pcFoto" HeaderText="Fotografar"
        AllowDragging="True" EnableAnimation="False" EnableViewState="False" ShowCloseButton="false"
        Width="630px" Height="250px">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
                <dxp:ASPxPanel runat="server" DefaultButton="btOK" ID="Panel1">
                    <PanelCollection>
                        <dxp:PanelContent runat="server">
                            <div class="contentarea">
                                <div class="camera">
                                    <video id="video">Video stream not available.</video>
                                    <%--<button id="btnt" visible="false" >Take photo</button> --%>
                                    <div>
                                    </div>
                                    <%--<asp:Button ID="btnCapture" Text="Capturar Foto" runat="server" OnClientClick="Bloqueio();return Capture();" />--%>
                                    <button id="startbutton">
                                        Capturar Foto</button>
                                    <asp:Button ID="btnFecharFoto" Text="Cancelar" runat="server" OnClientClick="fecharPopupFotos();" />
                                </div>
                                <canvas id="canvas">
  </canvas>
                                <div class="output">
                                    <img id="photo" alt="The screen capture will appear in this box.">
                                    <asp:Button ID="btnConfirmaFoto2" OnClientClick="Bloqueio()" Text="Confirmar Foto"
                                        runat="server" OnClick="btnConfirmaFoto_Click" />
                                </div>
                            </div>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed" OnLoad="tseAluno_Load">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField runat="server" ID="hdnFoto" Value="N" />
    <asp:HiddenField runat="server" ID="hdnCompartilhada" />
    <asp:HiddenField runat="server" ID="hdnUnidade" />
    <asp:HiddenField runat="server" ID="hdnFotoUrl" Value="N" />
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Alunos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <dxtc:ASPxTabControl ID="tab" runat="server" Width="940px" ActiveTabIndex="0" OnTabClick="tab_TabClick"
        AutoPostBack="True" SyncSelectionMode="None" Visible="false">
        <TabStyle Wrap="True" Width="105px">
        </TabStyle>
        <Tabs>
            <dxtc:Tab Text="Dados Pessoais" Name="tabDadosPessoais">
            </dxtc:Tab>
            <dxtc:Tab Text="Dados Escolares" Name="tabDadosEscolares">
            </dxtc:Tab>
            <dxtc:Tab Text="Transporte Escolar" Name="tabTransporteEscolar">
            </dxtc:Tab>
            <dxtc:Tab Text="Documentos Entregues" Name="tabDocumentos">
            </dxtc:Tab>
            <dxtc:Tab Text="Programas Socias/Especiais" Name="tabProgramas">
            </dxtc:Tab>
            <dxtc:Tab Text="Atendimento Educacional Especializado" Name="tabEspecializado">
            </dxtc:Tab>
            <dxtc:Tab Text="Irmãos" Name="tabIrmaos">
            </dxtc:Tab>
            <dxtc:Tab Text="AEDH - Escolarização em Outros Espaços" Name="tabAEDH">
            </dxtc:Tab>
        </Tabs>
    </dxtc:ASPxTabControl>
    <asp:Panel ID="pntabDadosPessoais" runat="server" Visible="false">
        <asp:HiddenField ID="hddTxtEmail" runat="server" />
        <asp:HiddenField ID="hddDataAlteracaoEmail" runat="server" />
        <asp:Panel ID="pnPessoa" GroupingText="Dados Pessoais" runat="server">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="text-align: right;">
                                    <dxe:ASPxBinaryImage ID="bimgFotoPessoa" runat="server" AlternateText="sem foto"
                                        Width="150px" Height="150px" ClientInstanceName="bimgFotoPessoa" StoreContentBytesInViewState="True">
                                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                                    </dxe:ASPxBinaryImage>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td>
                                    <asp:FileUpload ID="flFoto" runat="server" EnableViewState="true" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="hidden" id="imagem" runat="server" />
                                    <asp:ImageButton ID="btnFotografar" runat="server" OnClientClick="Bloqueio();return ShowLoginWindow();"
                                        SkinID="Fotografar" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblPessoa" runat="server" SkinID="lblObrigatorio" Text="Pessoa:* "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbltxtPessoa" runat="server" Text="Valor gerado após inclusão do aluno."
                                        Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtPessoa" runat="server" MaxLength="10" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNome" runat="server" SkinID="lblObrigatorio" Text="Nome:* "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNomeCompl" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                        Width="600px" />
                                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="X-Small" ForeColor="red"
                                        Text="(Preencher sem abreviações)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNomeSocial" runat="server" Text="Nome Social: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNomeSocial" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                                        Width="600px" />
                                    <asp:LinkButton ID="hplLink" Font-Size="12px" Font-Bold="true" OnClick="hplLinkNomeSocial_Click"
                                        OnClientClick="window.document.forms[0].target='_blank';" runat="server">Saiba Mais</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDtNasc" runat="server" SkinID="lblObrigatorio" Text="Data Nascimento:* "></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lblTipoSanguineo" Text="Tipo Sanguíneo: "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoSanguineo" runat="server" DataTextField="descr" DataValueField="item">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="lblSexo" runat="server" SkinID="lblObrigatorio" Text="Sexo:* "></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
                                        Width="150px">
                                        <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lblEtnia" Text="Cor/Raça:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlEtnia" AutoPostBack="true" runat="server" DataTextField="NOME"
                                        DataValueField="TABELAITEMID" OnSelectedIndexChanged="ddlEtnia_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label runat="server" ID="lblPovo" Text="Povo Indígena:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPovoIndigena" runat="server" DataTextField="DESCRICAO" DataValueField="POVOINDIGENAID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="lblCredo" runat="server" Text="Credo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCredo" runat="server" DataTextField="descr" DataValueField="item">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="lblFilhos" runat="server" Text="Quantidade de Filhos: "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFilhos" runat="server" MaxLength="2" Width="50px" SkinID="numerico" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <table>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="lblNacionalidade" runat="server" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlNacionalidade_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="lblMunicipioNasc" runat="server" Text="Naturalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <%-- Nascido no Brasil --%>
                        <tweb:TSearchBox ID="tseNaturalidade" runat="server" Visible="true" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                            Columns="10" ArgumentColumns="30" AutoPostBack="true" Key="codigo" MaxLength="10"
                            OnChanged="tseNaturalidade_Changed" OnLoad="tseNaturalidade_Load">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="UF" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                        <%-- Nascido fora do Brasil --%>
                        <tweb:TSearchBox ID="tseNaturalidadeEstrangeira" runat="server" Visible="false" SqlSelect="SELECT ESTADO, SIGLA, ID_PAIS, PAIS FROM HADES.dbo.VW_MUNICIPIO_ESTRANGEIRO"
                            Columns="10" Argument="MUNICIPIO" ArgumentColumns="30" AutoPostBack="true" Key="CODIGO"
                            MaxLength="10" DataType="Number" OnChanged="tseNaturalidadeEstrangeira_Changed"
                            OnLoad="tseNaturalidadeEstrangeira_Load">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Cidade" FieldName="MUNICIPIO" Width="50%" />
                                <tweb:TSearchBoxColumn Caption="Estado/Província" FieldName="ESTADO" Width="30%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td style="text-align: left">
                        <asp:Label ID="lblNaturalidadeUF" runat="server" Text="UF Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUFNascimento" runat="server" Width="100px" ReadOnly="true" BackColor="Gainsboro" />
                    </td>
                    <td>
                        <asp:Label ID="lblPaisNasc" runat="server" Text="País:* " SkinID="lblObrigatorio"
                            Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPaisNasc" runat="server" MaxLength="100" Width="150px" ReadOnly="true"
                            BackColor="Gainsboro" Visible="false" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="text-align: left" colspan="3">
                        <asp:Label ID="lblDescFamilia" runat="server" Text="O(a) aluno(a) se declara descendente de família integrante de comunidade quilombola, indígena ou caiçara?"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblDescFamilia" runat="server" RepeatDirection="Horizontal"
                            DataValueField="DescFamilia" Width="400px">
                            <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Sim, quilombola." Value="Q"></asp:ListItem>
                            <asp:ListItem Text="Sim, indígena." Value="I"></asp:ListItem>
                            <asp:ListItem Text="Sim, caiçara." Value="C"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNecEspecial" GroupingText="Necessidade Especial/Transtorno" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNecessidadeEspecial" runat="server" Text="Necessidade Especial:* "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbNecessidadeEspecial" runat="server" DataValueField="NECESSIDADEESPECIALID"
                            AutoPostBack="true" DataTextField="DESCRICAO" Width="289px" Height="16px" OnSelectedIndexChanged="cmbNecessidadeEspecial_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:LinkButton ID="hplLinkNecEspecial" Font-Size="12px" Font-Bold="true" OnClick="hplLinkNecEspecial_Click"
                            OnClientClick="window.document.forms[0].target='_blank';" runat="server">Saiba Mais</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label32" runat="server" Text="Aluno(a) com transtorno(s) que impacta(m) no desenvolvimento da aprendizagem?"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblPossuiTranstorno" runat="server" RepeatDirection="Horizontal"
                            Width="150px" OnSelectedIndexChanged="rblPossuiTranstorno_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="pnlTipoTranstorno" GroupingText="Tipo de transtorno" runat="server"
                Width="50%">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="chkTipoTranstorno" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="2" Width="100%">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkDeclaroNecessidadeEspecial" Text="Declaro estar de posse do Laudo Médico ou Parecer Pedagógico"
                            Visible="false" SkinID="lblObrigatorio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnRecursos" GroupingText="Recursos necessários" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chkRecursoNecessidadeEspecial" runat="server" RepeatDirection="Vertical"
                                            Width="100%" Enabled="false">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlAplicacaoProva" GroupingText="Aplicação de provas" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chkRecursoAplicacaoProva" runat="server" RepeatColumns="2"
                                            RepeatDirection="Vertical" RepeatLayout="Table" Width="100%">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 3px">
                                        <asp:CheckBox ID="chkNenhumRecursoAplicacaoProva" Text="Nenhum" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblRecursoAplicaProvaExclusivo" runat="server" RepeatDirection="Horizontal"
                                            RepeatColumns="2" RepeatLayout="Table" Width="100%">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlFiliacao" GroupingText="Filiação" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label2" runat="server" Text="Nome da Mãe:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeMae" runat="server" Width="250px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Width="140px"
                            AutoPostBack="true" OnCheckedChanged="chkNaoDeclarMae_CheckedChanged" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkFalecidaMae" Text="Falecida" Width="140px" AutoPostBack="true"
                            OnCheckedChanged="chkFalecidaMae_CheckedChanged" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFMae" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTelefoneMae" runat="server"
                            Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefoneMae" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            Width="100px" MaxLength="14" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CheckBox runat="server" ID="chkDeclaroAusenciaMae" Text="Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome da mãe"
                            Visible="false" SkinID="lblObrigatorio" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="Nome do Pai:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomePai" runat="server" Width="250px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" Width="140px"
                            AutoPostBack="true" OnCheckedChanged="chkNaoDeclarPai_CheckedChanged" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkFalecidoPai" Text="Falecido" Width="140px" AutoPostBack="true"
                            OnCheckedChanged="chkFalecidoPai_CheckedChanged" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label6" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFPai" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label7" runat="server" Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefonePai" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            Width="100px" MaxLength="14" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CheckBox runat="server" ID="chkDeclaroAusenciaPai" Text="Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome do pai"
                            Visible="false" SkinID="lblObrigatorio" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label8" runat="server" Text="Responsável Legal:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblResponsavel" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rblResponsavel_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem>Mãe</asp:ListItem>
                            <asp:ListItem>Pai</asp:ListItem>
                            <asp:ListItem>Próprio Aluno</asp:ListItem>
                            <asp:ListItem>Outros</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Visible="false" Names="Verdana" Font-Size="Smaller" ID="lblNomeResponsavel"
                            runat="server" Text="Nome do Responsável:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeResponsavel" runat="server" Width="250px" Visible="false" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblCPFResponsavel"
                            runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFResponsavel" runat="server" Width="150px" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" Visible="false" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblTelefoneResponsavel"
                            runat="server" Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefoneResp" Visible="false" onkeyup="formataTelefoneDDD(this,event)"
                            runat="server" Width="100px" MaxLength="14" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CheckBox runat="server" ID="chkPossuiIrmao" Text="Possui irmão matriculado na rede estadual de ensino."
                            OnCheckedChanged="chkPossuiIrmao_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnEndereco" GroupingText="Endereço" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false" />
                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                            Modal="true" SkinID="CEP" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                        Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                                    <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="250px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                        Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                        Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                        Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                        Text="Compl.:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndCompl" runat="server" MaxLength="50" onkeypress="return endereco(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                        Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                        Width="400px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLocalZona" runat="server"
                                        Text="Localização/Zona<br> de Residência:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLocalZona" runat="server">
                                        <asp:ListItem Text="Urbana" Value="Urbana"> </asp:ListItem>
                                        <asp:ListItem Text="Rural" Value="Rural"> </asp:ListItem>
                                        <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblLocalizacaoDiferenciada" runat="server" Text="Localização Diferenciada: * "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox AutoPostBack="true" ID="chkNaoSeAplica" ValueChecked="S" ValueUnchecked="N"
                                                    ValueType="System.String" runat="server" Checked="true" Text="Não se aplica"
                                                    EnableViewState="false" OnCheckedChanged="chkNaoSeAplica_CheckedChanged">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkQuilombos" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Comunidade quilombola">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkAreaTradicional" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Área onde se localizam povos e comunidades tradicionais">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxCheckBox ID="chkAreaAssentamento" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Área de assentamento">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkTerraIndigena" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Terra indígena">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="pnlLocalizacao" runat="server" GroupingText="Localização" Width="900px">
                <table width="100%">
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblLatitude" runat="server" Text="Latitude:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLatitude" runat="server" Width="130px" MaxLength="50" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblLongitude" runat="server" Text="Longitude:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLongitude" runat="server" Width="130px" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <div id="mapa" style="width: 500px; height: 400px">
                            </div>
                            <br />
                            <asp:HiddenField ID="hdnMarcarMapa" runat="server" />
                            <asp:Button ID="btnEncontraNoMapa" runat="server" Text="Obter coordenadas" OnClientClick="MarcaMapa() return false;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnContato" GroupingText="Contato" runat="server">
            <table>
                <tr>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone ou Celular:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtFone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                            MaxLength="14" Width="100px" />
                    </td>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular" runat="server"
                            Text="Celular:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"
                            MaxLength="14"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                            SkinID="lblObrigatorio" Text="E-mail:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" Style="text-transform: lowercase;" onchange="txtEmailTextChanged()"
                            onblur="txtEmailTextBlur()" runat="server" Width="400px" MaxLength="100" />
                    </td>
                    <td colspan="2">
                        <asp:Table ID="tblDadosEmail" runat="server" GridLines="None">
                            <asp:TableRow ID="rowConfirmaEmail">
                                <asp:TableCell>
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmailConfirmacao" runat="server"
                                        Text="Confirme o e-mail:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox Style="text-transform: lowercase;" ID="txtEmailConfirmacao" runat="server"
                                        MaxLength="100" Width="400px" AutoComplete="Off"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="rowDtAtualizacaoEmail">
                                <asp:TableCell>
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label25" runat="server" Text="Data de Atualização do E-mail:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtDataAtualizacaoEmail" runat="server" ReadOnly="true" Width="150px"
                                        MaxLength="100" Style="text-transform: lowercase" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblNotaEmailDuplicado" runat="server"
                            Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label14" runat="server" Text="E-mail Google for Education:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailGoogle" Style="text-transform: lowercase;" runat="server"
                            Width="400px" MaxLength="100" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlLoginCartao" GroupingText="Dados da operadora de cartões" runat="server"
            Visible="false">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLogin" runat="server"
                            Text="Login" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtLoginCartao" runat="server" Enabled="false" ReadOnly="true" Width="300px"
                            MaxLength="100" Style="text-transform: lowercase" />
                    </td>
                    <td colspan="4">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblObsLoginCartao" runat="server"
                            Visible="false" Text="Obs.: Caso a opção de gratuidade esteja indicada como 'sim' na aba 'dados escolares' as alterações realizadas no cadastro do aluno serão atualizadas pela operadora de cartões, sendo que a alteração do e-mail do aluno também modificará o login que o aluno utiliza para acessar o site da operadora. Caso a opção de gratuidade esteja marcada como 'não' as modificações realizadas no cadastro do aluno não serão atualizadas pela operadora de cartões." />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblOperadora" runat="server"
                            Text="Operadora" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtOperadora" runat="server" Enabled="false" ReadOnly="true" Width="150px"
                            MaxLength="100" Style="text-transform: lowercase" />
                    </td>
                    <td style="text-align: right; width: 200px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCodigoAlunoOperadora"
                            runat="server" Text="Código do Aluno na Operadora" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCodigoAlunoOperadora" runat="server" Enabled="false" ReadOnly="true"
                            Width="150px" MaxLength="100" Style="text-transform: lowercase" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataAtualizacaoLoginOperadora"
                            runat="server" Text="Data de Atualização do Login" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataAtualizacaoLoginOperadora" runat="server" Enabled="false"
                            ReadOnly="true" Width="150px" MaxLength="100" Style="text-transform: lowercase" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnDocumento" GroupingText="Outros Documentos" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="50"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Tipo" runat="server"
                            Text="Tipo:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                            Width="130px" OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                            Text="Número:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="20" Width="160px" SkinID="numeroDocumento" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblComplIdentidade" runat="server"
                            Text="Complemento da identidade:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComplIdentidade" runat="server" MaxLength="20" Width="160px"
                            Visible="true" SkinID="numeroDocumento" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
                            Text="Estado:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                            Width="130px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
                            Text="Órgão Emissor:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                            Width="160px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
                            Text="Data de Expedição:"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje" Width="140px">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnInformacoes" GroupingText="Outras Informações" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblIDCenso" runat="server"
                            Text="Número de Identificação no INEP:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtIDCenso" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            Enable="false" ReadOnly="true" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label3" runat="server" Text="Número de Identificação Social(NIS):"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNIS" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            Enabled="false" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnDocumentos_CertNasc" runat="server" GroupingText="Certidão Civil">
            <table>
                <tr>
                    <td style="text-align: left">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTipoCertidao" runat="server"
                            Text="Tipo Certidão Civil:*" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label><asp:DropDownList
                                ID="ddlTipoCertidao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCertidao_SelectedIndexChanged">
                                <asp:ListItem Text="Não Informado" Value="Nenhum">
                                </asp:ListItem>
                                <asp:ListItem Text="Nascimento" Value="Nascimento"></asp:ListItem>
                                <asp:ListItem Text="Casamento" Value="Casamento"> </asp:ListItem>
                                <asp:ListItem Selected="True" Text="Selecione" Value=""> </asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTipoCertidaoCivil" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDeclaroCertidaoCivil" runat="server" Style="text-align: left"
                                            SkinID="lblObrigatorio" Text="Declaro que o aluno não apresentou a certidão de nascimento/casamento de acordo com o motivo descrito" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMotivoCertidao" runat="server"
                                            Text="Motivo:*" SkinID="lblObrigatorio"></asp:Label><asp:TextBox ID="txtMotivoCertidaoCivil"
                                                runat="server" Width="462px" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="lblCertCivil" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                            Text="Certidão Civil:*" Style="text-align: left"></asp:Label><asp:DropDownList ID="ddlCertidaoCivil"
                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCertidaoCivil_SelectedIndexChanged">
                                <asp:ListItem Text="Selecione" Value=""> </asp:ListItem>
                                <asp:ListItem Text="Modelo Antigo" Value="Modelo Antigo"> </asp:ListItem>
                                <asp:ListItem Text="Modelo Novo" Value="Modelo Novo"> </asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAntigo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblUFCartorio" runat="server" Text="UF do Cartório: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUFCartorio" runat="server" AutoPostBack="true" DataTextField="UF"
                                            DataValueField="codigo_uf" OnSelectedIndexChanged="ddlUFCartorio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMunicipioCartorio" runat="server" Text="Município do Cartório: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlMunicipioCartorio" runat="server" AutoPostBack="true" DataTextField="municipio"
                                            DataValueField="codigo_municipio" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMunicipioCartorio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCartorio" runat="server" Text="Cartório: "></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlCartorio" runat="server" DataTextField="nome_cartorio" DataValueField="cod_cartorio">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNasc" runat="server" Text="Número do Termo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Numero" runat="server" MaxLength="15" SkinID="numerico" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNascEmissao" runat="server" Text="Data de Emissão: "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dboDOC_CertNasc_DtEmissao" runat="server" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" MinDate="1901-01-01">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="CertNascUF" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDOC_CertNasc_Uf" runat="server" DataTextField="sigla" DataValueField="sigla">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNascFolha" runat="server" Text="Folha: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Folha" runat="server" MaxLength="15" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNascLivro" runat="server" Text="Livro: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Livro" runat="server" MaxLength="15" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlNovo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumMatricula" runat="server" Text="Número da matrícula: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumMatriculaCertidao" Width="200px" runat="server" MaxLength="32"
                                            SkinID="numerico" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: right;">
                    <dxe:ASPxButton ID="btnProximo" runat="server" Text="Próximo >>" OnClick="btnProximo_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pntabDadosEscolares" runat="server" Visible="false">
        <asp:Panel ID="pnAluno" GroupingText="Aluno" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblNumInscricao" runat="server"
                            Text="Inscrição Matrícula Fácil:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:Label ID="txtNumInscricao" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAluno" runat="server"
                            Text="Matrícula:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:Label ID="lblMatricula" runat="server" Text="Valor gerado após inclusão do aluno."
                            Visible="false"></asp:Label><asp:TextBox ID="txtAluno" runat="server" MaxLength="20"
                                ReadOnly="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSituacao" runat="server"
                            Text="Situação:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSituacao" runat="server" Width="150px" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCausaEncerramento" runat="server"
                            Text="Causa do Encerramento:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCausaEncerramento" runat="server" Width="150px" ReadOnly="true" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMotivo" runat="server"
                            Text="Motivo:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMotivo" runat="server" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDadosIngresso" GroupingText="Dados de Ingresso" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAno_Ingresso" runat="server"
                            Text="Ano Ingresso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbAnoIngresso" runat="server" DataTextField="ano" DataValueField="ano"
                            AutoPostBack="true" OnSelectedIndexChanged="cmbAnoIngresso_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSem_Ingresso" runat="server"
                            Text="Período Ingresso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbSemIngresso" runat="server" DataTextField="periodo" DataValueField="periodo"
                            OnSelectedIndexChanged="cmbSemIngresso_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblDataCadastro" runat="server"
                            Text="Data de Inclusão do Aluno:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataCadastro" runat="server" Width="150px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTipoIngresso" runat="server" Text="Tipo Ingresso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoIngresso" runat="server" DataTextField="descricao" DataValueField="tipo_ingresso">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblRedeEnsinoOrigem" runat="server" Text="Rede de Ensino Origem:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRedeEnsinoOrigem" runat="server" AutoPostBack="true" DataTextField="descr"
                            DataValueField="descr" OnSelectedIndexChanged="ddlRedeEnsinoOrigem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTempoAfastamento" runat="server" Text="Tempo de Afastamento(em meses):*"
                            SkinID="lblObrigatorio" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTempoAfastamento" runat="server" Visible="false" SkinID="numerico"
                            MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnCurso" GroupingText="Escolaridade" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblUniEnsino" runat="server"
                            Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="width: auto" colspan="3">
                        <tweb:TSearchBox ID="tseUnidadeEns" AutoPostBack="true" runat="server" Key="unidade_ens"
                            Argument="nome_comp" Caption="" OnLoad="tseUnidadeEns_Load" MaxLength="20" GridWidth="850px"
                            OnChanged="tseUnidadeEns_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="municipio" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNivel" runat="server" Text="Nível/Segmento*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblModalidade" runat="server" Text="Modalidade*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlModalidade_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCurso" runat="server"
                            Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome, pc.curso as pccurso FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO
                                                left join LY_EVENTO_GERAL pc on pc.CURSO = uec.CURSO and pc.TIPO_FILTRO = 'Bloqueio_Cadastro_Aluno' and CONVERT(date,GetDate()) between pc.DT_INICIO and DT_FIM"
                            ArgumentColumns="60" Columns="10" OnChanged="tseCurso_Changed" OnLoad="tseCurso_Load"
                            MaxLength="20" GridWidth="800px" SqlOrder="nome" SqlWhere="pc.curso is null">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                            Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoCurso" runat="server" Visible="false">
                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Concomitante" Value="Concomitante"></asp:ListItem>
                            <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTurno" runat="server"
                            Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbTurno" runat="server" DataTextField="descricao" DataValueField="turno"
                            OnSelectedIndexChanged="cmbTurno_SelectedIndexChanged" AutoPostBack="true" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCurriculo" runat="server"
                            Text="Matriz Curricular:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCurriculo" runat="server" DataTextField="curriculo" DataValueField="curriculo"
                            DataSourceID="odsCurriculo" OnSelectedIndexChanged="cmbCurriculo_SelectedIndexChanged"
                            AutoPostBack="true" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ObjectDataSource ID="odsCurriculo" runat="server" TypeName="Techne.Lyceum.RN.Curriculo"
                            SelectMethod="ConsultarMatrizAluno">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseCurso" PropertyName="DBValue" Name="curso" />
                                <asp:ControlParameter ControlID="cmbTurno" PropertyName="SelectedValue" Name="turno" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSerie" runat="server"
                            Text="Série/Ano Escolar:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="descricao" DataValueField="serie">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblOutroEnsino" runat="server" Text="Recebe Escolarização em Outro Espaço (diferente da escola)? :*"
                            SkinID="lblObrigatorio" Width="150"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOutroEnsino" runat="server" DataValueField="item" DataTextField="descr"
                            Width="200px" OnSelectedIndexChanged="ddlOutroEnsino_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnConfirmaMatricula" GroupingText="Confirmação/Renovação de Matrícula"
            runat="server" Visible="true">
            <dxwgv:ASPxGridView ClientInstanceName="grdRenovacaoMatricula" ID="grdRenovacaoMatricula"
                OnAfterPerformCallback="grdRenovacaoMatricula_AfterPerformCallback" runat="server"
                AutoGenerateColumns="False" KeyFieldName="RENOVACAOID" Width="100%">
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="Código" Visible="false" FieldName="RENOVACAOID"
                        Name="RENOVACAOID" VisibleIndex="0">
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
                </Columns>
            </dxwgv:ASPxGridView>
            <br />
            <asp:Label runat="server" ID="lblConfirmarMatricula" SkinID="lblMensagem"></asp:Label><br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblMsgHistoricoRenovacao" runat="server" Text="Nota: Este quadro somente será atualizado após o fechamento do período letivo, para os campos de realização de renovação de matrícula durante o período para o processo de renovação." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxwgv:ASPxGridView ClientInstanceName="grdConfirmacao" ID="grdConfirmacao" runat="server"
                            AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"
                            OnHtmlRowCreated="grdConfirmacao_HtmlRowCreated">
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                            <SettingsText EmptyDataRow="Não existem dados." />
                            <SettingsPager Mode="ShowAllRecords" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONFIRMACAO_MATRICULA"
                                    Name="ID_CONFIRMACAO_MATRICULA" VisibleIndex="0">
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
                                <dxwgv:GridViewDataDateColumn VisibleIndex="8" Caption="Data Sugerida" Name="DT_SUGERIDA"
                                    FieldName="DT_SUGERIDA" Width="100px" Visible="true" ReadOnly="true">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ensino Religioso" FieldName="ENSINO_RELIGIOSO"
                                    Name="ENSINO_RELIGIOSO" VisibleIndex="9">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkEnsinoReligioso" runat="server" Checked='<%# this.VerificarCheck(Eval("ENSINO_RELIGIOSO")) %>' />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Língua Estrangeira Facultativa" FieldName="LINGUA_ESTRANGEIRA_FACULTATIVA"
                                    Name="LINGUA_ESTRANGEIRA_FACULTATIVA" VisibleIndex="10">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Checked='<%# this.VerificarCheck(Eval("LINGUA_ESTRANGEIRA_FACULTATIVA")) %>' />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Progr. de aceleração de estudos (Proj. Autonomia)"
                                    FieldName="PROJETO_AUTONOMIA" Name="PROJETO_AUTONOMIA" VisibleIndex="11">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkProjetoAutonomia" runat="server" Checked='<%# this.VerificarCheck(Eval("PROJETO_AUTONOMIA")) %>' />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataColumn Caption="Situação *" Name="STATUS" VisibleIndex="12" Width="150px">
                                    <DataItemTemplate>
                                        <asp:RadioButton ID="rbConfirmado" runat="server" GroupName='<%# Bind("ID_CONFIRMACAO_MATRICULA") %>'
                                            Text="Confirmada" />
                                        <br />
                                        <asp:RadioButton ID="rbNaoConfirmado" runat="server" GroupName='<%# Bind("ID_CONFIRMACAO_MATRICULA") %>'
                                            Text="Não Confirmada" />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataDateColumn VisibleIndex="13" Caption="Data Situação" Name="DT_ALTERACAO"
                                    FieldName="DT_ALTERACAO" Width="100px" Visible="true" ReadOnly="true">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" VisibleIndex="14"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Codigo Curso" FieldName="COD_CURSO" VisibleIndex="15"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSO" VisibleIndex="16"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ensino Religioso" FieldName="PODE_ENSINO_RELIGIOSO"
                                    Visible="false" VisibleIndex="17">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Lingua Estrangueira Facultativa" FieldName="PODE_LINGUA_ESTRANGEIRA"
                                    Visible="false" VisibleIndex="18">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cadastrou" FieldName="CADASTROU" VisibleIndex="19"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" VisibleIndex="20"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="20"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curriculo" FieldName="CURRICULO" VisibleIndex="20"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Vaga" FieldName="TIPOVAGAOCUPADA" VisibleIndex="21"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Styles>
                                <CommandColumn Wrap="False">
                                </CommandColumn>
                            </Styles>
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnConfRenovMatricula" Text="Confirmar Matrícula" runat="server"
                            Visible="false" OnClick="btnConfRenovMatricula_Click" ValidationGroup="ConfirmaMatriculaForm" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblErroConfirmacao" runat="server" SkinID="lblMensagem"></asp:Label></asp:Panel>
        <br />
        <asp:Panel ID="pnImprimirMatricula" GroupingText="Imprimir Fichas de Matrícula" runat="server"
            Visible="false">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label11" runat="server" Text="Confirmação de Matrícula:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="width: auto">
                        <dxe:ASPxComboBox ID="ddlConfirmacaoMatricula" runat="server" DataSourceID="odsImprimirConfirmacao"
                            ValueField="ID_CONFIRMACAO_MATRICULA" AutoPostBack="true" TextFormatString="{0} - {1}|{2} - {3}"
                            DropDownWidth="700px" Width="480px" Height="5px" ClientInstanceName="ddlConfirmacaoMatricula">
                            <Columns>
                                <dxe:ListBoxColumn Caption="Código" FieldName="ID_CONFIRMACAO_MATRICULA" Width="15%" />
                                <dxe:ListBoxColumn Caption="Ano" FieldName="ano" Width="10%" />
                                <dxe:ListBoxColumn Caption="Periodo" FieldName="PERIODO" Width="10%" />
                                <dxe:ListBoxColumn Caption="Modalidade/Segmento/Curso" FieldName="MOD_SEG_CURSO"
                                    Width="60%" />
                                <dxe:ListBoxColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO" Width="60%" />
                                <dxe:ListBoxColumn Caption="Série" FieldName="SERIE" Width="10%" />
                                <dxe:ListBoxColumn Caption="Turno" FieldName="NOME_TURNO" Width="20%" />
                            </Columns>
                        </dxe:ASPxComboBox>
                    </td>
                    <td>
                        <asp:Button ID="btnImprimirConfirmacao" runat="server" OnClick="btnImprimirConfirmacao_Click"
                            Text="Imprimir Matrícula" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label16" runat="server" Text="Renovação de Matrícula:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="width: auto">
                        <dxe:ASPxComboBox ID="ddlRenovacaoMatricula" runat="server" DataSourceID="odsImprimirRenovacao"
                            ValueField="RENOVACAOID" AutoPostBack="true" TextFormatString="{0} - {1}|{2} - {3}"
                            DropDownWidth="700px" Width="480px" Height="5px" ClientInstanceName="ddlRenovacaoMatricula">
                            <Columns>
                                <dxe:ListBoxColumn Caption="Código" FieldName="RENOVACAOID" Width="15%" />
                                <dxe:ListBoxColumn Caption="Ano" FieldName="ANO" Width="10%" />
                                <dxe:ListBoxColumn Caption="Periodo" FieldName="PERIODO" Width="10%" />
                                <dxe:ListBoxColumn Caption="Modalidade/Segmento/Curso" FieldName="MOD_SEG_CURSO"
                                    Width="60%" />
                                <dxe:ListBoxColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO" Width="60%" />
                                <dxe:ListBoxColumn Caption="Série" FieldName="SERIE" Width="10%" />
                                <dxe:ListBoxColumn Caption="Turno" FieldName="NOME_TURNO" Width="20%" />
                            </Columns>
                        </dxe:ASPxComboBox>
                    </td>
                    <td>
                        <asp:Button ID="btnImprimirRenovacao" runat="server" OnClick="btnImprimirRenovacao_Click"
                            Text="Imprimir Renovação" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="display: none; font-family: arial; color: red; font-size: 14px; font-weight: bold;"
                            id="divPopupBloqueado">
                            A janela popup de impressão foi bloqueada pelo navegador, para abri-la <a href=""
                                style="font-family: arial; font-size: 14px; font-weight: bold;" onclick="return abrir()">
                                clique aqui</a>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ClientInstanceName="btnAnterior" ID="btnAnterior" runat="server"
                        Text="<< Anterior" OnClick="btnAnterior_Click">
                    </dxe:ASPxButton>
                </td>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnProximo2" runat="server" Text="Próximo >>" OnClick="btnProximo2_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    <asp:Panel ID="pntabTransporteEscolar" runat="server" Visible="false">
        <asp:Panel ID="pnTransporte" runat="server" Visible="true">
            <br />
            <table>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmailTransp" runat="server"
                            SkinID="lblObrigatorio" Text="E-mail:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailTransp" Style="text-transform: lowercase;" ReadOnly="true"
                            Enabled="false" runat="server" Width="400px" MaxLength="100" />
                    </td>
                    <td>
                        <asp:Table ID="tblDadosEmailTransp" runat="server" GridLines="None">
                            <asp:TableRow ID="rowDtAtualizacaoEmailTransp">
                                <asp:TableCell>
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label25Transp" runat="server"
                                        Text="Data de Atualização do E-mail:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtDataAtualizacaoEmailTransp" runat="server" ReadOnly="true" Width="150px"
                                        MaxLength="100" Style="text-transform: lowercase" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblGratuidade" runat="server"
                            Text="Utiliza Transporte?*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGratuidade" runat="server" OnSelectedIndexChanged="ddlGratuidade_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="Sim" Value="S"> </asp:ListItem>
                            <asp:ListItem Text="Não" Value="N"> </asp:ListItem>
                            <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblPoderPublTransp" runat="server"
                            Text="Poder público responsável pelo transporte escolar:*" SkinID="lblObrigatorio">
                        </asp:Label>
                        <asp:DropDownList ID="ddlPoderPublicoTransp" runat="server">
                            <asp:ListItem Text="Estadual" Value="Estadual"> </asp:ListItem>
                            <asp:ListItem Text="Municipal" Value="Municipal"> </asp:ListItem>
                            <asp:ListItem Text="Nenhum" Value=""> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblModais" runat="server"
                            Text="Modal*:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkModais" runat="server" DataTextField="descr" DataValueField="item"
                            Width="300" OnSelectedIndexChanged="chkModais_SelectedIndexChanged" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRodoviario" runat="server"
                            Text="Rodoviário:" Visible="false"></asp:Label><asp:CheckBoxList ID="chkRodoviario"
                                runat="server" Visible="false" RepeatDirection="Horizontal">
                                <asp:ListItem>Vans/Kombis</asp:ListItem>
                                <asp:ListItem>Microônibus</asp:ListItem>
                                <asp:ListItem>Ônibus</asp:ListItem>
                                <asp:ListItem>Bicicleta</asp:ListItem>
                                <asp:ListItem>Outro tipo de veículo</asp:ListItem>
                            </asp:CheckBoxList>
                        <br />
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAquaviario" runat="server"
                            Text="Aquaviário/embarcação:" Visible="false"></asp:Label><asp:CheckBoxList ID="chkAquaviario"
                                runat="server" Visible="false" AutoPostBack="true" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="chkAquaviario_SelectedIndexChanged">
                                <asp:ListItem>Capacidade de até 5 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade entre 5 e 15 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade entre 15 e 35 pessoas</asp:ListItem>
                                <asp:ListItem>Capacidade acima de 35 pessoas</asp:ListItem>
                                <asp:ListItem>Não utiliza transporte Aquaviário</asp:ListItem>
                            </asp:CheckBoxList>
                        <br />
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblOnibus" runat="server"
                            Text="Operadora (selecionar ATÉ DUAS operadoras apenas):" Visible="false"></asp:Label>
                        <asp:CheckBoxList ID="chkOnibus" runat="server" Visible="false" RepeatDirection="Horizontal">
                            <asp:ListItem>RioCard</asp:ListItem>
                            <asp:ListItem>Jaé</asp:ListItem>
                            <asp:ListItem>SindPass</asp:ListItem>
                            <asp:ListItem>Outros</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSalvarTransporte" Width="160px" Text="Salvar Transporte Escolar"
                            runat="server" OnClick="btnSalvarTransporte_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table width="100%">
                <tr>
                    <td style="text-align: left;">
                        <dxe:ASPxButton ClientInstanceName="btnAnteriorTransp" ID="btnAnteriorTransp" runat="server"
                            Text="<< Anterior" OnClick="btnAnteriorTransp_Click">
                        </dxe:ASPxButton>
                    </td>
                    <td style="text-align: left;">
                        <dxe:ASPxButton ID="btnProximoTransp" runat="server" Text="Próximo >>" OnClick="btnProximoTransp_Click">
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pntabDocumentos" runat="server" Visible="false">
        <dxwgv:ASPxGridView ClientInstanceName="grdDocumentos" ID="grdDocumentos" runat="server"
            AutoGenerateColumns="False" KeyFieldName="ID_DOCUMENTO_ALUNO" Width="70%" OnHtmlRowCreated="grdDocumentos_HtmlRowCreated">
            <SettingsBehavior AllowMultiSelection="False" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="ID_DOCUMENTO_ALUNO" FieldName="ID_DOCUMENTO_ALUNO"
                    Name="ID_DOCUMENTO_ALUNO" VisibleIndex="0" Width="10%" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID_DOCUMENTO" FieldName="ID_DOCUMENTO" Name="ID_DOCUMENTO"
                    VisibleIndex="1" Width="20%" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Documento" FieldName="DESCRICAO" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Obrigatório?" FieldName="OBRIGATORIO" VisibleIndex="3"
                    Width="120px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="4" Caption="Prazo para entrega" Name="PRAZO"
                    FieldName="PRAZO" Width="100px" Visible="true" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Entregue? *" FieldName="ENTREGA" Name="ENTREGA"
                    VisibleIndex="5">
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkEntrega" runat="server" Checked='<%# this.VerificarCheck(Eval("ENTREGA")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn Caption="Data da Entrega*" FieldName="DT_ENTREGA" VisibleIndex="6"
                    Name="DT_ENTREGA">
                    <DataItemTemplate>
                        <asp:TextBox ID="txtDataEntrega" runat="server" Text='<%# Bind("DT_ENTREGA") %>'>
                        </asp:TextBox>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="MATRICULA" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="6" Caption="Data do Cadastro" Name="DT_CADASTRO"
                    FieldName="DT_CADASTRO" Width="100px" Visible="true" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Styles>
                <CommandColumn Wrap="False">
                </CommandColumn>
            </Styles>
        </dxwgv:ASPxGridView>
        <asp:Button ID="btnSalvarDoc" runat="server" Text="Salvar" OnClick="btnSalvarDoc_Click" />
        <br />
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ClientInstanceName="btnAnterior2" ID="ASPxButton3" runat="server"
                        Text="<< Anterior" OnClick="btnAnterior2_Click">
                    </dxe:ASPxButton>
                </td>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnProximo3" runat="server" Text="Próximo >>" OnClick="btnProximo3_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pntabProgramas" runat="server" Visible="false">
        <dxwgv:ASPxGridView ClientInstanceName="grdProgramas" ID="grdProgramas" runat="server"
            AutoGenerateColumns="False" KeyFieldName="ID_PROGRAMA_SOCIAL" Width="70%">
            <SettingsBehavior AllowMultiSelection="False" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="ID_PROGRAMA_SOCIAL" FieldName="ID_PROGRAMA_SOCIAL"
                    VisibleIndex="0" Width="10%" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Programa" FieldName="PROGRAMA" VisibleIndex="1"
                    Width="20%">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Elegível?" FieldName="ELEGIVEL" VisibleIndex="3"
                    Width="120px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Beneficiário?" FieldName="BENEFICIARIO" VisibleIndex="4"
                    Width="120px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="5" Caption="Início da Vigência" Name="INICIO_VIGENCIA"
                    FieldName="INICIO_VIGENCIA" Width="100px" Visible="true" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="6" Caption="Fim da Vigência" Name="FIM_VIGENCIA"
                    FieldName="FIM_VIGENCIA" Width="100px" Visible="true" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Styles>
                <CommandColumn Wrap="False">
                </CommandColumn>
            </Styles>
        </dxwgv:ASPxGridView>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ClientInstanceName="btnAnterior3" ID="ASPxButton2" runat="server"
                        Text="<< Anterior" OnClick="btnAnterior3_Click">
                    </dxe:ASPxButton>
                </td>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnProximo4" runat="server" Text="Próximo >>" OnClick="btnProximo4_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pntabEspecializado" runat="server" Visible="false">
        <asp:Panel ID="Panel3" GroupingText="Atendimento Educacional Especializado" runat="server"
            Style="width: 850px;">
            <asp:Label ID="lblMensagemAtendimentoEspecializado" runat="server" SkinID="lblMensagem"></asp:Label>
            <asp:Panel ID="pnlCuidador" GroupingText="Cuidador" runat="server" Style="font-size: 15px;">
                <asp:Panel ID="Panel7" GroupingText="Avaliação" runat="server" Style="font-size: 14px;">
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalNecCuidador" runat="server" Text="Necessita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNecessitaCuidador" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalTipoCuidador" runat="server" Text="Tipo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoAvaliacaoCuidador" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalVigenciaCuidador" runat="server" Text="Vigência:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVigenciaCuidador" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalJustificativaCuidador" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativaCuidador" TextMode="MultiLine" runat="server" Enabled="false"
                                    Height="65px" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dxwgv:ASPxGridView ID="grdCuidador" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdCuidador"
                    KeyFieldName="CUIDADORALUNOID" DataSourceID="odsCuidador" Width="850px">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="CUIDADORALUNOID" Visible="false"
                            VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Nome Cuidador" FieldName="NOME_COMPL" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="5">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlLedor" GroupingText="Ledor" runat="server" Style="font-size: 15px;">
                <asp:Panel ID="Panel8" GroupingText="Avaliação" runat="server" Style="font-size: 14px;">
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalNecLedor" runat="server" Text="Necessita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNecessitaLedor" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalTipoLedor" runat="server" Text="Tipo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoAvaliacaoLedor" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalVigenciaLedor" runat="server" Text="Vigência:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVigenciaLedor" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalJustificativaLedor" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativaLedor" TextMode="MultiLine" runat="server" Enabled="false"
                                    Height="65px" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dxwgv:ASPxGridView ID="grdLedor" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdLedor"
                    KeyFieldName="CUIDADORALUNOID" DataSourceID="odsLedor" Width="850px">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="LEDORALUNOID" Visible="false"
                            VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Nome Ledor" FieldName="NOME_COMPL" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="5">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlInterprete" GroupingText="Intérprete de Turma" runat="server" Style="font-size: 15px;">
                <asp:Panel ID="Panel9" GroupingText="Avaliação" runat="server" Style="font-size: 14px;">
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalNecInterprete" runat="server" Text="Necessita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNecessitaInterprete" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalTipoInterprete" runat="server" Text="Tipo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoAvaliacaoInterprete" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalVigenciaInterprete" runat="server" Text="Vigência:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVigenciaInterprete" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalJustificativaInterprete" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativaInterprete" TextMode="MultiLine" runat="server"
                                    Enabled="false" Height="65px" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dxwgv:ASPxGridView ID="grdInterprete" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="grdInterprete" KeyFieldName="INTERPRETETURMAID" DataSourceID="odsInterprete"
                    Width="850px">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="INTERPRETETURMAID" Visible="false"
                            VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Nome Intérprete" FieldName="NOME_COMPL" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="5">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <asp:Panel ID="pnlSalaRecurso" GroupingText="Sala de Recursos" runat="server" Style="font-size: 15px;">
                <asp:Panel ID="Panel5" GroupingText="Avaliação" runat="server" Style="font-size: 14px;">
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalTipoSalaRecurso" runat="server" Text="Necessita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNecessitaSalaRecurso" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label24" runat="server" Text="Tipo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoAvaliacaoSalaRecurso" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalVigenciaSalaRecurso" runat="server" Text="Vigência:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVigenciaSalaRecurso" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalUnidadeSalaRecurso" runat="server" Text="Unidade:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblUnidadeSalaRecurso" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label30" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativaSalaRecurso" TextMode="MultiLine" runat="server"
                                    Enabled="false" Height="65px" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dxwgv:ASPxGridView ID="grdSalaRecurso" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="grdSalaRecurso" KeyFieldName="CHAVE" DataSourceID="odsSalaRecurso"
                    Width="850px">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="CHAVE" Visible="false" VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Turno" FieldName="TURNO" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Professor" FieldName="NOME_COMPL" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="CPF Professor" FieldName="CPF" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataColumn Caption="Situação" FieldName="SIT_MATRICULA" VisibleIndex="5">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <asp:Panel ID="pnlPAPEE" GroupingText="Professor Articulador Pedagógico Educação Especial"
                runat="server" Style="font-size: 15px;">
                <asp:Panel ID="Panel10" GroupingText="Avaliação" runat="server" Style="font-size: 14px;">
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label17" runat="server" Text="Necessita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNecessitaPAPEE" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalTipoPAPEE" runat="server" Text="Tipo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoAvaliacaoPAPEE" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="lblAvalVigenciaPAPEE" runat="server" Text="Vigência:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVigenciaPAPEE" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label34" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativaPAPEE" TextMode="MultiLine" runat="server" Enabled="false"
                                    Height="65px" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dxwgv:ASPxGridView ID="grdPAPEE" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdPAPEE"
                    KeyFieldName="CHAVE" DataSourceID="odsPAPEE" Width="850px">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="CHAVE" Visible="false" VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="ANO" FieldName="ANO" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="SEMESTRE" FieldName="SEMESTRE" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Professor" FieldName="NOME_COMPL" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="CPF Professor" FieldName="CPF" VisibleIndex="3">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataColumn Caption="Situação" FieldName="SIT_MATRICULA" VisibleIndex="5">
                        </dxwgv:GridViewDataColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </asp:Panel>
            <asp:ObjectDataSource ID="odsCuidador" TypeName="Techne.Lyceum.Net.Academico.Alunos"
                runat="server" SelectMethod="ListaCuidador">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsLedor" TypeName="Techne.Lyceum.Net.Academico.Alunos"
                runat="server" SelectMethod="ListaLedor">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsInterprete" TypeName="Techne.Lyceum.Net.Academico.Alunos"
                runat="server" SelectMethod="ListaInterprete">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsSalaRecurso" TypeName="Techne.Lyceum.Net.Academico.Alunos"
                runat="server" SelectMethod="ListaSalaRecurso">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsPAPEE" TypeName="Techne.Lyceum.Net.Academico.Alunos"
                runat="server" SelectMethod="ListaPAPEE">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnAnterior4" runat="server" Text="<< Anterior" OnClick="btnAnterior4_Click">
                    </dxe:ASPxButton>
                </td>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnProximo5" runat="server" Text="Próximo >>" OnClick="btnProximo5_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pntabIrmaos" runat="server">
        <asp:Panel ID="pnIrmaos" runat="server">
            <asp:Panel ID="pnPesquisar" GroupingText="Pesquisar Irmãos" runat="server">
                <table width="100%">
                    <tr>
                        <td style="text-align: right">
                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMae" runat="server" Text="Mãe:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMae" runat="server" Width="250px" Editable="true" onkeypress="return nomeSemNum(event); return removeEspacosDuplicados(event); return validaLetrasConsec(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <caption>
                        <br />
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="lblPai" runat="server" Font-Names="Verdana" Font-Size="Smaller" SkinID="lblObrigatorio"
                                    Text="Pai:*"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPai" runat="server" onkeypress="return nomeSemNum(event); return removeEspacosDuplicados(this); return validaLetrasConsec(this);"
                                    Width="250px"></asp:TextBox><asp:Label ID="lblMsgBuscaIrmaos" runat="server" Font-Names="Verdana"
                                        Font-Size="X-Small" ForeColor="red" Text=" (Para efetuar a busca é necessário preencher pelo menos dois campos.)"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label12" runat="server" Font-Names="Verdana" Font-Size="Smaller" SkinID="lblObrigatorio"
                                    Text="Nome Irmão:*"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNomeIrmao" runat="server" onkeypress="return nomeSemNum(event); return removeEspacosDuplicados(this); return validaLetrasConsec(this);"
                                    Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnDataNascimento" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2" width="100%">
                                <asp:ImageButton ID="btnBuscarIrmaos" runat="server" OnClick="btnBuscarIrmaos_Click"
                                    SkinID="BcBuscar" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMensagemIrmaos" runat="server" SkinID="lblMensagem"></asp:Label>
                            </td>
                        </tr>
                    </caption>
                </table>
                <table>
                    <dxwgv:ASPxGridView ClientInstanceName="grdBuscaIrmaos" ID="grdBuscaIrmaos" runat="server"
                        AutoGenerateColumns="false" KeyFieldName="Aluno" Width="100%" AllowPaging="true"
                        EnableCallBacks="true" OnAfterPerformCallback="grdBuscaIrmaos_AfterPerformCallback">
                        <SettingsPager PageSize="10" />
                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewDataCheckColumn Caption="#" FieldName="IRMAOS_FLAG" Name="IRMAOS"
                                VisibleIndex="1">
                                <DataItemTemplate>
                                    <asp:CheckBox ID="chkIrmaos" runat="server" Checked="false" AutoPostBack="false" />
                                </DataItemTemplate>
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOA" VisibleIndex="0"
                                Name="Pessoa" Width="20" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" VisibleIndex="2"
                                Name="ALUNO" Width="20">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome da Mãe" FieldName="NOME_MAE" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome do Pai" FieldName="NOME_PAI" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn VisibleIndex="6" Caption="Nascimento" Name="DT_NASC"
                                FieldName="DT_NASC" Width="100px" Visible="true" ReadOnly="true">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                </PropertiesDateEdit>
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Cert. de Nascimento" FieldName="CERT_NASC_NUM"
                                VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Termo" FieldName="CERT_NUMERO_MATRICULA" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" Visible="false" FieldName="UNIDADE_ENSINO"
                                VisibleIndex="8">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="9">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <CommandColumn Wrap="False">
                            </CommandColumn>
                        </Styles>
                    </dxwgv:ASPxGridView>
                    <tr>
                        <td>
                            <asp:Button ID="btnSalvarIrmaos" Width="160px" Text="Salvar Irmãos Cadastrados" runat="server"
                                OnClick="btnSalvarIrmaos_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnIrmaosCadastrados" GroupingText="Irmãos Cadastrados" runat="server">
                <table>
                    <dxwgv:ASPxGridView ClientInstanceName="grdBuscaIrmaosCadastrados" ID="grdBuscaIrmaosCadastrados"
                        runat="server" AutoGenerateColumns="false" KeyFieldName="ALUNO" Width="100%"
                        OnRowDeleting="grdBuscaIrmaosCadastrados_RowDeleting">
                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ProcessSelectionChangedOnServer="true" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="PARENTESCOID" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" VisibleIndex="2"
                                Width="20">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome da Mãe" FieldName="NOME_MAE" VisibleIndex="4"
                                Name="NOME_MAE">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome do Pai" FieldName="NOME_PAI" VisibleIndex="5"
                                Name="NOME_PAI">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn VisibleIndex="6" Caption="Nascimento" Name="DT_NASC"
                                FieldName="DT_NASC" Width="100px" Visible="true" ReadOnly="true">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                </PropertiesDateEdit>
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Cert. de Nascimento" FieldName="CERT_NASC_NUM"
                                VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Termo" FieldName="CERT_NUMERO_MATRICULA" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" Visible="false" FieldName="UNIDADE_ENSINO"
                                VisibleIndex="8">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="9">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <CommandColumn Wrap="False">
                            </CommandColumn>
                        </Styles>
                    </dxwgv:ASPxGridView>
                </table>
                <asp:Label ID="Label22" runat="server" SkinID="lblMensagem"></asp:Label></asp:Panel>
        </asp:Panel>
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnAnterior5" runat="server" Text="<< Anterior" OnClick="btnAnterior5_Click">
                    </dxe:ASPxButton>
                </td>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnProximo6" runat="server" Text="Próximo >>" OnClick="btnProximo6_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pntabAEDH" runat="server">
        <asp:HiddenField ID="hdnIdAEDH" runat="server" />
        <asp:HiddenField ID="hdnDataMatricula" runat="server" />
        <asp:HiddenField ID="hdnPerfilAEDH" runat="server" />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label29" runat="server" SkinID="lblMensagem" Text="Para os demais casos de amparo que não sejam AEDH, favor acessar a tela Controle de Faltas Justificadas."></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnAEDH" GroupingText="AEDH - Escolarização em Outros Espaços" runat="server"
            Width="50%">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label15" runat="server" SkinID="lblObrigatorio" Text="Ano:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAnoAEDH" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label26" runat="server" SkinID="lblObrigatorio" Text="Período:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPeriodoAEDH" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label28" runat="server" SkinID="lblObrigatorio" Text="Turma:* "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblTurmaAEDH" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        <asp:HiddenField ID="hdnCensoAEDH" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblTpAtendimento" runat="server" SkinID="lblObrigatorio" Text="Tipo de Atendimento:* "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblTpAtendimento" runat="server" RepeatDirection="Horizontal"
                            Width="150px" Enabled="false">
                            <asp:ListItem Text="Hospitalar" Value="Hospitalar"></asp:ListItem>
                            <asp:ListItem Text="Domiciliar" Value="Domiciliar"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblDtInicioAEDH" runat="server" SkinID="lblObrigatorio" Text="Data Início:* "></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtInicioAEDH" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                            ClientInstanceName="dtInicioAEDH" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblDtFimAEDH" runat="server" SkinID="lblObrigatorio" Text="Data Fim:* "></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtFimAEDH" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                            ClientInstanceName="dtFimAEDH" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblLaudoEntregue" runat="server" SkinID="lblObrigatorio" Text="Laudo Entregue?* "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblLaudoEntregue" runat="server" RepeatDirection="Horizontal"
                            Width="150px">
                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblReqAtendEntregue" runat="server" SkinID="lblObrigatorio" Text="Requerimento de Atendimento Entregue?* "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblReqAtendEntregue" runat="server" RepeatDirection="Horizontal"
                            Width="150px">
                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblPlEspecial" runat="server" SkinID="lblObrigatorio" Text="Plano Especial de Estudo Entregue?* "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblPlEspecial" runat="server" RepeatDirection="Horizontal"
                            Width="150px">
                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNumSEI" runat="server" Text="Número do Processo SEI:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumSEI" runat="server" MaxLength="20" Width="100px" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblProrrogacao" runat="server" SkinID="lblObrigatorio" Text="Houve Prorrogação?* "></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblProrrogacao" runat="server" RepeatDirection="Horizontal"
                            Width="150px">
                            <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblDescricao" runat="server" Text="Descrição:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescricaoAEDH" runat="server" MaxLength="1000" Width="300px"
                            Height="100px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <asp:Button ID="btnSalvarAEDH" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                            OnClick="btnSalvarAEDH_Click" />
                    </td>
                    <td colspan="4" align="right">
                        <asp:Button ID="btnCancelarAEDH" runat="server" Text="Cancelar" OnClick="btnCancelarAEDH_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ObjectDataSource ID="odsAEDH" runat="server" TypeName="Techne.Lyceum.Net.Academico.Alunos"
            SelectMethod="ListaAEDH">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdAEDH" runat="server" DataSourceID="odsAEDH" KeyFieldName="ATENDIMENTOOUTROESPACOID"
            AutoGenerateColumns="false" ClientInstanceName="grdAEDH" OnInitNewRow="grdAEDH_InitNewRow"
            EnableCallBacks="false" OnAfterPerformCallback="grdAEDH_AfterPerformCallback"
            OnCustomButtonCallback="grdAEDH_CustomButtonCallback" OnCustomButtonInitialize="grdAEDH_CustomButtonInitialize">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center" id="dvteste">
                            <input type="image" id="btnNovoGridAEDH" src="../img/bt_novo.png" style="cursor: pointer"
                                title="Novo" onserverclick="HabilitaPnlNovoAEDH" runat="server" />
                        </div>
                    </HeaderCaptionTemplate>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarAEDH" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirAEDH" Visibility="AllDataRows"
                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" FieldName="ATENDIMENTOOUTROESPACOID"
                    VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano Letivo" FieldName="ANO" VisibleIndex="5">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="PERIODO" VisibleIndex="6">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo de Atendimento" FieldName="TIPO" VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO" VisibleIndex="9">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="10">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Laudo Entregue?" FieldName="LAUDO" VisibleIndex="11"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Requerimento de Atendimento Entregue?" FieldName="REQUERIMENTO"
                    VisibleIndex="12" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Plano Especial de Estudo Entregue?" FieldName="PLANOESPECIAL"
                    Visible="false" VisibleIndex="13">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Laudo Entregue?" FieldName="DESCRLAUDO" VisibleIndex="11">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Requerimento de Atendimento Entregue?" FieldName="DESCRREQUERIMENTO"
                    VisibleIndex="12">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Plano Especial de Estudo Entregue?" FieldName="DESCRPLANOESPECIAL"
                    VisibleIndex="13">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Número do Processo SEI" Name="NUMEROSEI" VisibleIndex="14"
                    FieldName="NUMEROSEI">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Houve Prorrogação?" FieldName="PRORROGACAO"
                    VisibleIndex="15" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Houve Prorrogação?" FieldName="DESCRPRORROGACAO"
                    VisibleIndex="15">
                    <CellStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição" Name="DESCRICAO" VisibleIndex="16"
                    FieldName="DESCRICAO">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <dxe:ASPxButton ID="btnAnterior6" runat="server" Text="<< Anterior" OnClick="btnAnterior6_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
        <dxpc:ASPxPopupControl ID="pucConfirmarAEDH" ClientInstanceName="pucConfirmarAEDH"
            runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
            ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
            EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
            <ContentCollection>
                <dxpc:PopupControlContentControl>
                    <table>
                        <tr align="center">
                            <td>
                                Confirma a exclusão do atendimento?<br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnSimAEDH" runat="server" Text="Sim" OnClick="btnSimAEDH_Click" />
                                <asp:Button ID="btnNaoAEDH" runat="server" Text="Não" OnClick="btnNaoAEDH_Click" />
                            </td>
                        </tr>
                    </table>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
    </asp:Panel>
    <asp:Panel ID="pnlBuscaAlunoNovo" runat="server" Visible="false" Width="900px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="text-align: right" class="style4">
                    <asp:Label ID="lblNomeAlunoNovo" runat="server" SkinID="lblObrigatorio" Text="Nome:* "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeAlunoNovo" runat="server" Width="380px" onkeypress="return nomeSemNum(event);" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="style4">
                    <asp:Label ID="lblMaeAlunoNovo" runat="server" SkinID="lblObrigatorio" Text="Mãe:* "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeMaeAlunoNovo" runat="server" Width="380px" />
                    <asp:CheckBox runat="server" ID="chkNaoDeclarMaeNovo" Text="Não Declarada" AutoPostBack="true"
                        OnCheckedChanged="chkNaoDeclarMaeNovo_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="style4">
                    <asp:Label ID="lbldataNascAlunoNovo" runat="server" SkinID="lblObrigatorio" Text="Data Nascimento:* "></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataNascAlunoNovo" runat="server" MinDate="1901-01-01" Width="120px"
                        EnableDefaultAppearance="true" ClientInstanceName="dtDataNascAlunoNovo" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" ClientEnabled="True">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="btnNovoBusca" runat="server" SkinID="BcNovo" OnClick="btnNovoBusca_Click"
                        Visible="false" />
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxwgv:ASPxGridView ID="grdBusca" KeyFieldName="ALUNO" runat="server" EnableRowsCache="false"
                        EnableViewState="false" ClientInstanceName="grdBusca" AutoGenerateColumns="False"
                        Font-Names="Verdana" DataSourceID="odsBuscaAlunoNovo" Font-Size="Small" OnSelectionChanged="grdBusca_SelectionChanged"
                        EnableCallBacks="false">
                        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="True" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="5%">
                                <SelectButton Text="Selecionar" Visible="True">
                                    <Image Url="~/img/bt_busca.png" />
                                </SelectButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataColumn FieldName="ALUNO" Caption="Aluno" VisibleIndex="0" Width="100px"
                                Name="aluno" />
                            <dxwgv:GridViewDataColumn FieldName="NOME_COMPL" Caption="Nome" VisibleIndex="0"
                                Width="150px" />
                            <dxwgv:GridViewDataColumn FieldName="RG_NUM" Caption="Documento" VisibleIndex="0"
                                Width="100px" />
                            <dxwgv:GridViewDataColumn FieldName="CPF" Caption="CPF" VisibleIndex="0" Width="100px" />
                            <dxwgv:GridViewDataColumn FieldName="NOME_MAE" Caption="Nome da Mãe" VisibleIndex="0"
                                Width="150px" />
                            <dxwgv:GridViewDataColumn FieldName="NOME_PAI" Caption="Nome do Pai" VisibleIndex="0"
                                Width="150px" />
                            <dxwgv:GridViewDataColumn FieldName="DT_NASC" Caption="Nascimento" VisibleIndex="0"
                                Width="100px" />
                            <dxwgv:GridViewDataColumn FieldName="NUMINSCRICAO" Caption="Inscrição Matrícula"
                                Width="100px" VisibleIndex="0" />
                            <dxwgv:GridViewDataColumn FieldName="UNIDADE_ENSINO" Caption="Escola" Width="100px"
                                VisibleIndex="0" />
                            <dxwgv:GridViewDataColumn FieldName="SIT_ALUNO" Caption="situação do cadastro" Width="100px"
                                VisibleIndex="0" />
                            <dxwgv:GridViewDataColumn FieldName="nome_comp" Caption="Nome da Unidade" Width="100px"
                                VisibleIndex="0" />
                        </Columns>
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMensagemAlunoNovo" runat="server" SkinID="lblMensagem" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlNovaMatricula" runat="server" Visible="false" Width="900px">
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMensagemBloqueio" runat="server" SkinID="lblMensagem"></asp:Label><br />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label27" runat="server" Text="Ano:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAnoMatricula" runat="server" DataTextField="ano" DataValueField="ano"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlAnoMatricula_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label23" runat="server" Text="Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodoMatricula" runat="server" DataTextField="periodo"
                        DataValueField="periodo" OnSelectedIndexChanged="ddlPeriodoMatricula_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label18" runat="server" Text="Unidade de Ensino:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseUnidadeEnsinoMatricula" AutoPostBack="true" runat="server"
                        Key="unidade_ens" Argument="nome_comp" Caption="" OnLoad="tseUnidadeEnsinoMatricula_Load"
                        MaxLength="20" GridWidth="850px" OnChanged="tseUnidadeEnsinoMatricula_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="municipio" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label19" runat="server" Text="Nível/Segmento*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlNivelMatricula" runat="server" DataTextField="DESCRICAO"
                        DataValueField="TIPO" AutoPostBack="true" OnSelectedIndexChanged="ddlNivelMatricula_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label20" runat="server" Text="Modalidade*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlModalidadeMatricula" runat="server" DataTextField="DESCRICAO"
                        DataValueField="MODALIDADE" AutoPostBack="true" OnSelectedIndexChanged="ddlModalidadeMatricula_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label21" runat="server" Text="Curso:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseCursoMatricula" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome, pc.curso as pccurso FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO
                                                left join LY_EVENTO_GERAL pc on pc.CURSO = uec.CURSO and pc.TIPO_FILTRO = 'Bloqueio_Cadastro_Aluno' and CONVERT(date,GetDate()) between pc.DT_INICIO and DT_FIM"
                        ArgumentColumns="60" Columns="10" MaxLength="20" GridWidth="800px" SqlOrder="nome"
                        OnChanged="tseCursoMatricula_Changed" SqlWhere="pc.curso is null">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label9" runat="server" Text="Turno:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbTurnoMatricula" runat="server" DataTextField="descricao"
                        DataValueField="turno" OnSelectedIndexChanged="cmbTurnoMatricula_SelectedIndexChanged"
                        AutoPostBack="true" Width="200px">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label10" runat="server" Text="Matriz Curricular:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbCurriculoMatricula" runat="server" DataTextField="curriculo"
                        DataValueField="curriculo" DataSourceID="odsCurriculoMatricula" OnSelectedIndexChanged="cmbCurriculoMatricula_SelectedIndexChanged"
                        AppendDataBoundItems="false" AutoPostBack="true" Width="200px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ObjectDataSource ID="odsCurriculoMatricula" runat="server" TypeName="Techne.Lyceum.RN.Curriculo"
                        SelectMethod="ConsultarMatrizAluno">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="tseCursoMatricula" PropertyName="DBValue" Name="curso" />
                            <asp:ControlParameter ControlID="cmbTurnoMatricula" PropertyName="SelectedValue"
                                Name="turno" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label13" runat="server" Text="Série/Ano Escolar:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerieMatricula" runat="server" DataTextField="descricao"
                        DataValueField="serie" AppendDataBoundItems="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnProsseguirMatricula" runat="server" Style="text-align: right"
                        Text="Prosseguir Matrícula" OnClick="btnProsseguirMatricula_Click" ValidationGroup="ProsseguirMatricula" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:TextBox ID="hdnIdade" runat="server" MaxLength="20" ReadOnly="true" Visible="false"
        Width="150px" />
    <asp:ObjectDataSource ID="odsImprimirConfirmacao" runat="server" TypeName="Techne.Lyceum.RN.ConfirmacaoMatricula"
        SelectMethod="ListaConfirmacaoMatriculaConfirmadaPor">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtAluno" Name="aluno" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsImprimirRenovacao" runat="server" TypeName="Techne.Lyceum.RN.RenovacaoMatricula.Renovacao"
        SelectMethod="ListaRenovacoesMatriculaAtivaOuPossuiConfirmacaoPor">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtAluno" Name="aluno" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="410px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Busca Aluno Novo">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:HiddenField runat="server" ID="hdnAlunoTransf" />
                <table id="Table1" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblMensagemPopup" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnConfirmarTransf" runat="server" Text="Sim" OnClick="btnConfirmarTransf_Click"
                                OnClientClick="pucConfirmar.Hide(); return true;" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Não" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <asp:ObjectDataSource ID="odsBuscaAlunoNovo" runat="server" TypeName="Techne.Lyceum.RN.Aluno"
        SelectMethod="ListarAlunoNovo">
        <SelectParameters>
            <asp:Parameter Name="aluno" DefaultValue="0" />
            <asp:ControlParameter ControlID="txtNomeAlunoNovo" Name="nome" PropertyName="Text" />
            <asp:ControlParameter ControlID="txtNomeMaeAlunoNovo" Name="mae" PropertyName="Text" />
            <asp:ControlParameter ControlID="dtDataNascAlunoNovo" Name="dataNascimento" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 38px;
        }
        .style4
        {
            width: 94px;
        }
        .style5
        {
            height: 19px;
        }
    </style>
</asp:Content>
