namespace CosmosManagementApi.Dtos
{
  public class PaymentMethodDto
  {
    //支付方式
    public string? Method { get; set; }
    //支付金额
    public decimal? Amount { get; set; }
    public int CardId { get; set; } = 0;
    //卡的ID
  }
}
