using System;
using System.Collections.Generic;

namespace Brun.Services
{
    public interface IBackRunFilterService
    {
        List<Type> GetBackRunTypes();
        List<Type> GetOnceBackRunTypes();
        List<Type> GetPlanBackRunTypes();
        List<Type> GetQueueBackRunTypes();
        List<Type> GetTimeBackRunTypes();
    }
}