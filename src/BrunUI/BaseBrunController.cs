using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI
{
    [Route(template: "/brunapi/{controller=Home}/{action=Index}/{id?}")]
    public class BaseBrunController:Controller
    {

    }
}
