﻿using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Auth.Models.ViewModels
{
    public class UserInfoVm
    {
        public List<string> Roles { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }
    }
}
