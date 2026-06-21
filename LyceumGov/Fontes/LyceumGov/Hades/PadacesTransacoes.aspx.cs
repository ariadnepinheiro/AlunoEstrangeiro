using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{

    public class clsTreeView
    {

        public clsTreeView()
        {

        }



        public void TratarEventoCheckedDaTreeView(TreeNode tndNo, bool bolChecked)
        {
            if (bolChecked)
            {
                ChecarNosFilhos(tndNo, true);
                ChecarNosPais(tndNo, true);
            }

            else
            {
                //DesmarcarNoPaiSeFilhoDesmarcado(tndNo);
                ChecarNosFilhos(tndNo, false);
            }
        }


        private void DesmarcarNoPaiSeFilhoDesmarcado(TreeNode tndNo)
        {
            try
            {
                if (VerificarIrmaosChecados(tndNo, true) == 0)
                {
                    if (tndNo.Parent != null)
                        tndNo.Parent.Checked = false;
                    DesmarcarNoPaiSeFilhoDesmarcado(tndNo.Parent);
                }
            }
            catch
            {
                return;
            }
        }



        private void ChecarNosPais(TreeNode tndNo, bool bolMarcar)
        {
            try
            {
                if (tndNo.Parent != null)
                    tndNo.Parent.Checked = bolMarcar;
                TreeNode tndNovo = tndNo.Parent;
                if (tndNovo.Parent != null)
                    ChecarNosPais(tndNovo, bolMarcar);
            }
            catch
            {
                return;
            }
        }



        private void ChecarNosFilhos(TreeNode tndNo, bool bolChecked)
        {
            foreach (TreeNode tndNovoNo in tndNo.ChildNodes)
            {
                tndNovoNo.Checked = bolChecked;
                ChecarNosFilhos(tndNovoNo, bolChecked);
            }
        }


        /// <summary>
        /// Verifica se tem mais algum nó irmão checado
        /// </summary>
        /// <param name="tndNo"></param>
        /// <param name="bolChecked"></param>
        /// <returns></returns>
        private int VerificarIrmaosChecados(TreeNode tndNo, bool bolChecked)
        {
            try
            {
                TreeNode tndNoPai = tndNo.Parent;
                int intIrmaosChecados = 0;
                foreach (TreeNode tndNovoNo in tndNoPai.ChildNodes)
                {
                    if (tndNovoNo.Checked == bolChecked)
                        intIrmaosChecados += 1;
                }
                return intIrmaosChecados;
            }
            catch
            {
                return 0;
            }

        }
    }

    [NavUrl("~/Hades/PadacesTransacoes.aspx"),
    ControlText("Padrões de Acesso"),
    Title("Padrões de Acesso"),]

    public partial class PadacesTransacoes : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            treePaginas.Attributes.Add("onClick", "if(window.event.srcElement.tagName=='INPUT') {DisparaCliqueBotao();}");
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0 && Permission.AllowUpdate)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    lblInvisible.Text = decodedText;
                    lblPadaces.Text = ObterPadacesQueryString(decodedText);
                    lblTexto.Text = "Visualização de permissões do padrão de acesso:  ";

                    CarregarTransacoesVisualizacao(lblPadaces.Text);

                }
                else
                    Response.Redirect("PadroesdeAcesso.aspx");
            }
        }

        private void CarregarTransacoesVisualizacao(string padaces)
        {
            treePaginas.Nodes.Clear();
            treePaginas.Visible = false;
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
            btnAlterar.Visible = true;

            treeMenu.Nodes.Clear();
            treeMenu.ShowCheckBoxes = TreeNodeTypes.None;
            QueryTable qtMenu = RN.PadroesDeAcessos.ConsultarMenu(padaces);
            QueryTable qtItem = RN.PadroesDeAcessos.ConsultarItemMenu(padaces);

            for (int i = 0; i < qtMenu.Rows.Count; i++)
            {
                TreeNode masterNode = new TreeNode(qtMenu.Rows[i]["TEXTO"].ToString(), qtMenu.Rows[i]["TRANS"].ToString());
                masterNode.SelectAction = TreeNodeSelectAction.None;
                treeMenu.Nodes.Add(masterNode);
                SimpleRow[] linhas = qtItem.Select("intermediario = '" + RN.RNBase.MudarAspas(qtMenu.Rows[i]["MENUITEM"].ToString()) + "'", "MENUITEM");

                if (linhas.Length > 0)
                {
                    for (int j = 0; j < linhas.Length; j++)
                    {
                        TreeNode node = new TreeNode(linhas[j]["TEXTO"].ToString(), linhas[j]["TRANS"].ToString());
                        node.SelectAction = TreeNodeSelectAction.None;
                        masterNode.ChildNodes.Add(node);

                        if (RN.PadroesDeAcessos.VerificaPodeCad(padaces, linhas[j]["TRANS"].ToString(), linhas[j]["SIS"].ToString()))
                        {
                            TreeNode pode = new TreeNode("Cadastrar", "cadastrar");
                            pode.SelectAction = TreeNodeSelectAction.None;
                            node.ChildNodes.Add(pode);
                        }
                        if (RN.PadroesDeAcessos.VerificaPodeAlt(padaces, linhas[j]["TRANS"].ToString(), linhas[j]["SIS"].ToString()))
                        {
                            TreeNode pode = new TreeNode("Alterar", "Alterar");
                            pode.SelectAction = TreeNodeSelectAction.None;
                            node.ChildNodes.Add(pode);
                        }
                        if (RN.PadroesDeAcessos.VerificaPodeRem(padaces, linhas[j]["TRANS"].ToString(), linhas[j]["SIS"].ToString()))
                        {
                            TreeNode pode = new TreeNode("Remover", "Remover");
                            pode.SelectAction = TreeNodeSelectAction.None;
                            node.ChildNodes.Add(pode);
                        }
                        // verificaria se existem operações e se tem acesso a elas, inclui no nó
                        QueryTable qt_oper = RN.PadroesDeAcessos.ConsultarOperacoes(Convert.ToInt32(linhas[j]["MENUITEM"]));
                        if (qt_oper != null)
                        {
                            if (qt_oper.Rows.Count > 0)
                            {
                                for (int f = 0; f < qt_oper.Rows.Count; f++)
                                {
                                    string sis = qtItem.Rows[i]["SIS"].ToString();
                                    string trans_oper = qt_oper.Rows[f]["TRANS"].ToString();
                                    if (RN.PadroesDeAcessos.VerificaExiste(padaces, trans_oper, sis))
                                    {
                                        TreeNode node_oper = new TreeNode(qt_oper.Rows[f]["TEXTO"].ToString(), trans_oper);
                                        node_oper.SelectAction = TreeNodeSelectAction.None;
                                        node.ChildNodes.Add(node_oper);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            treeMenu.CollapseAll();
        }

        //carrega árvore de menus para edição
        private void CarregarMenu()
        {
            treeMenu.Nodes.Clear();
            treeMenu.ShowCheckBoxes = TreeNodeTypes.None;
            QueryTable qtMenu = RN.PadroesDeAcessos.ConsultarMenu();
            for (int i = 0; i < qtMenu.Rows.Count; i++)
            {
                TreeNode masterNode = new TreeNode(qtMenu.Rows[i]["TEXTO"].ToString(), qtMenu.Rows[i]["MENUITEM"].ToString());

                treeMenu.Nodes.Add(masterNode);
            }
            treeMenu.CollapseAll();
        }

        protected void treeMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (btnAlterar.Visible == false)
            {
                lblMensagem.Text = string.Empty;
                CarregaArvorePaginas(lblPadaces.Text, treeMenu.SelectedNode);
                CarregarMenu();
            }
        }

        private void CarregaArvorePaginas(string padaces, TreeNode no)
        {
            treePaginas.Visible = true;
            btnCancelar.Visible = true;
            btnSalvar.Visible = true;

            treePaginas.Nodes.Clear();
            treePaginas.ShowCheckBoxes = TreeNodeTypes.All;
            QueryTable qtItem = RN.PadroesDeAcessos.ConsultarItensMenu(no.Value.ToString());

            TreeNode masterNode = no;
            masterNode.SelectAction = TreeNodeSelectAction.None;
            treePaginas.Nodes.Add(masterNode); //add no principal menu
            string transacao = RN.PadroesDeAcessos.ConsultaTransacao(no.Value.ToString());
            string sistema = RN.PadroesDeAcessos.ConsultaSistema(no.Value.ToString());
            if (!string.IsNullOrEmpty(transacao))
            {
                if (RN.PadroesDeAcessos.VerificaExiste(padaces, transacao, sistema))
                {
                    masterNode.Checked = true;
                }
            }

            for (int i = 0; i < qtItem.Rows.Count; i++)
            {
                TreeNode noPagina = new TreeNode(qtItem.Rows[i]["TEXTO"].ToString(), qtItem.Rows[i]["MENUITEM"].ToString());
                noPagina.SelectAction = TreeNodeSelectAction.None;
                masterNode.ChildNodes.Add(noPagina);

                string trans = qtItem.Rows[i]["TRANS"].ToString();
                string sis = qtItem.Rows[i]["SIS"].ToString();
                if (RN.PadroesDeAcessos.VerificaExiste(padaces, trans, sis))
                {
                    noPagina.Checked = true;
                }

                TreeNode nodeadd = new TreeNode("Adicionar", "podecad");
                if (RN.PadroesDeAcessos.VerificaPodeCad(padaces, trans, sis))
                {
                    nodeadd.Checked = true;
                }
                nodeadd.SelectAction = TreeNodeSelectAction.None;
                noPagina.ChildNodes.Add(nodeadd);

                TreeNode nodeup = new TreeNode("Alterar", "podealt");
                if (RN.PadroesDeAcessos.VerificaPodeAlt(padaces, trans, sis))
                {
                    nodeup.Checked = true;
                }
                nodeup.SelectAction = TreeNodeSelectAction.None;
                noPagina.ChildNodes.Add(nodeup);

                TreeNode nodedel = new TreeNode("Remover", "poderem");
                if (RN.PadroesDeAcessos.VerificaPodeRem(padaces, trans, sis))
                {
                    nodedel.Checked = true;
                }
                nodedel.SelectAction = TreeNodeSelectAction.None;
                noPagina.ChildNodes.Add(nodedel);

                #region Adiciona Operação
                //insere outro nivel para operações diferentes de alterar/cadastrar/remover
                //fazer um laço dessas operações encontradas, adicionando ao nó
                QueryTable qt = RN.PadroesDeAcessos.ConsultarOperacoes(Convert.ToInt32(qtItem.Rows[i]["MENUITEM"]));
                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                    {
                        for (int f = 0; f < qt.Rows.Count; f++)
                        {
                            TreeNode node_oper = new TreeNode(qt.Rows[f]["TEXTO"].ToString(), qt.Rows[f]["TRANS"].ToString());
                            string trans_oper = qt.Rows[f]["TRANS"].ToString();
                            if (RN.PadroesDeAcessos.VerificaExiste(padaces, trans_oper, sis))
                            {
                                node_oper.Checked = true;
                            }
                            node_oper.SelectAction = TreeNodeSelectAction.None;
                            noPagina.ChildNodes.Add(node_oper);
                        }
                    }
                }
                #endregion

            }
            masterNode.Expand();
        }


        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            string padaces = lblInvisible.Text;
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(padaces);
            Response.Redirect("PadroesdeAcesso.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        private string ObterPadacesQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string padaces = string.Empty;
            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("value") >= 0)
                    padaces = dados.Substring(dados.LastIndexOf('=') + 1);
            }
            return padaces;
        }

        protected void treePaginas_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            e.Node.Expand();
            clsTreeView clsTree = new clsTreeView();
            //clsTree.TratarEventoCheckedDaTreeView(e.Node, e.Node.Checked);
        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            lblTexto.Text = "Selecione o menu que deseja alterar para o padrão de acesso:  ";
            lblMensagem.Text = string.Empty;
            CarregarMenu();
            btnAlterar.Visible = false;
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (SalvarPadroes())
            {
                btnSalvar.Visible = false;
                btnCancelar.Visible = false;
                treePaginas.Nodes.Clear();
                treePaginas.Visible = false;
                //CarregarTransacoesVisualizacao(lblPadaces.Text);
            }
        }


        protected bool SalvarPadroes()
        {
            bool ok = true;
            string padaces = lblPadaces.Text;
            lblMensagem.Text = RN.PadroesDeAcessos.SalvaPadroes(treePaginas, padaces, ref ok);
            if (ok)
                return true;
            return false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
            treePaginas.Nodes.Clear();
            treePaginas.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        protected void dummy_Click(object sender, EventArgs e)
        {

        }

    }//fim classe
}//fim namespace
