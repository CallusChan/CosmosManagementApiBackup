using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class ProductStorageBill
{
    /// <summary>
    /// ProductStorage Primary id
    /// </summary>
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? BillId { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Product? Product { get; set; }
}
