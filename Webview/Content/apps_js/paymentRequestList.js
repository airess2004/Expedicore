﻿$(document).ready(function () {

    // Initialize
    $('#dialogEdit').dialog('close');
    $('#dialogDelete').dialog('close');
    $('#dialogPrint').dialog('close');
    $('#dialogPrintFixedDraft').dialog('close');

    // First Load
    // tools.js
    var type = getQueryStringByName('type');
    if (type == undefined || type == "") {
        type = '1';
    }
    $('#jobtype').val(type);
    //setTimeout(ReloadGrid(), 5000);


    function ReloadGrid() {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();

        $("#list_invoice").setGridParam({ url: base_url + 'PaymentRequest/GetList', postData: { filters: null, jobId: value }, page: 'last' }).trigger("reloadGrid");
    }

    /*================================================ List ================================================*/
    jQuery("#list_invoice").jqGrid({
        url: base_url + 'PaymentRequest/GetList',
        postData: { 'jobId': function () { return $("#jobtype").val(); } },
        datatype: "json",
        colNames: ['Del', 'Confirmed','Type', 'PR No', 'Paid', 'Shipment No', 'Status', 'D/C', 'Cancel D/C Number',
				'Customer', 'Amount USD', 'Amount IDR', 'Print Date', 'Rate', 'Date Rate', 'Entry Date', 'Prepared By', 'Print', 'Date Deleted',
                'Date Paid',  '', '', '', '', 'ie', 'bd', 'so'],
        colModel: [{ name: 'deleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
                  { name: 'confirmed', index: 'IsConfirmed', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } , formatter : 'select'},
				  { name: 'type', index: 'JenisPaymentRequest', width: 80, align: "center", stype: 'select', editoptions: { value: ":All;G:General;PR:PR" }, formatter: 'select' },
				  { name: 'invoicesno', index: 'invoicesno', width: 80, align: "center" },
				  { name: 'paid', index: 'Paid', width: 50, align: "center", stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
				  { name: 'shipmentno', index: 'shipmentno', width: 130, align: "center" },
				  { name: 'job', index: 'invoicestatus', width: 50, align: "center" },
				  { name: 'debetcredit', index: 'debetcredit', width: 50, align: "center" },
				  { name: 'canceldcnumber', index: 'linkto', width: 150 },
				  { name: 'customer', index: 'customer', width: 200 },
				  { name: 'amountusd', index: 'amountusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'amountidr', index: 'amountidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'dateprint', index: 'dateprint', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'rate', index: 'rate', width: 80, align: "center", align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'daterate', index: 'daterate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'entrydate', index: 'createdon', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'usercode', index: 'name', width: 100 },
				  { name: 'printing', index: 'printing', width: 100, align: 'right' },
				  { name: 'datedeleted', index: 'datedeleted', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'datepaid', index: 'datepaid', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'debetcredit', index: 'debetcredit', width: 100, hidden: true },
				  { name: 'isallowaddnew', index: 'isallowaddnew', width: 100, hidden: true, sortable: false },
				  { name: 'companycode', index: 'companycode', width: 100, hidden: true, sortable: false },
				  { name: 'intcompany', index: 'intcompany', width: 100, hidden: true, sortable: false },
				  { name: 'invoicesedit', index: 'invoicesedit', width: 100, hidden: true, sortable: false },
				  { name: 'baddebt', index: 'baddebt', width: 100, hidden: true, sortable: false },
				  { name: 'saveor', index: 'saveor', width: 100, hidden: true, sortable: false }
        ],
        page: 'last', // last page
        pager: jQuery('#pager_list_invoice'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        viewrecords: true,
        shrinkToFit: false,
        sortname: "id",
        sortorder: "asc",
        width: $("#invoice_toolbar").width(),
        height: $(window).height() - 120,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowDel = $(this).getRowData(cl).deleted;
		          if (rowDel == 'true') {
		              img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		          } else {
		              img = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { deleted: img });

		          rowPaid = $(this).getRowData(cl).paid;
		          if (rowPaid == 'true') {
		              rowPaid = "Yes";
		          }
		          else {
		              rowPaid = "No";
		          }
		          $(this).jqGrid('setRowData', ids[i], { paid: rowPaid });
		      }
		  }
    });//END GRID

    $("#list_invoice").jqGrid('navGrid', '#toolbar_trans_paymentrequest', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    /*================================================ End List ================================================*/

    $("#invoice_btn_reload").click(function () {
        ReloadGrid();
    });

    $("#invoice_btn_add_new").click(function () {

        var jobText = $('#jobtype option:selected').text();
        var jobValue = $('#jobtype').val();
        var invoice = null;
        //$.ajax({
        //    dataType: "json",
        //    url: base_url + "PaymentRequest/AllowNewPaymentRequest",
        //    success: function (result) {
        //        if (!result.isValid) {
        //            $.messager.alert('Information', result.message, 'info');
        //        }
        //        else {
                    //invoice = result.objJob;
                    window.location = base_url + "PaymentRequest/detail?JobId=" + jobValue;
                //}
            //}
        //});
    });

    $("#jobtype").live("change", function () {
        ReloadGrid();
    });


    $("#invoice_btn_edit, #invoice_btn_delete, #invoice_btn_print").click(function () {
        var buttonID = $(this).attr('id');
        var jobText = $('#jobtype option:selected').text();
        var jobValue = $('#jobtype').val();

        var id = jQuery("#list_invoice").jqGrid('getGridParam', 'selrow');
        if (id) {

            //var ret = jQuery("#list_invoice").jqGrid('getRowData', id);
            var invoice = null;
            $.ajax({
                dataType: "json",
                url: base_url + "PaymentRequest/GetInfo?Id=" + id + "&JobId=" + jobValue,
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
                    //if (!result.isValid)
                    //    $.messager.alert('Information', result.message, 'info', function () {
                    //        window.location = base_url + "PaymentRequest";
                    //    });
                    //if (result.objJob == null)
                    //    $.messager.alert('Information', "Object is null", 'info', function () {
                    //        window.location = base_url + "PaymentRequest";
                    //    });
                    else {
                        invoice = result;

                        // Except Cancellation Printing
                        if (invoice.Deleted == true && !(buttonID == "invoice_btn_print" && invoice.LinkTo.substr(0, 6).toLowerCase() == 'cancel')) {
                            $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                            return;
                        }

                        // Has been Paid
                        if (invoice.Paid == true && buttonID != "invoice_btn_print") {
                            $.messager.alert('Information', 'PaymentRequests has been Paid !', 'info');
                        }
                            // Has been Official Receipt (OR)
                        else if (invoice.SaveOR == true && buttonID != "invoice_btn_print") {
                            $.messager.alert('Information', 'PaymentRequests has been used by Official Receipt !', 'info', function () {
                                window.location = base_url + "PaymentRequest/detail?Id=" + id + "&JobId=" + jobValue + "&State=view";
                            });
                        }
                            // Has been Paid/Del/BadDebt
                            // Cancellation
                            // Already Printed
                        else if (!(invoice.Paid == true || invoice.Deleted == true || invoice.BadDebt == true) &&
                            !(invoice.LinkTo.substr(0, 6).toLowerCase() == 'cancel' || invoice.PaymentRequestsEdit != 'F') &&
                            invoice.Printing >= 1) {

                            var urlEdit = base_url + "PaymentRequest/detail?Id=" + id + "&JobId=" + jobValue + "&State=edit";
                            var urlView = base_url + "PaymentRequest/detail?Id=" + id + "&JobId=" + jobValue + "&State=view";

                            // On Edit Mode
                            if (buttonID == "invoice_btn_edit") {
                                $('#dialogEdit').dialog('open');

                                $('#btnDialogEditOk').attr('rel', urlEdit).data('id', id);
                                $('#btnDialogEditView').attr('rel', urlView);
                            }

                            // On Delete Mode
                            if (buttonID == "invoice_btn_delete") {
                                $('#dialogDelete').dialog('open');

                                $('#btnDialogDeleteOk').data('id', id);
                                $('#btnDialogDeleteView').attr('rel', urlView);
                                //alert(buttonID);
                            }

                            // On Print Mode
                            if (buttonID == "invoice_btn_print") {
                                PrintPaymentRequest('seaexport', id, invoice.debetcredit, invoice.companycode, invoice.intcompany);
                                //window.open(base_url + "Print_Forms/PrintPaymentRequests.aspx?PaymentRequestsNo=" + invoicesno + "&DebetCredit=" + debetcredit + "&ShipmentNo=" + shipmentno);
                            }
                        }
                        else {

                            // On Delete Mode
                            if (buttonID == "invoice_btn_delete") {

                                $.messager.confirm('Confirm', 'Are you sure you want to delete selected PaymentRequest?', function (r) {
                                    if (r) {

                                        DeletePaymentRequest(id);
                                    }
                                });
                            }
                                // On Print Mode
                            else if (buttonID == "invoice_btn_print") {
                                PrintPaymentRequest('seaexport', id, invoice.debetcredit, invoice.companycode, invoice.intcompany);
                                //window.open(base_url + "Print_Forms/PrintPaymentRequests.aspx?PaymentRequestsNo=" + invoicesno + "&DebetCredit=" + debetcredit + "&ShipmentNo=" + shipmentno);
                            }
                            else {
                                // invoice_se.js
                                window.location = base_url + "PaymentRequest/detail?Id=" + id + "&JobId=" + jobValue;
                            }
                        }
                    }
                }
            });
        }
        else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $("#btnDialogEditOk").click(function () {
        var jobText = $('#jobtype option:selected').text();
        var jobValue = $('#jobtype').val();

        $('#dialogEdit').dialog('close');
        var invoiceId = $(this).data('id');
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "PaymentRequest/CreateContraOnEditMode",
            data: JSON.stringify({
                PaymentRequestId: invoiceId
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        if (result.objResult != null || result.objResult != undefined)
                            window.location = base_url + "PaymentRequest/Detail?Id=" + result.objResult.PaymentRequestId + "&JobId=" + jobValue;
                        else {
                            ReloadGrid();
                        }
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    $("#btnDialogEditView").click(function () {
        window.location = $(this).attr('rel');
    });

    $("#btnDialogDeleteOk").click(function () {

        var id = $(this).data('id');
        $('#dialogDelete').dialog('close');

        DeletePaymentRequest(id);
    });

    $("#btnDialogDeleteView").click(function () {
        window.location = $(this).attr('rel');
    });

    function DeletePaymentRequest(invoiceId) {

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "PaymentRequest/Delete",
            data: JSON.stringify({
                PaymentRequestId: invoiceId
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        ReloadGrid();
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }


            }
        });
    }

    // PRINT

    var bo = "";
    var fd = "";
    function PrintPaymentRequest(job, invoicesno, debetcredit, companycode, intcompany) {

        // bcr semarang "BC 18", jkt 27, sby 1, bali 23
        if ((intcompany == "BC" && companycode == 18) || (intcompany == "BC" && companycode == 1) || (intcompany == "BC" && companycode == 27) ||
            (intcompany == "BC" && companycode == 23)) {

            $('#dialogPrint').dialog('open');

            $("#btnDialogPrintFixedDraftOk").data('invoicesno', invoicesno).data('debetcredit', debetcredit);
            $("#btnDialogPrintFixedDraftNo").data('invoicesno', invoicesno).data('debetcredit', debetcredit);
        }
        else {
            $('#dialogPrintFixedDraft').dialog('open');

            $("#btnDialogPrintFixedDraftOk").data('invoicesno', invoicesno).data('debetcredit', debetcredit);
            $("#btnDialogPrintFixedDraftNo").data('invoicesno', invoicesno).data('debetcredit', debetcredit);
        }
    }

    $("#btnDialogPrintOk").click(function () {
        $('#dialogPrint').dialog('close');

        bo = "b";

        $('#dialogPrintFixedDraft').dialog('open');
    });

    $("#btnDialogPrintNo").click(function () {
        $('#dialogPrint').dialog('close');

        bo = "o";

        $('#dialogPrintFixedDraft').dialog('open');
    });

    $("#btnDialogPrintFixedDraftOk").click(function () {
        $('#dialogPrintFixedDraft').dialog('close');

        fd = "f";
        var PaymentRequestsNo = $(this).data('invoicesno');
        var DC = $(this).data('debetcredit');

        window.open(base_url + "PaymentRequest/Print?Id=" + PaymentRequestsNo + "&bo=" + bo + "&fd=" + fd);
    });

    $("#btnDialogPrintFixedDraftNo").click(function () {
        $('#dialogPrintFixedDraft').dialog('close');

        fd = "d";
        var PaymentRequestsNo = $(this).data('invoicesno');
        var DC = $(this).data('debetcredit');

        window.open(base_url + "PaymentRequest/PrintPaymentRequests?Id=" + PaymentRequestsNo + "&bo=" + bo + "&fd=" + fd);
    });

    // END PRINT

    // RePrint Approval
    $('#form_btn_reprintapproval').click(function () {

        $.messager.confirm('Confirm', 'Are you sure you want to process this?', function (r) {
            if (r) {

                var buttonID = $(this).attr('id');
                var jobText = $('#jobtype option:selected').text();
                var jobValue = $('#jobtype').val();

                var id = jQuery("#list_invoice").jqGrid('getGridParam', 'selrow');
                if (id) {

                    //var ret = jQuery("#list_invoice").jqGrid('getRowData', id);
                    var invoice = null;
                    $.ajax({
                        dataType: "json",
                        url: base_url + "PaymentRequest/GetPaymentRequestInfo?Id=" + id + "&JobId=" + jobValue,
                        success: function (result) {
                            if (!result.isValid)
                                $.messager.alert('Information', result.message, 'info', function () {
                                    window.location = base_url + "PaymentRequest";
                                });
                            if (result.objJob == null)
                                $.messager.alert('Information', "Object is null", 'info', function () {
                                    window.location = base_url + "PaymentRequest";
                                });
                            else {
                                invoice = result.objJob;

                                // Except Cancellation Printing
                                if (invoice.Deleted == true && !(invoice.LinkTo.substr(0, 6).toLowerCase() == 'cancel')) {
                                    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                                    return;
                                }

                                $.ajax({
                                    contentType: "application/json",
                                    type: 'POST',
                                    url: base_url + "PaymentRequest/RePrintApproval",
                                    data: JSON.stringify({
                                        Id: id
                                    }),
                                    success: function (result) {
                                        if (result.isValid) {
                                            $.messager.alert('Information', result.message, 'info');
                                        }
                                        else {
                                            $.messager.alert('Warning', result.message, 'warning');
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    // END RePrint Approval

    //Confirm Invoice
    $('#btn_confirm').click(function () {
        var id = jQuery("#list_invoice").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_invoice").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to confirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "PaymentRequest/Confirm",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
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
                                ReloadGrid();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // END Confirm Invoice

    //Unconfirm Invoice
    $('#btn_unconfirm').click(function () {
        var id = jQuery("#list_invoice").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_invoice").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unconfirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "PaymentRequest/Unconfirm",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
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
                                ReloadGrid();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // END Unconfirm Invoice

}); //END DOCUMENT READY
