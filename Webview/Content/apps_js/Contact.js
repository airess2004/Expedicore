
$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

 
    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstContact/GetList' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#lookup_div_city").dialog('close');
    $("#lookup_div_airport").dialog('close');
    $("#lookup_div_port").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#print_div").dialog('close');

    

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstContact/GetList',
        datatype: "json",
        colNames: ['Del', 'Code', 'Status', 'Name', 'Contact As', 'Contact Person', 'Last Ship', 'Phone', 'Fax', 'Email', 'City', 'Port',
                 //'Marketing',
                 'Entry Date', 'Prepared By', 'Address', 'Postal Code',
                 'CityCode', 'PortCode', 'IntCity', 'IntPort', 'CountryName', 'ContinentName', 'CreditTerm', '', '', 'NPWP', 'NPPKP', 'CreditLimitIDR',
                 '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        colModel: [
			{ name: 'deleted', index: 'IsDeleted', width: 60, align: "center", sortable: false, stype: 'select', editoptions: { value: ":All;true:Yes;false:No" } },
			{ name: 'ContactCode', index: 'contactcode', width: 60, align: "center" },
			{ name: 'CompanyStatus', index: 'companystatus', width: 30, align: "center" },
			{ name: 'ContactName', index: 'contactname', width: 300 },
			{ name: 'ContactType', index: 'contacttype', width: 150 },
			{ name: 'ContactPerson', index: 'contactperson' },
			{ name: 'ShipDate', index: 'shipdate', align: "right", formatter: 'date', formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'd/m/Y' } },
			{ name: 'Phone', index: 'phone' },
			{ name: 'Fax', index: 'fax' },
			{ name: 'Email', index: 'email' },
			{ name: 'CityName', index: 'cityname' },
			{ name: 'PortName', index: 'portname' },
			{ name: 'EntryDate', index: 'EntryDate', hidden: true, width: 50, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'd/m/Y' } },
			{ name: 'UserCode', index: 'usercode', hidden: true },
			{ name: 'Address', index: 'address', hidden: true },
			{ name: 'PostalCode', index: 'postalcode', hidden: true },
			{ name: 'CityCode', index: 'citycode', hidden: true },
			{ name: 'PortCode', index: 'portcode', hidden: true },
			{ name: 'IntCity', index: 'intcity', hidden: true },
			{ name: 'IntPort', index: 'intport', hidden: true },
			{ name: 'CountryName', index: 'countryname', hidden: true },
			{ name: 'ContinentName', index: 'continentname', hidden: true },
			{ name: 'CreditTerm', index: 'creditterm', hidden: true },
			{ name: 'MarketId', index: 'userid', hidden: true },
			{ name: 'MarketName', index: 'username', hidden: true },
			{ name: 'NPWP', index: 'a.NPWP', hidden: true },
			{ name: 'NPPKP', index: 'a.NPPKP', hidden: true },
			{ name: 'CreditLimitIDR', index: 'a.CreditLimitIDR', hidden: true },
			{ name: 'ShipStat', index: 'a.shipstat', hidden: true },
			{ name: 'AirPortCode', index: 'a.AirPortCode', hidden: true },
			{ name: 'IntAirPort', index: 'IntAirPort', hidden: true },
			{ name: 'AirPortName', index: 'AirPortName', hidden: true },
			{ name: 'GroupId', index: 'GroupId', hidden: true },
			{ name: 'GroupName', index: 'GroupName', hidden: true },
			{ name: 'jobownerlist', index: 'jobownerlist', hidden: true },
			{ name: 'IsAgent', index: 'isagent', hidden: true },
			{ name: 'IsShipper', index: 'isshipper', hidden: true },
			{ name: 'IsConsignee', index: 'isconsignee', hidden: true },
			{ name: 'IsEMKL', index: 'isemkl', hidden: true },
			{ name: 'IsIATA', index: 'isiata', hidden: true },
			{ name: 'IsSSLine', index: 'isssline', hidden: true },
			{ name: 'IsDepo', index: 'isdepo', hidden: true }
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
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
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#pager1', { del: false, add: false, edit: false, search: true })
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
        $('#rbcostatuspt,#cbcontact_typeagent').attr("checked", true);
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
                url: base_url + "MstContact/GetInfo?Id=" + id,
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
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#contactcode').val(result.MasterCode).data('kode',result.id);
                            $('#contactname').val(result.ContactName);
                            var companystatus = result.ContactStatus;

                            switch (companystatus.toLowerCase()) {
                                case 'pt': $("#rbcostatuspt").attr('checked', true); break;
                                case 'cv': $("#rbcostatuscv").attr('checked', true); break;
                                case 'ot': $("#rbcostatusother").attr('checked', true); break;
                            }

                            $('#contactaddress').val(result.ContactAddress);
                            $('#contactcontact').val(result.ContactPerson);
                            $('#contactpostalcode').val(result.PostalCode);
                            $('#contactphone').val(result.Phone);
                            $('#contactfax').val(result.Fax);
                            $('#contactemail').val(result.Email);
                            $('#contactnpwp').val(result.NPWP);
                            $('#contactnppkp').val(result.NPPKP);
                            $('#contactcredittermidr').val(result.CreditLimitIDR);

                            $('#Country').val(result.CountryName);
                            $('#Continent').val(result.ContinentName);

                            $('#txtcontactshipdate').val(result.LastShipmentDate);

                            $('#CityCode').data("kode", result.CityId);
                            $('#CityCode').val(result.CityAbbrevation);
                            $('#City').val(result.CityName);

                            $('#PortCode').data("kode", result.PortId);
                            $('#PortCode').val(result.PortAbbrevation);
                            $('#Port').val(result.PortName);

                            $('#AirportCode').data("kode", result.AirPortCode);
                            $('#AirportCode').val(result.AirportAbbrevation);
                            $('#Airport').val(result.AirportName);

                            if (result.IsAgent == true)
                                $('#cbcontact_typeagent').attr('checked', 'checked');
                            if (result.IsShipper == true)
                                $('#cbcontact_typeshipper').attr('checked', 'checked');
                            if (result.IsConsignee == true)
                                $('#cbcontact_typeconsignee').attr('checked', 'checked');
                            if (result.IsEMKL == true)
                                $('#cbcontact_typeemkl').attr('checked', 'checked');
                            if (result.IsIATA == true)
                                $('#cbcontact_typeiata').attr('checked', 'checked');
                            if (result.IsSSLine == true)
                                $('#cbcontact_typessline').attr('checked', 'checked');
                            if (result.IsDepo == true)
                                $('#cbcontact_typedepo').attr('checked', 'checked');
                            $('#form_div').dialog('open');
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
            //if (ret.deletedimg != '') {
            //    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
            //    return;
            //}
            $('#delete_confirm_id').val(id);
            $('#delete_confirm_name').text(ret.ContactName);
            $('#delete_confirm_btn_submit').data('Id', id);
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
            url: base_url + "MstContact/Delete",
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


        var vcompanystatus = "";
        if ($('#rbcostatuspt').is(':checked'))
            vcompanystatus = "PT";
        if ($('#rbcostatuscv').is(':checked'))
            vcompanystatus = "CV";
        if ($('#rbcostatusother').is(':checked'))
            vcompanystatus = "OT";

        var vIsAgent = false;
        if ($('#cbcontact_typeagent').is(':checked'))
            vIsAgent = true;
        var vIsShipper = false;
        if ($('#cbcontact_typeshipper').is(':checked'))
            vIsShipper = true;
        var vIsConsignee = false;
        if ($('#cbcontact_typeconsignee').is(':checked'))
            vIsConsignee = true;
        var vIsEMKL = false;
        if ($('#cbcontact_typeemkl').is(':checked'))
            vIsEMKL = true;
        var vIsIATA = false;
        if ($('#cbcontact_typeiata').is(':checked'))
            vIsIATA = true;
        var vIsSSLine = false;
        if ($('#cbcontact_typessline').is(':checked'))
            vIsSSLine = true;
        var vIsDepo = false;
        if ($('#cbcontact_typedepo').is(':checked'))
            vIsDepo = true;

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'MstContact/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstContact/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ContactName: $("#contactname").val(), Phone: $("#contactphone").val(), ContactAddress: $("#contactaddress").val(),
                ContactPerson: $("#contactcontact").val(), PostalCode: $("#contactpostalcode").val(), ContactStatus: vcompanystatus,
                Fax: $("#contactfax").val(), Email: $("#contactemail").val(), CityId: $("#CityCode").data("kode"),
                PortId: $("#PortCode").data("kode"), AirPortId: $("#AirportCode").data("kode"),
                NPWP: $("#contactnpwp").val(), NPPKP: $("#contactnppkp").val(),
                IsAgent: vIsAgent, IsShipper: vIsShipper, IsConsignee: vIsConsignee, IsEMKL: vIsEMKL,
                IsSSLine: vIsSSLine, IsIATA: vIsIATA, IsDepo: vIsDepo,
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}')
                {
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
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    // -------------------------------------------------------Look Up port-------------------------------------------------------
    $('#btnPort').click(function () {
        var lookUpURL = base_url + 'Port/GetLookUp';
        var lookupGrid = $('#lookup_table_port');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_port').dialog('open');
    });

    jQuery("#lookup_table_port").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'IntCode', 'City', '', 'Country'],
        colModel: [{ name: 'MasterCode', index: 'MasterCode', width: 100, align: "center" },
				  { name: 'Name', index: 'Name', width: 320 },
				  { name: 'Abbrevation', index: 'Abbrevation', hidden: true },
				  { name: 'cityname', index: 'cityname' },
				  { name: 'intcity', index: 'intcity', hidden: true },
				  { name: 'countryname', index: 'countryname' }
        ],
        page: '1',
        pager: $('#lookup_pager_port'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'MasterCode',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_port").width() - 10,
        height: $("#lookup_div_port").height() - 110,
    });
    $("#lookup_table_port").jqGrid('navGrid', '#lookup_toolbar_port', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_port').click(function () {
        $('#lookup_div_port').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_port').click(function () {
        var id = jQuery("#lookup_table_port").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_port").jqGrid('getRowData', id);

            $('#PortCode').val(ret.Abbrevation).data("kode", id);
            $('#Port').val(ret.Name);
            $('#lookup_div_port').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup port----------------------------------------------------------------

    // -------------------------------------------------------Look Up airport-------------------------------------------------------
    $('#btnAirport').click(function () {
        var lookUpURL = base_url + 'Airport/GetLookUp';
        var lookupGrid = $('#lookup_table_airport');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_airport').dialog('open');
    });

    jQuery("#lookup_table_airport").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'IntCode', 'City', '', 'Country'],
        colModel: [{ name: 'code', index: 'MasterCode', width: 100, align: "center" },
				  { name: 'Name', index: 'Name', width: 320 },
				  { name: 'Abbrevation', index: 'Abbrevation', hidden: true },
				  { name: 'cityname', index: 'cityname' },
				  { name: 'intcity', index: 'intcity', hidden: true },
				  { name: 'countryname', index: 'countryname' }
        ],
        page: '1',
        pager: $('#lookup_pager_airport'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'MasterCode',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_airport").width() - 10,
        height: $("#lookup_div_airport").height() - 110,
    });
    $("#lookup_table_airport").jqGrid('navGrid', '#lookup_toolbar_airport', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_airport').click(function () {
        $('#lookup_div_airport').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_airport').click(function () {
        var id = jQuery("#lookup_table_airport").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_airport").jqGrid('getRowData', id);

            $('#AirportCode').val(ret.Abbrevation).data("kode", id);
            $('#Airport').val(ret.Name);
            $('#lookup_div_airport').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup airport----------------------------------------------------------------

    // -------------------------------------------------------Look Up city-------------------------------------------------------
    $('#btnCity').click(function () {
        var lookUpURL = base_url + 'CityLocation/GetLookUp';
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
        colNames: ['Code', 'Name', 'IntCode', 'Country', 'Continent'],
        colModel: [{ name: 'code', index: 'MasterCode', width: 100, align: "center" },
				  { name: 'Name', index: 'Name', width: 320 },
				  { name: 'Abbrevation', index: 'Abbrevation', hidden: true },
				  { name: 'countryname', index: 'countryname' },
				  { name: 'continentname', index: 'continentname' }],
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
            $('#Country').val(ret.countryname);
            $('#Continent').val(ret.continentname);

            $('#lookup_div_city').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup city----------------------------------------------------------------

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

   
}); //END DOCUMENT READY