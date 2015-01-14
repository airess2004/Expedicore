$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'TruckOrder/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $.datetimeEntry.setDefaults({ show24Hours: true, datetimeFormat: 'O/D/Y', spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] }); // , spinnerBigImage: 'Content/spinnerTextBig.png', spinnerBigSize: [60, 40, 16]
    $('#TanggalSPPB').datetimeEntry();
    $('#TanggalOrder').datetimeEntry();
    $('#TglLoloBayar').datetimeEntry();
    $('#TimeCheck').datetimeEntry({ show24Hours: true, datetimeFormat: 'O/D/Y H:M', spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] });

    $('#btn_BOGASARI').hide();
    $('#btn_CYMILL').hide();
    $('#btn_DIMILL').hide();
    $('#btn_OTWKEPRIOK').hide();
    $('#btn_ADATOLCIUJUNG').hide();
    $('#btn_JORR').hide();


    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#finish_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close');
    $("#lookup_div_containeryard").dialog('close');
    $("#lookup_div_depo").dialog('close');
    $("#lookup_div_truck").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#lookup_div_shipmentorder").dialog('close');
    $("#confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'TruckOrder/GetList',
        datatype: "json",
        colNames: ['IsFinished', 'ID', 'NoJob', 'TanggalSPPB', 'TanggalOrder', 'AGENDATRUCK'
                    , 'PICORDER', 'Customer', 'AGENDAMTP', 'InvoiceShipper', 'Party', 'PIC'
                    , 'Truck', 'Employee', 'NoContainer' , 'ContainerYard', 'Tujuan'
                    , 'Depo', 'TanggalLoloBayar','InvoiceNo','UJ', 'CYIN', 'CYOUT', 'MILLIN'
                    , 'REGIN', 'REGOUT', 'MILLOUT', 'BOGASARI', 'DEPOIN', 'DEPOOUT', 'GARASI'
                    , 'CYMILL', 'DIMILL', 'OTWKEPRIOK', 'ADATOLCIUJUNG', 'JORR', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'IsFinished', index: 'IsFinished', width: 80, align: "center", stype: 'select', editoptions: { value: ":;false:No;true:Yes" } },
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'nojob', index: 'nojob', width: 180 },
            	  { name: 'tanggalsppb', index: 'tanggalsppb', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y' } },
            	  { name: 'tanggalorder', index: 'tanggalorder', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y' } },
                  { name: 'agendatruck', index: 'agendatruck', width: 180 },
                  { name: 'picorder', index: 'picorder', width: 180 },
                  { name: 'contact', index: 'contact', width: 180 },
                  { name: 'agendamtp', index: 'agendamtp', width: 180 },
                  { name: 'invoiceshipper', index: 'invoiceshipper', width: 180 },
                  { name: 'party', index: 'party', width: 180 },
                  { name: 'pic', index: 'pic', width: 180 },
                  { name: 'truck', index: 'truck', width: 180 },
                  { name: 'employee', index: 'employee', width: 180 },
                  { name: 'nocontainer', index: 'nocontainer', width: 180 },
                  { name: 'containeryard', index: 'containeryard', width: 180 },
                  { name: 'tujuan', index: 'tujuan', width: 180 },
                  { name: 'depo', index: 'depo', width: 180 },
            	  { name: 'tanggallolobayar', index: 'tanggallolobayar', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y' } },
                  { name: 'invoiceno', index: 'invoiceno', width: 180 },
                  { name: 'uj', index: 'uj', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'cyin', index: 'cyin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'cyout', index: 'cyout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'millin', index: 'millin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'regin', index: 'regin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'regout', index: 'regout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'millout', index: 'millout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'bogasari', index: 'bogasari', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
                  { name: 'depoin', index: 'depoin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'depoout', index: 'depoout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'garasi', index: 'garasi', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' } },
                  { name: 'cymill', index: 'cymill', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
                  { name: 'dimill', index: 'dimill', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
                  { name: 'otwkepriok', index: 'otwkepriok', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
                  { name: 'adatolciujung', index: 'adatolciujung', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
                  { name: 'jorr', index: 'jorr', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y H:i' }, hidden: true },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'd/m/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
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
	              rowIsConfirmed = $(this).getRowData(cl).IsFinished;
	              if (rowIsConfirmed == 'true') {
	                  rowIsConfirmed = "YES";
	              } else {
	                  rowIsConfirmed = "NO";
	              }
	              $(this).jqGrid('setRowData', ids[i], { IsFinished: rowIsConfirmed });
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
                url: base_url + "TruckOrder/GetInfo?Id=" + id,
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
                            $('#id').val(result.Id);
                            $('#NoJob').val(result.NoJob);
                            $('#NoContainer').val(result.NoContainer);
                            $('#Party').val(result.Party);
                            $('#InvoiceNo').val(result.InvoiceNo);
                            $('#InvoiceShipper').val(result.InvoiceShipper);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#TruckId').val(result.TruckId);
                            $('#Truck').val(result.Truck);
                            $('#Tujuan').val(result.Tujuan);
                            $('#Employee').val(result.Employee);
                            $('#EmployeeId').val(result.EmployeeId);
                            $('#TanggalSPPB').val(dateTimeEnt(result.TanggalSPPB));
                            $('#TanggalOrder').val(dateTimeEnt(result.TanggalOrder));
                            $('#TglLoloBayar').val(dateTimeEnt(result.TglLoloBayar));
                            $('#PIC').val(result.PIC);
                            $('#PICOrder').val(result.PICOrder);
                            $('#AgendaMTP').val(result.AgendaMTP);
                            $('#AgendaTruck').val(result.AgendaTruck);
                            $('#ContainerYardId').val(result.ContainerYardId);
                            $('#ContainerYard').val(result.ContainerYard);
                            $('#DepoId').val(result.DepoId);
                            $('#Depo').val(result.Depo);
                            $('#Tujuan').val(result.Tujuan);
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
            url: base_url + "TruckOrder/Delete",
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

    $('#btn_finish').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#finish_confirm_btn_submit').data('Id', ret.id);
            $("#finish_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#finish_confirm_btn_cancel').click(function () {
        $('#finish_confirm_btn_submit').val('');
        $("#finish_confirm_div").dialog('close');
    });

    $('#finish_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "TruckOrder/Finish",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#finish_confirm_btn_submit').data('Id'),
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
                    $("#finish_confirm_div").dialog('close');
                }
                else {
                    ReloadGrid();
                    $("#finish_confirm_div").dialog('close');
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
            submitURL = base_url + 'TruckOrder/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'TruckOrder/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(),
                NoJob: $('#ShipmentOrder').val(),
                ShipmentOrderId :  $('#ShipmentOrderId').val(),
                GroupEmployeeId: $("#EmployeeGroupId").val(),
                NoContainer:$('#NoContainer').val(),
                Party: $('#Party').val(),
                InvoiceNo:$('#InvoiceNo').val(),
                InvoiceShipper: $('#InvoiceShipper').val(),
                ContactId:$('#ContactId').val(),
                Contact: $('#Contact').val(),
                TruckId: $('#TruckId').val(),
                Tujuan: $('#Tujuan').val(),
                EmployeeId:$('#EmployeeId').val(),
                TanggalSPPB: $('#TanggalSPPB').val(),
                TanggalOrder: $('#TanggalOrder').val(),
                TglLoloBayar: $('#TglLoloBayar').val(),
                PIC:$('#PIC').val(),
                PICOrder: $('#PICOrder').val(),
                AgendaMTP: $('#AgendaMTP').val(),
                AgendaTruck: $('#AgendaTruck').val(),
                ContainerYardId: $('#ContainerYardId').val(),
                ContainerYard: $('#ContainerYard').val(),
                DepoId: $('#DepoId').val(),
                Tujuan: $('#Tujuan').val(),
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



    $('#btn_UJ').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("UJ");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_CYIN').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("CYIN");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_CYOUT').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("CYOUT");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_MILLIN').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("MILLIN");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_REGIN').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("REGIN");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_MILLOUT').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("MILLOUT");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_BOGASARI').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("BOGASARI");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_DEPOIN').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("DEPOIN");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_DEPOOUT').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("DEPOOUT");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_GARASI').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("GARASI");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_REGOUT').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("REGOUT");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_CYMILL').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("CYMILL");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_DIMILL').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("DIMILL");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    
    $('#btn_OTWKEPRIOK').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("OTWKEPRIOK");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_ADATOLCIUJUNG').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("ADATOLCIUJUNG");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_JORR').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#idconfirm').val(ret.id);
            $('#NoJobCode').val(ret.id);
            $('#Job').val("JORR");
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    $('#confirm_btn_submit').click(function () {
        ClearErrorMessage();
        $.ajax({
            url: base_url + "TruckOrder/Confirm?job=" + $("#Job").val() + "&id=" + $("#idconfirm").val() + "&checkin=" + $('#TimeCheck').val(),
            type: "POST",
            contentType: "application/json",
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
                    $("#confirm_div").dialog('close');
                }
            }
        });
    });

    $('#confirm_btn_cancel').click(function () {
        $('#confirm_div').dialog('close');
    });


    // -------------------------------------------------------Look Up employee-------------------------------------------------------
    $('#btnEmployee').click(function () {
        var lookUpURL = base_url + 'MstEmployee/GetList';
        var lookupGrid = $('#lookup_table_employee');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_employee').dialog('open');
    });

    jQuery("#lookup_table_employee").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_employee'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_employee").width() - 10,
        height: $("#lookup_div_employee").height() - 110,
    });
    $("#lookup_table_employee").jqGrid('navGrid', '#lookup_toolbar_employee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_employee').click(function () {
        $('#lookup_div_employee').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_employee').click(function () {
        var id = jQuery("#lookup_table_employee").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_employee").jqGrid('getRowData', id);

            $('#EmployeeId').val(ret.id).data("kode", id);
            $('#Employee').val(ret.name);

            $('#lookup_div_employee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup employee----------------------------------------------------------------

    // -------------------------------------------------------Look Up contact-------------------------------------------------------
    $('#btnContact').click(function () {
        var lookUpURL = base_url + 'MstContact/GetLookUp';
        var lookupGrid = $('#lookup_table_contact');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_contact').dialog('open');
    });

    jQuery("#lookup_table_contact").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Contact Status', 'Contact Name', 'Contact As', 'Address'],
        colModel: [{ name: 'code', index: 'MasterCode', width: 80, align: 'right' },
                  { name: 'status', index: 'ContactStatus', width: 200 },
                  { name: 'name', index: 'ContactName', width: 200 },
                  { name: 'as', index: 'ContactAs', width: 200 },
                  { name: 'address', index: 'ContactAddress', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_contact'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'MasterCode',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_contact").width() - 10,
        height: $("#lookup_div_contact").height() - 110,
    });
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_contact').click(function () {
        $('#lookup_div_contact').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_contact').click(function () {
        var id = jQuery("#lookup_table_contact").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_contact").jqGrid('getRowData', id);

            $('#ContactId').val(id).data("kode", id);
            $('#Contact').val(ret.name);

            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up shipmentorder-------------------------------------------------------
    $('#btnShipmentOrder').click(function () {
        var lookUpURL = base_url + 'ShipmentOrder/GetLookUp';
        var lookupGrid = $('#lookup_table_shipmentorder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_shipmentorder').dialog('open');
    });

    jQuery("#lookup_table_shipmentorder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Shipment Order', 'ETD', 'Consignee', 'Agent', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        colModel: [{ name: 'shipmentorderno', index: 'ShipmentOrderId', width: 130, align: "center" },
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
        page: '1',
        pager: $('#lookup_pager_shipmentorder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'ShipmentOrderId',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_shipmentorder").width() - 10,
        height: $("#lookup_div_shipmentorder").height() - 110,
    });
    $("#lookup_table_shipmentorder").jqGrid('navGrid', '#lookup_toolbar_shipmentorder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_shipmentorder').click(function () {
        $('#lookup_div_shipmentorder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_shipmentorder').click(function () {
        var id = jQuery("#lookup_table_shipmentorder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_shipmentorder").jqGrid('getRowData', id);

            $('#ShipmentOrderId').val(id).data("kode", id);
            $('#ShipmentOrder').val(ret.shipmentorderno);

            $('#lookup_div_shipmentorder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup shipmentorder----------------------------------------------------------------

    // -------------------------------------------------------Look Up depo-------------------------------------------------------
    $('#btnDepo').click(function () {
        var lookUpURL = base_url + 'MstDepo/GetList';
        var lookupGrid = $('#lookup_table_depo');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_depo').dialog('open');
    });

    jQuery("#lookup_table_depo").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_depo'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_depo").width() - 10,
        height: $("#lookup_div_depo").height() - 110,
    });
    $("#lookup_table_depo").jqGrid('navGrid', '#lookup_toolbar_depo', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_depo').click(function () {
        $('#lookup_div_depo').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_depo').click(function () {
        var id = jQuery("#lookup_table_depo").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_depo").jqGrid('getRowData', id);

            $('#DepoId').val(ret.id).data("kode", id);
            $('#Depo').val(ret.name);

            $('#lookup_div_depo').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup depo----------------------------------------------------------------
    // -------------------------------------------------------Look Up containeryard-------------------------------------------------------
    $('#btnContainerYard').click(function () {
        var lookUpURL = base_url + 'MstContainerYard/GetList';
        var lookupGrid = $('#lookup_table_containeryard');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_containeryard').dialog('open');
    });

    jQuery("#lookup_table_containeryard").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_containeryard'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_containeryard").width() - 10,
        height: $("#lookup_div_containeryard").height() - 110,
    });
    $("#lookup_table_containeryard").jqGrid('navGrid', '#lookup_toolbar_containeryard', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_containeryard').click(function () {
        $('#lookup_div_containeryard').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_containeryard').click(function () {
        var id = jQuery("#lookup_table_containeryard").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_containeryard").jqGrid('getRowData', id);

            $('#ContainerYardId').val(ret.id).data("kode", id);
            $('#ContainerYard').val(ret.name);

            $('#lookup_div_containeryard').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup containeryard----------------------------------------------------------------

    // -------------------------------------------------------Look Up truck-------------------------------------------------------
    $('#btnTruck').click(function () {
        var lookUpURL = base_url + 'MstTruck/GetList';
        var lookupGrid = $('#lookup_table_truck');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_truck').dialog('open');
    });

    jQuery("#lookup_table_truck").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_truck'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_truck").width() - 10,
        height: $("#lookup_div_truck").height() - 110,
    });
    $("#lookup_table_truck").jqGrid('navGrid', '#lookup_toolbar_truck', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_truck').click(function () {
        $('#lookup_div_truck').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_truck').click(function () {
        var id = jQuery("#lookup_table_truck").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_truck").jqGrid('getRowData', id);

            $('#TruckId').val(ret.id).data("kode", id);
            $('#Truck').val(ret.name);

            $('#lookup_div_truck').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup truck----------------------------------------------------------------
}); //END DOCUMENT READY