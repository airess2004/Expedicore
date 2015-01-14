$(document).ready(function () {
    // Initialize
    var vDemurrageEdit = 0;
    var vDemurrageType = '';
    var vNonAgentType = '';
    var vNonAgentEdit = 0;
    var vAgentType = '';
    var vAgentEdit = 0;
    var vCostSSLineEdit = 0;
    var vConsignee = '';
    var vAccount = "";
    var vAgent = "";
    var vContainerCount20 = 0;
    var vContainerCount40 = 0;
    var vContainerCount45 = 0;
    var counterSellRate = 0;
    var counterBuyRate = 0;
    var counterProfitShare = 0;
    $('#seaimportdetail_content').css("height", $(window).height() - 120);
    $('#seaimportdetail_content').css("width", $(window).width() - 20);
    $('#txtSEexchangerate').mask("99.99");
    $('#lookup_div_non_agent').dialog('close');
    $('#lookup_div_inc_cost_agent').dialog('close');
    $('#lookup_div_inc_cost_agent_handlingfee').dialog('close');
    $('#lookup_div_inc_cost_agent_profitsplit').dialog('close');
    $('#lookup_div_cost_ssline').dialog('close');
    $('#lookup_div_shipmentorder').dialog('close');
    $('#lookup_div_consignee').dialog('close');
    $('#lookup_div_ssline').dialog('close');
    $('#lookup_div_emkl').dialog('close');
    $('#lookup_div_depo').dialog('close');
    $('#lookup_div_agent').dialog('close');
    $('#lookup_div_account').dialog('close');
    $('#lookup_div_demurrage').dialog('close');
    $('#lookup_div_container').dialog('close');

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
    var eplId = getQueryStringByName('Id');
    var JobId = getQueryStringByName('JobId');
    var jobNumber = getQueryStringByName('JobNumber');
    var subJobNumber = getQueryStringByName('SubJobNumber');
    // Edit Mode
    if (eplId != undefined && eplId != "") {
        FirstLoad();
        $("#btnSEshipmentno").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
    }
    else {
        $("#btn_add_inc_consignee").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        $("#btn_edit_inc_consignee").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        $("#btn_delete_inc_consignee").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        $("#btn_add_inc_agent").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        $("#btn_edit_inc_agent").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
        $("#btn_delete_inc_agent").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
    }
    // End First Load

    // Print EPL
    $('#seaimport_form_btn_print').click(function () {
        window.open(base_url + "EPL/PrintEPL?Id=" + eplId);
    });
    // END Print EPL

    function FirstLoad() {

        $.ajax({
            dataType: "json",
            url: base_url + "estimateprofitloss/GetInfo?Id=" + eplId,
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
                    //alert(result.objJob.ShipperCode);
                    shipmentId = result.ShipmentOrderId;
                    $("#txtSEshipmentno").val(result.ShipmentNo)
                        //.data('sslinekode', result.objJob.SSLineId).data('sslinekode2', result.objJob.SSLineCode).data('sslinename', result.objJob.SSLineName)
                        //.data('emklkode', result.objJob.EMKLId).data('emklkode2', result.objJob.EMKLCode).data('emklname', result.objJob.EMKLName)
                        //.data('depokode', result.objJob.DepoId).data('depokode2', result.objJob.DepoCode).data('deponame', result.objJob.DepoName);
                    $("#txtSEetd").val(dateEnt(result.ETD));
                    $("#txtSEconsignee").val(result.ConsigneeName).data('name', result.ConsigneeName).data('kode', result.ConsigneeId).data('kode2', result.ConsigneeCode);
                    $("#txtSEagent").val(result.AgentName).data('name', result.AgentName).data('kode', result.AgentId).data('kode2', result.AgentCode);
                   // $("#txtSEdateexchangerate").val(dateEnt(result.objJob.DateRate));
                   // $("#txtSEexchangerate").numberbox("setValue", result.objJob.Rate);
                   // $("#txtSEjobowner").val(result.objJob.JobOwner);
                    $("#txtSEcloseepl").val(result.DateClose);
                    $('#txtSEjobentrydate').val(dateEnt(result.JobEntryDate));

                    // Enable or Disable Button
                   // if (result.IsClosing || !result.objJob.IsValidEPLDate) {
                        if (result.IsClosing ) {
                        // Disable Save 
                        $('#seaimport_form_btn_save').removeAttr('onclick').unbind('click').off('click');
                        // Open / Close EPL
                        if (result.objJob.CloseEPL) {
                            $('#seaimport_form_btn_open_close_epl').linkbutton({
                                iconCls: 'icon-book',
                                text: 'Open EPL'
                            });
                        }

                        $('#btn_add_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_edit_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_delete_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_add_inc_agent').attr('disabled', 'disabled');
                        $('#btn_edit_inc_agent').attr('disabled', 'disabled');
                        $('#btn_delete_inc_agent').attr('disabled', 'disabled');
                    }


                    // Income
                    if (result.IncomeList != null) {
                        if (result.IncomeList.length > 0) {
                            for (var i = 0; i < result.IncomeList.length; i++) {
                               // (isEdit, eplDetailId, consigneeId, consigneeCode, consigneename, accountId, accountName, desc, currencyType, amountUsd, amountIdr,
                               //         quantity, perquantity, codingquantity, sign)
                                addIncConsignee(
                                    0,
                                    result.IncomeList[i].Id,
                                    result.IncomeList[i].ContactId,
                                    result.IncomeList[i].ContactCode,
                                    result.IncomeList[i].ContactName,
                                    result.IncomeList[i].CostId,
                                    result.IncomeList[i].CostName,
                                    result.IncomeList[i].Description,
                                    result.IncomeList[i].AmountCrr,
                                    result.IncomeList[i].AmountUSD,
                                    result.IncomeList[i].AmountIDR,
                                    result.IncomeList[i].Quantity,
                                    result.IncomeList[i].PerQty,
                                    result.IncomeList[i].CodingQuantity,
                                    result.IncomeList[i].Sign);
                            }
                        }
                    }


                    // Cost
                    if (result.CostList != null) {
                        if (result.CostList.length > 0) {
                            for (var i = 0; i < result.CostList.length; i++) {
                                addIncAgent(0,
                                    result.CostList[i].Id,
                                    result.CostList[i].ContactId,
                                    result.CostList[i].ContactCode,
                                    result.CostList[i].ContactName,
                                    result.CostList[i].CostId,
                                    result.CostList[i].CostName,
                                    result.CostList[i].Description,
                                    result.CostList[i].AmountCrr,
                                    result.CostList[i].AmountUSD,
                                    result.CostList[i].AmountIDR,
                                    result.CostList[i].Quantity,
                                    result.CostList[i].PerQty,
                                    result.CostList[i].CodingQuantity,
                                    result.CostList[i].Sign);
                            }
                        }
                    }
                   TotalEstimatedCalculation();
                }
            }
        });
    }

    // Total Estimated Calculation ------------------------------------------------------------------------------------------------
    function TotalEstimatedCalculation() {
        // Income 
        var estIncomeUSD = 0;
        var estIncomeIDR = 0;
        var data = $("#tbl_incconsignee").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estIncomeUSD += parseFloat(data[i].amountusd);
            estIncomeIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstIncConsigneeUsd").numberbox("setValue", estIncomeUSD);
        $("#txtEstIncConsigneeIdr").numberbox("setValue", estIncomeIDR);
        // END Income 
        
        // Cost
        var estCostUSD = 0;
        var estCostIDR = 0;
        var data = $("#tbl_incagent").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estCostUSD += parseFloat(data[i].amountusd);
            estCostIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstIncAgentUsd").numberbox("setValue", estCostUSD);
        $("#txtEstIncAgentIdr").numberbox("setValue", estCostIDR);
        // END Cost

        // Total
        var totalIncomeUsd = parseFloat(estIncomeUSD);
        var totalIncomeIdr = parseFloat(estIncomeIDR)
        var totalCostUsd = parseFloat(estCostUSD);
        var totalCostIdr = parseFloat(estCostIDR);
        //alert(estIncAgentUSD + "===" + estCostAgentUSD);
        var estTotalUSD = parseFloat(totalIncomeUsd) - parseFloat(totalCostUsd);
        estTotalUSD = Math.round(estTotalUSD * 100) / 100;
        var estTotalIDR = parseFloat(totalIncomeIdr) - parseFloat(totalCostIdr);
        estTotalIDR = Math.round(estTotalIDR * 100) / 100;
        $("#txtEstTotalUsd").numberbox("setValue", estTotalUSD);
        $("#txtEstTotalIdr").numberbox("setValue", estTotalIDR);
    }
    // END Total Estimated Calculation ------------------------------------------------------------------------------------------------

    // Inc Consignee Calculation ------------------------------------------------------------------------------------------------
    $("#txtISquantity, #txtISperunitcost").blur(function () {
        var total = parseFloat($('#txtISquantity').numberbox('getValue')) * parseFloat($('#txtISperunitcost').numberbox('getValue'));
        total = Math.round(total * 100) / 100;
        $('#txtISamount').numberbox('setValue', total);
    });
    //$("#txtISamount").blur(function () {
    //    $('#txtISquantity').numberbox('setValue', 0);
    //    $('#txtISperunitcost').numberbox('setValue', 0);
    //});
    // END Inc Consignee Calculation ------------------------------------------------------------------------------------------------

    // ------------------------------------------------------- LOOKUP ------------------
    // ----------------------- Shipment Order
    // Browse
    $('#btnSEshipmentno').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_shipmentorder');
        lookupGrid.setGridParam({ url: base_url + 'shipmentOrder/GetLookUp', postData: { filters: null, job: JobId }, page: 'last', search: false }).trigger("reloadGrid");
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
            $('#txtSEshipmentno').val(ret.shipmentorderno)
                .data('sslinekode', ret.sslineid).data('sslinekode2', ret.sslinecode).data('sslinename', ret.sslinename)
                .data('emklkode', ret.emklid).data('emklkode2', ret.emklcode).data('emklname', ret.emklname)
                .data('depokode', ret.depoid).data('depokode2', ret.depocode).data('deponame', ret.deponame);
            $('#txtSEetd').val(ret.etd);
            $('#txtSEconsignee').val(ret.shippername).data('name', ret.shippername).data('kode', ret.shipperid).data('kode2', ret.shippercode);
            $('#txtSEagent').val(ret.agentname).data('name', ret.agentname).data('kode', ret.agentid).data('kode2', ret.agentcode);
            $('#txtSEjobentrydate').val(dateEnt(ret.entrydate));

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

            jobNumber = ret.jobnumber;
            subJobNumber = ret.subjobno;
            shipmentId = id;

            // Clear and Reload all grid
            $("#tbl_incconsignee").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_incagent").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_costssline").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_costemkl").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_costrebate").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_costagent").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_costdepo").jqGrid("clearGridData", true).trigger("reloadGrid");

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
        colNames: ['Shipment Order', 'ETD', 'Consignee', 'Agent',''],
        colModel: [{ name: 'shipmentorderno', index: 'shipmentorderid', width: 130, align: "center" },
                  { name: 'etd', index: 'etd', width: 80, align: "right", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'shippername', index: 'consignename' },
                  { name: 'agentname', index: 'agentname' },
                  { name: 'entrydate', index: 'createdat' },
        ],
        pager: jQuery('#lookup_pager_shipmentorder'),
        sortname: "shipmentorderid",
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
    // End Shipment Order

    // --------------------------------------------------------------- Contact    // Browse Inc Consignee
    $('#btnISconsignee').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vConsignee = "inc";
        var lookupGrid = $('#lookup_table_consignee');
        lookupGrid.setGridParam({ url: base_url + 'MstContact/GetLookUp', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_consignee').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_consignee').click(function () {
        $('#lookup_div_consignee').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_consignee').click(function () {
        var id = jQuery("#lookup_table_consignee").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_consignee").jqGrid('getRowData', id);
            switch (vConsignee) {
                case 'inc':
                    $('#txtISkdconsignee').val(ret.code).data("kode", id);
                    $('#txtISnmconsignee').val(ret.name);
                    break;
                case 'cost':
                    $('#txtISkdrebate').val(ret.code).data("kode", id);
                    $('#txtISnmrebate').val(ret.name);
                    break;
                case 'demurrage':
                    $('#txtDemkdcustomer').val(ret.code).data("kode", id);
                    $('#txtDemnmcustomer').val(ret.name);
                    break;
            }
            $('#lookup_div_consignee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_consignee").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', '', 'Name', 'Contact As', '', '','', 'City'],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: "center" },
                  { name: 'companystatus', index: 'ContactStatus', width: 200, hidden: true },
                  { name: 'name', index: 'ContactName', width: 200 },
                  { name: 'contactas', index: 'ContactAs', width: 80, align: "center" },
                  { name: 'address', index: 'ContactAddress', hidden: true },
                  { name: 'citycode', index: 'citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_consignee'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "MasterCode",
        width: 460,
        height: 350
    });
    $("#lookup_table_consignee").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Consignee


    // ------------------------------------ Account - NON AGENT (Inc Consignee, Cost SSLine, Cost EMKL, Cost Rebate, Cost Depo), AGENT (Inc Agent, Cost Agent)
    // Browse Non Agent
    $('#btnISaccount').click(function () {

        // Get Number of Container

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vAccount = "nonagent";
        var lookupGrid = $('#lookup_table_account');
        lookupGrid.setGridParam({ url: base_url + 'Cost/GetLookUPPJK', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_account').dialog('open');
    });
  
    // Cancel or CLose
    $('#lookup_btn_cancel_account').click(function () {
        $('#lookup_div_account').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_account').click(function () {
        var id = jQuery("#lookup_table_account").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_account").jqGrid('getRowData', id);
            var check = true;
            switch (vAccount) {
                case "nonagent":

                    // Default
                    $('#optIScontainersize20').attr('disabled', 'disabled');
                    $('#optIScontainersize40').attr('disabled', 'disabled');
                    $('#optIScontainersize45').attr('disabled', 'disabled');
                    // Remove Disabled
                    $('#txtISquantity').removeAttr('disabled');
                    $('#txtISamount').removeAttr('disabled');

                    $('#optIScontainersizeall').removeAttr('disabled');
                    $('#optIScontainersizeall').attr("checked", "checked");

                    if (check) {
                        $('#txtISkdaccount').val(ret.code).data("kode", id).data("minidr", ret.minidr).data("minusd", ret.minusd).data("codingquantity", ret.quantity);
                        $('#txtISnmaccount').val(ret.name);
                        $('#txtISdesc').val(ret.name);
                        if ($('#optIScurrencytypeUSD').is(':checked')) {
                            $('#txtISamount').numberbox('setValue', ret.minusd);
                        }
                        else
                            $('#txtISamount').numberbox('setValue', ret.minidr);
                    }
                    else
                        $.messager.alert('Warning', "Job doesn't have Container !", 'warning');


                    break;
                case "incagent":

                    // Default
                    $('#optIAcontainersize20').attr('disabled', 'disabled');
                    $('#optIAcontainersize40').attr('disabled', 'disabled');
                    $('#optIAcontainersize45').attr('disabled', 'disabled');
                    // Remove Disabled
                    $('#txtIAquantity').removeAttr('disabled');
                    $('#txtIAamount').removeAttr('disabled');

                    $('#optIAcontainersizeall').removeAttr('disabled');
                    $('#optIAcontainersizeall').attr("checked", "checked");

                    // CodingQuantity
                    if (ret.quantity == "true") {

                        // Disable Size All
                        $('#optIAcontainersizeall').attr('disabled', 'disabled');
                        // Container 20'
                        if (vContainerCount20 > 0) {
                            $('#optIAcontainersize20').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtIAquantity').attr('disabled', 'disabled');
                            //$('#txtIAamount').attr('disabled', 'disabled');
                        }
                        // Container 40'
                        if (vContainerCount40 > 0) {
                            $('#optIAcontainersize40').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtIAquantity').attr('disabled', 'disabled');
                            //$('#txtIAamount').attr('disabled', 'disabled');
                        }
                        // Container 45'
                        if (vContainerCount45 > 0) {
                            $('#optIAcontainersize45').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtIAquantity').attr('disabled', 'disabled');
                            //$('#txtIAamount').attr('disabled', 'disabled');
                        }

                        $('#txtIAquantity').numberbox('setValue', 0);

                        if ($('#optIAcontainersizem3').is(":not(:disabled)"))
                            $('#optIAcontainersizem3').attr("checked", "checked");
                        else if ($('#optIAcontainersize20').is(":not(:disabled)")) {
                            $('#optIAcontainersize20').attr("checked", "checked");
                            $('#txtIAquantity').numberbox('setValue', vContainerCount20);
                        }
                        else if ($('#optIAcontainersize40').is(":not(:disabled)")) {
                            $('#optIAcontainersize40').attr("checked", "checked");
                            $('#txtIAquantity').numberbox('setValue', vContainerCount40);
                        }
                        else if ($('#optIAcontainersize45').is(":not(:disabled)")) {
                            $('#optIAcontainersize45').attr("checked", "checked");
                            $('#txtIAquantity').numberbox('setValue', vContainerCount45);
                        }
                        else
                            check = false;

                    }
                    else
                        $('#txtIAquantity').numberbox('setValue', 0);

                    if (check) {
                        $('#txtIAkdaccount').val(ret.code).data("kode", id).data("minidr", ret.minidr).data("minusd", ret.minusd).data("codingquantity", ret.quantity);
                        $('#txtIAnmaccount').val(ret.name);
                        $('#txtIAdesc').val(ret.name);
                        $('#txtIAamount').numberbox('setValue', ret.minusd);
                    }
                    else
                        $.messager.alert('Warning', "Job doesn't have Container !", 'warning');
                    break;
            }

            $('#lookup_div_account').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_account").jqGrid({
        url: base_url ,
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', ''],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: "center" },
                  { name: 'name', index: 'Name', width: 200 },
                  { name: 'minusd', index: 'ChargeUSD', width: 200, hidden: true },
                  { name: 'minidr', index: 'ChargeIDR', width: 200, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true }
        ],
        pager: jQuery('#lookup_pager_account'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortname : "Name",
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_account").jqGrid('navGrid', '#toolbar_lookup_table_account', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Account - NON AGENT (Inc Consignee, Cost SSLine, Cost EMKL, Cost Rebate, Cost Depo), AGENT (Inc Agent, Cost Agent)
    // ----------------- END LOOKUP ------------------

    // Non Agent Functionality
    // DisableOnEditNonAgent
    function disableOnEditNonAgent() {
        $('#btnISconsignee').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnISssline').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnISemkl').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnISrebate').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnISdepo').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnISaccount').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        //$('#optIScontainersize20').attr('disabled', 'disabled');
        //$('#optIScontainersize40').attr('disabled', 'disabled');
        //$('#optIScontainersize45').attr('disabled', 'disabled');
        //$('#optIScontainersizem3').attr('disabled', 'disabled');
        //$('#optIScontainersizeall').attr('disabled', 'disabled');
        $('#optIScurrencytypeIDR').attr('disabled', 'disabled');
        $('#optIScurrencytypeUSD').attr('disabled', 'disabled');
    }
    // EnableOnAddNonAgent
    function enableOnAddNonAgent() {
        $('#btnISconsignee').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnISssline').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnISemkl').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnISrebate').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnISdepo').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnISaccount').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        //$('#optIScontainersize20').removeAttr('disabled');
        //$('#optIScontainersize40').removeAttr('disabled');
        //$('#optIScontainersize45').removeAttr('disabled');
        //$('#optIScontainersizem3').removeAttr('disabled');
        //$('#optIScontainersizeall').removeAttr('disabled');
        $('#optIScurrencytypeIDR').removeAttr('disabled');
        $('#optIScurrencytypeUSD').removeAttr('disabled');
    }
    // Clear
    function clearNonAgent() {
        $('#txtISkdconsignee').val('').data('kode', '');
        $('#txtISnmconsignee').val('');
        $('#txtISkdssline').val('').data('kode', '');
        $('#txtISnmssline').val('');
        $('#txtISkdemkl').val('').data('kode', '');
        $('#txtISnmemkl').val('');
        $('#txtISkdrebate').val('').data('kode', '');
        $('#txtISnmrebate').val('');
        $('#txtISkddepo').val('').data('kode', '');
        $('#txtISnmdepo').val('');
        $('#txtISkdaccount').val('').data("kode", '');
        $('#txtISnmaccount').val('');
        $('#txtISdesc').val('');
        $('#txtISquantity').numberbox('setValue', 0);
        $('#txtISperunitcost').numberbox('setValue', 0);
        $('#txtISamount').numberbox('setValue', 0);
        $('#optIScontainersizeall').attr('checked', 'checked');
        $('#optIScurrencytypeUSD').attr('checked', 'checked');
        $('#cbSplitAccount').removeAttr('checked');
    }
    // Close
    $('#lookup_btn_cancel_non_agent').click(function () {
        clearNonAgent();
        $('#lookup_div_non_agent').dialog('close');
    });
    // Save
    $('#lookup_btn_add_non_agent').click(function () {
        // default
        var sign = true;
        var amountidr = 0;
        var amountusd = 0;
        var currencyTpe = 0;
        if ($('#optIScurrencytypeUSD').is(':checked'))
            amountusd = $('#txtISamount').numberbox('getValue');
        if ($('#optIScurrencytypeIDR').is(':checked')) {
            currencyTpe = 1;
            amountidr = $('#txtISamount').numberbox('getValue');
        }

        // Add or Edit
        var submitURL = "";

        // Assigned EPL Detail Id
        var eplDetailId = 0
        if (vNonAgentEdit == 1) {
            eplDetailId = $('#lookup_btn_add_non_agent').data('epldetailid');
        }
        
        switch (vNonAgentType) {
            case 'income':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "EstimateProfitLoss/InsertDetail" : base_url + "EstimateProfitLoss/UpdateDetail" : "";
                var isValid = true;
                var IsIncome = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'),
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, IsIncome, function (result) {
                                    if (JSON.stringify(result.Errors) != '{}') {
                                        for (var key in result.Errors) {
                                            if (key != null && key != undefined && key != 'Generic') {
                                                $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                                $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                            }
                                            else {
                                                $.messager.alert('Warning', data.model.Errors[key], 'warning');
                                            }
                                        }
                                    }
                                    else {
                                        $("#txtSEconsignee").data('kode', $('#txtISkdconsignee').data('kode')).data('kode2', $('#txtISkdconsignee').val());
                                        $("#txtSEconsignee").data('name', $('#txtISnmconsignee').val());

                                        addIncConsignee(vNonAgentEdit, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdconsignee').val(), $('#txtISnmconsignee').val(), $('#txtISkdaccount').data("kode"),
                                            $('#txtISnmaccount').val(), $('#txtISdesc').val(), currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                                            $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign);
                                        $('#lookup_div_non_agent').dialog('close');
                                        clearNonAgent();
                                    }
                                });
                break;
            case 'cost':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "EstimateProfitLoss/InsertDetail" : base_url + "EstimateProfitLoss/UpdateDetail" : "";
                var isValid = true;
                var IsIncome = false;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'),
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, IsIncome, function (result) {
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
                                        $("#txtSEconsignee").data('kode', $('#txtISkdconsignee').data('kode')).data('kode2', $('#txtISkdconsignee').val());
                                        $("#txtSEconsignee").data('name', $('#txtISnmconsignee').val());

                                        addIncAgent(vNonAgentEdit, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdconsignee').val(), $('#txtISnmconsignee').val(), $('#txtISkdaccount').data("kode"),
                                            $('#txtISnmaccount').val(), $('#txtISdesc').val(), currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                                            $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign);
                                        $('#lookup_div_non_agent').dialog('close');
                                        clearNonAgent();
                                    }
                                });
                break;
        }

        // Reset CodingQuantity
        $('#txtISkdaccount').data("codingquantity", 0);

        // Calculate Total Estimated
        TotalEstimatedCalculation();
        $('#lookup_div_non_agent').dialog('close');
    });
    // END Non Agent Functionality


    // ------------------------------------------------------ Inc Consignee
    // Currency Type Selection
    $('input[name=optIScurrencytype]').click(function () {
        if ($('#optIScurrencytypeUSD').is(':checked'))
            $('#txtISamount').numberbox('setValue', $('#txtISkdaccount').data("minusd"));
        else
            $('#txtISamount').numberbox('setValue', $('#txtISkdaccount').data("minidr"));
    });
    // ADD
    $('#btn_add_inc_consignee').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddNonAgent();
        vNonAgentType = 'income';
        $('.non_agent_info').hide();
        $('.consignee_info').show();

        vNonAgentEdit = 0;
        // Clear Input
        clearNonAgent();
        //$('#txtISkdconsignee').data('kode', $("#txtSEconsignee").data('kode')).val($("#txtSEconsignee").data('kode2'));
       // $('#txtISnmconsignee').val($("#txtSEconsignee").data('name'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Income' });
        $('#lookup_div_non_agent').dialog('open');
    });
    // Delete
    $('#btn_delete_inc_consignee').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_incconsignee").jqGrid('getGridParam', 'selrow');
                if (id) {
                    // Validate Account EPL on Payment Request and Invoice
                    var ret = jQuery("#tbl_incconsignee").jqGrid('getRowData', id);
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (result) {
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
                            isValid = true;
                            message = "";
                            DeleteEPLDetail(id, function (result) {
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
                                    $('#lookup_btn_add_non_agent').data('epldetailid', 0);
                                    jQuery("#tbl_incconsignee").jqGrid('delRowData', id);
                                }
                            });
                            // Calculate Total Estimated
                            TotalEstimatedCalculation();
                        }
                    });
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_inc_consignee').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'income';
        $('.non_agent_info').hide();
        $('.consignee_info').show();

        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_incconsignee").jqGrid('getGridParam', 'selrow');
        if (id) {

            // Validate Account EPL on Payment Request and Invoice
            var ret = jQuery("#tbl_incconsignee").jqGrid('getRowData', id);
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (result) {
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
                    vNonAgentEdit = 1;

                    // Assigned EPL Detail Id to Button Save
                    $('#lookup_btn_add_non_agent').data('epldetailid', id);

                    $('#txtISkdconsignee').data('kode', ret.consigneeid).val(ret.consigneecode);
                    $('#txtISnmconsignee').val(ret.consigneename);
                    $('#txtISnmaccount').val(ret.accountname);
                    $('#txtISdesc').val(ret.description);
                    if (ret.currencytype == 0)   // USD
                    {
                        $('#txtISamount').numberbox('setValue', ret.amountusd);
                        $('#optIScurrencytypeUSD').attr('checked', 'checked');
                    }
                    else  // IDR
                    {
                        $('#txtISamount').numberbox('setValue', ret.amountidr);
                        $('#optIScurrencytypeIDR').attr('checked', 'checked');
                    }
                    $('#txtISkdaccount').data("kode", ret.accountid).val(ret.accountid);
                    
                    $('#txtISquantity').numberbox('setValue', ret.quantity);
                    $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                    $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                    $('#lookup_div_non_agent').dialog({ title: 'Income' });
                    $('#lookup_div_non_agent').dialog('open');
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Add or Edit
    function addIncConsignee(isEdit, eplDetailId, consigneeId, consigneeCode, consigneename, accountId, accountName, desc, currencyType, amountUsd, amountIdr,
                                        quantity, perquantity, codingquantity, sign) {
        // Validate
        if (accountId == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (consigneeCode == "" || parseInt(consigneeCode) == 0) {
            $.messager.alert('Information', 'Invalid Consignee...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {
            var newData = {};
            newData.consigneeid = consigneeId;
            newData.consigneecode = consigneeCode;
            newData.consigneename = (consigneename != undefined && consigneename != "") ? consigneename.toUpperCase() : "";
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.description = (desc != undefined && desc != "") ? desc.toUpperCase() : "";
            var amountidr = 0;
            var amountusd = 0;
            if (currencyType == 0) // USD
                amountusd = amountUsd;
            if (currencyType == 1) // IDR
                amountidr = amountIdr;
            newData.amountusd = amountusd;
            newData.amountidr = amountidr;
            newData.currencytype = currencyType;
            newData.accountid = accountId;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            // Demurrage
            //newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incconsignee")) : eplDetailId;
                jQuery("#tbl_incconsignee").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_incconsignee").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_incconsignee").jqGrid('setRowData', id, newData);
                }
            }
            // Sorting
            $("#tbl_incconsignee").trigger("reloadGrid");
        }
    }

    function addIncAgent(isEdit, eplDetailId, consigneeId, consigneeCode, consigneename, accountId, accountName, desc, currencyType, amountUsd, amountIdr,
                                       quantity, perquantity, codingquantity, sign) {
        // Validate
        if (accountId == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (consigneeCode == "" || parseInt(consigneeCode) == 0) {
            $.messager.alert('Information', 'Invalid Contact...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {

            var newData = {};
            newData.consigneeid = consigneeId;
            newData.consigneecode = consigneeCode;
            newData.consigneename = (consigneename != undefined && consigneename != "") ? consigneename.toUpperCase() : "";
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.description = (desc != undefined && desc != "") ? desc.toUpperCase() : "";
            var amountidr = 0;
            var amountusd = 0;
            if (currencyType == 0) // USD
                amountusd = amountUsd;
            if (currencyType == 1) // IDR
                amountidr = amountIdr;
            newData.amountusd = amountusd;
            newData.amountidr = amountidr;
            newData.currencytype = currencyType;
            newData.accountid = accountId;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            // Demurrage
            //newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incagent")) : eplDetailId;
                jQuery("#tbl_incagent").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_incagent").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_incagent").jqGrid('setRowData', id, newData);
                }
            }
            // Sorting
            $("#tbl_incagent").trigger("reloadGrid");
        }
    }
    // Declare Table jqgrid
    jQuery("#tbl_incconsignee").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['ConsigneeId', 'ConsigneeCode', 'Contact', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '',
                        'Dmr', 'DmrContainerDetail', 'DmrContainer', ''],
        colModel: [
            { name: 'consigneeid', index: 'consigneeid', width: 60, hidden: true },
            { name: 'consigneecode', index: 'consigneecode', width: 60, hidden: true },
            { name: 'consigneename', index: 'consigneename', width: 250 },
            { name: 'accountname', index: 'accountname', width: 350, hidden: true },
            { name: 'description', index: 'description', width: 350 },
            { name: 'amountusd', index: 'amountusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'amountidr', index: 'amountidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currencytype', index: 'currencytype', width: 60, hidden: true },
            { name: 'accountid', index: 'accountid', width: 60, hidden: true },
            { name: 'type', index: 'type', width: 60, hidden: true },
            { name: 'quantity', index: 'quantity', width: 60, hidden: true },
            { name: 'perqty', index: 'perqty', width: 60, hidden: true },
            { name: 'codingquantity', index: 'codingquantity', width: 60, hidden: true },
            { name: 'sign', index: 'sign', width: 30, align: "center" },
            { name: 'dmr', index: 'dmr', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainerdetail', index: 'dmrcontainerdetail', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainer', index: 'dmrcontainer', width: 50, align: "right", hidden: true },
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Inc Consignee

    // Declare Table jqgrid
    jQuery("#tbl_incagent").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['ConsigneeId', 'ConsigneeCode', 'Contact', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '',
                        'Dmr', 'DmrContainerDetail', 'DmrContainer', ''],
        colModel: [
            { name: 'consigneeid', index: 'consigneeid', width: 60, hidden: true },
            { name: 'consigneecode', index: 'consigneecode', width: 60, hidden: true },
            { name: 'consigneename', index: 'consigneename', width: 250 },
            { name: 'accountname', index: 'accountname', width: 350, hidden: true },
            { name: 'description', index: 'description', width: 350 },
            { name: 'amountusd', index: 'amountusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'amountidr', index: 'amountidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currencytype', index: 'currencytype', width: 60, hidden: true },
            { name: 'accountid', index: 'accountid', width: 60, hidden: true },
            { name: 'type', index: 'type', width: 60, hidden: true },
            { name: 'quantity', index: 'quantity', width: 60, hidden: true },
            { name: 'perqty', index: 'perqty', width: 60, hidden: true },
            { name: 'codingquantity', index: 'codingquantity', width: 60, hidden: true },
            { name: 'sign', index: 'sign', width: 30, align: "center" },
            { name: 'dmr', index: 'dmr', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainerdetail', index: 'dmrcontainerdetail', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainer', index: 'dmrcontainer', width: 50, align: "right", hidden: true },
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Inc Agent
    // ------------------------------------------------------ Inc Agent
    // ADD
    $('#btn_add_inc_agent').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddNonAgent();
        vNonAgentType = 'cost';
        $('.non_agent_info').hide();
        $('.consignee_info').show();

        vNonAgentEdit = 0;
        // Clear Input
        clearNonAgent();

        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Cost' });
        $('#lookup_div_non_agent').dialog('open');

    });

    // Delete
    $('#btn_delete_inc_agent').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_incagent").jqGrid('getGridParam', 'selrow');
                if (id) {
                    var ret = jQuery("#tbl_incagent").jqGrid('getRowData', id);

                    // Validate Account EPL on Payment Request and Invoice
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (result) {
                        if (JSON.stringify(result.Errors) != '{}') {
                            for (var key in result.Errors) {
                                if (key != null && key != undefined && key != 'Generic') {
                                    $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                    $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                }
                                else {
                                    $.messager.alert('Warning', result.model.Errors[key], 'warning');
                                }
                            }
                        }
                        else {
                            isValid = true;
                            message = "";
                            DeleteEPLDetail(id, function (result) {
                                if (JSON.stringify(result.Errors) != '{}') {
                                    for (var key in result.Errors) {
                                        if (key != null && key != undefined && key != 'Generic') {
                                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                        }
                                        else {
                                            $.messager.alert('Warning', result.model.Errors[key], 'warning');
                                        }
                                    }
                                }
                                else {
                                    // Removed EPL Detail Id from Button Save
                                    $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                                    jQuery("#tbl_incagent").jqGrid('delRowData', id);
                                }
                            });
                            TotalEstimatedCalculation();
                        }
                    });
                   
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });

    // Edit
    $('#btn_edit_inc_agent').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'cost';
        $('.non_agent_info').hide();
        $('.consignee_info').show();

        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_incagent").jqGrid('getGridParam', 'selrow');
        if (id) {

            // Validate Account EPL on Payment Request and Invoice
            var ret = jQuery("#tbl_incagent").jqGrid('getRowData', id);
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.model.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    vNonAgentEdit = 1;

                    // Assigned EPL Detail Id to Button Save
                    $('#lookup_btn_add_non_agent').data('epldetailid', id);

                    $('#txtISkdconsignee').data('kode', ret.consigneeid).val(ret.consigneecode);
                    $('#txtISnmconsignee').val(ret.consigneename);
                    $('#txtISnmaccount').val(ret.accountname);
                    $('#txtISdesc').val(ret.description);
                    if (ret.currencytype == 0)   // USD
                    {
                        $('#txtISamount').numberbox('setValue', ret.amountusd);
                        $('#optIScurrencytypeUSD').attr('checked', 'checked');
                    }
                    else  // IDR
                    {
                        $('#txtISamount').numberbox('setValue', ret.amountidr);
                        $('#optIScurrencytypeIDR').attr('checked', 'checked');
                    }
                    $('#txtISkdaccount').data("kode", ret.accountid).val(ret.accountid);

                    $('#txtISquantity').numberbox('setValue', ret.quantity);
                    $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                    $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                    $('#lookup_div_non_agent').dialog({ title: 'Cost' });
                    $('#lookup_div_non_agent').dialog('open');
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    // Add or Edit
   

   

    function GetNextId(objJqGrid) {
        var nextid = 0;
        if (objJqGrid.getDataIDs().length > 0)
            nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

        return nextid;
    }

   
    // ------------------------------------------------------------------------ SAVE EPL
    $('#seaimport_form_btn_save').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }


        var submitURL = base_url + "EstimateProfitLoss/Insert";

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                ShipmentOrderId: shipmentId,
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
                    window.location = base_url + "estimateprofitloss/detail?Id=" + result.eplId + "&JobId=" + JobId;
                }
            }
        });
    });

    // ValidateAccountEPL
    function ValidateAccountEPL(v_eplId, v_epldetailId, callback) {

        // Check Payment Request && Invoice
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "EstimateProfitLoss/ValidateEPL",
            data: JSON.stringify({
                Id: v_epldetailId,
                EPLId: v_eplId
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
    // END ValidateAccountEPL

    // Save EPL Detail
    function SaveEPLDetail(submitURL, v_epldetailId, v_customerId, v_accountId, v_description, v_quantity, v_perqty, v_codingquantity,
                                        v_sign, v_amountCrr, v_amountUSD, v_amountIDR, v_isincome,callback) {
        if (submitURL != undefined && submitURL != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    Id: v_epldetailId,
                    EstimateProfitLossId: eplId, ContactId: v_customerId,
                    CostId: v_accountId, Description: v_description, Quantity: v_quantity, PerQty: v_perqty,
                    Sign: v_sign, CodingQuantity: v_codingquantity,
                    AmountCrr: v_amountCrr, AmountUSD: v_amountUSD, AmountIDR: v_amountIDR,IsIncome : v_isincome,
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
    // END Save EPL Detail

    // Delete EPL Detail
    function DeleteEPLDetail(v_epldetailId, callback) {
        if (eplId != undefined && eplId != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + 'EstimateProfitLoss/DeleteDetail',
                data: JSON.stringify({
                    Id: v_epldetailId
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
    // END Delete EPL Detail



});