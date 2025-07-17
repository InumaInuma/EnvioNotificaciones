using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesDomian.Entities
{
    [Table("Cliente")] // Asegúrate que el nombre de la tabla sea el que quieres en la DB
    public class Cliente
    {
        [Key]
        [Column("CodCli")] // Si la columna en la DB se llamará CodCli
        public int CodCli { get; set; } // Esta será la PK de Cliente

        [Column("NomEmp")] // Si la columna en la DB se llamará NombreEmpresa
        [MaxLength(255)] // Ajusta la longitud según necesites
        [Required]
        public string NomEmp { get; set; }

        // Puedes añadir más propiedades si las necesitas en el futuro
    }
}
