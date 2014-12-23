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
    // End First Load

    // Print EPL
    $('#seaimport_form_btn_print').click(function () {
        window.open(base_url + "EPL/PrintEPL?Id=" + eplId);
    });
    // END Print EPL

    function FirstLoad() {

        $.ajax({
            dataType: "json",
            url: base_url + "epl/GetEPLInfo?Id=" + eplId,
            success: function (result) {
                if (!result.isValid)
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "epl";
                    });
                else {
                    //alert(result.objJob.ShipperCode);
                    shipmentId = result.objJob.ShipmentId;
                    $("#txtSEshipmentno").val(result.objJob.ShipmentNo)
                        .data('sslinekode', result.objJob.SSLineId).data('sslinekode2', result.objJob.SSLineCode).data('sslinename', result.objJob.SSLineName)
                        .data('emklkode', result.objJob.EMKLId).data('emklkode2', result.objJob.EMKLCode).data('emklname', result.objJob.EMKLName)
                        .data('depokode', result.objJob.DepoId).data('depokode2', result.objJob.DepoCode).data('deponame', result.objJob.DepoName);
                    $("#txtSEetd").val(dateEnt(result.objJob.ETDETA));
                    $("#txtSEconsignee").val(result.objJob.ConsigneeName).data('name', result.objJob.ConsigneeName).data('kode', result.objJob.ConsigneeId).data('kode2', result.objJob.ConsigneeCode);
                    $("#txtSEagent").val(result.objJob.AgentName).data('name', result.objJob.AgentName).data('kode', result.objJob.AgentId).data('kode2', result.objJob.AgentCode);
                    $("#txtSEdateexchangerate").val(dateEnt(result.objJob.DateRate));
                    $("#txtSEexchangerate").numberbox("setValue", result.objJob.Rate);
                    $("#txtSEjobowner").val(result.objJob.JobOwner);
                    $("#txtSEcloseepl").val(result.objJob.TimeCloseEPL);
                    $('#txtSEjobentrydate').val(dateEnt(result.objJob.JobEntryDate));

                    $("#txtEstTotalUsd").numberbox("setValue", result.objJob.SumUSD);
                    $("#txtEstConsigneeUsd").numberbox("setValue", result.objJob.EstUSDShipCons);
                    $("#txtEstAgentUsd").numberbox("setValue", result.objJob.EstUSDAgent);
                    $("#txtEstTotalIdr").numberbox("setValue", result.objJob.SumIDR);
                    $("#txtEstConsigneeIdr").numberbox("setValue", result.objJob.EstIDRShipCons);
                    $("#txtEstAgentIdr").numberbox("setValue", result.objJob.EstIDRAgent);

                    // Enable or Disable Button
                    if (result.objJob.IsClosing || !result.objJob.IsValidEPLDate) {
                        // Disable Save 
                        $('#seaimport_form_btn_save').removeAttr('onclick').unbind('click').off('click');
                        // Open / Close EPL
                        if (result.objJob.CloseEPL) {
                            $('#seaimport_form_btn_open_close_epl').linkbutton({
                                iconCls: 'icon-book',
                                text: 'Open EPL'
                            });
                        }
                        //else {
                        //    // Disable Close EPL button
                        //    $('#seaimport_form_btn_open_close_epl').removeAttr('onclick').unbind('click').off('click');
                        //}

                        $('#btn_add_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_edit_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_delete_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_demurrage_inc_consignee').attr('disabled', 'disabled');
                        $('#btn_add_inc_agent').attr('disabled', 'disabled');
                        $('#btn_edit_inc_agent').attr('disabled', 'disabled');
                        $('#btn_delete_inc_agent').attr('disabled', 'disabled');
                        $('#btn_add_inc_agent_handlingfee').attr('disabled', 'disabled');
                        $('#btn_add_inc_agent_profitsplit').attr('disabled', 'disabled');
                        $('#btn_add_cost_ssline').attr('disabled', 'disabled');
                        $('#btn_edit_cost_ssline').attr('disabled', 'disabled');
                        $('#btn_delete_cost_ssline').attr('disabled', 'disabled');
                        $('#btn_add_cost_emkl').attr('disabled', 'disabled');
                        $('#btn_edit_cost_emkl').attr('disabled', 'disabled');
                        $('#btn_delete_cost_emkl').attr('disabled', 'disabled');
                        $('#btn_add_cost_rebate').attr('disabled', 'disabled');
                        $('#btn_edit_cost_rebate').attr('disabled', 'disabled');
                        $('#btn_delete_cost_rebate').attr('disabled', 'disabled');
                        $('#btn_add_cost_agent').attr('disabled', 'disabled');
                        $('#btn_edit_cost_agent').attr('disabled', 'disabled');
                        $('#btn_delete_cost_agent').attr('disabled', 'disabled');
                        $('#btn_demurrage_cost_agent').attr('disabled', 'disabled');
                        $('#btn_add_cost_agent_handlingfee').attr('disabled', 'disabled');
                        $('#btn_add_cost_agent_profitsplit').attr('disabled', 'disabled');
                        $('#btn_add_cost_depo').attr('disabled', 'disabled');
                        $('#btn_edit_cost_depo').attr('disabled', 'disabled');
                        $('#btn_delete_cost_depo').attr('disabled', 'disabled');
                    }

                    // Check TypeEPL
                    if (parseInt(result.objJob.TypeEPL) < 9 || parseInt(result.objJob.TypeEPL) == 13) {
                        // Check Container have 20', 40' 45'
                        var containerSizeList = result.objJob.ContainerSizeList;
                        if (containerSizeList != null || containerSizeList != undefined) {
                            for (var i = 0; i < containerSizeList.length; i++) {
                                switch (containerSizeList[i].SizeType) {
                                    case 1:   // Container 20'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            //$('#optIScontainersize20').removeAttr('disabled');
                                            //$('#optIAcontainersize20').removeAttr('disabled');

                                            vContainerCount20 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                    case 2:   // Container 40'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            //$('#optIScontainersize40').removeAttr('disabled');
                                            //$('#optIAcontainersize40').removeAttr('disabled');

                                            vContainerCount40 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                    case 3:   // Container 45'
                                        if (parseInt(containerSizeList[i].SizeCount) > 0) {
                                            //$('#optIScontainersize45').removeAttr('disabled');
                                            //$('#optIAcontainersize45').removeAttr('disabled');

                                            vContainerCount45 = parseInt(containerSizeList[i].SizeCount);
                                        }
                                        break;
                                }
                            }
                        }
                    }

                    // Inc Consignee
                    if (result.objJob.IncConsigneeList != null) {
                        if (result.objJob.IncConsigneeList.length > 0) {
                            for (var i = 0; i < result.objJob.IncConsigneeList.length; i++) {
                                addIncConsignee(0, result.objJob.IncConsigneeList[i].EPLDetailId,
                                    result.objJob.IncConsigneeList[i].ConsigneeId,
                                    result.objJob.IncConsigneeList[i].ConsigneeCode, result.objJob.IncConsigneeList[i].ConsigneeName,
                                    result.objJob.IncConsigneeList[i].AccountID, result.objJob.IncConsigneeList[i].AccountName,
                                    result.objJob.IncConsigneeList[i].Description,
                                    result.objJob.IncConsigneeList[i].Type, result.objJob.IncConsigneeList[i].AmountCrr,
                                    result.objJob.IncConsigneeList[i].AmountUSD, result.objJob.IncConsigneeList[i].AmountIDR,
                                    result.objJob.IncConsigneeList[i].Quantity, result.objJob.IncConsigneeList[i].Perqty,
                                    result.objJob.IncConsigneeList[i].CodingQuantity, result.objJob.IncConsigneeList[i].Sign,
                                    result.objJob.IncConsigneeList[i].Dmr, result.objJob.IncConsigneeList[i].DmrContainerDetail, result.objJob.IncConsigneeList[i].DmrContainer);
                            }
                        }
                    }

                    // Cost SSLine
                    if (result.objJob.CostSSLineList != null) {
                        if (result.objJob.CostSSLineList.length > 0) {
                            for (var i = 0; i < result.objJob.CostSSLineList.length; i++) {
                                addCostSSLine(0, result.objJob.CostSSLineList[i].EPLDetailId,
                                    result.objJob.CostSSLineList[i].SSLineId,
                                    result.objJob.CostSSLineList[i].SSLineCode, result.objJob.CostSSLineList[i].SSLineName,
                                    result.objJob.CostSSLineList[i].AccountID, result.objJob.CostSSLineList[i].AccountName,
                                    result.objJob.CostSSLineList[i].Description,
                                    result.objJob.CostSSLineList[i].Type, result.objJob.CostSSLineList[i].AmountCrr,
                                    result.objJob.CostSSLineList[i].AmountUSD, result.objJob.CostSSLineList[i].AmountIDR,
                                    result.objJob.CostSSLineList[i].Quantity, result.objJob.CostSSLineList[i].Perqty,
                                    result.objJob.CostSSLineList[i].CodingQuantity, result.objJob.CostSSLineList[i].Sign);
                            }
                        }
                    }

                    // Cost Rebate(Consignee)
                    if (result.objJob.CostRebateConsigneeList != null) {
                        if (result.objJob.CostRebateConsigneeList.length > 0) {
                            for (var i = 0; i < result.objJob.CostRebateConsigneeList.length; i++) {
                                addCostRebate(0, result.objJob.CostRebateConsigneeList[i].EPLDetailId,
                                    result.objJob.CostRebateConsigneeList[i].ConsigneeId,
                                    result.objJob.CostRebateConsigneeList[i].ConsigneeCode, result.objJob.CostRebateConsigneeList[i].ConsigneeName,
                                    result.objJob.CostRebateConsigneeList[i].AccountID, result.objJob.CostRebateConsigneeList[i].AccountName,
                                    result.objJob.CostRebateConsigneeList[i].Description,
                                    result.objJob.CostRebateConsigneeList[i].Type, result.objJob.CostRebateConsigneeList[i].AmountCrr,
                                    result.objJob.CostRebateConsigneeList[i].AmountUSD, result.objJob.CostRebateConsigneeList[i].AmountIDR,
                                    result.objJob.CostRebateConsigneeList[i].Quantity, result.objJob.CostRebateConsigneeList[i].Perqty,
                                    result.objJob.CostRebateConsigneeList[i].CodingQuantity, result.objJob.CostRebateConsigneeList[i].Sign);
                            }
                        }
                    }


                    // Cost EMKL
                    if (result.objJob.CostEMKLList != null) {
                        if (result.objJob.CostEMKLList.length > 0) {
                            for (var i = 0; i < result.objJob.CostEMKLList.length; i++) {
                                addCostEMKL(0, result.objJob.CostEMKLList[i].EPLDetailId,
                                    result.objJob.CostEMKLList[i].EMKLId,
                                    result.objJob.CostEMKLList[i].EMKLCode, result.objJob.CostEMKLList[i].EMKLName,
                                    result.objJob.CostEMKLList[i].AccountID, result.objJob.CostEMKLList[i].AccountName,
                                    result.objJob.CostEMKLList[i].Description,
                                    result.objJob.CostEMKLList[i].Type, result.objJob.CostEMKLList[i].AmountCrr,
                                    result.objJob.CostEMKLList[i].AmountUSD, result.objJob.CostEMKLList[i].AmountIDR,
                                    result.objJob.CostEMKLList[i].Quantity, result.objJob.CostEMKLList[i].Perqty,
                                    result.objJob.CostEMKLList[i].CodingQuantity, result.objJob.CostEMKLList[i].Sign);
                            }
                        }
                    }

                    // Cost Depo
                    if (result.objJob.CostDepoList != null) {
                        if (result.objJob.CostDepoList.length > 0) {
                            for (var i = 0; i < result.objJob.CostDepoList.length; i++) {
                                addCostDepo(0, result.objJob.CostDepoList[i].EPLDetailId,
                                    result.objJob.CostDepoList[i].DepoId,
                                    result.objJob.CostDepoList[i].DepoCode, result.objJob.CostDepoList[i].DepoName,
                                    result.objJob.CostDepoList[i].AccountID, result.objJob.CostDepoList[i].AccountName,
                                    result.objJob.CostDepoList[i].Description,
                                    result.objJob.CostDepoList[i].Type, result.objJob.CostDepoList[i].AmountCrr,
                                    result.objJob.CostDepoList[i].AmountUSD, result.objJob.CostDepoList[i].AmountIDR,
                                    result.objJob.CostDepoList[i].Quantity, result.objJob.CostDepoList[i].Perqty,
                                    result.objJob.CostDepoList[i].CodingQuantity, result.objJob.CostDepoList[i].Sign);
                            }
                        }
                    }

                    // Inc Agent
                    if (result.objJob.IncAgentList != null) {
                        if (result.objJob.IncAgentList.length > 0) {
                            for (var i = 0; i < result.objJob.IncAgentList.length; i++) {
                                addIncAgent(0, result.objJob.IncAgentList[i].EPLDetailId,
                                    result.objJob.IncAgentList[i].AgentId,
                                    result.objJob.IncAgentList[i].AgentCode, result.objJob.IncAgentList[i].AgentName,
                                    result.objJob.IncAgentList[i].AccountID, result.objJob.IncAgentList[i].AccountName,
                                    result.objJob.IncAgentList[i].Description,
                                    result.objJob.IncAgentList[i].Type, result.objJob.IncAgentList[i].AmountUSD,
                                    result.objJob.IncAgentList[i].Quantity, result.objJob.IncAgentList[i].Perqty,
                                    result.objJob.IncAgentList[i].CodingQuantity, result.objJob.IncAgentList[i].Sign,
                                    result.objJob.IncAgentList[i].HFPSinfo);
                            }
                        }
                    }

                    // Cost Agent
                    if (result.objJob.CostAgentList != null) {
                        if (result.objJob.CostAgentList.length > 0) {
                            for (var i = 0; i < result.objJob.CostAgentList.length; i++) {
                                addCostAgent(0, result.objJob.CostAgentList[i].EPLDetailId,
                                    result.objJob.CostAgentList[i].AgentId,
                                    result.objJob.CostAgentList[i].AgentCode, result.objJob.CostAgentList[i].AgentName,
                                    result.objJob.CostAgentList[i].AccountID, result.objJob.CostAgentList[i].AccountName,
                                    result.objJob.CostAgentList[i].Description,
                                    result.objJob.CostAgentList[i].Type, result.objJob.CostAgentList[i].AmountUSD,
                                    result.objJob.CostAgentList[i].Quantity, result.objJob.CostAgentList[i].Perqty,
                                    result.objJob.CostAgentList[i].CodingQuantity, result.objJob.CostAgentList[i].Sign,
                                    result.objJob.CostAgentList[i].HFPSinfo,
                                    result.objJob.CostAgentList[i].Dmr, result.objJob.CostAgentList[i].DmrContainerDetail, result.objJob.CostAgentList[i].DmrContainer);
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
        // Income Consignee
        var estIncConsigneeUSD = 0;
        var estIncConsigneeIDR = 0;
        var data = $("#tbl_incconsignee").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estIncConsigneeUSD += parseFloat(data[i].amountusd);
            estIncConsigneeIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstIncConsigneeUsd").numberbox("setValue", estIncConsigneeUSD);
        $("#txtEstIncConsigneeIdr").numberbox("setValue", estIncConsigneeIDR);
        // END Income Consignee
        // Cost SSLine
        var estCostSSLineUSD = 0;
        var estCostSSLineIDR = 0;
        var data = $("#tbl_costssline").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estCostSSLineUSD += parseFloat(data[i].amountusd);
            estCostSSLineIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstCostSSLineUsd").numberbox("setValue", estCostSSLineUSD);
        $("#txtEstCostSSLineIdr").numberbox("setValue", estCostSSLineIDR);
        // END Cost SSLine
        // Cost EMKL
        var estCostEMKLUSD = 0;
        var estCostEMKLIDR = 0;
        var data = $("#tbl_costemkl").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estCostEMKLUSD += parseFloat(data[i].amountusd);
            estCostEMKLIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstCostEMKLUsd").numberbox("setValue", estCostEMKLUSD);
        $("#txtEstCostEMKLIdr").numberbox("setValue", estCostEMKLIDR);
        // END Cost EMKL
        // Cost Rebate
        var estCostRebateUSD = 0;
        var estCostRebateIDR = 0;
        var data = $("#tbl_costrebate").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estCostRebateUSD += parseFloat(data[i].amountusd);
            estCostRebateIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstCostRebateUsd").numberbox("setValue", estCostRebateUSD);
        $("#txtEstCostRebateIdr").numberbox("setValue", estCostRebateIDR);
        // END Cost Rebate
        // Cost Rebate
        var estCostDepoUSD = 0;
        var estCostDepoIDR = 0;
        var data = $("#tbl_costdepo").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            estCostDepoUSD += parseFloat(data[i].amountusd);
            estCostDepoIDR += parseFloat(data[i].amountidr);
        }
        $("#txtEstCostDepoUsd").numberbox("setValue", estCostDepoUSD);
        $("#txtEstCostDepoIdr").numberbox("setValue", estCostDepoIDR);
        // END Cost Rebate

        // Income Agent
        var estIncAgentUSD = 0;
        var estIncAgentIDR = 0;
        // Income
        var data = $("#tbl_incagent").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            if (data[i].accountid != '003' && data[i].accountid != '004') { // Ignore Buying Rate and Selling Rate
                if (data[i].sign == '-') {
                    estIncAgentUSD -= parseFloat(data[i].amountusd);
                    //estAgentIDR -= parseFloat(data[i].amountidr);
                }
                else if (data[i].sign == '+') {
                    estIncAgentUSD += parseFloat(data[i].amountusd);
                    //estAgentIDR += parseFloat(data[i].amountidr);
                }
            }
        }
        $("#txtEstIncAgentUsd").numberbox("setValue", estIncAgentUSD);
        $("#txtEstIncAgentIdr").numberbox("setValue", estIncAgentIDR);
        // END Income Agent
        // Cost Agent
        var estCostAgentUSD = 0;
        var estCostAgentIDR = 0;
        var data = $("#tbl_costagent").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            if (data[i].accountid != '003' && data[i].accountid != '004') { // Ignore Buying Rate and Selling Rate
                if (data[i].sign == '-') {
                    estCostAgentUSD -= parseFloat(data[i].amountusd);
                    //estAgentIDR -= parseFloat(data[i].amountidr);
                }
                else if (data[i].sign == '+') {
                    estCostAgentUSD += parseFloat(data[i].amountusd);
                    //estAgentIDR += parseFloat(data[i].amountidr);
                }
            }
        }
        $("#txtEstCostAgentUsd").numberbox("setValue", estCostAgentUSD);
        $("#txtEstCostAgentIdr").numberbox("setValue", estCostAgentIDR);
        // END Cost Agent
        // Total
        var totalIncomeUsd = parseFloat(estIncConsigneeUSD) + parseFloat(estIncAgentUSD);
        var totalIncomeIdr = parseFloat(estIncConsigneeIDR) + parseFloat(estIncAgentIDR);
        var totalCostUsd = parseFloat(estCostAgentUSD) + parseFloat(estCostDepoUSD) + parseFloat(estCostEMKLUSD) + parseFloat(estCostRebateUSD) + parseFloat(estCostSSLineUSD);
        var totalCostIdr = parseFloat(estCostAgentIDR) + parseFloat(estCostDepoIDR) + parseFloat(estCostEMKLIDR) + parseFloat(estCostRebateIDR) + parseFloat(estCostSSLineIDR);
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

    // Inc Agent Calculation ------------------------------------------------------------------------------------------------
    $("#txtIAquantity, #txtIAperunitcost").blur(function () {
        var total = parseFloat($('#txtIAquantity').numberbox('getValue')) * parseFloat($('#txtIAperunitcost').numberbox('getValue'));
        total = Math.round(total * 100) / 100;
        $('#txtIAamount').numberbox('setValue', total);
    });
    //$("#txtIAamount").blur(function () {
    //    $('#txtIAquantity').numberbox('setValue', 0);
    //    $('#txtIAperunitcost').numberbox('setValue', 0);
    //});
    // END Inc Agent Calculation ------------------------------------------------------------------------------------------------

    // Inc Agent - Handling Fee Calculation ------------------------------------------------------------------------------------------------
    $("#txtIA_handlingfee20, #txtIA_handlingfee20usd, #txtIA_handlingfee40, #txtIA_handlingfee40usd, #txtIA_handlingfee45, #txtIA_handlingfee45usd, " +
        "#txtIA_handlingfeem3, #txtIA_handlingfeem3usd").blur(function () {
            IncAgentHFCalculation();
        });

    function IncAgentHFCalculation() {
        var hf20total = (Math.round(parseFloat($('#txtIA_handlingfee20').numberbox('getValue')) * parseFloat($('#txtIA_handlingfee20usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_handlingfee20total').numberbox('setValue', hf20total);

        var hf40total = (Math.round(parseFloat($('#txtIA_handlingfee40').numberbox('getValue')) * parseFloat($('#txtIA_handlingfee40usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_handlingfee40total').numberbox('setValue', hf40total);

        var hf45total = (Math.round(parseFloat($('#txtIA_handlingfee45').numberbox('getValue')) * parseFloat($('#txtIA_handlingfee45usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_handlingfee45total').numberbox('setValue', hf45total);

        var hfm3total = (Math.round(parseFloat($('#txtIA_handlingfeem3').numberbox('getValue')) * parseFloat($('#txtIA_handlingfeem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_handlingfeem3total').numberbox('setValue', hfm3total);
    }
    // END Inc Agent - Handling Fee Calculation ------------------------------------------------------------------------------------------------

    // Inc Agent - Profit Split Calculation ------------------------------------------------------------------------------------------------
    $('#txtIA_profitsplitpercent').blur(function () {
        var percentAmount = (parseFloat($('#txtIA_profitsplittotal').numberbox('getValue')) * parseFloat($('#txtIA_profitsplitpercent').numberbox('getValue'))) / 100;
        $('#txtIA_profitsplitpercentamount').numberbox('setValue', percentAmount);

        IncAgentProfitSplitCalculation();
    });
    function IncAgentProfitSplitCalculation() {
        var brTotal = parseFloat($('#txtIA_buyingrate20total').numberbox('getValue')) + parseFloat($('#txtIA_buyingrate40total').numberbox('getValue')) + parseFloat($('#txtIA_buyingrate45total').numberbox('getValue')) + parseFloat($('#txtIA_buyingratem3total').numberbox('getValue'));
        var srTotal = parseFloat($('#txtIA_sellingrate20total').numberbox('getValue')) + parseFloat($('#txtIA_sellingrate40total').numberbox('getValue')) + parseFloat($('#txtIA_sellingrate45total').numberbox('getValue')) + parseFloat($('#txtIA_sellingratem3total').numberbox('getValue'));
        var psTotal = parseFloat(srTotal) - parseFloat(brTotal);
        $('#txtIA_profitsplittotal').numberbox('setValue', psTotal);
        var percentAmount = (Math.round(parseFloat($('#txtIA_profitsplittotal').numberbox('getValue')) * parseFloat($('#txtIA_profitsplitpercent').numberbox('getValue')) * 100) / 100) / 100;
        $('#txtIA_profitsplitpercentamount').numberbox('setValue', percentAmount);
        var psTotalPayment = psTotal - percentAmount;
        $('#txtIA_profitsplittotalpayment').numberbox('setValue', psTotalPayment);

    }
    // Buying Rate Calculation
    $("#txtIA_buyingrate20, #txtIA_buyingrate20usd, #txtIA_buyingrate40, #txtIA_buyingrate40usd, #txtIA_buyingrate45, #txtIA_buyingrate45usd, " +
        "#txtIA_buyingratem3, #txtIA_buyingratem3usd").blur(function () {
            IncAgentPSBuyingRateCalculation();
            IncAgentProfitSplitCalculation();
        });
    function IncAgentPSBuyingRateCalculation() {
        var br20total = (Math.round(parseFloat($('#txtIA_buyingrate20').numberbox('getValue')) * parseFloat($('#txtIA_buyingrate20usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_buyingrate20total').numberbox('setValue', br20total);

        var br40total = (Math.round(parseFloat($('#txtIA_buyingrate40').numberbox('getValue')) * parseFloat($('#txtIA_buyingrate40usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_buyingrate40total').numberbox('setValue', br40total);

        var br45total = (Math.round(parseFloat($('#txtIA_buyingrate45').numberbox('getValue')) * parseFloat($('#txtIA_buyingrate45usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_buyingrate45total').numberbox('setValue', br45total);

        var brm3total = (Math.round(parseFloat($('#txtIA_buyingratem3').numberbox('getValue')) * parseFloat($('#txtIA_buyingratem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_buyingratem3total').numberbox('setValue', brm3total);
    }
    // Selling Rate Calculation
    $("#txtIA_sellingrate20, #txtIA_sellingrate20usd, #txtIA_sellingrate40, #txtIA_sellingrate40usd, #txtIA_sellingrate45, #txtIA_sellingrate45usd, " +
        "#txtIA_sellingratem3, #txtIA_sellingratem3usd").blur(function () {
            IncAgentPSSellingRateCalculation();
            IncAgentProfitSplitCalculation();
        });
    function IncAgentPSSellingRateCalculation() {
        var sr20total = (Math.round(parseFloat($('#txtIA_sellingrate20').numberbox('getValue')) * parseFloat($('#txtIA_sellingrate20usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_sellingrate20total').numberbox('setValue', sr20total);

        var sr40total = (Math.round(parseFloat($('#txtIA_sellingrate40').numberbox('getValue')) * parseFloat($('#txtIA_sellingrate40usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_sellingrate40total').numberbox('setValue', sr40total);

        var sr45total = (Math.round(parseFloat($('#txtIA_sellingrate45').numberbox('getValue')) * parseFloat($('#txtIA_sellingrate45usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_sellingrate45total').numberbox('setValue', sr45total);

        var srm3total = (Math.round(parseFloat($('#txtIA_sellingratem3').numberbox('getValue')) * parseFloat($('#txtIA_sellingratem3usd').numberbox('getValue')) * 100) / 100);
        $('#txtIA_sellingratem3total').numberbox('setValue', srm3total);
    }
    // END Inc Agent - Profit Split Calculation ------------------------------------------------------------------------------------------------

    // ------------------------------------------------------- LOOKUP ------------------
    // ----------------------- Shipment Order
    // Browse
    $('#btnSEshipmentno').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_shipmentorder');
        lookupGrid.setGridParam({ url: base_url + 'shipmentOrder/LookUpEPL', postData: { filters: null, job: JobId }, page: 'last', search: false }).trigger("reloadGrid");
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
            $('#txtSEjobowner').val(ret.jobowner);
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
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Shipment Order', 'Principle By', 'ETD', 'Consignee', 'Agent', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
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
                  { name: 'jobcode', index: 'jobcode', hidden: true },
                  { name: 'typeepl', index: 'typeepl', hidden: true },
                  { name: 'containersizelist', index: 'containersizelist', hidden: true },
                  { name: 'sslineid', index: 'sslineid', hidden: true },
                  { name: 'sslinecode', index: 'sslinecode', hidden: true },
                  { name: 'sslinename', index: 'sslinename', hidden: true },
                  { name: 'emklid', index: 'emklid', hidden: true },
                  { name: 'emklcode', index: 'emklcode', hidden: true },
                  { name: 'emklname', index: 'emklname', hidden: true },
                  { name: 'depoid', index: 'depoid', hidden: true },
                  { name: 'depocode', index: 'depocode', hidden: true },
                  { name: 'deponame', index: 'deponame', hidden: true },
                  { name: 'entrydate', index: 'entrydate', hidden: true }
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
    // End Shipment Order

    // --------------------------------------------------------------- Consignee, Rebate(consignee), Demurrage (Consignee)
    // Browse Inc Consignee
    $('#btnISconsignee').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vConsignee = "inc";
        var lookupGrid = $('#lookup_table_consignee');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsConsignee', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_consignee').dialog('open');
    });
    // Browse Cost Rebate(consignee)
    $('#btnISrebate').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vConsignee = "cost";
        var lookupGrid = $('#lookup_table_consignee');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsConsignee', postData: { filters: null }, search: false }).trigger("reloadGrid");
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
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', '', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'companystatus', index: 'a.companystatus', width: 200, hidden: true },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_consignee'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_consignee").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Consignee

    // --------------------------------------------------------------- SSLine
    // Browse
    $('#btnISssline').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_ssline');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsSSLine', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_ssline').dialog('open');
    });
    // Cancel or CLose
    $('#lookup_btn_cancel_ssline').click(function () {
        $('#lookup_div_ssline').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_ssline').click(function () {
        var id = jQuery("#lookup_table_ssline").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_ssline").jqGrid('getRowData', id);
            $('#txtISkdssline').val(ret.code).data("kode", id);
            $('#txtISnmssline').val(ret.name);

            $('#lookup_div_ssline').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_ssline").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_ssline'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_ssline").jqGrid('navGrid', '#toolbar_lookup_table_ssline', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End SSLine

    // --------------------------------------------------------------- EMKL
    // Browse
    $('#btnISemkl').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_emkl');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsEMKL', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_emkl').dialog('open');
    });
    // Cancel or CLose
    $('#lookup_btn_cancel_emkl').click(function () {
        $('#lookup_div_emkl').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_emkl').click(function () {
        var id = jQuery("#lookup_table_emkl").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_emkl").jqGrid('getRowData', id);
            $('#txtISkdemkl').val(ret.code).data("kode", id);
            $('#txtISnmemkl').val(ret.name);

            $('#lookup_div_emkl').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_emkl").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_emkl'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_emkl").jqGrid('navGrid', '#toolbar_lookup_table_emkl', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End EMKL

    // --------------------------------------------------------------- Depo
    // Browse
    $('#btnISdepo').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_depo');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsDepo', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_depo').dialog('open');
    });
    // Cancel or CLose
    $('#lookup_btn_cancel_depo').click(function () {
        $('#lookup_div_depo').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_depo').click(function () {
        var id = jQuery("#lookup_table_depo").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_depo").jqGrid('getRowData', id);
            $('#txtISkddepo').val(ret.code).data("kode", id);
            $('#txtISnmdepo').val(ret.name);

            $('#lookup_div_depo').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_depo").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_depo'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_depo").jqGrid('navGrid', '#toolbar_lookup_table_depo', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Depo

    // --------------------------------------------------------------- Agent - Add, Add HF, Add PS
    // Browse Add
    $('#btnIAagent').click(function () {
        vAgent = "add";
        var lookupGrid = $('#lookup_table_agent');
        lookupGrid.setGridParam({
            postData: { 'shipmentId': function () { return shipmentId; } },
            url: base_url + 'ShipmentOrder/LookUpAgent'
        }).trigger("reloadGrid");
        $('#lookup_div_agent').dialog('open');
    });
    // Browse Add HF
    $('#btnIAagent_handlingfee').click(function () {
        vAgent = "addhf";
        var lookupGrid = $('#lookup_table_agent');
        lookupGrid.setGridParam({
            postData: { 'shipmentId': function () { return shipmentId; } },
            url: base_url + 'ShipmentOrder/LookUpAgent'
        }).trigger("reloadGrid");
        $('#lookup_div_agent').dialog('open');
    });
    // Browse Add PS
    $('#btnIAagent_profitsplit').click(function () {
        vAgent = "addps";
        var lookupGrid = $('#lookup_table_agent');
        lookupGrid.setGridParam({
            postData: { 'shipmentId': function () { return shipmentId; } },
            url: base_url + 'ShipmentOrder/LookUpAgent'
        }).trigger("reloadGrid");
        $('#lookup_div_agent').dialog('open');
    });
    // Cancel or CLose
    $('#lookup_btn_cancel_agent').click(function () {
        $('#lookup_div_agent').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_agent').click(function () {
        var id = jQuery("#lookup_table_agent").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_agent").jqGrid('getRowData', id);
            switch (vAgent) {
                case 'add':
                    $('#txtIAkdagent').val(ret.code).data("kode", id);
                    $('#txtIAnmagent').val(ret.name);
                    break;
                case 'addhf':
                    $('#txtIAkdagent_handlingfee').val(ret.code).data("kode", id);
                    $('#txtIAnmagent_handlingfee').val(ret.name);
                    break;
                case 'addps':
                    $('#txtIAkdagent_profitsplit').val(ret.code).data("kode", id);
                    $('#txtIAnmagent_profitsplit').val(ret.name);
                    break;
                case 'demurrage':
                    $('#txtDemkdcustomer').val(ret.code).data("kode", id);
                    $('#txtDemnmcustomer').val(ret.name);
                    break;
            }

            $('#lookup_div_agent').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_agent").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name'],
        colModel: [{ name: 'code', index: 'agentcode', width: 80, align: "center" },
                  { name: 'name', index: 'agentname', width: 200 }
        ],
        pager: jQuery('#lookup_pager_agent'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_agent").jqGrid('navGrid', '#toolbar_lookup_table_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Agent - Add, Add HF, Add PS

    // Change Container Type/Size (NON AGENT)
    $('input[name=optIScontainersize]').click(function () {
        switch ($(this).attr('id')) {
            case 'optIScontainersize20':
                $('#txtISquantity').numberbox('setValue', vContainerCount20);
                break;
            case 'optIScontainersize40':
                $('#txtISquantity').numberbox('setValue', vContainerCount40);
                break;
            case 'optIScontainersize45':
                $('#txtISquantity').numberbox('setValue', vContainerCount45);
                break;
        }
        var total = parseFloat($('#txtISquantity').numberbox('getValue')) * parseFloat($('#txtISperunitcost').numberbox('getValue'));
        $('#txtISamount').numberbox('setValue', total);
    });
    // END Change Container Type/Size (NON AGENT)

    // Change Container Type/Size (AGENT)
    $('input[name=optIAcontainersize]').click(function () {
        switch ($(this).attr('id')) {
            case 'optIAcontainersize20':
                $('#txtIAquantity').numberbox('setValue', vContainerCount20);
                break;
            case 'optIAcontainersize40':
                $('#txtIAquantity').numberbox('setValue', vContainerCount40);
                break;
            case 'optIAcontainersize45':
                $('#txtIAquantity').numberbox('setValue', vContainerCount45);
                break;
        }
        var total = parseFloat($('#txtIAquantity').numberbox('getValue')) * parseFloat($('#txtIAperunitcost').numberbox('getValue'));
        $('#txtIAamount').numberbox('setValue', total);
    });
    // END Change Container Type/Size (AGENT)

    // ------------------------------------ Account - NON AGENT (Inc Consignee, Cost SSLine, Cost EMKL, Cost Rebate, Cost Depo), AGENT (Inc Agent, Cost Agent)
    // Browse Non Agent
    $('#btnISaccount').click(function () {

        // Get Number of Container
        GetContainerSizeList()

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vAccount = "nonagent";
        var lookupGrid = $('#lookup_table_account');
        lookupGrid.setGridParam({ url: base_url + 'CostSalesSea/LookUpCostSalesSea', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_account').dialog('open');
    });
    // Browse Inc Agent
    $('#btnIAaccount').click(function () {

        // Get Number of Container
        GetContainerSizeList()

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        vAccount = "incagent";
        var lookupGrid = $('#lookup_table_account');
        lookupGrid.setGridParam({ url: base_url + 'CostSalesSea/LookUpCostSalesSea', postData: { filters: null }, search: false }).trigger("reloadGrid");
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

                    // CodingQuantity
                    if (ret.quantity == "true") {

                        // Disable Size All
                        $('#optIScontainersizeall').attr('disabled', 'disabled');
                        // Container 20'
                        if (vContainerCount20 > 0) {
                            $('#optIScontainersize20').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtISquantity').attr('disabled', 'disabled');
                            //$('#txtISamount').attr('disabled', 'disabled');
                        }
                        // Container 40'
                        if (vContainerCount40 > 0) {
                            $('#optIScontainersize40').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtISquantity').attr('disabled', 'disabled');
                            //$('#txtISamount').attr('disabled', 'disabled');
                        }
                        // Container 45'
                        if (vContainerCount45 > 0) {
                            $('#optIScontainersize45').removeAttr('disabled');

                            //// Set Disabled
                            //$('#txtISquantity').attr('disabled', 'disabled');
                            //$('#txtISamount').attr('disabled', 'disabled');
                        }

                        $('#txtISquantity').numberbox('setValue', 0);

                        if ($('#optIScontainersizem3').is(":not(:disabled)"))
                            $('#optIScontainersizem3').attr("checked", "checked");
                        else if ($('#optIScontainersize20').is(":not(:disabled)")) {
                            $('#optIScontainersize20').attr("checked", "checked");
                            $('#txtISquantity').numberbox('setValue', vContainerCount20);
                        }
                        else if ($('#optIScontainersize40').is(":not(:disabled)")) {
                            $('#optIScontainersize40').attr("checked", "checked");
                            $('#txtISquantity').numberbox('setValue', vContainerCount40);
                        }
                        else if ($('#optIScontainersize45').is(":not(:disabled)")) {
                            $('#optIScontainersize45').attr("checked", "checked");
                            $('#txtISquantity').numberbox('setValue', vContainerCount45);
                        }
                        else
                            check = false;

                    }
                    else
                        $('#txtISquantity').numberbox('setValue', 0);

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
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', ''],
        colModel: [{ name: 'code', index: 'accountcode', width: 80, align: "center" },
                  { name: 'name', index: 'accountname', width: 200 },
                  { name: 'minusd', index: 'a.minusd', width: 200, hidden: true },
                  { name: 'minidr', index: 'b.minidr', width: 200, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 200, hidden: true }
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
        // default All
        var containerType = 5;
        if ($('#optIScontainersize20').is(':checked'))
            containerType = 1;
        if ($('#optIScontainersize40').is(':checked'))
            containerType = 2;
        if ($('#optIScontainersize45').is(':checked'))
            containerType = 3;
        if ($('#optIScontainersizem3').is(':checked'))
            containerType = 4;

        // Split Account
        var isSplitAccount = false;
        if ($('#cbSplitAccount').is(':checked'))
            isSplitAccount = true;

        if (isSplitAccount) {
            var quantity = $('#txtISquantity').numberbox('getValue');
            if (isNaN(quantity))
                quantity = 0;
            else
                quantity = parseFloat(quantity);

            if (quantity == 0) {
                $.messager.alert('Warning', 'Quantity harus lebih besar NOL jika mencentang SPLIT ACCOUNT', 'warning');
                return;
            }
        }

        // Add or Edit
        var submitURL = "";

        // Assigned EPL Detail Id
        var eplDetailId = 0
        if (vNonAgentEdit == 1) {
            eplDetailId = $('#lookup_btn_add_non_agent').data('epldetailid');
        }

        switch (vNonAgentType) {
            case 'consignee':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "epl/InsertIncConsignee" : base_url + "epl/UpdateIncConsignee" : "";
                var isValid = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'), containerType,
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, '', '', '', '', isSplitAccount, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {

                    // Set Default Customer from Last Input
                    $("#txtSEconsignee").data('kode', $('#txtISkdconsignee').data('kode')).data('kode2', $('#txtISkdconsignee').val());
                    $("#txtSEconsignee").data('name', $('#txtISnmconsignee').val());

                    addIncConsignee(vNonAgentEdit, eplDetailId, $('#txtISkdconsignee').data('kode'), $('#txtISkdconsignee').val(), $('#txtISnmconsignee').val(), $('#txtISkdaccount').data("kode"),
                        $('#txtISnmaccount').val(), $('#txtISdesc').val(), containerType, currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                        $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign, isSplitAccount);
                }
                clearNonAgent();
                break;
            case 'ssline':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "epl/InsertCostSSLine" : base_url + "epl/UpdateCostSSLine" : "";
                var isValid = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdssline').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'), containerType,
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, '', '', '', '', isSplitAccount, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {

                    // Set Default Customer from Last Input
                    $("#txtSEshipmentno").data('sslinekode', $('#txtISkdssline').data('kode')).data('sslinekode2', $('#txtISkdssline').val());
                    $("#txtSEshipmentno").data('sslinename', $('#txtISnmssline').val());

                    addCostSSLine(vNonAgentEdit, eplDetailId, $('#txtISkdssline').data('kode'), $('#txtISkdssline').val(), $('#txtISnmssline').val(), $('#txtISkdaccount').data("kode"),
                        $('#txtISnmaccount').val(), $('#txtISdesc').val(), containerType, currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                        $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign, isSplitAccount);
                }
                clearNonAgent();
                break;
            case 'emkl':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "epl/InsertCostEMKL" : base_url + "epl/UpdateCostEMKL" : "";
                var isValid = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdemkl').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'), containerType,
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, '', '', '', '', isSplitAccount, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {

                    // Set Default Customer from Last Input
                    $("#txtSEshipmentno").data('emklkode', $('#txtISkdemkl').data('kode')).data('emklkode2', $('#txtISkdemkl').val());
                    $("#txtSEshipmentno").data('emklname', $('#txtISnmemkl').val());

                    addCostEMKL(vNonAgentEdit, eplDetailId, $('#txtISkdemkl').data('kode'), $('#txtISkdemkl').val(), $('#txtISnmemkl').val(), $('#txtISkdaccount').data("kode"),
                        $('#txtISnmaccount').val(), $('#txtISdesc').val(), containerType, currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                        $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign, isSplitAccount);
                }
                clearNonAgent();
                break;
            case 'rebate':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "epl/InsertCostRebateConsignee" : base_url + "epl/UpdateCostRebateConsignee" : "";
                var isValid = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkdrebate').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'), containerType,
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, '', '', '', '', isSplitAccount, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {

                    // Set Default Customer from Last Input
                    $("#txtSEconsignee").data('kode', $('#txtISkdrebate').data('kode')).data('kode2', $('#txtISkdrebate').val());
                    $("#txtSEconsignee").data('name', $('#txtISnmrebate').val());

                    addCostRebate(vNonAgentEdit, eplDetailId, $('#txtISkdrebate').data('kode'), $('#txtISkdrebate').val(), $('#txtISnmrebate').val(), $('#txtISkdaccount').data("kode"),
                        $('#txtISnmaccount').val(), $('#txtISdesc').val(), containerType, currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                        $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign, isSplitAccount);
                }
                clearNonAgent();
                break;
            case 'depo':
                submitURL = (eplId != undefined && eplId != "") ? (vNonAgentEdit == 0) ? base_url + "epl/InsertCostDepo" : base_url + "epl/UpdateCostDepo" : "";
                var isValid = true;
                var message = "OK";
                SaveEPLDetail(submitURL, eplDetailId, $('#txtISkddepo').data('kode'), $('#txtISkdaccount').data("kode"), $('#txtISdesc').val(),
                                $('#txtISquantity').numberbox('getValue'), $('#txtISperunitcost').numberbox('getValue'), containerType,
                                $('#txtISkdaccount').data("codingquantity"), sign, currencyTpe, amountusd, amountidr, '', '', '', '', isSplitAccount, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {

                    // Set Default Customer from Last Input
                    $("#txtSEshipmentno").data('depokode', $('#txtISkddepo').data('kode')).data('depokode2', $('#txtISkddepo').val());
                    $("#txtSEshipmentno").data('deponame', $('#txtISnmdepo').val());

                    addCostDepo(vNonAgentEdit, eplDetailId, $('#txtISkddepo').data('kode'), $('#txtISkddepo').val(), $('#txtISnmdepo').val(), $('#txtISkdaccount').data("kode"),
                        $('#txtISnmaccount').val(), $('#txtISdesc').val(), containerType, currencyTpe, amountusd, amountidr, $('#txtISquantity').numberbox('getValue'),
                        $('#txtISperunitcost').numberbox('getValue'), $('#txtISkdaccount').data("codingquantity"), sign, isSplitAccount);
                }
                clearNonAgent();
                break;
        }

        // Reset CodingQuantity
        $('#txtISkdaccount').data("codingquantity", 0);

        // Calculate Total Estimated
        TotalEstimatedCalculation();
        $('#lookup_div_non_agent').dialog('close');
    });
    // END Non Agent Functionality

    // ---------------------------------------------------------------- Cost SSLine
    // ADD
    $('#btn_add_cost_ssline').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        vNonAgentEdit = 0;
        vNonAgentType = 'ssline';
        $('.non_agent_info').hide();
        $('.ssline_info').show();
        enableOnAddNonAgent();

        // Clear Input
        clearNonAgent();
        $('#txtISkdssline').data('kode', $("#txtSEshipmentno").data('sslinekode')).val($("#txtSEshipmentno").data('sslinekode2'));
        $('#txtISnmssline').val($("#txtSEshipmentno").data('sslinename'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Cost SSLine' });
        $('#lookup_div_non_agent').dialog("open");
    });
    // Delete
    $('#btn_delete_cost_ssline').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_costssline").jqGrid('getGridParam', 'selrow');
                if (id) {
                    var ret = jQuery("#tbl_costssline").jqGrid('getRowData', id);

                    // Validate Account EPL on Payment Request and Invoice
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        isValid = true;
                        message = "";
                        DeleteEPLDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {

                            // Removed EPL Detail Id from Button Save
                            $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                            jQuery("#tbl_costssline").jqGrid('delRowData', id);
                        }

                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_cost_ssline').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'ssline';
        $('.non_agent_info').hide();
        $('.ssline_info').show();
        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_costssline").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#tbl_costssline").jqGrid('getRowData', id);

            // Validate Account EPL on Payment Request and Invoice
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Assigned EPL Detail Id to Button Save
                $('#lookup_btn_add_non_agent').data('epldetailid', id);

                $('#txtISkdssline').data('kode', ret.sslineid).val(ret.sslinecode);
                $('#txtISnmssline').val(ret.sslinename);
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
                //ret.type = $('#txtISamount').val();
                switch (ret.type) {
                    case '1': $('#optIScontainersize20').attr('checked', 'checked'); break;
                    case '2': $('#optIScontainersize40').attr('checked', 'checked'); break;
                    case '3': $('#optIScontainersize45').attr('checked', 'checked'); break;
                    case '4': $('#optIScontainersizem3').attr('checked', 'checked'); break;
                    case '5': $('#optIScontainersizeall').attr('checked', 'checked'); break;
                }
                $('#txtISquantity').numberbox('setValue', ret.quantity);
                $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                if (ret.issplitinccost == 'true')
                    $('#cbSplitAccount').attr('checked', 'checked');

                vNonAgentEdit = 1;
                $('#lookup_div_non_agent').dialog({ title: 'Cost SSLine' });
                $('#lookup_div_non_agent').dialog('open');
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // Add or Edit
    function addCostSSLine(isEdit, eplDetailId, sslineId, sslineCode, sslineName, accountId, accountName, desc, containerType, currencyType, amountUsd, amountIdr,
                                        quantity, perquantity, codingquantity, sign, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (sslineCode == "" || parseInt(sslineCode) == 0) {
            $.messager.alert('Information', 'Invalid SSLine...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {
            var newData = {};
            newData.sslineid = sslineId;
            newData.sslinecode = sslineCode;
            newData.sslinename = (sslineName != undefined && sslineName != "") ? sslineName.toUpperCase() : "";
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
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costssline")) : eplDetailId;
                jQuery("#tbl_costssline").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_costssline").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_costssline").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#tbl_costssline").trigger("reloadGrid");
        }
    }
    // Declare Table jqgrid
    jQuery("#tbl_costssline").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['SSLineId', 'SSLineCode', 'SSLine', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '', ''],
        colModel: [
            { name: 'sslineid', index: 'sslineid', width: 60, hidden: true },
            { name: 'sslinecode', index: 'sslinecode', width: 60, hidden: true },
            { name: 'sslinename', index: 'sslinename', width: 250 },
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
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Cost SSLine

    // ------------------------------------------------------ Cost EMKL
    // ADD
    $('#btn_add_cost_emkl').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddNonAgent();
        vNonAgentEdit = 0;
        vNonAgentType = 'emkl';
        $('.non_agent_info').hide();
        $('.emkl_info').show();

        // Clear Input
        clearNonAgent();
        $('#txtISkdemkl').data('kode', $("#txtSEshipmentno").data('emklkode')).val($("#txtSEshipmentno").data('emklkode2'));
        $('#txtISnmemkl').val($("#txtSEshipmentno").data('emklname'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Cost EMKL' });
        $('#lookup_div_non_agent').dialog("open");
    });
    // Delete
    $('#btn_delete_cost_emkl').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_costemkl").jqGrid('getGridParam', 'selrow');
                if (id) {
                    // Validate Account EPL on Payment Request and Invoice
                    var ret = jQuery("#tbl_costemkl").jqGrid('getRowData', id);
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        isValid = true;
                        message = "";
                        DeleteEPLDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {

                            // Removed EPL Detail Id from Button Save
                            $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                            jQuery("#tbl_costemkl").jqGrid('delRowData', id);
                        }
                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_cost_emkl').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'emkl';
        $('.non_agent_info').hide();
        $('.emkl_info').show();
        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_costemkl").jqGrid('getGridParam', 'selrow');
        if (id) {
            // Validate Account EPL on Payment Request and Invoice
            var ret = jQuery("#tbl_costemkl").jqGrid('getRowData', id);
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Assigned EPL Detail Id to Button Save
                $('#lookup_btn_add_non_agent').data('epldetailid', id);

                $('#txtISkdemkl').data('kode', ret.emklid).val(ret.emklcode);
                $('#txtISnmemkl').val(ret.emklname);
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
                //ret.type = $('#txtISamount').val();
                switch (ret.type) {
                    case '1': $('#optIScontainersize20').attr('checked', 'checked'); break;
                    case '2': $('#optIScontainersize40').attr('checked', 'checked'); break;
                    case '3': $('#optIScontainersize45').attr('checked', 'checked'); break;
                    case '4': $('#optIScontainersizem3').attr('checked', 'checked'); break;
                    case '5': $('#optIScontainersizeall').attr('checked', 'checked'); break;
                }
                $('#txtISquantity').numberbox('setValue', ret.quantity);
                $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                vNonAgentEdit = 1;
                $('#lookup_div_non_agent').dialog({ title: 'Cost EMKL' });
                $('#lookup_div_non_agent').dialog('open');
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // Add or Edit
    function addCostEMKL(isEdit, eplDetailId, emklId, emklCode, emklName, accountId, accountName, desc, containerType, currencyType, amountUsd, amountIdr,
                                quantity, perquantity, codingquantity, sign, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (emklCode == "" || parseInt(emklCode) == 0) {
            $.messager.alert('Information', 'Invalid EMKL...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {
            var newData = {};
            newData.emklid = emklId;
            newData.emklcode = emklCode;
            newData.emklname = (emklName != undefined && emklName != "") ? emklName.toUpperCase() : "";
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
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costemkl")) : eplDetailId;
                jQuery("#tbl_costemkl").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_costemkl").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_costemkl").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#tbl_costemkl").trigger("reloadGrid");
        }
    }
    // Declare Table jqgrid
    jQuery("#tbl_costemkl").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['EMKLId', 'EMKLCode', 'EMKL', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '', ''],
        colModel: [
            { name: 'emklid', index: 'emklid', width: 60, hidden: true },
            { name: 'emklcode', index: 'emklcode', width: 60, hidden: true },
            { name: 'emklname', index: 'emklname', width: 250 },
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
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Cost EMKL

    // ------------------------------------------------------ Cost Rebate
    // ADD
    $('#btn_add_cost_rebate').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddNonAgent();
        vNonAgentEdit = 0;
        vNonAgentType = 'rebate';
        $('.non_agent_info').hide();
        $('.rebate_info').show();

        // Clear Input
        clearNonAgent();
        $('#txtISkdrebate').data('kode', $("#txtSEconsignee").data('kode')).val($("#txtSEconsignee").data('kode2'));
        $('#txtISnmrebate').val($("#txtSEconsignee").val());

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Cost Rebate' });
        $('#lookup_div_non_agent').dialog("open");
    });
    // Delete
    $('#btn_delete_cost_rebate').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_costrebate").jqGrid('getGridParam', 'selrow');
                if (id) {
                    // Validate Account EPL on Payment Request and Invoice
                    var ret = jQuery("#tbl_costrebate").jqGrid('getRowData', id);
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        isValid = true;
                        message = "";
                        DeleteEPLDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {

                            // Removed EPL Detail Id from Button Save
                            $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                            jQuery("#tbl_costrebate").jqGrid('delRowData', id);
                        }
                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_cost_rebate').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'rebate';
        $('.non_agent_info').hide();
        $('.rebate_info').show();
        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_costrebate").jqGrid('getGridParam', 'selrow');
        if (id) {

            // Validate Account EPL on Payment Request and Invoice
            var ret = jQuery("#tbl_costrebate").jqGrid('getRowData', id);
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Assigned EPL Detail Id to Button Save
                $('#lookup_btn_add_non_agent').data('epldetailid', id);

                $('#txtISkdrebate').data('kode', ret.rebateid).val(ret.rebatecode);
                $('#txtISnmrebate').val(ret.rebatename);
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
                //ret.type = $('#txtISamount').val();
                switch (ret.type) {
                    case '1': $('#optIScontainersize20').attr('checked', 'checked'); break;
                    case '2': $('#optIScontainersize40').attr('checked', 'checked'); break;
                    case '3': $('#optIScontainersize45').attr('checked', 'checked'); break;
                    case '4': $('#optIScontainersizem3').attr('checked', 'checked'); break;
                    case '5': $('#optIScontainersizeall').attr('checked', 'checked'); break;
                }
                $('#txtISquantity').numberbox('setValue', ret.quantity);
                $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                if (ret.issplitinccost == 'true')
                    $('#cbSplitAccount').attr('checked', 'checked');

                vNonAgentEdit = 1;
                $('#lookup_div_non_agent').dialog({ title: 'Cost Rebate' });
                $('#lookup_div_non_agent').dialog('open');
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // Add or Edit
    function addCostRebate(isEdit, eplDetailId, rebateId, rebateCode, rebateName, accountId, accountName, desc, containerType, currencyType, amountUsd, amountIdr,
                                quantity, perquantity, codingquantity, sign, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (rebateCode == "" || parseInt(rebateCode) == 0) {
            $.messager.alert('Information', 'Invalid Rebate...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {
            var newData = {};
            newData.rebateid = rebateId;
            newData.rebatecode = rebateCode;
            newData.rebatename = (rebateName != undefined && rebateName != "") ? rebateName.toUpperCase() : "";
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
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costrebate")) : eplDetailId;
                jQuery("#tbl_costrebate").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_costrebate").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_costrebate").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#tbl_costrebate").trigger("reloadGrid");
        }
    }
    // Declare Table jqgrid
    jQuery("#tbl_costrebate").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['RebateId', 'RebateCode', 'Consignee', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '', ''],
        colModel: [
            { name: 'rebateid', index: 'rebateid', width: 60, hidden: true },
            { name: 'rebatecode', index: 'rebatecode', width: 60, hidden: true },
            { name: 'rebatename', index: 'rebatename', width: 250 },
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
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Cost Rebate

    // ------------------------------------------------------ Cost Depo
    // ADD
    $('#btn_add_cost_depo').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddNonAgent();
        vNonAgentEdit = 0;
        vNonAgentType = 'depo';
        $('.non_agent_info').hide();
        $('.depo_info').show();

        // Clear Input
        clearNonAgent();
        $('#txtISkddepo').data('kode', $("#txtSEshipmentno").data('depokode')).val($("#txtSEshipmentno").data('depokode2'));
        $('#txtISnmdepo').val($("#txtSEshipmentno").data('deponame'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Cost Depo' });
        $('#lookup_div_non_agent').dialog("open");
    });
    // Delete
    $('#btn_delete_cost_depo').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_costdepo").jqGrid('getGridParam', 'selrow');
                if (id) {
                    // Validate Account EPL on Payment Request and Invoice
                    var ret = jQuery("#tbl_costdepo").jqGrid('getRowData', id);
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        isValid = true;
                        message = "";
                        DeleteEPLDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {

                            // Removed EPL Detail Id from Button Save
                            $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                            jQuery("#tbl_costdepo").jqGrid('delRowData', id);
                        }
                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_cost_depo').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'depo';
        $('.non_agent_info').hide();
        $('.depo_info').show();
        // Clear Input
        clearNonAgent();
        var id = jQuery("#tbl_costdepo").jqGrid('getGridParam', 'selrow');
        if (id) {

            // Validate Account EPL on Payment Request and Invoice
            var ret = jQuery("#tbl_costdepo").jqGrid('getRowData', id);
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Assigned EPL Detail Id to Button Save
                $('#lookup_btn_add_non_agent').data('epldetailid', id);

                $('#txtISkddepo').data('kode', ret.depoid).val(ret.depocode);
                $('#txtISnmdepo').val(ret.deponame);
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
                //ret.type = $('#txtISamount').val();
                switch (ret.type) {
                    case '1': $('#optIScontainersize20').attr('checked', 'checked'); break;
                    case '2': $('#optIScontainersize40').attr('checked', 'checked'); break;
                    case '3': $('#optIScontainersize45').attr('checked', 'checked'); break;
                    case '4': $('#optIScontainersizem3').attr('checked', 'checked'); break;
                    case '5': $('#optIScontainersizeall').attr('checked', 'checked'); break;
                }
                $('#txtISquantity').numberbox('setValue', ret.quantity);
                $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                if (ret.issplitinccost == 'true')
                    $('#cbSplitAccount').attr('checked', 'checked');

                vNonAgentEdit = 1;
                $('#lookup_div_non_agent').dialog({ title: 'Cost Depo' });
                $('#lookup_div_non_agent').dialog('open');
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    // Add or Edit
    function addCostDepo(isEdit, eplDetailId, depoId, depoCode, depoName, accountId, accountName, desc, containerType, currencyType, amountUsd, amountIdr,
                                quantity, perquantity, codingquantity, sign, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (depoCode == "" || parseInt(depoCode) == 0) {
            $.messager.alert('Information', 'Invalid Depo...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {
            var newData = {};
            newData.depoid = depoId;
            newData.depocode = depoCode;
            newData.deponame = (depoName != undefined && depoName != "") ? depoName.toUpperCase() : "";
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
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            newData.issplitinccost = isSplitIncCost;

            // New Record
            if (isEdit == 0) {
                eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costdepo")) : eplDetailId;
                jQuery("#tbl_costdepo").jqGrid('addRowData', eplDetailId, newData);
            }
            else {
                var id = jQuery("#tbl_costdepo").jqGrid('getGridParam', 'selrow');
                if (id) {
                    //  Edit
                    jQuery("#tbl_costdepo").jqGrid('setRowData', id, newData);
                }
            }
            // Reload
            $("#tbl_costdepo").trigger("reloadGrid");
        }
    }
    // Declare Table jqgrid
    jQuery("#tbl_costdepo").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['DepoId', 'DepoCode', 'Depo', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', '', '', '', '', '', '', '', ''],
        colModel: [
            { name: 'depoid', index: 'depoid', width: 60, hidden: true },
            { name: 'depocode', index: 'depocode', width: 60, hidden: true },
            { name: 'deponame', index: 'deponame', width: 250 },
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
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Cost Depo

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
        vNonAgentType = 'consignee';
        $('.non_agent_info').hide();
        $('.consignee_info').show();

        vNonAgentEdit = 0;
        // Clear Input
        clearNonAgent();
        $('#txtISkdconsignee').data('kode', $("#txtSEconsignee").data('kode')).val($("#txtSEconsignee").data('kode2'));
        $('#txtISnmconsignee').val($("#txtSEconsignee").data('name'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_non_agent').data('epldetailid', 0);

        $('#lookup_div_non_agent').dialog({ title: 'Inc Consignee' });
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
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        isValid = true;
                        message = "";
                        DeleteEPLDetail(id, function (data) {
                            isValid = data.isValid;
                            message = data.message
                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            // Removed EPL Detail Id from Button Save
                            $('#lookup_btn_add_non_agent').data('epldetailid', 0);

                            jQuery("#tbl_incconsignee").jqGrid('delRowData', id);
                        }
                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });
    // Edit
    $('#btn_edit_inc_consignee').click(function () {
        disableOnEditNonAgent();
        vNonAgentType = 'consignee';
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
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                vNonAgentEdit = 1;

                // Demurrage
                if (ret.accountid == '000') {
                    ClearDemurrage();
                    vDemurrageEdit = 1;
                    vDemurrageType = 'consignee';
                    $('#spanDemCustmer').html('Consignee');

                    // Assigned EPL Detail Id
                    $('#lookup_btn_add_demurrage').data('epldetailid', id);

                    $('#txtDemkdcustomer').data('kode', ret.consigneeid).val(ret.consigneecode);
                    $('#txtDemnmcustomer').val(ret.consigneename);
                    $('#txtDemeta').val($('#txtSEetd').val());

                    // Demurrage
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
                else {

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
                    //ret.type = $('#txtISamount').val();
                    switch (ret.type) {
                        case '1': $('#optIScontainersize20').attr('checked', 'checked'); break;
                        case '2': $('#optIScontainersize40').attr('checked', 'checked'); break;
                        case '3': $('#optIScontainersize45').attr('checked', 'checked'); break;
                        case '4': $('#optIScontainersizem3').attr('checked', 'checked'); break;
                        case '5': $('#optIScontainersizeall').attr('checked', 'checked'); break;
                    }
                    $('#txtISquantity').numberbox('setValue', ret.quantity);
                    $('#txtISperunitcost').numberbox('setValue', ret.perqty);
                    $('#txtISkdaccount').data("codingquantity", ret.codingquantity);

                    if (ret.issplitinccost == 'true')
                        $('#cbSplitAccount').attr('checked', 'checked');

                    $('#lookup_div_non_agent').dialog({ title: 'Inc Consignee' });
                    $('#lookup_div_non_agent').dialog('open');
                }
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Add or Edit
    function addIncConsignee(isEdit, eplDetailId, consigneeId, consigneeCode, consigneename, accountId, accountName, desc, containerType, currencyType, amountUsd, amountIdr,
                                        quantity, perquantity, codingquantity, sign, dmr, dmrContainerDetail, dmrContainer, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
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
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            // Demurrage
            newData.dmr = dmr;
            newData.dmrcontainerdetail = dmrContainerDetail;
            newData.dmrcontainer = dmrContainer;
            newData.issplitinccost = isSplitIncCost;

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


    // DisableOnEditNonAgent
    function disableOnEditAgent() {
        $('#btnIAagent').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#btnIAaccount').addClass('ui-state-disabled').removeClass('ui-state-default').attr('disabled', 'disabled');
        $('#optIAcurrencytypeIDR').attr('disabled', 'disabled');
        $('#optIAcurrencytypeUSD').attr('disabled', 'disabled');
        $('#optIAsignplus').attr('disabled', 'disabled');
        $('#optIAsignminus').attr('disabled', 'disabled');
    }
    // EnableOnAddNonAgent
    function enableOnAddAgent() {
        $('#btnIAagent').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#btnIAaccount').removeClass('ui-state-disabled').addClass('ui-state-default').removeAttr('disabled');
        $('#optIAcurrencytypeIDR').removeAttr('disabled');
        $('#optIAcurrencytypeUSD').removeAttr('disabled');
        $('#optIAsignplus').removeAttr('disabled');
        $('#optIAsignminus').removeAttr('disabled');
    }
    // Clear
    function clearIncCostAgent() {
        $('#txtIAkdagent').val('').data('kode', '');
        $('#txtIAnmagent').val('');
        $('#txtIAkdaccount').val('').data("kode", '');
        $('#txtIAnmaccount').val('');
        $('#txtIAdesc').val('');
        $('#txtIAquantity').numberbox('setValue', 0);
        $('#txtIAperunitcost').numberbox('setValue', 0);
        $('#txtIAamount').numberbox('setValue', 0);
        $('#optIAcontainersizeall').attr('checked', 'checked');
        $('#optIAcurrencytypeUSD').attr('checked', 'checked');

        // Profit Split
        $('#txtIAkdagent_profitsplit').val('').data('kode', '');
        $('#txtIAnmagent_profitsplit').val('');
        $('input[id*=txtIA_sellingrate]').numberbox('setValue', 0);
        $('input[id*=txtIA_buyingrate]').numberbox('setValue', 0);
        $('input[id*=txtIA_profitsplit]').numberbox('setValue', 0);

        // Handling Fee
        $('#txtIAkdagent_handlingfee').val('').data('kode', '');
        $('#txtIAnmagent_handlingfee').val('');
        $('input[id*=txtIA_handlingfee]').numberbox('setValue', 0);
    }

    // ------------------------------------------------------ Inc Agent
    // ADD HF
    $('#btn_add_inc_agent_handlingfee').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        vAgentType = 'inc';
        vAgentEdit = 0;

        // Reset EPL Detail Id
        $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', 0);

        // Clear Input
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_handlingfee').dialog({ title: 'Inc Agent - Handling Fee' });
        $('#lookup_div_inc_cost_agent_handlingfee').dialog('open');
    });

    // Save HF
    $('#lookup_btn_add_inc_cost_agent_handlingfee').click(function () {

        var sign = true;
        if ($('#optIAsignminus_handlingfee').is(':checked'))
            sign = false;

        var hfpsinfo = {};
        hfpsinfo.Feet20 = $("#txtIA_handlingfee20").numberbox('getValue');
        hfpsinfo.Rate20 = $("#txtIA_handlingfee20usd").numberbox('getValue');
        hfpsinfo.Feet40 = $("#txtIA_handlingfee40").numberbox('getValue');
        hfpsinfo.Rate40 = $("#txtIA_handlingfee40usd").numberbox('getValue');
        hfpsinfo.FeetHQ = $("#txtIA_handlingfee45").numberbox('getValue');
        hfpsinfo.RateHQ = $("#txtIA_handlingfee45usd").numberbox('getValue');
        hfpsinfo.FeetM3 = $("#txtIA_handlingfeem3").numberbox('getValue');
        hfpsinfo.RateM3 = $("#txtIA_handlingfeem3usd").numberbox('getValue');

        var hftotal = parseFloat($('#txtIA_handlingfee20total').numberbox('getValue')) + parseFloat($('#txtIA_handlingfee40total').numberbox('getValue')) + parseFloat($('#txtIA_handlingfee45total').numberbox('getValue')) + parseFloat($('#txtIA_handlingfeem3total').numberbox('getValue'));

        // Assigned EPL Detail Id
        var eplDetailId = 0
        if (vAgentEdit == 1) {
            eplDetailId = $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid');
        }

        //alert(eplDetailId); return;
        // Add or Edit
        if (vAgentType == 'cost') {
            submitURL = (eplId != undefined && eplId != "") ? (vAgentEdit == 0) ? base_url + "epl/InsertCostAgent" : base_url + "epl/UpdateCostAgent" : "";
            var isValid = true;
            var message = "OK";
            SaveEPLDetail(submitURL, eplDetailId, $('#txtIAkdagent_handlingfee').data('kode'), '005', 'Handling fee',
                            0, 0, 5, 0, sign, 0, hftotal, 0, JSON.stringify(hfpsinfo), '', '', '', false, function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    eplDetailId = data.objResult.EPLDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addCostAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent_handlingfee').data('kode'), $('#txtIAkdagent_handlingfee').val(),
                    $('#txtIAnmagent_handlingfee').val(), '005', 'Handling fee', 'Handling fee', 5,
                    hftotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));
            }
        }
        else {
            submitURL = (eplId != undefined && eplId != "") ? (vAgentEdit == 0) ? base_url + "epl/InsertIncAgent" : base_url + "epl/UpdateIncAgent" : "";
            var isValid = true;
            var message = "OK";
            SaveEPLDetail(submitURL, eplDetailId, $('#txtIAkdagent_handlingfee').data('kode'), '005', 'Handling fee',
                            0, 0, 5, 0, sign, 0, hftotal, 0, JSON.stringify(hfpsinfo), '', '', '', false, function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    eplDetailId = data.objResult.EPLDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addIncAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent_handlingfee').data('kode'), $('#txtIAkdagent_handlingfee').val(),
                    $('#txtIAnmagent_handlingfee').val(), '005', 'Handling fee', 'Handling fee', 5,
                    hftotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));
            }
        }

        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_handlingfee').dialog('close');
    });

    // Close HF
    $('#lookup_btn_cancel_inc_cost_agent_handlingfee').click(function () {
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_handlingfee').dialog('close');
    });

    // ADD PS
    $('#btn_add_inc_agent_profitsplit').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        vAgentType = 'inc';
        vAgentEdit = 0;

        // Reset EPL Detail Id
        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', 0);
        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', 0);

        // Clear Input
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_profitsplit').dialog({ title: 'Inc Agent - Profit Split' });
        $('#lookup_div_inc_cost_agent_profitsplit').dialog('open');
    });

    // Save PS
    $('#lookup_btn_add_inc_cost_agent_profitsplit').click(function () {

        var sign = true;
        if ($('#optIAsignminus_profitsplit').is(':checked'))
            sign = false;

        var hfpsinfo = {};
        hfpsinfo.SFeet20 = $("#txtIA_sellingrate20").numberbox('getValue');
        hfpsinfo.SRate20 = $("#txtIA_sellingrate20usd").numberbox('getValue');
        hfpsinfo.SFeet40 = $("#txtIA_sellingrate40").numberbox('getValue');
        hfpsinfo.SRate40 = $("#txtIA_sellingrate40usd").numberbox('getValue');
        hfpsinfo.SFeetHQ = $("#txtIA_sellingrate45").numberbox('getValue');
        hfpsinfo.SRateHQ = $("#txtIA_sellingrate45usd").numberbox('getValue');
        hfpsinfo.SFeetM3 = $("#txtIA_sellingratem3").numberbox('getValue');
        hfpsinfo.SRateM3 = $("#txtIA_sellingratem3usd").numberbox('getValue');

        hfpsinfo.BFeet20 = $("#txtIA_buyingrate20").numberbox('getValue');
        hfpsinfo.BRate20 = $("#txtIA_buyingrate20usd").numberbox('getValue');
        hfpsinfo.BFeet40 = $("#txtIA_buyingrate40").numberbox('getValue');
        hfpsinfo.BRate40 = $("#txtIA_buyingrate40usd").numberbox('getValue');
        hfpsinfo.BFeetHQ = $("#txtIA_buyingrate45").numberbox('getValue');
        hfpsinfo.BRateHQ = $("#txtIA_buyingrate45usd").numberbox('getValue');
        hfpsinfo.BFeetM3 = $("#txtIA_buyingratem3").numberbox('getValue');
        hfpsinfo.BRateM3 = $("#txtIA_buyingratem3usd").numberbox('getValue');
        hfpsinfo.Percentage = $("#txtIA_profitsplitpercent").numberbox('getValue');

        var brTotal = parseFloat($('#txtIA_buyingrate20total').numberbox('getValue')) + parseFloat($('#txtIA_buyingrate40total').numberbox('getValue')) + parseFloat($('#txtIA_buyingrate45total').numberbox('getValue')) + parseFloat($('#txtIA_buyingratem3total').numberbox('getValue'));
        var srTotal = parseFloat($('#txtIA_sellingrate20total').numberbox('getValue')) + parseFloat($('#txtIA_sellingrate40total').numberbox('getValue')) + parseFloat($('#txtIA_sellingrate45total').numberbox('getValue')) + parseFloat($('#txtIA_sellingratem3total').numberbox('getValue'));

        var psTotal = parseFloat($('#txtIA_profitsplittotalpayment').numberbox('getValue'));

        // Assigned EPL Detail Id
        var eplDetailId = 0
        if (vAgentEdit == 1) {
            eplDetailId = $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid');
        }

        // Add or Edit special for PROFIT SPLIT
        if (vAgentType == 'cost')
            addCostAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent_profitsplit').data('kode'), $('#txtIAkdagent_profitsplit').val(),
                $('#txtIAnmagent_profitsplit').val(), '-1', 'PROFIT SPLIT', 'PROFIT SPLIT', 5,
                srTotal + "^" + brTotal + "^" + psTotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));
        else
            addIncAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent_profitsplit').data('kode'), $('#txtIAkdagent_profitsplit').val(),
                $('#txtIAnmagent_profitsplit').val(), '-1', 'PROFIT SPLIT', 'PROFIT SPLIT', 5,
                srTotal + "^" + brTotal + "^" + psTotal, 0, 0, 0, sign, JSON.stringify(hfpsinfo));

        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_profitsplit').dialog('close');
    });

    // Close PS
    $('#lookup_btn_cancel_inc_cost_agent_profitsplit').click(function () {
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_profitsplit').dialog('close');
    });

    // ADD
    $('#btn_add_inc_agent').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddAgent();
        vAgentType = 'inc';
        vAgentEdit = 0;
        // Clear Input
        clearIncCostAgent();
        $('#txtIAkdagent').data('kode', $("#txtSEagent").data('kode')).val($("#txtSEagent").data('kode2'));
        $('#txtIAnmagent').val($("#txtSEagent").data('name'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_inc_cost_agent').data('epldetailid', 0);

        $('#lookup_div_inc_cost_agent').dialog({ title: 'Inc Agent' });
        $('#lookup_div_inc_cost_agent').dialog('open');
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
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        // Removed EPL Detail Id from Button Save
                        $('#lookup_btn_add_inc_cost_agent').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', 0);

                        // Check if Profit Split (Selling Rate, Buying Rate, Profit Share)
                        var data = jQuery("#tbl_incagent").jqGrid('getRowData', id);
                        if (data.accountid == 3 || data.accountid == 4 || data.accountid == 6) {
                            // Delete OLD Buying rate, Selling rate, Profit Share
                            var ids = $("#tbl_incagent").jqGrid('getDataIDs');
                            for (var i = 0; i < ids.length; i++) {
                                var datagrid = jQuery("#tbl_incagent").jqGrid('getRowData', ids[i]);
                                if (data.agentcode == datagrid.agentcode && data.counterprofitsplit == datagrid.counterprofitsplit) {
                                    // Selling Rate, Buying Rate, Profit Share
                                    if (parseInt(datagrid.accountid) == 3 || parseInt(datagrid.accountid) == 4 || parseInt(datagrid.accountid) == 6) {
                                        var idx = i + 1;

                                        isValid = true;
                                        message = "";
                                        DeleteEPLDetail(ids[i], function (data) {
                                            isValid = data.isValid;
                                            message = data.message
                                        });

                                        if (!isValid) {
                                            $.messager.alert('Warning', message, 'warning');
                                        }
                                        else {
                                            jQuery("#tbl_incagent").jqGrid('delRowData', ids[i]);
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            isValid = true;
                            message = "";
                            DeleteEPLDetail(id, function (data) {
                                isValid = data.isValid;
                                message = data.message
                            });

                            if (!isValid) {
                                $.messager.alert('Warning', message, 'warning');
                            }
                            else {
                                jQuery("#tbl_incagent").jqGrid('delRowData', id);
                            }
                        }

                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });

    // Edit
    $('#btn_edit_inc_agent').click(function () {
        // Clear Input
        clearIncCostAgent();
        var id = jQuery("#tbl_incagent").jqGrid('getGridParam', 'selrow');
        if (id) {

            disableOnEditAgent();
            vAgentType = 'inc';
            vAgentEdit = 1;

            var ret = jQuery("#tbl_incagent").jqGrid('getRowData', id);

            // Validate Account EPL on Payment Request and Invoice
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Assigned EPL Detail Id
                $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', id);
                // Assigned Counter Profit Split
                $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', ret.counterprofitsplit);

                // Parse from JSON contenttype
                var hfpsinfo = $.parseJSON(ret.hfpsinfo);
                // Profit Split - Buying Rate, Selling Rate, Profit Share
                if (parseInt(ret.accountid) == 3 || parseInt(ret.accountid) == 4 || parseInt(ret.accountid) == 6) {

                    // Search ProfitSplit info if not in their hfpsinfo
                    if (hfpsinfo == null) {
                        // Get all data
                        var data = $("#tbl_incagent").jqGrid('getGridParam', 'data');
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].agentcode == ret.agentcode && data[i].counterprofitsplit == ret.counterprofitsplit) {
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
                    $('#optIAsignplus_profitsplit').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus_profitsplit').attr('checked', 'checked');
                    }

                    $("#txtIA_sellingrate20").numberbox("setValue", hfpsinfo.SFeet20);
                    $("#txtIA_sellingrate20usd").numberbox("setValue", hfpsinfo.SRate20);
                    $("#txtIA_sellingrate40").numberbox("setValue", hfpsinfo.SFeet40);
                    $("#txtIA_sellingrate40usd").numberbox("setValue", hfpsinfo.SRate40);
                    $("#txtIA_sellingrate45").numberbox("setValue", hfpsinfo.SFeetHQ);
                    $("#txtIA_sellingrate45usd").numberbox("setValue", hfpsinfo.SRateHQ);
                    $("#txtIA_sellingratem3").numberbox("setValue", hfpsinfo.SFeetM3);
                    $("#txtIA_sellingratem3usd").numberbox("setValue", hfpsinfo.SRateM3);
                    IncAgentPSSellingRateCalculation();
                    $("#txtIA_buyingrate20").numberbox("setValue", hfpsinfo.BFeet20);
                    $("#txtIA_buyingrate20usd").numberbox("setValue", hfpsinfo.BRate20);
                    $("#txtIA_buyingrate40").numberbox("setValue", hfpsinfo.BFeet40);
                    $("#txtIA_buyingrate40usd").numberbox("setValue", hfpsinfo.BRate40);
                    $("#txtIA_buyingrate45").numberbox("setValue", hfpsinfo.BFeetHQ);
                    $("#txtIA_buyingrate45usd").numberbox("setValue", hfpsinfo.BRateHQ);
                    $("#txtIA_buyingratem3").numberbox("setValue", hfpsinfo.BFeetM3);
                    $("#txtIA_buyingratem3usd").numberbox("setValue", hfpsinfo.BRateM3);
                    IncAgentPSBuyingRateCalculation();

                    IncAgentProfitSplitCalculation();

                    $("#txtIA_profitsplitpercent").numberbox("setValue", hfpsinfo.Percentage);
                    var percentAmount = (parseFloat($('#txtIA_profitsplittotal').val()) * parseFloat(hfpsinfo.Percentage)) / 100;
                    $('#txtIA_profitsplitpercentamount').numberbox('setValue', percentAmount);

                    IncAgentProfitSplitCalculation();

                    $('#txtIAkdagent_profitsplit').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent_profitsplit').val(ret.agentname);

                    $('#lookup_div_inc_cost_agent_profitsplit').dialog({ title: 'Inc Agent - Profit Split' });
                    $('#lookup_div_inc_cost_agent_profitsplit').dialog('open');
                }
                    // END Profit Split - Buying Rate, Selling Rate, Profit Share
                    // Handling Fee
                else if (parseInt(ret.accountid) == 5) {

                    // Assigned EPL Detail Id
                    $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', id);

                    // Sign
                    $('#optIAsignplus_handlingfee').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus_handlingfee').attr('checked', 'checked');
                    }

                    $("#txtIA_handlingfee20").numberbox("setValue", hfpsinfo.Feet20);
                    $("#txtIA_handlingfee20usd").numberbox("setValue", hfpsinfo.Rate20);
                    $("#txtIA_handlingfee40").numberbox("setValue", hfpsinfo.Feet40);
                    $("#txtIA_handlingfee40usd").numberbox("setValue", hfpsinfo.Rate40);
                    $("#txtIA_handlingfee45").numberbox("setValue", hfpsinfo.FeetHQ);
                    $("#txtIA_handlingfee45usd").numberbox("setValue", hfpsinfo.RateHQ);
                    $("#txtIA_handlingfeem3").numberbox("setValue", hfpsinfo.FeetM3);
                    $("#txtIA_handlingfeem3usd").numberbox("setValue", hfpsinfo.RateM3);
                    IncAgentHFCalculation();

                    $('#txtIAkdagent_handlingfee').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent_handlingfee').val(ret.agentname);

                    $('#lookup_div_inc_cost_agent_handlingfee').dialog({ title: 'Inc Agent - Handling Fee' });
                    $('#lookup_div_inc_cost_agent_handlingfee').dialog('open');
                }
                    // END Handling Fee
                else {

                    // Assigned EPL Detail Id to Button Save
                    $('#lookup_btn_add_inc_cost_agent').data('epldetailid', id);

                    $('#txtIAkdagent').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent').val(ret.agentname);
                    $('#txtIAnmaccount').val(ret.accountname);
                    $('#txtIAdesc').val(ret.description);
                    $('#txtIAamount').numberbox('setValue', ret.amountusd);
                    $('#optIAsignplus').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus').attr('checked', 'checked');
                    }
                    $('#txtIAkdaccount').data("kode", ret.accountid).val(ret.accountid);
                    //ret.type = $('#txtISamount').val();
                    switch (ret.type) {
                        case '1': $('#optIAcontainersize20').attr('checked', 'checked'); break;
                        case '2': $('#optIAcontainersize40').attr('checked', 'checked'); break;
                        case '3': $('#optIAcontainersize45').attr('checked', 'checked'); break;
                        case '4': $('#optIAcontainersizem3').attr('checked', 'checked'); break;
                        case '5': $('#optIAcontainersizeall').attr('checked', 'checked'); break;
                    }
                    $('#txtIAquantity').numberbox('setValue', ret.quantity);
                    $('#txtIAperunitcost').numberbox('setValue', ret.perqty);
                    $('#txtIAkdaccount').data("codingquantity", ret.codingquantity);


                    $('#lookup_div_inc_cost_agent').dialog({ title: 'Inc Agent' });
                    $('#lookup_div_inc_cost_agent').dialog('open');
                }
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Save
    $('#lookup_btn_add_inc_cost_agent').click(function () {
        var amountusd = $('#txtIAamount').numberbox('getValue');
        // default All
        var containerType = 5;
        if ($('#optIAcontainersize20').is(':checked'))
            containerType = 1;
        if ($('#optIAcontainersize40').is(':checked'))
            containerType = 2;
        if ($('#optIAcontainersize45').is(':checked'))
            containerType = 3;
        if ($('#optIAcontainersizem3').is(':checked'))
            containerType = 4;

        var sign = true;
        if ($('#optIAsignminus').is(':checked'))
            sign = false;

        // Assigned EPL Detail Id
        var eplDetailId = 0
        if (vAgentEdit == 1) {
            eplDetailId = $('#lookup_btn_add_inc_cost_agent').data('epldetailid');
        }

        // Add or Edit
        if (vAgentType == 'cost') {
            submitURL = (eplId != undefined && eplId != "") ? (vAgentEdit == 0) ? base_url + "epl/InsertCostAgent" : base_url + "epl/UpdateCostAgent" : "";
            var isValid = true;
            var message = "OK";
            SaveEPLDetail(submitURL, eplDetailId, $('#txtIAkdagent').data('kode'), $('#txtIAkdaccount').data("kode"), $('#txtIAdesc').val(),
                            $('#txtIAquantity').numberbox('getValue'), $('#txtIAperunitcost').numberbox('getValue'), containerType,
                            $('#txtIAkdaccount').data("codingquantity"), sign, 0, amountusd, 0, '', '', '', '', false, function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    eplDetailId = data.objResult.EPLDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addCostAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent').data('kode'), $('#txtIAkdagent').val(),
                    $('#txtIAnmagent').val(), $('#txtIAkdaccount').data("kode"), $('#txtIAnmaccount').val(), $('#txtIAdesc').val(),
                    containerType, amountusd, $('#txtIAquantity').numberbox('getValue'), $('#txtIAperunitcost').numberbox('getValue'),
                    $('#txtIAkdaccount').data("codingquantity"), sign);
            }
        }
        else {
            submitURL = (eplId != undefined && eplId != "") ? (vAgentEdit == 0) ? base_url + "epl/InsertIncAgent" : base_url + "epl/UpdateIncAgent" : "";
            var isValid = true;
            var message = "OK";
            SaveEPLDetail(submitURL, eplDetailId, $('#txtIAkdagent').data('kode'), $('#txtIAkdaccount').data("kode"), $('#txtIAdesc').val(),
                            $('#txtIAquantity').numberbox('getValue'), $('#txtIAperunitcost').numberbox('getValue'), containerType,
                            $('#txtIAkdaccount').data("codingquantity"), sign, 0, amountusd, 0, '', '', '', '', false, function (data) {
                                isValid = data.isValid;
                                message = data.message;
                                if (data.objResult != null)
                                    eplDetailId = data.objResult.EPLDetailId;
                            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {
                addIncAgent(vAgentEdit, eplDetailId, $('#txtIAkdagent').data('kode'), $('#txtIAkdagent').val(),
                    $('#txtIAnmagent').val(), $('#txtIAkdaccount').data("kode"), $('#txtIAnmaccount').val(), $('#txtIAdesc').val(),
                    containerType, amountusd, $('#txtIAquantity').numberbox('getValue'), $('#txtIAperunitcost').numberbox('getValue'),
                    $('#txtIAkdaccount').data("codingquantity"), sign);
            }
        }

        clearIncCostAgent();

        $('#lookup_div_inc_cost_agent').dialog('close');
    });

    // Add or Edit
    function addIncAgent(isEdit, eplDetailId, agentId, agentCode, agentName, accountId, accountName, desc, containerType, amountUsd, quantity,
                            perquantity, codingquantity, sign, hfpsinfo, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (agentCode == "" || parseInt(agentCode) == 0) {
            $.messager.alert('Information', 'Invalid Agent...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {

            // Set Default Customer from Last Input
            $('#txtSEagent').data('kode', $("#txtIAkdagent").data('kode')).val($("#txtIAkdagent").data('kode2'));
            $('#txtSEagent').val($("#txtSEagent").data('name'));

            var newData = {};
            newData.agentid = agentId;
            newData.agentcode = agentCode;
            newData.agentname = (agentName != undefined && agentName != "") ? agentName.toUpperCase() : "";
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.description = (desc != undefined && desc != "") ? desc.toUpperCase() : "";
            newData.amountusd = amountUsd;
            newData.accountid = accountId;
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";

            newData.hfpsinfo = hfpsinfo;

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

                if (newData.description == "PROFIT SPLIT") {
                    // default
                    var eplDetailId = 0;
                    var isValid = true;
                    var message = "OK";

                    // Get Total - Selling Rate, Buying Rate, Profit Share
                    var tempValue = newData.amountusd.split('^');
                    var srTotal = tempValue[0];
                    var brTotal = tempValue[1];
                    var psTotal = tempValue[2];

                    // Add New Record - SELLING RATE
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;
                    counterSellRate = counterSellRate + 1;
                    newData.counterprofitsplit = counterSellRate;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertIncAgent", eplDetailId, agentId, '003', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, srTotal, 0, hfpsinfo, '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var srEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incagent")) : eplDetailId;
                        jQuery("#tbl_incagent").jqGrid('addRowData', srEplDetailId, newData);
                    }

                    // Add New Record - BUYING RATE
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";
                    counterBuyRate = counterBuyRate + 1;
                    newData.counterprofitsplit = counterBuyRate;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertIncAgent", eplDetailId, agentId, '004', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, brTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var brEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incagent")) : eplDetailId;
                        jQuery("#tbl_incagent").jqGrid('addRowData', brEplDetailId, newData);
                    }

                    // Add New Record - PROFIT SHARE
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";
                    counterProfitShare = counterProfitShare + 1;
                    newData.counterprofitsplit = counterProfitShare;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertIncAgent", eplDetailId, agentId, '006', 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, psTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var psEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incagent")) : eplDetailId;
                        jQuery("#tbl_incagent").jqGrid('addRowData', psEplDetailId, newData);
                    }
                }
                else {
                    eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_incagent")) : eplDetailId;
                    jQuery("#tbl_incagent").jqGrid('addRowData', eplDetailId, newData);
                }
            }
                // Update Mode
            else {
                var id = jQuery("#tbl_incagent").jqGrid('getGridParam', 'selrow');
                var accountidSelected = jQuery("#tbl_incagent").jqGrid('getCell', id, 'accountid');

                // ProfitSplit - selling rate, buying rate, profit share
                if (parseInt(accountidSelected) == 3 || parseInt(accountidSelected) == 4 || parseInt(accountidSelected) == 6) {
                    // Get Total - Selling Rate, Buying Rate, Profit Share
                    var tempValue = newData.amountusd.split('^');
                    var srTotal = tempValue[0];
                    var brTotal = tempValue[1];
                    var psTotal = tempValue[2];

                    // default
                    var isValid = true;
                    var message = "OK";
                    var eplDetailId = 0;
                    var sellRateEPLDetailID = 0;
                    var buyRateEPLDetailID = 0;
                    var profitShareEPLDetailID = 0;
                    // Delete OLD Buying rate, Selling rate, Profit Share
                    var ids = $("#tbl_incagent").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var data = jQuery("#tbl_incagent").jqGrid('getRowData', ids[i]);
                        if (data.agentcode == newData.agentcode && data.counterprofitsplit == $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit')) {
                            // Selling Rate, Buying Rate, Profit Share
                            if (parseInt(data.accountid) == 3 || parseInt(data.accountid) == 4 || parseInt(data.accountid) == 6) {
                                var idx = i + 1;
                                if (parseInt(data.accountid) == 3) {
                                    sellRateEPLDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 4) {
                                    buyRateEPLDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 6) {
                                    profitShareEPLDetailID = ids[i];
                                }
                                //jQuery("#tbl_incagent").jqGrid('delRowData', ids[i]);

                            }
                        }
                    }
                    $("#tbl_incagent").jqGrid().trigger("reloadGrid");

                    // Add New Record - SELLING RATE
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateIncAgent", sellRateEPLDetailID, agentId, '003', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, srTotal, 0, hfpsinfo, '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_incagent").jqGrid('setRowData', sellRateEPLDetailID, newData);

                    // Add New Record - BUYING RATE
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateIncAgent", buyRateEPLDetailID, agentId, '004', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, brTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_incagent").jqGrid('setRowData', buyRateEPLDetailID, newData);

                    // Add New Record - PROFIT SHARE
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateIncAgent", profitShareEPLDetailID, agentId, '006', 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, psTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_incagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_incagent").jqGrid('setRowData', profitShareEPLDetailID, newData);
                }
                    // END ProfitSplit
                else if (id) {
                    //  Edit
                    jQuery("#tbl_incagent").jqGrid('setRowData', id, newData);
                }
            }
            // Calculate Total Estimated
            TotalEstimatedCalculation();

            // Reload
            $("#tbl_incagent").trigger("reloadGrid");
        }
    }

    // Close
    $('#lookup_btn_cancel_inc_cost_agent').click(function () {
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent').dialog('close');
    });

    // Declare Table jqgrid
    jQuery("#tbl_incagent").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['AgentId', 'AgentCode', 'Contact', 'AccountName', 'Description', 'Amount USD', 'Amount IDR', 'accountid', 'type', 'quantity', 'perqty', 'codingquantity', '',
                        'hfpsinfo', 'counterprofitsplit', ''],
        colModel: [
            { name: 'agentid', index: 'agentid', width: 60, hidden: true },
            { name: 'agentcode', index: 'agentcode', width: 60, hidden: true },
            { name: 'agentname', index: 'agentname', width: 250 },
            { name: 'accountname', index: 'accountname', width: 350, hidden: true },
            { name: 'description', index: 'description', width: 400 },
            { name: 'amountusd', index: 'amountusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'amountidr', index: 'amountidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'accountid', index: 'accountid', width: 60, hidden: true },
            { name: 'type', index: 'type', width: 60, hidden: true },
            { name: 'quantity', index: 'quantity', width: 60, hidden: true },
            { name: 'perqty', index: 'perqty', width: 60, hidden: true },
            { name: 'codingquantity', index: 'codingquantity', width: 60, hidden: true },
            { name: 'sign', index: 'sign', width: 30, align: "center" },
            { name: 'hfpsinfo', index: 'hfpsinfo', hidden: true },
            { name: 'counterprofitsplit', index: 'counterprofitsplit', width: 60, hidden: true },
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Inc Agent


    // ------------------------------------------------------ Cost Agent
    // ADD HF
    $('#btn_add_cost_agent_handlingfee').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        vAgentType = 'cost';
        vAgentEdit = 0;

        // Reset EPL Detail Id
        $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', 0);

        // Clear Input
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_handlingfee').dialog({ title: 'Cost Agent - Handling Fee' });
        $('#lookup_div_inc_cost_agent_handlingfee').dialog('open');
    });

    // ADD PS
    $('#btn_add_cost_agent_profitsplit').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        vAgentType = 'cost';
        vAgentEdit = 0;

        // Reset EPL Detail Id
        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', 0);
        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', 0);

        // Clear Input
        clearIncCostAgent();
        $('#lookup_div_inc_cost_agent_profitsplit').dialog({ title: 'Cost Agent - Profit Split' });
        $('#lookup_div_inc_cost_agent_profitsplit').dialog('open');
    });

    // ADD
    $('#btn_add_cost_agent').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        enableOnAddAgent();
        vAgentType = 'cost';
        vAgentEdit = 0;
        // Clear Input
        clearIncCostAgent();
        $('#txtIAkdagent').data('kode', $("#txtSEagent").data('kode')).val($("#txtSEagent").data('kode2'));
        $('#txtIAnmagent').val($("#txtSEagent").data('name'));

        // Removed EPL Detail Id from Button Save
        $('#lookup_btn_add_inc_cost_agent').data('epldetailid', 0);

        $('#lookup_div_inc_cost_agent').dialog({ title: 'Cost Agent' });
        $('#lookup_div_inc_cost_agent').dialog('open');
    });

    // Delete
    $('#btn_delete_cost_agent').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                var id = jQuery("#tbl_costagent").jqGrid('getGridParam', 'selrow');
                if (id) {
                    var ret = jQuery("#tbl_costagent").jqGrid('getRowData', id);

                    // Validate Account EPL on Payment Request and Invoice
                    var isValid = false;
                    var message = "";
                    ValidateAccountEPL(eplId, id, function (data) {
                        isValid = data.isValid;
                        message = data.message
                    });

                    if (!isValid) {
                        $.messager.alert('Warning', message, 'warning');
                    }
                    else {

                        // Removed EPL Detail Id from Button Save
                        $('#lookup_btn_add_inc_cost_agent').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', 0);
                        $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', 0);

                        // Check if Profit Split (Selling Rate, Buying Rate, Profit Share)
                        var data = jQuery("#tbl_costagent").jqGrid('getRowData', id);
                        if (data.accountid == 3 || data.accountid == 4 || data.accountid == 6) {
                            // Delete OLD Buying rate, Selling rate, Profit Share
                            var ids = $("#tbl_costagent").jqGrid('getDataIDs');
                            for (var i = 0; i < ids.length; i++) {
                                var datagrid = jQuery("#tbl_costagent").jqGrid('getRowData', ids[i]);
                                if (data.agentcode == datagrid.agentcode && data.counterprofitsplit == datagrid.counterprofitsplit) {
                                    // Selling Rate, Buying Rate, Profit Share
                                    if (parseInt(datagrid.accountid) == 3 || parseInt(datagrid.accountid) == 4 || parseInt(datagrid.accountid) == 6) {
                                        var idx = i + 1;

                                        isValid = true;
                                        message = "";
                                        DeleteEPLDetail(ids[i], function (data) {
                                            isValid = data.isValid;
                                            message = data.message
                                        });

                                        if (!isValid) {
                                            $.messager.alert('Warning', message, 'warning');
                                        }
                                        else {
                                            jQuery("#tbl_costagent").jqGrid('delRowData', ids[i]);
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            isValid = true;
                            message = "";
                            DeleteEPLDetail(id, function (data) {
                                isValid = data.isValid;
                                message = data.message
                            });

                            if (!isValid) {
                                $.messager.alert('Warning', message, 'warning');
                            }
                            else {
                                jQuery("#tbl_costagent").jqGrid('delRowData', id);
                            }
                        }
                        // Calculate Total Estimated
                        TotalEstimatedCalculation();
                    }
                } else {
                    $.messager.alert('Information', 'Please Select Data...!!', 'info');
                }
            }
        });
    });

    // Edit
    $('#btn_edit_cost_agent').click(function () {
        // Clear Input
        clearIncCostAgent();
        var id = jQuery("#tbl_costagent").jqGrid('getGridParam', 'selrow');
        if (id) {

            disableOnEditAgent();
            vAgentType = 'cost';
            vAgentEdit = 1;

            var ret = jQuery("#tbl_costagent").jqGrid('getRowData', id);

            // Validate Account EPL on Payment Request and Invoice
            var isValid = false;
            var message = "";
            ValidateAccountEPL(eplId, id, function (data) {
                isValid = data.isValid;
                message = data.message
            });

            if (!isValid) {
                $.messager.alert('Warning', message, 'warning');
            }
            else {

                // Demurrage
                if (ret.accountid == '000') {
                    ClearDemurrage();
                    vDemurrageEdit = 1;
                    vDemurrageType = 'agent';
                    $('#spanDemCustmer').html('Agent');

                    // Assigned EPL Detail Id
                    $('#lookup_btn_add_demurrage').data('epldetailid', id);

                    $('#txtDemkdcustomer').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtDemnmcustomer').val(ret.agentname);
                    $('#txtDemeta').val($('#txtSEetd').val());

                    // Demurrage
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
                }   //END Demurrage
                    // Profit Split - Buying Rate, Selling Rate, Profit Share
                else if (parseInt(ret.accountid) == 3 || parseInt(ret.accountid) == 4 || parseInt(ret.accountid) == 6) {

                    // Assigned EPL Detail Id
                    $('#lookup_btn_add_inc_cost_agent_profitsplit').data('epldetailid', id);
                    // Assigned Counter Profit Split
                    $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit', ret.counterprofitsplit);

                    // Parse from JSON contenttype
                    var hfpsinfo = $.parseJSON(ret.hfpsinfo);

                    // Search ProfitSplit info if not in their hfpsinfo
                    if (hfpsinfo == null) {
                        // Get all data
                        var data = $("#tbl_costagent").jqGrid('getGridParam', 'data');
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].agentcode == ret.agentcode && data[i].counterprofitsplit == ret.counterprofitsplit) {
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
                    $('#optIAsignplus_profitsplit').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus_profitsplit').attr('checked', 'checked');
                    }

                    $("#txtIA_sellingrate20").numberbox("setValue", hfpsinfo.SFeet20);
                    $("#txtIA_sellingrate20usd").numberbox("setValue", hfpsinfo.SRate20);
                    $("#txtIA_sellingrate40").numberbox("setValue", hfpsinfo.SFeet40);
                    $("#txtIA_sellingrate40usd").numberbox("setValue", hfpsinfo.SRate40);
                    $("#txtIA_sellingrate45").numberbox("setValue", hfpsinfo.SFeetHQ);
                    $("#txtIA_sellingrate45usd").numberbox("setValue", hfpsinfo.SRateHQ);
                    $("#txtIA_sellingratem3").numberbox("setValue", hfpsinfo.SFeetM3);
                    $("#txtIA_sellingratem3usd").numberbox("setValue", hfpsinfo.SRateM3);
                    IncAgentPSSellingRateCalculation();
                    $("#txtIA_buyingrate20").numberbox("setValue", hfpsinfo.BFeet20);
                    $("#txtIA_buyingrate20usd").numberbox("setValue", hfpsinfo.BRate20);
                    $("#txtIA_buyingrate40").numberbox("setValue", hfpsinfo.BFeet40);
                    $("#txtIA_buyingrate40usd").numberbox("setValue", hfpsinfo.BRate40);
                    $("#txtIA_buyingrate45").numberbox("setValue", hfpsinfo.BFeetHQ);
                    $("#txtIA_buyingrate45usd").numberbox("setValue", hfpsinfo.BRateHQ);
                    $("#txtIA_buyingratem3").numberbox("setValue", hfpsinfo.BFeetM3);
                    $("#txtIA_buyingratem3usd").numberbox("setValue", hfpsinfo.BRateM3);
                    IncAgentPSBuyingRateCalculation();

                    IncAgentProfitSplitCalculation();

                    $("#txtIA_profitsplitpercent").numberbox("setValue", hfpsinfo.Percentage);
                    var percentAmount = (parseFloat($('#txtIA_profitsplittotal').val()) * parseFloat(hfpsinfo.Percentage)) / 100;
                    $('#txtIA_profitsplitpercentamount').numberbox('setValue', percentAmount);

                    IncAgentProfitSplitCalculation();

                    $('#txtIAkdagent_profitsplit').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent_profitsplit').val(ret.agentname);

                    $('#lookup_div_inc_cost_agent_profitsplit').dialog({ title: 'Cost Agent - Profit Split' });
                    $('#lookup_div_inc_cost_agent_profitsplit').dialog('open');
                }
                    // END Profit Split - Buying Rate, Selling Rate, Profit Share
                    // Handling Fee
                else if (parseInt(ret.accountid) == 5) {

                    // Assigned EPL Detail Id
                    $('#lookup_btn_add_inc_cost_agent_handlingfee').data('epldetailid', id);

                    // Parse from JSON contenttype
                    var hfpsinfo = $.parseJSON(ret.hfpsinfo);
                    // Sign
                    $('#optIAsignplus_handlingfee').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus_handlingfee').attr('checked', 'checked');
                    }

                    $("#txtIA_handlingfee20").numberbox("setValue", hfpsinfo.Feet20);
                    $("#txtIA_handlingfee20usd").numberbox("setValue", hfpsinfo.Rate20);
                    $("#txtIA_handlingfee40").numberbox("setValue", hfpsinfo.Feet40);
                    $("#txtIA_handlingfee40usd").numberbox("setValue", hfpsinfo.Rate40);
                    $("#txtIA_handlingfee45").numberbox("setValue", hfpsinfo.FeetHQ);
                    $("#txtIA_handlingfee45usd").numberbox("setValue", hfpsinfo.RateHQ);
                    $("#txtIA_handlingfeem3").numberbox("setValue", hfpsinfo.FeetM3);
                    $("#txtIA_handlingfeem3usd").numberbox("setValue", hfpsinfo.RateM3);
                    IncAgentHFCalculation();

                    $('#txtIAkdagent_handlingfee').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent_handlingfee').val(ret.agentname);

                    $('#lookup_div_inc_cost_agent_handlingfee').dialog({ title: 'Cost Agent - Handling Fee' });
                    $('#lookup_div_inc_cost_agent_handlingfee').dialog('open');
                }
                    // END Handling Fee
                else {

                    // Assigned EPL Detail Id to Button Save
                    $('#lookup_btn_add_inc_cost_agent').data('epldetailid', id);

                    $('#txtIAkdagent').data('kode', ret.agentid).val(ret.agentcode);
                    $('#txtIAnmagent').val(ret.agentname);
                    $('#txtIAnmaccount').val(ret.accountname);
                    $('#txtIAdesc').val(ret.description);
                    $('#txtIAamount').numberbox('setValue', ret.amountusd);
                    $('#optIAsignplus').attr('checked', 'checked');
                    if (ret.sign == '-')   // minus
                    {
                        $('#optIAsignminus').attr('checked', 'checked');
                    }
                    $('#txtIAkdaccount').data("kode", ret.accountid).val(ret.accountid);
                    //ret.type = $('#txtISamount').val();
                    switch (ret.type) {
                        case '1': $('#optIAcontainersize20').attr('checked', 'checked'); break;
                        case '2': $('#optIAcontainersize40').attr('checked', 'checked'); break;
                        case '3': $('#optIAcontainersize45').attr('checked', 'checked'); break;
                        case '4': $('#optIAcontainersizem3').attr('checked', 'checked'); break;
                        case '5': $('#optIAcontainersizeall').attr('checked', 'checked'); break;
                    }
                    $('#txtIAquantity').numberbox('setValue', ret.quantity);
                    $('#txtIAperunitcost').numberbox('setValue', ret.perqty);
                    $('#txtIAkdaccount').data("codingquantity", ret.codingquantity);

                    $('#lookup_div_inc_cost_agent').dialog({ title: 'Cost Agent' });
                    $('#lookup_div_inc_cost_agent').dialog('open');
                }
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Add or Edit
    function addCostAgent(isEdit, eplDetailId, agentId, agentCode, agentName, accountId, accountName, desc, containerType, amountUsd, quantity, perquantity, codingquantity, sign, hfpsinfo,
                    dmr, dmrContainerDetail, dmrContainer, isSplitIncCost) {
        // Validate
        if (accountId.trim() == '') {
            $.messager.alert('Information', 'Invalid AccountId...!!', 'info');
            return
        }
        if (agentCode == "" || parseInt(agentCode) == 0) {
            $.messager.alert('Information', 'Invalid Agent...!!', 'info');
            return
        }

        var isValid = true;
        if (!isValid) {
            $.messager.alert('Information', 'Data has been exist...!!', 'info');
        }
            // End Validate
        else {

            // Set Default Customer from Last Input
            $('#txtSEagent').data('kode', $("#txtIAkdagent").data('kode')).val($("#txtIAkdagent").data('kode2'));
            $('#txtSEagent').val($("#txtSEagent").data('name'));

            var newData = {};
            newData.agentid = agentId;
            newData.agentcode = agentCode;
            newData.agentname = (agentName != undefined && agentName != "") ? agentName.toUpperCase() : "";
            newData.accountname = (accountName != undefined && accountName != "") ? accountName.toUpperCase() : "";
            newData.description = (desc != undefined && desc != "") ? desc.toUpperCase() : "";
            newData.amountusd = amountUsd;
            newData.accountid = accountId;
            newData.type = containerType;
            newData.quantity = quantity;
            newData.perqty = perquantity;
            newData.codingquantity = codingquantity;
            newData.sign = "-";
            if (sign)
                newData.sign = "+";
            // Demurrage
            newData.dmr = dmr;
            newData.dmrcontainerdetail = dmrContainerDetail;
            newData.dmrcontainer = dmrContainer;

            newData.issplitinccost = isSplitIncCost;
            newData.hfpsinfo = hfpsinfo;

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

                if (newData.description == "PROFIT SPLIT") {
                    // default
                    var isValid = true;
                    var message = "OK";
                    var eplDetailId = 0;

                    // Get Total - Selling Rate, Buying Rate, Profit Share
                    var tempValue = newData.amountusd.split('^');
                    var srTotal = tempValue[0];
                    var brTotal = tempValue[1];
                    var psTotal = tempValue[2];

                    // Add New Record - SELLING RATE
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;
                    counterSellRate = counterSellRate + 1;
                    newData.counterprofitsplit = counterSellRate;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertCostAgent", eplDetailId, agentId, '003', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, srTotal, 0, hfpsinfo, '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var srEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costagent")) : eplDetailId;
                        jQuery("#tbl_costagent").jqGrid('addRowData', srEplDetailId, newData);
                    }

                    // Add New Record - BUYING RATE
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";
                    counterBuyRate = counterBuyRate + 1;
                    newData.counterprofitsplit = counterBuyRate;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertCostAgent", eplDetailId, agentId, '004', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, brTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var brEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costagent")) : eplDetailId;
                        jQuery("#tbl_costagent").jqGrid('addRowData', brEplDetailId, newData);
                    }

                    // Add New Record - PROFIT SHARE
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";
                    counterProfitShare = counterProfitShare + 1;
                    newData.counterprofitsplit = counterProfitShare;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/InsertCostAgent", eplDetailId, agentId, '006', 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, psTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('addRowData', eplDetailId, newData);
                        }
                    }
                    else {
                        var psEplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costagent")) : eplDetailId;
                        jQuery("#tbl_costagent").jqGrid('addRowData', psEplDetailId, newData);
                    }

                }
                else {
                    eplDetailId = eplDetailId == 0 ? GetNextId($("#tbl_costagent")) : eplDetailId;
                    jQuery("#tbl_costagent").jqGrid('addRowData', eplDetailId, newData);
                }
            }
                // Update Mode
            else {
                var id = jQuery("#tbl_costagent").jqGrid('getGridParam', 'selrow');
                var accountidSelected = jQuery("#tbl_costagent").jqGrid('getCell', id, 'accountid');

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
                    var sellRateEPLDetailID = 0;
                    var buyRateEPLDetailID = 0;
                    var profitShareEPLDetailID = 0;
                    var ids = $("#tbl_costagent").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var data = jQuery("#tbl_costagent").jqGrid('getRowData', ids[i]);
                        if (data.agentcode == newData.agentcode && data.counterprofitsplit == $('#lookup_btn_add_inc_cost_agent_profitsplit').data('counterprofitsplit')) {
                            // Selling Rate, Buying Rate, Profit Share
                            if (parseInt(data.accountid) == 3 || parseInt(data.accountid) == 4 || parseInt(data.accountid) == 6) {
                                var idx = i + 1;
                                if (parseInt(data.accountid) == 3) {
                                    sellRateEPLDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 4) {
                                    buyRateEPLDetailID = ids[i];
                                }
                                if (parseInt(data.accountid) == 6) {
                                    profitShareEPLDetailID = ids[i];
                                }
                                //jQuery("#tbl_costagent").jqGrid('delRowData', ids[i]);

                            }
                        }
                    }
                    $("#tbl_costagent").jqGrid().trigger("reloadGrid");

                    // Add New Record - SELLING RATE
                    newData.accountid = '003';
                    newData.description = 'SELLING RATE'
                    newData.amountusd = srTotal;

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateCostAgent", sellRateEPLDetailID, agentId, '003', 'SELLING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, srTotal, 0, hfpsinfo, '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_costagent").jqGrid('setRowData', sellRateEPLDetailID, newData);

                    // Add New Record - BUYING RATE
                    newData.accountid = '004';
                    newData.description = 'BUYING RATE'
                    newData.amountusd = brTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateCostAgent", buyRateEPLDetailID, agentId, '004', 'BUYING RATE',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, brTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_costagent").jqGrid('setRowData', buyRateEPLDetailID, newData);

                    // Add New Record - PROFIT SHARE
                    newData.accountid = '006';
                    newData.description = 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %';
                    newData.amountusd = psTotal;
                    newData.hfpsinfo = "";

                    // ONLY on UPDATE EPL
                    if ((eplId != undefined && eplId != "")) {
                        // Submit To Server
                        SaveEPLDetail(base_url + "epl/UpdateCostAgent", profitShareEPLDetailID, agentId, '006', 'PROFIT SHARE ' + $('#txtIA_profitsplitpercent').val() + ' %',
                                        quantity, perquantity, containerType, codingquantity, sign, 0, psTotal, 0, '', '', '', '', false, function (data) {
                                            isValid = data.isValid;
                                            message = data.message;
                                            if (data.objResult != null)
                                                eplDetailId = data.objResult.EPLDetailId;
                                        });

                        if (!isValid) {
                            $.messager.alert('Warning', message, 'warning');
                        }
                        else {
                            jQuery("#tbl_costagent").jqGrid('setRowData', eplDetailId, newData);
                        }
                    }
                    else
                        jQuery("#tbl_costagent").jqGrid('setRowData', profitShareEPLDetailID, newData);
                }
                    // END ProfitSplit
                else if (id) {
                    //  Edit
                    jQuery("#tbl_costagent").jqGrid('setRowData', id, newData);
                }
            }
            // Calculate Total Estimated
            TotalEstimatedCalculation();

            // Reload
            $("#tbl_costagent").trigger("reloadGrid");
        }
    }

    // Declare Table jqgrid
    jQuery("#tbl_costagent").jqGrid({
        datatype: "local",
        height: 180,
        rowNum: 150,
        colNames: ['AgentId', 'AgentCode', 'Agent', 'AccountName', 'Description', 'Amount USD', 'accountid', 'type', 'quantity', 'perqty', 'codingquantity', '',
                        'hfpsinfo', 'dmr', 'dmrcontainerdetail', 'dmrcontainer', 'counterprofitsplit', ''],
        colModel: [
            { name: 'agentid', index: 'agentid', width: 60, hidden: true },
            { name: 'agentcode', index: 'agentcode', width: 60, hidden: true },
            { name: 'agentname', index: 'agentname', width: 250 },
            { name: 'accountname', index: 'accountname', width: 350, hidden: true },
            { name: 'description', index: 'description', width: 400 },
            { name: 'amountusd', index: 'amountusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'accountid', index: 'accountid', width: 60, hidden: true },
            { name: 'type', index: 'type', width: 60, hidden: true },
            { name: 'quantity', index: 'quantity', width: 60, hidden: true },
            { name: 'perqty', index: 'perqty', width: 60, hidden: true },
            { name: 'codingquantity', index: 'codingquantity', width: 60, hidden: true },
            { name: 'sign', index: 'sign', width: 30, align: "center" },
            { name: 'hfpsinfo', index: 'hfpsinfo', hidden: true },
            { name: 'dmr', index: 'dmr', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainerdetail', index: 'dmrcontainerdetail', width: 50, align: "right", hidden: true },
            { name: 'dmrcontainer', index: 'dmrcontainer', width: 50, align: "right", hidden: true },
            { name: 'counterprofitsplit', index: 'counterprofitsplit', width: 60, hidden: true },
            { name: 'issplitinccost', index: 'issplitinccost', width: 60, hidden: true }
        ]
    });
    // End Cost Agent

    function GetNextId(objJqGrid) {
        var nextid = 0;
        if (objJqGrid.getDataIDs().length > 0)
            nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

        return nextid;
    }

    // -------------------------- Demurrage ----------------------------------------------------------------------
    // Browse Demurrage(Agent, Consignee)
    $('#btnDemcustomer').click(function () {
        if (vDemurrageType == 'agent') {
            // Clear Search string jqGrid
            $('input[id*="gs_"]').val("");
            vAgent = "demurrage";
            var lookupGrid = $('#lookup_table_agent');
            lookupGrid.setGridParam({
                postData: { 'shipmentId': function () { return shipmentId; } },
                url: base_url + 'ShipmentOrder/LookUpAgent'
            }).trigger("reloadGrid");
            $('#lookup_div_agent').dialog('open');
        }
        else if (vDemurrageType == 'consignee') {
            // Clear Search string jqGrid
            $('input[id*="gs_"]').val("");
            vConsignee = "demurrage";
            var lookupGrid = $('#lookup_table_consignee');
            lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsConsignee', postData: { filters: null }, search: false }).trigger("reloadGrid");
            $('#lookup_div_consignee').dialog('open');
        }
        else
            $.messager.alert('Information', 'Invalid Customer Type...!!', 'info');
    });

    function ClearDemurrage() {
        if (vDemurrageType == 'agent') {
            $('#txtDemkdcustomer').data('kode', $("#txtSEagent").data('kode')).val($("#txtSEagent").data('kode'));
            $('#txtDemnmcustomer').val($("#txtSEagent").val());
        }
            // Inc Consignee
        else {
            $('#txtDemkdcustomer').data('kode', $("#txtSEconsignee").data('kode')).val($("#txtSEconsignee").data('kode'));
            $('#txtDemnmcustomer').val($("#txtSEconsignee").val());
        }
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

        var eplDetailId = 0;

        // Inc Consignee
        if (vDemurrageType == 'consignee') {

            // Grand Total
            var grandTotal = $('#txtDemgrandtotal').numberbox('getValue');

            // Demurrage
            var dmr = {};
            dmr.Freetime = $('#txtDemfreetimedays').val();
            dmr.Fase = $('#txtDemfasedays').val();
            dmr.DateBackContainer = $('#txtDembackcontainer').datebox('getValue');
            dmr.TotalContainer20 = $('#txtDemtotal20').val();
            dmr.TotalContainer40 = $('#txtDemtotal40').val();
            dmr.DiscType = 'P';
            if ($('#rbDemdiscountamount').is(':checked'))
                dmr.DiscType = 'A';
            dmr.DiscAmount = $('#txtDemdiscount').val();

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

            // Demurrage Container Detail
            var dmrContainerDetail = [];
            var ids = $("#tbl_dem_list").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var data = jQuery("#tbl_dem_list").jqGrid('getRowData', ids[i]);
                var newData = {};
                newData.Description = data.description;
                newData.DayContainer = data.days;
                newData.AmountContainer20 = data.type20;
                newData.AmountContainer40 = data.type40;
                dmrContainerDetail.push(newData);
            }

            // Add NEW
            dmr = JSON.stringify(dmr);
            dmrContainerDetail = JSON.stringify(dmrContainerDetail);
            dmrContainer = JSON.stringify(dmrContainer);

            // Assigned EPL Detail Id
            var eplDetailId = 0
            if (vDemurrageEdit == 1) {
                eplDetailId = $('#lookup_btn_add_demurrage').data('epldetailid');
            }

            // ONLY on UPDATE EPL
            if ((eplId != undefined && eplId != "")) {

                var submitURL = (eplId != undefined && eplId != "") ? (vDemurrageEdit == 0) ? base_url + "epl/InsertIncConsignee" : base_url + "epl/UpdateIncConsignee" : "";
                // Submit To Server
                SaveEPLDetail(submitURL, eplDetailId, $('#txtDemkdcustomer').data('kode'), '000', 'TOTAL DEMURRAGE',
                                0, 0, 5, 0, true, 0, grandTotal, 0, '', dmr, dmrContainer, dmrContainerDetail, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {
                    addIncConsignee(vDemurrageEdit, eplDetailId, $('#txtDemkdcustomer').data('kode'), $('#txtDemkdcustomer').val(),
                        $('#txtDemnmcustomer').val(), '000', 'TOTAL DEMURRAGE', 'TOTAL DEMURRAGE', 5, 0, grandTotal, 0, 0, 0, 0, true, dmr, dmrContainerDetail, dmrContainer);
                }
            }
            else {
                addIncConsignee(vDemurrageEdit, eplDetailId, $('#txtDemkdcustomer').data('kode'), $('#txtDemkdcustomer').val(),
                    $('#txtDemnmcustomer').val(), '000', 'TOTAL DEMURRAGE', 'TOTAL DEMURRAGE', 5, 0, grandTotal, 0, 0, 0, 0, true, dmr, dmrContainerDetail, dmrContainer);
            }
        }
            // Cost Agent
        else if (vDemurrageType == 'agent') {

            // Grand Total
            var grandTotal = $('#txtDemgrandtotal').numberbox('getValue');

            // Demurrage
            var dmr = {};
            dmr.Freetime = $('#txtDemfreetimedays').val();
            dmr.Fase = $('#txtDemfasedays').val();
            dmr.DateBackContainer = $('#txtDembackcontainer').datebox('getValue');
            dmr.TotalContainer20 = $('#txtDemtotal20').val();
            dmr.TotalContainer40 = $('#txtDemtotal40').val();
            dmr.DiscType = 'P';
            if ($('#rbDemdiscountamount').is(':checked'))
                dmr.DiscType = 'A';
            dmr.DiscAmount = $('#txtDemdiscount').val();

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

            // Demurrage Container Detail
            var dmrContainerDetail = [];
            var ids = $("#tbl_dem_list").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var data = jQuery("#tbl_dem_list").jqGrid('getRowData', ids[i]);
                var newData = {};
                newData.Description = data.description;
                newData.DayContainer = data.days;
                newData.AmountContainer20 = data.type20;
                newData.AmountContainer40 = data.type40;
                dmrContainerDetail.push(newData);
            }

            // Add NEW
            dmr = JSON.stringify(dmr);
            dmrContainerDetail = JSON.stringify(dmrContainerDetail);
            dmrContainer = JSON.stringify(dmrContainer);

            // Assigned EPL Detail Id
            var eplDetailId = 0
            if (vDemurrageEdit == 1) {
                eplDetailId = $('#lookup_btn_add_demurrage').data('epldetailid');
            }

            // ONLY on UPDATE EPL
            if ((eplId != undefined && eplId != "")) {

                var submitURL = (eplId != undefined && eplId != "") ? (vDemurrageEdit == 0) ? base_url + "epl/InsertCostAgent" : base_url + "epl/UpdateCostAgent" : "";
                // Submit To Server
                SaveEPLDetail(submitURL, eplDetailId, $('#txtDemkdcustomer').data('kode'), '000', 'TOTAL DEMURRAGE',
                                0, 0, 5, 0, true, 0, grandTotal, 0, '', dmr, dmrContainer, dmrContainerDetail, function (data) {
                                    isValid = data.isValid;
                                    message = data.message;
                                    if (data.objResult != null)
                                        eplDetailId = data.objResult.EPLDetailId;
                                });

                if (!isValid) {
                    $.messager.alert('Warning', message, 'warning');
                }
                else {
                    addCostAgent(vDemurrageEdit, eplDetailId, $('#txtDemkdcustomer').data('kode'), $('#txtDemkdcustomer').val(),
                        $('#txtDemnmcustomer').val(), '000', 'TOTAL DEMURRAGE', 'TOTAL DEMURRAGE', 5, grandTotal, 0, 0, 0, true, '', dmr, dmrContainerDetail, dmrContainer);
                }
            }
            else {
                addCostAgent(vDemurrageEdit, eplDetailId, $('#txtDemkdcustomer').data('kode'), $('#txtDemkdcustomer').val(),
                    $('#txtDemnmcustomer').val(), '000', 'TOTAL DEMURRAGE', 'TOTAL DEMURRAGE', 5, grandTotal, 0, 0, 0, true, '', dmr, dmrContainerDetail, dmrContainer);
            }
        }


        // Calculate Total Estimated
        TotalEstimatedCalculation();
        $('#lookup_div_demurrage').dialog('close');
    });

    // Close Demurrage
    $('#lookup_btn_cancel_demurrage').click(function () {
        $('#lookup_div_demurrage').dialog('close');
    });

    // Demurrage Cost Agent Button click
    $('#btn_demurrage_cost_agent').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        $('#spanDemCustmer').html('Agent');
        vDemurrageType = 'agent';
        vDemurrageEdit = 0;
        ClearDemurrage();

        $('#lookup_div_demurrage').dialog('open');
    });

    // Demurrage Inc Consignee Button click
    $('#btn_demurrage_inc_consignee').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        $('#spanDemCustmer').html('Consignee');
        vDemurrageType = 'consignee';
        vDemurrageEdit = 0;
        ClearDemurrage();

        $('#lookup_div_demurrage').dialog('open');
    });

    // Declare Table jqgrid Container List
    jQuery("#tbl_dem_container_list").jqGrid({
        datatype: "local",
        height: 60,
        rowNum: 150,
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
        rowNum: 150,
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


    // ------------------------------------------------------------------------ Demurrage - Container
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

    // ------------------------------------------------- Demurrage Discount Calculation -----------------------------------------------

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
    // ------------------------------------------------- END Demurrage Discount Calculation -------------------------------------------

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


    // ------------------------------------------------------------------------ SAVE EPL
    $('#seaimport_form_btn_save').click(function () {
        if (shipmentId == "") {
            $.messager.alert('Information', "Shipment Order Number can't be empty...!!", 'info');
            return;
        }

        // Summary JPL Calculation
        var estIncConsigneeUSD = $("#txtEstIncConsigneeUsd").numberbox("getValue");
        var estIncConsigneeIDR = $("#txtEstIncConsigneeIdr").numberbox("getValue");
        var estCostSSLineUSD = $("#txtEstCostSSLineUsd").numberbox("getValue");
        var estCostSSLineIDR = $("#txtEstCostSSLineIdr").numberbox("getValue");
        var estCostEMKLUSD = $("#txtEstCostEMKLUsd").numberbox("getValue");
        var estCostEMKLIDR = $("#txtEstCostEMKLIdr").numberbox("getValue");
        var estCostRebateUSD = $("#txtEstCostRebateUsd").numberbox("getValue");
        var estCostRebateIDR = $("#txtEstCostRebateIdr").numberbox("getValue");
        var estCostDepoUSD = $("#txtEstCostDepoUsd").numberbox("getValue");
        var estCostDepoIDR = $("#txtEstCostDepoIdr").numberbox("getValue");
        var estIncAgentUSD = $("#txtEstIncAgentUsd").numberbox("getValue");
        var estIncAgentIDR = $("#txtEstIncAgentIdr").numberbox("getValue");
        var estCostAgentUSD = $("#txtEstCostAgentUsd").numberbox("getValue");
        var estCostAgentIDR = $("#txtEstCostAgentIdr").numberbox("getValue");

        var estUSDShipCons = parseFloat(estIncConsigneeUSD) - (parseFloat(estCostSSLineUSD) + parseFloat(estCostEMKLUSD) + parseFloat(estCostRebateUSD) + parseFloat(estCostDepoUSD));
        estUSDShipCons = Math.round(estUSDShipCons * 100) / 100;
        var estIDRShipCons = parseFloat(estIncConsigneeIDR) - (parseFloat(estCostSSLineIDR) + parseFloat(estCostEMKLIDR) + parseFloat(estCostRebateIDR) + parseFloat(estCostDepoIDR));
        estIDRShipCons = Math.round(estIDRShipCons * 100) / 100;

        var estUSDAgent = parseFloat(estIncAgentUSD) - parseFloat(estCostAgentUSD);
        estUSDAgent = Math.round(estUSDAgent * 100) / 100;
        var estIDRAgent = parseFloat(estIncAgentIDR) - parseFloat(estCostAgentIDR);
        estIDRAgent = Math.round(estIDRAgent * 100) / 100;
        // END Summary JPL Calculation

        // Income Consignee
        var IncConsignee = PopulateIncomeConsignee();
        // END Income Consignee

        // Cost SSLine
        var CostSSLine = PopulateCostSSLine();
        // END Cost SSLine

        // Cost EMKL
        var CostEMKL = PopulateCostEMKL();
        // END Cost EMKL

        // Cost Rebate
        var CostRebate = PopulateCostRebate();
        // END Cost Rebate

        // Cost Depo
        var CostDepo = PopulateCostDepo();
        // END Cost Depo

        // Inc Agent
        var IncAgent = PopulateIncAgent();
        // END Inc Agent

        // Cost Agent
        var CostAgent = PopulateCostAgent();
        // END Cost Agent


        var submitURL = base_url + "epl/Insert";
        if (eplId != undefined && eplId != 0)
            submitURL = base_url + "epl/Update";

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                EPLId: eplId, shipmentId: shipmentId,
                ETD: $('#txtSEetd').val(),
                EstUSDShipCons: estUSDShipCons, EstIDRShipCons: estIDRShipCons, EstUSDAgent: estUSDAgent, EstIDRAgent: estIDRAgent,
                IncConsigneeList: IncConsignee, CostSSLineList: CostSSLine, CostEMKLList: CostEMKL, CostRebateConsigneeList: CostRebate, CostDepoList: CostDepo,
                IncAgentList: IncAgent, CostAgentList: CostAgent,
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "epl/detail?Id=" + result.objResult.EPLId + "&JobId=" + JobId;
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // Income Consignee
    function PopulateIncomeConsignee() {
        var IncConsignee = [];
        var data = $("#tbl_incconsignee").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['ConsigneeId'] = data[i].consigneeid;
            obj['ConsigneeCode'] = data[i].consigneecode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            // Demurrage            
            obj['Dmr'] = data[i].dmr;
            obj['DmrContainerDetail'] = data[i].dmrcontainerdetail;
            obj['DmrContainer'] = data[i].dmrcontainer;

            IncConsignee.push(obj);
        }

        return IncConsignee
    }
    // END Income Consignee

    // Cost SSLine
    function PopulateCostSSLine() {
        var CostSSLine = [];
        var data = $("#tbl_costssline").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['SSLineId'] = data[i].sslineid;
            obj['SSLineCode'] = data[i].sslinecode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            CostSSLine.push(obj);
        }

        return CostSSLine
    }
    // END Cost SSLine

    // Cost EMKL
    function PopulateCostEMKL() {
        var CostEMKL = [];
        var data = $("#tbl_costemkl").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['EMKLId'] = data[i].emklid;
            obj['EMKLCode'] = data[i].emklcode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            CostEMKL.push(obj);
        }

        return CostEMKL
    }
    // END Cost EMKL

    // Cost Rebate
    function PopulateCostRebate() {
        var CostRebate = [];
        var data = $("#tbl_costrebate").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['ShipperId'] = data[i].rebateid;
            obj['ShipperCode'] = data[i].rebatecode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            CostRebate.push(obj);
        }

        return CostRebate
    }
    // END Cost Rebate

    // Cost Depo
    function PopulateCostDepo() {
        var CostDepo = [];
        var data = $("#tbl_costdepo").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['DepoId'] = data[i].depoid;
            obj['DepoCode'] = data[i].depocode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            CostDepo.push(obj);
        }

        return CostDepo
    }
    // END Cost Depo

    // Inc Agent
    function PopulateIncAgent() {
        var IncAgent = [];
        var data = $("#tbl_incagent").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['AgentId'] = data[i].agentid;
            obj['AgentCode'] = data[i].agentcode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['HFPSinfo'] = data[i].hfpsinfo;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            IncAgent.push(obj);
        }

        return IncAgent
    }
    // END Inc Agent

    // Cost Agent
    function PopulateCostAgent() {
        var CostAgent = [];
        var data = $("#tbl_costagent").jqGrid('getGridParam', 'data');
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj['AgentId'] = data[i].agentid;
            obj['AgentCode'] = data[i].agentcode;
            obj['AmountUSD'] = data[i].amountusd;
            obj['AmountIDR'] = data[i].amountidr;
            obj['Description'] = data[i].description;
            obj['AmountCrr'] = data[i].currencytype;
            obj['AccountID'] = data[i].accountid;
            obj['Type'] = data[i].type;
            obj['Quantity'] = data[i].quantity;
            obj['PerQty'] = data[i].perqty;
            obj['CodingQuantity'] = data[i].codingquantity;
            obj['HFPSinfo'] = data[i].hfpsinfo;
            obj['Sign'] = false;
            if (data[i].sign == "+")
                obj['Sign'] = true;
            obj['IsSplitIncCost'] = data[i].issplitinccost;

            // Demurrage            
            obj['DmrHeader'] = data[i].dmrheader;
            obj['DmrAmount'] = data[i].dmramount;
            obj['DmrContainer'] = data[i].dmrcontainer;

            CostAgent.push(obj);
        }

        return CostAgent
    }
    // END Cost Agent

    // ValidateAccountEPL
    function ValidateAccountEPL(v_eplId, v_epldetailId, callback) {

        // Check Payment Request && Invoice
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "EPL/ValidateAccountEPL",
            data: JSON.stringify({
                EPLDetailId: v_epldetailId,
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
    function SaveEPLDetail(submitURL, v_epldetailId, v_customerId, v_accountId, v_description, v_quantity, v_perqty, v_type, v_codingquantity,
                                        v_sign, v_amountCrr, v_amountUSD, v_amountIDR, v_hfpsinfo, v_dmr, v_dmrContainer, v_dmrContainerDetail, v_isSplitInCost, callback) {
        if (submitURL != undefined && submitURL != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify({
                    EPLDetailId: v_epldetailId,
                    EPLId: eplId, shipmentId: shipmentId, CustomerId: v_customerId,
                    AccountID: v_accountId, Description: v_description, Quantity: v_quantity, PerQty: v_perqty,
                    Type: v_type, Sign: v_sign, CodingQuantity: v_codingquantity,
                    AmountCrr: v_amountCrr, AmountUSD: v_amountUSD, AmountIDR: v_amountIDR,
                    HFPSinfo: v_hfpsinfo, IsSplitIncCost: v_isSplitInCost,
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
    // END Save EPL Detail

    // Delete EPL Detail
    function DeleteEPLDetail(v_epldetailId, callback) {
        if (eplId != undefined && eplId != "") {
            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + 'epl/DeleteEPLDetail',
                data: JSON.stringify({
                    EPLDetailId: v_epldetailId
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

    // Get Container Size List
    function GetContainerSizeList() {

        // Get Container Size List
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + 'ShipmentOrder/GetContainerSizeList',
            data: JSON.stringify({
                shipmentId: shipmentId
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (result.isValid) {
                    vContainerCount20 = 0;
                    vContainerCount40 = 0;
                    vContainerCount45 = 0;
                    if (result.objResult != null || result.objResult != undefined) {
                        for (var i = 0; i < result.objResult.length; i++) {
                            switch (result.objResult[i].SizeType) {
                                case 1:   // Container 20'
                                    if (parseInt(result.objResult[i].SizeCount) > 0) {

                                        vContainerCount20 = parseInt(result.objResult[i].SizeCount);
                                    }
                                    break;
                                case 2:   // Container 40'
                                    if (parseInt(result.objResult[i].SizeCount) > 0) {

                                        vContainerCount40 = parseInt(result.objResult[i].SizeCount);
                                    }
                                    break;
                                case 3:   // Container 45'
                                    if (parseInt(result.objResult[i].SizeCount) > 0) {

                                        vContainerCount45 = parseInt(result.objResult[i].SizeCount);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        });

    }
    // END Get Container Size List

});