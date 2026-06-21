using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Certificacao;
using DevExpress.Web.ASPxTabControl;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;
using System.Text;
using Seeduc.Infra.Helpers;
using DevExpress.Web.ASPxEditors;
using System.Net.Mail;
using System.Net;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;
using System.Web.Script.Serialization;
using System.IO;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/PainelCertificacao.aspx"), ControlText("Painel Certificação"), Title("Painel Certificação")]
    public partial class PainelCertificacao : TPage
    {
        public object ListaAluno(object nome, object cpf, object numeroProtocolo, object statusSolicitacao, object polo, object dataIni, object dataFim)
        {
            if (statusSolicitacao == null)
            {
                return null;
            }

            int? unidade = !Convert.ToString(polo).IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(polo) : (int?)null;

            RN.Certificacao.EnccejaRequerimento rnEnccejaRequerimento = new RN.Certificacao.EnccejaRequerimento();
            var dados = rnEnccejaRequerimento.ListaEnccejaAlunoPor(Convert.ToString(nome), Convert.ToString(cpf), Convert.ToString(numeroProtocolo), Convert.ToString(statusSolicitacao), User.Identity.Name, unidade, Convert.ToDateTime(dataIni ?? SqlDateTime.MinValue.Value), Convert.ToDateTime(dataFim ?? SqlDateTime.MaxValue.Value));

            return dados;
        }

        public object ListarMotivo()
        {
            RN.Certificacao.MotivoIndeferido rnMotivo = new RN.Certificacao.MotivoIndeferido();
            return rnMotivo.ListaAtivoPor();
        }

        void grdDocumento_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";
            }
        }

        void grdDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).DataBind();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
                lblMensagem.Text = string.Empty;
                CarregarArquivos();

                if (!IsPostBack)
                {
                    CarregaUnidadeCertificadora();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdDocumento, string.Empty);
            ControlaAcesso(this.grdDocumento, AcaoControle.editar, "btnSalvarGrid");
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNome.Text = "";
            txtCpf.Text = "";
            txtNumeroProtocolo.Text = "";
            rbStatusSituacao.ClearSelection();
            ddlUnidade.ClearSelection();
            hdnEnccejaRequirimentoId.Value = string.Empty;
            dtDataIni.Value = null;
            dtDataFim.Value = null;
        }

        protected void EnviaEmailAlunosCertEmitido(string nome, string email, string cpf, string dataNascimento, string nomeUnidade, string enderecoUnidade)
        {
            EmailApi rnEmailApi = new EmailApi();

            try
            {
                var from = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_From"];
                var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_FromName"];
                var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Bcc"];
                var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_BccName"];
                var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Password"];
                var subject = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Subject"];
                var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_UrlRoot"];

                string emailMessage = @"
                
                <table cellpadding=""0"" cellspacing=""0"" width=""600"" border=""0"" style=""border:1px solid gray;"">
                    <tbody>
                        <tr>
                            <td colspan=""3"" height=""120"" style=""border-bottom:1px solid gray;"">
                                <img src=""{{URL_ROOT}}/images/email-top.png"">
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""35"" align=""center"" valign=""middle"" bgcolor=""#e0eaf1"">
                                <p>Prezado, {{NOME}}</p>
                            </td>
                        </tr>
                        
                        <tr>
                            <td width=""200"" style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">CPF:</td>
                            <td width=""400"" colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{CPF}}</td>
                        </tr>
                        <tr>
                            <td width=""200"" style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">DATA DE NASCIMENTO:</td>
                            <td width=""400"" colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{DATANASCIMENTO}}</td>                        
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""35"" style=""padding-right:5px;"" align=""center"" bgcolor=""#e0eaf1"">O seu certificado escolar foi emitido. Encontra-se na unidade solicitada, disponível para a retirada.</td>
                        </tr>
                        <tr>
                            <td style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">Unidade:</td>
                            <td colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{NOME_UNIDADE}}</td>
                        </tr>
                        <tr>
                            <td style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">Endereço:</td>
                            <td colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{ENDERECO_UNIDADE}}</td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""70"" align=""center"" bgcolor=""#e0eaf1"">
                                <p> Atenciosamente, COOGIE </p>
                            </td>
                        </tr>
                        <tr>
                            <td height=""140"" align=""center"" bgcolor=""#246DA7"" width=""180"" style=""border-top:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Siga nossas redes sociais</b></td>
                                        </tr>
                                        <tr>
                                            <td width=""60"" height=""100"">
                                                <img src=""{{URL_ROOT}}/images/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-instagram.png"">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align=""center"" bgcolor=""#246DA7"" width=""180"" style=""border-top:1px solid gray;border-left:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan="" 3"" align="" center"" valign="" bottom"" height="" 40"" style="" color: white; font-size: small;""></td>
                                        </tr>
                                        <tr>
                                            <td colspan="" 3"" align="" center"" height="" 100"" style="" color: white; font-size: small"">
                                                <p> Para maiores informações acesse: https://www.inspecaoescolar.educacao.rj.gov.br/</p>
                                            </td>
                                        </tr>
                                    </tbody>


                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>

                ";

                emailMessage = emailMessage
                    .Replace("{{URL_ROOT}}", urlRoot)
                    .Replace("{{NOME}}", nome)
                    .Replace("{{CPF}}", cpf)
                    .Replace("{{DATANASCIMENTO}}", dataNascimento)
                    .Replace("{{NOME_UNIDADE}}", nomeUnidade)
                    .Replace("{{ENDERECO_UNIDADE}}", enderecoUnidade)
                ;

                var emailObject = new RN.Util.EmailApi.EmailDTO
                {
                    Smtp = new EmailApi.EmailDTO.SmtpDTO
                    {
                        Host = host,
                        Port = port,
                        UserName = userName,
                        Password = password,
                        EnableSSL = true,
                    },
                    Message = new EmailApi.EmailDTO.MessageDTO
                    {
                        From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
                        {
                            Address = from,
                            Name = fromName
                        },
                        To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
                        {
                            new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = nome },
                        },
                        Subject = subject,
                        Body = emailMessage,
                        IsBodyHtml = true,
                    },
                };

                if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void EnviaEmailAlunosCertIndeferido(string nome, string email, string cpf, string dataNascimento, string dataIndeferido, string motivoIndeferido)
        {
            EmailApi rnEmailApi = new EmailApi();

            try
            {
                var from = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_From"];
                var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_FromName"];
                var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Bcc"];
                var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_BccName"];
                var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Password"];
                var subject = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_Subject"];
                var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailCertificacaoEncceja_UrlRoot"];

                string emailMessage = @"

                <table cellpadding=""0"" cellspacing=""0"" width=""600"" border=""0"" style=""border:1px solid gray;"">
                    <tbody>
                        <tr>
                            <td colspan=""3"" height=""120"" style=""border-bottom:1px solid gray;"">
                                <img src=""{{URL_ROOT}}/images/email-top.png"">
                            </td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""35"" align=""center"" valign=""middle"" bgcolor=""#e0eaf1"">
                                <p>Prezado, {{NOME}}</p>
                            </td>
                        </tr>
                        
                        <tr>
                            <td width=""200"" style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">CPF:</td>
                            <td width=""400"" colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{CPF}}</td>
                        </tr>
                        <tr>
                            <td width=""200"" style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">DATA DE NASCIMENTO:</td>
                            <td width=""400"" colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{DATANASCIMENTO}}</td>                        
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""35"" style=""padding-right:5px;"" align=""center"" bgcolor=""#e0eaf1"">A solicitação de emissão do certificado escolar foi indeferida. O motivo está indicado abaixo. Será necessário realizar uma nova solicitação para a emissão.</td>
                        </tr>
                        <tr>
                            <td style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">Data Indeferimento:</td>
                            <td colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{DATA_INDEFERIDO}}</td>
                        </tr>
                        <tr>
                            <td style=""padding-right:5px;"" align=""right"" bgcolor=""#e0eaf1"">Motivo:</td>
                            <td colspan=""2"" style=""text-align:left;font-weight:bold;"" bgcolor=""#e0eaf1"">{{MOTIVO_INDEFERIDO}}</td>
                        </tr>
                        <tr>
                            <td colspan=""3"" height=""70"" align=""center"" bgcolor=""#e0eaf1"">
                                <p> Atenciosamente, COOGIE </p>
                            </td>
                        </tr>
                        <tr>
                            <td height=""140"" align=""center"" bgcolor=""#246DA7"" width=""180"" style=""border-top:1px solid gray;"">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan=""3"" align=""center"" valign=""bottom"" height=""40"" style=""color:white;font-size:small;""><b>Siga nossas redes sociais</b></td>
                                        </tr>
                                        <tr>
                                            <td width=""60"" height=""100"">
                                                <img src=""{{URL_ROOT}}/images/email-fb.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-twitter.png"">
                                            </td>
                                            <td width=""60"">
                                                <img src=""{{URL_ROOT}}/images/email-instagram.png"">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align=""center"" bgcolor=""#246DA7"" width=""180"" style=""border-top:1px solid gray;border-left:1px solid gray;"">
                                <table>

                                    <tbody>
                                        <tr>
                                            <td colspan="" 3"" align="" center"" valign="" bottom"" height="" 40"" style="" color: white; font-size: small;""></td>
                                        </tr>
                                        <tr>
                                            <td colspan="" 3"" align="" center"" height="" 100"" style="" color: white; font-size: small"">
                                                <p> Para maiores informações acesse: https://www.inspecaoescolar.educacao.rj.gov.br/</p>
                                            </td>
                                        </tr>
                                    </tbody>
                                    
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>

                ";

                emailMessage = emailMessage
                    .Replace("{{URL_ROOT}}", urlRoot)
                    .Replace("{{NOME}}", nome)
                    .Replace("{{CPF}}", cpf)
                    .Replace("{{DATANASCIMENTO}}", dataNascimento)
                    .Replace("{{DATA_INDEFERIDO}}", dataIndeferido)
                    .Replace("{{MOTIVO_INDEFERIDO}}", motivoIndeferido)
                ;

                var emailObject = new EmailApi.EmailDTO
                {
                    Smtp = new EmailApi.EmailDTO.SmtpDTO
                    {
                        Host = host,
                        Port = port,
                        UserName = userName,
                        Password = password,
                        EnableSSL = true,
                    },
                    Message = new EmailApi.EmailDTO.MessageDTO
                    {
                        From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
                        {
                            Address = from,
                            Name = fromName
                        },
                        To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
                        {
                            new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = nome },
                        },
                        Subject = subject,
                        Body = emailMessage,
                        IsBodyHtml = true,
                    },
                };

                if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
                    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdDocumento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.EnccejaRequerimento rnEnccejaRequerimento = new RN.Certificacao.EnccejaRequerimento();
            RN.Certificacao.DTOs.PainelCertificacao rnPainelentidade = new RN.Certificacao.DTOs.PainelCertificacao();
            RN.Certificacao.MotivoIndeferido rnMotivoIndeferido = new MotivoIndeferido();

            if (e.ButtonID == "btnSalvarGrid")
            {
                try
                {
                    int EnccejaRequerimentoId = Convert.ToInt32(grdDocumento.GetRowValues(e.VisibleIndex, "ENCCEJAREQUERIMENTOID"));
                    int SituacaoEnccejaRequerimentoId = Convert.ToInt32(grdDocumento.GetRowValues(e.VisibleIndex, "SITUACAOENCCEJAREQUERIMENTOID"));
                    SituacaoEnccejaRequerimentoId = 1;
                    RadioButtonList rblistSituacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["Situacao"] as GridViewDataColumn, "rblistSituacao") as RadioButtonList;

                    ListItem radioButtonEmitido = null;
                    ListItem radioButtonEntregue = null;
                    ListItem radioButtonIndeferido = null;
                    ListItem radioButtonSolicitado = null;

                    foreach (var item in rblistSituacao.Items)
                    {
                        var temp = item as ListItem;
                        if (temp.Text == "Emitido")
                        {
                            radioButtonEmitido = temp;
                        }
                        else if (temp.Text == "Entregue")
                        {
                            radioButtonEntregue = temp;
                        }
                        else if (temp.Text == "Indeferido")
                        {
                            radioButtonIndeferido = temp;
                        }
                        else if (temp.Text == "Solicitado")
                        {
                            radioButtonSolicitado = temp;
                        }
                    }


                    ASPxDateEdit dtVerificacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DATAVERIFICACAO"] as GridViewDataColumn, "dtVerificacao") as ASPxDateEdit;
                    ASPxDateEdit dtEntrega = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DTENTREGA"] as GridViewDataColumn, "dtEntrega") as ASPxDateEdit;
                    DropDownList dpdMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "cmbMotivo") as DropDownList;
                    DateTime dataSolicitacao = Convert.ToDateTime(grdDocumento.GetRowValues(e.VisibleIndex, "DATASOLICITACAO"));


                    rnPainelentidade.SituacaoEnccejaRequerimentoId = !rblistSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblistSituacao.SelectedValue) : -1;
                    rnPainelentidade.MotivoIndeferidoId = !dpdMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(dpdMotivo.SelectedItem.Value) : (int?)null;
                    rnPainelentidade.DataSolicitacao = dataSolicitacao;
                    rnPainelentidade.DataVerificacao = !dtVerificacao.Date.ToString().IsNullOrEmptyOrWhiteSpace() ? dtVerificacao.Date : new DateTime(1753, 1, 1);
                    rnPainelentidade.EnccejaRequerimentoId = EnccejaRequerimentoId;
                    rnPainelentidade.UsuarioId = User.Identity.Name;

                    int result = -1;
                    if (dpdMotivo.SelectedItem.Value != null)
                    {
                        if (!Int32.TryParse(dpdMotivo.SelectedItem.Value, out result))
                        {
                            result = rnMotivoIndeferido.ObtemIdMotivoInderidoPor(Convert.ToString(dpdMotivo.SelectedItem.Value));
                        }
                    }

                    rnPainelentidade.MotivoIndeferidoId = (result > 0 ? result : (int?)null);
                    rnPainelentidade.Entregue = radioButtonEntregue.Selected;

                    string nome = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "NOME"));
                    string cpf = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "CPF"));
                    string datanascimento = Convert.ToDateTime(Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "DATANASCIMENTO"))).ToShortDateString();
                    string email = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "EMAIL"));

                    string logradouro = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "LOGRADOURO"));
                    string numero = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "NUMERO"));
                    string bairro = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "BAIRRO"));
                    string complemento = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "COMPLEMENTO"));
                    string municipio = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "MUNICIPIO"));
                    string dataIndeferido = !String.IsNullOrEmpty(dtVerificacao.Date.ToString()) ? dtVerificacao.Date.ToString("dd/MM/yyyy") : "";
                    string motivoIndeferido = !String.IsNullOrEmpty(dpdMotivo.SelectedItem.Text) ? dpdMotivo.SelectedItem.Text : "";
                    string endereco = string.Format("{0}, {1}{2} - {3}, {4} / RJ",
                             logradouro,
                             numero,
                             complemento.IsNullOrEmptyOrWhiteSpace() ? string.Empty : " " + complemento,
                             bairro,
                             municipio);

                    string unidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "UNIDADE"));
                    string logradouroUnidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "ENDERECOUNIDADE"));
                    string numeroUnidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "NUMEROUNIDADE"));
                    string bairroUnidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "BAIRROUNIDADE"));
                    string complementoUnidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "COMPLEMENTOUNIDADE"));
                    string municipioUnidade = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "MUNICIPIOUNIDADE"));
                    string enderecoUnidade = string.Format("{0}, {1}{2} - {3}, {4} / RJ",
                             logradouroUnidade,
                             numeroUnidade,
                             complementoUnidade.IsNullOrEmptyOrWhiteSpace() ? string.Empty : " " + complementoUnidade,
                             bairroUnidade,
                             municipioUnidade);


                    rnPainelentidade.DataSolicitacao = Convert.ToDateTime(dataSolicitacao);
                    rnPainelentidade.DataVerificacao = Convert.ToDateTime(!String.IsNullOrEmpty(dtVerificacao.Date.ToString()) ? dtVerificacao.Date.ToString() : null);
                    rnPainelentidade.DataEntrega = Convert.ToDateTime(!String.IsNullOrEmpty(dtEntrega.Date.ToString()) ? dtEntrega.Date.ToString() : DateTime.MinValue.ToString());
                    rnPainelentidade.EnccejaRequerimentoId = EnccejaRequerimentoId;

                    validacao = rnEnccejaRequerimento.Valida(rnPainelentidade);

                    if (!validacao.Valido)
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                    else
                    {
                        rnEnccejaRequerimento.Atualiza(rnPainelentidade);
                        odsDocumento.Select();
                        odsDocumento.DataBind();
                        grdDocumento.DataBind();

                        //Envia email de acordo com a situação
                        if (rnPainelentidade.SituacaoEnccejaRequerimentoId == 2) //Emitido
                        {
                            EnviaEmailAlunosCertEmitido(nome, email, cpf, datanascimento, unidade, enderecoUnidade);
                        }
                        else if (rnPainelentidade.SituacaoEnccejaRequerimentoId == 4) //Indeferido
                        {
                            EnviaEmailAlunosCertIndeferido(nome, email, cpf, datanascimento, dataIndeferido, motivoIndeferido);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        lblMensagem.Text += "<br />" + ex.Message;
                    }
                }
            }
        }

        protected DateTime VerificaData(object valor)
        {
            if (valor is DBNull)
            {
                return DateTime.MinValue;
            }

            return (DateTime)valor;
        }

        protected void grdDocumento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (this.grdDocumento.Visible || this.grdDocumento.VisibleRowCount == 0)
            {
                DropDownList cmbMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "cmbMotivo") as DropDownList;

                if (cmbMotivo != null)
                {
                    HiddenField hfMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "hfMotivo") as HiddenField;
                    TextBox txtMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "txtMotivo") as TextBox;
                    txtMotivo.Visible = false;
                    int MotivoIndeferidoId = !string.IsNullOrEmpty(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDOID").ToString()) ? Convert.ToInt32(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDOID")) : 0;
                    string dataVerificacao = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "DATAVERIFICACAO"));
                    string dataEntrega = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "DATAENTREGA"));
                    RadioButtonList rblistSituacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["Situacao"] as GridViewDataColumn, "rblistSituacao") as RadioButtonList;
                    ListItem radioButtonEmitido = null; ListItem radioButtonEntregue = null; ListItem radioButtonIndeferido = null; ListItem radioButtonSolicitado = null;
                    string motivoDesc = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDODESCRICAO"));


                    foreach (var item in rblistSituacao.Items)
                    {
                        var temp = item as ListItem;
                        if (temp.Text == "Emitido")
                        {
                            radioButtonEmitido = temp;
                        }
                        else if (temp.Text == "Entregue")
                        {
                            radioButtonEntregue = temp;
                        }
                        else if (temp.Text == "Indeferido")
                        {
                            radioButtonIndeferido = temp;
                        }
                        else if (temp.Text == "Solicitado")
                        {
                            radioButtonSolicitado = temp;
                        }
                    }

                    ASPxDateEdit dtVerificacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DATAVERIFICACAO"] as GridViewDataColumn, "dtVerificacao") as ASPxDateEdit;
                    ASPxDateEdit dtEntrega = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DTENTREGA"] as GridViewDataColumn, "dtEntrega") as ASPxDateEdit;

                    if (!dataVerificacao.IsNullOrEmptyOrWhiteSpace())
                    {
                        dtVerificacao.Date = Convert.ToDateTime(dataVerificacao);
                    }
                    if (!dataEntrega.IsNullOrEmptyOrWhiteSpace())
                    {
                        dtEntrega.Date = Convert.ToDateTime(dataEntrega);
                    }

                    string motivoSelecionado = Convert.ToString(MotivoIndeferidoId);

                    if (!string.IsNullOrEmpty(motivoDesc))
                    {
                        if (cmbMotivo.Items.FindByValue(motivoSelecionado) != null)
                        {
                            cmbMotivo.SelectedValue = motivoSelecionado;
                        }
                    }

                    if (radioButtonIndeferido.Selected)
                    {
                        rblistSituacao.Enabled = false;
                        dtVerificacao.Enabled = false;
                        dtEntrega.Enabled = false;
                        txtMotivo.Visible = true;
                        cmbMotivo.Visible = false;

                    }

                    if (radioButtonEmitido.Selected)
                    {
                        dtVerificacao.Enabled = false;
                        dtEntrega.Enabled = true;
                        rblistSituacao.Enabled = true;
                        rblistSituacao.Items[0].Enabled = false;
                        rblistSituacao.Items[1].Enabled = true; //Entregue
                        rblistSituacao.Items[2].Enabled = false;
                        rblistSituacao.Items[3].Enabled = false;
                        txtMotivo.Visible = true;
                        txtMotivo.Enabled = false;
                        cmbMotivo.Visible = false;
                    }

                    if (radioButtonEntregue.Selected)
                    {
                        dtVerificacao.Enabled = false;
                        dtEntrega.Enabled = false;
                        rblistSituacao.Enabled = false;
                        txtMotivo.Visible = true;
                        txtMotivo.Enabled = false;
                        cmbMotivo.Visible = false;
                    }
                    if (radioButtonSolicitado.Selected)
                    {
                        rblistSituacao.Enabled = true;
                    }

                    for (int i = 0; i <= rblistSituacao.Items.Count - 1; i++)
                    {
                        rblistSituacao.Items[i].Attributes.Add("cmbMotivo", cmbMotivo.ClientID);
                    }

                    var observacao = string.IsNullOrEmpty(hfMotivo.Value) ? "Selecione" : hfMotivo.Value;

                    //cmbMotivo.SelectedValue = observacao;



                }


            }
        }

        protected void grdDocumento_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            var status = grdDocumento.GetRowValues(e.VisibleIndex, "SITUACAOENCCEJAREQUERIMENTOID");

            e.Button.Visibility = GridViewCustomButtonVisibility.AllDataRows;

            if (Convert.ToInt32(status) == 3 || Convert.ToInt32(status) == 4)
            {
                e.Button.Visibility = GridViewCustomButtonVisibility.Invisible;
            }
        }

        protected void grdDocumento_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CPF" && e.Value != null)
            {
                e.DisplayText = Convert.ToUInt64(e.Value).ToString(@"000\.000\.000\-00");
            }
        }

        protected void lkDadosCadastro_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (chave != null && chave.Count() > 0)
                {
                    pucDadosCadastro.ShowOnPageLoad = true;
                    string nome = Convert.ToString(chave[0]);
                    string nomeMae = Convert.ToString(chave[1]);
                    string nomePai = Convert.ToString(chave[2]);
                    string cpf = Convert.ToString(chave[3]);
                    string rg = Convert.ToString(chave[4]);
                    string telefone = Convert.ToString(chave[5]);
                    string celular = Convert.ToString(chave[6]);
                    string email = Convert.ToString(chave[7]);
                    string dtnasc = Convert.ToString(chave[8]);

                    lblNome.Text = nome;
                    lblNomePai.Text = nomePai;
                    lblNomeMae.Text = nomeMae;
                    lblCpf.Text = cpf;
                    lblRg.Text = rg;
                    lblTelefone.Text = telefone;
                    lblCelular.Text = celular;
                    lblEmail.Text = email;
                    lblDataNasc.Text = dtnasc;
                    lblLogradouro.Text = Convert.ToString(chave[9]);
                    lblNumero.Text = Convert.ToString(chave[10]);
                    lblBairro.Text = Convert.ToString(chave[11]);
                    lblComplemento.Text = Convert.ToString(chave[12]);
                    lblMunicipio.Text = Convert.ToString(chave[13]);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lkImg_Command(object sender, CommandEventArgs e)
        {
            Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo rnEnccejaDocumentoArquivo = new Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo();

            try
            {
                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });
                hdnEnccejaRequirimentoId.Value = Convert.ToString(chave[0]);
                CarregarArquivos();
                pucArquivos.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarArquivos()
        {
            Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo rnEnccejaDocumentoArquivo = new Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo();

            try
            {
                if(hdnEnccejaRequirimentoId.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    return;
                }

                int requerimentoId = Convert.ToInt32(hdnEnccejaRequirimentoId.Value);

                //Busca arquivos do RG
                DataTable arquivosRg = rnEnccejaDocumentoArquivo.ObtemListaPor(requerimentoId, 1);
                rpAruivosRG.DataSource = arquivosRg;
                rpAruivosRG.DataBind();

                //Busca arquivos do CPF
                DataTable arquivosCpf = rnEnccejaDocumentoArquivo.ObtemListaPor(requerimentoId, 2);
                rpAruivosCPF.DataSource = arquivosCpf;
                rpAruivosCPF.DataBind();

                //Busca arquivos do BOLETIM
                DataTable arquivosBOLETIM = rnEnccejaDocumentoArquivo.ObtemListaPor(requerimentoId, 3);
                rpAruivosBOLETIM.DataSource = arquivosBOLETIM;
                rpAruivosBOLETIM.DataBind();

                //Busca arquivos do HISTORICO
                DataTable arquivosHISTORICO = rnEnccejaDocumentoArquivo.ObtemListaPor(requerimentoId, 4);
                rpAruivosHISTORICO.DataSource = arquivosHISTORICO;
                rpAruivosHISTORICO.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lkArquivo_Command(object sender, CommandEventArgs e)
        {
            Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo rnResolucaoDocumentoArquivo = new Techne.Lyceum.RN.Certificacao.EnccejaDocumentoArquivo();

            try
            {
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    switch (chave[1])
                    {
                        case "1":
                            pucVisualizarArquivo.HeaderText = "RG";
                            break;

                        case "2": pucVisualizarArquivo.HeaderText = "CPF";
                            break;

                        case "3": pucVisualizarArquivo.HeaderText = "BOLETIM";
                            break;

                        case "4": pucVisualizarArquivo.HeaderText = "HISTÓRICO";
                            break;
                    }

                    if (chave[2].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Certificacao/FileCS.ashx?Tabela=certificacaoescolar.ENCCEJAALUNO&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnResolucaoDocumentoArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }

                    grdDocumento.DataBind();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                hdnEnccejaRequirimentoId.Value = string.Empty;

                if (rbStatusSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = " Status solicitação obrigatório ";
                    return;
                }

                string mensagem = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaUnidadeCertificadora()
        {
            RN.Certificacao.UnidadeCertificadora rnUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UnidadeCertificadora();
            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlUnidade.Items.Clear();
                ddlUnidade.DataSource = rnUnidadeCertificadora.ListaUnidadePor(User.Identity.Name);
                ddlUnidade.DataBind();
                ddlUnidade.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdDocumento.PageIndex * grdDocumento.SettingsPager.PageSize;
            for (int i = 0; i < grdDocumento.VisibleRowCount; i++)
            {
                if (grdDocumento.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }


        protected void rblistSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;
            ASPxGridView grd = MainContent.FindControl("grdDocumento") as ASPxGridView;
            //RadioButtonList rbl = grd.FindControl("rblistSituacao") as RadioButtonList;



            int curPageSelection = grdDocumento.FocusedRowIndex;//.EditingRowVisibleIndex;// GetSelectedRowOnTheCurrentPage();

            var situacao = DevExpressHelper.GetControl<RadioButtonList>(this.grdDocumento, curPageSelection, "Situacao", "rblistSituacao");




        }

        
        
       
    }
}
