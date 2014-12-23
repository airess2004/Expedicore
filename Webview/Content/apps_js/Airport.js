$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Airport/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_city").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Airport/GetList',
        datatype: "json",
        colNames: ['Del','ID', 'Int. Code', 'Airport Name', 'City Name', 'Country Name', 'Continent Name', 'Created At', 'Updated At'],
        colModel: [
		          { name: 'IsDeleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
    			  { name: 'id', index: 'MasterCode', width: 80, align: "center" },
				  { name: 'Abbrevation', index: 'Abbrevation', width: 130, align: "center" },
    			  { name: 'Name', index: 'Name', width: 130, align: "center" },
                  { name: 'CityName', index: 'CityName', width: 130, align: "center" },
                  { name: 'CountryName', index: 'CountryName', width: 130, align: "center" },
    			  { name: 'ContinentName', index: 'ContinentName', width: 130, align: "center" },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'MasterCode',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowDel = $(this).getRowData(cl).IsDeleted;
		          if (rowDel == 'true') {
		              img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		          } else {
		              img = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { IsDeleted: img });
		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "Airport/GetInfo?Id=" + id,
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
                            $("#form_btn_save").data('kode', id);
                            $('#id').val(result.MasterCode);
                            $('#Name').val(result.Name);
                            $('#Abbrevation').val(result.Abbrevation);
                            $('#CityCode').val(result.CityAbbrevation).data('kode', result.CityId);
                            $('#City').val(result.CityName);
                            $('#CountryCode').val(result.CountryAbbrevation);
                            $('#Country').val(result.CountryName);
                            $('#ContinentCode').val(result.ContinentAbbrevation);
                            $('#Continent').val(result.ContinentName);
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#delete_confirm_btn_submit').data('Id', ret.id);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "Airport/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
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
                    $("#delete_confirm_div").dialog('close');
                }
                else {
                    ReloadGrid();
                    $("#delete_confirm_div").dialog('close');
                }
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {



        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'Airport/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'Airport/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(), Abbrevation: $("#Abbrevation").val(), CityLocationId: $("#CityCode").data('kode'),
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
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
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    function clearForm(form) {

        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
        });
    }


    // -------------------------------------------------------Look Up city-------------------------------------------------------
    $('#btnCity').click(function () {
        var lookUpURL = base_url + 'CityLocation/GetList';
        var lookupGrid = $('#lookup_table_city');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_city').dialog('open');
    });

    jQuery("#lookup_table_city").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Int. Code', 'City Name', 'Country Name', 'Continent Name', 'Created At', 'Updated At','CountryAbbrevation','ContinentAbbrevation'],
        colModel: [
    			  { name: 'id', index: 'MasterCode', width: 80, align: "center" },
				  { name: 'Abbrevation', index: 'Abbrevation', width: 130, align: "center" },
    			  { name: 'Name', index: 'Name', width: 130, align: "center" },
                  { name: 'CountryName', index: 'CountryName', width: 130, align: "center" },
    			  { name: 'ContinentName', index: 'ContinentName', width: 130, align: "center" },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'CountryAbbrevation', index: 'CountryAbbrevation', width: 130, align: "center" ,hidden : true },
    			  { name: 'ContinentAbbrevation', index: 'ContinentAbbrevation', width: 130, align: "center", hidden: true }
        ],
        page: '1',
        pager: $('#lookup_pager_city'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'MasterCode',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_city").width() - 10,
        height: $("#lookup_div_city").height() - 110,
    });
    $("#lookup_table_city").jqGrid('navGrid', '#lookup_toolbar_city', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_city').click(function () {
        $('#lookup_div_city').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_city').click(function () {
        var id = jQuery("#lookup_table_city").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_city").jqGrid('getRowData', id);

            $('#CityCode').val(ret.Abbrevation).data("kode", id);
            $('#City').val(ret.Name);
            $('#CountryCode').val(ret.CountryAbbrevation);
            $('#Country').val(ret.CountryName);
            $('#ContinentCode').val(ret.ContinentAbbrevation);
            $('#Continent').val(ret.ContinentName);
            $('#lookup_div_city').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup city----------------------------------------------------------------

}); //END DOCUMENT READY