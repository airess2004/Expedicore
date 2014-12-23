$(document).ready(function () {

    $('#find_shipmentorder').dialog('close');

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
        colNames: ['Del', 'Shipment', 'Load', 'Status', 'Principle By',
				'Shipper Name', 'Agent Name', 'Consignee Name', 'Notify Party', 'ETD', 'ETA', 'First Feeder',
				'Second Feeder', 'Delivery P.N', 'HBL / HAWB', 'OBL / MAWB',
				'Total Sub', 'Close', 'Entry Date', 'User'],
        colModel: [{ name: 'deleted', index: 'deleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
				  { name: 'shipmentno', index: 'ShipmentOrderId', width: 130, align: "center" },
				  { name: 'loadstatus', index: 'loadstatus', width: 60, align: "center" },
				  { name: 'shipmentstatus', index: 'shipmentstatus', width: 60, align: "center" },
				  { name: 'jobowner', index: 'jobownername', width: 100, align: "center" },
				  { name: 'shippername', index: 'shippername', width: 200 },
				  { name: 'agentname', index: 'agentname', width: 250 },
				  { name: 'consigneename', index: 'ConsigneeName', width: 200 },
				  { name: 'npartyname', index: 'npartyname', width: 200, hidden: true },
				  { name: 'etd', index: 'etd', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'eta', index: 'eta', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'firstfeeder', index: 'FeederVessel', width: 200, search: false, sortable: false },
				  { name: 'secondfeeder', index: 'MotherVessel', width: 200, search: false, sortable: false },
				  { name: 'destination', index: 'DeliveryPortName', width: 200 },
				  { name: 'hbl', index: 'HouseBL', width: 150 },
				  { name: 'obl', index: 'MasterBL', width: 150 },
				  { name: 'totalsub', index: 'totalsub', width: 80, align: "right" },
				  { name: 'jobclose', index: 'jobclose', width: 60, align: "center" },
				  { name: 'entrydate', index: 'createdon', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'usercode', index: 'usercode', width: 100 }
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


}); //END DOCUMENT READY
