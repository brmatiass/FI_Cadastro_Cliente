using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiarios
    /// </summary>
    internal class DaoBeneficiarios : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiarios">Objeto de beneficiario</param>
        internal long Incluir(DML.Beneficiarios beneficiarios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiarios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdClient", beneficiarios.IdClient));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);

            return ret;
        }

        /// <summary>
        /// Consulta beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de Beneficiario</param>
        internal DML.Beneficiarios Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben.FirstOrDefault();
        }

        /// <summary>
        /// Lista todos os beneficiarios
        /// </summary>
        internal List<DML.Beneficiarios> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Consulta beneficiario IdClient
        /// </summary>
        /// <param name="IdClient">id do cliente</param>
        internal List<DML.Beneficiarios> Listar(long IdClient)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IdClient", IdClient));

            DataSet ds = base.Consultar("FI_SP_ListByIdBeneficiario", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto Beneficiario</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        /// <summary>
        /// Atualiza um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto Beneficiario</param>
        internal void Alterar(DML.Beneficiarios beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id));

            base.Executar("FI_SP_AltBeneficiario", parametros);
        }

        internal bool VerificarExistencia(string CPF, long IdClient)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdClient", IdClient));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal bool ValidarCPF(string CPF)
        {
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11 || CPF.Distinct().Count() == 1)
                return false;

            int[] multiplicadores = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(CPF[i].ToString()) * multiplicadores[i];

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(CPF[9].ToString()) != primeiroDigitoVerificador)
                return false;

            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(CPF[i].ToString()) * multiplicadores[i];

            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(CPF[10].ToString()) != segundoDigitoVerificador)
                return false;

            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return !ben.existCPF(CPF);
        }

        public bool existCPF(string CPF)
        {
            ConnectionStringSettings conn = System.Configuration.ConfigurationManager.ConnectionStrings["BancoDeDados"];
            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM BENEFICIARIOS WHERE CPF = @CPF";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CPF", CPF);

                connection.Open();
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        private List<DML.Beneficiarios> Converter(DataSet ds)
        {
            List<DML.Beneficiarios> lista = new List<DML.Beneficiarios>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiarios ben = new DML.Beneficiarios();
                    ben.Id = row.Field<long>("Id");
                    ben.Nome = row.Field<string>("Nome");
                    ben.CPF = row.Field<string>("CPF");
                    ben.IdClient = row.Field<long>("IdClient");
                    lista.Add(ben);
                }
            }
            return lista;
        }
    }
}
