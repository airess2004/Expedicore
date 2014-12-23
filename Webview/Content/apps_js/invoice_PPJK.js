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
    ShipperMode();
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
            url: base_url + "Invoice/GetInvoiceInfo?Id=" + InvoiceId + "&JobId=" + jobId,
            success: function (result) {
                if (!result.isValid)
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "Invoice?type=" + jobId;
                    });
                else {
                    shipmentId = result.objJob.ShipmentId;
                    jobNumber = result.objJob.JobNumber;
                    subJobNumber = result.objJob.SubJobNo;
                    jobCode = result.objJob.JobCode;
                    companycode = result.objJob.CompanyCode;
                    intcompany = result.objJob.IntCompany;
                    $("#InvoicesNo").val(result.objJob.InvoicesNo);
                    $("#txtSEetd").val(dateEnt(result.objJob.ETDETA));
                    $("#txtSEduedate").val(dateEnt(result.objJob.DueDate));
                    $("#txtSEshipmentno").val(result.objJob.ShipmentNo);
                    $("#txtSEprinting").val(result.objJob.Printing);
                    $("#txtSEjobowner").val(result.objJob.JobOwner);
                    $("#txtSEdateprint").val(dateEnt(result.objJob.DatePrint));
                    $("#txtSEinvoicefromagentno").val(result.objJob.InvoicesAgent);
                    $("#ddlSEinvoiceheader").val(result.objJob.InvHeader);

                    $("#optSEDCcredit").attr("checked", "checked");
                    if (result.objJob.DebetCredit == 'D')
                        $("#optSEDCdebet").attr("checked", "checked");

                    $("#optSEpaymentfrompr").attr("checked", "checked");
                    if (result.objJob.InvoiceType == "G")
                        $("#optSEpaymentfromgpr").attr("checked", "checked");

                    switch (result.objJob.InvoicesTo) {
                        case "CU": $("#rbInvoicesToShipper").attr("checked", "checked");
                            $("#ddlcurrency").val('USD');
                            ShipperMode();
                            break;
                        case "CI": $("#rbInvoicesToShipper").attr("checked", "checked");
                            $("#ddlcurrency").val('IDR');
                            ShipperMode();
                            break;
                        case "CM": $("#rbInvoicesToShipper").attr("checked", "checked");
                            $("#ddlcurrency").val('');
                            ShipperMode();
                            break;
                        case "AG": $("#rbInvoicesToAgent").attr("checked", "checked");
                            AgentMode();
                            break;
                    }

                    $("#optSEDCcredit").attr("checked", "checked");
                    if (result.objJob.DebetCredit == 'D')
                        $("#optSEDCdebet").attr("checked", "checked");

                    $("#txtSEkdcustomer").val(result.objJob.CustomerCode).data('kode', result.objJob.CustomerId);
                    $("#txtSEnmcustomer").val(result.objJob.CustomerName);
                    $("#txtSEcustomeraddress").text(result.objJob.CustomerAddress);

                    $("#txtSEkdbillto").val(result.objJob.BillCode).data('kode', result.objJob.BillId);
                    $("#txtSEnmbillto").val(result.objJob.BillName);
                    $("#txtSEbilltoaddress").text(result.objJob.BillAddress);

                    $("#rbTotalUSDNotPaid").attr("checked", "checked");
                    if (result.objJob.PaidUSD)
                        $("#rbTotalUSDPaid").attr("checked", "checked");

                    $("#rbTotalIDRNotPaid").attr("checked", "checked");
                    if (result.objJob.PaidIDR)
                        $("#rbTotalIDRPaid").attr("checked", "checked");

                    $("#txtSEpaymentidr").numberbox('setValue', result.objJob.PaymentIDR);
                    $("#txtSEpaymentusd").numberbox('setValue', result.objJob.PaymentUSD);
                    $("#txtSErate").numberbox('setValue', result.objJob.Rate);
                    $("#txtSEdaterate").val(dateEnt(result.objJob.DateRate));

                    // Check TypeEPL
                    if (parseInt(result.objJob.TypeEPL) < 9 || parseInt(result.objJob.TypeEPL) == 13) {
                        // Check Container have 20', 40' 45'
                        var containerSizeList = result.objJob.ContainerSizeList;
                        if (containerSizeList != null || containerSizeList != undefined) {
                            for (var i = 0; i < containerSizeList.length; i++) {
                                switch (containerSizeList[i].SizeType) {
                                    case 1:   // Container 20'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            vContainerCount20 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                    case 2:   // Container 40'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            vContainerCount40 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                    case 3:   // Container 45'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            vContainerCount45 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                }
                            }
                        }
                    }


                    // Invoice Detail
                    if (result.objJob.ListInvoiceDetail != null) {
                        if (result.objJob.ListInvoiceDetail.length > 0) {
                            for (var i = 0; i < result.objJob.ListInvoiceDetail.length; i++) {
                                addInvDetail(0,
                                    result.objJob.ListInvoiceDetail[i].InvoiceDetailId,
                                    result.objJob.ListInvoiceDetail[i].EPLDetailId,
                                    result.objJob.ListInvoiceDetail[i].AccountCode,
                                    result.objJob.ListInvoiceDetail[i].AccountName, result.objJob.ListInvoiceDetail[i].Description,
                                    result.objJob.ListInvoiceDetail[i].Type, result.objJob.ListInvoiceDetail[i].AmountCrr,
                                    result.objJob.ListInvoiceDetail[i].Amount,
                                    result.objJob.ListInvoiceDetail[i].Quantity, result.objJob.ListInvoiceDetail[i].PerQty,
                                    result.objJob.ListInvoiceDetail[i].CodingQuantity, result.objJob.ListInvoiceDetail[i].Sign,
                                    result.objJob.ListInvoiceDetail[i].HFPSinfo, result.objJob.ListInvoiceDetail[i].PercentVat, result.objJob.ListInvoiceDetail[i].VatId,
                                    result.objJob.ListInvoiceDetail[i].Dmr, result.objJob.ListInvoiceDetail[i].DmrContainerDetail, result.objJob.ListInvoiceDetail[i].DmrContainer);
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
                    $("#optSEDCdebet").attr("disabled", "disabled");
                    $("#optSEDCcredit").attr("disabled", "disabled");
                    $("#ddlcurrency").attr("disabled", "disabled");

                    // Disable Button for Printed or Cancellation Invoice
                    if (result.objJob.Printing > 0 || (result.objJob.LinkTo != null && result.objJob.LinkTo != '' && result.objJob.LinkTo.substr(0, 6).toLowerCase() == 'cancel')) {
                        // Disable Save 
                        $('#seaimport_form_btn_save').removeAttr('onclick').unbind('click').off('click');
                        $('#btn_add_invoice_detail').attr('disabled', 'disabled');
                        $('#btn_edit_invoice_detail').attr('disabled', 'disabled');
                        $('#btn_delete_invoice_detail').attr('disabled', 'disabled');
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
        lookupGrid.setGridParam({ url: base_url + 'ShipmentOrder/LookUpInvoice', postData: { filters: null, job: jobId }, page: 'last' }).trigger("reloadGrid");
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

            $('#btnSEcustomer')
                .data('shippercode', ret.shipperid).data('shippercode2', ret.shippercode).data('shippername', ret.shippername).data('shipperaddress', ret.shipperaddress)
                .data('agentcode', ret.agentid).data('agentcode2', ret.agentcode).data('agentname', ret.agentname).data('agentaddress', ret.agentaddress);
            if (isAgent) {
                $('#txtSEkdcustomer').val(ret.agentcode).data('kode', ret.agentid);
                $('#txtSEnmcustomer').val(ret.agentname);
                $('#txtSEcustomeraddress').val(ret.agentaddress);
            }
            else {
                $('#txtSEkdcustomer').val(ret.shippercode).data('kode', ret.shipperid);
                $('#txtSEnmcustomer').val(ret.shippername);
                $('#txtSEcustomeraddress').val(ret.shipperaddress);
            }

            $('#txtSEjobowner').val(ret.jobowner);
            jobNumber = ret.jobnumber;
            subJobNumber = ret.subjobno;
            jobCode = ret.jobcode;
            shipmentId = id;

            // Set Container Size
            vContainerCount20 = 0;
            vContainerCount40 = 0;
            vContainerCount45 = 0;

            var containerSizeList = $.parseJSON(ret.containersizelist);
            if (parseInt(ret.typeepl) < 9 || parseInt(ret.typeepl) == 13) {
                if (containerSizeList != null || containerSizeList != undefined) {
                    for (var i = 0; i < containerSizeList.length; i++) {
                        switch (containerSizeList[i].SizeType) {
                            case 1:   // Container 20'
                                if (parseInt(containerSizeList[i].SizeCount) > 0) {

                                    vContainerCount20 = parseInt(containerSizeList[i].SizeCount);
                                }
                                break;
                            case 2:   // Container 40'
                                if (parseInt(containerSizeList[i].SizeCount) > 0) {

                                    vContainerCount40 = parseInt(containerSizeList[i].SizeCount);
                                }
                                break;
                            case 3:   // Container 45'
                                if (parseInt(containerSizeList[i].SizeCount) > 0) {

                                    vContainerCount45 = parseInt(containerSizeList[i].SizeCount);
                                }
                                break;
                        }
                    }
                }
            }

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
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Shipment Order', 'Principle By', 'ETD', 'Consignee', 'Agent', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        colModel: [{ name: 'shipmentorderno', index: 'shipmentno', width: 130, align: "center" },
                  { name: 'jobowner', index: 'jobowner', width: 80 },
                  { name: 'etd', index: 'feederetd', width: 80, align: "right", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'shippername', index: 'shippername' },
                  { name: 'agentname', index: 'agentname' },
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
        sortname: "shipmentno",
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
        $('#lookup_table_shipper_div').hide();
        $('#lookup_table_agent_div').hide();
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'EPL/LookUpCustomerGeneral';

        //var lookUpURL = base_url + 'EPL/LookUpCustomer';
        //if ($('#optSEpaymentfromgpr').is(':checked')) {
        //    lookUpURL = base_url + 'EPL/LookUpCustomerGeneral';
        //}

        var custType = "";
        var debetCredit = "";
        if ($('#rbInvoicesToShipper').is(':checked'))
            custType = "CO";
        if ($('#rbInvoicesToAgent').is(':checked')) {
            custType = "AG";
            debetCredit = "D";
            if ($('#optSEDCcredit').is(':checked')) {
                debetCredit = "C";
            }
        }

        if (isAgent == true) {
            var lookupGrid = $('#lookup_table_agent');
            lookupGrid.setGridParam({
                postData: { 'shipmentId': function () { return shipmentId; }, 'custType': function () { return custType; }, 'debetCredit': function () { return debetCredit; } },
                url: lookUpURL
            }).trigger("reloadGrid");
            $('#lookup_table_agent_div').show();
        } else {
            var lookupGrid = $('#lookup_table_shipper');
            lookupGrid.setGridParam({
                postData: { 'shipmentId': function () { return shipmentId; }, 'custType': function () { return custType; } },
                url: lookUpURL
            }).trigger("reloadGrid");
            $('#lookup_table_shipper_div').show();
        }

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
        if (isAgent == false) {
            id = jQuery("#lookup_table_shipper").jqGrid('getGridParam', 'selrow');
        }
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

    jQuery("#lookup_table_shipper").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Address'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: 'right' },
                  { name: 'name', index: 'contactname', width: 200 },
                    { name: 'address', index: 'contactaddress', width: 200 }],
        pager: jQuery('#lookup_pager_shipper'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_shipper").jqGrid('navGrid', '#toolbar_lookup_table_shipper', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipper

    jQuery("#lookup_table_agent").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Address'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: 'right' },
                  { name: 'name', index: 'contactname', width: 200 },
                    { name: 'address', index: 'contactaddress', width: 200 }],
        pager: jQuery('#lookup_pager_agent'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_agent").jqGrid('navGrid', '#toolbar_lookup_table_shipper', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- End Customer (Agent / Shipper) ----------------------------------------------------

    // ---------------------------------------------------- Bill To ------------------------------------------------------------------------------
    // Browse
    $('#btnSEbillto').click(function () {
        $('#lookup_table_billto_shipper_div').hide();
        $('#lookup_table_billto_agent_div').hide();
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'EPL/LookUpCustomer';
        if ($('#optSEpaymentfromgpr').is(':checked')) {
            lookUpURL = base_url + 'EPL/LookUpCustomerGeneral';
        }
        var custType = "";
        var debetCredit = "";
        if ($('#rbInvoicesToShipper').is(':checked'))
            custType = "CO";
        if ($('#rbInvoicesToAgent').is(':checked')) {
            custType = "AG";
            debetCredit = "D";
            if ($('#optSEDCcredit').is(':checked')) {
                debetCredit = "C";
            }
        }

        if (isAgent == true) {
            var lookupGrid = $('#lookup_table_billto_agent');
            lookupGrid.setGridParam({
                postData: { 'shipmentId': function () { return shipmentId; }, 'custType': function () { return custType; }, 'debetCredit': function () { return debetCredit; } },
                url: lookUpURL
            }).trigger("reloadGrid");
            $('#lookup_table_billto_agent_div').show();
        } else {
            var lookupGrid = $('#lookup_table_billto_shipper');
            lookupGrid.setGridParam({
                postData: { 'shipmentId': function () { return shipmentId; }, 'custType': function () { return custType; } },
                url: lookUpURL
            }).trigger("reloadGrid");
            $('#lookup_table_billto_shipper_div').show();
        }

        $('#lookup_div_billto').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_billto').click(function () {
        $('#lookup_div_billto').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_billto').click(function () {
        // If Agent Or Shipper
        var id = jQuery("#lookup_table_billto_agent").jqGrid('getGridParam', 'selrow');
        if (isAgent == false) {
            id = jQuery("#lookup_table_billto_shipper").jqGrid('getGridParam', 'selrow');
        }
        if (id) {
            // If Agent Or Shipper
            var ret = jQuery("#lookup_table_billto_agent").jqGrid('getRowData', id);
            if (isAgent == false) {
                ret = jQuery("#lookup_table_billto_shipper").jqGrid('getRowData', id);
            }

            if ($('#txtSEkdcustomer').data('kode') == id) {
                $.messager.alert('Information', 'This code already in use...', 'info');
            }
            else {
                $('#txtSEkdbillto').val(ret.code).data("kode", id);
                $('#txtSEnmbillto').val(ret.name);
                $('#txtSEbilltoaddress').val(ret.address);

                // Auto Populate Account Invoice
                PopulateAccountInvoiceDefault(id, $('#txtSEkdcustomer').data('kode'));
                // END Auto Populate Account Invoice

            }

            $('#lookup_div_billto').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_billto_shipper").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 }
        ],
        pager: jQuery('#lookup_pager_shipper'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_billto_shipper").jqGrid('navGrid', '#toolbar_lookup_table_shipper', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipper

    jQuery("#lookup_table_billto_agent").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 }
        ],
        pager: jQuery('#lookup_pager_agent'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_billto_agent").jqGrid('navGrid', '#toolbar_lookup_table_shipper', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- END Bill To ------------------------------------------------------------------------------

    // ---------------------------------------------------- Account ------------------------------------------------------------------------------
    // Browse
    $('#btnInvaccount').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var vBillTo = false;
        var custType = "xxx";
        var debetCredit = "xxx";
        var incomeAgent = 0;

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

        var lookupGrid = $('#lookup_table_account');
        $('#lookup_table_account_div').show();
        $('#lookup_table_account_general_div').hide();

        // Invoice
        var accountUrl = 'EPL/GetEPLAccountInvoice';
        if ($('#optSEpaymentfromgpr').is(':checked')) {
            // General Invoice
            accountUrl = 'CostSalesSea/LookUpCostSalesSea';
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
                filters: null, 'shipmentId': function () { return shipmentId; },
                'custType': function () { return custType; }, 'customerId': function () { return customerId; }, 'billId': function () { return billId; },
                'debetCredit': function () { return debetCredit; },
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

            // Default
            $('#optInvcontainersize20').attr('disabled', 'disabled');
            $('#optInvcontainersize40').attr('disabled', 'disabled');
            $('#optInvcontainersize45').attr('disabled', 'disabled');
            // Remove Disabled
            $('#txtISquantity').removeAttr('disabled');
            $('#txtISamount').removeAttr('disabled');

            // Container Size Selected
            $('#optInvcontainersizeall').attr("disabled", "disabled");
            $('#optInvcontainersizeall').attr("checked", "checked");
            if (ret.type == 20)
                $('#optInvcontainersize20').removeAttr('disabled');
            if (ret.type == 40)
                $('#optInvcontainersize40').removeAttr('disabled');
            if (ret.type == 45)
                $('#optInvcontainersize45').removeAttr('disabled');
            // END Container Size Selected

            // CodingQuantity
            if (ret.codingquantity == "true") {

                // General
                if ($('#optSEpaymentfromgpr').is(':checked')) {

                    // Disable Size All
                    $('#optInvcontainersizeall').attr('disabled', 'disabled');
                    // Container 20'
                    if (vContainerCount20 > 0) {
                        $('#optInvcontainersize20').removeAttr('disabled');

                        //// Set Disabled
                        //$('#txtInvquantity').attr('disabled', 'disabled');
                        //$('#txtInvamount').attr('disabled', 'disabled');
                    }
                    // Container 40'
                    if (vContainerCount40 > 0) {
                        $('#optInvcontainersize40').removeAttr('disabled');

                        //// Set Disabled
                        //$('#txtInvquantity').attr('disabled', 'disabled');
                        //$('#txtInvamount').attr('disabled', 'disabled');
                    }
                    // Container 45'
                    if (vContainerCount45 > 0) {
                        $('#optInvcontainersize45').removeAttr('disabled');

                        //// Set Disabled
                        //$('#txtInvquantity').attr('disabled', 'disabled');
                        //$('#txtInvamount').attr('disabled', 'disabled');
                    }
                }

                $('#txtInvquantity').numberbox('setValue', 0);

                if ($('#optInvcontainersizem3').is(":not(:disabled)"))
                    $('#optInvcontainersizem3').attr("checked", "checked");
                else if ($('#optInvcontainersize20').is(":not(:disabled)")) {
                    $('#optInvcontainersize20').attr("checked", "checked");
                    $('#txtInvquantity').numberbox('setValue', vContainerCount20);
                }
                else if ($('#optInvcontainersize40').is(":not(:disabled)")) {
                    $('#optInvcontainersize40').attr("checked", "checked");
                    $('#txtInvquantity').numberbox('setValue', vContainerCount40);
                }
                else if ($('#optInvcontainersize45').is(":not(:disabled)")) {
                    $('#optInvcontainersize45').attr("checked", "checked");
                    $('#txtInvquantity').numberbox('setValue', vContainerCount45);
                }
                else
                    check = false;
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
                $('#txtInvkdaccount').val(ret.code).data("kode", id).data("amountidr", ret.amountidr).data("amountusd", ret.amountusd).data("codingquantity", ret.codingquantity);
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
        colNames: ['Code', 'Name', 'desc', 'amountusd', 'amountidr', 'codingquantity', 'Crr', 'Payment', 'Type', '', '', ''],
        colModel: [{ name: 'code', index: 'accountcode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'desc', index: 'desc', width: 200, hidden: true },
                  { name: 'amountusd', index: 'a.amountusd', width: 200, hidden: true },
                  { name: 'amountidr', index: 'b.amountidr', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'codingquantity', width: 200, hidden: true },
                  { name: 'currencytype', index: 'currencytype', width: 50, hidden: false },
                  { name: 'payment', index: 'payment', width: 200, hidden: false, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'type', index: 'type', width: 50, hidden: false },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true },
                  { name: 'perqty', index: 'perqty', width: 200, hidden: true },
                  { name: 'epldetailid', index: 'epldetailid', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
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
        colModel: [{ name: 'code', index: 'accountcode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'amountusd', index: 'a.amountusd', width: 200, hidden: true },
                  { name: 'amountidr', index: 'b.amountidr', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'codingquantity', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account_general'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_account_general").jqGrid('navGrid', '#toolbar_lookup_table_account', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ---------------------------------------------------- END Account ------------------------------------------------------------------------------
    // -------------------------------------------------------------------- END LOOKUP ----------------------------------------------------

    // Auto Populate Account Invoice
    function PopulateAccountInvoiceDefault(billId, customerId) {

        // ADD Default Account Invoice From EPL
        var custType = "xxx";
        var debetCredit = "D";

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

        // Currency
        var currency = $('#ddlcurrency').val();

        // Invoice
        var accountUrl = 'EPL/GetEPLAccountInvoiceFirstTime';
        if ($('#optSEpaymentfrompr').is(':checked')) {

            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + accountUrl,
                data: JSON.stringify({
                    shipmentId: shipmentId, custType: custType, customerId: customerId, billId: billId, debetCredit: debetCredit, currency: currency
                }),
                success: function (result) {
                    if (result != null) {
                        if (result.data != null) {
                            // Clear and Reload all grid
                            $("#table_invoice_detail").jqGrid("clearGridData", true).trigger("reloadGrid");

                            for (var i = 0; i < result.data.length; i++) {
                                // default
                                var amount = result.data[i].Payment;
                                var currencyType = 0;
                                if (result.data[i].Currency == 'IDR') {
                                    currencyType = 1;
                                }
                                // default All
                                var containerType = 5;
                                if ('20' == result.data[i].strType)
                                    containerType = 1;
                                if ('40' == result.data[i].strType)
                                    containerType = 2;
                                if ('45' == result.data[i].strType)
                                    containerType = 3;
                                if ('M3' == result.data[i].strType)
                                    containerType = 4;

                                addInvDetail(0, 0, result.data[i].EPLDetailId, result.data[i].AccountID, result.data[i].AccountName, result.data[i].Description,
                                    containerType, currencyType, amount,
                                    result.data[i].Quantity, result.data[i].Perqty, result.data[i].CodingQuantity,
                                    result.data[i].Sign, result.data[i].HFPSinfo, 0, '', result.data[i].Dmr, result.data[i].DmrContainerDetail, result.data[i].DmrContainer);

                            }

                            //// Calculate Total
                            //TotalCalculation();
                        }
                    }


                }
            });
        }
        // END ADD Default Account Invoice From EPL

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

    // Choose Consignee / Agent
    $('input[name=rbInvoicesTo]').click(function () {
        var id = $(this).attr('id');

        $('#optSEDCdebet').attr('disabled', 'disabled');
        $('#optSEDCcredit').attr('disabled', 'disabled');
        if ($('#rbInvoicesToAgent').is(':checked')) {
            AgentMode();

            $('#txtSEkdcustomer').val($('#btnSEcustomer').data('agentcode2')).data('kode', $('#btnSEcustomer').data('agentcode'));
            $('#txtSEnmcustomer').val($('#btnSEcustomer').data('agentname'));
            $('#txtSEcustomeraddress').val($('#btnSEcustomer').data('agentaddress'));

            $('#optSEDCdebet').removeAttr('disabled');
            $('#optSEDCcredit').removeAttr('disabled');
        }
        else {
            ShipperMode();

            $('#txtSEkdcustomer').val($('#btnSEcustomer').data('shippercode2')).data('kode', $('#btnSEcustomer').data('shippercode'));
            $('#txtSEnmcustomer').val($('#btnSEcustomer').data('shippername'));
            $('#txtSEcustomeraddress').val($('#btnSEcustomer').data('shipperaddress'));

            $('#optSEDCdebet').attr('checked', 'checked');
        }

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

    function AgentMode() {
        $('#rbInvoicesToAgent').attr('checked', 'checked');
        $('#lblSEinvoicefromagentno').show();
        $('#txtSEinvoicefromagentno').show();
        $('#spaninvoiceto').text('Agent');
        $('#btn_add_invoice_detail_agent').show();
        isAgent = true;
        isShipper = false;
        $('#lblInvinvoicesto').text('AGENT');
        $('#lblInvagentshipper').text('Agent');
        $('#rowVat').hide();

        $('#optSEDCdebet').removeAttr('disabled');
        $('#optSEDCcredit').removeAttr('disabled');
    }

    function ShipperMode() {
        $('#rbInvoicesToShipper').attr('checked', 'checked');
        $('#lblSEinvoicefromagentno').hide();
        $('#txtSEinvoicefromagentno').hide();
        $('#spaninvoiceto').text('Consignee');
        $('#btn_add_invoice_detail_agent').hide();
        isAgent = false;
        isShipper = true;
        $('#lblInvinvoicesto').text('CONSIGNEE');
        $('#lblInvagentshipper').text('Consignee');
        $('#rowVat').show();

        $('#optSEDCdebet').attr('disabled', 'disabled');
        $('#optSEDCcredit').attr('disabled', 'disabled');
        $('#optSEDCdebet').attr('checked', 'checked');
    }

    // Calculation ------------------------------------------------------------------------------------------------
    function TotalCalculation() {
        var totalUSD = 0;
        var totalIDR = 0;
        var totalVatUSD = 0;
        var totalVatIDR = 0;
        var data = $("#table_invoice_detail").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            if (data[i].accountid != '003' && data[i].accountid != '004') { // Ignore Buying Rate and Selling Rate
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



    // Inc Agent Calculation ------------------------------------------------------------------------------------------------
    $("#txtInvquantity, #txtInvperunitcost").blur(function () {
        var total = parseFloat($('#txtInvquantity').numberbox('getValue')) * parseFloat($('#txtInvperunitcost').numberbox('getValue'));
        total = Math.round(total * 100) / 100;
        $('#txtInvamount').numberbox('setValue', total);
    });
    //$("#txtInvamount").blur(function () {
    //    $('#txtInvquantity').numberbox('setValue', 0);
    //    $('#txtInvperunitcost').numberbox('setValue', 0);
    //});
    // END Inc Agent Calculation ------------------------------------------------------------------------------------------------

    // Inc Agent - Handling Fee Calculation ------------------------------------------------------------------------------------------------
    $("#txtInv_handlingfee20, #txtInv_handlingfee20usd, #txtInv_handlingfee40, #txtInv_handlingfee40usd, #txtInv_handlingfee45, #txtInv_handlingfee45usd, " +
        "#txtInv_handlingfeem3, #txtInv_handlingfeem3usd").blur(function () {
            IncAgentHFCalculation();
        });

    function IncAgentHFCalculation() {
        var hf20total = (Math.round(parseFloat($('#txtInv_handlingfee20').numberbox('getValue')) * parseFloat($('#txtInv_handlingfee20usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_handlingfee20total').numberbox('setValue', hf20total);

        var hf40total = (Math.round(parseFloat($('#txtInv_handlingfee40').numberbox('getValue')) * parseFloat($('#txtInv_handlingfee40usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_handlingfee40total').numberbox('setValue', hf40total);

        var hf45total = (Math.round(parseFloat($('#txtInv_handlingfee45').numberbox('getValue')) * parseFloat($('#txtInv_handlingfee45usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_handlingfee45total').numberbox('setValue', hf45total);

        var hfm3total = (Math.round(parseFloat($('#txtInv_handlingfeem3').numberbox('getValue')) * parseFloat($('#txtInv_handlingfeem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_handlingfeem3total').numberbox('setValue', hfm3total);
    }
    // END Inc Agent - Handling Fee Calculation ------------------------------------------------------------------------------------------------

    // Inc Agent - Profit Split Calculation ------------------------------------------------------------------------------------------------
    $('#txtInv_profitsplitpercent').blur(function () {
        var percentAmount = (parseFloat($('#txtInv_profitsplittotal').numberbox('getValue')) * parseFloat($('#txtInv_profitsplitpercent').numberbox('getValue'))) / 100;
        $('#txtInv_profitsplitpercentamount').numberbox('setValue', percentAmount);

        IncAgentProfitSplitCalculation();
    });
    function IncAgentProfitSplitCalculation() {
        var brTotal = parseFloat($('#txtInv_buyingrate20total').numberbox('getValue')) + parseFloat($('#txtInv_buyingrate40total').numberbox('getValue')) + parseFloat($('#txtInv_buyingrate45total').numberbox('getValue')) + parseFloat($('#txtInv_buyingratem3total').numberbox('getValue'));
        var srTotal = parseFloat($('#txtInv_sellingrate20total').numberbox('getValue')) + parseFloat($('#txtInv_sellingrate40total').numberbox('getValue')) + parseFloat($('#txtInv_sellingrate45total').numberbox('getValue')) + parseFloat($('#txtInv_sellingratem3total').numberbox('getValue'));
        var psTotal = parseFloat(srTotal) - parseFloat(brTotal);
        $('#txtInv_profitsplittotal').numberbox('setValue', psTotal);
        var percentAmount = (Math.round(parseFloat($('#txtInv_profitsplittotal').numberbox('getValue')) * parseFloat($('#txtInv_profitsplitpercent').numberbox('getValue')) * 100) / 100) / 100;
        $('#txtInv_profitsplitpercentamount').numberbox('setValue', percentAmount);
        var psTotalPayment = psTotal - percentAmount;
        $('#txtInv_profitsplittotalpayment').numberbox('setValue', psTotalPayment);

    }
    // Buying Rate Calculation
    $("#txtInv_buyingrate20, #txtInv_buyingrate20usd, #txtInv_buyingrate40, #txtInv_buyingrate40usd, #txtInv_buyingrate45, #txtInv_buyingrate45usd, " +
        "#txtInv_buyingratem3, #txtInv_buyingratem3usd").blur(function () {
            IncAgentPSBuyingRateCalculation();
            IncAgentProfitSplitCalculation();
        });
    function IncAgentPSBuyingRateCalculation() {
        var br20total = (Math.round(parseFloat($('#txtInv_buyingrate20').numberbox('getValue')) * parseFloat($('#txtInv_buyingrate20usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_buyingrate20total').numberbox('setValue', br20total);

        var br40total = (Math.round(parseFloat($('#txtInv_buyingrate40').numberbox('getValue')) * parseFloat($('#txtInv_buyingrate40usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_buyingrate40total').numberbox('setValue', br40total);

        var br45total = (Math.round(parseFloat($('#txtInv_buyingrate45').numberbox('getValue')) * parseFloat($('#txtInv_buyingrate45usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_buyingrate45total').numberbox('setValue', br45total);

        var brm3total = (Math.round(parseFloat($('#txtInv_buyingratem3').numberbox('getValue')) * parseFloat($('#txtInv_buyingratem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_buyingratem3total').numberbox('setValue', brm3total);
    }
    // Selling Rate Calculation
    $("#txtInv_sellingrate20, #txtInv_sellingrate20usd, #txtInv_sellingrate40, #txtInv_sellingrate40usd, #txtInv_sellingrate45, #txtInv_sellingrate45usd, " +
        "#txtInv_sellingratem3, #txtInv_sellingratem3usd").blur(function () {
            IncAgentPSSellingRateCalculation();
            IncAgentProfitSplitCalculation();
        });
    function IncAgentPSSellingRateCalculation() {
        var sr20total = (Math.round(parseFloat($('#txtInv_sellingrate20').numberbox('getValue')) * parseFloat($('#txtInv_sellingrate20usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_sellingrate20total').numberbox('setValue', sr20total);

        var sr40total = (Math.round(parseFloat($('#txtInv_sellingrate40').numberbox('getValue')) * parseFloat($('#txtInv_sellingrate40usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_sellingrate40total').numberbox('setValue', sr40total);

        var sr45total = (Math.round(parseFloat($('#txtInv_sellingrate45').numberbox('getValue')) * parseFloat($('#txtInv_sellingrate45usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_sellingrate45total').numberbox('setValue', sr45total);

        var srm3total = (Math.round(parseFloat($('#txtInv_sellingratem3').numberbox('getValue')) * parseFloat($('#txtInv_sellingratem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtInv_sellingratem3total').numberbox('setValue', srm3total);
    }
    // END Inc Agent - Profit Split Calculation ------------------------------------------------------------------------------------------------


    // DisableOnEditNonAgent
    function disableOnEditAgentShipper() {
        $('#btnInvshipper').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvssline').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvemkl').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvrebate').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvdepo').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnInvaccount').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        //$('#optInvcontainersize20').attr('disabled', 'disabled');
        //$('#optInvcontainersize40').attr('disabled', 'disabled');
        //$('#optInvcontainersize45').attr('disabled', 'disabled');
        //$('#optInvcontainersizem3').attr('disabled', 'disabled');
        //$('#optInvcontainersizeall').attr('disabled', 'disabled');
        //$('#optInvcurrencytypeIDR').attr('disabled', 'disabled');
        //$('#optInvcurrencytypeUSD').attr('disabled', 'disabled');
    }
    // EnableOnAddNonAgent
    function enableOnEditAgentShipper() {
        $('#btnInvshipper').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvssline').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvemkl').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvrebate').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvdepo').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnInvaccount').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        //$('#optInvcontainersize20').removeAttr('disabled');
        //$('#optInvcontainersize40').removeAttr('disabled');
        //$('#optInvcontainersize45').removeAttr('disabled');
        //$('#optInvcontainersizem3').removeAttr('disabled');
        //$('#optInvcontainersizeall').removeAttr('disabled');
        //$('#optInvcurrencytypeIDR').removeAttr('disabled');
        //$('#optInvcurrencytypeUSD').removeAttr('disabled');
    }

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

        // Profit Split
        //$('#txtInvkdagent_profitsplit').val('').data('kode', '');
        //$('#txtInvnmagent_profitsplit').val('');
        $('input[id*=txtInv_sellingrate]').numberbox('setValue', 0);
        $('input[id*=txtInv_buyingrate]').numberbox('setValue', 0);
        $('input[id*=txtInv_profitsplit]').numberbox('setValue', 0);

        // Handling Fee
        //$('#txtInvkdagent_handlingfee').val('').data('kode', '');
        //$('#txtInvnmagent_handlingfee').val('');
        $('input[id*=txtInv_handlingfee]').numberbox('setValue', 0);
    }

    function insertAccountHfps(ret) {
        // default
        var amount = ret.amountusd;
        var currency = 0;
        if (ret.currencytype == 'IDR') {
            amount = ret.amountidr;
            currency = 1;
        }
        // default All
        var containerType = 5;
        if ('20' == ret.type)
            containerType = 1;
        if ('40' == ret.type)
            containerType = 2;
        if ('45' == ret.type)
            containerType = 3;
        if ('M3' == ret.type)
            containerType = 4;


        if ((InvoiceId != undefined && InvoiceId != "")) {
            var submitURL = base_url + "Invoice/InsertDetail";
            var isValid = true;
            var message = "OK";

            // Add or Edit
            SaveInvoiceDetail(submitURL, 0, ret.epldetailid, $('#txtSEkdcustomer').data('kode'), ret.code, ret.name, ret.desc, 0, 0, 5,
                            0, ret.sign, 0, amount, 0, 0, JSON.stringify(ret.hfpsinfo), function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    invoiceDetailId = data.objResult.InvoiceDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addInvDetail(0, 0, ret.epldetailid, ret.code, ret.name, ret.desc, ret.type, currency, amount, ret.quantity, ret.perqty, ret.codingquantity, ret.sign,
                    ret.hfpsinfo, 0);
            }
        }
        else {
            addInvDetail(0, 0, ret.epldetailid, ret.code, ret.name, ret.desc, ret.type, currency, amount, ret.quantity, ret.perqty, ret.codingquantity, ret.sign,
                ret.hfpsinfo, 0);
        }

    }

    // Cancel or CLose
    $('#lookup_btn_cancel_account_hfps').click(function () {
        $('#lookup_div_account_hfps').dialog('close');
    });

    // ADD or Select Data - Handling Fee & Profit Share from EPL
    $('#lookup_btn_add_account_hfps').click(function () {
        var accountGrid = jQuery("#lookup_table_account_hfps");
        var id = accountGrid.jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = accountGrid.jqGrid('getRowData', id);

            var counterprofitsplit = 0;
            if (ret.code == "003" || ret.code == "004" || ret.code == "006") {
                counterprofitsplit = ret.counterprofitsplit;

                // Get Selling Rate, Buying Rate and Profit Share
                var ids = accountGrid.jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var datagrid = accountGrid.jqGrid('getRowData', ids[i]);
                    if (datagrid.counterprofitsplit == counterprofitsplit)
                        insertAccountHfps(datagrid);
                }
            }
            else {
                insertAccountHfps(ret);
            }

            clearInvAgentShipper();
            $('#lookup_div_account_hfps').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_account_hfps").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'desc', 'amountusd', 'amountidr', 'codingquantity', 'Crr', 'Payment', 'Type', '', '', '', '', '', ''],
        colModel: [{ name: 'code', index: 'accountcode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'desc', index: 'desc', width: 200, hidden: true },
                  { name: 'amountusd', index: 'a.amountusd', width: 200, hidden: true },
                  { name: 'amountidr', index: 'b.amountidr', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'codingquantity', width: 200, hidden: true },
                  { name: 'currencytype', index: 'currencytype', width: 50, hidden: false },
                  { name: 'payment', index: 'payment', width: 200, hidden: false, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'type', index: 'type', width: 50, hidden: false },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true },
                  { name: 'perqty', index: 'perqty', width: 200, hidden: true },
                  { name: 'epldetailid', index: 'epldetailid', width: 200, hidden: true },
                  { name: 'sign', index: 'sign', width: 30, align: 'center', hidden: true },
                  { name: 'hfpsinfo', index: 'hfpsinfo', width: 200, hidden: true },
                  { name: 'counterprofitsplit', index: 'counterprofitsplit', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account_hfps'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_account_hfps").jqGrid('navGrid', '#lookup_table_account_hfps', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Add HF
    $('#btn_add_invoice_detail_handlingfee').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Clear Input
        clearInvAgentShipper();
        enableOnEditAgentShipper();
        vInvoiceDetailEdit = 0;

        var custType = "xxx";
        // Agent
        if (isAgent == true) {
            custType = "agent";
        }

        var vBillTo = false;
        var customerId = $('#txtSEkdcustomer').data("kode");
        var customerCode = $('#txtSEkdcustomer').val();
        var customerName = $('#txtSEnmcustomer').val();

        // Set BillTo as customerCode
        var billId = 0;
        var billToCode = $('#txtSEkdbillto').val();
        if (billToCode != undefined && billToCode != "") {
            customerId = $('#txtSEkdbillto').data("kode");
            customerCode = $('#txtSEkdbillto').val();
            customerName = $('#txtSEnmbillto').val();
            billId = $('#txtSEkdbillto').data("kode");
            vBillTo = true;
        }

        debetCredit = "D";
        // Consignee
        if (isShipper == true) {
            custType = "CO";
        }
        else {
            custType = "AG";
            debetCredit = "D";
            if ($('#optSEDCcredit').is(':checked'))
                debetCredit = "C";
        }

        // As General
        if ($("#optSEpaymentfromgpr").is(":checked")) {

            $('#txtInvkdagent_handlingfee').val(customerCode);
            $('#txtInvnmagent_handlingfee').val(customerName);

            $('#lookup_div_invoice_detail_handlingfee').dialog({ title: 'Handling Fee' });
            $('#lookup_div_invoice_detail_handlingfee').dialog('open');
        }
        else {

            var lookupGrid = $('#lookup_table_account_hfps');
            var accountUrl = 'EPL/GetEPLAccountInvoiceHandlingFee';
            $('#lookup_div_account_hfps').dialog({ title: 'List Handling Fee' });

            lookupGrid.setGridParam({
                url: base_url + accountUrl,
                postData: {
                    filters: null, 'shipmentId': function () { return shipmentId; },
                    'custType': function () { return custType; }, 'customerId': function () { return customerId; }, 'debetCredit': function () { return debetCredit; },
                    'billId': function () { return billId; }
                }
            }).trigger("reloadGrid");
            $('#lookup_div_account_hfps').dialog('open');
        }
    });

    // Save HF
    $('#lookup_btn_add_invoice_detail_handlingfee').click(function () {

        var sign = true;
        if ($('#optInvsignminus_handlingfee').is(':checked'))
            sign = false;

        var hfpsinfo = {};
        hfpsinfo.Feet20 = $("#txtInv_handlingfee20").numberbox('getValue');
        hfpsinfo.Rate20 = $("#txtInv_handlingfee20usd").numberbox('getValue');
        hfpsinfo.Feet40 = $("#txtInv_handlingfee40").numberbox('getValue');
        hfpsinfo.Rate40 = $("#txtInv_handlingfee40usd").numberbox('getValue');
        hfpsinfo.FeetHQ = $("#txtInv_handlingfee45").numberbox('getValue');
        hfpsinfo.RateHQ = $("#txtInv_handlingfee45usd").numberbox('getValue');
        hfpsinfo.FeetM3 = $("#txtInv_handlingfeem3").numberbox('getValue');
        hfpsinfo.RateM3 = $("#txtInv_handlingfeem3usd").numberbox('getValue');

        var hftotal = parseFloat($('#txtInv_handlingfee20total').numberbox('getValue')) + parseFloat($('#txtInv_handlingfee40total').numberbox('getValue')) + parseFloat($('#txtInv_handlingfee45total').numberbox('getValue')) + parseFloat($('#txtInv_handlingfeem3total').numberbox('getValue'));

        // ONLY Update Invoice
        if ((InvoiceId != undefined && InvoiceId != "")) {
            // Assigned Invoice Detail Id
            var invoiceDetailId = 0
            if (vInvoiceDetailEdit == 1) {
                invoiceDetailId = $('#lookup_btn_add_invoice_detail_handlingfee').data('invoicedetailid');
            }
            var eplDetailId = $('#lookup_btn_add_invoice_detail_handlingfee').data('epldetailid');

            var submitURL = (InvoiceId != undefined && InvoiceId != "") ? (vInvoiceDetailEdit == 0) ? base_url + "Invoice/InsertDetail" : base_url + "Invoice/UpdateDetail" : "";
            var isValid = true;
            var message = "OK";

            // Add or Edit
            SaveInvoiceDetail(submitURL, invoiceDetailId, eplDetailId, $('#txtSEkdcustomer').data('kode'), '005', 'Handling fee', 'Handling fee', 0, 0, 5,
                            0, sign, 0, hftotal, 0, 0, JSON.stringify(hfpsinfo), function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    invoiceDetailId = data.objResult.InvoiceDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, '005', 'Handling fee', 'Handling fee',
                    5, 0, hftotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));
            }
        }
        else {
            addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, '005', 'Handling fee', 'Handling fee',
                5, 0, hftotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));
        }


        clearInvAgentShipper();
        $('#lookup_div_invoice_detail_handlingfee').dialog('close');
    });

    // Close HF
    $('#lookup_btn_cancel_invoice_detail_handlingfee').click(function () {
        clearInvAgentShipper();
        $('#lookup_div_invoice_detail_handlingfee').dialog('close');
    });

    // Add PS
    $('#btn_add_invoice_detail_profitsplit').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Clear Input
        clearInvAgentShipper();
        enableOnEditAgentShipper();
        vInvoiceDetailEdit = 0;

        $('#lookup_btn_add_invoice_detail_profitsplit').data('counterprofitsplit', 0);

        var custType = "xxx";
        // Agent
        if (isAgent == true) {
            custType = "agent";
        }

        var vBillTo = false;
        var customerId = $('#txtSEkdcustomer').data("kode");
        var customerCode = $('#txtSEkdcustomer').val();
        var customerName = $('#txtSEnmcustomer').val();

        // Set BillTo as customerCode
        var billId = 0;
        var billToCode = $('#txtSEkdbillto').val();
        if (billToCode != undefined && billToCode != "") {
            customerId = $('#txtSEkdbillto').data("kode");
            customerCode = $('#txtSEkdbillto').val();
            customerName = $('#txtSEnmbillto').val();
            billId = $('#txtSEkdbillto').data("kode");
            vBillTo = true;
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


        // As General
        if ($("#optSEpaymentfromgpr").is(":checked")) {

            $('#txtInvkdagent_profitsplit').val(customerCode);
            $('#txtInvnmagent_profitsplit').val(customerName);

            $('#lookup_div_invoice_detail_profitsplit').dialog({ title: 'Profit Split' });
            $('#lookup_div_invoice_detail_profitsplit').dialog('open');
        }
        else {

            var lookupGrid = $('#lookup_table_account_hfps');
            var accountUrl = 'EPL/GetEPLAccountInvoiceProfitShare';
            $('#lookup_div_account_hfps').dialog({ title: 'List Profit Share' });

            lookupGrid.setGridParam({
                url: base_url + accountUrl,
                postData: {
                    filters: null, 'shipmentId': function () { return shipmentId; },
                    'custType': function () { return custType; }, 'customerId': function () { return customerId; }, 'debetCredit': function () { return debetCredit; },
                    'billId': function () { return billId; }
                }
            }).trigger("reloadGrid");
            $('#lookup_div_account_hfps').dialog('open');
        }
    });

    // Save PS
    $('#lookup_btn_add_invoice_detail_profitsplit').click(function () {

        var sign = true;
        if ($('#optInvsignminus_profitsplit').is(':checked'))
            sign = false;

        var hfpsinfo = {};
        hfpsinfo.SFeet20 = $("#txtInv_sellingrate20").numberbox('getValue');
        hfpsinfo.SRate20 = $("#txtInv_sellingrate20usd").numberbox('getValue');
        hfpsinfo.SFeet40 = $("#txtInv_sellingrate40").numberbox('getValue');
        hfpsinfo.SRate40 = $("#txtInv_sellingrate40usd").numberbox('getValue');
        hfpsinfo.SFeetHQ = $("#txtInv_sellingrate45").numberbox('getValue');
        hfpsinfo.SRateHQ = $("#txtInv_sellingrate45usd").numberbox('getValue');
        hfpsinfo.SFeetM3 = $("#txtInv_sellingratem3").numberbox('getValue');
        hfpsinfo.SRateM3 = $("#txtInv_sellingratem3usd").numberbox('getValue');

        hfpsinfo.BFeet20 = $("#txtInv_buyingrate20").numberbox('getValue');
        hfpsinfo.BRate20 = $("#txtInv_buyingrate20usd").numberbox('getValue');
        hfpsinfo.BFeet40 = $("#txtInv_buyingrate40").numberbox('getValue');
        hfpsinfo.BRate40 = $("#txtInv_buyingrate40usd").numberbox('getValue');
        hfpsinfo.BFeetHQ = $("#txtInv_buyingrate45").numberbox('getValue');
        hfpsinfo.BRateHQ = $("#txtInv_buyingrate45usd").numberbox('getValue');
        hfpsinfo.BFeetM3 = $("#txtInv_buyingratem3").numberbox('getValue');
        hfpsinfo.BRateM3 = $("#txtInv_buyingratem3usd").numberbox('getValue');
        hfpsinfo.Percentage = $("#txtInv_profitsplitpercent").numberbox('getValue');

        var brTotal = parseFloat($('#txtInv_buyingrate20total').numberbox('getValue')) + parseFloat($('#txtInv_buyingrate40total').numberbox('getValue')) + parseFloat($('#txtInv_buyingrate45total').numberbox('getValue')) + parseFloat($('#txtInv_buyingratem3total').numberbox('getValue'));
        var srTotal = parseFloat($('#txtInv_sellingrate20total').numberbox('getValue')) + parseFloat($('#txtInv_sellingrate40total').numberbox('getValue')) + parseFloat($('#txtInv_sellingrate45total').numberbox('getValue')) + parseFloat($('#txtInv_sellingratem3total').numberbox('getValue'));

        var psTotal = parseFloat($('#txtInv_profitsplittotalpayment').numberbox('getValue'));

        // Assigned Invoice Detail Id
        var invoiceDetailId = 0
        if (vInvoiceDetailEdit == 1) {
            invoiceDetailId = $('#lookup_btn_add_invoice_detail_profitsplit').data('invoicedetailid');
        }
        var eplDetailId = $('#lookup_btn_add_invoice_detail_profitsplit').data('epldetailid');

        var submitURL = (InvoiceId != undefined && InvoiceId != "") ? (vInvoiceDetailEdit == 0) ? base_url + "Invoice/InsertDetail" : base_url + "Invoice/UpdateDetail" : "";
        var isValid = true;
        var message = "OK";
        //var amountvat = 0;
        //if ($('#ddlSEvat').val() != '') {
        //    amountvat = parseFloat(($('#ddlSEvat').val() / 100) * amount);
        //    amountvat = Math.round(amountvat * 100) / 100;
        //}

        addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, '-1', 'PROFIT SHARE', 'PROFIT SHARE', 5, 0,
            srTotal + "^" + brTotal + "^" + psTotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo), 0);

        clearInvAgentShipper();
        $('#lookup_div_invoice_detail_profitsplit').dialog('close');
    });

    // Close PS
    $('#lookup_btn_cancel_invoice_detail_profitsplit').click(function () {
        clearInvAgentShipper();
        $('#lookup_div_invoice_detail_profitsplit').dialog('close');
    });

    // Add Demurrage
    $('#btn_demurrage_invoice_detail').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Clear Input
        clearInvAgentShipper();
        enableOnEditAgentShipper();
        ClearDemurrage();
        vInvoiceDetailEdit = 0;

        var custType = "xxx";
        // Agent
        if (isAgent == true) {
            custType = "agent";
        }

        var vBillTo = false;
        var customerId = $('#txtSEkdcustomer').data("kode");
        var customerCode = $('#txtSEkdcustomer').val();
        var customerName = $('#txtSEnmcustomer').val();

        // Set BillTo as customerCode
        var billToCode = $('#txtSEkdbillto').val();
        if (billToCode != undefined && billToCode != "") {
            customerId = $('#txtSEkdbillto').data("kode");
            customerCode = $('#txtSEkdbillto').val();
            customerName = $('#txtSEnmbillto').val();
            vBillTo = true;
        }

        debetCredit = "D";
        // Consignee
        if (isShipper == true) {
            custType = "CO";
        }
        else {
            custType = "AG";
            debetCredit = "D";
            if ($('#optSEDCcredit').is(':checked'))
                debetCredit = "C";
        }

        // As General
        if ($("#optSEpaymentfromgpr").is(":checked")) {

            $('#txtDemkddemurrage').val(customerCode);
            $('#txtDemnmdemurrage').val(customerName);

            $('#lookup_div_demurrage').dialog({ title: 'Demurrage' });
            $('#lookup_div_demurrage').dialog('open');
        }
        else {

            var lookupGrid = $('#lookup_table_account_dem');
            var accountUrl = 'EPL/GetEPLAccountInvoiceDemurrage';
            $('#lookup_div_account_dem').dialog({ title: 'List Demurrage' });

            lookupGrid.setGridParam({
                url: base_url + accountUrl,
                postData: {
                    filters: null, 'shipmentId': function () { return shipmentId; },
                    'custType': function () { return custType; }, 'customerId': function () { return customerId; }, 'debetCredit': function () { return debetCredit; }
                }
            }).trigger("reloadGrid");
            $('#lookup_div_account_dem').dialog('open');
        }

    });

    // Cancel or CLose
    $('#lookup_btn_cancel_account_dem').click(function () {
        $('#lookup_div_account_dem').dialog('close');
    });

    // ADD or Select Data - Demurrage from EPL
    $('#lookup_btn_add_account_dem').click(function () {
        var accountGrid = jQuery("#lookup_table_account_dem");
        var id = accountGrid.jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = accountGrid.jqGrid('getRowData', id);

            // default
            var amount = ret.amountusd;
            var currency = 0;
            if (ret.currencytype == 'IDR') {
                amount = ret.amountidr;
                currency = 1;
            }
            // default All
            var containerType = 5;
            if ('20' == ret.type)
                containerType = 1;
            if ('40' == ret.type)
                containerType = 2;
            if ('45' == ret.type)
                containerType = 3;
            if ('M3' == ret.type)
                containerType = 4;


            if ((InvoiceId != undefined && InvoiceId != "")) {
                var submitURL = base_url + "Invoice/InsertDetail";
                var isValid = true;
                var message = "OK";

                // Add or Edit
                SaveInvoiceDetail(submitURL, 0, ret.epldetailid, $('#txtSEkdcustomer').data('kode'), ret.code, ret.name, ret.desc, 0, 0, 5,
                                0, ret.sign, 0, amount, 0, 0, JSON.stringify(ret.hfpsinfo), ret.dmr, ret.dmrcontainer, ret.dmrcontainerdetail, 0, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        invoiceDetailId = data.objResult.InvoiceDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {
                    addInvDetail(0, 0, ret.epldetailid, ret.code, ret.name, ret.desc, ret.type, currency, amount, ret.quantity, ret.perqty, ret.codingquantity, ret.sign,
                        ret.hfpsinfo, 0, 0, ret.dmr, ret.dmrcontainerdetail, ret.dmrcontainer);
                }
            }
            else {
                addInvDetail(0, 0, ret.epldetailid, ret.code, ret.name, ret.desc, ret.type, currency, amount, ret.quantity, ret.perqty, ret.codingquantity, ret.sign,
                    ret.hfpsinfo, 0, 0, ret.dmr, ret.dmrcontainerdetail, ret.dmrcontainer);
            }

            clearInvAgentShipper();
            $('#lookup_div_account_dem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_account_dem").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'desc', 'amountusd', 'amountidr', 'codingquantity', 'Crr', 'Payment', 'Type', '', '', '', '', '', '', '', ''],
        colModel: [{ name: 'code', index: 'accountcode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'desc', index: 'desc', width: 200, hidden: true },
                  { name: 'amountusd', index: 'a.amountusd', width: 200, hidden: true },
                  { name: 'amountidr', index: 'b.amountidr', width: 200, hidden: true },
                  { name: 'codingquantity', index: 'codingquantity', width: 200, hidden: true },
                  { name: 'currencytype', index: 'currencytype', width: 50, hidden: false },
                  { name: 'payment', index: 'payment', width: 200, hidden: false, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'type', index: 'type', width: 50, hidden: false },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true },
                  { name: 'perqty', index: 'perqty', width: 200, hidden: true },
                  { name: 'epldetailid', index: 'epldetailid', width: 200, hidden: true },
                  { name: 'sign', index: 'sign', width: 30, align: 'center', hidden: true },
                  { name: 'hfpsinfo', index: 'hfpsinfo', width: 200, hidden: true },
                  { name: 'dmr', index: 'dmr', width: 200, hidden: true },
                  { name: 'dmrcontainer', index: 'dmrcontainer', width: 200, hidden: true },
                  { name: 'dmrcontainerdetail', index: 'dmrdmrcontainerdetail', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account_dem'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_account_dem").jqGrid('navGrid', '#lookup_table_account_hfps', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
        if (isAgent) {
            $('#rowCurrencyType').hide();
        }
        else {
            $('#rowCurrencyType').show();
        }

        // Assigned Invoice Detail Id
        $('#lookup_btn_add_invoice_detail').data('invoicedetailid', 0);
        // Assigned EPL Detail Id
        $('#lookup_btn_add_invoice_detail').data('epldetailid', 0);


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
            //$('#txtInvkdagent').val($('#txtSEkdcustomer').val());
            //$('#txtInvnmagent').val($('#txtSEnmcustomer').val());

            // Set BillTo as LinkCode
            var billTo = $('#txtSEkdbillto').data("kode");
            if (billTo != undefined && billTo != "") {
                customerCode = $('#txtSEkdbillto').val();
                customerName = $('#txtSEnmbillto').val();
                //$('#txtInvkdagent').val($('#txtSEkdbillto').val());
                //$('#txtInvnmagent').val($('#txtSEnmbillto').val());
            }
            $('#txtInvkdagent').val(customerCode);
            $('#txtInvnmagent').val(customerName);

            var ret = jQuery("#table_invoice_detail").jqGrid('getRowData', id);

            // Demurrage
            if (ret.accountid == '000') {
                ClearDemurrage();
                $('#txtDemkddemurrage').val($('#txtSEkdcustomer').val());
                $('#txtDemnmdemurrage').val($('#txtSEnmcustomer').val());
                var billTo = $('#txtSEkdbillto').data("kode");
                if (billTo != undefined && billTo != "") {
                    $('#txtDemkddemurrage').val($('#txtSEkdbillto').val());
                    $('#txtDemnmdemurrage').val($('#txtSEnmbillto').val());
                }
                $('#txtDemeta').val($('#txtSEetd').val());

                // Demurrage Header
                var dmr = $.parseJSON(ret.dmr);
                if (dmr != null) {
                    $('#txtDemfreetimedays').val(dmr.Freetime);
                    $('#txtDemfasedays').val(dmr.Fase);
                    $('#txtDembackcontainer').datebox('setValue', dateFormat(new Date(dmr.DateBackContainer), 'MM/DD/YYYY'));

                    $('#txtDemtotal20').val(dmr.TotalContainer20);
                    $('#txtDemtotal40').val(dmr.TotalContainer40);
                }


                // Demurrage Container
                var dmrcontainer = $.parseJSON(ret.dmrcontainer);
                //console.log(dmrcontainer);
                if (dmrcontainer != null) {
                    for (var i = 0; i < dmrcontainer.length; i++) {
                        var newData = {};
                        newData.containerno = dmrcontainer[i].ContainerNo;
                        newData.sealno = dmrcontainer[i].SealNo;
                        newData.size = dmrcontainer[i].Size;
                        newData.type = dmrcontainer[i].Type;
                        jQuery("#tbl_dem_container_list").jqGrid('addRowData', $("#tbl_dem_container_list").getGridParam("reccount") + 1, newData);
                    }
                }


                // Demurrage Container Detail
                var dmrcontainerdetail = $.parseJSON(ret.dmrcontainerdetail);
                //console.log(dmramount);
                if (dmrcontainerdetail != null) {
                    // Clear Grid
                    $("#tbl_dem_list").jqGrid("clearGridData", true).trigger("reloadGrid");

                    for (var i = 0; i < dmrcontainerdetail.length; i++) {
                        var newData = {};
                        newData.description = dmrcontainerdetail[i].Description;
                        newData.days = dmrcontainerdetail[i].DayContainer;
                        newData.type20 = dmrcontainerdetail[i].AmountContainer20;
                        newData.subtotal20 = (dmrcontainerdetail[i].AmountContainer20 * dmrcontainerdetail[i].DayContainer) * dmr.TotalContainer20;
                        newData.type40 = dmrcontainerdetail[i].AmountContainer40;
                        newData.subtotal40 = (dmrcontainerdetail[i].AmountContainer40 * dmrcontainerdetail[i].DayContainer) * dmr.TotalContainer40;
                        newData.total = newData.subtotal20 + newData.subtotal40;
                        jQuery("#tbl_dem_list").jqGrid('addRowData', $("#tbl_dem_list").getGridParam("reccount") + 1, newData);
                    }
                }

                if (dmr != null) {
                    if (dmr.DiscType == 'P')
                        $('#rbDemdiscountprecentage').attr('checked', 'checked');
                    else
                        $('#rbDemdiscountamount').attr('checked', 'checked');
                    $('#txtDemdiscount').val(dmr.DiscAmount);
                }

                // Calculate Total
                CalculateDemurrageGrandTotal();

                $('#lookup_div_demurrage').dialog('open');
            }
                // Profit Split - Buying Rate, Selling Rate, Profit Share
            else if (parseInt(ret.accountid) == 3 || parseInt(ret.accountid) == 4 || parseInt(ret.accountid) == 6) {

                // Assigned Invoice Detail Id
                $('#lookup_btn_add_invoice_detail_profitsplit').data('invoicedetailid', id);
                // Assigned EPL Detail Id
                $('#lookup_btn_add_invoice_detail_profitsplit').data('epldetailid', ret.epldetailid);
                // Assigned Counter Profit Split
                $('#lookup_btn_add_invoice_detail_profitsplit').data('counterprofitsplit', ret.counterprofitsplit);

                // Parse from JSON contenttype
                var hfpsinfo = $.parseJSON(ret.hfpsinfo);

                // Search ProfitSplit info if not in their hfpsinfo
                if (hfpsinfo == null) {
                    // Get all data
                    var data = $("#table_invoice_detail").jqGrid('getGridParam', 'data');
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].counterprofitsplit == ret.counterprofitsplit) {
                            if (parseInt(data[i].accountid) == 3 || parseInt(data[i].accountid) == 4 || parseInt(data[i].accountid) == 6) {
                                if (data[i].hfpsinfo != null) {
                                    hfpsinfo = $.parseJSON(data[i].hfpsinfo);
                                    i = data.length;
                                }
                            }
                        }
                    }
                }

                // Sign
                $('#optInvsignplus_profitsplit').attr('checked', 'checked');
                if (ret.sign == '-')   // minus
                {
                    $('#optInvsignminus_profitsplit').attr('checked', 'checked');
                }

                $("#txtInv_sellingrate20").numberbox("setValue", hfpsinfo.SFeet20);
                $("#txtInv_sellingrate20usd").numberbox("setValue", hfpsinfo.SRate20);
                $("#txtInv_sellingrate40").numberbox("setValue", hfpsinfo.SFeet40);
                $("#txtInv_sellingrate40usd").numberbox("setValue", hfpsinfo.SRate40);
                $("#txtInv_sellingrate45").numberbox("setValue", hfpsinfo.SFeetHQ);
                $("#txtInv_sellingrate45usd").numberbox("setValue", hfpsinfo.SRateHQ);
                $("#txtInv_sellingratem3").numberbox("setValue", hfpsinfo.SFeetM3);
                $("#txtInv_sellingratem3usd").numberbox("setValue", hfpsinfo.SRateM3);
                IncAgentPSSellingRateCalculation();
                $("#txtInv_buyingrate20").numberbox("setValue", hfpsinfo.BFeet20);
                $("#txtInv_buyingrate20usd").numberbox("setValue", hfpsinfo.BRate20);
                $("#txtInv_buyingrate40").numberbox("setValue", hfpsinfo.BFeet40);
                $("#txtInv_buyingrate40usd").numberbox("setValue", hfpsinfo.BRate40);
                $("#txtInv_buyingrate45").numberbox("setValue", hfpsinfo.BFeetHQ);
                $("#txtInv_buyingrate45usd").numberbox("setValue", hfpsinfo.BRateHQ);
                $("#txtInv_buyingratem3").numberbox("setValue", hfpsinfo.BFeetM3);
                $("#txtInv_buyingratem3usd").numberbox("setValue", hfpsinfo.BRateM3);
                IncAgentPSBuyingRateCalculation();

                IncAgentProfitSplitCalculation();

                $("#txtInv_profitsplitpercent").numberbox("setValue", hfpsinfo.Percentage);
                var percentAmount = (parseFloat($('#txtInv_profitsplittotal').val()) * parseFloat(hfpsinfo.Percentage)) / 100;
                $('#txtInv_profitsplitpercentamount').numberbox('setValue', percentAmount);

                IncAgentProfitSplitCalculation();

                $('#txtInvkdagent_profitsplit').data('kode', customerCode).val(customerCode);
                $('#txtInvnmagent_profitsplit').val(customerName);

                $('#lookup_div_invoice_detail_profitsplit').dialog({ title: 'Invoice To Agent - Profit Split' });
                $('#lookup_div_invoice_detail_profitsplit').dialog('open');
            }
                // END Profit Split - Buying Rate, Selling Rate, Profit Share
                // Handling Fee
            else if (parseInt(ret.accountid) == 5) {

                // Assigned Invoice Detail Id
                $('#lookup_btn_add_invoice_detail_handlingfee').data('invoicedetailid', id);
                // Assigned EPL Detail Id
                $('#lookup_btn_add_invoice_detail_handlingfee').data('epldetailid', ret.epldetailid);

                // Parse from JSON contenttype
                var hfpsinfo = $.parseJSON(ret.hfpsinfo);

                // Sign
                $('#optInvsignplus_handlingfee').attr('checked', 'checked');
                if (ret.sign == '-')   // minus
                {
                    $('#optInvsignminus_handlingfee').attr('checked', 'checked');
                }

                $("#txtInv_handlingfee20").numberbox("setValue", hfpsinfo.Feet20);
                $("#txtInv_handlingfee20usd").numberbox("setValue", hfpsinfo.Rate20);
                $("#txtInv_handlingfee40").numberbox("setValue", hfpsinfo.Feet40);
                $("#txtInv_handlingfee40usd").numberbox("setValue", hfpsinfo.Rate40);
                $("#txtInv_handlingfee45").numberbox("setValue", hfpsinfo.FeetHQ);
                $("#txtInv_handlingfee45usd").numberbox("setValue", hfpsinfo.RateHQ);
                $("#txtInv_handlingfeem3").numberbox("setValue", hfpsinfo.FeetM3);
                $("#txtInv_handlingfeem3usd").numberbox("setValue", hfpsinfo.RateM3);
                IncAgentHFCalculation();

                $('#txtInvkdagent_handlingfee').data('kode', customerCode).val(customerCode);
                $('#txtInvnmagent_handlingfee').val(customerName);

                $('#lookup_div_invoice_detail_handlingfee').dialog({ title: 'Invoice To Agent - Handling Fee' });
                $('#lookup_div_invoice_detail_handlingfee').dialog('open');
            }
                // END Handling Fee
            else {

                // Assigned Invoice Detail Id
                $('#lookup_btn_add_invoice_detail').data('invoicedetailid', id);
                // Assigned EPL Detail Id
                $('#lookup_btn_add_invoice_detail').data('epldetailid', ret.epldetailid);

                $('#txtInvnmaccount').val(ret.accountname);
                $('#txtInvdesc').val(ret.description);
                $('#txtInvamount').numberbox('setValue', ret.amountusd);
                $('#optInvsignplus').attr('checked', 'checked');
                if (ret.sign == '-')   // minus
                {
                    $('#optInvsignminus').attr('checked', 'checked');
                }
                $('#txtInvkdaccount').data("kode", ret.accountid).val(ret.accountid);
                //ret.type = $('#txtInvamount').val();
                switch (ret.type) {
                    case '1': $('#optInvcontainersize20').attr('checked', 'checked'); break;
                    case '2': $('#optInvcontainersize40').attr('checked', 'checked'); break;
                    case '3': $('#optInvcontainersize45').attr('checked', 'checked'); break;
                    case '4': $('#optInvcontainersizem3').attr('checked', 'checked'); break;
                    case '5': $('#optInvcontainersizeall').attr('checked', 'checked'); break;
                }
                $('#txtInvquantity').numberbox('setValue', ret.quantity);
                $('#txtInvperunitcost').numberbox('setValue', ret.perqty);
                $('#txtInvkdaccount').data("codingquantity", ret.codingquantity);
                $('#ddlSEvat').val(ret.vatid);

                // Currency Type
                if (isAgent) {
                    $('#rowCurrencyType').hide();
                }
                else {
                    $('#rowCurrencyType').show();
                    if (ret.currencytype == 1) {
                        $('#optInvcurrencytypeIDR').attr('checked', 'checked');
                        $('#txtInvamount').numberbox('setValue', ret.amountidr);
                    }
                }

                if (isAgent == true) {
                    $('#lookup_div_invoice_detail').dialog({ title: 'Invoices to Agent' });
                }
                else {
                    $('#lookup_div_invoice_detail').dialog({ title: 'Invoices to Consignee' });
                }
                $('#lookup_div_invoice_detail').dialog('open');
            }
        } else {
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
                    // Check if Profit Split (Selling Rate, Buying Rate, Profit Share)
                    var data = jQuery("#table_invoice_detail").jqGrid('getRowData', id);
                    var counterprofitsplit = data.counterprofitsplit;
                    if (data.accountid == 3 || data.accountid == 4 || data.accountid == 6) {
                        // Delete OLD Buying rate, Selling rate, Profit Share
                        var ids = $("#table_invoice_detail").jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            var datagrid = jQuery("#table_invoice_detail").jqGrid('getRowData', ids[i]);
                            if (data.counterprofitsplit == datagrid.counterprofitsplit) {
                                // Selling Rate, Buying Rate, Profit Share
                                if (parseInt(datagrid.accountid) == 3 || parseInt(datagrid.accountid) == 4 || parseInt(datagrid.accountid) == 6) {
                                    var idx = i + 1;

                                    var isValid = true;
                                    var message = "";
                                    DeleteInvoiceDetail(ids[i], function (data) {
                                        isValid = data.isValid;
                                        message = data.message
                                    });

                                    if (!isValid) {
                                        $.messager.alert('Warning', message, 'warning');
                                    }
                                    else {
                                        jQuery("#table_invoice_detail").jqGrid('delRowData', ids[i]);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        var isValid = true;
                        var message = "";
                        DeleteInvoiceDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {

                            jQuery("#table_invoice_detail").jqGrid('delRowData', id);
                        }
                    }

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
        if ($('#optInvcontainersize20').is(':checked'))
            containerType = 1;
        if ($('#optInvcontainersize40').is(':checked'))
            containerType = 2;
        if ($('#optInvcontainersize45').is(':checked'))
            containerType = 3;
        if ($('#optInvcontainersizem3').is(':checked'))
            containerType = 4;

        var sign = true;
        if ($('#optInvsignminus').is(':checked'))
            sign = false;

        // Currency Type
        var currencyType = 0;
        if (isAgent) {
            $('#rowCurrencyType').hide();
        }
        else {
            $('#rowCurrencyType').show();
            if ($('#optInvcurrencytypeIDR').is(':checked'))
                currencyType = 1;
        }

        // Vat Id for Non Agent
        var vatId = 0;
        if (isAgent == false) {
            vatId = $('#ddlSEvat').val();
        }

        var invoiceDetailId = 0
        var eplDetailId = $('#lookup_btn_add_invoice_detail').data('epldetailid');

        // ONLY Update Invoice
        if ((InvoiceId != undefined && InvoiceId != "")) {
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
                            $('#txtInvkdaccount').data("codingquantity"), sign, currencyType, amount, $('#ddlSEvat option:selected').text(), amountvat, '', '', '', '', vatId, function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    invoiceDetailId = data.objResult.InvoiceDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, $('#txtInvkdaccount').data("kode"), $('#txtInvnmaccount').val(), $('#txtInvdesc').val(),
                    containerType, currencyType, amount, $('#txtInvquantity').numberbox('getValue'), $('#txtInvperunitcost').numberbox('getValue'),
                    $('#txtInvkdaccount').data("codingquantity"), sign, '', $('#ddlSEvat option:selected').text(), vatId, '', '', '');
            }
        }
        else {
            addInvDetail(vInvoiceDetailEdit, invoiceDetailId, eplDetailId, $('#txtInvkdaccount').data("kode"), $('#txtInvnmaccount').val(), $('#txtInvdesc').val(),
                containerType, currencyType, amount, $('#txtInvquantity').numberbox('getValue'), $('#txtInvperunitcost').numberbox('getValue'),
                $('#txtInvkdaccount').data("codingquantity"), sign, '', $('#ddlSEvat option:selected').text(), vatId, '', '', '');
        }
        clearInvAgentShipper();

        $('#lookup_div_invoice_detail').dialog('close');
    });

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
                // Set Counter Profit Share ONLY on ADDING from Form Load
                if (accountId != "") {
                    var accountIdVal = parseInt(accountId);
                    // As Selling Rate
                    if (accountIdVal == 3) {
                        counterSellRate = counterSellRate + 1;
                        newData.counterprofitsplit = counterSellRate;
                    }
                    // As Selling Rate
                    if (accountIdVal == 4) {
                        counterBuyRate = counterBuyRate + 1;
                        newData.counterprofitsplit = counterBuyRate;
                    }
                    // As Profit Share
                    if (accountIdVal == 6) {
                        counterProfitShare = counterProfitShare + 1;
                        newData.counterprofitsplit = counterProfitShare;
                    }
                }

                if (newData.description == "PROFIT SHARE" && newData.accountname == "PROFIT SHARE") {
                    // Get Total - Selling Rate, Buying Rate, Profit Share
                    var tempValue = newData.amountusd.split('^');
                    var srTotal = tempValue[0];
                    var brTotal = tempValue[1];
                    var psTotal = tempValue[2];

                    // Add New Record - SELLING RATE
                    counterSellRate = counterSellRate + 1;
                    newData.counterprofitsplit = counterSellRate;
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/InsertDetail", invoiceDetailId, eplDetailId, 0, '003', 'SELLING RATE', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, srTotal, vat, amountvat, hfpsinfo, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoicelDetailId = data.objResult.InvoicelDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('addRowData', invoicelDetailId, newData);
                        }
                    }
                    else {
                        var srEplDetailId = invoicelDetailId == 0 ? GetNextId($("#table_invoice_detail")) : invoicelDetailId;
                        jQuery("#table_invoice_detail").jqGrid('addRowData', srEplDetailId, newData);
                    }

                    // Add New Record - BUYING RATE
                    counterBuyRate = counterBuyRate + 1;
                    newData.counterprofitsplit = counterBuyRate;
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/InsertDetail", invoiceDetailId, eplDetailId, 0, '004', 'BUYING RATE', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, brTotal, vat, amountvat, '', function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoicelDetailId = data.objResult.InvoicelDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('addRowData', invoicelDetailId, newData);
                        }
                    }
                    else {
                        var brInvoiceDetailId = invoicelDetailId == 0 ? GetNextId($("#table_invoice_detail")) : invoicelDetailId;
                        jQuery("#table_invoice_detail").jqGrid('addRowData', brInvoiceDetailId, newData);
                    }

                    // Add New Record - PROFIT SHARE
                    counterProfitShare = counterProfitShare + 1;
                    newData.counterprofitsplit = counterProfitShare;
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/InsertDetail", invoiceDetailId, eplDetailId, 0, '006', 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %', 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, psTotal, vat, amountvat, '', function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoicelDetailId = data.objResult.InvoicelDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('addRowData', invoicelDetailId, newData);
                        }
                    }
                    else {
                        var psInvoiceDetailId = invoiceDetailId == 0 ? GetNextId($("#table_invoice_detail")) : invoiceDetailId;
                        jQuery("#table_invoice_detail").jqGrid('addRowData', psInvoiceDetailId, newData);
                    }

                }
                else {
                    invoiceDetailId = invoiceDetailId == 0 ? GetNextId($("#table_invoice_detail")) : invoiceDetailId;
                    jQuery("#table_invoice_detail").jqGrid('addRowData', invoiceDetailId, newData);
                }
            }
                // Update Mode
            else {
                var id = jQuery("#table_invoice_detail").jqGrid('getGridParam', 'selrow');
                var accountidSelected = jQuery("#table_invoice_detail").jqGrid('getCell', id, 'accountid');

                // ProfitSplit - selling rate, buying rate, profit share
                if (parseInt(accountidSelected) == 3 || parseInt(accountidSelected) == 4 || parseInt(accountidSelected) == 6) {
                    // default
                    var eplDetailId = 0;
                    var isValid = true;
                    var message = "OK";

                    // Get Total - Selling Rate, Buying Rate, Profit Share
                    var tempValue = newData.amountusd.split('^');
                    var srTotal = tempValue[0];
                    var brTotal = tempValue[1];
                    var psTotal = tempValue[2];

                    // Delete OLD Buying rate, Selling rate, Profit Share
                    var sellRateInvoiceDetailID = 0;
                    var buyRateInvoiceDetailID = 0;
                    var profitShareInvoiceDetailID = 0;
                    var ids = $("#table_invoice_detail").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var data = jQuery("#table_invoice_detail").jqGrid('getRowData', ids[i]);
                        if (data.counterprofitsplit == $('#lookup_btn_add_invoice_detail_profitsplit').data('counterprofitsplit')) {
                            // Selling Rate, Buying Rate, Profit Share
                            if (parseInt(data.accountid) == 3 || parseInt(data.accountid) == 4 || parseInt(data.accountid) == 6) {
                                var idx = i + 1;
                                if (parseInt(data.accountid) == 3) {
                                    sellRateInvoiceDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 4) {
                                    buyRateInvoiceDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 6) {
                                    profitShareInvoiceDetailID = ids[i];
                                }
                                //jQuery("#table_invoice_detail").jqGrid('delRowData', ids[i]);
                            }
                        }
                    }
                    $("#table_invoice_detail").jqGrid().trigger("reloadGrid");

                    // Add New Record - SELLING RATE
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/UpdateDetail", sellRateInvoiceDetailID, eplDetailId, 0, '003', 'SELLING RATE', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, srTotal, vat, amountvat, hfpsinfo, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoiceDetailId = data.objResult.InvoiceDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('setRowData', invoiceDetailId, newData);
                        }
                    }
                    else
                        jQuery("#table_invoice_detail").jqGrid('setRowData', sellRateInvoiceDetailID, newData);

                    // Add New Record - BUYING RATE
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/UpdateDetail", buyRateInvoiceDetailID, eplDetailId, 0, '004', 'BUYING RATE', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, brTotal, vat, amountvat, '', function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoiceDetailId = data.objResult.InvoiceDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('setRowData', invoiceDetailId, newData);
                        }
                    }
                    else
                        jQuery("#table_invoice_detail").jqGrid('setRowData', buyRateInvoiceDetailID, newData);

                    // Add New Record - PROFIT SHARE
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE Invoice
                    if ((InvoiceId != undefined && InvoiceId != "")) {
                        // Submit To Server
                        SaveInvoiceDetail(base_url + "invoice/UpdateDetail", profitShareInvoiceDetailID, eplDetailId, 0, '006', 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %', 'PROFIT SHARE ' + $('#txtInv_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, currencyType, psTotal, vat, amountvat, '', function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                invoiceDetailId = data.objResult.InvoiceDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#table_invoice_detail").jqGrid('setRowData', invoiceDetailId, newData);
                        }
                    }
                    else
                        jQuery("#table_invoice_detail").jqGrid('setRowData', profitShareInvoiceDetailID, newData);
                }
                    // END ProfitSplit
                else if (id) {
                    //  Edit
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

    // -------------------------- Demurrage ----------------------------------------------------------------------
    function ClearDemurrage() {
        //if (vDemurrageType == 'agent') {
        //    $('#txtDemkdconsignee').data('kode', $("#txtSEagent").data('kode')).val($("#txtSEagent").data('kode'));
        //    $('#txtDemnmconsignee').val($("#txtSEagent").val());
        //}
        //    // Inc Consignee
        //else {
        //    $('#txtDemkdconsignee').data('kode', $("#txtSEconsignee").data('kode')).val($("#txtSEconsignee").data('kode'));
        //    $('#txtDemnmconsignee').val($("#txtSEconsignee").val());
        //}
        $('#txtDemeta').val($('#txtSEetd').val());

        // Default value
        var vDate = new Date();
        $('#txtDembackcontainer').datebox('setValue', (vDate.getMonth() + 1) + '/' + vDate.getDate() + '/' + vDate.getFullYear());
        $('#txtDemfreetimedays').val(7);
        $('#txtDemfasedays').val(5);

        // Clear Grid
        $("#tbl_dem_container_list").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_dem_list").jqGrid("clearGridData", true).trigger("reloadGrid");

        $('#txtDemtotal20').val(0);
        $('#txtDemtotal40').val(0);

        $('#txtDemdiscount').numberbox('setValue', 0);
        $('#txtDemtotal').numberbox('setValue', 0);
        $('#txtDemdiscountpercentage').numberbox('setValue', 0);
        $('#txtDemdiscountamount').numberbox('setValue', 0);
        $('#txtDemgrandtotal').numberbox('setValue', 0);

    }
    // Save Demurrage
    $('#lookup_btn_add_demurrage').click(function () {

        // Grand Total
        var grandTotal = $('#txtDemgrandtotal').numberbox('getValue');

        // Demurrage Header
        var dmrHeader = {};
        dmrHeader.Freetime = $('#txtDemfreetimedays').val();
        dmrHeader.Fase = $('#txtDemfasedays').val();
        dmrHeader.BackContainer = $('#txtDembackcontainer').datebox('getValue');
        dmrHeader.Container20 = $('#txtDemtotal20').val();
        dmrHeader.Container40 = $('#txtDemtotal40').val();
        dmrHeader.DiscTo = 'P';
        if ($('#rbDemdiscountamount').is(':checked'))
            dmrHeader.DiscTo = 'A';
        dmrHeader.DiscAmount = $('#txtDemdiscount').val();

        // Demurrage Container
        var dmrContainer = [];
        var ids = $("#tbl_dem_container_list").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var data = jQuery("#tbl_dem_container_list").jqGrid('getRowData', ids[i]);
            var newData = {};
            newData.ContainerNo = data.containerno;
            newData.SealNo = data.sealno;
            newData.Size = data.size;
            newData.Type = data.type;
            dmrContainer.push(newData);
        }

        // Demurrage Amount
        var dmrAmount = [];
        var ids = $("#tbl_dem_list").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var data = jQuery("#tbl_dem_list").jqGrid('getRowData', ids[i]);
            var newData = {};
            newData.Description = data.description;
            newData.DayContainer = data.days;
            newData.USD20 = data.type20;
            newData.USD40 = data.type40;
            dmrAmount.push(newData);
        }

        // Add NEW
        dmrHeader = JSON.stringify(dmrHeader);
        dmrAmount = JSON.stringify(dmrAmount);
        dmrContainer = JSON.stringify(dmrContainer);

        // ON EDIT, Delete OLD Record
        if (vAgentShipperEdit == 1) {
            // Delete OLD Total Demurrage
            var ids = $("#table_invoice_detail").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var data = jQuery("#table_invoice_detail").jqGrid('getRowData', ids[i]);

                // Total Demurrage
                if (parseInt(data.accountid) == 0) {        // Total Demurrage
                    var idx = i + 1;
                    jQuery("#table_invoice_detail").jqGrid('delRowData', ids[i]);
                }

            }
            $("#table_invoice_detail").jqGrid().trigger("reloadGrid");
        }

        addInvAgentShipper(0, '000', 'TOTAL DEMURRAGE', 5, 0, grandTotal, 0, 0, 0, true, '', 0,
            dmrHeader, dmrAmount, dmrContainer);

        $('#lookup_div_demurrage').dialog('close');
    });

    // Close Demurrage
    $('#lookup_btn_cancel_demurrage').click(function () {
        $('#lookup_div_demurrage').dialog('close');
    });

    // Declare Table jqgrid Container List
    jQuery("#tbl_dem_container_list").jqGrid({
        datatype: "local",
        height: 60,
        colNames: ['Container No', 'Seal', 'Size', 'Type'],
        colModel: [
            { name: 'containerno', index: 'containerno', width: 150 },
            { name: 'sealno', index: 'sealno', width: 150 },
            { name: 'size', index: 'size', width: 50 },
            { name: 'type', index: 'type', width: 50 }
        ]
    });
    // Declare Table jqgrid Demurrage List
    jQuery("#tbl_dem_list").jqGrid({
        datatype: "local",
        height: 90,
        colNames: ['Description', 'Days', '20', 'Sub Total', '40', 'Sub Total', 'Total'],
        colModel: [
            { name: 'description', index: 'description', width: 200 },
            { name: 'days', index: 'days', width: 50, align: "right" },
            { name: 'type20', index: 'type20', width: 50, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'subtotal20', index: 'subtotal20', width: 70, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'type40', index: 'type40', width: 50, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'subtotal40', index: 'subtotal40', width: 70, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'total', index: 'total', width: 50, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ]
    });

    // ------------------------------------------------------ Demurrage - Container ------------------------------------------------------
    // Browse
    $('#btnDemcontainer').click(function () {
        var lookupGrid = $('#lookup_table_container');
        lookupGrid.setGridParam({ url: base_url + 'Container/LookUpContainer' }).trigger("reloadGrid");
        $('#lookup_div_container').dialog('open');
    });

    // ADD or Select Data
    $('#lookup_btn_add_container').click(function () {
        var id = jQuery("#lookup_table_container").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_container").jqGrid('getRowData', id);

            var isValid = true;
            var data = $("#tbl_dem_container_list").jqGrid('getGridParam', 'data');
            for (var i = 0; i < data.length; i++) {
                if (data[i].containerno == ret.ContainerNo) {
                    $.messager.alert('Warning', 'Container has been exist...!!', 'warning');
                    isValid = false;
                    break;
                }
            }

            if (isValid) {
                var newData = {};
                newData.containerno = ret.ContainerNo;
                newData.sealno = ret.SealNo;
                newData.size = ret.Size;
                newData.type = ret.Type;
                jQuery("#tbl_dem_container_list").jqGrid('addRowData', $("#tbl_dem_container_list").getGridParam("reccount") + 1, newData);

                CalculateContainerTotal();
            }

            $('#lookup_div_container').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // Delete
    $('#btn_delete_dem_container').click(function () {
        var id = jQuery("#tbl_dem_container_list").jqGrid('getGridParam', 'selrow');
        if (id) {
            jQuery("#tbl_dem_container_list").jqGrid('delRowData', id);
            CalculateContainerTotal();
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // Refresh
    $('#btn_refresh_dem_container').click(function () {
        RefreshDemurrageList();
    });
    // Close
    $("#lookup_btn_cancel_container").click(function () {
        $('#lookup_div_container').dialog('close');
    });
    // Refresh Container Data
    function RefreshDemurrageList() {

        var eta = $('#txtDemeta').val();
        eta = new Date(eta);

        var freetime = $('#txtDemfreetimedays').val();
        var fase = $('#txtDemfasedays').val();

        // From SeaImportDetail.cshtml
        var dmrCharge = dmrChargeList;

        //// ONLY ON ADDING
        //if (vDemurrageEdit == 0) {
        // Clear Grid
        $("#tbl_dem_list").jqGrid("clearGridData", true).trigger("reloadGrid");

        var vDate = new Date();
        var loop = 6;
        for (var i = 0; i < loop; i++) {
            // freetime
            if (i == 0) {
                vDate = new Date(eta.getTime() + freetime * 24 * 60 * 60 * 1000)
                var newData = {};
                newData.description = 'Freetime ' + dateFormat(eta, 'MM/DD/YYYY') + ' - ' + dateFormat(vDate, 'MM/DD/YYYY');
                newData.days = freetime;
                newData.type20 = 0; newData.subtotal20 = 0;
                newData.type40 = 0; newData.subtotal40 = 0;
                newData.total = 0;
                jQuery("#tbl_dem_list").jqGrid('addRowData', $("#tbl_dem_list").getGridParam("reccount") + 1, newData);
            }
                // END Fase
            else if (i == loop - 1) {
                var endDate = new Date($('#txtDembackcontainer').datebox('getValue'));
                var svDate = new Date(vDate.getTime() + 1 * 24 * 60 * 60 * 1000);
                var days = (Math.abs(endDate.getTime() - svDate.getTime()) / 86400000) + 1;
                var newData = {};
                newData.description = dateFormat(svDate, 'MM/DD/YYYY') + ' - ' + dateFormat(endDate, 'MM/DD/YYYY');
                newData.days = days;
                newData.type20 = dmrCharge[i - 1].USD20; newData.subtotal20 = parseFloat(dmrCharge[i - 1].USD20) * parseFloat($('#txtDemtotal20').val()) * days;
                newData.type40 = dmrCharge[i - 1].USD40; newData.subtotal40 = parseFloat(dmrCharge[i - 1].USD40) * parseFloat($('#txtDemtotal40').val()) * days;
                newData.total = newData.subtotal20 + newData.subtotal40;
                jQuery("#tbl_dem_list").jqGrid('addRowData', $("#tbl_dem_list").getGridParam("reccount") + 1, newData);
            }
            else {
                var newData = {};
                var endDate = new Date($('#txtDembackcontainer').datebox('getValue'));
                var svDate = new Date(vDate.getTime() + 1 * 24 * 60 * 60 * 1000);
                var evDate = new Date(svDate.getTime() + (fase - 1) * 24 * 60 * 60 * 1000);
                if (endDate == svDate) {
                    evDate = svDate;
                    fase = (Math.abs(endDate.getTime() - svDate.getTime()) / 86400000) + 1;
                    vDate = evDate;
                    newData.description = dateFormat(svDate, 'MM/DD/YYYY') + ' - ' + dateFormat(evDate, 'MM/DD/YYYY');
                    newData.days = fase;
                    newData.type20 = dmrCharge[i - 1].USD20; newData.subtotal20 = parseFloat(dmrCharge[i - 1].USD20) * parseFloat($('#txtDemtotal20').val()) * fase;
                    newData.type40 = dmrCharge[i - 1].USD40; newData.subtotal40 = parseFloat(dmrCharge[i - 1].USD40) * parseFloat($('#txtDemtotal40').val()) * fase;
                    newData.total = newData.subtotal20 + newData.subtotal40;
                    i = loop;
                }
                else if (endDate < svDate) {
                    break;
                }
                else if (endDate < evDate) {
                    evDate = endDate;
                    fase = (Math.abs(endDate.getTime() - svDate.getTime()) / 86400000) + 1;
                    vDate = evDate;
                    newData.description = dateFormat(svDate, 'MM/DD/YYYY') + ' - ' + dateFormat(evDate, 'MM/DD/YYYY');
                    newData.days = fase;
                    newData.type20 = dmrCharge[i - 1].USD20; newData.subtotal20 = parseFloat(dmrCharge[i - 1].USD20) * parseFloat($('#txtDemtotal20').val()) * fase;
                    newData.type40 = dmrCharge[i - 1].USD40; newData.subtotal40 = parseFloat(dmrCharge[i - 1].USD40) * parseFloat($('#txtDemtotal40').val()) * fase;
                    newData.total = newData.subtotal20 + newData.subtotal40;
                    i = loop;
                }
                else {
                    vDate = evDate;
                    newData.description = dateFormat(svDate, 'MM/DD/YYYY') + ' - ' + dateFormat(evDate, 'MM/DD/YYYY');
                    newData.days = fase;
                    newData.type20 = dmrCharge[i - 1].USD20; newData.subtotal20 = parseFloat(dmrCharge[i - 1].USD20) * parseFloat($('#txtDemtotal20').val()) * fase;
                    newData.type40 = dmrCharge[i - 1].USD40; newData.subtotal40 = parseFloat(dmrCharge[i - 1].USD40) * parseFloat($('#txtDemtotal40').val()) * fase;
                    newData.total = newData.subtotal20 + newData.subtotal40;
                }
                jQuery("#tbl_dem_list").jqGrid('addRowData', $("#tbl_dem_list").getGridParam("reccount") + 1, newData);
            }
        }
        //}
        // Calculate Demurrage Grand Total
        CalculateDemurrageGrandTotal();
    }
    // Calculate Container
    function CalculateContainerTotal() {
        var total20 = 0;
        var total40 = 0;
        var data = $("#tbl_dem_container_list").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            if (data[i].size == 20)
                total20++;
            else
                total40++;
        }
        $('#txtDemtotal20').val(total20);
        $('#txtDemtotal40').val(total40);
    }

    // Calculate Demurrage Grand Total
    function CalculateDemurrageGrandTotal() {
        var total = 0;
        var data = $("#tbl_dem_list").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            total += data[i].total;
        }
        $('#txtDemtotal').numberbox('setValue', total);

        // Calculate Demurrage Discount
        CalculateDemurrageDiscount();

        var grandTotal = parseFloat(total) - parseFloat($("#txtDemdiscountamount").val());
        $('#txtDemgrandtotal').numberbox('setValue', grandTotal);
    }

    // Demurrage Discount Calculation ------------------------------------------------------------------------------------------------

    // Calculate Demurrage Discount
    function CalculateDemurrageDiscount() {
        var total = parseFloat($('#txtDemtotal').numberbox('getValue'));
        var discount = parseFloat($('#txtDemdiscount').val());
        if ($('#rbDemdiscountprecentage').is(':checked')) {
            $('#txtDemdiscountpercentage').numberbox('setValue', discount);
            discount = parseFloat((discount / 100) * total);
        }
        else {
            var discountpercent = (discount * 100) / total;
            $('#txtDemdiscountpercentage').numberbox('setValue', discountpercent);
        }
        $('#txtDemdiscountamount').numberbox('setValue', discount);
    }

    $("#txtDemdiscount").blur(function () {
        CalculateDemurrageGrandTotal();
    });
    $("#rbDemdiscountprecentage, #rbDemdiscountamount").change(function () {
        CalculateDemurrageGrandTotal();
    });
    // END Demurrage Discount Calculation ------------------------------------------------------------------------------------------------

    jQuery("#lookup_table_container").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        postData: { 'shipmentId': function () { return shipmentId; } },
        //loadonce:true,
        colNames: ['Container No', 'Seal No', 'Size', 'Type', 'GrossWeight', 'NetWeight', 'CBM', 'Commodity', 'NoOfPieces', 'PackagingCode', 'PartOf'],
        colModel: [{ name: 'ContainerNo', index: 'ContainerNo', width: 120 },
                  { name: 'SealNo', index: 'SealNo', width: 120 },
                  { name: 'Size', index: 'Size', width: 80 },
                  { name: 'Type', index: 'Type', width: 100 },
                  { name: 'GrossWeight', index: 'GrossWeight', width: 320, hidden: true },
                  { name: 'NetWeight', index: 'NetWeight', width: 320, hidden: true },
                  { name: 'CBM', index: 'CBM', width: 320, hidden: true },
                  { name: 'Commodity', index: 'Commodity', width: 320, hidden: true },
                  { name: 'NoOfPieces', index: 'NoOfPieces', width: 320, hidden: true },
                  { name: 'PackagingCode', index: 'PackagingCode', width: 320, hidden: true },
                  { name: 'PartOf', index: 'PartOf', width: 320, hidden: true }],
        pager: jQuery('#lookup_pager_container'),
        rowNum: 20,
        sortname: 'ContainerNo',
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_container").jqGrid('navGrid', '#toolbar_lookup_table_container', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Demurrage - Container
    // END Demurrage

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

        // Invoice To Consignee
        var invoicesTo = 'CM';
        if ($('#rbInvoicesToShipper').is(':checked') && $("#ddlcurrency").val() == 'USD') {
            invoicesTo = 'CU';
        }
        else if ($('#rbInvoicesToShipper').is(':checked') && $("#ddlcurrency").val() == 'IDR') {
            invoicesTo = 'CI';
        }
        else if ($('#rbInvoicesToAgent').is(':checked'))
            // Invoice To Agent
            invoicesTo = 'AG';

        // Invoice Detail
        var InvoiceDetail = PopulateInvoiceDetail();
        // END Invoice Detail

        // DebetCredit
        var debetCredit = "C";
        if ($('#optSEDCdebet').is(':checked'))
            debetCredit = "D";


        var submitURL = base_url + "Invoice/Insert";
        if (InvoiceId != undefined && InvoiceId != 0)
            submitURL = base_url + "Invoice/Update";

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                InvoiceId: InvoiceId, ShipmentId: shipmentId,
                CustomerId: $('#txtSEkdcustomer').data('kode'), CustomerName: $('#txtSEnmcustomer').val(), CustomerAddress: $('#txtSEcustomeraddress').val(),
                BillId: $('#txtSEkdbillto').data('kode'), BillName: $('#txtSEnmbillto').val(), BillAddress: $('#txtSEbilltoaddress').val(),
                TotalVatIDR: $("#txtSEtotalvatidr").numberbox("getValue"), TotalVatUSD: $("#txtSEtotalvatusd").numberbox("getValue"),
                PaymentIDR: $("#txtSEpaymentidr").numberbox("getValue"), PaymentUSD: $("#txtSEpaymentusd").numberbox("getValue"), ShipmentNo: $("#txtSEshipmentno").val(),
                InvoicesNo: $("#InvoicesNo").val(), JenisInvoices: invoiceType, InvoicesTo: invoicesTo, ETD: $('#txtSEetd').val(), DebetCredit: debetCredit,
                Rate: $("#txtSErate").numberbox("getValue"), DateRate: $("#txtSEdaterate").val(), InvoicesAgent: $('#txtSEinvoicefromagentno').val(),
                InvHeader: $("#ddlSEinvoiceheader").val(),
                ListInvoiceDetail: InvoiceDetail
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "invoice/detail?Id=" + result.objResult.InvoiceId + "&JobId=" + jobId;
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
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

    // Save Invoice Detail
    function SaveInvoiceDetail(submitURL, v_invoicedetailId, v_epldetailId, v_customerId, v_accountId, v_accountName, v_description,
                                        v_quantity, v_perqty, v_type, v_codingquantity, v_sign, v_amountCrr, v_amount, v_percentvat, v_amountvat, v_hfpsinfo,
                                        v_dmr, v_dmrContainer, v_dmrContainerDetail, v_vatid, callback) {
        if (submitURL != undefined && submitURL != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    EPLDetailId: v_epldetailId, InvoiceDetailId: v_invoicedetailId,
                    InvoiceId: InvoiceId, shipmentId: shipmentId, CustomerId: v_customerId,
                    AccountID: v_accountId, AccountName: v_accountName, Description: v_description,
                    Quantity: v_quantity, PerQty: v_perqty, Sign: v_sign,
                    Type: v_type, Sign: v_sign, CodingQuantity: v_codingquantity,
                    AmountCrr: v_amountCrr, Amount: v_amount, AmountVat: v_amountvat, PercentVat: v_percentvat, VatId: v_vatid,
                    HFPSInfo: v_hfpsinfo,
                    Dmr: v_dmr, DmrContainer: v_dmrContainer, DmrContainerDetail: v_dmrContainerDetail
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

    // Delete Invoice Detail
    function DeleteInvoiceDetail(v_invoicedetailId, callback) {
        if (InvoiceId != undefined && InvoiceId != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + 'Invoice/DeleteDetail',
                data: JSON.stringify({
                    InvoiceDetailId: v_invoicedetailId
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

        window.open(base_url + "Invoice/PrintInvoices?Id=" + InvoiceId + "&bo=" + bo + "&fd=" + fd);
    });

    $("#btnDialogPrintFixedDraftNo").click(function () {
        $('#dialogPrintFixedDraft').dialog('close');

        fd = "d";

        window.open(base_url + "Invoice/PrintInvoices?Id=" + InvoiceId + "&bo=" + bo + "&fd=" + fd);
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

    // Change Invoice Header
    $('#seaimport_form_btn_changeinvheader').click(function () {

        if (InvoiceId == undefined || InvoiceId == "") {
            $.messager.alert('Information', 'Invalid Invoice ID..', 'info');
            return;
        }

        $.messager.confirm('Confirm', 'Are you sure you want to change Invoice Header?', function (r) {
            if (r) {

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "Invoice/ChangeInvoiceHeader",
                    data: JSON.stringify({
                        Id: InvoiceId, InvHeader: $('#ddlSEinvoiceheader').val()
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
    // END Change Invoice Header

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