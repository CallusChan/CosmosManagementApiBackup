namespace CosmosManagementApi.Dtos
{
    public class AllBillsDto
    {
        public DateTime? Date { get; set; }
        public string? BillNo { get; set; }
        public string? CustomerNo { get; set; }
        public string? CustomerName { get; set;}
        public string? Name { get; set; }
        public string? PaymentMethod{ get; set;}
        public string? AmountSum { get; set; }
        public string? Department { get; set; } 
        public string? Comment { get; set; }
        public string? StaffName{ get; set; }
        public string? BillType{ get; set; }

    }
}
