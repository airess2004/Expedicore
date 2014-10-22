using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IJobValidation
    {
        Job VCreateObject(Job job, IJobService _jobService);
        Job VUpdateObject(Job job, IJobService _jobService);
    }
}
