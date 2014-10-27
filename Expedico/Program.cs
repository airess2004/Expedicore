using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expedico
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ExpedicoEntities();
            using (db)
            {

//db.DeleteAllTables();
            }

        }
    }
}
