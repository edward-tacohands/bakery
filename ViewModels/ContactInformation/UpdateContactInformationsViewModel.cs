using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.ContactInformation;

public class UpdateContactInformationsViewModel
{
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
