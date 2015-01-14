$(document).ready(function () {
    var caDetailEdit = 0;

    $('#lookup_div_personalcode').dialog('close');
    $('#lookup_div_balanceoverall').dialog('close');
    $('#lookup_div_cashadvance').dialog('close');
    $('#casdetail_content').css("height", $(window).height() - 120);
    $('#casdetail_content').css("width", $(window).width() - 20);

    // First Load
    // tools.js
    var SettlementId = getQueryStringByName('Id');
    // Edit Mode
    if (SettlementId != "") {
        $('#btnPersonalCode, #btncano').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
        //$('#txtCashUsd, #txtCashIdr').attr('disabled', 'disabled');
        FirstLoad();
    }

    function FirstLoad() {
        $.ajax({
            dataType: "json",
            url: base_url + "CashSettlement/GetInfo?Id=" + SettlementId,
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                            window.location = base_url + "CashSettlement";
                        }
                    }
                }
                else {
                    $('#txtReference').val(result.Reference);
                    $('#txtcasettlementno').val(result.CashAdvanceNo);
                    $('#txtRate').numberbox('setValue', result.CashSettlementRate);
                    $('#txtDateRate').val(dateEnt(result.ExRateDate));
                    $('#txtDatePrint').datebox('setValue', dateEnt(result.PrintedAt));

                    $('#txtPersonalCode').val(result.EmployeeId).data('kode', result.EmployeeId);
                    $('#txtPersonalName').val(result.EmployeeName);

                    $('#txtCashUsd, #txtBsUsd').numberbox('setValue', result.SettlementUSD);
                    $('#txtCashIdr, #txtBsIdr').numberbox('setValue', result.SettlementIDR);

                    $('#txtBsUsd').numberbox('setValue', result.SettlementUSD);
                    $('#txtBsIdr').numberbox('setValue', result.SettlementIDR);

                    $('#txtBpcaIdr').numberbox('setValue', result.CashAdvanceIDR);
                    $('#txtBpcaUsd').numberbox('setValue', result.CashAdvanceUSD);
                    $('#txtBUsd').numberbox('setValue', result.CashAdvanceIDR);
                    $('#txtBIdr').numberbox('setValue', result.CashAdvanceUSD);

                    $('#txtcano').val(result.CashAdvanceNo).data('kode', result.CashAdvanceId);

                    CalculatePayment();

                    // Enable or Disable Button
                    if (result.CashSettlementPrint > 0) {
                        // Disable Save 
                        // infoss.Models.ConfigurationModels.MODE_TRIAL()
                        //$('#cas_form_btn_save').removeAttr('onclick').unbind('click').off('click');
                    }
                    else {
                        $('#txtDatePrint').datebox({
                            disabled: true
                        });
                    }

                }
            }
        });
    }

    //Table GI Detail
    jQuery("#table_ca_detail").jqGrid({
        datatype: "local",
        height: 80,
        width: 915,
        rowNum: 150,
        scrollrows: true,
        shrinkToFit: false,
        colNames: ['Description', 'Amount USD', 'Amount IDR', 'Ref Shipment', 'Ref PR'],
        colModel: [
            { name: 'description', index: 'description', width: 310 },
            { name: 'amountusd', index: 'amountusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'amountidr', index: 'amountidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'refshipment', index: 'refshipment', width: 115 },
            { name: 'refpr', index: 'refpr', width: 115 },
        ]
    });

    // ---------------------------------------------------- LOOK UP CASH ADVANCE --------------------------------------------------------------------------
    // Browse
    $('#btncano').click(function () {
        var empId = $('#txtPersonalCode').data('kode');
        if (empId == undefined || empId == "" || empId == "0" || empId == 0) {
            $.messager.alert('Warning', "Please select Personal Code First...", 'warning');
            return;
        }

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'CashAdvance/GetLookUp';
        var lookupGrid = $('#lookup_table_cashadvance');
        lookupGrid.setGridParam({
            postData: { 'EmployeeId': function () { return empId; }, filters: null },
            url: lookUpURL
        }).trigger("reloadGrid");

        $('#lookup_div_cashadvance').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_cashadvance').click(function () {
        $('#lookup_div_cashadvance').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_cashadvance').click(function () {
        var id = jQuery("#lookup_table_cashadvance").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_cashadvance").jqGrid('getRowData', id);

            $('#txtcano').val(ret.cashbondno).data("kode", id);
            $('#txtBpcaUsd').numberbox('setValue', ret.cashbondusd);
            $('#txtBpcaIdr').numberbox('setValue', ret.cashbondidr);

            $('#lookup_div_cashadvance').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_cashadvance").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['CA. No', 'C.A. USD', 'C.A. IDR', 'Balance USD', 'Balance IDR'],
        colModel: [{ name: 'cashbondno', index: 'CashAdvanceNo', width: 130, align: "center" },
				  { name: 'cashbondusd', index: 'CashAdvanceUSD', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'cashbondidr', index: 'CashAdvanceIDR', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'balanceusd', index: 'balanceusd', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'balanceidr', index: 'balanceidr', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ],
        page: 'last', // last page
        pager: jQuery('#lookup_pager_cashadvance'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'CashAdvanceNo',
        viewrecords: true,
        sortorder: "asc",
        width: 490,
        height: 340
    });
    $("#lookup_table_cashadvance").jqGrid('navGrid', '#toolbar_lookup_cashadvance', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_cashadvance").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_cashadvance').click();
        },
        scrollingRows: true
    });

    // ---------------------------------------------------- END LOOK UP CASH ADVANCE --------------------------------------------------------------------------   

    // ---------------------------------------------------- LOOK UP PERSONAL CODE --------------------------------------------------------------------------
    // Browse
    $('#btnPersonalCode').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'MstEmployee/GetList';
        var lookupGrid = $('#lookup_table_personalcode');
        lookupGrid.setGridParam({
            url: lookUpURL,
            postData: { filters: null }
        }).trigger("reloadGrid");

        $('#lookup_div_personalcode').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_personalcode').click(function () {
        $('#lookup_div_personalcode').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_personalcode').click(function () {
        var id = jQuery("#lookup_table_personalcode").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_personalcode").jqGrid('getRowData', id);

            $('#txtPersonalCode').val(ret.code).data("kode", id);
            $('#txtPersonalName').val(ret.name);

            $('#lookup_div_personalcode').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_personalcode").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Personal Name', 'Group'],
        colModel: [{ name: 'code', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'department', index: 'GroupEmployee', width: 200 }],
        page: 'last', // last page
        pager: jQuery('#lookup_pager_personalcode'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'name',
        viewrecords: true,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_personalcode").jqGrid('navGrid', '#toolbar_lookup_personalcode', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_table_personalcode").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#lookup_btn_add_personalcode').click();
        },
        scrollingRows: true
    });

    // ---------------------------------------------------- END LOOK UP PERSONAL CODE --------------------------------------------------------------------------   


    // ----------------------------------------------------  GRID BALANCE OVERALL --------------------------------------------------------------------------
    $('#btn_balance_overall').click(function () {
        var empId = $('#txtPersonalCode').data('kode');
        if (empId == undefined || empId == "" || empId == "0" || empId == 0) {
            $.messager.alert('Warning', "Please select Personal Code First...", 'warning');
            return;
        }

        $("#lookup_table_balanceoverall").setGridParam({
            postData: { 'EmployeeId': function () { return empId; } },
            url: base_url + 'CashAdvance/GetValidCombCashAdvanceList'
        }).trigger("reloadGrid");
        $('#lookup_div_balanceoverall').dialog('open');
    });

    $("#lookup_table_balanceoverall").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        colNames: ['Personal Name', 'Balance USD', 'BalanceIDR'],
        colModel: [
				  { name: 'personalname', index: 'employeename', width: 150 },
                  { name: 'balanceusd', index: 'balanceusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'balanceidr', index: 'balanceidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } }
        ],
        page: '1',
        pager: $('#lookup_pager_balanceoverall'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'employeename',
        scrollrows: true,
        shrinkToFit: false,
        viewrecords: true,
        sortorder: "ASC",
        width: $("#lookup_div_balanceoverall").width() - 10,
        height: $("#lookup_div_balanceoverall").height() - 110
    });

    $("#lookup_table_balanceoverall").jqGrid('navGrid', '#lookup_toolbar_balanceoverall', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#lookup_btn_cancel_balanceoverall").click(function () {
        $('#lookup_div_balanceoverall').dialog("close");
    });

    $("#lookup_btn_add_balanceoverall").click(function () {
        var id = jQuery("#lookup_table_balanceoverall").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_balanceoverall").jqGrid('getRowData', id);
            $('#lookup_div_balanceoverall').dialog("close");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }

    });

    // ---------------------------------------------------- END GRID BALANCE OVERALL --------------------------------------------------------------------------

    function CalculatePayment() {
        var balanceUsd = 0;
        var balanceIdr = 0;
        var balanceSettleUsd = $('#txtBsUsd').numberbox('getValue');
        var balanceSettleIdr = $('#txtBsIdr').numberbox('getValue');
        var balanceCAidr = $('#txtBpcaIdr').numberbox('getValue');
        var balanceCAusd = $('#txtBpcaUsd').numberbox('getValue');

        balanceIdr = parseFloat(parseFloat(balanceCAidr) - parseFloat(balanceSettleIdr));
        if (isNaN(balanceIdr))
            balanceIdr = 0;
        balanceUsd = parseFloat(parseFloat(balanceCAusd) - parseFloat(balanceSettleUsd));
        if (isNaN(balanceUsd))
            balanceUsd = 0;

        $('#txtBUsd').numberbox('setValue', balanceUsd);
        $('#txtBIdr').numberbox('setValue', balanceIdr);
    }

    function GetNextId(objJqGrid) {
        var nextid = 0;
        if (objJqGrid.getDataIDs().length > 0)
            nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

        return nextid;
    }

    // ---------------------------------------------------------------------- SAVE
    $('#cas_form_btn_save').click(function () {

        var submitURL = base_url + "CashSettlement/Insert";
        if (SettlementId != undefined && SettlementId != 0)
            submitURL = base_url + "CashSettlement/Update";

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: SettlementId,
                EmployeeId: $('#txtPersonalCode').data("kode"),
                CashAdvanceId: $('#txtcano').data("kode"),
                SettlementUSD: $('#txtCashUsd').numberbox("getValue"),
                PrintDate: $('#txtDatePrint').datebox('getValue'),
                SettlementIDR: $('#txtCashIdr').numberbox("getValue")
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
                    window.location = base_url + "CashSettlement/Detail?Id=" + result.cashSettlementId;
                }
            }
        });
    });

    //Print Cash Advance Settlement
    $('#cas_form_btn_print').click(function () {
        window.open(base_url + "CashSettlement/Print?Id=" + SettlementId);
    });
    //END Print Cash Advance Settlement

    // Add New Cash Advance Settlement
    $('#casettlement_btn_add_new').click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to create New Settlement?', function (r) {
            if (r) {
                window.location = base_url + "CashSettlement/Detail";
            }
        });
    });
    // END Add New Cash Advance Settlement

}); //END DOCUMENT READY
