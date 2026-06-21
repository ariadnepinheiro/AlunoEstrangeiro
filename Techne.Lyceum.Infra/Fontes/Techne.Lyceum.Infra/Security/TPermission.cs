using System;
using System.Threading;

namespace Techne
{
    public class TPermission
    {
        public const string DenialMessage = "O seu perfil de acesso não permite operações nesta página.";

        internal static LocalDataStoreSlot permissionSlot = Thread.AllocateNamedDataSlot("tpermission");

        private readonly bool allowDelete;

        private readonly bool allowInsert;

        private readonly bool allowUpdate;

        private readonly bool execute;

        private readonly string resource;

        private readonly string resourceType;

        private bool audit = true;

        private bool readOnly;

        internal TPermission(string resource, string resourceType, bool execute, bool insert, bool update, bool delete)
        {
            this.resource = resource;
            this.resourceType = resourceType;
            this.execute = execute;
            this.allowInsert = insert;
            this.allowUpdate = update;
            this.allowDelete = delete;
            this.readOnly = !insert || !update || !delete;
        }

        public static TPermission CurrentPermission
        {
            get
            {
                return TPermission.ThreadPermission;
            }

            set
            {
                TPermission.ThreadPermission = value;
            }
        }

        public bool AllowDelete
        {
            get
            {
                return this.allowDelete;
            }
        }

        public bool AllowInsert
        {
            get
            {
                return this.allowInsert;
            }
        }

        public bool AllowUpdate
        {
            get
            {
                return this.allowUpdate;
            }
        }

        public bool Audit
        {
            get
            {
                return this.audit;
            }

            internal set
            {
                this.audit = value;
            }
        }

        public bool Execute
        {
            get
            {
                return this.execute;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }

        public string Resource
        {
            get
            {
                return this.resource;
            }
        }

        public string ResourceType
        {
            get
            {
                return this.resourceType;
            }
        }

        internal static TPermission ThreadPermission
        {
            get
            {
                try
                {
                    return Thread.GetData(permissionSlot) as TPermission;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                try
                {
                    Thread.SetData(permissionSlot, value);
                }
                catch
                {
                    try
                    {
                        Thread.SetData(permissionSlot, null);
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void SetReadOnly(bool value)
        {
            this.readOnly = value;
        }
    }
}