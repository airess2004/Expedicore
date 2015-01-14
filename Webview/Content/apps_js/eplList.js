$(document).ready(function () {

    // First Load
    // tools.js
    var type = getQueryStringByName('type');
    if (type == undefined || type == "") {
        type = '1';
    }
    $('#jobtype').val(type);

    function ReloadGrid() {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();

        $("#list_epl").setGridParam({ url: base_url + 'EstimateProfitLoss/GetList', postData: { filters: null, JobId: value }, page: 'last' }).trigger("reloadGrid");
    }

    /*================================================ EPL List ================================================*/
    jQuery("#list_epl").jqGrid({
        url: base_url + 'EstimateProfitLoss/GetList',
        postData: { 'JobId': function () { return $("#jobtype").val(); } },
        datatype: "json",
        colNames: ['Del', 'Confirmed','Shipment', 'ETD/ETA', 'Total Est. USD', 'Total Est. IDR', 'Total Income USD', 'Total Cost USD',
				'Total Income IDR', 'Total Cost IDR', 'Close', 'Printing', 'Print Date', 'Entry Date', 'User'],
        colModel: [{ name: 'deleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
                  { name: 'confirmed', index: 'IsConfirmed', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } ,formatter : 'select'},
                  { name: 'shipmentno', index: 'ShipmentOrderCode', width: 130, align: "center" },
				  { name: 'etd', index: 'ETDETA', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'estimateusd', index: 'TotalUSD', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'estimateusd', index: 'TotalIDR', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'estimateusd', index: 'TotalIncomeUSD', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'estimateidr', index: 'TotalCostUSD', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'estimateusdagent', index: 'TotalCostIDR', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'estimateidragent', index: 'TotalIncomeIDR', width: 130, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'datecloseepl', index: 'DateClose', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'printing', index: 'Printing', width: 60 },
				  { name: 'printedon', index: 'PrintDate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'entrydate', index: 'CreatedAt', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'usercode', index: 'CreatedBy', width: 100 }
        ],
        page: 'last', // last page
        pager: jQuery('#pager_list_epl'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        viewrecords: true,
        shrinkToFit: false,
        sortname: "ShipmentOrderCode",
        sortorder: "asc",
        width: $("#epl_toolbar").width(),
        height: $(window).height() - 120,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowDel = $(this).getRowData(cl).deleted;
		          if (rowDel == 'true') {
		              rowDel = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		          } else {
		              rowDel = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { deleted: rowDel });
		          rowJClose = $(this).getRowData(cl).jobclose;
		          if (rowJClose == 'true') {
		              rowJClose = "YES";
		          } else {
		              rowJClose = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { jobclose: rowJClose });
		          estimateusd = $(this).getRowData(cl).estimateusd;
		          estimateidr = $(this).getRowData(cl).estimateidr;
		          if (estimateusd.indexOf('-') !== -1) {
		              //alert("estimateusd:" + estimateusd);
		              $(this).jqGrid('setRowData', ids[i], false, 'fontred');
		          }
		          if (estimateidr.indexOf('-') !== -1) {
		              //alert("estimateidr:" + estimateidr);
		              $(this).jqGrid('setRowData', ids[i], false, 'fontred');
		          }
		      }
		  }
    });//END GRID
    $("#list_epl").jqGrid('navGrid', '#toolbar_trans_epl', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    /*================================================ End EPL List ================================================*/


    $("#epl_btn_reload").click(function () {
        ReloadGrid();
    });

    $("#jobtype").live("change", function () {
        ReloadGrid();
    });

    $("#epl_btn_add_new").click(function () {

        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        window.location = base_url + "EstimateProfitLoss/detail?JobId=" + value;

    });

    $("#epl_btn_edit, #epl_btn_del").click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        var buttonID = $(this).attr('id');

        var eplid = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (eplid) {
            var ret = jQuery("#list_epl").jqGrid('getRowData', eplid);
            if (ret.deleted != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }

            // On Edit
            if (buttonID == "epl_btn_edit") {
                window.location = base_url + "EstimateProfitLoss/detail?Id=" + eplid + "&JobId=" + value;
            }
            else {
                DeleteEPL(eplid);
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    function DeleteEPL(eplid) {

        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "EstimateProfitLoss/Delete",
                    data: JSON.stringify({
                        EPLId: eplid
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
        });
    }

    // Un-Delete
    $('#epl_btn_undel').click(function () {
        var id = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.messager.confirm('Confirm', 'Are you sure you want to Undelete this EPL?', function (r) {
                if (r) {

                    $.ajax({
                        url: base_url + "EstimateProfitLoss/UnDelete",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ EPLId: id }),
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
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Close
    $('#epl_btn_close').click(function () {
        var id = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.messager.confirm('Confirm', 'Are you sure you want to CLOSE this EPL?', function (r) {
                if (r) {

                    $.ajax({
                        url: base_url + "EPL/Close",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ EPLId: id }),
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
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Un-Close
    $('#epl_btn_unclose').click(function () {
        var id = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.messager.confirm('Confirm', 'Are you sure you want to Un-Close this EPL?', function (r) {
                if (r) {

                    $.ajax({
                        url: base_url + "EPL/UnClose",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ EPLId: id }),
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
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    // Print
    $("#epl_btn_print").click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        var buttonID = $(this).attr('id');

        var eplid = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (eplid) {
            var ret = jQuery("#list_epl").jqGrid('getRowData', eplid);
            window.open(base_url + "EPL/PrintEPL?Id=" + eplid);
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    //Confirm EPL
    $('#btn_confirm').click(function () {
        var id = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_epl").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to confirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "EstimateProfitLoss/Confirm",
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
    // END Confirm 

    //Unconfirm EPL
    $('#btn_unconfirm').click(function () {
        var id = jQuery("#list_epl").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_epl").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unconfirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "EstimateProfitLoss/Unconfirm",
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
    // END Unconfirm EPL

}); //END DOCUMENT READY
