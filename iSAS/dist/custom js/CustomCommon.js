function getStateListByCountry(countryId, stateId, cityId) {
    var stateHtml = '<option value="">--Select--</option>';
    var cityHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../Common/GetStateListByCountryID",
        data: { CountryID: $('#' + countryId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                stateHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
            }
            $('#' + stateId).html(stateHtml);
            $('#' + cityId).html(cityHtml);
        },
    });
}

function getCityListByState(stateId, cityId) {
    var cityHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../Common/GetCityListByStateID",
        data: { StateID: $('#' + stateId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                cityHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
            }
            $('#' + cityId).html(cityHtml);
        },
    });
}
function getSectionListByClass(ClassId, SectionId) {
    var sectionHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../StudentAttendance/GetSectionsForClass",
        data: { classId: $('#' + ClassId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                sectionHtml += '<option value=' + result[i].SecId + '>' + result[i].SecName + '</option>';
            }
            $('#' + SectionId).html(sectionHtml);
        },
    });
}





function getFeeStrectureListByClassSectionId(ClassSectionId, StrectureId) {
    var strectureHtml = '<option value="">--Select--</option>';
    if ($('#' + ClassSectionId).val() == null || $('#' + ClassSectionId).val() == '') {
        $('#' + StrectureId).html(strectureHtml);
    } else {
        $.ajax({
            type: "GET",
            url: "../Fee_FeeStructureMaster/Fee_StrectureDropDown_ByClassSectionId",
            data: { ClassSectionId: $('#' + ClassSectionId).val() },
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    strectureHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
                }
                $('#' + StrectureId).html(strectureHtml);
            },
        });
    }
}



function getInstallmentListByStrectureId(StrectureId, InstallmentId) {
    var installmentHtml = '<option value="">--Select--</option>';
    if ($('#' + StrectureId).val() == null || $('#' + StrectureId).val() == '') {
        $('#' + InstallmentId).html(installmentHtml);
    }
    else {
        $.ajax({
            type: "GET",
            url: "../Fee_InstallmentSetup/InstallmetDropdwon_ByStrectureId",
            data: { StrectureId: $('#' + StrectureId).val() },
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    installmentHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
                }
                $('#' + InstallmentId).html(installmentHtml);
            },
        });
    }
}














function GetStudentListForTCByClassSection(sessionId, classId, sectionId, studentId) {
    var studentHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../Student_TC/GetddlStudentList",
        data: { sessionId: $('#' + sessionId).val(), classId: $('#' + classId).val(), sectionId: $('#' + sectionId).val() },
        success: function (data) {
            //for (var i = 0; i < result.length; i++) {
            //    studentHtml += '<option value=' + result[i].ERPNo + '>' + result[i].Student + '</option>';
            //}
            //$('#' + studentId).html(studentHtml);
            var $ddlStudent = $('#' + studentId);
            $ddlStudent.empty();
            $ddlStudent.append($("<option />").text("--- Select Student ---"));
            $.each(data, function () {
                if (this.Student.indexOf("(NSO)") <= 0) {
                    $ddlStudent.append($("<option />").val(this.ERPNo).text(this.Student));
                }
                else {
                    $ddlStudent.append($("<option />").val(this.ERPNo).text(this.Student).prop('disabled', true));
                }
            });


            $('#ERPNO').val('');
            $('#AdmissionNo').val('');
            $('#FatherName').val('');
        },
    });
}



function GetStudentListByClassSection(sessionId, classId, sectionId, studentId) {
    var studentHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../Common/GetStudentList",
        data: { sessionId: $('#' + sessionId).val(), classId: $('#' + classId).val(), sectionId: $('#' + sectionId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                studentHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
            }
            $('#' + studentId).html(studentHtml);
        },
    });
}

function AddValidAttributeCSS(ValidAttributeList) {
    if (ValidAttributeList !== undefined) {
        for (var i = 0; i < ValidAttributeList.length; i++) {
            $('#' + ValidAttributeList[i].Title).css('border-color', '');
            $('span[data-valmsg-for="' + ValidAttributeList[i].Title + '"]').text('');
        }
    }
}
function AddErrorAttributeCSS(ErrorList) {
    if (ErrorList !== undefined) {
        for (var i = 0; i < ErrorList.length; i++) {
            $('#' + ErrorList[i].Title).css('border-color', 'red');
            $('span[data-valmsg-for="' + ErrorList[i].Title + '"]').text(ErrorList[i].Error);
        }
    }
    else {
        OpenMessegeAutoHideDiv('Internal server error..!', 'Danger');
    }
}

function NoSpaceAtAnyPlace(e) {
    if (e.which === 32) {
        OpenMessegeAutoHideDiv('Can not pass space in this text', 'Info');
        return false;
    } else {
        return true;
    }
}

function NumericOnly(evt) {
    var charCode = (evt.which) ? evt.which : enventkeyCode;
    if (charCode >= 48 && charCode <= 57) {
        return true;
    } else {
        return false;
    }
}

function NumericOnlyWithHyphin(evt) {
    var charCode = (evt.which) ? evt.which : enventkeyCode;
    if (charCode >= 48 && charCode <= 57 || charCode == 45) {
        return true;
    } else {
        return false;
    }
}

function NumericDecimalOnly(evt) {
    var charCode = (evt.which) ? evt.which : enventkeyCode;
    if (
        (charCode != 45 || $(this).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(this).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function getRouteStopeByRouteId(RouteId, StopId) {
    var stopHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../Common/GetRouteStopByRouteId",
        data: { RouteId: $('#' + RouteId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                stopHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
            }
            $('#' + StopId).html(stopHtml);
        },
    });
}

function OpenMessegeAutoHideDiv(msg, msgType, modalTitle) {
    var backGroundIcon = "icon fa fa-info";
    if (msg == '' || msg == null) {
        msg = 'There is no response from your request..!';
        backGroundIcon = "icon fa fa-ban";
    } else {
        if (msgType == 'Success') {
            backGroundIcon = "icon fa fa-check";
        }
        else if (msgType == 'Warning') {
            backGroundIcon = "icon fa fa-warning";
        }
        else if (msgType == 'Danger') {
            backGroundIcon = "icon fa fa-ban";
        }
        if (modalTitle == null || modalTitle == "") {
            modalTitle = msgType;
        }
    }
    if (msgType != null && msgType != '') {
        $.notify({
            icon: backGroundIcon,
            message: msg
        }, {
                type: msgType.toLowerCase(),
                z_index: 3000,
            });
    }
    else {
        $.notify({
            icon: backGroundIcon,
            message: msg
        }, {
                type: 'info',
                z_index: 3000,
            });
    }
}




function OpenMessegeModal(msg, modalType, modalsize, modalTitle) {
    var backGroundColor = "#337ab7";
    if (msg == '' || msg == null) {
        msg = 'There is no response from your request..!';
    }
    if (modalType == '' || modalType == null) {
        modalType = 'Primary';
    }
    if (modalsize == '' || modalsize == null) {
        modalsize = 'sm';
    }

    if (modalsize == 'sm') {
        $('#modalDialog').addClass('modal-sm');
    } else if (modalsize == 'lg') {
        $('#modalDialog').addClass('modal-lg');
    }
    else {
        $('#modalDialog').removeClass('modal-lg');
        $('#modalDialog').removeClass('modal-sm');
    }

    if (modalType == 'Info') {
        backGroundColor = "#5bc0de";
    }
    else if (modalType == 'Success') {
        backGroundColor = "#5cb85c";
    }
    else if (modalType == 'Warning') {
        backGroundColor = "#f0ad4e";
    }
    else if (modalType == 'Danger') {
        backGroundColor = "#d9534f";
    }
    $('#modalTitle').html(modalTitle);
    $('#mainModalHeader').css('background-color', backGroundColor);
    $('#modalbtnOK').css('background-color', backGroundColor);
    $('#modalMsgBody').html(msg);
    $("#mainModal").modal();
}


function OpenModalWithSave(body, modalTitle) {
    if (body == '' || body == null) {
        body = 'There is no content to display..!';
    }
    $('#modalTitleWithSave').html(modalTitle);
    $('#modalMsgBodyWithSave').html(body);
    $("#mainModalWithSave").modal();
}

function OpenChatModal(body) {
    if (body == '' || body == null) {
        body = 'There is no messege..!';
    }
    $('#mainModalChatBody').html(body);
    $("#mainModalChat").modal();
}

function modalBtnOkClick() {
    if ($('#hddnRedirectRoute').val() != null && $('#hddnRedirectRoute').val() != 'undefined' && $('#hddnRedirectRoute').val() != '') {
        window.location.href = $('#hddnRedirectRoute').val();
    }
}

function AsyncConfirmYesNo(title, msg, yesFn, noFn, evt, modalsize) {
    swal({
        title: title,
        text: msg,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, do it!',
        //preConfirm: (value) => {
        //    return yesFn(evt);
        //},
    }).then((result) => {
        if (result.value) {
            return yesFn(evt);
        } else {
            return noFn(evt);
        }
    });
}

function copyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;
    document.body.appendChild(textArea);
    textArea.select();
    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }
    document.body.removeChild(textArea);
}

function fun_GetAttenCal(evt) {
    var object = $(evt).attr("id");
    var str = object.split('/');
    // str[0] contains "month"
    // str[1] contains "year"
    fun_loadAttendence(str[0], str[1], str[2]);
}
function fun_loadAttendence(month, year, erpNo) {
    $.ajax
        ({
            url: '../Dashboard/AsyncUpdateCalender',
            type: 'GET',
            traditional: true,
            contentType: 'application/json',
            data: { month: month, year: year, erpNo: erpNo },
            success: function (result) {
                $('#showAttendanceCalMainDiv').html(result);
            },
        });
}

function fun_GetAttenCal_Staff(evt) {
    var object = $(evt).attr("id");
    var str = object.split('/');
    // str[0] contains "month"
    // str[1] contains "year"
    fun_loadAttendence_Staff(str[0], str[1]);
}

function fun_loadAttendence_Staff(month, year, userId) {
    $.ajax
        ({
            url: '../DashBoard_StaffStatatics/AsyncUpdateCalender',
            beforeSend: function () {
                $('#spinner').show();
            },
            type: 'GET',
            traditional: true,
            contentType: 'application/json',
            data: { month: month, year: year },
            success: function (result) {
                $('#staffAttnCalendarMainDiv').html(result);
            },
            beforeSend: function () {
                $('#spinner').hide();
            },
        });
}


function spinnerShow() {
    $('#spinner').show();
}

function spinnerHide() {
    $('#spinner').hide();
}

function PrintReceipt_ByReceiptNo(receiptno, erpno) {
    $.ajax({
        type: "GET",
        beforeSend: function () {
            spinnerShow();
        },
        url: "../StudentFeeDetails/GetFeeDocument_PrintOnly",
        data: { TransRefNo: receiptno, Mode: 'RECEIPT', erpno: erpno },
        error: function (xhr, status, error) {
            OpenMessegeModal($('#ErrorMsgOnJasonFailed').val());
        },
        success: function (response) {
            if (response != '' && response != null && response.length() > 2) {
                OpenMessegeModal(response, 'Info', 'sm', 'Messege');
            }
        },
        complete: function () {
            spinnerHide();
        }
    });
}

function GetDaysBetweenDays(FromDate_Id, ToDate_Id, DaysCount_Id) {
    var start = new Date($('#' + FromDate_Id).val().split("/").reverse().join("-")),
        end = new Date($('#' + ToDate_Id).val().split("/").reverse().join("-")),
        diff = new Date(end - start),
        days = diff / 1000 / 60 / 60 / 24;

    days; //=> 8.525845775462964
    if ((days + 1) > 0) {
        $('#' + DaysCount_Id).val(days + 1);
    } else {
        OpenMessegeAutoHideDiv('In-Valid Date', 'Warning');
        $('#' + DaysCount_Id).val(0);
    }
}

function checkValidFromAndToDate(FromDate_Id, ToDate_Id, Error_AttributeId) {
    if ($('#' + FromDate_Id).val() !== null && $('#' + FromDate_Id).val() !== "" && $('#' + ToDate_Id).val() !== null && $('#' + ToDate_Id).val() !== "") {
        var start = new Date($('#' + FromDate_Id).val().split("/").reverse().join("-")),
            end = new Date($('#' + ToDate_Id).val().split("/").reverse().join("-")),
            diff = new Date(end - start),
            days = diff / 1000 / 60 / 60 / 24;
        if (days < 0) {
            $('#' + Error_AttributeId).css('border-color', 'red');
            return false;
            //$('span[data-valmsg-for="' + Error_AttributeId + '"]').text("Please select valid date.!");
        } else {
            $('#' + FromDate_Id).css('border-color', '');
            //$('span[data-valmsg-for="' + FromDate_Id + '"]').text('');

            $('#' + ToDate_Id).css('border-color', '');
            //$('span[data-valmsg-for="' + ToDate_Id + '"]').text('');

            $('#' + Error_AttributeId).css('border-color', '');
            //$('span[data-valmsg-for="' + Error_AttributeId + '"]').text('');
            return true;
        }

    }
}