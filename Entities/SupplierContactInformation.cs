using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class SupplierContactInformation
{
    public int SupplierId { get; set; }
    public int ContactInformationId { get; set; }
    public Supplier Supplier { get; set; }
    public ContactInformation ContactInformation { get; set; }
}
