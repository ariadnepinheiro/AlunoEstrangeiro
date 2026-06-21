namespace Proderj.DOL.Domain
{
	public class TCE_LOGRADOURO
	{
		public virtual int ID_LOGRADOURO { get; set; }
        public virtual string ID_MUNICIPIO { get; set; }
        public virtual string CEP { get; set; }
        public virtual string NOME { get; set; }

        public virtual TCE_MUNICIPIO Municipio { get; set; }
	}
}
