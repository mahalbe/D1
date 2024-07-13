function getClasses(ClassId, SectionId) {
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
        }
    });
}
function getAllSections_ByClassId(ClassId, SectionId) {
    var sectionHtml = '<option value="">--Select--</option>';
    $.ajax({
        type: "GET",
        url: "../DropDown/GetSectionsForClass",
        data: { classId: $('#' + ClassId).val() },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                sectionHtml += '<option value=' + result[i].Value + '>' + result[i].Text + '</option>';
            }
            $('#' + SectionId).html(sectionHtml);
        }
    });
}