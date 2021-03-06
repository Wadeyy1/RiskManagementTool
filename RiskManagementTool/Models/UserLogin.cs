﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiskManagementTool.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int Valid { get; set; }
    }
}