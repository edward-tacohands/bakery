using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class AddressType
{
    public int AddressTypeId { get; set; }
    public string Value { get; set; }
    public IList<Address> Addresses { get; set; }
}
