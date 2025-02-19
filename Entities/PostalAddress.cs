using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class PostalAddress
{
    public int PostalAddressId { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public IList<Address> Addresses { get; set; }
}
