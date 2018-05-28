<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="FirstTest.main" %>
<%@ Import Namespace="FirstTest" %>
<%@ Import Namespace="ClassStore" %>
<!DOCTYPE html> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
    <script src="./Script/jquery-1.10.2"></script>
    <script>
        function getid(name) { 

        }
        function delit(id) {
            $ajax({
                url: "main.aspx.cs",
                success: function () {
                }
            });
        }
        /*<asp:button id="button" runset="server" text="发送请求" onclick="button_click" />*/
    </script>

<body>

    <form id="form1" runat="server">
        <table>
            <tbody>
                <tr>
                    <th>名称</th>
                    <th>单价</th>
                    <th>单位</th>
                    <th>库存</th>
                    <th>产地</th>
                    <th>操作</th>
                </tr>
            </tbody>
            <tbody>
                <% foreach(Goods a in list){ %>
                    <tr>
                        <td onclick="getid(a.name)"><%=a.name %></td>
                        <td><%=a.price %></td>
                        <td><%=a.unit %></td>
                        <td><%=a.store %></td>
                        <td><%=a.place %></td>
                        <td onclick="delit(a.id)">删除</td>
                    </tr>
                <% }%>
            </tbody>
        </table>
    </form>
</body>
</html>
<style>
        table {
            border-top:1px solid #ccc;
            border-left:1px solid #ccc;
            width:600px;
            border-collapse:collapse;
            margin:0 auto;
        }
        table th {
            font-weight:bold;
        }
        table td{
            text-align:center;
        }
        table td,table th  {
            border-bottom:1px solid #ccc;
            border-right:1px solid #ccc;
            line-height:32px;
            height:32px;
        }
</style>