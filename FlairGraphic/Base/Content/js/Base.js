//'**********************************************************************
//'*** REGION NAME:DIGIMINDBase.Js
/// MERGE CODE

/*base Js: Start--------------------------------------------------------------------------------------------------------------------------------------------*/
base = {};
base.appPath = "";
base.loginID = "";
base.getUrl = function (controllerName, actionName, id) {
    return base.appPath + '/' + controllerName + '/' + actionName + (id == undefined || id == null ? '' : ('/' + id));
}
base.GetURLParameter = function GetURLParameter(sParam) {

    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0].toUpperCase() == sParam.toUpperCase()) {
            return sParameterName[1];
        }
    }
}
base.hasValue = function (val) {
    if (typeof (val) == 'undefined' || val == undefined || val == null || val == '')
        return false;
    else
        return true;
}
base.isNullOrEmpty = function (val) {
    var ret = false;
    if (base.hasValue(val)) {
        if (val == '' || val == null)
            ret = true;
        else
            ret = false;
    } else {
        ret = true;
    }
    return ret;
}
base.getTagName = function (id) { //frm1 city_id
    id = base.getCtrlId(id);
    if (base.isHtml(id)) {
        var prefix = id.indexOf('#') == -1 ? '#' : '';
        id = prefix + id;
        if (!base.isNullOrEmpty(id)) {
            return $(id).prop("tagName").toLowerCase();
        }
    }
    return null;
}
base.getCtrlId = function (idOrCtrl) { //frm city_id / #city_id
    var id = null;
    if (typeof (idOrCtrl) == 'string') { //frm city_id
        id = idOrCtrl;
    } else if (typeof (idOrCtrl) == 'object') { //this
        id = base.getFullCtrlId(idOrCtrl);
    }
    if (id.indexOf('#') == -1) { // no #, only frm city_id
        id = '#' + id.replace(' ', ' #')
    } else {
        //do nothing..., as id prefixed by # already
    }
    //if (base.isHtml(id)) {
    //}
    return id;
}
base.getCtrlNameOnly = function (ctrlId) {
    var myCtrl = ctrlId.split(' ').pop(-1).replace('#', ''); ////'#frm1 #ctrl_id' ==> ctrl_id
    return myCtrl;
}
base.getFullCtrlId = function (th) { //this
    var id = $(th).attr('id');
    var parentId = base.getParentId(th);
    var frmParentId = '';
    if (!base.isNullOrEmpty(parentId)) {
        frmParentId += ('#' + parentId + ' ');
    }
    //if (!base.isNullOrEmpty(formId)) {
    //    frmParentId += ('#' + formId + ' ');
    //}
    var fullId = (frmParentId + ((base.isNullOrEmpty(frmParentId) ? '#' : ' #') + id));
    return fullId;
}
base.getLength = function (str) {
    var len = -1;
    try {
        len = base.isNullOrEmpty(str) ? len : str.toString().length;
    }
    catch (ex) {
        console.log(ex.message);
    }
    return len;
}
base.replaceAll = function (str, replace, by) {
    return str.split(replace).join(by)
}
base.setCookie = function (cname, cvalue, exdays) {

    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires + ';path=/';
}
base.getCookie = function (cname) {

    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
base.deleteCookie = function (cname) {
    document.cookie = cname + "=;expires=Wed; 01 Jan 1970";
}

base.notificaton = function (s) {
    $('.jq-toast-wrap').html('')
    if (haseValue(s)) {
        var result = s.result;

        var title = base.isNullOrEmpty(result.title) ? '' : result.title;
        var msg = '';
        var type = '';
        if (result.Message != undefined) {
            msg = result.Message;
            switch (result.MessageType) {
                case MessageType.Success:
                    title = base.isNullOrEmpty(result.title) ? 'Success' : title;
                    type = 'Success'.toLowerCase();
                    break;
                case MessageType.Error:
                    title = base.isNullOrEmpty(result.title) ? 'Error' : title;
                    type = 'Error'.toLowerCase();
                    break;
                case MessageType.Info:
                    title = base.isNullOrEmpty(result.title) ? 'Info' : title;
                    type = 'Info'.toLowerCase();
                    break;
                case MessageType.Warning:
                    title = base.isNullOrEmpty(result.title) ? 'Warning' : title;
                    type = 'Warning'.toLowerCase();
                    break;
                default:
            }
            //type = title.toLowerCase();
            $.toast({
                heading: title,
                text: msg,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: type,
                hideAfter: 3000,
                stack: 6
            });
        }
        if (result.Redirect != undefined) {
            window.location = result.Redirect;
        }
    }
    else {

        var msg = decodeURI(base.GetURLParameter('result'));
        var type = base.GetURLParameter('MessageType')
        if (haseValue(type)) {
            type = type.toLowerCase();
            $.toast({
                heading: msg,
                text: msg,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: type,
                hideAfter: 3000,
                stack: 6
            });
        }
    }


}

base.showMessage = function showMessage(MSG, TY, style, position, to) {
    $('.alertDivPosition').html('');
    // type =  info,warning, success, danger etc
    //position =left, top, bottom,top-left,top-right,bottom-left,bottom-right
    var message = MSG != '' ? MSG : decodeURI(base.GetURLParameter('result'));
    var type = TY != '' ? TY : base.GetURLParameter('MessageType');
    var Timeout = to != '' ? to : 10000;
    var style = style != '' ? style : 'Bar';
    var position = position != '' ? position : 'top';
    if (type == 'Error') { type = 'danger'; }
    if (type != undefined) { type = type.toLowerCase(); }
    if (message != 'undefined' && type != 'undefined' && message != undefined && type != undefined) {
        if (style == 'Bar') {
            $('.alertDivPosition').pgNotification({
                style: 'bar',
                message: message,
                position: position,
                timeout: Timeout,
                type: type
            }).show();
        } else if (style == 'Bouncy Flip') {
            $('.alertDivPosition').pgNotification({
                style: 'flip',
                message: message,
                position: position,
                timeout: Timeout,
                type: type
            }).show();
        } else if (style == 'Circle Notification') {
            // Slide-in a circle notification from sides
            // You have to provide the HTML for thumbnail
            $('.alertDivPosition').pgNotification({
                style: 'circle',
                title: 'John Doe',
                message: message,
                position: position,
                timeout: Timeout,
                type: type,
                thumbnail: '<img width="40" height="40" style="display: inline-block;" src="assets/img/profiles/avatar2x.jpg" data-src="assets/img/profiles/avatar.jpg" data-src-retina="assets/img/profiles/avatar2x.jpg" alt="">'
            }).show();
        } else if (style == 'Simple Alert') {
            // Simple notification having bootstrap's .alert class
            $('.alertDivPosition').pgNotification({
                style: 'simple',
                message: message,
                position: position,
                timeout: Timeout,
                type: type
            }).show();
        } else {
            return;
        }
    }

    //e.preventDefault();


}
base.isValidEmailAddress = function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
    //alert( pattern.test(emailAddress) );
    return pattern.test(emailAddress);
};
base.isHtml = function (id) {
    ret = true;
    if (isNaN(id)) { //if not number then go...
        id = base.getCtrlId(id);
        //ret = arrNonHtml.indexOf(id) == -1;
        var r = new RegExp("^[a-z A-Z 0-9 #_]*$");
        ret = r.test(id);
        if (ret) {
            var prefix = id.indexOf('#') == -1 ? '#' : '';
            ret = $(prefix + id).length > 0;
        }
    } else {
        ret = false;
    }
    return ret;
}
base.getInputType = function (ctrlId) {
    var type = null;
    ctrlId = base.getCtrlId(ctrlId);
    //var prefix = ctrlId.indexOf('#') == -1 ? '#' : '';
    //var ctrlId = prefix + ctrlId;
    var info = $(ctrlId).data();
    if (!base.isNullOrEmpty(info)) {
        if (base.isNullOrEmpty(info.inputType)) {
            if (base.getTagName(ctrlId) == 'input') {
                type = $(ctrlId).attr('type');
            }
        }
        else {
            type = info.inputType;
        }
    }
    return type;
}
base.isTrue = function (val) {
    var ret = false;
    if (base.hasValue(val)) {
        if (String(val).toUpperCase() == 'TRUE')
            return true;
    }
    return ret;
}
base.dateFromJson = function (jsonDate) {
    if (jsonDate == null) return null;
    ///var dateString = jsonDate.substr(6).replace(')/', '');
    var currentTime = new Date(jsonDate);
    return moment(currentTime).format('MM/DD/YYYY');
}
treeUtil = {};
treeUtil.processTree = function (s) {
    var ctrlId = base.getCtrlId(s.ctrlId);
    var data = s.data;
    var isPlugins = s.isPlugins;
    var info = $(ctrlId).data();
    var selected = eval(info.values);
    if (!base.isNullOrEmpty(selected)) {
    }
    var values = [];
    var nodes = [];
    var loop = 0;
    $.each(data, function () {
        var node = { "id": '' + this.id + '', "parent": '' + this.parent + '', "text": '' + this.text + '', "state": { "selected": this.selected } }
        if (this.selected) {
            values.push(this.id);
        }
        if (!base.isNullOrEmpty(selected)) {
            for (var i = 0; i < selected.length; i++) {
                if (this.id == selected[i]) {
                    node = { "id": '' + this.id + '', "parent": '' + this.parent + '', "text": '' + this.text + '', "state": { "selected": true } }
                }
            }
        }
        nodes.push(node);
    });
    $(ctrlId).attr('data-orig-values', values.toString());
    $(ctrlId).attr('data-values', values.toString());
    $(ctrlId).jstree({
        'core': {
            'data': nodes,
        },
        //checkbox: {
            //real_checkboxes: true,
            //real_checkboxes_names: function (n) {
                //return [("check_" + (n[0].id || Math.ceil(Math.random() * 10000))), n[0].id]
            //}
        //},
        //plugins: isPlugins ? ["html_data", "types", "themes", "ui", "checkbox"] : ["html_data", "types", "themes", "ui"]
        'checkbox': {
            three_state: false,
            cascade: 'up'
        },
        'plugins': ["checkbox"]

    });
    $(ctrlId).on('changed.jstree', function (e, data) {
        $(ctrlId).attr('data-values', treeUtil.getSelectedValues(ctrlId));
    });

    $(ctrlId).on('deselect_node.jstree Event', function (e, data) {
        //do nothing...
    });
    $(ctrlId).on('select_node.jstree Event', function (e, data) {
        //do nothing...
    });
}
treeUtil.getSelectedValues = function (ctrlId) {
    if (base.isNullOrEmpty(ctrlId)) {
        return null;
    } else {
        return $(ctrlId).jstree('get_selected');
    }
}
treeUtil.getValue = function (ctrlId) {
    return $('#' + ctrlId).attr('data-values');
    //return $('#' + ctrlId).data('values');
}
function setActiveMenu(settings) {
    if (base.hasValue(settings)) {
        var isEnabled = settings.isEnabled;
        if (isEnabled) {

            var scope = $('#sidebarnav');
            var path = window.location.pathname + window.location.search;
            if (path == '/') return;
            var find = $(scope).find('a[href*= "' + path + '"]').length > 0 ? true : false;
            if (!find) {
                //Lets try on controller basis
                //var info = path.split('/');
                //var controller = info[1];
                //path = '/' + controller + '/' //new path
                path = base.getCookie('selMenu');
            }

            $(scope).find('a[href*= "' + path + '"]').each(function () {
                 //alert(path);
                var li = $(this).parent('li');
                 var span = $(li).addClass("active");
                 //alert('1')
                //var ul = $(li).parent('ul');
                //ul.attr('aria-expanded', 'true')
                //ul.addClass('collapse in')
                //if ($(ul).attr("class") == 'sub-menu') {
                //    ul.css('display', 'block');
                //    var liParent = $(ul).parent('li');
                //    var spanParent = $(liParent).find('span.THP');
                //    spanParent.addClass('bg-success');
                //}
                base.setCookie('selMenu', path, 1);
            });
        }
    }
}
//'**********************************************************************
//'*** REGION NAME:Base.Js
//'***  DESCRIPTION:FOR ALL COMMON FUNCTION FOR BASICS AND LOGICAL CLASSES

///CONST FOR STUtil FUNCTION--

function haseValue(p) {
    if (p == '' || p == undefined || p == null || p == '0') {
        return false;
    }
    else {
        return true;
    }

}
function haseTrue(p) {
    if (p == '' || p == undefined || p == null || p == false) {
        return false;
    }
    else {
        return true;
    }

}
FormUtil = {};
ConstMessage = {};
ConstMessage.CreateMessage = CreateMessage = "record Inserted Successfully.";
ConstMessage.UpdateMessage = "record updated Successfully.";
ConstMessage.DeleteMessage = "record deleted Successfully.";
ConstMessage.ConfirmDeleteMessage = "Are you sure want to delete?";
ConstMessage.success = "success";
ConstMessage.DeleteText = "You will not be able to recover this imaginary file!";
//For Clear Options------
//DESCRIPTION:FOR CLEAR OPTIONS
var STUtil = {};
STUtil.ClearOption = function (p) {
    if (p.ctrlId != '') {
        var ctrlId = p.ctrlId.split(',');
        var captions = p.caption.split(',');
        for (var i = 0; i < ctrlId.length; i++) {
            var caption = haseValue(captions[i]) ? captions[i] : 'Please Select'
            $(ctrlId[i]).children().remove();
            $(ctrlId[i]).append($("<option>").val("").text(caption));
        }
    }
}
//DESCRIPTION:FOR CASCADING OPTIONS
STUtil.Cascading = function (p) {
    var ctrlId = p.ctrlId;
    var formId = p.formId;
    var fullctrlId = haseValue(formId) ? ('#' + formId + ' #' + ctrlId) : '#' + ctrlId;
    var clearId = p.clearId;
    var controllerName = p.controllerName;
    var actionName = p.actionName;
    var parmVal = p.parmVal;
    var SelectedValue = p.SelectedValue;
    var ismultiSelect = haseValue(p.ismultiSelect) ? p.ismultiSelect : false;
    var ctrlcaption = haseValue(p.ctrlCaption) ? p.ctrlCaption : 'Please Select'
    if (parmVal != "") {
        $.post(base.getUrl(controllerName, actionName), { id: parmVal }, function (data) {
            !(ismultiSelect) ? STUtil.ClearOption({ ctrlId: fullctrlId, caption: ctrlcaption }) : $(fullctrlId).empty();
            $.each(data, function () {
                $(fullctrlId).append($("<option>").val(this.Value).text(this.Text));
            });
            if (haseValue(SelectedValue)) {
                $(fullctrlId).val(SelectedValue).trigger('change');
            }
        });


    }
    else {
        STUtil.ClearOption({ ctrlId: ctrlId, caption: ctrlcaption })
    }
    STUtil.ClearOption({ ctrlId: clearId, caption: p.clearCaption })
}
//DESCRIPTION:FOR ASSIGN VALUE
STUtil.AssignValue = function (p) {
    var ctrlId = p.ctrlId.split(',');
    var value = p.value.split(',');
    for (var i = 0; i < ctrlId.length; i++) {
        $('#' + ctrlId[i]).val(value[i])
    }
}
//DESCRIPTION:FOR DELETE MASTER COMMAN FUNCTION
//DESCRIPTION:FOR DELETE CONFIRM POPUP COMMAN FUNCTION
STUtil.confirm = function (p) {
    var title = p.title != '' ? p.title : ConstMessage.ConfirmDeleteMessage;
    var text = p.text != '' ? p.text : "";
    var showCancelButton = true;
    var confirmButtonColor = "#DD6B55";
    var confirmButtonText = "Yes, delete it!";
    var closeOnConfirm = false;
    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel please!",
        closeOnConfirm: false,
        closeOnCancel: false,
        timer: 5000///1000 = 1 sec
    },
        function (isConfirm) {
            if (isConfirm) {
                var msg = STUtil.DeleteMaster({ CtrlId: p.DeleteFunction.CtrlId, Url: p.DeleteFunction.Url, IsLoad: p.DeleteFunction.IsLoad, PrimaryVal: p.DeleteFunction.PrimaryVal })
                if (msg == ConstMessage.success) {
                    swal("Deleted!", ConstMessage.DeleteMessage, "success");
                }
                else {
                    swal("Rrecord not deleted!", msg, "error");
                }
                return true;
            }
            else { swal("Cancelled", "Your imaginary file is safe :)", "error"); }
            if (p.DeleteFunction.IsLoad) {
                window.location.reload(true);
            }

        });

}
//DESCRIPTION:FOR GetFileSize FUNCTION
function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        //for IE
        if ($.browser.msie) {
            //before making an object of ActiveXObject, 
            //please make sure ActiveX is enabled in your IE browser
            var objFSO = new ActiveXObject("Scripting.FileSystemObject"); var filePath = $("#" + fileid)[0].value;
            var objFile = objFSO.getFile(filePath);
            var fileSize = objFile.size; //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }
        //for FF, Safari, Opeara and Others
        else {
            fileSize = $("#" + fileid)[0].files[0].size //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }

        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
    }
}
//DESCRIPTION:FOR GetFileSize FUNCTION
function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}
//DESCRIPTION:FOR GetFileSize COMMAN FUNCTION
FormUtil.Valid = function FormValiidation(btn) {


    var btnId = btn.id == undefined ? btn : btn.id;
    btnId = ('#' + btnId);
    if (typeof (tinyMCE) != 'undefined' && tinyMCE != undefined && tinyMCE != null) {
        tinyMCE.triggerSave();
    }
    var form = $(btnId).closest("form");
    var isValid = true;
    if (form != null) {
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
        try {
            isValid = form.valid();
        }
        catch (ex) {
            console.log(ex.message);
        }
    }
    if (isValid) {
        return true;
    }
    else {
        return false;
    }
}
$(document).ready(function () {
    STUtil.InitFunction()
});
STUtil.DynamicValMsg = function (element, ret) {
    var id = element.id;
    var formId = $('#' + element.id).closest("form").attr('id')
    var ctrlId = haseValue(formId) ? ('#' + formId + ' #' + id) : '#' + id;
    var ctrlTextInfo = id.split('_');
    var ctrlText = "";
    var msg = "";
    for (var i = 0; i < ctrlTextInfo.length; i++) {
        ctrlText = haseValue(ctrlText) ? ctrlText + " " + ctrlTextInfo[i] : ctrlTextInfo[i];
    }
    if (ret) {
        $(ctrlId).attr('title', 'This Field Is Required')
        //  msg =  'This Field Is Required';
    }
    else {
        $(ctrlId).attr('title', 'Please enter a valid ' + ctrlText)
    }
    return msg;

}
STUtil.FileValidation = function () {
    $('input[type="file"]').change(function () {
        // debugger//
        //this.files[0].size gets the size of your file.
        var id = $(this).attr("id");
        var imgVal = $('#' + $(this).attr('id')).val();
        var imageExt = imgVal.substr((imgVal.lastIndexOf('.') + 1));
        var imgPreview = id + '_Preview';
        var imgSrc = '';
        ////$('#id').append("<img style='display:none' id='" + imgPreview + "' width='80px' height='80px' />")
        $('#' + id).parent().append("<img style='display:none'  id='" + imgPreview + "' width='80px' height='80px' />")
        if (base.hasValue(imgVal)) {
            switch (imageExt.toUpperCase()) {
                case 'CSV':
                    imgSrc = '/Base/Content/images/CSV.png';
                    break;
                case 'PDF':
                    imgSrc = '/Base/Content/images/pdf.png';
                    break;
                case 'DOC':
                    imgSrc = '/Base/Content/images/DOC.png';
                    break;
                default:
                    imgSrc = '';
            }
            $('#' + imgPreview).show()
        }
        else {
            $('#' + imgPreview).hide()
        }
        var reader = new FileReader();
        reader.onload = function () {
            var oustput = document.getElementById(imgPreview);
            oustput.src = haseValue(imgSrc) ? imgSrc : reader.result;
        };
        reader.readAsDataURL(event.target.files[0]);
        var fileInput = $('#' + id);
        STUtil.FileExtValid(id);
        var maxSize = fileInput.attr("max-size");
        if (fileInput.get(0).files.length) {
            var fileSize = fileInput.get(0).files[0].size; // in bytes
            fileSize = parseFloat(fileSize) / 1000000.00;
            maxSize = parseFloat(maxSize);
            if (fileSize > maxSize) {
                base.notificaton({ result: { title: 'file size is more then ' + maxSize + ' MB, Please choose another file.', MessageType: 1, Message: "Error" } })

                $("#" + id).val('');
                $('#' + imgPreview).hide();
                return false;
            } else {
                $('#' + imgPreview).show();
            }
        } else {
            $('#' + imgPreview).hide();
            return false;
        }
    });
}
STUtil.FileExtValid = function (ctrlId) {
    var ret = false;
    var imgVal = $('#' + ctrlId).val();
    var fileInput = $('#' + ctrlId);
    var imgPreview = ctrlId + '_Preview';
    var imageExt = imgVal.substr((imgVal.lastIndexOf('.') + 1));
    var ctrlExt = $('#' + ctrlId).attr('data-file-ext');
    if (haseValue(ctrlExt)) {
        var exceExt = ctrlExt.split(',');
        for (var i = 0; i < exceExt.length; i++) {
            if (imageExt.toUpperCase().trim() == exceExt[i].toUpperCase().trim()) {
                ret = true;
            }
        }
        if (!ret) {
            $("#" + ctrlId).val('');
            $('#' + imgPreview).hide()
            base.notificaton({ result: { title: imageExt + ' File type not excepted plz try for  .' + ctrlExt, MessageType: 1, Message: "Error" } })
            $('#' + imgPreview).hide();
            return false;
        }
    }
    else {
        $('#' + imgPreview).show();

    }
}
//DESCRIPTION:FOR GetFileSize COMMAN FUNCTION
STUtil.validate = function validateform(btn) {
    var btnId = btn.id == undefined ? btn : btn.id;
    btnId = ('#' + btnId);
    if (typeof (tinyMCE) != 'undefined' && tinyMCE != undefined && tinyMCE != null) {
        tinyMCE.triggerSave();
    }
    var form = $(btnId).closest("form");
    var isValid = true;

    if (form != null) {

        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
        try {
            isValid = form.valid();
        }
        catch (ex) {
            console.log(ex.message);
        }
    }
    var isValidCustom = true;

    isValid = isValid && isValidCustom;
    //formUtil.autoValidation = true;
    // setInterval(function () { formUtil.resetValidation(); }, 500);
    if (isValid) {
        return true;
    }
    else {

        $('.field-validation-error').each(function () {
            if ($(this).prev().length > 0) {
                //MultiSelect
                var isMultiSelect = false;
                var prevId = '#' + $(this).prev().attr('id');
                if (hasValue($(prevId))) {
                    if (hasValue($(prevId).prop("tagName"))) {
                        if ($(prevId).prop("tagName").toUpperCase() == 'DL') {
                            $(prevId + ' a').addClass('val-error');
                            isMultiSelect = true;
                        }
                    }
                }
                //Other than MultiSelect
                if (!isMultiSelect) {
                    $(this).prev().addClass('val-error');
                }
            }
        });
        return false;
    }

}
STUtil.SetExist = function (p) {
    $('input,select').each(function () {
        var id = $(this).attr('id');
        var fromid = $(this).closest("form").attr("id");
        var isExist = $(this).attr('isExist');
        if (haseValue(isExist)) {
            if (isExist.toUpperCase().trim() == 'TRUE') {
                var cntrlFunction = id + '_isExist';
                if (typeof (window[cntrlFunction]) == "function") {
                    $('#' + id).blur(function () {
                        eval(cntrlFunction + "()");
                    });
                    $('#' + id).change(function () {
                        eval(cntrlFunction + "()");
                    });
                }
                else {
                    var cntrlFunction = fromid + "_" + id + '_isExist';
                    if (typeof (window[cntrlFunction]) == "function") {
                        $('#' + fromid + ' #' + id).blur(function () {
                            eval(cntrlFunction + "()");
                        });
                        $('#' + fromid + ' #' + id).change(function () {
                            eval(cntrlFunction + "()");
                        });

                    }
                }
            }
        }
    });
}
STUtil.IsExist = function (p) {
    //////////------------------------------
    debugger;
    var ctrlId = p.ctrlId;
    var ctrlVal = $('#' + p.ctrlId).val();
    var controllerName = p.controllerName;
    var actionName = p.actionName;
    var parmVal = p.parmVal;
    var formId = p.formId;
    var inputCtrlId = '#' + p.ctrlId;
    inputCtrlId = haseValue(formId) ? "#" + formId + " " + inputCtrlId : inputCtrlId;
    var ctrlText = $(inputCtrlId).parent().text().trim()
    ctrlText = $(inputCtrlId).is('input') ? (haseValue(ctrlText) ? ctrlText : 'This') : haseTrue($(inputCtrlId).parent().find('label').text()) ? $(inputCtrlId).parent().find('label').text() : 'This';
    if ($(inputCtrlId).is('select')) {
        ctrlVal = $(inputCtrlId + " option[value='" + ctrlVal + "']").text();
    }
    $.post(base.getUrl(controllerName, actionName), { id: parmVal }, function (data) {
        if (data == true) {
            base.notificaton({ result: { title: ctrlText + '  ' + ctrlVal + ' already exist', MessageType: 1, Message: "Error" } })
            $(inputCtrlId).focus();
            $(inputCtrlId).val('');
            return false;
        }
        else {
            return true;
        }
    });
}


STUtil.GetControlByForm = function (p) {
    var formId = p.formId;
    var infoId = "";
    var splitform = haseValue(p.splitForm) ? p.splitForm : ',';
    $("#" + formId + " :input").each(function () {
        var thisId = $(this).attr('id');
        if (haseValue(thisId)) {

            switch (base.getTagName('#' + formId + ' #' + thisId)) {
                case 'textarea':
                    infoId = haseValue(infoId) ? infoId + splitform+ thisId : thisId;
                    break;
                case 'input':
                    infoId = haseValue(infoId) ? infoId + splitform + thisId : thisId;
                    break;
                case 'button':
                    break;
                default:
                    infoId = haseValue(infoId) ? infoId + splitform+ thisId : thisId;
            }
        }
    })
    return infoId;
}
STUtil.GetControlValByForm = function (p) {
    var formId = p.formId;
    var infoVal = "";
    var splitform = haseValue(p.splitForm) ? p.splitForm : ',';
    $("#" + formId + " :input").each(function () {
        var thisId = $(this).attr('id');
        var thisVal = $(this).val();
        if (haseValue(thisId)) {
            switch (base.getTagName('#' + formId + ' #' + thisId)) {
                case 'textarea':
                    infoVal = haseValue(infoVal) ? infoVal + splitform + thisVal : thisVal;
                    break;
                case 'input':
                    var type = base.getInputType('#' + formId + ' #' + thisId).toLowerCase();
                    if (base.isNullOrEmpty(type)) {
                        return;
                    }
                    switch (type) {
                        case 'checkbox':
                            thisVal = $('#' + formId + ' #' + thisId).is(':checked');
                            infoVal = haseValue(infoVal) ? infoVal + splitform + thisVal : thisVal;
                            break
                        default:
                            infoVal = haseValue(infoVal) ? infoVal + splitform+ thisVal : thisVal;
                    }
                    break;
                case 'button':
                    break;
                default:
                    infoVal = haseValue(infoVal) ? infoVal + splitform + thisVal : thisVal;
            }
        }

    })
    return infoVal;

}

STUtil.EditForm = function (p) {
    var parmVals = p.parmVals;
    var controllerName = p.controllerName;
    var actionName = p.actionName;
    var cntrlFunction = p.ctrlId + 'AfterCall';
    debugger;
    $.ajaxSetup({ async: false });
    $.ajax({
        type: "POST",
        url: base.getUrl(controllerName, actionName),
        data: '{ id : ' + parmVals + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            debugger;
            var jsonObject = response;
            var prefix = '#';
            if (!base.isNullOrEmpty(p.formId)) {
                prefix += p.formId + ' #'
            }
            if (typeof (jsonObject) == 'object') {
                debugger;
                var properties = Object.keys(jsonObject);
                for (var i = 0; i < properties.length; i++) {
                    var property = properties[i];
                    var propId = prefix + property;
                    if (STUtil.isHtml(propId)) {
                        switch (base.getTagName(propId)) {
                            case 'textarea':
                                $(propId).text(jsonObject[property]);
                                $(propId).val(jsonObject[property]);
                                //$(propId).code(jsonObject[property]);
                                break;
                            case 'input':
                                var type = base.getInputType(propId).toLowerCase();
                                if (base.isNullOrEmpty(type)) {
                                    return;
                                }
                                switch (type) {
                                    case 'checkbox':
                                        $(propId).prop('checked', base.isTrue(jsonObject[property]));
                                        $(propId).val(base.isTrue(jsonObject[property]));



                                        //if ($(propId).next().attr('class') == "switchery switchery-small") {

                                        //    if (base.isTrue(jsonObject[property])) {
                                        //        $(propId).next().attr('style', 'background-color: rgb(109, 92, 174); border-color: rgb(109, 92, 174); box-shadow: rgb(109, 92, 174) 0px 0px 0px 11px inset; transition: border 0.4s, box-shadow 0.4s, background-color 1.2s;')
                                        //        $(propId).next().find('small').attr('style', 'left: 13px; background-color: rgb(255, 255, 255); transition: left 0.2s;')
                                        //    }
                                        //    else {

                                        //        $(propId).next().attr('style', 'background-color: rgb(255, 255, 255); border-color: rgb(223, 223, 223); box-shadow: rgb(223, 223, 223) 0px 0px 0px 0px inset; transition: border 0.4s, box-shadow 0.4s;')
                                        //        $(propId).next().find('small').attr('style', 'left: 0px; background-color: rgb(255, 255, 255); transition: left 0.2s;')
                                        //    }
                                        //}
                                        //else {
                                        //    $(propId).val(base.isTrue(jsonObject[property]));
                                        //}
                                        break;
                                    case 'datetime':
                                        var dt = base.dateFromJson(jsonObject[property]);
                                        $(propId).val(dt);
                                        break;
                                    case 'month':
                                        var ym = base.yearMonthFromJson(jsonObject[property]);
                                        $(propId).val(ym);
                                        break;
                                    case 'time':
                                        var tm = base.timeFromJson(jsonObject[property]);
                                        $(propId).val(tm);
                                        break;
                                    case 'file':
                                        var src = property + 'SRC';
                                        //var tm = base.timeFromJson(jsonObject[property]);
                                        // DO FOR FILES IF REQUIRED ST
                                        if (haseValue($(propId).next().attr('src'))) {
                                            $(propId).next().remove();
                                        }
                                        $(propId).after('<img id="theImg" class="profile-user-img  img-circle" style="height:100px;width:100px" src="' + jsonObject[src] +'" />')
                                        break;
                                    default:
                                        $(propId).val(jsonObject[property]);
                                }
                                break;
                            case 'select':
                                if ($(propId).prop('multiple')) {
                                    var selectedVals = [];
                                    selectedVals = haseValue(jsonObject[property]) ? jsonObject[property].split(',') : [];
                                    $(propId).val(selectedVals);
                                    $(propId).trigger('change');

                                }
                                else {
                                    $(propId).val(jsonObject[property]);
                                }
                                break;

                            default:
                                $(propId).val(jsonObject[property]);
                        }
                    }
                    else {

                        if (haseValue(p.loadList)) {
                            var loadlistInfo = p.loadList.split(',');
                            for (var j = 0; j < loadlistInfo.length; j++) {
                                if (property.replace(loadlistInfo[j], '') == 'List') {
                                    $('#' + loadlistInfo[j]).children().remove();
                                    $('#' + loadlistInfo[j]).append($("<option>").val("").text("Please Select"));
                                    $.each(jsonObject[property], function () {
                                        $('#' + loadlistInfo[j]).append($("<option>").val(this.Value).text(this.Text));
                                    });
                                }
                            }
                        }
                    }
                }
            }
            if (!base.isNullOrEmpty(p.formId)) {
                $('#' + p.formId).valid()
            }
            if (typeof (window[cntrlFunction]) == "function") {
                ///eval(cntrlFunction + "(" + response + ")");
                return window[cntrlFunction](response);
            }
        },
    });
}
STUtil.PostFrom = function (p) {
    debugger;
    var parmIds = p.parmIds;
    var ctrlId = p.ctrlId;
    var isShowMsg = p.isShowMsg == false ? false : true;
    var controllerName = p.controllerName;
    var actionName = p.actionName;
    var formId = p.formId;
    var dataObj = {};
    var splitChar = haseValue(p.splitChar) ? p.splitChar : ',';
    var infoIds = haseValue(p.parmIds) ? p.parmIds.split(splitChar) : STUtil.GetControlByForm({ formId: formId, splitForm: splitChar }).split(splitChar);
    var infoVals = haseValue(p.parmVals) ? p.parmVals.split(splitChar) : STUtil.GetControlValByForm({ formId: formId, splitForm: splitChar }).split(splitChar);
    var extraPrms = haseValue(p.extraPrms) ? p.extraPrms : extraPrms;
    var cntrlFunction = ctrlId + 'AfterCall';
    for (var i = 0; i < infoIds.length; i++) {
        dataObj[infoIds[i]] = infoVals[i];
    }
    if ($('#' + formId).valid()) {
        $('#' + ctrlId).attr('disabled', 'disabled');

        var para = haseValue(p.jsonObj) ? '{' + p.jsonObj + ': ' + JSON.stringify(dataObj) + ' , id:"' + extraPrms + '"}' : '{ id:"' + extraPrms + '"} ';

        $.ajaxSetup({ async: false });
        $.ajax({
            type: "POST",
            url: base.getUrl(controllerName, actionName),
            data: para,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#' + ctrlId).removeAttr('disabled');
                if (isShowMsg) {
                    base.notificaton({ result: { title: '', MessageType: response.MessageType, Message: response.Message } })
                }
                if (typeof (window[cntrlFunction]) == "function") {
                    ///eval(cntrlFunction + "(" + response + ")");
                    return window[cntrlFunction](response);
                }
            },
            failure: function (response) {
                $('#' + ctrlId).removeAttr('disabled');
            },
            error: function (response) {
                $('#' + ctrlId).removeAttr('disabled');
            }

        });

    }
}
STUtil.isHtml = function (id) {
    ret = true;
    if (isNaN(id)) { //if not number then go...
        id = base.getCtrlId(id);
        //ret = arrNonHtml.indexOf(id) == -1;
        var r = new RegExp("^[a-z A-Z 0-9 #_]*$");
        ret = r.test(id);
        if (ret) {
            var prefix = id.indexOf('#') == -1 ? '#' : '';
            ret = $(prefix + id).length > 0;
        }
    } else {
        ret = false;
    }
    return ret;
}
STUtil.addValidationRule = function () {
    var id = '';
    $.validator.addMethod(
        "pattern",
        function (value, element, regexp) {
            // debugger//
            if (regexp.constructor != RegExp) {
                regexp = new RegExp(regexp);
            }
            else if (regexp.global) {
                regexp.lastIndex = 0;
            }
            var ret = this.optional(element) || regexp.test(value)
            $.validator.messages.pattern = STUtil.DynamicValMsg(element, ret);
            id = element.id;

            return this.optional(element) || regexp.test(value);

        }, $.validator.messages.pattern);
}
STUtil.focusInvalid = function () {
    $('button').click(function () {
        var form = $(this).closest("form");
        if (haseValue(form.validate())) {
            form.validate().focusInvalid();

        }
    })
}
STUtil.initValid = function () {
    var ret = true;
    $('button,submit').click(function () {
        var isValid = $(this).attr("valid");
        if (isValid == "true") {
            var formId = $(this).closest("form").attr("id")
            ret = $("#" + formId).valid();
        }
        else {
            ret = true;
        }
        return ret;
    });
    $('input[type="datetime"]').bootstrapMaterialDatePicker({ format: 'MM/DD/YYYY',weekStart: 0, time: false }); 
    $('input[type="checkbox"]').each(function () {
        //  $(this).val($(this).is(':checked'));
        $(this).prop('checked', $(this).val() == 'True')
    }).click(function () {
        $(this).val($(this).is(':checked'));
    })
}
STUtil.datatableDateFormate = function (p) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(p);
    var dt = new Date(parseFloat(results[1]));
    return zeroPad(dt.getDate(), 2) + "/" + zeroPad((dt.getMonth() + 1), 2) + "/" + dt.getFullYear();
}
function zeroPad(num, places) {
    var zero = places - num.toString().length + 1;
    return Array(+(zero > 0 && zero)).join("0") + num;
}
STUtil.ddlEnabledDisabled = function (settings) {
    var ctrlId = settings.ctrlId;
    ctrlId = '#' + ctrlId;
    var readOnly = settings.readOnly;
    $(ctrlId + " option").each(function () {
        if (!$(this).is(':selected')) {
            if (readOnly) {
                $(this).attr('disabled', true)
                $(ctrlId).css({ 'backgroundColor': 'lightgray' })
            }
            else {
                $(ctrlId).css({ 'backgroundColor': '' })
                $(this).attr('disabled', false)
            }
        }
    });
}
STUtil.GetMonthName = function (monthNumber) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return months[monthNumber - 1];
}
STUtil.showDays = function (firstDate, secondDate) {
    var startDay = new Date(firstDate);
    var endDay = new Date(secondDate);
    var millisecondsPerDay = 1000 * 60 * 60 * 24;
    var millisBetween = startDay.getTime() - endDay.getTime();
    var days = millisBetween / millisecondsPerDay;
    return (Math.floor(days));
}
STUtil.formatDate = function (date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}
STUtil.DynamicTimePicker24 = function (p) {
    var ctrlId = p.ctrlId;
    var interVal = haseValue(p.interVal) ? p.interVal : 0;
    var ctrlId = p.ctrlId;
    var caption = haseValue(p.caption) ? p.caption : 'Select Time';
    var selectedValue = haseValue(p.selectedValue) ? p.selectedValue : "";
    $('#' + ctrlId).empty()
    $('#' + ctrlId).append($("<option>").val('').text(caption));
    var mnt = 0
    for (var i = 0; i <= 24; i++) {
        mnt = 0;
        for (var j = 0; j <= 59; j++) {
            if ((i <= 23 && j > 0)) {
                var value = ("0" + i).slice(-2) + ':' + ("0" + mnt).slice(-2);
                $('#' + ctrlId).append($("<option>").val(value).text(value));
                j = j == 0 ? (j + interVal) : (j + interVal - 1);
                mnt = j;
            }
        }
    }
    selectedValue = haseValue(selectedValue) ? selectedValue.replace(/\s/g, '') : selectedValue;
    $('#' + ctrlId).val(selectedValue);

}
STUtil.convertNumberToWords = function (amount) {
    var words = new Array();
    words[0] = '';
    words[1] = 'One';
    words[2] = 'Two';
    words[3] = 'Three';
    words[4] = 'Four';
    words[5] = 'Five';
    words[6] = 'Six';
    words[7] = 'Seven';
    words[8] = 'Eight';
    words[9] = 'Nine';
    words[10] = 'Ten';
    words[11] = 'Eleven';
    words[12] = 'Twelve';
    words[13] = 'Thirteen';
    words[14] = 'Fourteen';
    words[15] = 'Fifteen';
    words[16] = 'Sixteen';
    words[17] = 'Seventeen';
    words[18] = 'Eighteen';
    words[19] = 'Nineteen';
    words[20] = 'Twenty';
    words[30] = 'Thirty';
    words[40] = 'Forty';
    words[50] = 'Fifty';
    words[60] = 'Sixty';
    words[70] = 'Seventy';
    words[80] = 'Eighty';
    words[90] = 'Ninety';
    //// amount = amount.toString();
    var atemp = amount.split(".");
    var number = atemp[0].split(",").join("");
    var n_length = number.length;
    var words_string = "";
    if (n_length <= 9) {
        var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
        var received_n_array = new Array();
        for (var i = 0; i < n_length; i++) {
            received_n_array[i] = number.substr(i, 1);
        }
        for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
            n_array[i] = received_n_array[j];
        }
        for (var i = 0, j = 1; i < 9; i++ , j++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                if (n_array[i] == 1) {
                    n_array[j] = 10 + parseInt(n_array[j]);
                    n_array[i] = 0;
                }
            }
        }
        value = "";
        for (var i = 0; i < 9; i++) {
            if (i == 0 || i == 2 || i == 4 || i == 7) {
                value = n_array[i] * 10;
            } else {
                value = n_array[i];
            }
            if (value != 0) {
                words_string += words[value] + " ";
            }
            if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Crores ";
            }
            if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Lakhs ";
            }
            if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                words_string += "Thousand ";
            }
            if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                words_string += "Hundred and ";
            } else if (i == 6 && value != 0) {
                words_string += "Hundred ";
            }
        }
        words_string = words_string.split("  ").join(" ");
    }
    return words_string;
}
STUtil.GetNumberOfMonthBetweenDAte = function (d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth() + 1;
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}
STUtil.TextLanguageAr = function (txt) {

    yas = txt.value;
    yas = yas.replace(/`/g, "ذ");
    yas = yas.replace(/0/g, "۰");
    yas = yas.replace(/1/g, "۱");
    yas = yas.replace(/2/g, "۲");
    yas = yas.replace(/3/g, "۳");
    yas = yas.replace(/4/g, "٤");
    yas = yas.replace(/5/g, "۵");
    yas = yas.replace(/6/g, "٦");
    yas = yas.replace(/7/g, "۷");
    yas = yas.replace(/8/g, "۸");
    yas = yas.replace(/9/g, "۹");
    yas = yas.replace(/0/g, "۰");
    yas = yas.replace(/q/g, "ض");
    yas = yas.replace(/w/g, "ص");
    yas = yas.replace(/e/g, "ث");
    yas = yas.replace(/r/g, "ق");
    yas = yas.replace(/t/g, "ف");
    yas = yas.replace(/y/g, "غ");
    yas = yas.replace(/u/g, "ع");
    yas = yas.replace(/i/g, "ه");
    yas = yas.replace(/o/g, "خ");
    yas = yas.replace(/p/g, "ح");
    yas = yas.replace(/\[/g, "ج");
    yas = yas.replace(/\]/g, "د");
    yas = yas.replace(/a/g, "ش");
    yas = yas.replace(/s/g, "س");
    yas = yas.replace(/d/g, "ي");
    yas = yas.replace(/f/g, "ب");
    yas = yas.replace(/g/g, "ل");
    yas = yas.replace(/h/g, "ا");
    yas = yas.replace(/j/g, "ت");
    yas = yas.replace(/k/g, "ن");
    yas = yas.replace(/l/g, "م");
    yas = yas.replace(/\;/g, "ك");
    yas = yas.replace(/\'/g, "ط");
    yas = yas.replace(/z/g, "ئ");
    yas = yas.replace(/x/g, "ء");
    yas = yas.replace(/c/g, "ؤ");
    yas = yas.replace(/v/g, "ر");
    yas = yas.replace(/b/g, "لا");
    yas = yas.replace(/n/g, "ى");
    yas = yas.replace(/m/g, "ة");
    yas = yas.replace(/\,/g, "و");
    yas = yas.replace(/\./g, "ز");
    yas = yas.replace(/\//g, "ظ");
    yas = yas.replace(/~/g, " ّ");
    yas = yas.replace(/Q/g, "َ");
    yas = yas.replace(/W/g, "ً");
    yas = yas.replace(/E/g, "ُ");
    yas = yas.replace(/R/g, "ٌ");
    yas = yas.replace(/T/g, "لإ");
    yas = yas.replace(/Y/g, "إ");
    yas = yas.replace(/U/g, "‘");
    yas = yas.replace(/I/g, "÷");
    yas = yas.replace(/O/g, "×");
    yas = yas.replace(/P/g, "؛");
    yas = yas.replace(/A/g, "ِ");
    yas = yas.replace(/S/g, "ٍ");
    yas = yas.replace(/G/g, "لأ");
    yas = yas.replace(/H/g, "أ");
    yas = yas.replace(/J/g, "ـ");
    yas = yas.replace(/K/g, "،");
    yas = yas.replace(/L/g, "/");
    yas = yas.replace(/Z/g, "~");
    yas = yas.replace(/X/g, "ْ");
    yas = yas.replace(/B/g, "لآ");
    yas = yas.replace(/N/g, "آ");
    yas = yas.replace(/M/g, "’");
    yas = yas.replace(/\?/g, "؟");
    txt.value = yas;
}
function TabIndexSet() {
    var loop = 0;
    $('input,select').each(function () {
        $(this).attr('tabindex', loop)
        loop = loop + 1;
    });

}
STUtil.InitFunction = function () {

    STUtil.addValidationRule()
    STUtil.FileValidation();//FOR FILE VALIDATION
    STUtil.SetExist();
    STUtil.focusInvalid();
    TabIndexSet();
    STUtil.initValid();
}
STUtil.checkboxCheck = function (p) {
    var ctrlId = p.ctrlId;
    var formId = p.formId;
    var fullCtrlId = '#' + formId + ' #' + ctrlId;
    var checked = $(fullCtrlId).prop('checked') ? '' : 'checked';
    $(fullCtrlId).prop('checked', checked)
    $(fullCtrlId).val($(fullCtrlId).is(':checked'));

}

