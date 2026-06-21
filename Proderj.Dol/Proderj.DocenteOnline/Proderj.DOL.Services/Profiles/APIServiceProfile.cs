using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class APIServiceProfile: MapperProfileBase
    {
        public APIServiceProfile() : base("APIServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DadosAPI, DTOApi>();
        }
    }
}
