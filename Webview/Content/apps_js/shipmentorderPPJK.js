$(document).ready(function () {
    // Initialize
    var initETA = '';
    var containerSEonedit = 0;
    var vlookupSOagent = '';
    var vlookupSOconsignee = '';
    var vlookupSOcity = '';
    var vlookupSOport = '';
    var vlookupSOssline = '';
    var vlookupSOdepo = '';
    var vlookupSOemkl = '';
    var vlookupSOvessel = '';
    var vlookupSOvesselIndex = 0;
    $('#seaimportdetail_content').css("height", $(window).height() - 120);
    $('#seaimportdetail_content').css("width", $(window).width() - 20);
    // Close dialog
    $('#lookup_div_so_agent').dialog('close');
    $('#lookup_div_so_shipper').dialog('close');
    $('#lookup_div_so_consignee').dialog('close');
    $('#lookup_div_so_marketing').dialog('close');
    $('#lookup_div_so_port').dialog('close');
    $('#lookup_div_so_city').dialog('close');
    $('#lookup_div_so_ssline').dialog('close');
    $('#lookup_div_so_depo').dialog('close');
    $('#lookup_div_so_emkl').dialog('close');
    $('#lookup_div_so_vessel').dialog('close');
    $('#lookup_div_so_container').dialog('close');
    $('#lookup_div_bl_blform').dialog('close');
    $('#dialogPrint').dialog('close');
    $('#lookup_div_so_freight_obl_payable').dialog('close');
    $('#lookup_div_so_freight_hbl_payable').dialog('close');
    $('#containerSEcontainerno').mask("aaaa 999999-9");
    $('#mst_vessel_form_div').dialog('close');
    // End Initialize

    /*================================================ JQuery Tabs ================================================*/
    $('ul.tabs').each(function () {
        // For each set of tabs, we want to keep track of
        // which tab is active and it's associated content
        var $active, $content, $links = $(this).find('a');

        // If the location.hash matches one of the links, use that as the active tab.
        // If no match is found, use the first link as the initial active tab.
        $active = $($links.filter('[href="' + location.hash + '"]')[0] || $links[0]);
        $active.addClass('active');

        $content = $($active[0].hash);

        // Hide the remaining content
        $links.not($active).each(function () {
            $(this.hash).hide();
        });

        // Bind the click event handler
        $(this).on('click', 'a', function (e) {
            // Make the old tab inactive.
            $active.removeClass('active');
            $content.hide();

            // Update the variables with the new link and content
            $active = $(this);
            $content = $(this.hash);

            // Make the tab active.
            $active.addClass('active');
            $content.show();

            // Prevent the anchor's default click action
            e.preventDefault();
        });
    });
    /*================================================ END JQuery Tabs ================================================*/

    // First Load
    // tools.js
    var shipmentId = getQueryStringByName('Id');
    var jobId = getQueryStringByName('JobId');
    var jobNumber = getQueryStringByName('JobNumber');
    var subJobNumber = getQueryStringByName('SubJobNumber');
    FirstLoad();

    createContainerTable();
    // End First Load

    //Print DO
    $('#seaimport_form_btn_print_do').click(function () {
        $('#dialogPrint').dialog('open');

        $('#txtgudang').val("");
    });

    $('#btnDialogPrintYes').click(function () {
        window.open(base_url + "Print_Forms/PrintDOSI.aspx?Reference=" + $("#txtSIshipmentno").val() + "&Gudang=" +
                        $("#txtgudang").val() + "&JobNumber=" + jobNumber + "&SubJobNo=" + subJobNumber + "&JobCode=31");

        $('#dialogPrint').dialog('close');
    });

    $('#btnDialogPrintNo').click(function () {
        $('#dialogPrint').dialog('close');
    });
    //END Print DO

    // Shipment Order - Freight Info Container
    $("#btnSubmitcontainerSE").click(function () {

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "shipmentorder/InsertUpdateShipmentSeaContainer",
            data: JSON.stringify({
                ShipmentId: shipmentId, SeaContainerId: $(this).data('id'), ContainerNo: $('#containerSEcontainerno').val(), SealNo: $('#containerSEsealno').val(),
                Size: $('#containerSEsize').val(), Type: $('#containerSEtype').val(), GrossWeight: $('#containerSEgrossweight').numberbox('getValue'),
                NetWeight: $('#containerSEnetweight').numberbox('getValue'), CBM: $('#containerSEcbm').numberbox('getValue'),
                PartOf: $('#containerSEpartof').val(), Commodity: $('#containerSEcommodity').val(), PackagingCode: $('#containerSEpackaging').val(),
                NoOfPieces: $('#containerSEnoofpieces').val()
            }),
            success: function (result) {
                if (result.isValid) {
                    console.log(result.objResult);
                    $.messager.alert('Information', result.message, 'info', function () {
                        //$.messager.alert('Warning', result.message, 'warning');
                        addContainerTable(result.objResult.SeaContainerId, $('#containerSEcontainerno').val(), $('#containerSEsealno').val(),
                            $('#containerSEsize').val(), $('#containerSEtype').val(),
                            $('#containerSEgrossweight').numberbox('getValue'), $('#containerSEnetweight').numberbox('getValue'), $('#containerSEcbm').numberbox('getValue'),
                            $('#containerSEcommodity').val(), $('#containerSEnoofpieces').val(),
                            $('#containerSEpackaging').val(), $('#containerSEpartof').val());

                        $(this).data('id', '');

                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }


            }
        });
    });

    $("a[id^=containerSEdel]").live("click", function () {
        var delID = $(this);

        $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
            if (r) {

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "shipmentorder/DeleteShipmentSeaContainer",
                    data: JSON.stringify({
                        Id: delID.data('id')
                    }),
                    success: function (result) {
                        if (result.isValid) {
                            console.log(result.objResult);
                            $.messager.alert('Information', result.message, 'info', function () {
                                delID.parent().parent().remove();
                                // Summarize Container size
                                summarizeContainerSize()
                                // End Summarize Container size
                            });
                        }
                        else {
                            $.messager.alert('Warning', result.message, 'warning');
                        }


                    }
                });

                // Clear Input
                clearFormContainer();
            }
        });
    });

    $("a[id^=containerSEedit]").live("click", function () {

        // Assign Id
        $("#btnSubmitcontainerSE").data('id', $(this).data('id'));

        containerSEonedit = 1;
        var size = $(this).parent().parent().find('td:eq(2)').text().trim();
        switch (size) {
            case '20': $('#containerSEsize').val('1'); break;
            case '40': $('#containerSEsize').val('2'); break;
            case '45': $('#containerSEsize').val('3'); break;
        }
        var type = $(this).parent().parent().find('td:eq(3)').text().trim();
        switch (type) {
            case 'GP': $('#containerSEtype').val('1'); break;
            case 'HC': $('#containerSEtype').val('2'); break;
            case 'RF': $('#containerSEtype').val('3'); break;
            case 'FR': $('#containerSEtype').val('4'); break;
            case 'OT': $('#containerSEtype').val('5'); break;
            case 'GOH': $('#containerSEtype').val('6'); break;
            case 'ISO': $('#containerSEtype').val('7'); break;
        }
        $('#containerSEcontainerno').val($(this).parent().parent().find('td:eq(0)').text());
        $('#containerSEsealno').val($(this).parent().parent().find('td:eq(1)').text());
        $('#containerSEgrossweight').numberbox('setValue', $(this).parent().parent().find('td:eq(4)').text());
        $('#containerSEnetweight').numberbox('setValue', $(this).parent().parent().find('td:eq(5)').text());
        $('#containerSEcbm').numberbox('setValue', $(this).parent().parent().find('td:eq(6)').text());
        $('#containerSEcommodity').val($(this).parent().parent().find('td:eq(7)').text());
        $('#containerSEnoofpieces').val($(this).parent().parent().find('td:eq(8)').text());
        $('#containerSEpackaging').val($(this).parent().parent().find('td:eq(9)').text());
        var partof = $(this).parent().parent().find('td:eq(10)').text().trim();
        switch (partof) {
            case 'YES': $('#containerSEpartof').val('false'); break;
            case 'NO': $('#containerSEpartof').val('true'); break;
        }

        // Disabled Size, Type and Container No
        $('#containerSEcontainerno').attr('disabled', 'disabled');
        $('#containerSEtype').attr('disabled', 'disabled');
        $('#containerSEsize').attr('disabled', 'disabled');
    });

    $("#btnCancelcontainerSE").click(function () {
        clearFormContainer();
    });

    function addContainerTable(id, containerno, sealno, size, type, grossweight, netweight, cbm, commodity, noofpieces, packaging, partof) {
        var tbody = $('#list_containerSE');

        // Validate
        if (containerno.trim() == '') {
            $.messager.alert('Information', 'Invalid Container...!!', 'info');
            return
        }

        var isValid = true;
        $('#list_containerSE tr').each(function () {
            // Check exist on adding
            if (containerno == $.trim($(this).find('td:eq(0)').text()) && containerSEonedit == 0) {
                isValid = false;
            }
        });
        if (!isValid) {
            $.messager.alert('Information', 'Container has been exist...!!', 'info');
            return
        }
        // End Validate

        // partof
        switch (partof) {
            case false:
            case 'false':
                partof = 'YES'; break;
            case true:
            case 'true':
                partof = 'NO'; break;
        }
        // size
        switch (size) {
            case '1': size = '20'; break;
            case '2': size = '40'; break;
            case '3': size = '45'; break;
        }
        // type
        switch (type) {
            case '1': type = 'GP'; break;
            case '2': type = 'HC'; break;
            case '3': type = 'RF'; break;
            case '4': type = 'FR'; break;
            case '5': type = 'OT'; break;
            case '6': type = 'GOH'; break;
            case '7': type = 'ISO'; break;
        }

        // Put to table
        if (containerSEonedit == 1) {
            $('#list_containerSE tr').each(function () {
                // Container No
                var cno = $.trim($(this).find('td:eq(0)').text());
                if (cno == containerno) {
                    //$(this).remove();
                    $(this).find('td:eq(1)').text(sealno.toUpperCase());
                    $(this).find('td:eq(2)').text(size);
                    $(this).find('td:eq(3)').text(type);
                    $(this).find('td:eq(4)').text(Number(grossweight).toFixed(4));
                    $(this).find('td:eq(5)').text(Number(netweight).toFixed(4));
                    $(this).find('td:eq(6)').text(Number(cbm).toFixed(4));
                    $(this).find('td:eq(7)').text(commodity.toUpperCase());
                    $(this).find('td:eq(8)').text(noofpieces);
                    $(this).find('td:eq(9)').text(packaging);
                    $(this).find('td:eq(10)').text(partof);
                }
            });
        }
            // end remove first if on edit
        else {
            var trow = $("<tr>").addClass("tableRow").addClass('ui-widget-content');
            $("<td>").addClass("tableCell").text(containerno.toUpperCase()).appendTo(trow);
            $("<td>").addClass("tableCell").text(sealno.toUpperCase()).appendTo(trow);
            $("<td>").addClass("tableCell").text(size).appendTo(trow);
            $("<td>").addClass("tableCell").text(type).appendTo(trow);
            $("<td>").addClass("tableCell").text(Number(grossweight).toFixed(4)).appendTo(trow);
            $("<td>").addClass("tableCell").text(Number(netweight).toFixed(4)).appendTo(trow);
            $("<td>").addClass("tableCell").text(Number(cbm).toFixed(4)).appendTo(trow);
            $("<td>").addClass("tableCell").text(commodity.toUpperCase()).appendTo(trow);
            $("<td>").addClass("tableCell").text(noofpieces).appendTo(trow);
            $("<td>").addClass("tableCell").text(packaging).appendTo(trow);
            $("<td>").addClass("tableCell").text(partof).appendTo(trow);
            $("<td>").addClass("tableCell")
                .append($('<a>').attr('href', 'javascript: void(0)').attr('rel', containerno).data('id', id).attr('id', 'containerSEedit' + containerno).text('edit'))
                .append($('<span>').text('').css("padding", "0px 5px"))
                .append($('<a>').attr('href', 'javascript: void(0)').attr('rel', containerno).data('id', id).attr('id', 'containerSEdel' + containerno).text('delete'))
                .appendTo(trow);
            trow.appendTo(tbody);
        }
        // End Put to table

        // Summarize Container size
        summarizeContainerSize()
        // End Summarize Container size

        // Clear Input
        clearFormContainer();
    }

    function summarizeContainerSize() {
        var size20 = 0;
        var size40 = 0;
        var size45 = 0;
        var sizeAll = 0;
        $('#list_containerSE tr').each(function () {
            // size 20
            var size = $.trim($(this).find('td:eq(2)').text());
            if (size == '20') {
                size20++;
                sizeAll++;
            }
            else if (size == '40') {
                size40++;
                sizeAll++;
            }
            else if (size == '45') {
                size45++;
                sizeAll++;
            }
        });
        $('#txtcontainerSEtotal20').val(size20);
        $('#txtcontainerSEtotal40').val(size40);
        $('#txtcontainerSEtotal45').val(size45);
        $('#txtcontainerSEtotalall').val(sizeAll);
    }

    function createContainerTable() {
        var tbody = $('#list_containerSE');
        if (tbody == null || tbody.length < 1) return;
        // Clear 
        $("#list_containerSE tr.tableRow").each(function () {
            $(this).remove();
        });

        var trow = $("<tr>").addClass("tableRow").addClass('ui-jqgrid-labels ui-widget-content');
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Container No.').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Seal No.').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Size').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Type').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Gross Weight').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Net Weight').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('CBM').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Commodity').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('No Of Pieces').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Packaging').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Part Of').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Option').appendTo(trow);
        trow.appendTo(tbody);
    }

    function clearFormContainer() {

        // Reset Sea Container ID
        $("#btnSubmitcontainerSE").data('id', '');

        // Clear Input
        $('#list_containerSEinput input').each(function () {
            $(this).val('');
            $(this).text('');
            $(this).removeAttr('disabled');
        });
        $('#containerSEcommodity').text('').val('').removeAttr('disabled');
        $('#list_containerSEinput select').each(function () {
            $(this).val('1');
            $(this).removeAttr('disabled');
        });

        containerSEonedit = 0;
    }

    // Input Focus after click ENTER
    $('input#containerSEcontainerno').keypress(function (e) {
        if (e.which == '13') {
            $('input#containerSEsealno').focus();
            e.preventDefault();
        }
    });

    $('input#containerSEsealno').keypress(function (e) {
        if (e.which == '13') {
            $('input#containerSEgrossweight').focus();
            e.preventDefault();
        }
    });

    $('input#containerSEgrossweight').keypress(function (e) {
        if (e.which == '13') {
            $('input#containerSEnetweight').focus();
            e.preventDefault();
        }
    });

    $('input#containerSEnetweight').keypress(function (e) {
        if (e.which == '13') {
            $('input#containerSEcbm').focus();
            e.preventDefault();
        }
    });

    $('input#containerSEcbm').keypress(function (e) {
        if (e.which == '13') {
            $('textarea#containerSEcommodity').focus();
            e.preventDefault();
        }
    });

    $('textarea#containerSEcommodity').keypress(function (e) {
        if (e.which == '13') {
            $('input#containerSEnoofpieces').focus();
            e.preventDefault();
        }
    });

    $('input#containerSEnoofpieces').keypress(function (e) {
        if (e.which == '13') {
            $('select#containerSEpackaging').focus();
            e.preventDefault();
        }
    });

    $('select#containerSEpackaging').keypress(function (e) {
        if (e.which == '13') {
            $('select#containerSEpartof').focus();
            e.preventDefault();
        }
    });

    $('select#containerSEpartof').keypress(function (e) {
        if (e.which == '13') {
            $('button#btnSubmitcontainerSE').focus();
            e.preventDefault();
        }
    });
    // END Input Focus after click ENTER

    // End Shipment Order - Freight Info Container


    function FirstLoad() {

        $.ajax({
            dataType: "json",
            url: base_url + "shipmentorder/GetShipmentOrderInfo?Id=" + shipmentId,
            success: function (result) {
                if (!result.isValid)
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "shipmentorder?type=" + jobId;
                    });
                else {
                    // Open / Close Job
                    if (result.objJob.JobClosed) {
                        $('#seaimport_form_btn_open_close_job').linkbutton({
                            iconCls: 'icon-book',
                            text: 'Open Job'
                        });
                    }

                    // Set Title
                    $('#seaimportdetail').panel({
                        title: ".: Shipment Order - EMKL Sea Import - " + result.objJob.ShipmentNo + " :."
                    });

                    $("#txtSIshipmentno").val(result.objJob.ShipmentNo);
                    jobNumber = result.objJob.JobNumber;
                    subJobNumber = result.objJob.SubJobNo;
                    $('#nav_searchjobnumber').val(result.objJob.JobNumber);
                    $('#nav_searchsubjobno').val(result.objJob.SubJobNo);
                    $("#txtSItotalsubship").val(result.objJob.TotalSub);
                    $("#ddlSIjobowner").val(result.objJob.JobOwner);
                    // Marketing
                    $("#txtSIkdmarketing").val(result.objJob.MarketCode).data('kode', result.objJob.MarketCode).data("companycode", result.objJob.MarketCompany);
                    $("#txtSInmmarketing").val(result.objJob.MarketName);
                    // Ref. SI From Shipper
                    $("#txtSIrefSI").val(result.objJob.SIReference);
                    // Date SI From Shipper
                    $("#txtSIDateSI").datebox('setValue', dateEnt(result.objJob.SIDate));
                    // Conversion
                    switch (result.objJob.Conversion) {
                        case "T": $("#optSIconversionyes").attr('checked', true); break;
                        case "F": $("#optSIconversionno").attr('checked', true); break;
                        default: $("#optSIconversionyes").attr('checked', true); break;
                    }
                    // Load Status
                    switch (result.objJob.LoadStatus) {
                        case "FCL": $("#rbSIfcl").attr('checked', true); break;
                        case "LCL": $("#rbSIlcl").attr('checked', true); break;
                        default: $("#rbSIfcl").attr('checked', true); break;
                    }
                    // ShipmentStatus
                    switch (result.objJob.ShipmentStatus) {
                        case 'F': $("#optSIfreehand").attr('checked', true);
                            $("#btnSImarketing").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                            break;
                        case 'N': $("#optSInominate").attr('checked', true); break;
                        case 'C': $("#optSIcorporate").attr('checked', true); break;
                        default: $("#optSIfreehand").attr('checked', true); break;
                    }
                    // ContainerStatus
                    switch (result.objJob.ContainerStatus) {
                        case 'G': $("#optSIgroupage").attr('checked', true); break;
                        case 'P': $("#optSIpartof").attr('checked', true); break;
                        case 'N': $("#optSInone").attr('checked', true); break;
                        default: $("#optSIgroupage").attr('checked', true); break;
                    }
                    // JobStatus
                    switch (result.objJob.JobStatus) {
                        case 'LCL Console': $("#optSIjobLCLConsole").attr('checked', true); break;
                        case 'LCL Co-Load': $("#optSIjobLCLCoload").attr('checked', true); break;
                        case 'LCL Submarine': $("#optSIjobLCLSubmarine").attr('checked', true); break;
                        case 'FCL': $("#optSIjobFCL").attr('checked', true); break;
                        case 'Other service': $("#optSIjobOtherservice").attr('checked', true); break;
                        default: $("#optSIjobFCL").attr('checked', true); break;
                    }
                    // Type Of Service
                    switch (result.objJob.ServiceNo) {
                        case 1: $("#rbSIfcl_fcl").attr('checked', true); break;
                        case 2: $("#rbSIfcl_lcl").attr('checked', true); break;
                        case 3: $("#rbSIlcl_fcl").attr('checked', true); break;
                        case 4: $("#rbSIlcl_lcl").attr('checked', true); break;
                        case 5: $("#rbSIlcl_lcl_g").attr('checked', true); break;
                        default: $("#rbSIfcl_fcl").attr('checked', true); break;
                    }
                    // Agent
                    $("#txtSIkdagent").val(result.objJob.AgentCode).data('kode', result.objJob.AgentId);
                    $("#txtSInmagent").val(result.objJob.AgentName);
                    $("#txtSIagentaddress").val(result.objJob.AgentAddress);
                    // Transipment
                    $("#txtSIkdtranshipment").val(result.objJob.TranshipmentCode).data('kode', result.objJob.TranshipmentId);
                    $("#txtSInmtranshipment").val(result.objJob.TranshipmentName);
                    $("#txtSItranshipaddress").val(result.objJob.TranshipmentAddress);
                    // Shipper
                    $("#txtSInmshipper").val(result.objJob.ShipperName);
                    $("#txtSIshipperaddress").val(result.objJob.ShipperAddress);
                    // Consignee
                    $("#txtSIkdconsignee").val(result.objJob.ConsigneeCode).data('kode', result.objJob.ConsigneeId);
                    $("#txtSInmconsignee").val(result.objJob.ConsigneeName);
                    $("#txtSIconsigneeaddress").val(result.objJob.ConsigneeAddress);
                    // Party
                    $("#txtSIkdnparty").val(result.objJob.NPartyCode).data('kode', result.objJob.NPartyId);
                    $("#txtSInmnparty").val(result.objJob.NPartyName);
                    $("#txtSInpartyaddress").val(result.objJob.NPartyAddress);
                    // Ref. SI From Shipper
                    $("#txtSIrefSI").val(result.objJob.SIReference);
                    // Date SI From Shipper
                    $("#txtSIDateSI").datebox('setValue', dateEnt(result.objJob.SIDate));
                    // Goods Received at Origin
                    $("#txtSIgrorigin").datebox('setValue', dateEnt(result.objJob.GoodsRecDate));

                    // Create HTML Table for Freight OBL Payable At
                    createAgentTable($("#lookup_table_freight_obl_payable"));

                    // Create HTML Table for Freight HBL Payable At
                    createConsigneeTable($("#lookup_table_freight_hbl_payable"));

                    // Freight Info - Vessel
                    $("#txtSIetd").datebox('setValue', dateEnt(result.objJob.ETD));
                    $("#txtSIeta").datebox('setValue', dateEnt(result.objJob.ETA));
                    initETA = dateEnt(result.objJob.ETA);

                    if (result.objJob.ShipmentRoutingList != null) {
                        if (result.objJob.ShipmentRoutingList.length > 0) {
                            for (var i = 1; i <= result.objJob.ShipmentRoutingList.length; i++) {
                                if (i > 1) {
                                    var clonedRow = $('#tblVessel tr:eq(1)').clone();
                                    clonedRow.find('input').not('input[type="button"]').val('').data("kode", '');
                                    $('#tblVessel').append(clonedRow);

                                    // ReInitiate ETD as datebox
                                    $('#tblVessel tr:last td:eq(4)').html('<input id="txtSIvesseletd" name="txtSIvesseletd" class="editable easyui-datebox" type="text" title="mm/dd/yyyy" size="8" maxlength="10" />');
                                    $('#tblVessel tr:last td:eq(4)').find('#txtSIvesseletd').datebox();
                                }

                                var Id = result.objJob.ShipmentRoutingList[i - 1].Id;
                                var vesselId = result.objJob.ShipmentRoutingList[i - 1].VesselId;
                                var vesselName = result.objJob.ShipmentRoutingList[i - 1].VesselName;
                                var vesselType = result.objJob.ShipmentRoutingList[i - 1].VesselType;
                                var voyage = result.objJob.ShipmentRoutingList[i - 1].Voyage;
                                var vesselETD = result.objJob.ShipmentRoutingList[i - 1].ETD;
                                var cityId = result.objJob.ShipmentRoutingList[i - 1].CityId;
                                var cityInt = result.objJob.ShipmentRoutingList[i - 1].IntCity;
                                var cityName = result.objJob.ShipmentRoutingList[i - 1].CityName;

                                $('#tblVessel tr:eq(' + i + ') td:eq(0)').text(i);
                                $('#tblVessel tr:eq(' + i + ') td').find('input[name="txtSInmvessel"]').val(vesselName).data("kode", vesselId);
                                $('#tblVessel tr:eq(' + i + ') td').find('select[name="ddlSEvesseltype"]').val(vesselType);
                                $('#tblVessel tr:eq(' + i + ') td').find('input[name="txtSIvesselvoy"]').val(voyage);
                                $('#tblVessel tr:eq(' + i + ') td:eq(4)').find('#txtSIvesseletd').datebox('setValue', dateEnt(vesselETD));
                                $('#tblVessel tr:eq(' + i + ') td').find('input[name="txtSIvesselintcity"]').val(cityInt).data("kode", cityId);
                                $('#tblVessel tr:eq(' + i + ') td').find('input[name="txtSInmvesselcity"]').val(cityName);
                                $('#tblVessel tr:eq(' + i + ') td').find('a[name = "savevessel"]').attr('rel', Id);
                                $('#tblVessel tr:eq(' + i + ') td').find('a[name = "deletevessel"]').attr('rel', Id);
                            }
                        }
                    }

                    //// Feeder
                    //$("#txtSIfeedervessel").val(result.objJob.FirstVessel);
                    //$("#txtSIvoyfeedervessel").val(result.objJob.FirstVoyage);
                    //$("#txtSIfeedervesseletd").datebox('setValue', dateEnt(result.objJob.FirstETD));
                    //// Connecting
                    //$("#txtSIconectvessel").val(result.objJob.ConnectingVessel);
                    //$("#txtSIvoyconnectvessel").val(result.objJob.ConnectingVoyage);
                    //$("#txtSIkdcityconnectvessel").val(result.objJob.ConnectingIntCity).data("kode", result.objJob.ConnectingCityCode);
                    //$("#txtSInmcityconnectvessel").val(result.objJob.ConnectingCityName);
                    //$("#txtSIconnectingetd").datebox('setValue', dateEnt(result.objJob.ConnectingETD));
                    // Port Of Loading
                    $("#txtSIkdportofloading").val(result.objJob.LoadingIntPort).data("kode", result.objJob.LoadingPortCode);
                    $("#txtSInmportofloading").val(result.objJob.LoadingPortName);
                    // Place Of Receipt
                    $("#txtSIkdplaceofreceipt").val(result.objJob.ReceiptPlaceIntCity).data("kode", result.objJob.ReceiptPlaceCode);
                    $("#txtSInmplaceofreceipt").val(result.objJob.ReceiptPlaceName);
                    // Port Of Discharge
                    $("#txtSIkdportofdischarge").val(result.objJob.DischargeIntPort).data("kode", result.objJob.DischargePortCode);
                    $("#txtSInmportofdischarge").val(result.objJob.DischargePortName);
                    // Place Of Delivery
                    $("#txtSIkdplaceofdelivery").val(result.objJob.DeliveryPlaceIntCity).data("kode", result.objJob.DeliveryPlaceCode);
                    $("#txtSInmplaceofdelivery").val(result.objJob.DeliveryPlaceName);
                    //// Mother
                    //$("#txtSImothervessel").val(result.objJob.FeederVessel);
                    //$("#txtSIvoymothervessel").val(result.objJob.FeederVoyage);
                    //$("#txtSIetamothervessel").datebox('setValue', dateEnt(result.objJob.FeederETA));
                    //initETA = dateEnt(result.objJob.FeederETA);
                    // End Freight Info - Vessel

                    // Freight Info - Container
                    if (result.objJob.SeaContainerList != null) {
                        if (result.objJob.SeaContainerList.length > 0) {
                            for (var i = 0; i < result.objJob.SeaContainerList.length; i++) {
                                //alert(result.objJob.SeaContainerList[0].ContainerNo);
                                addContainerTable(result.objJob.SeaContainerList[i].SeaContainerId,
                                    result.objJob.SeaContainerList[i].ContainerNo, result.objJob.SeaContainerList[i].SealNo,
                                    result.objJob.SeaContainerList[i].Size.toString(),
                                    result.objJob.SeaContainerList[i].Type.toString(), result.objJob.SeaContainerList[i].GrossWeight, result.objJob.SeaContainerList[i].NetWeight,
                                    result.objJob.SeaContainerList[i].CBM, result.objJob.SeaContainerList[i].Commodity, result.objJob.SeaContainerList[i].NoOfPieces,
                                    result.objJob.SeaContainerList[i].PackagingCode.toString(),
                                    result.objJob.SeaContainerList[i].PartOf);
                            }
                        }
                    }
                    if (subJobNumber > 0)
                        $('#btncontainerSEcontainerno').removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                    // End Freight Info - Container

                    // Freight Info - Description
                    $("#txtSIdesc").text(result.objJob.GoodDescription);
                    // End Freight Info - Description

                    // Freight Info - Freight
                    // OBL Status
                    switch (result.objJob.OBLStatus) {
                        case 'P': $("#optSIoblprepaid").attr('checked', true); break;
                        case 'C':
                            $("#optSIoblcollect").attr('checked', true);
                            $("#btnSIoblcollect").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                            $("#btnSIoblpayable").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                            break;
                    }
                    // OBL Collect
                    $("#txtSIkdoblcollect").val(result.objJob.OBLCollectIntCity).data("kode", result.objJob.OBLCollectCode);
                    $("#txtSInmoblcollect").val(result.objJob.OBLCollectCityName);
                    // Save to temp data for updating Prepaid or Collect
                    $("#btnSIoblcollect").data("tempkode", result.objJob.OBLCollectCode).data("tempcint", result.objJob.OBLCollectIntCity).data("tempcname", result.objJob.OBLCollectCityName);
                    // OBL Payable
                    $("#txtSIkdoblpayable").val(result.objJob.OBLPayableCode).data("kode", result.objJob.OBLPayableId);
                    $("#txtSInmoblpayable").val(result.objJob.OBLPayableAgentName);
                    // Save to temp data for updating Prepaid or Collect
                    $("#btnSIoblpayable").data("tempkode", result.objJob.OBLPayableCode).data("tempaname", result.objJob.OBLPayableAgentName);
                    // HBL Status
                    switch (result.objJob.HBLStatus) {
                        case 'P': $("#optSIhblprepaid").attr('checked', true); break;
                        case 'C':
                            $("#optSIhblcollect").attr('checked', true);
                            $("#btnSIhblcollect").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                            $("#btnSIhblpayable").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                            break;
                    }
                    // HBL Collect
                    $("#txtSIkdhblcollect").val(result.objJob.HBLCollectIntCity).data("kode", result.objJob.HBLCollectCode);
                    $("#txtSInmhblcollect").val(result.objJob.HBLCollectCityName);
                    // Save to temp data for updating Prepaid or Collect
                    $("#btnSIhblcollect").data("tempkode", result.objJob.HBLCollectCode).data("tempcint", result.objJob.HBLCollectIntCity).data("tempcname", result.objJob.HBLCollectCityName);
                    // HBL Payable
                    $("#txtSIkdhblpayable").val(result.objJob.HBLPayableCode).data("kode", result.objJob.HBLPayableId);
                    $("#txtSInmhblpayable").val(result.objJob.HBLPayableConsigneeName);
                    // Save to temp data for updating Prepaid or Collect
                    $("#btnSIhblpayable").data("tempkode", result.objJob.HBLPayableCode).data("tempaname", result.objJob.HBLPayableConsigneeName);

                    $('#ddlSIoblcurrency').val(result.objJob.OBLCurrency);
                    $('#txtSIoblamount').val(result.objJob.OBLAmount);
                    $('#ddlSIhblcurrency').val(result.objJob.HBLCurrency);
                    $('#txtSIhblamount').val(result.objJob.HBLAmount);
                    // End Freight Info - Freight

                    // Freight Info - Bill Of Lading
                    $("#txtSIhouseblno").val(result.objJob.HouseBLNo);
                    $("#txtSIsecondhblno").val(result.objJob.SecondBLNo);
                    $("#txtSIwarehousename").val(result.objJob.WareHouseName);
                    $("#txtSIkins").val(result.objJob.KINS);
                    $("#txtSIcargofreightcompany").val(result.objJob.CFName);
                    $("#txtSIoceanmstblnr").val(result.objJob.OceanMSTBLNo);
                    $("#txtSIvolumebl").val(result.objJob.VolumeBL);
                    $("#txtSIvolumeinvoice").val(result.objJob.VolumeInvoice);
                    // SSLine
                    $("#txtSIkdssline").val(result.objJob.SSLineCode).data('kode', result.objJob.SSLineId);
                    $("#txtSInmssline").val(result.objJob.SSLineName);
                    // EMKL
                    $("#txtSIkdemkl").val(result.objJob.EMKLCode).data('kode', result.objJob.EMKLId);
                    $("#txtSInmemkl").val(result.objJob.EMKLName);
                    // Depo
                    $("#txtSIkddepo").val(result.objJob.DepoCode).data('kode', result.objJob.DepoId);
                    $("#txtSInmdepo").val(result.objJob.DepoName);
                    // END Freight Info - Bill Of Lading

                    // Enable Form
                    EnableseaimportForm();
                }
            }
        });
    }

    function EnableseaimportForm() {
        // Load Status
        $("#rbSIfcl").removeAttr("disabled");
        $("#rbSIlcl").removeAttr("disabled");
        // Enable if Not Have SubJob or In SubJob Mode
        //if ($("#txtSItotalsubship").val() == "00" || subJobNumber > 0) {
        //    // ContainerStatus
        //    $("#optSIgroupage").removeAttr("disabled");
        //    $("#optSIpartof").removeAttr("disabled");
        //    $("#optSInone").removeAttr("disabled");
        //    // Type Of Service
        //    $("#rbSIfcl_fcl").removeAttr("disabled");
        //    $("#rbSIfcl_lcl").removeAttr("disabled");
        //    $("#rbSIlcl_fcl").removeAttr("disabled");
        //    $("#rbSIlcl_lcl").removeAttr("disabled");
        //    $("#rbSIlcl_lcl_g").removeAttr("disabled");
        //}
        $("#optSIconversionyes").removeAttr("disabled");
        $("#optSIconversionno").removeAttr("disabled");

        // ShipmentStatus
        $("#optSIfreehand").removeAttr("disabled");
        $("#optSInominate").removeAttr("disabled");
        $("#optSIcorporate").removeAttr("disabled");
        // Job Status
        $("#optSIjobLCLConsole").removeAttr("disabled");
        $("#optSIjobLCLCoload").removeAttr("disabled");
        $("#optSIjobLCLSubmarine").removeAttr("disabled");
        $("#optSIjobFCL").removeAttr("disabled");
        $("#optSIjobOtherservice").removeAttr("disabled");

        $("#txtSIrefSI").removeAttr("disabled");
        $("#txtSIDateSI").removeAttr("disabled");
        $("#txtSIgrorigin").removeAttr("disabled");

        // Agent
        $("#btnSIagent").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmagent").removeAttr("disabled");
        $("#txtSIagentaddress").removeAttr("disabled");
        // Delivery
        $("#btnSIdelivery").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmdelivery").removeAttr("disabled");
        $("#txtSIdeliveryaddress").removeAttr("disabled");
        // Transipment
        $("#btnSItranshipment").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmtranshipment").removeAttr("disabled");
        $("#txtSItranshipaddress").removeAttr("disabled");
        // Shipper
        $("#txtSInmshipper").removeAttr("disabled");
        $("#txtSIshipperaddress").removeAttr("disabled");
        // Consignee
        $("#btnSIconsignee").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmconsignee").removeAttr("disabled");
        $("#txtSIconsigneeaddress").removeAttr("disabled");
        // Party
        $("#btnSInparty").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmnparty").removeAttr("disabled");
        $("#txtSInpartyaddress").removeAttr("disabled");

        // Freight Info - Vessel
        // Place of Receipt
        $("#btnSIplaceofreceipt").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmplaceofreceipt").removeAttr("disabled");
        // Port of Discharge
        $("#btnSIportofdischarge").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmportofdischarge").removeAttr("disabled");
        // Place of Delivery
        $("#btnSIplaceofdelivery").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmplaceofdelivery").removeAttr("disabled");
        // Port of Loading
        $("#btnSIportofloading").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#txtSInmportofloading").removeAttr("disabled");

        $("#txtSIfeedervessel").removeAttr("disabled");
        $("#txtSIvoyfeedervessel").removeAttr("disabled");
        $("#txtSIfeedervesseletd").removeAttr("disabled");

        $("#txtSIconectvessel").removeAttr("disabled");
        $("#txtSIvoyconnectvessel").removeAttr("disabled");
        $("#txtSIconnectingetd").removeAttr("disabled");
        $("#btnSIconnectvessel").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');

        $("#txtSImothervessel").removeAttr("disabled");
        $("#txtSIvoymothervessel").removeAttr("disabled");
        $("#txtSIetamothervessel").removeAttr("disabled");
        // END Freight Info - Vessel

        // Freight Info - Description
        $("#txtSIdesc").removeAttr("disabled");
        // End Freight Info - Description

        // Freight Info - Freight
        // OBL Status
        $("#optSIoblprepaid").removeAttr("disabled");
        $("#optSIoblcollect").removeAttr("disabled");
        // HBL Status
        $("#optSIhblprepaid").removeAttr("disabled");
        $("#optSIhblcollect").removeAttr("disabled");

        $("#txtSIoblamount").removeAttr("disabled");
        $("#txtSIhblamount").removeAttr("disabled");
        // End Freight Info - Freight

        $("#txtSIhouseblno").removeAttr("disabled");
        $("#txtSIsecondhblno").removeAttr("disabled");
        $("#txtSIoceanmstblnr").removeAttr("disabled");
        $("#txtSIvolumebl").removeAttr("disabled");
        $("#txtSIvolumeinvoice").removeAttr("disabled");
        $("#btnSIssline").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#btnSIemkl").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        $("#btnSIdepo").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');

        // Bill Of Lading
        $('#txtSIBLblno').removeAttr("disabled");
        $('#btnSIBLblform').removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
        // NoOfBL
        $("#optSIBLnoofblNone").removeAttr("disabled");
        $("#optSIBLnoofblThree").removeAttr("disabled");
        $("#optSIBLnoofblSeawaybill").removeAttr("disabled");

        $("#txtSIBLtotalcont").removeAttr("disabled");
        $("#optSIBLciNotcovered").removeAttr("disabled");
        $("#optSIBLciCovered").removeAttr("disabled");
        $("#txtSIBLfreightamount").removeAttr("disabled");
        $("#txtSIBLpayableat").removeAttr("disabled");
        $("#txtSIBLdesc").removeAttr("disabled");
        // End Bill Of Lading

        // Shipping Instruction
        // Attention
        $("#txtSISIattention").removeAttr("disabled");
        // Special Instruction
        $("#txtSISIspecialintruction").removeAttr("disabled");
        // OriginalBL
        $("#optSISIoriginalblNone").removeAttr("disabled");
        $("#optSISIoriginalblThree").removeAttr("disabled");
        $("#optSISIoriginalblSeawaybill").removeAttr("disabled");

        // Shipment Advice
        $("#txtSISAremarks").removeAttr("disabled");
        // End Shipment Advice

        // Telex Release
        // Original
        $("#optSITSfullsetoriginal").removeAttr("disabled");
        $("#optSITSfullsetcopy").removeAttr("disabled");
        // Seawaybill
        $("#optSITSrsTelexRelease").removeAttr("disabled");
        $("#optSITSrsSeawaybill").removeAttr("disabled");
        // End Telex Release
    }

    // Marketing Section based on Shipment Status
    $("input[name=shipmentstatus]").click(function () {
        var xxx = $("input[name=shipmentstatus]");
        xxx.each(function () {
            if ($(this).is(':checked')) {
                var id = $(this).attr('id');
                switch (id) {
                    case 'optSIfreehand':
                        $("#btnSImarketing").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                        break;
                    default:
                        $("#btnSImarketing").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
                        $("#txtSIkdmarketing").val('').data("kode", "0").data("companycode", "0");
                        $("#txtSInmmarketing").val('');
                        break;
                }
            }
        });
    });

    // Freight Info - Freight OBL radio Selection
    $("input[name=freightobl]").click(function () {
        var xxx = $("input[name=freightobl]");
        xxx.each(function () {
            if ($(this).is(':checked')) {
                var id = $(this).attr('id');
                switch (id) {
                    case 'optSIoblcollect':
                        $("#btnSIoblcollect").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                        var tempckode = $("#btnSIoblcollect").data("tempkode");
                        var tempcint = $("#btnSIoblcollect").data("tempcint");
                        var tempcname = $("#btnSIoblcollect").data("tempcname");
                        $("#txtSIkdoblcollect").val(tempcint).data("kode", tempckode);
                        $("#txtSInmoblcollect").val(tempcname);
                        $("#btnSIoblpayable").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                        var temppkode = $("#btnSIoblpayable").data("tempkode");
                        var tempaname = $("#btnSIoblpayable").data("tempaname");
                        $("#txtSIkdoblpayable").val(temppkode).data("kode", temppkode);
                        $("#txtSInmoblpayable").val(tempaname);

                        // Shipping Instruction
                        $('#optSISIfreightprepaid').removeAttr("checked");
                        $('#optSISIfreightprepaid').attr("disabled", "disabled");
                        $('#optSISIfreightcollect').removeAttr("disabled");
                        $('#optSISIfreightcollectat').removeAttr("disabled");
                        $('#optSISIfreightcollect').attr("checked", "checked");
                        // End Shipping Instruction
                        break;
                    default:
                        $("#btnSIoblcollect").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
                        $("#txtSIkdoblcollect").val('').data("kode", "0");
                        $("#txtSInmoblcollect").val('');
                        $("#btnSIoblpayable").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
                        $("#txtSIkdoblpayable").val('').data("kode", "0");
                        $("#txtSInmoblpayable").val('');

                        // Shipping Instruction
                        $('#optSISIfreightprepaid').attr("checked", "checked");
                        $('#optSISIfreightprepaid').removeAttr("disabled");
                        $('#optSISIfreightcollect').attr("disabled", "disabled");
                        $('#optSISIfreightcollectat').attr("disabled", "disabled");
                        $('#optSISIfreightcollect').removeAttr("checked");
                        // End Shipping Instruction
                        break;
                }
            }
        });
    });

    // Freight Info - Freight HBL radio Selection
    $("input[name=freighthbl]").click(function () {
        var xxx = $("input[name=freighthbl]");
        xxx.each(function () {
            if ($(this).is(':checked')) {
                var id = $(this).attr('id');
                switch (id) {
                    case 'optSIhblcollect':
                        $("#btnSIhblcollect").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                        var tempckode = $("#btnSIhblcollect").data("tempkode");
                        var tempcint = $("#btnSIhblcollect").data("tempcint");
                        var tempcname = $("#btnSIhblcollect").data("tempcname");
                        $("#txtSIkdhblcollect").val(tempcint).data("kode", tempckode);
                        $("#txtSInmhblcollect").val(tempcname);
                        $("#btnSIhblpayable").removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
                        var temppkode = $("#btnSIhblpayable").data("tempkode");
                        var tempaname = $("#btnSIhblpayable").data("tempaname");
                        $("#txtSIkdhblpayable").val(temppkode).data("kode", temppkode);
                        $("#txtSInmhblpayable").val(tempaname);
                        break;
                    default:
                        $("#btnSIhblcollect").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
                        $("#txtSIkdhblcollect").val('').data("kode", "0");
                        $("#txtSInmhblcollect").val('');
                        $("#btnSIhblpayable").attr("disabled", "disabled").removeClass('ui-state-default').addClass('ui-state-disabled');
                        $("#txtSIkdhblpayable").val('').data("kode", "0");
                        $("#txtSInmhblpayable").val('');
                        break;
                }
            }
        });
    });

    // Shipping Instruction - Freight OBL radio selection
    // Collect
    $("#optSISIfreightcollect").click(function () {
        if ($(this).is(':checked')) {
            $('#btnSISIcollectat').attr('disabled', 'disabled').removeClass('ui-state-default').addClass('ui-state-disabled');
            $('#txtSISIcollectataddress').attr('disabled', 'disabled');
        }
    });
    // Collect At
    $("#optSISIfreightcollectat").click(function () {
        if ($(this).is(':checked')) {
            $('#btnSISIcollectat').removeAttr("disabled").addClass('ui-state-default').removeClass('ui-state-disabled');
            $('#txtSISIcollectataddress').removeAttr("disabled");
        }
    });
    // END Shipping Instruction - Freight OBL radio selection

    // ------------------------------------------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------- LOOKUP -------------------------------------------------------------
    // Shipment Order - Agent, Delivery, Tranship
    // Browse Agent
    $('#btnSIagent').click(function () {
        if ($("#ddlSIjobowner").val() == "0") {
            $.messager.alert('Information', "Please Choose The Owner Of This Job First...!!", 'info');
            return;
        }
        vlookupSOagent = 'agent'
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_agent');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsAgent', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_agent').dialog('open');
    });
    // Browse Delivery
    $('#btnSIdelivery').click(function () {
        if ($("#ddlSIjobowner").val() == "0") {
            $.messager.alert('Information', "Please Choose The Owner Of This Job First...!!", 'info');
            return;
        }
        vlookupSOagent = 'delivery'
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_agent');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsAgent', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_agent').dialog('open');
    });
    // Browse Tranship
    $('#btnSItranshipment').click(function () {
        if ($("#ddlSIjobowner").val() == "0") {
            $.messager.alert('Information', "Please Choose The Owner Of This Job First...!!", 'info');
            return;
        }
        vlookupSOagent = 'transhipment'
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_agent');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsAgent', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_agent').dialog('open');
    });
    // Cancel or Close
    $('#lookup_btn_cancel_so_agent').click(function () {
        $('#lookup_div_so_agent').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_agent').click(function () {
        var id = jQuery("#lookup_table_so_agent").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_agent").jqGrid('getRowData', id);
            switch (vlookupSOagent) {
                case "agent":
                    $('#txtSIkdagent').val(ret.code).data("kode", id);
                    $('#txtSInmagent').val(ret.name);
                    $('#txtSIagentaddress').val(ret.address);
                    // transhipment
                    $('#txtSIkdtranshipment').val(ret.code).data("kode", id);
                    $('#txtSInmtranshipment').val(ret.name);
                    $('#txtSItranshipaddress').val(ret.address);

                    createAgentTable($("#lookup_table_freight_obl_payable"));
                    break;
                case "transhipment":
                    $('#txtSIkdtranshipment').val(ret.code).data("kode", id);
                    $('#txtSInmtranshipment').val(ret.name);
                    $('#txtSItranshipaddress').val(ret.address);

                    createAgentTable($("#lookup_table_freight_obl_payable"));
                    break;
            }


            var mydata = [
                    { "id": ret.code, "cell": [ret.code, ret.name] }
            ];

            $("#grid").setGridParam({ data: mydata }).trigger("reloadGrid");

            $('#lookup_div_so_agent').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_agent").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        postData: { 'jobowner': function () { return $("#ddlSIjobowner").val(); } },
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_so_agent'),
        rowNum: 50,
        scrollrows: true,
        viewrecords: true,
        sortname: "contactcode",
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_agent").jqGrid('navGrid', '#toolbar_lookup_table_so_agent', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Agent, Delivery, Tranship

    // ------------------------------------------------------------------ Shipment Order - Consignee, NParty
    // Browse Consignee
    $('#btnSIconsignee').click(function () {
        vlookupSOconsignee = 'consignee';
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_consignee');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsConsignee', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_consignee').dialog('open');
    });
    // Browse NParty
    $('#btnSInparty').click(function () {
        vlookupSOconsignee = 'nparty';
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_consignee');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsConsignee', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_consignee').dialog('open');
    });
    // Cancel Or Close
    $('#lookup_btn_cancel_so_consignee').click(function () {
        $('#lookup_div_so_consignee').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_consignee').click(function () {
        var id = jQuery("#lookup_table_so_consignee").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_consignee").jqGrid('getRowData', id);
            switch (vlookupSOconsignee) {
                case "consignee":
                    $('#txtSIkdconsignee').val(ret.code).data("kode", id);
                    $('#txtSInmconsignee').val(ret.name);
                    $('#txtSIconsigneeaddress').val(ret.address);

                    $('#txtSIkdnparty').val(ret.code).data("kode", id);
                    $('#txtSInmnparty').val(ret.name);
                    $('#txtSInpartyaddress').val(ret.address);

                    createConsigneeTable($("#lookup_table_freight_hbl_payable"));
                    break;
                case "nparty":
                    $('#txtSIkdnparty').val(ret.code).data("kode", id);
                    $('#txtSInmnparty').val(ret.name);
                    $('#txtSInpartyaddress').val(ret.address);

                    createConsigneeTable($("#lookup_table_freight_hbl_payable"));
                    break;
            }
            $('#lookup_div_so_consignee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_consignee").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', '', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'companystatus', index: 'companystatus', width: 200, hidden: true },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'a.address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_so_consignee'),
        rowNum: 50,
        scrollrows: true,
        viewrecords: true,
        sortname: "contactcode",
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_consignee").jqGrid('navGrid', '#toolbar_lookup_table_so_consignee', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Consignee, NParty

    // ------------------------------------------------------------------ Shipment Order - Marketing
    // Browse 
    $('#btnSImarketing').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_marketing');
        lookupGrid.setGridParam({ url: base_url + 'User/LookUpMarketing', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_marketing').dialog('open');
    });
    // Cancel or Close
    $('#lookup_btn_cancel_so_marketing').click(function () {
        $('#lookup_div_so_marketing').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_marketing').click(function () {
        var id = jQuery("#lookup_table_so_marketing").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_marketing").jqGrid('getRowData', id);
            $('#txtSIkdmarketing').val(ret.code).data("kode", id).data("companycode", ret.companycode);
            $('#txtSInmmarketing').val(ret.name);

            $('#lookup_div_so_marketing').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_marketing").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Title', ''],
        colModel: [{ name: 'code', index: 'usercode', width: 100 },
                  { name: 'name', index: 'name', width: 320 },
                  { name: 'title', index: 'a.title', search: false },
                  { name: 'companycode', index: 'companycode', hidden: true }],
        pager: jQuery('#lookup_pager_so_marketing'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortname: "usercode",
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_marketing").jqGrid('navGrid', '#toolbar_lookup_table_so_marketing', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Marketing

    // --------------------------- Shipment Order - City - PlaceOfReceipt, PlaceOfDelivery, ConnectingVessel, Freight OBL Collect At, Freight HBL Collect At
    // Browse PlaceOfReceipt
    $('#btnSIplaceofreceipt').click(function () {
        vlookupSOcity = "placeofreceipt";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_city').dialog('open');
    });

    // Browse PlaceOfDelivery
    $('#btnSIplaceofdelivery').click(function () {
        vlookupSOcity = "placeofdelivery";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_city').dialog('open');
    });

    // Browse Vessel
    $('input[name="btnSIvesselcity"]').live('click', function () {
        vlookupSOcity = "vessel";
        vlookupSOvesselIndex = $(this).closest('td').parent()[0].sectionRowIndex;
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_city').dialog('open');
    });

    // Browse Freight OBL Collect At
    $('#btnSIoblcollect').click(function () {
        vlookupSOcity = "oblcollectat";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_city').dialog('open');
    });

    // Browse Freight HBL Collect At
    $('#btnSIhblcollect').click(function () {
        vlookupSOcity = "hblcollectat";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_city');
        lookupGrid.setGridParam({ url: base_url + 'City/LookUpCities', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_city').dialog('open');
    });
    // Cancel or Close
    $('#lookup_btn_cancel_so_city').click(function () {
        $('#lookup_div_so_city').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_city').click(function () {
        var id = jQuery("#lookup_table_so_city").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_city").jqGrid('getRowData', id);
            switch (vlookupSOcity) {
                case "placeofreceipt":
                    $('#txtSIkdplaceofreceipt').val(ret.int).data("kode", ret.code);
                    $('#txtSInmplaceofreceipt').val(ret.name);
                    break;
                case "placeofdelivery":
                    $('#txtSIkdplaceofdelivery').val(ret.int).data("kode", ret.code);
                    $('#txtSInmplaceofdelivery').val(ret.name);

                    // Freight HBL 
                    $("#btnSIhblcollect").data("tempkode", ret.code);
                    $("#btnSIhblcollect").data("tempcint", ret.int);
                    $("#btnSIhblcollect").data("tempcname", ret.name);
                    if ($('#optSIhblcollect').is(':checked')) {
                        $('#txtSIkdhblcollect').val(ret.int).data("kode", ret.code);
                        $('#txtSInmhblcollect').val(ret.name);
                    }

                    // Freight OBL 
                    $("#btnSIoblcollect").data("tempkode", ret.code);
                    $("#btnSIoblcollect").data("tempcint", ret.int);
                    $("#btnSIoblcollect").data("tempcname", ret.name);
                    if ($('#optSIoblcollect').is(':checked')) {
                        $('#txtSIkdoblcollect').val(ret.int).data("kode", ret.code);
                        $('#txtSInmoblcollect').val(ret.name);
                    }
                    //alert($("#btnSIoblcollect").data("tempkode"));
                    break;
                case "vessel":
                    $('#tblVessel tr:eq(' + vlookupSOvesselIndex + ')').find('input[name="txtSIvesselintcity"]').val(ret.int).data("kode", ret.code);
                    $('#tblVessel tr:eq(' + vlookupSOvesselIndex + ')').find('input[name="txtSInmvesselcity"]').val(ret.name);
                    break;
                case "oblcollectat":
                    $('#txtSIkdoblcollect').val(ret.int).data("kode", ret.code);
                    $('#txtSInmoblcollect').val(ret.name);
                    break;
                case "hblcollectat":
                    $('#txtSIkdhblcollect').val(ret.int).data("kode", ret.code);
                    $('#txtSInmhblcollect').val(ret.name);
                    break;
            }

            $('#lookup_div_so_city').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_city").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Int'],
        colModel: [{ name: 'code', index: 'citycode', width: 100, align: "center" },
                  { name: 'name', index: 'cityname', width: 320 },
                  { name: 'int', index: 'intcity' }],
        pager: jQuery('#lookup_pager_so_city'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_city").jqGrid('navGrid', '#toolbar_lookup_table_so_city', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - City - PlaceOfReceipt, PlaceOfDelivery, ConnectingVessel, Freight OBL Collect At, Freight HBL Collect At

    // ------------------------------------------------------------------ Shipment Order - Port - PortOfLoading, PortOfDischarge
    // Browse PortOfLoading
    $('#btnSIportofloading').click(function () {
        vlookupSOport = "portofloading";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_port');
        lookupGrid.setGridParam({ url: base_url + 'Port/LookUpPorts', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_port').dialog('open');
    });
    // Browse PortOfLoading
    $('#btnSIportofdischarge').click(function () {
        vlookupSOport = "portofdischarge";
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_port');
        lookupGrid.setGridParam({ url: base_url + 'Port/LookUpPorts', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_port').dialog('open');
    });
    // Cancel or Close
    $('#lookup_btn_cancel_so_port').click(function () {
        $('#lookup_div_so_port').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_port').click(function () {
        var id = jQuery("#lookup_table_so_port").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_port").jqGrid('getRowData', id);
            switch (vlookupSOport) {
                case "portofloading":
                    $('#txtSIkdportofloading').val(ret.int).data("kode", ret.code);
                    $('#txtSInmportofloading').val(ret.name);
                    break;
                case "portofdischarge":
                    $('#txtSIkdportofdischarge').val(ret.int).data("kode", ret.code);
                    $('#txtSInmportofdischarge').val(ret.name);

                    // Set Place Of Delivery
                    $('#txtSIkdplaceofdelivery').val(ret.intcity).data("kode", ret.citycode);
                    $('#txtSInmplaceofdelivery').val(ret.cityname);

                    break;
            }

            $('#lookup_div_so_port').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_port").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', 'Int', 'CityName', 'IntCity', 'CountryName', 'IntCountry', 'CityCode'],
        colModel: [{ name: 'code', index: 'portcode', width: 100, align: "center" },
                  { name: 'name', index: 'portname', width: 320 },
                  { name: 'int', index: 'intport', hidden: true },
                  { name: 'cityname', index: 'a.cityname', hidden: true },
                  { name: 'intcity', index: 'a.intcity', hidden: true },
                  { name: 'countryname', index: 'a.countryname', hidden: true },
                  { name: 'intcountry', index: 'a.intcountry', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true }
        ],
        pager: jQuery('#lookup_pager_so_port'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_port").jqGrid('navGrid', '#toolbar_lookup_table_so_port', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Port - PortOfLoading, PortOfDischarge

    // ------------------------------------------------------------------ Shipment Order - SSLine
    // Browse
    $('#btnSIssline').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_ssline');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsSSline', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_ssline').dialog('open');
    });
    // Cancel or Close
    $('#lookup_btn_cancel_so_ssline').click(function () {
        $('#lookup_div_so_ssline').dialog('close');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_ssline').click(function () {
        var id = jQuery("#lookup_table_so_ssline").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_ssline").jqGrid('getRowData', id);
            $('#txtSIkdssline').val(ret.code).data("kode", id);
            $('#txtSInmssline').val(ret.name);

            $('#lookup_div_so_ssline').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_ssline").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_so_ssline'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_ssline").jqGrid('navGrid', '#toolbar_lookup_table_so_ssline', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - SSLine

    // ------------------------------------------------------------------ Shipment Order - Depo
    // Browse
    $('#btnSIdepo').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_depo');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsDepo', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_depo').dialog('open');
    });

    // Cancel or Close
    $('#lookup_btn_cancel_so_depo').click(function () {
        $('#lookup_div_so_depo').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_so_depo').click(function () {
        var id = jQuery("#lookup_table_so_depo").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_depo").jqGrid('getRowData', id);
            $('#txtSIkddepo').val(ret.code).data("kode", id);
            $('#txtSInmdepo').val(ret.name);

            $('#lookup_div_so_depo').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_depo").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_so_depo'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_depo").jqGrid('navGrid', '#toolbar_lookup_table_so_depo', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Depo

    // ------------------------------------------------------------------ Shipment Order - EMKL
    // Browse
    $('#btnSIemkl').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_emkl');
        lookupGrid.setGridParam({ url: base_url + 'Contact/LookUpContactAsEMKL', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_emkl').dialog('open');
    });

    // Cancel or CLose
    $('#lookup_btn_cancel_so_emkl').click(function () {
        $('#lookup_div_so_emkl').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_so_emkl').click(function () {
        var id = jQuery("#lookup_table_so_emkl").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_emkl").jqGrid('getRowData', id);
            $('#txtSIkdemkl').val(ret.code).data("kode", id);
            $('#txtSInmemkl').val(ret.name);

            $('#lookup_div_so_emkl').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_so_emkl").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'Name', '', '', '', 'City'],
        colModel: [{ name: 'code', index: 'contactcode', width: 80, align: "center" },
                  { name: 'name', index: 'contactname', width: 200 },
                  { name: 'address', index: 'address', hidden: true },
                  { name: 'citycode', index: 'a.citycode', hidden: true },
                  { name: 'intcity', index: 'intcity', hidden: true },
                  { name: 'cityname', index: 'cityname' }],
        pager: jQuery('#lookup_pager_so_emkl'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        sortname: "contactcode",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_emkl").jqGrid('navGrid', '#toolbar_lookup_table_so_emkl', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - EMKL

    // ------------------------------------------------------------------ Bill Of Lading - BL Form
    // Browse
    $('#btnSIBLblform').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_bl_blform');
        lookupGrid.setGridParam({ url: base_url + 'BillOfLading/LookUpBLSea', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_bl_blform').dialog('open');
    });

    // Cancel or Close
    $("#lookup_btn_cancel_bl_blform").click(function () {
        $('#lookup_div_bl_blform').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_bl_blform').click(function () {
        var id = jQuery("#lookup_table_bl_blform").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_bl_blform").jqGrid('getRowData', id);
            $('#txtSIBLkdblform').val(ret.fromcode).data("kode", ret.code);
            $('#txtSIBLnmblform').val(ret.fromname);

            $('#lookup_div_bl_blform').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    jQuery("#lookup_table_bl_blform").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['BLNo', 'ShipmentBy', 'BLFormCode', 'BLFormName'],
        colModel: [{ name: 'code', index: 'BLCodeId', width: 80, align: "center" },
                  { name: 'shipmentby', index: 'ShipmentBy', hidden: true },
                  { name: 'fromcode', index: 'BLFormCode', width: 100 },
                  { name: 'fromname', index: 'BLFormName', width: 320 }],
        pager: jQuery('#lookup_pager_bl_blform'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_bl_blform").jqGrid('navGrid', '#toolbar_lookup_table_bl_blform', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Bill Of Lading - BL Form

    // ---------------------------- Shipment Order - Freight OBL Payable At, Shipping Instruction - Freight OBL Collect At
    var agentselection = "";
    // Browse
    $("#btnSIoblpayable").click(function () {
        agentselection = "soobl";
        $('#lookup_div_so_freight_obl_payable').dialog('open');
    });

    // Browse Shipping Instruction - Freight OBL Collect At
    $("#btnSISIcollectat").click(function () {
        agentselection = "sibobl";
        $('#lookup_div_so_freight_obl_payable').dialog('open');
    });

    // Cancel or Close
    $("#lookup_btn_cancel_so_freight_obl_payable").click(function () {
        $('#lookup_div_so_freight_obl_payable').dialog('close');
    });

    // ADD or Select Data
    $("#lookup_btn_add_so_freight_obl_payable").click(function () {
        var id = $("#lookup_table_freight_obl_payable tr.selected td:eq(0)").text().trim();
        var code = $("#lookup_table_freight_obl_payable tr.selected td:eq(1)").text().trim();
        var name = $("#lookup_table_freight_obl_payable tr.selected td:eq(2)").text().trim();

        switch (agentselection) {
            case 'soobl':
                $('#txtSIkdoblpayable').val(code).data("kode", id);
                $('#txtSInmoblpayable').val(name);
                // Save to temp data for updating Prepaid or Collect
                $("#btnSIoblpayable").data("tempkode", id).data("tempaname", name);
                break;
            case 'sibobl':
                $('#txtSISIkdcollectat').val(code).data("kode", id);
                $('#txtSISInmcollectat').val(name);
        }


        $('#lookup_div_so_freight_obl_payable').dialog('close');
    });

    // Selection
    $("#lookup_table_freight_obl_payable tr.tableRow").live("click", function () {
        $("#lookup_table_freight_obl_payable tr.tableRow").each(function () {
            $(this).removeClass("selected");
        });
        $(this).removeClass("selected").addClass("selected");
    });

    function createAgentTable(tbody) {
        if (tbody == null || tbody.length < 1) return;
        // Clear 
        $("#lookup_table_freight_obl_payable tr.tableRow").each(function () {
            $(this).remove();
        });

        var trow = $("<tr>").addClass("tableRow").addClass('ui-widget-content');
        $("<td>").addClass("tableCell").text($("#txtSIkdagent").data('kode')).appendTo(trow);
        $("<td>").addClass("tableCell").text($("#txtSIkdagent").val()).appendTo(trow);
        $("<td>").addClass("tableCell").text($("#txtSInmagent").val()).appendTo(trow);
        trow.appendTo(tbody);

        if ($("#txtSIkdagent").val().trim() != $("#txtSIkdtranshipment").val().trim()) {
            trow = $("<tr>").addClass("tableRow");
            $("<td>").addClass("tableCell").text($("#txtSIkdtranshipment").data('kode')).appendTo(trow);
            $("<td>").addClass("tableCell").text($("#txtSIkdtranshipment").val()).appendTo(trow);
            $("<td>").addClass("tableCell").text($("#txtSInmtranshipment").val()).appendTo(trow);
            trow.appendTo(tbody);
        }
    }
    // End Shipment Order - Freight OBL Payable At

    // ------------------------------------------------------------------ Shipment Order - Freight HBL Payable At
    // Browse
    $("#btnSIhblpayable").click(function () {
        $('#lookup_div_so_freight_hbl_payable').dialog('open');
    });

    // Cancel or Close
    $("#lookup_btn_cancel_so_freight_hbl_payable").click(function () {
        $('#lookup_div_so_freight_hbl_payable').dialog('close');
    });

    // ADD or Slect Data
    $("#lookup_btn_add_so_freight_hbl_payable").click(function () {
        var id = $("#lookup_table_freight_hbl_payable tr.selected td:eq(0)").text().trim();
        var code = $("#lookup_table_freight_hbl_payable tr.selected td:eq(1)").text().trim();
        var name = $("#lookup_table_freight_hbl_payable tr.selected td:eq(2)").text().trim();

        $('#txtSIkdhblpayable').val(code).data("kode", id);
        $('#txtSInmhblpayable').val(name);
        // Save to temp data for updating Prepaid or Collect
        $("#btnSIhblpayable").data("tempkode", id).data("tempaname", name);

        $('#lookup_div_so_freight_hbl_payable').dialog('close');
    });

    // Selection
    $("#lookup_table_freight_hbl_payable tr.tableRow").live("click", function () {
        $("#lookup_table_freight_hbl_payable tr.tableRow").each(function () {
            $(this).removeClass("selected");
        });
        $(this).removeClass("selected").addClass("selected");
    });

    function createConsigneeTable(tbody) {
        if (tbody == null || tbody.length < 1) return;
        // Clear 
        $("#lookup_table_freight_hbl_payable tr.tableRow").each(function () {
            $(this).remove();
        });

        var trow = $("<tr>").addClass("tableRow").addClass('ui-widget-content');
        $("<td>").addClass("tableCell").text($("#txtSIkdconsignee").data('kode')).appendTo(trow);
        $("<td>").addClass("tableCell").text($("#txtSIkdconsignee").val()).appendTo(trow);
        $("<td>").addClass("tableCell").text($("#txtSInmconsignee").val()).appendTo(trow);
        trow.appendTo(tbody);

        if ($("#txtSIkdconsignee").val().trim() != $("#txtSIkdnparty").val().trim()) {
            trow = $("<tr>").addClass("tableRow");
            $("<td>").addClass("tableCell").text($("#txtSIkdnparty").data('kode')).appendTo(trow);
            $("<td>").addClass("tableCell").text($("#txtSIkdnparty").val()).appendTo(trow);
            $("<td>").addClass("tableCell").text($("#txtSInmnparty").val()).appendTo(trow);
            trow.appendTo(tbody);
        }

    }
    // End Shipment Order - Freight Payable At, Shipping Instruction - Freight OBL Collect At

    // ------------------------------------------------------------------ Shipment Order - Container
    // Browse
    $('#btncontainerSEcontainerno').click(function () {
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_container');
        lookupGrid.setGridParam({ url: base_url + 'lookupContainer/GetAllSeaImportContainer', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_container').dialog('open');
    });
    // ADD or Select Data
    $('#lookup_btn_add_so_container').click(function () {
        var id = jQuery("#lookup_table_so_container").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_container").jqGrid('getRowData', id);
            $('#containerSEcontainerno').val(ret.ContainerNo);
            $('#containerSEsealno').val(ret.SealNo);
            switch (ret.Size) {
                case "20": $('#containerSEsize').val('1'); break;
                case "40": $('#containerSEsize').val('2'); break;
                case "45": $('#containerSEsize').val('3'); break;
            }
            switch (ret.Type) {
                case 'GP': $('#containerSEtype').val('1'); break;
                case 'HC': $('#containerSEtype').val('2'); break;
                case 'RF': $('#containerSEtype').val('3'); break;
                case 'FR': $('#containerSEtype').val('4'); break;
                case 'OT': $('#containerSEtype').val('5'); break;
                case 'GOH': $('#containerSEtype').val('6'); break;
                case 'ISO': $('#containerSEtype').val('7'); break;
            }
            $('#containerSEgrossweight').val(ret.GrossWeight);
            $('#containerSEnetweight').val(ret.NetWeight);
            $('#containerSEcbm').val(ret.CBM);
            $('#containerSEcommodity').val(ret.Commodity);
            $('#containerSEnoofpieces').val(ret.NoOfPieces);
            $('#containerSEpackaging').val(ret.PackagingCode);

            switch (ret.PartOf) {
                case 'true': $('#containerSEpartof').val('0'); break;
                case 'false': $('#containerSEpartof').val('1'); break;
            }

            $('#lookup_div_so_container').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // Close
    $("#lookup_btn_cancel_so_container").click(function () {
        $('#lookup_div_so_container').dialog('close');
    });

    jQuery("#lookup_table_so_container").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        postData: { jobNumber: jobNumber, subJobNumber: 0 },
        //loadonce:true,
        colNames: ['Container No', 'Seal No', 'Size', 'Type', 'GrossWeight', 'NetWeight', 'CBM', 'Commodity', 'NoOfPieces', 'PackagingCode', 'PartOf'],
        colModel: [{ name: 'ContainerNo', index: 'ContainerNo', width: 120 },
                  { name: 'SealNo', index: 'SealNo', width: 120 },
                  { name: 'Size', index: 'Size', width: 80 },
                  { name: 'Type', index: 'Type', width: 100 },
                  { name: 'GrossWeight', index: 'GrossWeight', width: 320, hidden: true },
                  { name: 'NetWeight', index: 'NetWeight', width: 320, hidden: true },
                  { name: 'CBM', index: 'CBM', width: 320, hidden: true },
                  { name: 'Commodity', index: 'Commodity', width: 320, hidden: true },
                  { name: 'NoOfPieces', index: 'NoOfPieces', width: 320, hidden: true },
                  { name: 'PackagingCode', index: 'PackagingCode', width: 320, hidden: true },
                  { name: 'PartOf', index: 'PartOf', width: 320, hidden: true }],
        pager: jQuery('#lookup_pager_so_container'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_container").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Container


    // ======================================================== Shipment Order - Vessel
    // Browse Vessel
    $('input[name="btnSIvessel"]').live('click', function () {
        vlookupSOvessel = $(this).closest('td').parent()[0].sectionRowIndex;
        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");
        var lookupGrid = $('#lookup_table_so_vessel');
        lookupGrid.setGridParam({ url: base_url + 'Vessel/LookUpVessel', postData: { filters: null }, search: false }).trigger("reloadGrid");
        $('#lookup_div_so_vessel').dialog('open');
    });

    // Cancel or Close
    $("#lookup_btn_cancel_so_vessel").click(function () {
        $('#lookup_div_so_vessel').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_so_vessel').click(function () {
        var id = jQuery("#lookup_table_so_vessel").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_so_vessel").jqGrid('getRowData', id);

            $('#tblVessel tr:eq(' + vlookupSOvessel + ')').find('input[name="txtSInmvessel"]').val(ret.name).data("kode", ret.code);

            $('#lookup_div_so_vessel').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // ADD NEW
    $('#lookup_btn_new_so_vessel').click(function () {
        $('#txtmstvesselname').val('');
        $('#txtmstintvessel').val('');

        $('#mst_vessel_form_div').dialog('open');
    });

    // Save Add New
    $('#mst_vessel_form_btn_save').click(function () {
        $.ajax({
            url: base_url + "vessel/InsertMstVessel",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                VesselName: $('#txtmstvesselname').val(),
                IntVessel: $('#txtmstintvessel').val()
            }),
            success: function (result) {
                $.messager.alert('Result', result, 'info', function () {
                    $('#mst_vessel_form_div').dialog('close');
                });

                var lookupGrid = $('#lookup_table_so_vessel');
                lookupGrid.setGridParam({ url: base_url + 'Vessel/LookUpVessel', postData: { filters: null }, search: false }).trigger("reloadGrid");
            }
        });
    });

    // Close Add New
    $('#mst_vessel_form_btn_cancel').click(function () {
        $('#mst_vessel_form_div').dialog('close');
    });

    jQuery("#lookup_table_so_vessel").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        //loadonce:true,
        colNames: ['Code', 'VesselName', 'Int'],
        colModel: [{ name: 'code', index: 'vesselid', width: 40, align: "center", hidden: true },
                  { name: 'name', index: 'vesselname' },
                  { name: 'intcode', index: 'intvessel', width: 40, hidden: true }],
        pager: jQuery('#lookup_pager_so_airline'),
        rowNum: 20,
        viewrecords: true,
        gridview: true,
        scroll: 1,
        sortorder: "asc",
        width: 460,
        height: 350
    });
    $("#lookup_table_so_vessel").jqGrid('navGrid', '#toolbar_lookup_table_bl_blform', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // End Shipment Order - Vessel


    // ======================================================== Shipment Order - Vessel
    // Save Vessel
    $('a[name="savevessel"]').live('click', function () {
        var rowIndex = $(this).closest('td').parent()[0].sectionRowIndex;
        var vesselId = $('#tblVessel tr:eq(' + rowIndex + ')').find('input[name="txtSInmvessel"]').data("kode");
        var vesselName = $('#tblVessel tr:eq(' + rowIndex + ')').find('input[name="txtSInmvessel"]').val();
        var vesselType = $('#tblVessel tr:eq(' + rowIndex + ')').find('select[name="ddlSEvesseltype"]').val();
        var vesselETD = $('#tblVessel tr:eq(' + rowIndex + ')').find('input[name="txtSIvesseletd"]').val();
        var voyage = $('#tblVessel tr:eq(' + rowIndex + ')').find('input[name="txtSIvesselvoy"]').val();
        var cityId = $('#tblVessel tr:eq(' + rowIndex + ')').find('input[name="txtSIvesselintcity"]').data("kode");

        var objSaveButton = $(this);
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "shipmentorder/InsertUpdateShipmentRouting",
            data: JSON.stringify({
                ShipmentId: shipmentId, Id: $(this).attr('rel'), VesselId: vesselId, VesselName: vesselName, VesselType: vesselType,
                ETD: vesselETD, Voyage: voyage, CityId: cityId
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        objSaveButton.attr('rel', result.objResult.Id);
                        //$(this).attr('rel', result.objResult.Id);
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // Delete Vessel
    $('a[name="deletevessel"]').live('click', function () {

        var delID = $(this).attr('rel');
        var rowIndex = $(this).closest('td').parent()[0].sectionRowIndex;
        $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
            if (r) {
                if (delID == undefined || delID == "") {
                    $('#tblVessel tr:eq(' + rowIndex + ')').remove();
                    // Reset Number
                    var i = 1;
                    $('#tblVessel tr').each(function () {
                        if (i > 1) {
                            $(this).find('td:eq(0)').html(i - 1);
                        }
                        i++;
                    });
                }
                else {
                    $.ajax({
                        contentType: "application/json",
                        type: 'POST',
                        url: base_url + "shipmentorder/deleteShipmentRouting",
                        data: JSON.stringify({
                            Id: delID
                        }),
                        success: function (result) {
                            if (result.isValid) {
                                $.messager.alert('Information', result.message, 'info', function () {
                                    $('#tblVessel tr:eq(' + rowIndex + ')').remove();
                                    // Reset Number
                                    var i = 1;
                                    $('#tblVessel tr').each(function () {
                                        if (i > 1) {
                                            $(this).find('td:eq(0)').html(i - 1);
                                        }
                                        i++;
                                    });
                                });
                            }
                            else {
                                $.messager.alert('Warning', result.message, 'warning');
                            }
                        }
                    });
                }
            }
        });
    });

    // New Vessel
    $('#newVessel').live('click', function () {
        var clonedRow = $('#tblVessel tr:eq(1)').clone();
        clonedRow.find('input').not('input[type="button"]').val('').data("kode", '');
        $('#tblVessel').append(clonedRow);

        $('#tblVessel tr:last td:eq(0)').html($('#tblVessel tr').length - 1);

        // ReInitiate ETD as datebox
        $('#tblVessel tr:last td:eq(4)').html('<input id="txtSIvesseletd" name="txtSIvesseletd" class="editable easyui-datebox" type="text" title="mm/dd/yyyy" size="8" maxlength="10" />');
        $('#tblVessel tr:last td:eq(4)').find('#txtSIvesseletd').datebox();

        // Reset ID
        $('#tblVessel tr:last td').find('a[name = "savevessel"]').attr('rel', '');
        $('#tblVessel tr:last td').find('a[name = "deletevessel"]').attr('rel', '');
    });

    // End Shipment Order - Vessel


    // ----------------------------------------------------------------- END LOOKUP ------------------------------------------------

    // Validation
    function formValidation() {
        var result = {};
        result.isValid = true;
        result.message = "";
        // Shipment Status
        if ($("#optSIfreehand").is(':checked')) {
            if ($('#txtSIkdmarketing').val().trim().length == 0) {
                result.message = 'Invalid Marketing Code...';
                result.isValid = false;
            }
        }
        // Agent, Delivery, Transhipment, Shipper, Consignee
        if ($('#txtSIkdagent').val().trim().length == 0 || $('#txtSInmagent').val().trim().length == 0) {
            result.message = 'Invalid Agent Code or Name...';
            result.isValid = false;
        }
        if ($('#txtSIkdtranshipment').val().trim().length == 0 || $('#txtSInmtranshipment').val().trim().length == 0) {
            result.message = 'Invalid Transhipment Code or Name...';
            result.isValid = false;
        }
        if ($('#txtSInmconsignee').val().trim().length == 0 || $('#txtSIkdconsignee').val().trim().length == 0) {
            result.message = 'Invalid Consignee Code or Name...';
            result.isValid = false;
        }
        // End Agent, Delivery, Transhipment, Shipper, Consignee

        // Feight Info - Vessel
        if ($('#txtSIfeedervesseletd').val() > $('#txtSIetamothervessel').val()) {
            result.message = "You can't revise date ETD..";
            result.isValid = false;
        }
        // End Feight Info - Vessel
        //// Volume
        //if ($('#txtSIvolumebl').val() < 0){
        //    result.message = 'Invalid Volume BL...';
        //    result.isValid = false;
        //}$('#txtSIvolumeinvoice').val())
        //// End Volume

        //alert($('#txtSIfeedervesseletd').val() +"---"+ $('#txtSIetamothervessel').val());
        result.isValid = true;
        return result;
    }
    // End Validation

    // SAVE
    $('#seaimport_form_btn_save').click(function () {
        var result = formValidation();
        if (!result.isValid) {
            $.messager.alert('Warning', result.message, 'warning');
        }
        else {
            //alert('SUBMIT..-' + $("#txtSIdesc").val());

            ShowLoadingImage();
            // Load Status
            var loadStatus = "FCL";
            if ($("#rbSIlcl").is(':checked')) {
                loadStatus = "LCL";
            }
            // ShipmentStatus
            var shipmentStatus = "F";
            if ($("#optSInominate").is(':checked')) {
                shipmentStatus = "N";
            }
            else if ($("#optSIcorporate").is(':checked')) {
                shipmentStatus = "C";
            }

            // JobStatus
            var jobStatus = "FCL";
            if ($("#optSIjobLCLConsole").is(':checked')) {
                jobStatus = "LCL Console";
            }
            else if ($("#optSIjobLCLCoload").is(':checked')) {
                jobStatus = "LCL Co-Load";
            }
            else if ($("#optSIjobLCLSubmarine").is(':checked')) {
                jobStatus = "LCL Submarine";
            }
            else if ($("#optSIjobOtherservice").is(':checked')) {
                jobStatus = "Other service";
            }

            HBLStatus = "P";
            if ($("#optSIhblcollect").is(':checked'))
                HBLStatus = "C";

            // Populate Sea Container
            var seaContainer = [];
            var seaContainerIdx = 0;
            $('#list_containerSE tr').each(function () {
                if (seaContainerIdx > 0) {
                    obj = {};
                    obj['ContainerNo'] = $.trim($(this).find('td:eq(0)').text());
                    obj['SealNo'] = $.trim($(this).find('td:eq(1)').text());
                    switch ($.trim($(this).find('td:eq(2)').text())) {
                        case '20': obj['Size'] = 1; break;
                        case '40': obj['Size'] = 2; break;
                        case '45': obj['Size'] = 3; break;
                    }
                    switch ($.trim($(this).find('td:eq(3)').text())) {
                        case 'GP': obj['Type'] = 1; break;
                        case 'HC': obj['Type'] = 2; break;
                        case 'RF': obj['Type'] = 3; break;
                        case 'FR': obj['Type'] = 4; break;
                        case 'OT': obj['Type'] = 5; break;
                        case 'GOH': obj['Type'] = 6; break;
                        case 'ISO': obj['Type'] = 7; break;
                    }
                    obj['GrossWeight'] = $.trim($(this).find('td:eq(4)').text()) == "" ? 0 : parseFloat($.trim($(this).find('td:eq(4)').text()));
                    obj['NetWeight'] = $.trim($(this).find('td:eq(5)').text()) == "" ? 0 : parseFloat($.trim($(this).find('td:eq(5)').text()));
                    obj['CBM'] = $.trim($(this).find('td:eq(6)').text()) == "" ? 0 : parseFloat($.trim($(this).find('td:eq(6)').text()));
                    switch ($.trim($(this).find('td:eq(10)').text())) {
                        case 'YES': obj['PartOf'] = true; break;
                        case 'NO': obj['PartOf'] = false; break;
                    }
                    obj['Commodity'] = $.trim($(this).find('td:eq(7)').text());
                    obj['PackagingCode'] = $.trim($(this).find('td:eq(9)').text());
                    obj['NoOfPieces'] = $.trim($(this).find('td:eq(8)').text()) == "" ? 0 : parseInt($.trim($(this).find('td:eq(8)').text()));
                    seaContainer.push(obj);
                }
                seaContainerIdx++;
            });

            // Shipping Instruction - OriginalBL
            var originalBL = "T";
            if ($('#optSISIoriginalblNone').is(':checked'))
                originalBL = "N";
            else if ($('#optSIBLoriginalblSeawaybill').is(':checked'))
                originalBL = "S";

            // Shipping Instruction - FreightOBL
            var freightOBL = "P";
            if ($('#optSISIfreightcollect').is(':checked'))
                freightOBL = "C";
            else if ($('#optSISIfreightcollectat').is(':checked'))
                freightOBL = "A";

            // Telex Release - full set / original
            var original = "O";
            if ($('#optSITSfullsetcopy').is(':checked'))
                original = "C";

            // Telex Release - Seawaybill
            var seawaybill = "T";
            if ($('#optSITSrsSeawaybill').is(':checked'))
                seawaybill = "S";

            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: base_url + "shipmentorder/Update",
                data: JSON.stringify({
                    Id: shipmentId,
                    JobNumber: jobNumber, SubJobNo: subJobNumber, JobOwner: $('#ddlSIjobowner').val(),
                    MarketCode: $('#txtSIkdmarketing').data("kode"), MarketCompany: $('#txtSIkdmarketing').data("companycode"), MarketName: $('#txtSInmmarketing').val(),
                    LoadStatus: loadStatus, ShipmentStatus: shipmentStatus, JobStatus: jobStatus,
                    SIReference: $('#txtSIrefSI').val(), SIDate: $('#txtSIDateSI').datebox('getValue'),
                    // Agent, Delivery etc
                    AgentCode: $('#txtSIkdagent').data('kode'), AgentName: $('#txtSInmagent').val(), AgentAddress: $('#txtSIagentaddress').val(),
                    TranshipmentCode: $('#txtSIkdtranshipment').data('kode'), TranshipmentName: $('#txtSInmtranshipment').val(), TranshipmentAddress: $('#txtSItranshipaddress').val(),
                    ShipperName: $('#txtSInmshipper').val(), ShipperAddress: $('#txtSIshipperaddress').val(),
                    ConsigneeCode: $('#txtSIkdconsignee').data('kode'), ConsigneeName: $('#txtSInmconsignee').val(), ConsigneeAddress: $('#txtSIconsigneeaddress').val(),
                    NPartyCode: $('#txtSIkdnparty').data('kode'), NPartyName: $('#txtSInmnparty').val(), NPartyAddress: $('#txtSInpartyaddress').val(),
                    // Vessel
                    LoadingPortCode: $('#txtSIkdportofloading').data("kode"), LoadingPortName: $('#txtSInmportofloading').val(),
                    ReceiptPlaceCode: $('#txtSIkdplaceofreceipt').data("kode"), ReceiptPlaceName: $('#txtSInmplaceofreceipt').val(),
                    DischargePortCode: $('#txtSIkdportofdischarge').data("kode"), DischargePortName: $('#txtSInmportofdischarge').val(),
                    DeliveryPlaceCode: $('#txtSIkdplaceofdelivery').data("kode"), DeliveryPlaceName: $('#txtSInmplaceofdelivery').val(),
                    ETD: $("#txtSIetd").datebox('getValue'), ETA: $("#txtSIeta").datebox('getValue'),
                    // Freight
                    HBLStatus: HBLStatus, HBLCollectCode: $("#txtSIkdhblcollect").data("kode"), HBLPayableCode: $("#txtSIkdhblpayable").data("kode"),
                    HBLCurrency: $('#ddlSIhblcurrency').val(), HBLAmount: $('#txtSIhblamount').val(),
                    // Container
                    SeaContainerList: seaContainer,
                    // Description
                    GoodDescription: $("#txtSIdesc").val(),
                    HouseBLNo: $('#txtSIhouseblno').val(), SecondBLNo: $('#txtSIsecondhblno').val(),
                    OceanMSTBLNo: $('#txtSIoceanmstblnr').val(), VolumeBL: $('#txtSIvolumebl').val(), Invoice: $('#txtSIvolumeinvoice').val(),
                    SSLineCode: $('#txtSIkdssline').data('kode'), EMKLCode: $('#txtSIkdemkl').data('kode'), DepoCode: $('#txtSIkddepo').data('kode'),
                    WareHouseName: $('#txtSIwarehousename').val(), KINS: $('#txtSIkins').val(), CFName: $('#txtSIcargofreightcompany').val()
                }),
                success: function (result) {
                    if (result.isValid) {
                        $.messager.alert('Information', result.message, 'info');
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');

                        // Set Back Feeder ETD Value
                        if (result.message.indexOf("You can't revise date ETA") !== -1) {
                            $("#txtSIeta").datebox('setValue', initETA);
                        }
                    }
                }
            });
        }
    });
    // End SAVE

    // Open / Close Job
    $('#seaimport_form_btn_open_close_job').click(function () {
        // Enable only in Main Job
        if (subJobNumber > 0) {
            $('#seaimport_form_btn_open_close_job').attr("disabled", "disabled");
            $.messager.alert('Warning', "Sub Order can't be Opened or Closed !", 'warning');
            return;
        }

        var text = $(this).text().toLowerCase();
        var submitURL = '';
        // Open
        if (text.indexOf('open') !== -1) {
            submitURL = base_url + "shipmentorder/OpenJob";
        }
            // Close
        else if (text.indexOf('close') !== -1) {
            submitURL = base_url + "shipmentorder/CloseJob";
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: shipmentId
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "shipmentorder/detail?Id=" + shipmentId + "&JobId=" + jobId;
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });
    // End Open Close Job


    // Ctrl + Alt + D
    Mousetrap.bind('ctrl+alt+d', function (e) {
        // ctrl + alt + d key pressed
        $.messager.confirm('Confirm', 'Are you sure to Add Container Info to Job Description?', function (r) {
            if (r) {
                $.ajax({
                    url: base_url + "Container/GetContainerByShipment?ShipmentId=" + shipmentId,
                    success: function (result) {
                        if (result != null) {
                            if (result.list != null) {
                                var container = '';
                                for (var i = 0; i < result.list.length; i++) {
                                    container += "\n" + result.list[i].ContainerNo + "/" + result.list[i].strSize + "'" + result.list[i].strType + "/" + result.list[i].SealNo;
                                }
                                container = $('#txtSIdesc').val() + container;
                                $('#txtSIdesc').val(container);
                            }
                        }
                    }
                });
            }
        });
    });

    // Ctrl + Shift + 1
    Mousetrap.bind('ctrl+shift+1', function (e) {
        var text = $('#txtSIdesc').val() + "1 x 20'GP CONTAINER, SAID TO CONTAIN:";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 2
    Mousetrap.bind('ctrl+shift+2', function (e) {
        var text = $('#txtSIdesc').val() + "1 x 40'GP CONTAINER, SAID TO CONTAIN:";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 3
    Mousetrap.bind('ctrl+shift+3', function (e) {
        var text = $('#txtSIdesc').val() + "## FREIGHT PREPAID ##";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 4
    Mousetrap.bind('ctrl+shift+4', function (e) {
        var text = $('#txtSIdesc').val() + "## FREIGHT COLLECT ##";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 5
    Mousetrap.bind('ctrl+shift+5', function (e) {
        var text = $('#txtSIdesc').val() + "INTENDED CONX VESSEL";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 6
    Mousetrap.bind('ctrl+shift+6', function (e) {
        var text = $('#txtSIdesc').val() + "SAID TO CONTAIN:";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 7
    Mousetrap.bind('ctrl+shift+7', function (e) {
        var text = $('#txtSIdesc').val() + "TCT/WN/DPN/SN/CF";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 8
    Mousetrap.bind('ctrl+shift+8', function (e) {
        var text = $('#txtSIdesc').val() + "### EXPRESS BILL OF LADING ###";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 9
    Mousetrap.bind('ctrl+shift+9', function (e) {
        var text = $('#txtSIdesc').val() + "SHIPPER'S LOAD, STOW AND COUNT";
        $('#txtSIdesc').val(text);
    });

    // Ctrl + Shift + 0
    Mousetrap.bind('ctrl+shift+0', function (e) {
        var text = $('#txtSIdesc').val() + "CY/CY - FCL/FCL";
        $('#txtSIdesc').val(text);
    });

    // Next Shipment
    $('#nav_next').click(function () {
        $.ajax({
            url: base_url + "ShipmentOrder/NextShipment?CurrentId=" + shipmentId,
            success: function (result) {
                if (result.isValid) {
                    window.location = base_url + "shipmentorder/detail?Id=" + result.shipmentId + "&JobId=" + jobId;
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // Prev Shipment
    $('#nav_prev').click(function () {
        $.ajax({
            url: base_url + "ShipmentOrder/PrevShipment?CurrentId=" + shipmentId,
            success: function (result) {
                if (result.isValid) {
                    window.location = base_url + "shipmentorder/detail?Id=" + result.shipmentId + "&JobId=" + jobId;
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // First Shipment
    $('#nav_first').click(function () {
        $.ajax({
            url: base_url + "ShipmentOrder/FirstShipment?CurrentId=" + shipmentId,
            success: function (result) {
                if (result.isValid) {
                    window.location = base_url + "shipmentorder/detail?Id=" + result.shipmentId + "&JobId=" + jobId;
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // Last Shipment
    $('#nav_last').click(function () {
        $.ajax({
            url: base_url + "ShipmentOrder/LastShipment?CurrentId=" + shipmentId,
            success: function (result) {
                if (result.isValid) {
                    window.location = base_url + "shipmentorder/detail?Id=" + result.shipmentId + "&JobId=" + jobId;
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });

    // Search By JobNumber or SubJobNo
    $('#nav_searchjobnumber, #nav_searchsubjobno').live("keypress", function (e) {
        if (e.keyCode == 13) {
            $.ajax({
                url: base_url + "ShipmentOrder/SearchShipment?JobId=" + jobId + "&JobNumber=" + $('#nav_searchjobnumber').val() + "&SubJobNo=" + $('#nav_searchsubjobno').val(),
                success: function (result) {
                    if (result.isValid) {
                        window.location = base_url + "shipmentorder/detail?Id=" + result.shipmentId + "&JobId=" + jobId;
                    }
                    else {
                        $.messager.alert('Warning', result.message, 'warning');
                    }
                }
            });
            return false; // prevent the button click from happening
        }
    });


}); //END DOCUMENT READY