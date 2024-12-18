﻿using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.ViewModels
{
    [NotMapped]
    public class ClaimVM
    {
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal? ClaimExpenses { get; set; }
        public int? LeaveDays { get; set; }
        public int? KilometersTravelled { get; set; }
        public byte[]? DocumentBinary { get; set; }
 
        public IFormFile? Document { get; set; }
        public string ClaimStatus { get; set; }
        public string UserId { get; set; }
    }
}
