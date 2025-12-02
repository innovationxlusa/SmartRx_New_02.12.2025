using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.DTOs
{
    public class AcronymsDTO
    {
        public long Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Acronym { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Abbreviation { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Elaboration { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? Details { get; set; }
    }
}
