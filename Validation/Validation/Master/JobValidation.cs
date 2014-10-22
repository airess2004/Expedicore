using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class JobValidation : IJobValidation
    {  
        public Job VName(Job job, IJobService _jobService)
        {
            if (String.IsNullOrEmpty(job.Name) || job.Name.Trim() == "")
            {
                job.Errors.Add("Name", "Tidak boleh kosong");
            }
            return job;
        }

        
        public Job VCreateObject(Job job, IJobService _jobService)
        {
            VName(job, _jobService);
            if (!isValid(job)) { return job; }
            return job;
        }

        public Job VUpdateObject(Job job, IJobService _jobService)
        { 
            VName(job, _jobService);
            if (!isValid(job)) { return job; }
            return job;
        }

        public bool isValid(Job obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
