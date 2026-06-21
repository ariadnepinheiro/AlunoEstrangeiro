namespace Techne
{
    internal sealed class HadesPermission : TPermission
    {
        internal HadesPermission(string resource, string resourceType, bool execute, bool insert, bool update, bool delete) :
            base(resource, resourceType, execute, insert, update, delete)
        {
        }
    }
}