var itemCodeSearch = function (spec) {
    var that = {};
 

    that.show = function () {
        var itemWin = spec.itemWin;

        var itemToolbar = itemWin.attachToolbar({
            iconset: "awesome",
            items: [
               { id: "input", type: "text", text: "文本查询" },
               { id: "s1", type: "separator" },
                { id: "searchTxt", type: "buttonInput", text: "", width: 140 },
               { id: "search", type: "button", text: "点击查询", img: "fa fa-clone" },
                { id: "s2", type: "separator" },
                { id: "Save", type: "button", text: "确定", img: "fa fa-clone" }
            ]
        });



        var dhxTree = itemWin.attachTree();
        itemWin.progressOn();
        dhxTree.setImagePath("../../Dhtmlx/skins/material/imgs/dhxtree_material/");
        dhxTree.enableCheckBoxes(1);
        dhxTree.enableThreeStateCheckboxes(true);
        dhxTree.load("/TotalSearch/ItemCodeTree", function () {
            itemWin.progressOff();
        });

        itemToolbar.attachEvent("onClick", function (id) {
            if (id == 'search') {
                var sText = window.dhx.trim(itemToolbar.getValue('searchTxt'));
                if (sText.length > 0) {
                    dhxTree.findItem(sText);
                }
            } else if (id == 'Save') {
                var selecteIds = dhxTree.getAllChecked().split(',');
                if (selecteIds.length > 0) {

                    var itemCodes = $.grep(selecteIds, function (items) { return items.length == 5; });
                    var parmCodes = $.grep(selecteIds, function (items) { return items.length > 5; });
                    var allCodes = [];
                    if (itemCodes.length == 0) {
                        allCodes = parmCodes;
                    } else {
                        allCodes = $.grep(parmCodes, function (pData) {
                            return $.grep(itemCodes, function (iData) {
                                return pData.indexOf(iData) > -1;
                            }).length == 0;
                        });
                    }

                    $.merge(allCodes, itemCodes);

                    var allNames = $.map(allCodes, function (ac) {
                        if (ac.length <= 5) {
                            return dhxTree.getItemText(ac);
                        } else {
                            var codename = ac.replace('补', '').substring(0, 5);
                            return dhxTree.getItemText(codename) + '(' + dhxTree.getItemText(ac) + ')';
                        }
                    });

                    myForm.setItemValue('CheckItem', allNames.join(','));

                }
            }
        });
    };

    return that;
}