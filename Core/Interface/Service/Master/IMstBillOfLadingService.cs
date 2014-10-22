using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IMstBillOfLadingService
    {
        IQueryable<MstBillOfLading> GetQueryable();
        MstBillOfLading GetObjectById(int Id);
        MstBillOfLading CreateObject(MstBillOfLading mstbilloflading);
        MstBillOfLading UpdateObject(MstBillOfLading mstbilloflading);
        MstBillOfLading SoftDeleteObject(MstBillOfLading mstbilloflading);
        bool IsNameDuplicated(MstBillOfLading mstbilloflading);
    }
}