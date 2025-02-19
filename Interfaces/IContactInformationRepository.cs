using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Entities;
using bageri.api.ViewModels;

namespace bageri.api.Interfaces;

public interface IContactInformationRepository
{
    public Task<ContactInformation> Add(AddContactViewModel model);
}
