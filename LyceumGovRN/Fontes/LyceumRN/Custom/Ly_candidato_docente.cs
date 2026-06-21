using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;
using System;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_candidato_docenteCustom : Ly_candidato_docente.CustomBase
    {        
        //public override string PreInsert(Ly_candidato_docente.Row row, TConnectionWritable cn)
        //{

        //    RN.RNBase.RetirarEspaco(row);
        //    if (!string.IsNullOrEmpty(row.Cep))
        //    {
        //        row.Cep = row.Cep.RetirarCaracteres();
        //        if (row.Cep.ToString().Length < 8) return "CEP inválido. <br>O CEP deve ter 8 números.";
        //    }

        //    if (!string.IsNullOrEmpty(row.Nome))
        //    {
        //        if (row.Nome.Length < 5)
        //            return "Nome deve conter pelo menos cinco letras.";
        //    }

        //    if (!Validacao.Validou(row.Nome, Validacao.Tipo.nome)) return "Nome inválido.<br>O nome deve ter apenas letras.";

        //    if (!string.IsNullOrEmpty(row.Dt_nasc.ToString()) && !Validacao.ValidouData(row.Dt_nasc, Validacao.Tipo.data)) return "Data de nascimento inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.";

        //    if (string.IsNullOrEmpty(row.Dt_nasc.ToString())) return "Data de nascimento inválida.<br>Preenchimento obrigatório.";

        //    if (!string.IsNullOrEmpty(row.Cpf))
        //    {
        //        row.Cpf = row.Cpf.RetirarMascaraCPF();
        //        if (!Validacao.ValidaCpf(row.Cpf)) return "O CPF informado é inválido.";
        //    }

        //    if (!string.IsNullOrEmpty(row.Rg_num))
        //    {
        //        if (row.Rg_num.Length < 5 || row.Rg_num.Length > 15)
        //            return "O número do documento deve conter no mínimo cinco dígitos e no máximo quinze caracteres.";

        //        row.Rg_num = row.Rg_num.Replace("-", "").Replace(".", "");
        //    }

        //    //pessoa:


        //    if (string.IsNullOrEmpty(Convert.ToString(row.Municipio_nasc))) return "Município de nascimento inválido.<br>Preenchimento obrigatório.";

        //    if (string.IsNullOrEmpty(row.Nacionalidade.ToString())) return "Nacionalidade inválida.<br>Preenchimento obrigatório.";


        //    //if (!string.IsNullOrEmpty(row.Cprof_num))
        //    //{
        //    //    if (row.Cprof_num.ToString().Length < 7) return "Número da carteira profissional inválido. <br>O número da carteira profissional deve ter no mínimo 7 caracteres.";
        //    //}

        //    if (!string.IsNullOrEmpty(row.E_mail) && !Validacao.Validou(row.E_mail, Validacao.Tipo.email)) return "Email externo inválido.<br>O e-mail está em um formato incorreto.";

        //    if (!string.IsNullOrEmpty(row.Celular)) row.Celular = row.Celular.RetirarMascaraTelefone();
        //    if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();


        //    //dados carteira profissional
        //    if (!string.IsNullOrEmpty(row.Cprof_num) && (string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
        //        return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
        //    else if (!string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
        //        return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
        //    else if (!string.IsNullOrEmpty(row.Cprof_serie) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_uf)))
        //        return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
        //    else if (!string.IsNullOrEmpty(row.Cprof_uf) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie)))
        //        return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";


        //    //Verificação de Município
        //    //País Nascimento
        //    if (!string.IsNullOrEmpty(row.Pais_nasc))
        //    {
        //        string paisNasc;
        //        paisNasc = RN.Endereco.ObterPais(row.Pais_nasc);
        //        if (!string.IsNullOrEmpty(paisNasc))
        //        {
        //            if (paisNasc.ToUpper() == "BRASIL")
        //            {
        //                if (!string.IsNullOrEmpty(row.Municipio_nasc))
        //                {
        //                    if (!RN.Endereco.VerificarMunicipio(row.Municipio_nasc))
        //                        return row.Nome + " - Município de nascimento inválido.";
        //                }
        //            }
        //            else
        //            {
        //                //if (!string.IsNullOrEmpty(row.Municipio_nasc))
        //                //{
        //                //    if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.Municipio_nasc.ToString(), row.Pais_nasc))
        //                //        return row.Nome + " - Município de nascimento inválido.";
        //                //}
        //            }
        //        }
        //    }

        //    //País Endereço
        //    if (!string.IsNullOrEmpty(row.End_pais))
        //    {
        //        string paisEnd;
        //        paisEnd = RN.Endereco.ObterPais(row.End_pais);
        //        if (!string.IsNullOrEmpty(paisEnd))
        //        {
        //            if (paisEnd.ToUpper() == "BRASIL")
        //            {
        //                if (!string.IsNullOrEmpty(row.End_municipio))
        //                {
        //                    if (!RN.Endereco.VerificarMunicipio(row.End_municipio))
        //                        return row.Nome + " - Município de nascimento inválido.";
        //                }
        //            }
        //            else
        //            {
        //                //if (!string.IsNullOrEmpty(row.End_municipio))
        //                //{
        //                //    if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.End_municipio.ToString(), row.End_pais))
        //                //        return row.Nome + " - Município de nascimento inválido.";
        //                //}
        //            }
        //        }
        //    }

        //    //datas
        //    if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Rg_dtexp)) return "Data expedição documento/nascimento inválidas.<br>A data de expedição do documento de indentificação deve ser maior que a data de nascimento.";
        //    if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Cprof_dtexp)) return "Data expedição carteira profissional/nascimento inválidas.<br>A data de expedição da carteira profissional deve ser maior que a data de nascimento.";

        //    return string.Empty;
        //}

        public override string PreUpdate(Ly_candidato_docente.Row row, TConnectionWritable cn)
        {
            RN.RNBase.RetirarEspaco(row);
            if (!string.IsNullOrEmpty(row.Cep))
            {
                row.Cep = row.Cep.RetirarCaracteres();
                if (row.Cep.ToString().Length < 8) return "CEP inválido. <br>O CEP deve ter 8 números.";
            }

            if (!string.IsNullOrEmpty(row.Nome))
            {
                if (row.Nome.Length < 5)
                    return "Nome deve conter pelo menos cinco letras.";
            }

            if (!Validacao.Validou(row.Nome, Validacao.Tipo.nome)) return "Nome inválido.<br>O nome deve ter apenas letras.";

            if (!string.IsNullOrEmpty(row.Dt_nasc.ToString()) && !Validacao.ValidouData(row.Dt_nasc, Validacao.Tipo.data)) return "Data de nascimento inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.";

            if (string.IsNullOrEmpty(row.Dt_nasc.ToString())) return "Data de nascimento inválida.<br>Preenchimento obrigatório.";

            if (!string.IsNullOrEmpty(row.Cpf))
            {
                row.Cpf = row.Cpf.RetirarMascaraCPF();
                if (!Validacao.ValidaCpf(row.Cpf)) return "O CPF informado é inválido.";
            }

            if (!string.IsNullOrEmpty(row.Rg_num))
            {
                if (row.Rg_num.Length < 5 || row.Rg_num.Length > 15)
                    return "O número do documento deve conter no mínimo cinco dígitos e no máximo quinze caracteres.";

                row.Rg_num = row.Rg_num.Replace("-", "").Replace(".", "");
            }

            //pessoa:


            if (string.IsNullOrEmpty(Convert.ToString(row.Municipio_nasc))) return "Município de nascimento inválido.<br>Preenchimento obrigatório.";

            if (string.IsNullOrEmpty(row.Nacionalidade.ToString())) return "Nacionalidade inválida.<br>Preenchimento obrigatório.";


            //if (!string.IsNullOrEmpty(row.Cprof_num))
            //{
            //    if (row.Cprof_num.ToString().Length < 7) return "Número da carteira profissional inválido. <br>O número da carteira profissional deve ter no mínimo 7 caracteres.";
            //}

            if (!string.IsNullOrEmpty(row.E_mail) && !Validacao.Validou(row.E_mail, Validacao.Tipo.email)) return "Email externo inválido.<br>O e-mail está em um formato incorreto.";

            if (!string.IsNullOrEmpty(row.Celular)) row.Celular = row.Celular.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();


            //dados carteira profissional
            if (!string.IsNullOrEmpty(row.Cprof_num) && (string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_serie) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_uf) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";


            //Verificação de Município
            //País Nascimento
            if (!string.IsNullOrEmpty(row.Pais_nasc))
            {
                string paisNasc;
                paisNasc = RN.Endereco.ObterPais(row.Pais_nasc);
                if (!string.IsNullOrEmpty(paisNasc))
                {
                    if (paisNasc.ToUpper() == "BRASIL")
                    {
                        if (!string.IsNullOrEmpty(row.Municipio_nasc))
                        {
                            if (!RN.Endereco.VerificarMunicipio(row.Municipio_nasc))
                                return row.Nome + " - Município de nascimento inválido.";
                        }
                    }
                    else
                    {
						//if (!string.IsNullOrEmpty(row.Municipio_nasc))
						//{
						//    if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.Municipio_nasc.ToString(), row.Pais_nasc))
						//        return row.Nome + " - Município de nascimento inválido.";
						//}
                    }
                }
            }

            //País Endereço
            if (!string.IsNullOrEmpty(row.End_pais))
            {
                string paisEnd;
                paisEnd = RN.Endereco.ObterPais(row.End_pais);
                if (!string.IsNullOrEmpty(paisEnd))
                {
                    if (paisEnd.ToUpper() == "BRASIL")
                    {
                        if (!string.IsNullOrEmpty(row.End_municipio))
                        {
                            if (!RN.Endereco.VerificarMunicipio(row.End_municipio))
                                return row.Nome + " - Município de nascimento inválido.";
                        }
                    }
                    else
                    {
						//if (!string.IsNullOrEmpty(row.End_municipio))
						//{
						//    if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.End_municipio.ToString(), row.End_pais))
						//        return row.Nome + " - Município de nascimento inválido.";
						//}
                    }
                }
            }

            //datas
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Rg_dtexp)) return "Data expedição documento/nascimento inválidas.<br>A data de expedição do documento de indentificação deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Cprof_dtexp)) return "Data expedição carteira profissional/nascimento inválidas.<br>A data de expedição da carteira profissional deve ser maior que a data de nascimento.";

            return string.Empty;
        }
    }
}
