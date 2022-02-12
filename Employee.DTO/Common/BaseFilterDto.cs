namespace Employee.DTO.Common
{
    public class BaseFilterDto
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public bool ApplySort { get; set; }
        public string? SortProperty { get; set; }
        public bool IsAscending { get; set; }
    }
}
