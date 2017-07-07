using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.ViewModels
{
    public class AccountViewModel : IBaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
