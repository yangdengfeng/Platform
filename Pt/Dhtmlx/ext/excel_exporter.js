
/**
 * Excel导出器
 * @param {dhtmlXForm} form - 触发弹窗的控件所在表单（关联表单）
 * @param {string} id - 触发弹窗的控件的ID（关联控件）
 * @param {dhtmlxGrid} grid - 显示了数据集的网格（关联网格）
 * @param {number} pageSize - 每页记录数
 * @param {string} url - 数据导出Action的URL
 */
var ExcelExporter = (function () {
    var introFormat = "本列表共有数据{0}条，每页{1}条记录，共{2}页，请选择要导出的数据及文件格式";
    var configFormStructure = [
        { type: "settings", position: "label-left" },
        {
            type: "label", name: "intro", label: introFormat
        },
        {
            type: "fieldset", label: "导出数据范围", width: 550, list: [
                { type: "settings", position: "label-right" },
                { type: "radio", name: "rangeType", value: 1, label: "全部数据" },
                { type: "newcolumn" },
                { type: "radio", name: "rangeType", value: 2, label: "当前页", checked: true, offsetLeft: 36 },
                { type: "newcolumn" },
                {
                    type: "radio", name: "rangeType", value: 3, label: "指定页", offsetLeft: 36, list: [
                        { type: "settings", position: "label-left" },
                        { type: "input", name: "fromPage", value: 1, label: "从第", validate: "ValidInteger", inputWidth: 36 },
                        { type: "newcolumn" },
                        { type: "input", name: "toPage", value: 1, label: "到", validate: "ValidInteger", inputWidth: 36 },
                        { type: "newcolumn" },
                        { type: "label", label: "页" }
                    ]
                },
            ]
        },
        {
            type: "fieldset", label: "存为Excel文件格式", width: 550, list: [
                { type: "settings", position: "label-right" },
                { type: "radio", name: "fileFormat", value: 2003, label: "2003(.xls)" },
                { type: "newcolumn" },
                { type: "radio", name: "fileFormat", value: 2007, label: "2007(.xlsx)", checked: true, offsetLeft: 36 },
            ]
        },
        {
            type: "block", width: 556, list: [
                { type: "button", name: "start", value: "开始", offsetLeft: 160 },
                { type: "newcolumn" },
                { type: "button", name: "close", value: "关闭", offsetLeft: 20 }
            ]
        }
    ];

    /**
     * 取表单字段值，连成串
     * param form {dhtmlXForm }
     */
    function getQueryString(form) {
        var data = [];
        var formData = form.getFormData(true);
        for (var key in formData) {
            data.push(key + "=" + encodeURIComponent(formData[key]));
        }
        return data.join('&');
    }

    /**
     * Excel导出器
     * @param {dhtmlXForm} form - 查询条件所在表单（关联表单）
     * @param {dhtmlxGrid} grid - 显示了数据集的网格（关联网格）
     * @param {number} pageSize - 每页记录数
     * @param {string} url - 数据导出Action的URL
     */
    function ExcelExporter(form, grid, pageSize, url, dhxWins) {
        var self = this;
        var configWindow;
        var curPage = 1;
        var totalRows = 1;
        var pageCount = 1;
        dhxWins = dhxWins || new dhtmlXWindows();

        function isPageValid(data) {
            var v = parseInt(data);
            return (v >= 1 && v <= pageCount);
        }

        // 实时取得网格中的当前页号（从1开始计数）
        grid.attachEvent("onPageChanged", function (ind, find, lind) {
            curPage = ind;
        });

        // 实时取得网格中的记录总数，并计算页数
        grid.attachEvent("onDataReady", function () {
            totalRows = grid.getRowsNum();
            pageCount = Math.ceil(totalRows / pageSize);
        });

        this.onGetQueryString = null;

        // 显示导出配置表单
        this.show = function () {
            configWindow = dhxWins.createWindow('exportConfigWindow', 600, 200, 560, 370);
            configWindow.setText('导出配置');
            configWindow.denyResize();
            configWindow.button('minmax').hide();
            var configForm = configWindow.attachForm();
            configForm.load(configFormStructure);
            configForm.enableLiveValidation(true);

            var introText = introFormat.replace("{0}", totalRows)
                .replace("{1}", pageSize)
                .replace("{2}", pageCount);
            configForm.setItemLabel("intro", introText);
            configForm.setItemValue("fromPage", curPage);
            configForm.setItemValue("toPage", curPage);
            configWindow.show();
            configWindow.setModal(true);

            configForm.attachEvent("onButtonClick", function (id) {
                if (id == "start") {    // 开始导出
                    if (!configForm.validate()) {
                        alert("页码请填写数字");
                        return false;
                    }

                    var fileFormat = configForm.getItemValue("fileFormat"); // 2003, 2007
                    // 页码范围转换为行号范围
                    var pos, count;
                    var rangeType = configForm.getItemValue("rangeType");   // 1, 2, 3
                    var fromPage = configForm.getItemValue("fromPage");
                    var toPage = configForm.getItemValue("toPage");

                    if (!isPageValid(fromPage) || !isPageValid(toPage)) {
                        window.alert("页码必须在1和" + pageCount + "之间。");
                        return false;
                    }

                    switch (rangeType) {
                        case 1: // 全部
                            pos = 0;
                            count = totalRows;
                            break;
                        case 2: // 当前页
                            pos = (curPage - 1) * pageSize;
                            count = pageSize;
                            break;
                        case 3: // 指定页号范围
                            pos = (fromPage - 1) * pageSize;
                            count = (toPage - fromPage + 1) * pageSize;
                            break;
                    }

                    var qs;
                    if (self.onGetQueryString)
                        qs = self.onGetQueryString();
                    else if(form)
                        qs = getQueryString(form);
                    var openUrl = url + "?" + qs + "&posStart=" + pos + "&count=" + count + "&fileFormat=" + fileFormat;
                    window.open(openUrl, "_blank");
                    configWindow.hide();
                    configWindow.setModal(false);
                }
                else if (id == "close") {   // 关闭配置弹窗
                    configWindow.hide();
                    configWindow.setModal(false);
                }
            });
        };
    }

    return ExcelExporter;
})();
