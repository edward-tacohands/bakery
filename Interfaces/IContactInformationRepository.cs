using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Entities;
using bageri.api.ViewModels;
using bageri.api.ViewModels.ContactInformation;

namespace bageri.api.Interfaces;

public interface IContactInformationRepository
{
    public Task<bool> Update(int id, UpdateContactInformationsViewModel model);
}
