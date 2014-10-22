using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactService
    {
        IQueryable<Contact> GetQueryable();
        Contact GetObjectById(int Id);
        Contact CreateObject(Contact contact,ICityLocationService _citylocationService);
        Contact UpdateObject(Contact contact, ICityLocationService _citylocationService);
        Contact SoftDeleteObject(Contact contact);
        bool IsNameDuplicated(Contact contact);
        Contact UpdateLastShipment(Contact contact);
    }

}