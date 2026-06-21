using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class NucleoServiceProfile: MapperProfileBase
    {
        public NucleoServiceProfile() : base("NucleoServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<Nucleo, DTOCoordenadoria>();
        }
    }
}
