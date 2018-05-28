var istrue = true;//是否可以保存
var opt = "ins";
var workbillid = "";

//转换为页面表格数据
function OutputData(target, item) {

    target.append('<tr id=' + item.id + '>' +
        '<td onclick="getid(a.name)">' + item.name + '</td>' +
        '<td>' + item.price + '</td>' +
        '<td>' + item.unit + '</td>' +
        '<td>' + item.store + '</td>' +
        '<td>' + item.place + '</td>' +
        '<td><a onclick="delit(' + item.id + ')"  style="margin-right:6px;" class="a-link">删除</a>' +
        '<a onclick="edtit(' + item.id + ')" class="a-link">编辑</a>' +
        '</td>' +
    '</tr>');
}

//表单数据数字验证
function regis(name) {
    var _val = $.trim($("input[name=" + name + "]").val());//去空格
    var p = /^[1-9](\d+(\.\d{1,2})?)?$/;
    var p1 = /^[0-9](\.\d{1,2})?$/;
    //正则匹配
    _val
    if (p.test(_val) || p1.test(_val) || _val == "") {
        $("#" + name + "1").hide();
    } else {
        istrue = false;
        $("#" + name + "1").show();
    }
}
//刷新一行数据
function uploadtr(item) {
    $("#" + workbillid).html(
         '<td onclick="getid(a.name)">' + item.name + '</td>' +
        '<td>' + item.price + '</td>' +
        '<td>' + item.unit + '</td>' +
        '<td>' + item.store + '</td>' +
        '<td>' + item.place + '</td>' +
        '<td><a onclick="delit(' + item.id + ')"  style="margin-right:6px;" class="a-link">删除</a>' +
        '<a onclick="edtit(' + item.id + ')" class="a-link">编辑</a>' +
        '</td>' 
        );
}
/*--------------新增-----------------*/
function saveme() {
    var formtb = $("#insform").serializeArray();
    var obj = {};

    if (!istrue) {
        alert("保存失败，请确认数据格式是否正确！");
        return;
    }

    for (d in formtb) {
        if (Boolean(formtb[d].value)) {
            obj[formtb[d].name] = formtb[d].value;
        } else {
            alert("保存失败，请确认数据是否填写完整！");
            return;
        }
    }
    obj["opt"] = opt;
    if (opt == "edt") obj["id"] = workbillid;
    else obj["id"] = "123";

    /*数据连接*/
    setTimeout(function () {
        $.ajax({
            async: false,//判断同步异步
            type: "Get",
            url: "/sys/aspx/myfirst.ashx",
            data: obj,
            dataType: "json",
            crossDomain: true,
            cache: false,
            success: function (data) {//返回新增的id
                if (Boolean(data)) {
                    if (opt == "ins") {
                        //增加元素
                        var _id = Number(data);//为新列表赋值随机数id
                        obj["id"] = _id;
                        OutputData($("#tbody"), obj);
                        alert("保存成功");
                        $("#insert-form").slideUp(500);//隐藏新增表单
                    } else {
                        alert("修改成功");
                        uploadtr(obj);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrow) {
                alert("Message Error:" + textStatus + "errorThrow:" + errorThrow);
                console.log(jqXHR);
                console.log("Message Error:" + textStatus);
                console.log("errorThrow:" + errorThrow);
            }
        });
    }, 1000);
}
    function inslist() {
        opt = "ins";
        $("#insert-form").slideToggle(500);
        $(".ins-head").text("新增");
        $("#insert-form input").val('');
    }
    /*-------------新增end--------------*/


    /*----------------通过id删除某条数据--------------*/
    function delit(id) {
        /*数据连接*/
        $.ajax({
            async: true,//判断同步异步
            type: "Get",
            url: "/sys/aspx/myfirst.ashx",
            data: { opt: "del", id: Number(id)},
            dataType: "json",
            crossDomain: true,
            cache: false,
            success: function (data) {//返回影响行数
                if (data > 0) {
                    $("#" + id).remove().slideUp(800);
                }
            },
            error: function (jqXHR, textStatus, errorThrow) {
                alert("Message Error:" + textStatus + "errorThrow:" + errorThrow);
                console.log(jqXHR);
                console.log("Message Error:" + textStatus);
                console.log("errorThrow:" + errorThrow);
            }
        });
    }
    /*--------------删除end-------------------*/

    //----------------------编辑----------------------
    function edtit(id) {
        opt = "edt";
        $("#insert-form").slideToggle(500);
        $(".ins-head").text("编辑");
        loadme("info", id);
        workbillid = id;
    };

    //------------------读数据---------------------
    function loadme(type, id) {
        /*数据连接*/
        /*数据类型dataType设置为 "jsonp" 时，jQuery 将自动调用回调函数
          JSON
            {
            “name”: “sb”
            }
          JSONP
            callback({
            “name”: “sb”
            })
        */
        var tbody = $("#tbody");
        var data = { opt: "search", type: "list",id:0 };//默认读取全列表
        switch (type) {
            case "keyword": {
                data["keyword"] = $(".input-search").val();
                data["type"] = "key1";
                tbody.html("");//清空元素内容
                break;
            }
            case "info": {
                data["type"] = "info";
                data["id"] = id;
                break;
            }
        }
        $.ajax({
            async: true,//判断同步异步
            type: "Get",
            url: "/sys/aspx/myfirst.ashx",
            data: data,
            dataType: "jsonp",
            jsonp: "callback",//数据接收
            crossDomain: true,
            cache: false,
            success: function (data) {
                console.log(data);
                data.forEach(function (item) {
                    if (type == "info") {//表单赋值
                        var formtb = $("#insform").serializeArray();
                        for (d in formtb) {
                            for (var i in item) {
                                if (i == formtb[d].name) {
                                    $("#insform input[name=" + i + "]").val(item[i]);
                                    //formtb[d].value = item[i];
                                }
                            }
                        }
                    } else {
                        OutputData(tbody, item);
                    }
                });
            },
            error: function (jqXHR, textStatus, errorThrow) {
                alert("Message Error:" + textStatus + "errorThrow:" + errorThrow);
                console.log(jqXHR);
                console.log("Message Error:" + textStatus);
                console.log("errorThrow:" + errorThrow);
            }
        });
    }
    jQuery(document).ready(function () {
        $("#insert-form").hide();
        loadme();
    });