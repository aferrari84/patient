using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Interfaces.Services
{
    public interface IEmailService
    {
        bool Send(string To, string Subject, string Body, bool isHtml = false);

    }
}
