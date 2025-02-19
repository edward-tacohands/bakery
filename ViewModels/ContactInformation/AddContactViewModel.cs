using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels;

public class AddContactViewModel : ContactInformationsViewModel
{
    public int CustomerId { get; set; }
}
