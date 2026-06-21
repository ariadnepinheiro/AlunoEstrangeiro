namespace Seeduc.Infra.Data
{
    using System.Data;

    public enum TechneDbType
    {
        [TechneTextUdtType(SqlDbType.VarChar, 255)]
        T_ALFA_HUGE, 

        [TechneTextUdtType(SqlDbType.VarChar, 500)]
        T_ALFA500, 

        [TechneTextUdtType(SqlDbType.VarChar, 7000)]
        T_ALFA7000, 

        [TechneTextUdtType(SqlDbType.VarChar, 2000)]
        T_ALFAEXTRALARGE, 

        [TechneTextUdtType(SqlDbType.VarChar, 100)]
        T_ALFALARGE, 

        [TechneTextUdtType(SqlDbType.VarChar, 50)]
        T_ALFAMEDIUM, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_ALFASMALL, 

        [TechneTextUdtType(SqlDbType.VarChar, 10)]
        T_ALFASMALL_10, 

        [TechneTextUdtType(SqlDbType.VarChar, 17)]
        T_ALFASMALL_17, 

        [TechneNumericUdtType(SqlDbType.Decimal, 4, 0)]
        T_ANO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 1, 0)]
        T_AULA, 

        [TechneTextUdtType(SqlDbType.VarChar, 9)]
        T_CEP, 

        [TechneTextUdtType(SqlDbType.VarChar, 19)]
        T_CGC, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_CODIGO, 

        [TechneTextUdtType(SqlDbType.VarChar, 80)]
        T_CODIGO2, 

        [TechneTextUdtType(SqlDbType.VarChar, 14)]
        T_CPF, 

        [TechneUdtTypeAttribute(SqlDbType.DateTime)]
        T_DATA, 

        [TechneNumericUdtType(SqlDbType.Decimal, 10, 2)]
        T_DECIMAL_MEDIO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 12, 4)]
        T_DECIMAL_MEDIO_PRECISO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 14, 6)]
        T_DECIMAL_MEDIO_PRECISO6, 

        [TechneNumericUdtType(SqlDbType.Decimal, 7, 4)]
        T_DECIMAL_PRECISO, 

        [TechneTextUdtType(SqlDbType.VarChar, 50)]
        T_E_MAIL, 

        [TechneTextUdtType(SqlDbType.VarChar, 30)]
        T_ESTADO_CIVIL, 

        [TechneTextUdtType(SqlDbType.VarChar, 4)]
        T_FALTA, 

        [TechneTextUdtType(SqlDbType.VarChar, 50)]
        T_GRUPO_RELATORIO, 

        [TechneUdtTypeAttribute(SqlDbType.DateTime)]
        T_HORA, 

        [TechneUdtTypeAttribute(SqlDbType.Image)]
        T_IMAGEM, 

        [TechneTextUdtType(SqlDbType.VarChar, 200)]
        T_MENSAGEM, 

        [TechneNumericUdtType(SqlDbType.Decimal, 2, 0)]
        T_MES, 

        [TechneTextUdtType(SqlDbType.VarChar, 2)]
        T_MNEMONICO, 

        [TechneTextUdtType(SqlDbType.VarChar, 6)]
        T_MNEMONICO6, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_MOTIVO, 

        [TechneTextUdtType(SqlDbType.VarChar, 100)]
        T_NOME_LONGO, 

        [TechneTextUdtType(SqlDbType.VarChar, 50)]
        T_NOME_MED, 

        [TechneNumericUdtType(SqlDbType.Decimal, 10, 0)]
        T_NUMERO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 15, 0)]
        T_NUMERO_GRANDE, 

        [TechneNumericUdtType(SqlDbType.Decimal, 3, 0)]
        T_NUMERO_PEQUENO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 15, 0)]
        T_NUMFUNC, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_OCORRENCIA_FALTA, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_ORIGEM_ESTORNO, 

        [TechneNumericUdtType(SqlDbType.Decimal, 3, 2)]
        T_PERCENTUAL, 

        [TechneNumericUdtType(SqlDbType.Decimal, 5, 2)]
        T_PERCENTUAL52, 

        [TechneNumericUdtType(SqlDbType.Decimal, 5, 4)]
        T_PERCENTUAL54, 

        [TechneTextUdtType(SqlDbType.VarChar, 11)]
        T_PISPASEP, 

        [TechneTextUdtType(SqlDbType.VarChar, 4)]
        T_PROVA, 

        [TechneTextUdtType(SqlDbType.VarChar, 10)]
        T_PROVA10, 

        [TechneTextUdtType(SqlDbType.VarChar, 10)]
        T_REGIME, 

        [TechneTextUdtType(SqlDbType.VarChar, 50)]
        T_RELATORIO, 

        [TechneTextUdtType(SqlDbType.VarChar, 30)]
        T_SALA_ESPECIAL, 

        [TechneNumericUdtType(SqlDbType.Decimal, 1, 0)]
        T_SEMESTRE, 

        [TechneNumericUdtType(SqlDbType.Decimal, 2, 0)]
        T_SEMESTRE2, 

        [TechneTextUdtType(SqlDbType.Char, 1)]
        T_SEXO, 

        [TechneTextUdtType(SqlDbType.Char, 1)]
        T_SIMNAO, 

        [TechneTextUdtType(SqlDbType.VarChar, 12)]
        T_SISTEMA, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_SIT_ALUNO, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_SIT_CANDIDATO_VEST, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_SIT_CHEQUE, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_SIT_HISTMATRICULA, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_SIT_MATGRADE, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_SIT_MATRICULA, 

        [TechneTextUdtType(SqlDbType.VarChar, 30)]
        T_TELEFONE, 

        [TechneTextUdtType(SqlDbType.VarChar, 20)]
        T_TIPO_AVALIACAO, 

        [TechneTextUdtType(SqlDbType.Char, 1)]
        T_TIPO_COMPOSICAO, 

        [TechneTextUdtType(SqlDbType.VarChar, 8)]
        T_TIPO_NOTA, 

        [TechneTextUdtType(SqlDbType.VarChar, 15)]
        T_TIPO_TAXA, 

        [TechneTextUdtType(SqlDbType.VarChar, 2)]
        T_UF, 

        [TechneNumericUdtType(SqlDbType.Decimal, 6, 0)]
        T_VALOR_INTEIRO
    }
}