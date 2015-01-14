$(document).ready(function () {

    // Initialize
    $('#dialogEdit').dialog('close');
    $('#dialogDelete').dialog('close');

    function ReloadGrid() {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();

        $("#list_paymentvoucher").setGridParam({ url: base_url + 'CashSettlement/GetList', postData: { filters: null, JobId: value }, page: 'last' }).trigger("reloadGrid");
    }

    //GRID LOOKUP +++++++++++++++
    $("#list_paymentvoucher").jqGrid({
        url: base_url + 'CashSettlement/GetList',
        datatype: "json",
        colNames: ['Del', 'Confirmed', 'Settlement No', 'Employee Name','C.A. No', 'Cash USD', 'Cash IDR', 'Created At', 'Created By', 'Updated At'],
        colModel: [
			      { name: 'deleted', index: 'Isdeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
			      { name: 'isConfirmed', index: 'IsConfirmed', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" },formatter : 'select' },
                  { name: 'pvno', index: 'SettlementNo', width: 80, align: "center" },
                  { name: 'reference', index: 'EmployeeName', width: 180 },
                  { name: 'pvno', index: 'CashAdvanceNo', width: 80, align: "center" },
				  { name: 'totalpayment', index: 'SettlementIDR', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'totalpayment', index: 'SettlementUSD', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'dateprint', index: 'CreatedAt', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'dateprint', index: 'CreatedBy'},
                  { name: 'dateprint', index: 'UpdatedAt', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: 'last',
        pager: $('#pager_paymentvoucher'),
        rowNum: 50,
        rowList: [50, 100, 150],
        sortname: 'SettlementNo',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "ASC",
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];

		          var showRowDel = "";
		          if ($(this).getRowData(cl).deleted == 'true') {
		              showRowDel = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		          } else {
		              showRowDel = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { deleted: showRowDel });

		          var showRowVerify = "";
		          if ($(this).getRowData(cl).verify == 'true') {
		              showRowVerify = "OK";
		          } else {
		              showRowVerify = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { verify: showRowVerify });

		          var showRowApproved = "";
		          if ($(this).getRowData(cl).approved == 'true') {
		              showRowApproved = "OK";
		          } else {
		              showRowApproved = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { approved: showRowApproved });
		      }
		  },
        width: $("#paymentvoucher_toolbar").width(),
        height: $(window).height() - 120
    });//END GRID
    $("#list_paymentvoucher").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $("#list_paymentvoucher").jqGrid('bindKeys', {
        onEnter: function (rowid) {
            $('#paymentvoucher_btn_edit').click();
        },
        scrollingRows: true
    });

    /*================================================ END List ================================================*/

    $("#paymentvoucher_btn_reload").click(function () {
        ReloadGrid();
    });

    $("#jobtype").change(function () {
        ReloadGrid();
    });

    $("#paymentvoucher_btn_add_new").click(function () {

        //$.messager.confirm('Confirm', 'Are you sure you want to create New Payment Voucher?', function (r) {
        //    if (r) {
        //        $.ajax({
        //            dataType: "json",
        //            url: base_url + "CashSettlement/IsAllowNew",
        //            success: function (result) {
        //                if (!result.isValid) {
        //                    $.messager.alert('Information', result.message, 'info');
        //                }
        //                else {
                            window.location = base_url + "CashSettlement/Detail";
        //                }
        //            }
        //        });
        //    }
        //});
    });


    $("#paymentvoucher_btn_edit, #paymentvoucher_btn_del").click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        var buttonID = $(this).attr('id');

        var pvid = jQuery("#list_paymentvoucher").jqGrid('getGridParam', 'selrow');
        if (pvid) {
            var ret = jQuery("#list_paymentvoucher").jqGrid('getRowData', pvid);
            if (ret.deleted != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }

            // On Edit
            if (buttonID == "paymentvoucher_btn_edit") {
                window.location = base_url + "CashSettlement/Detail?Id=" + pvid + "&JobId=" + value;
            }
                // On Delete
            else {
                DeleteCashSettlement(pvid);
            }

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $("#paymentvoucher_btn_print").click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        var buttonID = $(this).attr('id');

        var pvId = jQuery("#list_paymentvoucher").jqGrid('getGridParam', 'selrow');
        if (pvId) {
            var ret = jQuery("#list_paymentvoucher").jqGrid('getRowData', pvId);
            PrintCashSettlement(pvId);
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    function DeleteCashSettlement(pvId) {
        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "CashSettlement/Delete",
                    data: JSON.stringify({
                        Id: pvId
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

    function PrintCashSettlement(pvId) {
        window.open(base_url + "CashSettlement/Print?Id=" + pvId);
    }

    //Confirm Invoice
    $('#btn_confirm').click(function () {
        var id = jQuery("#list_paymentvoucher").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_paymentvoucher").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to confirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "CashSettlement/Confirm",
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
        var id = jQuery("#list_paymentvoucher").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_paymentvoucher").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unconfirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "CashSettlement/Unconfirm",
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

});