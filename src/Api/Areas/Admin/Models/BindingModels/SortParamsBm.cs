﻿using HireHive.Application.DTOs.Admin;

namespace HireHive.Api.Areas.Admin.Models.BindingModels
{
    public class SortParamsBm
    {
        public List<AssessmentResultDto>? AssessmentData { get; set; }
        public string? SortOrder { get; set; }
        public string? OrderByField { get; set; }
    }
}
