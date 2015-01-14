$(document).ready(function () {
    //========================================================================================================================
    // Initiate
    var pvDetailEdit = 0;
    var pvBankEdit = 0;
    var jobNumber = 0;
    var subJobNumber = 0;
    var jobCode = 0;
    var pvd_settlement_for = '';
    var pvd_lookupGrid_settlement_for = '';
    var pvd_lookupUrl_settlement_for = '';
    $('#paymentvoucher_content').css("height", $(window).height() - 120);
    $('#paymentvoucher_content').css("width", $(window).width() - 20);
    $('#lookup_div_shipmentorder').dialog('close');
    $('#lookup_div_payment_voucher_detail').dialog('close');
    $('#lookup_div_payment_voucher_detail_bank').dialog('close');
    $('#lookup_div_bank').dialog('close');
    $('#lookup_div_city').dialog('close');
    $('#lookup_div_customer').dialog('close');
    $('#lookup_div_pvd_settlement').dialog('close');
    $('#lookup_div_coa').dialog('close');
    $('#lookup_div_coa_bank').dialog('close');
    $('#lookup_div_coa_different').dialog('close');
    $('#dialogPrint').dialog('close');
   
    // First Load
    // tools.js
    var PVId = getQueryStringByName('Id');
   
    // Edit Mode
    if (PVId != "") {
        $('input[name=rbPaymentBy]').attr('disabled', 'disabled');
        $('input[name=rbSettlementFor]').attr('disabled', 'disabled');
        $('input[name=rbPaymentTo]').attr('disabled', 'disabled');
        $('#btnPVcustomer').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
        $('#btnPVcity').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
        $('#btn_add_payment_voucher_detail').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#btn_edit_payment_voucher_detail').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#btn_delete_payment_voucher_detail').removeClass('ui-state-disabled').removeAttr('disabled');
        $('#txtPVaddress').attr('disabled', 'disabled');
        $('#ddlJobowner').attr('disabled', 'disabled');
        FirstLoad();
    }
    else {
        $('#btnPVbank').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#btnPVcustomer').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#btn_add_payment_voucher_detail').attr('disabled', 'disabled');
        $('#btn_edit_payment_voucher_detail').attr('disabled', 'disabled');
        $('#btn_delete_payment_voucher_detail').attr('disabled', 'disabled');
    }
    // End First Load

    function FirstLoad() {

        $.ajax({
            dataType: "json",
            url: base_url + "CashAdvance/GetInfo?Id=" + PVId,
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                            window.location = base_url + "CashAdvance";
                        }
                    }
                }
                else {
                    //$('#txtPVreference').val(result.Code);
                    $('#txtPVpaymentvoucherno').val(result.CashAdvanceNo);

                    $('#txtPVrate').numberbox('setValue', result.Rate);
                    $('#txtPVdaterate').val(dateEnt(result.ExRateDate));
                    //$('#txtPVdateprint').datebox('setValue', dateEnt(result.PrintedOn)).data('printing', result.Printing);
                    //if (result.VerifyAccOn != null && result.VerifyAccOn != 'null')
                    //    $('#txtPVverify').val(dateEnt(result.VerifyAccOn));

                    $('#txtPVcustomer').val(result.EmployeeId).data('kode', result.EmployeeId);
                    $('#txtPVnmcustomer').val(result.EmployeeName);

                    // Payment Voucher Detail
                    if (result.ListCashAdvanceDetail != null) {
                        if (result.ListCashAdvanceDetail.length > 0) {
                            for (var i = 0; i < result.ListCashAdvanceDetail.length; i++) {
                                AddPaymentDetail(0, result.ListCashAdvanceDetail[i].Id,
                                    result.ListCashAdvanceDetail[i].AccountId,
                                    result.ListCashAdvanceDetail[i].AccountCode,
                                    result.ListCashAdvanceDetail[i].AccountName,
                                    result.ListCashAdvanceDetail[i].Description,
                                    result.ListCashAdvanceDetail[i].DCNote,
                                    result.ListCashAdvanceDetail[i].AmountUSD,
                                    result.ListCashAdvanceDetail[i].AmountIDR,
                                    result.ListCashAdvanceDetail[i].PayableCode,
                                    result.ListCashAdvanceDetail[i].ShipmentOrderNo,
                                    result.ListCashAdvanceDetail[i].RefRate,
                                    result.ListCashAdvanceDetail[i].ExRateId,
                                    result.ListCashAdvanceDetail[i].ShipmentOrderId,
                                    result.ListCashAdvanceDetail[i].RemainingAmountUSD,
                                    result.ListCashAdvanceDetail[i].RemainingAmountIDR,
                                   result.ListCashAdvanceDetail[i].PaymentCash,
                                   result.ListCashAdvanceDetail[i].PayableId,
                                   result.ListCashAdvanceDetail[i].RefId,
                                   result.ListCashAdvanceDetail[i].PayableId,
                                   result.ListCashAdvanceDetail[i].PVBank);
                            }
                        }
                    }
                    CalculatePayment();

                    //// Enable or Disable Button
                    //if (result.Printing > 0) {

                    //    //$('#txtPVdateprint').datebox({
                    //    //    disabled: false
                    //    //});

                    //    $('#btn_save_cash, #txtPVBankcashpayment').attr('disabled', 'disabled');
                    //    $('#btn_add_pv_bank, #btn_edit_pv_bank, #btn_delete_pv_bank').attr('disabled', 'disabled');

                    //    // PV Detail
                    //    $('#btnPVDrefno').attr('disabled', 'disabled');
                    //    $('#btn_add_payment_voucher_detail, #btn_delete_payment_voucher_detail').attr('disabled', 'disabled');
                    //    $('#txtPVDamountusd, #txtPVDamountidr, #txtPVDaccountdesc').attr('disabled', 'disabled');

                    //    $('#txtPVBankbankcode, #txtPVBankremarks, #txtPVBankamount, #optPVBankamountusd, #optPVBankamountidr').attr('disabled', 'disabled');
                    //    $('#txtPVBankduedate').datebox({
                    //        disabled: true
                    //    });
                    //}

                    //// Have been Verified
                    //if ($('#txtPVverify').val() != '') {
                    //    $('#btn_edit_payment_voucher_detail').attr('disabled', 'disabled');

                    //    //$('#txtPVdateprint').datebox({
                    //    //    disabled: true
                    //    //});

                    //    $('#btnPVdifferentaccountcode').attr('disabled', 'disabled');

                    //    // Disable Save 
                    //    $('#paymentvoucher_form_btn_save').removeAttr('onclick').unbind('click').off('click');

                    //}
                }
            }
        });
    }

    function AddPaymentDetail(isEdit, pvDetailId, accountId, accountCode, accountName, description, dcnote, amountusd, amountidr, refno,
                            shipment, rate, exrateid, shipmentid, differenceusd, differenceidr, cashpayment, refaccountid, refid, refdetailid, pvbank) {
        //// Validate
        //if (accountCode.trim() == '') {
        //    $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
        //    return
        //}

        var isValid = true;

        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
        else if (description == null || description == undefined || description == '') {
            $.messager.alert('Information', 'Description cannot be empty...!!', 'info');
        }
            // End Validate
        else {
            var newData = {}
            newData.accountid = accountId;
            newData.accountcode = accountCode;
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.description = (description != undefined && description != "") ? description.toUpperCase() : "";
            newData.amountidr = amountidr;
            newData.amountusd = amountusd;
            newData.dcnote = dcnote;
            newData.refno = refno;
            newData.shipment = shipment;
            newData.shipmentid = shipmentid;
            newData.differenceusd = differenceusd;
            newData.differenceidr = differenceidr;
            newData.rate = rate;
            newData.exrateid = exrateid;
            newData.cashpayment = cashpayment;
            newData.refaccountid = refaccountid;
            newData.refid = refid;
            newData.refdetailid = refdetailid;
            newData.pvbank = pvbank;

            // New Record
            if (isEdit == 0) {
                pvDetailId = pvDetailId == 0 ? GetNextId($("#table_payment_voucher_detail")) : pvDetailId;
                jQuery("#table_payment_voucher_detail").jqGrid('addRowData', pvDetailId, newData);
            }
            else {
                var id = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#table_payment_voucher_detail").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#table_payment_voucher_detail").trigger("reloadGrid");

            CalculatePayment();
        }
    }

    jQuery("#table_payment_voucher_detail").jqGrid({
        datatype: "local",
        onSelectRow: function (id) {
            // Cash
            var cashpayment = $(this).getRowData(id).cashpayment;
            if (cashpayment == '')
                cashpayment = 0;
            $('#txtPVBankcashpayment').numberbox('setValue', cashpayment);

            // Bank
            $("#table_pv_bank").jqGrid("clearGridData", true).trigger("reloadGrid");
            var pvbank = $.parseJSON($(this).getRowData(id).pvbank);
            if (pvbank != null) {

                // Disabled Edit PV Bank if already Verified
                $('#btn_edit_pv_bank').removeAttr('disabled');
                if ($('#txtPVverify').val() != '')
                    $('#btn_edit_pv_bank').attr('disabled', 'disabled');

                for (var i = 0; i < pvbank.length; i++) {
                    AddPVBank(0, pvbank[i].Id, pvbank[i].PVDetailId, pvbank[i].AccountId, pvbank[i].AccountCode, pvbank[i].AccountName, pvbank[i].BankCode, pvbank[i].DueDate,
                                    pvbank[i].AmountCrr, pvbank[i].Amount, pvbank[i].Remarks, pvbank[i].StatusDueDate)
                }
            }
        },
        height: 130,
        width: 915,
        rowNum: 150,
        scrollrows: true,
        shrinkToFit: false,
        colNames: ['Account Code', 'Account ID', 'Account Name', 'Description', 'DCNote', 'Amount USD', 'Amount IDR', 'Ref No', 'Shipment', 'Rate',
                    'cashpayment', 'pvbank', 'exrateid', 'shipmentid', 'refaccountid', 'refid', 'refdetailid', 'differenceusd', 'differenceidr'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120, align: 'right', hidden: true },
            { name: 'accountid', index: 'accountid', width: 310, hidden: true },
            { name: 'accountname', index: 'accountname', width: 310, hidden: true },
            { name: 'description', index: 'description', width: 310 },
            { name: 'dcnote', index: 'dcnote', width: 60, hidden: true },
            { name: 'amountusd', index: 'amountusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'amountidr', index: 'amountidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'refno', index: 'refno', width: 60, align: "right" },
            { name: 'shipment', index: 'shipment', width: 130 },
            { name: 'rate', index: 'rate', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'cashpayment', index: 'cashpayment', width: 80, hidden: true },
            { name: 'pvbank', index: 'pvbank', width: 80, hidden: true },
            { name: 'exrateid', index: 'pvbank', width: 80, hidden: true },
            { name: 'shipmentid', index: 'shipmentid', width: 80, hidden: true },
            { name: 'refaccountid', index: 'refaccountid', width: 80, hidden: true },
            { name: 'refid', index: 'refdetailid', width: 80, hidden: true },
            { name: 'refdetailid', index: 'refdetailid', width: 80, hidden: true },
            { name: 'differenceusd', index: 'differenceusd', width: 80, hidden: true },
            { name: 'differenceidr', index: 'differenceidr', width: 80, hidden: true }
        ],
        gridComplete:
          function () {
              var ids = $(this).jqGrid('getDataIDs');
              for (var i = 0; i < ids.length; i++) {
                  var cl = ids[i];
                  $(this).setCell(cl, 'accountcode', '', '', { 'title': $(this).getRowData(cl).accountname });
              }
          }
    });

    $('input[name=rbPaymentBy]').click(function () {

        $('#txtPVcustomer').val('').data('kode', '');
        $('#txtPVnmcustomer').val('');
        $('#txtPVaddress').val('');
        $("#table_payment_voucher_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#table_pv_bank").jqGrid("clearGridData", true).trigger("reloadGrid");
        CalculatePayment();

        // Payment
        $('#btn_add_pv_bank, #btn_edit_pv_bank, #btn_delete_pv_bank').attr('disabled', 'disabled');
        $('#btn_save_cash, #txtPVBankcashpayment').attr('disabled', 'disabled');

        // Check Payment Voucher Detail Bank Currency
        var type = $(this).attr('id').toLowerCase();

        // Bank IDR
        if (type.indexOf("bankidr") != -1) {
            $('#optPVDBamountidr').attr('checked', 'checked');

            // Enable Browse Bank button
            $('#btnPVbank').removeAttr('disabled').addClass('ui-state-default').removeClass('ui-state-disabled');

            // Payment
            $('#btn_add_pv_bank, #btn_edit_pv_bank, #btn_delete_pv_bank').removeAttr('disabled');
            $('#btn_save_cash, #txtPVBankcashpayment').attr('disabled', 'disabled');
        }
            // Bank USD
        else if (type.indexOf("bankusd") != -1) {
            $('#optPVDBamountusd').attr('checked', 'checked');

            // Enable Browse Bank button
            $('#btnPVbank').removeAttr('disabled').addClass('ui-state-default').removeClass('ui-state-disabled');

            // Payment
            $('#btn_add_pv_bank, #btn_edit_pv_bank, #btn_delete_pv_bank').removeAttr('disabled');
            $('#btn_save_cash, #txtPVBankcashpayment').attr('disabled', 'disabled');
        }
        else {
            // Disable Browse Bank button
            $('#btnPVbank').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');

            $('#txtPVbankcode, #txtPVbankname, #txtPVintbankname').val('').data('kode', '');

            // Payment
            $('#btn_save_cash, #txtPVBankcashpayment').removeAttr('disabled');
            $('#btn_add_pv_bank, #btn_edit_pv_bank, #btn_delete_pv_bank').attr('disabled', 'disabled');

            // Remove Account Bank
            $('#txtPVBankaccountcode').val('').data('kode', '');
            $('#txtPVBankaccountname').val('');
        }
    })

    $('input[name=rbPaymentTo]').click(function () {

        $('#txtPVcustomer').val('').data('kode', '');
        $('#txtPVnmcustomer').val('');
        $('#txtPVaddress').val('');
        $("#table_payment_voucher_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#table_pv_bank").jqGrid("clearGridData", true).trigger("reloadGrid");
        CalculatePayment();

        $('#btnPVcustomer').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#btnPVcity').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#txtPVaddress').attr('disabled', 'disabled').val('');

        if ($('#rbPaymentToNone').is(':checked')) {
            $('#btnPVcustomer').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
            $('#btnPVcity').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
            $('#txtPVaddress').removeAttr('disabled', 'disabled');
        }
    });

    $('input[name=rbSettlementFor]').click(function () {

        //    /*
        //    'Jika bukan General harus bukan None
        //'Jika Inv C/N harus Agent
        //'Jika PR harus bukan Agent, Company, Personal, None
        //'Jika TR harus Shipper, Consignee
        //*/

        $('#txtPVcustomer').val('').data('kode', '');
        $('#txtPVnmcustomer').val('');
        $('#txtPVaddress').val('');
        $("#table_payment_voucher_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#table_pv_bank").jqGrid("clearGridData", true).trigger("reloadGrid");
        CalculatePayment();

        $('#btnPVcustomer').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#btnPVcity').addClass('ui-state-default').removeClass('ui-state-disabled').removeAttr('disabled', 'disabled');
        $('#txtPVaddress').attr('disabled', 'disabled').val('');

        $('input[name=rbPaymentTo]').attr('disabled', 'disabled');
        // General
        if ($('#rbSettlementForGeneral').is(':checked')) {
            $('#rbPaymentToSSLine, #rbPaymentToEMKL, #rbPaymentToDepo, #rbPaymentToIATA, #rbPaymentToPersonal, #rbPaymentToAgent, #rbPaymentToCompany, #rbPaymentToShipper, #rbPaymentToConsignee').removeAttr('disabled');

            $('#rbPaymentToNone').removeAttr('disabled').attr('checked', 'checked');

            if ($('#rbPaymentToNone').is(':checked')) {
                $('#btnPVcustomer').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
                $('#btnPVcity').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
                $('#txtPVaddress').removeAttr('disabled', 'disabled');
            }
        }
            // Temporary Receipt
        else if ($('#rbSettlementForTR').is(':checked')) {
            $('#rbPaymentToShipper, #rbPaymentToConsignee').removeAttr('disabled');
            $('#rbPaymentToShipper').attr('checked', 'checked');
        }
            // Invoice C/N Agent
        else if ($('#rbSettlementForAgent').is(':checked')) {
            $('#rbPaymentToAgent').removeAttr('disabled').attr('checked', 'checked');
        }
            // Payment Request
        else if ($('#rbSettlementForPR').is(':checked')) {
            $('#rbPaymentToSSLine, #rbPaymentToEMKL, #rbPaymentToDepo, #rbPaymentToShipper, #rbPaymentToConsignee, #rbPaymentToIATA').removeAttr('disabled');
            $('#rbPaymentToSSLine').attr('checked', 'checked');
        }
            // Receipt Voucher
        else {
            $('#rbPaymentToSSLine, #rbPaymentToEMKL, #rbPaymentToDepo, #rbPaymentToIATA, #rbPaymentToPersonal, #rbPaymentToAgent, #rbPaymentToCompany, #rbPaymentToShipper, #rbPaymentToConsignee').removeAttr('disabled');
        }
    });

    // -------------------------------------------------------------------- LOOKUP ----------------------------------------------------
    // ---------------------------------------------------- Bank ----------------------------------------------------
    // Browse
    $('#btnPVbank').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_bank');
        lookupGrid.setGridParam({ url: base_url + 'CashBank/GetLookUp', postData: { filters: null }, page: 'last' }).trigger("reloadGrid");
        $('#lookup_div_bank').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_bank').click(function () {
        $('#lookup_div_bank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_bank').click(function () {
        var id = jQuery("#lookup_table_bank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_bank").jqGrid('getRowData', id);

            $('#txtPVbankcode').val(ret.MasterCode).data('kode', id);
            $('#txtPVintbankname').val(ret.Name);
            $('#txtPVbankname').val(ret.Currency);

            $('#lookup_div_bank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_bank").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Id', 'Name', 'Description', 'Currency', 'IsBank'],
        colModel: [
            { name: 'MasterCode', index: 'MasterCode', width: 60, align: 'right' },
            { name: 'Name', index: 'Name', width: 100 },
            { name: 'Description', index: 'Description', width: 250 },
            { name: 'Currency', index: 'Currency', width: 150 },
            { name: 'IsBank', index: 'IsBank', width: 150  ,sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" }, formatter: 'select'},
        ],
        pager: jQuery('#lookup_pager_bank'),
        sortname: 'MasterCode',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 460,
        height: 350
    });
    $("#lookup_table_bank").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_bank").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_bank').click();
        },
        scrollingRows: true
    });
    // ---------------------------------------------------- End Bank ----------------------------------------------------

    // ---------------------------------------------------- Customer ----------------------------------------------------
    // Browse
    $('#btnPVcustomer').click(function () {

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");


            var lookUpURL = base_url + 'MstEmployee/GetList';
            var lookupGrid = $('#lookup_table_customer');
            lookupGrid.setGridParam({ url: base_url + 'MstEmployee/GetList', postData: { filters: null }, page: 'last' }).trigger("reloadGrid");
        $('#lookup_div_customer').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_customer').click(function () {
        $('#lookup_div_customer').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_customer').click(function () {
        var id = jQuery("#lookup_table_customer").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_customer").jqGrid('getRowData', id);

            $('#txtPVcustomer').val(ret.code).data("kode", id);
            $('#txtPVnmcustomer').val(ret.name);
            $('#lookup_div_customer').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_customer").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Group'],
        colModel: [{ name: 'code', index: 'Id', width: 80, align: 'right' },
                  { name: 'name', index: 'Name', width: 200 },
                  { name: 'as', index: 'Group', width: 200 }],
        //page: 'last', // last page
        pager: jQuery('#lookup_pager_customer'),
        //sortorder: "asc",
        sortname: "Id",
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 460,
        height: 350
    });
    $("#lookup_table_customer").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_customer").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_customer').click();
        },
        scrollingRows: true
    });
    // ---------------------------------------------------- End Customer ----------------------------------------------------

    // ---------------------------------------------------- City ----------------------------------------------------
    // Browse
    $('#btnPVcity').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_city').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_city').click(function () {
        $('#lookup_div_city').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_city').click(function () {
        var id = jQuery("#lookup_table_city").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_city").jqGrid('getRowData', id);

            $('#txtPVcity').val(ret.int).data("kode", ret.code);
            $('#txtPVnmity').val(ret.name);

            $('#lookup_div_city').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_city").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Int'],
        colModel: [{ name: 'code', index: 'citycode', width: 100, align: "center" },
                  { name: 'name', index: 'cityname', width: 250 },
                  { name: 'int', index: 'intcity', width: 100 }
        ],
        pager: jQuery('#lookup_pager_city'),
        sortname: 'citycode',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 460,
        height: 350
    });
    $("#lookup_table_city").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- End City ----------------------------------------------------    
    // -------------------------------------------------------------------- END LOOKUP ----------------------------------------------------



    // ------------------------------------------------------ Payment Detail ------------------------------------

    // ---------------------------------------------------- Settlement For ----------------------------------------------------
    // Browse
    $('#btnPVDrefno').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        $('.lookup_table_pvd_settlement').hide();
        $('.lookup_table_pvd_settlement_pr_detail').hide();

        var customerId = $('#txtPVcustomer').data('kode');
        var currencyId = $('#txtPVbankname').data('kode');
        var listExistedInvoice = [];
            pvd_settlement_for = 'pr';
            $('.lookup_table_pvd_settlement_pr').show();
            //$("#lookup_table_pvd_settlement_pr_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
            pvd_lookupGrid_settlement_for = $('#lookup_table_pvd_settlement_pr');
            pvd_lookupUrl_settlement_for = 'PaymentRequest/GetLookUpCA';
            $('#lookup_div_pvd_settlement').dialog({ title: 'Payment Request List' });


        pvd_lookupGrid_settlement_for.setGridParam({
            url: base_url + pvd_lookupUrl_settlement_for,
            postData: {
                filters: null, CustomerId: function () { return customerId; },
                CurrencyId: function () { return currencyId; }
            },
            page: 'last'
        }).trigger("reloadGrid");

        $('#lookup_div_pvd_settlement').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_pvd_settlement').click(function () {
        $('#lookup_div_pvd_settlement').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_pvd_settlement').click(function () {
        var settlement_master = '';
        var settlement_detail = '';
        
        settlement_master = $('#lookup_table_pvd_settlement_pr');
        settlement_detail = $('#lookup_table_pvd_settlement_pr_detail');

            var idMaster = settlement_master.jqGrid('getGridParam', 'selrow');
            if (idMaster) {
                var retMaster = settlement_master.jqGrid('getRowData', idMaster);

                var idDetail = settlement_detail.jqGrid('getGridParam', 'selrow');
                if (idDetail) {
                    var retDetail = settlement_detail.jqGrid('getRowData', idDetail);
                    $('#txtPVDrefno').val(retDetail.prno).data('refid', idMaster).data('refdetailid', idDetail);
                    $('#txtPVDaccountdesc').val(retDetail.description);
                    $('#txtPVDrefshipmentno').val(retMaster.ShipmentOrderCode);
                    $('#txtPVDamountusd').numberbox('setValue', retDetail.amountusd);
                    $('#txtPVDamountidr').numberbox('setValue', retDetail.amountidr);
                    $('#txtPVDamountpaidusd').numberbox('setValue', retDetail.remainingamountusd);
                    $('#txtPVDamountpaididr').numberbox('setValue', retDetail.remainingamountidr);

                    $('#txtPVDrefrate').numberbox('setValue', retDetail.rate).data('kode', retDetail.exrateid);
                    $('#lookup_div_pvd_settlement').dialog('close');
                } else {
                    $.messager.alert('Information', 'Please Select Data...1!!', 'info');
                };

            } else {
                $.messager.alert('Information', 'Please Select Data...2!!', 'info');
            };
    });

    // ------------------------------ JqGrid Payment Request ------------------------------------
    jQuery("#lookup_table_pvd_settlement_pr").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['PRNo', 'Shipment No', 'Payment USD', 'Payment IDR'],
        colModel: [{ name: 'prno', index: 'Id', width: 80, align: "center" },
                  { name: 'ShipmentOrderCode', index: 'ShipmentOrderCode', width: 200 },
                  { name: 'paymentusd', index: 'paymentusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'paymentidr', index: 'paymentidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'Id',
        sortorder: "asc",
        onSelectRow: function (id) {
            //$("#lookup_table_pvd_settlement_pr_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
            var id = jQuery("#lookup_table_pvd_settlement_pr").jqGrid('getGridParam', 'selrow');
            if (id) {
                var lookUpURL = base_url + 'PaymentRequest/GetLookUpPVDetail';
                var lookupGrid = $('#lookup_table_pvd_settlement_pr_detail');

                // Existed PRDetail
                var listExistedPRDetail = [];
                var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');
                for (var i = 0; i < data.length; i++) {
                    var obj = data[i].refdetailid;
                    if (!isNaN(obj))
                        obj = parseInt(obj);
                    listExistedPRDetail.push(obj);
                }
                // END Existed PRDetail


                lookupGrid.setGridParam({
                    postData: {
                        'PRId': function () { return id; }, 
                        'listExistedPRDetail': function () { return JSON.stringify(listExistedPRDetail); },
                    },
                    url: lookUpURL
                }).trigger("reloadGrid");
            } else {
                $.messager.alert('Information', 'Please Select PR First...!!', 'info');
            };
        },
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 570,
        height: 150
    });
    $("#lookup_table_pvd_settlement_pr").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_pr").jqGrid('bindKeys', {
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Payment Request ------------------------------------

    // ------------------------------ JqGrid Payment Request Detail ------------------------------------
    jQuery("#lookup_table_pvd_settlement_pr_detail").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['PRNo', 'Description', 'Amount USD', 'Remaining USD', 'Amount IDR', ' Remaining IDR', 'shipment no', 'shipment id', 'rate', 'exrateid',
                    'prmasteraccountid', 'prmasteraccountcode', 'prmasteraccountiname'],
        colModel: [{ name: 'prno', index: 'prno', width: 80, align: "center" },
                  { name: 'description', index: 'description', width: 180 },
                  { name: 'amountusd', index: 'amountusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'remainingamountusd', index: 'remainingamountusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'amountidr', index: 'amountidr', width: 120 ,align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'remainingamountidr', index: 'remainingamountidr', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'shipmentno', index: 'shipmentno', width: 200, hidden: true },
                  { name: 'shipmentid', index: 'shipmentid', width: 200, hidden: true },
                  { name: 'rate', index: 'rate', width: 200, hidden: true },
                  { name: 'exrateid', index: 'exrateid', width: 200, hidden: true },
                  { name: 'prmasteraccountid', index: 'prmasteraccountid', width: 200, hidden: true },
                  { name: 'prmasteraccountcode', index: 'prmasteraccountcode', width: 200, hidden: true },
                  { name: 'prmasteraccountname', index: 'prmasteraccountname', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'Id',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 650,
        height: 150
    });
    $("#lookup_table_pvd_settlement_pr_detail").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_pr_detail").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_pvd_settlement').click();
        },
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Payment Request Detail ------------------------------------

    // ------------------------------ JqGrid Temporary Receipt ------------------------------------
    jQuery("#lookup_table_pvd_settlement_tr").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['TRNo', 'Customer Name', 'Payment USD', 'Payment IDR'],
        colModel: [{ name: 'trno', index: 'trno', width: 80, align: "center" },
                  { name: 'contactname', index: 'contactname', width: 200 },
                  { name: 'paymentusd', index: 'paymentusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'paymentidr', index: 'paymentidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'trno',
        sortorder: "asc",
        onSelectRow: function (id) {
            //$("#lookup_table_pvd_settlement_tr_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
            var id = jQuery("#lookup_table_pvd_settlement_tr").jqGrid('getGridParam', 'selrow');
            if (id) {
                var lookUpURL = base_url + 'TemporaryReceipt/LookUpPVTRDetail';
                var lookupGrid = $('#lookup_table_pvd_settlement_tr_detail');

                // Existed PRDetail
                var listExistedTRDetail = [];
                var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');
                for (var i = 0; i < data.length; i++) {
                    var obj = data[i].refdetailid;
                    if (!isNaN(obj))
                        obj = parseInt(obj);
                    listExistedTRDetail.push(obj);
                }
                // END Existed PRDetail

                // Currency
                var currency = "IDR";
                if ($('#rbPaymentByCashUSD').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) {
                    currency = "USD";
                }

                lookupGrid.setGridParam({
                    postData: {
                        'TRId': function () { return id; }, 'listExistedTRDetail': function () { return JSON.stringify(listExistedTRDetail); },
                        'Currency': function () { return currency; }
                    },
                    url: lookUpURL
                }).trigger("reloadGrid");
            } else {
                $.messager.alert('Information', 'Please Select PR First...!!', 'info');
            };
        },
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 570,
        height: 150
    });
    $("#lookup_table_pvd_settlement_tr").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_tr").jqGrid('bindKeys', {
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Temporary Receipt ------------------------------------

    // ------------------------------ JqGrid Temporary Receipt Detail ------------------------------------
    jQuery("#lookup_table_pvd_settlement_tr_detail").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['id', 'TRNo', 'Shipment No', 'Account ID', 'Account Code', 'Account Name', 'Description', 'Amount USD', 'Amount IDR', 'shipment id', 'rate', 'exrateid',
                    'trmasteraccountid', 'trmasteraccountcode', 'trmasteraccountiname', 'trmasteraccountdesc'],
        colModel: [{ name: 'id', index: 'id', width: 50, hidden: true },
                  { name: 'trno', index: 'trno', width: 80, align: "center" },
                  { name: 'shipmentno', index: 'shipmentno', width: 200 },
                  { name: 'accountid', index: 'accountid', width: 50, hidden: true },
                  { name: 'accountcode', index: 'accountcode', width: 70, align: "center", hidden: true },
                  { name: 'accountname', index: 'accountname', width: 130, hidden: true },
                  { name: 'description', index: 'description', width: 180, hidden: true },
                  { name: 'amountusd', index: 'amountusd', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'amountidr', index: 'amountidr', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'shipmentid', index: 'shipmentid', width: 200, hidden: true },
                  { name: 'rate', index: 'rate', width: 200, hidden: true },
                  { name: 'exrateid', index: 'exrateid', width: 200, hidden: true },
                  { name: 'trmasteraccountid', index: 'trmasteraccountid', width: 200, hidden: true },
                  { name: 'trmasteraccountcode', index: 'trmasteraccountcode', width: 200, hidden: true },
                  { name: 'trmasteraccountname', index: 'trmasteraccountname', width: 200, hidden: true },
                  { name: 'trmasteraccountdesc', index: 'trmasteraccountdesc', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'trno',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 650,
        height: 150
    });
    $("#lookup_table_pvd_settlement_tr_detail").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_tr_detail").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_pvd_settlement').click();
        },
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Temporary Receipt Detail ------------------------------------

    // ------------------------------ JqGrid Receipt Voucher ------------------------------------
    jQuery("#lookup_table_pvd_settlement_rv").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['RV No', 'Shipment No', 'Payment USD', 'Payment IDR'],
        colModel: [{ name: 'rvno', index: 'prno', width: 80, align: "center" },
                  { name: 'shipmentno', index: 'shipmentno', width: 200 },
                  { name: 'paymentusd', index: 'paymentusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'paymentidr', index: 'paymentidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'rvno',
        sortorder: "asc",
        onSelectRow: function (id) {
            //$("#lookup_table_pvd_settlement_rv_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
            var id = jQuery("#lookup_table_pvd_settlement_rv").jqGrid('getGridParam', 'selrow');
            if (id) {
                var lookUpURL = base_url + 'ReceiptVoucher/LookUpPVRVDetail';
                var lookupGrid = $('#lookup_table_pvd_settlement_rv_detail');

                // Existed RVDetail
                var listExistedRVDetail = [];
                var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');
                for (var i = 0; i < data.length; i++) {
                    var obj = data[i].refdetailid;
                    if (!isNaN(obj))
                        obj = parseInt(obj);
                    listExistedRVDetail.push(obj);
                }
                // END Existed RVDetail

                // Currency
                var currency = "IDR";
                if ($('#rbPaymentByCashUSD').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) {
                    currency = "USD";
                }

                lookupGrid.setGridParam({
                    postData: {
                        'RVId': function () { return id; }, 'listExistedRVDetail': function () { return JSON.stringify(listExistedRVDetail); },
                        'Currency': function () { return currency; }
                    },
                    url: lookUpURL
                }).trigger("reloadGrid");
            } else {
                $.messager.alert('Information', 'Please Select PR First...!!', 'info');
            };
        },
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 570,
        height: 150
    });

    $("#lookup_table_pvd_settlement_rv").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_rv").jqGrid('bindKeys', {
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Receipt Voucher ------------------------------------

    // ------------------------------ JqGrid Receipt Voucher Detail ------------------------------------
    jQuery("#lookup_table_pvd_settlement_rv_detail").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['RV No', 'Account ID', 'Account Code', 'Account Name', 'Description', 'Amount USD', 'Amount IDR', 'shipment no', 'shipment id', 'rate', 'exrateid'],
        colModel: [{ name: 'rvno', index: 'prno', width: 80, align: "center" },
                  { name: 'accountid', index: 'accountid', width: 50, hidden: true },
                  { name: 'accountcode', index: 'accountcode', width: 70, align: "center" },
                  { name: 'accountname', index: 'accountname', width: 130 },
                  { name: 'description', index: 'description', width: 180 },
                  { name: 'amountusd', index: 'amountusd', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'amountidr', index: 'amountidr', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'shipmentno', index: 'shipmentno', width: 200, hidden: true },
                  { name: 'shipmentid', index: 'shipmentid', width: 200, hidden: true },
                  { name: 'rate', index: 'rate', width: 200, hidden: true },
                  { name: 'exrateid', index: 'exrateid', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'rvno',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 650,
        height: 150
    });
    $("#lookup_table_pvd_settlement_rv_detail").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_rv_detail").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_pvd_settlement').click();
        },
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Receipt Voucher Detail ------------------------------------

    // ------------------------------ JqGrid Invoice ------------------------------------
    jQuery("#lookup_table_pvd_settlement_inv").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Invoice No', 'Principle By', 'Shipment No', 'Payment USD', 'Payment IDR', 'Shipment Id', 'rate', 'rate id', '', '', ''],
        colModel: [{ name: 'invoiceno', index: 'invoicesno', width: 80, align: "center" },
                  { name: 'jobowner', index: 'jobowner', width: 100, align: "center" },
                  { name: 'shipmentno', index: 'shipmentno', width: 200 },
                  { name: 'paymentusd', index: 'paymentusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'paymentidr', index: 'paymentidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'shipmentid', index: 'shipmentid', width: 200, hidden: true },
                  { name: 'rate', index: 'rate', width: 200, hidden: true },
                  { name: 'exrateid', index: 'exrateid', width: 200, hidden: true },
                  { name: 'invoicemasteraccountid', index: 'invoicemasteraccountid', width: 200, hidden: true },
                  { name: 'invoicemasteraccountcode', index: 'invoicemasteraccountcode', width: 200, hidden: true },
                  { name: 'invoicemasteraccountname', index: 'invoicemasteraccountname', width: 200, hidden: true }
        ],
        multiselect: true,
        pager: jQuery('#lookup_pager_pvd_settlement'),
        sortname: 'invoicesno',
        sortorder: "asc",
        //onSelectRow: function (id) {
        //    //$("#lookup_table_pvd_settlement_inv_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
        //    var id = jQuery("#lookup_table_pvd_settlement_inv").jqGrid('getGridParam', 'selrow');
        //    if (id) {
        //        var lookUpURL = base_url + 'Invoice/LookUpPVInvoiceDetail';
        //        var lookupGrid = $('#lookup_table_pvd_settlement_inv_detail');

        //        // Existed InvoiceDetail
        //        var listExistedInvoiceDetail = [];
        //        var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');
        //        for (var i = 0; i < data.length; i++) {
        //            var obj = data[i].refdetailid;
        //            if (!isNaN(obj))
        //                obj = parseInt(obj);
        //            listExistedInvoiceDetail.push(obj);
        //        }
        //        // END Existed InvoiceDetail

        //        // Currency
        //        var currency = "IDR";
        //        if ($('#rbPaymentByCashUSD').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) {
        //            currency = "USD";
        //        }

        //        lookupGrid.setGridParam({
        //            postData: {
        //                'InvoiceId': function () { return id; }, 'listExistedInvoiceDetail': function () { return JSON.stringify(listExistedInvoiceDetail); },
        //                'Currency': function () { return currency; }
        //            },
        //            url: lookUpURL
        //        }).trigger("reloadGrid");
        //    } else {
        //        $.messager.alert('Information', 'Please Select PR First...!!', 'info');
        //    };
        //},
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 610,
        height: 150
    });
    $("#lookup_table_pvd_settlement_inv").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_inv").jqGrid('bindKeys', {
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Invoice ------------------------------------

    // ------------------------------ JqGrid Invoice Detail ------------------------------------
    jQuery("#lookup_table_pvd_settlement_inv_detail").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Invoice No', 'Account ID', 'Account Code', 'Account Name', 'Description', 'Amount USD', 'Amount IDR', 'shipment no', 'shipment id', 'rate', 'exrateid', '', '', ''],
        colModel: [{ name: 'invoiceno', index: 'invoicesno', width: 80, align: "center" },
                  { name: 'accountid', index: 'accountid', width: 50, hidden: true },
                  { name: 'accountcode', index: 'accountcode', width: 70, align: "center" },
                  { name: 'accountname', index: 'accountname', width: 130 },
                  { name: 'description', index: 'description', width: 180 },
                  { name: 'amountusd', index: 'amountusd', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'amountidr', index: 'amountidr', width: 80, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'shipmentno', index: 'shipmentno', width: 200, hidden: true },
                  { name: 'shipmentid', index: 'shipmentid', width: 200, hidden: true },
                  { name: 'rate', index: 'rate', width: 200, hidden: true },
                  { name: 'exrateid', index: 'exrateid', width: 200, hidden: true },
                  { name: 'invoicemasteraccountid', index: 'invoicemasteraccountid', width: 200, hidden: true },
                  { name: 'invoicemasteraccountcode', index: 'invoicemasteraccountcode', width: 200, hidden: true },
                  { name: 'invoicemasteraccountname', index: 'invoicemasteraccountname', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_pvd_settlement_detail'),
        sortname: 'invoicesno',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 650,
        height: 150
    });
    $("#lookup_table_pvd_settlement_inv_detail").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_pvd_settlement_inv_detail").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_pvd_settlement').click();
        },
        scrollingRows: true
    });
    // ------------------------------ END JqGrid Invoice Detail ------------------------------------

    // ---------------------------------------------------- End Settlement For ----------------------------------------------------

    function ClearPaymentDetailForm() {
        $('#txtPVDrefno, #txtPVDrefaccountid, #txtPVDaccountcode, #txtPVDaccountname, #txtPVDaccountdesc, #txtPVDrefshipmentno, #txtPVDrefrate').val('').data('kode', '');
        $("#btnPVDrefno, #btnPVDaccountcode").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $('#txtPVDamountusd, #txtPVDamountidr, #txtPVDamountpaidusd, #txtPVDamountpaididr, #txtPVDrefrate').numberbox('setValue', 0);
        $('#txtPVDamountusd, #txtPVDamountidr').attr('disabled', 'disabled').numberbox('setValue', 0);

        if ($('#rbPaymentByCashIDR').is(':checked') || $('#rbPaymentByBankIDR').is(':checked')) {
            $('#txtPVDamountidr').removeAttr('disabled');
        }
        else if ($('#rbPaymentByCashUSD').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) {
            $('#txtPVDamountusd').removeAttr('disabled');
        }

        // PR or C/N Agent
        if ($('#rbSettlementForPR').is(':checked') || $('#rbSettlementForAgent').is(':checked')) {
            $("#btnPVDaccountcode").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        }
        // General and Not Printed
        if ($('#rbSettlementForGeneral').is(':checked')) {
            //if ($('#txtPVdateprint').datebox('getValue') == '')
            //    $('#txtPVDamountusd, #txtPVDamountidr').removeAttr('disabled');
            $("#btnPVDrefno").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
            $('#txtPVDrefrate').numberbox('setValue', $('#txtPVrate').numberbox('getValue')).data('kode', $('#txtPVrate').attr('data-exrateid'));
        }
        else {
            $('#txtPVDamountusd, #txtPVDamountidr').attr('disabled', 'disabled').numberbox('setValue', 0);
        }

        // Already Printed
        //if ($('#txtPVdateprint').data('printing') != undefined && $('#txtPVdateprint').data('printing') != '' && $('#txtPVdateprint').data('printing') > 0) {
        //    $("#btnPVDrefno").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        //}
    }

    function ValidateViewCashAdvanceDetail() {
        var isValid = true;

        // Must have Bank if Bank USD or Bank IDR option
        if (($('#rbPaymentByBankIDR').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) && $('#txtPVbankcode').val() == "") {
            $.messager.alert('Warning', "In-Valid Bank !", 'warning');
            isValid = false;
        }
            // Must have Address if Payment is None
        else if ($('#rbPaymentToNone').is(':checked') && $.trim($('#txtPVaddress').val()) == "") {
            $.messager.alert('Warning', "In-Valid Address !", 'warning');
            isValid = false;
        }
            // Must Have Customer Code if Payment is NOT None
        else if (($('#txtPVcustomer').val() == "" || $('#txtPVcustomer').val() == "0") && (!$('#rbPaymentToNone').is(':checked'))) {
            $.messager.alert('Warning', "In-Valid Customer !", 'warning');
            isValid = false;
        }
            /*
            'Jika bukan General harus bukan None
        'Jika Inv C/N harus Agent
        'Jika PR harus bukan Agent, Company, Personal, None
        'Jika TR harus Shipper, Consignee
        */
        else if (($('#rbPaymentToNone').is(':checked') && !$('#rbSettlementForGeneral').is(':checked')) ||
            (!$('#rbPaymentToAgent').is(':checked') && $('#rbSettlementForAgent').is(':checked')) ||
            (($('#rbPaymentToAgent').is(':checked') || $('#rbPaymentToCompany').is(':checked') || $('#rbPaymentToPersonal').is(':checked') || $('#rbPaymentToNone').is(':checked'))
                && $('#rbSettlementForPR').is(':checked')) ||
            (!($('#rbPaymentToShipper').is(':checked') || $('#rbPaymentToConsignee').is(':checked')) && $('#rbSettlementForTR').is(':checked'))) {
            $.messager.alert('Warning', "In-Valid 'Payment To' !", 'warning');
            isValid = false;
        }

        return isValid;
    }

    // ADD
    $('#btn_add_payment_voucher_detail').click(function () {
        $('input[name=rbSettlementFor]').each(function () {
            if ($(this).is(':checked')) {
                //alert($(this).parent().children());
                $('#lookup_table_payment_voucher_detail fieldset legend').text('Settlement For');
            }
        });

        pvDetailEdit = 0;
        ClearPaymentDetailForm();
        var isValid = ValidateViewCashAdvanceDetail();
        if (isValid) {
            $('#lookup_div_payment_voucher_detail').dialog("open");
        }
    })

    // EDIT
    $('#btn_edit_payment_voucher_detail').click(function () {
        var isValid = ValidateViewCashAdvanceDetail();
        if (isValid) {
            var id = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
            if (id) {
                var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', id);

                ClearPaymentDetailForm();
                pvDetailEdit = 1;

                $('#lookup_btn_add_payment_voucher_detail').data('pvdetailid', id);

                //$('#txtPVDcustomername').val($('#txtCustomerName').val());
                $('#txtPVDaccountcode').val(ret.accountcode);
                $('#txtPVDaccountname').val(ret.accountname);
                $('#txtPVDaccountdesc').val(ret.description);
                $('#txtPVDamountusd').numberbox('setValue', ret.differenceusd);
                $('#txtPVDamountidr').numberbox('setValue', ret.differenceidr);
                $('#txtPVDamountpaidusd').numberbox('setValue', ret.amountusd);
                $('#txtPVDamountpaididr').numberbox('setValue', ret.amountidr);
                $('#txtPVDrefno').val(ret.refno).data('refid', ret.refid).data('refdetailid', ret.refdetailid);
                $('#txtPVDrefshipmentno').val(ret.shipment).data('kode', ret.shipmentid);
                $('#txtPVDrefrate').numberbox('setValue', ret.rate).data('kode', ret.exrateid);
                $('#txtPVDrefaccountid').val(ret.refaccountid);


                $('#lookup_div_payment_voucher_detail').dialog("open");
            } else {
                $.messager.alert('Information', 'Please Select Data...!!', 'info');
            };
        }
    })

    // Close
    $('#lookup_btn_cancel_payment_voucher_detail').click(function () {

        $('#lookup_div_payment_voucher_detail').dialog('close');
    });

    // Delete
    $('#btn_delete_payment_voucher_detail').click(function () {
        var id = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
                if (r) {
                    var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', id);

                    var isValid = true;
                    var message = "OK";
                    DeletePVDetail(id, function (result) {
                        if (JSON.stringify(result.Errors) != '{}') {
                            for (var key in result.Errors) {
                                if (key != null && key != undefined && key != 'Generic') {
                                    $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                    $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                }
                                else {
                                    $.messager.alert('Warning', result.Errors[key], 'warning');
                                }
                            }
                        }
                        else {
                            jQuery("#table_payment_voucher_detail").jqGrid('delRowData', id);

                            CalculatePayment();
                        }
                    });
                    
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // Save
    $('#lookup_btn_add_payment_voucher_detail').click(function () {


        if (!$('#rbSettlementForGeneral').is(':checked') && $.trim($('#txtPVDrefno').val()) == "") {
            $.messager.alert('Warning', "In-Valid Reference !", 'warning');
        }
            //else if($.trim($('#txtPVDaccountcode').val()) == "")
            //{
            //    $.messager.alert('Warning', "In-Valid Account Code !", 'warning');
            //}
        else if ($.trim($('#txtPVDaccountdesc').val()) == "") {
            $.messager.alert('Warning', "In-Valid Data Description !", 'warning');
        }
        else if (($('#txtPVDamountusd').numberbox('getValue') == 0 && $('#txtPVDamountidr').numberbox('getValue') == 0) ||
                ($('#txtPVDamountusd').numberbox('getValue') > 0 && $('#txtPVDamountidr').numberbox('getValue') > 0)) {
            $.messager.alert('Warning', "In-Valid Amount !", 'warning');
        }
        else {

            //// ONLY on ADD PV Detail
            //if (pvDetailEdit == 0) {
            //    if ($('#rbPaymentByBankUSD').is(":checked") || $('#rbPaymentByBankIDR').is(":checked")) {
            //        $('#txtPVBankremarks').val($('#txtPVDaccountdesc').val());
            //    }
            //}

            var pvDetailId = 0;
            var cashpayment = 0;
            var pvbank = "";
            if (pvDetailEdit == 1) {
                pvDetailId = $('#lookup_btn_add_payment_voucher_detail').data('pvdetailid');

                var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', pvDetailId);
                cashpayment = ret.cashpayment;
                pvbank = ret.pvbank;
            }

            var dcnote = "";
            if ($('#optPVDtransdebit').is(':checked'))
                dcnote = "D";
            else if ($('#optPVDtranscredit').is(':checked'))
                dcnote = "C";

            var submitURL = (PVId != undefined && PVId != "") ? (pvDetailEdit == 0) ? base_url + "CashAdvance/InsertDetail" : base_url + "CashAdvance/UpdateDetail" : "";
            var isValid = true;
            var message = "OK";
            SavePVDetail(submitURL, pvDetailId, $('#txtPVDrefshipmentno').val(), $('#txtPVDaccountcode').data('kode'), $('#txtPVDaccountdesc').val(), dcnote,
                $('#txtPVDamountpaidusd').numberbox('getValue'), $('#txtPVDamountpaididr').numberbox('getValue'), cashpayment, $('#txtPVDrefno').data('refid'),
                $('#txtPVDrefno').data('refdetailid'), $('#txtPVDrefaccountid').val(), pvbank,
                $('#txtPVDrefrate').numberbox('getValue'), $('#txtPVDrefrate').data('kode'), $('#txtPVDrefshipmentno').data('kode'), function (result) {
                    if (JSON.stringify(result.Errors) != '{}') {
                        for (var key in result.Errors) {
                            if (key != null && key != undefined && key != 'Generic') {
                                $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            }
                            else {
                                $.messager.alert('Warning', result.Errors[key], 'warning');
                            }
                        }
                    }
                    else {
                        AddPaymentDetail(pvDetailEdit, pvDetailId, $('#txtPVDaccountcode').data('kode'), $('#txtPVDaccountcode').val(), $('#txtPVDaccountname').val(),
                        $('#txtPVDaccountdesc').val(), dcnote, $('#txtPVDamountpaidusd').numberbox('getValue'), $('#txtPVDamountpaididr').numberbox('getValue'),
                        $('#txtPVDrefno').val(), $('#txtPVDrefshipmentno').val(), $('#txtPVDrefrate').numberbox('getValue'), $('#txtPVDrefrate').data('kode'),
                        $('#txtPVDrefshipmentno').data('kode'), $('#txtPVDamountusd').numberbox('getValue'), $('#txtPVDamountidr').numberbox('getValue'),
                        cashpayment, $('#txtPVDrefaccountid').val(), $('#txtPVDrefno').data('refid'), $('#txtPVDrefno').data('refdetailid'), pvbank);

                        CalculatePayment();

                        ClearPaymentDetailForm();

                        $('#lookup_div_payment_voucher_detail').dialog('close');
                    }
                });


        
        }
    });

    // Save PV Detail
    function SavePVDetail(submitURL, v_pvDetailId, v_shipmentNo, v_accountId, v_desc, v_dcnote, v_amountUsd, v_amountIdr,
                                        v_cashPayment, v_refId, v_refDetailId, v_refAccountId, v_pvBank, v_rate, v_rateId, v_shipmentId, callback) {
        if (submitURL != undefined && submitURL != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    CashAdvanceId: PVId, Id: v_pvDetailId,
                    ShipmentNo: v_shipmentNo, PayableId: v_refDetailId, Description: v_desc, DCNote: v_dcnote,
                    AmountUSD: v_amountUsd, AmountIDR: v_amountIdr, PaymentCash: v_cashPayment,
                    RefId: v_refId, RefDetailId: v_refDetailId, RefAccountId: v_refAccountId,
                    PVBank: v_pvBank, RefRate: v_rate, ExRateId: v_rateId, ShipmentOrderId: v_shipmentId
                }),
                async: false,
                cache: false,
                timeout: 30000,
                error: function () {
                    return false;
                },
                success: function (result) {
                    callback(result);
                }
            });
        }
    }
    // END Save PV Detail


    // Save Payment Cash
    $('#btn_save_cash').click(function () {
        var id = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', id);

            var amount = 0;
            if ($('#rbPaymentByCashIDR').is('checked')) {
                if (isNaN(ret.amountidr))
                    amount = ret.amountidr;
            }
            else if ($('#rbPaymentByCashUSD').is('checked')) {
                if (isNaN(ret.amountusd))
                    amount = ret.amountusd;
            }

            var cashpayment = $('#txtPVBankcashpayment').numberbox('getValue');

            //if (cashpayment != amount) {
            //    $.messager.confirm('Confirm', 'Invalid Amount.. Are you sure to process?', function (r) {
            //        if (r) {

            var submitURL = (PVId != undefined && PVId != "") ? base_url + "CashAdvance/UpdatePVDetail" : "";
            var isValid = true;
            var message = "OK";
            SavePVDetail(submitURL, id, ret.shipment, ret.accountid, ret.description, ret.dcnote, ret.amountusd, ret.amountidr, cashpayment, ret.refid, ret.refdetailid,
                ret.refaccountid, ret.pvbank, ret.rate, ret.exrateid, ret.shipmentid, function (data) {
                    isValid = data.isValid;
                    message = data.message;
                });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
                return;
            }

            AddPaymentDetail(1, id, ret.accountid, ret.accountcode, ret.accountname, ret.description, ret.dcnote, ret.amountusd, ret.amountidr, ret.refno,
                ret.shipment, ret.rate, ret.exrateid, ret.shipmentid, ret.differenceusd, ret.differenceidr, cashpayment,
                ret.refaccountid, ret.refid, ret.refdetailid, ret.pvbank);

            $('#txtPVBankcashpayment').numberbox('setValue', 0);

            $("#table_pv_bank").jqGrid("clearGridData", true).trigger("reloadGrid");

            CalculatePayment();
            //        }
            //    });
            //}

        } else {
            $.messager.alert('Information', 'Please Select Payment Voucher Detail First...!!', 'info');
        };
    });

    


    // Delete PV Detail
    function DeletePVDetail(v_pvDetailId, callback) {
        if (PVId != undefined && PVId != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + 'CashAdvance/DeleteDetail',
                data: JSON.stringify({
                    Id: v_pvDetailId
                }),
                async: false,
                cache: false,
                timeout: 30000,
                error: function () {
                    return false;
                },
                success: function (result) {
                    callback(result);
                }
            });
        }
    }
    // END Delete PV Detail

    // Payment Voucher Detail
    function PopulateCashAdvanceDetail() {
        var pvDetail = [];
        var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['PVId'] = PVId;
            obj['AccountId'] = data[i].accountid;
            obj['Description'] = data[i].description;
            obj['DCNote'] = data[i].dcnote;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['PaymentCash'] = data[i].cashpayment;
            obj['RefId'] = data[i].refid;
            obj['RefAccountId'] = data[i].refaccountid;
            obj['RefDetailId'] = data[i].refdetailid;
            obj['ShipmentOrderId'] = data[i].shipmentid;
            obj['ExRateId'] = data[i].exrateid;
            obj['PVBank'] = data[i].pvbank;
            pvDetail.push(obj);
        }

        return pvDetail
    }
    // END Payment Voucher Detail

    // ------------------------------------------------------ END Payment Detail ------------------------------------

    // ------------------------------------------------------ Payment Detail Bank ------------------------------------

    // ADD
    $('#btn_add_pv_bank').click(function () {
        var pvDetailId = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
        if (!pvDetailId) {
            $.messager.alert('Information', 'Please Select Detail First...!!', 'info');
            return;
        }
        else {
            // ONLY on ADD PV Detail
            if (pvDetailId) {
                var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', pvDetailId);
                if (pvDetailEdit == 0) {
                    if ($('#rbPaymentByBankUSD').is(":checked") || $('#rbPaymentByBankIDR').is(":checked")) {
                        $('#txtPVBankremarks').val(ret.description);
                    }
                }
            }
        }

        // Bank IDR
        if ($('#rbPaymentByBankIDR').is(':checked')) {
            $('#optPVBankamountidr').attr('checked', 'checked');
        }
            /// Bank USD
        else if ($('#rbPaymentByBankUSD').is(':checked')) {
            $('#optPVBankamountusd').attr('checked', 'checked');
        }

        $('#lookup_div_payment_voucher_detail_bank').dialog("open");
    })

    // EDIT
    $('#btn_edit_pv_bank').click(function () {
        var id = jQuery("#table_pv_bank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#table_pv_bank").jqGrid('getRowData', id);

            pvBankEdit = 1;
            // Assign ID
            $('#lookup_btn_add_payment_voucher_detail_bank').data('pvbankid', id);

            $('#txtPVBankaccountcode').val(ret.accountcode).data('kode', ret.accountid);
            $('#txtPVBankaccountname').val(ret.accountname);
            $('#txtPVBankbankcode').val(ret.bankcode);
            $('#txtPVBankduedate').datebox('setValue', ret.duedate).data('statusduedate', ret.statusduedate);
            $('#txtPVBankamount').numberbox('setValue', ret.amount);
            $('#txtPVBankremarks').val(ret.remarks);

            $('#optPVBankamountusd').attr('checked', 'checked');
            if (ret.amountcrr == 1)
                $('#optPVBankamountidr').attr('checked', 'checked');


            $('#lookup_div_payment_voucher_detail_bank').dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    })

    // Close
    $('#lookup_btn_cancel_payment_voucher_detail_bank').click(function () {

        $('#lookup_div_payment_voucher_detail_bank').dialog('close');
    });

    // Delete
    $('#btn_delete_pv_bank').click(function () {
        var id = jQuery("#table_pv_bank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#table_pv_bank").jqGrid('getRowData', id);

            var isValid = true;
            var message = "OK";

            var pvDetailId = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
            if (!pvDetailId) {
                $.messager.alert('Information', 'Please Select Detail First...!!', 'info');
                return;
            }

            DeletePVBank(id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Delete
                jQuery("#table_pv_bank").jqGrid('delRowData', id);
                jQuery("#table_pv_bank").trigger("reloadGrid");

                // Repopulate
                var pvBank = [];
                var data = $("#table_pv_bank").jqGrid('getGridParam', 'data');
                for (var i = 0; i < data.length; i++) {
                    var obj = {};
                    obj['Id'] = data[i].id;
                    obj['PVId'] = (PVId == undefined || PVId == "") ? 0 : PVId;
                    obj['PVDetailId'] = data[i].pvdetailid;
                    obj['AccountCode'] = data[i].accountcode;
                    obj['AccountName'] = data[i].accountname;
                    obj['BankCode'] = data[i].bankcode;
                    obj['DueDate'] = (data[i].duedate == "&#160;") ? "" : data[i].duedate;
                    obj['AmountCrr'] = data[i].amountcrr;
                    obj['Amount'] = data[i].amount;
                    obj['Remarks'] = data[i].remarks;
                    obj['StatusDueDate'] = (data[i].statusduedate == "&#160;") ? false : data[i].statusduedate;
                    obj['AccountId'] = (data[i].accountid == undefined || data[i].accountid == "") ? 0 : data[i].accountid;
                    pvBank.push(obj);
                }

                var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', pvDetailId);
                AddPaymentDetail(1, pvDetailId, ret.accountid, ret.accountcode, ret.accountname, ret.description, ret.dcnote, ret.amountusd, ret.amountidr, ret.refno,
                                  ret.shipment, ret.rate, ret.exrateid, ret.shipmentid, ret.differenceusd, ret.differenceidr, ret.cashpayment, ret.refaccountid,
                                  ret.refid, ret.refdetailid, JSON.stringify(pvBank));

                CalculatePayment();
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // Save
    $('#lookup_btn_add_payment_voucher_detail_bank').click(function () {

        var tempDueDate = "";
        if ($.trim($('#txtPVBankduedate').datebox('getValue')) != "") {
            tempDueDate = $('#txtPVBankduedate').datebox('getValue');
        }

        var statusDueDate = false;
        if ($('#txtPVBankduedate').data('statusduedate') != undefined)
            statusDueDate = $('#txtPVBankduedate').data('statusduedate');

        var amountCrr = 0
        if ($('#optPVBankamountidr').is(':checked'))
            amountCrr = 1;

        var pvDetailId = jQuery("#table_payment_voucher_detail").jqGrid('getGridParam', 'selrow');
        if (!pvDetailId) {
            $.messager.alert('Information', 'Please Select Detail First...!!', 'info');
            return;
        }
        else if ($('#txtPVBankbankcode').val() == undefined || $('#txtPVBankbankcode').val() == '' || $('#txtPVBankbankcode').val() == null) {
            $.messager.alert('Warning', 'Check BG cannot be empty...!!', 'warning');
            return;
        }
        else if ($('#txtPVBankremarks').val() == undefined || $('#txtPVBankremarks').val() == '' || $('#txtPVBankremarks').val() == null) {
            $.messager.alert('Warning', 'Remarks cannot be empty...!!', 'warning');
            return;
        }

        var pvBankId = 0;
        if (pvBankEdit == 1) {
            pvBankId = $('#lookup_btn_add_payment_voucher_detail_bank').data('pvbankid');
        }

        var submitURL = (PVId != undefined && PVId != "") ? (pvBankEdit == 0) ? base_url + "CashAdvance/InsertPVBank" : base_url + "CashAdvance/UpdatePVBank" : "";
        var isValid = true;
        var message = "OK";
        SavePVBank(submitURL, pvBankId, pvDetailId, $('#txtPVBankbankcode').val(), amountCrr, $('#txtPVBankremarks').val(), $('#txtPVBankamount').numberbox('getValue'),
                    tempDueDate, //$('#txtPVBankduedate').datebox('getValue'),
                    statusDueDate, $('#txtPVBankaccountcode').data('kode'), function (data) {
                        isValid = data.isValid;
                        message = data.message;
                        if (data.objResult != null)
                            pvBankId = data.objResult.Id;
                    });

        if (!isValid) {
            $.messager.alert('Warning', message, 'warning');
            return;
        }

        AddPVBank(pvBankEdit, pvBankId, pvDetailId, $('#txtPVBankaccountcode').data('kode'), $('#txtPVBankaccountcode').val(), $('#txtPVBankaccountname').val(),
            $('#txtPVBankbankcode').val(), tempDueDate, //$('#txtPVBankduedate').datebox('getValue'),
            amountCrr, $('#txtPVBankamount').numberbox('getValue'), $('#txtPVBankremarks').val(), statusDueDate);

        var pvBank = [];
        var data = $("#table_pv_bank").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['Id'] = data[i].id;
            obj['PVId'] = (PVId == undefined || PVId == "") ? 0 : PVId;
            obj['PVDetailId'] = data[i].pvdetailid;
            obj['AccountCode'] = data[i].accountcode;
            obj['AccountName'] = data[i].accountname;
            obj['BankCode'] = data[i].bankcode;
            obj['DueDate'] = (data[i].duedate == "&#160;") ? "" : data[i].duedate;
            obj['AmountCrr'] = data[i].amountcrr;
            obj['Amount'] = data[i].amount;
            obj['Remarks'] = data[i].remarks;
            obj['StatusDueDate'] = (data[i].statusduedate == "&#160;") ? false : data[i].statusduedate;
            obj['AccountId'] = (data[i].accountid == undefined || data[i].accountid == "") ? 0 : data[i].accountid;
            pvBank.push(obj);
        }

        var ret = jQuery("#table_payment_voucher_detail").jqGrid('getRowData', pvDetailId);
        AddPaymentDetail(1, pvDetailId, ret.accountid, ret.accountcode, ret.accountname, ret.description, ret.dcnote, ret.amountusd, ret.amountidr, ret.refno, ret.shipment,
                            ret.rate, ret.exrateid, ret.shipmentid, ret.differenceusd, ret.differenceidr, ret.cashpayment, ret.refaccountid,
                            ret.refid, ret.refdetailid, JSON.stringify(pvBank));

        CalculatePayment();

        ClearPVBankForm();

        $("#table_pv_bank").jqGrid("clearGridData", true);

        $('#lookup_div_payment_voucher_detail_bank').dialog('close');
    });



   
    function AddPVBank(isEdit, pvBankId, pvDetailId, accountId, accountCode, accountName, bankcode, duedate, amountcrr, amount, remarks, statusduedate) {

        var isValid = true;

        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
        else if (remarks == null || remarks == undefined || remarks == '') {
            $.messager.alert('Information', 'Description cannot be empty...!!', 'info');
        }
            // End Validate
        else {
            if (accountId == undefined || accountId == "")
                accountId = 0;

            var newData = {}
            newData.pvdetailid = pvDetailId;
            newData.accountid = accountId;
            newData.accountcode = accountCode;
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.remarks = (remarks != undefined && remarks != "") ? remarks.toUpperCase() : "";
            newData.bankcode = (bankcode != undefined && bankcode != "") ? bankcode.toUpperCase() : "";
            newData.amountcrr = amountcrr;
            newData.amount = amount;
            newData.duedate = duedate;
            newData.statusduedate = statusduedate;

            // New Record
            if (isEdit == 0) {
                pvBankId = pvBankId == 0 ? GetNextId($("#table_pv_bank")) : pvBankId;
                jQuery("#table_pv_bank").jqGrid('addRowData', pvBankId, newData);
            }
            else {
                var id = jQuery("#table_pv_bank").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#table_pv_bank").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#table_pv_bank").trigger("reloadGrid");

            CalculatePayment();
        }
    }

    jQuery("#table_pv_bank").jqGrid({
        datatype: "local",
        height: 80,
        width: 540,
        rowNum: 150,
        scrollrows: true,
        shrinkToFit: false,
        colNames: ['Code', 'Account Title', 'BG No', 'Amount Crr', 'Amount', 'Due Date', 'Description', 'Status Due Date', 'pvdetailid', 'accountid'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 100, align: 'right' },
            { name: 'accountname', index: 'accountname', width: 150, align: 'right', hidden: true },
            { name: 'bankcode', index: 'bankcode', width: 100 },
            { name: 'amountcrr', index: 'amountcrr', width: 100, hidden: true },
            { name: 'amount', index: 'amount', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'duedate', index: 'duedate', width: 80, align: 'right', formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "Y-m-d" } },
            { name: 'remarks', index: 'remarks', width: 150 },
            { name: 'statusduedate', index: 'statusduedate', width: 100, hidden: false, align: "center" },
            { name: 'pvdetailid', index: 'pvdetailid', width: 80, hidden: true },
            { name: 'accountid', index: 'accountid', width: 80, hidden: true }
        ],
        gridComplete:
          function () {
              var ids = $(this).jqGrid('getDataIDs');
              for (var i = 0; i < ids.length; i++) {
                  var cl = ids[i];
                  $(this).setCell(cl, 'accountcode', '', '', { 'title': $(this).getRowData(cl).accountname });

                  //var statusDueDate = "";
                  //if ($(this).getRowData(cl).statusduedate == 'OK' || $(this).getRowData(cl).statusduedate == 'true' || $(this).getRowData(cl).statusduedate == true) {
                  //    statusDueDate = "OK";
                  //} else {
                  //    statusDueDate = "";
                  //}
                  //$(this).jqGrid('setRowData', ids[i], { statusduedate: statusDueDate });

              }
          }
    });

    // Save PV Bank
    function SavePVBank(submitURL, v_pvBankId, v_pvDetailId, v_bankCode, v_amountCrr, v_remarks, v_amount, v_duedate,
                                        v_statusDueDate, v_accountId, callback) {
        if (submitURL != undefined && submitURL != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    PVId: PVId, Id: v_pvBankId,
                    PVDetailId: v_pvDetailId, BankCode: v_bankCode, AmountCrr: v_amountCrr,
                    Remarks: v_remarks, Amount: v_amount, DueDate: v_duedate, StatusDueDate: v_statusDueDate, AccountId: v_accountId
                }),
                async: false,
                cache: false,
                timeout: 30000,
                error: function () {
                    return false;
                },
                success: function (result) {
                    callback(result);
                }
            });
        }
    }
    // END Save PV Bank

    // Delete PV Bank
    function DeletePVBank(v_pvBankId, callback) {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                if (PVId != undefined && PVId != "") {
                    $.ajax({
                        contentType: "application/json",
                        type: 'POST',
                        url: base_url + 'CashAdvance/DeletePVBank',
                        data: JSON.stringify({
                            Id: v_pvBankId
                        }),
                        async: false,
                        cache: false,
                        timeout: 30000,
                        error: function () {
                            return false;
                        },
                        success: function (result) {
                            callback(result);
                        }
                    });
                }
            }
        });
    }
    // END Delete PV Bank

    function ClearPVBankForm() {
        pvBankEdit = 0;
        $('#txtPVBankaccountcode').val('').data('kode', '');
        $('#txtPVBankaccountname').val('');

        // IF Payment By BANK
        if ($('#rbPaymentByBankIDR').is(':checked') || $('#rbPaymentByBankUSD').is(':checked')) {
            $('#txtPVBankaccountcode').val($('#txtPVBankaccountcode').data('accountcode')).data('kode', $('#txtPVBankaccountcode').data('accountid'));
            $('#txtPVBankaccountname').val($('#txtPVBankaccountcode').data('accounttitle'));
        }

        $('#txtPVBankbankcode').val(''),
        //$('#txtORBankduedate').datebox('setValue', ),
        $('#txtPVBankamount').numberbox('setValue', 0);
        $('#txtPVBankremarks').val('');
        $('#lookup_btn_add_payment_voucher_detail_bank').data('pvbankid', 0);
    }

    // ------------------------------------------------------ END Payment Detail Bank ------------------------------------

    // ---------------------------------------------------- Chart Of Account (COA) --------------------------------------------------------------------------

    // Browse
    $('#btnPVDaccountcode').click(function () {

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'MstCOA/LookUpCOATransactionDetail';
        var lookupGrid = $('#lookup_table_coa');
        lookupGrid.setGridParam({
            url: lookUpURL,
            postData: { filters: null }
        }).trigger("reloadGrid");

        $('#lookup_div_coa').dialog('open');
    });

    // Close
    $('#lookup_btn_cancel_coa').click(function () {
        $('#lookup_div_coa').dialog('close');
    });

    // Save
    $('#lookup_btn_add_coa').click(function () {
        var id = jQuery("#lookup_table_coa").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_coa").jqGrid('getRowData', id);

            $('#txtPVDaccountcode').val(ret.accountcode).data('kode', id);
            $('#txtPVDaccountname').val(ret.accounttitle);

            $('#lookup_div_coa').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_coa").jqGrid({
        height: 350,
        width: 450,
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Account Code', 'AccountName'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120, align: 'right' },
            { name: 'accounttitle', index: 'accounttitle', width: 300 }
        ],
        pager: jQuery('#lookup_pager_coa'),
        sortname: 'accountcode',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false
    });
    $("#lookup_table_coa").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_coa").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_coa').click();
        },
        scrollingRows: true
    });
    // ---------------------------------------------------- END Chart Of Account (COA) --------------------------------------------------------------------------

    // ---------------------------------------------------- Chart Of Account (COA) - Bank --------------------------------------------------------------------------

    // Browse
    $('#btnPVBankaccountcode').click(function () {

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'mstcoa/LookupCOA?accLevel=5'; //base_url + 'MstCOA/LookUpCOABank';
        var lookupGrid = $('#lookup_table_coa_bank');
        lookupGrid.setGridParam({
            url: lookUpURL,
            postData: { filters: null }
        }).trigger("reloadGrid");

        $('#lookup_div_coa_bank').dialog('open');
    });

    // Close
    $('#lookup_btn_cancel_coa_bank').click(function () {
        $('#lookup_div_coa_bank').dialog('close');
    });

    // Save
    $('#lookup_btn_add_coa_bank').click(function () {
        var id = jQuery("#lookup_table_coa_bank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_coa_bank").jqGrid('getRowData', id);

            $('#txtPVBankaccountcode').val(ret.accountcode).data('kode', id);
            $('#txtPVBankaccountname').val(ret.accounttitle);

            $('#lookup_div_coa_bank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_coa_bank").jqGrid({
        height: 350,
        width: 450,
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Account Code', 'AccountName'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120, align: 'right' },
            { name: 'accounttitle', index: 'accounttitle', width: 300 }
        ],
        pager: jQuery('#lookup_pager_coa_bank'),
        sortname: 'accountcode',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false
    });
    $("#lookup_table_coa_bank").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_coa_bank").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_coa_bank').click();
        },
        scrollingRows: true
    });
    // ---------------------------------------------------- END Chart Of Account (COA) - Bank --------------------------------------------------------------------------

    // ---------------------------------------------------- Chart Of Account (COA) - Different --------------------------------------------------------------------------

    // Browse
    $('#btnPVdifferentaccountcode').click(function () {

        var different = $("#txtPVTotalBalanced").numberbox("getValue");
        if (different == 0) {
            return;
        }

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'mstcoa/LookupCOA?accLevel=5'; //base_url + 'MstCOA/LookUpCOABank';
        var lookupGrid = $('#lookup_table_coa_different');
        lookupGrid.setGridParam({
            url: lookUpURL,
            postData: { filters: null }
        }).trigger("reloadGrid");

        $('#lookup_div_coa_different').dialog('open');
    });

    // Close
    $('#lookup_btn_cancel_coa_different').click(function () {
        $('#lookup_div_coa_different').dialog('close');
    });

    // Save
    $('#lookup_btn_add_coa_different').click(function () {
        var id = jQuery("#lookup_table_coa_different").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_coa_different").jqGrid('getRowData', id);

            $('#txtPVdifferentaccountcode').val(ret.accountcode).data('kode', id);
            $('#txtPVdifferentaccountname').val(ret.accounttitle);

            $('#lookup_div_coa_different').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_coa_different").jqGrid({
        height: 350,
        width: 450,
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Account Code', 'AccountName'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120, align: 'right' },
            { name: 'accounttitle', index: 'accounttitle', width: 300 }
        ],
        pager: jQuery('#lookup_pager_coa_bank'),
        sortname: 'accountcode',
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false
    });
    $("#lookup_table_coa_different").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_coa_different").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_coa_different').click();
        },
        scrollingRows: true
    });
    // ---------------------------------------------------- END Chart Of Account (COA) - Different --------------------------------------------------------------------------

    function CalculatePayment() {
        var cashpayment = 0;
        var debitIDR = 0;
        var creditIDR = 0;
        var debitUSD = 0;
        var creditUSD = 0;
        var debitInIDR = 0;
        var creditInIDR = 0;
        var balanced = 0;
        var data = $("#table_payment_voucher_detail").jqGrid('getGridParam', 'data');

        var rateDetail = 0;
        var rateMaster = $('#txtPVrate').numberbox('getValue');

        for (var i = 0; i < data.length; i++) {
           
                debitUSD += parseFloat(data[i].amountusd);
                debitIDR += parseFloat(data[i].amountidr);

                if (isNaN(debitIDR))
                    debitIDR = 0;

                if (isNaN(data[i].rate))
                    rateDetail = rateMaster;
                else
                    rateDetail = data[i].rate;

            debitInIDR += parseFloat(parseFloat(parseFloat(data[i].amountusd) * parseFloat(rateDetail)) + parseFloat(data[i].amountidr));
            if (data[i].dcnote == 'C') {
                creditUSD += parseFloat(data[i].amountusd);
                creditIDR += parseFloat(data[i].amountidr);

                if (isNaN(creditIDR))
                    creditIDR = 0;

                creditInIDR += parseFloat(parseFloat(parseFloat(data[i].amountusd) * parseFloat(rateDetail)) + parseFloat(data[i].amountidr));
            }

            // Payment As ALWAYS Credit
            if ($('#rbPaymentByBankUSD').is(':checked') || $('#rbPaymentByCashUSD').is(':checked')) {
                creditUSD += parseFloat(data[i].cashpayment);
                creditInIDR += parseFloat(parseFloat(data[i].cashpayment) * parseFloat(rateDetail));
            }
            else if ($('#rbPaymentByBankIDR').is(':checked') || $('#rbPaymentByCashIDR').is(':checked')) {
                creditIDR += parseFloat(data[i].cashpayment);
                creditInIDR += parseFloat(data[i].cashpayment);

            }

            // Payment As ALWAYS Credit
            // Bank
            var pvbank = $.parseJSON(data[i].pvbank);
            if (pvbank != null) {
                for (var x = 0; x < pvbank.length; x++) {
                    // Bank IDR
                    if ($('#rbPaymentByBankIDR').is(':checked')) {
                        if (isNaN(pvbank[x].Amount) == false) {
                            creditIDR += parseFloat(pvbank[x].Amount);
                            creditInIDR += parseFloat(pvbank[x].Amount);

                        }
                    }
                        // Bank USD
                    else if ($('#rbPaymentByBankUSD').is(':checked')) {
                        if (isNaN(pvbank[x].Amount) == false) {
                            creditUSD += parseFloat(pvbank[x].Amount);
                            creditInIDR += parseFloat(parseFloat(pvbank[x].Amount) * parseFloat(rateDetail));
                        }
                    }
                }
            }
        }

        $("#txtPVTotalDebitIdr").numberbox("setValue", debitIDR);
        $("#txtPVTotalDebitUsd").numberbox("setValue", debitUSD);

        $("#txtPVTotalCreditIdr").numberbox("setValue", creditIDR);
        $("#txtPVTotalCreditUsd").numberbox("setValue", creditUSD);


        //var rate = $('#txtPVrate').numberbox('getValue');

        //debitInIDR = parseFloat(rate * debitUSD) + debitIDR;
        //creditInIDR = parseFloat(rate * creditUSD) + creditIDR;
        balanced = debitInIDR - creditInIDR;
        balanced = Math.round(balanced * 100) / 100;

        if (balanced > 0) {
            $('#lblPVTotalBalanced').text('(Credit)');
            $('#lblPVTotalBalancedExchange').text('Exchange Rate Difference');
        }
        else if (balanced < 0) {
            $('#lblPVTotalBalanced').text('(Debit)');
            $('#lblPVTotalBalancedExchange').text('Exchange Rate Difference');
        }
        else {
            $('#lblPVTotalBalanced').text('');
            $('#lblPVTotalBalancedExchange').text('Balanced');
        }


        if (balanced != 0) {
            $('#btnPVdifferentaccountcode').removeAttr('disabled').addClass('ui-state-default').removeClass('ui-state-disabled');
        }
        else {
            $('#btnPVdifferentaccountcode').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
        }

        balanced = Math.abs(balanced);
        $('#txtPVTotalDebitInIdr').numberbox("setValue", debitInIDR);
        $('#txtPVTotalCreditInIdr').numberbox("setValue", creditInIDR);
        $('#txtPVTotalBalanced').numberbox("setValue", balanced);

    }

    function GetNextId(objJqGrid) {
        var nextid = 0;
        if (objJqGrid.getDataIDs().length > 0)
            nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

        return nextid;
    }


    // ---------------------------------------------------------------------- SAVE
    $('#paymentvoucher_form_btn_save').click(function () {
            SaveAll();
    });

    function SaveAll() {

        var submitURL = base_url + "CashAdvance/Insert";
        if (PVId != undefined && PVId != 0)
            submitURL = base_url + "CashAdvance/Update";

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: PVId,
                EmployeeId: $('#txtPVcustomer').data('kode'),
            }),
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    window.location = base_url + "CashAdvance/Detail?Id=" + result.cashAdvanceId;
                }
            }
        });
    }

    // Payment Voucher Detail Bank
    function PopulateCashAdvanceDetailBank() {
        var pvDetailBank = [];
        var data = $("#table_payment_voucher_detail_bank").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            if (data[i].currencytype == 1)
                obj['Amount'] = data[i].amountidr;
            else
                obj['Amount'] = data[i].amountusd;
            pvDetailBank.push(obj);
        }

        return pvDetailBank
    }
    // END Payment Voucher Detail Bank

    // Print Payment Voucher
    $('#paymentvoucher_form_btn_print').click(function () {
        if (PVId == null || PVId == undefined || PVId == '') {
            $.messager.alert('Warning', 'Invalid PV No.', 'warning');
            return;
        }
        $.ajax({
            dataType: "json",
            url: base_url + "paymentvoucher/GetCashAdvanceInfo?Id=" + PVId,
            success: function (result) {
                if (result.isValid) {
                    if (dateEnt(result.objJob.PrintedOn) != '') {
                        $('#txtdialogdateprint').datebox('setValue', dateEnt(result.objJob.PrintedOn));
                    }
                    $('#txtdialogdateprint').datebox({
                        disabled: false
                    });
                    if (result.objJob.Printing > 0) {
                        $('#txtdialogdateprint').datebox({
                            disabled: true
                        });
                    }
                    $('#dialogPrint').dialog('open');
                }
            }
        });
    });

    $('#btnDialogPrintYes').click(function () {
        var dateprint = $('#txtdialogdateprint').datebox('getValue');
        $('#dialogPrint').dialog('close');
        window.open(base_url + "CashAdvance/Print?Id=" + PVId + "&PrintedOn=" + dateprint);
    });

    $('#btnDialogPrintNo').click(function () {
        $('#dialogPrint').dialog('close');
    });
    // END PRINT Payment Voucher

    // Add New Payment Voucher
    $('#paymentvoucher_btn_add_new').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to create New Payment Voucher?', function (r) {
            if (r) {
                //$.ajax({
                //    dataType: "json",
                //    url: base_url + "CashAdvance/IsAllowNew",
                //    success: function (result) {
                //        if (!result.isValid) {
                //            $.messager.alert('Information', result.message, 'info');
                //        }
                //        else {
                            window.location = base_url + "CashAdvance/Detail";
                //        }
                //    }
                //});
            }
        });
    });
    // END Add New Payment Voucher
});