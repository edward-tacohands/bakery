using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace bageri.api.Interfaces;

public interface IProductPreparationRepository
{
    public Task<bool> Add(NewBatchViewModel model);
}
