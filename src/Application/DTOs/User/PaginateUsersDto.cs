namespace HireHive.Application.DTOs.User
{
    public class PaginateUsersDto
    {
        public string jobFilter { get; set; } = null!;
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
