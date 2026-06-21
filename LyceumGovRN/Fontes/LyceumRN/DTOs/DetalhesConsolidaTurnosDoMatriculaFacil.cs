using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Techne.Lyceum.RN.DTOs
{
    public class DetalhesConsolidaTurnosDoMatriculaFacil : IEquatable<DetalhesConsolidaTurnosDoMatriculaFacil>
    {
        public String Curso { get; set; }
        public String NomeCurso { get; set; }
        public String ModalidadeCurso { get; set; }
        public String TipoCurso { get; set; }
        public String Serie { get; set; }
        public String Turno { get; set; }
        public String TipoOperacao { get; set; }
        public String TipoRetorno = string.Empty;
        public String DescricaoRetorno = string.Empty;

        public bool Equals(DetalhesConsolidaTurnosDoMatriculaFacil obj)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(obj, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, obj)) return true;

            //Check whether the products' properties are equal.
            return Curso.Equals(obj.Curso) && Serie.Equals(obj.Serie) && TipoCurso.Equals(obj.TipoCurso);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashTipoCurso = TipoCurso == null ? 0 : TipoCurso.GetHashCode();

            //Get hash code for the Code field.
            int hashSerie = Serie.GetHashCode();

            //Calculate the hash code for the product.
            return hashTipoCurso ^ hashSerie;
        }

        public DetalhesConsolidaTurnosDoMatriculaFacil()
        { }

        public DetalhesConsolidaTurnosDoMatriculaFacil( string _Curso, string _NomeCurso
            , string _ModalidadeCurso, string _TipoCurso, string _Serie, string _Turno, string _TipoOperacao)
        {
            this.Curso = _Curso;
            this.NomeCurso = _NomeCurso;
            this.ModalidadeCurso = _ModalidadeCurso;
            this.TipoCurso = _TipoCurso;
            this.Serie = _Serie;
            this.Turno = _Turno;
            this.TipoCurso = _TipoOperacao;
        }
    }
}
