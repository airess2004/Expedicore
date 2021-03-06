﻿using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IContinentValidation
    {
        Continent VCreateObject(Continent continent, IContinentService _continentService);
        Continent VUpdateObject(Continent continent, IContinentService _continentService);
    }
}
