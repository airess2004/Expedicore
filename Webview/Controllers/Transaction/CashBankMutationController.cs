using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Constant;

namespace WebView.Controllers
{
    public class CashBankMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashBankMutationController");
        private ICashBankMutationService _cashBankMutationService;
        private IAccountUserService _accountUserService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private ICashMutationService _cashMutationService;
        private ICashBankService _cashBankService;

        public CashBankMutationController()
        {
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
        }


        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashBankMutationService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             SourceCashBank = model.SourceCashBank.Name,
                             TargetCashBank = model.TargetCashBank.Name,
                             model.MutationDate,
                             model.Amount,
                             model.ConfirmationDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Id,
                            model.Code,
                            model.SourceCashBank,
                            model.TargetCashBank,
                            model.MutationDate,
                            model.Amount,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashBankMutation model = new CashBankMutation();
            try
            {
                model = _cashBankMutationService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.Amount,
                model.SourceCashBankId,
                SourceCashBank = _cashBankService.GetObjectById(model.SourceCashBankId).Name,
                model.TargetCashBankId,
                TargetCashBank = _cashBankService.GetObjectById(model.TargetCashBankId).Name,
                model.MutationDate,
                model.IsConfirmed,
                model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashBankMutation model)
        {
            try
            {
                int userId = AuthenticationModel.GetUserId();
                model.CreatedById = userId;
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _cashBankMutationService.CreateObject(model, _cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(CashBankMutation model)
        {
            try
            {
                var data = _cashBankMutationService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.UpdatedById = userId;
                data.SourceCashBankId = model.SourceCashBankId;
                data.TargetCashBankId = model.TargetCashBankId;
                data.Amount = model.Amount;
                data.MutationDate = model.MutationDate;
                model = _cashBankMutationService.UpdateObject(data, _cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(CashBankMutation model)
        {
            try
            {
                var data = _cashBankMutationService.GetObjectById(model.Id);
                model = _cashBankMutationService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Confirm(CashBankMutation model)
        {
            try
            {

                var data = _cashBankMutationService.GetObjectById(model.Id);
                DateTime ConfirmationDate = DateTime.Now;
                model = _cashBankMutationService.ConfirmObject(data,ConfirmationDate,_cashMutationService,_cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(CashBankMutation model)
        {
            try
            {
                var data = _cashBankMutationService.GetObjectById(model.Id);
                model = _cashBankMutationService.UnconfirmObject(data,_cashMutationService,_cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
       
    }
}
