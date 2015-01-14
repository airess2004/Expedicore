using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptVoucherValidator
    {
        ReceiptVoucher VCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                       IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService);
        ReceiptVoucher VUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                        ICashBankService _cashBankService);
        ReceiptVoucher VReconcileObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService);
        bool ValidCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                IReceivableService _receivableService);
        bool ValidUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService);
        bool ValidReconcileObject(ReceiptVoucher receiptVoucher);
        bool ValidUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService);
        bool isValid(ReceiptVoucher receiptVoucher);
        string PrintError(ReceiptVoucher receiptVoucher);
    }
}
