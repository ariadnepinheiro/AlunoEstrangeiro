using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyFotoPessoa : IEntity
    {
        public byte[] Foto { get; set; }

        public Decimal? Pessoa { get; set; }
    }
}
