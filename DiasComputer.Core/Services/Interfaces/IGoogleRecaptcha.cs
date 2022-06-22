using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IGoogleRecaptcha
    {
        Task<bool> IsUserValid();
    }
}
