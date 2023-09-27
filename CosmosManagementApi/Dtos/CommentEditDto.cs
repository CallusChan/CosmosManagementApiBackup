namespace CosmosManagementApi.Dtos
{
    public class CommentEditDto
    {
        public int PId { get; set; }
        public string? BillId { get; set; }
        //public string? Name { get; set; }
        public string? Comment { get; set; }
    }
}
