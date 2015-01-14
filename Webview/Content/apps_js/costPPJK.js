var vStatusSaving,//Status Saving data if its new or edit
    vMainGrid,
    vListMaster,
    vCode;

function showData(ret) {
}


function clearForm(form) {
    $(':input', form).each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase();
        if (type == 'text' || type == 'password' || tag == 'textarea') {
            this.value = "";
        }
        else if (type == 'checkbox' || type == 'radio')
            this.checked = false;
        else if (tag == 'select')
            this.selectedIndex = 0;
        this.disabled = "disabled";
    }
	);
}



$(function () {
    clearForm('#frmaddmaster');
    $("#dialog_addmaster").dialog("close");
    $("#delete_confirm_div").dialog("close");

    vMainGrid = $("#list_CostSalesAir");
    vMainGrid.jqGrid({
        url: base_url + 'cost/getListPPJK',
        datatype: "json",
        colNames: ['Del', 'ID', 'Account', 'Charge USD','Charge IDR', 'Create At', 'Update At', '', ''],
        colModel: [
			{ name: 'deleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
			{ name: 'AccountID', index: 'MasterCode', width: 20, align: "center" },
			{ name: 'AccountName', index: 'Name', width: 50, align: "left" },
      	    { name: 'ChargeUSD', index: 'ChargeUSD', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
		    { name: 'ChargeIDR', index: 'ChargeIDR', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
			{ name: 'EntryDate', index: 'EntryDate', width: 50, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "d/m/Y" }, searchoptions: { dataInit: function (el) { $(el).datepicker({ changeMonth: true, changeYear: true, dateFormat: 'yy-mm-dd' }); } } },
			{ name: 'Remarks', index: 'Remarks', hidden: true },
			{ name: 'MinChargeUSD', index: 'MinChargeUSD', hidden: true },
			{ name: 'MinChargeIDR', index: 'MinChargeIDR', hidden: true }
        ],
        pager: $('#pager_CostSalesAir'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        scrollrows: true,
        viewrecords: true,
        shrinkToFit: true,
        sortname: 'id',
        sortorder: "ASC",
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
		      }
		  },
        width: $('#mst_CostSalesAir_toolbar').width(),
        height: $(window).height() - 150
    });//END GRID

    vMainGrid.jqGrid('navGrid', '#toolbar_CostSalesAir', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#mst_CostSalesAir_btn_add_new').click(function () {
        clearForm('#frmaddmaster');
        vStatusSaving = 0; //add data mode	    
        $('.editable').attr("disabled", false);
        $('input[name=quantity],input[name=cbm],select[name=combomst_aktif]').attr("disabled", false);
        $('#dialog_addmaster').dialog('open');
        $('#txtmastername').focus();
    });

    $('#mst_CostSalesAir_btn_edit').click(function () {
        vStatusSaving = 1; //add edit mode	
        var id = vMainGrid.jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_CostSalesAir").jqGrid('getRowData', id);
            if (ret.deleted != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "Cost/GetInfo?Id=" + id,
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
                            $('#txtmastercode').val(result.MasterCode).data('kode', id);
                            $('#txtmastername').val(result.Name).attr("disabled", false);


                            $('#txtremarks').val(result.Remarks).attr("disabled", false);
                            $('#txtminimumusd').numberbox('setValue', result.ChargeUSD).attr("disabled", false);
                            $('#txtminimumidr').numberbox('setValue', result.ChargeIDR).attr("disabled", false);

                            $('input[name=quantity],input[name=cbm]').attr("disabled", false);
                            $('#dialog_addmaster').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#mst_CostSalesAir_btn_del').click(function () {
        clearForm("#frmaddmaster");

        var id = vMainGrid.jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = vMainGrid.jqGrid('getRowData', id);
            $('#delete_confirm_id').val(id);
            $('#delete_confirm_name').text(ret.AccountName);

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
            url: base_url + "Cost/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Id: $('#delete_confirm_id').val() }),
            success: function (result) {
                $.messager.alert('Result', result, 'info');
                //reload

                $("#list_CostSalesAir").setGridParam({ url: base_url + 'Cost/GetListPPJK' }).trigger("reloadGrid");
            }
        });

    });


    $("#mst_CostSalesAir_form_btn_cancel").click(function () {
        clearForm('frmaddmaster');
        $('#dialog_addmaster').dialog('close');
    });

    $("#mst_CostSalesAir_form_btn_save").click(function () {
        var v_accountid,
			v_accountname = $('#txtmastername').val(),
			v_edit = vStatusSaving;

        var remarks = $('#txtremarks').val();
        var minchargeusd = $('#txtminimumusd').numberbox('getValue');
        var minchargeidr = $('#txtminimumidr').numberbox('getValue');

        var accountId = 0;
        var submitUrl = "";
        // Insert
        if (vStatusSaving == 0)
            submitUrl = base_url + "cost/InsertCostPPJK";
            // Update
        else if (vStatusSaving == 1) {
            accountId = $('#txtmastercode').data('kode');
            submitUrl = base_url + "cost/Update";
        }


        $.ajax({
            url: submitUrl,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: accountId, Name: v_accountname,
                ChargeUSD: minchargeusd, ChargeIDR: minchargeidr, Remarks: remarks
            }),
            success: function (result) {
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
                    $("#list_CostSalesAir").setGridParam({ url: base_url + 'Cost/GetListPPJK' }).trigger("reloadGrid");
                    $('#dialog_addmaster').dialog('close');
                }
            }
        });

    });

    $('#mst_CostSalesAir_btn_reload').click(function () {
        $("#list_CostSalesAir").setGridParam({ url: base_url + 'Cost/GetListPPJK' }).trigger("reloadGrid");
    });

}); //END DOCUMENT READY