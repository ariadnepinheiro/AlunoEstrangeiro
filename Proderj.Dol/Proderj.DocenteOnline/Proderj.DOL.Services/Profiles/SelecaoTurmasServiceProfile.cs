using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class SelecaoTurmasServiceProfile: MapperProfileBase
    {
        public SelecaoTurmasServiceProfile() : base("SelecaoTurmasServiceProfile") { }
        
        protected override void CreateMaps()
        {
            CreateMap<SelecaoTurmas, DTOSelecaoTurmas>();
        }
    }
}
