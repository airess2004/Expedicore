using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IMstBillOfLadingRepository : IRepository<MstBillOfLading>
    {
       IQueryable<MstBillOfLading> GetQueryable();
       MstBillOfLading GetObjectById(int Id);
       MstBillOfLading CreateObject(MstBillOfLading model);
       MstBillOfLading UpdateObject(MstBillOfLading model);
       MstBillOfLading SoftDeleteObject(MstBillOfLading model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(MstBillOfLading model);
       int GetLastMasterCode(int officeId);
    }
}