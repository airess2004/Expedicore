using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobService : IJobService 
    {  
        private IJobRepository _repository;
        private IJobValidation _validator;

        public JobService(IJobRepository _jobRepository, IJobValidation _jobValidation)
        {
            _repository = _jobRepository;
            _validator = _jobValidation;
        }

        public IQueryable<Job> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Job GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Job CreateObject(Job job)
        {
            job.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(job,this)))
            {
                job = _repository.CreateObject(job);
            }
            return job;
        }
         
        public Job UpdateObject(Job job)
        {
            if (!isValid(_validator.VUpdateObject(job, this)))
            {
                job = _repository.UpdateObject(job);
            }
            return job;
        }
         
        public Job SoftDeleteObject(Job job)
        {
            job = _repository.SoftDeleteObject(job);
            return job;
        }



        public bool isValid(Job obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
