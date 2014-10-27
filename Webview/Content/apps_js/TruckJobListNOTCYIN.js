$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'TruckOrder/GetListNOTCYIN', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'TruckOrder/GetListNOTCYIN',
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

    // ---------------------------------------------End Lookup truck----------------------------------------------------------------
}); //END DOCUMENT READY