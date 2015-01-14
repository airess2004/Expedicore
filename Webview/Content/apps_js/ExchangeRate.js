$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    $("#delete_confirm_div").dialog('close');
    $("#lookup_exchangerate_div").dialog('close');
    $("#exchangerate_form_div").dialog('close');

    //GRID LOOKUP +++++++++++++++
    $("#list_exchangerate").jqGrid({
        url: base_url + 'exchangerate/getList',
        datatype: "json",
        colNames: ['Del', 'Entry Date', 'Rate', 'Created At','Updated At'],
        colModel: [
			{ name: 'deleted', index: 'deleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
				  { name: 'ExRateDate', index: 'ExRateDate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'exrate1', index: 'exrate1', align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
				  { name: 'CreatedAt', index: 'CreatedAt', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'UpdatedAt', index: 'UpdatedAt', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager_lookup_exchangerate'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'ExRateDate',
        scrollrows: true,
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "ASC",
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowDel = $(this).getRowData(cl).deleted;
		          if (rowDel == 'true') {
		              img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		              //img = "Yes";
		          } else {
		              img = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { deleted: img });

		          $("#txtregionalid").val($(this).getRowData(cl).userregionalid);
		          $("#txtregionalname").val($(this).getRowData(cl).userregionalname);
		      }
		  },
        width: $("#exchangerate_toolbar").width(),
        height: $(window).height() - 180
    });//END GRID
    $("#list_exchangerate").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });


    $('#exchangerate_btn_reload').click(function () {
        $("#list_exchangerate").setGridParam({ url: base_url + 'exchangerate/getList' }).trigger("reloadGrid");
    });

    $('#exchangerate_btn_print').click(function () {
        window.open(base_url + 'ExchangeRate/Print');
    });

    $('#exchangerate_btn_add_new').click(function () {
        clearForm('#frm_exchangerate');

        vStatusSaving = 0; //add data mode	

        var today = new Date();
        $('#txtentrydate').datebox('setValue', dateFormat(today, 'MM/DD/YYYY'));


        $('#exchangerate_form_div').dialog('open');
    });

    $('#exchangerate_form_btn_cancel').click(function () {
        vStatusSaving = 0;

        clearForm('#frm_exchangerate');
        $("#exchangerate_form_div").dialog('close');
    });

    $('#exchangerate_btn_edit').click(function () {
        clearForm("#frm_exchangerate");
        $('#txtentrydate').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
        var id = jQuery("#list_exchangerate").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "ExchangeRate/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#exchangerate_form_btn_save').data('id', id);
                            $('#txtentrydate').datebox('setValue', dateEnt(result.ExRateDate));
                            $('#txthighrate').numberbox('setValue', result.ExRate1);
                            $("#exchangerate_form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#exchangerate_btn_del').click(function () {
        clearForm("#frm_exchangerate");

        var id = jQuery("#list_exchangerate").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_exchangerate").jqGrid('getRowData', id);
            if (ret.deletedimg != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }
            $('#delete_confirm_btn_submit').data('id', ret.id);


            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_id').val('');
        $('#delete_confirm_name').text('');

        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {
        $.ajax({
            url: base_url + "exchangerate/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                ExchangeRateId: $('#delete_confirm_btn_submit').data('id'),
            }),
            success: function (result) {
                $.messager.alert('Result', result.message, 'info', function () {
                    //reload
                    $("#list_exchangerate").setGridParam({ url: base_url + 'exchangerate/getList' }).trigger("reloadGrid");
                    $("#delete_confirm_div").dialog('close');
                });
            }
        });
    });


    $("#exchangerate_form_btn_save").click(function () {

        var id = $('#exchangerate_form_btn_save').data('id')
        var submitUrl = "";
        // Insert
        if (vStatusSaving == 0) {
            submitUrl = base_url + "exchangerate/Insert";
        }
            // Update
        else if (vStatusSaving == 1) {
            submitUrl = base_url + "exchangerate/Update";
        }

        $.ajax({
            url: submitUrl,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                ExRate1: $("#txthighrate").numberbox('getValue'), ExRateDate: $("#txtentrydate").datebox('getValue'), Id: id
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
                $("#list_exchangerate").setGridParam({ url: base_url + 'exchangerate/getList' }).trigger("reloadGrid");
                $("#exchangerate_form_div").dialog("close");
                }
            }
        });
    });

    function clearForm(form) {
        $('#txthighrate').numberbox('setValue', 0);
        $('#txtentrydate').removeAttr('disabled').addClass('ui-state-default').removeClass('ui-state-disabled');
    }


}); //END DOCUMENT READY