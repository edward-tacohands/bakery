using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Helpers;

public class BageriException(string message) : Exception(message)
{
}
