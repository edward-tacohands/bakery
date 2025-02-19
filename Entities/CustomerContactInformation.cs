using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class CustomerContactInformation
{
    public int CustomerId { get; set; }
    public int ContactInformationId { get; set; }
    public Customer Customer { get; set; }
    public ContactInformation ContactInformation { get; set; }
}
