using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using Techne.Data;

namespace Techne.Controls
{
    internal class ContainerManagerLib
    {
        /// <summary>
        ///   Indica se todos os IRecordContainer's contidos dentro do manager informado estão no modo informado.
        ///   Se não houverem IRecordContainer's no manager informado, devolve true se o parâmetro mode for 'View'.
        /// </summary>
        public static bool AllContainersInMode(IContainerManager manager, RecordManagerMode mode)
        {
            var containers = TControl.GetChildRecordContainers((Control)manager, true);

            foreach (var container in containers)
            {
                if (container.Mode != mode)
                {
                    return false;
                }
            }

            return containers.Length == 0 ? mode == RecordManagerMode.View : true;
        }

        public static bool CanCommit(IContainerManagerBase manager)
        {
            if (manager is IRecordContainer)
            {
                return RecordContainerLib.CanCommit((IRecordContainer)manager);
            }
            else
            {
                foreach (var childContainer in TControl.GetChildRecordContainers((Control)manager, false))
                {
                    if (RecordContainerLib.CanCommit(childContainer))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static bool CanDelete(IContainerManager manager)
        {
            if (manager is IRecordContainer)
            {
                return RecordContainerLib.CanDelete((IRecordContainer)manager);
            }
            else
            {
                foreach (var childContainer in TControl.GetChildRecordContainers((Control)manager, false))
                {
                    if (RecordContainerLib.CanDelete(childContainer))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static bool CanEnterAddNew(IContainerManager manager)
        {
            if (manager is IRecordContainer)
            {
                return RecordContainerLib.CanEnterAddNew((IRecordContainer)manager);
            }
            else
            {
                var containers = TControl.GetChildRecordContainers((Control)manager, true);

                foreach (var container in containers)
                {
                    if (!RecordContainerLib.CanEnterAddNew(container))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        ///   Um manager pode entrar no modo Edit se pelo menos um de seus containers puder entrar em modo Edit.
        /// </summary>
        public static bool CanEnterEdit(IContainerManagerBase manager)
        {
            if (manager is IContainerManager)
            {
                if (manager is IRecordContainer)
                {
                    return RecordContainerLib.CanEnterEdit((IRecordContainer)manager);
                }
                else
                {
                    foreach (var childContainer in TControl.GetChildRecordContainers((Control)manager, false))
                    {
                        if (RecordContainerLib.CanEnterEdit(childContainer))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool CanUndelete(IContainerManager manager)
        {
            if (manager is IRecordContainer)
            {
                return RecordContainerLib.CanUndelete((IRecordContainer)manager);
            }
            else
            {
                foreach (var childContainer in TControl.GetChildRecordContainers((Control)manager, false))
                {
                    if (RecordContainerLib.CanUndelete(childContainer))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static bool Commit(IContainerManagerInternal manager)
        {
            try
            {
                var permission = TControl.GetPermission(manager);
                if (permission.ReadOnly)
                {
                    manager.AddMessage(TPermission.DenialMessage, true);
                    return false;
                }

                PreCommitRecursive(manager);

                var commitInfo = CommitPrepare(manager, null);

                // Verifica se existe algum container que não comitou
                var commit = true;
                foreach (var triplet in commitInfo)
                {
                    if (!(bool)triplet.Third)
                    {
                        commit = false;
                        break;
                    }
                }

                if (commit)
                {
                    CommitCommit(manager, commitInfo);

                    // Deve ser feito antes de SetModeToView() para limpar o DataTable porque este disparará
                    // indiretamente o PostContainerOperation, e, o tratamento deste poderá estar chamando o
                    // TDataGrid.Get(), por exemplo, que exige DataTable vazio.
                    PosCommitRecursive(manager, true, true);

                    SetModeToView(manager, ContainerManagerAction.Commit, true);
                }
                else
                {
                    PosCommitRecursive(manager, false, true);
                }

                return commit;
            }
            finally
            {
            }
        }

        public static TConnection CreateConnection(IContainerManager manager)
        {
            var cn = manager.Table == null ? null : manager.Table.CreateConnection();

            foreach (var child in TControl.GetChildManagers((Control)manager))
            {
                if (cn == null)
                {
                    cn = child.CreateConnection();
                }
                else if (!ConnectionEquals(cn, child.CreateConnection()))
                {
                    throw new InvalidOperationException("As conexões dos managers filhos do record manager '" + ((Control)manager).ID + "' não podem participar da mesma transação.");
                }
            }

            return cn;
        }

        public static TConnectionWritable CreateWritableConnection(IContainerManager manager)
        {
            var permission = TControl.GetPermission(manager);
            var cn = manager.Table == null ? null : manager.Table.CreateWritableConnection(permission);

            foreach (IContainerManagerInternal child in TControl.GetChildManagers((Control)manager))
            {
                if (cn == null)
                {
                    cn = child.CreateWritableConnection();
                }
                else if (!ConnectionEquals(cn, child.CreateWritableConnection()))
                {
                    throw new InvalidOperationException("As conexões dos managers filhos do record manager '" + ((Control)manager).ID + "' não podem participar da mesma transação.");
                }
            }

            return cn;
        }

        /// <summary>
        ///   Remove os registros dos containers do manager informado.
        ///   O método se encarregará de trazer os registros afetados do banco. Não é necessário que a tabela manager.Table contenha os registros.
        ///   Estes registros serão removidos da tabela no final da operação.
        /// </summary>
        public static bool Delete(IContainerManagerInternal manager)
        {
            var permission = TControl.GetPermission(manager);
            if (permission == null || permission.ReadOnly)
            {
                manager.AddMessage(TPermission.DenialMessage, true);
                return false;
            }

            PreDeleteRecursive(manager);

            var deletedInfo = DeletePrepare(manager, manager.CreateWritableConnection());

            // Verifica se existe algum container que não deletou
            var commit = true;
            foreach (var pair in deletedInfo)
            {
                if (!(bool)pair.Second)
                {
                    commit = false;
                    break;
                }
            }

            if (commit)
            {
                DeleteCommit(deletedInfo);

                // Precisa chamar SetModeToView() porque pode, por exemplo, existir um RecordManager
                // sem TControl's controlando outros managers internos.
                // firePostContainerOperation é false porque o evento já foi disparado em DeleteCommit()
                SetModeToView(manager, ContainerManagerAction.Delete, false);
            }
            else
                
// Deve desfazer a operação em todos os registros. Na verdade somente os registros
                // que foram removidos com sucesso serão recuperados da base de dados. Os que não
                // puderam ser removidos contém mensagens de erro.
                // TODO O problema aqui é que os registros recuperados serão postos no final da tabela
                if (manager.Table != null)
                {
                    DeleteRollback(deletedInfo, manager.Table);
                }

            PosDeleteRecursive(manager, commit);

            return commit;
        }

        public static void EnterAddNewMode(IContainerManagerInternal manager)
        {
            

            if (TControl.GetRootManager((Control)manager) == manager && !manager.IsRoot)
            {
                // Operações só devem ser realizadas no root
                throw new InvalidOperationException("Operações só podem ser executadas no RootManager (" + ((Control)TControl.GetRootManager((Control)manager)).UniqueID + ")");
            }

            // Não pode verificar CanEnterAddNew() aqui porque o IContainerManager.PreEnterAddNewMode() precisa ser chamado antes

            
            PreEnterAddNewModeRecursive(manager);
            EnterAddNewModeRecursive(manager);
            PosEnterAddNewModeRecursive(manager);
        }

        /// <summary>
        ///   Chama SetMode(Edit) para cada um dos containers internos ao manager informado
        ///   (inclui o próprio manager se ele implementar IRecordContainer também).
        ///   Se o container não permitir Edit mas permitir AddNew, entra em AddNew.
        ///   Se houverem managers internos, chama recursivamente EnterEditMode() para
        ///   cada um deles.
        /// </summary>
        public static void EnterEditMode(IContainerManagerInternal manager)
        {
            

            if (TControl.GetRootManager((Control)manager) == manager && !manager.IsRoot)
            {
                // Operações só devem ser realizadas no root
                throw new InvalidOperationException("Operações só podem ser executadas no RootManager (" + ((Control)TControl.GetRootManager((Control)manager)).UniqueID + ")");
            }

            // Não pode verificar CanEnterEdit() aqui porque o IContainerManager.PreEnterEditMode() precisa ser chamado antes

            

            // Precisa chamar o PreEnterEditMode() antes de CanEnterEdit
            // para que os containers sejam criados (no caso da grid).
            PreEnterEditModeRecursive(manager);

            EnterEditModeRecursive(manager);
            PosEnterEditModeRecursive(manager);
        }

        /// <summary>
        ///   Obtém o estado inicial do manager de acordo com os parâmetros 'manager' e 'mode' da query string.
        /// </summary>
        public static RecordManagerMode GetInitialMode(IContainerManager manager)
        {
            var page = ((Control)manager).Page;
            var uniqueId = ((Control)manager).UniqueID;

            var initMode = page.Request["mode"];

            if (!page.IsPostBack && initMode != null && initMode != string.Empty)
            {
                RecordManagerMode initManagerMode;

                switch (initMode.ToLower())
                {
                    case "edit":
                        initManagerMode = RecordManagerMode.Edit;
                        break;
                    case "new":
                        initManagerMode = RecordManagerMode.New;
                        break;
                    case "view":
                        initManagerMode = RecordManagerMode.View;
                        break;
                    default:
                        throw new InvalidOperationException("Valor inválido para o parâmetro 'mode' na query string.");
                }

                var managerName = page.Request["manager"];
                if (managerName == null)
                {
                    managerName = string.Empty;
                }

                if (managerName == string.Empty && initManagerMode != RecordManagerMode.View)
                {
                    if (TechLib.FindControls(typeof (IContainerManager), page).Length > 1)
                    {
                        throw new InvalidOperationException("Existe mais de um manager. Especifique qual deles entrará no modo inicial especificado.");
                    }
                }

                // Verifica se o parâmetro "manager" da query string refere-se ao manager em questão.
                // Se "manager" não foi informado ou for vazio, também assume o modo.
                if (managerName == string.Empty ||
                    string.Compare(managerName, uniqueId, true) == 0)
                {
                    return initManagerMode;
                }
            }

            return RecordManagerMode.View;
        }

        public static IContainerManager GetManager(IRecordContainer[] containers)
        {
            if (containers == null)
            {
                throw new ArgumentNullException();
            }

            IContainerManager manager = null;

            foreach (var container in containers)
            {
                if (manager == null)
                {
                    manager = TControl.GetManager((Control)container);
                    if (manager == null)
                    {
                        throw new ArgumentException("Existe pelo menos um IRecordContainer informado que não pertence a nenhum IContainerManager.");
                    }
                }
                else if (TControl.GetManager((Control)container) != manager)
                {
                    throw new ArgumentException("Todos os IRecordContainer's devem pertencer ao mesmo IContainerManager.");
                }
            }

            return manager;
        }

        public static string GetTitle(IContainerManager manager)
        {
            var title = manager.Title;

            if (title == "?")
            {
                if (manager.Table == null)
                {
                    title = string.Empty;
                }
                else
                {
                    title = string.Empty; // Table.Caption[Thread.CurrentThread.CurrentCulture.Name]; // 1) Caption

                    if (title.Length == 0)
                    {
                        title = manager.Table.Name; // 2) Names
                        if (title.Length == 0)
                        {
                            title = manager.Table.TableName; // 3) TableName
                        }

                        title = StrLib.ToProper(title);
                    }
                }
            }

            return title;
        }

        public static bool IsEmpty(IContainerManager manager)
        {
            var childManagers = TControl.GetChildManagers((Control)manager);

            foreach (var child in childManagers)
            {
                if (!child.IsEmpty)
                {
                    return false;
                }
            }

            foreach (var child in TControl.GetChildRecordContainers((Control)manager, true))
            {
                if (child.PrimaryKeyValues != null)
                {
                    return false;
                }
            }

            return childManagers.Length == 0;
        }

        /// <summary>
        ///   Chama Refresh() de todos os IContainerManager's que baseiam-se
        ///   em data tables cuja tabela principal esteja na lista informada.
        /// </summary>
        public static void Refresh(BusinessMethod businessMethod, Page page)
        {
            // Faz o refresh em todos os managers que referenciam tabelas alteradas pelo BusinessMethod.
            var changedTables = businessMethod.ChangedTables;
            foreach (IContainerManager mgr in TechLib.FindControls(typeof (IContainerManager), page))
            {
                if (changedTables.Contains(mgr.Table.MainTableName))
                {
                    mgr.Refresh();
                }
            }
        }

        /// <summary>
        ///   Recoloca os valores originais nos controles baseado nas primary keys guardadas em cada um dos containers.
        ///   A tabela manager.Table NÃO precisa conter os DataRow's com os valores originais.
        ///   A operação é executada recursivamente nos managers filhos.
        /// </summary>
        public static void Rollback(IContainerManagerInternal manager)
        {
            try
            {
                PreRollbackRecursive(manager);
                RollbackRecursive(manager);
                PosRollbackRecursive(manager, true);
            }
            finally
            {
            }
        }

        /// <summary>
        ///   Indica se algum dos IRecordContainer's contidos dentro do manager informado está no modo informado.
        /// </summary>
        public static bool SomeContainerInMode(IContainerManager manager, RecordManagerMode mode)
        {
            foreach (var container in TControl.GetChildRecordContainers((Control)manager, true))
            {
                if (container.Mode == mode)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Undelete(IContainerManagerInternal manager)
        {
            PreUndeleteRecursive(manager);

            var undeletedInfo = UndeletePrepare(manager, manager.CreateWritableConnection());

            // Verifica se existe algum container que não deletou
            var commit = true;
            foreach (var pair in undeletedInfo)
            {
                if (!(bool)pair.Second)
                {
                    commit = false;
                    break;
                }
            }

            if (commit)
            {
                UndeleteCommit(undeletedInfo);
            }
            else
                
// Deve desfazer a operação em todos os registros. Na verdade somente os registros
                // que foram reinseridos com sucesso serão recuperados da base de dados. Os que não
                // puderam ser reinseridos contém mensagens de erro.
                if (manager.Table != null)
                {
                    UndeleteRollback(undeletedInfo, manager.Table);
                }

            PosUndeleteRecursive(manager, commit);

            return commit;
        }

        public static void WriteDebugInfo(HtmlTextWriter writer, IContainerManager manager)
        {
            writer.WriteLine(
                "<B>IsRoot: </B>" + manager.IsRoot +
                (manager.IsRoot ? string.Empty : ", <B>RootManager: </B>" + ((Control)TControl.GetRootManager((Control)manager)).UniqueID) +
                "<BR/>"
                );
            if (manager.Table == null && TControl.GetChildManagers((Control)manager).Length == 0)
            {
                writer.WriteLine("<FONT COLOR=pink><B>WARNING</B></FONT>: A propriedade DataSource e/ou DataMember não foi informada.<BR/>");
            }
        }

        private static void CommitCommit(IContainerManagerInternal manager, 
                                         Triplet[] commitInfo)
        {
            foreach (var triplet in commitInfo)
            {
                var container = (IRecordContainerInternal)triplet.First;

                if (container.Mode == RecordManagerMode.New && manager.DataEntry)
                {
                    RecordContainerLib.ResetTControls(container, true);
                }

                if (container.Changed || manager.ProcessUnchangedContainers)
                {
                    container.SetChanged(false);
                    container.SetPrimaryKeyValues((DbObject[])triplet.Second);
                }
            }

            SetIsRootRecursive(manager, true);
        }

        /// <summary>
        ///   Efetiva a operação de insert ou update no banco de dados, mostrando possíveis mensagens de erro
        ///   genéricas (as específicas de cada coluna já estão nos controles correspondentes).
        ///   Devolve array de Triplet's, onde First é o IRecordContainer, Second é o PrimaryKeyValues e
        ///   Third é uma booleana que indica se a operação foi bem sucedida no manager correspondente.
        ///   Este método obtém o registro do banco se for necessário.
        ///   A manager.Table não precisa ter os registros pré-carregados.
        /// </summary>
        private static Triplet[] CommitPrepare(IContainerManager manager, 
                                               TConnectionWritable connection)
        {
            

            if (TControl.GetRootManager((Control)manager) == manager && !manager.IsRoot)
            {
                // Operações só devem ser realizadas no root
                throw new InvalidOperationException("Operações só podem ser executadas no RootManager (" + ((Control)TControl.GetRootManager((Control)manager)).UniqueID + ")");
            }

            if (!CanCommit(manager))
            {
                throw new InvalidOperationException("Operação inválida na situação atual do manager " + ((Control)manager).UniqueID + ".");
            }

            

            var commitedTriplets = new ArrayList();
            var cn = connection;
            if (connection == null)
            {
                cn = ((IContainerManagerInternal)manager).CreateWritableConnection();
                if (cn != null)
                {
                    if (manager.Table == null)
                    {
                        cn.Transacao = manager.Transacao.Length == 0
                                           ? "Manager: " + ((Control)manager).UniqueID
                                           : manager.Transacao;
                    }
                    else
                    {
                        var nome = manager.Table.Name;
                        cn.Transacao = nome.Length == 0
                                           ? "Operação no DataTable " + manager.Table.TableName
                                           : "Operação em " + nome;
                    }
                }
            }

            if (cn == null)
            {
                throw new InvalidOperationException("Não foi possível obter conexão ao banco de dados.");
            }

            var containers = TControl.GetChildRecordContainers((Control)manager, true);

            cn.Open(true);

            try
            {
                // Comita cada um dos managers internos
                foreach (var child in TControl.GetChildManagers((Control)manager))
                {
                    commitedTriplets.AddRange(CommitPrepare(child, cn));
                }

                if (manager.Table != null)
                {
                    // Comita o registro de cada um dos containers internos
                    foreach (var container in containers)
                    {
                        var row = RecordContainerLib.GetRecordFromDb(container, cn, manager.Table, 
                                                                     container.Mode == RecordManagerMode.Edit);

                        var commit = true;
                        var newPk = new DbObject[0];
                        var processUnchangedContainers = ((IContainerManagerInternal)manager).ProcessUnchangedContainers;

                        if (row == null && container.PrimaryKeyValues != null)
                        {
                            ((IRecordContainerInternal)container).AddMessage("O registro não foi encontrado ou já foi removido", true);
                            newPk = container.PrimaryKeyValues;
                            commit = false;
                        }
                        else if (processUnchangedContainers || container.Changed ||
                                 (container is TGridItem && ((TGridItem)container).Marked))
                        {
                            // Quando estiver comitando no modo AddNew, certifica-se que os valores foram realmente
                            // alterados. Isso é necessário porque o usuário pode alterar algum controle e, depois de
                            // um postback, desistir da alteração digitando o valor inicial.
                            var changed = false;
                            if (container.Mode == RecordManagerMode.New)
                            {
                                // Vai comparar o valor de cada controle do container com os valores iniciais de um novo registro.
                                var dummyRow = manager.Table.NewRow();
                                foreach (var control in TControl.GetChildTControls((Control)container))
                                {
                                    if (control.ColumnName.Length > 0)
                                    {
                                        if (!control.DBValue.Equals(dummyRow[control.ColumnName]))
                                        {
                                            changed = true;
                                            break;
                                        }
                                    }
                                }

                                if (!changed)
                                {
                                    ((IRecordContainerInternal)container).SetChanged(false);
                                }
                            }

                            if (container.Mode == RecordManagerMode.Edit || changed || processUnchangedContainers)
                            {
                                commit = RecordEditorLib.Commit((IRecordContainerInternal)container, cn, manager.Table, out newPk);
                            }
                        }
                        else if (container.Mode == RecordManagerMode.Edit || container.Mode == RecordManagerMode.New)
                        {
                            newPk = container.PrimaryKeyValues;
                        }

                        commitedTriplets.Add(new Triplet(container, newPk, commit));
                    }
                }
            }
            finally
            {
                // Se existir algum manager que falhou, o próprio já chamou cn.Rollback()
                cn.Close();
            }

            return (Triplet[])commitedTriplets.ToArray(typeof (Triplet));
        }

        /// <summary>
        ///   Compara cada parâmetro da connect string de cada uma das connections informadas.
        /// </summary>
        private static bool ConnectionEquals(TConnection connection1, TConnection connection2)
        {
            var info1 = StrLib.StrToDictionary(connection1.ConnectionString, ';', "=", true);
            var info2 = StrLib.StrToDictionary(connection2.ConnectionString, ';', "=", true);

            if (info1.Count != info2.Count)
            {
                return false;
            }

            foreach (DictionaryEntry item1 in info1)
            {
                // Verifica se o elemento existe na coleção 2
                if (!info2.Contains(item1.Key))
                {
                    return false;
                }

                // Verifica se os valores são iguais
                if (((string)item1.Value).ToLower() != ((string)info2[item1.Key]).ToLower())
                {
                    return false;
                }
            }

            return true;
        }

        private static void DeleteCommit(Pair[] deletedInfo)
        {
            foreach (var pair in deletedInfo)
            {
                var container = (IRecordContainerInternal)pair.First;
                var manager = container is IContainerManager ? (IContainerManager)container
                                  : TControl.GetManager((Control)container);

                if (!manager.Table.HistoryEnabled || !manager.ShowDeletedRecords)
                {
                    // A propriedade PrimaryKeyValues deve ser resetada antes de chamar ResetTControls() porque
                    // este último eventualmente chamará o método TDropDownList.DataLoad(), que utiliza a
                    // propriedade do container.
                    container.SetPrimaryKeyValues(null);

                    

                    // Para poder resetar os controles (setar o DBValue de cada um), eles não devem estar em modo View.
                    container.SetMode(RecordManagerMode.Edit);

                    RecordContainerLib.ResetTControls(container, false);

                    
                }

                // O SetMode() disparará o evento PostContainerOperation
                container.SetMode(RecordManagerMode.View, ContainerManagerAction.Delete);
            }
        }

        /// <summary>
        ///   Devolve array de Pair's, onde First é o IRecordContainer e Second é uma booleana
        ///   que indica se a operação foi bem sucedida no manager correspondente.
        ///   Este método obtém o registro do banco se for necessário. A manager.Table não precisa ter os registros pré-carregados.
        /// </summary>
        private static Pair[] DeletePrepare(IContainerManagerInternal manager, 
                                            TConnectionWritable connection)
        {
            if (!CanDelete(manager))
            {
                throw new InvalidOperationException("Operação inválida na situação atual do manager " + ((Control)manager).UniqueID + ".");
            }

            var deletedPairs = new ArrayList();
            var page = (TPage)((Control)manager).Page;

            connection.Open(true);

            try
            {
                foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
                {
                    deletedPairs.AddRange(DeletePrepare(childManager, connection));
                }

                if (manager.Table != null)
                {
                    // Remove o registro de cada um dos containers internos
                    foreach (IRecordContainerInternal container in manager.DeleteContainers)
                    {
                        var row = RecordContainerLib.GetRecordFromDb(
                            container, connection, manager.Table, 
// O parâmetro getAllColumns é true para que o entry-point PreDelete da tabela traga um
                            // TDataRow com todas as colunas preenchidas, não somente as colunas que pertençam à grid.
                            true
                            );

                        if (row == null && container.PrimaryKeyValues != null)
                        {
                            container.AddMessage("O registro não foi encontrado ou já foi removido", true);
                            deletedPairs.Add(new Pair(container, false));
                        }
                        else
                        {
                            deletedPairs.Add(new Pair(container, RecordEditorLib.Delete(container, connection, manager.Table)));
                        }
                    }
                }
            }
            finally
            {
                // Se existir algum manager que falhou, o próprio já chamou connection.Rollback()
                connection.Close();
            }

            return (Pair[])deletedPairs.ToArray(typeof (Pair));
        }

        /// <summary>
        ///   Tentará recuperar os dados originais dos containers cujos registros foram removidos com sucesso.
        ///   Aqueles que não foram removidos conterão mensagens de erro.
        /// </summary>
        private static void DeleteRollback(Pair[] deletedInfo, 
                                           TDataTable dataTable)
        {
            foreach (var pair in deletedInfo)
            {
                var container = (IRecordContainer)pair.First;

                if ((bool)pair.Second)
                {
                    RecordContainerLib.GetRecordFromDb(container, null, dataTable, false);
                }
            }
        }

        private static void EnterAddNewModeRecursive(IContainerManagerInternal manager)
        {
            if (manager is IRecordContainer)
            {
                RecordContainerLib.EnterAddNewMode((IRecordContainerInternal)manager);
            }
            else
            {
                foreach (IRecordContainerInternal childContainer in TControl.GetChildRecordContainers((Control)manager, true))
                {
                    if (RecordContainerLib.CanEnterAddNew(childContainer))
                    {
                        RecordContainerLib.EnterAddNewMode(childContainer);
                    }
                }
            }
        }

        private static void EnterEditModeRecursive(IContainerManagerInternal manager)
        {
            bool willEdit;

            

            if (CanEnterEdit(manager))
            {
                willEdit = true;
            }

                // A exigência do modo View em todos os containers é porque o manager.PreEnterAddNewMode(),
                // no caso do TDataGrid, preencherá a grid com linhas vazias. Assim, se a grid já estivesse
                // em modo Edit, ela entraria indevidamente em modo AddNew.
            else if (AllContainersInMode(manager, RecordManagerMode.View))
            {
                // No caso da grid, os containers serão criados neste momento.
                // Por isso se houvesse a verificação manager.CanEnterAddNew antes disso, ela falharia.
                manager.PreEnterAddNewMode();
                willEdit = false;

                if (!CanEnterAddNew(manager))
                {
                    throw new InvalidOperationException("O manager " + ((Control)manager).UniqueID + " não permite modo Edit ou AddNew");
                }
            }
            else
            {
                throw new InvalidOperationException("O manager " + ((Control)manager).UniqueID + " não permite modo Edit porque pelo menos um container não está em modo View");
            }

            

            if (manager is IRecordContainer)
            {
                if (willEdit)
                {
                    RecordContainerLib.EnterEditMode((IRecordContainerInternal)manager);
                }
                else
                {
                    RecordContainerLib.EnterAddNewMode((IRecordContainerInternal)manager);
                }
            }
            else
            {
                foreach (IRecordContainerInternal childContainer in TControl.GetChildRecordContainers((Control)manager, false))
                {
                    if (willEdit)
                    {
                        if (RecordContainerLib.CanEnterEdit(childContainer))
                        {
                            RecordContainerLib.EnterEditMode(childContainer);
                        }
                    }
                    else if (RecordContainerLib.CanEnterAddNew(childContainer))
                    {
                        RecordContainerLib.EnterAddNewMode(childContainer);
                    }
                }
            }
        }

        private static void PosCommitRecursive(IContainerManagerInternal manager, bool commited, bool isRoot)
        {
            manager.PosCommit(commited, isRoot);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosCommitRecursive(childManager, commited, false);
            }
        }

        private static void PosDeleteRecursive(IContainerManagerInternal manager, bool deleted)
        {
            manager.PosDelete(deleted);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosDeleteRecursive(childManager, deleted);
            }
        }

        private static void PosEnterAddNewModeRecursive(IContainerManagerInternal manager)
        {
            manager.PosEnterAddNewMode();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosEnterAddNewModeRecursive(childManager);
            }
        }

        private static void PosEnterEditModeRecursive(IContainerManagerInternal manager)
        {
            manager.PosEnterEditMode();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosEnterEditModeRecursive(childManager);
            }
        }

        private static void PosRollbackRecursive(IContainerManagerInternal manager, bool isRoot)
        {
            manager.PosRollback(isRoot);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosRollbackRecursive(childManager, false);
            }
        }

        private static void PosUndeleteRecursive(IContainerManagerInternal manager, bool undeleted)
        {
            manager.PosUndelete(undeleted);

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PosUndeleteRecursive(childManager, undeleted);
            }
        }

        private static void PreCommitRecursive(IContainerManagerInternal manager)
        {
            manager.PreCommit();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreCommitRecursive(childManager);
            }
        }

        private static void PreDeleteRecursive(IContainerManagerInternal manager)
        {
            manager.PreDelete();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreDeleteRecursive(childManager);
            }
        }

        private static void PreEnterAddNewModeRecursive(IContainerManagerInternal manager)
        {
            manager.PreEnterAddNewMode();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreEnterAddNewModeRecursive(childManager);
            }
        }

        private static void PreEnterEditModeRecursive(IContainerManagerInternal manager)
        {
            manager.PreEnterEditMode();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreEnterEditModeRecursive(childManager);
            }
        }

        private static void PreRollbackRecursive(IContainerManagerInternal manager)
        {
            manager.PreRollback();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreRollbackRecursive(childManager);
            }
        }

        private static void PreUndeleteRecursive(IContainerManagerInternal manager)
        {
            manager.PreUndelete();

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                PreUndeleteRecursive(childManager);
            }
        }

        private static void RollbackRecursive(IContainerManagerInternal manager)
        {
            

            if (TControl.GetRootManager((Control)manager) == manager && !manager.IsRoot)
            {
                // Operações só devem ser realizadas no root
                throw new InvalidOperationException("Operações só podem ser executadas no RootManager (" + ((Control)TControl.GetRootManager((Control)manager)).UniqueID + ")");
            }

            if (!CanCommit(manager))
            {
                throw new InvalidOperationException("Operação inválida na situação atual do manager " + ((Control)manager).UniqueID + ".");
            }

            

            foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
            {
                RollbackRecursive(childManager);
            }

            foreach (IRecordContainerInternal container in TControl.GetChildRecordContainers((Control)manager, true))
            {
                if (container.Mode == RecordManagerMode.Edit || container.Mode == RecordManagerMode.New)
                {
                    var row = RecordContainerLib.GetRecordFromDb(container, null, manager.Table, false);
                    RecordEditorLib.Rollback(container, manager.Table);
                    if (row != null)
                    {
                        manager.Table.Rows.Remove(row);
                    }

                    container.SetMode(RecordManagerMode.View, ContainerManagerAction.Rollback);
                }
            }

            manager.IsRoot = true;
        }

        private static void SetIsRootRecursive(IContainerManager manager, bool value)
        {
            manager.IsRoot = value;
            foreach (var childManager in TControl.GetChildManagers((Control)manager))
            {
                SetIsRootRecursive(childManager, value);
            }
        }

        /// <summary>
        ///   Seta o modo de cada um dos containers internos para View. Opera recursivamente nos managers filhos.
        ///   Opcionalmente dispara o evento PostContainerOperation para cada container afetado.
        /// </summary>
        /// <param name = "firePostContainerOperation">
        ///   Dispara OnPostContainerOperation().
        /// </param>
        private static void SetModeToView(IContainerManager manager, 
                                          ContainerManagerAction action, 
                                          bool firePostContainerOperation)
        {
            foreach (var childManager in TControl.GetChildManagers((Control)manager))
            {
                SetModeToView(childManager, action, firePostContainerOperation);
            }

            foreach (IRecordContainerInternal container in TControl.GetChildRecordContainers((Control)manager, true))
            {
                var mode = container.Mode;
                var dataEntry = manager.DataEntry;

                if (mode == RecordManagerMode.Edit ||
                    mode == RecordManagerMode.New && !dataEntry)
                {
                    if (firePostContainerOperation)
                    {
                        container.SetMode(RecordManagerMode.View, action);
                    }
                    else
                    {
                        container.SetMode(RecordManagerMode.View);
                    }
                }
                else if (mode == RecordManagerMode.New && dataEntry && firePostContainerOperation)
                {
                    container.OnPostContainerOperation(new PostContainerOperationEventArgs(container, RecordManagerMode.New, ContainerManagerAction.Commit));
                }
            }
        }

        private static void UndeleteCommit(Pair[] undeletedInfo)
        {
            foreach (var pair in undeletedInfo)
            {
                var container = (IRecordContainerInternal)pair.First;
                var manager = container is IContainerManager ? (IContainerManager)container
                                  : TControl.GetManager((Control)container);

                // Remove da tabela o registro que o UndeletePrepare obteve
                var row = manager.Table.Rows.Find(container.PrimaryKeyValues);
                manager.Table.Rows.Remove(row);

                // O SetMode() disparará o evento PostContainerOperation
                container.SetMode(RecordManagerMode.View, ContainerManagerAction.Undelete);
            }
        }

        /// <summary>
        ///   Devolve array de Pair's, onde First é o IRecordContainer e Second é uma booleana
        ///   que indica se a operação foi bem sucedida no manager correspondente.
        ///   Este método obtém o registro do banco se for necessário. A manager.Table não precisa ter os registros pré-carregados.
        /// </summary>
        private static Pair[] UndeletePrepare(IContainerManagerInternal manager, 
                                              TConnectionWritable connection)
        {
            if (!CanUndelete(manager))
            {
                throw new InvalidOperationException("Operação inválida na situação atual do manager " + ((Control)manager).UniqueID + ".");
            }

            var undeletedPairs = new ArrayList();
            var permission = TControl.GetPermission(manager);

            connection.Open(true);

            try
            {
                foreach (IContainerManagerInternal childManager in TControl.GetChildManagers((Control)manager))
                {
                    undeletedPairs.AddRange(UndeletePrepare(childManager, connection));
                }

                if (manager.Table != null)
                {
                    // Reinsere o registro de cada um dos containers internos
                    foreach (IRecordContainerInternal container in manager.DeleteContainers)
                    {
                        var row = RecordContainerLib.GetRecordFromDb(container, connection, manager.Table, false);
                        if (row == null && container.PrimaryKeyValues != null)
                        {
                            container.AddMessage("O registro não foi encontrado ou já foi removido", true);
                            undeletedPairs.Add(new Pair(container, false));
                        }
                        else
                        {
                            undeletedPairs.Add(new Pair(container, RecordEditorLib.Undelete(container, connection, manager.Table)));
                        }
                    }
                }
            }
            finally
            {
                // Se existir algum manager que falhou, o próprio já chamou connection.Rollback()
                connection.Close();
            }

            return (Pair[])undeletedPairs.ToArray(typeof (Pair));
        }

        /// <summary>
        ///   Tentará recuperar os dados originais dos containers cujos registros foram reinseridos com sucesso.
        ///   Aqueles que não foram reinseridos conterão mensagens de erro.
        /// </summary>
        private static void UndeleteRollback(Pair[] undeletedInfo, 
                                             TDataTable dataTable)
        {
            foreach (var pair in undeletedInfo)
            {
                var container = (IRecordContainer)pair.First;

                if ((bool)pair.Second)
                {
                    RecordContainerLib.GetRecordFromDb(container, null, dataTable, false);
                }
            }
        }
    }
}