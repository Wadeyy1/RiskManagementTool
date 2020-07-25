using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiskManagementTool.Models
{
    public class RiskInfo
    {
        public int ID { get; set; }
        [Required]
        public string RiskSummary { get; set; }
        [Required]
        public string RiskDescription { get; set; }
        [Required]
        public decimal RiskRating { get; set; }
    }

}