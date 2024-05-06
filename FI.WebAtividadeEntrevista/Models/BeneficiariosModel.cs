using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiarios
    /// </summary>
    public class BeneficiariosModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite um CPF válido - EX. 999.999.999-99")]
        public string CPF { get; set; }

        /// <summary>
        /// ID Cliente
        /// </summary>
        public long IdClient { get; set; }

    }
}