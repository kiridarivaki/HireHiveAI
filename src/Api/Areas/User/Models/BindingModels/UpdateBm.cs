﻿using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.User.Models.BindingModels
{
    public class UpdateBm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }
    }
}
