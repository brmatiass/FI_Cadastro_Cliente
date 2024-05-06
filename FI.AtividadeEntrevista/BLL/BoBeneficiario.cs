using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo Beneficiario
        /// </summary>
        /// <param name="beneficiarios">Objeto de Beneficiario</param>
        public long Incluir(DML.Beneficiarios Beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Incluir(Beneficiarios);
        }

        /// <summary>
        /// Altera um Beneficiario
        /// </summary>
        /// <param name="beneficiarios">Objeto de Beneficiario</param>
        public void Alterar(DML.Beneficiarios Beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            ben.Alterar(Beneficiarios);
        }

        /// <summary>
        /// Consulta o Beneficiario pelo id
        /// </summary>
        /// <param name="id">id do Beneficiario</param>
        /// <returns></returns>
        public DML.Beneficiarios Consultar(long id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Excluir Beneficiario por id
        /// </summary>
        /// <param name="id">id do Beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiarios> Listar()
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Listar();
        }

        /// <summary>
        /// Consulta o beneficiario pelo id do cliente
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public List<DML.Beneficiarios> Listar(long id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Listar(id);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool ValidarCPF(string CPF)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.ValidarCPF(CPF);
        }
    }
}
