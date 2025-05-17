namespace HireHive.Api.Areas.Admin.Models.BindingModels
{
    public class ListUsersBm
    {
        public string jobFilter { get; set; } = null!;
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

    }
}
