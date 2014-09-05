
<%@ Page Title="员工经历" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeCareerEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeCareerEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        var eid;

        function onPgLoad() {
            eid = $.getQueryString({ "ID": "eid" });
        
            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#EmployeeId").val(eid); 
                        
                $("#CreatorName").val(PICState.UserInfo.Name);
                $("#CreatedDate").val(jQuery.dateOnly(PICState.SystemInfo.Date));
            }

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: "员工经历",
                tbar: {
                    xtype: 'picfrmtoolbar',
                    items: [{
                        xtype: 'picsavebutton',
                        id: 'btnSubmit'
                    }, '-', 'cancel', '->', '-', 'help']
                }
            });

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            PICFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    
    <div id="editDiv" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                        <input id="EmployeeId" name="EmployeeId" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        公司名称
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Company" name="Company" class="validate[required]" />
                    </td>
                    <td class="pic-ui-td-caption">
                        部门
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Department" name="Department" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        职位
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="Position" name="Position" />
                    </td>
                    <td class="pic-ui-td-caption">
                        &nbsp;
                    </td>
                    <td class="pic-ui-td-data">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">
                        入职时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="BeginDate" name="BeginDate" picctrl="date" />
                    </td>
                    <td class="pic-ui-td-caption">
                        离职时间
                    </td>
                    <td class="pic-ui-td-data">
                        <input id="EndDate" name="EndDate" picctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="pic-ui-td-caption">工作内容描述</td>
                    <td class="pic-ui-td-data" colspan="3">
                        <textarea id="Description" name="Description" style="width: 90%; height:100px"></textarea>
                    </td>
                </tr>
                <tr width="100%">
                    <td class="pic-ui-td-caption" >
                        录入人
                    </td>
                    <td class="pic-ui-td-data">
                        <input disabled id="CreatorName" name="CreatorName" />
                    </td>
                    <td class="pic-ui-td-caption" >
                        录入日期
                    </td>
                    <td class="pic-ui-td-data">
                        <input disabled id="CreatedDate" name="CreatedDate" dateonly="true" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="descDiv" class="pic-ui-form-desc">
        表单描述：员工经历信息编辑
    </div>
</asp:Content>


