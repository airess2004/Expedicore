$(document).ready(function () {
    // Initiate
    var isAgent = true;
    var isShipper = false;
    var vInvoiceDetailEdit = 0;
    var jobNumber = 0;
    var subJobNumber = 0;
    var jobCode = 0;
    var companycode = 0;
    var intcompany = "";
    var vContainerCount20 = 0;
    var vContainerCount40 = 0;
    var vContainerCount45 = 0;
    var counterSellRate = 0;
    var counterBuyRate = 0;
    var counterProfitShare = 0;
    $('#dialogPrint').dialog('close');
    $('#dialogPrintFixedDraft').dialog('close');
    $('#seaimportdetail_content').css("height", $(window).height() - 120);
    $('#seaimportdetail_content').css("width", $(window).width() - 20);
    $('#lookup_div_shipmentorder').dialog('close');
    $('#lookup_div_account').dialog('close');
    $('#lookup_div_account_hfps').dialog('close');
    $('#lookup_div_account_dem').dialog('close');
    $('#lookup_div_user').dialog('close');
    $('#lookup_div_customer').dialog('close');
    $('#lookup_div_invoice_detail').dialog('close');
    $('#lookup_div_demurrage').dialog('close');
    $('#lookup_div_container').dialog('close');
    $('#lookup_div_invoice_detail_handlingfee').dialog('close');
    $('#lookup_div_invoice_detail_profitsplit').dialog('close');
    $('#lookup_div_billto').dialog('close');
    $('#rowVat').show();
    $('#btn_add_invoice_detail').attr('disabled', 'disabled');
    $('#btn_edit_invoice_detail').attr('disabled', 'disabled');
    $('#btn_delete_invoice_detail').attr('disabled', 'disabled');
    $("#optSEDCdebet").attr("disabled", "disabled");
    $("#optSEDCcredit").attr("disabled", "disabled");
    /*================================================ JQuery Tabs ================================================*/
    $('ul.tabs').each(function () {
        // For each set of tabs, we want to keep track of
        // which tab is active and it's associated content
        var $active, $content, $links = $(this).find('a');

        // If the location.hash matches one of the links, use that as the active tab.
        // If no match is found, use the first link as the initial active tab.
        $active = $($links.filter('[href="' + location.hash + '"]')[0] || $links[0]);
        $active.addClass('active');

        $content = $($active[0].hash);

        // Hide the remaining content
        $links.not($active).each(function () {
            $(this.hash).hide();
        });

        // Bind the click event handler
        $(this).on('click', 'a', function (e) {
            // Make the old tab inactive.
            $active.removeClass('active');
            $content.hide();

            // Update the variables with the new link and content
            $active = $(this);
            $content = $(this.hash);

            // Make the tab active.
            $active.addClass('active');
            $content.show();

            // Prevent the anchor's default click action
            e.preventDefault();
        });
    });
    /*================================================ END JQuery Tabs ================================================*/


    // First Load
    // tools.js
    var shipmentId = 0;
    var jobId = getQueryStringByName('JobId');
    var InvoiceId = getQueryStringByName('Id');
    var InvoicesNo = getQueryStringByName('InvoicesNo');
    var DC = getQueryStringByName('DC');
    var State = getQueryStringByName('State');

    // Edit Mode
    if (InvoiceId != "") {
        FirstLoad();
    }
    // End First Load

    function FirstLoad(InvoicesNo) {

        $.ajax({
            dataType: "json",
            url: base_url + "Invoice/GetInfo?Id=" + InvoiceId + "&JobId=" + jobId,
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
                    shipmentId = result.ShipmentOrderId;
                    jobNumber = result.JobNumber;
                    subJobNumber = result.SubJobNo;
                    jobCode = result.JobCode;
                    companycode = result.CompanyCode;
                    intcompany = result.IntCompany;
                    $("#InvoicesNo").val(result.InvoicesNo);
                    $("#txtSEetd").val(dateEnt(result.ETDETA));
                    $("#txtSEduedate").val(dateEnt(result.DueDate));
                    $("#txtSEshipmentno").val(result.ShipmentOrderCode);
                    $("#txtSEprinting").val(result.Printing);
                    $("#txtSEdateprint").val(dateEnt(result.PrintedAt));

                    $("#optSEDCcredit").attr("checked", "checked");
                    if (result.DebetCredit == 'D')
                        $("#optSEDCdebet").attr("checked", "checked");

                    $("#optSEpaymentfrompr").attr("checked", "checked");
                    if (result.JenisInvoices == "G")
                        $("#optSEpaymentfromgpr").attr("checked", "checked");

                    if (result.CurrencyId == 0)
                    {
                        $("#ddlcurrency").val('USD');
                    }
                    else
                    {
                        $("#ddlcurrency").val('IDR');
                    }
                      

                    $("#txtSEkdcustomer").val(result.CustomerCode).data('kode', result.ContactId);
                    $("#txtSEnmcustomer").val(result.CustomerName);
                    $("#txtSEcustomeraddress").text(result.CustomerAddress);

                    $("#rbTotalIDRNotPaid").attr("checked", "checked");
                    if (result.Paid)
                        $("#rbTotalIDRPaid").attr("checked", "checked");

                    $("#txtSEpaymentidr").numberbox('setValue', result.PaymentIDR);
                    $("#txtSEpaymentusd").numberbox('setValue', result.PaymentUSD);
                    $("#txtSErate").numberbox('setValue', result.Rate);
                    $("#txtSEdaterate").val(dateEnt(result.ExRateDate));


                    // Invoice Detail
                    if (result.ListInvoiceDetail != null) {
                        if (result.ListInvoiceDetail.length > 0) {
                            for (var i = 0; i < result.ListInvoiceDetail.length; i++) {
                                addInvDetail(0,
                                    result.ListInvoiceDetail[i].Id,
                                    result.ListInvoiceDetail[i].EPLDetailId,
                                    result.ListInvoiceDetail[i].AccountCode,
                                    result.ListInvoiceDetail[i].AccountName,
                                    result.ListInvoiceDetail[i].Description,
                                    result.ListInvoiceDetail[i].Type,
                                    result.ListInvoiceDetail[i].AmountCrr,
                                    result.ListInvoiceDetail[i].Amount,
                                    result.ListInvoiceDetail[i].Quantity,
                                    result.ListInvoiceDetail[i].PerQty,
                                    result.ListInvoiceDetail[i].CodingQuantity,
                                    result.ListInvoiceDetail[i].Sign,
                                    result.ListInvoiceDetail[i].HFPSinfo,
                                    result.ListInvoiceDetail[i].PercentVat,
                                    result.ListInvoiceDetail[i].VatId,
                                    result.ListInvoiceDetail[i].Dmr,
                                    result.ListInvoiceDetail[i].DmrContainerDetail,
                                    result.ListInvoiceDetail[i].DmrContainer);
                            }
                        }
                    }

                    $('#btnSEshipmentno').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
                    $('#btnSEbillto').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
                    $('#btnSEcustomer').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');

                    $("#rbInvoicesToShipper").attr("disabled", "disabled");
                    $("#rbInvoicesToAgent").attr("disabled", "disabled");
                    $("#optSEpaymentfrompr").attr("disabled", "disabled");
                    $("#optSEpaymentfromgpr").attr("disabled", "disabled");
                   
                    $("#ddlcurrency").attr("disabled", "disabled");

                    $('#btn_add_invoice_detail').removeClass('ui-state-disabled').removeAttr('disabled');
                    $('#btn_edit_invoice_detail').removeClass('ui-state-disabled').removeAttr('disabled');
                    $('#btn_delete_invoice_detail').removeClass('ui-state-disabled').removeAttr('disabled');
                    // Disable Button for Printed or Cancellation Invoice
                    if (result.Printing > 0 || (result.LinkTo != null && result.LinkTo != '' && result.LinkTo.substr(0, 6).toLowerCase() == 'cancel')) {
                        // Disable Save 
                        $('#seaimport_form_btn_save').removeAttr('onclick').unbind('click').off('click');
                       // $('#btn_add_invoice_detail').attr('disabled', 'disabled');
                       // $('#btn_edit_invoice_detail').attr('disabled', 'disabled');
                       // $('#btn_delete_invoice_detail').attr('disabled', 'disabled');
                        $('#btn_demurrage_invoice_detail').attr('disabled', 'disabled');
                        $('#btn_add_invoice_detail_handlingfee').attr('disabled', 'disabled');
                        $('#btn_add_invoice_detail_profitsplit').attr('disabled', 'disabled');
                        $("#txtSEinvoicefromagentno").attr("disabled", "disabled");

                        $("#txtSEnmcustomer, #txtSEcustomeraddress, #txtSEnmbillto, #txtSEbilltoaddress").attr("disabled", "disabled");
                    }

                }


            }
        });
    }

    // ============================================================================ LOOKUP =============================================
    // ------------------------------------------------ Shipment Order
    // Browse
    $('#btnSEshipmentno').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_shipmentorder');
        lookupGrid.setGridParam({ url: base_url + 'ShipmentOrder/GetLookUpInvoice', postData: { filters: null, job: jobId }, page: 'last' }).trigger("reloadGrid");
        $('#lookup_div_shipmentorder').dialog('open');
    });
    // Cancel or CLose
    $('#lookup_btn_cancel_shipmentorder').click(function () {
        $('#lookup_div_shipmentorder').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_shipmentorder').click(function () {
        var id = jQuery("#lookup_table_shipmentorder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_shipmentorder").jqGrid('getRowData', id);
            $('#txtSEshipmentno').val(ret.shipmentorderno);
            $('#txtSEetd').val(ret.etd);

            //$('#btnSEcustomer')
            //    .data('shippercode', ret.shipperid).data('shippercode2', ret.shippercode).data('shippername', ret.shippername).data('shipperaddress', ret.shipperaddress)
            //    .data('agentcode', ret.agentid).data('agentcode2', ret.agentcode).data('agentname', ret.agentname).data('agentaddress', ret.agentaddress);
            //if (isAgent) {
            //    $('#txtSEkdcustomer').val(ret.agentcode).data('kode', ret.agentid);
            //    $('#txtSEnmcustomer').val(ret.agentname);
            //    $('#txtSEcustomeraddress').val(ret.agentaddress);
            //}
            //else {
            //    $('#txtSEkdcustomer').val(ret.shippercode).data('kode', ret.shipperid);
            //    $('#txtSEnmcustomer').val(ret.shippername);
            //    $('#txtSEcustomeraddress').val(ret.shipperaddress);
            //}

            $('#txtSEjobowner').val(ret.jobowner);
            jobNumber = ret.jobnumber;
            subJobNumber = ret.subjobno;
            jobCode = ret.jobcode;
            shipmentId = id;

            // Clear and Reload all grid
            $("#table_invoice_detail").jqGrid("clearGridData", true).trigger("reloadGrid");

            // Auto Populate Account Invoice
            PopulateAccountInvoiceDefault(0, ret.shipperid);
            // END Auto Populate Account Invoice

            $('#lookup_div_shipmentorder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_shipmentorder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Shipment Order', 'ETD', 'Consignee', 'Agent', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        colModel: [{ name: 'shipmentorderno', index: 'ShipmentOrderCode', width: 130, align: "center" },
                  { name: 'etd', index: 'ETD', width: 80, align: "right", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'shippername', index: 'ConsigneeName' },
                  { name: 'agentname', index: 'AgentName' },
                  { name: 'jobnumber', index: 'jobnumber', hidden: true },
                  { name: 'subjobno', index: 'subjobno', hidden: true },
                  { name: 'shipperid', index: 'shipperid', hidden: true },
                  { name: 'shippercode', index: 'shippercode', hidden: true },
                  { name: 'agentid', index: 'agentid', hidden: true },
                  { name: 'agentcode', index: 'agentcode', hidden: true },
                  { name: 'shipperaddress', index: 'shipperaddress', hidden: true },
                  { name: 'agentaddress', index: 'agentaddress', hidden: true },
                  { name: 'jobcode', index: 'jobcode', hidden: true },
                  { name: 'rate', index: 'rate', hidden: true },
                  { name: 'daterate', index: 'daterate', hidden: true },
                  { name: 'typeepl', index: 'typeepl', hidden: true },
                  { name: 'containersizelist', index: 'containersizelist', hidden: true }
        ],
        pager: jQuery('#lookup_pager_shipmentorder'),
        sortname: "ShipmentOrderCode",
        sortorder: "asc",
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        viewrecords: true,
        shrinkToFit: false,
        width: 570,
        height: 350
    });
    $("#lookup_table_shipmentorder").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- End Shipment Order----------------------------------------------------

    // ---------------------------------------------------- Customer (Agent / Shipper) ------------------------------------------------------------------------------
    // Browse
    $('#btnSEcustomer').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'MstContact/GetLookUpEPLIncome';

        //var lookUpURL = base_url + 'EPL/LookUpCustomer';
        if ($('#optSEpaymentfromgpr').is(':checked')) {
            lookUpURL = base_url + 'MstContact/GetLookUP';
        }
        if ($('#ddlcurrency').val() == 'USD') {
            amountCrr = 0;
        }
        else {
            amountCrr = 1;
        }
        var lookupGrid = $('#lookup_table_agent');
        lookupGrid.setGridParam({
            postData: { 'ShipmentOrderID': function () { return shipmentId; }, 'amountCrr': function () { return amountCrr; } },
            url: lookUpURL
        }).trigger("reloadGrid");

        $('#lookup_div_customer').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_customer').click(function () {
        $('#lookup_div_customer').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_customer').click(function () {
        // If Agent Or Shipper
        var id = jQuery("#lookup_table_agent").jqGrid('getGridParam', 'selrow');
        //if (isAgent == false) {
        //    id = jQuery("#lookup_table_shipper").jqGrid('getGridParam', 'selrow');
        //}
        if (id) {
            // If Agent Or Shipper
            var ret = jQuery("#lookup_table_agent").jqGrid('getRowData', id);
            if (isAgent == false) {
                ret = jQuery("#lookup_table_shipper").jqGrid('getRowData', id);
            }

            $('#txtSEkdcustomer').val(ret.code).data("kode", id);
            $('#txtSEnmcustomer').val(ret.name);
            $('#txtSEcustomeraddress').val(ret.address);

            // Auto Populate Account Invoice
            PopulateAccountInvoiceDefault(0, id);
            // END Auto Populate Account Invoice

            $('#lookup_div_customer').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_agent").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Contact Status','Contact Name', 'Contact As','Address'],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: 'right' },
                  { name: 'status', index: 'ContactStatus', width: 200 },
                  { name: 'name', index: 'ContactName', width: 200 },
                  { name: 'as', index: 'ContactAs', width: 200 },
                  { name: 'address', index: 'ContactAddress', width: 200 }],
        pager: jQuery('#lookup_pager_shipper'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortname: "ContactName",
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_agent").jqGrid('navGrid', '#toolbar_lookup_table_shipper', { del: false, add: false, edit: false, search: false })
        .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

  


    // ---------------------------------------------------- End Customer (Agent / Shipper) ----------------------------------------------------


    // ---------------------------------------------------- Account ------------------------------------------------------------------------------
    // Browse
    $('#btnInvaccount').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var vBillTo = false;
        var custType = "xxx";
        var debetCredit = "xxx";
        var incomeAgent = 0;
        var amountCrr = 0;
        var customerId = $('#txtSEkdcustomer').data("kode");
        var customerCode = $('#txtSEkdcustomer').val();

        // Set BillTo as customerCode
        var billId = 0;
        var billTo = $('#txtSEkdbillto').val();
        if (billTo != undefined && billTo != "") {
            linkCode = billTo;
            vBillTo = true;
            customerCode = billTo;
            billId = $('#txtSEkdbillto').data("kode");
        }

        // SHipper
        if (isShipper == true) {
            custType = "CO";
        }
        else {
            custType = "AG";
            debetCredit = "D";
            if ($('#optSEDCcredit').is(':checked'))
                debetCredit = "C";
        }

        if ($('#ddlcurrency').val() == 'USD')
        {
            amountCrr = 0;
        }
        else
        {
            amountCrr = 1;
        }
        var lookupGrid = $('#lookup_table_account');
        $('#lookup_table_account_div').show();
        $('#lookup_table_account_general_div').hide();


        // Invoice
        var accountUrl = 'EstimateProfitLoss/GetLookUpIncome';
        if ($('#optSEpaymentfromgpr').is(':checked')) {
            // General Invoice
            accountUrl = 'Cost/GetLookUPPJK';
            lookupGrid = $('#lookup_table_account_general');
            $('#lookup_table_account_general_div').show();
            $('#lookup_table_account_div').hide();
        }

        // Existed Account
        var listExistedEPLAccount = [];
        var data = $("#table_invoice_detail").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = data[i].epldetailid;
            listExistedEPLAccount.push(obj);
        }
        // END Existed Account

        lookupGrid.setGridParam({
            url: base_url + accountUrl,
            postData: {
                filters: null, 'shipmentOrderId': function () { return shipmentId; },
                'custType': function () { return custType; }, 'customerId': function () { return customerId; }, 'billId': function () { return billId; },
                'debetCredit': function () { return debetCredit; }, 'amountCrr': function () { return amountCrr; },
                'listExistedEPLAccount': function () { return JSON.stringify(listExistedEPLAccount); }
            }
        }).trigger("reloadGrid");
        $('#lookup_div_account').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_account').click(function () {
        $('#lookup_div_account').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_account').click(function () {
        var accountGrid = jQuery("#lookup_table_account");
        // General
        if ($('#optSEpaymentfromgpr').is(':checked')) {
            accountGrid = jQuery("#lookup_table_account_general");
        }
        var id = accountGrid.jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = accountGrid.jqGrid('getRowData', id);
            var check = true;

            // Remove Disabled
            $('#txtISquantity').removeAttr('disabled');
            $('#txtISamount').removeAttr('disabled');

            // CodingQuantity
            if (ret.codingquantity == "true") {
                $('#txtInvquantity').numberbox('setValue', 0);
            }
            else if (ret.quantity != undefined) {
                $('#txtISquantity').numberbox('setValue', ret.quantity);
            }
            else
                $('#txtISquantity').numberbox('setValue', 0);

            // Per Unit Cost
            if (ret.perqty != undefined) {
                $('#txtISperunitcost').numberbox('setValue', ret.perqty);
            }
            else
                $('#txtISperunitcost').numberbox('setValue', 0);

            // Check Job Container or NOT
            if (check) {
                $('#txtInvkdaccount').val(ret.code).data("kode", ret.costid).data("amountidr", ret.amountidr).data("amountusd", ret.amountusd).data("codingquantity", ret.codingquantity);
                $('#txtInvnmaccount').val(ret.name);
                $('#txtInvdesc').val(ret.name);

                // As Payment Request
                if ($('#optSEpaymentfrompr').is(':checked')) {
                    $('#txtInvdesc').val(ret.desc);
                    if (ret.currencytype == 'USD') {
                        $('#optInvcurrencytypeUSD').attr('checked', 'checked');
                        $('#txtInvamount').numberbox('setValue', ret.payment);
                    }
                    else {
                        $('#optInvcurrencytypeIDR').attr('checked', 'checked');
                        $('#txtInvamount').numberbox('setValue', ret.payment);
                    }
                }
                else {
                    if ($('#optInvcurrencytypeUSD').is(':checked')) {
                        $('#txtInvamount').numberbox('setValue', ret.amountusd);
                    }
                    else {
                        $('#txtInvamount').numberbox('setValue', ret.amountidr);
                    }
                }

                $('#txtInvdifferentcost').numberbox('setValue', ret.payment);
                if (ret.sign == 'false')
                    $('#optInvsignminus').attr('checked', 'checked');
                else
                    $('#optInvsignplus').attr('checked', 'checked');
            }
            else
                $.messager.alert('Warning', "Job doesn't have Container !", 'warning');

            // As NOT General
            if ($('#optSEpaymentfrompr').is(':checked')) {
                $('#lookup_btn_add_invoice_detail').data('epldetailid', ret.epldetailid);
            }
            else {
                $('#txtInvkdaccount').data("kode", id);
            }
            $('#lookup_div_account').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_account").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Description', 'amountusd', 'amountidr', 'codingquantity', 'Crr', 'Payment', 'Type', '', '', '',''],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: "center" },
                  { name: 'name', index: 'Contact', width: 200 ,hidden: true},
                  { name: 'desc', index: 'Description', width: 200,  },
                  { name: 'amountusd', index: 'AmountUSD', width: 200, hidden: true },
                  { name: 'amountidr', index: 'AmountIDR', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'CodingQuantity', width: 200, hidden: true },
                  { name: 'currencytype', index: 'currencytype', width: 50, hidden: false },
                  { name: 'payment', index: 'payment', width: 200, hidden: false, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'type', index: 'type', width: 50, hidden: false },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true },
                  { name: 'perqty', index: 'perqty', width: 200, hidden: true },
                  { name: 'epldetailid', index: 'epldetailid', width: 200, hidden: true },
                  { name: 'costid', index: 'costid', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortname: "Id",
        sortorder: "asc",
        width: 460,
        height: 350
    });

    $("#lookup_table_account").jqGrid('navGrid', '#toolbar_lookup_table_account', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    jQuery("#lookup_table_account_general").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', ''],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'amountusd', index: 'amountusd', width: 200, hidden: true },
                  { name: 'amountidr', index: 'amountidr', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'codingquantity', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account_general'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "Id",
        width: 460,
        height: 350
    });
    $("#lookup_table_account_general").jqGrid('navGrid', '#toolbar_lookup_table_account', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- END Account ------------------------------------------------------------------------------
    // -------------------------------------------------------------------- END LOOKUP ----------------------------------------------------

    // Auto Populate Account Invoice
    function PopulateAccountInvoiceDefault(billId, customerId) {
          $("#table_invoice_detail").jqGrid("clearGridData", true).trigger("reloadGrid");
        }

    // END Auto Populate Account Invoice

    // Choose Debet / Credit
    $('input[name=optSEDC], input[name=optSEpaymentfrom]').click(function () {

        $('#txtSEkdbillto').val('').data('kode', '');
        $('#txtSEnmbillto').val('');

        // Clear and Reload all grid
        $("#table_invoice_detail").jqGrid("clearGridData", true).trigger("reloadGrid");

        $('#txtSEpaymentusd, #txtSEpaymentidr, #txtSEtotalvatusd, #txtSEtotalvatidr').numberbox('setValue', 0);

        // Auto Populate Account Invoice
        var vBillTo = false;
        var linkCode = $('#txtSEkdcustomer').data('kode');
        // Set BillTo as LinkCode
        var billTo = $('#txtSEkdbillto').data('kode');
        if (billTo != undefined && billTo != "") {
            linkCode = billTo;
            vBillTo = true;
        }

        PopulateAccountInvoiceDefault(vBillTo, linkCode);
        // END Auto Populate Account Invoice

    });

    $("#ddlcurrency").change(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Auto Populate Account Invoice
        var vBillTo = false;
        var linkCode = $('#txtSEkdcustomer').data('kode');
        // Set BillTo as LinkCode
        var billTo = $('#txtSEkdbillto').data('kode');
        if (billTo != undefined && billTo != "") {
            linkCode = billTo;
            vBillTo = true;
        }
        PopulateAccountInvoiceDefault(vBillTo, linkCode);
    });

   

    // Calculation ------------------------------------------------------------------------------------------------
    function TotalCalculation() {
        var totalUSD = 0;
        var totalIDR = 0;
        var totalVatUSD = 0;
        var totalVatIDR = 0;
        var data = $("#table_invoice_detail").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            if (data[i].accountid != '003' && data[i].accountid != '004') { 
                if (data[i].sign == '-') {
                    totalUSD -= parseFloat(data[i].amountusd);
                    totalIDR -= parseFloat(data[i].amountidr);
                }
                else if (data[i].sign == '+') {
                    totalUSD += parseFloat(data[i].amountusd);
                    totalIDR += parseFloat(data[i].amountidr);
                }
                if (data[i].currencytype == 1)
                    totalVatIDR += parseFloat(data[i].amountvat);
                else
                    totalVatUSD += parseFloat(data[i].amountvat);

            }
        }
        totalUSD = Math.round(totalUSD * 100) / 100;
        totalIDR = Math.round(totalIDR * 100) / 100;
        totalVatUSD = Math.round(totalVatUSD * 100) / 100;
        totalVatIDR = Math.round(totalVatIDR * 100) / 100;
        $("#txtSEpaymentusd").numberbox("setValue", totalUSD);
        $("#txtSEpaymentidr").numberbox("setValue", totalIDR);
        $("#txtSEtotalvatusd").numberbox("setValue", totalVatUSD);
        $("#txtSEtotalvatidr").numberbox("setValue", totalVatIDR);
    }

    // DisableOnEditNonAgent
    function disableOnEditAgentShipper() {
        $('#btnInvshipper').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvssline').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvemkl').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvrebate').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvdepo').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvaccount').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
    }
    // EnableOnAddNonAgent
    function enableOnEditAgentShipper() {
        $('#btnInvshipper').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvssline').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvemkl').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvrebate').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvdepo').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvaccount').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
    }

    $("#txtInvquantity, #txtInvperunitcost").blur(function () {
        var total = parseFloat($('#txtInvquantity').numberbox('getValue')) * parseFloat($('#txtInvperunitcost').numberbox('getValue'));
        total = Math.round(total * 100) / 100;
        $('#txtInvamount').numberbox('setValue', total);
    });

    // Clear
    function clearInvAgentShipper() {
        //$('#txtInvkdagent').val('').data('kode', '');
        //$('#txtInvnmagent').val('');
        $('#txtInvkdaccount').val('').data("kode", '');
        $('#txtInvnmaccount').val('');
        $('#txtInvdesc').val('');
        $('#txtInvquantity').numberbox('setValue', 0);
        $('#txtInvperunitcost').numberbox('setValue', 0);
        $('#txtInvamount').numberbox('setValue', 0);
        $('#txtInvvat').numberbox('setValue', 0);
        $('#optInvcontainersizeall').attr('checked', 'checked');
        $('#optInvcurrencytypeUSD').attr('checked', 'checked');

        // Currency
        $('input[name=optInvcurrencytype]').removeAttr('disabled');
        if ($('#ddlcurrency').val() == 'USD') {
            $('input[name=optInvcurrencytype]').attr('disabled', 'disabled');
            $('#optInvcurrencytypeUSD').attr('checked', 'checked');
        }
        else if ($('#ddlcurrency').val() == 'IDR') {
            $('input[name=optInvcurrencytype]').attr('disabled', 'disabled');
            $('#optInvcurrencytypeIDR').attr('checked', 'checked');
        }
    }


    // Add
    $('#btn_add_invoice_detail').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Clear Input
        clearInvAgentShipper();
        enableOnEditAgentShipper();
        vInvoiceDetailEdit = 0;

        // Currency Type
        $('#rowCurrencyType').show();

        // Assigned Invoice Detail Id
        $('#lookup_btn_add_invoice_detail').data('invoicedetailid', null);
        // Assigned EPL Detail Id
        $('#lookup_btn_add_invoice_detail').data('epldetailid', null);


        // Set Shipper or Agent
        $('#txtInvkdagent').val($('#txtSEkdcustomer').val());
        $('#txtInvnmagent').val($('#txtSEnmcustomer').val());

        // Set BillTo as LinkCode
        var billTo = $('#txtSEkdbillto').data("kode");
        if (billTo != undefined && billTo != "") {
            $('#txtInvkdagent').val($('#txtSEkdbillto').val());
            $('#txtInvnmagent').val($('#txtSEnmbillto').val());
        }

        if (isAgent == true) {
            $('#lookup_div_invoice_detail').dialog({ title: 'Invoices to Agent' });
        }
        else {
            $('#lookup_div_invoice_detail').dialog({ title: 'Invoices to Consignee' });
        }
        $('#lookup_div_invoice_detail').dialog('open');

    });

    // Edit
    $('#btn_edit_invoice_detail').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Clear Input
        clearInvAgentShipper();
        var id = jQuery("#table_invoice_detail").jqGrid('getGridParam', 'selrow');
        if (id) {

            disableOnEditAgentShipper();
            vInvoiceDetailEdit = 1;

            // Set Shipper or Agent
            var customerCode = $('#txtSEkdcustomer').val();
            var customerName = $('#txtSEnmcustomer').val();

            var ret = jQuery("#table_invoice_detail").jqGrid('getRowData', id);


            // Assigned Invoice Detail Id
            $('#lookup_btn_add_invoice_detail').data('invoicedetailid', id);
            // Assigned EPL Detail Id
            $('#lookup_btn_add_invoice_detail').data('epldetailid', ret.epldetailid);
            $('#txtInvkdagent').val($('#txtSEkdcustomer').val());
            $('#txtInvnmagent').val($('#txtSEnmcustomer').val());
            $('#txtInvnmaccount').val(ret.accountname);
            $('#txtInvdesc').val(ret.description);
            $('#txtInvamount').numberbox('setValue', ret.amountusd);
            $('#optInvsignplus').attr('checked', 'checked');
            if (ret.sign == '-')   // minus
            {
                $('#optInvsignminus').attr('checked', 'checked');
            }
            $('#txtInvkdaccount').data("kode", ret.accountid).val(ret.accountid);
            $('#txtInvquantity').numberbox('setValue', ret.quantity);
            $('#txtInvperunitcost').numberbox('setValue', ret.perqty);
            $('#txtInvkdaccount').data("codingquantity", ret.codingquantity);
            $('#ddlSEvat').val(ret.vatid);

            $('#rowCurrencyType').show();
            if (ret.currencytype == 1) {
                    $('#optInvcurrencytypeIDR').attr('checked', 'checked');
                    $('#txtInvamount').numberbox('setValue', ret.amountidr);
            }
            $('#lookup_div_invoice_detail').dialog('open');
        }
        else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Delete
    $('#btn_delete_invoice_detail').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                if (shipmentId == "") {
                    $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
                    return;
                }

                // Assigned Invoice Detail Id
                $('#lookup_btn_add_invoice_detail').data('invoicedetailid', 0);
                // Assigned EPL Detail Id
                $('#lookup_btn_add_invoice_detail').data('epldetailid', 0);

                $('#lookup_btn_add_invoice_detail_profitsplit').data('counterprofitsplit', 0);

                var id = jQuery("#table_invoice_detail").jqGrid('getGridParam', 'selrow');
                if (id) {
                    var data = jQuery("#table_invoice_detail").jqGrid('getRowData', id);
                        DeleteInvoiceDetail(id, function (result) {
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
                            else
                            {
                                jQuery("#table_invoice_detail").jqGrid('delRowData', id);
                            }
                        });
                    TotalCalculation();
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });

    // Close
    $('#lookup_btn_cancel_invoice_detail').click(function () {
        clearInvAgentShipper();
        $('#lookup_div_invoice_detail').dialog('close');
    });

    // Save
    $('#lookup_btn_add_invoice_detail').click(function () {
        var amount = $('#txtInvamount').numberbox('getValue');
        // default All
        var containerType = 5;

        var sign = true;
        if ($('#optInvsignminus').is(':checked'))
            sign = false;

        // Currency Type
        var currencyType = 0;
        $('#rowCurrencyType').show();
        if ($('#optInvcurrencytypeIDR').is(':checked'))
            currencyType = 1;

        var vatId = 0;
        vatId = $('#ddlSEvat').val();

        var invoiceDetailId = 0
        var eplDetailId = $('#lookup_btn_add_invoice_detail').data('epldetailid');
        // Assigned Invoice Detail Id
        if (vInvoiceDetailEdit == 1) {
            invoiceDetailId = $('#lookup_btn_add_invoice_detail').data('invoicedetailid');
        }
        var submitURL = (InvoiceId != undefined && InvoiceId != "") ? (vInvoiceDetailEdit == 0) ? base_url + "Invoice/InsertDetail" : base_url + "Invoice/UpdateDetail" : "";
        var isValid = true;
        var message = "OK";
        var amountvat = 0;
        if ($('#ddlSEvat').val() != '') {
            amountvat = parseFloat(($('#ddlSEvat').val() / 100) * amount);
            amountvat = Math.round(amountvat * 100) / 100;
        }

        // Add or Edit
        SaveInvoiceDetail(submitURL, invoiceDetailId, eplDetailId, $('#txtSEkdcustomer').data('kode'), $('#txtInvkdaccount').data("kode"), $('#txtInvnmaccount').val(),
                        $('#txtInvdesc').val(), $('#txtInvquantity').numberbox('getValue'), $('#txtInvperunitcost').numberbox('getValue'), containerType,
                        $('#txtInvkdaccount').data("codingquantity"), sign, currencyType, amount, $('#ddlSEvat option:selected').text(), amountvat, vatId, function (result) {
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
                            else
                            {
                                addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, $('#txtInvkdaccount').data("kode"), $('#txtInvnmaccount').val(), $('#txtInvdesc').val(),
                                 containerType, currencyType, amount, $('#txtInvquantity').numberbox('getValue'), $('#txtInvperunitcost').numberbox('getValue'),
                                  $('#txtInvkdaccount').data("codingquantity"), sign, '', $('#ddlSEvat option:selected').text(), vatId, '', '', '');
                                invoiceDetailId = result.Id;
                            }
                            $('#lookup_div_invoice_detail').dialog('close');
                            clearInvAgentShipper();
                        });
    });

        // Save Invoice Detail
   function SaveInvoiceDetail(submitURL, v_invoicedetailId, v_epldetailId, v_customerId, v_accountId, v_accountName, v_description,
                                            v_quantity, v_perqty, v_type, v_codingquantity, v_sign, v_amountCrr, v_amount, v_percentvat, v_amountvat,
                                            v_vatid, callback) {
            if (submitURL != undefined && submitURL != "") {
                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: submitURL,
                    data: JSON.stringify({
                        EPLDetailId: v_epldetailId, Id: v_invoicedetailId,
                        InvoiceId: InvoiceId, 
                        CostId: v_accountId, Description: v_description,
                        Quantity: v_quantity, PerQty: v_perqty, Sign: v_sign,
                        Type: v_type, Sign: v_sign, CodingQuantity: v_codingquantity,
                        AmountCrr: v_amountCrr, Amount: v_amount, AmountVat: v_amountvat, PercentVat: v_percentvat, VatId: v_vatid,
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
        // END Save Invoice Detail

        // Add or Edit
        function addInvDetail(isEdit, invoiceDetailId, eplDetailId, accountId, accountName, desc, containerType, currencyType, amount, quantity,
                                        perquantity, codingquantity, sign, hfpsinfo, vat, vatId, dmr, dmrContainerDetail, dmrContainer) {
            // Validate
            if (accountId == undefined || accountId == '' || accountId == 0) {
                $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
                return
            }

            var isValid = true;
            if (!isValid) {
                $.messager.alert('Information', 'Data has been exist...!!', 'info');
            }
                // End Validate
            else {
                var newData = {};
                newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
                newData.description = (desc != undefined && desc != "") ? desc.toUpperCase() : "";
                newData.vat = vat;
                newData.vatid = vatId;
                var amountvat = 0;
                var amountidr = 0;
                var amountusd = 0;
                if (vat > 0)
                    amountvat = ((vat / 100) * amount);
                if (currencyType == 0) // USD
                {
                    amountusd = amount;
                }
                if (currencyType == 1) // IDR
                {
                    amountidr = amount;
                }
                newData.amountvat = amountvat;
                newData.amountusd = amountusd;
                newData.amountidr = amountidr;
                newData.currencytype = currencyType;
                newData.accountid = accountId;
                newData.type = containerType;
                newData.quantity = quantity;
                newData.perqty = perquantity;
                newData.codingquantity = codingquantity;
                newData.sign = "-";
                if (sign)
                    newData.sign = "+";
                newData.epldetailid = eplDetailId;

                newData.hfpsinfo = hfpsinfo;

                newData.dmr = dmr;
                newData.dmrcontainerdetail = dmrContainerDetail;
                newData.dmrcontainer = dmrContainer;

                // New Record
                if (isEdit == 0) {
                        invoiceDetailId = invoiceDetailId == 0 ? GetNextId($("#table_invoice_detail")) : invoiceDetailId;
                        jQuery("#table_invoice_detail").jqGrid('addRowData', invoiceDetailId, newData);
                }
                    // Update Mode
                else {
                    var id = jQuery("#table_invoice_detail").jqGrid('getGridParam', 'selrow');
                    var accountidSelected = jQuery("#table_invoice_detail").jqGrid('getCell', id, 'accountid');
                    if (id) {
                        jQuery("#table_invoice_detail").jqGrid('setRowData', id, newData);
                    }
                }

                TotalCalculation();
            }
        }

        jQuery("#table_invoice_detail").jqGrid({
            datatype: "local",
            height: 120,
            rowNum: 150,
            colNames: ['Code', 'Account Name', 'Description', '', 'Amount USD', 'Amount IDR', 'VAT', 'VATId', 'AmountVat', '', '', '', '', '', '', '', '', '', '', 'counterprofitsplit'],
            colModel: [
                { name: 'accountid', index: 'accountid', width: 55, align: 'right' },
                { name: 'accountname', index: 'accountname', width: 285, hidden: true },
                { name: 'description', index: 'description', width: 350 },
                { name: 'currencytype', index: 'currencytype', width: 60, hidden: true },
                { name: 'amountusd', index: 'amountusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                { name: 'amountidr', index: 'amountidr', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                { name: 'vat', index: 'vat', width: 55, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "%", defaultValue: '0' } },
                { name: 'vatid', index: 'vatid', width: 60, hidden: true },
                { name: 'amountvat', index: 'amountvat', width: 120, hidden: true, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                { name: 'type', index: 'type', width: 60, hidden: true },
                { name: 'quantity', index: 'quantity', width: 60, hidden: true },
                { name: 'perqty', index: 'perqty', width: 60, hidden: true },
                { name: 'codingquantity', index: 'codingquantity', width: 60, hidden: true },
                { name: 'hfpsinfo', index: 'hfpsinfo', width: 60, hidden: true },
                { name: 'sign', index: 'sign', width: 30, align: 'center', hidden: false },
                { name: 'dmr', index: 'dmr', width: 50, align: "right", hidden: true },
                { name: 'dmrcontainerdetail', index: 'dmrcontainerdetail', width: 50, align: "right", hidden: true },
                { name: 'dmrcontainer', index: 'dmrcontainer', width: 50, align: "right", hidden: true },
                { name: 'epldetailid', index: 'epldetailid', width: 60, hidden: true },
                { name: 'counterprofitsplit', index: 'counterprofitsplit', width: 60, hidden: true }
            ]
        });


        function GetNextId(objJqGrid) {
            var nextid = 0;
            if (objJqGrid.getDataIDs().length > 0)
                nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

            return nextid;
        }


        // ------------------------------------------------------------------------ SAVE
        $('#seaimport_form_btn_save').click(function () {
            if (shipmentId == "") {
                $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
                return;
            }

            // Invoice
            var invoiceType = 'I';
            if ($('#optSEpaymentfromgpr').is(':checked'))
                // General Invoice
                invoiceType = 'G';

            var debetCredit = "C";
            if ($('#optSEDCdebet').is(':checked'))
                debetCredit = "D";

            var submitURL = base_url + "Invoice/Insert";
            if (InvoiceId != undefined && InvoiceId != 0)
                submitURL = base_url + "Invoice/Update";
            if ($('#ddlcurrency').val() == 'USD') {
                amountCrr = 0;
            }
            else {
                amountCrr = 1;
            }
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    Id: InvoiceId, ShipmentOrderId: shipmentId,
                    ContactId: $('#txtSEkdcustomer').data('kode'), CustomerName: $('#txtSEnmcustomer').val(), CustomerAddress: $('#txtSEcustomeraddress').val(),
                    TotalVatIDR: $("#txtSEtotalvatidr").numberbox("getValue"), TotalVatUSD: $("#txtSEtotalvatusd").numberbox("getValue"),
                    PaymentIDR: $("#txtSEpaymentidr").numberbox("getValue"), PaymentUSD: $("#txtSEpaymentusd").numberbox("getValue"), ShipmentNo: $("#txtSEshipmentno").val(),
                    InvoicesNo: $("#InvoicesNo").val(), JenisInvoices: invoiceType, ETD: $('#txtSEetd').val(), CurrencyId: amountCrr, DebetCredit: debetCredit,
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
                        window.location = base_url + "invoice/detail?Id=" + result.invoiceId + "&JobId=" + jobId;
                    }
                }
            });
        });

        // Invoice Detail
        function PopulateInvoiceDetail() {
            var InvDetail = [];
            var data = $("#table_invoice_detail").jqGrid('getGridParam', 'data');
            for (var i = 0; i < data.length; i++) {
                var obj = {};
                if (data[i].currencytype == 1)
                    obj['Amount'] = data[i].amountidr;
                else
                    obj['Amount'] = data[i].amountusd;
                obj['Description'] = data[i].description;
                obj['AmountCrr'] = data[i].currencytype;
                obj['AccountID'] = data[i].accountid;
                obj['AccountName'] = data[i].accountname;
                obj['Type'] = data[i].type;
                obj['Quantity'] = data[i].quantity;
                obj['PerQty'] = data[i].perqty;
                obj['CodingQuantity'] = data[i].codingquantity;
                obj['Sign'] = false;
                if (data[i].sign == "+")
                    obj['Sign'] = true;
                obj['HFPSinfo'] = data[i].hfpsinfo;
                obj['PercentVat'] = data[i].vat;
                obj['VatId'] = data[i].vatid;
                obj['AmountVat'] = data[i].amountvat;
                obj['EPLDetailId'] = data[i].epldetailid;

                // Demurrage            
                obj['Dmr'] = data[i].dmr;
                obj['DmrContainerDetail'] = data[i].dmrcontainerdetail;
                obj['DmrContainer'] = data[i].dmrcontainer;

                InvDetail.push(obj);
            }

            return InvDetail
        }
        // END Invoice Detail

   

        // Delete Invoice Detail
        function DeleteInvoiceDetail(v_invoicedetailId, callback) {
            if (InvoiceId != undefined && InvoiceId != "") {
                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + 'Invoice/DeleteDetail',
                    data: JSON.stringify({
                        Id: v_invoicedetailId
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
        // END Delete Invoice Detail

        // Print
        $('#seaimport_form_btn_print').click(function () {
            if (InvoiceId == "") {
                $.messager.alert('Information', "Invoice Number can't be empty...!!", 'info');
                return;
            }

            PrintInvoice('seaimport', InvoicesNo, DC, companycode, intcompany);
        });

        var bo = "";
        var fd = "";
        function PrintInvoice(job, invoicesno, debetcredit, companycode, intcompany) {

            // bcr semarang "BC 18", jkt 27, sby 1, bali 23
            if ((intcompany == "BC" && companycode == 18) || (intcompany == "BC" && companycode == 1) || (intcompany == "BC" && companycode == 27) ||
                (intcompany == "BC" && companycode == 23)) {

                $('#dialogPrint').dialog('open');
            }
            else {
                $('#dialogPrintFixedDraft').dialog('open');
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

            window.open(base_url + "Invoice/Print?Id=" + InvoiceId + "&bo=" + bo + "&fd=" + fd);
        });

        $("#btnDialogPrintFixedDraftNo").click(function () {
            $('#dialogPrintFixedDraft').dialog('close');

            fd = "d";

            window.open(base_url + "Invoice/Print?Id=" + InvoiceId + "&bo=" + bo + "&fd=" + fd);
        });
        // END Print

        // RePrint Approval
        $('#seaimport_form_btn_reprintapproval').click(function () {

            if (InvoiceId == undefined || InvoiceId == "") {
                $.messager.alert('Information', 'Invalid Invoice ID..', 'info');
                return;
            }

            $.messager.confirm('Confirm', 'Are you sure you want to process this?', function (r) {
                if (r) {

                    $.ajax({
                        contentType: "application/json",
                        type: 'POST',
                        url: base_url + "Invoice/RePrintApproval",
                        data: JSON.stringify({
                            Id: InvoiceId
                        }),
                        success: function (result) {
                            if (result.isValid) {
                                $.messager.alert('Information', result.message, 'info', function () {
                                    window.location = base_url + "Invoice/Detail?Id=" + InvoiceId + "&JobId=" + jobId;
                                });
                            }
                            else {
                                $.messager.alert('Warning', result.message, 'warning');
                            }
                        }
                    });
                }
            });
        });
        // END RePrint Approval

        // Create New
        $("#invoice_btn_add_new").click(function () {

            $.messager.confirm('Confirm', 'Are you sure you want to create NEW Invoice?', function (r) {
                if (r) {

                    $.ajax({
                        dataType: "json",
                        url: base_url + "Invoice/AllowNewInvoice",
                        success: function (result) {
                            if (!result.isValid) {
                                $.messager.alert('Information', result.message, 'info');
                            }
                            else {
                                window.location = base_url + "invoice/detail?JobId=" + jobId;
                            }
                        }
                    });
                }
            });
        });
        // END Create New

        // Next Invoice
        $('#nav_next').click(function () {
            $.ajax({
                url: base_url + "Invoice/NextInvoice?CurrentId=" + InvoiceId + "&JobId=" + jobId,
                success: function (result) {
                    if (result.isValid) {
                        window.location = base_url + "Invoice/detail?Id=" + result.InvoiceId + "&JobId=" + jobId;
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');
                    }
                }
            });
        });

        // Prev Invoice
        $('#nav_prev').click(function () {
            $.ajax({
                url: base_url + "Invoice/PrevInvoice?CurrentId=" + InvoiceId + "&JobId=" + jobId,
                success: function (result) {
                    if (result.isValid) {
                        window.location = base_url + "Invoice/detail?Id=" + result.InvoiceId + "&JobId=" + jobId;
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');
                    }
                }
            });
        });

        // First Invoice
        $('#nav_first').click(function () {
            $.ajax({
                url: base_url + "Invoice/FirstInvoice?CurrentId=" + InvoiceId + "&JobId=" + jobId,
                success: function (result) {
                    if (result.isValid) {
                        window.location = base_url + "Invoice/detail?Id=" + result.InvoiceId + "&JobId=" + jobId;
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');
                    }
                }
            });
        });

        // Last Invoice
        $('#nav_last').click(function () {
            $.ajax({
                url: base_url + "Invoice/LastInvoice?CurrentId=" + InvoiceId + "&JobId=" + jobId,
                success: function (result) {
                    if (result.isValid) {
                        window.location = base_url + "Invoice/detail?Id=" + result.InvoiceId + "&JobId=" + jobId;
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');
                    }
                }
            });
        });
    });