$(document).ready(function () {

    $('#find_shipmentorder').dialog('close');
    $("#form_div").dialog('close');
    $('#item_div').dialog('close');
    $('#confirm_div').dialog('close');
    
    var type = getQueryStringByName('type');
    if (type == undefined || type == "") {
        type = '1';
    }
    $('#jobtype').val(type);

    function ReloadGrid() {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();

        $("#list_shipment").setGridParam({ url: base_url + 'shipmentorder/GetShipmentOrderList', postData: { filters: null, JobId: value }, page: 'last' }).trigger("reloadGrid");
    }

    /*================================================ Shipment ================================================*/
    jQuery("#list_shipment").jqGrid({
        url: base_url + 'shipmentorder/GetList',
        postData: { 'JobId': function () { return $("#jobtype").val(); } },
        datatype: "json",
        colNames: ['Del', 'Shipment', 'Load', 
				'Shipper Name', 'Agent Name', 'Consignee Name', 'ETD', 'ETA', 'Vessel',
				 'Delivery P.N', 'HBL / HAWB', 'OBL / MAWB',
				'Total Sub', 'Close', 'Entry Date', 'User'],
        colModel: [{ name: 'deleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
				  { name: 'shipmentno', index: 'ShipmentOrderId', width: 130, align: "center" },
				  { name: 'loadstatus', index: 'loadstatus', width: 60, align: "center" },
				  { name: 'shippername', index: 'shippername', width: 200 },
				  { name: 'agentname', index: 'agentname', width: 250 },
				  { name: 'consigneename', index: 'ConsigneeName', width: 200 },
				  { name: 'etd', index: 'etd', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'eta', index: 'eta', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'firstfeeder', index: 'Vessel', width: 200, search: false, sortable: false },
				  { name: 'destination', index: 'DeliveryPlaceName', width: 200 },
				  { name: 'hbl', index: 'HouseBLNo', width: 150 },
				  { name: 'obl', index: 'SecondBLNo', width: 150 },
				  { name: 'totalsub', index: 'TotalSub', width: 80, align: "right" },
				  { name: 'jobclose', index: 'JobClosed', width: 60, align: "center" },
				  { name: 'entrydate', index: 'CreatedAt', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'usercode', index: 'CreateBy', width: 100 }
        ],
        page: 'last', // last page
        pager: jQuery('#pager_list_shipment'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'ShipmentOrderId',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "asc",
        width: $("#so_toolbar").width(),
        height: $(window).height() - 120,
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
		          rowJClose = $(this).getRowData(cl).jobclose;
		          if (rowJClose == 'true') {
		              img = "YES";
		          } else {
		              img = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { jobclose: img });
		      }
		  }
    });//END GRID
    $("#list_shipment").jqGrid('navGrid', '#toolbar_trans_shipmentorder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    /*================================================ End Shipment ================================================*/

    $("#so_btn_reload").click(function () {
        ReloadGrid();
    });

    $("#so_btn_add_new").click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        $.messager.confirm('Confirm', 'Add Shipment will automatically create a New Job (' + id + ')! \n Are you sure?', function (r) {
            if (r) {

                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        JobId: value
                    }),
                    url: base_url + "shipmentorder/Insert",
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
                            var shipmentId = result.shipmentId;
                            var jobNumber = result.jobNumber;
                            var subJobNumber = result.subJobNumber;

                            window.location = base_url + "shipmentorder/detail?Id=" + shipmentId + "&JobId=" + value;
                        }
                    }
                });
            }
        });
    });

    $("#so_btn_edit, #so_btn_del").click(function () {
        var buttonID = $(this).attr('id');
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();

        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            if (ret.deleted != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }

            // On Edit Mode
            if (buttonID == "so_btn_edit") {
                // shipmentorder_se.js
                window.location = base_url + "shipmentorder/detail?Id=" + id + "&JobId=" + value;
            }
                // On Delete Mode
            else if (buttonID == "so_btn_del") {
                DeleteShipmentOrder(id);
            }
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }

    });

    $("#jobtype").live("change", function () {
        ReloadGrid();
    });

    function DeleteShipmentOrder(shipmentId) {

        $.messager.confirm('Confirm', 'Are you sure you want to delete the selected record?', function (r) {
            if (r) {

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "shipmentorder/Delete",
                    data: JSON.stringify({
                        shipmentId: shipmentId
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

    // ************************************************************** Find / Search **************************************************************
    var today = new Date();
    $('#findso_rangefrom').datebox('setValue', dateFormat(today, 'MM/DD/YYYY'));
    $('#findso_rangeto').datebox('setValue', dateFormat(today, 'MM/DD/YYYY'));

    $('#findso_allrange').click(function () {
        if ($(this).is(':checked')) {
            $('#pnlrange').hide();
        }
        else
            $('#pnlrange').show();
    });

    // Ctrl + F
    Mousetrap.bind('ctrl+alt+f', function (e) {
        // ctrl + f key pressed
        $('#findso_containerno').val('');
        $('#findso_feedervessel').val('');
        $('#findso_housebl').val('');
        $('#findso_allrange').attr('checked', 'checked');
        $('#pnlrange').hide();
        // Clear Grid
        $("#find_shipmentderlist").jqGrid("clearGridData", true);
        $('#find_shipmentorder').dialog('open');
    });

    $("#so_btn_find").click(function () {
        $('#findso_containerno').val('');
        $('#findso_feedervessel').val('');
        $('#findso_housebl').val('');
        $('#findso_masterbl').val('');
        $('#findso_allrange').attr('checked', 'checked');
        $('#pnlrange').hide();
        // Clear Grid
        $("#find_shipmentderlist").jqGrid("clearGridData", true);
        $('#find_shipmentorder').dialog('open');
    });

    $("#find_shipmentorder_btn_close").click(function () {

        $('#find_shipmentorder').dialog('close');
    });

    $("#find_shipmentorder_btn_report").click(function () {
        var rowcount = $("#find_shipmentderlist").getGridParam("reccount");
        if (rowcount < 1) {
            $.messager.alert('Information', 'No record to view...!!', 'info');
        }
        else {

            //var Job = "";
            //if ($('#seaexport').is(":checked"))
            //    Job = "SeaExport";
            //else if ($('#airexport').is(":checked"))
            //    Job = "AirExport";
            //else if ($('#seaimport').is(":checked"))
            //    Job = "SeaImport";
            //else if ($('#airimport').is(":checked"))
            //    Job = "AirImport";
            var Job = $('#jobtype').val();
            var containerno = $.trim($('#findso_containerno').val());
            var vessel = $.trim($('#findso_feedervessel').val());
            var hbl = $.trim($('#findso_housebl').val());
            var obl = $.trim($('#findso_masterbl').val());
            var isall = false;
            if ($('#findso_allrange').is(':checked')) {
                isall = true;
            }
            var datefrom = $('#findso_rangefrom').datebox('getValue');
            var dateto = $('#findso_rangeto').datebox('getValue');

            window.open(base_url + "ShipmentOrder/PrintShipmentOrderReport?Job=" + Job + "&containerno=" + containerno + "&vessel=" + vessel + "&hbl=" + hbl + "&obl=" + obl +
                                "&isall=" + isall + "&datefrom=" + datefrom + "&dateto=" + dateto);
        }
    });

    $("#find_shipmentorder_btn_report_excel").click(function () {
        var rowcount = $("#find_shipmentderlist").getGridParam("reccount");
        if (rowcount < 1) {
            $.messager.alert('Information', 'No record to view...!!', 'info');
        }
        else {

            //var Job = "";
            //if ($('#seaexport').is(":checked"))
            //    Job = "SeaExport";
            //else if ($('#airexport').is(":checked"))
            //    Job = "AirExport";
            //else if ($('#seaimport').is(":checked"))
            //    Job = "SeaImport";
            //else if ($('#airimport').is(":checked"))
            //    Job = "AirImport";
            var Job = $('#jobtype').val();
            var containerno = $.trim($('#findso_containerno').val());
            var vessel = $.trim($('#findso_feedervessel').val());
            var hbl = $.trim($('#findso_housebl').val());
            var obl = $.trim($('#findso_masterbl').val());
            var isall = false;
            if ($('#findso_allrange').is(':checked')) {
                isall = true;
            }
            var datefrom = $('#findso_rangefrom').datebox('getValue');
            var dateto = $('#findso_rangeto').datebox('getValue');

            window.open(base_url + "ShipmentOrder/PrintShipmentOrderReportExcel?Job=" + Job + "&containerno=" + containerno + "&vessel=" + vessel + "&hbl=" + hbl + "&obl=" + obl +
                                "&isall=" + isall + "&datefrom=" + datefrom + "&dateto=" + dateto);
        }
    });

    $("#find_shipmentorder_btn_find").click(function () {
        var Job = $('#jobtype').val();

        var containerno = $.trim($('#findso_containerno').val());
        var vessel = $.trim($('#findso_feedervessel').val());
        var hbl = $.trim($('#findso_housebl').val());
        var obl = $.trim($('#findso_masterbl').val());
        var isall = false;
        if ($('#findso_allrange').is(':checked')) {
            isall = true;
        }
        var datefrom = $('#findso_rangefrom').datebox('getValue');
        var dateto = $('#findso_rangeto').datebox('getValue');

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        $("#find_shipmentderlist").setGridParam({
            url: base_url + 'ShipmentOrder/Find',
            postData: { filters: null, job: Job, containerno: containerno, vessel: vessel, hbl: hbl, obl: obl, isall: isall, datefrom: datefrom, dateto: dateto }, page: 'last'
        }).trigger("reloadGrid");

    });

    jQuery("#find_shipmentderlist").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Shipment', 'Principle By', 'Container', 'Vessel / Flight', 'ETD / ETA', 'HouseBL / HAWB', 'MasterBL / MAWB', 'Agent Name', 'Shipper Name', 'Consignee Name'],
        colModel: [
            { name: 'shipmentno', index: 'jobnumber', width: 120, align: "center" },
				  { name: 'jobowner', index: 'jobownerid', width: 80, align: "center" },
				  { name: 'containerno', index: 'containerno', width: 210 },
				  { name: 'vessel', index: 'vessel', width: 160 },
				  { name: 'eta', index: 'etdeta', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'hbl', index: 'hbl', width: 160 },
				  { name: 'obl', index: 'obl', width: 160 },
				  { name: 'agentname', index: 'agentname', width: 200 },
				  { name: 'shippername', index: 'shippername', width: 200 },
				  { name: 'consigneename', index: 'ConsigneeName', width: 200 }
        ],
        page: 'last', // last page
        pager: jQuery('#pager_find_shipmentderlist'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'jobnumber',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "asc",
        width: 550,
        height: 250,
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
		          rowJClose = $(this).getRowData(cl).jobclose;
		          if (rowJClose == 'true') {
		              img = "YES";
		          } else {
		              img = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { jobclose: img });
		      }
		  }
    });//END GRID
    //$("#find_shipmentderlist").jqGrid('navGrid', '#toolbar_trans_shipmentorder', { del: false, add: false, edit: false, search: false })
    //       .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // ************************************************************** END Find / Search **************************************************************

    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Doc.Name', 'Desciption','Submit Date','CreatedDate','CreatedBy'
        ],
        colModel: [
                  { name: 'code', index: 'DocumentName', width: 70, sortable: false },
                  { name: 'description', index: 'description', width: 180, sortable: false },
				  { name: 'submitdate', index: 'SubmitDate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'entrydate', index: 'CreatedAt', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'createdby', index: 'CreatedBy', width: 180, sortable: false },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'DocumentName',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    function ClearData() {
        $('#form_btn_save').data('kode', '');
        $('#item_btn_submit').data('kode', '');
        ClearErrorMessage();
    }

    function clearForm(form) {

        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea') {
                this.value = "";
            }
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
            if ($(this).hasClass('easyui-numberbox'))
                $(this).numberbox('clear');
        });
    }

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            ClearData();
            clearForm('#item_div');
            $("#DocumentName").val(ret.code);
            $("#Description").val(ret.description);
            $("#item_btn_submit").data('kode',id);
            $('#item_div').dialog('open');

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del_detail').click(function () {
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "ShipmentOrder/DeleteDocument",
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
                                ReloadGridDetail();
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
    });


    $('#btn_document').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            if (ret.deleted != '') {
                $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
                return;
            }
            ClearData();
            clearForm('#item_div');
            $('#id').val(id);
            $('#Code').val(ret.shipmentno);
            ReloadGridDetail();
            $('#form_div').dialog('open');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ShipmentOrder/UpdateDocument';
        }
            // Insert
        else {
            submitURL = base_url + 'ShipmentOrder/InsertDocument';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ShipmentOrderId: $("#id").val(), DocumentName: $("#DocumentName").val(), Description: $("#Description").val(),
                SubmitDate: $("#submitdate").datebox('getValue')
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
                    ReloadGridDetail();
                    $("#item_div").dialog('close')
                }
            }
        });
    });

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'ShipmentOrder/GetListDocument?ShipmentOrderId=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }


    // item_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });

    $('#confirm_btn_cancel').click(function () {
        $('#confirm_div').dialog('close');
    });

    $('#btn_SPPB').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "SPPB";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode', result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_DOKORI').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "DOK ORI";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode',result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                  }
              });
           
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_STRIPPING').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "STRIPPING";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode',result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });
           
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_SP2').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "BUAT SP2";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode', result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_H3').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "H+3";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode', result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_PBM').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "PBM";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode', result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_MUAT').click(function () {
        var id = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        var job = "MUAT";
        if (id) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', id);
            $.ajax({
                dataType: "json",
                url: base_url + "ShipmentOrder/GetInfoDocument?Id=" + id + "&Name=" + job,
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
                        $('#idconfirm').val(id).data('kode', result.id);
                        $('#NoJobCode').val(ret.shipmentno);
                        $('#Job').val(job);
                        $('#TimeCheck').datebox('setValue', result.SubmitDate);
                        $("#confirm_div").dialog("open");
                    }
                }
            });

        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#confirm_btn_submit').click(function () {
        ClearErrorMessage();

        var submitURL = '';
        var id = $("#idconfirm").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ShipmentOrder/UpdateDocument';
        }
            // Insert
        else {
            submitURL = base_url + 'ShipmentOrder/InsertDocument';
        }

        $.ajax({
            url: submitURL,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: id, DocumentName: $("#Job").val(), IsLegacy: true, ShipmentOrderId: $('#idconfirm').val(),
                SubmitDate: $('#TimeCheck').datebox('getValue')
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
                    $("#confirm_div").dialog('close');
                }
            }
        });
    });

    // Print Payment Voucher
    $('#btn_print').click(function () {
        var id = $('#jobtype option:selected').text();
        var value = $('#jobtype').val();
        var buttonID = $(this).attr('id');

        var pvId = jQuery("#list_shipment").jqGrid('getGridParam', 'selrow');
        if (pvId) {
            var ret = jQuery("#list_shipment").jqGrid('getRowData', pvId);
            PrintPaymentVoucher(pvId);
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
        //        }
        //    }
        //});
    });

    function PrintPaymentVoucher(pvId) {
        window.open(base_url + "ShipmentOrder/Print?Id=" + pvId);
    }

}); //END DOCUMENT READY
