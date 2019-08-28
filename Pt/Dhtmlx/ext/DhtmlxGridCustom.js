function eXcell_edit(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            this.setCValue("<button class='layui-btn layui-btn-xs layui-btn-normal' onClick='excellEdit_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-bianji'></i></button>")
            //this.setCValue("<a style='text-decoration:none' onClick='excellEdit_click(this)'  data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><i title='" + val + "' class='Hui-iconfont'>&#xe6df;</i></a> ");
        }
        else {
            this.setCValue('<span></span>')
        }
    }
    this.getValue = function () {
        return 'edit';
    }

}
eXcell_edit.prototype = new eXcell;

function eXcell_layuiview(cell) {
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        var row_id = this.cell.parentNode.idd;
        var str = [];
        str.push("<div class='layui-btn-group'>");
        if (val.indexOf("view") != -1) {
            str.push("<button class='layui-btn layui-btn-xs layui-btn-primary' onClick='excellView_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-search'></i></button>");
            //str += "<a style='text-decoration:none' onClick='excellView_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><button class='layui-btn layui-btn-primary layui-btn-xs'>查看</button></a> ";//<i  title='" + val + "' class='layui-btn'>查看</i>
        }
        if (val.indexOf("edit") != -1) {
            str.push("<button class='layui-btn layui-btn-xs layui-btn-normal' onClick='excellEdit_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-bianji'></i></button>");
            //str += "<a style='text-decoration:none' onClick='excellEdit_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><button class='layui-btn layui-btn-primary layui-btn-xs'>编辑</button></a> ";//<i  title='" + val + "' class='layui-btn'>查看</i>
        }
        if (val.indexOf("delete") != -1) {
            str.push("<button class='layui-btn layui-btn-xs layui-btn-danger' onClick='excellDel_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-delete'></i></button>");
            //str += "<a style='text-decoration:none' onClick='excellDel_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><button class='layui-btn layui-btn-primary layui-btn-xs'>删除</button></a> ";//<i  title='" + val + "' class='layui-btn'>查看</i>
        }
        str.push("</div>");
        this.setCValue(str.join(" "));
    }
    this.getValue = function () {
        return 'layui';
    }
}
eXcell_layuiview.prototype = new eXcell;


function eXcell_del(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            this.setCValue("<button class='layui-btn layui-btn-xs layui-btn-danger' onClick='excellDel_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-delete'></i></button>")
            //this.setCValue("<a style='text-decoration:none' onClick='excellDel_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><i  title='" + val + "' class='Hui-iconfont' title='删除'>&#xe6e2;</i></a> ");
        }
        else {
            this.setCValue('<span></span>')
        }
    }
    this.getValue = function () {
        return 'del';
    }
}
eXcell_del.prototype = new eXcell;

function eXcell_view(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        var row_id = this.cell.parentNode.idd;
        //this.setCValue("<a style='text-decoration:none' onClick='excellView_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><i  title='" + val + "' class='Hui-iconfont'>&#xe709;</i></a> ");
        this.setCValue("<button class='layui-btn layui-btn-xs layui-btn-primary' onClick='excellView_click(this)'  data-rowId='" + row_id + "'  title='" + val + "'><i class='layui-icon layui-icon-search'></i></button>")
    }
    this.getValue = function () {
        return 'view';
    }
}
eXcell_view.prototype = new eXcell;

function eXcell_view1(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        var row_id = this.cell.parentNode.idd;
        this.setCValue("<a style='text-decoration:none' onClick='excellView1_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><i  title='" + val + "' class='Hui-iconfont'>&#xe709;</i></a> ");
    }
    this.getValue = function () {
        return 'view';
    }
}
eXcell_view1.prototype = new eXcell;

function eXcell_view2(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        var row_id = this.cell.parentNode.idd;
        this.setCValue("<a style='text-decoration:none' onClick='excellView2_click(this)' data-rowId='" + row_id + "' href='javascript:;' title='" + val + "'><i  title='" + val + "' class='Hui-iconfont'>&#xe709;</i></a> ");
    }
    this.getValue = function () {
        return 'view';
    }
}
eXcell_view2.prototype = new eXcell;

function eXcell_jslink(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var valsAr = val.split("^^");
            var color = 'black'
            if (valsAr.length >= 3) {
                color = valsAr[2];
            }
            var row_id = this.cell.parentNode.idd;
            var strTem = "<a style='text-decoration:#textDecoration#;color:#color#' onClick='#click# return false;' data-rowId='#rowId#' href='javascript:;' title='#title#' >#title#</a> ";
            var values = {};
            values["textDecoration"] = "none";
            if (valsAr[1] && valsAr[1].length > 0 && valsAr[1] != 'javascript:;') {
                values["textDecoration"] = "underline";
            }
            values["color"] = color;
            values["click"] = valsAr[1];
            values["title"] = valsAr[0];
            values["rowId"] = row_id;

            //this.setCValue("<a style='text-decoration:none;color:'" + valsAr[2] + "' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + valsAr[0] + "</a> ");
            this.setCValue(window.dhx4.template(strTem, values));
        } else {
            this.setCValue('<span></span>')
        }
    }
    this.getValue = function () {
        return this.cell.firstChild.innerText;
    }
}
eXcell_jslink.prototype = new eXcell;


function eXcell_layuijslink(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var valsAr = val.split("^^");
            var color = 'black'
            if (valsAr.length >= 3) {
                color = valsAr[2];
            }
            var row_id = this.cell.parentNode.idd;
            var strTem = "<a style='text-decoration:#textDecoration#;color:#color#' onClick='#click# return false;' data-rowId='#rowId#' href='javascript:;'   >#title#</a> ";
            var values = {};
            values["textDecoration"] = "none";
            if (valsAr[1] && valsAr[1].length > 0 && valsAr[1] != 'javascript:;') {
                values["textDecoration"] = "underline";
                values["title"] = "<button class='layui-btn layui-btn-normal layui-btn-xs'>" + valsAr[0] + "</button>";
            }
            else {
                values["title"] = valsAr[0];
            }
            values["color"] = color;
            values["click"] = valsAr[1];
            values["rowId"] = row_id;

            //this.setCValue("<a style='text-decoration:none;color:'" + valsAr[2] + "' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + valsAr[0] + "</a> ");
            this.setCValue(window.dhx4.template(strTem, values));
        } else {
            this.setCValue('<span></span>')
        }
    }
    this.getValue = function () {
        return this.cell.firstChild.innerText;
    }
}
eXcell_layuijslink.prototype = new eXcell;


function eXcell_jsbrlinks(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            var aArr = val.split("~");
            aArr = jQuery.map(aArr, function (a) {
                var valsAr = a.split('^');

                return "<a style='text-decoration:underline' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + valsAr[0] + "</a> ";
            });

            this.setCValue(aArr.join('<br> '));
        } else {
            this.setCValue('<span></span>')
        }

    }
    this.getValue = function () {
        return 'jsbrlinks';
    }
}
eXcell_jsbrlinks.prototype = new eXcell;

function eXcell_jslinks(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            var aArr = val.split("~");
            aArr = jQuery.map(aArr, function (a) {
                var valsAr = a.split('^');

                return "<a style='text-decoration:underline' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + valsAr[0] + "</a> ";
            });

            this.setCValue(aArr.join(' '));
        } else {
            this.setCValue('<span></span>')
        }

    }
    this.getValue = function () {
        return 'jslinks';
    }
}
eXcell_jslinks.prototype = new eXcell;

function eXcell_layuijslinks(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            var aArr = val.split("~");
            aArr = jQuery.map(aArr, function (a) {
                var valsAr = a.split('^');
                if (valsAr[0]) {
                    return "<a style='text-decoration:underline' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + "<button class='layui-btn layui-btn-primary layui-btn-xs'>" + valsAr[0] + "</button>" + "</a> ";
                }
            });

            this.setCValue(aArr.join(' '));
        } else {
            this.setCValue('<span></span>')
        }

    }
    this.getValue = function () {
        return 'layuijslinks';
    }
}
eXcell_layuijslinks.prototype = new eXcell;

function eXcell_mjslinks(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        if (val && val.length > 0) {
            var row_id = this.cell.parentNode.idd;
            var aArr = val.split("~");
            aArr = jQuery.map(aArr, function (a) {
                var valsAr = a.split('^');

                return "<a style='text-decoration:underline' onClick='" + valsAr[1] + " return false;" + "' data-rowId='" + row_id + "' href='javascript:;' title='" + valsAr[0] + "'>" + valsAr[0] + "</a> ";
            });

            this.setCValue(aArr.join('</br>'));
        } else {
            this.setCValue('<span></span>')
        }

    }
    this.getValue = function () {
        return 'jsmlinks';
    }
}
eXcell_mjslinks.prototype = new eXcell;

function eXcell_status(cell) { //the eXcell name is defined here
    if (cell) {                // the default pattern, just copy it
        this.cell = cell;
        this.grid = this.cell.parentNode.grid;
    }
    this.edit = function () { }  //read-only cell doesn't have edit method
    // the cell is read-only, so it's always in the disabled state
    this.isDisabled = function () { return true; }
    this.setValue = function (val) {
        var row_id = this.cell.parentNode.idd;
        if (val == '1') {
            this.setCValue('<span class="label label-success radius">正常</span>')
        } else if (val == '0') {
            console.log("forbidden");
            this.setCValue('<span class="layui-bg-red">已禁用</span>')
        }
    }
    this.getValue = function () {
        return 'status';
    }
}
eXcell_status.prototype = new eXcell;