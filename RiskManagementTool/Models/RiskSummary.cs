using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RiskManagementTool.Models
{
    public class RiskSummary
    {
        [Required]
        public decimal RiskRatingTotal { get; set; }

        [Required]
        public int RiskRatingTotalCount { get; set; }
    }
}